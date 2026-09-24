using System.Numerics;
using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Dropship;

[RegisterComponent, NetworkedComponent]
[Access(typeof(SharedDropshipSystem))]
public sealed partial class DropshipDestinationComponent : Component
{
    [DataField]
    public EntityUid? Ship;

    [DataField]
    public bool AutoRecall;

    [DataField]
    public bool CanBePrimary = true;

    [DataField]
    public int LightSearchRadius = 14;

    [DataField]
    public EntityUid? ArrivalSoundEntity;

    [DataField("FactionControlling", required: false)]
    public string FactionController = String.Empty;

    [DataField("destinationtype")]
    public DestinationType Destinationtype = DestinationType.Dropship;

    [DataField("Home")]
    public bool Home = false;

<<<<<<< HEAD
    /// <summary>
    ///     Offset from this destination marker to the dropship grid origin on landing.
    /// </summary>
    [DataField("landingOffset")]
    public Vector2 LandingOffset;
=======
    // CMU14: Large multi-deck hulls can need a different center on the same pad.
    // This is expressed in the destination grid's coordinates; ordinary ships
    // keep using the marker itself.
    [DataField]
    public Vector2 MultiDeckOffset;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

    public enum DestinationType
    {
        Figher,
        Dropship,
        Bigship
    }
}
