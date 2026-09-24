using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Damage;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(SharedRMCDamageableSystem))]
public sealed partial class DamageMultipliersComponent : Component
{
    // Runtime-added projectile components need a collection before their first network state is applied.
    [DataField(required: true), AutoNetworkedField]
<<<<<<< HEAD
    public Dictionary<DamageMultiplierFlag, float> Multipliers = new(); // RuMC edit
=======
    public Dictionary<DamageMultiplierFlag, float> Multipliers = new();
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
}
