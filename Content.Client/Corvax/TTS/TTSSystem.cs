<<<<<<< HEAD
using Content.Shared.Chat;
using Content.Shared.Corvax.CCCVars;
using Content.Shared.Corvax.TTS;
using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Components;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Configuration;
using Robust.Shared.ContentPack;
using Robust.Shared.Player;
using Robust.Shared.Utility;

namespace Content.Client.Corvax.TTS;

/// <summary>
/// Plays TTS audio in world
/// </summary>
// ReSharper disable once InconsistentNaming
public sealed class TTSSystem : EntitySystem
{
    [Dependency] private readonly IConfigurationManager _cfg = default!;
    [Dependency] private readonly IResourceManager _res = default!;
    [Dependency] private readonly AudioSystem _audio = default!;

    private ISawmill _sawmill = default!;
    private static MemoryContentRoot _contentRoot = new();
    private static readonly ResPath Prefix = ResPath.Root / "TTS";

    private static bool _contentRootAdded;

    /// <summary>
    /// Reducing the volume of the TTS when whispering. Will be converted to logarithm.
    /// </summary>
    private const float WhisperFade = 4f;

    /// <summary>
    /// The volume at which the TTS sound will not be heard.
    /// </summary>
    private const float MinimalVolume = -10f;

    /// <summary>
    /// Occlusion value applied to radio TTS audio to simulate bandpass filter.
    /// cutoff = exp(-RadioOcclusion) ≈ 0.082 HF gain — characteristic muffled radio sound.
    /// </summary>
    private const float RadioOcclusion = 2.5f;

    private static readonly SoundSpecifier RadioStaticSound =
        new SoundPathSpecifier("/Audio/_RMC14/Effects/radiostatic.ogg");

    private float _volume = 0.0f;
    private int _fileIdx = 0;
    private readonly Dictionary<uint, (EntityUid Audio, NetEntity? Source)> _trackedPlayback = new();

    public event Action<AddReferenceVoiceResponse>? ReferenceVoiceResultReceived;
    public event Action? ReferenceVoiceCatalogUpdated;
    public event Action? ReferenceVoiceAccessUpdated;
    public event Action<DeleteReferenceVoiceResponse>? ReferenceVoiceDeleteResultReceived;
    public IReadOnlyList<string> ReferenceVoices { get; private set; } = Array.Empty<string>();
    public bool CanCreateReferenceVoice { get; private set; }

    public override void Initialize()
    {
        if (!_contentRootAdded)
        {
            _contentRootAdded = true;
            _res.AddRoot(Prefix, _contentRoot);
        }

        _sawmill = Logger.GetSawmill("tts");
        _cfg.OnValueChanged(CCCVars.TTSVolume, OnTtsVolumeChanged, true);
=======
using Content.Shared.Corvax.TTS;
using Content.Shared.Corvax.CCCVars;
using Robust.Client.Audio;
using Robust.Client.ResourceManagement;
using Robust.Shared.Audio;
using Robust.Shared.Audio.Systems;
using Robust.Shared.ContentPack;
using Robust.Shared.Utility;
using Robust.Shared.Configuration;
using Content.Shared.Chat;
using System.Linq;
using Robust.Shared.Audio.Components;
using Content.Shared.GameTicking;

namespace Content.Client.Corvax.TTS;

// RuCM TTS
public sealed partial class TTSSystem : EntitySystem
{
    [Dependency] private readonly IResourceManager _res = default!;
    [Dependency] private readonly AudioSystem _audio = default!;
    [Dependency] private readonly IConfigurationManager _cfg = default!;

    private static readonly MemoryContentRoot ContentRoot = new();
    private static readonly ResPath Prefix = ResPath.Root / "TTS";

    private static bool _contentRootAdded;
    private int _fileIndex;
    private readonly Dictionary<EntityUid, (AudioStream Stream, bool Whisper)> _playing = new();

    public override void Initialize()
    {
        base.Initialize();

        if (!_contentRootAdded)
        {
            _contentRootAdded = true;
            _res.AddRoot(Prefix, ContentRoot);
        }

>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
        SubscribeNetworkEvent<PlayTTSEvent>(OnPlayTTS);
        SubscribeNetworkEvent<AddReferenceVoiceResponse>(OnReferenceVoiceResult);
        SubscribeNetworkEvent<ReferenceVoiceCatalogResponse>(OnReferenceVoiceCatalog);
        SubscribeNetworkEvent<ReferenceVoiceAccessResponse>(OnReferenceVoiceAccess);
        SubscribeNetworkEvent<DeleteReferenceVoiceResponse>(OnReferenceVoiceDeleteResult);
<<<<<<< HEAD
=======
        SubscribeNetworkEvent<RoundRestartCleanupEvent>(_ => StopTTS());
        _cfg.OnValueChanged(CCCVars.TTSVolume, OnVolumeChanged);
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    }

    public override void Shutdown()
    {
<<<<<<< HEAD
        base.Shutdown();
        _cfg.UnsubValueChanged(CCCVars.TTSVolume, OnTtsVolumeChanged);
=======
        _cfg.UnsubValueChanged(CCCVars.TTSVolume, OnVolumeChanged);
        StopTTS();
        base.Shutdown();
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

<<<<<<< HEAD
        foreach (var (playbackId, tracked) in _trackedPlayback.ToArray())
        {
            if (TryComp<AudioComponent>(tracked.Audio, out var audio) && (!audio.Started || audio.Playing))
                continue;

            _trackedPlayback.Remove(playbackId);
            RaiseNetworkEvent(new TTSPlaybackFinishedEvent(playbackId, tracked.Source, true));
        }
    }

=======
        foreach (var (uid, sound) in _playing.ToArray())
        {
            if (Exists(uid) && HasComp<AudioComponent>(uid))
                continue;

            sound.Stream.Dispose();
            _playing.Remove(uid);
        }
    }

    private void StopTTS()
    {
        foreach (var (uid, sound) in _playing)
        {
            _audio.Stop(uid);
            sound.Stream.Dispose();
        }

        _playing.Clear();

        CleanupRadioEffect();
    }

    private void OnVolumeChanged(float volume)
    {
        if (!float.IsFinite(volume) || volume <= 0)
        {
            StopTTS();
            return;
        }
        foreach (var (uid, sound) in _playing)
            _audio.SetVolume(uid, SharedAudioSystem.GainToVolume(Math.Clamp(volume, 0f, 1f)) - (sound.Whisper ? 6f : 0f));
    }

>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    public void RequestPreviewTTS(string voiceId)
    {
        RaiseNetworkEvent(new RequestPreviewTTSEvent(voiceId));
    }

<<<<<<< HEAD
    public void AddReferenceVoice(string speakerName, byte[] audio)
    {
        RaiseNetworkEvent(new AddReferenceVoiceRequest(speakerName, audio));
    }

    public void RequestReferenceVoiceCatalog()
    {
        RaiseNetworkEvent(new ReferenceVoiceCatalogRequest());
    }

    public void DeleteReferenceVoice(string speakerName)
    {
        RaiseNetworkEvent(new DeleteReferenceVoiceRequest(speakerName));
    }

    private void OnReferenceVoiceResult(AddReferenceVoiceResponse response)
    {
        ReferenceVoiceResultReceived?.Invoke(response);
    }

    private void OnReferenceVoiceCatalog(ReferenceVoiceCatalogResponse response)
    {
        ReferenceVoices = response.SpeakerNames;
        ReferenceVoiceCatalogUpdated?.Invoke();
    }

    private void OnReferenceVoiceAccess(ReferenceVoiceAccessResponse response)
    {
        CanCreateReferenceVoice = response.CanCreate;
        ReferenceVoiceAccessUpdated?.Invoke();
    }

    private void OnReferenceVoiceDeleteResult(DeleteReferenceVoiceResponse response)
    {
        ReferenceVoiceDeleteResultReceived?.Invoke(response);
    }

    private void OnTtsVolumeChanged(float volume)
    {
        _volume = volume;
    }

    private void OnPlayTTS(PlayTTSEvent ev)
    {
        _sawmill.Verbose($"Play TTS audio {ev.Data.Length} bytes from {ev.SourceUid} entity");

        var filePath = new ResPath($"{_fileIdx++}.ogg");
        _contentRoot.AddOrUpdateFile(filePath, ev.Data);

        var audioResource = new AudioResource();
        audioResource.Load(IoCManager.Instance!, Prefix / filePath);

        var audioParams = AudioParams.Default
            .WithVolume(AdjustVolume(ev.IsWhisper))
            .WithMaxDistance(AdjustDistance(ev.IsWhisper));

        var soundSpecifier = new ResolvedPathSpecifier(Prefix / filePath);

        if (ev.IsRadio)
        {
            _audio.PlayGlobal(RadioStaticSound, Filter.Local(), false,
                AudioParams.Default.WithVolume(-8f).WithVariation(0.1f));
            var result = _audio.PlayGlobal(audioResource.AudioStream, soundSpecifier, audioParams);
            if (result.HasValue)
                result.Value.Component.Occlusion = RadioOcclusion;
            TrackPlayback(ev, result);
            _contentRoot.RemoveFile(filePath);
            return;
        }

        if (ev.SourceUid != null)
        {
            if (!TryGetEntity(ev.SourceUid.Value, out _))
            {
                PlaybackFailed(ev);
                _contentRoot.RemoveFile(filePath);
                return;
            }
            var sourceUid = GetEntity(ev.SourceUid.Value);
            TrackPlayback(ev, _audio.PlayEntity(audioResource.AudioStream, sourceUid, soundSpecifier, audioParams));
        }
        else
        {
            TrackPlayback(ev, _audio.PlayGlobal(audioResource.AudioStream, soundSpecifier, audioParams));
        }

        _contentRoot.RemoveFile(filePath);
    }

    private void TrackPlayback(PlayTTSEvent ev, (EntityUid Entity, AudioComponent Component)? playback)
    {
        if (ev.PlaybackId == 0)
            return;

        if (playback is not { } audio)
        {
            PlaybackFailed(ev);
            return;
        }

        _trackedPlayback[ev.PlaybackId] = (audio.Entity, ev.SourceUid);
    }

    private void PlaybackFailed(PlayTTSEvent ev)
    {
        if (ev.PlaybackId != 0)
            RaiseNetworkEvent(new TTSPlaybackFinishedEvent(ev.PlaybackId, ev.SourceUid, false));
    }

    private float AdjustVolume(bool isWhisper)
    {
        var volume = MinimalVolume + SharedAudioSystem.GainToVolume(_volume);

        if (isWhisper)
        {
            volume -= SharedAudioSystem.GainToVolume(WhisperFade);
        }

        return volume;
    }

    private float AdjustDistance(bool isWhisper)
    {
        return isWhisper ? SharedChatSystem.WhisperMuffledRange : SharedChatSystem.VoiceRange;
=======
    private void OnPlayTTS(PlayTTSEvent ev)
    {
        var volume = _cfg.GetCVar(CCCVars.TTSVolume);
        if (!float.IsFinite(volume) || volume <= 0f || ev.Data.Length is < 12 or > 8388608 || _playing.Count >= 32)
            return;

        volume = Math.Clamp(volume, 0f, 1f);
        var filePath = new ResPath($"{_fileIndex++}.wav");

        ContentRoot.AddOrUpdateFile(filePath, ev.Data);
        AudioStream? stream = null;

        try
        {
            var audioResource = new AudioResource();
            audioResource.Load(
                IoCManager.Instance!,
                Prefix / filePath);
            stream = audioResource.AudioStream;

            var soundSpecifier =
                new ResolvedPathSpecifier(Prefix / filePath);

            var audioParams = AudioParams.Default
                .WithVolume(SharedAudioSystem.GainToVolume(volume) - (ev.IsWhisper ? 6f : 0f))
                .WithMaxDistance(ev.IsWhisper ? SharedChatSystem.WhisperMuffledRange : SharedChatSystem.VoiceRange);

            if (ev.SourceUid != null && !ev.IsRadio)
            {
                if (!TryGetEntity(ev.SourceUid.Value, out _))
                    return;

                var source = GetEntity(ev.SourceUid.Value);

                var playback = _audio.PlayEntity(
                    audioResource.AudioStream,
                    source,
                    soundSpecifier,
                    audioParams);

                if (playback is { } played)
                {
                    _playing.Add(played.Entity, (stream, ev.IsWhisper));
                    stream = null;
                }

                return;
            }

            var globalPlayback = _audio.PlayGlobal(
                audioResource.AudioStream,
                soundSpecifier,
                ev.IsRadio
                    ? audioParams
                        .WithPitchScale(0.98f)
                        .WithVariation(0.015f)
                    : audioParams);

            if (globalPlayback is { } global)
            {
                if (ev.IsRadio)
                    ApplyRadioEffect(global);

                _playing.Add(global.Entity, (stream, ev.IsWhisper));
                stream = null;
            }
        }
        catch (Exception e)
        {
            Logger.Warning($"Could not play TTS audio: {e.Message}");
        }
        finally
        {
            stream?.Dispose();
            ContentRoot.RemoveFile(filePath);
        }
>>>>>>> ee5c3f07eab149fc5eabc97c0cc1d76ed75fab34
    }
}
