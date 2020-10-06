//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Localization;
using System;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace PokemonUnity
{
	/// <summary>
	/// When game boots up, it loads user's game configuration
	/// </summary>
	public class ProcedureLaunch : ProcedureBase
	{
		public override bool UseNativeDialog
		{
			get
			{
				return true;
			}
		}

		protected override void OnEnter(ProcedureOwner procedureOwner)
		{
			base.OnEnter(procedureOwner);

			// Build information: When the version is released, write some data into Assets/GameMain/Configs/BuildInfo.txt in Json format for the game logic to read
			GameEntry.BuiltinData.InitBuildInfo();

			// Language configuration: set the current language, if not set, the operating system language will be used by default
			InitLanguageSettings();

			// Variation configuration: According to the language used, notify the bottom layer to load the corresponding resource variant
			InitCurrentVariant();

			// 画质配置：根据检测到的硬件信息 Assets/Main/Configs/DeviceModelConfig 和用户配置数据，设置即将使用的画质选项。
			InitQualitySettings();

			// Sound configuration: Set the sound options to be used according to the user configuration data
			InitSoundSettings();

			// Default dictionary: Load the default dictionary file Assets/GameMain/Configs/DefaultDictionary.xml
			// This dictionary file records the strings of various languages ​​used before the resource update. It will be released with the App, so it cannot be updated
			GameEntry.BuiltinData.InitDefaultDictionary();
		}

		protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

			ChangeState<ProcedureSplash>(procedureOwner);
		}

		private void InitLanguageSettings()
		{
			if (GameEntry.Base.EditorResourceMode && GameEntry.Base.EditorLanguage != Language.Unspecified)
			{
				// Editor resource mode directly uses the language set on the Inspector
				return;
			}

			//Language language = GameEntry.Localization.Language;
			PokemonUnity.Languages language = Game.UserLanguage;
			string languageString = GameEntry.Setting.GetString(Constant.Setting.Language);
			if (!string.IsNullOrEmpty(languageString))
			{
				try
				{
					language = (PokemonUnity.Languages)Enum.Parse(typeof(PokemonUnity.Languages), languageString);
				}
				catch
				{

				}
			}

			if (language != PokemonUnity.Languages.English)
				//&& language != Language.ChineseSimplified
				//&& language != Language.ChineseTraditional
				//&& language != Language.Korean)
			{
				// If the language is not currently supported, use English
				language = PokemonUnity.Languages.English;

				GameEntry.Setting.SetString(Constant.Setting.Language, language.ToString());
				GameEntry.Setting.Save();
			}

			//GameEntry.Localization.Language = language;
			Game.UserLanguage = language;

			Log.Info("Init language settings complete, current language is '{0}'.", language.ToString());
		}

		private void InitCurrentVariant()
		{
			if (GameEntry.Base.EditorResourceMode)
			{
				//The editor resource mode does not use AssetBundle, so there is no variant
				return;
			}

			string currentVariant = null;
			//switch (GameEntry.Localization.Language)
			switch (Game.UserLanguage)
			{
				case Languages.English:
					currentVariant = "en-us";
					break;
				//case Language.ChineseSimplified:
				//    currentVariant = "zh-cn";
				//    break;
				//case Language.ChineseTraditional:
				//    currentVariant = "zh-tw";
				//    break;
				//case Language.Korean:
				//    currentVariant = "ko-kr";
				//    break;
				default:
					currentVariant = "en-us";
					break;
			}

			GameEntry.Resource.SetCurrentVariant(currentVariant);

			Log.Info("Init current variant complete.");
		}

		private void InitQualitySettings()
		{
			QualityLevelType defaultQuality = QualityLevelType.Fantastic;
			int qualityLevel = GameEntry.Setting.GetInt(Constant.Setting.QualityLevel, (int)defaultQuality);
			QualitySettings.SetQualityLevel(qualityLevel, true);

			Log.Info("Init quality settings complete.");
		}

		private void InitSoundSettings()
		{
			GameEntry.Sound.Mute("Music", GameEntry.Setting.GetBool(Constant.Setting.MusicMuted, false));
			GameEntry.Sound.SetVolume("Music", GameEntry.Setting.GetFloat(Constant.Setting.MusicVolume, 0.3f));
			GameEntry.Sound.Mute("Sound", GameEntry.Setting.GetBool(Constant.Setting.SoundMuted, false));
			GameEntry.Sound.SetVolume("Sound", GameEntry.Setting.GetFloat(Constant.Setting.SoundVolume, 1f));
			GameEntry.Sound.Mute("UISound", GameEntry.Setting.GetBool(Constant.Setting.UISoundMuted, false));
			GameEntry.Sound.SetVolume("UISound", GameEntry.Setting.GetFloat(Constant.Setting.UISoundVolume, 1f));

			Log.Info("Init sound settings complete.");
		}
	}
}