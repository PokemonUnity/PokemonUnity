//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
//using FMOD;
using Debug = UnityEngine.Debug;

public class BgmHandler : MonoBehaviour
{
    public static BgmHandler main;

    private float baseVolume;

    private int samplesEndBuffer = 5000;
    private float defaultFadeSpeed = 1.8f;

    private AudioSource source;

    public enum Track
    {
        Main,
        Overlay,
        MFX
    }

    private Track currentTrack;

    private AudioTrack
        mainTrack = new AudioTrack(),
        mainTrackNext = new AudioTrack(),
        overlayTrack = new AudioTrack(),
        mfxTrack = new AudioTrack();

    private bool loop = true;
    private Coroutine fading;


    void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else if (main != this)
        {
            Destroy(gameObject);
        }

        source = this.GetComponent<AudioSource>();
        source.loop = false;
    }

    void Update()
    {
        baseVolume = PlayerPrefs.GetFloat("musicVolume");
        if (fading == null)
        {
            source.volume = baseVolume;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (mainTrack.clip == null)
            {
                Debug.Log("CLIP: null");
            }
            else
            {
                int test = (source.clip.samples - samplesEndBuffer);
                Debug.Log("CLIP: " + mainTrack.clip + "\nCurrent Sample:" + source.timeSamples  + "\nLoop Sample" + test);
            }
        }

        if (source.clip != null)
        {
            //Every looping clip contains some samples on the end of the clip that is simply 
            //a buffer for the program to start looping again.
            if (loop)
            {
                if (source.timeSamples >= source.clip.samples - samplesEndBuffer)
                {
                    //jump back the relative amount so that the least audio possible is skipped
                    int loopStartSamples = (currentTrack == Track.Main)
                        ? mainTrack.loopStartSamples
                        : overlayTrack.loopStartSamples;
                    source.timeSamples -= source.clip.samples - samplesEndBuffer - loopStartSamples;
                    source.Play();
                }
            }
        }
    }


    private IEnumerator Fade(float time)
    {
        Debug.Log("Starting Fade");
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }

            //by reducing it by increment*1.2f, it will leave a silent gap on the end before the next track begins.
            source.volume = (1f - increment * 1.2f) * baseVolume;

            yield return null;
        }
        fading = null;
        Debug.Log("End Fade");
    }

    private void Play(Track trackType)
    {
        AudioTrack track = mainTrack;
        if (trackType == Track.Main)
        {
            mainTrack = mainTrackNext;
            track = mainTrack;
        }
        else if (trackType == Track.Overlay)
        {
            track = overlayTrack;
        }
        else if (trackType == Track.MFX)
        {
            track = mfxTrack;
        }

        currentTrack = trackType;

        source.clip = track.clip;
        source.timeSamples = track.samplesPosition;
        source.volume = baseVolume;
        Debug.Log("Starting to play: "+source.clip);
        source.Play();
    }

    private void Pause()
    {
        if (currentTrack == Track.Main)
        {
            mainTrack.samplesPosition = source.timeSamples;
        }
        else if (currentTrack == Track.Overlay)
        {
            overlayTrack.samplesPosition = source.timeSamples;
        }
        source.Stop();
    }

    public void PlayMain(AudioClip bgm, int loopStartSamples)
    {
        loop = true;
        //if main is playing:   Fade out current main track (if any/not fading), THEN play new track
        if (currentTrack == Track.Main)
        {
            //if current track is not already playing
            if (bgm != mainTrack.clip)
            {
                StartCoroutine(PlayMainIE(bgm, loopStartSamples));
            }
            else
            {
                //but if it is, set the next track to main, in case of fading
                mainTrackNext = new AudioTrack(bgm, loopStartSamples);
            }
        }
        //if overlay/MFX is playing:   Set main track to the new track
        else
        {
            mainTrack = new AudioTrack(bgm, loopStartSamples);
        }
    }

    private IEnumerator PlayMainIE(AudioClip bgm, int loopStartSamples)
    {
        mainTrackNext = new AudioTrack(bgm, loopStartSamples);
        //Fade out current main track (if any/not fading), THEN play new track
        if (mainTrack.clip == null)
        {
            Play(Track.Main);
        }
        else if (fading == null)
        {
            yield return fading = StartCoroutine(Fade(defaultFadeSpeed));
            Play(Track.Main);
        }
    }

    private void StopMainFade()
    {
        StopCoroutine(fading);
        fading = null;
        StopCoroutine(PlayMainIE(null, 0));
        mainTrack = mainTrackNext;
    }

    public bool PlayingOverlay()
    {
        return currentTrack == Track.Overlay;
    }

    public void PlayOverlay(AudioClip bgm, int loopStartSamples, float fadeTime = 0.1f)
    {
        //if overlay track is already playing the bgm, do not continue
        if (overlayTrack.clip != bgm || currentTrack != Track.Overlay)
        {
            StartCoroutine(PlayOverlayIE(bgm, loopStartSamples, fadeTime));
        }
    }
    
    public void PlayOverlay(AudioClip bgm, int loopStartSamples, int startSample, float fadeTime = 0.1f)
    {
        //if overlay track is already playing the bgm, do not continue
        if (overlayTrack.clip != bgm || currentTrack != Track.Overlay)
        {
            StartCoroutine(PlayOverlayIE(bgm, loopStartSamples, fadeTime, startSample));
        }
    }

    private IEnumerator PlayOverlayIE(AudioClip bgm, int loopStartSamples, float fadeTime, int startSample = 0)
    {
        loop = true;
        //if main is playing: fade at given time then Pause main track
        if (currentTrack == Track.Main)
        {
            //if main is fading: stop the fading
            if (fading != null)
            {
                StopMainFade();
            }
            else
            {
                yield return fading = StartCoroutine(Fade(fadeTime));
                Pause();
            }
        }
        //if Overlay is playing, fade at given time
        else if (currentTrack == Track.Overlay)
        {
            yield return fading = StartCoroutine(Fade(fadeTime));
        }
        //if MFX is playing:   ONLY set overlay track to new track, don't play
        overlayTrack = new AudioTrack(bgm, loopStartSamples);
        if (startSample > 0)
        {
            overlayTrack.samplesPosition = startSample;
        }
        if (currentTrack != Track.MFX)
        {
            Play(Track.Overlay);
        }
    }

    /// If you need to play a second MFX immediately after the other, use Play MFX Consecutive
    public void PlayMFX(AudioClip mfx)
    {
        StartCoroutine(PlayMfxIE(mfx));
    }

    /// Plays an MFX with a slight delay to prevent musical glitches
    public void PlayMFXConsecutive(AudioClip mfx)
    {
        StartCoroutine(PlayMfxConsecutiveIE(mfx));
    }

    private IEnumerator PlayMfxConsecutiveIE(AudioClip mfx)
    {
        yield return StartCoroutine(MuteIE(0.2f));
        StartCoroutine(PlayMfxIE(mfx));
    }

    private IEnumerator PlayMfxIE(AudioClip mfx)
    {
        if (fading != null)
        {
            StopCoroutine(fading);
            fading = null;
        }
        Track previousTrackType = currentTrack;
        Pause();

        loop = false;
        mfxTrack = new AudioTrack(mfx, 0);
        Play(Track.MFX);
        while (source.isPlaying)
        {
            yield return null;
        }

        loop = true;
        Play(previousTrackType);
    }



    public void ResumeMain(float time = -1f, AudioClip clip = null, int loopStartSamples = 0)
    {
        ResumeMain(time == -1f ? defaultFadeSpeed : time, new AudioTrack(clip, loopStartSamples));
    }
    
    public void ForceResumeMain(float time = -1f, AudioClip clip = null, int loopStartSamples = 0)
    {
        ResumeMain(time == -1f ? defaultFadeSpeed : time, new AudioTrack(clip, loopStartSamples), true);
    }

    private void ResumeMain(float time, AudioTrack track, bool forceResume = false)
    {
        loop = true;
        //if overlay is playing, fade out and play main
        if (forceResume || currentTrack == Track.Overlay)
        {
            Debug.Log("Overlay when resume");
            StartCoroutine(ResumeMainIE(track, time));
        }
        else
        {
            Debug.Log("Not Overlay when resume");
        }
    }

    private IEnumerator ResumeMainIE(AudioTrack resumedTrack, float time)
    {
        mainTrackNext = resumedTrack;
        //Fade out current track, THEN resume main track
        Debug.Log("fading variable null: "+ (fading == null));
        if (fading == null)
        {
            Debug.Log("Fade Coroutine");
            yield return fading = StartCoroutine(Fade(time));
            Debug.Log("Playing: "+resumedTrack.ToString());
            Play(Track.Main);
        }
    }

    public void Mute(float time)
    {
        StartCoroutine(MuteIE(time));
    }

    private IEnumerator MuteIE(float time)
    {
        source.mute = true;
        yield return new WaitForSeconds(time);
        source.mute = false;
    }

    public int GetSample()
    {
        Debug.Log("[BgmHandler] Time samples : "+source.timeSamples);
        return source.timeSamples;
    }
}

public class AudioTrack : PokemonEssentials.Interface.IAudioObject
{
	public string name { get; set; }
	public int volume { get; set; }
	public float pitch { get; set; }

    public AudioClip clip;
    public int loopStartSamples;
    public int samplesPosition;

    public AudioTrack()
    {
        this.clip = null;
        this.loopStartSamples = 0;
        this.samplesPosition = 0;
    }

    public AudioTrack(AudioClip clip, int loopStartSamples)
    {
        this.clip = clip;
        this.loopStartSamples = loopStartSamples;
        this.samplesPosition = 0;
    }

    public PokemonEssentials.Interface.IAudioObject initialize(string name, float volume = 100, float pitch = 100)
	{
        this.name = name;
        this.volume = (int)volume;
        this.pitch = pitch;
        return this;
	}
}