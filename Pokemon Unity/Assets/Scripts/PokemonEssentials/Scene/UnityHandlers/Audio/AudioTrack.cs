using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonUnity;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	//[RequireComponent(typeof(AudioSource))]	//One Source
	[RequireComponent(typeof(AudioClip))]		//Many Clips
	public class AudioTrack : global::UnityEngine.MonoBehaviour, PokemonEssentials.Interface.IAudioObject
	{
		//public string name { get { return clip.name; } set { clip.name = value; } }
		string IAudioObject.name { get { return gameObject.name; } set { gameObject.name = value; } }
		public int volume { get; set; }
		public float pitch { get; set; }

		public AudioClip clip;
		public int loopStartSamples;
		public int loopEndSamples;
		public int samplesPosition;

		public AudioTrack()
		{
			this.clip = null;
			this.loopStartSamples = 0;
			this.samplesPosition = 0;
		}

		public AudioTrack(AudioClip clip, int loopStartSamples, int? loopEndSamples = null)
		{
			this.clip = clip;
			this.loopStartSamples = loopStartSamples;
			if (loopEndSamples != null) this.loopEndSamples = loopEndSamples.Value;
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
}