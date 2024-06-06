using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonUnity;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	//[RequireComponent(typeof(AudioSource))]	//One Source
	[RequireComponent(typeof(AudioClip))]		//Many Clips
	public class AudioTrack : //global::UnityEngine.MonoBehaviour,
		PokemonEssentials.Interface.IAudioObject,
		PokemonEssentials.Interface.IAudioBGM,
		PokemonEssentials.Interface.IAudioBGS,
		PokemonEssentials.Interface.IAudioSE,
		PokemonEssentials.Interface.IAudioME
	{
		public string name { get { return clip.name; } set { clip.name = value; } }
		//string IAudioObject.name { get { return gameObject.name; } set { gameObject.name = value; } }
		public int volume { get; set; }
		public float pitch { get; set; }

		public AudioClip clip;
		//public AudioSource source;
		public int loopStartSamples;
		public int loopEndSamples;
		public int samplesPosition;
		private AudioType type;

		public AudioTrack()
		{
			this.clip = null;
			this.loopStartSamples = 0;
			this.samplesPosition = 0;
		}

		public AudioTrack(AudioClip clip, int loopStartSamples = 0, int? loopEndSamples = null)
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

		#region Explicit Interface
		IAudioBGM IAudioBGM.last()
		{
			if ((AudioManager.AudioHandler as AudioManager).bgmSource.isPlaying)
			{
				IAudioObject audio = new AudioTrack((AudioManager.AudioHandler as AudioManager).bgmSource.clip)
					.initialize((AudioManager.AudioHandler as AudioManager).bgmSource.clip.name,
						(AudioManager.AudioHandler as AudioManager).bgmSource.volume,
						(AudioManager.AudioHandler as AudioManager).bgmSource.pitch);
				return audio as IAudioBGM;
			}
			return null;
		}

		void IAudioBGM.stop()
		{
			AudioManager.AudioHandler.bgm_stop();
		}

		void IAudioBGM.fade(int time)
		{
			AudioManager.AudioHandler.bgm_fade(time);
		}

		void IAudioBGM.play()
		{
			type = AudioType.BGM;
			if (clip != null)
				(AudioManager.AudioHandler as AudioManager).bgm_play(this);
		}

		IAudioBGS IAudioBGS.last()
		{
			if ((AudioManager.AudioHandler as AudioManager).bgsSource.isPlaying)
			{
				IAudioObject audio = new AudioTrack((AudioManager.AudioHandler as AudioManager).bgsSource.clip)
					.initialize((AudioManager.AudioHandler as AudioManager).bgsSource.clip.name,
						(AudioManager.AudioHandler as AudioManager).bgsSource.volume,
						(AudioManager.AudioHandler as AudioManager).bgsSource.pitch);
				return audio as IAudioBGS;
			}
			return null;
		}

		void IAudioBGS.stop()
		{
			AudioManager.AudioHandler.bgs_stop();
		}

		void IAudioBGS.fade(int time)
		{
			AudioManager.AudioHandler.bgs_fade(time);
		}

		void IAudioBGS.play()
		{
			type = AudioType.BGS;
			if (clip != null)
				(AudioManager.AudioHandler as AudioManager).bgs_play(this);
		}

		void IAudioSE.stop()
		{
			//AudioManager.AudioHandler.se_stop();
			(AudioManager.AudioHandler as AudioManager).seSources
				.Find(s => (s.clip & s.isPlaying))
				.Stop();//.Remove(this);
		}

		void IAudioSE.play()
		{
			type = AudioType.SE;
			if (clip != null)
				(AudioManager.AudioHandler as AudioManager).se_play(this);
		}

		void IAudioME.stop()
		{
			AudioManager.AudioHandler.me_stop();
		}

		void IAudioME.fade(int time)
		{
			AudioManager.AudioHandler.me_fade(time);
		}

		void IAudioME.play()
		{
			type = AudioType.ME;
			if (clip != null)
				(AudioManager.AudioHandler as AudioManager).me_play(this);
		}

		object ICloneable.Clone()
		{
			return this.MemberwiseClone();
		}
		#endregion

		private enum AudioType
		{
			NONE = 0,
			BGM = 1,
			BGS = 2,
			ME = 3,
			SE = 4
		}
	}
}