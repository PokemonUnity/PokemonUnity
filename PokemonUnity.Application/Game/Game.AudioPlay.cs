using System;
using PokemonEssentials.Interface;

namespace PokemonUnity
{
	public partial class Game : PokemonEssentials.Interface.IGameAudioPlay
	{
		public IAudioObject StringToAudioFile(string name)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

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
				//return new AudioTrack().initialize(file,volume,pitch);
				//return Audio.ResolveAudioFile(file,volume,pitch);
				return FileTest.initialize(file,volume,pitch);
			} else if (m2.Success) {//str[/^(.*)\:\s*(\d+)\s*$/]
				string file = m2.Captures[1].Value; //$1;
				int volume = int.Parse(m2.Captures[2].Value); //$2.to_i;
				//return new AudioTrack().initialize(file,volume,100);
				return FileTest.initialize(file,volume,100);
			} else {
				//return new AudioTrack().initialize(name,100,100);
				return FileTest.initialize(name,100,100);
			}
		}

		public IAudioObject ResolveAudioFile(string name, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
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
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (str is IAudioObject) //str.is_a(AudioFile)
			{
				if (volume != null || pitch != null)
				{
					//return new AudioTrack().initialize(str.name,
					return FileTest.initialize(str.name,
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
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioBGM param = ResolveAudioFile(name, volume, pitch) as IAudioBGM;
			BGMPlay(param, volume, pitch);
			//if (param.name != null && param.name != "")
			//{
			//	if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("bgm_play"))
			//	{
			//		GameData.GameSystem.bgm_play(param);
			//		return;
			//	}
			//	//else if (RPG.const_defined(:BGM)) { //rescue false
			//	//	b = new RPG.BGM(param.name, param.volume, param.pitch);
			//	//	if (b != null && b.respond_to("play"))
			//	//	{
			//	//		b.play(); return;
			//	//	}
			//	//}
			//	if (GameData.Audio is IAudio audio) audio.bgm_play("AudioManager/BGM/" + param.name, param.volume, param.pitch);
			//}
		}

		public void BGMPlay(IAudioObject name, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (name == null) return;
			IAudioBGM param = name as IAudioBGM;
			if (param.name != null && param.name != "")
			{
				//if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("bgm_play"))
				//{
				//	GameData.GameSystem.bgm_play(param);
				//	return;
				//}
				if (GameData.Audio is IAudio audio) audio.bgm_play("Audio/BGM/" + param.name, param.volume, param.pitch);
			}
		}

		public void BGMFade(float x = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			BGMStop(x);
		}

		public void BGMStop(float timeInSeconds = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (GameData.GameSystem != null && timeInSeconds>0.0) //&& GameData.GameSystem.respond_to("bgm_fade"))
			//{
			//	GameData.GameSystem.bgm_fade(timeInSeconds);
			//	return;
			//} else if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("bgm_stop"))
			//{
			//	GameData.GameSystem.bgm_stop();
			//	return;
			//}
			//(timeInSeconds > 0.0) ? AudioManager.AudioHandler.bgm_fade((timeInSeconds * 1000).floor) : AudioManager.AudioHandler.bgm_stop();
			if (timeInSeconds > 0.0)
				if (GameData.Audio is IAudio audio) audio.bgm_fade((int)Math.Floor(timeInSeconds * 1000));
			else
				if (GameData.Audio is IAudio audio1) audio1.bgm_stop();
		}
		#endregion

		#region Music Effects
		public void MEPlay(string name, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioME param = ResolveAudioFile(name, volume, pitch) as IAudioME;
			MEPlay(param, volume, pitch);
			//if (param.name != null && param.name != "")
			//{
			//	if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("me_play"))
			//	{
			//		GameData.GameSystem.me_play(param);
			//		return;
			//	}
			//	//else if (RPG.const_defined(:ME)) { //rescue false
			//	//	b = new RPG.ME(param.name, param.volume, param.pitch);
			//	//	if (b && b.respond_to("play"))
			//	//	{
			//	//		b.play(); return;
			//	//	}
			//	//}
			//	AudioManager.AudioHandler.me_play("AudioManager/ME/" + param.name, param.volume, param.pitch);
			//}
		}

		public void MEPlay(IAudioObject name, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (name == null) return;
			IAudioME param = name as IAudioME;
			if (param.name != null && param.name != "")
			{
				//if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("me_play"))
				//{
				//	GameData.GameSystem.me_play(param);
				//	return;
				//}
				if (GameData.Audio is IAudio audio) audio.me_play("Audio/ME/" + param.name, param.volume, param.pitch);
			}
		}

		public void MEFade(float x = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			MEStop(x);
		}

		public void MEStop(float timeInSeconds = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (GameData.GameSystem != null && timeInSeconds > 0.0) //&& GameData.GameSystem.respond_to("me_fade"))
			//{
			//	GameData.GameSystem.me_fade(timeInSeconds);
			//	return;
			//}
			//else if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("me_stop"))
			//{
			//	//GameData.GameSystem.me_stop(null);
			//	GameData.GameSystem.me_stop();
			//	return;
			//}
			//(timeInSeconds>0.0) ? AudioManager.AudioHandler.me_fade((timeInSeconds*1000).floor) : AudioManager.AudioHandler.me_stop();
			if (timeInSeconds > 0.0)
				if (GameData.Audio is IAudio audio) audio.me_fade((int)Math.Floor(timeInSeconds * 1000));
			else
				if (GameData.Audio is IAudio audio1) audio1.me_stop();
		}
		#endregion

		#region Background Sounds
		public void BGSPlay(string name, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(name)) return;
			IAudioBGS param = ResolveAudioFile(name, volume, pitch) as IAudioBGS;
			if (param.name != null && param.name != "")
			{
				//if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("bgs_play"))
				//{
				//	GameData.GameSystem.bgs_play(param);
				//	return;
				//}
				if (GameData.Audio is IAudio audio) audio.bgs_play("Audio/BGS/" + param.name, param.volume, param.pitch);
			}
		}

		public void BGSPlay(IAudioObject param, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		public void BGSFade(float x = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			BGSStop(x);
		}

		public void BGSStop(float timeInSeconds = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (GameData.GameSystem != null && timeInSeconds > 0.0) //&& GameData.GameSystem.respond_to("bgs_fade"))
			//{
			//	GameData.GameSystem.bgs_fade(timeInSeconds);
			//	return;
			//}
			//else if (GameData.GameSystem != null) //&& GameData.GameSystem.respond_to("bgs_play"))
			//{
			//	GameData.GameSystem.bgs_play(null);
			//	return;
			//}
			//(timeInSeconds > 0.0) ? AudioManager.AudioHandler.bgs_fade((timeInSeconds * 1000).floor) : AudioManager.AudioHandler.bgs_stop();
			if (timeInSeconds > 0.0)
				if (GameData.Audio is IAudio audio) audio.bgs_fade((int)Math.Floor(timeInSeconds * 1000));
			else
				if (GameData.Audio is IAudio audio1) audio1.bgs_stop();
		}
		#endregion

		#region Sound Effects
		public void SEPlay(string param, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (string.IsNullOrEmpty(param)) return;
			SEPlay(ResolveAudioFile(param, volume, pitch), volume, pitch);
		}

		public void SEPlay(IAudioObject param, int? volume = null, float? pitch = null)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (param.name != null && param.name != "")
			{
				// If layer like unity cant handle, then try a different approach like
				// playing it directly from device, older/newer unity technology, or something...
				//if (GameData.GameSystem != null && param is IAudioSE audioSE) //GameData.GameSystem.respond_to("se_play")
				//{
				//	GameData.GameSystem.se_play(audioSE);
				//	return;
				//}
				//else if ((RPG.const_defined(:SE) rescue false)) {
				//	b = new RPG.ME(param.name, param.volume, param.pitch);
				//	if (b && b.respond_to("play"))
				//	{
				//		b.play; return;
				//	}
				//}
				if (GameData.Audio is IAudio audio) audio.se_play("Audio/SE/" + param.name, param.volume, param.pitch);
			}
		}

		public void SEFade(float x = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			SEStop(x);
		}

		public void SEStop(float timeInSeconds = 0)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (GameData.GameSystem != null)
			//{
			//	GameData.GameSystem.se_stop();
			//	return;
			//}
			if (GameData.Audio is IAudio audio) audio.se_stop();
		}
		#endregion

		#region Misc Game Sounds
		public void PlayCursorSE()
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (GameData.DataSystem != null && //GameData.DataSystem.respond_to("cursor_se") &&
			   GameData.DataSystem.cursor_se != null && GameData.DataSystem.cursor_se.name != "")
			{
				SEPlay(GameData.DataSystem.cursor_se);
			}
			//else if (GameData.DataSystem && GameData.DataSystem.respond_to("sounds") &&
			// GameData.DataSystem.sounds && GameData.DataSystem.sounds[0] && GameData.DataSystem.sounds[0].name != "")
			//{
			//	SEPlay(GameData.DataSystem.sounds[0]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				SEPlay("Choose", 80);
			}
		}

		public void PlayDecisionSE()
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (GameData.DataSystem != null && //GameData.DataSystem.respond_to("decision_se") &&
			   GameData.DataSystem.decision_se != null && GameData.DataSystem.decision_se.name != "")
			{
				SEPlay(GameData.DataSystem.decision_se);
			}
			//else if (GameData.DataSystem && GameData.DataSystem.respond_to("sounds") &&
			// GameData.DataSystem.sounds && GameData.DataSystem.sounds[1] && GameData.DataSystem.sounds[1].name != "")
			//{
			//	SEPlay(GameData.DataSystem.sounds[1]);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				SEPlay("Choose", 80);
			}
		}

		public void PlayCancelSE()
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (GameData.DataSystem != null && //GameData.DataSystem.respond_to("cancel_se") &&
			   GameData.DataSystem.cancel_se != null && GameData.DataSystem.cancel_se.name != "")
			{
				SEPlay(GameData.DataSystem.cancel_se);
			}
			//else if (GameData.DataSystem != null && GameData.DataSystem.respond_to("sounds") &&
			// GameData.DataSystem.sounds && GameData.DataSystem.sounds[2] && GameData.DataSystem.sounds[2].name != "")
			//{
			//	SEPlay(GameData.DataSystem.sounds[2]);
			//}
			//else if (FileTest.Instance.SoundEffectChoose != null)
			//{
			//	SEPlay(FileTest.Instance.SoundEffectChoose, 80);
			//}
			else if (FileTest.audio_exist("Audio/SE/Choose"))
			{
				SEPlay("Choose", 80);
			}
		}

		public void PlayBuzzerSE()
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (GameData.DataSystem != null && //GameData.DataSystem.respond_to("buzzer_se") &&
			   GameData.DataSystem.buzzer_se != null && GameData.DataSystem.buzzer_se.name != "")
			{
				SEPlay(GameData.DataSystem.buzzer_se);
			}
			//else if (GameData.DataSystem && GameData.DataSystem.respond_to("sounds") &&
			// GameData.DataSystem.sounds && GameData.DataSystem.sounds[3] && GameData.DataSystem.sounds[3].name != "")
			//{
			//	SEPlay(GameData.DataSystem.sounds[3]);
			//}
			else if (FileTest.audio_exist("Audio/SE/buzzer"))
			{
				SEPlay("buzzer", 80);
			}
		}
		#endregion
	}
}