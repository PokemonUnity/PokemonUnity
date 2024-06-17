using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PokemonEssentials.Interface;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	[RequireComponent(typeof(AudioData))]
	public class FileTest : MonoBehaviour, IFileTest//, IResources
	{
		public static IResourcesAudio Instance { get { return _audioData; } }
		private static AudioData _audioData;
		private global::UnityEngine.UI.Image[] images;
		public string[] Image_ext { get; }
		public string[] Audio_ext { get; }
		[SerializeField] private AudioData audioData;

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
			//if (Instance == null)
			//{
			//	Instance = this;
			//}
			//else if (Instance != this)
			//{
			//	Destroy(gameObject);
			//}
			audioData = GetComponent<AudioData>();
			_audioData = audioData;
		}
		#endregion

		public static bool ResolveBitmap(string filename)
		{
			if (filename == null || filename == "")
				return false;
			return false;
		}

		public static string TrainerSpriteFile(TrainerTypes trainer)
		{
			string filename = "Graphics/Trainers/" + trainer.ToString();
			if (filename == null || filename == "")
				return null;
			return null;
		}

		public static string TrainerSpriteBackFile(TrainerTypes trainer)
		{
			string filename = "Graphics/Trainers/" + trainer.ToString();
			if (filename == null || filename == "")
				return null;
			return null;
		}

		public static string PlayerSpriteBackFile(TrainerTypes trainer)
		{
			string filename = "Graphics/Trainers/" + trainer.ToString();
			if (filename == null || filename == "")
				return null;
			return null;
		}

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
	public class AudioData : ScriptableObject, IResourcesAudio
	{
		/// <summary>
		/// Use for the sound effect of the Pokémon's cry
		/// </summary>
		/// <remarks>
		/// Whatever value is assigned here, is what will be played when <see cref="SoundEffectPkmnCry_"/> is called
		/// </remarks>
		public Pokemons Pokemon;
		#region Sound Effects
		[Header("Sound Effects")]
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectMenu;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectBump;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectChoose;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectBuzzer;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectExpFull;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectThrow;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectRecall;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectBallShake;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectBallDrop;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectJumpToBall;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectJump;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectComputerClose;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectNormalDamage	;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectNotVeryDamage;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectSuperDamage	;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectFaint		;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectFlash		;
		[SerializeField] private global::UnityEngine.AudioClip _soundEffectSword		;
		//[SerializeField] private global::UnityEngine.AudioClip _soundEffectPkmnCry_		;

		private AudioTrack soundEffectMenu;
		private AudioTrack soundEffectBump;
		private AudioTrack soundEffectChoose;
		private AudioTrack soundEffectBuzzer;
		private AudioTrack soundEffectExpFull;
		private AudioTrack soundEffectThrow;
		private AudioTrack soundEffectRecall;
		private AudioTrack soundEffectBallShake;
		private AudioTrack soundEffectBallDrop;
		private AudioTrack soundEffectJumpToBall;
		private AudioTrack soundEffectJump;
		private AudioTrack soundEffectComputerClose;
		private AudioTrack soundEffectNormalDamage;
		private AudioTrack soundEffectNotVeryDamage;
		private AudioTrack soundEffectSuperDamage;
		private AudioTrack soundEffectFaint;
		private AudioTrack soundEffectFlash;
		private AudioTrack soundEffectSword;
		private AudioTrack soundEffectPkmnCry_;

		public IAudioObject	SoundEffectMenu				{ get {	if (_soundEffectMenu			!= null	&&	soundEffectMenu				!= null)	soundEffectMenu				= new AudioTrack(_soundEffectMenu);				return soundEffectMenu			;	} }
		public IAudioObject	SoundEffectBump				{ get {	if (_soundEffectBump			!= null	&&	soundEffectBump				!= null)	soundEffectBump				= new AudioTrack(_soundEffectBump);				return soundEffectBump			;	} }
		public IAudioObject	SoundEffectChoose			{ get {	if (_soundEffectChoose			!= null	&&	soundEffectChoose			!= null)	soundEffectChoose			= new AudioTrack(_soundEffectChoose);			return soundEffectChoose		;	} }
		public IAudioObject	SoundEffectBuzzer			{ get {	if (_soundEffectBuzzer			!= null	&&	soundEffectBuzzer			!= null)	soundEffectBuzzer			= new AudioTrack(_soundEffectBuzzer);			return soundEffectBuzzer		;	} }
		public IAudioObject	SoundEffectExpFull			{ get {	if (_soundEffectExpFull			!= null	&&	soundEffectExpFull			!= null)	soundEffectExpFull			= new AudioTrack(_soundEffectExpFull);			return soundEffectExpFull		;	} }
		//Battle
		public IAudioObject	SoundEffectThrow			{ get {	if (_soundEffectThrow			!= null	&&	soundEffectThrow			!= null)	soundEffectThrow			= new AudioTrack(_soundEffectThrow);			return soundEffectThrow			;	} }
		public IAudioObject	SoundEffectRecall			{ get {	if (_soundEffectRecall			!= null	&&	soundEffectRecall			!= null)	soundEffectRecall			= new AudioTrack(_soundEffectRecall);			return soundEffectRecall		;	} }
		public IAudioObject	SoundEffectBallShake		{ get {	if (_soundEffectBallShake		!= null	&&	soundEffectBallShake		!= null)	soundEffectBallShake		= new AudioTrack(_soundEffectBallShake);		return soundEffectBallShake		;	} }
		public IAudioObject	SoundEffectBallDrop			{ get {	if (_soundEffectBallDrop		!= null	&&	soundEffectBallDrop			!= null)	soundEffectBallDrop			= new AudioTrack(_soundEffectBallDrop);			return soundEffectBallDrop		;	} }
		public IAudioObject	SoundEffectJumpToBall		{ get {	if (_soundEffectJumpToBall		!= null	&&	soundEffectJumpToBall		!= null)	soundEffectJumpToBall		= new AudioTrack(_soundEffectJumpToBall);		return soundEffectJumpToBall	;	} }
		public IAudioObject	SoundEffectJump				{ get {	if (_soundEffectJump			!= null	&&	soundEffectJump				!= null)	soundEffectJump				= new AudioTrack(_soundEffectJump);				return soundEffectJump			;	} }
		public IAudioObject	SoundEffectComputerClose	{ get {	if (_soundEffectComputerClose	!= null	&&	soundEffectComputerClose	!= null)	soundEffectComputerClose	= new AudioTrack(_soundEffectComputerClose);	return soundEffectComputerClose	;	} }
		public IAudioObject	SoundEffectNormalDamage		{ get {	if (_soundEffectNormalDamage	!= null	&&	soundEffectNormalDamage		!= null)	soundEffectNormalDamage		= new AudioTrack(_soundEffectNormalDamage);		return soundEffectNormalDamage	;	} }
		public IAudioObject	SoundEffectNotVeryDamage	{ get {	if (_soundEffectNotVeryDamage	!= null	&&	soundEffectNotVeryDamage	!= null)	soundEffectNotVeryDamage	= new AudioTrack(_soundEffectNotVeryDamage);	return soundEffectNotVeryDamage	;	} }
		public IAudioObject	SoundEffectSuperDamage		{ get {	if (_soundEffectSuperDamage		!= null	&&	soundEffectSuperDamage		!= null)	soundEffectSuperDamage		= new AudioTrack(_soundEffectSuperDamage);		return soundEffectSuperDamage	;	} }
		public IAudioObject	SoundEffectFaint			{ get {	if (_soundEffectFaint			!= null	&&	soundEffectFaint			!= null)	soundEffectFaint			= new AudioTrack(_soundEffectFaint);			return soundEffectFaint			;	} }
		// Versus Battle, Flashing of "VS" text
		public IAudioObject	SoundEffectFlash			{ get {	if (_soundEffectFlash			!= null	&&	soundEffectFlash			!= null)	soundEffectFlash			= new AudioTrack(_soundEffectFlash);			return soundEffectFlash			;	} }
		// Versus Battle, After flashing of "VS" text, Sword shwing sound effect
		public IAudioObject	SoundEffectSword			{ get {	if (_soundEffectSword			!= null	&&	soundEffectSword			!= null)	soundEffectSword			= new AudioTrack(_soundEffectSword);			return soundEffectSword			;	} }
		public IAudioObject	SoundEffectPkmnCry_			{ get {	if (SoundEffectPkmnCry			!= null	&&	SoundEffectPkmnCry.ContainsKey(Pokemon))	soundEffectPkmnCry_		= new AudioTrack(SoundEffectPkmnCry[Pokemon]);	return soundEffectPkmnCry_		;	} }


		//MiniGame - Fishing
		//MiniGame - Mining
		//MiniGame - Slot Machine
		//MiniGame - Tile Puzzle
		//MiniGame - Triple Triad
		//MiniGame - Voltorb Flip
		//MiniGame -
		#endregion

		#region Background Music
		[Header("Background Music")]
		[SerializeField] private global::UnityEngine.AudioClip _backgroundMusicWildBattleDefault;

		/// <summary>
		/// Some wild encounters have their own theme music
		/// </summary>
		public IAudioObject	BackgroundMusicWildBattle_			{ get; }
		public IAudioObject	BackgroundMusicWildBattleDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		public IAudioObject	BackgroundMusicTrainerBattle_		{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all trainer battles on that map
		/// </summary>
		public IAudioObject	BackgroundMusicTrainerBattleMap_		{ get; }
		public IAudioObject	BackgroundMusicTrainerBattleDefault		{ get; }
		/// <summary>
		/// Some maps have day and night versions of their theme music
		/// </summary>
		public IAudioObject	BackgroundMusicMap_			{ get; }
		public IAudioObject	BackgroundMusicMapDefault	{ get; }
		public IAudioObject	BackgroundMusicEvolve		{ get; }
		public IAudioObject	BackgroundMusicHallOfFame	{ get; }
		public IAudioObject	BackgroundMusicRadioMarch	{ get; }
		public IAudioObject	BackgroundMusicRadioOak		{ get; }
		public IAudioObject	BackgroundMusicRadioLullaby	{ get; }
		public IAudioObject	BackgroundMusicBike			{ get; }
		public IAudioObject	BackgroundMusicSurf			{ get; }
		#endregion

		#region Music Effect
		[Header("Music Effects")]
		[SerializeField] private global::UnityEngine.AudioClip _musicEffectTitle;

		/// <summary>
		/// Some wild encounters have their own theme music
		/// </summary>
		public IAudioObject	MusicEffectWildVictory_			{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all wild battles on that map
		/// </summary>
		public IAudioObject	MusicEffectWildVictoryMap_		{ get; }
		public IAudioObject	MusicEffectWildVictoryDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		public IAudioObject	MusicEffectTrainerIntro_		{ get; }
		public IAudioObject	MusicEffectTrainerIntroDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		public IAudioObject	MusicEffectTrainerVictory_		{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all trainer battles on that map
		/// </summary>
		public IAudioObject	MusicEffectTrainerVictoryMap_	{ get; }
		public IAudioObject	MusicEffectTrainerVictoryDefault{ get; }
		public IAudioObject	MusicEffectEvolutionSuccess		{ get; }
		public IAudioObject	MusicEffectJingleHMTM			{ get; }
		public IAudioObject	MusicEffectCredits				{ get; }
		public IAudioObject	MusicEffectTitle				{ get; }
		#endregion

		public global::UnityEngine.AudioClip[] BackgroundMusic;
		public global::UnityEngine.AudioClip[] BackgroundSound;
		public global::UnityEngine.AudioClip[] MusicEffect;
		public global::UnityEngine.AudioClip[] SoundEffect;
		public Dictionary<string,global::UnityEngine.AudioClip> BackgroundMusicMap;
		public Dictionary<string,global::UnityEngine.AudioClip> BackgroundMusicBattle;
		public Dictionary<Pokemons,global::UnityEngine.AudioClip> SoundEffectPkmnCry;
	}
}