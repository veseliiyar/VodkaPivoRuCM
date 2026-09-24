// SPDX-License-Identifier: AGPL-3.0-only
// Copyright (c) 2026 wray-git
// SPDX-License-Identifier: AGPL-3.0-only
using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.CMU14.ZLevelBuilding;

/// <summary>
/// Building overhaul (z-level): digs straight UP one level from where you are standing. You surface at the same
/// world x/y, so where you come up reflects how far you travelled underground. Blocked if a solid wall sits
/// directly above the spot (dig somewhere without a wall above instead).
///
/// (A proper in-world digging tool/interaction is a later polish step; this drives the same
/// <see cref="ZLevelBuildingSystem.DigUp"/> pipeline.)
/// </summary>
[AdminCommand(AdminFlags.Debug)]
public sealed class ZDigUpCommand : IConsoleCommand
{
    public string Command => "au_digup";
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/ZDigUpCommand.cs
    public string Description => Loc.GetString("cmd-au-digup-desc");
    public string Help => Loc.GetString("cmd-au-digup-help");
=======
    public string Description => Loc.GetString("cmu-cmd-digup-desc");
    public string Help => Loc.GetString("cmu-cmd-digup-help");
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/ZDigUpCommand.cs

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player?.AttachedEntity is not { } player)
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/ZDigUpCommand.cs
            shell.WriteError(Loc.GetString("cmd-au-dig-player-only"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-zdig-player-only"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/ZDigUpCommand.cs
            return;
        }

        var system = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<ZLevelBuildingSystem>();
        if (system.DigUp(player))
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/ZDigUpCommand.cs
            shell.WriteLine(Loc.GetString("cmd-au-digup-success"));
        else
            shell.WriteError(Loc.GetString("cmd-au-digup-failed"));
=======
            shell.WriteLine(Loc.GetString("cmu-cmd-digup-success"));
        else
            shell.WriteError(Loc.GetString("cmu-cmd-digup-failed"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/ZDigUpCommand.cs
    }
}
