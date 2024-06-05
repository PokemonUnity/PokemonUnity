using System;
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
	public class Audio : MonoBehaviour, IAudio
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
		private List<AudioSource> seSources = new List<AudioSource>();

		private void Awake()
		{
			// Create different AudioSource components for different types of audio
			bgmSource = gameObject.AddComponent<AudioSource>();
			bgsSource = gameObject.AddComponent<AudioSource>();
			meSource = gameObject.AddComponent<AudioSource>();
		}

		public void bgm_play(string filename, float volume, float pitch)
		{
			PlayAudio(bgmSource, filename, volume, pitch, true);
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

		private void PlayAudio(AudioSource source, string filename, float volume, float pitch, bool loop)
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
			source.Play();
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
	}

	public class GameAudioPlay : PokemonEssentials.Interface.IGameAudioPlay
	{
		public IAudioObject StringToAudioFile(string name)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			string pat1 = @"^(.*)\:\s*(\d+)\s*\:\s*(\d+)\s*$";
			string pat2 = @"^(.*)\:\s*(\d+)\s*$";
			System.Text.RegularExpressions.Regex r1 = new System.Text.RegularExpressions.Regex(pat1, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.Regex r2 = new System.Text.RegularExpressions.Regex(pat2, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

			//Regex the string to get name value, and other information
			System.Text.RegularExpressions.Match m1 = r1.Match(name);
			System.Text.RegularExpressions.Match m2 = r2.Match(name);
			if (m1.Success) { //str[/^(.*)\:\s*(\d+)\s*\:\s*(\d+)\s*$/]
				string file = m1.Captures[1].Value; //$1;
				int volume = int.Parse(m1.Captures[2].Value); //$2.to_i;
				int pitch = int.Parse(m1.Captures[3].Value); //$3.to_i;
				return new AudioTrack().initialize(file,volume,pitch);
			} else if (m2.Success) {//str[/^(.*)\:\s*(\d+)\s*$/]
				string file = m2.Captures[1].Value; //$1;
				int volume = int.Parse(m2.Captures[2].Value); //$2.to_i;
				return new AudioTrack().initialize(file,volume,100);
			} else {
				return new AudioTrack().initialize(name,100,100);
			}
		}

		public IAudioObject ResolveAudioFile(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (name is string)
			{
				IAudioObject str = StringToAudioFile(name);
				str.volume = 100;
				if (volume != null) str.volume = volume.Value;
				str.pitch = 100;
				if (pitch != null) str.pitch = pitch.Value;
				return ResolveAudioFile(str, str.volume, str.pitch);
			}
			return null; //str doesnt exist...
		}

		public IAudioObject ResolveAudioFile(IAudioObject str, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (str is IAudioObject) //str.is_a(AudioFile)
			{
				if (volume != null || pitch != null)
				{
					return new AudioTrack().initialize(str.name,
											  volume ?? str.volume, //?? 80
											  pitch ?? str.pitch); //?? 100
				}
				else
				{
					return str;
				}
			}

			return str;
		}

		#region Background Music
		public void BGMPlay(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioBGM param = ResolveAudioFile(name, volume, pitch) as IAudioBGM;
			BGMPlay(param, volume, pitch);
			//if (param.name != null && param.name != "")
			//{
			//	if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("bgm_play"))
			//	{
			//		Game.GameData.GameSystem.bgm_play(param);
			//		return;
			//	}
			//	//else if (RPG.const_defined(:BGM)) { //rescue false
			//	//	b = new RPG.BGM(param.name, param.volume, param.pitch);
			//	//	if (b != null && b.respond_to("play"))
			//	//	{
			//	//		b.play(); return;
			//	//	}
			//	//}
			//	Audio.AudioHandler.bgm_play("Audio/BGM/" + param.name, param.volume, param.pitch);
			//}
		}

		public void BGMPlay(IAudioObject name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (name == null) return;
			IAudioBGM param = name as IAudioBGM;
			if (param.name != null && param.name != "")
			{
				if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("bgm_play"))
				{
					Game.GameData.GameSystem.bgm_play(param);
					return;
				}
				//else if ((RPG.const_defined(:BGM))) { //rescue false
				//	b = new RPG.BGM(param.name, param.volume, param.pitch);
				//	if (b && b.respond_to("play"))
				//	{
				//		b.play; return;
				//	}
				//}
				Audio.AudioHandler.bgm_play("Audio/BGM/" + param.name, param.volume, param.pitch);
			}
		}

		public void BGMFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			BGMStop(x);
		}

		public void BGMStop(float timeInSeconds = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.GameSystem != null && timeInSeconds>0.0) //&& Game.GameData.GameSystem.respond_to("bgm_fade"))
			{
				Game.GameData.GameSystem.bgm_fade(timeInSeconds);
				return;
			} else if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("bgm_stop"))
			{
				Game.GameData.GameSystem.bgm_stop();
				return;
			}
			//else if (RPG.const_defined(:BGM)) { //rescue false
			//	try{ //begin;
			//		//(timeInSeconds > 0.0) ? RPG.BGM.fade((timeInSeconds * 1000).floor) : RPG.BGM.stop();
			//		if (timeInSeconds > 0.0)
			//			RPG.BGM.fade((timeInSeconds * 1000).floor) : RPG.BGM.stop();
			//		return;
			//	} catch (Exception) { } //rescue;
			//}
			//(timeInSeconds > 0.0) ? Audio.AudioHandler.bgm_fade((timeInSeconds * 1000).floor) : Audio.AudioHandler.bgm_stop();
			if (timeInSeconds > 0.0)
				Audio.AudioHandler.bgm_fade((int)Math.Floor(timeInSeconds * 1000));
			else
				Audio.AudioHandler.bgm_stop();
		}
		#endregion

		#region Music Effects
		public void MEPlay(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioME param = ResolveAudioFile(name, volume, pitch) as IAudioME;
			MEPlay(param, volume, pitch);
			//if (param.name != null && param.name != "")
			//{
			//	if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("me_play"))
			//	{
			//		Game.GameData.GameSystem.me_play(param);
			//		return;
			//	}
			//	//else if (RPG.const_defined(:ME)) { //rescue false
			//	//	b = new RPG.ME(param.name, param.volume, param.pitch);
			//	//	if (b && b.respond_to("play"))
			//	//	{
			//	//		b.play(); return;
			//	//	}
			//	//}
			//	Audio.AudioHandler.me_play("Audio/ME/" + param.name, param.volume, param.pitch);
			//}
		}

		public void MEPlay(IAudioObject name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (name == null) return;
			IAudioME param = name as IAudioME;
			if (param.name != null && param.name != "")
			{
				if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("me_play"))
				{
					Game.GameData.GameSystem.me_play(param);
					return;
				}
				//else if (RPG.const_defined(:ME)) { //rescue false
				//	b = new RPG.ME(param.name, param.volume, param.pitch);
				//	if (b && b.respond_to("play"))
				//	{
				//		b.play(); return;
				//	}
				//}
				Audio.AudioHandler.me_play("Audio/ME/" + param.name, param.volume, param.pitch);
			}
		}

		public void MEFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			MEStop(x);
		}

		public void MEStop(float timeInSeconds = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.GameSystem != null && timeInSeconds > 0.0) //&& Game.GameData.GameSystem.respond_to("me_fade"))
			{
				Game.GameData.GameSystem.me_fade(timeInSeconds);
				return;
			}
			else if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("me_stop"))
			{
				//Game.GameData.GameSystem.me_stop(null);
				Game.GameData.GameSystem.me_stop();
				return;
			}
			//else if (RPG.const_defined(:ME)) { //rescue false
			//	try { //begin;
			//		  //(timeInSeconds > 0.0) ? RPG.ME.fade((timeInSeconds * 1000).floor) : RPG.ME.stop();
			//		if (timeInSeconds > 0.0)
			//			RPG.ME.fade(Math.Floor(timeInSeconds * 1000));
			//		else
			//			RPG.ME.stop();
			//		return;
			//	} catch (Exception) { } //rescue;
			//}
			//(timeInSeconds>0.0) ? Audio.AudioHandler.me_fade((timeInSeconds*1000).floor) : Audio.AudioHandler.me_stop();
			if (timeInSeconds > 0.0)
				Audio.AudioHandler.me_fade((int)Math.Floor(timeInSeconds * 1000));
			else
				Audio.AudioHandler.me_stop();
		}
		#endregion

		#region Background Sounds
		public void BGSPlay(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioBGS param = ResolveAudioFile(name, volume, pitch) as IAudioBGS;
			if (param.name != null && param.name != "")
			{
				if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("bgs_play"))
				{
					Game.GameData.GameSystem.bgs_play(param);
					return;
				}
				//else if (RPG.const_defined(:BGS)) { //rescue false
				//	b = new RPG.BGS(param.name, param.volume, param.pitch);
				//	if (b != null && b.respond_to("play"))
				//	{
				//		b.play(); return;
				//	}
				//}
				Audio.AudioHandler.bgs_play("Audio/BGS/" + param.name, param.volume, param.pitch);
			}
		}

		public void BGSPlay(IAudioObject param, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		public void BGSFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			BGSStop(x);
		}

		public void BGSStop(float timeInSeconds = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.GameSystem != null && timeInSeconds > 0.0) //&& Game.GameData.GameSystem.respond_to("bgs_fade"))
			{
				Game.GameData.GameSystem.bgs_fade(timeInSeconds);
				return;
			}
			else if (Game.GameData.GameSystem != null) //&& Game.GameData.GameSystem.respond_to("bgs_play"))
			{
				Game.GameData.GameSystem.bgs_play(null);
				return;
			}
			//else if (RPG.const_defined(:BGS)) { //rescue false
			//	try { //begin;
			//		//(timeInSeconds > 0.0) ? RPG.BGS.fade((timeInSeconds * 1000).floor) : RPG.BGS.stop();
			//		if (timeInSeconds > 0.0)
			//			RPG.BGS.fade((timeInSeconds * 1000).floor);
			//		else
			//			RPG.BGS.stop();
			//		return;
			//	} catch (Exception) {
			//		//rescue;
			//	}
			//}
			//(timeInSeconds > 0.0) ? Audio.AudioHandler.bgs_fade((timeInSeconds * 1000).floor) : Audio.AudioHandler.bgs_stop();
			if (timeInSeconds > 0.0)
				Audio.AudioHandler.bgs_fade((int)Math.Floor(timeInSeconds * 1000));
			else
				Audio.AudioHandler.bgs_stop();
		}
		#endregion

		#region Sound Effects
		public void SEPlay(string param, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(param)) return;
			SEPlay(ResolveAudioFile(param, volume, pitch), volume, pitch);
		}

		public void SEPlay(IAudioObject param, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (param.name != null && param.name != "")
			{
				// If layer like unity cant handle, then try a different approach like
				// playing it directly from device, older/newer unity technology, or something...
				//if (Game.GameData.GameSystem != null && param is IAudioSE audioSE) //Game.GameData.GameSystem.respond_to("se_play")
				//{
				//	Game.GameData.GameSystem.se_play(audioSE);
				//	return;
				//}
				//else if ((RPG.const_defined(:SE) rescue false)) {
				//	b = new RPG.ME(param.name, param.volume, param.pitch);
				//	if (b && b.respond_to("play"))
				//	{
				//		b.play; return;
				//	}
				//}
				Audio.AudioHandler.se_play("Audio/SE/" + param.name, param.volume, param.pitch);
			}
		}

		public void SEFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			SEStop(x);
		}

		public void SEStop(float timeInSeconds = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.GameSystem != null)
			{
				Game.GameData.GameSystem.se_stop();
			}
			//else if ((RPG.const_defined(:SE) rescue false)) {
			//	RPG.ME.stop; //rescue null
			//}
			else
			{
				Audio.AudioHandler.se_stop();
			}
		}
		#endregion

		#region Misc Game Sounds
		public void PlayCursorSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("cursor_se") &&
			   Game.GameData.DataSystem.cursor_se != null && Game.GameData.DataSystem.cursor_se.name != "")
			{
				SEPlay(Game.GameData.DataSystem.cursor_se);
			}
			//else if (Game.GameData.DataSystem && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[0] && Game.GameData.DataSystem.sounds[0].name != "")
			//{
			//	SEPlay(Game.GameData.DataSystem.sounds[0]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				SEPlay("Choose", 80);
			}
		}

		public void PlayDecisionSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("decision_se") &&
			   Game.GameData.DataSystem.decision_se != null && Game.GameData.DataSystem.decision_se.name != "")
			{
				SEPlay(Game.GameData.DataSystem.decision_se);
			}
			//else if (Game.GameData.DataSystem && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[1] && Game.GameData.DataSystem.sounds[1].name != "")
			//{
			//	SEPlay(Game.GameData.DataSystem.sounds[1]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				SEPlay("Choose", 80);
			}
		}

		public void PlayCancelSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("cancel_se") &&
			   Game.GameData.DataSystem.cancel_se != null && Game.GameData.DataSystem.cancel_se.name != "")
			{
				SEPlay(Game.GameData.DataSystem.cancel_se);
			}
			//else if (Game.GameData.DataSystem != null && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[2] && Game.GameData.DataSystem.sounds[2].name != "")
			//{
			//	SEPlay(Game.GameData.DataSystem.sounds[2]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				SEPlay("Choose", 80);
			}
		}

		public void PlayBuzzerSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("buzzer_se") &&
			   Game.GameData.DataSystem.buzzer_se != null && Game.GameData.DataSystem.buzzer_se.name != "")
			{
				SEPlay(Game.GameData.DataSystem.buzzer_se);
			}
			//else if (Game.GameData.DataSystem && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[3] && Game.GameData.DataSystem.sounds[3].name != "")
			//{
			//	SEPlay(Game.GameData.DataSystem.sounds[3]);
			//}
			else if (FileTest.audio_exist("Audio/SE/buzzer"))
			{
				SEPlay("buzzer", 80);
			}
		}
		#endregion
	}
}