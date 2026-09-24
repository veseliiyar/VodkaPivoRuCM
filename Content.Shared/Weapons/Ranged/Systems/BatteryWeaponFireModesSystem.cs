<<<<<<< HEAD
using Content.Shared._RMC14.Weapons.Common;
using Content.Shared.Access.Components;
=======
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
using Content.Shared.Access.Systems;
using Content.Shared.Database;
using Content.Shared.Examine;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Verbs;
using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.Prototypes;

namespace Content.Shared.Weapons.Ranged.Systems;

public sealed partial class BatteryWeaponFireModesSystem : EntitySystem
{
    [Dependency] private AccessReaderSystem _accessReaderSystem = default!;
    [Dependency] private SharedAppearanceSystem _appearanceSystem = default!;
<<<<<<< HEAD
    [Dependency] private SharedGunSystem _gunSystem = default!;
=======
    [Dependency] private SharedGunSystem _gun = default!;
    [Dependency] private SharedPopupSystem _popupSystem = default!;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BatteryWeaponFireModesComponent, UseInHandEvent>(OnUseInHandEvent);
        SubscribeLocalEvent<BatteryWeaponFireModesComponent, UniqueActionEvent>(OnUniqueAction);
        SubscribeLocalEvent<BatteryWeaponFireModesComponent, GetVerbsEvent<Verb>>(OnGetVerb);
        SubscribeLocalEvent<BatteryWeaponFireModesComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(Entity<BatteryWeaponFireModesComponent> ent, ref ExaminedEvent args)
    {
        if (ent.Comp.FireModes.Count < 2)
            return;

        var fireMode = GetMode(ent.Comp);

        if (!ProtoMan.TryIndex<EntityPrototype>(fireMode.Prototype, out var proto))
            return;

        args.PushMarkup(Loc.GetString("gun-set-fire-mode-examine", ("mode", proto.Name)));
    }

    private BatteryWeaponFireMode GetMode(BatteryWeaponFireModesComponent component)
    {
        return component.FireModes[component.CurrentFireMode];
    }

    private void OnGetVerb(EntityUid uid, BatteryWeaponFireModesComponent component, GetVerbsEvent<Verb> args)
    {
        if (!args.CanAccess || !args.CanInteract || !args.CanComplexInteract)
            return;

        if (component.FireModes.Count < 2)
            return;

        if (!_accessReaderSystem.IsAllowed(args.User, uid))
            return;

        for (var i = 0; i < component.FireModes.Count; i++)
        {
            var fireMode = component.FireModes[i];
            var entProto = ProtoMan.Index<EntityPrototype>(fireMode.Prototype);
            var index = i;

            var v = new Verb
            {
                Priority = 1,
                Category = VerbCategory.SelectType,
                Text = entProto.Name,
                Disabled = i == component.CurrentFireMode,
                Impact = LogImpact.Medium,
                DoContactInteraction = true,
                Act = () =>
                {
                    TrySetFireMode((uid, component), index, args.User);
                }
            };

            args.Verbs.Add(v);
        }
    }

    private void OnUseInHandEvent(Entity<BatteryWeaponFireModesComponent> ent, ref UseInHandEvent args)
    {
        if (args.Handled)
            return;

        args.Handled = true;
        TryCycleFireMode(ent, args.User);
    }

<<<<<<< HEAD
    private void OnUniqueAction(EntityUid uid, BatteryWeaponFireModesComponent component, UniqueActionEvent args)
    {
        if (args.Handled || !component.CycleOnUniqueAction)
            return;

        if (TryCycleFireMode(uid, component, args.UserUid))
            args.Handled = true;
    }

    public bool TryCycleFireMode(EntityUid uid, BatteryWeaponFireModesComponent component, EntityUid? user = null)
    {
        if (component.FireModes.Count < 2)
            return false;

        var index = (component.CurrentFireMode + 1) % component.FireModes.Count;
        return TrySetFireMode(uid, component, index, user);
=======
    public void TryCycleFireMode(Entity<BatteryWeaponFireModesComponent> ent, EntityUid? user = null)
    {
        if (ent.Comp.FireModes.Count < 2)
            return;

        var index = (ent.Comp.CurrentFireMode + 1) % ent.Comp.FireModes.Count;
        TrySetFireMode(ent, index, user);
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    }

    public bool TrySetFireMode(Entity<BatteryWeaponFireModesComponent> ent, int index, EntityUid? user = null)
    {
        if (index < 0 || index >= ent.Comp.FireModes.Count)
            return false;

        if (user != null && !_accessReaderSystem.IsAllowed(user.Value, ent))
            return false;

        SetFireMode(ent, index, user);

        return true;
    }

    private void SetFireMode(Entity<BatteryWeaponFireModesComponent> ent, int index, EntityUid? user = null)
    {
        var fireMode = ent.Comp.FireModes[index];
        ent.Comp.CurrentFireMode = index;
        Dirty(ent);

        if (ProtoMan.TryIndex<EntityPrototype>(fireMode.Prototype, out var prototype))
        {
            if (TryComp<AppearanceComponent>(ent, out var appearance))
                _appearanceSystem.SetData(ent, BatteryWeaponFireModeVisuals.State, prototype.ID, appearance);

            if (user != null)
<<<<<<< HEAD
            {
                var popup = fireMode.PopupText.Length > 0
                    ? fireMode.PopupText
                    : Loc.GetString("gun-set-fire-mode", ("mode", prototype.Name));

                _popupSystem.PopupClient(popup, uid, user.Value);
            }
=======
                _popupSystem.PopupEntity(Loc.GetString("gun-set-fire-mode-popup", ("mode", prototype.Name)), ent, user.Value);
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
        }

        if (TryComp(ent, out BatteryAmmoProviderComponent? batteryAmmoProviderComponent))
        {
            batteryAmmoProviderComponent.Prototype = fireMode.Prototype;
            batteryAmmoProviderComponent.FireCost = fireMode.FireCost;

            Dirty(ent, batteryAmmoProviderComponent);

            _gun.UpdateShots((ent, batteryAmmoProviderComponent));
        }

        if (fireMode.FireRate > 0 && TryComp(uid, out GunComponent? gun))
        {
            _gunSystem.SetFireRate((uid, gun), fireMode.FireRate);
            _gunSystem.RefreshModifiers((uid, gun));
        }
    }
}
