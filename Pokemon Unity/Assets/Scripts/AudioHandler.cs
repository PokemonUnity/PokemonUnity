//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour
{
    //FOREWORD ON ALL MUSIC FOR THIS PROJECT.

    /*	Every track for this project is required to have a LoopStart point (defined in samples)		*/
    /*	that designates where the track will jump back to when it reaches the end.					*/
    /*	to designate the end whilst keeping the audio stream consistantly playing, every track		*/
    /*	is required to have 5,000 samples (beginning at the LoopStart point) added to the end		*/
    /*	of the track to act as a buffer zone.														*/

    //TL;DR: Every track requires...
    /*	- LoopStart point in samples																*/
    /*	- 5,000 samples of the loop beginning tacked on the end										*/

    public bool isBgmHandler = false;

    public AudioSource source;

    public static AudioHandler bgmHandler;

    private AudioClip nextClip;
    private int currentLoopStartTimeSamples = 0;
    private int nextLoopStartTimeSamples = 0;

    private AudioClip pausedClip;
    private int pausedTimeSamples = 0;

    private bool fading = false;
    private float stdFadeSpeed = 2.4f; //time taken to fade from 100% to 0% volume
    private float fadeSpeed;

    private float increment = 1;

    private bool loop = true;

    void Awake()
    {
        if (isBgmHandler)
        {
            if (bgmHandler == null)
            {
                bgmHandler = this;
            }
            else if (bgmHandler != this)
            {
                Destroy(gameObject);
            }
        }

        source = transform.GetComponent<AudioSource>();
        fadeSpeed = stdFadeSpeed;
    }


    void Start()
    {
        source.volume = PlayerPrefs.GetFloat("musicVolume");
    }

    void Update()
    {
        if (increment < 1 && fading)
        {
            increment += (1 / fadeSpeed) * Time.deltaTime; //volume is measured 0-1
            source.volume = (1f - increment * 1.2f) * PlayerPrefs.GetFloat("musicVolume");
                //by reducing it by increment*1.2, it will leave  
        } //a silent gap on the end before the next track begins.
        else if (increment >= 1 && fading)
        {
            if (source.isPlaying)
            {
                //don't start new song until unpaused.
                source.Stop();
                source.volume = PlayerPrefs.GetFloat("musicVolume");
                source.clip = nextClip;
                currentLoopStartTimeSamples = nextLoopStartTimeSamples;
                if (source.clip != null)
                {
                    source.timeSamples = pausedTimeSamples;
                    pausedTimeSamples = 0;
                }
                source.Play();
                fading = false;
            }
        }
        else
        {
            source.volume = PlayerPrefs.GetFloat("musicVolume");
        }

        if (source.clip != null)
        {
            //Every looping clip contains 5000 samples on the end of the clip that is simply 
            //a buffer for the program to start looping again.
            if (loop)
            {
                if (source.timeSamples >= source.clip.samples - 5000)
                {
                    //jump back the relative amount so that the least audio possible is skipped
                    source.timeSamples -= source.clip.samples - 5000 - currentLoopStartTimeSamples;
                    source.Play();
                }
            }
        }
    }

    private IEnumerator Fade(float fadeTime)
    {
        float increment = 0;
        if (increment < 1)
        {
            increment += (1 / fadeTime) * Time.deltaTime; //volume is measured 0-1
            if (increment > 1)
            {
                increment = 1;
            }
            source.volume = (1f - increment * 1.2f) * PlayerPrefs.GetFloat("musicVolume");
            yield return null;
        }
    }

    public void playBGM(AudioClip BGM, int loopStartTimeSamples = 0)
    {
        nextClip = BGM; //set up the nextClip to play
        fadeSpeed = stdFadeSpeed;
        if (source.clip == null)
        {
            source.volume = PlayerPrefs.GetFloat("musicVolume");
            source.clip = nextClip;
            currentLoopStartTimeSamples = loopStartTimeSamples;
            source.timeSamples = 0;
            source.Play();
        }
        else if (source.clip != BGM)
        {
            //if BGM is not already playing
            if (!fading)
            {
                //if fading is off, then begin to fade
                increment = 0;
                fading = true;
            }
            nextLoopStartTimeSamples = loopStartTimeSamples;
        }
    }

    public void playBGM(AudioClip BGM, float sentFadeSpeed, int loopStartTimeSamples = 0)
    {
        nextClip = BGM; //set up the nextClip to play
        fadeSpeed = sentFadeSpeed;
        if (!fading)
        {
            //if fading is off, then begin to fade
            increment = 0;
            fading = true;
            nextLoopStartTimeSamples = loopStartTimeSamples;
        }
    }

    public void fadeBGM(float sentFadeSpeed)
    {
        fadeSpeed = sentFadeSpeed;
        if (!fading)
        {
            increment = 0;
            fading = true;
            nextClip = null;
        }
    }

    public void pauseBGM(float sentFadeSpeed = 0.5f)
    {
        pausedClip = source.clip;
        pausedTimeSamples = source.timeSamples;
        fadeBGM(sentFadeSpeed);
    }

    public void unpauseFadeBGM(float sentFadeSpeed = 0.5f)
    {
        nextClip = pausedClip;
        fadeSpeed = sentFadeSpeed;
        if (!fading)
        {
            increment = 0;
            fading = true;
        }
    }

    public void emptyPausedClip()
    {
        pausedClip = null;
        pausedTimeSamples = 0;
    }

    public void playMFX(AudioClip mfx)
    {
        StartCoroutine(playMFXcr(mfx));
    }

    private IEnumerator playMFXcr(AudioClip mfx)
    {
        AudioClip originalClip = source.clip;
        int originalSamples = source.timeSamples;
        fading = true;
        yield return StartCoroutine(Fade(0.2f));
        source.volume = PlayerPrefs.GetFloat("musicVolume");

        source.timeSamples = 0;
        source.clip = mfx;

        source.Play();
        loop = false;

        while (source.isPlaying)
        {
            yield return null;
        }

        source.Stop();

        source.timeSamples = originalSamples;
        source.clip = originalClip;
        loop = true;
        source.Play();
    }
}