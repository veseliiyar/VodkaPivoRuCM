// SPDX-License-Identifier: AGPL-3.0-only
// Copyright (c) 2026 wray-git
// SPDX-License-Identifier: AGPL-3.0-only
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.CMU14.ZLevelBuilding;

/// <summary>
/// Building overhaul (z-level), Phase 2 test/driver command: digs straight down from where you are standing.
/// On the first dig over a map, this lazily creates a stone level below and links it into a z-network - so it
/// works on ANY map, including ones that were not authored as multi-z. Run again to keep descending.
///
/// (A proper in-world digging tool/interaction is a later polish step; this command drives the same
/// <see cref="ZLevelBuildingSystem.DigDown"/> pipeline.)
/// </summary>
[AdminCommand(AdminFlags.Debug)]
public sealed class ZDigDownCommand : IConsoleCommand
{
    public string Command => "au_digdown";
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/ZDigDownCommand.cs
    public string Description => Loc.GetString("cmd-au-digdown-desc");
    public string Help => Loc.GetString("cmd-au-digdown-help");
=======
    public string Description => Loc.GetString("cmu-cmd-digdown-desc");
    public string Help => Loc.GetString("cmu-cmd-digdown-help");
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/ZDigDownCommand.cs

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player?.AttachedEntity is not { } player)
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/ZDigDownCommand.cs
            shell.WriteError(Loc.GetString("cmd-au-dig-player-only"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-zdig-player-only"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/ZDigDownCommand.cs
            return;
        }

        var system = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ZLevelBuildingSystem>();
        if (system.DigDown(player))
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/ZDigDownCommand.cs
            shell.WriteLine(Loc.GetString("cmd-au-digdown-success"));
        else
            shell.WriteError(Loc.GetString("cmd-au-digdown-failed"));
=======
            shell.WriteLine(Loc.GetString("cmu-cmd-digdown-success"));
        else
            shell.WriteError(Loc.GetString("cmu-cmd-digdown-failed"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/ZDigDownCommand.cs
    }
}
