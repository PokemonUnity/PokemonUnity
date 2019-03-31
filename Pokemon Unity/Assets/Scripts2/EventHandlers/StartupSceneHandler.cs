using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Battle;
using PokemonUnity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;
//using UnityEngine;

public class StartupSceneHandler : UnityEngine.MonoBehaviour, UnityEngine.EventSystems.ISubmitHandler, UnityEngine.EventSystems.IScrollHandler
{
	#region Variables
	//public static GameVariables PersistantPlayerData { get; private set; }
	public UnityEngine.UI.Image DialogSkin, WindowSkin;
    private static UnityEngine.GameObject MainMenu;// = UnityEngine.GameObject.Find("MainMenu");
    /// <summary>
    /// This is the panel display that shows save data for currently selected CONTINUE option
    /// </summary>
    private static UnityEngine.GameObject FileDataPanel;// = MainMenu.transform.GetChild(0).gameObject;
    private static UnityEngine.GameObject MenuOptions;// = MainMenu.transform.GetChild(1).gameObject;
    //private static UnityEngine.UI.Text DialogUITextDump = MainMenu.GetComponent<UnityEngine.UI.Text>();
    //private static UnityEngine.UI.Text DialogUIScrollText = UnityEngine.GameObject.Find("DialogScrollText").GetComponent<UnityEngine.UI.Text>();
	//map class with headers
	//pokemon array for list of encounters on current map loaded
	#endregion

	#region Unity MonoBehavior
    void Awake()
    {
		//PersistantPlayerData = new GameVariables();
		//ToDo: On Start-up, Load & Process GameVariables, to begin and instantiate game
		GameVariables.SetStartScene(this);
        MainMenu = UnityEngine.GameObject.Find("MainMenu");
        FileDataPanel = MainMenu.transform.GetChild(0).gameObject;
        MenuOptions = MainMenu.transform.GetChild(1).gameObject;
		//ToDo: Awake Audio Components
    }
    void OnEnable()
    {
        /* If no previous saved data was found: 
         * disable the right playerData window,
         * disable continue menu-option,
         * extend menu option width size,
         * transform menu positions to collapse 
         * to top and fill in empty gap
         */
        //Load Any/All GameSaves
        //"ContinuePanel"
        MenuOptions.transform.GetChild(0).gameObject.SetActive(GameVariables.SaveFileFound);
        FileDataPanel.SetActive(GameVariables.SaveFileFound);
        if (!GameVariables.SaveFileFound)
        {
            //"MainMenu"
            //Stretch menu to fit width across
            MenuOptions.GetComponent<UnityEngine.RectTransform>().anchorMax = new UnityEngine.Vector2(1, 1);
            //Move options up to fill in gap
            MenuOptions.transform.GetChild(1).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            MenuOptions.transform.GetChild(2).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            MenuOptions.transform.GetChild(3).gameObject.transform.localPosition += new UnityEngine.Vector3(0f, 70f, 0f);
            //UnityEngine.Debug.Log(MenuOptions.transform.GetChild(1).gameObject.transform.position);
            //UnityEngine.Debug.Log(MenuOptions.transform.GetChild(1).gameObject.transform.localPosition);
            //ToDo: Git was giving build error on `ForceUpdateRectTransforms()`; says it doesnt exist...
            //Refresh the changes to display the new position
            //MenuOptions.transform.GetChild(1).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
            //MenuOptions.transform.GetChild(2).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
            //MenuOptions.transform.GetChild(3).gameObject.GetComponent<UnityEngine.RectTransform>().ForceUpdateRectTransforms();
        }
    }
    void Start()
    {
    }
    void Update()
    {
        /* Ping GameNetwork server every 15-45secs
         * If game server is offline or unable to
         * ping connection:
         * Netplay toggle-icon bool equals false
         * otherwise toggle bool to true
         */
        //StartCoroutine(PingServerEveryXsec);
        //Use coroutine that has a while loop instead of using here
        /*while (MainMenu.activeSelf)
        {
            //While scene is enabled, run coroutine to ping server
            break;
        }*/
        //int index = (int)(UnityEngine.Time.timeSinceLevelLoad * Settings.framesPerSecond);
        //index = index % sprites[].Length;
    }
	#endregion

	#region GameStart and Main Menu
	/* If Continue option is available:
     * file slot data should reflect in the 
     * playerData window on the right side;
     * disable slot options with no data
     */
	void ContinueSavedGame()
    {
        //If Continue Option is select
        if (MenuOptions.transform.GetChild(0).gameObject.GetComponent<UnityEngine.UI.Toggle>().isOn)
        {
			//Get Toggle Value from Toggle group for which toggleOption is selected
			//use gamesave toggle to load game from that slot
			GameVariables.Load();
        }
    }

    public void ChangeDataPanel(int slot)
    {
        //Refresh the panel for continue screen to reflect gamesave data
        UnityEngine.Debug.Log(slot);
    }

    public void OnScroll(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        //throw new NotImplementedException();
        switch (eventData.selectedObject.name)
        {
            //If the object is slots, submit continue
            case "":
            //If the object is continue, transistion to next scene
            case "1":
            default:
                break;
        }
    }

	/* If settings option is accessed, 
     * Use GameVariables.ChangeScene to transition
     */
	#endregion
		
	#region Methods
	//Your map class should be the one talking to unity and triggering if battles should occur

	/// <summary>
	/// Start a single wild battle
	/// </summary>
	public bool WildBattle(PokemonUnity.Monster.Pokemon pkmn, bool cantescape = true, bool canlose = false)
	{
		if (GameVariables.playerTrainer.Trainer.Party.Length == 0 || (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftControl) && GameVariables.IS_DEBUG_ACTIVE))
		{
			if (GameVariables.playerTrainer.Trainer.Party.Length > 0)
				GameVariables.DebugLog("SKIPPING BATTLE...");
			GameVariables.nextBattleBGM = null;
			GameVariables.nextBattleME = null;
			GameVariables.nextBattleBack = null;
			return true;
		}
		PokemonUnity.Monster.Pokemon[] generateWildPkmn = new PokemonUnity.Monster.Pokemon[1];
		generateWildPkmn[0] = pkmn; //new Pokemon();
		//int decision = 0;
		Battle battle =
		//GameVariables.battle =
			new Battle(
				GameVariables.playerTrainer.Trainer,
				new Trainer(generateWildPkmn)
			)
			.InternalBattle(true)
			.CantEscape(!cantescape);
			//.StartBattle(canlose); //Switch to battle scene and trigger coroutine 
			//.AfterBattle(ref decision,canlose);
		//GameVariables.battle.StartBattle(canlose);  
		IEnumerator<BattleResults> e = BattleAnimationHandler.BattleCoroutineResults;
		//while battle scene is active
		//delay results of battle
		//on battle end return the results of the battle 
		//ToDo: and save data to profile?... maybe that would be done from battle class
		return e.Current != BattleResults.LOST;
	}

	/// <summary>
	/// Start a double wild battle
	/// </summary>
	public bool DoubleWildBattle(PokemonUnity.Monster.Pokemon pkmn1, PokemonUnity.Monster.Pokemon pkmn2, bool cantescape = true, bool canlose = false)
	{
		if (GameVariables.playerTrainer.Trainer.Party.Length == 0 || (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.LeftControl) && GameVariables.IS_DEBUG_ACTIVE))
		{
			if (GameVariables.playerTrainer.Trainer.Party.Length > 0)
				GameVariables.DebugLog("SKIPPING BATTLE...");
			GameVariables.nextBattleBGM = null;
			GameVariables.nextBattleME = null;
			GameVariables.nextBattleBack = null;
			return true;
		}
		PokemonUnity.Monster.Pokemon[] generateWildPkmn = new PokemonUnity.Monster.Pokemon[] { pkmn1, pkmn2 };//new Pokemon(), new Pokemon()
		//int decision = 0;
		Battle battle =
		//GameVariables.battle =
			new Battle(
				GameVariables.playerTrainer.Trainer,
				new Trainer(generateWildPkmn) { IsDouble = true }
			)
			.InternalBattle(true)
			.CantEscape(!cantescape);
			//.StartBattle(canlose); //Switch to battle scene and trigger coroutine 
			//.AfterBattle(ref decision,canlose);
		//GameVariables.battle.StartBattle(canlose);  
		IEnumerator<BattleResults> e = BattleAnimationHandler.BattleCoroutineResults;
		//while battle scene is active
		//delay results of battle
		//on battle end return the results of the battle 
		//ToDo: and save data to profile?... maybe that would be done from battle class
		//return battle.decision;
		return (e.Current != BattleResults.LOST && e.Current != BattleResults.DRAW);
	}

	public void AfterBattle()//BattleResults decision, bool canlose
	{
		BattleResults decision = GameVariables.battle.decision;
		bool canlose = GameVariables.battle.canLose;
		for (int i = 0; i < GameVariables.playerTrainer.Trainer.Party.Length; i++)
		{
			//GameVariables.playerTrainer.Trainer.Party[i].MakeUnMega();
			//GameVariables.playerTrainer.Trainer.Party[i].MakeUnPrimal();
		}
		//if () //In a party
		//{
		//	HealAll();
		//	for (int i = 0; i < GameVariables.Partner.Trainer.Party.Length; i++)
		//	{
		//		//GameVariables.Partner.Trainer.Party[i].Heal();
		//		//GameVariables.Partner.Trainer.Party[i].MakeUnMega();
		//		//GameVariables.Partner.Trainer.Party[i].MakeUnPrimal();
		//	}
		//}
		if (decision == BattleResults.LOST || decision == BattleResults.DRAW)
		{
			if (canlose)
			{
				for (int i = 0; i < GameVariables.playerTrainer.Trainer.Party.Length; i++)
				{
					GameVariables.playerTrainer.Trainer.Party[i].Heal();
				}
			}
		}
		//yield return new WaitWhile(() => OnEndBattle);
	}

	public System.Collections.IEnumerator OnEndBattle()
	{
		BattleResults decision = GameVariables.battle.decision;
		bool canlose = GameVariables.battle.canLose;
		//if (Settings.USENEWBATTLEMECHANICS || (decision == BattleResults.LOST || decision == BattleResults.DRAW))
		//	if()
		if(decision == BattleResults.WON)
		{
			for (int pkmn = 0; pkmn < GameVariables.playerTrainer.Trainer.Party.Length; pkmn++)
			{
				if (GameVariables.playerTrainer.Trainer.Party[pkmn].hasAbility(Abilities.HONEY_GATHER) &&
					GameVariables.playerTrainer.Trainer.Party[pkmn].Item == Items.NONE)
				{
					int chance = 5 + (int)Math.Floor((GameVariables.playerTrainer.Trainer.Party[pkmn].Level - 1) / 10f) * 5;
					if (Settings.Rand.Next(100) < chance)
						//ToDo: Create class to give items to pokemon?... or maybe remove `private set`?
						continue;//GameVariables.playerTrainer.Trainer.Party[pkmn].SetItem(Items.HONEY);						
				}
			}
		}
		if ((decision == BattleResults.LOST || decision == BattleResults.DRAW) && !canlose)
		{
			//Audio.BGM.UnPause
			//Audio.BGS.UnPause
			//Redo Fight
			//StartOver(); 
		}
		return null;
	}

	public void EvolutionCheck(PokemonUnity.Monster.Pokemon[] currentlevels)
	{
		for (int i = 0; i < GameVariables.playerTrainer.Trainer.Party.Length; i++)
		{
			PokemonUnity.Monster.Pokemon pokemon = GameVariables.playerTrainer.Trainer.Party[i];
			if (pokemon.HP == 0 && !Settings.USENEWBATTLEMECHANICS) continue;
			if(pokemon.Species != Pokemons.NONE &&
				(currentlevels[i].Species == Pokemons.NONE
			//|| pokemon.Level != currentlevels[i]
			))
			{
				//newSpecies = CheckEvolution(pokemon);
				//if (newSpecies > 0)
				//{
				//	//evo = new PokemonEvolutionScene();
				//	//evo.StartScreen(pokemon, newSpecies)
				//	//evo.Evolution();
				//	//evo.EndScreen();
				//}
			}
		}
	}

	/// <summary>
	/// Runs the Pickup event after a battle if a Pokemon has the ability Pickup.
	/// </summary>
	public void Pickup(PokemonUnity.Monster.Pokemon pokemon)
	{
		if (pokemon.Ability == Abilities.PICKUP || pokemon.isEgg) return;
		if (pokemon.Item != Items.NONE) return;
		if (Settings.Rand.Next(10) != 0) return;
		Items[] pickupList = new Items[]
		{
			Items.POTION,
			Items.ANTIDOTE,
			Items.SUPER_POTION,
			Items.GREAT_BALL,
			Items.REPEL,
			Items.ESCAPE_ROPE,
			Items.FULL_HEAL,
			Items.HYPER_POTION,
			Items.ULTRA_BALL,
			Items.REVIVE,
			Items.RARE_CANDY,
			Items.SUN_STONE,
			Items.MOON_STONE,
			Items.HEART_SCALE,
			Items.FULL_RESTORE,
			Items.MAX_REVIVE,
			Items.PP_UP,
			Items.MAX_ELIXIR
		};
		Items[] pickupListRare = new Items[]
		{
			Items.HYPER_POTION,
			Items.NUGGET,
			Items.KINGS_ROCK,
			Items.FULL_RESTORE,
			Items.ETHER,
			Items.IRON_BALL,
			Items.DESTINY_KNOT,
			Items.ELIXIR,
			Items.DESTINY_KNOT,
			Items.LEFTOVERS,
			Items.DESTINY_KNOT
		};
		if (pickupList.Length < 18) return;
		if (pickupListRare.Length < 11) return;
		int[] randlist = new int[] { 30, 10, 10, 10, 10, 10, 10, 4, 4, 1, 1 };
		List<Items> items = new List<Items>();
		int plevel = Math.Min(100, pokemon.Level);
		int itemstart = (plevel - 1) / 10;
		if (itemstart < 0) itemstart = 0;
		for (int i = 0; i < 9; i++)
		{
			items.Add(pickupList[itemstart + i]);
		}
		for (int i = 0; i < 2; i++)
		{
			items.Add(pickupListRare[itemstart + i]);
		}
		int rand = Settings.Rand.Next(100);
		int cumnumber = 0;
		for (int i = 0; i < randlist.Length; i++)
		{
			cumnumber += randlist[i];
			if (rand < cumnumber)
			{
				//ToDo: Uncomment after creating SetItem()
				//pokemon.SetItem(items[i]);
				break;
			}
		}
	}
	#endregion
}
