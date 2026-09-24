// SPDX-License-Identifier: AGPL-3.0-only
// Copyright (c) 2026 wray-git
// SPDX-License-Identifier: AGPL-3.0-only
using System;
using Content.Server.Administration;
using Content.Shared.CMU14.ZLevelBuilding;
using Content.Shared.Administration;
using Robust.Shared.Console;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.CMU14.ZLevelBuilding;

/// <summary>
/// Building overhaul (z-level): lists every map with its AU14 "Multi Z-Level" status (whether players may build
/// the overhaul's stairs / vertical floors there) and lets an admin toggle it per map or globally at runtime.
///
/// This is the live counterpart to the mapper opt-out (<see cref="ZBuildableMapComponent"/> <c>enabled: false</c>
/// in a map file): use it to confirm which maps allow z-building and to switch a map off so players can't build
/// under it. The toggle is networked (the build condition + cave-in vignette respect it immediately) but, like
/// any runtime change, it is not persisted - bake it into the map prototype to make it permanent.
/// </summary>
[AdminCommand(AdminFlags.Mapping)]
public sealed class AU14MultiZCommand : IConsoleCommand
{
    public string Command => "au_multiz";
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
    public string Description => Loc.GetString("cmd-au-multiz-desc");
    public string Help => Loc.GetString("cmd-au-multiz-help");
=======
    public string Description => Loc.GetString("cmu-cmd-multiz-desc");
    public string Help => Loc.GetString("cmu-cmd-multiz-help");
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var entMan = IoCManager.Resolve<IEntityManager>();
        var building = entMan.System<ZLevelBuildingSystem>();

        // No args: list every map.
        if (args.Length == 0)
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
            shell.WriteLine(Loc.GetString("cmd-au-multiz-global-status",
                ("state", Loc.GetString(building.GloballyEnabled ? "cmd-au-multiz-enabled" : "cmd-au-multiz-disabled"))));
=======
            shell.WriteLine(Loc.GetString("cmu-cmd-multiz-global-list",
                ("state", Loc.GetString(building.GloballyEnabled ? "cmu-cmd-multiz-enabled" : "cmu-cmd-multiz-disabled"))));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            var query = entMan.AllEntityQueryEnumerator<MapComponent>();
            while (query.MoveNext(out var uid, out var map))
            {
                var yes = building.IsEnabledOn(uid);
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
                shell.WriteLine(Loc.GetString("cmd-au-multiz-map-status",
                    ("id", map.MapId),
                    ("map", entMan.ToPrettyString(uid)),
                    ("state", Loc.GetString(yes ? "cmd-au-multiz-yes" : "cmd-au-multiz-no"))));
=======
                shell.WriteLine(Loc.GetString("cmu-cmd-multiz-map-list",
                    ("mapId", $"{map.MapId,-4}"),
                    ("map", $"{entMan.ToPrettyString(uid),-28}"),
                    ("enabled", Loc.GetString(yes ? "cmu-cmd-multiz-yes" : "cmu-cmd-multiz-no"))));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            }
            return;
        }

        if (args.Length != 2)
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
            shell.WriteError(Loc.GetString("cmd-au-multiz-usage"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-multiz-usage"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            return;
        }

        var on = args[1].Equals("on", StringComparison.OrdinalIgnoreCase);
        if (!on && !args[1].Equals("off", StringComparison.OrdinalIgnoreCase))
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
            shell.WriteError(Loc.GetString("cmd-au-multiz-invalid-state"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-multiz-invalid-state"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            return;
        }

        // Global switch.
        if (args[0].Equals("global", StringComparison.OrdinalIgnoreCase))
        {
            building.GloballyEnabled = on;
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
            shell.WriteLine(Loc.GetString("cmd-au-multiz-global-changed",
                ("state", Loc.GetString(on ? "cmd-au-multiz-enabled" : "cmd-au-multiz-disabled"))));
=======
            shell.WriteLine(Loc.GetString("cmu-cmd-multiz-global-changed",
                ("state", Loc.GetString(on ? "cmu-cmd-multiz-enabled" : "cmu-cmd-multiz-disabled"))));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            return;
        }

        if (!int.TryParse(args[0], out var mapIdInt))
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
            shell.WriteError(Loc.GetString("cmd-au-multiz-invalid-map"));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-multiz-invalid-map"));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            return;
        }

        var mapManager = entMan.System<SharedMapSystem>();
        var mapId = new MapId(mapIdInt);
        if (!mapManager.MapExists(mapId))
        {
<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
            shell.WriteError(Loc.GetString("cmd-au-multiz-map-not-found", ("id", mapIdInt)));
=======
            shell.WriteError(Loc.GetString("cmu-cmd-multiz-map-missing", ("mapId", mapIdInt)));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
            return;
        }

        var mapUid = mapManager.GetMapOrInvalid(mapId);
        var comp = entMan.EnsureComponent<ZBuildableMapComponent>(mapUid);
        comp.Enabled = on;
        entMan.Dirty(mapUid, comp);

<<<<<<< HEAD:Content.Server/_AU14/ZLevelBuilding/AU14MultiZCommand.cs
        shell.WriteLine(Loc.GetString("cmd-au-multiz-map-changed",
            ("id", mapIdInt),
            ("state", Loc.GetString(on ? "cmd-au-multiz-yes" : "cmd-au-multiz-no")),
            ("permission", Loc.GetString(on ? "cmd-au-multiz-can-build" : "cmd-au-multiz-cannot-build"))));
=======
        shell.WriteLine(Loc.GetString("cmu-cmd-multiz-map-changed",
            ("mapId", mapIdInt),
            ("enabled", Loc.GetString(on ? "cmu-cmd-multiz-yes" : "cmu-cmd-multiz-no")),
            ("permission", Loc.GetString(on ? "cmu-cmd-multiz-can-build" : "cmu-cmd-multiz-cannot-build"))));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/ZLevelBuilding/AU14MultiZCommand.cs
    }
}
