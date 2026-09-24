using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Content.Server.Administration.Managers;
using Content.Server.Database;
using Content.Server._CMU14.Yautja;
using Content.Server._RMC14.LinkAccount;
using Content.Shared._RMC14.LinkAccount;
using Content.Shared._RMC14.Mentor;
using Content.Shared._CMU14.Yautja;
using Content.Shared.Administration;
using Content.Shared.CCVar;
using Content.Shared.Players.JobWhitelist;
using Content.Shared.Roles;
using Robust.Server.Player;
using Robust.Shared.Configuration;
using Robust.Shared.Network;
using Robust.Shared.Player;
using Robust.Shared.Prototypes;

namespace Content.Server.Players.JobWhitelist;

public sealed partial class JobWhitelistManager : IPostInjectInit
{
    private const string YautjaHunterJob = "CMUYautjaHunter";
    private const YautjaWhitelistFlags HunterWhitelistFlags =
        YautjaWhitelistFlags.Yautja |
        YautjaWhitelistFlags.Legacy |
        YautjaWhitelistFlags.Council |
        YautjaWhitelistFlags.CouncilLegacy |
        YautjaWhitelistFlags.Leader;

    [Dependency] private IAdminManager _admin = default!;
    [Dependency] private IConfigurationManager _config = default!;
    [Dependency] private IServerDbManager _db = default!;
    [Dependency] private INetManager _net = default!;
    [Dependency] private IPlayerManager _player = default!;
    [Dependency] private IPrototypeManager _prototypes = default!;
    [Dependency] private LinkAccountManager _linkAccount = default!;
    [Dependency] private UserDbDataManager _userDb = default!;
<<<<<<< HEAD
    [Dependency] private YautjaRankManager _yautjaRank = default!;

    private readonly Dictionary<NetUserId, HashSet<string>> _whitelists = new();
    private readonly Dictionary<NetUserId, YautjaWhitelistFlags> _yautjaWhitelistFlags = new();
=======
    [Dependency] private ILogManager _logManager = default!;

    private readonly Dictionary<NetUserId, HashSet<string>> _whitelists = new();
    private ISawmill _sawmill = default!;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

    public void Initialize()
    {
        _net.RegisterNetMessage<MsgJobWhitelist>();
        _linkAccount.PatronUpdated += OnPatronUpdated;
    }

    private void OnPatronUpdated((NetUserId Id, SharedRMCPatronFull Patron) update)
    {
        if (_player.TryGetSessionById(update.Id, out var session))
            SendJobWhitelist(session);
    }

    private async Task LoadData(ICommonSession session, CancellationToken cancel)
    {
        var whitelists = await _db.GetJobWhitelists(session.UserId, cancel);
        cancel.ThrowIfCancellationRequested();
        _whitelists[session.UserId] = whitelists.ToHashSet();
        _yautjaWhitelistFlags[session.UserId] =
            (YautjaWhitelistFlags) await _db.GetYautjaWhitelistFlagsAsync(session.UserId.UserId);
    }

    private void FinishLoad(ICommonSession session)
    {
        SendJobWhitelist(session);
    }

    private void ClientDisconnected(ICommonSession session)
    {
        _whitelists.Remove(session.UserId);
        _yautjaWhitelistFlags.Remove(session.UserId);
    }

    public async void AddWhitelist(NetUserId player, ProtoId<JobPrototype> job)
    {
        if (_whitelists.TryGetValue(player, out var whitelists))
            whitelists.Add(job);

        await _db.AddJobWhitelist(player, job);

        if (_player.TryGetSessionById(player, out var session))
            SendJobWhitelist(session);
    }

    /// <summary>
    /// Returns false if role whitelist is required but the player does not have it.
    /// </summary>
    public bool IsAllowed(ICommonSession session, ProtoId<JobPrototype> job)
    {
        if (!_config.GetCVar(CCVars.GameRoleWhitelist) && job.Id != YautjaHunterJob)
            return true;
<<<<<<< HEAD

        if (job == MentorConstants.Job &&
            _admin.HasAdminFlag(session, AdminFlags.MentorHelp, includeDeAdmin: true))
        {
            return true;
        }

        // RMC14-Whitelist-Tweak-Start
        if (!_prototypes.TryIndex(job, out var jobPrototype))
            return true;
=======
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

        HashSet<ProtoId<JobPrototype>> visited = [];
        var current = job;

<<<<<<< HEAD
        if (BoostyYautjaWhitelist.IsAllowed(job, _linkAccount.GetConnectedPatron(session.UserId)?.Tier?.Priority))
            return true;

        if (IsWhitelisted(session.UserId, job))
            return true;
=======
        while (visited.Add(current))
        {
            if (!_prototypes.TryIndex(current, out var jobPrototype) ||
                !jobPrototype.Whitelisted)
            {
                return true;
            }
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

            if (IsWhitelisted(session.UserId, current))
                return true;

            if (jobPrototype.WhitelistParent is not { } parent)
                return false;

            current = parent;
        }

        return false;
    }

    public bool IsWhitelisted(NetUserId player, ProtoId<JobPrototype> job)
    {
        if (!_whitelists.TryGetValue(player, out var whitelists))
        {
            _sawmill.Error("Unable to check if player {Player} is whitelisted for {Job}. Stack trace:\\n{StackTrace}",
                player,
                job,
                Environment.StackTrace);
            return AllowsYautjaHunter(player, job);
        }

        return whitelists.Contains(job) || AllowsYautjaHunter(player, job);
    }

    public async Task RefreshYautjaWhitelist(NetUserId player)
    {
        await _yautjaRank.Refresh(player);
        _yautjaWhitelistFlags[player] =
            (YautjaWhitelistFlags) await _db.GetYautjaWhitelistFlagsAsync(player.UserId);

        if (_player.TryGetSessionById(player, out var session))
            SendJobWhitelist(session);
    }

    public async void RemoveWhitelist(NetUserId player, ProtoId<JobPrototype> job)
    {
        _whitelists.GetValueOrDefault(player)?.Remove(job);
        await _db.RemoveJobWhitelist(player, job);

        if (_player.TryGetSessionById(new NetUserId(player), out var session))
            SendJobWhitelist(session);
    }

    public void SendJobWhitelist(ICommonSession player)
    {
        var whitelist = _whitelists.GetValueOrDefault(player.UserId)?.ToHashSet() ?? new HashSet<string>();
        if (AllowsYautjaHunter(player.UserId, YautjaHunterJob))
            whitelist.Add(YautjaHunterJob);

        var msg = new MsgJobWhitelist
        {
            Whitelist = whitelist,
            YautjaCapabilities = _yautjaRank.ResolveProfileCapabilitiesCached(player.UserId),
        };

        _net.ServerSendMessage(msg, player.Channel);
    }

    private bool AllowsYautjaHunter(NetUserId player, ProtoId<JobPrototype> job)
    {
        if (job.Id != YautjaHunterJob)
            return false;

        if (BoostyYautjaWhitelist.IsAllowed(job.Id, _linkAccount.GetConnectedPatron(player)?.Tier?.Priority))
            return true;

        return _yautjaWhitelistFlags.TryGetValue(player, out var flags) &&
               (flags & HunterWhitelistFlags) != YautjaWhitelistFlags.None;
    }

    void IPostInjectInit.PostInject()
    {
        _userDb.AddOnLoadPlayer(LoadData);
        _userDb.AddOnFinishLoad(FinishLoad);
        _userDb.AddOnPlayerDisconnect(ClientDisconnected);
        _sawmill = _logManager.GetSawmill("job_whitelist");
    }
}
