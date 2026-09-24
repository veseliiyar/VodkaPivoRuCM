using Content.Shared.CMU14.Salvage;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Robust.Shared.Random;

namespace Content.Server.CMU14.Salvage;

public sealed partial class SalvageSpawnerSystem : EntitySystem
{
    [Dependency] private SharedDoAfterSystem _doAfter = default!;
    [Dependency] private SharedPopupSystem _popup = default!;
    [Dependency] private IRobustRandom _random = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SalvageSpawnerComponent, InteractHandEvent>(OnInteractHand);
        SubscribeLocalEvent<SalvageSpawnerComponent, SalvageSpawnerDoAfterEvent>(OnDoAfter);
    }

    private void OnInteractHand(EntityUid uid, SalvageSpawnerComponent comp, InteractHandEvent args)
    {
        if (args.Handled)
            return;
        if (comp.Loot.Count == 0)
            return;

<<<<<<< HEAD:Content.Server/AU14/Salvage/SalvageSpawnerSystem.cs
        _popup.PopupEntity(Loc.GetString("salvage-spawner-rummage"), uid, args.User); // RuMC edit
=======
        _popup.PopupEntity(Loc.GetString("salvage-spawner-rummage"), uid, args.User);
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/Salvage/SalvageSpawnerSystem.cs

        var doAfterArgs = new DoAfterArgs(
            EntityManager,
            args.User,
            comp.DoAfterTime,
            new SalvageSpawnerDoAfterEvent(),
            uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true,
            BlockDuplicate = true,
            DuplicateCondition = DuplicateConditions.SameTarget,
        };

        _doAfter.TryStartDoAfter(doAfterArgs);
        args.Handled = true;
    }

    private void OnDoAfter(EntityUid uid, SalvageSpawnerComponent comp, SalvageSpawnerDoAfterEvent args)
    {
        if (args.Cancelled || comp.Loot.Count == 0)
            return;

        var user = args.User;
        var pick = _random.Pick(comp.Loot);
        Spawn(pick, Transform(user).Coordinates);
<<<<<<< HEAD:Content.Server/AU14/Salvage/SalvageSpawnerSystem.cs
        _popup.PopupEntity(Loc.GetString("salvage-spawner-found"), uid, user); // RuMC edit
=======
        _popup.PopupEntity(Loc.GetString("salvage-spawner-found"), uid, user);
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Server/Salvage/SalvageSpawnerSystem.cs
    }
}
