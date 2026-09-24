using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Content.Shared.CCVar;
using Robust.Shared.Configuration;

namespace Content.Server.Chat.Managers;

/// <summary>
///     Sanitizes messages!
///     It currently ony removes the shorthands for emotes (like "lol" or "^-^") from a chat message and returns the last
///     emote in their message
/// </summary>
public sealed partial class ChatSanitizationManager : IChatSanitizationManager
{
<<<<<<< HEAD
    private static readonly Dictionary<string, string> ShorthandToEmote = new()
    {
        // RuMC-start
        { "хд", "chatsan-laughs" },
        { "о-о", "chatsan-wide-eyed" }, // cyrillic о
        { "о.о", "chatsan-wide-eyed" }, // cyrillic о
        { "0_о", "chatsan-wide-eyed" }, // cyrillic о
        { "о/", "chatsan-waves" }, // cyrillic о
        { "о7", "chatsan-salutes" }, // cyrillic о
        { "0_o", "chatsan-wide-eyed" },
        { "лол", "chatsan-laughs" },
        { "лмао", "chatsan-laughs" },
        { "рофл", "chatsan-laughs" },
        { "яхз", "chatsan-shrugs" },
        { ":0", "chatsan-surprised" },
        { ":р", "chatsan-stick-out-tongue" }, // cyrillic р
        { "кек", "chatsan-laughs" },
        { "T_T", "chatsan-cries" },
        { "Т_Т", "chatsan-cries" }, // cyrillic T
        { "=_(", "chatsan-cries" },
        { "!с", "chatsan-laughs" },
        { "!в", "chatsan-sighs" },
        { "!х", "chatsan-claps" },
        { "!щ", "chatsan-snaps" },
        { "))", "chatsan-smiles-widely" },
        { ")", "chatsan-smiles" },
        { "(", "chatsan-frowns" },
        // RuMC-end
        { ":)", "chatsan-smiles" },
        { ":]", "chatsan-smiles" },
        { "=)", "chatsan-smiles" },
        { "=]", "chatsan-smiles" },
        { "(:", "chatsan-smiles" },
        { "[:", "chatsan-smiles" },
        { "(=", "chatsan-smiles" },
        { "[=", "chatsan-smiles" },
        { "^^", "chatsan-smiles" },
        { "^-^", "chatsan-smiles" },
        { ":(", "chatsan-frowns" },
        { ":[", "chatsan-frowns" },
        { "=(", "chatsan-frowns" },
        { "=[", "chatsan-frowns" },
        { "):", "chatsan-frowns" },
        { ")=", "chatsan-frowns" },
        { "]:", "chatsan-frowns" },
        { "]=", "chatsan-frowns" },
        { ":D", "chatsan-smiles-widely" },
        { "D:", "chatsan-frowns-deeply" },
        { ":O", "chatsan-surprised" },
        { "!", "chatsan-surprised" }, // RMC14
        { ":3", "chatsan-smiles" },
        { ":S", "chatsan-uncertain" },
        { ":>", "chatsan-grins" },
        { ":<", "chatsan-pouts" },
        { "xD", "chatsan-laughs" },
        { ":'(", "chatsan-cries" },
        { ":'[", "chatsan-cries" },
        { "='(", "chatsan-cries" },
        { "='[", "chatsan-cries" },
        { ")':", "chatsan-cries" },
        { "]':", "chatsan-cries" },
        { ")'=", "chatsan-cries" },
        { "]'=", "chatsan-cries" },
        { ";-;", "chatsan-cries" },
        { ";_;", "chatsan-cries" },
        { "qwq", "chatsan-cries" },
        { "t.t", "rmc-chatsan-emote-sobs" }, // RMC14 should be cries after case sensitive emote detection
        { "t-t", "rmc-chatsan-emote-sobs" }, // RMC14
        { "t_t", "rmc-chatsan-emote-sobs" }, // RMC14
        { "t~t", "rmc-chatsan-emote-sobs" }, // RMC14
//        { "T.t", "chatsan-cries" }, // RMC14
//        { "T-t", "chatsan-cries" }, // RMC14
//        { "T_t", "chatsan-cries" }, // RMC14
//        { "T~t", "chatsan-cries" }, // RMC14
//        { "t.T", "chatsan-cries" }, // RMC14
//        { "t-T", "chatsan-cries" }, // RMC14
//        { "t_T", "chatsan-cries" }, // RMC14
//        { "t~T", "chatsan-cries" }, // RMC14
//        { "T.T", "rmc-chatsan-emote-sobs" }, // RMC14 pending case sensitive emote detection option, make lowercase cries
//        { "T-T", "rmc-chatsan-emote-sobs" }, // RMC14
//        { "T_T", "rmc-chatsan-emote-sobs" }, // RMC14
//        { "T~T", "rmc-chatsan-emote-sobs" }, // RMC14
        { ":u", "chatsan-smiles-smugly" },
        { ":v", "chatsan-smiles-smugly" },
        { ">:i", "chatsan-annoyed" },
        { ":i", "chatsan-sighs" },
        { ":|", "chatsan-sighs" },
        { ":p", "chatsan-stick-out-tongue" },
        { ";p", "chatsan-stick-out-tongue" },
        { ":b", "chatsan-stick-out-tongue" },
        { "0-0", "chatsan-wide-eyed" },
        { "o-o", "chatsan-wide-eyed" },
        { "o.o", "chatsan-wide-eyed" },
        { "._.", "chatsan-surprised" },
        { ".-.", "chatsan-confused" },
        { "?", "chatsan-confused" }, // RMC14
        { "-_-", "chatsan-unimpressed" },
        { "smh", "chatsan-unimpressed" },
        { "o/", "chatsan-waves" },
        { "^^/", "chatsan-waves" },
        { ":/", "chatsan-uncertain" },
        { ":\\", "chatsan-uncertain" },
        { "lmao", "chatsan-laughs" },
        { "lmfao", "chatsan-laughs" },
        { "lol", "chatsan-laughs" },
        { "lel", "chatsan-laughs" },
        { "kek", "chatsan-laughs" },
        { "rofl", "chatsan-laughs" },
        { "o7", "chatsan-salutes" },
        { "a10", "chatsan-comes-to-attention" },
        { ";_;7", "chatsan-tearfully-salutes" },
        { ";-;7", "chatsan-tearfully-salutes" }, // RMC14
        { "t.t7", "chatsan-tearfully-salutes" }, // RMC14
        { "t-t7", "chatsan-tearfully-salutes" }, // RMC14
        { "t_t7", "chatsan-tearfully-salutes" }, // RMC14
        { "t~t7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T.t7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T-t7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T_t7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T~t7", "chatsan-tearfully-salutes" }, // RMC14
//        { "t.T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "t-T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "t_T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "t~T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T.T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T-T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T_T7", "chatsan-tearfully-salutes" }, // RMC14
//        { "T~T7", "chatsan-tearfully-salutes" }, // RMC14
        { "idk", "chatsan-shrugs" },
        { "idgaf", "chatsan-shrugs" }, // RMC14
        { ";)", "chatsan-winks" },
        { ";]", "chatsan-winks" },
        { "(;", "chatsan-winks" },
        { "[;", "chatsan-winks" },
        { ":')", "chatsan-tearfully-smiles" },
        { ":']", "chatsan-tearfully-smiles" },
        { "=')", "chatsan-tearfully-smiles" },
        { "=']", "chatsan-tearfully-smiles" },
        { "(':", "chatsan-tearfully-smiles" },
        { "[':", "chatsan-tearfully-smiles" },
        { "('=", "chatsan-tearfully-smiles" },
        { "['=", "chatsan-tearfully-smiles" }
    };
=======
    private static readonly (Regex regex, string emoteKey)[] ShorthandToEmote =
    [
        Entry(":)", "chatsan-smiles"),
        Entry(":]", "chatsan-smiles"),
        Entry("=)", "chatsan-smiles"),
        Entry("=]", "chatsan-smiles"),
        Entry("(:", "chatsan-smiles"),
        Entry("[:", "chatsan-smiles"),
        Entry("(=", "chatsan-smiles"),
        Entry("[=", "chatsan-smiles"),
        Entry("^^", "chatsan-smiles"),
        Entry("^-^", "chatsan-smiles"),
        Entry(":(", "chatsan-frowns"),
        Entry(":[", "chatsan-frowns"),
        Entry("=(", "chatsan-frowns"),
        Entry("=[", "chatsan-frowns"),
        Entry("):", "chatsan-frowns"),
        Entry(")=", "chatsan-frowns"),
        Entry("]:", "chatsan-frowns"),
        Entry("]=", "chatsan-frowns"),
        Entry(":D", "chatsan-smiles-widely"),
        Entry("D:", "chatsan-frowns-deeply"),
        Entry(":O", "chatsan-surprised"),
        Entry("!", "chatsan-surprised"), // RMC14
        Entry(":3", "chatsan-smiles"),
        Entry(":S", "chatsan-uncertain"),
        Entry(":>", "chatsan-grins"),
        Entry(":<", "chatsan-pouts"),
        Entry("xD", "chatsan-laughs"),
        Entry(":'(", "chatsan-cries"),
        Entry(":'[", "chatsan-cries"),
        Entry("='(", "chatsan-cries"),
        Entry("='[", "chatsan-cries"),
        Entry(")':", "chatsan-cries"),
        Entry("]':", "chatsan-cries"),
        Entry(")'=", "chatsan-cries"),
        Entry("]'=", "chatsan-cries"),
        Entry(";-;", "chatsan-cries"),
        Entry(";_;", "chatsan-cries"),
        Entry("qwq", "chatsan-cries"),
        Entry("t.t", "rmc-chatsan-emote-sobs"), // RMC14
        Entry("t-t", "rmc-chatsan-emote-sobs"), // RMC14
        Entry("t_t", "rmc-chatsan-emote-sobs"), // RMC14
        Entry("t~t", "rmc-chatsan-emote-sobs"), // RMC14
        Entry(":u", "chatsan-smiles-smugly"),
        Entry(":v", "chatsan-smiles-smugly"),
        Entry(">:i", "chatsan-annoyed"),
        Entry(":i", "chatsan-sighs"),
        Entry(":|", "chatsan-sighs"),
        Entry(":p", "chatsan-stick-out-tongue"),
        Entry(";p", "chatsan-stick-out-tongue"),
        Entry(":b", "chatsan-stick-out-tongue"),
        Entry("0-0", "chatsan-wide-eyed"),
        Entry("o-o", "chatsan-wide-eyed"),
        Entry("o.o", "chatsan-wide-eyed"),
        Entry("._.", "chatsan-surprised"),
        Entry(".-.", "chatsan-confused"),
        Entry("?", "chatsan-confused"), // RMC14
        Entry("-_-", "chatsan-unimpressed"),
        Entry("smh", "chatsan-unimpressed"),
        Entry(":?", "chatsan-shrugs"),
        Entry("o/", "chatsan-waves"),
        Entry("^^/", "chatsan-waves"),
        Entry(":/", "chatsan-uncertain"),
        Entry(":\\", "chatsan-uncertain"),
        Entry("lmao", "chatsan-laughs"),
        Entry("lmfao", "chatsan-laughs"),
        Entry("lol", "chatsan-laughs"),
        Entry("lel", "chatsan-laughs"),
        Entry("kek", "chatsan-laughs"),
        Entry("rofl", "chatsan-laughs"),
        Entry("o7", "chatsan-salutes"),
        Entry("a10", "chatsan-comes-to-attention"), // RMC14
        Entry(";_;7", "chatsan-tearfully-salutes"),
        Entry(";-;7", "chatsan-tearfully-salutes"), // RMC14
        Entry("t.t7", "chatsan-tearfully-salutes"), // RMC14
        Entry("t-t7", "chatsan-tearfully-salutes"), // RMC14
        Entry("t_t7", "chatsan-tearfully-salutes"), // RMC14
        Entry("t~t7", "chatsan-tearfully-salutes"), // RMC14
        Entry("idgaf", "chatsan-shrugs"), // RMC14
        Entry(";)", "chatsan-winks"),
        Entry(";]", "chatsan-winks"),
        Entry("(;", "chatsan-winks"),
        Entry("[;", "chatsan-winks"),
        Entry(":')", "chatsan-tearfully-smiles"),
        Entry(":']", "chatsan-tearfully-smiles"),
        Entry("=')", "chatsan-tearfully-smiles"),
        Entry("=']", "chatsan-tearfully-smiles"),
        Entry("(':", "chatsan-tearfully-smiles"),
        Entry("[':", "chatsan-tearfully-smiles"),
        Entry("('=", "chatsan-tearfully-smiles"),
        Entry("['=", "chatsan-tearfully-smiles"),
    ];
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34

    [Dependency] private IConfigurationManager _configurationManager = default!;
    [Dependency] private ILocalizationManager _loc = default!;

    private bool _doSanitize;

    public void Initialize()
    {
        _configurationManager.OnValueChanged(CCVars.ChatSanitizerEnabled, x => _doSanitize = x, true);
    }

    /// <summary>
    ///     Remove the shorthands from the message, returning the last one found as the emote
    /// </summary>
    /// <param name="message">The pre-sanitized message</param>
    /// <param name="speaker">The speaker</param>
    /// <param name="sanitized">The sanitized message with shorthands removed</param>
    /// <param name="emote">The localized emote</param>
    /// <returns>True if emote has been sanitized out</returns>
    public bool TrySanitizeEmoteShorthands(string message,
        EntityUid speaker,
        out string sanitized,
        [NotNullWhen(true)] out string? emote)
    {
        emote = null;
        sanitized = message;

        if (!_doSanitize)
            return false;

        // -1 is just a canary for nothing found yet
        var lastEmoteIndex = -1;

        foreach (var (r, emoteKey) in ShorthandToEmote)
        {
            // We're using sanitized as the original message until the end so that we can make sure the indices of
            // the emotes are accurate.
            var lastMatch = r.Match(sanitized);

            if (!lastMatch.Success)
                continue;

            if (lastMatch.Index > lastEmoteIndex)
            {
                lastEmoteIndex = lastMatch.Index;
                emote = _loc.GetString(emoteKey, ("ent", speaker));
            }

            message = r.Replace(message, string.Empty);
        }

        sanitized = message; // RMC14
        return emote is not null;
    }

    private static (Regex regex, string emoteKey) Entry(string shorthand, string emoteKey)
    {
        // We have to escape it because shorthands like ":)" or "-_-" would break the regex otherwise.
        var escaped = Regex.Escape(shorthand);

        // So there are 2 cases:
        // - If there is whitespace before it and after it is either punctuation, whitespace, or the end of the line
        //   Delete the word and the whitespace before
        // - If it is at the start of the string and is followed by punctuation, whitespace, or the end of the line
        //   Delete the word and the punctuation if it exists.
        var pattern = new Regex(
            $@"\s{escaped}(?=\p{{P}}|\s|$)|^{escaped}(?:\p{{P}}|(?=\s|$))",
            RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Compiled);

        return (pattern, emoteKey);
    }
}
