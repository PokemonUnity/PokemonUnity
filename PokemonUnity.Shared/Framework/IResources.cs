using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// Resource file used for tracking assets for the game
	/// </summary>
	/// <remarks>
	/// If it ends with `_` then it's a placeholder for a specific instance of the object, use a parameter to get the specific instance
	/// </remarks>
	public interface IResourcesAudio
	{
		#region Sound Effects
		IAudioObject	SoundEffectMenu			{ get; }
		IAudioObject	SoundEffectBump			{ get; }
		IAudioObject	SoundEffectChoose		{ get; }
		IAudioObject	SoundEffectBuzzer		{ get; }
		IAudioObject	SoundEffectJump			{ get; }
		IAudioObject	SoundEffectExpFull		{ get; }
		IAudioObject	SoundEffectComputerClose{ get; }
		//Battle
		IAudioObject	SoundEffectThrow		{ get; }
		IAudioObject	SoundEffectRecall		{ get; }
		IAudioObject	SoundEffectBallShake	{ get; }
		IAudioObject	SoundEffectBallDrop		{ get; }
		IAudioObject	SoundEffectJumpToBall	{ get; }
		IAudioObject	SoundEffectNormalDamage	{ get; }
		IAudioObject	SoundEffectNotVeryDamage{ get; }
		IAudioObject	SoundEffectSuperDamage	{ get; }
		IAudioObject	SoundEffectFaint		{ get; }
		IAudioObject	SoundEffectPkmnCry_		{ get; }

		// Versus Battle, Flashing of "VS" text
		IAudioObject	SoundEffectFlash		{ get; }
		// Versus Battle, After flashing of "VS" text, Sword shwing sound effect
		IAudioObject	SoundEffectSword		{ get; }
		//MiniGame - Fishing
		//MiniGame - Mining
		//MiniGame - Slot Machine
		//MiniGame - Tile Puzzle
		//MiniGame - Triple Triad
		//MiniGame - Voltorb Flip
		//MiniGame -
		#endregion

		#region Background Music
		/// <summary>
		/// Some wild encounters have their own theme music
		/// </summary>
		IAudioObject	BackgroundMusicWildBattle_			{ get; }
		IAudioObject	BackgroundMusicWildBattleDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		IAudioObject	BackgroundMusicTrainerBattle_		{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all trainer battles on that map
		/// </summary>
		IAudioObject	BackgroundMusicTrainerBattleMap_		{ get; }
		IAudioObject	BackgroundMusicTrainerBattleDefault		{ get; }
		/// <summary>
		/// Some maps have day and night versions of their theme music
		/// </summary>
		IAudioObject	BackgroundMusicMap_			{ get; }
		IAudioObject	BackgroundMusicMapDefault	{ get; }
		IAudioObject	BackgroundMusicEvolve		{ get; }
		IAudioObject	BackgroundMusicHallOfFame	{ get; }
		IAudioObject	BackgroundMusicRadioMarch	{ get; }
		IAudioObject	BackgroundMusicRadioOak		{ get; }
		IAudioObject	BackgroundMusicRadioLullaby	{ get; }
		IAudioObject	BackgroundMusicBike			{ get; }
		IAudioObject	BackgroundMusicSurf			{ get; }
		#endregion

		#region Music Effect
		/// <summary>
		/// Some wild encounters have their own theme music
		/// </summary>
		IAudioObject	MusicEffectWildVictory_			{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all wild battles on that map
		/// </summary>
		IAudioObject	MusicEffectWildVictoryMap_		{ get; }
		IAudioObject	MusicEffectWildVictoryDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		IAudioObject	MusicEffectTrainerIntro_		{ get; }
		IAudioObject	MusicEffectTrainerIntroDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		IAudioObject	MusicEffectTrainerVictory_		{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all trainer battles on that map
		/// </summary>
		IAudioObject	MusicEffectTrainerVictoryMap_	{ get; }
		IAudioObject	MusicEffectTrainerVictoryDefault{ get; }
		IAudioObject	MusicEffectEvolutionSuccess		{ get; }
		IAudioObject	MusicEffectJingleHMTM			{ get; }
		IAudioObject	MusicEffectCredits				{ get; }
		IAudioObject	MusicEffectTitle				{ get; }
		#endregion
	}
}