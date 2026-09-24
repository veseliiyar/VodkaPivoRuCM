using System.Collections.Frozen;
using Content.Shared.CMU14.Chat;
using Content.Shared._RMC14.Voicelines;
using Content.Shared.Chat.Prototypes;
using Robust.Shared.Player;
using Robust.Shared.Random;

namespace Content.Server.Chat.Systems;

public sealed partial class ChatSystem
{
    private const string ScreamEmoteId = "Scream";

    private static readonly string[] RunechatPainMessages =
    [
<<<<<<< HEAD
        // RuMC edit start
        "rmc-runechat-pain-ow",
        "rmc-runechat-pain-agh",
        "rmc-runechat-pain-argh",
        "rmc-runechat-pain-ouch",
        "rmc-runechat-pain-ack",
        "rmc-runechat-pain-ouf",
        // RuMC edit end
=======
        "АУ!!",
        "АГХ!!",
        "АРГХ!!",
        "АУЧ!!",
        "АЙ!!",
        "УФ!",
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    ];

    private static readonly string[] RunechatScreamMessages =
    [
<<<<<<< HEAD
        // RuMC edit start
        "rmc-runechat-scream-fuck",
        "rmc-runechat-scream-agh",
        "rmc-runechat-scream-argh",
        "rmc-runechat-scream-aaaa",
        "rmc-runechat-scream-hgh",
        "rmc-runechat-scream-nghhh",
        "rmc-runechat-scream-nnhh",
        "rmc-runechat-scream-shit",
        // RuMC edit end
=======
        "БЛЯТЬ!!!",
        "АГХ!!!",
        "АРГХ!!!",
        "АААА!!!",
        "НГХ!!!",
        "НГХХХХ!!!",
        "ННХХ!!!",
        "СУКА!!!",
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    ];

    private static readonly FrozenSet<string> PainEmoteIds = new[]
    {
        "PainGrimace",
        "TroubleEyeOpen",
        "TroubleStanding",
    }.ToFrozenSet();

    [Dependency] private HumanoidVoicelinesSystem _humanoidVoicelines = default!;

    protected override void GetEmotePresentation(
        EmotePrototype emote,
<<<<<<< HEAD
        ChatTransmitRange range = ChatTransmitRange.Normal,
        bool hideLog = false,
        string? nameOverride = null,
        bool ignoreActionBlocker = false,
        bool forceEmote = false
        )
    {
        emote = GetEmoteOverride(source, emote);

        if (!forceEmote && !AllowedToUseEmote(source, emote))
            return;

        // check if proto has valid message for chat
        if (emote.ChatMessages.Count != 0)
        {
            // not all emotes are loc'd, but for the ones that are we pass in entity
            var action = Loc.GetString(_random.Pick(emote.ChatMessages), ("entity", source));
            var bubbleMessage = GetRunechatEmoteMessage(emote, out var speechStyleClass);
            SendEntityEmote(
                source,
                action,
                range,
                nameOverride,
                hideLog: hideLog,
                checkEmote: false,
                ignoreActionBlocker: ignoreActionBlocker,
                speechBubbleMessage: bubbleMessage,
                speechStyleClass: speechStyleClass);
        }

        // do the rest of emote event logic here
        TryEmoteWithoutChat(source, emote, ignoreActionBlocker);
    }

    private string? GetRunechatEmoteMessage(EmotePrototype emote, out string? speechStyleClass)
=======
        out string? speechBubbleMessage,
        out string? speechStyleClass)
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    {
        if (emote.ID == ScreamEmoteId)
        {
            speechBubbleMessage = _random.Pick(RunechatScreamMessages);
            speechStyleClass = CMURunechatStyles.Scream;
<<<<<<< HEAD
            return Loc.GetString(_random.Pick(RunechatScreamMessages)); // RuMC edit
=======
            return;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
        }

        if (PainEmoteIds.Contains(emote.ID))
        {
            speechBubbleMessage = _random.Pick(RunechatPainMessages);
            speechStyleClass = CMURunechatStyles.Pain;
<<<<<<< HEAD
            return Loc.GetString(_random.Pick(RunechatPainMessages)); // RuMC edit
=======
            return;
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
        }

        speechBubbleMessage = null;
        speechStyleClass = null;
    }

    protected override bool CanInvokeChatEmote(EntityUid source, EmotePrototype emote)
    {
        return _rmcEmote.TryEmote(source);
    }

    protected override Filter GetEmoteSoundFilter(EntityUid source)
    {
<<<<<<< HEAD
        proto = GetEmoteOverride(uid, proto);

        if (!_actionBlocker.CanEmote(uid) && !ignoreActionBlocker)
            return;

        InvokeEmoteEvent(uid, proto);
    }

    /// <summary>
    ///     Tries to find and play relevant emote sound in emote sounds collection.
    /// </summary>
    /// <returns>True if emote sound was played.</returns>
    public bool TryPlayEmoteSound(EntityUid uid, EmoteSoundsPrototype? proto, EmotePrototype emote, AudioParams? audioParams = null)
    {
        return TryPlayEmoteSound(uid, proto, emote.ID, audioParams);
    }

    /// <summary>
    ///     Tries to find and play relevant emote sound in emote sounds collection.
    /// </summary>
    /// <returns>True if emote sound was played.</returns>
    public bool TryPlayEmoteSound(EntityUid uid, EmoteSoundsPrototype? proto, string emoteId, AudioParams? audioParams = null)
    {
        if (proto == null)
            return false;

        // try to get specific sound for this emote
        if (!proto.Sounds.TryGetValue(emoteId, out var sound))
        {
            // no specific sound - check fallback
            sound = proto.FallbackSound;
            if (sound == null)
                return false;
        }

        // optional override params > general params for all sounds in set > individual sound params
        var param = audioParams ?? proto.GeneralParams ?? sound.Params;

        // RMC14
        var filter = Filter.Pvs(uid).RemoveWhere(s => !_humanoidVoicelines.ShouldPlayEmote(uid, s));
        if (filter.Count == 0)
            return false;

        _audio.PlayEntity(sound, filter, uid, true, param);
        // RMC14

        return true;
    }
    /// <summary>
    /// Checks if a valid emote was typed, to play sounds and etc and invokes an event.
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="textInput"></param>
    private void TryEmoteChatInput(EntityUid uid, string textInput)
    {
        var actionTrimmedLower = TrimPunctuation(textInput.ToLower());
        if (!_wordEmoteDict.TryGetValue(actionTrimmedLower, out var emote))
            return;

        emote = GetEmoteOverride(uid, emote);

        if (!AllowedToUseEmote(uid, emote))
            return;

        if (!_rmcEmote.TryEmote(uid))
            return;

        InvokeEmoteEvent(uid, emote);
        return;

        static string TrimPunctuation(string textInput)
        {
            var trimEnd = textInput.Length;
            while (trimEnd > 0 && char.IsPunctuation(textInput[trimEnd - 1]))
            {
                trimEnd--;
            }

            var trimStart = 0;
            while (trimStart < trimEnd && char.IsPunctuation(textInput[trimStart]))
            {
                trimStart++;
            }

            return textInput[trimStart..trimEnd];
        }
    }
    /// <summary>
    /// Checks if we can use this emote based on the emotes whitelist, blacklist, and availibility to the entity.
    /// </summary>
    /// <param name="source">The entity that is speaking</param>
    /// <param name="emote">The emote being used</param>
    /// <returns></returns>
    private bool AllowedToUseEmote(EntityUid source, EmotePrototype emote)
    {
        // If emote is in AllowedEmotes, it will bypass whitelist and blacklist
        if (TryComp<SpeechComponent>(source, out var speech) &&
            speech.AllowedEmotes.Contains(emote.ID))
        {
            return true;
        }

        // Check the whitelist and blacklist
        if (_whitelistSystem.IsWhitelistFail(emote.Whitelist, source) ||
            _whitelistSystem.IsBlacklistPass(emote.Blacklist, source))
        {
            return false;
        }

        // Check if the emote is available for all
        if (!emote.Available)
        {
            return false;
        }

        return true;
    }

    private EmotePrototype GetEmoteOverride(EntityUid source, EmotePrototype emote)
    {
        if (!TryComp<SpeechComponent>(source, out var speech) ||
            !speech.EmoteOverrides.TryGetValue(emote.ID, out var overrideId) ||
            !_prototypeManager.TryIndex(overrideId, out EmotePrototype? overrideEmote))
        {
            return emote;
        }

        return overrideEmote;
    }


    private void InvokeEmoteEvent(EntityUid uid, EmotePrototype proto)
    {
        var ev = new EmoteEvent(proto);
        RaiseLocalEvent(uid, ref ev);
    }
}

/// <summary>
///     Raised by chat system when entity made some emote.
///     Use it to play sound, change sprite or something else.
/// </summary>
[ByRefEvent]
public struct EmoteEvent
{
    public bool Handled;
    public readonly EmotePrototype Emote;

    public EmoteEvent(EmotePrototype emote)
    {
        Emote = emote;
        Handled = false;
=======
        return Filter.Pvs(source)
            .RemoveWhere(session => !_humanoidVoicelines.ShouldPlayEmote(source, session));
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    }
}
