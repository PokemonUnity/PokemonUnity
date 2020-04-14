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
		//public void Environment (PokemonUnity.Environment environment) { this.environment = environment; }
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
		public Moves lastMoveUsed { get; private set; }
		/// <summary>
		/// Last move user
		/// </summary>
		public int lastMoveUser { get; private set; }
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
		/// 
		/// </summary>
		public List<string> rules { get; private set; }
		/// <summary>
		/// Counter to track number of turns for battle
		/// </summary>
		public byte turncount { get; set; }
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
		public Battle(Trainer player, Trainer opponent)
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
		
			if (p2.Length > 2 && opponent.ID == TrainerTypes.WildPokemon) { //opponent == null
				//raise ArgumentError.new(_INTL("Wild battles with more than two Pokémon are not allowed."))
				GameDebug.LogError("Wild battles with more than two Pokémon are not allowed.");
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

			//if (opponent != null && player.Length == 1) 
			//	this.player = player[0]; 

			//if (opponent != null && opponent.Length == 1)
			//	this.opponent = opponent[0];

			this.player = Game.GameData.Player; //player;
			this.opponent = opponent;
			party1 = Pokemon.GetBattlers(p1, this); //ToDo: Redo this...
			party2 = Pokemon.GetBattlers(p2, this); //ToDo: Redo this...

			party1order = new List<int>();

			//the #12 represents a double battle with 2 trainers using 6 pokemons each on 1 side
			for (int i = 0; i < 12; i++)
			{
				party1order.Add(i);
			}

			party2order = new List<int>();

			//for i in 0...12;party2order.push(i); }
			for (int i = 0; i < 12; i++)
			{
				party2order.Add(i);
			}

			fullparty1 = false;

			fullparty2 = false;

			battlers = new Pokemon[4];

			items = new List<Items>(); //null;

			//sides = [PokeBattle_ActiveSide.new,				// Player's side
			//                   PokeBattle_ActiveSide.new]		// Foe's side
			sides = new Effects.Side[2];						//ToDo: Not sure if it's 2 sides, or 4 sides (foreach pokemon)
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

			rules = new List<string>() { };

			turncount = 0;

			//peer = PokeBattle_BattlePeer.create()

			priority = new int[4];

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
		public bool pbOwnedByPlayer(int index)
		{
			return false;
		}
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
		public void StorePokemon(Pokemon pokemon)
		{
			//if(!pokemon.isShadow)
			//	//"Would you like to give a nickname to {1}?"
			//	if (DisplayConfirm(LanguageExtension.Translate(Text.ScriptTexts, "GiveNickname").Value))
			//	{
			//		//"{1}'s nickname?"
			//		pokemon.SetNickname(scene.NameEntry(LanguageExtension.Translate(Text.ScriptTexts, "SetNick", pokemon.Name).Value, pokemon));
			//	}
			//int oldcurbox = Game.CurrentBox;
			//int storedbox = Game.StorePokemon(Game.Player, pokemon)
			//string creator = Game.GetStorageCreator()
			//if (storedbox < 0) return;
			//string curboxname = Game.BoxName(oldcurbox)
			//string boxname = Game.BoxName(storedbox)
			//if (storedbox != oldcurbox) {
			//	if (Game.IsCreator)
			//		Game.DisplayPaused("Box \"{1}\" on {2}'s PC was full.", curboxname, creator)
			//	else
			//		DisplayPaused("Box \"{1}\" on someone's PC was full.", curboxname)
			//	DisplayPaused("{1} was transferred to box \"{2}\".", pokemon.name, boxname)
			//}else
			//{
			//  if (Game.IsCreator)
			//		DisplayPaused("{1} was transferred to {2}'s PC.", pokemon.name, creator)
			//  else {}
			//		DisplayPaused("{1} was transferred to someone's PC.", pokemon.name)
			//	}
			//	DisplayPaused("It was stored in box \"{1}\".", boxname)
			//}		
		}
		public void ThrowPokeball(int idxPokemon, Items ball, int? rareness = null, bool showplayer = false)
		{
			//ToDo: Repair Text Translation Dictionary
			string itemname = string.Empty;//LanguageExtension.Translate(Text.Items, ball.ToString()).Value;
			Pokemon battler = null;
			if (isOpposing(idxPokemon))
				battler = battlers[idxPokemon];
			else
				battler = battlers[idxPokemon].OppositeOpposing;
			if (battler.isFainted())
				battler = battler.Partner;
			//DisplayBrief(L(Text.ScriptTexts,"ThrowBall", Player.Name, itemname));
			//ToDo: Undo Comment
			//Game.TextBox.Show(LanguageExtension.Translate(Text.ScriptTexts,"ThrowBall", Player.Name, itemname).Value);
			if (battler.isFainted())
			{
				//"But there was no target..."
				//Display(L(Text.ScriptTexts, "NoTarget"));
				//ToDo: Undo comment
				//Game.TextBox.Show(LanguageExtension.Translate(Text.ScriptTexts, "NoTarget").Value);
				return;
			}
			if (opponent.ID != TrainerTypes.WildPokemon) 
				//&& (!IsSnagBall(ball) || !battler.isShadow))
			{
				//scene.ThrowAndDeflect(ball, 1);
				//"The Trainer blocked the Ball!\nDon't be a thief!"
				//Display(L(Text.ScriptTexts, "SnagRejected"));
				//ToDo: Undo comment
				//Game.TextBox.Show(LanguageExtension.Translate(Text.ScriptTexts, "SnagRejected").Value);
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
					x = (int)Math.Floor(x * 2.5f);
				else if (battler.Status != Status.NONE)
					x = (int)Math.Floor(x * 1.5f);
				int c = 0;
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
			}
		}
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
		#endregion

		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		bool CanShowCommands(int idxPokemon)
		{
			PokemonUnity.Battle.Pokemon thispkmn = this.battlers[idxPokemon];
			if (thispkmn.isFainted()) return false;
			if (thispkmn.effects.TwoTurnAttack > 0) return false; 
			if (thispkmn.effects.HyperBeam > 0) return false; 
			if (thispkmn.effects.Rollout > 0) return false; 
			if (thispkmn.effects.Outrage > 0) return false; 
			if (thispkmn.effects.Uproar > 0) return false;
			if (thispkmn.effects.Bide > 0) return false;
			return true;
		}
		bool CanShowFightMenu(int idxPokemon)
		{
			PokemonUnity.Battle.Pokemon thispkmn = this.battlers[idxPokemon];
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
		bool CanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false)
		{
			Pokemon thispkmn = battlers[idxPokemon];
			Attack.Move thismove = //new Move(this, 
				thispkmn.moves[idxMove];

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
			if ((int)thispkmn.effects.ChoiceBand >= 0 &&
			   (thispkmn.Item == Items.CHOICE_BAND ||
			   thispkmn.Item == Items.CHOICE_SPECS ||
			   thispkmn.Item == Items.CHOICE_SCARF))
			{
				bool hasmove = false;
				for (int i = 0; i < thispkmn.moves.Length; i++) 
				{
					if (thispkmn.moves[i].MoveId==thispkmn.effects.ChoiceBand) 
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
					//if (showMessages) pbDisplayPaused(_INTL("{1} can't use the sealed {2}!", thispkmn.ToString(), thismove.name))
					GameDebug.Log("[CanChoose][#{opp1.ToString()} has: #{opp1.moves[0].name}, #{opp1.moves[1].name}, #{opp1.moves[2].name}, #{opp1.moves[3].name}]");
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
					//if (showMessages) pbDisplayPaused(_INTL("{1} can't use the sealed {2}!", thispkmn.ToString(), thismove.name))
					GameDebug.Log("[CanChoose][#{opp2.ToString()} has: #{opp2.moves[0].name}, #{opp2.moves[1].name}, #{opp2.moves[2].name}, #{opp2.moves[3].name}]");
					return false;
				}
			}
			if (thispkmn.effects.Taunt > 0 && thismove.Power == 0) {//.BaseDamage
				//if (showMessages)pbDisplayPaused(_INTL("{1} can't use {2} after the taunt!", thispkmn.ToString(), thismove.name))
				return false;
			}
			if (thispkmn.effects.Torment){
				if (thismove.MoveId==thispkmn.lastMoveUsed){
					//if (showMessages) pbDisplayPaused(_INTL("{1} can't use the same move twice in a row due to the torment!", thispkmn.ToString()))
					return false;
				}
			}
			if (thismove.MoveId==thispkmn.effects.DisableMove && !sleeptalk){
				//if (showMessages) pbDisplayPaused(_INTL("{1}'s {2} is disabled!", thispkmn.ToString(), thismove.name))
				return false;
			}
			if (thismove.Effect==(Attack.Data.Effects)0x158 && // ToDo: Belch; Confirm value is correct
			   (thispkmn.Species != Pokemons.NONE || !thispkmn.belch)){
				//if (showMessages) pbDisplayPaused(_INTL("{1} hasn't eaten any held berry, so it can't possibly belch!", thispkmn.ToString()))
				return false;
			}
			if (thispkmn.effects.Encore>0 && idxMove!=thispkmn.effects.EncoreIndex){
				return false;
			}
			return true;
		}
		//ToDo: Rename to match above, delete after replacing all calls pointing to this method
		public bool pbCanChooseMove(int pkmn, int index, bool uhh, bool uh)
		{
			return false;
		}
		public bool pbCanRun(int index)
		{
			return false;
		}
		public void pbAnimation(Moves id, Pokemon attacker, Pokemon opponent, int hitnum)
		{
		}
		public Trainer pbGetOwner(int index)
		{
			return opponent;
		}

		//void MoveEffects(int who, int move)
		//{
		//	//this.battlers[who].moves[move].
		//}
	}
}
