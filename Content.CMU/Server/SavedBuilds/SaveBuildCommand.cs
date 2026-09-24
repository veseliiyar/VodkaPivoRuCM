// SPDX-License-Identifier: AGPL-3.0-only
// Copyright (c) 2026 wray-git
// SPDX-License-Identifier: AGPL-3.0-only
using Content.Shared.Administration;
using Robust.Shared.Console;

namespace Content.Server.CMU14.SavedBuilds;

/// <summary>
/// Test/dev command for the saved-builds save pipeline: serializes the player-built entities in a box
/// around you to a user-data file. Usage: <c>savebuild "My Base" [radius]</c> (radius 0-5, default 2).
/// The proper selection overlay drives the same <see cref="SavedBuildSystem"/> save path.
/// </summary>
[AnyCommand]
public sealed class SaveBuildCommand : IConsoleCommand
{
    public string Command => "savebuild";
<<<<<<< HEAD:Content.Server/_AU14/SavedBuilds/SaveBuildCommand.cs
    public string Description => Loc.GetString("cmd-savebuild-desc");
    public string Help => Loc.GetString("cmd-savebuild-help");
=======
    public string Description => Loc.GetString("cmu-cmd-savebuild-desc");
    public string Help => Loc.GetString("cmu-cmd-savebuild-help");
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/SavedBuilds/SaveBuildCommand.cs

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        if (shell.Player is not { } player)
        {
<<<<<<< HEAD:Content.Server/_AU14/SavedBuilds/SaveBuildCommand.cs
            shell.WriteError(Loc.GetString("cmd-savebuild-player-only"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-savebuild-player-only"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/SavedBuilds/SaveBuildCommand.cs
            return;
        }

        if (args.Length < 1)
        {
            shell.WriteLine(Help);
            return;
        }

        var radius = 2;
        if (args.Length >= 2 && !int.TryParse(args[1], out radius))
        {
<<<<<<< HEAD:Content.Server/_AU14/SavedBuilds/SaveBuildCommand.cs
            shell.WriteError(Loc.GetString("cmd-savebuild-invalid-radius"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-savebuild-radius-number"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/SavedBuilds/SaveBuildCommand.cs
            return;
        }

        var system = IoCManager.Resolve<IEntitySystemManager>().GetEntitySystem<SavedBuildSystem>();
        system.SaveAroundPlayer(player, args[0], radius);
    }
}
