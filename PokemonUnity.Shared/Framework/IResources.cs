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
		IAudioSE	SoundEffectMenu			{ get; }
		IAudioSE	SoundEffectBump			{ get; }
		IAudioSE	SoundEffectChoose		{ get; }
		IAudioSE	SoundEffectBuzzer		{ get; }
		IAudioSE	SoundEffectJump			{ get; }
		IAudioSE	SoundEffectExpFull		{ get; }
		IAudioSE	SoundEffectComputerClose{ get; }
		//Battle
		IAudioSE	SoundEffectThrow		{ get; }
		IAudioSE	SoundEffectRecall		{ get; }
		IAudioSE	SoundEffectBallShake	{ get; }
		IAudioSE	SoundEffectBallDrop		{ get; }
		IAudioSE	SoundEffectJumpToBall	{ get; }
		IAudioSE	SoundEffectNormalDamage	{ get; }
		IAudioSE	SoundEffectNotVeryDamage{ get; }
		IAudioSE	SoundEffectSuperDamage	{ get; }
		IAudioSE	SoundEffectFaint		{ get; }
		IAudioSE	SoundEffectPkmnCry_		{ get; }

		// Versus Battle, Flashing of "VS" text
		IAudioSE	SoundEffectFlash		{ get; }
		// Versus Battle, After flashing of "VS" text, Sword shwing sound effect
		IAudioSE	SoundEffectSword		{ get; }
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
		IAudioBGM	BackgroundMusicWildBattle_			{ get; }
		IAudioBGM	BackgroundMusicWildBattleDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		IAudioBGM	BackgroundMusicTrainerBattle_		{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all trainer battles on that map
		/// </summary>
		IAudioBGM	BackgroundMusicTrainerBattleMap_		{ get; }
		IAudioBGM	BackgroundMusicTrainerBattleDefault		{ get; }
		/// <summary>
		/// Some maps have day and night versions of their theme music
		/// </summary>
		IAudioBGM	BackgroundMusicMap_			{ get; }
		IAudioBGM	BackgroundMusicMapDefault	{ get; }
		IAudioBGM	BackgroundMusicEvolve		{ get; }
		IAudioBGM	BackgroundMusicHallOfFame	{ get; }
		IAudioBGM	BackgroundMusicRadioMarch	{ get; }
		IAudioBGM	BackgroundMusicRadioOak		{ get; }
		IAudioBGM	BackgroundMusicRadioLullaby	{ get; }
		IAudioBGM	BackgroundMusicBike			{ get; }
		IAudioBGM	BackgroundMusicSurf			{ get; }
		#endregion

		#region Music Effect
		/// <summary>
		/// Some wild encounters have their own theme music
		/// </summary>
		IAudioME	MusicEffectWildVictory_			{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all wild battles on that map
		/// </summary>
		IAudioME	MusicEffectWildVictoryMap_		{ get; }
		IAudioME	MusicEffectWildVictoryDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		IAudioME	MusicEffectTrainerIntro_		{ get; }
		IAudioME	MusicEffectTrainerIntroDefault	{ get; }
		/// <summary>
		/// Some trainers have their own theme music
		/// </summary>
		IAudioME	MusicEffectTrainerVictory_		{ get; }
		/// <summary>
		/// Some maps have their own theme music for any/all trainer battles on that map
		/// </summary>
		IAudioME	MusicEffectTrainerVictoryMap_	{ get; }
		IAudioME	MusicEffectTrainerVictoryDefault{ get; }
		IAudioME	MusicEffectEvolutionSuccess		{ get; }
		IAudioME	MusicEffectJingleHMTM			{ get; }
		IAudioME	MusicEffectCredits				{ get; }
		IAudioME	MusicEffectTitle				{ get; }
		#endregion
	}
}