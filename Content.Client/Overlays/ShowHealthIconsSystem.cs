using Content.Client._RMC14.Medical.HUD;
<<<<<<< HEAD
using Content.Shared._RMC14.Xenonids;
using Content.Shared.Damage;
=======
using Content.Shared.Damage.Components;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
using Content.Shared.Inventory.Events;
using Content.Shared.Overlays;
using Content.Shared.StatusIcon.Components;

namespace Content.Client.Overlays;

/// <summary>
/// Shows a healthy icon on mobs.
/// </summary>
public sealed partial class ShowHealthIconsSystem : EquipmentHudSystem<ShowHealthIconsComponent>
{
    [Dependency] private CMHealthIconsSystem _healthIcons = default!;
<<<<<<< HEAD
    [Dependency] private CMXenoHealthIconsSystem _xenoHealthIcons = default!;

=======
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    [ViewVariables]
    public HashSet<string> DamageContainers = new();

    public override void Initialize()
    {
        base.Initialize();

<<<<<<< HEAD
        SubscribeLocalEvent<DamageableComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
        SubscribeLocalEvent<XenoComponent, GetStatusIconsEvent>(OnGetXenoStatusIcons);
=======
        SubscribeLocalEvent<InjurableComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
        SubscribeLocalEvent<ShowHealthIconsComponent, AfterAutoHandleStateEvent>(OnHandleState);
    }

    protected override void UpdateInternal(RefreshEquipmentHudEvent<ShowHealthIconsComponent> component)
    {
        base.UpdateInternal(component);

        DamageContainers.Clear();
<<<<<<< HEAD

=======
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
        foreach (var comp in component.Components)
        {
            foreach (var damageContainerId in comp.DamageContainers)
            {
                DamageContainers.Add(damageContainerId);
            }
        }
    }

    protected override void DeactivateInternal()
    {
        base.DeactivateInternal();

        DamageContainers.Clear();
    }

    private void OnHandleState(Entity<ShowHealthIconsComponent> ent, ref AfterAutoHandleStateEvent args)
    {
        RefreshOverlay();
    }

    private void OnGetStatusIconsEvent(Entity<InjurableComponent> entity, ref GetStatusIconsEvent args)
    {
        if (!IsActive || HasComp<XenoComponent>(entity) || !IsAllowedDamageContainer(entity.Comp))
            return;

        if (entity.Comp.DamageContainer == null ||
            !DamageContainers.Contains(entity.Comp.DamageContainer) ||
            !TryComp(entity, out DamageableComponent? damageable))
        {
            return;
        }

        if (_healthIcons.TryGetIcon((entity.Owner, damageable), out var healthIcon))
            args.StatusIcons.Add(healthIcon);
    }

    private void OnGetXenoStatusIcons(Entity<XenoComponent> entity, ref GetStatusIconsEvent args)
    {
        if (!IsActive || !TryComp<DamageableComponent>(entity, out var damageable) ||
            !IsAllowedDamageContainer(damageable))
        {
            return;
        }

        if (_xenoHealthIcons.TryGetIcon(entity, out var healthIcon) && healthIcon is not null)
            args.StatusIcons.Add(healthIcon);
    }

    private bool IsAllowedDamageContainer(DamageableComponent damageable)
    {
        return damageable.DamageContainerID is { } id && DamageContainers.Contains(id);
    }
}
