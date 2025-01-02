using System;
using System.Collections;
using PokemonEssentials.Interface;
using PokemonUnity;
using UnityEngine;

namespace PokemonUnity
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
				//int ret = source.timeSamples; //unity using timeSamples to return position data of audio stream
				return ret;
			}
		}
		public int volume { get; set; }
		public float pitch { get; set; }

		public void bgm_play(string filename, float volume, float pitch)
		{
			bgm_play(name, volume, pitch, null);
		}
		public void bgm_play(string name, float volume= 80, float pitch= 100, int? position= null)
		{

		}
		public void bgm_fade(int ms)
		{

		}
		public void bgm_stop()
		{

		}
		public void me_play(string name, float volume= 80, float pitch= 100)
		{

		}
		public void me_fade(int ms)
		{

		}
		public void me_stop()
		{

		}
		public void bgs_play(string name, float volume= 80, float pitch= 100)
		{

		}
		public void bgs_fade(int ms)
		{

		}
		public void bgs_stop()
		{

		}
		public void se_play(string name, float volume= 80, float pitch= 100)
		{

		}
		public void se_fade(int ms)
		{

		}
		public void se_stop()
		{

		}

		//private void FadeSource(AudioSource source, float time)
		//{
		//	sfxHandler.StartCoroutine(sfxHandler.FadeIE(source, time));
		//}
		//
		//private IEnumerator FadeIE(AudioSource source, float time)
		//{
		//	float initialVolume = source.volume;
		//	float increment = 0f;
		//	while (increment < 1)
		//	{
		//		increment += (1 / time) * Time.deltaTime;
		//		if (increment > 1)
		//		{
		//			increment = 1f;
		//		}
		//
		//		if (!source.isPlaying)
		//		{
		//			increment = 1f;
		//			Debug.Log("early end");
		//		}
		//
		//		source.volume = initialVolume * (1 - increment);
		//		if (source.isPlaying)
		//		{
		//			yield return null;
		//		}
		//	}
		//	source.volume = initialVolume;
		//	source.Stop();
		//}
		//
		//private AudioSource Play(AudioClip clip, float volume = -1f, float pitch = 1f)
		//{
		//	AudioSource source = null;
		//	for (int i = 0; i < sfxHandler.sources.Length; i++)
		//	{
		//		if (!sfxHandler.sources[i].isPlaying)
		//		{
		//			source = sfxHandler.sources[i];
		//			i = sfxHandler.sources.Length;
		//		}
		//	}
		//	if (source == null)
		//	{
		//		float mostFinished = 0;
		//		int mostFinishedIndex = 0;
		//		//Find the source closest to finishing playing it's clip
		//		for (int i = 0; i < sfxHandler.sources.Length; i++)
		//		{
		//			if (sfxHandler.sources[i].clip != null)
		//			{
		//				if (sfxHandler.sources[i].clip.length != 0)
		//				{
		//					if ((sfxHandler.sources[i].timeSamples / sfxHandler.sources[i].clip.length) > mostFinished)
		//					{
		//						mostFinished = sfxHandler.sources[i].timeSamples / sfxHandler.sources[i].clip.length;
		//						mostFinishedIndex = i;
		//					}
		//				}
		//			}
		//		}
		//		//play the new clip on the source with the highest played percentage
		//		source = sfxHandler.sources[mostFinishedIndex];
		//	}
		//	source.clip = clip;
		//	if (volume < 0)
		//	{
		//		source.volume = PlayerPrefs.GetFloat("sfxVolume");
		//	}
		//	else
		//	{
		//		source.volume = volume * PlayerPrefs.GetFloat("sfxVolume");
		//	}
		//	source.pitch = pitch;
		//	source.Play();
		//
		//	return source;
		//}
	}

	public class GameAudioPlay : PokemonEssentials.Interface.IGameAudioPlay
	{
		public IAudioObject pbStringToAudioFile(string name)
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

		public IAudioObject pbResolveAudioFile(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			if (name is string)
			{
				IAudioObject str = pbStringToAudioFile(name);
				str.volume = 100;
				if (volume != null) str.volume = volume.Value;
				str.pitch = 100;
				if (pitch != null) str.pitch = pitch.Value;
				return pbResolveAudioFile(str, str.volume, str.pitch);
			}
			return null; //str doesnt exist...
		}

		public IAudioObject pbResolveAudioFile(IAudioObject str, int? volume = null, float? pitch = null)
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
		public void pbBGMPlay(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioBGM param = pbResolveAudioFile(name, volume, pitch) as IAudioBGM;
			pbBGMPlay(param, volume, pitch);
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

		public void pbBGMPlay(IAudioObject name, int? volume = null, float? pitch = null)
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

		public void pbBGMFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbBGMStop(x);
		}

		public void pbBGMStop(float timeInSeconds = 0)
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
		public void pbMEPlay(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioME param = pbResolveAudioFile(name, volume, pitch) as IAudioME;
			pbMEPlay(param, volume, pitch);
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

		public void pbMEPlay(IAudioObject name, int? volume = null, float? pitch = null)
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

		public void pbMEFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbMEStop(x);
		}

		public void pbMEStop(float timeInSeconds = 0)
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
		public void pbBGSPlay(string name, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioBGS param = pbResolveAudioFile(name, volume, pitch) as IAudioBGS;
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

		public void pbBGSPlay(IAudioObject param, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		public void pbBGSFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbBGSStop(x);
		}

		public void pbBGSStop(float timeInSeconds = 0)
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
		public void pbSEPlay(string param, int? volume = null, float? pitch = null)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(param)) return;
			pbSEPlay(pbResolveAudioFile(param, volume, pitch), volume, pitch);
		}

		public void pbSEPlay(IAudioObject param, int? volume = null, float? pitch = null)
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

		public void pbSEFade(float x = 0)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			
			pbSEStop(x);
		}

		public void pbSEStop(float timeInSeconds = 0)
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
		public void pbPlayCursorSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("cursor_se") &&
			   Game.GameData.DataSystem.cursor_se != null && Game.GameData.DataSystem.cursor_se.name != "")
			{
				pbSEPlay(Game.GameData.DataSystem.cursor_se);
			}
			//else if (Game.GameData.DataSystem && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[0] && Game.GameData.DataSystem.sounds[0].name != "")
			//{
			//	pbSEPlay(Game.GameData.DataSystem.sounds[0]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				pbSEPlay("Choose", 80);
			}
		}

		public void pbPlayDecisionSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("decision_se") &&
			   Game.GameData.DataSystem.decision_se != null && Game.GameData.DataSystem.decision_se.name != "")
			{
				pbSEPlay(Game.GameData.DataSystem.decision_se);
			}
			//else if (Game.GameData.DataSystem && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[1] && Game.GameData.DataSystem.sounds[1].name != "")
			//{
			//	pbSEPlay(Game.GameData.DataSystem.sounds[1]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				pbSEPlay("Choose", 80);
			}
		}
		 
		public void pbPlayCancelSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("cancel_se") &&
			   Game.GameData.DataSystem.cancel_se != null && Game.GameData.DataSystem.cancel_se.name != "")
			{
				pbSEPlay(Game.GameData.DataSystem.cancel_se);
			}
			//else if (Game.GameData.DataSystem != null && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[2] && Game.GameData.DataSystem.sounds[2].name != "")
			//{
			//	pbSEPlay(Game.GameData.DataSystem.sounds[2]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				pbSEPlay("Choose", 80);
			}
		}

		public void pbPlayBuzzerSE()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData.DataSystem != null && //Game.GameData.DataSystem.respond_to("buzzer_se") &&
			   Game.GameData.DataSystem.buzzer_se != null && Game.GameData.DataSystem.buzzer_se.name != "")
			{
				pbSEPlay(Game.GameData.DataSystem.buzzer_se);
			}
			//else if (Game.GameData.DataSystem && Game.GameData.DataSystem.respond_to("sounds") &&
			// Game.GameData.DataSystem.sounds && Game.GameData.DataSystem.sounds[3] && Game.GameData.DataSystem.sounds[3].name != "")
			//{
			//	pbSEPlay(Game.GameData.DataSystem.sounds[3]);
			//}
			else if (FileTest.audio_exist("Audio/SE/buzzer"))
			{
				pbSEPlay("buzzer", 80);
			}
		}
		#endregion
	}
}