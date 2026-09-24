using Robust.Shared.GameStates;
<<<<<<< HEAD
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;
=======
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

namespace Content.Shared.Corvax.TTS;

/// <summary>
<<<<<<< HEAD
/// Apply TTS for entity chat say messages
=======
/// Enables text-to-speech for entity speech.
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class TTSComponent : Component
{
    /// <summary>
<<<<<<< HEAD
    /// Prototype of used voice for TTS.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("voice", customTypeSerializer: typeof(PrototypeIdSerializer<TTSVoicePrototype>))]
    public string? VoicePrototypeId { get; set; }

    /// <summary>
    /// Фракция говорящего (ксено / люди)
=======
    /// TTS voice prototype ID or runtime custom voice ID.
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("voice")]
    public string? VoicePrototypeId { get; set; }

    /// <summary>
    /// Speaker hearing faction.
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    /// </summary>
    [ViewVariables(VVAccess.ReadWrite)]
    [DataField("faction")]
    public HearingFaction Faction { get; set; } = HearingFaction.Human;
}

public enum HearingFaction
{
    Human,
    Xeno
}
