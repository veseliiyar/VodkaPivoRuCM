using Content.Shared._RMC14.Body;
using Content.Shared.Damage;
using Content.Shared.Damage.Systems;
using Content.Shared.Damage.Prototypes;
using Content.Shared.Drunk;
using Content.Shared.EntityEffects;
using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.FixedPoint;
using Content.Shared.Speech.EntitySystems;
using Content.Shared.Speech.Muting;
using Content.Shared.StatusEffectNew;
using Robust.Shared.Prototypes;

namespace Content.Shared._RMC14.Chemistry.Effects.Neutral;

public sealed partial class Focusing : RMCChemicalEffect
{
    private static readonly ProtoId<DamageTypePrototype> PoisonType = "Poison";

    protected override string ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        // RuMC edit start
        return Loc.GetString("reagent-effect-guidebook-rmc-focusing",
            ("alcohol", PotencyPerSecond),
            ("drunk", PotencyPerSecond * 2),
            ("powerful", ActualPotency >= 3),
            ("odToxin", PotencyPerSecond),
            ("critToxin", PotencyPerSecond * 3));
            // RuMC edit end
    }

    protected override void Tick(RMCChemicalEffectSystem system, DamageableSystem damageable, FixedPoint2 potency, RMCReagentEffectArgs args)
    {
        var bloodstream = system.RMCBloodstream;
        var drunkSystem = system.Drunk;
        var stutterSystem = system.Stuttering;
        var statusEffects = system.StatusEffects;

        bloodstream.RemoveBloodstreamAlcohols(args.TargetEntity, potency);
        drunkSystem.TryRemoveDrunkennessTime(args.TargetEntity, TimeSpan.FromSeconds(args.PotencyPerSecond * 2));
        stutterSystem.DoRemoveStutterTime(args.TargetEntity, args.PotencyPerSecond * 2);
        statusEffects.TryAddTime(args.TargetEntity, "Jitter", TimeSpan.FromSeconds(args.PotencyPerSecond * -2));
        // ReduceEyeBlur(PotencyPerSecond * 2) but BlurryVisionComponent is sealed so only healing the eyes will remove blur.

        if (args.ActualPotency < 3)
            return;
        system.Blindable.AdjustEyeDamage(args.TargetEntity, -9);
        system.RemoveMuted(args.TargetEntity);
    }

    protected override void TickOverdose(RMCChemicalEffectSystem system, DamageableSystem damageable, FixedPoint2 potency, RMCReagentEffectArgs args)
    {
        var damage = new DamageSpecifier();
        damage.DamageDict[PoisonType] = potency;
        damageable.TryChangeDamage(args.TargetEntity, damage, true, interruptsDoAfters: false);
    }

    protected override void TickCriticalOverdose(RMCChemicalEffectSystem system, DamageableSystem damageable, FixedPoint2 potency, RMCReagentEffectArgs args)
    {
        var damage = new DamageSpecifier();
        damage.DamageDict[PoisonType] = potency * 3;
        damageable.TryChangeDamage(args.TargetEntity, damage, true, interruptsDoAfters: false);
    }
}
