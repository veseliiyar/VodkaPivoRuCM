using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.CMU14.ZLevels.Core;

[Serializable, NetSerializable]
public sealed partial class CMUZLevelLadderDoAfterEvent : SimpleDoAfterEvent
{
<<<<<<< HEAD:Content.Shared/_CMU14/ZLevels/Core/CMUZLevelLadderDoAfterEvent.cs
    public CMUZLevelLadderDoAfterEvent()
    {
    }

    public CMUZLevelLadderDoAfterEvent(int offset)
    {
        Offset = offset;
    }

    public int Offset { get; set; }
=======
    [DataField]
    public int Offset;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34:Content.CMU/Shared/ZLevels/Core/CMUZLevelLadderDoAfterEvent.cs
}
