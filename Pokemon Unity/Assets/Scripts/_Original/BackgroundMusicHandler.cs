//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
//using FMOD;
using Debug = UnityEngine.Debug;
using System;
using EasyButtons;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInputConsumer))]
public class BackgroundMusicHandler : MonoBehaviour
{
    #region Variables

    public static BackgroundMusicHandler Singleton;

    [SerializeField] 
    GameSetting<float> backgroundMusicVolume;

    float baseVolume;

    int samplesEndBuffer = 5000;
    float defaultFadeSpeed = 1.8f;

    private AudioSource source;

    public enum Track {
        Main,
        Overlay,
        MFX
    }

    private Track currentTrack;

    private AudioTrack
        mainTrack,
        mainTrackNext,
        overlayTrack,
        mfxTrack;

    private bool loop = true;
    LTDescr fadingTween;

    public AudioClip Clip { get => source.clip; set => source.clip = value; }

    #endregion

    #region MonoBehaviour Functions

    void OnValidate() {
        if (backgroundMusicVolume == null) Debug.LogError("No Background Music Volume setting provided", gameObject);
    }

    void Awake() {
        if (Singleton == null) Singleton = this;
        else {
            Destroy(gameObject);
            return;
        }

        source = GetComponent<AudioSource>();
        source.loop = false;

        updateBaseVolume(backgroundMusicVolume.Get());
        backgroundMusicVolume.OnValueChange.AddListener(updateBaseVolume);
    }

    void Update() {
        //if (!IsFading())
        //    source.volume = baseVolume;

        if (source.clip != null) {
            // Every looping clip contains some samples on the end of the clip that is simply 
            // a buffer for the program to start looping again.
            if (loop) {
                if (source.timeSamples >= source.clip.samples - samplesEndBuffer){
                    //jump back the relative amount so that the least audio possible is skipped
                    int loopStartSamples = (currentTrack == Track.Main)
                        ? mainTrack.LoopStartSamples
                        : overlayTrack.LoopStartSamples;
                    source.timeSamples -= source.clip.samples - samplesEndBuffer - loopStartSamples;
                    source.Play();
                }
            }
        }

        //Debug.Log($"Volume: {source.volume} Clip: {source.clip}");
    }

    #endregion

    #region Debug

    // Was bound to the 'I' key
    public void DebugGetClipAndSample() {
        if (mainTrack.Clip == null)
            Debug.Log("CLIP: null");
        else {
            int test = (source.clip.samples - samplesEndBuffer);
            Debug.Log("CLIP: " + mainTrack.Clip + "\nCurrent Sample:" + source.timeSamples + "\nLoop Sample" + test);
        }
    }

    #endregion

    #region Getter and Setters

    void updateBaseVolume(float newVolume) {
        baseVolume = newVolume;
        source.volume = baseVolume;
    }

    public bool PlayingOverlay() => currentTrack == Track.Overlay;
    
    void updateVolume(float value) => source.volume = value;

    public bool IsFading() => fadingTween != null && LeanTween.isTweening(fadingTween.id);

    #endregion

    #region Basic Actions
      
    [Button]
    public void Play() => Play(currentTrack);

    private void Play(Track trackType) {
        AudioTrack track = mainTrack;

        switch (trackType) {
            case Track.Main:
                mainTrack = mainTrackNext;
                track = mainTrack;
                break;
            case Track.Overlay:
                track = overlayTrack;
                break;
            case Track.MFX:
                track = mfxTrack;
                break;
            default:
                break;
        }

        currentTrack = trackType;

        source.clip = track.Clip;
        source.timeSamples = track.SamplesPosition;
        source.volume = baseVolume;
        Debug.Log("Starting to play: " + source.clip);
        source.Play();
    }

    [Button]
    private void Pause() {
        if (currentTrack == Track.Main)
            mainTrack.SamplesPosition = source.timeSamples;
        else if (currentTrack == Track.Overlay)
            overlayTrack.SamplesPosition = source.timeSamples;

        source.Stop();
    }

    public void Mute(float time) => StartCoroutine(MuteIE(time));

    [Button]
    public void FadeOut(float time) => StartCoroutine(FadeOutIE(time));

    public IEnumerator FadeOutIE(float time) {
        fadingTween = LeanTween.value(gameObject, updateVolume, source.volume, 0f, time);
        yield return new WaitForSeconds(time);
    }

    [Button]
    public void FadeIn(float time) => StartCoroutine(FadeInIE(time));

    public IEnumerator FadeInIE(float time) {
        fadingTween = LeanTween.value(gameObject, updateVolume, source.volume, baseVolume, time);
        yield return new WaitForSeconds(time);
    }

    private void StopMainFade() {
        if (fadingTween != null) LeanTween.cancel(fadingTween.id);
        fadingTween = null;
        PlayMain(AudioTrack.NullTrack);
        mainTrack = mainTrackNext;
    }

    #endregion

    #region Main audio

    public void PlayMain(AudioTrack backgroundMusic) => StartCoroutine(PlayMainIE(backgroundMusic));

    public void PlayMain(AudioClip bgm, int loopStartSamples) {
        // FIXME;
        throw new Exception("Need to refactor this method to use AudioTrack");
        //AudioTrack track = new AudioTrack(bgm, loopStartSamples);
        //PlayMain(track);
    }

    public IEnumerator PlayMainIE(AudioTrack backgroundMusic) {
        loop = true;
        //if main is playing:   Fade out current main track (if any/not fading), THEN play new track
        if (currentTrack == Track.Main)
            if (backgroundMusic != mainTrack) {
                //if current track is not already playing
                mainTrackNext = backgroundMusic;
                //Fade out current main track (if any/not fading), THEN play new track
                if (mainTrack == null)
                    Play(Track.Main);
                else if (!IsFading()) {
                    yield return StartCoroutine(FadeOutIE(defaultFadeSpeed));
                    Play(Track.Main);
                }
            } else
                //but if it is, set the next track to main, in case of fading
                mainTrackNext = backgroundMusic;
        else
            //if overlay/MFX is playing:   Set main track to the new track
            mainTrack = backgroundMusic;
    }

    public void ResumeMain(float time = -1f, AudioClip clip = null, int loopStartSamples = 0) {
        // FIXME;
        throw new Exception("Need to refactor this method to use AudioTrack");
        //ResumeMain(time == -1f ? defaultFadeSpeed : time, new AudioTrack(clip, loopStartSamples));
    }

    public void ForceResumeMain(float time = -1f, AudioClip clip = null, int loopStartSamples = 0) {
        // FIXME;
        throw new Exception("Need to refactor this method to use AudioTrack");
        //ResumeMain(time == -1f ? defaultFadeSpeed : time, new AudioTrack(clip, loopStartSamples), true);
    }

    public void ResumeMain(float time, AudioTrack track, bool forceResume = false) {
        StartCoroutine(ResumeMainIE(time, track, forceResume));
    }

    private IEnumerator ResumeMainIE(float time, AudioTrack track, bool forceResume = false) {
        loop = true;
        //if overlay is playing, fade out and play main
        if (forceResume || currentTrack == Track.Overlay) {
            Debug.Log("Overlay when resume");
            mainTrackNext = track;
            //Fade out current track, THEN resume main track
            Debug.Log("fading variable null: " + (!IsFading()));
            if (!IsFading()) {
                Debug.Log("Fade Coroutine");
                yield return StartCoroutine(FadeOutIE(time));
                Debug.Log("Playing: " + track.ToString());
            }
        } else
            Debug.Log("Not Overlay when resume");
    }

    #endregion

    #region Overlay Music
    
    public void PlayOverlay(AudioTrack audioTrack, float fadeTime = 0.1f) {
        StartCoroutine(PlayOverlayIE(audioTrack.Clip, audioTrack.LoopStartSamples, audioTrack.LoopStartSamples, fadeTime));
    }

    public void PlayOverlay(AudioClip bgm, int loopStartSamples, float fadeTime = 0.1f) {
        StartCoroutine(PlayOverlayIE(bgm, loopStartSamples, loopStartSamples, fadeTime));
    }

    public IEnumerator PlayOverlayIE(AudioClip bgm, int loopStartSamples, int startSample, float fadeTime = 0.1f) {
        //if overlay track is already playing the bgm, do not continue
        if (overlayTrack.Clip == bgm && currentTrack == Track.Overlay) 
            yield break;

        loop = true;
        //if main is playing: fade at given time then Pause main track
        if (currentTrack == Track.Main) {
            //if main is fading: stop the fading
            if (fadingTween != null)
                StopMainFade();
            else {
                yield return StartCoroutine(FadeOutIE(fadeTime));
                Pause();
            }
        } else if (currentTrack == Track.Overlay)
            //if Overlay is playing, fade at given time
            yield return StartCoroutine(FadeOutIE(fadeTime));

        //if MFX is playing:   ONLY set overlay track to new track, don't play
        // FIXME;
        throw new Exception("Need to refactor this method to use AudioTrack");
        //overlayTrack = new AudioTrack(bgm, loopStartSamples);
        if (startSample > 0)
            overlayTrack.SamplesPosition = startSample;

        if (currentTrack != Track.MFX)
            Play(Track.Overlay);
    }

    #endregion

    #region Music Effects

    /// If you need to play a second MFX immediately after the other, use Play MFX Consecutive
    public void PlayMFX(AudioClip mfx) {
        StartCoroutine(PlayMfxIE(mfx));
    }

    /// Plays an MFX with a slight delay to prevent musical glitches
    public void PlayMFXConsecutive(AudioClip mfx) {
        StartCoroutine(PlayMfxConsecutiveIE(mfx));
    }

    private IEnumerator PlayMfxConsecutiveIE(AudioClip mfx) {
        yield return StartCoroutine(MuteIE(0.2f));
        StartCoroutine(PlayMfxIE(mfx));
    }

    private IEnumerator PlayMfxIE(AudioClip mfx) {
        if (fadingTween != null) {
            LeanTween.cancel(fadingTween.id);
            fadingTween = null;
        }
        Track previousTrackType = currentTrack;
        Pause();

        loop = false;
        // FIXME;
        throw new Exception("Need to refactor this method to use AudioTrack");
        //mfxTrack = new AudioTrack(mfx, 0);
        Play(Track.MFX);
        while (source.isPlaying)
            yield return null;

        loop = true;
        Play(previousTrackType);
    }

    #endregion

    #region Other

    private IEnumerator MuteIE(float time) {
        source.mute = true;
        yield return new WaitForSeconds(time);
        source.mute = false;
    }

    public int GetSample() {
        Debug.Log("[BgmHandler] Time samples : "+source.timeSamples);
        return source.timeSamples;
    }
    
    #endregion
}

