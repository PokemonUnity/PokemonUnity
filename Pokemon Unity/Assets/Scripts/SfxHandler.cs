//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class SfxHandler : MonoBehaviour
{
    public static SfxHandler sfxHandler;

    public AudioSource[] sources;

    void Awake()
    {
        if (sfxHandler == null)
        {
            sfxHandler = this;
        }
        else if (sfxHandler != this)
        {
            Destroy(gameObject);
        }

        sources = this.gameObject.GetComponents<AudioSource>();
        foreach(AudioSource source in sources)
        {
            source.loop = false;
        }
    }

    
    public static AudioSource Play(AudioClip clip, float pitch = 1f, float volume = -1f)
    {
        AudioSource source = null;
        for (int i = 0; i < sfxHandler.sources.Length; i++)
        {
            if (!sfxHandler.sources[i].isPlaying)
            {
                source = sfxHandler.sources[i];
                i = sfxHandler.sources.Length;
            }
        }
        if (source == null)
        {
            float mostFinished = 0;
            int mostFinishedIndex = 0;
            //Find the source closest to finishing playing it's clip
            for (int i = 0; i < sfxHandler.sources.Length; i++)
            {
                if (sfxHandler.sources[i].clip != null)
                {
                    if (sfxHandler.sources[i].clip.length != 0)
                    {
                        if ((sfxHandler.sources[i].timeSamples / sfxHandler.sources[i].clip.length) > mostFinished)
                        {
                            mostFinished = sfxHandler.sources[i].timeSamples / sfxHandler.sources[i].clip.length;
                            mostFinishedIndex = i;
                        }
                    }
                }
            }
            //play the new clip on the source with the highest played percentage
            source = sfxHandler.sources[mostFinishedIndex];
        }
        source.clip = clip;
        if (volume < 0)
        {
            source.volume = PlayerPrefs.GetFloat("sfxVolume");
        }
        else
        {
            source.volume = volume*PlayerPrefs.GetFloat("sfxVolume");
        }
        source.pitch = pitch;
        source.Play();

        return source;
    }

    public static void FadeSource(AudioSource source, float time)
    {
        sfxHandler.StartCoroutine(sfxHandler.FadeIE(source, time));
    }

    private IEnumerator FadeIE(AudioSource source, float time)
    {
        float initialVolume = source.volume;
        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1f;
            }

            if (!source.isPlaying)
            {
                increment = 1f;
                Debug.Log("early end");
            }

            source.volume = initialVolume * (1 - increment);
            if (source.isPlaying)
            {
                yield return null;
            }
        }
        source.volume = initialVolume;
        source.Stop();
    }
}