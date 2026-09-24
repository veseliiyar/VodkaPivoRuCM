<<<<<<< HEAD
using System.Numerics;
using Content.Shared.Damage;
using Content.Shared.Damage.Prototypes;
using Content.Shared._CMU14.Yautja;
using Robust.Shared.Audio;
=======
using Content.Shared.Blocking.Components;
using Content.Shared.Damage;
using Content.Shared.Damage.Systems;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;

namespace Content.Shared.Blocking;

public sealed partial class BlockingSystem
{
    [Dependency] private DamageableSystem _damageable = default!;
    [Dependency] private SharedAudioSystem _audio = default!;

    private void InitializeUser()
    {
        SubscribeLocalEvent<BlockingUserComponent, DamageModifyEvent>(OnUserDamageModified);
        SubscribeLocalEvent<BlockingUserComponent, EntParentChangedMessage>(OnParentChanged);
        SubscribeLocalEvent<BlockingUserComponent, ContainerGettingInsertedAttemptEvent>(OnInsertAttempt);
        SubscribeLocalEvent<BlockingUserComponent, AnchorStateChangedEvent>(OnAnchorChanged);
        SubscribeLocalEvent<BlockingUserComponent, EntityTerminatingEvent>(OnEntityTerminating);
    }

    private void OnParentChanged(Entity<BlockingUserComponent> entity, ref EntParentChangedMessage args)
    {
        UserStopBlocking(entity);
    }

    private void OnInsertAttempt(Entity<BlockingUserComponent> entity, ref ContainerGettingInsertedAttemptEvent args)
    {
        UserStopBlocking(entity);
    }

    private void OnAnchorChanged(Entity<BlockingUserComponent> entity, ref AnchorStateChangedEvent args)
    {
        if (args.Anchored)
            return;

        UserStopBlocking(entity);
    }

    private void OnUserDamageModified(Entity<BlockingUserComponent> entity, ref DamageModifyEvent args)
    {
<<<<<<< HEAD
        if (TryComp<BlockingComponent>(component.BlockingItem, out var blocking))
        {
            if (args.Damage.GetTotal() <= 0)
                return;

            if (TryComp<YautjaSourceShieldBlockComponent>(component.BlockingItem, out var sourceBlock) &&
                sourceBlock.ShieldType is YautjaSourceShieldType.Directional or YautjaSourceShieldType.DirectionalTwoHands &&
                args.Origin is { } attacker &&
                !IsShieldFacingAttacker(uid, attacker))
            {
                return;
            }

            // A shield should only block damage it can itself absorb. To determine that we need the Damageable component on it.
            if (!TryComp<DamageableComponent>(component.BlockingItem, out var dmgComp))
                return;

            var blockFraction = blocking.IsBlocking ? blocking.ActiveBlockFraction : blocking.PassiveBlockFraction;
            blockFraction = Math.Clamp(blockFraction, 0, 1);
            _damageable.TryChangeDamage(component.BlockingItem, blockFraction * args.OriginalDamage);

            var modify = new DamageModifierSet();
            foreach (var key in dmgComp.Damage.DamageDict.Keys)
            {
                modify.Coefficients.TryAdd(key, 1 - blockFraction);
            }

            args.Damage = DamageSpecifier.ApplyModifierSet(args.Damage, modify);

            if (blocking.IsBlocking && !args.Damage.Equals(args.OriginalDamage))
            {
                _audio.PlayPvs(blocking.BlockSound, uid);
            }
        }
    }

    private bool IsShieldFacingAttacker(EntityUid user, EntityUid attacker)
    {
        var delta = _transformSystem.GetWorldPosition(attacker) - _transformSystem.GetWorldPosition(user);
        if (delta.LengthSquared() <= 0.0001f)
            return true;

        var forward = _transformSystem.GetWorldRotation(user).ToWorldVec();
        return Vector2.Dot(forward, delta) >= 0;
    }

    private void OnDamageModified(EntityUid uid, BlockingComponent component, DamageModifyEvent args)
    {
        var modifier = component.IsBlocking ? component.ActiveBlockDamageModifier : component.PassiveBlockDamageModifer;
        if (modifier == null)
        {
            return;
        }

        args.Damage = DamageSpecifier.ApplyModifierSet(args.Damage, modifier);
    }

    private void OnEntityTerminating(EntityUid uid, BlockingUserComponent component, ref EntityTerminatingEvent args)
    {
        if (!TryComp<BlockingComponent>(component.BlockingItem, out var blockingComponent))
=======
        if (entity.Comp.BlockingItem is not { } item || !_blockQuery.TryComp(item, out var blocking))
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
            return;

        if (args.Damage.GetTotal() <= 0)
            return;

        var blockFraction = blocking.IsRaised ? blocking.ActiveBlockFraction : blocking.PassiveBlockFraction;
        blockFraction = Math.Clamp(blockFraction, 0, 1);

        // This is how much damage the shield is attempting to block
        var split = args.OriginalDamage * blockFraction;
        var damage = _damageable.ChangeDamage(item, split);

        // Of the damage that went through, reduce by the appropriate blocking modifiers.
        var modifier = GetBlockingModifier((item, blocking));
        var blowthrough = DamageSpecifier.ApplyModifierSet(split, modifier);

        args.Damage *= 1f - blockFraction;
        args.Damage += blowthrough;

        if (blocking.IsRaised && damage.AnyPositive())
            _audio.PlayPvs(blocking.BlockSound, entity);
    }

    private void OnEntityTerminating(Entity<BlockingUserComponent> entity, ref EntityTerminatingEvent args)
    {
        if (!_blockQuery.TryComp(entity.Comp.BlockingItem, out var blockComponent))
            return;

        StopBlocking((entity.Comp.BlockingItem.Value, blockComponent), entity);
    }

    /// <summary>
    /// Check for the shield and has the user stop blocking
    /// Used where you'd like the user to stop blocking, but also don't want to remove the <see cref="BlockingUserComponent"/>
    /// </summary>
    /// <param name="entity">The user blocking</param>
    private void UserStopBlocking(Entity<BlockingUserComponent> entity)
    {
        if (!_blockQuery.TryComp(entity.Comp.BlockingItem, out var blockComponent))
            return;

        LowerShield((entity.Comp.BlockingItem.Value, blockComponent), entity);
    }
}
