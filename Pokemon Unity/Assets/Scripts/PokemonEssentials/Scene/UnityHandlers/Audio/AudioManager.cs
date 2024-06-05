using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonUnity;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// UnityEngine AudioHandler Logic
	/// </summary>
	public class AudioManager : MonoBehaviour, IAudio
	{
		public static IAudio AudioHandler;
		public int bgm_position
		{
			get
			{
				int ret = 0; //Kernel.Audio_bgm_get_position;
				//ret = bgmSource.timeSamples; //unity using timeSamples to return position data of audio stream
				ret = (int)(bgmSource.time * 1000);
				return ret;
			}
			set { bgmSource.time = value / 1000f; }
		}
		public int volume { get; set; }
		public float pitch { get; set; }
		private AudioSource bgmSource;
		private AudioSource bgsSource;
		private AudioSource meSource;
		private int loopStartSamples;
		private int loopEndSamples;
		private bool isLooping = false;
		private List<AudioSource> seSources = new List<AudioSource>();

		private void Awake()
		{
			// Create different AudioSource components for different types of audio
			bgmSource = gameObject.AddComponent<AudioSource>();
			bgsSource = gameObject.AddComponent<AudioSource>();
			meSource = gameObject.AddComponent<AudioSource>();
		}

		void IAudio.bgm_play(string filename, float volume, float pitch) { bgm_play(filename, volume, pitch); }
		public void bgm_play(string filename, float volume, float pitch, int position = 0)
		{
			PlayAudio(bgmSource, filename, volume, pitch, true, position);
		}

		public void bgm_stop()
		{
			bgmSource.Stop();
		}

		public void bgm_fade(float time)
		{
			StartCoroutine(FadeOut(bgmSource, time));
		}

		public void bgs_play(string filename, float volume, float pitch)
		{
			PlayAudio(bgsSource, filename, volume, pitch, true);
		}

		public void bgs_stop()
		{
			bgsSource.Stop();
		}

		public void bgs_fade(float time)
		{
			StartCoroutine(FadeOut(bgsSource, time));
		}

		public void me_play(string filename, float volume, float pitch)
		{
			PlayAudio(meSource, filename, volume, pitch, false);
		}

		public void me_stop()
		{
			meSource.Stop();
		}

		public void me_fade(float time)
		{
			StartCoroutine(FadeOut(meSource, time));
		}

		public void se_play(string filename, float volume, float pitch)
		{
			AudioSource seSource = new AudioSource();
			seSources.Add(seSource);
			PlayAudio(seSource, filename, volume, pitch, false);
		}

		public void se_stop()
		{
			foreach (var source in seSources)
			{
				source.Stop();
			}
			seSources.Clear();
		}

		private void PlayAudio(AudioSource source, string filename, float volume, float pitch, bool loop, int position = 0)
		{
			AudioClip clip = Resources.Load<AudioClip>(filename);
			if (clip == null)
			{
				GameDebug.LogError("Audio file not found: " + filename);
				return;
			}

			source.clip = clip;
			source.volume = volume;
			source.pitch = pitch;
			source.loop = loop;
			source.timeSamples = position;
			//isLooping = loop;
			source.Play();
			StopCoroutine("CheckAudioLoop");
			StartCoroutine(CheckAudioLoop());
		}

		private IEnumerator FadeOut(AudioSource source, float fadeTime)
		{
			float startVolume = source.volume;

			while (source.volume > 0)
			{
				source.volume -= startVolume * Time.deltaTime / fadeTime;
				yield return null;
			}

			source.Stop();
			source.volume = startVolume;
		}

		private IEnumerator CheckAudioLoop()
		{
			while (bgmSource.isPlaying)
			{
				if (!isLooping && bgmSource.timeSamples >= loopEndSamples)
				{
					bgmSource.timeSamples = loopStartSamples;
					isLooping = true; // Begin looping
				}
				else if (isLooping && bgmSource.timeSamples < loopStartSamples)
				{
					// If the audio source somehow goes before the loop start, reset to loop start
					bgmSource.timeSamples = loopStartSamples;
				}
				yield return null;
			}
		}
	}
}