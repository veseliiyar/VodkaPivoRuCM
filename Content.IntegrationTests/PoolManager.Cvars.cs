#nullable enable
using Content.Shared.CMU14.BalanceRating;
using Content.Shared.CCVar;
<<<<<<< HEAD
using Content.Shared.Corvax.CCCVars;
using Robust.Shared;
using Robust.Shared.Configuration;
using Robust.UnitTesting;
=======
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

namespace Content.IntegrationTests;

// Partial class containing test cvars
// This could probably be merged into the main file, but I'm keeping it separate to reduce
// conflicts for forks.
public static partial class PoolManager
{
    public static readonly (string cvar, string value)[] TestCvars =
    {
        // @formatter:off
        (CCVars.DatabaseSynchronous.Name,     "true"),
        (CCVars.DatabaseSnapshot.Name,        "true"),
        (CCVars.DatabaseSqliteDelay.Name,     "0"),
        (CCVars.HolidaysEnabled.Name,         "false"),
        (CCVars.GameMap.Name,                 TestMap),
        (CCVars.AdminLogsQueueSendDelay.Name, "0"),
        (CCVars.NPCMaxUpdates.Name,           "999999"),
        (CCVars.GameRoleTimers.Name,          "false"),
        (CCVars.GameRoleLoadoutTimers.Name,   "false"),
        (CCVars.GameRoleWhitelist.Name,       "false"),
        (CCVars.GridFill.Name,                "false"),
        (CCVars.PreloadGrids.Name,            "false"),
        (CCVars.ArrivalsShuttles.Name,        "false"),
        (CCVars.EmergencyShuttleEnabled.Name, "false"),
        (CCVars.ProcgenPreload.Name,          "false"),
        (CCVars.GameDummyTicker.Name,         "true"),
        (CCVars.GameLobbyEnabled.Name,        "false"),
        (CCVars.ConfigPresetDevelopment.Name, "false"),
        (CCVars.AdminLogsEnabled.Name,        "false"),
        (CCVars.AutosaveEnabled.Name,         "false"),
        (CCVars.InteractionRateLimitCount.Name, "9999999"),
        (CCVars.InteractionRateLimitPeriod.Name, "0.1"),
        (CCVars.MovementMobPushing.Name,       "false"),
        (CCVars.ResourceUploadingStoreDeletionDays.Name, "0"),
        (CMUBalanceRatingCVars.AutomaticEnabled.Name, "false"),
        (CCCVars.TTSEnabled.Name, "false"),
    };
}
