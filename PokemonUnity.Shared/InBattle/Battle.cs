using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Battle;
using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Localization;

namespace PokemonUnity.Battle
{
	/// <summary>
	/// </summary>
	public partial class Battle //: UnityUtilityIntegration
	{
		#region Variables
		/// <summary>
		/// Scene object for this battle
		/// </summary>
		public void SetScene(int scene) { }//this.scene = scene; }
		public Scene scene { get; private set; }
		/// <summary>
		/// Decision: 0=undecided; 1=win; 2=loss; 3=escaped; 4=caught
		/// </summary>
		public BattleResults decision { get; set; }
		/// <summary>
		/// Internal battle flag
		/// </summary>
		public Battle InternalBattle (bool internalBattle) { internalbattle = internalBattle; return this; }
		public bool internalbattle { get; set; }
		/// <summary>
		/// Double battle flag
		/// </summary>
		public bool doublebattle { get; private set; }
		public bool isDoubleBattleAllowed { get
			{
				if (!fullparty1 && party1.Length > Core.MAXPARTYSIZE) return false;
				if (!fullparty2 && party2.Length > Core.MAXPARTYSIZE) return false;

				/*
				//Trainer[] _opponent = this.opponent;
				//Trainer[] _player = this.player;

				#region Wild battle
				if (_opponent == null)
				{
					if (party2.Length == 1)
						return false;
					else if (party2.Length == 2)
						return true;
					else
						return false;
				}
				#endregion Wild
				#region Trainer battle
				else
				{
					if (_opponent != null)
					{
						if (_opponent.Length == 1)
							_opponent = _opponent[0];
						else if (_opponent.Length != 2)
							return false;
					}

					//_player = _player

					if (_player != null)
					{
						if (_player.Length == 1)
							_player = _player[0];
						else if (_player.Length != 2)
							return false;
					}

					if (_opponent != null)
					{
						int sendout1 = pbFindNextUnfainted(party2, 0, pbSecondPartyBegin(1));
						int sendout2 = pbFindNextUnfainted(party2, pbSecondPartyBegin(1));
						if (sendout1 < 0 || sendout2 < 0) return false;
					}
					else
					{
						int sendout1 = pbFindNextUnfainted(party2, 0);
						int sendout2 = pbFindNextUnfainted(party2, sendout1 + 1);
						if (sendout1 < 0 || sendout2 < 0) return false;
					}
				}
				#endregion Trainer

				if (_player != null) 
				{
					int sendout1 = pbFindNextUnfainted(party1, 0, pbSecondPartyBegin(0));
					int sendout2 = pbFindNextUnfainted(party1, pbSecondPartyBegin(0));
					if (sendout1 < 0 || sendout2 < 0) return false;
				}
				else //AI:wild vs wild?
				{
					int sendout1 = pbFindNextUnfainted(party1, 0);
					int sendout2 = pbFindNextUnfainted(party1, sendout1 + 1);
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
		public Character.Player player { get; private set; }
		/// <summary>
		/// Opponent trainer
		/// </summary>
		/// ToDo: Make Array, if null => wild encounter
		/// If wild encounter...
		/// Dont show pokeballs,
		/// Dont show trainer sprite
		public Trainer opponent { get; private set; }
		/// <summary>
		/// Player's Pokémon party
		/// </summary>
		public Pokemon[] party1 { get; private set; }
		/// <summary>
		/// Foe's Pokémon party
		/// </summary>
		public Pokemon[] party2 { get; private set; }
		/// <summary>
		/// Pokémon party for All Trainers in Battle.
		/// Array[4,6] = 0: Player, 1: Foe, 2: Ally, 3: Foe's Ally 
		/// </summary>
		public Pokemon[,] party { get; private set; }
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
		public Pokemon[] battlers { get; private set; }
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
		/// public List<FieldEffects> field { get; private set; }
		public Effects.Field field { get; private set; }
		/// <summary>
		/// Battle surroundings;
		/// Environment node is used for background animation, 
		/// that's displayed behind the floor tile
		/// </summary>
		/// ToDo: Might be a static get value from global/map variables.
		public Environment environment { get; set; }
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
						return Weather.NONE;
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
		/// <remarks>
		/// Execute whaatever move/function is stored in this variable
		/// </remarks>
		/// Func<PokeBattle>
		public Move struggle { get; private set; } 
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
		public Moves lastMoveUsed { get; set; }
		/// <summary>
		/// Last move user
		/// </summary>
		public int lastMoveUser { get; set; }
		/// <summary>
		/// Battle index of each trainer's Pokémon to Mega Evolve
		/// </summary>
		/// Instead of reflecting entire party, it displays for active on field?
		public bool?[][] megaEvolution { get; private set; }
		/// <summary>
		/// Whether Amulet Coin's effect applies
		/// </summary>
		public bool amuletcoin { get; private set; }
		/// <summary>
		/// Money gained in battle by using Pay Day
		/// </summary>
		public int extramoney { get; set; }
		/// <summary>
		/// Whether Happy Hour's effect applies
		/// </summary>
		public bool doublemoney { get; set; }
		/// <summary>
		/// Speech by opponent when player wins
		/// </summary>
		//ToDo: opponent.ScriptBattleEnd
		public string endspeech { get { return string.Empty; } }
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
		/// </summary>
		//ToDo: Dictionary<string,bool> 
		public List<string> rules { get; private set; }
		/// <summary>
		/// Counter to track number of turns for battle
		/// </summary>
		public byte turncount { get; set; }
		public Pokemon[] priority { get; private set; }
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
		//private Player Player { get; set; }

		//ToDo: Fix here... maybe new scene variable?...
		//public BattleAnimationHandler BattleScene { get; private set; }
		//ToDo: Implement Display Text on Screen function
		/// <summary>
		/// Displays a message on screen, and wait for player input
		/// </summary>
		/// <param name="text"></param>
		public void pbDisplay(string text) { }
		/// <summary>
		/// Displays a message on screen, 
		/// but will continue without player input after short delay
		/// </summary>
		/// <param name="text"></param>
		public void pbDisplayBrief(string text) { }
		public void pbDisplayPaused(string text) { }
		public string Display
		{
			/* Don't need a get, since value is not being stored/logged
			 * there's no method or caller that's going to fetch value
			private get
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
		/* ToDo: Move to Pokemon? => Display on PokemonUI 
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
				return (int)species > 0 && player.playerPokedex[(int)species].HasValue ? player.playerPokedex[(int)species].Value : false;
			}
			set
			{
				if ((int)species > 0) player.playerPokedex[(int)species] = true; 
			}
		}*/
		/// <summary>
		/// Match history for each pokemon action stored as a log 
		/// </summary>
		/// In order to create Replay; Need to store:
		/// Action Pokemon/Player took
		/// Result of Action (sucessful?/amount+-)
		/// State of Pokemons (status)
		public IList<Choice[]> Log { get; private set; }
		#endregion

		#region Constructor
		/// <summary>
		/// </summary>
		/// ToDo: Make a constructor to pass P1/P2 variables
		/// Cant have a battle without first establishing who you're battling
		public Battle(Scene scene, Trainer player, Trainer opponent)
		{
			PokemonUnity.Monster.Pokemon[] p1 = player.Party;
			PokemonUnity.Monster.Pokemon[] p2 = opponent.Party;
			if (p1.Length == 0) {
				//raise ArgumentError.new(_INTL("Party 1 has no Pokémon."))
				GameDebug.LogError("Party 1 has no Pokémon.");
				return;
			}
		
			if (p2.Length == 0) { 
				//raise ArgumentError.new(_INTL("Party 2 has no Pokémon."))
				GameDebug.LogError("Party 2 has no Pokémon.");
				return;
			}
		
			if (p2.Length > 2 && !opponent.IsNotNullOrNone()) { //ID == TrainerTypes.WildPokemon
				//raise ArgumentError.new(_INTL("Wild battles with more than two Pokémon are not allowed."))
				GameDebug.LogError("Wild battles with more than two Pokémon are not allowed.");
				return;
			}

			this.scene = scene;
			decision = 0;
			internalbattle = true;
			doublebattle = false;
			cantescape = false;
			shiftStyle = true;
			battlescene = true;
			debug = false;
			//debugupdate = 0;

			//if (opponent.IsNotNullOrNone() && player.Length == 1) 
			//	this.player = player[0]; 

			//if (opponent.IsNotNullOrNone() && opponent.Length == 1)
			//	this.opponent = opponent[0];

			this.player = Game.GameData.Player; //player;
			this.opponent = opponent;
			party1 = Pokemon.GetBattlers(p1, this); //ToDo: Redo this...
			party2 = Pokemon.GetBattlers(p2, this); //ToDo: Redo this...

			party1order = new List<int>();
			//the #12 represents a double battle with 2 trainers using 6 pokemons each on 1 side
			for (int i = 0; i < 12; i++)
				party1order.Add(i);

			party2order = new List<int>();
			//for i in 0...12;party2order.push(i); }
			for (int i = 0; i < 12; i++) 
				party2order.Add(i);

			fullparty1 = false;
			fullparty2 = false;
			battlers = new Pokemon[4]; 
			items = new List<Items>(); //null;

			sides = new Effects.Side[] {new Effects.Side(),		// Player's side
										new Effects.Side()};	// Foe's side
			//sides = new Effects.Side[2];						
			field = new Effects.Field();                        // Whole field (gravity/rooms)
			environment = Environment.None;						// e.g. Tall grass, cave, still water
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

			megaEvolution = new bool?[][] { new bool?[0], new bool?[0] }; 	//list, [2,], or [2][]...
			if (this.player != null) 										//ToDo: single/double or party?
				megaEvolution[0] = new bool?[this.player.Party.Length]; 	//[-1] * player.Length; 
			else
				megaEvolution[0] = new bool?[]{ null }; 					//[-1];
			if (this.opponent != null)
				megaEvolution[1] = new bool?[this.opponent.Party.Length]; 	//[-1] * opponent.Length;
			else
				megaEvolution[1] = new bool?[]{ null }; 					//[-1];
			//megaEvolution = new bool?[player.Length + opponent.Length][]; 	
			//for(int i = 0; i <)

			amuletcoin = false;
			extramoney = 0;
			doublemoney = false;

			//endspeech = opponent.ScriptBattleEnd;
			//endspeech2 = "";
			//endspeechwin = "";
			//endspeechwin2 = "";

			rules = new List<string>(); //Dictionary<string,bool>?

			turncount = 0;

			//peer = PokeBattle_BattlePeer.create()

			priority = new Pokemon[4];

			//usepriority = false; //False is already default value; redundant.

			snaggedpokemon = new List<byte>();

			runCommand = 0;

			//if (Moves.STRUGGLE.GetType() == typeof(Moves))
			//	struggle = new Move(Moves.STRUGGLE);//new PokeBattle_Move(this, new Attack.Move(Moves.STRUGGLE)).pbFromPBMove(Moves.STRUGGLE);
			//else
				struggle = new PokeBattle_Struggle(this, new Attack.Move(Moves.NONE));

			//struggle.PP = -1;

			for (byte i = 0; i < 4; i++)
			{
				this.battlers[i] = new Pokemon(this, (sbyte)i).Initialize(new PokemonUnity.Monster.Pokemon(), (sbyte)i);
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
		//public Battle(Player player, Trainer opponent) : this(player.Trainer, opponent)
		//{
		//	player = player;
		//}
		//public Battle(UnityEngine.GameObject battleScene) //: this(player.Trainer, opponent)
		//{
		//	//ToDo: Register Unity UI instance to variables
		//	//BattlePokemonHandler
		//	//Player = player;
		//}
		///// <summary>
		///// </summary>
		///// <param name="p1"></param>
		///// <param name="p2"></param>
		///// <param name="pvpMultiPlayer"></param>
		///// ToDo: Wacky idea for double battles, 1 v 2 or 2 v 2.
		///// Should be able to support both player v ai, and player v player
		//public Battle(Trainer[] p1, Trainer[] p2, bool pvpMultiPlayer = false)
		//{
		//
		//}
		#endregion

		#region Method
		//public void StartBattle(bool canlose)
		//{
		//	//return this;
		//	Game.battle = this;
		//}
		public IEnumerator<Choice[]> StartBattle(bool canlose)
		{
			//return this;
			//Game.battle = this;
			while (this.decision == BattleResults.InProgress)
			{
				if(choices != null && choices.Length == battlers.Length)
				yield return choices;
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
			return Core.Rand.Next(index);
		}
		/// <summary>
		/// Returns the trainer party of pokemon at this index?
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		/// ToDo: Not implemented
		public Pokemon[] pbParty(int index)
		{
			return party1;
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
		public Pokemon pbCheckGlobalAbility(Abilities index)
		{
			//ToDo: return none, not null
			return null;
		}
		public bool pbAllFainted(Pokemon[] party)
		{
			return false;
		}
		public bool pbIsUnlosableItem(Pokemon party, Items item)
		{
			return false;
		}
		public bool pbCanSwitch(int index, int pkmn, bool item)
		{
			return false;
		}
		public bool pbCanSwitch(int index, int pkmn, bool item, bool uhh)
		{
			return false;
		}
		public bool OwnedByPlayer(int index)
		{
			return false; //throw new NotImplementedException();
		}
		public bool pbCommonAnimation(string animation, Pokemon atk, object uh)
		{
			return false;
		}
		#endregion

		#region Catching and storing Pokémon.
		public void StorePokemon(Monster.Pokemon pokemon)
		{
			//if(!pokemon.isShadow)
			//	//"Would you like to give a nickname to {1}?"
			//	if (DisplayConfirm(LanguageExtension.Translate(Text.ScriptTexts, "GiveNickname").Value))
			//	{
			//		string nick = string.Empty; //(string)scene.NameEntry(LanguageExtension.Translate(Text.ScriptTexts, "SetNick", pokemon.Species.ToString(TextScripts.Name)).Value, pokemon)
			//		//"{1}'s nickname?"
			//		if(!string.IsNullOrEmpty(nick)) pokemon.SetNickname(nick);
			//	}
			int oldcurbox = Game.GameData.Player.PC.ActiveBox;
			/*/bool success = false;
			//int i = 0; do { 
			//	//success = Game.GameData.Player.PC.addPokemon(pokemon); 
			//	success = Game.GameData.Player.PC.hasSpace();
			//	if(!success) Game.GameData.Player.PC.getIndexOfFirstEmpty
			//} while (!success); //ToDo: Cycle through all boxes unless they're all full
			//int storedbox = Game.GameData.Player.PC.StorePokemon(Game.Player, pokemon);*/
			int? storedbox = Game.GameData.Player.PC.getIndexOfFirstEmpty();
			if (!storedbox.HasValue) return;
			string creator = Game.GameData.Player.IsCreator ? Game.GameData.Player.Name : "someone"; //Game.GameData.Player.GetStorageCreator();
			string curboxname = Game.GameData.Player.PC.BoxNames[oldcurbox];
			string boxname = Game.GameData.Player.PC.BoxNames[storedbox.Value];
			//if (storedbox != oldcurbox) {
			//	if (Game.GameData.Player.IsCreator)
			//		Game.DisplayPaused("Box \"{1}\" on {2}'s PC was full.", curboxname, creator);
			//	else
			//		Game.DisplayPaused("Box \"{1}\" on someone's PC was full.", curboxname);
			//	Game.DisplayPaused("{1} was transferred to box \"{2}\".", pokemon.Name, boxname);
			//}
			//else {
			//	if (Game.GameData.Player.IsCreator)
			//		Game.DisplayPaused("{1} was transferred to {2}'s PC.", pokemon.Name, creator);
			//	else 
			//		Game.DisplayPaused("{1} was transferred to someone's PC.", pokemon.Name);
			//	Game.DisplayPaused("It was stored in box \"{1}\".", boxname);
			//}		
		}
		public void ThrowPokeball(int idxPokemon, Items ball, int? rareness = null, bool showplayer = false)
		{
			string itemname = ball.ToString(TextScripts.Name);
			Pokemon battler = null;
			if (isOpposing(idxPokemon))
				battler = battlers[idxPokemon];
			else
				battler = battlers[idxPokemon].OppositeOpposing;
			if (battler.isFainted())
				battler = battler.Partner;
			//"{1} threw one {2}!"
			//pbDisplayBrief(L(Text.ScriptTexts,"ThrowBall", Game.GameData.Player.Name, itemname));
			if (battler.isFainted())
			{
				//"But there was no target..."
				//Display(L(Text.ScriptTexts, "NoTarget"));
				return;
			}
			int shakes = 0; bool critical = false;
			if (opponent.IsNotNullOrNone())//.ID != TrainerTypes.WildPokemon)
				//&& (!IsSnagBall(ball) || !battler.isShadow))
			{
				//scene.ThrowAndDeflect(ball, 1);
				//"The Trainer blocked the Ball!\nDon't be a thief!"
				//Display(L(Text.ScriptTexts, "SnagRejected"));
			}
			else
			{
				//Monster.Pokemon pkmn = battler.pokemon;
				if (!rareness.HasValue)
				{
					rareness = (int)Game.PokemonData[battler.Species].Rarity;
				}
				int a = battler.TotalHP;
				int b = battler.HP;
				//ToDo: Ball Throwing Class?
				//rareness = BallHandlers.ModifyCatchRate(ball, rareness, battler);
				int x = (int)Math.Floor(((a * 3 - b * 2) * rareness.Value) / (a * 3f));
				if (battler.Status == Status.SLEEP || battler.Status == Status.FROZEN)
					x = (int)Math.Floor(x * 2.5f);
				else if (battler.Status != Status.NONE)
					x = (int)Math.Floor(x * 1.5f);
				int c = 0;
				//if (this.player == Trainer)
					if (Game.GameData.Player.PokedexCaught > 600)
						c = (int)Math.Floor(x * 2.5f / 6);
					else if (Game.GameData.Player.PokedexCaught > 450)
						c = (int)Math.Floor(x * 2f / 6);
					else if (Game.GameData.Player.PokedexCaught > 300)
						c = (int)Math.Floor(x * 1.5f / 6);
					else if (Game.GameData.Player.PokedexCaught > 150)
						c = (int)Math.Floor(x * 1f / 6);
					else if (Game.GameData.Player.PokedexCaught > 30)
						c = (int)Math.Floor(x * .5 / 6);
				//int shakes = 0; bool critical = false;
				if (x > 255)//|| BallHandlers.isUnconditional?(ball,self,battler)
					shakes = 4;
				else
				{
					if (x < 1) x = 1;
					int y = (int)Math.Floor(65536 / (Math.Pow((255.0 / x), 0.1875)));//(255.0 / x) ^ 0.1875
					if (Core.USECRITICALCAPTURE && pbRandom(256) < 0)
					{
						critical = true;
						if (pbRandom(65536) < y) shakes = 4;
					}
					else
					{
						if (pbRandom(65536)<y				) shakes+=1;
						if (pbRandom(65536)<y && shakes==1	) shakes+=1;
						if (pbRandom(65536)<y && shakes==2	) shakes+=1;
						if (pbRandom(65536)<y && shakes==3	) shakes+=1;						
					}
				}
			}
			GameDebug.Log($"[Threw Poké Ball] #{itemname}, #{shakes} shakes (4=capture)");
			//@scene.pbThrow(ball, (critical) ? 1 : shakes, critical, battler.Index, showplayer);
			//switch (shakes)
			//{
			//	case 0:
			//		pbDisplay(_INTL("Oh no! The Pokémon broke free!"));
			//		//BallHandlers.onFailCatch(ball,this,battler);
			//		break;
			//	case 1:
			//		pbDisplay(_INTL("Aww... It appeared to be caught!"));
			//		//BallHandlers.onFailCatch(ball,this,battler);
			//		break;
			//	case 2:
			//		pbDisplay(_INTL("Aargh! Almost had it!"));
			//		//BallHandlers.onFailCatch(ball,this,battler);
			//		break;
			//	case 3:
			//		pbDisplay(_INTL("Gah! It was so close, too!"));
			//		//BallHandlers.onFailCatch(ball,this,battler);
			//		break;
			//	case 4:
			//		pbDisplayBrief(_INTL("Gotcha! {1} was caught!", pokemon.Name));
			//		@scene.pbThrowSuccess();
			//		if (pbIsSnagBall(ball) && @opponent.IsNotNullOrNone())
			//		{
			//			pbRemoveFromParty(battler.Index, battler.pokemonIndex);
			//			battler.pbReset();
			//			battler.participants = new List<byte>();
			//		}
			//		else
			//			@decision = BattleResults.CAPTURED;
			//		if (pbIsSnagBall(ball))
			//		{
			//			pokemon.ot = this.player.Name;
			//			pokemon.trainerID = this.player.Trainer;
			//		}
			//		//BallHandlers.onCatch(ball,this,pokemon);
			//		pokemon.ballused = pbGetBallType(ball);
			//		pokemon.makeUnmega(); //rescue null
			//		pokemon.makeUnprimal(); //rescue null
			//		pokemon.pbRecordFirstMoves();
			//		if (Core.GAINEXPFORCAPTURE)
			//		{
			//			battler.captured = true;
			//			pbGainEXP();
			//			battler.captured = false;
			//		}
			//		if (!this.player.hasOwned(species))
			//		{
			//			this.player.setOwned(species);
			//			if (Game.Player.Pokedex)
			//			{ //Not Caught?
			//				pbDisplayPaused(_INTL("{1}'s data was added to the Pokédex.", pokemon.Name));
			//				@scene.pbShowPokedex(species);
			//			}
			//		}
			//		@scene.pbHideCaptureBall();
			//		if (pbIsSnagBall(ball) && @opponent)
			//		{
			//			pokemon.pbUpdateShadowMoves(); //rescue null
			//			@snaggedpokemon.Add(pokemon);
			//		}
			//		else
			//			pbStorePokemon(pokemon);
			//		break;
			//}
		}
		#endregion

		#region Info about battle.
		public bool pbDoubleBattleAllowed()
		{
    if !@fullparty1 && @party1.length>Core.MAXPARTYSIZE
      return false
    end
    if !@fullparty2 && @party2.length>Core.MAXPARTYSIZE
      return false
    end
    _opponent=@opponent
    _player=@player
    // Wild battle
    if !_opponent
      if @party2.length==1
        return false
      elsif @party2.length==2
        return true
      else
        return false
      end
    // Trainer battle
    else
      if _opponent.is_a?(Array)
        if _opponent.length==1
          _opponent=_opponent[0]
        elsif _opponent.length!=2
          return false
        end
      end
      _player=_player
      if _player.is_a?(Array)
        if _player.length==1
          _player=_player[0]
        elsif _player.length!=2
          return false
        end
      end
      if _opponent.is_a?(Array)
        sendout1=pbFindNextUnfainted(@party2,0,pbSecondPartyBegin(1))
        sendout2=pbFindNextUnfainted(@party2,pbSecondPartyBegin(1))
        return false if sendout1<0 || sendout2<0
      else
        sendout1=pbFindNextUnfainted(@party2,0)
        sendout2=pbFindNextUnfainted(@party2,sendout1+1)
        return false if sendout1<0 || sendout2<0
      end
    end
    if _player.is_a?(Array)
      sendout1=pbFindNextUnfainted(@party1,0,pbSecondPartyBegin(0))
      sendout2=pbFindNextUnfainted(@party1,pbSecondPartyBegin(0))
      return false if sendout1<0 || sendout2<0
    else
      sendout1=pbFindNextUnfainted(@party1,0)
      sendout2=pbFindNextUnfainted(@party1,sendout1+1)
      return false if sendout1<0 || sendout2<0
    end
    return true
		}
		#endregion

		#region Get Battler Info.
		public bool isOpposing(int index)
		{
			return (index % 2) == 1;
		}
		public bool pbOwnedByPlayer(int index)
		{
			return false;
		}
/// Only used for Wish
		def pbThisEx(battlerindex, pokemonindex)

	party=pbParty(battlerindex)
    if pbIsOpposing? (battlerindex)
      if @opponent
        return _INTL("The foe {1}", party[pokemonindex].name)
      else
        return _INTL("The wild {1}", party[pokemonindex].name)
	  end
    else
      return _INTL("{1}", party[pokemonindex].name)
	end
  end
			
// Checks whether an item can be removed from a Pokémon.
  def pbIsUnlosableItem(pkmn,item)
    return true if pbIsMail?(item)
    return false if pkmn.effects.Transform
    if pkmn.ability == Abilities.MULTITYPE
      Items[] plates= new Items[] { Items.FISTPLATE, Items.SKYPLATE, Items.TOXICPLATE, Items.EARTHPLATE, Items.STONEPLATE,
               Items.INSECTPLATE, Items.SPOOKYPLATE, Items.IRONPLATE, Items.FLAMEPLATE, Items.SPLASHPLATE,
               Items.MEADOWPLATE, Items.ZAPPLATE, Items.MINDPLATE, Items.ICICLEPLATE, Items.DRACOPLATE,
               Items.DREADPLATE, Items.PIXIEPLATE };
      foreach (var i in plates) {
        return true if item == i
      end
    end
    combos=[[:GIRATINA,:GRISEOUSORB],
            [:GENESECT,:BURNDRIVE],
            [:GENESECT,:CHILLDRIVE],
            [:GENESECT,:DOUSEDRIVE],
            [:GENESECT,:SHOCKDRIVE],
            // Mega Stones
            [:ABOMASNOW,:ABOMASITE],
            [:ABSOL,:ABSOLITE],
            [:AERODACTYL,:AERODACTYLITE],
            [:AGGRON,:AGGRONITE],
            [:ALAKAZAM,:ALAKAZITE],
            [:ALTARIA,:ALTARIANITE],
            [:AMPHAROS,:AMPHAROSITE],
            [:AUDINO,:AUDINITE],
            [:BANETTE,:BANETTITE],
            [:BEEDRILL,:BEEDRILLITE],
            [:BLASTOISE,:BLASTOISINITE],
            [:BLAZIKEN,:BLAZIKENITE],
            [:CAMERUPT,:CAMERUPTITE],
            [:CHARIZARD,:CHARIZARDITEX],
            [:CHARIZARD,:CHARIZARDITEY],
            [:DIANCIE,:DIANCITE],
            [:GALLADE,:GALLADITE],
            [:GARCHOMP,:GARCHOMPITE],
            [:GARDEVOIR,:GARDEVOIRITE],
            [:GENGAR,:GENGARITE],
            [:GLALIE,:GLALITITE],
            [:GYARADOS,:GYARADOSITE],
            [:HERACROSS,:HERACRONITE],
            [:HOUNDOOM,:HOUNDOOMINITE],
            [:KANGASKHAN,:KANGASKHANITE],
            [:LATIAS,:LATIASITE],
            [:LATIOS,:LATIOSITE],
            [:LOPUNNY,:LOPUNNITE],
            [:LUCARIO,:LUCARIONITE],
            [:MANECTRIC,:MANECTITE],
            [:MAWILE,:MAWILITE],
            [:MEDICHAM,:MEDICHAMITE],
            [:METAGROSS,:METAGROSSITE],
            [:MEWTWO,:MEWTWONITEX],
            [:MEWTWO,:MEWTWONITEY],
            [:PIDGEOT,:PIDGEOTITE],
            [:PINSIR,:PINSIRITE],
            [:SABLEYE,:SABLENITE],
            [:SALAMENCE,:SALAMENCITE],
            [:SCEPTILE,:SCEPTILITE],
            [:SCIZOR,:SCIZORITE],
            [:SHARPEDO,:SHARPEDONITE],
            [:SLOWBRO,:SLOWBRONITE],
            [:STEELIX,:STEELIXITE],
            [:SWAMPERT,:SWAMPERTITE],
            [:TYRANITAR,:TYRANITARITE],
            [:VENUSAUR,:VENUSAURITE],
            // Primal Reversion stones
            [:KYOGRE,:BLUEORB],
            [:GROUDON,:REDORB]
           ]
    foreach (var i in combos) {
      if (pkmn.Species == (Pokemons)i[0] && item == (Items)i[1])
        return true
      end
    end
    return false
  end

  def pbCheckGlobalAbility(a)
    for (int i = 0; i < 4; i++) { // in order from own first, opposing first, own second, opposing second
      if @battlers[i].hasWorkingAbility(a)
        return @battlers[i]
      end
    end
    return null
  end

  def nextPickupUse
    @nextPickupUse+=1
    return @nextPickupUse
  end

		#endregion

		#region Player-related Info.
			def pbPlayer
    if @player.is_a?(Array)
      return @player[0]
    else
      return @player
    end
  end

  def pbGetOwnerItems(battlerIndex)
    return [] if !@items
    if pbIsOpposing?(battlerIndex)
      if @opponent.is_a?(Array)
        return (battlerIndex==1) ? @items[0] : @items[1]
      else
        return @items
      end
    else
      return []
    end
  end

  def pbSetSeen(pokemon)
    if pokemon && @internalbattle
      self.pbPlayer.seen[pokemon.species]=true
      pbSeenForm(pokemon)
    end
  end

  def pbGetMegaRingName(battlerIndex)
    if pbBelongsToPlayer?(battlerIndex)
      foreach (var i in MEGARINGS) {
        next if !hasConst?(PBItems,i)
        return i.ToString(TextScripts.Name) if $PokemonBag.pbQuantity(i)>0
      end
    end
    // Add your own Mega objects for particular trainer types here
//    if pbGetOwner(battlerIndex).trainertype == TrainerTypes.BUGCATCHER
//      return _INTL("Mega Net")
//    end
    return _INTL("Mega Ring")
  end

  def pbHasMegaRing(battlerIndex)
    return true if !pbBelongsToPlayer?(battlerIndex)
    foreach (var i in MEGARINGS) {
      next if !hasConst?(PBItems,i)
      return true if $PokemonBag.pbQuantity(i)>0
    end
    return false
  end
		#endregion

		#region Get party info, manipulate parties.
		int PokemonCount(PokemonUnity.Monster.Pokemon[] party)
		{
			int count = 0;
			for (int i = 0; i < party.Length; i++)
			{
				if (party[i].Species == Pokemons.NONE) continue;
				if (party[i].HP > 0 && !party[i].isEgg) count += 1;
			}
			return count;
		}
		bool AllFainted(PokemonUnity.Monster.Pokemon[] party)
		{
			return PokemonCount(party) == 0;
		}
		int MaxLevel(PokemonUnity.Monster.Pokemon[] party)
		{
			int lv = 0;
			for (int i = 0; i < party.Length; i++)
			{
				if (party[i].Species == Pokemons.NONE) continue;
				if (lv < party[i].Level) lv = party[i].Level;
			}
			return lv;
		}
			def pbPokemonCount(party)
    count=0
    foreach (var i in party) {
      next if !i
      count+=1 if i.hp>0 && !i.isEgg?
    end
    return count
  end

  def pbAllFainted?(party)
    pbPokemonCount(party)==0
  end

  def pbMaxLevel(party)
    lv=0
    foreach (var i in party) {
      next if !i
      lv=i.level if lv<i.level
    end
    return lv
  end

  def pbMaxLevelFromIndex(index)
    party=pbParty(index)
    owner=(pbIsOpposing?(index)) ? @opponent : @player
    maxlevel=0
    if owner.is_a?(Array)
      start=0
      limit=pbSecondPartyBegin(index)
      start=limit if pbIsDoubleBattler?(index)
      for (int i = start; i < start+limit; i++) {
        next if !party[i]
        maxlevel=party[i].level if maxlevel<party[i].level
      end
    else
      foreach (var i in party) {
        next if !i
        maxlevel=i.level if maxlevel<i.level
      end
    end
    return maxlevel
  end

  def pbParty(index)
    return pbIsOpposing?(index) ? party2 : party1
  end

  def pbOpposingParty(index)
    return pbIsOpposing?(index) ? party1 : party2
  end

  def pbSecondPartyBegin(battlerIndex)
    if pbIsOpposing?(battlerIndex)
      return @fullparty2 ? 6 : 3
    else
      return @fullparty1 ? 6 : 3
    end
  end

  def pbPartyLength(battlerIndex)
    if pbIsOpposing?(battlerIndex)
      return (@opponent.is_a?(Array)) ? pbSecondPartyBegin(battlerIndex) : MAXPARTYSIZE
    else
      return @player.is_a?(Array) ? pbSecondPartyBegin(battlerIndex) : MAXPARTYSIZE
    end
  end

  def pbFindNextUnfainted(party,start,finish=-1)
    finish=party.length if finish<0
    for (int i = start; i < finish; i++) {
      next if !party[i]
      return i if party[i].hp>0 && !party[i].isEgg?
    end
    return -1
  end

  def pbGetLastPokeInTeam(index)
    party=pbParty(index)
    partyorder=(!pbIsOpposing?(index)) ? @party1order : @party2order
    plength=pbPartyLength(index)
    pstart=pbGetOwnerIndex(index)*plength
    lastpoke=-1
    for (int i = pstart; i < pstart+plength; i++) {
      p=party[partyorder[i]]
      next if !p || p.isEgg? || p.hp<=0
      lastpoke=partyorder[i]
    end
    return lastpoke
  end

  def pbFindPlayerBattler(pkmnIndex)
    battler=null
    for (int k = 0; k < 4; k++) {
      if !pbIsOpposing?(k) && @battlers[k].pokemonIndex==pkmnIndex
        battler=@battlers[k]
        break
      end
    end
    return battler
  end

  def pbIsOwner?(battlerIndex,partyIndex)
    secondParty=pbSecondPartyBegin(battlerIndex)
    if !pbIsOpposing?(battlerIndex)
      return true if !@player || !@player.is_a?(Array)
      return (battlerIndex==0) ? partyIndex<secondParty : partyIndex>=secondParty
    else
      return true if !@opponent || !@opponent.is_a?(Array)
      return (battlerIndex==1) ? partyIndex<secondParty : partyIndex>=secondParty
    end
  end

  def pbGetOwner(battlerIndex)
    if pbIsOpposing?(battlerIndex)
      if @opponent.is_a?(Array)
        return (battlerIndex==1) ? @opponent[0] : @opponent[1]
      else
        return @opponent
      end
    else
      if @player.is_a?(Array)
        return (battlerIndex==0) ? @player[0] : @player[1]
      else
        return @player
      end
    end
  end

  def pbGetOwnerPartner(battlerIndex)
    if pbIsOpposing?(battlerIndex)
      if @opponent.is_a?(Array)
        return (battlerIndex==1) ? @opponent[1] : @opponent[0]
      else
        return @opponent
      end
    else
      if @player.is_a?(Array)
        return (battlerIndex==0) ? @player[1] : @player[0]
      else
        return @player
      end
    end
  end

  def pbGetOwnerIndex(battlerIndex)
    if pbIsOpposing?(battlerIndex)
      return (@opponent.is_a?(Array)) ? ((battlerIndex==1) ? 0 : 1) : 0
    else
      return (@player.is_a?(Array)) ? ((battlerIndex==0) ? 0 : 1) : 0
    end
  end

  def pbBelongsToPlayer?(battlerIndex)
    if @player.is_a?(Array) && @player.length>1
      return battlerIndex==0
    else
      return (battlerIndex%2)==0
    end
    return false
  end

  def pbPartyGetOwner(battlerIndex,partyIndex)
    secondParty=pbSecondPartyBegin(battlerIndex)
    if !pbIsOpposing?(battlerIndex)
      return @player if !@player || !@player.is_a?(Array)
      return (partyIndex<secondParty) ? @player[0] : @player[1]
    else
      return @opponent if !@opponent || !@opponent.is_a?(Array)
      return (partyIndex<secondParty) ? @opponent[0] : @opponent[1]
    end
  end

  def pbAddToPlayerParty(pokemon)
    party=pbParty(0)
    for (int i = 0; i < party.length; i++) {
      party[i]=pokemon if pbIsOwner?(0,i) && !party[i]
    end
  end

  def pbRemoveFromParty(battlerIndex,partyIndex)
    party=pbParty(battlerIndex)
    side=(pbIsOpposing?(battlerIndex)) ? @opponent : @player
    order=(pbIsOpposing?(battlerIndex)) ? @party2order : @party1order
    secondpartybegin=pbSecondPartyBegin(battlerIndex)
    party[partyIndex]=null
    if !side || !side.is_a?(Array) // Wild or single opponent
      party.compact!
      for (int i = partyIndex; i < party.length+1; i++) {
        for (int j = 0; j < 4; j++) {
          next if !@battlers[j]
          if pbGetOwner(j)==side && @battlers[j].pokemonIndex==i
            @battlers[j].pokemonIndex-=1
            break
          end
        end
      end
      for (int i = 0; i < order.length; i++) {
        order[i]=(i==partyIndex) ? order.length-1 : order[i]-1
      end
    else
      if partyIndex<secondpartybegin-1
        for (int i = partyIndex; i < secondpartybegin; i++) {
          if i>=secondpartybegin-1
            party[i]=null
          else
            party[i]=party[i+1]
          end
        end
        for (int i = 0; i < order.length; i++) {
          next if order[i]>=secondpartybegin
          order[i]=(i==partyIndex) ? secondpartybegin-1 : order[i]-1
        end
      else
        for (int i = partyIndex; i < secondpartybegin+pbPartyLength; i++) {(battlerIndex)
          if i>=party.length-1
            party[i]=null
          else
            party[i]=party[i+1]
          end
        end
        for (int i = 0; i < order.length; i++) {
          next if order[i]<secondpartybegin
          order[i]=(i==partyIndex) ? secondpartybegin+pbPartyLength(battlerIndex)-1 : order[i]-1
        end
      end
    end
  end
		#endregion
		
		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		public bool CanShowCommands(int idxPokemon)
		{
			PokemonUnity.Battle.Pokemon thispkmn = @battlers[idxPokemon];
			if (thispkmn.isFainted()) return false;
			if (thispkmn.effects.TwoTurnAttack > 0) return false; 
			if (thispkmn.effects.HyperBeam > 0) return false; 
			if (thispkmn.effects.Rollout > 0) return false; 
			if (thispkmn.effects.Outrage > 0) return false; 
			if (thispkmn.effects.Uproar > 0) return false;
			if (thispkmn.effects.Bide > 0) return false;
			return true;
		}

	#region Attacking
		public bool CanShowFightMenu(int idxPokemon)
		{
			PokemonUnity.Battle.Pokemon thispkmn = @battlers[idxPokemon];
			if (!CanShowCommands(idxPokemon)) return false;

			// No moves that can be chosen
			if (!CanChooseMove(idxPokemon, 0, false) &&
			   !CanChooseMove(idxPokemon, 1, false) &&
			   !CanChooseMove(idxPokemon, 2, false) &&
			   !CanChooseMove(idxPokemon, 3, false))
				return false;

			// Encore
			if (thispkmn.effects.Encore > 0) return false;
			return true;
		}
    
		public bool CanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false)
		{
			Pokemon thispkmn = @battlers[idxPokemon];
			Attack.Move thismove = thispkmn.moves[idxMove];

			//ToDo: Array for opposing pokemons, [i] changes based on if double battle
			Pokemon opp1 = thispkmn.pbOpposing1;
			Pokemon opp2 = thispkmn.pbOpposing2;
			if (thismove != null || thismove.MoveId == 0) return false;
			if (thismove.PP <= 0 && thismove.TotalPP > 0 && !sleeptalk) {
				if (showMessages) pbDisplayPaused(_INTL("There's no PP left for this move!"));
				return false;
			}
			if (thispkmn.Item == Items.ASSAULT_VEST) {// && thismove.IsStatus?
				if (showMessages) pbDisplayPaused(_INTL("The effects of the {1} prevent status moves from being used!", thispkmn.Item.ToString(TextScripts.Name)));
				return false;
			}
			if ((int)thispkmn.effects.ChoiceBand >= 0 &&
			   (thispkmn.Item == Items.CHOICE_BAND ||
			   thispkmn.Item == Items.CHOICE_SPECS ||
			   thispkmn.Item == Items.CHOICE_SCARF))
			{
				bool hasmove = false;
				for (int i = 0; i < thispkmn.moves.Length; i++)
					if (thispkmn.moves[i].MoveId==thispkmn.effects.ChoiceBand) { 
						hasmove = true; break;
					}
				if (hasmove && thismove.MoveId != thispkmn.effects.ChoiceBand) {
					if (showMessages)
						pbDisplayPaused(_INTL("{1} allows the use of only {2}!",
							thispkmn.Item.ToString(TextScripts.Name),
							thispkmn.effects.ChoiceBand.ToString(TextScripts.Name)));
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
					if (showMessages) pbDisplayPaused(_INTL("{1} can't use the sealed {2}!", thispkmn.ToString(), thismove.MoveId.ToString(TextScripts.Name)));
					GameDebug.Log($"[CanChoose][#{opp1.ToString()} has: #{opp1.moves[0].MoveId.ToString(TextScripts.Name)}, #{opp1.moves[1].MoveId.ToString(TextScripts.Name)}, #{opp1.moves[2].MoveId.ToString(TextScripts.Name)}, #{opp1.moves[3].MoveId.ToString(TextScripts.Name)}]");
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
					if (showMessages) pbDisplayPaused(_INTL("{1} can't use the sealed {2}!", thispkmn.ToString(), thismove.MoveId.ToString(TextScripts.Name)));
					GameDebug.Log($"[CanChoose][#{opp2.ToString()} has: #{opp2.moves[0].MoveId.ToString(TextScripts.Name)}, #{opp2.moves[1].MoveId.ToString(TextScripts.Name)}, #{opp2.moves[2].MoveId.ToString(TextScripts.Name)}, #{opp2.moves[3].MoveId.ToString(TextScripts.Name)}]");
					return false;
				}
			}
			if (thispkmn.effects.Taunt > 0 && thismove.Power == 0) {//.BaseDamage
				if (showMessages) pbDisplayPaused(_INTL("{1} can't use {2} after the taunt!", thispkmn.ToString(), thismove.MoveId.ToString(TextScripts.Name)));
				return false;
			}
			if (thispkmn.effects.Torment) {
				if (thismove.MoveId==thispkmn.lastMoveUsed) {
					if (showMessages) pbDisplayPaused(_INTL("{1} can't use the same move twice in a row due to the torment!", thispkmn.ToString()));
					return false;
				}
			}
			if (thismove.MoveId==thispkmn.effects.DisableMove && !sleeptalk) {
				if (showMessages) pbDisplayPaused(_INTL("{1}'s {2} is disabled!", thispkmn.ToString(), thismove.MoveId.ToString(TextScripts.Name)));
				return false;
			}
			if (thismove.Effect==(Attack.Data.Effects)0x158 && // ToDo: Belch; Confirm value is correct
			   (thispkmn.Species != Pokemons.NONE || !thispkmn.belch)) {
				if (showMessages) pbDisplayPaused(_INTL("{1} hasn't eaten any held berry, so it can't possibly belch!", thispkmn.ToString()));
				return false;
			}
			if (thispkmn.effects.Encore>0 && idxMove!=thispkmn.effects.EncoreIndex) {
				return false;
			}
			return true;
		}

  def pbAutoChooseMove(idxPokemon,showMessages=true)
    thispkmn=@battlers[idxPokemon]
    if thispkmn.isFainted?
      @choices[idxPokemon][0]=0
      @choices[idxPokemon][1]=0
      @choices[idxPokemon][2]=null
      return
    end
    if thispkmn.effects.Encore>0 && 
       pbCanChooseMove?(idxPokemon,thispkmn.effects.EncoreIndex,false)
      GameDebug.Log($"[Auto choosing Encore move] #{thispkmn.moves[thispkmn.effects.EncoreIndex].name}")
      @choices[idxPokemon][0]=1    // "Use move"
      @choices[idxPokemon][1]=thispkmn.effects.EncoreIndex // Index of move
      @choices[idxPokemon][2]=thispkmn.moves[thispkmn.effects.EncoreIndex]
      @choices[idxPokemon][3]=-1   // No target chosen yet
      if @doublebattle
        thismove=thispkmn.moves[thispkmn.effects.EncoreIndex]
        target=thispkmn.pbTarget(thismove)
        if target==Targets.SingleNonUser
          target=@scene.pbChooseTarget(idxPokemon,target)
          pbRegisterTarget(idxPokemon,target) if target>=0
        elsif target==Targets.UserOrPartner
          target=@scene.pbChooseTarget(idxPokemon,target)
          pbRegisterTarget(idxPokemon,target) if target>=0 && (target&1)==(idxPokemon&1)
        end
      end
    else
      if !pbIsOpposing?(idxPokemon)
        pbDisplayPaused(_INTL("{1} has no moves left!",thispkmn.name)) if showMessages
      end
      @choices[idxPokemon][0]=1           // "Use move"
      @choices[idxPokemon][1]=-1          // Index of move to be used
      @choices[idxPokemon][2]=@struggle   // Use Struggle
      @choices[idxPokemon][3]=-1          // No target chosen yet
    end
  end

  def pbRegisterMove(idxPokemon,idxMove,showMessages=true)
    thispkmn=@battlers[idxPokemon]
    thismove=thispkmn.moves[idxMove]
    return false if !pbCanChooseMove?(idxPokemon,idxMove,showMessages)
    @choices[idxPokemon][0]=1         // "Use move"
    @choices[idxPokemon][1]=idxMove   // Index of move to be used
    @choices[idxPokemon][2]=thismove  // PokeBattle_Move object of the move
    @choices[idxPokemon][3]=-1        // No target chosen yet
    return true
  end

  def pbChoseMove?(i,move)
    return false if @battlers[i].isFainted?
    if @choices[i][0]==1 && @choices[i][1]>=0
      choice=@choices[i][1]
      return @battlers[i].moves[choice].id == move
    end
    return false
  end

  def pbChoseMoveFunctionCode?(i,code)
    return false if @battlers[i].isFainted?
    if @choices[i][0]==1 && @choices[i][1]>=0
      choice=@choices[i][1]
      return @battlers[i].moves[choice].function==code
    end
    return false
  end

  def pbRegisterTarget(idxPokemon,idxTarget)
    @choices[idxPokemon][3]=idxTarget   // Set target of move
    return true
  end

  def pbPriority(ignorequickclaw=false,log=false)
    return @priority if @usepriority // use stored priority if round isn't over yet
    @priority.clear
    speeds=[]
    priorities=[]
    quickclaw=[]; lagging=[]
    minpri=0; maxpri=0
    temp=[]
    #region Calculate each Pokémon's speed
    for (int i = 0; i < 4; i++) {
      speeds[i]=@battlers[i].pbSpeed
      quickclaw[i]=false
      lagging[i]=false
      if !ignorequickclaw && @choices[i][0]==1 // Chose to use a move
        if !quickclaw[i] && @battlers[i].Item == Items.CUSTAPBERRY &&
           !@battlers[i].pbOpposing1.Ability == Abilities.UNNERVE &&
           !@battlers[i].pbOpposing2.Ability == Abilities.UNNERVE
          if (@battlers[i].Ability == Abilities.GLUTTONY && @battlers[i].hp<=(@battlers[i].totalhp/2).floor) ||
             @battlers[i].hp<=(@battlers[i].totalhp/4).floor
            pbCommonAnimation("UseItem",@battlers[i],null)
            quickclaw[i]=true
            pbDisplayBrief(_INTL("{1}'s {2} let it move first!",
               @battlers[i].pbThis,@battlers[i].item.ToString(TextScripts.Name))
            @battlers[i].pbConsumeItem
          end
        end
        if !quickclaw[i] && @battlers[i].Item == Items.QUICKCLAW
          if pbRandom(10)<2
            pbCommonAnimation("UseItem",@battlers[i],null)
            quickclaw[i]=true
            pbDisplayBrief(_INTL("{1}'s {2} let it move first!",
               @battlers[i].pbThis,@battlers[i].item.ToString(TextScripts.Name))
          end
        end
        if !quickclaw[i] &&
           (@battlers[i].Ability == Abilities.STALL ||
           @battlers[i].Item == Items.LAGGINGTAIL ||
           @battlers[i].Item == Items.FULLINCENSE)
          lagging[i]=true
        end
      end
    end
	#endregion
    #region Calculate each Pokémon's priority bracket, and get the min/max priorities
    for (int i = 0; i < 4; i++) {
      // Assume that doing something other than using a move is priority 0
      pri=0
      if @choices[i][0]==1 // Chose to use a move
        pri=@choices[i][2].priority
        pri+=1 if @battlers[i].Ability == Abilities.PRANKSTER &&
                  @choices[i][2].pbIsStatus?
        pri+=1 if @battlers[i].Ability == Abilities.GALEWINGS &&
                  @choices[i][2].type == Types.FLYING
      end
      priorities[i]=pri
      if i==0
        minpri=pri
        maxpri=pri
      else
        minpri=pri if minpri>pri
        maxpri=pri if maxpri<pri
      end
    end
	#endregion
    // Find and order all moves with the same priority
    curpri=maxpri
    loop do
      temp.clear
      for (int j = 0; j < 4; j++) {
        temp.push(j) if priorities[j]==curpri
      end
      // Sort by speed
      if temp.length==1
        @priority[@priority.length]=@battlers[temp[0]]
      elsif temp.length>1
        n=temp.length
        for (int m = 0; m < temp.length-1; m++) {
          for (int i = 1; i < temp.length; i++) {
            // For each pair of battlers, rank the second compared to the first
            // -1 means rank higher, 0 means rank equal, 1 means rank lower
            cmp=0
            if quickclaw[temp[i]]
              cmp=-1
              if quickclaw[temp[i-1]]
                if speeds[temp[i]]==speeds[temp[i-1]]
                  cmp=0
                else
                  cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? -1 : 1
                end
              end
            elsif quickclaw[temp[i-1]]
              cmp=1
            elsif lagging[temp[i]]
              cmp=1
              if lagging[temp[i-1]]
                if speeds[temp[i]]==speeds[temp[i-1]]
                  cmp=0
                else
                  cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? 1 : -1
                end
              end
            elsif lagging[temp[i-1]]
              cmp=-1
            elsif speeds[temp[i]]!=speeds[temp[i-1]]
              if @field.effects.TrickRoom>0
                cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? 1 : -1
              else
                cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? -1 : 1
              end
            end
            if cmp<0 || // Swap the pair according to the second battler's rank
               (cmp==0 && pbRandom(2)==0)
              swaptmp=temp[i]
              temp[i]=temp[i-1]
              temp[i-1]=swaptmp
            end
          end
        end
        // Battlers in this bracket are properly sorted, so add them to @priority
        foreach (var i in temp) {
          @priority[@priority.length]=@battlers[i]
        end
      end
      curpri-=1
      break if curpri<minpri
    end
    // Write the priority order to the debug log
    if log
      d="[Priority] "; comma=false
      for (int i = 0; i < 4; i++) {
        if @priority[i] && !@priority[i].isFainted?
          d+=", " if comma
          d+="#{@priority[i].pbThis(comma)} (#{@priority[i].index})"; comma=true
        end
      end
      GameDebug.Log($d)
    end
    @usepriority=true
    return @priority
  end

		#endregion

		#region Switching Pokemon
  def pbCanSwitchLax?(idxPokemon,pkmnidxTo,showMessages)
    if pkmnidxTo>=0
      party=pbParty(idxPokemon)
      if pkmnidxTo>=party.length
        return false
      end
      if !party[pkmnidxTo]
        return false
      end
      if party[pkmnidxTo].isEgg?
        pbDisplayPaused(_INTL("An Egg can't battle!")) if showMessages 
        return false
      end
      if !pbIsOwner?(idxPokemon,pkmnidxTo)
        owner=pbPartyGetOwner(idxPokemon,pkmnidxTo)
        pbDisplayPaused(_INTL("You can't switch {1}'s Pokémon with one of yours!",owner.name)) if showMessages 
        return false
      end
      if party[pkmnidxTo].hp<=0
        pbDisplayPaused(_INTL("{1} has no energy left to battle!",party[pkmnidxTo].name)) if showMessages 
        return false
      end
      if @battlers[idxPokemon].pokemonIndex==pkmnidxTo ||
         @battlers[idxPokemon].pbPartner.pokemonIndex==pkmnidxTo
        pbDisplayPaused(_INTL("{1} is already in battle!",party[pkmnidxTo].name)) if showMessages 
        return false
      end
    end
    return true
  end

  def pbCanSwitch?(idxPokemon,pkmnidxTo,showMessages,ignoremeanlook=false)
    thispkmn=@battlers[idxPokemon]
    // Multi-Turn Attacks/Mean Look
    if !pbCanSwitchLax?(idxPokemon,pkmnidxTo,showMessages)
      return false
    end
    isOpposing=pbIsOpposing?(idxPokemon)
    party=pbParty(idxPokemon)
    for (int i = 0; i < 4; i++) {
      next if isOpposing!=pbIsOpposing?(i)
      if choices[i][0]==2 && choices[i][1]==pkmnidxTo
        pbDisplayPaused(_INTL("{1} has already been selected.",party[pkmnidxTo].name)) if showMessages 
        return false
      end
    end
    if thispkmn.Item == Items.SHEDSHELL
      return true
    end
    if USENEWBATTLEMECHANICS && thispkmn.pbHasType?(:GHOST)
      return true
    end
    if thispkmn.effects.MultiTurn>0 ||
       (!ignoremeanlook && thispkmn.effects.MeanLook>=0)
      pbDisplayPaused(_INTL("{1} can't be switched out!",thispkmn.pbThis)) if showMessages
      return false
    end
    if @field.effects.FairyLock>0
      pbDisplayPaused(_INTL("{1} can't be switched out!",thispkmn.pbThis)) if showMessages
      return false
    end
    if thispkmn.effects.Ingrain
      pbDisplayPaused(_INTL("{1} can't be switched out!",thispkmn.pbThis)) if showMessages
      return false
    end
    opp1=thispkmn.pbOpposing1
    opp2=thispkmn.pbOpposing2
    opp=null
    if thispkmn.pbHasType?(:STEEL)
      opp=opp1 if opp1.Ability == Abilities.MAGNETPULL
      opp=opp2 if opp2.Ability == Abilities.MAGNETPULL
    end
    if !thispkmn.isAirborne?
      opp=opp1 if opp1.Ability == Abilities.ARENATRAP
      opp=opp2 if opp2.Ability == Abilities.ARENATRAP
    end
    if !thispkmn.Ability == Abilities.SHADOWTAG
      opp=opp1 if opp1.Ability == Abilities.SHADOWTAG
      opp=opp2 if opp2.Ability == Abilities.SHADOWTAG
    end
    if opp
      abilityname=opp.ability.ToString(TextScripts.Name)
      pbDisplayPaused(_INTL("{1}'s {2} prevents switching!",opp.pbThis,abilityname)) if showMessages
      return false
    end
    return true
  end

  def pbRegisterSwitch(idxPokemon,idxOther)
    return false if !pbCanSwitch?(idxPokemon,idxOther,false)
    @choices[idxPokemon][0]=2          // "Switch Pokémon"
    @choices[idxPokemon][1]=idxOther   // Index of other Pokémon to switch with
    @choices[idxPokemon][2]=null
    side=(pbIsOpposing?(idxPokemon)) ? 1 : 0
    owner=pbGetOwnerIndex(idxPokemon)
    if @megaEvolution[side][owner]==idxPokemon
      @megaEvolution[side][owner]=-1
    end
    return true
  end

  def pbCanChooseNonActive?(index)
    party=pbParty(index)
    for (int i = 0; i < party.length; i++) {
      return true if pbCanSwitchLax?(index,i,false)
    end
    return false
  end

  def pbSwitch(favorDraws=false)
    if !favorDraws
      return if @decision>0
    else
      return if @decision==5
    end
    pbJudge()
    return if @decision>0
    firstbattlerhp=@battlers[0].hp
    switched=[]
    for (int index = 0; index < 4; index++) {
      next if !@doublebattle && pbIsDoubleBattler?(index)
      next if @battlers[index] && !@battlers[index].isFainted?
      next if !pbCanChooseNonActive?(index)
      if !pbOwnedByPlayer?(index)
        if !pbIsOpposing?(index) || (@opponent && pbIsOpposing?(index))
          newenemy=pbSwitchInBetween(index,false,false)
          newenemyname=newenemy
          if newenemy>=0 && pbParty(index)[newenemy].ability == Abilities.ILLUSION
            newenemyname=pbGetLastPokeInTeam(index)
          end
          opponent=pbGetOwner(index)
          if !@doublebattle && firstbattlerhp>0 && @shiftStyle && @opponent &&
              @internalbattle && pbCanChooseNonActive?(0) && pbIsOpposing?(index) &&
              @battlers[0].effects.Outrage==0
            pbDisplayPaused(_INTL("{1} is about to send in {2}.",opponent.fullname,pbParty(index)[newenemyname].name))
            if pbDisplayConfirm(_INTL("Will {1} change Pokémon?",self.pbPlayer.name))
              newpoke=pbSwitchPlayer(0,true,true)
              if newpoke>=0
                newpokename=newpoke
                if @party1[newpoke].ability == Abilities.ILLUSION
                  newpokename=pbGetLastPokeInTeam(0)
                end
                pbDisplayBrief(_INTL("{1}, that's enough! Come back!",@battlers[0].name))
                pbRecallAndReplace(0,newpoke,newpokename)
                switched.push(0)
              end
            end
          end
          pbRecallAndReplace(index,newenemy,newenemyname,false,false)
          switched.push(index)
        end
      elsif @opponent
        newpoke=pbSwitchInBetween(index,true,false)
        newpokename=newpoke
        if @party1[newpoke].ability == Abilities.ILLUSION
          newpokename=pbGetLastPokeInTeam(index)
        end
        pbRecallAndReplace(index,newpoke,newpokename)
        switched.push(index)
      else
        switch=false
        if !pbDisplayConfirm(_INTL("Use next Pokémon?")) 
          switch=(pbRun(index,true)<=0)
        else
          switch=true
        end
        if switch
          newpoke=pbSwitchInBetween(index,true,false)
          newpokename=newpoke
          if @party1[newpoke].ability == Abilities.ILLUSION
            newpokename=pbGetLastPokeInTeam(index)
          end
          pbRecallAndReplace(index,newpoke,newpokename)
          switched.push(index)
        end
      end
    end
    if switched.length>0
      priority=pbPriority
      foreach (var i in priority) {
        i.pbAbilitiesOnSwitchIn(true) if switched.include?(i.index)
      end
    end
  end

  def pbSendOut(index,pokemon)
    pbSetSeen(pokemon)
    @peer.pbOnEnteringBattle(self,pokemon)
    if pbIsOpposing?(index)
      @scene.pbTrainerSendOut(index,pokemon)
    else
      @scene.pbSendOut(index,pokemon)
    end
    @scene.pbResetMoveIndex(index)
  end

  def pbReplace(index,newpoke,batonpass=false)
    party=pbParty(index)
    oldpoke=@battlers[index].pokemonIndex
    // Initialise the new Pokémon
    @battlers[index].pbInitialize(party[newpoke],newpoke,batonpass)
    // Reorder the party for this battle
    partyorder=(!pbIsOpposing?(index)) ? @party1order : @party2order
    bpo=-1; bpn=-1
    for (int i = 0; i < partyorder.length; i++) {
      bpo=i if partyorder[i]==oldpoke
      bpn=i if partyorder[i]==newpoke
    end
    p=partyorder[bpo]; partyorder[bpo]=partyorder[bpn]; partyorder[bpn]=p
    // Send out the new Pokémon
    pbSendOut(index,party[newpoke])
    pbSetSeen(party[newpoke])
  end

  def pbRecallAndReplace(index,newpoke,newpokename=-1,batonpass=false,moldbreaker=false)
    @battlers[index].pbResetForm
    if !@battlers[index].isFainted?
      @scene.pbRecall(index)
    end
    pbMessagesOnReplace(index,newpoke,newpokename)
    pbReplace(index,newpoke,batonpass)
    return pbOnActiveOne(@battlers[index],false,moldbreaker)
  end

  def pbMessagesOnReplace(index,newpoke,newpokename=-1)
    newpokename=newpoke if newpokename<0
    party=pbParty(index)
    if pbOwnedByPlayer?(index)
//     if !party[newpoke]
//       p [index,newpoke,party[newpoke],pbAllFainted?(party)]
//       GameDebug.Log($[index,newpoke,party[newpoke],"pbMOR"].inspect)
//       for (int i = 0; i < party.length; i++) {
//         GameDebug.Log($[i,party[i].hp].inspect)
//       end
//       raise BattleAbortedException.new
//     end
      opposing=@battlers[index].pbOppositeOpposing
      if opposing.isFainted? || opposing.hp==opposing.totalhp
        pbDisplayBrief(_INTL("Go! {1}!",party[newpokename].name))
      elsif opposing.hp>=(opposing.totalhp/2)
        pbDisplayBrief(_INTL("Do it! {1}!",party[newpokename].name))
      elsif opposing.hp>=(opposing.totalhp/4)
        pbDisplayBrief(_INTL("Go for it, {1}!",party[newpokename].name))
      else
        pbDisplayBrief(_INTL("Your opponent's weak!\nGet 'em, {1}!",party[newpokename].name))
      end
      GameDebug.Log($"[Send out Pokémon] Player sent out #{party[newpokename].name} in position #{index}")
    else
//     if !party[newpoke]
//       p [index,newpoke,party[newpoke],pbAllFainted?(party)]
//       GameDebug.Log($[index,newpoke,party[newpoke],"pbMOR"].inspect)
//       for (int i = 0; i < party.length; i++) {
//         GameDebug.Log($[i,party[i].hp].inspect)
//       end
//       raise BattleAbortedException.new
//     end
      owner=pbGetOwner(index)
      pbDisplayBrief(_INTL("{1} sent\r\nout {2}!",owner.fullname,party[newpokename].name))
      GameDebug.Log($"[Send out Pokémon] Opponent sent out #{party[newpokename].name} in position #{index}")
    end
  end

  def pbSwitchInBetween(index,lax,cancancel)
    if !pbOwnedByPlayer?(index)
      return @scene.pbChooseNewEnemy(index,pbParty(index))
    else
      return pbSwitchPlayer(index,lax,cancancel)
    end
  end

  def pbSwitchPlayer(index,lax,cancancel)
    if @debug
      return @scene.pbChooseNewEnemy(index,pbParty(index))
    else
      return @scene.pbSwitch(index,lax,cancancel)
    end
  end
		#endregion

		#region Using an Item
/// Uses an item on a Pokémon in the player's party.
  def pbUseItemOnPokemon(item,pkmnIndex,userPkmn,scene)
    pokemon=@party1[pkmnIndex]
    battler=null
    name=pbGetOwner(userPkmn.index).fullname
    name=pbGetOwner(userPkmn.index).name if pbBelongsToPlayer?(userPkmn.index)
    pbDisplayBrief(_INTL("{1} used the\r\n{2}.",name,PBItems.getName(item)))
    GameDebug.Log($"[Use item] Player used #{PBItems.getName(item)} on #{pokemon.name}")
    ret=false
    if pokemon.isEgg?
      pbDisplay(_INTL("But it had no effect!"))
    else
      for (int i = 0; i < 4; i++) {
        if !pbIsOpposing?(i) && @battlers[i].pokemonIndex==pkmnIndex
          battler=@battlers[i]
        end
      end
      ret=ItemHandlers.triggerBattleUseOnPokemon(item,pokemon,battler,scene)
    end
    if !ret && pbBelongsToPlayer?(userPkmn.index)
      if $PokemonBag.pbCanStore?(item)
        $PokemonBag.pbStoreItem(item)
      else
        raise _INTL("Couldn't return unused item to Bag somehow.")
      end
    end
    return ret
  end

/// Uses an item on an active Pokémon.
  def pbUseItemOnBattler(item,index,userPkmn,scene)
    GameDebug.Log($"[Use item] Player used #{PBItems.getName(item)} on #{@battlers[index].pbThis(true)}")
    ret=ItemHandlers.triggerBattleUseOnBattler(item,@battlers[index],scene)
    if !ret && pbBelongsToPlayer?(userPkmn.index)
      if $PokemonBag.pbCanStore?(item)
        $PokemonBag.pbStoreItem(item)
      else
        raise _INTL("Couldn't return unused item to Bag somehow.")
      end
    end
    return ret
  end

  def pbRegisterItem(idxPokemon,idxItem,idxTarget=null)
    if idxTarget!=null && idxTarget>=0
      for (int i = 0; i < 4; i++) {
        if !@battlers[i].pbIsOpposing?(idxPokemon) &&
           @battlers[i].pokemonIndex==idxTarget &&
           @battlers[i].effects.Embargo>0
          pbDisplay(_INTL("Embargo's effect prevents the item's use on {1}!",@battlers[i].pbThis(true)))
          if pbBelongsToPlayer?(@battlers[i].index)
            if $PokemonBag.pbCanStore?(idxItem)
              $PokemonBag.pbStoreItem(idxItem)
            else
              raise _INTL("Couldn't return unused item to Bag somehow.")
            end
          end
          return false
        end
      end
    end
    if ItemHandlers.hasUseInBattle(idxItem)
      if idxPokemon==0 // Player's first Pokémon
        if ItemHandlers.triggerBattleUseOnBattler(idxItem,@battlers[idxPokemon],self)
          // Using Poké Balls or Poké Doll only
          ItemHandlers.triggerUseInBattle(idxItem,@battlers[idxPokemon],self)
          if @doublebattle
            @battlers[idxPokemon+2].effects.SkipTurn=true
          end
        else
          if $PokemonBag.pbCanStore?(idxItem)
            $PokemonBag.pbStoreItem(idxItem)
          else
            raise _INTL("Couldn't return unusable item to Bag somehow.")
          end
          return false
        end
      else
        if ItemHandlers.triggerBattleUseOnBattler(idxItem,@battlers[idxPokemon],self)
          pbDisplay(_INTL("It's impossible to aim without being focused!"))
        end
        return false
      end
    end
    @choices[idxPokemon][0]=3         // "Use an item"
    @choices[idxPokemon][1]=idxItem   // ID of item to be used
    @choices[idxPokemon][2]=idxTarget // Index of Pokémon to use item on
    side=(pbIsOpposing?(idxPokemon)) ? 1 : 0
    owner=pbGetOwnerIndex(idxPokemon)
    if @megaEvolution[side][owner]==idxPokemon
      @megaEvolution[side][owner]=-1
    end
    return true
  end

  def pbEnemyUseItem(item,battler)
    return 0 if !@internalbattle
    items=pbGetOwnerItems(battler.index)
    return if !items
    opponent=pbGetOwner(battler.index)
    for (int i = 0; i < items.length; i++) {
      if items[i]==item
        items.delete_at(i)
        break
      end
    end
    itemname=PBItems.getName(item)
    pbDisplayBrief(_INTL("{1} used the\r\n{2}!",opponent.fullname,itemname))
    GameDebug.Log($"[Use item] Opponent used #{itemname} on #{battler.pbThis(true)}")
    if item == Items.POTION
      battler.pbRecoverHP(20,true)
      pbDisplay(_INTL("{1}'s HP was restored.",battler.pbThis))
    elsif item == Items.SUPERPOTION
      battler.pbRecoverHP(50,true)
      pbDisplay(_INTL("{1}'s HP was restored.",battler.pbThis))
    elsif item == Items.HYPERPOTION
      battler.pbRecoverHP(200,true)
      pbDisplay(_INTL("{1}'s HP was restored.",battler.pbThis))
    elsif item == Items.MAXPOTION
      battler.pbRecoverHP(battler.totalhp-battler.hp,true)
      pbDisplay(_INTL("{1}'s HP was restored.",battler.pbThis))
    elsif item == Items.FULLRESTORE
      fullhp=(battler.hp==battler.totalhp)
      battler.pbRecoverHP(battler.totalhp-battler.hp,true)
      battler.status=0; battler.statusCount=0
      battler.effects.Confusion=0
      if fullhp
        pbDisplay(_INTL("{1} became healthy!",battler.pbThis))
      else
        pbDisplay(_INTL("{1}'s HP was restored.",battler.pbThis))
      end
    elsif item == Items.FULLHEAL
      battler.status=0; battler.statusCount=0
      battler.effects.Confusion=0
      pbDisplay(_INTL("{1} became healthy!",battler.pbThis))
    elsif item == Items.XATTACK
      if battler.pbCanIncreaseStatStage?(Stats.ATTACK,battler)
        battler.pbIncreaseStat(Stats.ATTACK,1,battler,true)
      end
    elsif item == Items.XDEFEND
      if battler.pbCanIncreaseStatStage?(Stats.DEFENSE,battler)
        battler.pbIncreaseStat(Stats.DEFENSE,1,battler,true)
      end
    elsif item == Items.XSPEED
      if battler.pbCanIncreaseStatStage?(Stats.SPEED,battler)
        battler.pbIncreaseStat(Stats.SPEED,1,battler,true)
      end
    elsif item == Items.XSPECIAL
      if battler.pbCanIncreaseStatStage?(Stats.SPATK,battler)
        battler.pbIncreaseStat(Stats.SPATK,1,battler,true)
      end
    elsif item == Items.XSPDEF
      if battler.pbCanIncreaseStatStage?(Stats.SPDEF,battler)
        battler.pbIncreaseStat(Stats.SPDEF,1,battler,true)
      end
    elsif item == Items.XACCURACY
      if battler.pbCanIncreaseStatStage?(Stats.ACCURACY,battler)
        battler.pbIncreaseStat(Stats.ACCURACY,1,battler,true)
      end
    end
  end
		#endregion

		#region Fleeing from Battle
		public bool pbCanRun(int idxPokemon)
		{
    return false if @opponent
    return false if @cantescape && !pbIsOpposing?(idsPokemon)
    thispkmn=@battlers[idxPokemon]
    return true if thispkmn.pbHasType?(:GHOST) && USENEWBATTLEMECHANICS
    return true if thispkmn.Item == Items.SMOKEBALL
    return true if thispkmn.Ability == Abilities.RUNAWAY
    return pbCanSwitch?(idxPokemon,-1,false)
		}

  def pbRun(idxPokemon,duringBattle=false)
    thispkmn=@battlers[idxPokemon]
    if pbIsOpposing?(idxPokemon)
      return 0 if @opponent
      @choices[i][0]=5 // run
      @choices[i][1]=0 
      @choices[i][2]=null
      return -1
    end
    if @opponent
      if $DEBUG && Input.press?(Input::CTRL)
        if pbDisplayConfirm(_INTL("Treat this battle as a win?"))
          @decision=1
          return 1
        elsif pbDisplayConfirm(_INTL("Treat this battle as a loss?"))
          @decision=2
          return 1
        end
      elsif @internalbattle
        pbDisplayPaused(_INTL("No! There's no running from a Trainer battle!"))
      elsif pbDisplayConfirm(_INTL("Would you like to forfeit the match and quit now?"))
        pbDisplay(_INTL("{1} forfeited the match!",self.pbPlayer.name))
        @decision=3
        return 1
      end
      return 0
    end
    if $DEBUG && Input.press?(Input::CTRL)
      pbDisplayPaused(_INTL("Got away safely!"))
      @decision=3
      return 1
    end
    if @cantescape
      pbDisplayPaused(_INTL("Can't escape!"))
      return 0
    end
    if thispkmn.pbHasType?(:GHOST) && USENEWBATTLEMECHANICS
      pbDisplayPaused(_INTL("Got away safely!"))
      @decision=3
      return 1
    end
    if thispkmn.Ability == Abilities.RUNAWAY
      if duringBattle
        pbDisplayPaused(_INTL("Got away safely!"))
      else
        pbDisplayPaused(_INTL("{1} escaped using Run Away!",thispkmn.pbThis))
      end
      @decision=3
      return 1
    end
    if thispkmn.Item == Items.SMOKEBALL
      if duringBattle
        pbDisplayPaused(_INTL("Got away safely!"))
      else
        pbDisplayPaused(_INTL("{1} escaped using its {2}!",thispkmn.pbThis,PBItems.getName(thispkmn.item)))
      end
      @decision=3
      return 1
    end
    if !duringBattle && !pbCanSwitch?(idxPokemon,-1,false)
      pbDisplayPaused(_INTL("Can't escape!"))
      return 0
    end
    // Note: not pbSpeed, because using unmodified Speed
    speedPlayer=@battlers[idxPokemon].speed
    opposing=@battlers[idxPokemon].pbOppositeOpposing
    opposing=opposing.pbPartner if opposing.isFainted?
    if !opposing.isFainted?
      speedEnemy=opposing.speed
      if speedPlayer>speedEnemy
        rate=256
      else
        speedEnemy=1 if speedEnemy<=0
        rate=speedPlayer*128/speedEnemy
        rate+=@runCommand*30
        rate&=0xFF
      end
    else
      rate=256
    end
    ret=1
    if pbAIRandom(256)<rate
      pbDisplayPaused(_INTL("Got away safely!"))
      @decision=3
    else
      pbDisplayPaused(_INTL("Can't escape!"))
      ret=-1
    end
    @runCommand+=1 if !duringBattle
    return ret
  end
		#endregion

		#region Mega Evolve Battler
  def pbCanMegaEvolve?(index)
    return false if $game_switches[NO_MEGA_EVOLUTION]
    return false if !@battlers[index].hasMega?
    return false if pbIsOpposing?(index) && !@opponent
    return true if $DEBUG && Input.press?(Input::CTRL)
    return false if !pbHasMegaRing(index)
    side=(pbIsOpposing?(index)) ? 1 : 0
    owner=pbGetOwnerIndex(index)
    return false if @megaEvolution[side][owner]!=-1
    return false if @battlers[index].effects.SkyDrop
    return true
  end

  def pbRegisterMegaEvolution(index)
    side=(pbIsOpposing?(index)) ? 1 : 0
    owner=pbGetOwnerIndex(index)
    @megaEvolution[side][owner]=index
  end

  def pbMegaEvolve(index)
    return if !@battlers[index] || !@battlers[index].pokemon
    return if !(@battlers[index].hasMega? rescue false)
    return if (@battlers[index].isMega? rescue true)
    ownername=pbGetOwner(index).fullname
    ownername=pbGetOwner(index).name if pbBelongsToPlayer?(index)
    case (@battlers[index].pokemon.megaMessage rescue 0)
    when 1 // Rayquaza
      pbDisplay(_INTL("{1}'s fervent wish has reached {2}!",ownername,@battlers[index].pbThis))
    else
      pbDisplay(_INTL("{1}'s {2} is reacting to {3}'s {4}!",
         @battlers[index].pbThis,PBItems.getName(@battlers[index].item),
         ownername,pbGetMegaRingName(index)))
    end
    pbCommonAnimation("MegaEvolution",@battlers[index],null)
    @battlers[index].pokemon.makeMega
    @battlers[index].form=@battlers[index].pokemon.form
    @battlers[index].pbUpdate(true)
    @scene.pbChangePokemon(@battlers[index],@battlers[index].pokemon)
    pbCommonAnimation("MegaEvolution2",@battlers[index],null)
    meganame=(@battlers[index].pokemon.megaName rescue null)
    if !meganame || meganame==""
      meganame=_INTL("Mega {1}",PBSpecies.getName(@battlers[index].pokemon.species))
    end
    pbDisplay(_INTL("{1} has Mega Evolved into {2}!",@battlers[index].pbThis,meganame))
    GameDebug.Log($"[Mega Evolution] #{@battlers[index].pbThis} Mega Evolved")
    side=(pbIsOpposing?(index)) ? 1 : 0
    owner=pbGetOwnerIndex(index)
    @megaEvolution[side][owner]=-2
  end
		#endregion

		#region Primal Revert Battler
  def pbPrimalReversion(index)
    return if !@battlers[index] || !@battlers[index].pokemon
    return if !(@battlers[index].hasPrimal? rescue false)
    return if (@battlers[index].isPrimal? rescue true)
    if @battlers[index].pokemon.species == Pokemons.KYOGRE
      pbCommonAnimation("PrimalKyogre",@battlers[index],null)
    elsif @battlers[index].pokemon.species == Pokemons.GROUDON
      pbCommonAnimation("PrimalGroudon",@battlers[index],null)
    end
    @battlers[index].pokemon.makePrimal
    @battlers[index].form=@battlers[index].pokemon.form
    @battlers[index].pbUpdate(true)
    @scene.pbChangePokemon(@battlers[index],@battlers[index].pokemon)
    if @battlers[index].pokemon.species == Pokemons.KYOGRE
      pbCommonAnimation("PrimalKyogre2",@battlers[index],null)
    elsif @battlers[index].pokemon.species == Pokemons.GROUDON
      pbCommonAnimation("PrimalGroudon2",@battlers[index],null)
    end
    pbDisplay(_INTL("{1}'s Primal Reversion!\nIt reverted to its primal form!",@battlers[index].pbThis))
    GameDebug.Log($"[Primal Reversion] #{@battlers[index].pbThis} Primal Reverted")
  end
		#endregion

		#region Call Battler
  def pbCall(index)
    owner=pbGetOwner(index)
    pbDisplay(_INTL("{1} called {2}!",owner.name,@battlers[index].name))
    pbDisplay(_INTL("{1}!",@battlers[index].name))
    GameDebug.Log($"[Call to Pokémon] #{owner.name} called to #{@battlers[index].pbThis(true)}")
    if @battlers[index].isShadow?
      if @battlers[index].inHyperMode?
        @battlers[index].pokemon.hypermode=false
        @battlers[index].pokemon.adjustHeart(-300)
        pbDisplay(_INTL("{1} came to its senses from the Trainer's call!",@battlers[index].pbThis))
      else
        pbDisplay(_INTL("But nothing happened!"))
      end
    elsif @battlers[index].status!=Statuses.SLEEP &&
          @battlers[index].pbCanIncreaseStatStage?(Stats.ACCURACY,@battlers[index])
      @battlers[index].pbIncreaseStat(Stats.ACCURACY,1,@battlers[index],true)
    else
      pbDisplay(_INTL("But nothing happened!"))
    end
  end
		#endregion

		#region Gaining Experience
  def pbGainEXP
    return if !@internalbattle
    successbegin=true
    for (int i = 0; i < 4; i++) { // Not ordered by priority
      if !@doublebattle && pbIsDoubleBattler?(i)
        @battlers[i].participants=[]
        next
      end
      if pbIsOpposing?(i) && @battlers[i].participants.length>0 &&
         (@battlers[i].isFainted? || @battlers[i].captured)
        haveexpall=(hasConst?(PBItems,:EXPALL) && $PokemonBag.pbQuantity(:EXPALL)>0)
        // First count the number of participants
        partic=0
        expshare=0
        foreach (var j in @battlers[i].participants) {
          next if !@party1[j] || !pbIsOwner?(0,j)
          partic+=1 if @party1[j].hp>0 && !@party1[j].isEgg?
        end
        if !haveexpall
          for (int j = 0; j < @party1.length; j++) {
            next if !@party1[j] || !pbIsOwner?(0,j)
            expshare+=1 if @party1[j].hp>0 && !@party1[j].isEgg? && 
                           (@party1[j].item == Items.EXPSHARE ||
                           @party1[j].itemInitial == Items.EXPSHARE)
          end
        end
        // Now calculate EXP for the participants
        if partic>0 || expshare>0 || haveexpall
          if !@opponent && successbegin && pbAllFainted?(@party2)
            @scene.pbWildBattleSuccess
            successbegin=false
          end
          for (int j = 0; j < @party1.length; j++) {
            next if !@party1[j] || !pbIsOwner?(0,j)
            next if @party1[j].hp<=0 || @party1[j].isEgg?
            haveexpshare=(@party1[j].item == Items.EXPSHARE ||
                          @party1[j].itemInitial == Items.EXPSHARE)
            next if !haveexpshare && !@battlers[i].participants.include?(j)
            pbGainExpOne(j,@battlers[i],partic,expshare,haveexpall)
          end
          if haveexpall
            showmessage=true
            for (int j = 0; j < @party1.length; j++) {
              next if !@party1[j] || !pbIsOwner?(0,j)
              next if @party1[j].hp<=0 || @party1[j].isEgg?
              next if @party1[j].item == Items.EXPSHARE ||
                      @party1[j].itemInitial == Items.EXPSHARE
              next if @battlers[i].participants.include?(j)
              pbDisplayPaused(_INTL("The rest of your team gained Exp. Points thanks to the {1}!",
                 PBItems.getName(getConst(PBItems,:EXPALL)))) if showmessage
              showmessage=false
              pbGainExpOne(j,@battlers[i],partic,expshare,haveexpall,false)
            end
          end
        end
        // Now clear the participants array
        @battlers[i].participants=[]
      end
    end
  end

  def pbGainExpOne(index,defeated,partic,expshare,haveexpall,showmessages=true)
    thispoke=@party1[index]
    // Original species, not current species
    level=defeated.level
    baseexp=defeated.pokemon.baseExp
    evyield=defeated.pokemon.evYield
    // Gain effort value points, using RS effort values
    totalev=0
    for (int k = 0; k < 6; k++) {
      totalev+=thispoke.ev[k]
    end
    for (int k = 0; k < 6; k++) {
      evgain=evyield[k]
      evgain*=2 if thispoke.item == Items.MACHOBRACE ||
                   thispoke.itemInitial == Items.MACHOBRACE
      case k
      when Stats.HP
        evgain+=4 if thispoke.item == Items.POWERWEIGHT ||
                     thispoke.itemInitial == Items.POWERWEIGHT
      when Stats.ATTACK
        evgain+=4 if thispoke.item == Items.POWERBRACER ||
                     thispoke.itemInitial == Items.POWERBRACER
      when Stats.DEFENSE
        evgain+=4 if thispoke.item == Items.POWERBELT ||
                     thispoke.itemInitial == Items.POWERBELT
      when Stats.SPATK
        evgain+=4 if thispoke.item == Items.POWERLENS ||
                     thispoke.itemInitial == Items.POWERLENS
      when Stats.SPDEF
        evgain+=4 if thispoke.item == Items.POWERBAND ||
                     thispoke.itemInitial == Items.POWERBAND
      when Stats.SPEED
        evgain+=4 if thispoke.item == Items.POWERANKLET ||
                     thispoke.itemInitial == Items.POWERANKLET
      end
      evgain*=2 if thispoke.pokerusStage>=1 // Infected or cured
      if evgain>0
        // Can't exceed overall limit
        evgain-=totalev+evgain-PokeBattle_Pokemon::EVLIMIT if totalev+evgain>PokeBattle_Pokemon::EVLIMIT
        // Can't exceed stat limit
        evgain-=thispoke.ev[k]+evgain-PokeBattle_Pokemon::EVSTATLIMIT if thispoke.ev[k]+evgain>PokeBattle_Pokemon::EVSTATLIMIT
        // Add EV gain
        thispoke.ev[k]+=evgain
        if thispoke.ev[k]>PokeBattle_Pokemon::EVSTATLIMIT
          print "Single-stat EV limit #{PokeBattle_Pokemon::EVSTATLIMIT} exceeded.\r\nStat: #{k}  EV gain: #{evgain}  EVs: #{thispoke.ev.inspect}"
          thispoke.ev[k]=PokeBattle_Pokemon::EVSTATLIMIT
        end
        totalev+=evgain
        if totalev>PokeBattle_Pokemon::EVLIMIT
          print "EV limit #{PokeBattle_Pokemon::EVLIMIT} exceeded.\r\nTotal EVs: #{totalev} EV gain: #{evgain}  EVs: #{thispoke.ev.inspect}"
        end
      end
    end
    // Gain experience
    ispartic=0
    ispartic=1 if defeated.participants.include?(index)
    haveexpshare=(thispoke.item == Items.EXPSHARE ||
                  thispoke.itemInitial == Items.EXPSHARE) ? 1 : 0
    exp=0
    if expshare>0
      if partic==0 // No participants, all Exp goes to Exp Share holders
        exp=(level*baseexp).floor
        exp=(exp/(NOSPLITEXP ? 1 : expshare)).floor*haveexpshare
      else
        if NOSPLITEXP
          exp=(level*baseexp).floor*ispartic
          exp=(level*baseexp/2).floor*haveexpshare if ispartic==0
        else
          exp=(level*baseexp/2).floor
          exp=(exp/partic).floor*ispartic + (exp/expshare).floor*haveexpshare
        end
      end
    elsif ispartic==1
      exp=(level*baseexp/(NOSPLITEXP ? 1 : partic)).floor
    elsif haveexpall
      exp=(level*baseexp/2).floor
    end
    return if exp<=0
    exp=(exp*3/2).floor if @opponent
    if USESCALEDEXPFORMULA
      exp=(exp/5).floor
      leveladjust=(2*level+10.0)/(level+thispoke.level+10.0)
      leveladjust=leveladjust**5
      leveladjust=Math.sqrt(leveladjust)
      exp=(exp*leveladjust).floor
      exp+=1 if ispartic>0 || haveexpshare>0
    else
      exp=(exp/7).floor
    end
    isOutsider=(thispoke.trainerID!=self.pbPlayer.id ||
               (thispoke.language!=0 && thispoke.language!=self.pbPlayer.language))
    if isOutsider
      if thispoke.language!=0 && thispoke.language!=self.pbPlayer.language
        exp=(exp*1.7).floor
      else
        exp=(exp*3/2).floor
      end
    end
    exp=(exp*3/2).floor if thispoke.item == Items.LUCKYEGG ||
                           thispoke.itemInitial == Items.LUCKYEGG
    growthrate=thispoke.growthrate
    newexp=PBExperience.pbAddExperience(thispoke.exp,exp,growthrate)
    exp=newexp-thispoke.exp
    if exp>0
      if showmessages
        if isOutsider
          pbDisplayPaused(_INTL("{1} gained a boosted {2} Exp. Points!",thispoke.name,exp))
        else
          pbDisplayPaused(_INTL("{1} gained {2} Exp. Points!",thispoke.name,exp))
        end
      end
      newlevel=PBExperience.pbGetLevelFromExperience(newexp,growthrate)
      tempexp=0
      curlevel=thispoke.level
      if newlevel<curlevel
        debuginfo="#{thispoke.name}: #{thispoke.level}/#{newlevel} | #{thispoke.exp}/#{newexp} | gain: #{exp}"
        raise RuntimeError.new(_INTL("The new level ({1}) is less than the Pokémon's\r\ncurrent level ({2}), which shouldn't happen.\r\n[Debug: {3}]",
                               newlevel,curlevel,debuginfo))
        return
      end
      if thispoke.respond_to?("isShadow?") && thispoke.isShadow?
        thispoke.exp+=exp
      else
        tempexp1=thispoke.exp
        tempexp2=0
        // Find battler
        battler=pbFindPlayerBattler(index)
        loop do
          // EXP Bar animation
          startexp=PBExperience.pbGetStartExperience(curlevel,growthrate)
          endexp=PBExperience.pbGetStartExperience(curlevel+1,growthrate)
          tempexp2=(endexp<newexp) ? endexp : newexp
          thispoke.exp=tempexp2
          @scene.pbEXPBar(thispoke,battler,startexp,endexp,tempexp1,tempexp2)
          tempexp1=tempexp2
          curlevel+=1
          if curlevel>newlevel
            thispoke.calcStats 
            battler.pbUpdate(false) if battler
            @scene.pbRefresh
            break
          end
          oldtotalhp=thispoke.totalhp
          oldattack=thispoke.attack
          olddefense=thispoke.defense
          oldspeed=thispoke.speed
          oldspatk=thispoke.spatk
          oldspdef=thispoke.spdef
          if battler && battler.pokemon && @internalbattle
            battler.pokemon.changeHappiness("level up")
          end
          thispoke.calcStats
          battler.pbUpdate(false) if battler
          @scene.pbRefresh
          pbDisplayPaused(_INTL("{1} grew to Level {2}!",thispoke.name,curlevel))
          @scene.pbLevelUp(thispoke,battler,oldtotalhp,oldattack,
                           olddefense,oldspeed,oldspatk,oldspdef)
          // Finding all moves learned at this level
          movelist=thispoke.getMoveList
          foreach (var k in movelist) {
            if k[0]==thispoke.level   // Learned a new move
              pbLearnMove(index,k[1])
            end
          end
        end
      end
    end
  end
		#endregion

		#region Learning a move.
  def pbLearnMove(pkmnIndex,move)
    pokemon=@party1[pkmnIndex]
    return if !pokemon
    pkmnname=pokemon.name
    battler=pbFindPlayerBattler(pkmnIndex)
    movename=PBMoves.getName(move)
    for (int i = 0; i < 4; i++) {
      return if pokemon.moves[i].id==move
      if pokemon.moves[i].id==0
        pokemon.moves[i]=PBMove.new(move)
        battler.moves[i]=PokeBattle_Move.pbFromPBMove(self,pokemon.moves[i]) if battler
        pbDisplayPaused(_INTL("{1} learned {2}!",pkmnname,movename))
        GameDebug.Log($"[Learn move] #{pkmnname} learned #{movename}")
        return
      end
    end
    loop do
      pbDisplayPaused(_INTL("{1} is trying to learn {2}.",pkmnname,movename))
      pbDisplayPaused(_INTL("But {1} can't learn more than four moves.",pkmnname))
      if pbDisplayConfirm(_INTL("Delete a move to make room for {1}?",movename))
        pbDisplayPaused(_INTL("Which move should be forgotten?"))
        forgetmove=@scene.pbForgetMove(pokemon,move)
        if forgetmove>=0
          oldmovename=PBMoves.getName(pokemon.moves[forgetmove].id)
          pokemon.moves[forgetmove]=PBMove.new(move) // Replaces current/total PP
          battler.moves[forgetmove]=PokeBattle_Move.pbFromPBMove(self,pokemon.moves[forgetmove]) if battler
          pbDisplayPaused(_INTL("1,  2, and... ... ..."))
          pbDisplayPaused(_INTL("Poof!"))
          pbDisplayPaused(_INTL("{1} forgot {2}.",pkmnname,oldmovename))
          pbDisplayPaused(_INTL("And..."))
          pbDisplayPaused(_INTL("{1} learned {2}!",pkmnname,movename))
          GameDebug.Log($"[Learn move] #{pkmnname} forgot #{oldmovename} and learned #{movename}")
          return
        elsif pbDisplayConfirm(_INTL("Should {1} stop learning {2}?",pkmnname,movename))
          pbDisplayPaused(_INTL("{1} did not learn {2}.",pkmnname,movename))
          return
        end
      elsif pbDisplayConfirm(_INTL("Should {1} stop learning {2}?",pkmnname,movename))
        pbDisplayPaused(_INTL("{1} did not learn {2}.",pkmnname,movename))
        return
      end
    end
  end
		#endregion

		#region Abilities.
  def pbOnActiveAll
    for (int i = 0; i < 4; i++) { // Currently unfainted participants will earn EXP even if they faint afterwards
      @battlers[i].pbUpdateParticipants if pbIsOpposing?(i)
      @amuletcoin=true if !pbIsOpposing?(i) &&
                          (@battlers[i].item == Items.AMULETCOIN ||
                           @battlers[i].item == Items.LUCKINCENSE)
    end
    for (int i = 0; i < 4; i++) {
      if !@battlers[i].isFainted?
        if @battlers[i].isShadow? && pbIsOpposing?(i)
          pbCommonAnimation("Shadow",@battlers[i],null)
          pbDisplay(_INTL("Oh!\nA Shadow Pokémon!"))
        end
      end
    end
    // Weather-inducing abilities, Trace, Imposter, etc.
    @usepriority=false
    priority=pbPriority
    foreach (var i in priority) {
      i.pbAbilitiesOnSwitchIn(true)
    end
    // Check forms are correct
    for (int i = 0; i < 4; i++) {
      next if @battlers[i].isFainted?
      @battlers[i].pbCheckForm
    end
  end

  def pbOnActiveOne(pkmn,onlyabilities=false,moldbreaker=false)
    return false if pkmn.isFainted?
    if !onlyabilities
      for (int i = 0; i < 4; i++) { // Currently unfainted participants will earn EXP even if they faint afterwards
        @battlers[i].pbUpdateParticipants if pbIsOpposing?(i)
        @amuletcoin=true if !pbIsOpposing?(i) &&
                            (@battlers[i].item == Items.AMULETCOIN ||
                             @battlers[i].item == Items.LUCKINCENSE)
      end
      if pkmn.isShadow? && pbIsOpposing?(pkmn.index)
        pbCommonAnimation("Shadow",pkmn,null)
        pbDisplay(_INTL("Oh!\nA Shadow Pokémon!"))
      end
      // Healing Wish
      if pkmn.effects.HealingWish
        GameDebug.Log($"[Lingering effect triggered] #{pkmn.pbThis}'s Healing Wish")
        pbCommonAnimation("HealingWish",pkmn,null)
        pbDisplayPaused(_INTL("The healing wish came true for {1}!",pkmn.pbThis(true)))
        pkmn.pbRecoverHP(pkmn.totalhp,true)
        pkmn.pbCureStatus(false)
        pkmn.effects.HealingWish=false
      end
      // Lunar Dance
      if pkmn.effects.LunarDance
        GameDebug.Log($"[Lingering effect triggered] #{pkmn.pbThis}'s Lunar Dance")
        pbCommonAnimation("LunarDance",pkmn,null)
        pbDisplayPaused(_INTL("{1} became cloaked in mystical moonlight!",pkmn.pbThis))
        pkmn.pbRecoverHP(pkmn.totalhp,true)
        pkmn.pbCureStatus(false)
        for (int i = 0; i < 4; i++) {
          pkmn.moves[i].pp=pkmn.moves[i].totalpp
        end
        pkmn.effects.LunarDance=false
      end
      // Spikes
      if pkmn.pbOwnSide.effects.Spikes>0 && !pkmn.isAirborne?(moldbreaker)
        if !pkmn.Ability == Abilities.MAGICGUARD
          GameDebug.Log($"[Entry hazard] #{pkmn.pbThis} triggered Spikes")
          spikesdiv=[8,6,4][pkmn.pbOwnSide.effects.Spikes-1]
          @scene.pbDamageAnimation(pkmn,0)
          pkmn.pbReduceHP((pkmn.totalhp/spikesdiv).floor)
          pbDisplayPaused(_INTL("{1} is hurt by the spikes!",pkmn.pbThis))
        end
      end
      pkmn.pbFaint if pkmn.isFainted?
      // Stealth Rock
      if pkmn.pbOwnSide.effects.StealthRock && !pkmn.isFainted?
        if !pkmn.Ability == Abilities.MAGICGUARD
          atype=getConst(PBTypes,:ROCK) || 0
          eff=PBTypes.getCombinedEffectiveness(atype,pkmn.type1,pkmn.type2,pkmn.effects.Type3)
          if eff>0
            GameDebug.Log($"[Entry hazard] #{pkmn.pbThis} triggered Stealth Rock")
            @scene.pbDamageAnimation(pkmn,0)
            pkmn.pbReduceHP(((pkmn.totalhp*eff)/64).floor)
            pbDisplayPaused(_INTL("Pointed stones dug into {1}!",pkmn.pbThis))
          end
        end
      end
      pkmn.pbFaint if pkmn.isFainted?
      // Toxic Spikes
      if pkmn.pbOwnSide.effects.ToxicSpikes>0 && !pkmn.isFainted?
        if !pkmn.isAirborne?(moldbreaker)
          if pkmn.pbHasType?(:POISON)
            GameDebug.Log($"[Entry hazard] #{pkmn.pbThis} absorbed Toxic Spikes")
            pkmn.pbOwnSide.effects.ToxicSpikes=0
            pbDisplayPaused(_INTL("{1} absorbed the poison spikes!",pkmn.pbThis))
          elsif pkmn.pbCanPoisonSpikes?(moldbreaker)
            GameDebug.Log($"[Entry hazard] #{pkmn.pbThis} triggered Toxic Spikes")
            if pkmn.pbOwnSide.effects.ToxicSpikes==2
              pkmn.pbPoison(null,_INTL("{1} was badly poisoned by the poison spikes!",pkmn.pbThis,true))
            else
              pkmn.pbPoison(null,_INTL("{1} was poisoned by the poison spikes!",pkmn.pbThis))
            end
          end
        end
      end
      // Sticky Web
      if pkmn.pbOwnSide.effects.StickyWeb && !pkmn.isFainted? &&
         !pkmn.isAirborne?(moldbreaker)
        if pkmn.pbCanReduceStatStage?(Stats.SPEED,null,false,null,moldbreaker)
          GameDebug.Log($"[Entry hazard] #{pkmn.pbThis} triggered Sticky Web")
          pbDisplayPaused(_INTL("{1} was caught in a sticky web!",pkmn.pbThis))
          pkmn.pbReduceStat(Stats.SPEED,1,null,false,null,true,moldbreaker)
        end
      end
    end
    pkmn.pbAbilityCureCheck
    if pkmn.isFainted?
      pbGainEXP
      pbJudge //      pbSwitch
      return false
    end
//    pkmn.pbAbilitiesOnSwitchIn(true)
    if !onlyabilities
      pkmn.pbCheckForm
      pkmn.pbBerryCureCheck
    end
    return true
  end

  def pbPrimordialWeather
    // End Primordial Sea, Desolate Land, Delta Stream
    hasabil=false
    case @weather
    when Weather.HEAVYRAIN
      for (int i = 0; i < 4; i++) {
        if @battlers[i].ability == Abilities.PRIMORDIALSEA &&
           !@battlers[i].isFainted?
          hasabil=true; break
        end
        if !hasabil
          @weather=0
          pbDisplayBrief("The heavy rain has lifted!")
        end
      end
    when Weather.HARSHSUN
      for (int i = 0; i < 4; i++) {
        if @battlers[i].ability == Abilities.DESOLATELAND &&
           !@battlers[i].isFainted?
          hasabil=true; break
        end
        if !hasabil
          @weather=0
          pbDisplayBrief("The harsh sunlight faded!")
        end
      end
    when Weather.STRONGWINDS
      for (int i = 0; i < 4; i++) {
        if @battlers[i].ability == Abilities.DELTASTREAM &&
           !@battlers[i].isFainted?
          hasabil=true; break
        end
        if !hasabil
          @weather=0
          pbDisplayBrief("The mysterious air current has dissipated!")
        end
      end
    end
  end
		#endregion

		#region Judging
  def pbJudgeCheckpoint(attacker,move=0)
  end

  def pbDecisionOnTime
    count1=0
    count2=0
    hptotal1=0
    hptotal2=0
    foreach (var i in @party1) {
      next if !i
      if i.hp>0 && !i.isEgg?
        count1+=1
        hptotal1+=i.hp
      end
    end
    foreach (var i in @party2) {
      next if !i
      if i.hp>0 && !i.isEgg?
        count2+=1
        hptotal2+=i.hp
      end
    end
    return 1 if count1>count2     // win
    return 2 if count1<count2     // loss
    return 1 if hptotal1>hptotal2 // win
    return 2 if hptotal1<hptotal2 // loss
    return 5                      // draw
  end

  def pbDecisionOnTime2
    count1=0
    count2=0
    hptotal1=0
    hptotal2=0
    foreach (var i in @party1) {
      next if !i
      if i.hp>0 && !i.isEgg?
        count1+=1
        hptotal1+=(i.hp*100/i.totalhp)
      end
    end
    hptotal1/=count1 if count1>0
    foreach (var i in @party2) {
      next if !i
      if i.hp>0 && !i.isEgg?
        count2+=1
        hptotal2+=(i.hp*100/i.totalhp)
      end
    end
    hptotal2/=count2 if count2>0
    return 1 if count1>count2     // win
    return 2 if count1<count2     // loss
    return 1 if hptotal1>hptotal2 // win
    return 2 if hptotal1<hptotal2 // loss
    return 5                      // draw
  end

  def pbDecisionOnDraw
    return 5 // draw
  end

  def pbJudge
   GameDebug.Log($"[Counts: #{pbPokemonCount(@party1)}/#{pbPokemonCount(@party2)}]")
    if pbAllFainted?(@party1) && pbAllFainted?(@party2)
      @decision=pbDecisionOnDraw() // Draw
      return
    end
    if pbAllFainted?(@party1)
      @decision=2 // Loss
      return
    end
    if pbAllFainted?(@party2)
      @decision=1 // Win
      return
    end
  end
		#endregion

		#region Messages and animations.
  def pbDisplay(msg)
    @scene.pbDisplayMessage(msg)
  end

  def pbDisplayPaused(msg)
    @scene.pbDisplayPausedMessage(msg)
  end

  def pbDisplayBrief(msg)
    @scene.pbDisplayMessage(msg,true)
  end

  def pbDisplayConfirm(msg)
    @scene.pbDisplayConfirmMessage(msg)
  end

  def pbShowCommands(msg,commands,cancancel=true)
    @scene.pbShowCommands(msg,commands,cancancel)
  end

  def pbAnimation(move,attacker,opponent,hitnum=0)
    if @battlescene
      @scene.pbAnimation(move,attacker,opponent,hitnum)
    end
  end

  def pbCommonAnimation(name,attacker,opponent,hitnum=0)
    if @battlescene
      @scene.pbCommonAnimation(name,attacker,opponent,hitnum)
    end
  end
		#endregion

		#region Battle Core.
  def pbStartBattle(canlose=false)
    GameDebug.Log($"")
    GameDebug.Log($"******************************************")
    begin
      pbStartBattleCore(canlose)
    rescue BattleAbortedException
      @decision=0
      @scene.pbEndBattle(@decision)
    end
    return @decision
  end

  def pbStartBattleCore(canlose)
    if !@fullparty1 && @party1.length>MAXPARTYSIZE
      raise ArgumentError.new(_INTL("Party 1 has more than {1} Pokémon.",MAXPARTYSIZE))
    end
    if !@fullparty2 && @party2.length>MAXPARTYSIZE
      raise ArgumentError.new(_INTL("Party 2 has more than {1} Pokémon.",MAXPARTYSIZE))
    end
#region Initialize wild Pokémon
    if !@opponent
      if @party2.length==1
        if @doublebattle
          raise _INTL("Only two wild Pokémon are allowed in double battles")
        end
        wildpoke=@party2[0]
        @battlers[1].pbInitialize(wildpoke,0,false)
        @peer.pbOnEnteringBattle(self,wildpoke)
        pbSetSeen(wildpoke)
        @scene.pbStartBattle(self)
        pbDisplayPaused(_INTL("Wild {1} appeared!",wildpoke.name))
      elsif @party2.length==2
        if !@doublebattle
          raise _INTL("Only one wild Pokémon is allowed in single battles")
        end
        @battlers[1].pbInitialize(@party2[0],0,false)
        @battlers[3].pbInitialize(@party2[1],0,false)
        @peer.pbOnEnteringBattle(self,@party2[0])
        @peer.pbOnEnteringBattle(self,@party2[1])
        pbSetSeen(@party2[0])
        pbSetSeen(@party2[1])
        @scene.pbStartBattle(self)
        pbDisplayPaused(_INTL("Wild {1} and\r\n{2} appeared!",
           @party2[0].name,@party2[1].name))
      else
        raise _INTL("Only one or two wild Pokémon are allowed")
      end
#endregion
#region Initialize opponents in double battles
    elsif @doublebattle
      if @opponent.is_a?(Array)
        if @opponent.length==1
          @opponent=@opponent[0]
        elsif @opponent.length!=2
          raise _INTL("Opponents with zero or more than two people are not allowed")
        end
      end
      if @player.is_a?(Array)
        if @player.length==1
          @player=@player[0]
        elsif @player.length!=2
          raise _INTL("Player trainers with zero or more than two people are not allowed")
        end
      end
      @scene.pbStartBattle(self)
      if @opponent.is_a?(Array)
        pbDisplayPaused(_INTL("{1} and {2} want to battle!",@opponent[0].fullname,@opponent[1].fullname))
        sendout1=pbFindNextUnfainted(@party2,0,pbSecondPartyBegin(1))
        raise _INTL("Opponent 1 has no unfainted Pokémon") if sendout1<0
        sendout2=pbFindNextUnfainted(@party2,pbSecondPartyBegin(1))
        raise _INTL("Opponent 2 has no unfainted Pokémon") if sendout2<0
        @battlers[1].pbInitialize(@party2[sendout1],sendout1,false)
        pbDisplayBrief(_INTL("{1} sent\r\nout {2}!",@opponent[0].fullname,@battlers[1].name))
        pbSendOut(1,@party2[sendout1])
        @battlers[3].pbInitialize(@party2[sendout2],sendout2,false)
        pbDisplayBrief(_INTL("{1} sent\r\nout {2}!",@opponent[1].fullname,@battlers[3].name))
        pbSendOut(3,@party2[sendout2])
      else
        pbDisplayPaused(_INTL("{1}\r\nwould like to battle!",@opponent.fullname))
        sendout1=pbFindNextUnfainted(@party2,0)
        sendout2=pbFindNextUnfainted(@party2,sendout1+1)
        if sendout1<0 || sendout2<0
          raise _INTL("Opponent doesn't have two unfainted Pokémon")
        end
        @battlers[1].pbInitialize(@party2[sendout1],sendout1,false)
        @battlers[3].pbInitialize(@party2[sendout2],sendout2,false)
        pbDisplayBrief(_INTL("{1} sent\r\nout {2} and {3}!",
           @opponent.fullname,@battlers[1].name,@battlers[3].name))
        pbSendOut(1,@party2[sendout1])
        pbSendOut(3,@party2[sendout2])
      end
#endregion
#region Initialize opponent in single battles
    else
      sendout=pbFindNextUnfainted(@party2,0)
      raise _INTL("Trainer has no unfainted Pokémon") if sendout<0
      if @opponent.is_a?(Array)
        raise _INTL("Opponent trainer must be only one person in single battles") if @opponent.length!=1
        @opponent=@opponent[0]
      end
      if @player.is_a?(Array)
        raise _INTL("Player trainer must be only one person in single battles") if @player.length!=1
        @player=@player[0]
      end
      trainerpoke=@party2[sendout]
      @scene.pbStartBattle(self)
      pbDisplayPaused(_INTL("{1}\r\nwould like to battle!",@opponent.fullname))
      @battlers[1].pbInitialize(trainerpoke,sendout,false)
      pbDisplayBrief(_INTL("{1} sent\r\nout {2}!",@opponent.fullname,@battlers[1].name))
      pbSendOut(1,trainerpoke)
    end
#endregion
#region Initialize players in double battles
    if @doublebattle
      if @player.is_a?(Array)
        sendout1=pbFindNextUnfainted(@party1,0,pbSecondPartyBegin(0))
        raise _INTL("Player 1 has no unfainted Pokémon") if sendout1<0
        sendout2=pbFindNextUnfainted(@party1,pbSecondPartyBegin(0))
        raise _INTL("Player 2 has no unfainted Pokémon") if sendout2<0
        @battlers[0].pbInitialize(@party1[sendout1],sendout1,false)
        @battlers[2].pbInitialize(@party1[sendout2],sendout2,false)
        pbDisplayBrief(_INTL("{1} sent\r\nout {2}! Go! {3}!",
           @player[1].fullname,@battlers[2].name,@battlers[0].name))
        pbSetSeen(@party1[sendout1])
        pbSetSeen(@party1[sendout2])
      else
        sendout1=pbFindNextUnfainted(@party1,0)
        sendout2=pbFindNextUnfainted(@party1,sendout1+1)
        if sendout1<0 || sendout2<0
          raise _INTL("Player doesn't have two unfainted Pokémon")
        end
        @battlers[0].pbInitialize(@party1[sendout1],sendout1,false)
        @battlers[2].pbInitialize(@party1[sendout2],sendout2,false)
        pbDisplayBrief(_INTL("Go! {1} and {2}!",@battlers[0].name,@battlers[2].name))
      end
      pbSendOut(0,@party1[sendout1])
      pbSendOut(2,@party1[sendout2])
#endregion
#region Initialize player in single battles
    else
      sendout=pbFindNextUnfainted(@party1,0)
      if sendout<0
        raise _INTL("Player has no unfainted Pokémon")
      end
      @battlers[0].pbInitialize(@party1[sendout],sendout,false)
      pbDisplayBrief(_INTL("Go! {1}!",@battlers[0].name))
      pbSendOut(0,@party1[sendout])
    end
#endregion
#region Initialize battle
    if @weather==Weather.SUNNYDAY
      pbCommonAnimation("Sunny",null,null)
      pbDisplay(_INTL("The sunlight is strong."))
    elsif @weather==Weather.RAINDANCE
      pbCommonAnimation("Rain",null,null)
      pbDisplay(_INTL("It is raining."))
    elsif @weather==Weather.SANDSTORM
      pbCommonAnimation("Sandstorm",null,null)
      pbDisplay(_INTL("A sandstorm is raging."))
    elsif @weather==Weather.HAIL
      pbCommonAnimation("Hail",null,null)
      pbDisplay(_INTL("Hail is falling."))
    elsif @weather==Weather.HEAVYRAIN
      pbCommonAnimation("HeavyRain",null,null)
      pbDisplay(_INTL("It is raining heavily."))
    elsif @weather==Weather.HARSHSUN
      pbCommonAnimation("HarshSun",null,null)
      pbDisplay(_INTL("The sunlight is extremely harsh."))
    elsif @weather==Weather.STRONGWINDS
      pbCommonAnimation("StrongWinds",null,null)
      pbDisplay(_INTL("The wind is strong."))
    end
    pbOnActiveAll   // Abilities
    @turncount=0
    loop do   // Now begin the battle loop
      GameDebug.Log($"")
      GameDebug.Log($"***Round #{@turncount+1}***")
      if @debug && @turncount>=100
        @decision=pbDecisionOnTime()
        GameDebug.Log($"")
        GameDebug.Log($"***Undecided after 100 rounds, aborting***")
        pbAbort
        break
      end
      PBDebug.logonerr{
         pbCommandPhase
      }
      break if @decision>0
      PBDebug.logonerr{
         pbAttackPhase
      }
      break if @decision>0
      PBDebug.logonerr{
         pbEndOfRoundPhase
      }
      break if @decision>0
      @turncount+=1
    end
    return pbEndOfBattle(canlose)
  end
#endregion
		#endregion

		#region Command phase.
  def pbCommandMenu(i)
    return @scene.pbCommandMenu(i)
  end

  def pbItemMenu(i)
    return @scene.pbItemMenu(i)
  end

  def pbAutoFightMenu(i)
    return false
  end

  def pbCommandPhase
    @scene.pbBeginCommandPhase
    @scene.pbResetCommandIndices
    for (int i = 0; i < 4; i++) {   // Reset choices if commands can be shown
      @battlers[i].effects.SkipTurn=false
      if pbCanShowCommands?(i) || @battlers[i].isFainted?
        @choices[i][0]=0
        @choices[i][1]=0
        @choices[i][2]=null
        @choices[i][3]=-1
      else
        unless !@doublebattle && pbIsDoubleBattler?(i)
          GameDebug.Log($"[Reusing commands] #{@battlers[i].pbThis(true)}")
        end
      end
    end
    // Reset choices to perform Mega Evolution if it wasn't done somehow
    for (int i = 0; i < 2; i++) {
      for (int j = 0; j < @megaEvolution[i].length; j++) {
        @megaEvolution[i][j]=-1 if @megaEvolution[i][j]>=0
      end
    end
    for (int i = 0; i < 4; i++) {
      break if @decision!=0
      next if @choices[i][0]!=0
      if !pbOwnedByPlayer?(i) || @controlPlayer
        if !@battlers[i].isFainted? && pbCanShowCommands?(i)
          @scene.pbChooseEnemyCommand(i)
        end
      else
        commandDone=false
        commandEnd=false
        if pbCanShowCommands?(i)
          loop do
            cmd=pbCommandMenu(i)
            if cmd==0 // Fight
              if pbCanShowFightMenu?(i)
                commandDone=true if pbAutoFightMenu(i)
                until commandDone
                  index=@scene.pbFightMenu(i)
                  if index<0
                    side=(pbIsOpposing?(i)) ? 1 : 0
                    owner=pbGetOwnerIndex(i)
                    if @megaEvolution[side][owner]==i
                      @megaEvolution[side][owner]=-1
                    end
                    break
                  end
                  next if !pbRegisterMove(i,index)
                  if @doublebattle
                    thismove=@battlers[i].moves[index]
                    target=@battlers[i].pbTarget(thismove)
                    if target==Targets.SingleNonUser // single non-user
                      target=@scene.pbChooseTarget(i,target)
                      next if target<0
                      pbRegisterTarget(i,target)
                    elsif target==Targets.UserOrPartner // Acupressure
                      target=@scene.pbChooseTarget(i,target)
                      next if target<0 || (target&1)==1
                      pbRegisterTarget(i,target)
                    end
                  end
                  commandDone=true
                end
              else
                pbAutoChooseMove(i)
                commandDone=true
              end
            elsif cmd!=0 && @battlers[i].effects.SkyDrop
              pbDisplay(_INTL("Sky Drop won't let {1} go!",@battlers[i].pbThis(true)))
            elsif cmd==1 // Bag
              if !@internalbattle
                if pbOwnedByPlayer?(i)
                  pbDisplay(_INTL("Items can't be used here."))
                end
              else
                item=pbItemMenu(i)
                if item[0]>0
                  if pbRegisterItem(i,item[0],item[1])
                    commandDone=true
                  end
                end
              end
            elsif cmd==2 // Pokémon
              pkmn=pbSwitchPlayer(i,false,true)
              if pkmn>=0
                commandDone=true if pbRegisterSwitch(i,pkmn)
              end
            elsif cmd==3   // Run
              run=pbRun(i) 
              if run>0
                commandDone=true
                return
              elsif run<0
                commandDone=true
                side=(pbIsOpposing?(i)) ? 1 : 0
                owner=pbGetOwnerIndex(i)
                if @megaEvolution[side][owner]==i
                  @megaEvolution[side][owner]=-1
                end
              end
            elsif cmd==4   // Call
              thispkmn=@battlers[i]
              @choices[i][0]=4   // "Call Pokémon"
              @choices[i][1]=0
              @choices[i][2]=null
              side=(pbIsOpposing?(i)) ? 1 : 0
              owner=pbGetOwnerIndex(i)
              if @megaEvolution[side][owner]==i
                @megaEvolution[side][owner]=-1
              end
              commandDone=true
            elsif cmd==-1   // Go back to first battler's choice
              @megaEvolution[0][0]=-1 if @megaEvolution[0][0]>=0
              @megaEvolution[1][0]=-1 if @megaEvolution[1][0]>=0
              // Restore the item the player's first Pokémon was due to use
              if @choices[0][0]==3 && $PokemonBag && $PokemonBag.pbCanStore?(@choices[0][1])
                $PokemonBag.pbStoreItem(@choices[0][1])
              end
              pbCommandPhase
              return
            end
            break if commandDone
          end
        end
      end
    end
  end
		#endregion

		#region Attack phase.
  def pbAttackPhase
    @scene.pbBeginAttackPhase
    for (int i = 0; i < 4; i++) {
      @successStates[i].clear
      if @choices[i][0]!=1 && @choices[i][0]!=2
        @battlers[i].effects.DestinyBond=false
        @battlers[i].effects.Grudge=false
      end
      @battlers[i].turncount+=1 if !@battlers[i].isFainted?
      @battlers[i].effects.Rage=false if !pbChoseMove?(i,:RAGE)
    end
    // Calculate priority at this time
    @usepriority=false
    priority=pbPriority(false,true)
    // Mega Evolution
    megaevolved=[]
    foreach (var i in priority) {
      if @choices[i.index][0]==1 && !i.effects.SkipTurn
        side=(pbIsOpposing?(i.index)) ? 1 : 0
        owner=pbGetOwnerIndex(i.index)
        if @megaEvolution[side][owner]==i.index
          pbMegaEvolve(i.index)
          megaevolved.push(i.index)
        end
      end
    end
    if megaevolved.length>0
      foreach (var i in priority) {
        i.pbAbilitiesOnSwitchIn(true) if megaevolved.include?(i.index)
      end
    end
    // Call at Pokémon
    foreach (var i in priority) {
      if @choices[i.index][0]==4 && !i.effects.SkipTurn
        pbCall(i.index)
      end
    end
    // Switch out Pokémon
    @switching=true
    switched=[]
    foreach (var i in priority) {
      if @choices[i.index][0]==2 && !i.effects.SkipTurn
        index=@choices[i.index][1] // party position of Pokémon to switch to
        newpokename=index
        if pbParty(i.index)[index].ability == Abilities.ILLUSION
          newpokename=pbGetLastPokeInTeam(i.index)
        end
        self.lastMoveUser=i.index
        if !pbOwnedByPlayer?(i.index)
          owner=pbGetOwner(i.index)
          pbDisplayBrief(_INTL("{1} withdrew {2}!",owner.fullname,i.name))
          GameDebug.Log($"[Withdrew Pokémon] Opponent withdrew #{i.pbThis(true)}")
        else
          pbDisplayBrief(_INTL("{1}, that's enough!\r\nCome back!",i.name))
          GameDebug.Log($"[Withdrew Pokémon] Player withdrew #{i.pbThis(true)}")
        end
        foreach (var j in priority) {
          next if !i.pbIsOpposing?(j.index)
          // if Pursuit and this target ("i") was chosen
          if pbChoseMoveFunctionCode?(j.index,0x88) && // Pursuit
             !j.hasMovedThisRound?
            if j.status!=Statuses.SLEEP && j.status!=Statuses.FROZEN &&
               !j.effects.SkyDrop &&
               (!j.Ability == Abilities.TRUANT || !j.effects.Truant)
              @choices[j.index][3]=i.index // Make sure to target the switching Pokémon
              j.pbUseMove(@choices[j.index]) // This calls pbGainEXP as appropriate
              j.effects.Pursuit=true
              @switching=false
              return if @decision>0
            end
          end
          break if i.isFainted?
        end
        if !pbRecallAndReplace(i.index,index,newpokename)
          // If a forced switch somehow occurs here in single battles
          // the attack phase now ends
          if !@doublebattle
            @switching=false
            return
          end
        else
          switched.push(i.index)
        end
      end
    end
    if switched.length>0
      foreach (var i in priority) {
        i.pbAbilitiesOnSwitchIn(true) if switched.include?(i.index)
      end
    end
    @switching=false
    // Use items
    foreach (var i in priority) {
      if @choices[i.index][0]==3 && !i.effects.SkipTurn
        if pbIsOpposing?(i.index)
          // Opponent use item
          pbEnemyUseItem(@choices[i.index][1],i)
        else
          // Player use item
          item=@choices[i.index][1]
          if item>0
            usetype=$ItemData[item][ITEMBATTLEUSE]
            if usetype==1 || usetype==3
              if @choices[i.index][2]>=0
                pbUseItemOnPokemon(item,@choices[i.index][2],i,@scene)
              end
            elsif usetype==2 || usetype==4
              if !ItemHandlers.hasUseInBattle(item) // Poké Ball/Poké Doll used already
                pbUseItemOnBattler(item,@choices[i.index][2],i,@scene)
              end
            end
          end
        end
      end
    end
    // Use attacks
    foreach (var i in priority) {
      next if i.effects.SkipTurn
      if pbChoseMoveFunctionCode?(i.index,0x115) // Focus Punch
        pbCommonAnimation("FocusPunch",i,null)
        pbDisplay(_INTL("{1} is tightening its focus!",i.pbThis))
      end
    end
    10.times do
      // Forced to go next
      advance=false
      foreach (var i in priority) {
        next if !i.effects.MoveNext
        next if i.hasMovedThisRound? || i.effects.SkipTurn
        advance=i.pbProcessTurn(@choices[i.index])
        break if advance
      end
      return if @decision>0
      next if advance
      // Regular priority order
      foreach (var i in priority) {
        next if i.effects.Quash
        next if i.hasMovedThisRound? || i.effects.SkipTurn
        advance=i.pbProcessTurn(@choices[i.index])
        break if advance
      end
      return if @decision>0
      next if advance
      // Quashed
      foreach (var i in priority) {
        next if !i.effects.Quash
        next if i.hasMovedThisRound? || i.effects.SkipTurn
        advance=i.pbProcessTurn(@choices[i.index])
        break if advance
      end
      return if @decision>0
      next if advance
      // Check for all done
      foreach (var i in priority) {
        advance=true if @choices[i.index][0]==1 && !i.hasMovedThisRound? &&
                        !i.effects.SkipTurn
        break if advance
      end
      next if advance
      break
    end
    pbWait(20)
  end
		#endregion

		#region End of round.
  def pbEndOfRoundPhase
    GameDebug.Log($"[End of round]")
    for (int i = 0; i < 4; i++) {
      @battlers[i].effects.Electrify=false
      @battlers[i].effects.Endure=false
      @battlers[i].effects.FirstPledge=0
      @battlers[i].effects.HyperBeam-=1 if @battlers[i].effects.HyperBeam>0
      @battlers[i].effects.KingsShield=false
      @battlers[i].effects.LifeOrb=false
      @battlers[i].effects.MoveNext=false
      @battlers[i].effects.Powder=false
      @battlers[i].effects.Protect=false
      @battlers[i].effects.ProtectNegation=false
      @battlers[i].effects.Quash=false
      @battlers[i].effects.Roost=false
      @battlers[i].effects.SpikyShield=false
    end
    @usepriority=false  // recalculate priority
    priority=pbPriority(true) // Ignoring Quick Claw here
    // Weather
    case @weather
    when Weather.SUNNYDAY
      @weatherduration=@weatherduration-1 if @weatherduration>0
      if @weatherduration==0
        pbDisplay(_INTL("The sunlight faded."))
        @weather=0
        GameDebug.Log($"[End of effect] Sunlight weather ended")
      else
        pbCommonAnimation("Sunny",null,null)
        pbDisplay(_INTL("The sunlight is strong."))
        if pbWeather==Weather.SUNNYDAY
          foreach (var i in priority) {
            if i.Ability == Abilities.SOLARPOWER
              GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Solar Power")
              @scene.pbDamageAnimation(i,0)
              i.pbReduceHP((i.totalhp/8).floor)
              pbDisplay(_INTL("{1} was hurt by the sunlight!",i.pbThis))
              if i.isFainted?
                return if !i.pbFaint
              end
            end
          end
        end
      end
    when Weather.RAINDANCE
      @weatherduration=@weatherduration-1 if @weatherduration>0
      if @weatherduration==0
        pbDisplay(_INTL("The rain stopped."))
        @weather=0
        GameDebug.Log($"[End of effect] Rain weather ended")
      else
        pbCommonAnimation("Rain",null,null)
        pbDisplay(_INTL("Rain continues to fall."))
      end
    when Weather.SANDSTORM
      @weatherduration=@weatherduration-1 if @weatherduration>0
      if @weatherduration==0
        pbDisplay(_INTL("The sandstorm subsided."))
        @weather=0
        GameDebug.Log($"[End of effect] Sandstorm weather ended")
      else
        pbCommonAnimation("Sandstorm",null,null)
        pbDisplay(_INTL("The sandstorm rages."))
        if pbWeather==Weather.SANDSTORM
          GameDebug.Log($"[Lingering effect triggered] Sandstorm weather damage")
          foreach (var i in priority) {
            next if i.isFainted?
            if !i.pbHasType?(:GROUND) && !i.pbHasType?(:ROCK) && !i.pbHasType?(:STEEL) &&
               !i.Ability == Abilities.SANDVEIL &&
               !i.Ability == Abilities.SANDRUSH &&
               !i.Ability == Abilities.SANDFORCE &&
               !i.Ability == Abilities.MAGICGUARD &&
               !i.Ability == Abilities.OVERCOAT &&
               !i.Item == Items.SAFETYGOGGLES &&
               ![0xCA,0xCB].include?(PBMoveData.new(i.effects.TwoTurnAttack).function) // Dig, Dive
              @scene.pbDamageAnimation(i,0)
              i.pbReduceHP((i.totalhp/16).floor)
              pbDisplay(_INTL("{1} is buffeted by the sandstorm!",i.pbThis))
              if i.isFainted?
                return if !i.pbFaint
              end
            end
          end
        end
      end
    when Weather.HAIL
      @weatherduration=@weatherduration-1 if @weatherduration>0
      if @weatherduration==0
        pbDisplay(_INTL("The hail stopped."))
        @weather=0
        GameDebug.Log($"[End of effect] Hail weather ended")
      else
        pbCommonAnimation("Hail",null,null)
        pbDisplay(_INTL("Hail continues to fall."))
        if pbWeather==Weather.HAIL
          GameDebug.Log($"[Lingering effect triggered] Hail weather damage")
          foreach (var i in priority) {
            next if i.isFainted?
            if !i.pbHasType?(:ICE) &&
               !i.Ability == Abilities.ICEBODY &&
               !i.Ability == Abilities.SNOWCLOAK &&
               !i.Ability == Abilities.MAGICGUARD &&
               !i.Ability == Abilities.OVERCOAT &&
               !i.Item == Items.SAFETYGOGGLES &&
               ![0xCA,0xCB].include?(PBMoveData.new(i.effects.TwoTurnAttack).function) // Dig, Dive
              @scene.pbDamageAnimation(i,0)
              i.pbReduceHP((i.totalhp/16).floor)
              pbDisplay(_INTL("{1} is buffeted by the hail!",i.pbThis))
              if i.isFainted?
                return if !i.pbFaint
              end
            end
          end
        end
      end
    when Weather.HEAVYRAIN
      hasabil=false
      for (int i = 0; i < 4; i++) {
        if @battlers[i].ability == Abilities.PRIMORDIALSEA && !@battlers[i].isFainted?
          hasabil=true; break
        end
      end
      @weatherduration=0 if !hasabil
      if @weatherduration==0
        pbDisplay(_INTL("The heavy rain stopped."))
        @weather=0
        GameDebug.Log($"[End of effect] Primordial Sea's rain weather ended")
      else
        pbCommonAnimation("HeavyRain",null,null)
        pbDisplay(_INTL("It is raining heavily."))
      end
    when Weather.HARSHSUN
      hasabil=false
      for (int i = 0; i < 4; i++) {
        if @battlers[i].ability == Abilities.DESOLATELAND && !@battlers[i].isFainted?
          hasabil=true; break
        end
      end
      @weatherduration=0 if !hasabil
      if @weatherduration==0
        pbDisplay(_INTL("The harsh sunlight faded."))
        @weather=0
        GameDebug.Log($"[End of effect] Desolate Land's sunlight weather ended")
      else
        pbCommonAnimation("HarshSun",null,null)
        pbDisplay(_INTL("The sunlight is extremely harsh."))
        if pbWeather==Weather.HARSHSUN
          foreach (var i in priority) {
            if i.Ability == Abilities.SOLARPOWER
              GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Solar Power")
              @scene.pbDamageAnimation(i,0)
              i.pbReduceHP((i.totalhp/8).floor)
              pbDisplay(_INTL("{1} was hurt by the sunlight!",i.pbThis))
              if i.isFainted?
                return if !i.pbFaint
              end
            end
          end
        end
      end
    when Weather.STRONGWINDS
      hasabil=false
      for (int i = 0; i < 4; i++) {
        if @battlers[i].ability == Abilities.DELTASTREAM && !@battlers[i].isFainted?
          hasabil=true; break
        end
      end
      @weatherduration=0 if !hasabil
      if @weatherduration==0
        pbDisplay(_INTL("The air current subsided."))
        @weather=0
        GameDebug.Log($"[End of effect] Delta Stream's wind weather ended")
      else
        pbCommonAnimation("StrongWinds",null,null)
        pbDisplay(_INTL("The wind is strong."))
      end
    end
    // Shadow Sky weather
    if @weather == Weather.SHADOWSKY
      @weatherduration=@weatherduration-1 if @weatherduration>0
      if @weatherduration==0
        pbDisplay(_INTL("The shadow sky faded."))
        @weather=0
        GameDebug.Log($"[End of effect] Shadow Sky weather ended")
      else
        pbCommonAnimation("ShadowSky",null,null)
        pbDisplay(_INTL("The shadow sky continues."));
        if pbWeather == Weather.SHADOWSKY
          GameDebug.Log($"[Lingering effect triggered] Shadow Sky weather damage")
          foreach (var i in priority) {
            next if i.isFainted?
            if !i.isShadow?
              @scene.pbDamageAnimation(i,0)
              i.pbReduceHP((i.totalhp/16).floor)
              pbDisplay(_INTL("{1} was hurt by the shadow sky!",i.pbThis))
              if i.isFainted?
                return if !i.pbFaint
              end
            end
          end
        end
      end
    end
    // Future Sight/Doom Desire
    foreach (var i in battlers) {   // not priority
      next if i.isFainted?
      if i.effects.FutureSight>0
        i.effects.FutureSight-=1
        if i.effects.FutureSight==0
          move=i.effects.FutureSightMove
          GameDebug.Log($"[Lingering effect triggered] #{PBMoves.getName(move)} struck #{i.pbThis(true)}")
          pbDisplay(_INTL("{1} took the {2} attack!",i.pbThis,PBMoves.getName(move)))
          moveuser=null
          foreach (var j in battlers) {
            next if j.pbIsOpposing?(i.effects.FutureSightUserPos)
            if j.pokemonIndex==i.effects.FutureSightUser && !j.isFainted?
              moveuser=j; break
            end
          end
          if !moveuser
            party=pbParty(i.effects.FutureSightUserPos)
            if party[i.effects.FutureSightUser].hp>0
              moveuser=PokeBattle_Battler.new(self,i.effects.FutureSightUserPos)
              moveuser.pbInitDummyPokemon(party[i.effects.FutureSightUser],
                                          i.effects.FutureSightUser)
            end
          end
          if !moveuser
            pbDisplay(_INTL("But it failed!"))
          else
            @futuresight=true
            moveuser.pbUseMoveSimple(move,-1,i.index)
            @futuresight=false
          end
          i.effects.FutureSight=0
          i.effects.FutureSightMove=0
          i.effects.FutureSightUser=-1
          i.effects.FutureSightUserPos=-1
          if i.isFainted?
            return if !i.pbFaint
            next
          end
        end
      end
    end
    foreach (var i in priority) {
      next if i.isFainted?
      // Rain Dish
      if i.Ability == Abilities.RAINDISH &&
         (pbWeather==Weather.RAINDANCE ||
         pbWeather==Weather.HEAVYRAIN)
        GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Rain Dish")
        hpgain=i.pbRecoverHP((i.totalhp/16).floor,true)
        pbDisplay(_INTL("{1}'s {2} restored its HP a little!",i.pbThis,PBAbilities.getName(i.ability))) if hpgain>0
      end
      // Dry Skin
      if i.Ability == Abilities.DRYSKIN
        if pbWeather==Weather.RAINDANCE ||
           pbWeather==Weather.HEAVYRAIN
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Dry Skin (in rain)")
          hpgain=i.pbRecoverHP((i.totalhp/8).floor,true)
          pbDisplay(_INTL("{1}'s {2} was healed by the rain!",i.pbThis,PBAbilities.getName(i.ability))) if hpgain>0
        elsif pbWeather==Weather.SUNNYDAY ||
              pbWeather==Weather.HARSHSUN
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Dry Skin (in sun)")
          @scene.pbDamageAnimation(i,0)
          hploss=i.pbReduceHP((i.totalhp/8).floor)
          pbDisplay(_INTL("{1}'s {2} was hurt by the sunlight!",i.pbThis,PBAbilities.getName(i.ability))) if hploss>0
        end
      end
      // Ice Body
      if i.Ability == Abilities.ICEBODY && pbWeather==Weather.HAIL
        GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Ice Body")
        hpgain=i.pbRecoverHP((i.totalhp/16).floor,true)
        pbDisplay(_INTL("{1}'s {2} restored its HP a little!",i.pbThis,PBAbilities.getName(i.ability))) if hpgain>0
      end
      if i.isFainted?
        return if !i.pbFaint
      end
    end
    // Wish
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Wish>0
        i.effects.Wish-=1
        if i.effects.Wish==0
          GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s Wish")
          hpgain=i.pbRecoverHP(i.effects.WishAmount,true)
          if hpgain>0
            wishmaker=pbThisEx(i.index,i.effects.WishMaker)
            pbDisplay(_INTL("{1}'s wish came true!",wishmaker))
          end
        end
      end
    end
    // Fire Pledge + Grass Pledge combination damage
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.SeaOfFire>0 &&
         pbWeather!=Weather.RAINDANCE &&
         pbWeather!=Weather.HEAVYRAIN
        @battle.pbCommonAnimation("SeaOfFire",null,null) if i==0
        @battle.pbCommonAnimation("SeaOfFireOpp",null,null) if i==1
        foreach (var j in priority) {
          next if (j.index&1)!=i
          next if j.pbHasType?(:FIRE) || j.Ability == Abilities.MAGICGUARD
          @scene.pbDamageAnimation(j,0)
          hploss=j.pbReduceHP((j.totalhp/8).floor)
          pbDisplay(_INTL("{1} is hurt by the sea of fire!",j.pbThis)) if hploss>0
          if j.isFainted?
            return if !j.pbFaint
          end
        end
      end
    end
    foreach (var i in priority) {
      next if i.isFainted?
      // Shed Skin, Hydration
      if (i.Ability == Abilities.SHEDSKIN && pbRandom(10)<3) ||
         (i.Ability == Abilities.HYDRATION && (pbWeather==Weather.RAINDANCE ||
                                              pbWeather==Weather.HEAVYRAIN))
        if i.status>0
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s #{PBAbilities.getName(i.ability)}")
          s=i.status
          i.pbCureStatus(false)
          case s
          when Statuses.SLEEP
            pbDisplay(_INTL("{1}'s {2} cured its sleep problem!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.POISON
            pbDisplay(_INTL("{1}'s {2} cured its poison problem!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.BURN
            pbDisplay(_INTL("{1}'s {2} healed its burn!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.PARALYSIS
            pbDisplay(_INTL("{1}'s {2} cured its paralysis!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.FROZEN
            pbDisplay(_INTL("{1}'s {2} thawed it out!",i.pbThis,PBAbilities.getName(i.ability)))
          end
        end
      end
      // Healer
      if i.Ability == Abilities.HEALER && pbRandom(10)<3
        partner=i.pbPartner
        if partner && partner.status>0
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s #{PBAbilities.getName(i.ability)}")
          s=partner.status
          partner.pbCureStatus(false)
          case s
          when Statuses.SLEEP
            pbDisplay(_INTL("{1}'s {2} cured its partner's sleep problem!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.POISON
            pbDisplay(_INTL("{1}'s {2} cured its partner's poison problem!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.BURN
            pbDisplay(_INTL("{1}'s {2} healed its partner's burn!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.PARALYSIS
            pbDisplay(_INTL("{1}'s {2} cured its partner's paralysis!",i.pbThis,PBAbilities.getName(i.ability)))
          when Statuses.FROZEN
            pbDisplay(_INTL("{1}'s {2} thawed its partner out!",i.pbThis,PBAbilities.getName(i.ability)))
          end
        end
      end
    end
    foreach (var i in priority) {
      next if i.isFainted?
      // Grassy Terrain (healing)
      if @field.effects.GrassyTerrain>0 && !i.isAirborne?
        hpgain=i.pbRecoverHP((i.totalhp/16).floor,true)
        pbDisplay(_INTL("{1}'s HP was restored.",i.pbThis)) if hpgain>0
      end
      // Held berries/Leftovers/Black Sludge
      i.pbBerryCureCheck(true)
      if i.isFainted?
        return if !i.pbFaint
      end
    end
    // Aqua Ring
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.AquaRing
        GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s Aqua Ring")
        hpgain=(i.totalhp/16).floor
        hpgain=(hpgain*1.3).floor if i.Item == Items.BIGROOT
        hpgain=i.pbRecoverHP(hpgain,true)
        pbDisplay(_INTL("Aqua Ring restored {1}'s HP!",i.pbThis)) if hpgain>0
      end
    end
    // Ingrain
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Ingrain
        GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s Ingrain")
        hpgain=(i.totalhp/16).floor
        hpgain=(hpgain*1.3).floor if i.Item == Items.BIGROOT
        hpgain=i.pbRecoverHP(hpgain,true)
        pbDisplay(_INTL("{1} absorbed nutrients with its roots!",i.pbThis)) if hpgain>0
      end
    end
    // Leech Seed
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.LeechSeed>=0 && !i.Ability == Abilities.MAGICGUARD
        recipient=@battlers[i.effects.LeechSeed]
        if recipient && !recipient.isFainted?
          GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s Leech Seed")
          pbCommonAnimation("LeechSeed",recipient,i)
          hploss=i.pbReduceHP((i.totalhp/8).floor,true)
          if i.Ability == Abilities.LIQUIDOOZE
            recipient.pbReduceHP(hploss,true)
            pbDisplay(_INTL("{1} sucked up the liquid ooze!",recipient.pbThis))
          else
            if recipient.effects.HealBlock==0
              hploss=(hploss*1.3).floor if recipient.Item == Items.BIGROOT
              recipient.pbRecoverHP(hploss,true)
            end
            pbDisplay(_INTL("{1}'s health was sapped by Leech Seed!",i.pbThis))
          end
          if i.isFainted?
            return if !i.pbFaint
          end
          if recipient.isFainted?
            return if !recipient.pbFaint
          end
        end
      end
    end
    foreach (var i in priority) {
      next if i.isFainted?
      // Poison/Bad poison
      if i.status==Statuses.POISON
        if i.statusCount>0
          i.effects.Toxic+=1
          i.effects.Toxic=[15,i.effects.Toxic].min
        end
        if i.Ability == Abilities.POISONHEAL
          pbCommonAnimation("Poison",i,null)
          if i.effects.HealBlock==0 && i.hp<i.totalhp
            GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Poison Heal")
            i.pbRecoverHP((i.totalhp/8).floor,true)
            pbDisplay(_INTL("{1} is healed by poison!",i.pbThis))
          end
        else
          if !i.Ability == Abilities.MAGICGUARD
            GameDebug.Log($"[Status damage] #{i.pbThis} took damage from poison/toxic")
            if i.statusCount==0
              i.pbReduceHP((i.totalhp/8).floor)
            else
              i.pbReduceHP(((i.totalhp*i.effects.Toxic)/16).floor)
            end
            i.pbContinueStatus
          end
        end
      end
      // Burn
      if i.status==Statuses.BURN
        if !i.Ability == Abilities.MAGICGUARD
          GameDebug.Log($"[Status damage] #{i.pbThis} took damage from burn")
          if i.Ability == Abilities.HEATPROOF
            GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Heatproof")
            i.pbReduceHP((i.totalhp/16).floor)
          else
            i.pbReduceHP((i.totalhp/8).floor)
          end
        end
        i.pbContinueStatus
      end
      // Nightmare
      if i.effects.Nightmare
        if i.status==Statuses.SLEEP
          if !i.Ability == Abilities.MAGICGUARD
            GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s nightmare")
            i.pbReduceHP((i.totalhp/4).floor,true)
            pbDisplay(_INTL("{1} is locked in a nightmare!",i.pbThis))
          end
        else
          i.effects.Nightmare=false
        end
      end
      if i.isFainted?
        return if !i.pbFaint
        next
      end
    end
    // Curse
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Curse && !i.Ability == Abilities.MAGICGUARD
        GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s curse")
        i.pbReduceHP((i.totalhp/4).floor,true)
        pbDisplay(_INTL("{1} is afflicted by the curse!",i.pbThis))
      end
      if i.isFainted?
        return if !i.pbFaint
        next
      end
    end
    // Multi-turn attacks (Bind/Clamp/Fire Spin/Magma Storm/Sand Tomb/Whirlpool/Wrap)
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.MultiTurn>0
        i.effects.MultiTurn-=1
        movename=PBMoves.getName(i.effects.MultiTurnAttack)
        if i.effects.MultiTurn==0
          GameDebug.Log($"[End of effect] Trapping move #{movename} affecting #{i.pbThis} ended")
          pbDisplay(_INTL("{1} was freed from {2}!",i.pbThis,movename))
        else
          if i.effects.MultiTurnAttack == Moves.BIND
            pbCommonAnimation("Bind",i,null)
          elsif i.effects.MultiTurnAttack == Moves.CLAMP
            pbCommonAnimation("Clamp",i,null)
          elsif i.effects.MultiTurnAttack == Moves.FIRESPIN
            pbCommonAnimation("FireSpin",i,null)
          elsif i.effects.MultiTurnAttack == Moves.MAGMASTORM
            pbCommonAnimation("MagmaStorm",i,null)
          elsif i.effects.MultiTurnAttack == Moves.SANDTOMB
            pbCommonAnimation("SandTomb",i,null)
          elsif i.effects.MultiTurnAttack == Moves.WRAP
            pbCommonAnimation("Wrap",i,null)
          elsif i.effects.MultiTurnAttack == Moves.INFESTATION
            pbCommonAnimation("Infestation",i,null)
          else
            pbCommonAnimation("Wrap",i,null)
          end
          if !i.Ability == Abilities.MAGICGUARD
            GameDebug.Log($"[Lingering effect triggered] #{i.pbThis} took damage from trapping move #{movename}")
            @scene.pbDamageAnimation(i,0)
            amt=(USENEWBATTLEMECHANICS) ? (i.totalhp/8).floor : (i.totalhp/16).floor
            if @battlers[i.effects.MultiTurnUser].Item == Items.BINDINGBAND
              amt=(USENEWBATTLEMECHANICS) ? (i.totalhp/6).floor : (i.totalhp/8).floor
            end
            i.pbReduceHP(amt)
            pbDisplay(_INTL("{1} is hurt by {2}!",i.pbThis,movename))
          end
        end
      end  
      if i.isFainted?
        return if !i.pbFaint
      end
    end
    // Taunt
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Taunt>0
        i.effects.Taunt-=1
        if i.effects.Taunt==0
          pbDisplay(_INTL("{1}'s taunt wore off!",i.pbThis))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer taunted")
        end 
      end
    end
    // Encore
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Encore>0
        if i.moves[i.effects.EncoreIndex].id!=i.effects.EncoreMove
          i.effects.Encore=0
          i.effects.EncoreIndex=0
          i.effects.EncoreMove=0
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer encored (encored move was lost)")
        else
          i.effects.Encore-=1
          if i.effects.Encore==0 || i.moves[i.effects.EncoreIndex].pp==0
            i.effects.Encore=0
            pbDisplay(_INTL("{1}'s encore ended!",i.pbThis))
            GameDebug.Log($"[End of effect] #{i.pbThis} is no longer encored")
          end 
        end
      end
    end
    // Disable/Cursed Body
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Disable>0
        i.effects.Disable-=1
        if i.effects.Disable==0
          i.effects.DisableMove=0
          pbDisplay(_INTL("{1} is no longer disabled!",i.pbThis))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer disabled")
        end
      end
    end
    // Magnet Rise
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.MagnetRise>0
        i.effects.MagnetRise-=1
        if i.effects.MagnetRise==0
          pbDisplay(_INTL("{1} stopped levitating.",i.pbThis))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer levitating by Magnet Rise")
        end
      end
    end
    // Telekinesis
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Telekinesis>0
        i.effects.Telekinesis-=1
        if i.effects.Telekinesis==0
          pbDisplay(_INTL("{1} stopped levitating.",i.pbThis))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer levitating by Telekinesis")
        end
      end
    end
    // Heal Block
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.HealBlock>0
        i.effects.HealBlock-=1
        if i.effects.HealBlock==0
          pbDisplay(_INTL("{1}'s Heal Block wore off!",i.pbThis))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer Heal Blocked")
        end
      end
    end
    // Embargo
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Embargo>0
        i.effects.Embargo-=1
        if i.effects.Embargo==0
          pbDisplay(_INTL("{1} can use items again!",i.pbThis(true)))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer affected by an embargo")
        end
      end
    end
    // Yawn
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Yawn>0
        i.effects.Yawn-=1
        if i.effects.Yawn==0 && i.pbCanSleepYawn?
          GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s Yawn")
          i.pbSleep
        end
      end
    end
    // Perish Song
    perishSongUsers=[]
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.PerishSong>0
        i.effects.PerishSong-=1
        pbDisplay(_INTL("{1}'s perish count fell to {2}!",i.pbThis,i.effects.PerishSong))
        GameDebug.Log($"[Lingering effect triggered] #{i.pbThis}'s Perish Song count dropped to #{i.effects.PerishSong}")
        if i.effects.PerishSong==0
          perishSongUsers.push(i.effects.PerishSongUser)
          i.pbReduceHP(i.hp,true)
        end
      end
      if i.isFainted?
        return if !i.pbFaint
      end
    end
    if perishSongUsers.length>0
      // If all remaining Pokemon fainted by a Perish Song triggered by a single side
      if (perishSongUsers.find_all{|item| pbIsOpposing?(item) }.length==perishSongUsers.length) ||
         (perishSongUsers.find_all{|item| !pbIsOpposing?(item) }.length==perishSongUsers.length)
        pbJudgeCheckpoint(@battlers[perishSongUsers[0]])
      end
    end
    if @decision>0
      pbGainEXP
      return
    end
    // Reflect
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.Reflect>0
        sides[i].effects.Reflect-=1
        if sides[i].effects.Reflect==0
          pbDisplay(_INTL("Your team's Reflect faded!")) if i==0
          pbDisplay(_INTL("The opposing team's Reflect faded!")) if i==1
          GameDebug.Log($"[End of effect] Reflect ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Reflect ended on the opponent's side") if i==1
        end
      end
    end
    // Light Screen
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.LightScreen>0
        sides[i].effects.LightScreen-=1
        if sides[i].effects.LightScreen==0
          pbDisplay(_INTL("Your team's Light Screen faded!")) if i==0
          pbDisplay(_INTL("The opposing team's Light Screen faded!")) if i==1
          GameDebug.Log($"[End of effect] Light Screen ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Light Screen ended on the opponent's side") if i==1
        end
      end
    end
    // Safeguard
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.Safeguard>0
        sides[i].effects.Safeguard-=1
        if sides[i].effects.Safeguard==0
          pbDisplay(_INTL("Your team is no longer protected by Safeguard!")) if i==0
          pbDisplay(_INTL("The opposing team is no longer protected by Safeguard!")) if i==1
          GameDebug.Log($"[End of effect] Safeguard ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Safeguard ended on the opponent's side") if i==1
        end
      end
    end
    // Mist
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.Mist>0
        sides[i].effects.Mist-=1
        if sides[i].effects.Mist==0
          pbDisplay(_INTL("Your team's Mist faded!")) if i==0
          pbDisplay(_INTL("The opposing team's Mist faded!")) if i==1
          GameDebug.Log($"[End of effect] Mist ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Mist ended on the opponent's side") if i==1
        end
      end
    end
    // Tailwind
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.Tailwind>0
        sides[i].effects.Tailwind-=1
        if sides[i].effects.Tailwind==0
          pbDisplay(_INTL("Your team's Tailwind petered out!")) if i==0
          pbDisplay(_INTL("The opposing team's Tailwind petered out!")) if i==1
          GameDebug.Log($"[End of effect] Tailwind ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Tailwind ended on the opponent's side") if i==1
        end
      end
    end
    // Lucky Chant
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.LuckyChant>0
        sides[i].effects.LuckyChant-=1
        if sides[i].effects.LuckyChant==0
          pbDisplay(_INTL("Your team's Lucky Chant faded!")) if i==0
          pbDisplay(_INTL("The opposing team's Lucky Chant faded!")) if i==1
          GameDebug.Log($"[End of effect] Lucky Chant ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Lucky Chant ended on the opponent's side") if i==1
        end
      end
    end
    // End of Pledge move combinations
    for (int i = 0; i < 2; i++) {
      if sides[i].effects.Swamp>0
        sides[i].effects.Swamp-=1
        if sides[i].effects.Swamp==0
          pbDisplay(_INTL("The swamp around your team disappeared!")) if i==0
          pbDisplay(_INTL("The swamp around the opposing team disappeared!")) if i==1
          GameDebug.Log($"[End of effect] Grass Pledge's swamp ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Grass Pledge's swamp ended on the opponent's side") if i==1
        end
      end
      if sides[i].effects.SeaOfFire>0
        sides[i].effects.SeaOfFire-=1
        if sides[i].effects.SeaOfFire==0
          pbDisplay(_INTL("The sea of fire around your team disappeared!")) if i==0
          pbDisplay(_INTL("The sea of fire around the opposing team disappeared!")) if i==1
          GameDebug.Log($"[End of effect] Fire Pledge's sea of fire ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Fire Pledge's sea of fire ended on the opponent's side") if i==1
        end
      end
      if sides[i].effects.Rainbow>0
        sides[i].effects.Rainbow-=1
        if sides[i].effects.Rainbow==0
          pbDisplay(_INTL("The rainbow around your team disappeared!")) if i==0
          pbDisplay(_INTL("The rainbow around the opposing team disappeared!")) if i==1
          GameDebug.Log($"[End of effect] Water Pledge's rainbow ended on the player's side") if i==0
          GameDebug.Log($"[End of effect] Water Pledge's rainbow ended on the opponent's side") if i==1
        end
      end
    end
    // Gravity
    if @field.effects.Gravity>0
      @field.effects.Gravity-=1
      if @field.effects.Gravity==0
        pbDisplay(_INTL("Gravity returned to normal."))
        GameDebug.Log($"[End of effect] Strong gravity ended")
      end
    end
    // Trick Room
    if @field.effects.TrickRoom>0
      @field.effects.TrickRoom-=1
      if @field.effects.TrickRoom==0
        pbDisplay(_INTL("The twisted dimensions returned to normal."))
        GameDebug.Log($"[End of effect] Trick Room ended")
      end
    end
    // Wonder Room
    if @field.effects.WonderRoom>0
      @field.effects.WonderRoom-=1
      if @field.effects.WonderRoom==0
        pbDisplay(_INTL("Wonder Room wore off, and the Defense and Sp. Def stats returned to normal!"))
        GameDebug.Log($"[End of effect] Wonder Room ended")
      end
    end
    // Magic Room
    if @field.effects.MagicRoom>0
      @field.effects.MagicRoom-=1
      if @field.effects.MagicRoom==0
        pbDisplay(_INTL("The area returned to normal."))
        GameDebug.Log($"[End of effect] Magic Room ended")
      end
    end
    // Mud Sport
    if @field.effects.MudSportField>0
      @field.effects.MudSportField-=1
      if @field.effects.MudSportField==0
        pbDisplay(_INTL("The effects of Mud Sport have faded."))
        GameDebug.Log($"[End of effect] Mud Sport ended")
      end
    end
    // Water Sport
    if @field.effects.WaterSportField>0
      @field.effects.WaterSportField-=1
      if @field.effects.WaterSportField==0
        pbDisplay(_INTL("The effects of Water Sport have faded."))
        GameDebug.Log($"[End of effect] Water Sport ended")
      end
    end
    // Electric Terrain
    if @field.effects.ElectricTerrain>0
      @field.effects.ElectricTerrain-=1
      if @field.effects.ElectricTerrain==0
        pbDisplay(_INTL("The electric current disappeared from the battlefield."))
        GameDebug.Log($"[End of effect] Electric Terrain ended")
      end
    end
    // Grassy Terrain (counting down)
    if @field.effects.GrassyTerrain>0
      @field.effects.GrassyTerrain-=1
      if @field.effects.GrassyTerrain==0
        pbDisplay(_INTL("The grass disappeared from the battlefield."))
        GameDebug.Log($"[End of effect] Grassy Terrain ended")
      end
    end
    // Misty Terrain
    if @field.effects.MistyTerrain>0
      @field.effects.MistyTerrain-=1
      if @field.effects.MistyTerrain==0
        pbDisplay(_INTL("The mist disappeared from the battlefield."))
        GameDebug.Log($"[End of effect] Misty Terrain ended")
      end
    end
    // Uproar
    foreach (var i in priority) {
      next if i.isFainted?
      if i.effects.Uproar>0
        foreach (var j in priority) {
          if !j.isFainted? && j.status==Statuses.SLEEP && !j.Ability == Abilities.SOUNDPROOF
            GameDebug.Log($"[Lingering effect triggered] Uproar woke up #{j.pbThis(true)}")
            j.pbCureStatus(false)
            pbDisplay(_INTL("{1} woke up in the uproar!",j.pbThis))
          end
        end
        i.effects.Uproar-=1
        if i.effects.Uproar==0
          pbDisplay(_INTL("{1} calmed down.",i.pbThis))
          GameDebug.Log($"[End of effect] #{i.pbThis} is no longer uproaring")
        else
          pbDisplay(_INTL("{1} is making an uproar!",i.pbThis)) 
        end
      end
    end
    foreach (var i in priority) {
      next if i.isFainted?
      // Speed Boost
      // A Pokémon's turncount is 0 if it became active after the beginning of a round
      if i.turncount>0 && i.Ability == Abilities.SPEEDBOOST
        if i.pbIncreaseStatWithCause(Stats.SPEED,1,i,PBAbilities.getName(i.ability))
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s #{PBAbilities.getName(i.ability)}")
        end
      end
      // Bad Dreams
      if i.status==Statuses.SLEEP && !i.Ability == Abilities.MAGICGUARD
        if i.pbOpposing1.Ability == Abilities.BADDREAMS ||
           i.pbOpposing2.Ability == Abilities.BADDREAMS
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s opponent's Bad Dreams")
          hploss=i.pbReduceHP((i.totalhp/8).floor,true)
          pbDisplay(_INTL("{1} is having a bad dream!",i.pbThis)) if hploss>0
        end
      end
      if i.isFainted?
        return if !i.pbFaint
        next
      end
      // Pickup
      if i.Ability == Abilities.PICKUP && i.item<=0
        item=0; index=-1; use=0
        for (int j = 0; j < 4; j++) {
          next if j==i.index
          if @battlers[j].effects.PickupUse>use
            item=@battlers[j].effects.PickupItem
            index=j
            use=@battlers[j].effects.PickupUse
          end
        end
        if item>0
          i.item=item
          @battlers[index].effects.PickupItem=0
          @battlers[index].effects.PickupUse=0
          @battlers[index].pokemon.itemRecycle=0 if @battlers[index].pokemon.itemRecycle==item
          if !@opponent && // In a wild battle
             i.pokemon.itemInitial==0 &&
             @battlers[index].pokemon.itemInitial==item
            i.pokemon.itemInitial=item
            @battlers[index].pokemon.itemInitial=0
          end
          pbDisplay(_INTL("{1} found one {2}!",i.pbThis,PBItems.getName(item)))
          i.pbBerryCureCheck(true)
        end
      end
      // Harvest
      if i.Ability == Abilities.HARVEST && i.item<=0 && i.pokemon.itemRecycle>0
        if pbIsBerry?(i.pokemon.itemRecycle) &&
           (pbWeather==Weather.SUNNYDAY || 
           pbWeather==Weather.HARSHSUN || pbRandom(10)<5)
          i.item=i.pokemon.itemRecycle
          i.pokemon.itemRecycle=0
          i.pokemon.itemInitial=item if i.pokemon.itemInitial==0
          pbDisplay(_INTL("{1} harvested one {2}!",i.pbThis,PBItems.getName(i.item)))
          i.pbBerryCureCheck(true)
        end
      end
      // Moody
      if i.Ability == Abilities.MOODY
        randomup=[]; randomdown=[]
        foreeach (varj in new [] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,Stats.SPATK,
                  Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
          randomup.push(j) if i.pbCanIncreaseStatStage?(j,i)
          randomdown.push(j) if i.pbCanReduceStatStage?(j,i)
        end
        if randomup.length>0
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Moody (raise stat)")
          r=pbRandom(randomup.length)
          i.pbIncreaseStatWithCause(randomup[r],2,i,PBAbilities.getName(i.ability))
          for (int j = 0; j < randomdown.length; j++) {
            if randomdown[j]==randomup[r]
              randomdown[j]=null; randomdown.compact!
              break
            end
          end
        end
        if randomdown.length>0
          GameDebug.Log($"[Ability triggered] #{i.pbThis}'s Moody (lower stat)")
          r=pbRandom(randomdown.length)
          i.pbReduceStatWithCause(randomdown[r],1,i,PBAbilities.getName(i.ability))
        end
      end
    end
    foreach (var i in priority) {
      next if i.isFainted?
      // Toxic Orb
      if i.Item == Items.TOXICORB && i.status==0 && i.pbCanPoison?(null,false)
        GameDebug.Log($"[Item triggered] #{i.pbThis}'s Toxic Orb")
        i.pbPoison(null,_INTL("{1} was badly poisoned by its {2}!",i.pbThis,
           PBItems.getName(i.item)),true)
      end
      // Flame Orb
      if i.Item == Items.FLAMEORB && i.status==0 && i.pbCanBurn?(null,false)
        GameDebug.Log($"[Item triggered] #{i.pbThis}'s Flame Orb")
        i.pbBurn(null,_INTL("{1} was burned by its {2}!",i.pbThis,PBItems.getName(i.item)))
      end
      // Sticky Barb
      if i.Item == Items.STICKYBARB && !i.Ability == Abilities.MAGICGUARD
        GameDebug.Log($"[Item triggered] #{i.pbThis}'s Sticky Barb")
        @scene.pbDamageAnimation(i,0)
        i.pbReduceHP((i.totalhp/8).floor)
        pbDisplay(_INTL("{1} is hurt by its {2}!",i.pbThis,PBItems.getName(i.item)))
      end
      if i.isFainted?
        return if !i.pbFaint
      end
    end
    // Form checks
    for (int i = 0; i < 4; i++) {
      next if @battlers[i].isFainted?
      @battlers[i].pbCheckForm
    end
    pbGainEXP
    pbSwitch
    return if @decision>0
    foreach (var i in priority) {
      next if i.isFainted?
      i.pbAbilitiesOnSwitchIn(false)
    end
    // Healing Wish/Lunar Dance - should go here
    // Spikes/Toxic Spikes/Stealth Rock - should go here (in order of their 1st use)
    for (int i = 0; i < 4; i++) {
      if @battlers[i].turncount>0 && @battlers[i].Ability == Abilities.TRUANT
        @battlers[i].effects.Truant=!@battlers[i].effects.Truant
      end
      if @battlers[i].effects.LockOn>0   // Also Mind Reader
        @battlers[i].effects.LockOn-=1
        @battlers[i].effects.LockOnPos=-1 if @battlers[i].effects.LockOn==0
      end
      @battlers[i].effects.Flinch=false
      @battlers[i].effects.FollowMe=0
      @battlers[i].effects.HelpingHand=false
      @battlers[i].effects.MagicCoat=false
      @battlers[i].effects.Snatch=false
      @battlers[i].effects.Charge-=1 if @battlers[i].effects.Charge>0
      @battlers[i].lastHPLost=0
      @battlers[i].tookDamage=false
      @battlers[i].lastAttacker.clear
      @battlers[i].effects.Counter=-1
      @battlers[i].effects.CounterTarget=-1
      @battlers[i].effects.MirrorCoat=-1
      @battlers[i].effects.MirrorCoatTarget=-1
    end
    for (int i = 0; i < 2; i++) {
      if !@sides[i].effects.EchoedVoiceUsed
        @sides[i].effects.EchoedVoiceCounter=0
      end
      @sides[i].effects.EchoedVoiceUsed=false
      @sides[i].effects.QuickGuard=false
      @sides[i].effects.WideGuard=false
      @sides[i].effects.CraftyShield=false
      @sides[i].effects.Round=0
    end
    @field.effects.FusionBolt=false
    @field.effects.FusionFlare=false
    @field.effects.IonDeluge=false
    @field.effects.FairyLock-=1 if @field.effects.FairyLock>0
    // invalidate stored priority
    @usepriority=false
  end
		#endregion

		#region End of battle.
  def pbEndOfBattle(canlose=false)
    case @decision
    //#### WIN ####//
    when 1
      GameDebug.Log($"")
      GameDebug.Log($"***Player won***")
      if @opponent
        @scene.pbTrainerBattleSuccess
        if @opponent.is_a?(Array)
          pbDisplayPaused(_INTL("{1} defeated {2} and {3}!",self.pbPlayer.name,@opponent[0].fullname,@opponent[1].fullname))
        else
          pbDisplayPaused(_INTL("{1} defeated\r\n{2}!",self.pbPlayer.name,@opponent.fullname))
        end
        @scene.pbShowOpponent(0)
        pbDisplayPaused(@endspeech.gsub(/\\[Pp][Nn]/,self.pbPlayer.name))
        if @opponent.is_a?(Array)
          @scene.pbHideOpponent
          @scene.pbShowOpponent(1)
          pbDisplayPaused(@endspeech2.gsub(/\\[Pp][Nn]/,self.pbPlayer.name))
        end
        // Calculate money gained for winning
        if @internalbattle
          tmoney=0
          if @opponent.is_a?(Array)   // Double battles
            maxlevel1=0; maxlevel2=0; limit=pbSecondPartyBegin(1)
            for (int i = 0; i < limit; i++) {
              if @party2[i]
                maxlevel1=@party2[i].level if maxlevel1<@party2[i].level
              end
              if @party2[i+limit]
                maxlevel2=@party2[i+limit].level if maxlevel1<@party2[i+limit].level
              end
            end
            tmoney+=maxlevel1*@opponent[0].moneyEarned
            tmoney+=maxlevel2*@opponent[1].moneyEarned
          else
            maxlevel=0
            foreach (var i in @party2) {
              next if !i
              maxlevel=i.level if maxlevel<i.level
            end
            tmoney+=maxlevel*@opponent.moneyEarned
          end
          // If Amulet Coin/Luck Incense's effect applies, double money earned
          tmoney*=2 if @amuletcoin
          // If Happy Hour's effect applies, double money earned
          tmoney*=2 if @doublemoney
          oldmoney=self.pbPlayer.money
          self.pbPlayer.money+=tmoney
          moneygained=self.pbPlayer.money-oldmoney
          if moneygained>0
            pbDisplayPaused(_INTL("{1} got ${2}\r\nfor winning!",self.pbPlayer.name,tmoney))
          end
        end
      end
      if @internalbattle && @extramoney>0
        @extramoney*=2 if @amuletcoin
        @extramoney*=2 if @doublemoney
        oldmoney=self.pbPlayer.money
        self.pbPlayer.money+=@extramoney
        moneygained=self.pbPlayer.money-oldmoney
        if moneygained>0
          pbDisplayPaused(_INTL("{1} picked up ${2}!",self.pbPlayer.name,@extramoney))
        end
      end
      foreach (var pkmn in @snaggedpokemon) {
        pbStorePokemon(pkmn)
        self.pbPlayer.shadowcaught=[] if !self.pbPlayer.shadowcaught
        self.pbPlayer.shadowcaught[pkmn.species]=true
      end
      @snaggedpokemon.clear
    //#### LOSE, DRAW ####// 
    when 2, 5
      GameDebug.Log($"")
      GameDebug.Log($"***Player lost***") if @decision==2
      GameDebug.Log($"***Player drew with opponent***") if @decision==5
      if @internalbattle
        pbDisplayPaused(_INTL("{1} is out of usable Pokémon!",self.pbPlayer.name))
        moneylost=pbMaxLevelFromIndex(0)   // Player's Pokémon only, not partner's
        multiplier=[8,16,24,36,48,60,80,100,120]
        moneylost*=multiplier[[multiplier.length-1,self.pbPlayer.numbadges].min]
        moneylost=self.pbPlayer.money if moneylost>self.pbPlayer.money
        moneylost=0 if $game_switches[NO_MONEY_LOSS]
        oldmoney=self.pbPlayer.money
        self.pbPlayer.money-=moneylost
        lostmoney=oldmoney-self.pbPlayer.money
        if @opponent
          if @opponent.is_a?(Array)
            pbDisplayPaused(_INTL("{1} lost against {2} and {3}!",self.pbPlayer.name,@opponent[0].fullname,@opponent[1].fullname))
          else
            pbDisplayPaused(_INTL("{1} lost against\r\n{2}!",self.pbPlayer.name,@opponent.fullname))
          end
          if moneylost>0
            pbDisplayPaused(_INTL("{1} paid ${2}\r\nas the prize money...",self.pbPlayer.name,lostmoney))  
            pbDisplayPaused(_INTL("...")) if !canlose
          end
        else
          if moneylost>0
            pbDisplayPaused(_INTL("{1} panicked and lost\r\n${2}...",self.pbPlayer.name,lostmoney))
            pbDisplayPaused(_INTL("...")) if !canlose
          end
        end
        pbDisplayPaused(_INTL("{1} blacked out!",self.pbPlayer.name)) if !canlose
      elsif @decision==2
        @scene.pbShowOpponent(0)
        pbDisplayPaused(@endspeechwin.gsub(/\\[Pp][Nn]/,self.pbPlayer.name))
        if @opponent.is_a?(Array)
          @scene.pbHideOpponent
          @scene.pbShowOpponent(1)
          pbDisplayPaused(@endspeechwin2.gsub(/\\[Pp][Nn]/,self.pbPlayer.name))
        end
      end
    end
    // Pass on Pokérus within the party
    infected=[]
    for (int i = 0; i < $Trainer.party.length; i++) {
      if $Trainer.party[i].pokerusStage==1
        infected.push(i)
      end
    end
    if infected.length>=1
      foreach (var i in infected) {
        strain=$Trainer.party[i].pokerus/16
        if i>0 && $Trainer.party[i-1].pokerusStage==0
          $Trainer.party[i-1].givePokerus(strain) if rand(3)==0
        end
        if i<$Trainer.party.length-1 && $Trainer.party[i+1].pokerusStage==0
          $Trainer.party[i+1].givePokerus(strain) if rand(3)==0
        end
      end
    end
    @scene.pbEndBattle(@decision)
    foreach (var i in @battlers) {
      i.pbResetForm
      if i.Ability == Abilities.NATURALCURE
        i.status=0
      end
    end
    foreach (var i in $Trainer.party) {
      i.setItem(i.itemInitial)
      i.itemInitial=i.itemRecycle=0
      i.belch=false
    end
    return @decision
  end
		#endregion
	}
}
