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

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// List of all the scenes that utilized by frontend platform
	public partial class LevelLoader : MonoBehaviour, IGameScenesUI
	{
		#region Private Monobehavior Fields
		[SerializeField] private PokemonEssentials.Interface.Screen.IPokemonEntryScene textEntryScene;
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
		//[SerializeField] private PokemonEssentials.Interface.Screen. phone;
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
		public PokemonEssentials.Interface.Screen.IPokemonEntryScene TextEntryScene	{ get { return textEntryScene; } set { textEntryScene = value; } }
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
		//public PokemonEssentials.Interface.Screen. Phone	{ get { return phone; } set { phone = value; } }
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


		public PokemonEssentials.Interface.Screen.IPokeBattle_Scene BattleScene	{ get { return battleScene; } } //set { battleScene = (IPokeBattle_Scene)value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene BattleArenaScene	{ get { return battleArenaScene; } set { battleArenaScene = value; } }
		public PokemonEssentials.Interface.Screen.IBattleSwapScene BattleSwapScene	{ get { return battleSwapScene; } set { battleSwapScene = value; } }
		//public PokemonEssentials.Interface.Screen. BattlePalaceScene	{ get { return battlePalaceScene; } set { battlePalaceScene = value; } }
		public PokemonEssentials.Interface.Screen.ISafariZone_Scene BattleSafari	{ get { return battleSafari; } set { battleSafari = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene BattleSceneDebug	{ get { return battleSceneDebug; } set { battleSceneDebug = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive BattleSceneDebugNoUI	{ get { return battleSceneDebugNoUI; } set { battleSceneDebugNoUI = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging BattleSceneDebugWithoutLog	{ get { return battleSceneDebugWithoutLog; } set { battleSceneDebugWithoutLog = value; } }
		public PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics BattleSceneDebugWithoutGfx	{ get { return battleSceneDebugWithoutGfx; } set { battleSceneDebugWithoutGfx = value; } }
		#endregion
				

		public IGameScenesUI initialize (params PokemonEssentials.Interface.Screen.IScene[] scenes)
		{
			foreach (PokemonEssentials.Interface.Screen.IScene scene in scenes)
			{
				if (scene is PokemonEssentials.Interface.Screen.IPokemonEntryScene s0) TextEntryScene = s0;
				else if (scene is PokemonEssentials.Interface.Screen.IIntroEventScene s1) IntroScene = s1;
				else if (scene is PokemonEssentials.Interface.Screen.ILoadScene s2) Load = s2;
				else if (scene is PokemonEssentials.Interface.Screen.ISaveScene s3) Save = s3;
				else if (scene is PokemonEssentials.Interface.Screen.IOptionScene s4) OptionScene = s4;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonMenuScene s5) PauseMenuScene = s5;
				else if (scene is PokemonEssentials.Interface.Screen.IPartyDisplayScene s6) Party = s6;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonSummaryScene s7) Summary = s7;
				else if (scene is PokemonEssentials.Interface.Screen.IBagScene s8) Bag = s8;
				else if (scene is PokemonEssentials.Interface.Screen.IItemStorageScene s9) Bag_ItemStore = s9;
				else if (scene is PokemonEssentials.Interface.Screen.IWithdrawItemScene s10) Bag_ItemWithdraw = s10;
				else if (scene is PokemonEssentials.Interface.Screen.ITossItemScene s11) Bag_ItemToss = s11;
				else if (scene is PokemonEssentials.Interface.Screen.IPokegearScene s12) PokeGear = s12;
				else if (scene is PokemonEssentials.Interface.Screen.ITrainerCardScene s13) TrainerCard = s13;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonStorageScene s14) PokemonStorageScene = s14;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonPokedexScene s15) PokedexScene = s15;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonNestMapScene s16) PokedexNestScene = s16;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonFormScene s17) PokedexFormScene = s17;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonEvolutionScene s18) EvolvingScene = s18;
				else if (scene is PokemonEssentials.Interface.Screen.IMartScene s19) Mart = s19;
				else if (scene is PokemonEssentials.Interface.Screen.IRelicStoneScene s20) RelicStone = s20;
				else if (scene is PokemonEssentials.Interface.Screen.IPurifyChamberScene s21) PurityChamber = s21;
				//else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_Scene s22) battleScene = s22;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene s23) BattleArenaScene = s23;
				else if (scene is PokemonEssentials.Interface.Screen.IBattleSwapScene s24) BattleSwapScene = s24;
				else if (scene is PokemonEssentials.Interface.Screen.ISafariZone_Scene s25) BattleSafari = s25;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene s26) BattleSceneDebug = s26;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive s27) BattleSceneDebugNoUI = s27;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging s28) BattleSceneDebugWithoutLog = s28;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics s29) BattleSceneDebugWithoutGfx = s29;
			}
			return this;
		}
	}
}