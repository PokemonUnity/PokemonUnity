using System.Collections;
using PokemonUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnityPokemonScenes : MonoBehaviour
{
	#region Scenes
	[Header("Pokemon UI Scenes")]
	public PokemonEssentials.Interface.Screen.IPokemonEntryScene TextEntryScene;
	public PokemonEssentials.Interface.Screen.IIntroEventScene IntroScene;
	public PokemonEssentials.Interface.Screen.ILoadScene Load;
	public PokemonEssentials.Interface.Screen.ISaveScene Save;
	public PokemonEssentials.Interface.Screen.IOptionScene OptionScene;
	public PokemonEssentials.Interface.Screen.IPokemonMenuScene PauseMenuScene;
	public PokemonEssentials.Interface.Screen.IPartyDisplayScene Party;
	public PokemonEssentials.Interface.Screen.IPokemonSummaryScene Summary;
	public PokemonEssentials.Interface.Screen.IBagScene Bag;
	public PokemonEssentials.Interface.Screen.IItemStorageScene Bag_ItemStore;
	public PokemonEssentials.Interface.Screen.IWithdrawItemScene Bag_ItemWithdraw;
	public PokemonEssentials.Interface.Screen.ITossItemScene Bag_ItemToss;
	//public PokemonEssentials.Interface.Screen. RegionMap;
	//public PokemonEssentials.Interface.Screen. Phone;
	public PokemonEssentials.Interface.Screen.IPokegearScene PokeGear;
	public PokemonEssentials.Interface.Screen.ITrainerCardScene TrainerCard;
	public PokemonEssentials.Interface.Screen.IPokemonStorageScene PokemonStorageScene;
	public PokemonEssentials.Interface.Screen.IPokemonPokedexScene PokedexScene;
	public PokemonEssentials.Interface.Screen.IPokemonNestMapScene PokedexNestScene;
	public PokemonEssentials.Interface.Screen.IPokemonFormScene PokedexFormScene;
	public PokemonEssentials.Interface.Screen.IPokemonEvolutionScene EvolvingScene;
	//public PokemonEssentials.Interface.Screen. EggHatching;
	//public PokemonEssentials.Interface.Screen. Trading;
	//public PokemonEssentials.Interface.Screen. MoveRelearner;
	public PokemonEssentials.Interface.Screen.IMartScene Mart;
	//public PokemonEssentials.Interface.Screen. Debug;
	public PokemonEssentials.Interface.Screen.IRelicStoneScene RelicStone; //Shadow Pokemon
	public PokemonEssentials.Interface.Screen.IPurifyChamberScene PurityChamber; //Shadow Pokemon


	public PokemonEssentials.Interface.Screen.IPokeBattle_Scene BattleScene;
	public PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene BattleArenaScene;
	public PokemonEssentials.Interface.Screen.IBattleSwapScene BattleSwapScene;
	//public PokemonEssentials.Interface.Screen. BattlePalaceScene;
	//public PokemonEssentials.Interface.Screen. BattleSafari;
	//public PokemonEssentials.Interface.Screen.IPokeBattle_DebugScene BattleSceneDebug;
	//public PokemonEssentials.Interface.Screen.IPokeBattle_SceneNonInteractive BattleSceneDebugNoUI;
	//public PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoLogging BattleSceneDebugWithoutLog;
	//public PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics BattleSceneDebugWithoutGfx;
	#endregion

	#region Screens
	[Header("Pokemon UI Scene Windows")]
	public PokemonEssentials.Interface.Screen.IPokemonEntryScreen TextEntryScreen;
	public PokemonEssentials.Interface.Screen.IIntroEventScreen IntroScreen;
	public PokemonEssentials.Interface.Screen.ILoadScreen LoadScreen;
	public PokemonEssentials.Interface.Screen.ISaveScreen SaveScreen;
	public PokemonEssentials.Interface.Screen.IOptionScreen OptionScreen;
	public PokemonEssentials.Interface.Screen.IPokemonMenuScreen PauseMenuScreen;
	public PokemonEssentials.Interface.Screen.IPartyDisplayScreen PartyScreen;
	public PokemonEssentials.Interface.Screen.IPokemonSummaryScreen SummaryScreen;
	public PokemonEssentials.Interface.Screen.IBagScreen BagScreen;
	//public PokemonEssentials.Interface.Screen. RegionMapScreen;
	//public PokemonEssentials.Interface.Screen. PhoneScreen;
	public PokemonEssentials.Interface.Screen.ITrainerCardScreen TrainerCardScreen;
	public PokemonEssentials.Interface.Screen.IPokemonStorageScreen PokemonStorageScreen;
	public PokemonEssentials.Interface.Screen.IPokemonPokedexScreen PokedexScreen;
	public PokemonEssentials.Interface.Screen.IPokemonNestMapScreen PokedexNestScreen;
	public PokemonEssentials.Interface.Screen.IPokemonFormScreen PokedexFormScreen;
	//public PokemonEssentials.Interface.Screen. EggHatchingScreen;
	//public PokemonEssentials.Interface.Screen. TradingScreen;
	//public PokemonEssentials.Interface.Screen. MoveRelearnerScreen;
	public PokemonEssentials.Interface.Screen.IMartScreen MartScreen;
	//public PokemonEssentials.Interface.Screen. DebugScreen;
	public PokemonEssentials.Interface.Screen.IRelicStoneScreen RelicStoneScreen; //Shadow Pokemon
	//public PokemonEssentials.Interface.Screen. PurityChamberScreen; //Shadow Pokemon
	public PokemonEssentials.Interface.Screen.IBattleSwapScreen BattleSwapScreen;
	#endregion

	public PokemonEssentials.Interface.IGame Game { get; set; }
	private void Awake()
	{
		BattleScene = transform.GetComponent<BattleScene>();
		(Game as UnityGame).SetScenes(
			BattleScene
		);
		(Game as UnityGame).SetScreens();
	}
}

public partial class UnityGame : Game, PokemonEssentials.Interface.IGame
{
	
}