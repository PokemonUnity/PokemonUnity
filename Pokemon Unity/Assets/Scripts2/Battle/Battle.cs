using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Item;
using PokemonUnity.Move;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// </summary>
/// ToDo: Create a battle namespace?
public class Battle : UnityUtilityIntegration
{
	#region Variables
	/// <summary>
	/// Scene object for this battle
	/// </summary>
	//public void Scene (int scene) { this.scene = scene; }
	public int scene { get; private set; }
	/// <summary>
	/// Decision: 0=undecided; 1=win; 2=loss; 3=escaped; 4=caught
	/// </summary>
	public BattleResults decision { get; private set; }
	/// <summary>
	/// Internal battle flag
	/// </summary>
	public Battle InternalBattle (bool internalBattle) { internalbattle = internalBattle; return this; }
	private bool internalbattle { get; set; }
	/// <summary>
	/// Double battle flag
	/// </summary>
	public bool doublebattle { get; private set; }
	public bool isDoubleBattleAllowed { get
		{
			/*if (!fullparty1 && party1.Length > Settings.MAXPARTYSIZE) return false;

			if (!fullparty2 && party2.Length > Settings.MAXPARTYSIZE) return false;

			//_opponent = opponent;
			//_player = player;

			// Wild battle
			if (!_opponent)
			{
				if (party2.Length == 1)
					return false;
				else if (party2.Length == 2)
					return true;
				else
					return false;
			}
			// Trainer battle
			else
			{
				if (_opponent.is_a ? (Array))
				{
					if (_opponent.Length == 1)
						_opponent = _opponent[0];
					else if (_opponent.Length != 2)
						return false;
				}

				//_player = _player

				if (_player.is_a ? (Array))
				{
					if (_player.Length == 1)
						_player = _player[0];
					else if (_player.Length != 2)
						return false;
				}

				if (_opponent.is_a ? (Array))
				{
					sendout1 = pbFindNextUnfainted(party2, 0, pbSecondPartyBegin(1));
					sendout2 = pbFindNextUnfainted(party2, pbSecondPartyBegin(1));
					if (sendout1 < 0 || sendout2 < 0) return false;
				}
				else
				{
					sendout1 = pbFindNextUnfainted(party2, 0);
					sendout2 = pbFindNextUnfainted(party2, sendout1 + 1);
					if (sendout1 < 0 || sendout2 < 0) return false;
				}
			}

			if (_player.is_a ? (Array))
			{
				sendout1 = pbFindNextUnfainted(party1, 0, pbSecondPartyBegin(0));
				sendout2 = pbFindNextUnfainted(party1, pbSecondPartyBegin(0));
				if (sendout1 < 0 || sendout2 < 0) return false;
			}
			else
			{
				sendout1 = pbFindNextUnfainted(party1, 0);
				sendout2 = pbFindNextUnfainted(party1, sendout1 + 1);
				if (sendout1 < 0 || sendout2 < 0) return false;
			}*/
			return true;
		} }
	/// <summary>
	/// True if player can't escape
	/// </summary>
	public Battle CantEscape (bool cantEscape = true) { cantescape = cantEscape; return this; }
	private bool cantescape { get; set; }
	/// <summary>
	/// If game cannot progress UNLESS the player is victor of match.
	/// False if there are no consequences to player's defeat.
	/// </summary>
	/// (Ground Hogs day anyone?)
	public Battle CanLose (bool canlose = true) { canLose = canlose; return this; }
	public bool canLose { get; private set; }
	/// <summary>
	/// Shift/Set "battle style" option
	/// </summary>
	public bool shiftStyle { get; private set; }
	/// <summary>
	/// "Battle scene" option
	/// </summary>
	public bool battlescene { get; private set; }
	/// <summary>
	/// Debug flag
	/// </summary>
	public bool debug { get; private set; }
	/// <summary>
	/// Player trainer
	/// </summary>
	public Trainer player { get; private set; }
	/// <summary>
	/// Opponent trainer
	/// </summary>
	/// If wild encounter...
	/// Dont show pokeballs,
	/// Dont show trainer sprite
	public Trainer opponent { get; private set; }
	/// <summary>
	/// Player's Pokémon party
	/// </summary>
	public Battler[] party1 { get; private set; }
	/// <summary>
	/// Foe's Pokémon party
	/// </summary>
	public Battler[] party2 { get; private set; }
	/// <summary>
	/// Pokémon party for All Trainers in Battle.
	/// Array[4,6] = 0: Player, 1: Foe, 2: Ally, 3: Foe's Ally 
	/// </summary>
	public Battler[,] party { get; private set; }
	/// <summary>
	/// Order of Pokémon in the player's party
	/// </summary>
	public List<int> party1order { get; private set; }
	/// <summary>
	/// Order of Pokémon in the opponent's party
	/// </summary>
	public List<int> party2order { get; private set; }
	/// <summary>
	/// True if player's party's max size is 6 instead of 3
	/// </summary>
	public bool fullparty1 { get; private set; }
	/// <summary>
	/// True if opponent's party's max size is 6 instead of 3
	/// </summary>
	public bool fullparty2 { get; private set; }
	/// <summary>
	/// Currently active Pokémon
	/// </summary>
	public Battler[] battlers { get; private set; }
	/// <summary>
	/// Items held by opponents
	/// </summary>
	public List<Items> items { get; private set; }
	/// <summary>
	/// Effects common to each side of a battle
	/// </summary>
	/// public List<SideEffects> sides { get; private set; }
	public Effects.Side[] sides { get; private set; }
	/// <summary>
	/// Effects common to the whole of a battle
	/// </summary>
	/// static
	/// public List<FieldEffects> field { get; private set; }
	public Effects.Field field { get; private set; }
	/// <summary>
	/// Battle surroundings;
	/// Environment node is used for background animation, 
	/// that's displayed behind the floor tile
	/// </summary>
	/// ToDo: Should be enum value
	/// ToDo: Might be a static get value from global/map variables.
	//public void Environment (PokemonUnity.Environment environment) { this.environment = environment; }
	public PokemonUnity.Environment environment { get; set; }
	public Weather weather { get; set; }
	/// <summary>
	/// Current weather, custom methods should use <see cref="SetWeather"/>  instead
	/// </summary>
	public Weather Weather { get
		{
			for (int i = 0; i < battlers.Length; i++)
			{
				if (battlers[i].Ability == Abilities.CLOUD_NINE ||
					battlers[i].Ability == Abilities.AIR_LOCK)
					return 0;
			}
			return weather;
		}
	}
	public void SetWeather (Weather weather) { this.weather = weather; }
	/// <summary>
	/// Duration of current weather, or -1 if indefinite
	/// </summary>
	public int weatherduration { get; set; }
	/// <summary>
	/// True if during the switching phase of the round
	/// </summary>
	public bool switching { get; private set; }
	/// <summary>
	/// True if Future Sight is hitting
	/// </summary>
	public bool futuresight { get; private set; }
	/// <summary>
	/// The Struggle move
	/// </summary>
	public InBattleMove struggle { get; private set; }
	/// <summary>
	/// Choices made by each Pokémon this round
	/// </summary>
	public Choice[] choices { get; private set; }
	/// <summary>
	/// Success states
	/// </summary>
	public SuccessState[] successStates { get; private set; }
	/// <summary>
	/// Last move used
	/// </summary>
	public Moves lastMoveUsed { get; private set; }
	/// <summary>
	/// Last move user
	/// </summary>
	public int lastMoveUser { get; private set; }
	/// <summary>
	/// Battle index of each trainer's Pokémon to Mega Evolve
	/// </summary>
	public sbyte[] megaEvolution { get; private set; }
	/// <summary>
	/// Whether Amulet Coin's effect applies
	/// </summary>
	public bool amuletcoin { get; private set; }
	/// <summary>
	/// Money gained in battle by using Pay Day
	/// </summary>
	public int extramoney { get; private set; }
	/// <summary>
	/// Whether Happy Hour's effect applies
	/// </summary>
	public bool doublemoney { get; set; }
	/// <summary>
	/// Speech by opponent when player wins
	/// </summary>
	public string endspeech { get { return opponent.ScriptBattleEnd; } }
	/// <summary>
	/// Speech by opponent when player wins
	/// </summary>
	public string endspeech2 { get; private set; }
	/// <summary>
	/// Speech by opponent when opponent wins
	/// </summary>
	public string endspeechwin { get; private set; }
	/// <summary>
	/// Speech by opponent when opponent wins
	/// </summary>
	public string endspeechwin2 { get; private set; }
	/// <summary>
	/// 
	/// </summary>
	public List<string> rules { get; private set; }
	/// <summary>
	/// Counter to track number of turns for battle
	/// </summary>
	public byte turncount { get; private set; }
	public int[] priority { get; private set; }
	public List<byte> snaggedpokemon { get; private set; }
	/// <summary>
	/// Each time you use the option to flee, the counter goes up.
	/// </summary>
	public byte runCommand { get; private set; }
	/// <summary>
	/// Another counter that has something to do with tracking items picked up during a battle
	/// </summary>
	public byte nextPickupUse { get; private set; }
	//attr_accessor :controlPlayer
	private Player Player { get; set; }

	public BattleAnimationHandler BattleScene { get; private set; }
	new public string Display
	{
		/* Don't need a get, since value is not being stored/logged
		 * there's no method or caller that's going to fetch value
		get
		{
			return display;
		}*/
		set
		{
			//on set
			//display = value;
			//call unity engine (public static) dialog window
			//DialogEventHandler.Display(value);
		}
	}
	//private string display { get; set; }
	/* ToDo: Move to Battler? => Display on PokemonUI 
	/// <summary>
	/// </summary>
	/// ToDo: Might need to be a method
	public bool Seen {
		get
		{
			return (int)species > 0 && player.playerPokedex[(int)species].HasValue ? true : false;
		}
		set
		{
			if ((int)species > 0 && !player.playerPokedex[(int)species].HasValue) player.playerPokedex[(int)species] = false; 
		}
	}
	/// <summary>
	/// </summary>
	/// ToDo: Might need to be a method
	public bool Owned {
		get
		{
			return (int)species > 0 && player.playerPokedex[(int)species].HasValue ? player.playerPokedex[(int)species].Value == true : false;
		}
		set
		{
			if ((int)species > 0) player.playerPokedex[(int)species] = true; 
		}
	}*/
	#endregion

	/// <summary>
	/// Uses current battle and manipulates the data then return the current battle with updated values.
	/// </summary>
	/// Pokemon variable should use the pokemon trainerid as hashvalue, or an int of where pokemon is on battle lineup
	//public Func<Battle, Pokemon, Move, Battle> Func { get; set; }
	//public Action<Battle, Pokemon, Move> Action { get; set; }
	//public //async Task<Battle> 
	//	void GenerateBattleTurn(System.Linq.Expressions.Expression<Func<Battle, Pokemon, Move, Battle>> predicate)
	//{
	//	//return await _subscriptionPaymentRepository.GetAll().LastOrDefaultAsync(predicate);
	//}

	#region Constructor
	/// <summary>
	/// 
	/// </summary>
	/// ToDo: Make a constructor to pass P1/P2 variables
	/// Cant have a battle without first establishing who you're battling
	public Battle(Trainer player, Trainer opponent)
	{
		Pokemon[] p1 = player.Party;
		Pokemon[] p2 = opponent.Party;
		if (p1.Length == 0) {
			//raise ArgumentError.new(_INTL("Party 1 has no Pokémon."))
			GameVariables.DebugLog("Party 1 has no Pokémon.", true);
			return;
		}
		
		if (p2.Length == 0) { 
			//raise ArgumentError.new(_INTL("Party 2 has no Pokémon."))
			GameVariables.DebugLog("Party 2 has no Pokémon.", true);
			return;
		}
		
		if (p2.Length > 2 && opponent.ID == TrainerTypes.WildPokemon) { //opponent != null
			//raise ArgumentError.new(_INTL("Wild battles with more than two Pokémon are not allowed."))
			GameVariables.DebugLog("Wild battles with more than two Pokémon are not allowed.", true);
			return;
		}

		//scene = scene;
		decision = 0;
		internalbattle = true;
		doublebattle = false;
		cantescape = false;
		shiftStyle = true;
		battlescene = true;
		debug = false;
		//debugupdate = 0;

		//if (opponent && player.is_a ? (Array) && player.Party.Length == 0)
		//	player = player[0];

		//if (opponent && opponent.is_a ? (Array) && opponent.Length == 0)
		//	opponent = opponent[0];

		this.player = player;
		this.opponent = opponent;
		party1 = Battler.GetBattlers(p1);
		party2 = Battler.GetBattlers(p2);

		party1order = new List<int>();

		//the #12 represents a double battle with 2 trainers using 6 pokemons on 1 side
		/*for (int i = 0; i < 12; i++)
		{
			party1order.Add(i);
		}

		party2order = new List<int>();

		//for i in 0...12;party2order.push(i); }
		for (int i = 0; i < 12; i++)
		{
			party2order.Add(i);
		}*/

		fullparty1 = false;

		fullparty2 = false;

		battlers = new Battler[4];

		items = new List<Items>(); //null;

		//sides = [PokeBattle_ActiveSide.new,				// Player's side
		//                   PokeBattle_ActiveSide.new]		// Foe's side
		sides = new Effects.Side[4];						//ToDo: Not sure if it's 2 sides, or 4 sides (foreach pokemon)
		field = new Effects.Field();                        // Whole field (gravity/rooms)
		environment = PokemonUnity.Environment.None;		// e.g. Tall grass, cave, still water
		weather = 0;

		weatherduration = 0;

		switching = false;

		futuresight = false;

		choices = new Choice[4];

		successStates = new SuccessState[4];

		for (int i = 0; i < 4; i++)
		{
			successStates[i] = new SuccessState();
		}

		lastMoveUsed = Moves.NONE;

		lastMoveUser = -1;

		nextPickupUse = 0;

		//megaEvolution = [] //list, [2,], or [2][]...
		//if player.is_a ? (Array) //ToDo: single/double or party?
		//	megaEvolution[0] =[-1] * player.Length;
		//else
		//	megaEvolution[0] =[-1];
		//if opponent.is_a ? (Array)
		//	megaEvolution[1] =[-1] * opponent.Length;
		//else
		//	megaEvolution[1] =[-1];

		amuletcoin = false;

		extramoney = 0;

		doublemoney = false;

		//endspeech = opponent.ScriptBattleEnd;
		//
		//endspeech2 = "";
		//
		//endspeechwin = "";
		//
		//endspeechwin2 = "";

		rules = new List<string>() { };
		turncount = 0;

		//peer = PokeBattle_BattlePeer.create()

		priority = new int[4];

		//usepriority = false

		snaggedpokemon = new List<byte>();

		runCommand = 0;

		if (Moves.STRUGGLE.GetType() == typeof(Moves))
			struggle = new InBattleMove(Moves.STRUGGLE);//PokeBattle_Move.pbFromPBMove(Moves.STRUGGLE);
		else
			struggle = null;//PokeBattle_Struggle.new(self, nil)

		//struggle.PP = -1;

		for (byte i = 0; i < 4; i++)
		{
			battlers[i] = new Battler().Initialize(new Pokemon(), (sbyte)i);
		}

		foreach (var i in party1)
		{
			if (i.Species == Pokemons.NONE) continue;

			i.itemRecycle = 0;

			i.itemInitial = i.Item;

			i.belch = false;
		}

		foreach (var i in party2)
		{
			if (i.Species == Pokemons.NONE) continue;

			i.itemRecycle = 0;

			i.itemInitial = i.Item;

			i.belch = false;
		}
	}
	public Battle(Player player, Trainer opponent) : this(player.Trainer, opponent)
	{
		Player = player;
	}
	//public Battle(UnityEngine.GameObject battleScene) //: this(player.Trainer, opponent)
	//{
	//	//ToDo: Register Unity UI instance to variables
	//	//BattlePokemonHandler
	//	//Player = player;
	//}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="p1"></param>
	/// <param name="p2"></param>
	/// <param name="pvpMultiPlayer"></param>
	/// ToDo: Wacky idea for double battles, 1 v 2 or 2 v 2.
	/// Should be able to support both player v ai, and player v player
	public Battle(Trainer[] p1, Trainer[] p2, bool pvpMultiPlayer = false)
	{

	}
	#endregion

	#region Method
	//public void StartBattle(bool canlose)
	//{
	//	//return this;
	//	GameVariables.battle = this;
	//}
	public IEnumerator<Battle.BattleResults> StartBattle(bool canlose)
	{
		//return this;
		GameVariables.battle = this;
		while (this.decision == BattleResults.InProgress)
		{
			yield return BattleResults.InProgress;
		}
	}
	public IEnumerator<Battle> AfterBattle()
	{
		while (this.decision == BattleResults.InProgress)
		{
			yield return null;
		}
		//return this;
	}
	public int pbRandom(int index)
	{
		return Settings.Rand.Next(index);
	}
	/// <summary>
	/// Returns the trainer party of pokemon at this index?
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	/// ToDo: Not implemented
	public Battler[] pbParty(int index)
	{
		return party1;
	}
	public bool isOpposing(int index)
	{
		return (index % 2) == 1;
	}
	Abilities? CheckGlobalAbility() {
		// in order from own first, opposing first, own second, opposing second
		for (int i = 0; i < battlers.Length; i++)
		{
			if (battlers[i].Ability != Abilities.NONE) return battlers[i].Ability;
		}
		return null;
	}
	//ToDo: Everything below
	public bool pbCanChooseNonActive(int index)
	{
		return false;
	}
	public bool pbCheckGlobalAbility(Abilities index)
	{
		return false;
	}
	public bool pbAllFainted(Battler[] party)
	{
		return false;
	}
	public bool pbCommonAnimation(string animation, Battler atk, object uh)
	{
		return false;
	}
	#endregion

	#region Catching and storing Pokémon.
	public void StorePokemon(Battler pokemon)
	{
		//if(!pokemon.isShadow)
		//	//"Would you like to give a nickname to {1}?"
		//	if (DisplayConfirm(LanguageExtension.Translate(Text.ScriptTexts, "GiveNickname").Value))
		//	{
		//		//"{1}'s nickname?"
		//		pokemon.SetNickname(scene.NameEntry(LanguageExtension.Translate(Text.ScriptTexts, "SetNick", pokemon.Name).Value, pokemon));
		//	}
		//int oldcurbox = GameVariables.CurrentBox;
		//int storedbox = GameVariable.StorePokemon(GameVariable.Player,pokemon)
		//string creator = GameVariable.GetStorageCreator()
		//if (storedbox < 0) return;
		//string curboxname = GameVariable.BoxName(oldcurbox)
		//string boxname = GameVariable.BoxName(storedbox)
		//if (storedbox!=oldcurbox) {
		//	if (GameVariable.isCreator)
		//		GameVariable.DisplayPaused("Box \"{1}\" on {2}'s PC was full.",curboxname,creator)
		//	else
		//		DisplayPaused("Box \"{1}\" on someone's PC was full.",curboxname)
		//	DisplayPaused("{1} was transferred to box \"{2}\".",pokemon.name,boxname)
		//}else
		//{
		//  if (GameVariable.isCreator)
		//		DisplayPaused("{1} was transferred to {2}'s PC.",pokemon.name,creator)
		//  else
		//		DisplayPaused("{1} was transferred to someone's PC.",pokemon.name)
		//	}
		//	DisplayPaused("It was stored in box \"{1}\".",boxname)
		//}		
	}
	public void ThrowPokeball(int idxPokemon, Items ball, int? rareness = null, bool showplayer = false)
	{
		string itemname = LanguageExtension.Translate(Text.Items, ball.ToString()).Value;
		Battler battler = null;
		if (isOpposing(idxPokemon))
			battler = battlers[idxPokemon];
		else
			battler = battlers[idxPokemon].OppositeOpposing;
		if (battler.isFainted())
			battler = battler.Partner;
		//"{1} threw one {2}!"
		DisplayBrief(L(Text.ScriptTexts,"ThrowBall", Player.PlayerName, itemname));
		if (battler.isFainted())
		{
			//"But there was no target..."
			Display(L(Text.ScriptTexts, "NoTarget"));
			return;
		}
		if (opponent.ID != TrainerTypes.WildPokemon) 
			//&& (!IsSnagBall(ball) || !battler.isShadow))
		{
			//scene.ThrowAndDeflect(ball, 1);
			//"The Trainer blocked the Ball!\nDon't be a thief!"
			Display(L(Text.ScriptTexts, "SnagRejected"));
		}
		else
		{
			if (!rareness.HasValue)
			{
				//get rareness for pokemon
			}
			int a = battler.TotalHP;
			int b = battler.HP;
			//ToDo: Ball Throwing Class?
			//rareness = BallHandlers.ModifyCatchRate(ball, rareness, battler);
			int x = (int)Math.Floor(((a * 3 - b * 2) * rareness.Value) / (a * 3f));
			if (battler.Status == Status.SLEEP || battler.Status == Status.FROZEN)
				x = (int)Math.Floor(x * 2.5);
			else if (battler.Status != Status.NONE)
				x = (int)Math.Floor(x * 1.5);
			int c = 0;
			if (GameVariables.playerTrainer.PokedexCaught > 600)
				c = (int)Math.Floor(x * 2.5 / 6);
			else if (GameVariables.playerTrainer.PokedexCaught > 450)
				c = (int)Math.Floor(x * 2f / 6);
			else if (GameVariables.playerTrainer.PokedexCaught > 300)
				c = (int)Math.Floor(x * 1.5 / 6);
			else if (GameVariables.playerTrainer.PokedexCaught > 150)
				c = (int)Math.Floor(x * 1f / 6);
			else if (GameVariables.playerTrainer.PokedexCaught > 30)
				c = (int)Math.Floor(x * .5 / 6);
		}

	}
	#endregion

	#region Get party info, manipulate parties.
	int PokemonCount(Pokemon[] party)
	{
		int count = 0;
		for (int i = 0; i < party.Length; i++)
		{
			if (party[i].Species == Pokemons.NONE) continue;
			if (party[i].HP > 0 && !party[i].isEgg) count += 1;
		}
		return count;
	}
	bool AllFainted(Pokemon[] party)
	{
		return PokemonCount(party) == 0;
	}
	int MaxLevel(Pokemon[] party)
	{
		int lv = 0;
		for (int i = 0; i < party.Length; i++)
		{
			if (party[i].Species == Pokemons.NONE) continue;
			if (lv < party[i].Level) lv = party[i].Level;
		}
		return lv;
	}
	#endregion

	/// <summary>
	/// Check whether actions can be taken.
	/// </summary>
	/// <param name="idxPokemon"></param>
	/// <returns></returns>
	bool CanShowCommands(int idxPokemon)
	{
		//List<BattlerEffects> thispkmn = new List<PokemonUnity.Effects.BattlerEffects>(); //battlers[idxPokemon].
		Pokemon thispkmn = battlers[idxPokemon];
		if (thispkmn.isFainted()) return false;
		//if (thispkmn.Effects.Contains(BattlerEffects.TwoTurnAttack)) return false; 
		//if (thispkmn.Effects.Contains(BattlerEffects.HyperBeam)) return false; 
		//if (thispkmn.Effects.Contains(BattlerEffects.Rollout)) return false; 
		//if (thispkmn.Effects.Contains(BattlerEffects.Outrage)) return false; 
		//if (thispkmn.Effects.Contains(BattlerEffects.Uproar)) return false;
		//if (thispkmn.Effects.Contains(BattlerEffects.Bide)) return false;
		return true;
	}
	bool CanShowFightMenu(int idxPokemon)
	{
		Pokemon thispkmn = battlers[idxPokemon];
		if (!CanShowCommands(idxPokemon)) return false;

		// No moves that can be chosen
		if (!CanChooseMove(idxPokemon, 0, false) &&
		   !CanChooseMove(idxPokemon, 1, false) &&
		   !CanChooseMove(idxPokemon, 2, false) &&
		   !CanChooseMove(idxPokemon, 3, false))
			return false;

		// Encore
		//if (thispkmn.Effects.Contains(BattlerEffects.Encore)) return false;
		return true;
	}
	bool CanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false)
	{
		/*Pokemon thispkmn = battlers[idxPokemon];
		Move thismove = thispkmn.moves[idxMove];

		//ToDo: Array for opposing pokemons, [i] changes based on if double battle
		Pokemon opp1 = thispkmn.pbOpposing1;
		Pokemon opp2 = thispkmn.pbOpposing2;
		if (thismove != null || thismove.MoveId == 0) return false;
		if (thismove.PP <= 0 && thismove.TotalPP > 0 && !sleeptalk) {
			//if (showMessages) pbDisplayPaused(_INTL("There's no PP left for this move!"));
			return false;
		}
		if (thispkmn.Item == Items.ASSAULT_VEST) {// && thismove.IsStatus?
			//if (showMessages) pbDisplayPaused(_INTL("The effects of the {1} prevent status moves from being used!", PBItems.getName(thispkmn.item)))
			return false;
		}
		if (//thispkmn.effects.ChoiceBand>=0 &&
		   (thispkmn.Item == Items.CHOICE_BAND ||
		   thispkmn.Item == Items.CHOICE_SPECS ||
		   thispkmn.Item == Items.CHOICE_SCARF))
		{
			bool hasmove = false;
			for (int i = 0; i < party.Length; i++)
			{
				//if (thispkmn.moves[i].MoveId==thispkmn.effects.ChoiceBand) 
				hasmove = true; break;
			}
			if (hasmove && thismove.MoveId != thispkmn.effects.ChoiceBand) {
				//if (showMessages)
				//	pbDisplayPaused(_INTL("{1} allows the use of only {2}!",
				//		PBItems.getName(thispkmn.item),
				//		PBMoves.getName(thispkmn.effects.ChoiceBand)))
				return false;
			}
		}
		if (opp1.effects.Imprison)
		{
			if (thismove.MoveId == opp1.moves[0].MoveId ||
				thismove.MoveId == opp1.moves[1].MoveId ||
				thismove.MoveId == opp1.moves[2].MoveId ||
				thismove.MoveId == opp1.moves[3].MoveId)
			{
				//if (showMessages) pbDisplayPaused(_INTL("{1} can't use the sealed {2}!", thispkmn.pbThis, thismove.name))
				//PBDebug.log("[CanChoose][//{opp1.pbThis} has: //{opp1.moves[0].name}, //{opp1.moves[1].name},//{opp1.moves[2].name},//{opp1.moves[3].name}]")
				return false;
			}
		}
		if (opp2.effects.Imprison)
		{
			if (thismove.MoveId == opp2.moves[0].MoveId ||
				 thismove.MoveId == opp2.moves[1].MoveId ||
				 thismove.MoveId == opp2.moves[2].MoveId ||
				 thismove.MoveId == opp2.moves[3].MoveId)
			{
				//if (showMessages) pbDisplayPaused(_INTL("{1} can't use the sealed {2}!", thispkmn.pbThis, thismove.name))
				//PBDebug.log("[CanChoose][//{opp2.pbThis} has: //{opp2.moves[0].name}, //{opp2.moves[1].name},//{opp2.moves[2].name},//{opp2.moves[3].name}]")
				return false;
			}
		}
		if (thispkmn.effects.Taunt > 0 && thismove.basedamage == 0) {
			//if (showMessages)pbDisplayPaused(_INTL("{1} can't use {2} after the taunt!", thispkmn.pbThis, thismove.name))
			return false;
		}
		if (thispkmn.effects.Torment){
			if (thismove.MoveId==thispkmn.lastMoveUsed){
				//if (showMessages) pbDisplayPaused(_INTL("{1} can't use the same move twice in a row due to the torment!", thispkmn.pbThis))
				return false;
			}
		}
		if (thismove.MoveId==thispkmn.effects.DisableMove && !sleeptalk){
			//if (showMessages) pbDisplayPaused(_INTL("{1}'s {2} is disabled!", thispkmn.pbThis, thismove.name))
			return false;
		}
		if (thismove.function==0x158 && // Belch
		   (!thispkmn.pokemon || !thispkmn.pokemon.belch)){
			//if (showMessages) pbDisplayPaused(_INTL("{1} hasn't eaten any held berry, so it can't possibly belch!", thispkmn.pbThis))
			return false;
		}
		if (thispkmn.effects.Encore>0 && idxMove!=thispkmn.effects.EncoreIndex){
			return false;
		}*/
		return true;
	}

	//void MoveEffects(int who, int move)
	//{
	//	//this.battlers[who].moves[move].
	//}

	#region Nested Classes
	/// <summary>
	/// A Pokemon placeholder class to be used while in-battle, 
	/// to prevent changes from being permanent to original pokemon profile
	/// </summary>
	/// ToDo: Create a SaveResults() after battle has ended, to make changes permanent.
	/// ToDo: If battle namespace, rename back to pokemon? is "battler" (battle only) too confusing?
	public class Battler : Pokemon
	{
		#region Variables
		public int turncount { get; private set; }
		/// <summary>
		/// Participants will earn Exp. Points if this battler is defeated
		/// </summary>
		public List<byte> participants { get; private set; }
		/// <summary>
		/// Index list of all pokemons who attacked this battler on this/previous turn
		/// </summary>
		/// ToDo: not implemented
		public List<byte> lastAttacker { get; private set; }
		#region Move to PokemonBattle Class
		/// <summary>
		/// Consumed held item (used in battle only)
		/// </summary>
		/// ToDo: Is it an int or a bool?
		public Items itemRecycle { get; set; }
		/// <summary>
		/// Resulting held item (used in battle only)
		/// </summary>
		public Items itemInitial { get; set; }
		/// <summary>
		/// Where Pokemon can use Belch (used in battle only)
		/// </summary>
		/// ToDo: Move to pkemonBattle class
		public bool belch { get; set; }
		#endregion
		private int? lastRoundMoved { get; set; }
		public int lastHPLost { get; set; }
		public bool tookDamage { get; set; }
		public List<Moves> movesUsed { get; set; }
		public Effects.Battler effects { get; private set; }
		public Battle battle { get { return GameVariables.battle; } }
		public bool captured { get; private set; }
		//public bool Fainted { get { return isFainted(); } }
		public DamageState damagestate { get; set; }
		/// <summary>
		/// Int Buffs and debuffs (gains and loss) affecting this pkmn.
		/// 0: Attack, 1: Defense, 2: Speed, 3: SpAtk, 4: SpDef, 5: Evasion, 6: Accuracy
		/// </summary>
		public int[] stages { get; private set; }
		/// <summary>
		/// Returns the position of this pkmn in battle lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == battle.party[this.pkmn.index]
		public sbyte Index { get; private set; }
		/// <summary>
		/// Returns the position of this pkmn in party lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == party[this.pkmn.index]
		public byte pokemonIndex { get; private set; }
		//[Obsolete]
		//private int Index { get { return this.battle.battlers.Length; } }
		public bool IsOwned { get { return battle.Player.playerPokedex2[_base.ArrayId, 1] == 1; } }
		private Pokemon pokemon { get; set; }
		public Moves currentMove { get; set; }
		public Moves lastMoveUsed { get; private set; }
		public Types lastMoveUsedType { get; private set; }
		//ToDo: Types public get;set;
		//ToDo: create private fields of stats
		public override int ATK { get { return effects.PowerTrick ? DEF : base.ATK; } }
		public override int DEF {
			get
			{
				if (effects.PowerTrick) return base.ATK;
				return battle.field.WonderRoom > 0 ? base.SPD : base.DEF;
			}
		}
		public override int SPD { get { return battle.field.WonderRoom > 0 ? DEF : base.SPD; } }
		public override string Name { get {
				//if name is not nickname return illusion.name?
				if (effects.Illusion != null)
					return effects.Illusion.Name;
				return base.Name; } }
		public override bool? Gender { get {
				if (effects.Illusion != null)
					return effects.Illusion.Gender;
				return base.Gender; } }
		public override bool IsShiny { get {
				if (effects.Illusion != null)
					return effects.Illusion.IsShiny;
				return base.IsShiny; }
		}
		new public int Form { get { return _base.Form; } set { if (value >= 0 && value <= _base.Forms) _base.Form = value; } }
		new public Status Status
		{
			get
			{
				return base.Status;
			}
			set
			{
				if (base.Status == Status.SLEEP && value == 0)
					effects.Truant = false;
				base.Status = value;
				if (value != Status.POISON)
					effects.Toxic = 0;
				if (value != Status.POISON && value != Status.SLEEP)
					base.StatusCount = 0;
			}
		}
		new public Battle.InBattleMove[] moves { get; set; }

		public int GetWeight(Battler attacker = null)
		{
			float w = _base.Weight;
			if (attacker != null || !attacker.hasMoldBreaker())
			{
				if (hasWorkingAbility(Abilities.HEAVY_METAL)) w *= 2;
				if (hasWorkingAbility(Abilities.LIGHT_METAL)) w /= 2;
			}
			if (hasWorkingItem(Items.FLOAT_STONE)) w /= 2;
			w += effects.WeightChange;
			w = (float)Math.Floor((decimal)w);
			if (w < 1) w = 1;
			return (int)w;
		}

		public bool IsMega { get; private set; }
		//public override bool hasMegaForm { get { if (effects.Transform) return false; return base.hasMegaForm; } }
		public bool IsPrimal { get; private set; }
		//public override bool hasPrimalForm { get { if (effects.Transform) return false; return base.hasPrimalForm; } }
		public BattlePokemonHandler playerHUD { get; private set; }
		public BattlePokemonHandler nonplayerHUD { get; private set; }
		#endregion

		#region Constructors
		/*public Battler(Battler replacingPkmn, bool batonpass)
		{
			return replacingPkmn;
		}*/
		//[Obsolete("Don't think this is needed or should be used")]
		public Battler() : base() //Battle btl, int idx
		{
			//battle		= btl;
			//Index			= idx;
			//hp			= 0;
			//totalhp		= 0;
			//fainted		= true;
			captured		= false;
			stages			= new int[7];
			effects			= new Effects.Battler(false);
			damagestate		= new DamageState();
			InitBlank();
			InitEffects(false);
			InitPermanentEffects();
		}
		/// <summary>
		/// Used when switching a pokemon INTO to battle
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="index"></param>
		/// <param name="batonpass"></param>
		/// <returns></returns>
		public Battler Initialize(Pokemon pkmn, sbyte index, bool batonpass = false) //: base(pkmn)
		{
			//Cure status of previous Pokemon with Natural Cure
			if (this.hasWorkingAbility(Abilities.NATURAL_CURE))
				this.Status = 0;
			if (this.hasWorkingAbility(Abilities.REGENERATOR))
				this.RecoverHP((int)Math.Floor((decimal)this.TotalHP / 3));
			InitPokemon(pkmn, index);
			InitEffects(batonpass);
			/*effects = new Effects.Battler(batonpass);
			effects = new Effects.Battler(batonpass);
			if (!batonpass)
			{
				//These effects are retained if Baton Pass is used
				//stages[PBStats::ATTACK]   = 0;
				//stages[PBStats::DEFENSE]  = 0;
				//stages[PBStats::SPEED]    = 0;
				//stages[PBStats::SPATK]    = 0;
				//stages[PBStats::SPDEF]    = 0;
				//stages[PBStats::EVASION]  = 0;
				//stages[PBStats::ACCURACY] = 0;
				stages = new int[7];
				//lastMoveUsedSketch        = -1;
				
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					if (battle.battlers[i].Species == Pokemons.NONE) continue;
					if (battle.battlers[i].effects.LockOnPos==index &&
						battle.battlers[i].effects.LockOn > 0)
					{
						battle.battlers[i].effects.LockOn = 0;
						battle.battlers[i].effects.LockOnPos = -1;
					}
				}
			}
			else
			{
				if (this.effects.PowerTrick){
					//this.ATK = this.DEF;
					//this.DEF = this.ATK;
				}
			}
			damagestate.Reset();
			//isFainted				= false;
			//battle.lastAttacker     = []
			lastHPLost				= 0;
			tookDamage				= false;
			lastMoveUsed			= Moves.NONE;
			lastMoveUsedType		= -1;
			lastRoundMoved			= -1;
			movesUsed				= new List<Moves>();
			battle.turncount		= 0;
			
			for (int i = 0; i < battle.battlers.Length; i++)
			{
				if (battle.battlers[i].Species == Pokemons.NONE) continue;
				if (battle.battlers[i].effects.Attract == index)
				{
					battle.battlers[i].effects.Attract = -1;
				}
				if (battle.battlers[i].effects.MeanLook == index)
				{
					battle.battlers[i].effects.MeanLook = -1;
				}
				if (battle.battlers[i].effects.MultiTurnUser == index)
				{
					battle.battlers[i].effects.MultiTurn = 0;
					battle.battlers[i].effects.MultiTurnUser = -1;
				}
			}

			if (this.hasWorkingAbility(Abilities.ILLUSION)){
				//lastpoke = battle.GetLastPokeInTeam(index);
				//if (lastpoke!=pokemonIndex){
				//	this.Illusion     = battle.Party(index)[lastpoke]
				//}
			}*/
			return this;
		}
		private void InitBlank()
		{
			//Pokemon blank = new Pokemon();
			//Level = 0;
			//pokemonIndex = -1;
			participants = new List<byte>();
		}
		private void InitEffects(bool batonpass)
		{
			if (!batonpass)
			{
				//These effects are retained if Baton Pass is used
				//stages[0]	= 0; // [ATTACK]  
				//stages[1]	= 0; // [DEFENSE] 
				//stages[2]	= 0; // [SPEED]   
				//stages[3]	= 0; // [SPATK]   
				//stages[4]	= 0; // [SPDEF]   
				//stages[5]	= 0; // [EVASION] 
				//stages[6]	= 0; // [ACCURACY]
				stages = new int[7];
				//lastMoveUsedSketch        = -1;
				effects.AquaRing	= false;
				effects.Confusion	= 0;
				effects.Curse		= false;
				effects.Embargo		= 0;
				effects.FocusEnergy = 0;
				effects.GastroAcid  = false;
				effects.HealBlock   = 0;
				effects.Ingrain     = false;
				effects.LeechSeed   = -1;
				effects.LockOn      = 0;
				effects.LockOnPos   = -1;
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					if (battle.battlers[i].Species == Pokemons.NONE) continue;
					if (battle.battlers[i].effects.LockOnPos == Index &&
						battle.battlers[i].effects.LockOn > 0)
					{
						battle.battlers[i].effects.LockOn = 0;
						battle.battlers[i].effects.LockOnPos = -1;
					}
				}
				effects.MagnetRise     = 0;
				effects.PerishSong     = 0;
				effects.PerishSongUser = -1;
				effects.PowerTrick     = false;
				effects.Substitute     = 0;
				effects.Telekinesis    = 0;
			}
			else
			{
				if (effects.LockOn>0)
					effects.LockOn=2;
				else
					effects.LockOn=0;
				
				if (effects.PowerTrick)
				{
					//this.ATK = this.DEF;
					//this.DEF = this.ATK;
				}
			}			
			damagestate.Reset();
			//isFainted				 = false;
			//battle.lastAttacker    = []
			lastHPLost				 = 0;
			tookDamage				 = false;
			lastMoveUsed			 = Moves.NONE;
			lastMoveUsedType		 = Types.NONE; //-1;
			lastRoundMoved			 = -1;
			movesUsed				 = new List<Moves>();
			battle.turncount		 = 0;
			effects.Attract          = -1;
			effects.BatonPass        = false;
			effects.Bide             = 0;
			effects.BideDamage       = 0;
			effects.BideTarget       = -1;
			effects.Charge           = 0;
			effects.ChoiceBand       = null;
			effects.Counter          = -1;
			effects.CounterTarget    = -1;
			effects.DefenseCurl      = false;
			effects.DestinyBond      = false;
			effects.Disable          = 0;
			effects.DisableMove      = 0;
			effects.Electrify        = false;
			effects.Encore           = 0;
			effects.EncoreIndex      = 0;
			effects.EncoreMove       = 0;
			effects.Endure           = false;
			effects.FirstPledge      = 0;
			effects.FlashFire        = false;
			effects.Flinch           = false;
			effects.FollowMe         = 0;
			effects.Foresight        = false;
			effects.FuryCutter       = 0;
			effects.Grudge           = false;
			effects.HelpingHand      = false;
			effects.HyperBeam        = 0;
			effects.Illusion         = null;
			effects.Imprison         = false;
			effects.KingsShield      = false;
			effects.LifeOrb          = false;
			effects.MagicCoat        = false;
			effects.MeanLook         = -1;
			effects.MeFirst          = false;
			effects.Metronome        = 0;
			effects.MicleBerry       = false;
			effects.Minimize         = false;
			effects.MiracleEye       = false;
			effects.MirrorCoat       = -1;
			effects.MirrorCoatTarget = -1;
			effects.MoveNext         = false;
			effects.MudSport         = false;
			effects.MultiTurn        = 0;
			effects.MultiTurnAttack  = 0;
			effects.MultiTurnUser    = -1;
			effects.Nightmare        = false;
			effects.Outrage          = 0;
			effects.ParentalBond     = 0;
			effects.PickupItem       = 0;
			effects.PickupUse        = 0;
			effects.Pinch            = false;
			effects.Powder           = false;
			effects.Protect          = false;
			effects.ProtectNegation  = false;
			effects.ProtectRate      = 1;
			effects.Pursuit          = false;
			effects.Quash            = false;
			effects.Rage             = false;
			effects.Revenge          = 0;
			effects.Roar             = false;
			effects.Rollout          = 0;
			effects.Roost            = false;
			effects.SkipTurn         = false;
			effects.SkyDrop          = false;
			effects.SmackDown        = false;
			effects.Snatch           = false;
			effects.SpikyShield      = false;
			effects.Stockpile        = 0;
			effects.StockpileDef     = 0;
			effects.StockpileSpDef   = 0;
			effects.Taunt            = 0;
			effects.Torment          = false;
			effects.Toxic            = 0;
			effects.Transform        = false;
			effects.Truant           = false;
			effects.TwoTurnAttack    = 0;
			effects.Type3            = Types.NONE; //-1;
			effects.Unburden         = false;
			effects.Uproar           = 0;
			effects.Uturn            = false;
			effects.WaterSport       = false;
			effects.WeightChange     = 0;
			effects.Yawn             = 0;
			for (int i = 0; i < battle.battlers.Length; i++)
			{
				if (battle.battlers[i].Species == Pokemons.NONE) continue;
				if (battle.battlers[i].effects.Attract == Index)
				{
					battle.battlers[i].effects.Attract = -1;
				}
				if (battle.battlers[i].effects.MeanLook == Index)
				{
					battle.battlers[i].effects.MeanLook = -1;
				}
				if (battle.battlers[i].effects.MultiTurnUser == Index)
				{
					battle.battlers[i].effects.MultiTurn = 0;
					battle.battlers[i].effects.MultiTurnUser = -1;
				}
			}
			if (this.hasWorkingAbility(Abilities.ILLUSION))
			{
				//lastpoke = battle.GetLastPokeInTeam(index);
				//if (lastpoke!=pokemonIndex){
				//	this.Illusion     = battle.Party(index)[lastpoke]
				//}
			}
		}
		private void InitPermanentEffects()
		{
			// These effects are always retained even if a Pokémon is replaced
			effects.FutureSight        = 0	  ;
			effects.FutureSightMove    = 0	  ;
			effects.FutureSightUser    = -1	  ;
			effects.FutureSightUserPos = -1	  ;
			effects.HealingWish        = false;
			effects.LunarDance         = false;
			effects.Wish               = 0	  ;
			effects.WishAmount         = 0	  ;
			effects.WishMaker          = -1	  ;
		}
		private void InitPokemon(Pokemon pkmn, sbyte pkmnIndex)
		{
			if (pkmn.isEgg)
			{
				//Remove/Disable UI for Egg 
				//Pause game to display error message?
				//"An egg can't be an active Pokémon"
				GameVariables.Dialog(LanguageExtension.Translate(Text.Errors, "ActiveEgg").Value);
			}
			else
			{
				//Name			= pkmn.Name
				//Species		= pkmn.Species
				//Level			= pkmn.Level
				//HP			= pkmn.HP
				//TotalHP		= pkmn.TotalHP
				//Gender		= pkmn.Gender
				//Ability		= pkmn.Ability
				//Item			= pkmn.Item
				//Type			= pkmn.Type
				//Form			= pkmn.Form
				//ATK			= pkmn.ATK
				//DEF			= pkmn.DEF
				//SPE			= pkmn.SPE
				//SPA			= pkmn.SPA
				//SPD			= pkmn.SPD
				//Status		= pkmn.Status
				//StatusCount	= pkmn.StatusCount
				pokemon			= pkmn;
				Index			= pkmnIndex;
				participants	= new List<byte>();
				moves			= new InBattleMove[] {
					(InBattleMove)base.moves[0],
					(InBattleMove)base.moves[1],
					(InBattleMove)base.moves[2],
					(InBattleMove)base.moves[3]
				};
			}
#if (DEBUG == false || UNITY_EDITOR == true)
			UpdateUI();
#endif
		}
		public Battler Update(bool fullchange = false)
		{
			if(Species != Pokemons.NONE)
			{
				//calcStats(); //Not needed since fetching stats from base ( Battler => Pokemon )
				//ToDo: Uncomment and fetch data from baseClass
				//Level		= pokemon.Level;
				//HP		= pokemon.HP;
				//TotalHP	= pokemon.TotalHP;
				//Battler	= Pokemon, so not all stats need to be handpicked
				if (!effects.Transform) //Changed forms but did not transform?
				{
					//ATK		= pokemon.ATK;
					//DEF		= pokemon.DEF;
					//SPE		= pokemon.SPE;
					//SPA		= pokemon.SPA;
					//SPD		= pokemon.SPD;
					if (fullchange)
					{
						//Ability	= pokemon.Ability;
						//Type		= pokemon.Type;
					}
				}
			}
			return this;
		}
		/// <summary>
		/// Used only to erase the battler of a Shadow Pokémon that has been snagged.
		/// </summary>
		public Battler Reset()
		{
			pokemon		= new Pokemon();
			Index		= -1;
			InitEffects(false);
			//reset status
			Status		= Status.NONE;
			StatusCount	= 0;
			//IsFainted	= true;
			//reset choice
			battle.choices[Index] = new Choice(ChoiceAction.NoAction);
			return this;
		}
		/// <summary>
		/// Update Pokémon who will gain EXP if this battler is defeated
		/// </summary>
		public void UpdateParticipants()
		{
			//Can't update if already fainted
			if (!isFainted())
			{
				if (battle.isOpposing(Index))
				{
					bool found1, found2; found1 = found2 = false;
					for (int i = 0; i < participants.Count; i++)
					{
						if (participants[i] == battle.battlers[3].Index) found1 = true;
						if (participants[i] == battle.battlers[4].Index) found2 = true;
					}
					if (!found1 && !battle.battlers[3].isFainted())
						participants.Add((byte)battle.battlers[3].Index);
					if (!found2 && !battle.battlers[4].isFainted())
						participants.Add((byte)battle.battlers[4].Index);
				}
			}
		}
		#endregion

		#region About this Battler
		public string ToString(bool lowercase = false)
		{
			if (battle.isOpposing(Index))
			{
				if (battle.opponent.ID == TrainerTypes.WildPokemon)
					//return string.Format("The wild {0}", Name);
					return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "WildPokemonL" : "WildPokemon", Name).Value;
				else
					//return string.Format("The opposing {0}", Name);
					return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "OpponentPokemonL" : "OpponentPokemon", Name).Value;
			}
			else if (battle.OwnedByPlayer(Index))
				return Name;
			else
				//return string.Format("The ally {0}", Name);
				return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "AllyPokemonL" : "AllyPokemon", Name).Value;
			//return base.ToString();
		}
		
		public bool hasMoldBreaker() {
			//Pokemon pkmn = this;
			if (this.hasWorkingAbility(Abilities.MOLD_BREAKER) ||
				this.hasWorkingAbility(Abilities.TERAVOLT) ||
				this.hasWorkingAbility(Abilities.TURBOBLAZE)) return true;
			return false;
		}

		public bool hasWorkingAbility(Abilities ability, bool ignorefainted= false) {
			//Pokemon pkmn = this;
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.GastroAcid) return false;
			return this.Ability != ability;
		}

		public new bool hasType(Types type) {
			if (type == Types.NONE || type < 0) return false;
			bool ret = (this.Type1 == type || this.Type2 == type);
			if (effects.Type3 >= 0) ret |= (effects.Type3 == type);
			return ret;
		}

		public bool HasMoveType(Types type) {
			if (type == Types.NONE || type < 0) return false;
			for (int i = 0; i < moves.Length; i++)
			{
				if (moves[i].Type == type) return true;
			}
			return false;
		}

		public bool HasMoveFunction(string code) {
			if (string.IsNullOrEmpty(code)) return false;
			for (int i = 0; i < moves.Length; i++)
			{
				if (moves[i].FunctionAsString == code) return true;
			}
			return false;
		}

		public bool HasMoveFunction(short code) {
			//if (string.IsNullOrEmpty(code)) return false;
			for (int i = 0; i < moves.Length; i++)
			{
				if ((short)moves[i].Function == code) return true;
			}
			return false;
		}

		public bool HasMoveFunction(Move.Effect code) {
			//if (string.IsNullOrEmpty(code)) return false;
			for (int i = 0; i < moves.Length; i++)
			{
				if ((Move.Effect)moves[i].Function == code) return true;
			}
			return false;
		}

		public bool HasMovedThisRound() {
			if (!lastRoundMoved.HasValue) return false;
			return lastRoundMoved.Value == battle.turncount;
		}
		//ToDo: Double check this
		public bool hasMovedThisRound(int? turncount = null){
			if (!lastRoundMoved.HasValue) return false;
			return !turncount.HasValue 
				? lastRoundMoved.Value == battle.turncount
				: lastRoundMoved.Value == turncount.Value;
		}

		public bool hasWorkingItem(Items item, bool ignorefainted= false) {
			//Pokemon pkmn = null;
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			return this.Item != item;
		}

		public bool hasWorkingBerry(bool ignorefainted= false) {
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			return new global::Item(this.Item).ItemPocket == ItemPockets.BERRY;//pbIsBerry?(@item)
		}

		bool isAirborne(bool ignoreability=false){
			if (this.hasWorkingItem(Items.IRON_BALL)) return false;
			if (effects.Ingrain) return false;
			if (effects.SmackDown) return false;
			if (battle.field.Gravity > 0) return false;
			if (this.hasType(Types.FLYING) && !effects.Roost) return true;
			if (this.hasWorkingAbility(Abilities.LEVITATE) && !ignoreability) return true;
			if (this.hasWorkingItem(Items.AIR_BALLOON)) return true; 
			if (effects.MagnetRise > 0) return true;
			if (effects.Telekinesis > 0) return true;
			return false;
		}

		int Speed()
		{
			int[] stagemul = new int[] { 10, 10, 10, 10, 10, 10, 10, 15, 20, 25, 30, 35, 40 };
			int[] stagediv = new int[] { 40, 35, 30, 25, 20, 15, 10, 10, 10, 10, 10, 10, 10 };
			int speed = 0;
			int stage = stages[2] + 6;
			speed = (int)Math.Floor(speed * (decimal)stagemul[stage] / stagediv[stage]);
			int speedmult = 0x1000;
			switch (battle.weather)
			{
				case PokemonUnity.Weather.RAINDANCE:
				case PokemonUnity.Weather.HEAVYRAIN:
					speedmult = hasWorkingAbility(Abilities.SWIFT_SWIM) ? speedmult * 2 : speedmult;
					break;
				case PokemonUnity.Weather.SUNNYDAY:
				case PokemonUnity.Weather.HARSHSUN:
					speedmult = hasWorkingAbility(Abilities.CHLOROPHYLL) ? speedmult * 2 : speedmult;
					break;
				case PokemonUnity.Weather.SANDSTORM:
					speedmult = hasWorkingAbility(Abilities.SAND_RUSH) ? speedmult * 2 : speedmult;
					break;
				default:
					break;
			}
			if (hasWorkingAbility(Abilities.QUICK_FEET) && Status > 0)
				speedmult = (int)Math.Round(speedmult * 1.5);
			if (hasWorkingAbility(Abilities.UNBURDEN) && effects.Unburden && Item == Items.NONE)
				speedmult = speedmult * 2;
			if (hasWorkingAbility(Abilities.SLOW_START) && battle.turncount > 0)
				speedmult = (int)Math.Round(speedmult * 1.5);
			if (hasWorkingItem(Items.MACHO_BRACE) || 
				hasWorkingItem(Items.POWER_WEIGHT) ||
				hasWorkingItem(Items.POWER_BRACER) ||
				hasWorkingItem(Items.POWER_BELT) ||
				hasWorkingItem(Items.POWER_ANKLET) ||
				hasWorkingItem(Items.POWER_LENS) ||
				hasWorkingItem(Items.POWER_BAND) ||
				hasWorkingItem(Items.IRON_BALL))
				speedmult = (int)Math.Round((decimal)speedmult / 2);
			if (hasWorkingItem(Items.CHOICE_SCARF))
				speedmult = (int)Math.Round(speedmult * 2f);
			if (Item == Items.IRON_BALL)
				speedmult = (int)Math.Round((decimal)speedmult / 2);
			if (hasWorkingItem(Items.QUICK_POWDER) &&
				Species == Pokemons.DITTO &&
				!effects.Transform)
				speedmult = (int)Math.Round(speedmult * 2f);
			if (OwnSide.Tailwind > 0)
				speedmult = (int)Math.Round(speedmult * 2f);
			if (OwnSide.Swamp > 0)
				speedmult = (int)Math.Round(speedmult / 2f);
			if (!hasWorkingAbility(Abilities.QUICK_FEET) &&
				Status == Status.PARALYSIS)
				speedmult = (int)Math.Round(speedmult / 4f);
			if (battle.internalbattle && 
				//battle.OwnedByPlayer(Index) &&
				battle.Player.BadgesCount >= Settings.BADGESBOOSTSPEED)
				speedmult = (int)Math.Round(speedmult * 1.1f);
			speed = (int)Math.Round(speed * speedmult * 1f/0x1000);
			return Math.Max(speed, 1);
		}
		#endregion

		#region Change HP
		public int ReduceHP(int amt, bool animate = false, bool registerDamage = true)
		{
			if (amt >= HP)
				amt = HP;
			else if (amt < 1 && !isFainted())
				amt = 1;
			int oldhp = HP;
			HP -= amt;
			if (HP < 0)
				//"HP less than 0"
				GameVariables.Dialog(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
			if (HP > TotalHP)
				//"HP greater than total HP"
				GameVariables.Dialog(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
			//ToDo: Pass to UnityEngine
			//if (amt > 0)
			//	battle.scene.HPChanged(Index, oldhp, animate); //Unity takes over
			if (amt > 0 && registerDamage)
				tookDamage = true;
			return amt;
		}

		public void RecoverHP(int amount, bool animate = false)
		{
			// the checks here are redundant, cause they're also placed on HP { set; }
			if (HP + amount > TotalHP)
				amount = TotalHP - HP;
			else if (amount < 1 && HP != TotalHP)
				amount = 1;
			int oldhp = HP;
			HP += amount;
			if (HP < 0)
				//"HP less than 0"
				GameVariables.Dialog(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
			if (HP > TotalHP)
				//"HP greater than total HP"
				GameVariables.Dialog(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
			//ToDo: Pass to UnityEngine
			//if(amount > 0)
			//	battle.scene.HPChanged(Index, oldhp, animate); //Unity takes over
		}

		public void Faint(bool showMessage = true)
		{
			if(!isFainted() && HP > 0)
			{
				GameVariables.DebugLog("Can't faint with HP greater than 0", true);
				//return true;
			}
			if(isFainted())
			{
				GameVariables.DebugLog("Can't faint if already fainted", false);
				//return true;
			}
			//battle.scene.Fainted(Index);
			InitEffects(false);
			// Reset status
			Status = 0;
			StatusCount = 0;
			if (pokemon != null && battle.internalbattle)
				pokemon.ChangeHappiness(HappinessMethods.FAINT);
			//if (IsMega)
			//	pokemon.MakeUnmega;
			//if (IsPrimal)
			//	pokemon.MakeUnprimal;
			//Fainted = true;
			//reset choice
			battle.choices[Index] = new Choice(ChoiceAction.NoAction);
			OwnSide.LastRoundFainted = battle.turncount;
			if (showMessage)
				GameVariables.Dialog(LanguageExtension.Translate(Text.Errors, "Fainted", new string[] { ToString() }).Value);
			//return true;
		}
		#endregion

		#region Find other battlers/sides in relation to this battler
		/// <summary>
		/// Returns the data structure for this battler's side
		/// </summary>
		/// Player: 0 and 2; Foe: 1 and 3
		public Effects.Side OwnSide { get { return battle.sides[Index&1]; } }
		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// Player: 1 and 3; Foe: 0 and 2
		public Effects.Side OpposingSide { get { return battle.sides[(Index&1)^1]; } }
		/// <summary>
		/// Returns whether the position belongs to the opposing Pokémon's side
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public bool IsOpposing(int i)
		{
			return (Index & 1) != (i & 1);
		}
		/// <summary>
		/// Returns the battler's partner
		/// </summary>
		public Battler Partner { get { return battle.battlers[(Index & 1) | ((Index & 2) ^ 2)]; } }
		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		public Battler OppositeOpposing { get { return battle.battlers[(Index ^ 1)]; } }
		/// <summary>
		/// Returns the battler's first opposing Pokémon Index
		/// </summary>
		public int OpposingIndex { get { return (Index ^ 1) | ((Index & 2) ^ 2); } }
		public int NonActivePokemonCount
		{
			get
			{
				int count = 0;
				//Pokemon[] party;// = battle.party.([(Index & 1) | ((Index & 2) ^ 2)]
				for (int i = 0; i < 6; i++)
				{
					if ((isFainted() || i != Index) &&
						(Partner.isFainted() || i != Partner.Index) &&
						battle.party[(Index & 1) | ((Index & 2) ^ 2), i].Species != Pokemons.NONE &&
						!battle.party[(Index & 1) | ((Index & 2) ^ 2), i].isEgg &&
						battle.party[(Index & 1) | ((Index & 2) ^ 2), i].HP > 0)
							count += 1;
				}
				return count;
			}
		}
		#endregion

		#region Forms
		/// <summary>
		/// 
		/// </summary>
		/// ToDo: Changes stats on form changes here?
		public void CheckForm()
		{
			if (effects.Transform) return;
			if (isFainted()) return;
			bool transformed = false;
			//Forecast
			if (Species == Pokemons.CASTFORM)
			{
				if (hasWorkingAbility(Abilities.FORECAST))
				{
					switch (battle.Weather)
					{
						case Weather.SUNNYDAY:
						case Weather.HARSHSUN:
							if(Form != 1)
							{
								Form = 1;
								transformed = true;
							}
							break;
						case Weather.RAINDANCE:
						case Weather.HEAVYRAIN:
							if(Form != 2)
							{
								Form = 2;
								transformed = true;
							}
							break;
						case Weather.HAIL:
							if(Form != 3)
							{
								Form = 3;
								transformed = true;
							}
							break;
						case Weather.NONE:
						default:
							if(Form != 0)
							{
								Form = 0;
								transformed = true;
							}
							break;
					}
				}
			}
			if (Species == Pokemons.SHAYMIN)
			{
				if (Form != pokemon.Form)
				{
					Form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.GIRATINA)
			{
				if (Form != pokemon.Form)
				{
					Form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.ARCEUS && Ability == Abilities.MULTITYPE)
			{
				if (Form != pokemon.Form)
				{
					Form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.DARMANITAN)
			{
				if(hasWorkingAbility(Abilities.ZEN_MODE) && HP <= Math.Floor(TotalHP/2f))
					if (Form != 1)
					{
						Form = 1;
						transformed = true;
					}
				else
					if (Form != 0)
					{
						Form = 0;
						transformed = true;
					}
			}
			if (Species == Pokemons.KELDEO)
			{
				if (Form != pokemon.Form)
				{
					Form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.GENESECT)
			{
				if (Form != pokemon.Form)
				{
					Form = pokemon.Form;
					transformed = true;
				}
			}
			if (transformed)
			{
				Update(true);
				//battle.scene.ChangePokemon();
				//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "Transformed", ToString()).Value);
				GameVariables.DebugLog(string.Format("[Form changed] {0} changed to form {1}", ToString(), Form));
			}
		}
		public void ResetForm()
		{
			if (!effects.Transform)
				if (Species == Pokemons.CASTFORM ||
					Species == Pokemons.CHERRIM ||
					Species == Pokemons.DARMANITAN ||
					Species == Pokemons.MELOETTA ||
					Species == Pokemons.AEGISLASH ||
					Species == Pokemons.XERNEAS)
					Form = 0;
			Update(true);
		}
		#endregion

		#region Ability Effects
		public void AbilitiesOnSwitchIn(bool onactive)
		{
			if (isFainted()) return;
			//if (onactive)
			//	battle.PrimalReversion(Index);
			///Weather
			if (onactive)
			{
				if(hasWorkingAbility(Abilities.PRIMORDIAL_SEA) && battle.Weather != Weather.HEAVYRAIN)
				{
					battle.SetWeather(Weather.HEAVYRAIN);
					battle.weatherduration = -1;
					//battle.CommonAnimation("HeavyRain", null, null);
					//"{1}'s {2} made a heavy rain begin to fall!"
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HeavyRainStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Primordial Sea made it rain heavily", ToString()));
				}
				if(hasWorkingAbility(Abilities.DESOLATE_LAND) && battle.Weather != Weather.HARSHSUN)
				{
					battle.SetWeather(Weather.HARSHSUN);
					battle.weatherduration = -1;
					//battle.CommonAnimation("HarshSun", null, null);
					//"{1}'s {2} turned the sunlight extremely harsh!"
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HarshSunStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Desolate Land made the sun shine harshly", ToString()));
				}
				if(hasWorkingAbility(Abilities.DELTA_STREAM) && battle.Weather != Weather.STRONGWINDS)
				{
					battle.SetWeather(Weather.STRONGWINDS);
					battle.weatherduration = -1;
					//battle.CommonAnimation("StrongWinds", null, null);
					//"{1}'s {2} caused a mysterious air current that protects Flying-type Pokémon!"
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "StrongWindsStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Delta Stream made an air current blow", ToString()));
				}
				if (battle.Weather != Weather.HEAVYRAIN &&
					battle.Weather != Weather.HARSHSUN &&
					battle.Weather != Weather.STRONGWINDS)
				{
					if (hasWorkingAbility(Abilities.DRIZZLE) && 
						(battle.Weather != Weather.RAINDANCE || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.RAINDANCE);
						if (Settings.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.DAMP_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						//battle.CommonAnimation("Rain", null, null);
						//"{1}'s {2} made it rain!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "RainStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Drizzle made it rain", ToString()));
					}
					if (hasWorkingAbility(Abilities.DROUGHT) && 
						(battle.Weather != Weather.SUNNYDAY || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.SUNNYDAY);
						if (Settings.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.HEAT_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						//battle.CommonAnimation("Sunny", null, null);
						//"{1}'s {2} intensified the sun's rays!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SunnyStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Drought made it sunny", ToString()));
					}
					if (hasWorkingAbility(Abilities.SAND_STREAM) && 
						(battle.Weather != Weather.SANDSTORM || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.SANDSTORM);
						if (Settings.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.SMOOTH_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						//battle.CommonAnimation("Sandstorm", null, null);
						//"{1}'s {2} whipped up a sandstorm!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SandstormStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Sand Stream made it sandstorm", ToString()));
					}
					if (hasWorkingAbility(Abilities.SNOW_WARNING) && 
						(battle.Weather != Weather.HAIL || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.HAIL);
						if (Settings.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.ICY_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						//battle.CommonAnimation("Hail", null, null);
						//"{1}'s {2} madeit hail!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HailStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameVariables.DebugLog(string.Format("[Ability triggered] {0}'s Snow Warning made it hail", ToString()));
					}
				}
				if(hasWorkingAbility(Abilities.AIR_LOCK) || hasWorkingAbility(Abilities.CLOUD_NINE))
				{
					//battle.SetWeather(Weather.NONE);
					//battle.weatherduration = 0;
					//"{1} has {2}!"
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HasAbility", ToString(), Ability.ToString().Translate().Value).Value);
					//"The effects of the weather disappeared."
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "WeatherNullified").Value);
					GameVariables.DebugLog(string.Format("[Ability nullified] {0}'s Ability cancelled weather effects", ToString()));
				}
			}
			//battle.PrimordialWeather();
			///Trace
			if (hasWorkingAbility(Abilities.TRACE))
			{
				//Choice[] choices = new Choice[4];
				List<int> choices = new List<int>();
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					Battler foe = battle.battlers[i];
					if (IsOpposing(i) && !foe.isFainted())
					{
						Abilities abil = foe.Ability;
						if (abil > 0 &&
							abil != Abilities.TRACE &&
							abil != Abilities.MULTITYPE &&
							abil != Abilities.ILLUSION &&
							abil != Abilities.FLOWER_GIFT &&
							abil != Abilities.IMPOSTER &&
							abil != Abilities.STANCE_CHANGE)
							choices.Add(i);
					}
				}
				if (choices.Count > 0)
				{
					//ToDo: WIP; Finish from here...
				}
			}

		}
		#endregion

		#region ToDo: Everything here needs to be implemented
		public Moves lastMoveUsedSketch { get; set; }
		/// <summary>
		/// Does faint animation 
		/// (doesn't have to lower hp, as hp will be done by other set of code. 
		/// But just to be safe... do it anyways)
		/// </summary>
		public void pbFaint() { }
		public void pbConsumeItem() { }
		public void pbAbilityCureCheck() { }
		public void pbConfuse() { }
		public void pbSleep() { }
		public void pbSleepSelf(int turns) { }
		public void pbFreeze() { }
		public void pbAttract(Battler pkmn, byte? uh = null, bool animate = false) { }
		public void pbFlinch(Battler pkmn, byte? uh = null, bool animate = false) { }
		public void pbPoison(Battler pkmn, byte? uh = null, bool animate = false) { }
		public void pbParalyze(Battler pkmn, byte? uh = null, bool animate = false) { }
		public void pbBurn(Battler pkmn, byte? uh = null, bool animate = false) { }
		public void pbCureStatus(bool animate = false) { }
		public void pbAbilitiesOnSwitchIn(bool animate = false) { }
		public void pbUseMoveSimple(Moves move) { }
		public float Weight(Battler pkmn)
		{
			return 0f;
		}
		public bool pbTooLow(Stats stat)
		{
			return false;
		}
		public bool pbCanAttract(Battler pkmn)
		{
			return false;
		}
		public bool pbCanConfuseSelf(bool pkmn)
		{
			return false;
		}
		public bool pbCanConfuse(Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanSleep(Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanSleep(Battler pkmn, bool animate, PokeBattle_Move atk, bool uh)
		{
			return false;
		}
		public bool pbCanFreeze(Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanPoison(Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanParalyze(Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanBurn(Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanIncreaseStatStage(Stats stat, Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanReduceStatStage(Stats stat, Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbIncreaseStat(Stats stat, int num, Battler pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbIncreaseStat(Stats stat, int num, Battler pkmn, bool uh, PokeBattle_Move atk, bool animate)
		{
			return false;
		}
		public bool pbReduceStat(Stats stat, int num, Battler pkmn, bool uh, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbReduceStat(Stats stat, int num, Battler pkmn, bool uh, PokeBattle_Move atk, bool animate)
		{
			return false;
		}
		public bool pbAddTarget(byte index, Battler pkmn)
		{
			return false;
		}
		public void pbRandomTarget(byte index)
		{
			//return false;
		}
		#endregion

		/*public static implicit operator Battler[](Pokemon[] input)
		{
			Battler[] battlers = new Battler[input.Length];
			return battlers;
		}

		public static implicit operator Battler(Pokemon input)
		{
			Battler[] battlers = new Battler[input.Length];
			return battlers;
		}*/

		public static Battler[] GetBattlers(Pokemon[] input)
		{
			Battler[] battlers = new Battler[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				battlers[i] = (Battler)input[i];
			}
			return battlers;
		}
		public void BattlerUI(BattlePokemonHandler gameObject, bool player = false)
		{
			if (player) playerHUD = gameObject;
			else nonplayerHUD = gameObject;
		}
		/// <summary>
		/// Refreshes the HUD of this Battler
		/// </summary>
		private void UpdateUI()
		{
			playerHUD.HP = nonplayerHUD.HP = HP;
			playerHUD.TotalHP = nonplayerHUD.TotalHP = TotalHP;
			playerHUD.expSlider.value = nonplayerHUD.expSlider.value = Exp.Current;
			playerHUD.expSlider.maxValue = nonplayerHUD.expSlider.maxValue = Exp.NextLevel;
			playerHUD.Level.text = nonplayerHUD.Level.text = Level.ToString();
			playerHUD.Name.text = nonplayerHUD.Name.text = Name;
			playerHUD.Gender = nonplayerHUD.Gender = Gender;
			playerHUD.Status = nonplayerHUD.Status = Status;
			playerHUD.Item = nonplayerHUD.Item = Item != Items.NONE;
		}
	}

	private bool OwnedByPlayer(int index)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// A Move placeholder class to be used while in-battle, 
	/// to prevent temp changes from being permanent to original pokemon profile
	/// </summary>
	public class InBattleMove : Move
	{
		#region Variables
		public bool NOTYPE						{ get; set; } //= 0x01
		public bool IGNOREPKMNTYPES				{ get; set; } //= 0x02
		public bool NOWEIGHTING					{ get; set; } //= 0x04
		public bool NOCRITICAL					{ get; set; } //= 0x08
		public bool NOREFLECT					{ get; set; } //= 0x10
		public bool SELFCONFUSE					{ get; set; } //= 0x20

		public Category Category				{ get; set; }
		new public Moves MoveId					{ get; set; }
		new public Target Targets				{ get; set; }
		new public Types Type					{ get; set; }
		new public Flags Flag					{ get; set; }
		new public byte PP						{ get; set; }
		new public int TotalPP					{ get; set; }
		/// <summary>
		/// The probability that the move's additional effect occurs, as a percentage. 
		/// If the move has no additional effect (e.g. all status moves), this value is 0.
		/// Note that some moves have an additional effect chance of 100 (e.g.Acid Spray), 
		/// which is not the same thing as having an effect that will always occur. 
		/// Abilities like Sheer Force and Shield Dust only affect additional effects, not regular effects.
		/// </summary>
		public int AddlEffect					{ get; set; }
		/// <summary>
		/// The move's accuracy, as a percentage. 
		/// An accuracy of 0 means the move doesn't perform an accuracy check 
		/// (i.e. it cannot be evaded).
		/// </summary>
		public int Accuracy						{ get; set; }
		public int BaseDamage					{ get; set; }
		public int CritRatio					{ get; set; }
		public int Priority						{ get; set; }
		public bool IsPhysical					{ get { return Category == Category.PHYSICAL; } }
		public bool IsSpecial					{ get { return Category == Category.SPECIAL; } }
		public bool UnuseableInGravity			{ get; set; }
		public string EffectString				{ get; set; }
		public string Nothing = "But nothing happened!";
		#endregion

		private Battle Battle { get { return GameVariables.battle; } }

		//NOTYPE          = 0x01,
		//IGNOREPKMNTYPES = 0x02,
		//NOWEIGHTING     = 0x04,
		//NOCRITICAL      = 0x08,
		//NOREFLECT       = 0x10,
		//SELFCONFUSE     = 0x20
		/// <summary>
		/// </summary>
		/// Needed another place to put this, might remove from code later
		public enum SpecialCondition
		{
			NOTYPE			= 0x01,
			IGNOREPKMNTYPES	= 0x02,
			NOWEIGHTING		= 0x04,
			NOCRITICAL		= 0x08,
			NOREFLECT		= 0x10,
			SELFCONFUSE		= 0x20
		}

		public InBattleMove(Moves move) : base(move)
		{
			//battle	= battle
			BaseDamage	= _base.BaseDamage;
			Type		= _base.Type;
			Accuracy	= _base.Accuracy;
			PP			= base.PP;
			TotalPP		= base.TotalPP;
			AddlEffect	= _base.Effects;
			Targets		= base.Targets;
			Priority	= _base.Priority;
			Flag		= base.Flag;
			//thismove	= move
			//name		= ""
			MoveId		= base.MoveId;
		}

		public InBattleMove(Move move) : this(move.MoveId)
		{
			PP		= move.PP;
			TotalPP = move.TotalPP;
			CalcMoveFunc();
		}

		/*public InBattleMove(Battle battle, Move move) : base(move.MoveId)
		{
		}

		public static implicit operator Move(Battle.InBattleMove input)
		{

		}

		public void GetBattle(Battle battle)
		{

		}*/
		public void CalcMoveFunc()//(ref Battle.InBattleMove move)
		{
			//Effect function;
			switch ((Effect)Function)
			{
				case Effect.Confusion:
					//battle		= battle
					BaseDamage = 40;
					Type = Types.NONE;
					Accuracy = 100;
					PP = 0; //-1;
					//TotalPP	= ;
					AddlEffect = 0;
					Targets = Target.NoTarget;
					Priority = 0;
					Flag = new Flags();
					//thismove		= move
					//name			= ""
					MoveId = Moves.NONE;
					Category = Category.PHYSICAL;
					break;
				case Effect.Struggle:
				case Effect.x000:
					break;
				case Effect.x001:
					UnuseableInGravity = true;
					break;
				case Effect.x002:
				case Effect.x003:
				case Effect.x004:
				case Effect.x005:
				case Effect.x006:
				case Effect.x007:
				case Effect.x008:
				case Effect.x009:
				case Effect.x00A:
				case Effect.x00B:
				case Effect.x00C:
				case Effect.x00D:
				case Effect.x00E:
				case Effect.x00F:
				case Effect.x010:
				case Effect.x011:
				case Effect.x012:
				case Effect.x013:
				case Effect.x014:
				case Effect.x015:
				case Effect.x016:
				case Effect.x017:
				case Effect.x018:
				case Effect.x019:
				case Effect.x01A:
				case Effect.x01B:
				case Effect.x01C:
				case Effect.x01D:
				case Effect.x01E:
				case Effect.x01F:
				case Effect.x020:
				case Effect.x021:
				case Effect.x022:
				case Effect.x023:
				case Effect.x024:
				case Effect.x025:
				case Effect.x026:
				case Effect.x027:
				case Effect.x028:
				case Effect.x029:
				case Effect.x02A:
				case Effect.x02B:
				case Effect.x02C:
				case Effect.x02D:
				case Effect.x02E:
				case Effect.x02F:
				case Effect.x030:
				case Effect.x031:
				case Effect.x032:
				case Effect.x033:
				case Effect.x034:
				case Effect.x035:
				case Effect.x036:
				case Effect.x037:
				case Effect.x038:
				case Effect.x039:
				case Effect.x03A:
				case Effect.x03B:
				case Effect.x03C:
				case Effect.x03D:
				case Effect.x03E:
				case Effect.x03F:
				case Effect.x040:
				case Effect.x041:
				case Effect.x042:
				case Effect.x043:
				case Effect.x044:
				case Effect.x045:
				case Effect.x046:
				case Effect.x047:
				case Effect.x048:
				case Effect.x049:
				case Effect.x04A:
				case Effect.x04B:
				case Effect.x04C:
				case Effect.x04D:
				case Effect.x04E:
				case Effect.x04F:
				case Effect.x050:
				case Effect.x051:
				case Effect.x052:
				case Effect.x053:
				case Effect.x054:
				case Effect.x055:
				case Effect.x056:
				case Effect.x057:
				case Effect.x058:
				case Effect.x059:
				case Effect.x05A:
				case Effect.x05B:
				case Effect.x05C:
				case Effect.x05D:
				case Effect.x05E:
				case Effect.x05F:
				case Effect.x060:
				case Effect.x061:
				case Effect.x062:
				case Effect.x063:
				case Effect.x064:
				case Effect.x065:
				case Effect.x066:
				case Effect.x067:
				case Effect.x068:
				case Effect.x069:
				case Effect.x06A:
				case Effect.x06B:
				case Effect.x06C:
				case Effect.x06D:
				case Effect.x06E:
				case Effect.x06F:
				case Effect.x070:
				case Effect.x071:
				case Effect.x072:
				case Effect.x073:
				case Effect.x074:
				case Effect.x075:
				case Effect.x076:
				case Effect.x077:
				case Effect.x078:
				case Effect.x079:
				case Effect.x07A:
				case Effect.x07B:
				case Effect.x07C:
				case Effect.x07D:
				case Effect.x07E:
				case Effect.x07F:
				case Effect.x080:
				case Effect.x081:
				case Effect.x082:
				case Effect.x083:
				case Effect.x084:
				case Effect.x085:
				case Effect.x086:
				case Effect.x087:
				case Effect.x088:
				case Effect.x089:
				case Effect.x08A:
				case Effect.x08B:
				case Effect.x08C:
				case Effect.x08D:
				case Effect.x08E:
				case Effect.x08F:
				case Effect.x090:
				case Effect.x091:
				case Effect.x092:
				case Effect.x093:
				case Effect.x094:
				case Effect.x095:
				case Effect.x096:
				case Effect.x097:
				case Effect.x098:
				case Effect.x099:
				case Effect.x09A:
				case Effect.x09B:
				case Effect.x09C:
				case Effect.x09D:
				case Effect.x09E:
				case Effect.x09F:
				case Effect.x0A0:
				case Effect.x0A1:
				case Effect.x0A2:
				case Effect.x0A3:
				case Effect.x0A4:
				case Effect.x0A5:
				case Effect.x0A6:
				case Effect.x0A7:
				case Effect.x0A8:
				case Effect.x0A9:
				case Effect.x0AA:
				case Effect.x0AB:
				case Effect.x0AC:
				case Effect.x0AD:
				case Effect.x0AE:
				case Effect.x0AF:
				case Effect.x0B0:
				case Effect.x0B1:
				case Effect.x0B2:
				case Effect.x0B3:
				case Effect.x0B4:
				case Effect.x0B5:
				case Effect.x0B6:
				case Effect.x0B7:
				case Effect.x0B8:
				case Effect.x0B9:
				case Effect.x0BA:
				case Effect.x0BB:
				case Effect.x0BC:
				case Effect.x0BD:
				case Effect.x0BE:
				case Effect.x0BF:
				case Effect.x0C0:
				case Effect.x0C1:
				case Effect.x0C2:
				case Effect.x0C3:
				case Effect.x0C4:
				case Effect.x0C5:
				case Effect.x0C6:
				case Effect.x0C7:
				case Effect.x0C8:
				case Effect.x0C9:
				case Effect.x0CA:
				case Effect.x0CB:
				case Effect.x0CC:
				case Effect.x0CD:
				case Effect.x0CE:
				case Effect.x0CF:
				case Effect.x0D0:
				case Effect.x0D1:
				case Effect.x0D2:
				case Effect.x0D3:
				case Effect.x0D4:
				case Effect.x0D5:
				case Effect.x0D6:
				case Effect.x0D7:
				case Effect.x0D8:
				case Effect.x0D9:
				case Effect.x0DA:
				case Effect.x0DB:
				case Effect.x0DC:
				case Effect.x0DD:
				case Effect.x0DE:
				case Effect.x0DF:
				case Effect.x0E0:
				case Effect.x0E1:
				case Effect.x0E2:
				case Effect.x0E3:
				case Effect.x0E4:
				case Effect.x0E5:
				case Effect.x0E6:
				case Effect.x0E7:
				case Effect.x0E8:
				case Effect.x0E9:
				case Effect.x0EA:
				case Effect.x0EB:
				case Effect.x0EC:
				case Effect.x0ED:
				case Effect.x0EE:
				case Effect.x0EF:
				case Effect.x0F0:
				case Effect.x0F1:
				case Effect.x0F2:
				case Effect.x0F3:
				case Effect.x0F4:
				case Effect.x0F5:
				case Effect.x0F6:
				case Effect.x0F7:
				case Effect.x0F8:
				case Effect.x0F9:
				case Effect.x0FA:
				case Effect.x0FB:
				case Effect.x0FC:
				case Effect.x0FD:
				case Effect.x0FE:
				case Effect.x0FF:
				case Effect.x100:
				case Effect.x101:
				case Effect.x102:
				case Effect.x103:
				case Effect.x104:
				case Effect.x105:
				case Effect.x106:
				case Effect.x107:
				case Effect.x108:
				case Effect.x109:
				case Effect.x10A:
				case Effect.x10B:
				case Effect.x10C:
				case Effect.x10D:
				case Effect.x10E:
				case Effect.x10F:
				case Effect.x110:
				case Effect.x111:
				case Effect.x112:
				case Effect.x113:
				case Effect.x114:
				case Effect.x115:
				case Effect.x116:
				case Effect.x117:
				case Effect.x118:
				case Effect.x119:
				case Effect.x11A:
				case Effect.x11B:
				case Effect.x11C:
				case Effect.x11D:
				case Effect.x11E:
				case Effect.x11F:
				case Effect.x120:
				case Effect.x121:
				case Effect.x122:
				case Effect.x123:
				case Effect.x124:
				case Effect.x125:
				case Effect.x133:
				case Effect.x134:
				case Effect.x135:
				case Effect.x136:
				case Effect.x137:
				case Effect.x138:
				case Effect.x139:
				case Effect.x13A:
				case Effect.x13B:
				case Effect.x13C:
				case Effect.x13D:
				case Effect.x13E:
				case Effect.x13F:
				case Effect.x140:
				case Effect.x141:
				case Effect.x142:
				case Effect.x143:
				case Effect.x144:
				case Effect.x145:
				case Effect.x146:
				case Effect.x147:
				case Effect.x148:
				case Effect.x149:
				case Effect.x14A:
				case Effect.x14B:
				case Effect.x14C:
				case Effect.x14D:
				case Effect.x14E:
				case Effect.x14F:
				case Effect.x150:
				case Effect.x151:
				case Effect.x152:
				case Effect.x153:
				case Effect.x154:
				case Effect.x155:
				case Effect.x156:
				case Effect.x157:
				case Effect.x158:
					break;
				case Effect.FailedMove:
					//by default, both should result in failure
					//GameVariables.battle.pbDisplay("But it failed!");
					break;
				case Effect.UnimplementedMove:
				default:
					break;
			}
			//return move;
		}
	}

	public class DamageState
	{
		/// <summary>
		/// HP lost by opponent, inc. HP lost by a substitute
		/// </summary>
		public int HpLost { get; set; }
		/// <summary>
		/// Critical hit flag
		/// </summary>
		public bool Critical { get; set; }
		/// <summary>
		/// Calculated damage
		/// </summary>
		public int CalcDamage { get; set; }
		/// <summary>
		/// Type effectiveness
		/// </summary>
		public int TypeMod { get; set; }
		/// <summary>
		/// A substitute took the damage
		/// </summary>
		public bool Substitute { get; set; }
		/// <summary>
		/// Focus Band used
		/// </summary>
		public bool FocusBand { get; set; }
		/// <summary>
		/// Focus Sash used
		/// </summary>
		public bool FocusSash { get; set; }
		/// <summary>
		/// Sturdy ability used
		/// </summary>
		public bool Sturdy { get; set; }
		/// <summary>
		/// Damage was endured
		/// </summary>
		public bool Endured { get; set; }
		/// <summary>
		/// A type-resisting berry was used
		/// </summary>
		public bool BerryWeakened { get; set; }

		public void Reset()
		{
			HpLost        = 0;
			Critical      = false;
			CalcDamage    = 0;
			TypeMod       = 0;
			Substitute    = false;
			FocusBand     = false;
			FocusSash     = false;
			Sturdy        = false;
			Endured       = false;
			BerryWeakened = false;
		}
	}

	/// <summary>
	/// Success state (used for Battle Arena)
	/// </summary>
	public class SuccessState
	{
		/// <summary>
		/// Type effectiveness
		/// </summary>
		public int TypeMod { get; set; }
		/// <summary>
		/// null - not used, 0 - failed, 1 - succeeded
		/// </summary>
		/// instead of an int or enum
		/// 0 - not used, 1 - failed, 2 - succeeded
		public bool? UseState { get; set; }
		public bool Protected { get; set; }
		public int Skill { get; private set; }

		public SuccessState()
		{
			Clear();
		}

		public void Clear()
		{
			TypeMod		= 4;
			UseState	= null;
			Protected	= false;
			Skill		= 0;
		}

		public void UpdateSkill()
		{
			if (!UseState.Value && !Protected)
				Skill -= 2;
			else if (UseState.Value)
			{
				if (TypeMod > 4)
					Skill += 2; // "Super effective"
				else if (TypeMod >= 1 && TypeMod < 4)
					Skill -= 1; // "Not very effective"
				else if (TypeMod == 0)
					Skill -= 2; // Ineffective
				else
					Skill += 1;
			}
			TypeMod = 4;
			UseState = false;
			Protected = false;
		}
	}

	/// <summary>
	/// Options made on a given turn, per pokemon.
	/// </summary>
	/// ToDo: Make a logger of this as a List<> to document a match history.
	/// ToDo: If making logger, consider documenting math/results as well...
	public class Choice
	{
		public ChoiceAction Action { get; private set; }
		/// <summary>
		/// Index of Action being used
		/// </summary>
		public int Index { get; private set; }
		public InBattleMove Move { get; private set; }
		public int Target { get; private set; }

		/// <summary>
		/// If action you're choosing to take is to Attack with a Move
		/// </summary>
		/// <param name="action"></param>
		/// <param name="move"></param>
		/// <param name="target"></param>
		public Choice (ChoiceAction action, int moveIndex, InBattleMove move, int target = -1)
		{
			Action = action;
			Index = moveIndex;
			Move = move;
			Target = target;
		}

		/// <summary>
		/// If action you're choosing to take is to Switch Pkmns
		/// </summary>
		/// <param name="action"></param>
		/// <param name="pkmnIndex"></param>
		public Choice (ChoiceAction action, int pkmnIndex)
		{
			Action = action;
			Index = pkmnIndex;
		}

		/// <summary>
		/// If action you're choosing to take is to Use an Item on a Pkmn
		/// </summary>
		/// <param name="action"></param>
		/// <param name="itemIndex"></param>
		/// <param name="pkmnTarget"></param>
		public Choice (ChoiceAction action, int itemIndex, int pkmnTarget)
		{
			Action = action;
			Index = itemIndex;
			Target = pkmnTarget;
		}

		/// <summary>
		/// If action you're choosing to take is to Flee, Call Pokemon, or Nothing
		/// </summary>
		public Choice (ChoiceAction action = 0)
		{
			Action = action;
		}
	}

	public enum ChoiceAction
	{
		/// <summary>
		/// IsFainted
		/// </summary>
		NoAction = 0,
		UseMove = 1,
		SwitchPokemon = 2,
		UseItem = 3,
		CallPokemon = 4,
		Run = 5
	}

	public enum BattleResults
	{
		InProgress = -1,
		/// <summary>
		/// 0 - Undecided or aborted
		/// </summary>
		ABORTED = 0,
		/// <summary>
		/// 1 - Player won
		/// </summary>
		WON = 1,
		/// <summary>
		/// 2 - Player lost
		/// </summary>
		LOST = 2,
		/// <summary>
		/// 3 - Player or wild Pokémon ran from battle, or player forfeited the match
		/// </summary>
		FORFEIT = 3,
		/// <summary>
		/// 4 - Wild Pokémon was caught
		/// </summary>
		CAPTURED = 4,
		/// <summary>
		/// 5 - Draw
		/// </summary>
		DRAW = 5
	}
	#endregion
}
namespace PokemonUnity
{
	public enum Stats
	{
		ATTACK,
		DEFENSE,
		SPEED,
		SPATK,
		SPDEF,
		ACCURACY,
		EVASION,
		/// <summary>
		/// Use minus 2 when reflecting in IV, 
		/// as accuracy and evasion are battle only
		/// </summary>
		/// Should HP be here?
		HP
	}
	public enum Weather
	{
		NONE,
		RAINDANCE,
		HEAVYRAIN,
		SUNNYDAY,
		HARSHSUN,
		SANDSTORM,
		STRONGWINDS,
		HAIL
	}
	/// <summary>
	/// Terrain Tags or Tiles a player can be stepping on;
	/// used to contruct map floor plane
	/// </summary>
	public enum Terrain
	{
		Grass,
		Sand,
		Rock,
		DeepWater,
		StillWater,
		Water,
		TallGrass,
		SootGrass,
		Puddle
	}
	public enum Environment
	{
		None,
		/// <summary>
		/// Normal Grass, and Sooty Tall Grass, are both grass but different colors
		/// </summary>
		Grass,
		Cave,
		Sand,
		Rock,
		MovingWater,
		StillWater,
		Underwater,
		/// <summary>
		/// Tall Grass
		/// </summary>
		TallGrass,
		Forest,
		Snow,
		Volcano,
		Graveyard,
		Sky,
		Space
	}
}