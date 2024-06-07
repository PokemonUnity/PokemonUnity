using System;
using System.Linq;
using PokemonEssentials.Interface;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	public class FileTest : MonoBehaviour, IFileTest
	{
		private static AudioData _audioData;
		private global::UnityEngine.UI.Image[] images;
		public string[] Image_ext { get; }
		public string[] Audio_ext { get; }

		#region Audio
		[Header("Audio")]
		private AudioData audioData;
		public global::UnityEngine.AudioClip SoundEffectChoose;
		public global::UnityEngine.AudioClip SoundEffectBuzzer;
		public global::UnityEngine.AudioClip SoundEffectExpFull;
		public global::UnityEngine.AudioClip SoundEffectThrow;
		public global::UnityEngine.AudioClip SoundEffectRecall;
		public global::UnityEngine.AudioClip SoundEffectBallShake;
		public global::UnityEngine.AudioClip SoundEffectBallDrop;
		public global::UnityEngine.AudioClip SoundEffectJumpToBall;
		public global::UnityEngine.AudioClip SoundEffectJump;

		//public global::UnityEngine.AudioClip[] BackgroundMusic;
		//public global::UnityEngine.AudioClip[] BackgroundSound;
		#endregion

		#region Images
		[Header("Images")]
		public global::UnityEngine.UI.Image[] Types;
		/// <summary>
		/// Text Frames for the message box
		/// </summary>
		public global::UnityEngine.UI.Image[] Windowskins;
		/// <summary>
		/// The image of the Poké Ball being thrown to capture a Pokémon, during battle animation.
		/// </summary>
		public global::UnityEngine.UI.Image[] Balls;
		public global::UnityEngine.UI.Image[] BallsOpen;
		/// <summary>
		/// Used for the party line-up, to show the player's Pokémon status.
		/// </summary>
		public global::UnityEngine.UI.Image BallEmpty;
		public global::UnityEngine.UI.Image BallFainted;
		public global::UnityEngine.UI.Image BallStatus;
		public global::UnityEngine.UI.Image BallNormal;
		public global::UnityEngine.UI.Image BattleMegaEvo;
		public global::UnityEngine.UI.Image battleMegaEvoBox;
		public global::UnityEngine.UI.Image battlePrimalKyogreBox;
		public global::UnityEngine.UI.Image battlePrimalGroudonBox;
		public global::UnityEngine.UI.Image battleBoxOwned;
		public global::UnityEngine.UI.Image battlePlayerBoxS;
		public global::UnityEngine.UI.Image battleFoeBoxS;
		public global::UnityEngine.UI.Image battlePlayerBoxD;
		public global::UnityEngine.UI.Image battleFoeBoxD;
		#endregion

		#region Unity Monobehavior
		private void Awake()
		{
			_audioData = audioData;
		}
		#endregion

		public static bool audio_exist(string filename)
		{
			if (filename == null || filename == "")
				return false;
			string[] folders = filename.Split('/');
			if(folders.Length > 0)
			{
				//if (folders[1] == "BGM")
				if (filename.StartsWith("Audio/BGM/"))
					return _audioData.BackgroundMusic.Any(clip => clip.name.Contains(folders[2]));
				//if (folders[1] == "BGS")
				if (filename.StartsWith("Audio/BGS/"))
					return _audioData.BackgroundSound.Any(clip => clip.name.Contains(folders[2]));
				//if (folders[1] == "ME")
				if (filename.StartsWith("Audio/ME/"))
					return _audioData.MusicEffect.Any(clip => clip.name.Contains(folders[2]));
				//if (folders[1] == "SE")
				if (filename.StartsWith("Audio/SE/"))
					return _audioData.SoundEffect.Any(clip => clip.name.Contains(folders[2]));
			};
			return false;
		}

		public static bool image_exist(string filename)
		{
			return false;
		}

		bool IFileTest.audio_exist(string filename)
		{
			return audio_exist(filename);
		}

		bool IFileTest.image_exist(string filename)
		{
			return image_exist(filename);
		}
	}

	[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/Manager")]
	public class AudioData : ScriptableObject
	{
		//public const string BGM = "Audio/BGM/";
		//public const string BGM = "Audio/SE/Choose";
		//public const string BGM = "Audio/SE/buzzer";
		//public const string BGM = "Audio/SE/expfull";
		//public const string BGM = "Audio/SE/throw";
		//public const string BGM = "Audio/SE/recall";
		//public const string BGM = "Audio/SE/jumptoball";
		//public const string BGM = "Audio/SE/ballshake";
		//public const string BGM = "Audio/SE/balldrop";
		//public const string BGM = "Audio/SE/jump";

		public global::UnityEngine.AudioClip[] BackgroundMusic;
		public global::UnityEngine.AudioClip[] BackgroundSound;
		public global::UnityEngine.AudioClip[] MusicEffect;
		public global::UnityEngine.AudioClip[] SoundEffect;
	}
}