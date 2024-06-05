using System;
using PokemonEssentials.Interface;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
    public class FileTest : MonoBehaviour, IFileTest
	{
		private global::UnityEngine.UI.Image[] images;
		private global::UnityEngine.AudioClip[] audio;
		public string[] Image_ext { get; }
		public string[] Audio_ext { get; }

		#region Audio
		[Header("Audio")]
		public global::UnityEngine.AudioClip SoundEffectThrow;
		public global::UnityEngine.AudioClip SoundEffectRecall;
		public global::UnityEngine.AudioClip SoundEffectBallShake;
		public global::UnityEngine.AudioClip SoundEffectBallDrop;
		public global::UnityEngine.AudioClip SoundEffectJumpToBall;
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

		public static bool audio_exist(string filename)
		{
			throw new NotImplementedException();
		}

		public static bool image_exist(string filename)
		{
			throw new NotImplementedException();
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
}