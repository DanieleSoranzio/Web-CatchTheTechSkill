using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : Singleton<AudioManager>
{
    // ── Inspector ────────────────────────────────────────────────────────────

    [Header("Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Exposed Mixer Parameters")]
    [SerializeField] private string masterVolumeParam  = "MasterVolume";
    [SerializeField] private string musicVolumeParam   = "MusicVolume";
    [SerializeField] private string sfxVolumeParam     = "SFXVolume";

    [Header("SFX Pool")]
    [SerializeField] private int sfxPoolSize = 16;

    [Header("Music")]
    [SerializeField] private float defaultCrossfadeDuration = 1f;

    // ── State ────────────────────────────────────────────────────────────────

   
    private Queue<AudioSource> _sfxPool;
    private List<AudioSource>  _activeSfx;

   
    private AudioSource _musicSourceA;
    private AudioSource _musicSourceB;
    private bool        _musicOnA = true;

    private AudioSource CurrentMusic  => _musicOnA ? _musicSourceA : _musicSourceB;
    private AudioSource InactiveMusic => _musicOnA ? _musicSourceB : _musicSourceA;

   
    private float _masterVolume = 1f;
    private float _musicVolume  = 1f;
    private float _sfxVolume    = 1f;

    private bool _isMuted = false;

    // ── Unity ────────────────────────────────────────────────────────────────

    protected override void Awake()
    {
        base.Awake();
        InitPool();
        InitMusicSources();
    }

    // ── Init ─────────────────────────────────────────────────────────────────

    private void InitPool()
    {
        _sfxPool   = new Queue<AudioSource>();
        _activeSfx = new List<AudioSource>();

        var poolRoot = new GameObject("SFX_Pool");
        poolRoot.transform.SetParent(transform);

        for (int i = 0; i < sfxPoolSize; i++)
        {
            var source = CreateAudioSource($"SFX_{i}", poolRoot.transform);
            _sfxPool.Enqueue(source);
        }
    }

    private void InitMusicSources()
    {
        _musicSourceA = CreateAudioSource("Music_A", transform);
        _musicSourceB = CreateAudioSource("Music_B", transform);
        _musicSourceA.loop = true;
        _musicSourceB.loop = true;
    }

    private AudioSource CreateAudioSource(string sourceName, Transform parent)
    {
        var go     = new GameObject(sourceName);
        go.transform.SetParent(parent);
        var source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        if (audioMixer != null)
            source.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Master")?[0];
        return source;
    }

    // ── SFX ──────────────────────────────────────────────────────────────────

   
    public AudioSource PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) { return null; }

        var source = GetPooledSource();
        source.clip   = clip;
        source.volume = volume * _sfxVolume * (_isMuted ? 0f : 1f);
        source.pitch  = pitch;
        source.loop   = false;
        source.Play();

        _activeSfx.Add(source);
        StartCoroutine(ReturnWhenDone(source, clip.length / Mathf.Abs(pitch)));
        return source;
    }

 
    public void PlaySFXAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        AudioSource.PlayClipAtPoint(clip, position, volume * _sfxVolume * (_isMuted ? 0f : 1f));
    }


    public AudioSource PlaySFXRandomPitch(AudioClip clip, float volume = 1f, float minPitch = 0.9f, float maxPitch = 1.1f)
    {
        return PlaySFX(clip, volume, Random.Range(minPitch, maxPitch));
    }

 
    public AudioSource PlayRandomSFX(AudioClip[] clips, float volume = 1f, float pitch = 1f)
    {
        if (clips == null || clips.Length == 0) return null;
        return PlaySFX(clips[Random.Range(0, clips.Length)], volume, pitch);
    }


    public void StopAllSFX()
    {
        foreach (var source in _activeSfx)
        {
            source.Stop();
            ReturnToPool(source);
        }
        _activeSfx.Clear();
    }

    // ── Music ─────────────────────────────────────────────────────────────────
    
    public void PlayMusic(AudioClip clip, float volume = 1f, bool crossfade = true, float crossfadeDuration = -1f)
    {
        if (clip == null) { Debug.LogWarning("[AudioManager] PlayMusic: clip è null."); return; }
        if (crossfadeDuration < 0f) crossfadeDuration = defaultCrossfadeDuration;

        if (crossfade && CurrentMusic.isPlaying)
            StartCoroutine(CrossfadeMusic(clip, volume, crossfadeDuration));
        else
        {
            CurrentMusic.clip   = clip;
            CurrentMusic.volume = volume * _musicVolume * (_isMuted ? 0f : 1f);
            CurrentMusic.Play();
        }
    }
    
    public void StopMusic(float fadeDuration = 0f)
    {
        if (fadeDuration > 0f)
            StartCoroutine(FadeOut(CurrentMusic, fadeDuration));
        else
            CurrentMusic.Stop();
    }

    /// <summary>Pausa / riprende la musica.</summary>
    public void PauseMusic()  => CurrentMusic.Pause();
    public void ResumeMusic() => CurrentMusic.UnPause();

    // ── Volume ────────────────────────────────────────────────────────────────

    public void SetMasterVolume(float volume)
    {
        _masterVolume = Mathf.Clamp01(volume);
        if (audioMixer != null)
            audioMixer.SetFloat(masterVolumeParam, LinearToDecibel(_masterVolume));
        else
            AudioListener.volume = _masterVolume;
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        if (audioMixer != null)
            audioMixer.SetFloat(musicVolumeParam, LinearToDecibel(_musicVolume));
        else
        {
            _musicSourceA.volume = _musicVolume;
            _musicSourceB.volume = _musicVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        if (audioMixer != null)
            audioMixer.SetFloat(sfxVolumeParam, LinearToDecibel(_sfxVolume));
    }

    public float GetMasterVolume() => _masterVolume;
    public float GetMusicVolume()  => _musicVolume;
    public float GetSFXVolume()    => _sfxVolume;

    // ── Mute ─────────────────────────────────────────────────────────────────

    public void Mute(bool mute)
    {
        _isMuted = mute;
        AudioListener.volume = mute ? 0f : _masterVolume;
    }

    public void ToggleMute() => Mute(!_isMuted);
    public bool IsMuted      => _isMuted;

    // ── Pitch / Effects ───────────────────────────────────────────────────────

    /// <summary>Rallenta tutta l'audio — utile per slow motion.</summary>
    public void SetGlobalPitch(float pitch)
    {
        AudioListener.pause = false;
        _musicSourceA.pitch = pitch;
        _musicSourceB.pitch = pitch;
        foreach (var s in _activeSfx) s.pitch = pitch;
    }

    // ── Internal helpers ──────────────────────────────────────────────────────

    private AudioSource GetPooledSource()
    {
        if (_sfxPool.Count > 0)
            return _sfxPool.Dequeue();
        
        var poolRoot = transform.Find("SFX_Pool") ?? transform;
        for (int i = 0; i < 4; i++)
            _sfxPool.Enqueue(CreateAudioSource($"SFX_extra_{i}", poolRoot));

        return _sfxPool.Dequeue();
    }

    private void ReturnToPool(AudioSource source)
    {
        source.clip  = null;
        source.pitch = 1f;
        _sfxPool.Enqueue(source);
    }

    private IEnumerator ReturnWhenDone(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (source.isPlaying) source.Stop();
        _activeSfx.Remove(source);
        ReturnToPool(source);
    }

    private IEnumerator CrossfadeMusic(AudioClip clip, float targetVolume, float duration)
    {
        var incoming = InactiveMusic;
        var outgoing = CurrentMusic;

        incoming.clip   = clip;
        incoming.volume = 0f;
        incoming.Play();
        _musicOnA = !_musicOnA;

        float elapsed = 0f;
        float startVolume = outgoing.volume;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            outgoing.volume = Mathf.Lerp(startVolume, 0f, t);
            incoming.volume = Mathf.Lerp(0f, targetVolume * _musicVolume, t);
            yield return null;
        }

        outgoing.Stop();
        outgoing.clip   = null;
        incoming.volume = targetVolume * _musicVolume;
    }

    private IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float elapsed     = 0f;

        while (elapsed < duration)
        {
            elapsed        += Time.deltaTime;
            source.volume   = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume;
    }

    private static float LinearToDecibel(float linear)
    {
        return linear > 0.0001f ? 20f * Mathf.Log10(linear) : -80f;
    }
}