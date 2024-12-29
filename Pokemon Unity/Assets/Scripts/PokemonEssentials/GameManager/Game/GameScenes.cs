using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// List of all the scenes that utilized by frontend platform
	/// </summary>
	/// <remarks>
	/// When the unity scenes are instantiated, they are assigned here,
	/// so that they can be referenced as a global singleton instance
	/// </remarks>
	public partial class LevelLoader : MonoBehaviour, IGameScenesUI
	{
		#region Private Monobehavior Fields
		[SerializeField] private TypingForm textEntryScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IIntroEventScene introScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.ILoadScene load;
		[SerializeField] private PokemonEssentials.Interface.Screen.ISaveScene save;
		[SerializeField] private PokemonEssentials.Interface.Screen.IOptionScene optionScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonMenuScene pauseMenuScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPartyDisplayScene party;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonSummaryScene summary;
		[SerializeField] private PokemonEssentials.Interface.Screen.IBagScene bag;
		[SerializeField] private PokemonEssentials.Interface.Screen.IItemStorageScene bag_ItemStore;
		[SerializeField] private PokemonEssentials.Interface.Screen.IWithdrawItemScene bag_ItemWithdraw;
		[SerializeField] private PokemonEssentials.Interface.Screen.ITossItemScene bag_ItemToss;
		//[SerializeField] private PokemonEssentials.Interface.Screen. regionMap;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonPhoneScene phone;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokegearScene pokeGear;
		[SerializeField] private PokemonEssentials.Interface.Screen.ITrainerCardScene trainerCard;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonStorageScene pokemonStorageScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonPokedexScene pokedexScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonNestMapScene pokedexNestScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonFormScene pokedexFormScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonEvolutionScene evolvingScene;
		//[SerializeField] private PokemonEssentials.Interface.Screen. eggHatching;
		//[SerializeField] private PokemonEssentials.Interface.Screen. trading;
		//[SerializeField] private PokemonEssentials.Interface.Screen. moveRelearner;
		[SerializeField] private PokemonEssentials.Interface.Screen.IMartScene mart;
		//[SerializeField] private PokemonEssentials.Interface.Screen. debug;
		[SerializeField] private PokemonEssentials.Interface.Screen.IRelicStoneScene relicStone; //Shadow Pokemon
		[SerializeField] private PokemonEssentials.Interface.Screen.IPurifyChamberScene purityChamber; //Shadow Pokemon


		[SerializeField] private BattleScene battleScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene battleArenaScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.IBattleSwapScene battleSwapScene;
		//[SerializeField] private PokemonEssentials.Interface.Screen. battlePalaceScene;
		[SerializeField] private PokemonEssentials.Interface.Screen.ISafariZone_Scene battleSafari;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene battleSceneDebug;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive battleSceneDebugNoUI;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging battleSceneDebugWithoutLog;
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics battleSceneDebugWithoutGfx;
		#endregion

		#region Public Interface Property
		public PokemonEssentials.Interface.Screen.IPokemonEntryScene TextEntryScene	{ get { return textEntryScene; } set { textEntryScene = (TypingForm)value; } }
		public PokemonEssentials.Interface.Screen.IIntroEventScene IntroScene	{ get { return introScene; } set { introScene = value; } }
		public PokemonEssentials.Interface.Screen.ILoadScene Load	{ get { return load; } set { load = value; } }
		public PokemonEssentials.Interface.Screen.ISaveScene Save	{ get { return save; } set { save = value; } }
		public PokemonEssentials.Interface.Screen.IOptionScene OptionScene	{ get { return optionScene; } set { optionScene = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonMenuScene PauseMenuScene	{ get { return pauseMenuScene; } set { pauseMenuScene = value; } }
		public PokemonEssentials.Interface.Screen.IPartyDisplayScene Party	{ get { return party; } set { party = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonSummaryScene Summary	{ get { return summary; } set { summary = value; } }
		public PokemonEssentials.Interface.Screen.IBagScene Bag	{ get { return bag; } set { bag = value; } }
		public PokemonEssentials.Interface.Screen.IItemStorageScene Bag_ItemStore	{ get { return bag_ItemStore; } set { bag_ItemStore = value; } }
		public PokemonEssentials.Interface.Screen.IWithdrawItemScene Bag_ItemWithdraw	{ get { return bag_ItemWithdraw; } set { bag_ItemWithdraw = value; } }
		public PokemonEssentials.Interface.Screen.ITossItemScene Bag_ItemToss	{ get { return bag_ItemToss; } set { bag_ItemToss = value; } }
		//public PokemonEssentials.Interface.Screen. RegionMap	{ get { return regionMap; } set { regionMap = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonPhoneScene Phone	{ get { return phone; } set { phone = value; } }
		public PokemonEssentials.Interface.Screen.IPokegearScene PokeGear	{ get { return pokeGear; } set { pokeGear = value; } }
		public PokemonEssentials.Interface.Screen.ITrainerCardScene TrainerCard	{ get { return trainerCard; } set { trainerCard = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonStorageScene PokemonStorageScene	{ get { return pokemonStorageScene; } set { pokemonStorageScene = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonPokedexScene PokedexScene	{ get { return pokedexScene; } set { pokedexScene = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonNestMapScene PokedexNestScene	{ get { return pokedexNestScene; } set { pokedexNestScene = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonFormScene PokedexFormScene	{ get { return pokedexFormScene; } set { pokedexFormScene = value; } }
		public PokemonEssentials.Interface.Screen.IPokemonEvolutionScene EvolvingScene	{ get { return evolvingScene; } set { evolvingScene = value; } }
		//public PokemonEssentials.Interface.Screen. EggHatching	{ get { return eggHatching; } set { eggHatching = value; } }
		//public PokemonEssentials.Interface.Screen. Trading	{ get { return trading; } set { trading = value; } }
		//public PokemonEssentials.Interface.Screen. MoveRelearner	{ get { return moveRelearner; } set { moveRelearner = value; } }
		public PokemonEssentials.Interface.Screen.IMartScene Mart	{ get { return mart; } set { mart = value; } }
		//public PokemonEssentials.Interface.Screen. Debug	{ get { return debug; } set { debug = value; } }
		public PokemonEssentials.Interface.Screen.IRelicStoneScene RelicStone	{ get { return relicStone; } set { relicStone = value; } } //Shadow Pokemon
		public PokemonEssentials.Interface.Screen.IPurifyChamberScene PurityChamber	{ get { return purityChamber; } set { purityChamber = value; } } //Shadow Pokemon

		// Battle Scenes
		public PokemonEssentials.Interface.Screen.IPokeBattle_Scene BattleScene	{ get { return battleScene; } set { battleScene = (BattleScene)value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene BattleArenaScene	{ get { return battleArenaScene; } set { battleArenaScene = value; } }
		public PokemonEssentials.Interface.Screen.IBattleSwapScene BattleSwapScene	{ get { return battleSwapScene; } set { battleSwapScene = value; } }
		//public PokemonEssentials.Interface.Screen. BattlePalaceScene	{ get { return battlePalaceScene; } set { battlePalaceScene = value; } }
		public PokemonEssentials.Interface.Screen.ISafariZone_Scene BattleSafari	{ get { return battleSafari; } set { battleSafari = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene BattleSceneDebug	{ get { return battleSceneDebug; } set { battleSceneDebug = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive BattleSceneDebugNoUI	{ get { return battleSceneDebugNoUI; } set { battleSceneDebugNoUI = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging BattleSceneDebugWithoutLog	{ get { return battleSceneDebugWithoutLog; } set { battleSceneDebugWithoutLog = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics BattleSceneDebugWithoutGfx	{ get { return battleSceneDebugWithoutGfx; } set { battleSceneDebugWithoutGfx = value; } }
		#endregion

		/// <summary>
		/// </summary>
		/// <param name="scenes"></param>
		/// <returns></returns>
		/// This gets broken the more reliant on unity inspector the startup becomes...
		[System.Obsolete("Maybe better to instantiate scene and destroy object, than to persist in Manager")]
		public IGameScenesUI initialize (params PokemonEssentials.Interface.Screen.IScene[] scenes)
		{
			sceneMapping.Clear();
			foreach (PokemonEssentials.Interface.Screen.IScene scene in scenes)
			{
				if (scene is PokemonEssentials.Interface.Screen.IIntroEventScene s1) { IntroScene = s1; sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IIntroEventScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonEntryScene s0) { TextEntryScene = s0; sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonEntryScene), Scenes.TextEntry); }
				else if (scene is PokemonEssentials.Interface.Screen.ILoadScene s2) { Load = s2; sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.ILoadScene), Scenes.Load); }
				else if (scene is PokemonEssentials.Interface.Screen.ISaveScene s3) { Save = s3; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.ISaveScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IOptionScene s4) { OptionScene = s4; sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IOptionScene), Scenes.Option); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonMenuScene s5) { PauseMenuScene = s5; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonMenuScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPartyDisplayScene s6) { Party = s6; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPartyDisplayScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonSummaryScene s7) { Summary = s7; sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonSummaryScene), Scenes.Summary); }
				else if (scene is PokemonEssentials.Interface.Screen.IBagScene s8) { Bag = s8; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IBagScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IItemStorageScene s9) { Bag_ItemStore = s9; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IItemStorageScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IWithdrawItemScene s10) { Bag_ItemWithdraw = s10; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IWithdrawItemScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.ITossItemScene s11) { Bag_ItemToss = s11; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.ITossItemScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokegearScene s12) { PokeGear = s12; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokegearScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.ITrainerCardScene s13) { TrainerCard = s13; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.ITrainerCardScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonStorageScene s14) { PokemonStorageScene = s14; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonStorageScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonPokedexScene s15) { PokedexScene = s15; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonPokedexScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonNestMapScene s16) { PokedexNestScene = s16; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonNestMapScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonFormScene s17) { PokedexFormScene = s17; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonFormScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonEvolutionScene s18) { EvolvingScene = s18; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokemonEvolutionScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IMartScene s19) { Mart = s19; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IMartScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IRelicStoneScene s20) { RelicStone = s20; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IRelicStoneScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPurifyChamberScene s21) { PurityChamber = s21; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPurifyChamberScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_Scene s22) { BattleScene = s22; sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokeBattle_Scene), Scenes.Battle); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene s23) { BattleArenaScene = s23; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IBattleSwapScene s24) { BattleSwapScene = s24; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IBattleSwapScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.ISafariZone_Scene s25) { BattleSafari = s25; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.ISafariZone_Scene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene s26) { BattleSceneDebug = s26; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive s27) { BattleSceneDebugNoUI = s27; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging s28) { BattleSceneDebugWithoutLog = s28; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging), Scenes.Intro); }
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics s29) { BattleSceneDebugWithoutGfx = s29; } //sceneMapping.Add(typeof(PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics), Scenes.Intro); }
			}
			return this;
		}
	}

	/// <summary>
	/// Used to load scenes in the game. Assign once here, and use to match Unity's scene manager.
	/// </summary>
	/// <remarks>
	/// One display screen for every game scene in the pokemon framework.
	/// </remarks>
	public enum Scenes
	{
		None = -1,
		/// <summary>
		/// First scene to load when game boots up.
		/// Displays logos and intro credits/sponsors
		/// </summary>
		/// <remarks>
		/// <see cref="PokemonEssentials.Interface.Screen.IIntroEventScreen"/>
		/// </remarks>
		Intro = 0, //Only need to assign the first scene, the rest will be in order of the enum
		/// <summary>
		/// Scene after title card that displays that displays the main menu,
		/// and contains a list of save files and allows the player to choose one to load.
		/// </summary>
		/// <remarks>
		/// <see cref="PokemonEssentials.Interface.Screen.ILoadScreen"/>
		/// </remarks>
		Load,
		/// <summary>
		/// Scene to change game settings.
		/// </summary>
		/// <remarks>
		/// <see cref="PokemonEssentials.Interface.Screen.IOptionScreen"/>
		/// </remarks>
		Option,
		/// <summary>
		/// Used to display the summary scene, and overview stats for a Pokémon.
		/// </summary>
		/// <remarks>
		/// <see cref="PokemonEssentials.Interface.Screen.IPokemonSummaryScreen"/>
		/// </remarks>
		Summary,
		/// <summary>
		/// Scene used for entering text, such as naming a Pokémon or a box.
		/// </summary>
		/// <remarks>
		/// <see cref="PokemonEssentials.Interface.Screen.IPokemonEntryScreen"/>
		/// </remarks>
		TextEntry,
		/// <summary>
		/// Scene used for a regulat pokemon battle
		/// </summary>
		/// <see cref="PokemonEssentials.Interface.Screen.IPokeBattle_Scene"/>
		Battle
		// "MainMenu"
		// "Gameplay"
		// "Pokedex"
		// "Bag"
		// "Options"
		// "Save"
		// "Load"
		// "Exit"
	}
}