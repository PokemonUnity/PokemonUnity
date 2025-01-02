﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.Combat
{
	/// <summary>
	/// </summary>
	public partial class Battle : IBattle, IHasDisplayMessage
	{
		#region Variables
		/// <summary>
		/// Scene object for this battle
		/// </summary>
		public IScene scene { get; protected set; }
		/// <summary>
		/// Decision: 0=undecided; 1=win; 2=loss; 3=escaped; 4=caught; 5=draw
		/// </summary>
		public BattleResults decision { get; set; }
		/// <summary>
		/// Internal battle flag
		/// </summary>
		/// <remarks>
		/// battle occured without any frontend or ui output (most likely on backend for ai vs ai training)
		/// </remarks>
		public bool internalbattle { get; set; }
		/// <summary>
		/// Double battle flag
		/// </summary>
		public bool doublebattle { get; set; }
		/// <summary>
		/// True if player can't escape
		/// </summary>
		public bool cantescape { get; set; }
		/// <summary>
		/// If game cannot progress UNLESS the player is victor of match.
		/// False if there are no consequences to player's defeat.
		/// </summary>
		/// (Ground Hogs day anyone?)
		public bool canLose { get; private set; }
		/// <summary>
		/// Shift/Set "battle style" option
		/// </summary>
		public bool shiftStyle { get; set; }
		/// <summary>
		/// "Battle scene" option
		/// </summary>
		public bool battlescene { get; set; }
		/// <summary>
		/// Debug flag
		/// </summary>
		public bool debug { get; protected set; }
		public int debugupdate { get; protected set; }
		/// <summary>
		/// Player trainer
		/// </summary>
		public ITrainer[] player { get; protected set; }
		/// <summary>
		/// Opponent trainer; if null => wild encounter
		/// </summary>
		public ITrainer[] opponent { get; protected set; }
		/// <summary>
		/// Player's Pokémon party
		/// </summary>
		public IPokemon[] party1 { get; protected set; }
		/// <summary>
		/// Foe's Pokémon party
		/// </summary>
		public IPokemon[] party2 { get; protected set; }
		/// <summary>
		/// Pokémon party for All Trainers in Battle.
		/// Array[4,6] = 0: Player, 1: Foe, 2: Ally, 3: Foe's Ally
		/// </summary>
		//public IPokemon[,] party { get; private set; }
		/// <summary>
		/// Order of Pokémon in the player's party
		/// </summary>
		public IList<int> party1order { get; protected set; }
		/// <summary>
		/// Order of Pokémon in the opponent's party
		/// </summary>
		public IList<int> party2order { get; protected set; }
		/// <summary>
		/// True if player's party's max size is 6 instead of 3
		/// </summary>
		public bool fullparty1 { get; set; }
		/// <summary>
		/// True if opponent's party's max size is 6 instead of 3
		/// </summary>
		public bool fullparty2 { get; set; }
		/// <summary>
		/// Currently active Pokémon
		/// </summary>
		public IBattler[] battlers { get; protected set; }
		/// <summary>
		/// Items held by opponents
		/// </summary>
		//public List<Items> items { get; private set; }
		public Items[][] items { get; set; }
		/// <summary>
		/// Effects common to each side of a battle
		/// </summary>
		/// public List<SideEffects> sides { get; private set; }
		public IEffectsSide[] sides { get; private set; }
		/// <summary>
		/// Effects common to the whole of a battle
		/// </summary>
		/// public List<FieldEffects> field { get; private set; }
		public IEffectsField field { get; private set; }
		/// <summary>
		/// Battle surroundings;
		/// Environment node is used for background visual,
		/// that's displayed behind the floor tile
		/// </summary>
		public Environments environment { get; set; }
		/// <summary>
		/// Current weather, custom methods should use <see cref="pbWeather"/>  instead
		/// </summary>
		public Weather weather { get; set; }
		//public void SetWeather (Weather weather) { this.weather = weather; }
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
		public IBattleMove struggle { get; private set; }
		/// <summary>
		/// Choices made by each Pokémon this round
		/// </summary>
		public IBattleChoice[] choices { get; protected set; }
		/// <summary>
		/// Success states
		/// </summary>
		public ISuccessState[] successStates { get; private set; }
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
		public int[][] megaEvolution { get; private set; }
		/// <summary>
		/// Whether Amulet Coin's effect applies
		/// </summary>
		public bool amuletcoin { get; protected set; }
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
		public string endspeech { get; set; }
		/// <summary>
		/// Speech by opponent when player wins
		/// </summary>
		public string endspeech2 { get; set; }
		/// <summary>
		/// Speech by opponent when opponent wins
		/// </summary>
		public string endspeechwin { get; set; }
		/// <summary>
		/// Speech by opponent when opponent wins
		/// </summary>
		public string endspeechwin2 { get; set; }
		/// <summary>
		/// </summary>
		public IDictionary<string,bool> rules { get; private set; }
		//public List<string> rules { get; private set; }
		/// <summary>
		/// Counter to track number of turns for battle
		/// </summary>
		public int turncount { get; set; }
		public IBattler[] priority { get; protected set; }
		public List<int> snaggedpokemon { get; private set; }
		/// <summary>
		/// Each time you use the option to flee, the counter goes up.
		/// </summary>
		public int runCommand { get; private set; }
		/// <summary>
		/// Another counter that has something to do with tracking items picked up during a battle
		/// </summary>
		public int nextPickupUse { get { pickupUse+=1; return pickupUse; } }
		private int pickupUse { get; set; }
		public bool controlPlayer { get; set; }
		public bool usepriority { get; set; }
		public IBattlePeer peer { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// </summary>
		public Battle(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent)
		{
			(this as IBattle).initialize(scene, p1, p2, player, opponent);
		}
		public Battle(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent)
		{
			(this as IBattle).initialize(scene, p1, p2, player, opponent);
		}
		IBattle IBattle.initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent)
		{
			return initialize(scene, p1, p2, player == null ? null : new ITrainer[] { player }, opponent == null ? null : new ITrainer[] { opponent });
		}
		IBattle IBattle.initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent)
		{
			return initialize(scene, p1, p2, player, opponent);
		}
		public IBattle initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent, int maxBattlers = 4)
		{
			//if opponent (trainer battle) is not null but player array is empty, then player is null
			//if (opponent != null && player.Length == 0)
			//	this.player = null; //player[0];

			//if opponent is not null but opponent array is empty, then opponent is null
			//if (opponent != null && opponent.Length == 0)
			//	this.opponent = null; //opponent[0];

			this.player = player ?? new ITrainer[0];                   //Trainer object
			this.opponent = opponent ?? new ITrainer[0];               //Trainer object

			if (p1.Length == 0) {
				//raise new ArgumentError(Game._INTL("Party 1 has no Pokémon."))
				GameDebug.LogError("Party 1 has no Pokémon.");
				return this;
			}

			if (p2.Length == 0) {
				//raise new ArgumentError(Game._INTL("Party 2 has no Pokémon."))
				GameDebug.LogError("Party 2 has no Pokémon.");
				return this;
			}

			if (p2.Length > 2 && this.opponent.Length == 0) { //ID == TrainerTypes.WildPokemon
				//raise new ArgumentError(Game._INTL("Wild battles with more than two Pokémon are not allowed."))
				GameDebug.LogError("Wild battles with more than two Pokémon are not allowed.");
				return this;
			}

			this.scene = scene;
			decision = 0;
			internalbattle = Core.INTERNAL;
			doublebattle = false;
			cantescape = false;
			shiftStyle = true;
			battlescene = true;
			debug = Core.DEBUG;
			debugupdate = 0;

			party1 = p1;
			party2 = p2;

			party1order = new List<int>();
			//the #12 represents a double battle with 2 trainers using 6 pokemons each on 1 side
			for (int i = 0; i < Core.MAXPARTYSIZE * 2; i++)
				party1order.Add(i);

			party2order = new List<int>();
			//for i in 0...12;party2order.push(i); }
			for (int i = 0; i < Core.MAXPARTYSIZE * 2; i++)
				party2order.Add(i);

			fullparty1 = false;
			fullparty2 = false;
			battlers = new Pokemon[maxBattlers];
			//items = new List<Items>(); //null;
			items = new Items[this.opponent.Length][];
			for (int t = 0; t < this.opponent.Length; t++) //List of Trainers
				items[t] = new Items[0];

			sides = new Effects.Side[] { new Effects.Side(),	// Player's side
										 new Effects.Side() };	// Foe's side
			//sides = new Effects.Side[2];
			field = new Effects.Field();                        // Whole field (gravity/rooms)
			environment = Environments.None;					// e.g. Tall grass, cave, still water
			weather = 0;
			weatherduration = 0;
			switching = false;
			futuresight = false;
			choices = new IBattleChoice[4];

			successStates = new SuccessState[battlers.Length];
			for (int i = 0; i < battlers.Length; i++)
			{
				successStates[i] = new SuccessState();
			}

			lastMoveUsed = Moves.NONE;
			lastMoveUser = -1;
			//nextPickupUse = 0;
			pickupUse = 0;

			megaEvolution = new int[][] { new int[party1.Length], new int[party2.Length] };
			//if (this.player.Length > 0) 									//ToDo: single/double or party?
			//	megaEvolution[0] = new bool?[this.player.Party.Length]; 	//[-1] * player.Length;
			//else
			//	megaEvolution[0] = new bool?[]{ null }; 					//[-1];
			//if (this.opponent.Length > 0)
			//	megaEvolution[1] = new bool?[this.opponent.Party.Length]; 	//[-1] * opponent.Length;
			//else
			//	megaEvolution[1] = new bool?[]{ null }; 					//[-1];
			for (int side = 0; side < sides.Length; side++) //2 sides (yours / theirs)
				for (int i = 0; i < megaEvolution[side].Length; i++)
					megaEvolution[side][i] = -1; //Everyone starts match in default -1 value

			amuletcoin = false;
			extramoney = 0;
			doublemoney = false;

			endspeech = ""; //opponent.ScriptBattleEnd;
			endspeech2 = "";
			endspeechwin = "";
			endspeechwin2 = "";

			rules = new Dictionary<string,bool>();

			turncount = 0;

			peer = PokemonUnity.Monster.PokeBattle_BattlePeer.create();
			//peer = new PokeBattle_BattlePeer();

			priority = new Pokemon[battlers.Length];

			//usepriority = false; //False is already default value; redundant.

			snaggedpokemon = new List<int>();

			runCommand = 0;

			if (Kernal.MoveData.Keys.Contains(Moves.STRUGGLE))
				struggle = Combat.Move.pbFromPBMove(this, new Attack.Move(Moves.STRUGGLE));
			else
				struggle = new PokeBattle_Struggle().Initialize(this, new Attack.Move(Moves.STRUGGLE));

			//struggle.PP = -1;

			for (int i = 0; i < battlers.Length; i++) {
				this.battlers[i] = new Pokemon(this, (sbyte)i);
			//} for (int i = 0; i < battlers.Length; i++) {
			//	this.battlers[i].initialize(this, (sbyte)i);
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
			return this;
		}
		#endregion

		#region Method
		public virtual int pbRandom(int index)
		{
			return Core.Rand.Next(index);
		}

		public virtual void pbAbort() {
			//GameDebug.LogError("Battle aborted");
			throw new BattleAbortedException("Battle aborted");
		}

		#region Catching and storing Pokémon.
		public virtual void pbStorePokemon(IPokemon pokemon)
		{
			if(pokemon is IPokemonShadowPokemon p && !p.isShadow)
				if (pbDisplayConfirm(Game._INTL("Would you like to give a nickname to {1}?", Game._INTL(pokemon.Species.ToString(TextScripts.Name)))))
				{
					//string nick = @scene.pbNameEntry(Game._INTL("{1}'s nickname?", Game._INTL(pokemon.Species.ToString(TextScripts.Name))), pokemon);
					string nick = string.Empty;
					if (@scene is IPokeBattle_Scene s0)
						nick = s0.pbNameEntry(Game._INTL("{1}'s nickname?", Game._INTL(pokemon.Species.ToString(TextScripts.Name))), pokemon);
					//if(!string.IsNullOrEmpty(nick)) pokemon.Name = nick;
					if(!string.IsNullOrEmpty(nick)) (pokemon as Monster.Pokemon).SetNickname(nick);
				}
			//ToDo: Add to party before attempting to store in PC
			//Game.GameData.Player.addPokemon(pokemon)); return;
			//bool success = false;
			//int i = 0; do {
			//	//success = Game.GameData.Player.PC.addPokemon(pokemon);
			//	success = Game.GameData.Player.PC.hasSpace();
			//	if(!success) Game.GameData.Player.PC.getIndexOfFirstEmpty();
			//} while (!success); //Cycle through all boxes unless they're all full
			int oldcurbox = @peer.pbCurrentBox(); //Game.GameData.Player.PC.ActiveBox;
			int storedbox = @peer.pbStorePokemon(pbPlayer(), pokemon);
			//int? storedbox = Game.GameData.Player.PC.getIndexOfFirstEmpty();
			if (storedbox<0) return; //!storedbox.HasValue
			string creator = @peer.pbGetStorageCreator(); //Game.GameData.Player.IsCreator ? Game.GameData.Trainer.name : @peer.pbGetStorageCreator();
			string curboxname = @peer.pbBoxName(oldcurbox); //Game.GameData.Player.PC.BoxNames[oldcurbox];
			string boxname = @peer.pbBoxName(storedbox); //Game.GameData.Player.PC.BoxNames[storedbox.Value];
			if (storedbox != oldcurbox) {
				if (creator != null) //Game.GameData.Player.IsCreator
					pbDisplayPaused(Game._INTL("Box \"{1}\" on {2}'s PC was full.", curboxname, creator));
				else
					pbDisplayPaused(Game._INTL("Box \"{1}\" on someone's PC was full.", curboxname));
				pbDisplayPaused(Game._INTL("{1} was transferred to box \"{2}\".", pokemon.Name, boxname));
			}
			else {
				if (creator != null) //Game.GameData.Player.IsCreator
					pbDisplayPaused(Game._INTL("{1} was transferred to {2}'s PC.", pokemon.Name, creator));
				else
					pbDisplayPaused(Game._INTL("{1} was transferred to someone's PC.", pokemon.Name));
				pbDisplayPaused(Game._INTL("It was stored in box \"{1}\".", boxname));
			}
		}
		public virtual void pbThrowPokeball(int idxPokemon, Items ball, int? rareness = null, bool showplayer = false)
		{
			string itemname = Game._INTL(ball.ToString(TextScripts.Name));
			IBattler battler = null;
			if (pbIsOpposing(idxPokemon))
				battler = battlers[idxPokemon];
			else
				battler = battlers[idxPokemon].pbOppositeOpposing;
			if (battler.isFainted())
				battler = battler.pbPartner;
			pbDisplayBrief(Game._INTL("{1} threw one {2}!", Game.GameData.Trainer.name, itemname));
			if (battler.isFainted())
			{
				pbDisplay(Game._INTL("But there was no target..."));
				return;
			}
			int shakes = 0; bool critical = false;
			if (opponent.Length > 0//.ID != TrainerTypes.WildPokemon)
				&& ((Game.GameData is IItemCheck ch && !ch.pbIsSnagBall(ball)) || battler is IBattlerShadowPokemon s && !s.isShadow()))
			{
				if (@scene is IPokeBattle_Scene s0) s0.pbThrowAndDeflect(ball, 1);
				pbDisplay(Game._INTL("The Trainer blocked the Ball!\nDon't be a thief!"));
			}
			else
			{
				IPokemon pokemon = battler.pokemon;
				Pokemons species = pokemon.Species;
				if (!rareness.HasValue)
				{
					rareness = (int)Kernal.PokemonData[battler.Species].Rarity;
				}
				int a = battler.TotalHP;
				int b = battler.HP;
				//rareness = BallHandlers.ModifyCatchRate(ball, rareness, battler);
				rareness = BallHandlers.ModifyCatchRate(ball, rareness.Value, this, battler);
				int x = (int)Math.Floor(((a * 3 - b * 2) * rareness.Value) / (a * 3f));
				if (battler.Status == Status.SLEEP || battler.Status == Status.FROZEN)
					x = (int)Math.Floor(x * 2.5f);
				else if (battler.Status != Status.NONE)
					x = (int)Math.Floor(x * 1.5f);
				int c = 0;
				if (Game.GameData.Trainer != null)
					if (Game.GameData.Trainer.pokedexOwned() > 600) //Game.GameData.Player.PokedexCaught
						c = (int)Math.Floor(x * 2.5f / 6);
					else if (Game.GameData.Trainer.pokedexOwned() > 450)
						c = (int)Math.Floor(x * 2f / 6);
					else if (Game.GameData.Trainer.pokedexOwned() > 300)
						c = (int)Math.Floor(x * 1.5f / 6);
					else if (Game.GameData.Trainer.pokedexOwned() > 150)
						c = (int)Math.Floor(x * 1f / 6);
					else if (Game.GameData.Trainer.pokedexOwned() > 30)
						c = (int)Math.Floor(x * .5 / 6);
				//int shakes = 0; bool critical = false;
				if (x > 255 || BallHandlers.IsUnconditional(ball,this,battler))
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
				GameDebug.Log($"[Threw Poké Ball] #{itemname}, #{shakes} shakes (4=capture)");
				if (@scene is IPokeBattle_Scene s0) s0.pbThrow(ball, (critical) ? 1 : shakes, critical, battler.Index, showplayer);
				switch (shakes)
				{
					case 0:
						pbDisplay(Game._INTL("Oh no! The Pokémon broke free!"));
						BallHandlers.OnFailCatch(ball,this,battler);
						break;
					case 1:
						pbDisplay(Game._INTL("Aww... It appeared to be caught!"));
						BallHandlers.OnFailCatch(ball,this,battler);
						break;
					case 2:
						pbDisplay(Game._INTL("Aargh! Almost had it!"));
						BallHandlers.OnFailCatch(ball,this,battler);
						break;
					case 3:
						pbDisplay(Game._INTL("Gah! It was so close, too!"));
						BallHandlers.OnFailCatch(ball,this,battler);
						break;
					case 4:
						pbDisplayBrief(Game._INTL("Gotcha! {1} was caught!", pokemon.Name));
						if (@scene is IPokeBattle_Scene s1) s1.pbThrowSuccess();
						if (Game.GameData is IItemCheck c0 && c0.pbIsSnagBall(ball) && @opponent.Length > 0)
						{
							pbRemoveFromParty(battler.Index, battler.pokemonIndex);
							battler.pbReset();
							battler.participants = new List<int>();
						}
						else
							@decision = BattleResults.CAPTURED;
						//Script is repeated below as `SetCatchInfo`
						if (Game.GameData is IItemCheck c1 && c1.pbIsSnagBall(ball))
						{
							pokemon.ot = this.player[0].name;
							pokemon.trainerID = this.player[0].id;
							//pokemon.OT = this.player[0].Trainer;
						}
						BallHandlers.OnCatch(ball,this,pokemon);
						pokemon.ballUsed = ball; //pbGetBallType(ball);
						if (pokemon is IPokemonMegaEvolution m0) m0.makeUnmega();   //rescue null
						if (pokemon is IPokemonMegaEvolution m1) m1.makeUnprimal(); //rescue null
						pokemon.pbRecordFirstMoves();
						BallHandlers.OnCatch(ball,this,pokemon);
						//pokemon = new Monster.Pokemon(pokemon, ball, pbIsSnagBall(ball) ? Monster.Pokemon.ObtainedMethod.SNAGGED : Monster.Pokemon.ObtainedMethod.MET);
						//pokemon.SetCatchInfos(this.pbPlayer(), ball, Item.pbIsSnagBall(ball) ? Monster.Pokemon.ObtainedMethod.SNAGGED : Monster.Pokemon.ObtainedMethod.MET);
						if (Core.GAINEXPFORCAPTURE)
						{
							battler.captured = true;
							pbGainEXP();
							battler.captured = false;
						}
						if (Game.GameData.Trainer.pokedex) //Trainer has a Pokedex
						{
							if (!this.player[0].hasOwned(species))
							//if (Game.GameData.Player.Pokedex[(int)pokemon.Species, 1] == 0)
							{
								this.player[0].setOwned(species);
								//Game.GameData.Player.Pokedex[(int)pokemon.Species, 1] = 1;
								pbDisplayPaused(Game._INTL("{1}'s data was added to the Pokédex.", pokemon.Name));
								if (@scene is IPokeBattle_Scene s2) s2.pbShowPokedex(pokemon.Species);
							}
						}
						if (@scene is IPokeBattle_Scene s3) s3.pbHideCaptureBall();
						if (Game.GameData is IItemCheck c2 && c2.pbIsSnagBall(ball) && @opponent.Length > 0)
						{
							if (pokemon is IPokemonShadowPokemon sp) sp.pbUpdateShadowMoves(); //rescue null
							@snaggedpokemon.Add((byte)battler.Index); //pokemon
						}
						else
							pbStorePokemon(pokemon);
						break;
				}
			}
		}
		#endregion

		#region Info about battle.
		public virtual bool pbDoubleBattleAllowed()
		{
			if (!@fullparty1 && @party1.Length>Core.MAXPARTYSIZE) {
				return false;
			}
			if (!@fullparty2 && @party2.Length>Core.MAXPARTYSIZE) {
				return false;
			}
			ITrainer[] _opponent=@opponent;
			ITrainer[] _player=@player;
			// Wild battle
			if (_opponent?.Length == 0) {
				if (@party2.Length==1)
					return false;
				else if (@party2.Length==2)
					return true;
				else
					return false;
			}
			// Trainer battle
			else {
				if (_opponent?.Length > 0) {
					if (_opponent.Length==1) {
						//_opponent=_opponent[0];
					}
					else if (_opponent.Length!=2) { //less than 2?
						return false;
					}
				}
				//_player=_player;
				if (_player.Length > 0) {
					if (_player.Length==1) {
						//_player=_player[0];
					} else if (_player.Length!=2) {
						return false;
					}
				}
				if (_opponent.Length > 0) {
					int sendout1=pbFindNextUnfainted(@party2,0,pbSecondPartyBegin(1));
					int sendout2=pbFindNextUnfainted(@party2,pbSecondPartyBegin(1));
					if (sendout1<0 || sendout2<0) return false;
				}
				else {
					int sendout1=pbFindNextUnfainted(@party2,0);
					int sendout2=pbFindNextUnfainted(@party2,sendout1+1);
					if (sendout1<0 || sendout2<0) return false;
				}
			}
			if (_player.Length > 0) {
				int sendout1=pbFindNextUnfainted(@party1,0,pbSecondPartyBegin(0));
				int sendout2=pbFindNextUnfainted(@party1,pbSecondPartyBegin(0));
				if (sendout1<0 || sendout2<0) return false;
			}
			else {
				int sendout1=pbFindNextUnfainted(@party1,0);
				int sendout2=pbFindNextUnfainted(@party1,sendout1+1);
				if (sendout1<0 || sendout2<0) return false;
			}
			return true;
		}

		public Weather pbWeather { get //()
			{
				for (int i = 0; i < battlers.Length; i++) {
					if (@battlers[i].hasWorkingAbility(Abilities.CLOUD_NINE) ||
						@battlers[i].hasWorkingAbility(Abilities.AIR_LOCK)) {
						return Weather.NONE;
					}
				}
				return @weather;
			} 
		}
		#endregion

		#region Get Battler Info.
		public bool pbIsOpposing(int index)
		{
			return (index % 2) == 1;
		}
		public bool pbOwnedByPlayer(int index)
		{
			if (pbIsOpposing(index)) return false;
			if (@player.Length > 0 && index==2) return false;
			return true;
		}

		public bool pbIsDoubleBattler(int index) {
			return (index>=2);
		}
		/// <summary>
		/// Only used for Wish
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pokemonindex"></param>
		/// <returns></returns>
		public string ToString(int battlerindex, int pokemonindex) {
			IPokemon[] party=pbParty(battlerindex);
			if (pbIsOpposing(battlerindex)) {
				if (@opponent != null) {
					return Game._INTL("The foe {1}", party[pokemonindex].Name);
					//return Game._INTL("The foe {1}", party[battlerindex,pokemonindex].Name);
				}
				else {
					return Game._INTL("The wild {1}", party[pokemonindex].Name);
					//return Game._INTL("The wild {1}", party[battlerindex,pokemonindex].Name);
				}
			}
			else {
				return Game._INTL("{1}", party[pokemonindex].Name);
				//return Game._INTL("{1}", party[battlerindex,pokemonindex].Name);
			}
		}

		/// <summary>
		/// Checks whether an item can be removed from a Pokémon.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool pbIsUnlosableItem(IBattler pkmn, Items item) {
			if (Kernal.ItemData[item].IsLetter) return true; //pbIsMail(item)
			if (pkmn.effects.Transform) return false;
			if (pkmn.Ability == Abilities.MULTITYPE) {
				Items[] plates= new Items[] { Items.FIST_PLATE, Items.SKY_PLATE, Items.TOXIC_PLATE, Items.EARTH_PLATE, Items.STONE_PLATE,
						Items.INSECT_PLATE, Items.SPOOKY_PLATE, Items.IRON_PLATE, Items.FLAME_PLATE, Items.SPLASH_PLATE,
						Items.MEADOW_PLATE, Items.ZAP_PLATE, Items.MIND_PLATE, Items.ICICLE_PLATE, Items.DRACO_PLATE,
						Items.DREAD_PLATE, Items.PIXIE_PLATE };
				foreach (var i in plates) {
					if (item == i) return true;
				}
			}
			KeyValuePair<Pokemons, Items>[] combos= new KeyValuePair<Pokemons, Items>[] {
				new KeyValuePair<Pokemons, Items> (Pokemons.GIRATINA,		Items.GRISEOUS_ORB),
				new KeyValuePair<Pokemons, Items> (Pokemons.GENESECT,		Items.BURN_DRIVE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GENESECT,		Items.CHILL_DRIVE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GENESECT,		Items.DOUSE_DRIVE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GENESECT,		Items.SHOCK_DRIVE),
				// Mega Stones
				new KeyValuePair<Pokemons, Items> (Pokemons.ABOMASNOW,		Items.ABOMASITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.ABSOL,		    Items.ABSOLITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.AERODACTYL,		Items.AERODACTYLITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.AGGRON,		    Items.AGGRONITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.ALAKAZAM,		Items.ALAKAZITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.ALTARIA,		Items.ALTARIANITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.AMPHAROS,		Items.AMPHAROSITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.AUDINO,		    Items.AUDINITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.BANETTE,		Items.BANETTITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.BEEDRILL,		Items.BEEDRILLITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.BLASTOISE,		Items.BLASTOISINITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.BLAZIKEN,		Items.BLAZIKENITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.CAMERUPT,		Items.CAMERUPTITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.CHARIZARD,		Items.CHARIZARDITE_X),
				new KeyValuePair<Pokemons, Items> (Pokemons.CHARIZARD,		Items.CHARIZARDITE_Y),
				new KeyValuePair<Pokemons, Items> (Pokemons.DIANCIE,		Items.DIANCITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GALLADE,		Items.GALLADITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GARCHOMP,		Items.GARCHOMPITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GARDEVOIR,		Items.GARDEVOIRITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GENGAR,		    Items.GENGARITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GLALIE,		    Items.GLALITITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.GYARADOS,		Items.GYARADOSITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.HERACROSS,		Items.HERACRONITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.HOUNDOOM,		Items.HOUNDOOMINITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.KANGASKHAN,		Items.KANGASKHANITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.LATIAS,		    Items.LATIASITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.LATIOS,		    Items.LATIOSITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.LOPUNNY,		Items.LOPUNNITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.LUCARIO,		Items.LUCARIONITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.MANECTRIC,		Items.MANECTITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.MAWILE,		    Items.MAWILITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.MEDICHAM,		Items.MEDICHAMITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.METAGROSS,	   	Items.METAGROSSITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.MEWTWO,		    Items.MEWTWONITE_X),
				new KeyValuePair<Pokemons, Items> (Pokemons.MEWTWO,		    Items.MEWTWONITE_Y),
				new KeyValuePair<Pokemons, Items> (Pokemons.PIDGEOT,		Items.PIDGEOTITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.PINSIR,		    Items.PINSIRITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SABLEYE,		Items.SABLENITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SALAMENCE,		Items.SALAMENCITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SCEPTILE,		Items.SCEPTILITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SCIZOR,		    Items.SCIZORITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SHARPEDO,		Items.SHARPEDONITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SLOWBRO,		Items.SLOWBRONITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.STEELIX,		Items.STEELIXITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.SWAMPERT,		Items.SWAMPERTITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.TYRANITAR,		Items.TYRANITARITE),
				new KeyValuePair<Pokemons, Items> (Pokemons.VENUSAUR,		Items.VENUSAURITE),
				// Primal Reversion stones
				new KeyValuePair<Pokemons, Items> (Pokemons.KYOGRE,		    Items.BLUE_ORB),
				new KeyValuePair<Pokemons, Items> (Pokemons.GROUDON,		Items.RED_ORB)
			};
			foreach (KeyValuePair<Pokemons, Items> i in combos) {
				if (pkmn.Species == (Pokemons)i.Key && item == (Items)i.Value) {
					return true;
				}
			}
			return false;
		}

		public IBattler pbCheckGlobalAbility(Abilities a) {
			for (int i = 0; i < battlers.Length; i++) { // in order from own first, opposing first, own second, opposing second
				if (@battlers[i].hasWorkingAbility(a)) {
					return @battlers[i];
				}
			}
			return null;
		}
		#endregion

		#region Player-related Info.
		public ITrainer pbPlayer() {
			if (@player?.Length > 0) {
				return @player[0];
			}
			else {
				return new Trainer(battlers[0].Name, TrainerTypes.WildPokemon); //null;
			}
		}

		public Items[] pbGetOwnerItems(int battlerIndex) {
			if (@items == null) return new Items[0];
			if (pbIsOpposing(battlerIndex)) {
				if (@opponent.Length > 0) {
					return (battlerIndex==1) ? @items[0] : @items[1];
				}
				else {
					return new Items[0]; //Wild pokemons dont get a bag, only held items...
				}
			}
			else {
				return new Items[0]; //Maybe sort option out for ally to access [their own] bag inventory too?
			}
		}

		public void pbSetSeen(IPokemon pokemon) {
			if (Game.GameData.Trainer.pokedex &&
				(pokemon.IsNotNullOrNone() && @internalbattle)) { //Trainer has a Pokedex
				this.pbPlayer().seen[pokemon.Species]=true;
				if (Game.GameData is IGameUtility g) g.pbSeenForm(pokemon);
				//Game.GameData.Player.Pokedex[(int)pokemon.Species,0] = 1;
				//Game.GameData.Player.Pokedex[(int)pokemon.Species,2] = (byte)pokemon.form;
			}
		}

		public string pbGetMegaRingName(int battlerIndex) {
			if (pbBelongsToPlayer(battlerIndex)) {
				foreach (Items i in Core.MEGARINGS) {
					//if (!hasConst(PBItems,i)) continue;
					if (Game.GameData.Bag.pbQuantity(i)>0) return Game._INTL(i.ToString(TextScripts.Name));
					//if (Game.GameData.Player.Bag.GetItemAmount(i)>0) return Game._INTL(i.ToString(TextScripts.Name));
				}
			}
			// Add your own Mega objects for particular trainer types here
			if (pbGetOwner(battlerIndex).trainertype == TrainerTypes.BUGCATCHER) {
				return Game._INTL("Mega Net");
			}
			return Game._INTL("Mega Ring");
		}

		public bool pbHasMegaRing(int battlerIndex) {
			if (!pbBelongsToPlayer(battlerIndex)) return true;
			foreach (Items i in Core.MEGARINGS) {
				//if (!hasConst(PBItems,i)) continue;
				if (Game.GameData.Bag.pbQuantity(i)>0) return true;
				//if (Game.GameData.Player.Bag.GetItemAmount(i)>0) return true;
			}
			return false;
		}
		#endregion

		#region Get party info, manipulate parties.
		public int PokemonCount(IPokemon[] party)
		{
			int count = 0;
			for (int i = 0; i < party.Length; i++)
			{
				if (party[i].Species == Pokemons.NONE) continue;
				if (party[i].HP > 0 && !party[i].isEgg) count += 1;
			}
			return count;
		}
		public bool AllFainted(IPokemon[] party)
		{
			return PokemonCount(party) == 0;
		}
		public int MaxLevel(IPokemon[] party)
		{
			int lv = 0;
			for (int i = 0; i < party.Length; i++)
			{
				if (party[i].Species == Pokemons.NONE) continue;
				if (lv < party[i].Level) lv = party[i].Level;
			}
			return lv;
		}
		public int pbPokemonCount(IPokemon[] party) {
			int count=0;
			foreach (IPokemon i in party) {
				if (!i.IsNotNullOrNone()) continue;
				if (i.HP>0 && !i.isEgg) count+=1;
			}
			return count;
		}

		public bool pbAllFainted (IPokemon[] party) {
			return pbPokemonCount(party)==0;
		}

		public int pbMaxLevel(IPokemon[] party) {
			int lv=0;
			foreach (var i in party) {
				if (!i.IsNotNullOrNone()) continue;
				if (lv<i.Level) lv=i.Level;
			}
			return lv;
		}

		public int pbMaxLevelFromIndex(int index) {
			IPokemon[] party=pbParty(index);
			ITrainer[] owner=pbIsOpposing(index) ? @opponent : @player;
			int maxlevel=0;
			if (owner.Length > 0) {
				int start=0;
				int limit=pbSecondPartyBegin(index);
				if (pbIsDoubleBattler(index)) start=limit;
				for (int i = start; i < start+limit; i++) {
					if (!party[i].IsNotNullOrNone()) continue;
					if (maxlevel<party[i].Level) maxlevel=party[i].Level;
				}
			}
			else {
				foreach (var i in party) {
					if (!i.IsNotNullOrNone()) continue;
					if (maxlevel<i.Level) maxlevel=i.Level;
				}
			}
			return maxlevel;
		}

		/// <summary>
		/// Returns the trainer party of pokemon at this index?
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public IPokemon[] pbParty(int index) {
			return pbIsOpposing(index) ? party2 : party1;
			//return battlers.Where(b => (b.Index % 2) == (index % 2)).ToArray();
		}

		public IPokemon[] pbOpposingParty(int index) {
			return pbIsOpposing(index) ? party1 : party2;
		}

		public int pbSecondPartyBegin(int battlerIndex) {
			if (pbIsOpposing(battlerIndex)) {
				//return @fullparty2 ? 6 : 3; //split in half for doubles
				return @fullparty2 ? (Game.GameData as Game).Features.LimitPokemonPartySize : (int)(Core.MAXPARTYSIZE * .5);
			}
			else {
				//return @fullparty1 ? 6 : 3; //split in half for doubles
				return @fullparty1 ? (Game.GameData as Game).Features.LimitPokemonPartySize : (int)(Core.MAXPARTYSIZE * .5);
			}
		}

		public int pbPartyLength(int battlerIndex) {
			if (pbIsOpposing(battlerIndex)) {
				return (@opponent.Length > 0) ? pbSecondPartyBegin(battlerIndex) : (Game.GameData as Game).Features.LimitPokemonPartySize;
			}
			else {
				return @player.Length > 0 ? pbSecondPartyBegin(battlerIndex) : (Game.GameData as Game).Features.LimitPokemonPartySize;
			}
		}

		public int pbFindNextUnfainted(IPokemon[] party,int start,int finish=-1) {
			if (finish<0) finish=party.Length;
			for (int i = start; i < finish; i++) {
				if (!party[i].IsNotNullOrNone()) continue;
				if (party[i].HP>0 && !party[i].isEgg) return i;
			}
			return -1;
		}

		public int pbGetLastPokeInTeam(int index) {
			IPokemon[] party=pbParty(index);
			int[] partyorder=(!pbIsOpposing(index) ? @party1order : @party2order).ToArray();
			int plength=pbPartyLength(index);
			int pstart=pbGetOwnerIndex(index)*plength;
			int lastpoke=-1;
			for (int i = pstart; i < pstart+plength - 1; i++) {
				IPokemon p=party[partyorder[i]];
				if (!p.IsNotNullOrNone() || p.isEgg || p.HP<=0) continue;
				lastpoke=partyorder[i];
			}
			return lastpoke;
		}

		public IBattler pbFindPlayerBattler(int pkmnIndex) {
			IBattler battler=null;
			for (int k = 0; k < battlers.Length; k++) {
				if (!pbIsOpposing(k) && @battlers[k].pokemonIndex==pkmnIndex) {
					battler=@battlers[k];
					break;
				}
			}
			return battler;
		}

		public bool pbIsOwner (int battlerIndex, int partyIndex) {
			int secondParty=pbSecondPartyBegin(battlerIndex);
			if (!pbIsOpposing(battlerIndex)) {
				if (@player ==  null || @player.Length == 0) return true;
				return (battlerIndex==0) ? partyIndex<secondParty : partyIndex>=secondParty;
			}
			else {
				if (@opponent == null || @opponent.Length == 0) return true;
				return (battlerIndex==1) ? partyIndex<secondParty : partyIndex>=secondParty;
			}
		}

		public ITrainer pbGetOwner(int battlerIndex) {
			if (pbIsOpposing(battlerIndex)) {
				if (@opponent.Length > 0) {
					return (battlerIndex==1) ? @opponent[0] : @opponent[1];
				}
				else {
					//return null; //@opponent;
					return new Trainer(null,TrainerTypes.WildPokemon);
				}
			}
			else {
				if (@player.Length > 0) {
					return (battlerIndex==0) ? @player[0] : @player[1];
				}
				else {
					//return null; //@player;
					return new Trainer(null,TrainerTypes.WildPokemon);
				}
			}
		}

		public ITrainer pbGetOwnerPartner(int battlerIndex) {
			if (pbIsOpposing(battlerIndex)) {
				if (@opponent.Length > 0) {
					return (battlerIndex==1) ? @opponent[1] : @opponent[0];
				}
				else {
					//return @opponent[0];
					return new Trainer(null,TrainerTypes.WildPokemon);
				}
			}
			else {
				if (@player.Length > 0) {
					return (battlerIndex==0) ? @player[1] : @player[0];
				}
				else {
					//return @player[0];
					return new Trainer(null,TrainerTypes.WildPokemon);
				}
			}
		}

		public int pbGetOwnerIndex(int battlerIndex) {
			if (pbIsOpposing(battlerIndex)) {
				return (@opponent.Length > 0) ? ((battlerIndex==1) ? 0 : 1) : 0;
			}
			else {
				return (@player.Length > 0) ? ((battlerIndex==0) ? 0 : 1) : 0;
			}
		}

		public bool pbBelongsToPlayer (int battlerIndex) {
			if (@player.Length > 0 && @player.Length>1) {
				return battlerIndex==0;
			}
			else {
				return (battlerIndex%2)==0;
			}
			//return false;
		}

		public ITrainer pbPartyGetOwner(int battlerIndex, int partyIndex) {
			int secondParty=pbSecondPartyBegin(battlerIndex);
			if (!pbIsOpposing(battlerIndex)) {
				if (@player == null || @player.Length == 0) return new Trainer(null,TrainerTypes.WildPokemon);//wild pokemon instead of @player?
				return (partyIndex<secondParty) ? @player[0] : @player[1];
			}
			else {
				if (@opponent == null || @opponent.Length == 0) return new Trainer(null,TrainerTypes.WildPokemon);//wild pokemon instead of @opponent?
				return (partyIndex<secondParty) ? @opponent[0] : @opponent[1];
			}
		}

		public void pbAddToPlayerParty(IPokemon pokemon) {
			IPokemon[] party=pbParty(0);
			for (int i = 0; i < party.Length; i++) {
				if (pbIsOwner(0,i) && !party[i].IsNotNullOrNone()) party[i]=pokemon;
			}
		}

		public void pbRemoveFromParty(int battlerIndex, int partyIndex) {
			IPokemon[] party=pbParty(battlerIndex);
			ITrainer[] side=pbIsOpposing(battlerIndex) ? @opponent : @player;
			int[] order=(pbIsOpposing(battlerIndex) ? @party2order : @party1order).ToArray();
			int secondpartybegin=pbSecondPartyBegin(battlerIndex);
			party[partyIndex]=null;
			if (side == null || side.Length == 1) { // Wild or single opponent
				party.PackParty();//compact!
				for (int i = partyIndex; i < party.Length+1; i++) {
					for (int j = 0; j < @battlers.Length; j++) {
						if (!@battlers[j].IsNotNullOrNone()) continue;
						if (pbGetOwner(j)==side[0] && @battlers[j].pokemonIndex==i) {
							@battlers[j].pokemonIndex-=1; //Remove pokemon from party and adjust ref position for new party line-up
							break;
						}
					}
				}
				for (int i = 0; i < order.Length; i++) {
					order[i]=(i==partyIndex) ? order.Length-1 : order[i]-1;
				}
			}
			else {
				if (partyIndex<secondpartybegin-1) {
					for (int i = partyIndex; i < secondpartybegin; i++) {
						if (i>=secondpartybegin-1) {
							party[i]=null;
						}
						else {
							party[i]=party[i+1];
						}
					}
					for (int i = 0; i < order.Length; i++) {
						if (order[i]>=secondpartybegin) continue;
						order[i]=(i==partyIndex) ? secondpartybegin-1 : order[i]-1;
					}
				}
				else {
					for (int i = partyIndex; i < secondpartybegin+pbPartyLength(battlerIndex); i++) {
						if (i>=party.Length-1) {
							party[i]=null;
						}
						else {
							party[i]=party[i+1];
						}
					}
					for (int i = 0; i < order.Length; i++) {
						if (order[i]<secondpartybegin) continue;
						order[i]=(i==partyIndex) ? secondpartybegin+pbPartyLength(battlerIndex)-1 : order[i]-1;
					}
				}
			}
		}
		#endregion

		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		public bool CanShowCommands(int idxPokemon)
		{
			IBattler thispkmn = @battlers[idxPokemon];
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
			IBattler thispkmn = @battlers[idxPokemon];
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
			IBattler thispkmn = @battlers[idxPokemon];
			IBattleMove thismove = thispkmn.moves[idxMove];

			//ToDo: Array for opposing pokemons, [i] changes based on if double battle
			IBattler opp1 = thispkmn.pbOpposing1;
			IBattler opp2 = null; //ToDo: thispkmn.pbOpposing2;
			if (thismove == null || thismove.id == 0) return false;
			if (thismove.PP <= 0 && thismove.TotalPP > 0 && !sleeptalk) {
				if (showMessages) pbDisplayPaused(Game._INTL("There's no PP left for this move!"));
				return false;
			}
			if (thispkmn.hasWorkingItem(Items.ASSAULT_VEST)) {// && thismove.IsStatus?
				if (showMessages) pbDisplayPaused(Game._INTL("The effects of the {1} prevent status moves from being used!", Game._INTL(thispkmn.Item.ToString(TextScripts.Name))));
				return false;
			}
			if ((int)thispkmn.effects.ChoiceBand >= 0 &&
			   (thispkmn.hasWorkingItem(Items.CHOICE_BAND) ||
			   thispkmn.hasWorkingItem(Items.CHOICE_SPECS) ||
			   thispkmn.hasWorkingItem(Items.CHOICE_SCARF)))
			{
				bool hasmove = false;
				for (int i = 0; i < thispkmn.moves.Length; i++)
					if (thispkmn.moves[i].id==thispkmn.effects.ChoiceBand) {
						hasmove = true; break;
					}
				if (hasmove && thismove.id != thispkmn.effects.ChoiceBand) {
					if (showMessages)
						pbDisplayPaused(Game._INTL("{1} allows the use of only {2}!",
							Game._INTL(thispkmn.Item.ToString(TextScripts.Name)),
							Game._INTL(thispkmn.effects.ChoiceBand.ToString(TextScripts.Name))));
					return false;
				}
			}
			if (opp1.IsNotNullOrNone() && opp1.effects.Imprison)
			{
				if (thismove.id == opp1.moves[0].id ||
					thismove.id == opp1.moves[1].id ||
					thismove.id == opp1.moves[2].id ||
					thismove.id == opp1.moves[3].id)
				{
					if (showMessages) pbDisplayPaused(Game._INTL("{1} can't use the sealed {2}!", thispkmn.ToString(), Game._INTL(thismove.id.ToString(TextScripts.Name))));
					GameDebug.Log($"[CanChoose][#{opp1.ToString()} has: #{Game._INTL(opp1.moves[0].id.ToString(TextScripts.Name))}, #{Game._INTL(opp1.moves[1].id.ToString(TextScripts.Name))}, #{Game._INTL(opp1.moves[2].id.ToString(TextScripts.Name))}, #{Game._INTL(opp1.moves[3].id.ToString(TextScripts.Name))}]");
					return false;
				}
			}
			if (opp2.IsNotNullOrNone() && opp2.effects.Imprison)
			{
				if (thismove.id == opp2.moves[0].id ||
					 thismove.id == opp2.moves[1].id ||
					 thismove.id == opp2.moves[2].id ||
					 thismove.id == opp2.moves[3].id)
				{
					if (showMessages) pbDisplayPaused(Game._INTL("{1} can't use the sealed {2}!", thispkmn.ToString(), Game._INTL(thismove.id.ToString(TextScripts.Name))));
					GameDebug.Log($"[CanChoose][#{opp2.ToString()} has: #{Game._INTL(opp2.moves[0].id.ToString(TextScripts.Name))}, #{Game._INTL(opp2.moves[1].id.ToString(TextScripts.Name))}, #{Game._INTL(opp2.moves[2].id.ToString(TextScripts.Name))}, #{Game._INTL(opp2.moves[3].id.ToString(TextScripts.Name))}]");
					return false;
				}
			}
			if (thispkmn.effects.Taunt > 0 && thismove.basedamage == 0) {
				if (showMessages) pbDisplayPaused(Game._INTL("{1} can't use {2} after the taunt!", thispkmn.ToString(), Game._INTL(thismove.id.ToString(TextScripts.Name))));
				return false;
			}
			if (thispkmn.effects.Torment) {
				if (thismove.id==thispkmn.lastMoveUsed) {
					if (showMessages) pbDisplayPaused(Game._INTL("{1} can't use the same move twice in a row due to the torment!", thispkmn.ToString()));
					return false;
				}
			}
			if (thismove.id==thispkmn.effects.DisableMove && !sleeptalk) {
				if (showMessages) pbDisplayPaused(Game._INTL("{1}'s {2} is disabled!", thispkmn.ToString(), Game._INTL(thismove.id.ToString(TextScripts.Name))));
				return false;
			}
			if (thismove.Effect==Attack.Data.Effects.x153 && // ToDo: Belch
			   (thispkmn.Species != Pokemons.NONE || !thispkmn.pokemon.belch)) {
				if (showMessages) pbDisplayPaused(Game._INTL("{1} hasn't eaten any held berry, so it can't possibly belch!", thispkmn.ToString()));
				return false;
			}
			if (thispkmn.effects.Encore>0 && idxMove!=thispkmn.effects.EncoreIndex) {
				return false;
			}
			return true;
		}

		public virtual void pbAutoChooseMove(int idxPokemon, bool showMessages=true) {
			IBattler thispkmn=@battlers[idxPokemon];
			if (thispkmn.isFainted()) {
				//@choices[idxPokemon][0]=0;
				//@choices[idxPokemon][1]=0;
				//@choices[idxPokemon][2]=null;
				@choices[idxPokemon]=new Choice(ChoiceAction.NoAction);
				return;
			}
			if (thispkmn.effects.Encore>0 &&
				CanChooseMove(idxPokemon,thispkmn.effects.EncoreIndex,false)) {
				GameDebug.Log($"[Auto choosing Encore move] #{Game._INTL(thispkmn.moves[thispkmn.effects.EncoreIndex].id.ToString(TextScripts.Name))}");
				//@choices[idxPokemon][0]=1;    // "Use move"
				//@choices[idxPokemon][1]=thispkmn.effects.EncoreIndex; // Index of move
				//@choices[idxPokemon][2]=thispkmn.moves[thispkmn.effects.EncoreIndex];
				//@choices[idxPokemon][3]=-1;   // No target chosen yet
				@choices[idxPokemon]=new Choice(ChoiceAction.UseMove, thispkmn.effects.EncoreIndex, thispkmn.moves[thispkmn.effects.EncoreIndex]);
				if (@doublebattle) {
					IBattleMove thismove=thispkmn.moves[thispkmn.effects.EncoreIndex];
					Attack.Data.Targets targets=thispkmn.pbTarget(thismove);
					if (targets==Attack.Data.Targets.SELECTED_POKEMON || targets==Attack.Data.Targets.SELECTED_POKEMON_ME_FIRST) { //Targets.SingleNonUser
						int target=(@scene as IPokeBattle_SceneNonInteractive).pbChooseTarget(idxPokemon,targets);
						if (target>=0) pbRegisterTarget(idxPokemon,target);
					}
					else if (targets==Attack.Data.Targets.USER_OR_ALLY) { //Targets.UserOrPartner
						int target=(@scene as IPokeBattle_SceneNonInteractive).pbChooseTarget(idxPokemon,targets);
						if (target>=0 && (target&1)==(idxPokemon&1)) pbRegisterTarget(idxPokemon,target); //both integers are Even (ally) and Identical (selected)
					}
				}
			}
			else {
				if (!pbIsOpposing(idxPokemon)) {
					if (showMessages) pbDisplayPaused(Game._INTL("{1} has no moves left!",thispkmn.Name));
				}
				//@choices[idxPokemon][0]=1;           // "Use move"
				//@choices[idxPokemon][1]=-1;          // Index of move to be used
				//@choices[idxPokemon][2]=@struggle;   // Use Struggle
				//@choices[idxPokemon][3]=-1;          // No target chosen yet
				@choices[idxPokemon]=new Choice(ChoiceAction.UseMove, -1, @struggle);
			}
		}

		public virtual bool pbRegisterMove(int idxPokemon, int idxMove, bool showMessages=true) {
			IBattler thispkmn=@battlers[idxPokemon];
			IBattleMove thismove=thispkmn.moves[idxMove];
			if (!CanChooseMove(idxPokemon,idxMove,showMessages)) return false;
			//@choices[idxPokemon][0]=1;         // "Use move"
			//@choices[idxPokemon][1]=idxMove;   // Index of move to be used
			//@choices[idxPokemon][2]=thismove;  // PokeBattle_Move object of the move
			//@choices[idxPokemon][3]=-1;        // No target chosen yet
			@choices[idxPokemon]=new Choice(ChoiceAction.UseMove, idxMove, thismove);
			return true;
		}

		public bool pbChoseMove (int i, Moves move) {
			if (@battlers[i].isFainted()) return false;
			//if (@choices[i][0]==1 && @choices[i][1]>=0) {
			if (@choices[i].Action==ChoiceAction.UseMove && @choices[i].Index>=0) {
				int choice=@choices[i].Index; //@choices[i][1];
				return @battlers[i].moves[choice].id == move;
			}
			return false;
		}

		public bool pbChoseMoveFunctionCode (int i,Attack.Data.Effects code) {
			if (@battlers[i].isFainted()) return false;
			//if (@choices[i][0]==1 && @choices[i][1]>=0) {
			if (@choices[i].Action==ChoiceAction.UseMove && @choices[i].Index>=0) {
				int choice=@choices[i].Index; //@choices[i][1];
				return @battlers[i].moves[choice].Effect==code;
			}
			return false;
		}

		public virtual bool pbRegisterTarget(int idxPokemon, int idxTarget) {
			//@choices[idxPokemon][3]=idxTarget;   // Set target of move
			@choices[idxPokemon]=new Choice(ChoiceAction.UseMove, @choices[idxPokemon].Index, @choices[idxPokemon].Move, idxTarget);   // Set target of move
			return true;
		}

		public IBattler[] pbPriority(bool ignorequickclaw=false, bool log=false) {
			if (@usepriority) return @priority;	// use stored priority if round isn't over yet
			@priority = new Pokemon[battlers.Length]; //.Clear();
			int[] speeds=new int[battlers.Length];
			int[] priorities=new int[battlers.Length];
			bool[] quickclaw=new bool[battlers.Length]; bool[] lagging=new bool[battlers.Length];
			int minpri=0; int maxpri=0;
			List<int> temp=new List<int>();
			#region Calculate each Pokémon's speed
			for (int i = 0; i < battlers.Length; i++) {
				speeds[i]=@battlers[i].SPE;
				quickclaw[i]=false;
				lagging[i]=false;
				//if (!ignorequickclaw && @choices[i][0]==1) { // Chose to use a move
				if (!ignorequickclaw && @choices[i]?.Action==ChoiceAction.UseMove) {
					if (!quickclaw[i] && @battlers[i].hasWorkingItem(Items.CUSTAP_BERRY) &&
						!@battlers[i].pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) &&
						!@battlers[i].pbOpposing2.hasWorkingAbility(Abilities.UNNERVE)) {
						if ((@battlers[i].hasWorkingAbility(Abilities.GLUTTONY) && @battlers[i].HP<=(int)Math.Floor(@battlers[i].TotalHP * .5)) ||
							@battlers[i].HP<=(int)Math.Floor(@battlers[i].TotalHP * .25)) {
							pbCommonAnimation("UseItem",@battlers[i],null);
							quickclaw[i]=true;
							pbDisplayBrief(Game._INTL("{1}'s {2} let it move first!",
								@battlers[i].ToString(),Game._INTL(@battlers[i].Item.ToString(TextScripts.Name))));
							@battlers[i].pbConsumeItem();
						}
					}
					if (!quickclaw[i] && @battlers[i].hasWorkingItem(Items.QUICK_CLAW)) {
						if (pbRandom(10)<2) {
							pbCommonAnimation("UseItem",@battlers[i],null);
							quickclaw[i]=true;
							pbDisplayBrief(Game._INTL("{1}'s {2} let it move first!",
								@battlers[i].ToString(),Game._INTL(@battlers[i].Item.ToString(TextScripts.Name))));
						}
					}
					if (!quickclaw[i] &&
						(@battlers[i].hasWorkingAbility(Abilities.STALL) ||
						@battlers[i].hasWorkingItem(Items.LAGGING_TAIL) ||
						@battlers[i].hasWorkingItem(Items.FULL_INCENSE))) {
						lagging[i]=true;
					}
				}
			}
			#endregion
			#region Calculate each Pokémon's priority bracket, and get the min/max priorities
			for (int i = 0; i < battlers.Length; i++) {
				// Assume that doing something other than using a move is priority 0
				int pri=0;
				if (@choices[i]?.Action==ChoiceAction.UseMove) { // Chose to use a move
					pri=@choices[i].Move.Priority;
					if (@battlers[i].hasWorkingAbility(Abilities.PRANKSTER) &&
						@choices[i].Move.pbIsStatus) pri+=1;
					if (@battlers[i].hasWorkingAbility(Abilities.GALE_WINGS) &&
						@choices[i].Move.Type == Types.FLYING) pri+=1;
				}
				priorities[i]=pri;
				if (i==0) {
					minpri=pri;
					maxpri=pri;
				}
				else {
					if (minpri>pri) minpri=pri;
					if (maxpri<pri) maxpri=pri;
				}
			}
			#endregion
			// Find and order all moves with the same priority
			int curpri=maxpri;
			do { //loop
				temp.Clear();
				for (int j = 0; j < battlers.Length; j++) {
					if (priorities[j]==curpri) temp.Add(j);
				}
				// Sort by speed
				if (temp.Count==1) {
					@priority[@priority.Length]=@battlers[temp[0]]; //ToDo: Redo this, maybe use Math.Min to sort..
				}
				else if (temp.Count>1) {
					int n=temp.Count - 1;
					for (int m = 0; m < temp.Count-1; m++) {
						for (int i = 1; i < temp.Count; i++) {
							// For each pair of battlers, rank the second compared to the first
							// -1 means rank higher, 0 means rank equal, 1 means rank lower
							int cmp=0;
							if (quickclaw[temp[i]]) {
								cmp=-1;
								if (quickclaw[temp[i-1]]) {
									if (speeds[temp[i]]==speeds[temp[i-1]]) {
										cmp=0;
									}
									else {
										cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? -1 : 1;
									}
								}
							}
							else if (quickclaw[temp[i-1]]) {
								cmp=1;
							}
							else if (lagging[temp[i]]) {
								cmp=1;
								if (lagging[temp[i-1]]) {
									if (speeds[temp[i]]==speeds[temp[i-1]]) {
										cmp=0;
									}
									else {
										cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? 1 : -1;
									}
								}
							}
							else if (lagging[temp[i-1]]) {
								cmp=-1;
							}
							else if (speeds[temp[i]]!=speeds[temp[i-1]]) {
								if (@field.TrickRoom>0) {
									cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? 1 : -1;
								}
								else {
									cmp=(speeds[temp[i]]>speeds[temp[i-1]]) ? -1 : 1;
								}
							}
							if (cmp<0 || // Swap the pair according to the second battler's rank
								(cmp==0 && pbRandom(2)==0)) {
								int swaptmp=temp[i];
								temp[i]=temp[i-1];
								temp[i-1]=swaptmp;
							}
						}
					}
					// Battlers in this bracket are properly sorted, so add them to @priority
					int x = 0; foreach (int i in temp) {
						//@priority[@priority.Length - 1]=@battlers[i];
						@priority[x]=@battlers[i]; x++;
					}
				}
				curpri-=1;
				if (curpri<minpri) break;
			} while (true);
			// Write the priority order to the debug log
			if (log) {
				string d="[Priority] "; bool comma=false;
				for (int i = 0; i < battlers.Length; i++) {
					if (@priority[i].IsNotNullOrNone() && !@priority[i].isFainted()) {
						if (comma) d+=", ";
						d+=$"#{@priority[i].ToString(comma)} (#{@priority[i].Index})"; comma=true;
					}
				}
				GameDebug.Log(d);
			}
			@usepriority=true;
			return @priority;
		}
		#endregion

		#region Switching Pokemon
		public virtual bool pbCanSwitchLax (int idxPokemon,int pkmnidxTo,bool showMessages) {
			if (pkmnidxTo>=0) {
				IPokemon[] party=pbParty(idxPokemon);
				if (pkmnidxTo>=party.Length) {
					return false;
				}
				if (!party[pkmnidxTo].IsNotNullOrNone()) {
					return false;
				}
				if (party[pkmnidxTo].isEgg) {
					if (showMessages) pbDisplayPaused(Game._INTL("An Egg can't battle!"));
					return false;
				}
				if (!pbIsOwner(idxPokemon,pkmnidxTo)) {
					ITrainer owner=pbPartyGetOwner(idxPokemon,pkmnidxTo);
					if (showMessages) pbDisplayPaused(Game._INTL("You can't switch {1}'s Pokémon with one of yours!",owner.name));
					return false;
				}
				if (party[pkmnidxTo].HP<=0) {
					if (showMessages) pbDisplayPaused(Game._INTL("{1} has no energy left to battle!",party[pkmnidxTo].Name));
					return false;
				}
				if (@battlers[idxPokemon].pokemonIndex==pkmnidxTo ||
					@battlers[idxPokemon].pbPartner.pokemonIndex==pkmnidxTo) {
					if (showMessages) pbDisplayPaused(Game._INTL("{1} is already in battle!",party[pkmnidxTo].Name));
					return false;
				}
			}
			return true;
		}

		public bool pbCanSwitch (int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook=false) {
			IBattler thispkmn=@battlers[idxPokemon];
			// Multi-Turn Attacks/Mean Look
			if (!pbCanSwitchLax(idxPokemon,pkmnidxTo,showMessages)) {
				return false;
			}
			bool isOppose=pbIsOpposing(idxPokemon);
			IPokemon[] party=pbParty(idxPokemon);
			for (int i = 0; i < battlers.Length; i++) {
				if (isOppose!=pbIsOpposing(i)) continue;
				if (choices[i].Action==ChoiceAction.SwitchPokemon && choices[i].Index==pkmnidxTo) {
					if (showMessages) pbDisplayPaused(Game._INTL("{1} has already been selected.",party[pkmnidxTo].Name));
					return false;
				}
			}
			if (thispkmn.hasWorkingItem(Items.SHED_SHELL)) {
				return true;
			}
			if (Core.USENEWBATTLEMECHANICS && thispkmn.pbHasType(Types.GHOST)) {
				return true;
			}
			if (thispkmn.effects.MultiTurn>0 ||
				(!ignoremeanlook && thispkmn.effects.MeanLook>=0)) {
				if (showMessages) pbDisplayPaused(Game._INTL("{1} can't be switched out!",thispkmn.ToString()));
				return false;
			}
			if (@field.FairyLock>0) {
				if (showMessages) pbDisplayPaused(Game._INTL("{1} can't be switched out!",thispkmn.ToString()));
				return false;
			}
			if (thispkmn.effects.Ingrain) {
				if (showMessages) pbDisplayPaused(Game._INTL("{1} can't be switched out!",thispkmn.ToString()));
				return false;
			}
			IBattler opp1=thispkmn.pbOpposing1;
			IBattler opp2=thispkmn.pbOpposing2;
			IBattler opp=null;
			if (thispkmn.pbHasType(Types.STEEL)) {
				if (opp1.hasWorkingAbility(Abilities.MAGNET_PULL)) opp=opp1;
				if (opp2.hasWorkingAbility(Abilities.MAGNET_PULL)) opp=opp2;
			}
			if (!thispkmn.isAirborne()) {
				if (opp1.hasWorkingAbility(Abilities.ARENA_TRAP)) opp=opp1;
				if (opp2.hasWorkingAbility(Abilities.ARENA_TRAP)) opp=opp2;
			}
			if (!thispkmn.hasWorkingAbility(Abilities.SHADOW_TAG)) {
				if (opp1.hasWorkingAbility(Abilities.SHADOW_TAG)) opp=opp1;
				if (opp2.hasWorkingAbility(Abilities.SHADOW_TAG)) opp=opp2;
			}
			if (opp.IsNotNullOrNone()) {
				string abilityname=Game._INTL(opp.Ability.ToString(TextScripts.Name));
				if (showMessages) pbDisplayPaused(Game._INTL("{1}'s {2} prevents switching!",opp.ToString(),abilityname));
				return false;
			}
			return true;
		}

		public virtual bool pbRegisterSwitch(int idxPokemon,int idxOther) {
			if (!pbCanSwitch(idxPokemon,idxOther,false)) return false;
			//@choices[idxPokemon][0]=2;          // "Switch Pokémon"
			//@choices[idxPokemon][1]=idxOther;   // Index of other Pokémon to switch with
			//@choices[idxPokemon][2]=null;
			@choices[idxPokemon]=new Choice(ChoiceAction.SwitchPokemon, idxOther);
			int side=pbIsOpposing(idxPokemon) ? 1 : 0;
			int owner=pbGetOwnerIndex(idxPokemon);
			if (@megaEvolution[side][owner]==idxPokemon) {
				@megaEvolution[side][owner]=-1;
			}
			return true;
		}

		public bool pbCanChooseNonActive (int index) {
			IPokemon[] party=pbParty(index);
			for (int i = 0; i < party.Length; i++) {
				if (pbCanSwitchLax(index,i,false)) return true;
			}
			return false;
		}

		public virtual void pbSwitch(bool favorDraws=false) {
			if (!favorDraws) {
				if (@decision>0) return;
			}
			else {
				if (@decision== BattleResults.DRAW) return;
			}
			pbJudge();
			if (@decision>0) return;
			int firstbattlerhp=@battlers[0].HP;
			List<int> switched=new List<int>();
			for (int index = 0; index < battlers.Length; index++) {
				int newenemy; int newenemyname; int newpokename; int newpoke;
				if (!@doublebattle && pbIsDoubleBattler(index)) continue;
				if (@battlers[index].IsNotNullOrNone() && !@battlers[index].isFainted()) continue;
				if (!pbCanChooseNonActive(index)) continue;
				if (!pbOwnedByPlayer(index)) {
					if (!pbIsOpposing(index) || (@opponent.Length > 0 && pbIsOpposing(index))) {
						newenemy=pbSwitchInBetween(index,false,false);
						newenemyname=newenemy;
						if (newenemy>=0 && pbParty(index)[newenemy].Ability == Abilities.ILLUSION) {
							newenemyname=pbGetLastPokeInTeam(index);
						}
						ITrainer opponent=pbGetOwner(index);
						if (!@doublebattle && firstbattlerhp>0 && @shiftStyle && this.opponent.Length > 0 &&
							@internalbattle && pbCanChooseNonActive(0) && pbIsOpposing(index) &&
							@battlers[0].effects.Outrage==0) {
							pbDisplayPaused(Game._INTL("{1} is about to send in {2}.",opponent.name,pbParty(index)[newenemyname].Name));
							if (pbDisplayConfirm(Game._INTL("Will {1} change Pokémon?",this.pbPlayer().name))) {
								newpoke=pbSwitchPlayer(0,true,true);
								if (newpoke>=0) {
									newpokename=newpoke;
									if (@party1[newpoke].Ability == Abilities.ILLUSION) {
										newpokename=pbGetLastPokeInTeam(0);
									}
									pbDisplayBrief(Game._INTL("{1}, that's enough! Come back!",@battlers[0].Name));
									pbRecallAndReplace(0,newpoke,newpokename);
									switched.Add(0);
								}
							}
						}
						pbRecallAndReplace(index,newenemy,newenemyname,false,false);
						switched.Add(index);
					}
				}
				else if (@opponent.Length > 0) {
					newpoke=pbSwitchInBetween(index,true,false);
					newpokename=newpoke;
					if (@party1[newpoke].Ability == Abilities.ILLUSION) {
						newpokename=pbGetLastPokeInTeam(index);
					}
					pbRecallAndReplace(index,newpoke,newpokename);
					switched.Add(index);
				}
				else {
					bool swtch=false;
					if (!pbDisplayConfirm(Game._INTL("Use next Pokémon?"))) {
						swtch=(pbRun(index,true)<=0);
					}
					else {
						swtch=true;
					}
					if (swtch) {
						newpoke=pbSwitchInBetween(index,true,false);
						newpokename=newpoke;
						if (@party1[newpoke].Ability == Abilities.ILLUSION) {
							newpokename=pbGetLastPokeInTeam(index);
						}
						pbRecallAndReplace(index,newpoke,newpokename);
						switched.Add(index);
					}
				}
			}
			if (switched.Count>0) {
				priority=pbPriority();
				foreach (var i in priority) {
					if (switched.Contains(i.Index)) i.pbAbilitiesOnSwitchIn(true);
				}
			}
		}

		public void pbSendOut(int index,IPokemon pokemon) {
			pbSetSeen(pokemon);
			if(@peer is IBattlePeerMultipleForms p) p.pbOnEnteringBattle(this,pokemon);
			if (pbIsOpposing(index)) {
				if (@scene is IPokeBattle_Scene s0) s0.pbTrainerSendOut(index,pokemon);
			}
			else {
				if (@scene is IPokeBattle_Scene s0) s0.pbSendOut(index,pokemon);
			}
			if (@scene is IPokeBattle_Scene s1) s1.pbResetMoveIndex(index);
		}

		public void pbReplace(int index,int newpoke,bool batonpass=false) {
			IPokemon[] party=pbParty(index);
			int oldpoke=@battlers[index].pokemonIndex;
			// Initialise the new Pokémon
			@battlers[index].pbInitialize(party[newpoke],(sbyte)newpoke,batonpass);
			// Reorder the party for this battle
			int[] partyorder=(!pbIsOpposing(index) ? @party1order : @party2order).ToArray();
			int bpo=-1; int bpn=-1;
			for (int i = 0; i < partyorder.Length; i++) {
				if (partyorder[i]==oldpoke) bpo=i;
				if (partyorder[i]==newpoke) bpn=i;
			}
			int p=partyorder[bpo]; partyorder[bpo]=partyorder[bpn]; partyorder[bpn]=p;
			// Send out the new Pokémon
			pbSendOut(index,party[newpoke]);
			pbSetSeen(party[newpoke]);
		}

		public bool pbRecallAndReplace(int index,int newpoke,int newpokename=-1,bool batonpass=false,bool moldbreaker=false) {
			@battlers[index].pbResetForm();
			if (!@battlers[index].isFainted()) {
				(@scene as IPokeBattle_DebugSceneNoGraphics).pbRecall(index);
			}
			pbMessagesOnReplace(index,newpoke,newpokename);
			pbReplace(index,newpoke,batonpass);
			return pbOnActiveOne(@battlers[index],false,moldbreaker);
		}

		public void pbMessagesOnReplace(int index,int newpoke,int newpokename=-1) {
			if (newpokename<0) newpokename=newpoke;
			IPokemon[] party=pbParty(index);
			if (pbOwnedByPlayer(index)) {
				if (!party[newpoke].IsNotNullOrNone()) {
					//p [index,newpoke,party[newpoke],pbAllFainted(party)];
					GameDebug.Log($"[{index},{newpoke},{party[newpoke]},pbMOR]");
					for (int i = 0; i < party.Length; i++) {
						GameDebug.Log($"[{i},{party[i].HP}]");
					}
					//throw new BattleAbortedException();
					GameDebug.LogError("BattleAbortedException"); pbAbort();
				}
				IBattler opposing=@battlers[index].pbOppositeOpposing;
				if (opposing.isFainted() || opposing.HP==opposing.TotalHP) {
					pbDisplayBrief(Game._INTL("Go! {1}!",party[newpokename].Name));
				}
				else if (opposing.HP>=(opposing.TotalHP/2)) {
					pbDisplayBrief(Game._INTL("Do it! {1}!",party[newpokename].Name));
				}
				else if (opposing.HP>=(opposing.TotalHP/4)) {
					pbDisplayBrief(Game._INTL("Go for it, {1}!",party[newpokename].Name));
				}
				else {
					pbDisplayBrief(Game._INTL("Your opponent's weak!\nGet 'em, {1}!",party[newpokename].Name));
				}
				GameDebug.Log($"[Send out Pokémon] Player sent out #{party[newpokename].Name} in position #{index}");
			}
			else {
				if (!party[newpoke].IsNotNullOrNone()) {
					//p [index,newpoke,party[newpoke],pbAllFainted(party)]
					GameDebug.Log($"[{index},{newpoke},{party[newpoke]},pbMOR]");
					for (int i = 0; i < party.Length; i++) {
						GameDebug.Log($"[{i},{party[i].HP}]");
					}
					//throw new BattleAbortedException();
					GameDebug.LogError("BattleAbortedException"); pbAbort();
				}
				ITrainer owner=pbGetOwner(index);
				pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!",owner.name,party[newpokename].Name));
				GameDebug.Log($"[Send out Pokémon] Opponent sent out #{party[newpokename].Name} in position #{index}");
			}
		}

		public virtual int pbSwitchInBetween(int index, bool lax, bool cancancel) {
			if (!pbOwnedByPlayer(index)) {
				return (@scene as IPokeBattle_SceneNonInteractive).pbChooseNewEnemy(index,pbParty(index));
			}
			else {
				return pbSwitchPlayer(index,lax,cancancel);
			}
		}

		public int pbSwitchPlayer(int index,bool lax, bool cancancel) {
			//if (@debug) {
			//	return (@scene as IPokeBattle_SceneNonInteractive).pbChooseNewEnemy(index,pbParty(index));
			//}
			//else {
				return (@scene as IPokeBattle_SceneNonInteractive).pbSwitch(index,lax,cancancel);
			//}
		}
		#endregion

		#region Using an Item
		/// <summary>
		/// Uses an item on a Pokémon in the player's party.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pkmnIndex"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		//protected bool _pbUseItemOnPokemon(Items item,int pkmnIndex,IBattler userPkmn,IHasDisplayMessage scene) {
		bool IBattle.pbUseItemOnPokemon(Items item,int pkmnIndex,IBattler userPkmn,IHasDisplayMessage scene) {
			IPokemon pokemon=@party1[pkmnIndex];
			IBattler battler=null;
			string name=pbGetOwner(userPkmn.Index).name;
			if (pbBelongsToPlayer(userPkmn.Index)) name=pbGetOwner(userPkmn.Index).name;
			pbDisplayBrief(Game._INTL("{1} used the\r\n{2}.",name,Game._INTL(item.ToString(TextScripts.Name))));
			GameDebug.Log($"[Use item] Player used #{Game._INTL(item.ToString(TextScripts.Name))} on #{pokemon.Name}");
			bool ret=false;
			if (pokemon.isEgg) {
				pbDisplay(Game._INTL("But it had no effect!"));
			}
			else {
				for (int i = 0; i < battlers.Length; i++) {
					if (!pbIsOpposing(i) && @battlers[i].pokemonIndex==pkmnIndex) {
						battler=@battlers[i];
					}
				}
				ret=ItemHandlers.triggerBattleUseOnPokemon(item,pokemon,battler,scene); //Invoke Event, returns which pokemon selected
			}
			if (!ret && pbBelongsToPlayer(userPkmn.Index)) {
				if (Game.GameData.Bag.pbCanStore(item)) {
					Game.GameData.Bag.pbStoreItem(item);
				}
				else {
					//throw new Exception(Game._INTL("Couldn't return unused item to Bag somehow."));
					GameDebug.LogError(Game._INTL("Couldn't return unused item to Bag somehow."));
				}
			}
			return ret;
		}

		/// <summary>
		/// Uses an item on an active Pokémon.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		public bool pbUseItemOnBattler(Items item,int index,IBattler userPkmn,IHasDisplayMessage scene) {
			GameDebug.Log($"[Use item] Player used #{Game._INTL(item.ToString(TextScripts.Name))} on #{@battlers[index].ToString(true)}");
			bool ret=ItemHandlers.triggerBattleUseOnBattler(item,@battlers[index],scene);
			if (!ret && pbBelongsToPlayer(userPkmn.Index)) {
				if (Game.GameData.Bag.pbCanStore(item)) {
					Game.GameData.Bag.pbStoreItem(item);
				}
				else {
					//throw new Exception(Game._INTL("Couldn't return unused item to Bag somehow."));
					GameDebug.LogError(Game._INTL("Couldn't return unused item to Bag somehow."));
				}
			}
			return ret;
		}

		public bool pbRegisterItem(int idxPokemon,Items idxItem,int? idxTarget=null) {
			if (idxTarget!=null && idxTarget.Value>=0) {
				for (int i = 0; i < battlers.Length; i++) {
					if (!@battlers[i].pbIsOpposing(idxPokemon) &&
						@battlers[i].pokemonIndex==idxTarget.Value &&
						@battlers[i].effects.Embargo>0) {
						pbDisplay(Game._INTL("Embargo's effect prevents the item's use on {1}!",@battlers[i].ToString(true)));
						if (pbBelongsToPlayer(@battlers[i].Index)) {
							if (Game.GameData.Bag.pbCanStore(idxItem)) {
								Game.GameData.Bag.pbStoreItem(idxItem);
							}
							else {
								//throw new Exception(Game._INTL("Couldn't return unused item to Bag somehow."));
								GameDebug.LogError(Game._INTL("Couldn't return unused item to Bag somehow."));
							}
						}
						return false;
					}
				}
			}
			if (ItemHandlers.hasUseInBattle(idxItem)) {
				if (idxPokemon==0) { // Player's first Pokémon
					if (ItemHandlers.triggerBattleUseOnBattler(idxItem,@battlers[idxPokemon],this)) {
						// Using Poké Balls or Poké Doll only
						ItemHandlers.triggerUseInBattle(idxItem,@battlers[idxPokemon],this);
						if (@doublebattle) {
							@battlers[idxPokemon+2].effects.SkipTurn=true;
						}
					}
					else {
						if (Game.GameData.Bag.pbCanStore(idxItem)) {
							Game.GameData.Bag.pbStoreItem(idxItem);
						}
						else {
							//throw new Exception(Game._INTL("Couldn't return unusable item to Bag somehow."));
							GameDebug.LogError(Game._INTL("Couldn't return unusable item to Bag somehow."));
						}
						return false;
					}
				}
				else {
					if (ItemHandlers.triggerBattleUseOnBattler(idxItem,@battlers[idxPokemon],this)) {
						pbDisplay(Game._INTL("It's impossible to aim without being focused!"));
					}
					return false;
				}
			}
			//@choices[idxPokemon][0]=3;         // "Use an item"
			//@choices[idxPokemon][1]=idxItem;   // ID of item to be used
			//@choices[idxPokemon][2]=idxTarget; // Index of Pokémon to use item on
			@choices[idxPokemon]= new Choice(ChoiceAction.UseItem,idxItem,idxTarget.Value);
			int side=pbIsOpposing(idxPokemon) ? 1 : 0;
			int owner=pbGetOwnerIndex(idxPokemon);
			if (@megaEvolution[side][owner]==idxPokemon) {
				@megaEvolution[side][owner]=-1;
			}
			return true;
		}

		public void pbEnemyUseItem(Items item, IBattler battler) {
			if (!@internalbattle) return; //0
			Items[] items=pbGetOwnerItems(battler.Index);
			if (items == null) return; //Items.NONE
			ITrainer opponent=pbGetOwner(battler.Index);
			for (int i = 0; i < items.Length; i++) {
				if (items[i]==item) {
					//items.delete_at(i);
					items[i]=Items.NONE;
					break;
				}
			}
			string itemname=Game._INTL(item.ToString(TextScripts.Name));
			pbDisplayBrief(Game._INTL("{1} used the\r\n{2}!",opponent.name,itemname));
			GameDebug.Log($"[Use item] Opponent used #{itemname} on #{battler.ToString(true)}");
			if (item == Items.POTION) {
				battler.pbRecoverHP(20,true);
				pbDisplay(Game._INTL("{1}'s HP was restored.",battler.ToString()));
			}
			else if (item == Items.SUPER_POTION) {
				battler.pbRecoverHP(50,true);
				pbDisplay(Game._INTL("{1}'s HP was restored.",battler.ToString()));
			}
			else if (item == Items.HYPER_POTION) {
				battler.pbRecoverHP(200,true);
				pbDisplay(Game._INTL("{1}'s HP was restored.",battler.ToString()));
			}
			else if (item == Items.MAX_POTION) {
				battler.pbRecoverHP(battler.TotalHP-battler.HP,true);
				pbDisplay(Game._INTL("{1}'s HP was restored.",battler.ToString()));
			}
			else if (item == Items.FULL_RESTORE) {
				bool fullhp=(battler.HP==battler.TotalHP);
				battler.pbRecoverHP(battler.TotalHP-battler.HP,true);
				battler.Status=0; battler.StatusCount=0;
				battler.effects.Confusion=0;
				if (fullhp) {
					pbDisplay(Game._INTL("{1} became healthy!",battler.ToString()));
				}
				else {
					pbDisplay(Game._INTL("{1}'s HP was restored.",battler.ToString()));
				}
			}
			else if (item == Items.FULL_HEAL) {
				battler.Status=0; battler.StatusCount=0;
				battler.effects.Confusion=0;
				pbDisplay(Game._INTL("{1} became healthy!",battler.ToString()));
			}
			else if (item == Items.X_ATTACK) {
				if (battler is IBattlerEffect b && b.pbCanIncreaseStatStage(Stats.ATTACK,battler)) {
					b.pbIncreaseStat(Stats.ATTACK,1,battler,true);
				}
			}
			else if (item == Items.X_DEFENSE) {
				if (battler is IBattlerEffect b && b.pbCanIncreaseStatStage(Stats.DEFENSE,battler)) {
					b.pbIncreaseStat(Stats.DEFENSE,1,battler,true);
				}
			}
			else if (item == Items.X_SPEED) {
				if (battler is IBattlerEffect b && b.pbCanIncreaseStatStage(Stats.SPEED,battler)) {
					b.pbIncreaseStat(Stats.SPEED,1,battler,true);
				}
			}
			else if (item == Items.X_SP_ATK) {
				if (battler is IBattlerEffect b && b.pbCanIncreaseStatStage(Stats.SPATK,battler)) {
					b.pbIncreaseStat(Stats.SPATK,1,battler,true);
				}
			}
			else if (item == Items.X_SP_DEF) {
				if (battler is IBattlerEffect b && b.pbCanIncreaseStatStage(Stats.SPDEF,battler)) {
					b.pbIncreaseStat(Stats.SPDEF,1,battler,true);
				}
			}
			else if (item == Items.X_ACCURACY) {
				if (battler is IBattlerEffect b && b.pbCanIncreaseStatStage(Stats.ACCURACY,battler)) {
					b.pbIncreaseStat(Stats.ACCURACY,1,battler,true);
				}
			}
		}
		#endregion

		#region Fleeing from Battle
		public bool pbCanRun(int idxPokemon)
		{
			if (@opponent.Length > 0) return false;
			if (@cantescape && !pbIsOpposing(idxPokemon)) return false;
			IBattler thispkmn=@battlers[idxPokemon];
			if (thispkmn.pbHasType(Types.GHOST) && Core.USENEWBATTLEMECHANICS) return true;
			if (thispkmn.hasWorkingItem(Items.SMOKE_BALL)) return true;
			if (thispkmn.hasWorkingAbility(Abilities.RUN_AWAY)) return true;
			return pbCanSwitch(idxPokemon,-1,false);
		}

		public virtual int pbRun(int idxPokemon,bool duringBattle=false) {
			IBattler thispkmn=@battlers[idxPokemon];
			if (pbIsOpposing(idxPokemon)) {
				if (@opponent.Length > 0) return 0;
				//@choices[i][0]=5; // run
				//@choices[i][1]=0;
				//@choices[i][2]=null;
				@choices[idxPokemon] = new Choice(ChoiceAction.Run);
				return -1;
			}
			if (@opponent.Length > 0) {
				if (debug && Input.press((int)PokemonUnity.UX.InputKeys.DEBUG)) {
					if (pbDisplayConfirm(Game._INTL("Treat this battle as a win?"))) {
						@decision=BattleResults.WON;
						return 1;
					}
					else if (pbDisplayConfirm(Game._INTL("Treat this battle as a loss?"))) {
						@decision=BattleResults.LOST;
						return 1;
					}
				}
				else if (@internalbattle) {
					pbDisplayPaused(Game._INTL("No! There's no running from a Trainer battle!"));
				}
				else if (pbDisplayConfirm(Game._INTL("Would you like to forfeit the match and quit now?"))) {
					pbDisplay(Game._INTL("{1} forfeited the match!",this.pbPlayer().name));
					@decision=BattleResults.FORFEIT;
					return 1;
				}
				return 0;
			}
			if (debug && Input.press((int)PokemonUnity.UX.InputKeys.DEBUG)) {
				pbDisplayPaused(Game._INTL("Got away safely!"));
				@decision=BattleResults.FORFEIT;
				return 1;
			}
			if (@cantescape) {
				pbDisplayPaused(Game._INTL("Can't escape!"));
				return 0;
			}
			if (thispkmn.pbHasType(Types.GHOST) && Core.USENEWBATTLEMECHANICS) {
				pbDisplayPaused(Game._INTL("Got away safely!"));
				@decision=BattleResults.FORFEIT;
				return 1;
			}
			if (thispkmn.hasWorkingAbility(Abilities.RUN_AWAY)) {
				if (duringBattle) {
					pbDisplayPaused(Game._INTL("Got away safely!"));
				}
				else {
					pbDisplayPaused(Game._INTL("{1} escaped using Run Away!",thispkmn.ToString()));
				}
				@decision=BattleResults.FORFEIT;
				return 1;
			}
			if (thispkmn.hasWorkingItem(Items.SMOKE_BALL)) {
				if (duringBattle) {
					pbDisplayPaused(Game._INTL("Got away safely!"));
				}
				else {
					pbDisplayPaused(Game._INTL("{1} escaped using its {2}!",thispkmn.ToString(),Game._INTL(thispkmn.Item.ToString(TextScripts.Name))));
				}
				@decision=BattleResults.FORFEIT;
				return 1;
			}
			if (!duringBattle && !pbCanSwitch(idxPokemon,-1,false)) {
				pbDisplayPaused(Game._INTL("Can't escape!"));
				return 0;
			}
			int rate;
			// Note: not pbSpeed, because using unmodified Speed
			int speedPlayer=@battlers[idxPokemon].pokemon.SPE;
			IBattler opposing=@battlers[idxPokemon].pbOppositeOpposing;
			if (opposing.isFainted()) opposing=opposing.pbPartner;
			if (!opposing.isFainted()) {
				int speedEnemy=opposing.pokemon.SPE;
				if (speedPlayer>speedEnemy) {
					rate=256;
				}
				else {
					if (speedEnemy<=0) speedEnemy=1;
					rate=speedPlayer*128/speedEnemy;
					rate+=@runCommand*30;
					rate&=0xFF;
				}
			}
			else {
				rate=256;
			}
			int ret=1;
			if (pbRandom(256)<rate) { //pbAIRandom
				pbDisplayPaused(Game._INTL("Got away safely!"));
				@decision=BattleResults.FORFEIT;
			}
			else {
				pbDisplayPaused(Game._INTL("Can't escape!"));
				ret=-1;
			}
			if (!duringBattle) @runCommand+=1;
			return ret;
		}
		#endregion

		#region Mega Evolve Battler
		public bool pbCanMegaEvolve (int index) {
			if (Core.NO_MEGA_EVOLUTION) return false;
			if (!@battlers[index].hasMega) return false;
			if (pbIsOpposing(index) && @opponent.Length == 0) return false;
			if (debug && Input.press((int)PokemonUnity.UX.InputKeys.DEBUG)) return true;
			if (!pbHasMegaRing(index)) return false;
			int side=pbIsOpposing(index) ? 1 : 0;
			int owner=pbGetOwnerIndex(index);
			if (@megaEvolution[side][owner]!=-1) return false;
			if (@battlers[index].effects.SkyDrop) return false;
			return true;
		}

		public void pbRegisterMegaEvolution(int index) {
			int side=pbIsOpposing(index) ? 1 : 0;
			int owner=pbGetOwnerIndex(index);
			@megaEvolution[side][owner]=(sbyte)index;
		}

		public void pbMegaEvolve(int index) {
			if (!@battlers[index].IsNotNullOrNone() || !@battlers[index].pokemon.IsNotNullOrNone()) return;
			if (!@battlers[index].hasMega) return; //rescue false
			if (@battlers[index].isMega) return; //rescue true
			string ownername=pbGetOwner(index).name;
			if (pbBelongsToPlayer(index)) ownername=pbGetOwner(index).name;
			if (@battlers[index].pokemon is IPokemonMegaEvolution m && m.megaMessage() == 1) { //switch @battlers[index].pokemon.megaMessage rescue 0
				//case 1: // Rayquaza
					pbDisplay(Game._INTL("{1}'s fervent wish has reached {2}!",ownername,@battlers[index].ToString()));
				//  break;
			} else { //default:
				pbDisplay(Game._INTL("{1}'s {2} is reacting to {3}'s {4}!",
					@battlers[index].ToString(),Game._INTL(@battlers[index].Item.ToString(TextScripts.Name)),
					ownername,pbGetMegaRingName(index)));
				//  break;
			}
			pbCommonAnimation("MegaEvolution",@battlers[index],null);
			if (@battlers[index].pokemon is IPokemonMegaEvolution p) p.makeMega();
			@battlers[index].form=@battlers[index].pokemon is IPokemonMultipleForms f ? f.form : 0;
			@battlers[index].pbUpdate(true);
			if (@scene is IPokeBattle_Scene s0) s0.pbChangePokemon(@battlers[index],(@battlers[index] as Pokemon).Form.Id);
			pbCommonAnimation("MegaEvolution2",@battlers[index],null);
			string meganame=@battlers[index].pokemon.Name; //megaName rescue null
			if (string.IsNullOrEmpty(meganame)) {
				meganame=Game._INTL("Mega {1}",Game._INTL(@battlers[index].pokemon.Species.ToString(TextScripts.Name)));
			}
			pbDisplay(Game._INTL("{1} has Mega Evolved into {2}!",@battlers[index].ToString(),meganame));
			GameDebug.Log($"[Mega Evolution] #{@battlers[index].ToString()} Mega Evolved");
			int side=pbIsOpposing(index) ? 1 : 0;
			int owner=pbGetOwnerIndex(index);
			@megaEvolution[side][owner]=-2;
		}
		#endregion

		#region Primal Revert Battler
		public void pbPrimalReversion(int index) {
			if (!@battlers[index].IsNotNullOrNone() || !@battlers[index].pokemon.IsNotNullOrNone()) return;
			if (!@battlers[index].hasPrimal) return; //rescue false
			if (@battlers[index].pokemon.Species != Pokemons.KYOGRE || 
				@battlers[index].pokemon.Species != Pokemons.GROUDON) return;
			if (@battlers[index].isPrimal) return; //rescue true
			if (@battlers[index].pokemon.Species == Pokemons.KYOGRE) {
				pbCommonAnimation("PrimalKyogre",@battlers[index],null);
			}
			else if (@battlers[index].pokemon.Species == Pokemons.GROUDON) {
				pbCommonAnimation("PrimalGroudon",@battlers[index],null);
			}
			if (@battlers[index].pokemon is IPokemonMegaEvolution p) p.makePrimal();
			@battlers[index].form=@battlers[index].pokemon is IPokemonMultipleForms f ? f.form : 0;
			@battlers[index].pbUpdate(true);
			if (@scene is IPokeBattle_Scene s0) s0.pbChangePokemon(@battlers[index],(@battlers[index] as Pokemon).Form.Id);
			if (@battlers[index].pokemon.Species == Pokemons.KYOGRE) {
				pbCommonAnimation("PrimalKyogre2",@battlers[index],null);
			}
			else if (@battlers[index].pokemon.Species == Pokemons.GROUDON) {
				pbCommonAnimation("PrimalGroudon2",@battlers[index],null);
			}
			pbDisplay(Game._INTL("{1}'s Primal Reversion!\nIt reverted to its primal form!",@battlers[index].ToString()));
			GameDebug.Log($"[Primal Reversion] #{@battlers[index].ToString()} Primal Reverted");
		}
		#endregion

		#region Call Battler
		public void pbCall(int index) {
			ITrainer owner=pbGetOwner(index);
			pbDisplay(Game._INTL("{1} called {2}!",owner.name,@battlers[index].Name));
			pbDisplay(Game._INTL("{1}!",@battlers[index].Name));
			GameDebug.Log($"[Call to Pokémon] #{owner.name} called to #{@battlers[index].ToString(true)}");
			if (@battlers[index] is IBattlerShadowPokemon b && b.isShadow()) {
				if (b.inHyperMode() && @battlers[index].pokemon is IPokemonShadowPokemon p) {
					p.hypermode=false;
					p.adjustHeart(-300);
					//ToDo: There should be a method for this in pokemon class?...
					//@battlers[index].isHyperMode=false;
					//@battlers[index].pokemon.ChangeHappiness(HappinessMethods.CALL);
					pbDisplay(Game._INTL("{1} came to its senses from the Trainer's call!",@battlers[index].ToString()));
				}
				else {
					pbDisplay(Game._INTL("But nothing happened!"));
				}
			}
			else if (@battlers[index].Status!=Status.SLEEP && @battlers[index] is IBattlerEffect b0 &&
					b0.pbCanIncreaseStatStage(Stats.ACCURACY,@battlers[index])) {
				b0.pbIncreaseStat(Stats.ACCURACY,1,@battlers[index],true);
			}
			else {
				pbDisplay(Game._INTL("But nothing happened!"));
			}
		}
		#endregion

		#region Gaining Experience
		public virtual void pbGainEXP() {
			if (!@internalbattle) return;
			bool successbegin=true;
			for (int i = 0; i < battlers.Length; i++) { // Not ordered by priority
				if (!@doublebattle && pbIsDoubleBattler(i)) {
					@battlers[i].participants.Clear();//=[];
					continue;
				}
				if (pbIsOpposing(i) && @battlers[i].participants.Count>0 &&
					(@battlers[i].isFainted() || @battlers[i].captured)) {
					bool haveexpall=Game.GameData.Bag.pbQuantity(Items.EXP_ALL)>0; //hasConst(Items.EXP_ALL) &&
					// First count the number of participants
					int partic=0;
					int expshare=0;
					foreach (var j in @battlers[i].participants) {
						if (!@party1[j].IsNotNullOrNone() || !pbIsOwner(0,j)) continue;
						if (@party1[j].HP>0 && !@party1[j].isEgg) partic+=1;
					}
					if (!haveexpall) {
						for (int j = 0; j < @party1.Length; j++) {
							if (!@party1[j].IsNotNullOrNone() || !pbIsOwner(0,j)) continue;
							if (@party1[j].HP>0 && !@party1[j].isEgg &&
								(@party1[j].Item == Items.EXP_SHARE ||
								@party1[j].itemInitial == Items.EXP_SHARE)) expshare+=1;
						}
					}
					// Now calculate EXP for the participants
					if (partic>0 || expshare>0 || haveexpall) {
						if (@opponent == null && successbegin && pbAllFainted(@party2)) {
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbWildBattleSuccess();
							successbegin=false;
						}
						for (int j = 0; j < @party1.Length; j++) {
							if (!@party1[j].IsNotNullOrNone() || !pbIsOwner(0,j)) continue;
							if (@party1[j].HP<=0 || @party1[j].isEgg) continue;
							bool haveexpshare=@party1[j].Item == Items.EXP_SHARE ||
												@party1[j].itemInitial == Items.EXP_SHARE;
							if (!haveexpshare && !@battlers[i].participants.Contains((byte)j)) continue;
								pbGainExpOne(j,@battlers[i],partic,expshare,haveexpall);
						}
						if (haveexpall) {
							bool showmessage=true;
							for (int j = 0; j < @party1.Length; j++) {
								if (!@party1[j].IsNotNullOrNone() || !pbIsOwner(0,j)) continue;
								if (@party1[j].HP<=0 || @party1[j].isEgg) continue;
								if (@party1[j].Item == Items.EXP_SHARE ||
									@party1[j].itemInitial == Items.EXP_SHARE) continue;
								if (@battlers[i].participants.Contains((byte)j)) continue;
								if (showmessage) pbDisplayPaused(Game._INTL("The rest of your team gained Exp. Points thanks to the {1}!",
									Game._INTL(Items.EXP_ALL.ToString(TextScripts.Name))));
								showmessage=false;
								pbGainExpOne(j,@battlers[i],partic,expshare,haveexpall,false);
							}
						}
					}
					// Now clear the participants array
					@battlers[i].participants.Clear();//=[];
				}
			}
		}

		public void pbGainExpOne(int index,IBattler defeated,int partic,int expshare,bool haveexpall,bool showmessages=true) {
			IPokemon thispoke=@party1[index];
			// Original species, not current species
			int level=defeated.Level;
			float baseexp=Kernal.PokemonData[defeated.Species].BaseExpYield;
			int[] evyield=Kernal.PokemonData[defeated.Species].EVYield;
			// Gain effort value points, using RS effort values
			int totalev=0;
			for (int k = 0; k < Core.MAXPARTYSIZE; k++) {
				totalev+=thispoke.EV[k];
			}
			for (int k = 0; k < Core.MAXPARTYSIZE; k++) {
				int evgain=evyield[k];
				if (thispoke.Item == Items.MACHO_BRACE ||
					thispoke.itemInitial == Items.MACHO_BRACE) evgain*=2;
				switch ((Stats)k) {
					case Stats.HP:
						if (thispoke.Item == Items.POWER_WEIGHT ||
							thispoke.itemInitial == Items.POWER_WEIGHT) evgain+=4;
						break;
					case Stats.ATTACK:
						if (thispoke.Item == Items.POWER_BRACER ||
							thispoke.itemInitial == Items.POWER_BRACER) evgain+=4;
						break;
					case Stats.DEFENSE:
						if (thispoke.Item == Items.POWER_BELT ||
							thispoke.itemInitial == Items.POWER_BELT) evgain+=4;
						break;
					case Stats.SPATK:
						if (thispoke.Item == Items.POWER_LENS ||
							thispoke.itemInitial == Items.POWER_LENS) evgain+=4;
						break;
					case Stats.SPDEF:
						if (thispoke.Item == Items.POWER_BAND ||
							thispoke.itemInitial == Items.POWER_BAND) evgain+=4;
						break;
					case Stats.SPEED:
						if (thispoke.Item == Items.POWER_ANKLET ||
							thispoke.itemInitial == Items.POWER_ANKLET) evgain+=4;
						break;
				}
				//if (thispoke.PokerusStage>=1) evgain*=2;	// Infected or cured
				if (thispoke.PokerusStage == true) evgain*=2;	// Infected only
				if (evgain>0) {
					// Can't exceed overall limit
					if (totalev+evgain>Monster.Pokemon.EVLIMIT) evgain-=totalev+evgain-Monster.Pokemon.EVLIMIT;
					// Can't exceed stat limit
					if (thispoke.EV[k]+evgain>Monster.Pokemon.EVSTATLIMIT) evgain-=thispoke.EV[k]+evgain-Monster.Pokemon.EVSTATLIMIT;
					// Add EV gain
					//thispoke.EV[k]+=evgain;
					thispoke.EV[k]=(byte)(thispoke.EV[k]+evgain);
					if (thispoke.EV[k]>Monster.Pokemon.EVSTATLIMIT) {
						GameDebug.LogWarning($"Single-stat EV limit #{Monster.Pokemon.EVSTATLIMIT} exceeded.\r\nStat: #{k}  EV gain: #{evgain}  EVs: #{thispoke.EV.ToString()}");
						thispoke.EV[k]=Monster.Pokemon.EVSTATLIMIT;
					}
					totalev+=evgain;
					if (totalev>Monster.Pokemon.EVLIMIT) {
						GameDebug.LogWarning($"EV limit #{Monster.Pokemon.EVLIMIT} exceeded.\r\nTotal EVs: #{totalev} EV gain: #{evgain}  EVs: #{thispoke.EV.ToString()}");
					}
				}
			}
			(thispoke as Monster.Pokemon).GainEffort(defeated.Species);
			// Gain experience
			bool ispartic=false;
			//if (defeated.participants.Contains(index)) ispartic=true;
			ispartic=defeated.participants.Contains((byte)index);
			bool haveexpshare=thispoke.Item == Items.EXP_SHARE ||
							thispoke.itemInitial == Items.EXP_SHARE;
			int exp=0;
			if (expshare>0) {
				if (partic==0) { // No participants, all Exp goes to Exp Share holders
					exp=(int)Math.Floor(level*baseexp);
					//exp=(int)Math.Floor(exp/(Core.NOSPLITEXP ? 1 : expshare))*haveexpshare;
					exp=haveexpshare ? (int)Math.Floor(exp/(Core.NOSPLITEXP ? 1f : expshare)) : 0;
				}
				else {
					if (Core.NOSPLITEXP) {
						//exp=(int)Math.Floor(level*baseexp)*ispartic;
						//if (!ispartic) exp=(int)Math.Floor(level*baseexp*.5)*haveexpshare;
						exp=ispartic?(int)Math.Floor(level*baseexp):0;
						if (!ispartic) exp=haveexpshare ? (int)Math.Floor(level*baseexp*.5) : 0;
					}
					else {
						exp=(int)Math.Floor(level*baseexp*.5);
						//exp=(int)Math.Floor(exp/partic)*ispartic + (int)Math.Floor(exp/expshare)*haveexpshare;
						exp=haveexpshare ? (int)Math.Floor(exp/(float)partic)*(ispartic?1:0) + (int)Math.Floor(exp/(float)expshare) : 0;
					}
				}
			}
			else if (ispartic) {
				exp=(int)Math.Floor(level*baseexp/(Core.NOSPLITEXP ? 1 : partic));
			}
			else if (haveexpall) {
				exp=(int)Math.Floor(level*baseexp/2);
			}
			if (exp<=0) return;
			if (@opponent.Length>0) exp=(int)Math.Floor(exp*3*.5);
			if (Core.USESCALEDEXPFORMULA) {
				exp=(int)Math.Floor(exp/5f);
				double leveladjust=(2*level+10.0)/(level+thispoke.Level+10.0);
				leveladjust=Math.Pow(leveladjust,5);
				leveladjust=Math.Sqrt(leveladjust);
				exp=(int)Math.Floor(exp*leveladjust);
				if (ispartic || haveexpshare) exp+=1;
			}
			else {
				exp=(int)Math.Floor(exp/7f);
			}
			bool isOutsider = thispoke.isForeign(pbPlayer());
			//				|| (thispoke.language!=0 && thispoke.language!=this.pbPlayer().language);
			if (isOutsider) {
				//if (thispoke.language!=0 && thispoke.language!=this.pbPlayer().language) {
				//  exp=(int)Math.Floor(exp*1.7);
				//}
				//else {
					exp=(int)Math.Floor(exp*3/2f);
				//}
			}
			if (thispoke.Item == Items.LUCKY_EGG ||
				thispoke.itemInitial == Items.LUCKY_EGG) exp=(int)Math.Floor(exp*3/2f);
			Monster.LevelingRate growthrate=thispoke.GrowthRate;
			//int newexp=new Experience(thispoke.exp,exp,growthrate).AddExperience(exp).Current;
			Monster.Data.Experience gainedexp=new Monster.Data.Experience(growthrate,thispoke.Exp);
			gainedexp.AddExperience(exp);
			int newexp=gainedexp.Total;
			exp=newexp-thispoke.Exp;
			if (exp>0) {
				if (showmessages) {
					if (isOutsider) {
						pbDisplayPaused(Game._INTL("{1} gained a boosted {2} Exp. Points!",thispoke.Name,exp.ToString()));
					}
					else {
						pbDisplayPaused(Game._INTL("{1} gained {2} Exp. Points!",thispoke.Name,exp.ToString()));
					}
				}
				int newlevel=Monster.Data.Experience.GetLevelFromExperience(growthrate,newexp);
				//int tempexp=0;
				int curlevel=thispoke.Level;
				if (newlevel<curlevel) {
					string debuginfo=$"#{thispoke.Name}: #{thispoke.Level}/#{newlevel} | #{thispoke.Exp}/#{newexp} | gain: #{exp}";
					//throw new RuntimeError(Game._INTL("The new level ({1}) is less than the Pokémon's\r\ncurrent level ({2}), which shouldn't happen.\r\n[Debug: {3}]",
					GameDebug.LogError(Game._INTL("The new level {1) is less than the Pokémon's\r\ncurrent level (2), which shouldn't happen.\r\n[Debug: {3}]",
					newlevel.ToString(),curlevel.ToString(),debuginfo));
					return;
				}
				if (thispoke is IPokemonShadowPokemon p && p.isShadow) {
					//thispoke.exp+=exp;
					//thispoke.Experience.AddExperience(exp);
					p.savedexp+=exp;
				}
				else {
					int tempexp1=thispoke.Exp;
					int tempexp2=0;
					// Find battler
					IBattler battler=pbFindPlayerBattler(index);
					do { //loop
						// EXP Bar animation
						int startexp=Monster.Data.Experience.GetStartExperience(growthrate,curlevel); //0
						int endexp=Monster.Data.Experience.GetStartExperience(growthrate,curlevel+1); //100
						tempexp2=(endexp<newexp) ? endexp : newexp; //final < 100?
						thispoke.Exp = tempexp2;
						//thispoke.Experience.AddExperience(tempexp2 - thispoke.exp);
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbEXPBar(battler,thispoke,startexp,endexp,tempexp1,tempexp2);
						tempexp1=tempexp2;
						curlevel+=1;
						if (curlevel>newlevel) {
							thispoke.calcStats(); 
							if (battler.IsNotNullOrNone()) battler.pbUpdate(false);
							@scene.pbRefresh();
							break;
						}
						int oldtotalhp=thispoke.TotalHP;
						int oldattack=thispoke.ATK;
						int olddefense=thispoke.DEF;
						int oldspeed=thispoke.SPE;
						int oldspatk=thispoke.SPA;
						int oldspdef=thispoke.SPD;
						if (battler.IsNotNullOrNone() && @internalbattle) { //&& battler.pokemon.IsNotNullOrNone()
							(battler.pokemon as Monster.Pokemon).ChangeHappiness(HappinessMethods.LEVELUP);//"level up"
						}
						thispoke.calcStats(); 
						if (battler.IsNotNullOrNone()) battler.pbUpdate(false);
						@scene.pbRefresh();
						pbDisplayPaused(Game._INTL("{1} grew to Level {2}!",thispoke.Name,curlevel.ToString()));
						//ToDo: Can Evolve during battle?
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbLevelUp(battler,thispoke,oldtotalhp,oldattack,
										olddefense,oldspeed,oldspatk,oldspdef);
						// Finding all moves learned at this level
						Moves[] movelist=thispoke.getMoveList(Monster.LearnMethod.levelup);
						foreach (Moves k in movelist) {
							//if (k[0]==thispoke.Level)     // Learned a new move
								//pbLearnMove(index,k[1]);
								pbLearnMove(index,k);
						}
					} while (true);
				}
			}
		}
		#endregion

		#region Learning a move.
		public void pbLearnMove(int pkmnIndex,Moves move) {
			IPokemon pokemon=@party1[pkmnIndex];
			if (!pokemon.IsNotNullOrNone()) return;
			string pkmnname=pokemon.Name;
			IBattler battler=pbFindPlayerBattler(pkmnIndex);
			string movename=Game._INTL(move.ToString(TextScripts.Name));
			for (int i = 0; i < pokemon.moves.Length; i++) {
				if (pokemon.moves[i].id==move) return;
				if (pokemon.moves[i].id==0) {
					pokemon.moves[i]=new PokemonUnity.Attack.Move(move); //ToDo: Use LearnMove Method in Pokemon Class?
					if (battler.IsNotNullOrNone())
						battler.moves[i]=Combat.Move.pbFromPBMove(this,pokemon.moves[i]);
					pbDisplayPaused(Game._INTL("{1} learned {2}!",pkmnname,movename));
					GameDebug.Log($"[Learn move] #{pkmnname} learned #{movename}");
					return;
				}
			}
			do { //loop
				pbDisplayPaused(Game._INTL("{1} is trying to learn {2}.",pkmnname,movename));
				pbDisplayPaused(Game._INTL("But {1} can't learn more than four moves.",pkmnname));
				if (pbDisplayConfirm(Game._INTL("Delete a move to make room for {1}?",movename))) {
					pbDisplayPaused(Game._INTL("Which move should be forgotten?"));
					int forgetmove=(@scene as IPokeBattle_DebugSceneNoGraphics).pbForgetMove(pokemon,move);
					if (forgetmove>=0) {
						string oldmovename=Game._INTL(pokemon.moves[forgetmove].id.ToString(TextScripts.Name));
						pokemon.moves[forgetmove]=new PokemonUnity.Attack.Move(move); // Replaces current/total PP
						if (battler.IsNotNullOrNone())
							battler.moves[forgetmove]=Combat.Move.pbFromPBMove(this,pokemon.moves[forgetmove]); //ToDo: Use ForgetMove Method in Pokemon Class?
							//battler.pokemon.DeleteMoveAtIndex(forgetmove);
						pbDisplayPaused(Game._INTL("1,  2, and... ... ...")); //ToDo: 2sec delay between text
						pbDisplayPaused(Game._INTL("Poof!"));
						pbDisplayPaused(Game._INTL("{1} forgot {2}.",pkmnname,oldmovename));
						pbDisplayPaused(Game._INTL("And..."));
						pbDisplayPaused(Game._INTL("{1} learned {2}!",pkmnname,movename));
						GameDebug.Log($"[Learn move] #{pkmnname} forgot #{oldmovename} and learned #{movename}");
						return;
					}
					else if (pbDisplayConfirm(Game._INTL("Should {1} stop learning {2}?",pkmnname,movename))) {
					pbDisplayPaused(Game._INTL("{1} did not learn {2}.",pkmnname,movename));
					return;
				}
				}
				else if (pbDisplayConfirm(Game._INTL("Should {1} stop learning {2}?",pkmnname,movename))) {
					pbDisplayPaused(Game._INTL("{1} did not learn {2}.",pkmnname,movename));
					return;
				}
			} while(true);
		}
		#endregion

		#region Abilities.
		public virtual void pbOnActiveAll() {
			for (int i = 0; i < battlers.Length; i++) { // Currently unfainted participants will earn EXP even if they faint afterwards
				if (pbIsOpposing(i)) @battlers[i].pbUpdateParticipants();
				if (!pbIsOpposing(i) &&
					(@battlers[i].Item == Items.AMULET_COIN ||
					@battlers[i].Item == Items.LUCK_INCENSE)) @amuletcoin=true;
			}
			for (int i = 0; i < battlers.Length; i++) {
				if (!@battlers[i].isFainted()) {
					if (@battlers[i] is IBattlerShadowPokemon b && b.isShadow() && pbIsOpposing(i)) {
						pbCommonAnimation("Shadow",@battlers[i],null);
						pbDisplay(Game._INTL("Oh!\nA Shadow Pokémon!"));
					}
				}
			}
			// Weather-inducing abilities, Trace, Imposter, etc.
			@usepriority=false;
			IBattler[] priority=pbPriority();
			foreach (var i in priority) {
				i?.pbAbilitiesOnSwitchIn(true);
			}
			// Check forms are correct
			for (int i = 0; i < battlers.Length; i++) {
				if (@battlers[i].isFainted()) continue;
				@battlers[i].pbCheckForm();
			}
		}

		public virtual bool pbOnActiveOne(IBattler pkmn,bool onlyabilities=false,bool moldbreaker=false) {
			if (pkmn.isFainted()) return false;
			if (!onlyabilities) {
				for (int i = 0; i < battlers.Length; i++) { // Currently unfainted participants will earn EXP even if they faint afterwards
					if (pbIsOpposing(i)) @battlers[i].pbUpdateParticipants();
					if (!pbIsOpposing(i) &&
						(@battlers[i].Item == Items.AMULET_COIN ||
						@battlers[i].Item == Items.LUCK_INCENSE)) @amuletcoin=true;
				}
				if (pkmn is IPokemonShadowPokemon p && p.isShadow && pbIsOpposing(pkmn.Index)) {
					pbCommonAnimation("Shadow",pkmn,null);
					pbDisplay(Game._INTL("Oh!\nA Shadow Pokémon!"));
				}
				// Healing Wish
				if (pkmn.effects.HealingWish) {
					GameDebug.Log($"[Lingering effect triggered] #{pkmn.ToString()}'s Healing Wish");
					pbCommonAnimation("HealingWish",pkmn,null);
					pbDisplayPaused(Game._INTL("The healing wish came true for {1}!",pkmn.ToString(true)));
					pkmn.pbRecoverHP(pkmn.TotalHP,true);
					if (pkmn is IBattlerEffect b) b.pbCureStatus(false);
					pkmn.effects.HealingWish=false;
				}
				// Lunar Dance
				if (pkmn.effects.LunarDance) {
					GameDebug.Log($"[Lingering effect triggered] #{pkmn.ToString()}'s Lunar Dance");
					pbCommonAnimation("LunarDance",pkmn,null);
					pbDisplayPaused(Game._INTL("{1} became cloaked in mystical moonlight!",pkmn.ToString()));
					pkmn.pbRecoverHP(pkmn.TotalHP,true);
					if (pkmn is IBattlerEffect b) b.pbCureStatus(false);
					for (int i = 0; i < pkmn.moves.Length; i++) {
						pkmn.moves[i].PP=(byte)pkmn.moves[i].TotalPP;
					}
					pkmn.effects.LunarDance=false;
				}
				// Spikes
				if (pkmn.pbOwnSide.Spikes>0 && !pkmn.isAirborne(moldbreaker)) {
					if (!pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
						GameDebug.Log($"[Entry hazard] #{pkmn.ToString()} triggered Spikes");
						float spikesdiv=new int[] { 8, 6, 4 }[pkmn.pbOwnSide.Spikes-1];
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn,0);
						pkmn.pbReduceHP((int)Math.Floor(pkmn.TotalHP/spikesdiv));
						pbDisplayPaused(Game._INTL("{1} is hurt by the spikes!",pkmn.ToString()));
					}
				}
				if (pkmn.isFainted()) pkmn.pbFaint();
				// Stealth Rock
				if (pkmn.pbOwnSide.StealthRock && !pkmn.isFainted()) {
					if (!pkmn.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
						/*Types atype=Types.ROCK; //|| 0;
						//ToDo: Deal with Type Advantage...
						float eff=atype.GetCombinedEffectiveness(pkmn.Type1,pkmn.Type2,pkmn.effects.Type3);
						if (eff>0) {
							GameDebug.Log($"[Entry hazard] #{pkmn.ToString()} triggered Stealth Rock");
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(pkmn,0);
							pkmn.pbReduceHP((int)Math.Floor((pkmn.TotalHP*eff)/64f));
							pbDisplayPaused(Game._INTL("Pointed stones dug into {1}!",pkmn.ToString()));
						}*/
					}
				}
				if (pkmn.isFainted()) pkmn.pbFaint();
				// Toxic Spikes
				if (pkmn.pbOwnSide.ToxicSpikes>0 && !pkmn.isFainted()) {
					if (!pkmn.isAirborne(moldbreaker)) {
						if (pkmn.pbHasType(Types.POISON)) {
							GameDebug.Log($"[Entry hazard] #{pkmn.ToString()} absorbed Toxic Spikes");
							pkmn.pbOwnSide.ToxicSpikes=0;
							pbDisplayPaused(Game._INTL("{1} absorbed the poison spikes!",pkmn.ToString()));
						}
						else if (pkmn is IBattlerEffect b && b.pbCanPoisonSpikes(moldbreaker)) {
							GameDebug.Log($"[Entry hazard] #{pkmn.ToString()} triggered Toxic Spikes");
							if (pkmn.pbOwnSide.ToxicSpikes==2) {
								b.pbPoison(null,Game._INTL("{1} was badly poisoned by the poison spikes!",pkmn.ToString()),true);
							}
							else {
								b.pbPoison(null,Game._INTL("{1} was poisoned by the poison spikes!",pkmn.ToString()));
							}
						}
					}
				}
				// Sticky Web
				if (pkmn.pbOwnSide.StickyWeb && !pkmn.isFainted() &&
					!pkmn.isAirborne(moldbreaker)) {
					if (pkmn is IBattlerEffect b && b.pbCanReduceStatStage(Stats.SPEED,null,false,null,moldbreaker)) {
						GameDebug.Log($"[Entry hazard] #{pkmn.ToString()} triggered Sticky Web");
						pbDisplayPaused(Game._INTL("{1} was caught in a sticky web!",pkmn.ToString()));
						b.pbReduceStat(Stats.SPEED,1,null,false,null,true,moldbreaker);
					}
				}
			}
			pkmn.pbAbilityCureCheck();
			if (pkmn.isFainted()) {
				pbGainEXP();
				pbJudge(); //      pbSwitch
				return false;
			}
			pkmn.pbAbilitiesOnSwitchIn(true);
			if (!onlyabilities) {
				pkmn.pbCheckForm();
				pkmn.pbBerryCureCheck();
			}
			return true;
		}

		public void pbPrimordialWeather() {
			// End Primordial Sea, Desolate Land, Delta Stream
			bool hasabil=false;
			switch (@weather) {
				case Weather.HEAVYRAIN:
					for (int i = 0; i < battlers.Length; i++) {
						if (@battlers[i].Ability == Abilities.PRIMORDIAL_SEA &&
							!@battlers[i].isFainted()) {
							hasabil=true; break;
						}
						if (!hasabil) {
							@weather=Weather.NONE;
							pbDisplayBrief("The heavy rain has lifted!");
						}
					}
					break;
				case Weather.HARSHSUN:
					for (int i = 0; i < battlers.Length; i++) {
						if (@battlers[i].Ability == Abilities.DESOLATE_LAND &&
							!@battlers[i].isFainted()) {
							hasabil=true; break;
						}
						if (!hasabil) {
							@weather=Weather.NONE;
							pbDisplayBrief("The harsh sunlight faded!");
						}
					}
					break;
				case Weather.STRONGWINDS:
					for (int i = 0; i < battlers.Length; i++) {
						if (@battlers[i].Ability == Abilities.DELTA_STREAM &&
							!@battlers[i].isFainted()) {
							hasabil=true; break;
						}
						if (!hasabil) {
							@weather=Weather.NONE;
							pbDisplayBrief("The mysterious air current has dissipated!");
						}
					}
					break;
			}
		}
		#endregion

		#region Judging
		private void _pbJudgeCheckpoint(IBattler attacker,IBattleMove move=null) {
		}

		public BattleResults pbDecisionOnTime() {
			int count1=0;
			int count2=0;
			int hptotal1=0;
			int hptotal2=0;
			foreach (var i in @party1) {
				if (!i.IsNotNullOrNone()) continue;
				if (i.HP>0 && !i.isEgg) {
					count1+=1;
					hptotal1+=i.HP;
				}
			}
			foreach (var i in @party2) {
				if (!i.IsNotNullOrNone()) continue;
				if (i.HP>0 && !i.isEgg) {
					count2+=1;
					hptotal2+=i.HP;
				}
			}
			if (count1>count2    ) return BattleResults.WON;	// win
			if (count1<count2    ) return BattleResults.LOST;	// loss
			if (hptotal1>hptotal2) return BattleResults.WON;	// win
			if (hptotal1<hptotal2) return BattleResults.LOST;	// loss
			return BattleResults.DRAW;                          // draw;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// Made this and forgot to label it... idr details
		public BattleResults pbDecisionOnTime2() {
			int count1=0;
			int count2=0;
			int hptotal1=0;
			int hptotal2=0;
			foreach (var i in @party1) {
				if (!i.IsNotNullOrNone()) continue;
				if (i.HP>0 && !i.isEgg) {
				count1+=1;
				hptotal1+=(i.HP*100/i.TotalHP);
				}
			}
			if (count1>0) hptotal1/=count1;
			foreach (var i in @party2) {
				if (!i.IsNotNullOrNone()) continue;
				if (i.HP>0 && !i.isEgg) {
				count2+=1;
				hptotal2+=(i.HP*100/i.TotalHP);
				}
			}
			if (count2>0) hptotal2/=count2;
			if (count1>count2    ) return BattleResults.WON;	// win
			if (count1<count2    ) return BattleResults.LOST;	// loss
			if (hptotal1>hptotal2) return BattleResults.WON;	// win
			if (hptotal1<hptotal2) return BattleResults.LOST;	// loss
			return BattleResults.DRAW;                          // draw;
		}

		//private BattleResults _pbDecisionOnDraw() {
		BattleResults IBattle.pbDecisionOnDraw() {
			return BattleResults.DRAW; // draw;
		}

		public void pbJudge() {
			GameDebug.Log($"[Counts: #{pbPokemonCount(@party1)}/#{pbPokemonCount(@party2)}]");
			if (pbAllFainted(@party1) && pbAllFainted(@party2)) {
				@decision=pbDecisionOnDraw(); // Draw
				return;
			}
			if (pbAllFainted(@party1)) {
				@decision=BattleResults.LOST; // Loss
				return;
			}
			if (pbAllFainted(@party2)) {
				@decision=BattleResults.WON; // Win
				return;
			}
		}
		#endregion

		#region Messages and animations.
		/// <summary>
		/// Displays a message on screen, and wait for player input
		/// </summary>
		/// <param name="text"></param>
		public virtual void pbDisplay(string msg) {
			(@scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayMessage(msg);
		}

		public virtual void pbDisplayPaused(string msg) {
			(@scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayPausedMessage(msg);
		}

		/// <summary>
		/// Displays a message on screen,
		/// but will continue without player input after short delay
		/// </summary>
		/// <param name="text"></param>
		public virtual void pbDisplayBrief(string msg) {
			(@scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayMessage(msg,true);
		}

		public virtual bool pbDisplayConfirm(string msg) {
			return (@scene as IPokeBattle_DebugSceneNoGraphics).pbDisplayConfirmMessage(msg);
		}

		public void pbShowCommands(string msg,string[] commands,bool cancancel=true) {
			(@scene as IPokeBattle_DebugSceneNoGraphics).pbShowCommands(msg,commands,cancancel);
		}

		public void pbShowCommands(string msg,string[] commands,int cancancel) {
			(@scene as IPokeBattle_DebugSceneNoGraphics).pbShowCommands(msg,commands,cancancel);
		}

		public void pbAnimation(Moves move,IBattler attacker,IBattler opponent,int hitnum=0) {
			if (@battlescene) {
				(@scene as IPokeBattle_DebugSceneNoGraphics).pbAnimation(move,attacker,opponent,hitnum);
			}
		}

		public void pbCommonAnimation(string name,IBattler attacker,IBattler opponent,int hitnum=0) {
			if (@battlescene) {
				if (@scene is IPokeBattle_Scene s0) s0.pbCommonAnimation(name,attacker,opponent,hitnum);
			}
		}
		#endregion

		#region Battle Core.
		public virtual BattleResults pbStartBattle(bool canlose=false) {
			GameDebug.Log($"");
			GameDebug.Log($"******************************************");
			@decision = BattleResults.InProgress;
			try { 
				pbStartBattleCore(canlose);
			} catch (BattleAbortedException e){ //rescue BattleAbortedException;
				GameDebug.LogError(e.Message);
				GameDebug.LogError(e.StackTrace);
	  
				@decision = BattleResults.ABORTED;
				(@scene as IPokeBattle_DebugSceneNoGraphics).pbEndBattle(@decision);
			}
			return @decision;
		}

		public void pbStartBattleCore(bool canlose) {
			if (!@fullparty1 && @party1.Length>Core.MAXPARTYSIZE) {
				//throw new Exception(new ArgumentError(Game._INTL("Party 1 has more than {1} Pokémon.",Core.MAXPARTYSIZE)));
				GameDebug.LogError(Game._INTL("Party 1 has more than {1} Pokémon.",Core.MAXPARTYSIZE));
				@party1= new IPokemon[Core.MAXPARTYSIZE]; //Fixed error.
				for(int i = 0; i < Core.MAXPARTYSIZE; i++)
					@party1[i] = @party1[i];
			}
			if (!@fullparty2 && @party2.Length>Core.MAXPARTYSIZE) {
				//throw new Exception(new ArgumentError(Game._INTL("Party 2 has more than {1} Pokémon.",Core.MAXPARTYSIZE)));
				GameDebug.LogError(Game._INTL("Party 2 has more than {1} Pokémon.",Core.MAXPARTYSIZE));
				@party2= new IPokemon[Core.MAXPARTYSIZE]; //Fixed error.
				for(int i = 0; i < Core.MAXPARTYSIZE; i++)
					@party2[i] = @party2[i];
			}
			#region Initialize wild Pokémon;
			if (@opponent == null || @opponent.Length == 0) {
				initialize_wild_battle:
				if (@party2.Length==1) {
					if (@doublebattle) {
						//throw new Exception(Game._INTL("Only two wild Pokémon are allowed in double battles"));
						GameDebug.LogError(Game._INTL("Only two wild Pokémon are allowed in double battles"));
						doublebattle = false;
							GameDebug.LogWarning("Changed battle to single.");
					}
					IPokemon wildpoke=@party2[0];
					@battlers[1].pbInitialize(wildpoke,0,false);
					if (@peer is IBattlePeerMultipleForms p) p.pbOnEnteringBattle(this,wildpoke); 
					pbSetSeen(wildpoke);
					(@scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle(this);
					pbDisplayPaused(Game._INTL("Wild {1} appeared!",Game._INTL(wildpoke.Species.ToString(TextScripts.Name)))); //Wild pokemons dont get nicknames
				}
				else if (@party2.Length>1) { //length==2
					if (!@doublebattle) {
						//throw new Exception(Game._INTL("Only one wild Pokémon is allowed in single battles"));
						GameDebug.LogError(Game._INTL("Only one wild Pokémon is allowed in single battles"));
						if (party1.GetBattleCount() > 1) //either set double to true, or remove wild pokemon
						{
							doublebattle = true;
							GameDebug.LogWarning("Changed battle to double.");
						}
						else
						{
							@party2 = new IPokemon[] { @party2.First((x) => x.IsNotNullOrNone()) };
							GameDebug.LogWarning("Removed additional wild pokemon from opposing side.");
							goto initialize_wild_battle;
						}
					}
					@battlers[1].pbInitialize(@party2[0],0,false);
					@battlers[3].pbInitialize(@party2[1],0,false);
					if (@peer is IBattlePeerMultipleForms p0) p0.pbOnEnteringBattle(this,@party2[0]); 
					if (@peer is IBattlePeerMultipleForms p1) p1.pbOnEnteringBattle(this,@party2[1]); 
					pbSetSeen(@party2[0]);
					pbSetSeen(@party2[1]);
					(@scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle(this);
					pbDisplayPaused(Game._INTL("Wild {1} and\r\n{2} appeared!",
						Game._INTL(@party2[0].Species.ToString(TextScripts.Name)),Game._INTL(@party2[1].Species.ToString(TextScripts.Name)))); //Wild pokemons dont get nicknames
				}
				else {
					//throw new Exception(Game._INTL("Only one or two wild Pokémon are allowed"));
					GameDebug.LogError(Game._INTL("Only one or two wild Pokémon are allowed"));
				}
			}
			#endregion
			#region Initialize opponents in double battles;
			else if (@doublebattle) {
				if (@opponent.Length > 0) {
					if (@opponent.Length==1) {
						//@opponent=@opponent[0]; //No changes
					}
					else if (@opponent.Length!=2) {
						//throw new Exception(Game._INTL("Opponents with zero or more than two people are not allowed"));
						GameDebug.LogError(Game._INTL("Opponents with zero or more than two people are not allowed"));
						@opponent= new ITrainer[] { @opponent[0], @opponent[1] }; //Resolved Error
					}
				}
				if (@player.Length > 0) {
					if (@player.Length==1) {
						//@player=@player[0]; //No changes
					}
					else if (@player.Length!=2) {
						//throw new Exception(Game._INTL("Player trainers with zero or more than two people are not allowed"));
						GameDebug.LogError(Game._INTL("Player trainers with zero or more than two people are not allowed"));
						@player= new ITrainer[] { @player[0], @player[1] }; //Resolved Error
					}
				}
				(@scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle(this);
				if (@opponent.Length > 0) {
					pbDisplayPaused(Game._INTL("{1} and {2} want to battle!",@opponent[0].name,@opponent[1].name));
					int sendout1=pbFindNextUnfainted(@party2,0,pbSecondPartyBegin(1));
					if (sendout1<0) GameDebug.LogError(Game._INTL("Opponent 1 has no unfainted Pokémon")); //throw new Exception(Game._INTL("Opponent 1 has no unfainted Pokémon"));
					int sendout2=pbFindNextUnfainted(@party2,pbSecondPartyBegin(1));
					if (sendout2<0) GameDebug.LogError(Game._INTL("Opponent 2 has no unfainted Pokémon")); //throw new Exception(Game._INTL("Opponent 2 has no unfainted Pokémon"));
					@battlers[1].pbInitialize(@party2[sendout1],(sbyte)sendout1,false);
					pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!",@opponent[0].name,@battlers[1].Name));
					pbSendOut(1,@party2[sendout1]);
					@battlers[3].pbInitialize(@party2[sendout2],(sbyte)sendout2,false);
					pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!",@opponent[1].name,@battlers[3].Name));
					pbSendOut(3,@party2[sendout2]);
				}
				else {
					pbDisplayPaused(Game._INTL("{1}\r\nwould like to battle!",@opponent[0].name));
					int sendout1=pbFindNextUnfainted(@party2,0);
					int sendout2=pbFindNextUnfainted(@party2,sendout1+1);
					if (sendout1<0 || sendout2<0) {
						//throw new Exception(Game._INTL("Opponent doesn't have two unfainted Pokémon"));
						GameDebug.LogError(Game._INTL("Opponent doesn't have two unfainted Pokémon"));
					}
					@battlers[1].pbInitialize(@party2[sendout1],(sbyte)sendout1,false);
					@battlers[3].pbInitialize(@party2[sendout2],(sbyte)sendout2,false);
					pbDisplayBrief(Game._INTL("{1} sent\r\nout {2} and {3}!",
						@opponent[0].name,@battlers[1].Name,@battlers[3].Name));
					pbSendOut(1,@party2[sendout1]);
					pbSendOut(3,@party2[sendout2]);
				}
			}
			#endregion
			#region Initialize opponent in single battles
			else {
				int sendout=pbFindNextUnfainted(@party2,0);
				if (sendout<0) GameDebug.LogError(Game._INTL("Trainer has no unfainted Pokémon")); //throw new Exception(Game._INTL("Trainer has no unfainted Pokémon"));
				if (@opponent.Length > 0) {
					if (@opponent.Length!=1) GameDebug.LogError(Game._INTL("Opponent trainer must be only one person in single battles")); //throw new Exception(Game._INTL("Opponent trainer must be only one person in single battles"));
					@opponent=new ITrainer[] { @opponent[0] };
				}
				if (@player.Length > 0) {
					if (@player.Length!=1) GameDebug.LogError(Game._INTL("Player trainer must be only one person in single battles")); //throw new Exception(Game._INTL("Player trainer must be only one person in single battles"));
					@player=new ITrainer[] { @player[0] };
				}
				IPokemon trainerpoke=@party2[sendout];
				(@scene as IPokeBattle_DebugSceneNoGraphics).pbStartBattle(this);
				pbDisplayPaused(Game._INTL("{1}\r\nwould like to battle!",@opponent[0].name));
				@battlers[1].pbInitialize(trainerpoke,(sbyte)sendout,false);
				pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}!",@opponent[0].name,@battlers[1].Name));
				pbSendOut(1,trainerpoke);
			}
			#endregion
			#region Initialize players in double battles
			if (@doublebattle) {
				int sendout1 = 0; int sendout2 = 0;
				if (@player.Length > 0) {
					sendout1=pbFindNextUnfainted(@party1,0,pbSecondPartyBegin(0));
					if (sendout1<0) GameDebug.LogError(Game._INTL("Player 1 has no unfainted Pokémon")); //throw new Exception(Game._INTL("Player 1 has no unfainted Pokémon"));
					sendout2=pbFindNextUnfainted(@party1,pbSecondPartyBegin(0));
					if (sendout2<0) GameDebug.LogError(Game._INTL("Player 2 has no unfainted Pokémon")); //throw new Exception(Game._INTL("Player 2 has no unfainted Pokémon"));
					@battlers[0].pbInitialize(@party1[sendout1],(sbyte)sendout1,false);
					@battlers[2].pbInitialize(@party1[sendout2],(sbyte)sendout2,false);
					pbDisplayBrief(Game._INTL("{1} sent\r\nout {2}! Go! {3}!",
						@player[1].name,@battlers[2].Name,@battlers[0].Name));
					pbSetSeen(@party1[sendout1]);
					pbSetSeen(@party1[sendout2]);
				}
				else {
					sendout1=pbFindNextUnfainted(@party1,0);
					sendout2=pbFindNextUnfainted(@party1,sendout1+1);
					if (sendout1<0 || sendout2<0) {
						//throw new Exception(Game._INTL("Player doesn't have two unfainted Pokémon"));
						GameDebug.LogError(Game._INTL("Player doesn't have two unfainted Pokémon"));
					}
					@battlers[0].pbInitialize(@party1[sendout1],(sbyte)sendout1,false);
					@battlers[2].pbInitialize(@party1[sendout2],(sbyte)sendout2,false);
					pbDisplayBrief(Game._INTL("Go! {1} and {2}!",@battlers[0].Name,@battlers[2].Name));
				}
				pbSendOut(0,@party1[sendout1]);
				pbSendOut(2,@party1[sendout2]);
			}
			#endregion
			#region Initialize player in single battles
			else {
				int sendout=pbFindNextUnfainted(@party1,0);
				if (sendout<0) {
					//throw new Exception(Game._INTL("Player has no unfainted Pokémon"));
					GameDebug.LogError(Game._INTL("Player has no unfainted Pokémon"));
				}
				@battlers[0].pbInitialize(@party1[sendout],(sbyte)sendout,false);
				pbDisplayBrief(Game._INTL("Go! {1}!",@battlers[0].Name));
				pbSendOut(0,@party1[sendout]);
			}
			#endregion
			#region Initialize battle
			if (@weather==Weather.SUNNYDAY) {
				pbCommonAnimation("Sunny",null,null);
				pbDisplay(Game._INTL("The sunlight is strong."));
			}
			else if (@weather==Weather.RAINDANCE) {
				pbCommonAnimation("Rain",null,null);
				pbDisplay(Game._INTL("It is raining."));
			}
			else if (@weather==Weather.SANDSTORM) {
				pbCommonAnimation("Sandstorm",null,null);
				pbDisplay(Game._INTL("A sandstorm is raging."));
			}
			else if (@weather==Weather.HAIL) {
				pbCommonAnimation("Hail",null,null);
				pbDisplay(Game._INTL("Hail is falling."));
			}
			else if (@weather==Weather.HEAVYRAIN) {
				pbCommonAnimation("HeavyRain",null,null);
				pbDisplay(Game._INTL("It is raining heavily."));
			}
			else if (@weather==Weather.HARSHSUN) {
				pbCommonAnimation("HarshSun",null,null);
				pbDisplay(Game._INTL("The sunlight is extremely harsh."));
			}
			else if (@weather==Weather.STRONGWINDS) {
				pbCommonAnimation("StrongWinds",null,null);
				pbDisplay(Game._INTL("The wind is strong."));
			}
			pbOnActiveAll();   // Abilities
			@turncount=0;
			#endregion
			#region Battle-Sequence Loop
			do {   // Now begin the battle loop
				GameDebug.Log($"");
				GameDebug.Log($"***Round #{@turncount+1}***");
				if (@debug && @turncount>=100) {
					@decision=pbDecisionOnTime();
					GameDebug.Log($"");
					GameDebug.Log($"***Undecided after 100 rounds, aborting***");
					pbAbort();
					break;
				}
				//PBDebug.logonerr{
					pbCommandPhase();
				//}
				if (@decision>0) break;
				//PBDebug.logonerr{
					pbAttackPhase();
				//}
				if (@decision>0) break;
				//PBDebug.logonerr{
					pbEndOfRoundPhase();
				//}
				if (@decision>0) break;
				@turncount+=1;
			} while (this.decision == BattleResults.InProgress); //while (true);
			#endregion
			pbEndOfBattle(canlose);
		}
		#endregion

		#region Command phase.
		public virtual MenuCommands pbCommandMenu(int i) {
			return (MenuCommands)(@scene as IPokeBattle_SceneNonInteractive).pbCommandMenu(i);
		}

		public virtual KeyValuePair<Items,int?> pbItemMenu(int i) {
			//return (@scene as IPokeBattle_SceneNonInteractive).pbItemMenu(i);
			//Returns from UI the Selected item, and the target for the item's usage
			return new KeyValuePair<Items,int?>((@scene as IPokeBattle_SceneNonInteractive).pbItemMenu(i), null);
		}

		public virtual bool pbAutoFightMenu(int i) {
			return false;
		}

		public virtual void pbCommandPhase() {
			if (@scene is IPokeBattle_DebugSceneNoGraphics s0) s0.pbBeginCommandPhase();
			if (@scene is IPokeBattle_Scene s1) s1.pbResetCommandIndices();
			for (int i = 0; i < battlers.Length; i++) {   // Reset choices if commands can be shown
				@battlers[i].effects.SkipTurn=false;
				if (CanShowCommands(i) || @battlers[i].isFainted()) {
					//@choices[i][0]=0;
					//@choices[i][1]=0;
					//@choices[i][2]=null;
					//@choices[i][3]=-1;
					@choices[i]=new Choice(ChoiceAction.NoAction);
				}
				else {
					if (@doublebattle && !pbIsDoubleBattler(i)) {
						GameDebug.Log($"[Reusing commands] #{@battlers[i].ToString(true)}");
					}
				}
			}
			// Reset choices to perform Mega Evolution if it wasn't done somehow
			for (int i = 0; i < sides.Length; i++) {
				for (int j = 0; j < @megaEvolution[i].Length; j++) {
					if (@megaEvolution[i][j]>=0) @megaEvolution[i][j]=-1;
				}
			}
			for (int i = 0; i < battlers.Length; i++) {
				if (@decision==0) break;
				if (@choices[i].Action!=0) continue; //@choices[i][0]!=0
				if (!pbOwnedByPlayer(i) || @controlPlayer) {
					if (!@battlers[i].isFainted() && CanShowCommands(i)) {
						(@scene as IPokeBattle_SceneNonInteractive).pbChooseEnemyCommand(i);
					}
				}
				else {
					bool commandDone=false;
					//bool commandEnd=false;
					if (CanShowCommands(i)) {
						do { //loop
							MenuCommands cmd=pbCommandMenu(i);
							if (cmd==MenuCommands.FIGHT) { // Fight
								if (CanShowFightMenu(i)) {
									if (pbAutoFightMenu(i)) commandDone=true;
									do {
										int index=(@scene as IPokeBattle_SceneNonInteractive).pbFightMenu(i);
										if (index<0) {
											int side=(pbIsOpposing(i)) ? 1 : 0;
											int owner=pbGetOwnerIndex(i);
											if (@megaEvolution[side][owner]==i) {
												@megaEvolution[side][owner]=-1;
											}
											break;
										}
										if (!pbRegisterMove(i,index)) continue;
										if (@doublebattle) {
											IBattleMove thismove=@battlers[i].moves[index];
											//Attack.Target target=@battlers[i].pbTarget(thismove);
											Attack.Data.Targets targets=@battlers[i].pbTarget(thismove);
											//if (target==Attack.Target.SingleNonUser) {            // single non-user
											if (targets==Attack.Data.Targets.SELECTED_POKEMON ||	// single non-user
												targets==Attack.Data.Targets.SELECTED_POKEMON_ME_FIRST) {
												int target=(@scene as IPokeBattle_SceneNonInteractive).pbChooseTarget(i,targets);
												if (target<0) continue;
												pbRegisterTarget(i,target);
											}
											//else if (target==Attack.Target.UserOrPartner) {       // Acupressure
											else if (targets==Attack.Data.Targets.USER_OR_ALLY) {   // Acupressure
												int target=(@scene as IPokeBattle_SceneNonInteractive).pbChooseTarget(i,targets);
												if (target<0 || (target%2)==1) continue; //no choice or enemy
												pbRegisterTarget(i,target);
											}
											//ToDo: Else... random selected pokemon (not target select, but still register target)
										}
										commandDone=true;
									} while (!commandDone);
								}
								else {
									pbAutoChooseMove(i);
									commandDone=true;
								}
							}
							else if (cmd!=MenuCommands.FIGHT && @battlers[i].effects.SkyDrop) {
								pbDisplay(Game._INTL("Sky Drop won't let {1} go!",@battlers[i].ToString(true)));
							}
							else if (cmd==MenuCommands.BAG) { // Bag
								if (!@internalbattle) {
									if (pbOwnedByPlayer(i)) {
										pbDisplay(Game._INTL("Items can't be used here."));
									}
								}
								else {
									KeyValuePair<Items,int?> item=pbItemMenu(i);
									if (item.Key>0) {
										if (pbRegisterItem(i,(Items)item.Key,item.Value)) {
											commandDone=true;
										}
									}
								}
							}
							else if (cmd==MenuCommands.POKEMON) { // Pokémon
								int pkmn=pbSwitchPlayer(i,false,true);
								if (pkmn>=0) {
									if (pbRegisterSwitch(i,pkmn)) commandDone=true;
								}
							}
							else if (cmd==MenuCommands.RUN) {   // Run
								int run=pbRun(i);
								if (run>0) {
									commandDone=true;
									return;
								}
								else if (run<0) {
									commandDone=true;
									int side=(pbIsOpposing(i)) ? 1 : 0;
									int owner=pbGetOwnerIndex(i);
									if (@megaEvolution[side][owner]==i) {
										@megaEvolution[side][owner]=-1;
									}
								}
							}
							else if (cmd==MenuCommands.CALL) {   // Call
								IBattler thispkmn=@battlers[i];
								//@choices[i][0]=4;   // "Call Pokémon"
								//@choices[i][1]=0;
								//@choices[i][2]=null;
								@choices[i]=new Choice(ChoiceAction.CallPokemon);
								int side=(pbIsOpposing(i)) ? 1 : 0;
								int owner=pbGetOwnerIndex(i);
								if (@megaEvolution[side][owner]==i) {
									@megaEvolution[side][owner]=-1;
								}
								commandDone=true;
							}
							else if (cmd==MenuCommands.CANCEL) {   // Go back to first battler's choice
								if (@megaEvolution[0][0]>=0) @megaEvolution[0][0]=-1;
								if (@megaEvolution[1][0]>=0) @megaEvolution[1][0]=-1;
								// Restore the item the player's first Pokémon was due to use
								//if (@choices[0][0]==3 && Game.GameData.Bag && Game.GameData.Bag.pbCanStore(@choices[0][1])) {
								if (@choices[0].Action==ChoiceAction.UseItem && Game.GameData.Bag != null && Game.GameData.Bag.pbCanStore((Items)@choices[0].Index)) {
									Game.GameData.Bag.pbStoreItem((Items)@choices[0].Index); //@choices[0][1]
								}
								pbCommandPhase();
								return;
							}
							if (commandDone) break;
						} while (true);
					}
				}
			}
		}
		#endregion

		#region Attack phase.
		public void pbAttackPhase() {
			if (@scene is IPokeBattle_DebugSceneNoGraphics s0) s0.pbBeginAttackPhase();
			for (int i = 0; i < battlers.Length; i++) {
				@successStates[i].Clear();
				if (@choices[i].Action!= ChoiceAction.UseMove && @choices[i].Action!=ChoiceAction.SwitchPokemon) {
					@battlers[i].effects.DestinyBond=false;
					@battlers[i].effects.Grudge=false;
				}
				if (!@battlers[i].isFainted()) @battlers[i].turncount+=1;
				if (!pbChoseMove(i,Moves.RAGE)) @battlers[i].effects.Rage=false;
			}
			// Calculate priority at this time
			@usepriority=false;
			priority=pbPriority(false,true);
			// Mega Evolution
			List<int> megaevolved=new List<int>();
			foreach (var i in priority) {
				if (@choices[i.Index].Action==ChoiceAction.UseMove && !i.effects.SkipTurn) {
					int side=(pbIsOpposing(i.Index)) ? 1 : 0;
					int owner=pbGetOwnerIndex(i.Index);
					if (@megaEvolution[side][owner]==i.Index) { //@megaEvolution[side][owner]
						//pbMegaEvolve(i.Index);
						megaevolved.Add(i.Index);
					}
				}
			}
			if (megaevolved.Count>0) {
				foreach (var i in priority) {
					if (megaevolved.Contains(i.Index)) i.pbAbilitiesOnSwitchIn(true);
				}
			}
			// Call at Pokémon
			foreach (var i in priority) {
				if (@choices[i.Index].Action==ChoiceAction.CallPokemon && !i.effects.SkipTurn) {
					pbCall(i.Index);
				}
			}
			// Switch out Pokémon
			@switching=true;
			List<int> switched=new List<int>();
			foreach (var i in priority) {
				if (@choices[i.Index].Action==ChoiceAction.SwitchPokemon && !i.effects.SkipTurn) {
					int index=@choices[i.Index].Index; // party position of Pokémon to switch to
					int newpokename=index;
					if (pbParty(i.Index)[index].Ability == Abilities.ILLUSION) {
						newpokename=pbGetLastPokeInTeam(i.Index);
					}
					this.lastMoveUser=i.Index;
					if (!pbOwnedByPlayer(i.Index)) {
						ITrainer owner=pbGetOwner(i.Index);
						pbDisplayBrief(Game._INTL("{1} withdrew {2}!",owner.name,i.Name));
						GameDebug.Log($"[Withdrew Pokémon] Opponent withdrew #{i.ToString(true)}");
					}
					else {
						pbDisplayBrief(Game._INTL("{1}, that's enough!\r\nCome back!",i.Name));
						GameDebug.Log($"[Withdrew Pokémon] Player withdrew #{i.ToString(true)}");
					}
					foreach (var j in priority) {
						if (!i.pbIsOpposing(j.Index)) continue;
						// if Pursuit and this target ("i") was chosen
						if (pbChoseMoveFunctionCode(j.Index,Attack.Data.Effects.x081) && // Pursuit
							!j.hasMovedThisRound()) {
							if (j.Status!=Status.SLEEP && j.Status!=Status.FROZEN &&
								!j.effects.SkyDrop &&
								(!j.hasWorkingAbility(Abilities.TRUANT) || !j.effects.Truant)) {
								//@choices[j.Index].Target=i.Index; // Make sure to target the switching Pokémon
								@choices[j.Index]=new Choice(@choices[j.Index].Action, @choices[j.Index].Index, @choices[j.Index].Move, target: i.Index); // Make sure to target the switching Pokémon
								j.pbUseMove(@choices[j.Index]); // This calls pbGainEXP as appropriate
								j.effects.Pursuit=true;
								@switching=false;
								if (@decision>0) return;
							}
						}
						if (i.isFainted()) break;
					}
					if (!pbRecallAndReplace(i.Index,index,newpokename)) {
						// If a forced switch somehow occurs here in single battles
						// the attack phase now ends
						if (!@doublebattle) {
							@switching=false;
							return;
						}
					}
					else {
						switched.Add(i.Index);
					}
				}
			}
			if (switched.Count>0) {
				foreach (var i in priority) {
					if (switched.Contains(i.Index)) i.pbAbilitiesOnSwitchIn(true);
				}
			}
			@switching=false;
			// Use items
			foreach (var i in priority) {
				if (@choices[i.Index].Action== ChoiceAction.UseItem && !i.effects.SkipTurn) {
					if (pbIsOpposing(i.Index)) {
						// Opponent use item
						pbEnemyUseItem((Items)@choices[i.Index].Index,i);
					}
					else {
						// Player use item
						Items item=(Items)@choices[i.Index].Index;
						if (item>0) {
							//usetype=ItemData[item][Core.ITEMBATTLEUSE]; ToDo
							int usetype=0;//Kernal.ItemData[item].Flags.[Core.ITEMBATTLEUSE]; //AIs can use items?
							if (usetype==1 || usetype==3) {
								if (@choices[i.Index].Target>=0) { //@choices[i.Index][2]
									pbUseItemOnPokemon(item,@choices[i.Index].Target,i,@scene);
								}
							}
							else if (usetype==2 || usetype==4) {
								if (!ItemHandlers.hasUseInBattle(item)) { // Poké Ball/Poké Doll used already
									pbUseItemOnBattler(item,@choices[i.Index].Target,i,@scene);
								}
							}
						}
					}
				}
			}
			// Use attacks
			foreach (var i in priority) {
				if (i.effects.SkipTurn) continue;
				if (pbChoseMoveFunctionCode(i.Index,Attack.Data.Effects.x0AB)) { // Focus Punch
					pbCommonAnimation("FocusPunch",i,null);
					pbDisplay(Game._INTL("{1} is tightening its focus!",i.ToString()));
				}
			}
			int n = 0; do { //10.times
				// Forced to go next
				bool advance=false;
				foreach (var i in priority) {
					if (!i.effects.MoveNext) continue;
					if (i.hasMovedThisRound() || i.effects.SkipTurn) continue;
					advance=i.pbProcessTurn(@choices[i.Index]);
					if (advance) break;
				}
				if (@decision>0) return;
				if (advance) continue;
				// Regular priority order
				foreach (var i in priority) {
					if (i.effects.Quash) continue;
					if (i.hasMovedThisRound() || i.effects.SkipTurn) continue;
					advance=i.pbProcessTurn(@choices[i.Index]);
					if (advance) break;
				}
				if (@decision>0) return;
				if (advance) continue;
				// Quashed
				foreach (var i in priority) {
					if (!i.effects.Quash) continue;
					if (i.hasMovedThisRound() || i.effects.SkipTurn) continue;
					advance=i.pbProcessTurn(@choices[i.Index]);
					if (advance) break;
				}
				if (@decision>0) return;
				if (advance) continue;
				// Check for all done
				foreach (var i in priority) {
					if (@choices[i.Index].Action== ChoiceAction.UseMove && !i.hasMovedThisRound() &&
						!i.effects.SkipTurn) advance=true;
					if (advance) break;
				}
				if (advance) continue;
				n++;//break;
			} while (n < 10);
			if (Game.GameData is IGameField f) f.pbWait(20);
		}
		#endregion

		#region End of round.
		//private void _pbEndOfRoundPhase() {
		void IBattle.pbEndOfRoundPhase() {
			GameDebug.Log($"[End of round]");
			for (int i = 0; i < battlers.Length; i++) {
				@battlers[i].effects.Electrify=false;
				@battlers[i].effects.Endure=false;
				@battlers[i].effects.FirstPledge=0;
				if (@battlers[i].effects.HyperBeam>0) @battlers[i].effects.HyperBeam-=1;
				@battlers[i].effects.KingsShield=false;
				@battlers[i].effects.LifeOrb=false;
				@battlers[i].effects.MoveNext=false;
				@battlers[i].effects.Powder=false;
				@battlers[i].effects.Protect=false;
				@battlers[i].effects.ProtectNegation=false;
				@battlers[i].effects.Quash=false;
				@battlers[i].effects.Roost=false;
				@battlers[i].effects.SpikyShield=false;
			}
			@usepriority=false;  // recalculate priority
			priority=pbPriority(true); // Ignoring Quick Claw here
			// Weather
			switch (@weather) {
				case Weather.SUNNYDAY:
					if (@weatherduration>0) @weatherduration=@weatherduration-1;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The sunlight faded."));
						@weather=0;
						GameDebug.Log($"[End of effect] Sunlight weather ended");
					}
					else {
						pbCommonAnimation("Sunny",null,null);
						pbDisplay(Game._INTL("The sunlight is strong."));
						if (pbWeather==Weather.SUNNYDAY) {
							foreach (var i in priority) {
								if (i.hasWorkingAbility(Abilities.SOLAR_POWER)) {
									GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Solar Power");
									(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
									i.pbReduceHP((int)Math.Floor(i.TotalHP/8f));
									pbDisplay(Game._INTL("{1} was hurt by the sunlight!",i.ToString()));
									if (i.isFainted()) {
										if (!i.pbFaint()) return;
									}
								}
							}
						}
					}
					break;
				case Weather.RAINDANCE:
					if (@weatherduration>0) @weatherduration=@weatherduration-1;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The rain stopped."));
						@weather=0;
						GameDebug.Log($"[End of effect] Rain weather ended");
					}
					else {
						pbCommonAnimation("Rain",null,null);
						pbDisplay(Game._INTL("Rain continues to fall."));
					}
					break;
				case Weather.SANDSTORM:
					if (@weatherduration>0) @weatherduration=@weatherduration-1;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The sandstorm subsided."));
						@weather=0;
						GameDebug.Log($"[End of effect] Sandstorm weather ended");
					}
					else {
						pbCommonAnimation("Sandstorm",null,null);
						pbDisplay(Game._INTL("The sandstorm rages."));
						if (pbWeather==Weather.SANDSTORM) {
							GameDebug.Log($"[Lingering effect triggered] Sandstorm weather damage");
							foreach (var i in priority) {
								if (i.isFainted()) continue;
								if (!i.pbHasType(Types.GROUND) && !i.pbHasType(Types.ROCK) && !i.pbHasType(Types.STEEL) &&
									!i.hasWorkingAbility(Abilities.SAND_VEIL) &&
									!i.hasWorkingAbility(Abilities.SAND_RUSH) &&
									!i.hasWorkingAbility(Abilities.SAND_FORCE) &&
									!i.hasWorkingAbility(Abilities.MAGIC_GUARD) &&
									!i.hasWorkingAbility(Abilities.OVERCOAT) &&
									!i.hasWorkingItem(Items.SAFETY_GOGGLES) &&
									!new Attack.Data.Effects[] {
										Attack.Data.Effects.x101, // Dig
										Attack.Data.Effects.x100  // Dive
									}.Contains(Kernal.MoveData[i.effects.TwoTurnAttack].Effect)) {
									(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
									i.pbReduceHP((int)Math.Floor(i.TotalHP/16f));
									pbDisplay(Game._INTL("{1} is buffeted by the sandstorm!",i.ToString()));
									if (i.isFainted()) {
										if (!i.pbFaint()) return;
									}
								}
							}
						}
					}
					break;
				case Weather.HAIL:
					if (@weatherduration>0) @weatherduration=@weatherduration-1;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The hail stopped."));
						@weather=0;
						GameDebug.Log($"[End of effect] Hail weather ended");
					}
					else {
						pbCommonAnimation("Hail",null,null);
						pbDisplay(Game._INTL("Hail continues to fall."));
						if (pbWeather==Weather.HAIL) {
							GameDebug.Log($"[Lingering effect triggered] Hail weather damage");
							foreach (var i in priority) {
								if (i.isFainted()) continue;
								if (!i.pbHasType(Types.ICE) &&
									!i.hasWorkingAbility(Abilities.ICE_BODY) &&
									!i.hasWorkingAbility(Abilities.SNOW_CLOAK) &&
									!i.hasWorkingAbility(Abilities.MAGIC_GUARD) &&
									!i.hasWorkingAbility(Abilities.OVERCOAT) &&
									!i.hasWorkingItem(Items.SAFETY_GOGGLES) &&
									!new int[] { 0xCA,0xCB }.Contains((int)Kernal.MoveData[i.effects.TwoTurnAttack].Effect)) { // Dig, Dive
									(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
									i.pbReduceHP((int)Math.Floor(i.TotalHP/16f));
									pbDisplay(Game._INTL("{1} is buffeted by the hail!",i.ToString()));
									if (i.isFainted()) {
										if (!i.pbFaint()) return;
									}
								}
							}
						}
					}
					break;
				case Weather.HEAVYRAIN:
					bool hasabil=false;
					for (int i = 0; i < battlers.Length; i++) {
						if (@battlers[i].Ability == Abilities.PRIMORDIAL_SEA && !@battlers[i].isFainted()) {
							hasabil=true; break;
						}
					}
					if (!hasabil) @weatherduration=0;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The heavy rain stopped."));
						@weather=0;
						GameDebug.Log($"[End of effect] Primordial Sea's rain weather ended");
					}
					else {
						pbCommonAnimation("HeavyRain",null,null);
						pbDisplay(Game._INTL("It is raining heavily."));
					}
					break;
				case Weather.HARSHSUN:
					hasabil=false;
					for (int i = 0; i < battlers.Length; i++) {
						if (@battlers[i].Ability == Abilities.DESOLATE_LAND && !@battlers[i].isFainted()) {
							hasabil=true; break;
						}
					}
					if (!hasabil) @weatherduration=0;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The harsh sunlight faded."));
						@weather=0;
						GameDebug.Log($"[End of effect] Desolate Land's sunlight weather ended");
					}
					else {
						pbCommonAnimation("HarshSun",null,null);
						pbDisplay(Game._INTL("The sunlight is extremely harsh."));
						if (pbWeather==Weather.HARSHSUN) {
							foreach (var i in priority) {
								if (i.hasWorkingAbility(Abilities.SOLAR_POWER)) {
									GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Solar Power");
									(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
									i.pbReduceHP((int)Math.Floor(i.TotalHP/8f));
									pbDisplay(Game._INTL("{1} was hurt by the sunlight!",i.ToString()));
									if (i.isFainted()) {
										if (!i.pbFaint()) return;
									}
								}
							}
						}
					}
					break;
				case Weather.STRONGWINDS:
					hasabil=false;
					for (int i = 0; i < battlers.Length; i++) {
						if (@battlers[i].Ability == Abilities.DELTA_STREAM && !@battlers[i].isFainted()) {
							hasabil=true; break;
						}
					}
					if (!hasabil) @weatherduration=0;
					if (@weatherduration==0) {
						pbDisplay(Game._INTL("The air current subsided."));
						@weather=Weather.NONE;
						GameDebug.Log($"[End of effect] Delta Stream's wind weather ended");
					}
					else {
						pbCommonAnimation("StrongWinds",null,null);
						pbDisplay(Game._INTL("The wind is strong."));
					}
					break;
			}
			// Shadow Sky weather
			if (@weather == Weather.SHADOWSKY) {
				if (@weatherduration>0) @weatherduration=@weatherduration-1;
				if (@weatherduration==0) {
					pbDisplay(Game._INTL("The shadow sky faded."));
					@weather=Weather.NONE;
					GameDebug.Log($"[End of effect] Shadow Sky weather ended");
				}
				else {
					pbCommonAnimation("ShadowSky",null,null);
					pbDisplay(Game._INTL("The shadow sky continues."));
					if (pbWeather == Weather.SHADOWSKY) {
						GameDebug.Log($"[Lingering effect triggered] Shadow Sky weather damage");
						foreach (var i in priority) {
							if (i.isFainted()) continue;
							if (i is IBattlerShadowPokemon s && !s.isShadow()) {
								(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
								i.pbReduceHP((int)Math.Floor(i.TotalHP/16f));
								pbDisplay(Game._INTL("{1} was hurt by the shadow sky!",i.ToString()));
								if (i.isFainted()) {
									if (!i.pbFaint()) return;
								}
							}
						}
					}
				}
			}
			// Future Sight/Doom Desire
			foreach (IBattler i in battlers) {   // not priority
				if (i.isFainted()) continue;
				if (i.effects.FutureSight>0) {
					i.effects.FutureSight-=1;
					if (i.effects.FutureSight==0) {
						Moves move=i.effects.FutureSightMove;
						GameDebug.Log($"[Lingering effect triggered] #{Game._INTL(move.ToString(TextScripts.Name))} struck #{i.ToString(true)}");
						pbDisplay(Game._INTL("{1} took the {2} attack!",i.ToString(),Game._INTL(move.ToString(TextScripts.Name))));
						IBattler moveuser=null;
						foreach (var j in battlers) {
						if (j.pbIsOpposing(i.effects.FutureSightUserPos)) continue;
							if (j.pokemonIndex==i.effects.FutureSightUser && !j.isFainted()) {
								moveuser=j; break;
							}
						}
						if (!moveuser.IsNotNullOrNone()) {
							IPokemon[] party=pbParty(i.effects.FutureSightUserPos);
							if (party[i.effects.FutureSightUser].HP>0) {
								moveuser=new Pokemon(this,(sbyte)i.effects.FutureSightUserPos);
								moveuser.pbInitPokemon(party[i.effects.FutureSightUser],
													(sbyte)i.effects.FutureSightUser);
							}
						}
						if (!moveuser.IsNotNullOrNone()) {
							pbDisplay(Game._INTL("But it failed!"));
						}
						else {
							@futuresight=true;
							moveuser.pbUseMoveSimple(move,-1,i.Index);
							@futuresight=false;
						}
						i.effects.FutureSight=0;
						i.effects.FutureSightMove=0;
						i.effects.FutureSightUser=-1;
						i.effects.FutureSightUserPos=-1;
						if (i.isFainted()) {
							if (!i.pbFaint()) return;
							continue;
						}
					}
				}
			}
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				// Rain Dish
				if (i.hasWorkingAbility(Abilities.RAIN_DISH) &&
					(pbWeather==Weather.RAINDANCE ||
					pbWeather==Weather.HEAVYRAIN)) {
					GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Rain Dish");
					int hpgain=i.pbRecoverHP((int)Math.Floor(i.TotalHP/16f),true);
					if (hpgain>0) pbDisplay(Game._INTL("{1}'s {2} restored its HP a little!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
				}
				// Dry Skin
				if (i.hasWorkingAbility(Abilities.DRY_SKIN)) {
					if (pbWeather==Weather.RAINDANCE ||
						pbWeather==Weather.HEAVYRAIN) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Dry Skin (in rain)");
						int hpgain=i.pbRecoverHP((int)Math.Floor(i.TotalHP/8f),true);
						if (hpgain>0) pbDisplay(Game._INTL("{1}'s {2} was healed by the rain!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
					}
					else if (pbWeather==Weather.SUNNYDAY ||
							pbWeather==Weather.HARSHSUN) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Dry Skin (in sun)");
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
						int hploss=i.pbReduceHP((int)Math.Floor(i.TotalHP/8f));
						if (hploss>0) pbDisplay(Game._INTL("{1}'s {2} was hurt by the sunlight!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
					}
				}
				// Ice Body
				if (i.hasWorkingAbility(Abilities.ICE_BODY) && pbWeather==Weather.HAIL) {
					GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Ice Body");
					int hpgain=i.pbRecoverHP((int)Math.Floor(i.TotalHP/16f),true);
					if (hpgain>0) pbDisplay(Game._INTL("{1}'s {2} restored its HP a little!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
				}
			}
			// Wish
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Wish>0) {
					i.effects.Wish-=1;
					if (i.effects.Wish==0) {
						GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Wish");
						int hpgain=i.pbRecoverHP(i.effects.WishAmount,true);
						if (hpgain>0) {
							string wishmaker=ToString(i.Index,i.effects.WishMaker);
							pbDisplay(Game._INTL("{1}'s wish came true!",wishmaker));
						}
					}
				}
			}
			// Fire Pledge + Grass Pledge combination damage
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].SeaOfFire>0 &&
					pbWeather!=Weather.RAINDANCE &&
					pbWeather!=Weather.HEAVYRAIN) {
					if (i==0) pbCommonAnimation("SeaOfFire",null,null);     //@battle.
					if (i==1) pbCommonAnimation("SeaOfFireOpp",null,null);  //@battle.
					foreach (var j in priority) {
						if ((j.Index&1)!=i) continue;
						if (j.pbHasType(Types.FIRE) || j.hasWorkingAbility(Abilities.MAGIC_GUARD)) continue;
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(j,0);
						int hploss=j.pbReduceHP((int)Math.Floor(j.TotalHP/8f));
						if (hploss>0) pbDisplay(Game._INTL("{1} is hurt by the sea of fire!",j.ToString()));
						if (j.isFainted()) {
							if (!j.pbFaint()) return;
						}
					}
				}
			}
			foreach (IBattler i in priority) {
				if (i.isFainted()) continue;
				// Shed Skin, Hydration
				if ((i.hasWorkingAbility(Abilities.SHED_SKIN) && pbRandom(10)<3) ||
					(i.hasWorkingAbility(Abilities.HYDRATION) && (pbWeather==Weather.RAINDANCE ||
					pbWeather==Weather.HEAVYRAIN))) {
					if (i.Status>0) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s #{Game._INTL(i.Ability.ToString(TextScripts.Name))}");
						Status s=i.Status;
						if (i is IBattlerEffect b) b.pbCureStatus(false);
						switch (s) {
							case Status.SLEEP:
								pbDisplay(Game._INTL("{1}'s {2} cured its sleep problem!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.POISON:
								pbDisplay(Game._INTL("{1}'s {2} cured its poison problem!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.BURN:
								pbDisplay(Game._INTL("{1}'s {2} healed its burn!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.PARALYSIS:
								pbDisplay(Game._INTL("{1}'s {2} cured its paralysis!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.FROZEN:
								pbDisplay(Game._INTL("{1}'s {2} thawed it out!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
						}
					}
				}
				// Healer
				if (i.hasWorkingAbility(Abilities.HEALER) && pbRandom(10)<3) {
					IBattler partner=i.pbPartner;
					if (partner.IsNotNullOrNone() && partner.Status>0) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s #{Game._INTL(i.Ability.ToString(TextScripts.Name))}");
						Status s=partner.Status;
						if (partner is IBattlerEffect b) b.pbCureStatus(false);
						switch (s) {
							case Status.SLEEP:
								pbDisplay(Game._INTL("{1}'s {2} cured its partner's sleep problem!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.POISON:
								pbDisplay(Game._INTL("{1}'s {2} cured its partner's poison problem!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.BURN:
								pbDisplay(Game._INTL("{1}'s {2} healed its partner's burn!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.PARALYSIS:
								pbDisplay(Game._INTL("{1}'s {2} cured its partner's paralysis!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
							case Status.FROZEN:
								pbDisplay(Game._INTL("{1}'s {2} thawed its partner out!",i.ToString(),Game._INTL(i.Ability.ToString(TextScripts.Name))));
								break;
						}
					}
				}
			}
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				// Grassy Terrain (healing)
				if (@field.GrassyTerrain>0 && !i.isAirborne()) {
					int hpgain=i.pbRecoverHP((int)Math.Floor(i.TotalHP/16f),true);
					if (hpgain>0) pbDisplay(Game._INTL("{1}'s HP was restored.",i.ToString()));
				}
				// Held berries/Leftovers/Black Sludge
				i.pbBerryCureCheck(true);
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
				}
			}
			// Aqua Ring
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.AquaRing) {
					GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Aqua Ring");
					int hpgain=(int)Math.Floor(i.TotalHP/16f);
					if (i.hasWorkingItem(Items.BIG_ROOT)) hpgain=(int)Math.Floor(hpgain*1.3);
					hpgain=i.pbRecoverHP(hpgain,true);
					if (hpgain>0) pbDisplay(Game._INTL("Aqua Ring restored {1}'s HP!",i.ToString()));
				}
			}
			// Ingrain
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Ingrain) {
					GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Ingrain");
					int hpgain=(int)Math.Floor(i.TotalHP/16f);
					if (i.hasWorkingItem(Items.BIG_ROOT)) hpgain=(int)Math.Floor(hpgain*1.3);
					hpgain=i.pbRecoverHP(hpgain,true);
					if (hpgain>0) pbDisplay(Game._INTL("{1} absorbed nutrients with its roots!",i.ToString()));
				}
			}
			// Leech Seed
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.LeechSeed>=0 && !i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					IBattler recipient=@battlers[i.effects.LeechSeed];
					if (recipient.IsNotNullOrNone() && !recipient.isFainted()) {
						GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Leech Seed");
						pbCommonAnimation("LeechSeed",recipient,i);
						int hploss=i.pbReduceHP((int)Math.Floor(i.TotalHP/8f),true);
						if (i.hasWorkingAbility(Abilities.LIQUID_OOZE)) {
							recipient.pbReduceHP(hploss,true);
							pbDisplay(Game._INTL("{1} sucked up the liquid ooze!",recipient.ToString()));
						}
						else {
							if (recipient.effects.HealBlock==0) {
								if (recipient.hasWorkingItem(Items.BIG_ROOT)) hploss=(int)Math.Floor(hploss*1.3);
								recipient.pbRecoverHP(hploss,true);
							}
							pbDisplay(Game._INTL("{1}'s health was sapped by Leech Seed!",i.ToString()));
						}
						if (i.isFainted()) {
							if (!i.pbFaint()) return;
						}
						if (recipient.isFainted()) {
							if (!recipient.pbFaint()) return;
						}
					}
				}
			}
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				// Poison/Bad poison
				if (i.Status==Status.POISON) {
					if (i.StatusCount>0) {
						i.effects.Toxic+=1;
						i.effects.Toxic=(int)Math.Min(15,i.effects.Toxic);
					}
					if (i.hasWorkingAbility(Abilities.POISON_HEAL)) {
						pbCommonAnimation("Poison",i,null);
						if (i.effects.HealBlock==0 && i.HP<i.TotalHP) {
							GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Poison Heal");
							i.pbRecoverHP((int)Math.Floor(i.TotalHP/8f),true);
							pbDisplay(Game._INTL("{1} is healed by poison!",i.ToString()));
						}
					}
					else {
						if (!i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Status damage] #{i.ToString()} took damage from poison/toxic");
							if (i.StatusCount==0) {
								i.pbReduceHP((int)Math.Floor(i.TotalHP/8f));
							}
							else {
								i.pbReduceHP((int)Math.Floor((i.TotalHP*i.effects.Toxic)/16f));
							}
							if (i is IBattlerEffect b) b.pbContinueStatus();
						}
					}
				}
				// Burn
				if (i.Status==Status.BURN) {
					if (!i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
						GameDebug.Log($"[Status damage] #{i.ToString()} took damage from burn");
						if (i.hasWorkingAbility(Abilities.HEATPROOF)) {
							GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Heatproof");
							i.pbReduceHP((int)Math.Floor(i.TotalHP/16f));
						}
						else {
							i.pbReduceHP((int)Math.Floor(i.TotalHP/8f));
						}
					}
					if (i is IBattlerEffect b) b.pbContinueStatus();
				}
				// Nightmare
				if (i.effects.Nightmare) {
					if (i.Status==Status.SLEEP) {
						if (!i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s nightmare");
							i.pbReduceHP((int)Math.Floor(i.TotalHP/4f),true);
							pbDisplay(Game._INTL("{1} is locked in a nightmare!",i.ToString()));
						}
					}
					else {
						i.effects.Nightmare=false;
					}
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
					continue;
				}
			}
			// Curse
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Curse && !i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s curse");
					i.pbReduceHP((int)Math.Floor(i.TotalHP/4f),true);
					pbDisplay(Game._INTL("{1} is afflicted by the curse!",i.ToString()));
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
					continue;
				}
			}
			// Multi-turn attacks (Bind/Clamp/Fire Spin/Magma Storm/Sand Tomb/Whirlpool/Wrap)
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.MultiTurn>0) {
					i.effects.MultiTurn-=1;
					string movename=Game._INTL(i.effects.MultiTurnAttack.ToString(TextScripts.Name));
					if (i.effects.MultiTurn==0) {
						GameDebug.Log($"[End of effect] Trapping move #{movename} affecting #{i.ToString()} ended");
						pbDisplay(Game._INTL("{1} was freed from {2}!",i.ToString(),movename));
					}
					else {
						if (i.effects.MultiTurnAttack == Moves.BIND) {
							pbCommonAnimation("Bind",i,null);
						}
						else if (i.effects.MultiTurnAttack == Moves.CLAMP) {
							pbCommonAnimation("Clamp",i,null);
						}
						else if (i.effects.MultiTurnAttack == Moves.FIRE_SPIN) {
							pbCommonAnimation("FireSpin",i,null);
						}
						else if (i.effects.MultiTurnAttack == Moves.MAGMA_STORM) {
							pbCommonAnimation("MagmaStorm",i,null);
						}
						else if (i.effects.MultiTurnAttack == Moves.SAND_TOMB) {
							pbCommonAnimation("SandTomb",i,null);
						}
						else if (i.effects.MultiTurnAttack == Moves.WRAP) {
							pbCommonAnimation("Wrap",i,null);
						}
						else if (i.effects.MultiTurnAttack == Moves.INFESTATION) {
							pbCommonAnimation("Infestation",i,null);
						}
						else {
							pbCommonAnimation("Wrap",i,null);
						}
						if (!i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							GameDebug.Log($"[Lingering effect triggered] #{i.ToString()} took damage from trapping move #{movename}");
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
							int amt=Core.USENEWBATTLEMECHANICS ? (int)Math.Floor(i.TotalHP/8f) : (int)Math.Floor(i.TotalHP/16f);
							if (@battlers[i.effects.MultiTurnUser].hasWorkingItem(Items.BINDING_BAND)) {
								amt=Core.USENEWBATTLEMECHANICS ? (int)Math.Floor(i.TotalHP/6f) : (int)Math.Floor(i.TotalHP/8f);
							}
							i.pbReduceHP(amt);
							pbDisplay(Game._INTL("{1} is hurt by {2}!",i.ToString(),movename));
						}
					}
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
				}
			}
			// Taunt
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Taunt>0) {
					i.effects.Taunt-=1;
					if (i.effects.Taunt==0) {
						pbDisplay(Game._INTL("{1}'s taunt wore off!",i.ToString()));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer taunted");
					}
				}
			}
			// Encore
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Encore>0) {
					if (i.moves[i.effects.EncoreIndex].id!=i.effects.EncoreMove) {
						i.effects.Encore=0;
						i.effects.EncoreIndex=0;
						i.effects.EncoreMove=0;
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer encored (encored move was lost)");
					}
					else {
						i.effects.Encore-=1;
						if (i.effects.Encore==0 || i.moves[i.effects.EncoreIndex].PP==0) {
							i.effects.Encore=0;
							pbDisplay(Game._INTL("{1}'s encore ended!",i.ToString()));
							GameDebug.Log($"[End of effect] #{i.ToString()} is no longer encored");
						}
					}
				}
			}
			// Disable/Cursed Body
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Disable>0) {
					i.effects.Disable-=1;
					if (i.effects.Disable==0) {
						i.effects.DisableMove=0;
						pbDisplay(Game._INTL("{1} is no longer disabled!",i.ToString()));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer disabled");
					}
				}
			}
			// Magnet Rise
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.MagnetRise>0) {
					i.effects.MagnetRise-=1;
					if (i.effects.MagnetRise==0) {
						pbDisplay(Game._INTL("{1} stopped levitating.",i.ToString()));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer levitating by Magnet Rise");
					}
				}
			}
			// Telekinesis
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Telekinesis>0) {
					i.effects.Telekinesis-=1;
					if (i.effects.Telekinesis==0) {
						pbDisplay(Game._INTL("{1} stopped levitating.",i.ToString()));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer levitating by Telekinesis");
					}
				}
			}
			// Heal Block
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.HealBlock>0) {
					i.effects.HealBlock-=1;
					if (i.effects.HealBlock==0) {
						pbDisplay(Game._INTL("{1}'s Heal Block wore off!",i.ToString()));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer Heal Blocked");
					}
				}
			}
			// Embargo
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Embargo>0) {
					i.effects.Embargo-=1;
					if (i.effects.Embargo==0) {
						pbDisplay(Game._INTL("{1} can use items again!",i.ToString(true)));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer affected by an embargo");
					}
				}
			}
			// Yawn
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Yawn>0) {
					i.effects.Yawn-=1;
					if (i.effects.Yawn==0 && i is IBattlerClause b0 && b0.pbCanSleepYawn()) {
						GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Yawn");
						if (i is IBattlerEffect b1) b1.pbSleep();
					}
				}
			}
			// Perish Song
			List<int> perishSongUsers=new List<int>();
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.PerishSong>0) {
					i.effects.PerishSong-=1;
					pbDisplay(Game._INTL("{1}'s perish count fell to {2}!",i.ToString(),i.effects.PerishSong.ToString()));
					GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Perish Song count dropped to #{i.effects.PerishSong}");
					if (i.effects.PerishSong==0) {
						perishSongUsers.Add(i.effects.PerishSongUser);
						i.pbReduceHP(i.HP,true);
					}
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
				}
			}
			if (perishSongUsers.Count>0) {
				// If all remaining Pokemon fainted by a Perish Song triggered by a single side
				if ((perishSongUsers.Count(item => pbIsOpposing(item))==perishSongUsers.Count) ||
					(perishSongUsers.Count(item => !pbIsOpposing(item))==perishSongUsers.Count)) {
					pbJudgeCheckpoint(@battlers[(int)perishSongUsers[0]]);
				}
			}
			if (@decision>0) {
				pbGainEXP();
				return;
			}
			// Reflect
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].Reflect>0) {
					sides[i].Reflect-=1;
					if (sides[i].Reflect==0) {
						if (i==0) pbDisplay(Game._INTL("Your team's Reflect faded!"));
						if (i==1) pbDisplay(Game._INTL("The opposing team's Reflect faded!"));
						if (i==0) GameDebug.Log($"[End of effect] Reflect ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Reflect ended on the opponent's side");
					}
				}
			}
			// Light Screen
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].LightScreen>0) {
					sides[i].LightScreen-=1;
					if (sides[i].LightScreen==0) {
						if (i==0) pbDisplay(Game._INTL("Your team's Light Screen faded!"));
						if (i==1) pbDisplay(Game._INTL("The opposing team's Light Screen faded!"));
						if (i==0) GameDebug.Log($"[End of effect] Light Screen ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Light Screen ended on the opponent's side");
					}
				}
			}
			// Safeguard
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].Safeguard>0) {
					sides[i].Safeguard-=1;
					if (sides[i].Safeguard==0) {
						if (i==0) pbDisplay(Game._INTL("Your team is no longer protected by Safeguard!"));
						if (i==1) pbDisplay(Game._INTL("The opposing team is no longer protected by Safeguard!"));
						if (i==0) GameDebug.Log($"[End of effect] Safeguard ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Safeguard ended on the opponent's side");
					}
				}
			}
			// Mist
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].Mist>0) {
					sides[i].Mist-=1;
					if (sides[i].Mist==0) {
						if (i==0) pbDisplay(Game._INTL("Your team's Mist faded!"));
						if (i==1) pbDisplay(Game._INTL("The opposing team's Mist faded!"));
						if (i==0) GameDebug.Log($"[End of effect] Mist ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Mist ended on the opponent's side");
					}
				}
			}
			// Tailwind
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].Tailwind>0) {
					sides[i].Tailwind-=1;
					if (sides[i].Tailwind==0) {
						if (i==0) pbDisplay(Game._INTL("Your team's Tailwind petered out!"));
						if (i==1) pbDisplay(Game._INTL("The opposing team's Tailwind petered out!"));
						if (i==0) GameDebug.Log($"[End of effect] Tailwind ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Tailwind ended on the opponent's side");
					}
				}
			}
			// Lucky Chant
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].LuckyChant>0) {
					sides[i].LuckyChant-=1;
					if (sides[i].LuckyChant==0) {
						if (i==0) pbDisplay(Game._INTL("Your team's Lucky Chant faded!"));
						if (i==1) pbDisplay(Game._INTL("The opposing team's Lucky Chant faded!"));
						if (i==0) GameDebug.Log($"[End of effect] Lucky Chant ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Lucky Chant ended on the opponent's side");
					}
				}
			}
			// End of Pledge move combinations
			for (int i = 0; i < sides.Length; i++) {
				if (sides[i].Swamp>0) {
					sides[i].Swamp-=1;
					if (sides[i].Swamp==0) {
						if (i==0) pbDisplay(Game._INTL("The swamp around your team disappeared!"));
						if (i==1) pbDisplay(Game._INTL("The swamp around the opposing team disappeared!"));
						if (i==0) GameDebug.Log($"[End of effect] Grass Pledge's swamp ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Grass Pledge's swamp ended on the opponent's side");
					}
				}
				if (sides[i].SeaOfFire>0) {
					sides[i].SeaOfFire-=1;
					if (sides[i].SeaOfFire==0) {
						if (i==0) pbDisplay(Game._INTL("The sea of fire around your team disappeared!"));
						if (i==1) pbDisplay(Game._INTL("The sea of fire around the opposing team disappeared!"));
						if (i==0) GameDebug.Log($"[End of effect] Fire Pledge's sea of fire ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Fire Pledge's sea of fire ended on the opponent's side");
					}
				}
				if (sides[i].Rainbow>0) {
					sides[i].Rainbow-=1;
					if (sides[i].Rainbow==0) {
						if (i==0) pbDisplay(Game._INTL("The rainbow around your team disappeared!"));
						if (i==1) pbDisplay(Game._INTL("The rainbow around the opposing team disappeared!"));
						if (i==0) GameDebug.Log($"[End of effect] Water Pledge's rainbow ended on the player's side");
						if (i==1) GameDebug.Log($"[End of effect] Water Pledge's rainbow ended on the opponent's side");
					}
				}
			}
			// Gravity
			if (@field.Gravity>0) {
				@field.Gravity-=1;
				if (@field.Gravity==0) {
					pbDisplay(Game._INTL("Gravity returned to normal."));
					GameDebug.Log($"[End of effect] Strong gravity ended");
				}
			}
			// Trick Room
			if (@field.TrickRoom>0) {
				@field.TrickRoom-=1;
				if (@field.TrickRoom==0) {
					pbDisplay(Game._INTL("The twisted dimensions returned to normal."));
					GameDebug.Log($"[End of effect] Trick Room ended");
				}
			}
			// Wonder Room
			if (@field.WonderRoom>0) {
				@field.WonderRoom-=1;
				if (@field.WonderRoom==0) {
					pbDisplay(Game._INTL("Wonder Room wore off, and the Defense and Sp. public void stats returned to normal!"));
					GameDebug.Log($"[End of effect] Wonder Room ended");
				}
			}
			// Magic Room
			if (@field.MagicRoom>0) {
				@field.MagicRoom-=1;
				if (@field.MagicRoom==0) {
					pbDisplay(Game._INTL("The area returned to normal."));
					GameDebug.Log($"[End of effect] Magic Room ended");
				}
			}
			// Mud Sport
			if (@field.MudSportField>0) {
				@field.MudSportField-=1;
				if (@field.MudSportField==0) {
					pbDisplay(Game._INTL("The effects of Mud Sport have faded."));
					GameDebug.Log($"[End of effect] Mud Sport ended");
				}
			}
			// Water Sport
			if (@field.WaterSportField>0) {
				@field.WaterSportField-=1;
				if (@field.WaterSportField==0) {
					pbDisplay(Game._INTL("The effects of Water Sport have faded."));
					GameDebug.Log($"[End of effect] Water Sport ended");
				}
			}
			// Electric Terrain
			if (@field.ElectricTerrain>0) {
				@field.ElectricTerrain-=1;
				if (@field.ElectricTerrain==0) {
					pbDisplay(Game._INTL("The electric current disappeared from the battlefield."));
					GameDebug.Log($"[End of effect] Electric Terrain ended");
				}
			}
			// Grassy Terrain (counting down)
			if (@field.GrassyTerrain>0) {
				@field.GrassyTerrain-=1;
				if (@field.GrassyTerrain==0) {
					pbDisplay(Game._INTL("The grass disappeared from the battlefield."));
					GameDebug.Log($"[End of effect] Grassy Terrain ended");
				}
			}
			// Misty Terrain
			if (@field.MistyTerrain>0) {
				@field.MistyTerrain-=1;
				if (@field.MistyTerrain==0) {
					pbDisplay(Game._INTL("The mist disappeared from the battlefield."));
					GameDebug.Log($"[End of effect] Misty Terrain ended");
				}
			}
			// Uproar
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				if (i.effects.Uproar>0) {
					foreach (var j in priority) {
						if (!j.isFainted() && j.Status==Status.SLEEP && !j.hasWorkingAbility(Abilities.SOUNDPROOF)) {
							GameDebug.Log($"[Lingering effect triggered] Uproar woke up #{j.ToString(true)}");
							if (j is IBattlerEffect b) b.pbCureStatus(false);
							pbDisplay(Game._INTL("{1} woke up in the uproar!",j.ToString()));
						}
					}
					i.effects.Uproar-=1;
					if (i.effects.Uproar==0) {
						pbDisplay(Game._INTL("{1} calmed down.",i.ToString()));
						GameDebug.Log($"[End of effect] #{i.ToString()} is no longer uproaring");
					}
					else {
						pbDisplay(Game._INTL("{1} is making an uproar!",i.ToString()));
					}
				}
			}
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				// Speed Boost
				// A Pokémon's turncount is 0 if it became active after the beginning of a round
				if (i.turncount>0 && i.hasWorkingAbility(Abilities.SPEED_BOOST)) {
					if (i is IBattlerEffect b && b.pbIncreaseStatWithCause(Stats.SPEED,1,i,Game._INTL(i.Ability.ToString(TextScripts.Name)))) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s #{Game._INTL(i.Ability.ToString(TextScripts.Name))}");
					}
				}
				// Bad Dreams
				if (i.Status==Status.SLEEP && !i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					if (i.pbOpposing1.hasWorkingAbility(Abilities.BAD_DREAMS) ||
						i.pbOpposing2.hasWorkingAbility(Abilities.BAD_DREAMS)) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s opponent's Bad Dreams");
						int hploss=i.pbReduceHP((int)Math.Floor(i.TotalHP/8f),true);
						if (hploss>0) pbDisplay(Game._INTL("{1} is having a bad dream!",i.ToString()));
					}
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
					continue;
				}
				// Pickup
				if (i.hasWorkingAbility(Abilities.PICKUP) && i.Item<=0) {
					Items item=0; int index=-1; int use=0;
					for (int j = 0; j < battlers.Length; j++) {
						if (j==i.Index) continue;
						if (@battlers[j].effects.PickupUse>use) {
							item=@battlers[j].effects.PickupItem;
							index=j;
							use=@battlers[j].effects.PickupUse;
						}
					}
					if (item>0) {
						i.Item=item;
						@battlers[index].effects.PickupItem=0;
						@battlers[index].effects.PickupUse=0;
						if (@battlers[index].pokemon.itemRecycle==item) @battlers[index].pokemon.itemRecycle=0;
						if (@opponent.Length == 0 && // In a wild battle
							i.pokemon.itemInitial==0 &&
							@battlers[index].pokemon.itemInitial==item) {
							i.pokemon.itemInitial=item;
							@battlers[index].pokemon.itemInitial=0;
						}
						pbDisplay(Game._INTL("{1} found one {2}!",i.ToString(),Game._INTL(item.ToString(TextScripts.Name))));
						i.pbBerryCureCheck(true);
					}
				}
				// Harvest
				if (i.hasWorkingAbility(Abilities.HARVEST) && i.Item<=0 && i.pokemon.itemRecycle>0) {
					if (Kernal.ItemData[i.pokemon.itemRecycle].IsBerry && //pbIsBerry(i.itemRecycle)
						(pbWeather==Weather.SUNNYDAY ||
						pbWeather==Weather.HARSHSUN || pbRandom(10)<5)) {
						i.Item=i.pokemon.itemRecycle;
						i.pokemon.itemRecycle=0;
						if (i.pokemon.itemInitial==0) i.pokemon.itemInitial=i.Item;
						pbDisplay(Game._INTL("{1} harvested one {2}!",i.ToString(),Game._INTL(i.Item.ToString(TextScripts.Name))));
						i.pbBerryCureCheck(true);
					}
				}
				// Moody
				if (i.hasWorkingAbility(Abilities.MOODY)) {
					List<Stats> randomup=new List<Stats>(); List<Stats> randomdown=new List<Stats>();
					foreach (var j in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,Stats.SPATK,
								Stats.SPDEF,Stats.ACCURACY,Stats.EVASION }) {
						if (i is IBattlerEffect b0 && b0.pbCanIncreaseStatStage(j,i)) randomup.Add(j);
						if (i is IBattlerEffect b1 && b1.pbCanReduceStatStage(j,i)) randomdown.Add(j);
					}
					if (randomup.Count>0) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Moody (raise stat)");
						int r=pbRandom(randomup.Count);
						if (i is IBattlerEffect b) b.pbIncreaseStatWithCause(randomup[r],2,i,Game._INTL(i.Ability.ToString(TextScripts.Name)));
						for (int j = 0; j < randomdown.Count; j++) {
							if (randomdown[j]==randomup[r]) {
								//randomdown[j]=null; randomdown.compact!;
								randomdown.RemoveAt(j);
								break;
							}
						}
					}
					if (randomdown.Count>0) {
						GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Moody (lower stat)");
						int r=pbRandom(randomdown.Count);
						if (i is IBattlerEffect b) b.pbReduceStatWithCause(randomdown[r],1,i,Game._INTL(i.Ability.ToString(TextScripts.Name)));
					}
				}
			}
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				// Toxic Orb
				if (i.hasWorkingItem(Items.TOXIC_ORB) && i.Status==0 && i is IBattlerEffect b0 && b0.pbCanPoison(null,false)) {
					GameDebug.Log($"[Item triggered] #{i.ToString()}'s Toxic Orb");
					b0.pbPoison(null,Game._INTL("{1} was badly poisoned by its {2}!",i.ToString(),
						Game._INTL(i.Item.ToString(TextScripts.Name))),true);
				}
				// Flame Orb
				if (i.hasWorkingItem(Items.FLAME_ORB) && i.Status==0 && i is IBattlerEffect b1 && b1.pbCanBurn(null,false)) {
					GameDebug.Log($"[Item triggered] #{i.ToString()}'s Flame Orb");
					b1.pbBurn(null,Game._INTL("{1} was burned by its {2}!",i.ToString(),Game._INTL(i.Item.ToString(TextScripts.Name))));
				}
				// Sticky Barb
				if (i.hasWorkingItem(Items.STICKY_BARB) && !i.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					GameDebug.Log($"[Item triggered] #{i.ToString()}'s Sticky Barb");
					(@scene as IPokeBattle_DebugSceneNoGraphics).pbDamageAnimation(i,0);
					i.pbReduceHP((int)Math.Floor(i.TotalHP/8f));
					pbDisplay(Game._INTL("{1} is hurt by its {2}!",i.ToString(),Game._INTL(i.Item.ToString(TextScripts.Name))));
				}
				if (i.isFainted()) {
					if (!i.pbFaint()) return;
				}
			}
			// Form checks
			for (int i = 0; i < battlers.Length; i++) {
				if (@battlers[i].isFainted()) continue;
				@battlers[i].pbCheckForm();
			}
			pbGainEXP();
			pbSwitch();
			if (@decision>0) return;
			foreach (var i in priority) {
				if (i.isFainted()) continue;
				i.pbAbilitiesOnSwitchIn(false);
			}
			// Healing Wish/Lunar Dance - should go here
			// Spikes/Toxic Spikes/Stealth Rock - should go here (in order of their 1st use)
			for (int i = 0; i < battlers.Length; i++) {
				if (@battlers[i].turncount>0 && @battlers[i].hasWorkingAbility(Abilities.TRUANT)) {
					@battlers[i].effects.Truant=!@battlers[i].effects.Truant;
				}
				if (@battlers[i].effects.LockOn>0) {   // Also Mind Reader
					@battlers[i].effects.LockOn-=1;
					if (@battlers[i].effects.LockOn==0) @battlers[i].effects.LockOnPos=-1;
				}
				@battlers[i].effects.Flinch=false;
				@battlers[i].effects.FollowMe=0;
				@battlers[i].effects.HelpingHand=false;
				@battlers[i].effects.MagicCoat=false;
				@battlers[i].effects.Snatch=false;
				if (@battlers[i].effects.Charge>0) @battlers[i].effects.Charge-=1;
				@battlers[i].lastHPLost=0;
				@battlers[i].tookDamage=false;
				@battlers[i].lastAttacker.Clear();
				@battlers[i].effects.Counter=-1;
				@battlers[i].effects.CounterTarget=-1;
				@battlers[i].effects.MirrorCoat=-1;
				@battlers[i].effects.MirrorCoatTarget=-1;
			}
			for (int i = 0; i < sides.Length; i++) {
				if (!@sides[i].EchoedVoiceUsed) {
					@sides[i].EchoedVoiceCounter=0;
				}
				@sides[i].EchoedVoiceUsed=false;
				@sides[i].QuickGuard=false;
				@sides[i].WideGuard=false;
				@sides[i].CraftyShield=false;
				@sides[i].Round=0;
			}
			@field.FusionBolt=false;
			@field.FusionFlare=false;
			@field.IonDeluge=false;
			if (@field.FairyLock>0) @field.FairyLock-=1;
			// invalidate stored priority
			@usepriority=false;
		}
		#endregion

		#region End of battle.
		public BattleResults pbEndOfBattle(bool canlose=false) {
			//switch (@decision) {
				//#### WIN ####//
				if (@decision == BattleResults.WON) { //case BattleResults.WON:
					GameDebug.Log($"");
					GameDebug.Log($"***Player won***");
					if (@opponent.Length > 0) {
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbTrainerBattleSuccess();
						if (@opponent.Length > 0) {
							pbDisplayPaused(Game._INTL("{1} defeated {2} and {3}!",this.pbPlayer().name,@opponent[0].name,@opponent[1].name));
						}
						else {
							pbDisplayPaused(Game._INTL("{1} defeated\r\n{2}!",this.pbPlayer().name,@opponent[0].name));
						}
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(0);
						pbDisplayPaused(@endspeech.Replace("/\\[Pp][Nn]/",this.pbPlayer().name));
						if (@opponent.Length > 0) {
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbHideOpponent();
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(1);
							pbDisplayPaused(@endspeech2.Replace("/\\[Pp][Nn]/",this.pbPlayer().name));
						}
						// Calculate money gained for winning
						if (@internalbattle) {
							int tmoney=0;
							if (@opponent.Length > 1) {   // Double battles
								int maxlevel1=0; int maxlevel2=0; int limit=pbSecondPartyBegin(1);
								for (int i = 0; i < limit; i++) {
									if (@party2[i].IsNotNullOrNone()) {
										if (maxlevel1<@party2[i].Level) maxlevel1=@party2[i].Level;
									}
									if (@party2[i+limit].IsNotNullOrNone()) {
										if (maxlevel1<@party2[i+limit].Level) maxlevel2=@party2[i+limit].Level;
									}
								}
								tmoney+=maxlevel1*@opponent[0].Money;
								tmoney+=maxlevel2*@opponent[1].Money;
							}
							else {
								int maxlevel=0;
								foreach (var i in @party2) {
									if (!i.IsNotNullOrNone()) continue;
									if (maxlevel<i.Level) maxlevel=i.Level;
								}
								tmoney+=maxlevel*@opponent[0].Money;
							}
							// If Amulet Coin/Luck Incense's effect applies, double money earned
							if (@amuletcoin) tmoney*=2;
							// If Happy Hour's effect applies, double money earned
							if (@doublemoney) tmoney*=2;
							int oldmoney=this.pbPlayer().Money;
							this.pbPlayer().Money+=tmoney;
							int moneygained=this.pbPlayer().Money-oldmoney;
							if (moneygained>0) {
								pbDisplayPaused(Game._INTL("{1} got ${2}\r\nfor winning!",this.pbPlayer().name,tmoney.ToString()));
							}
						}
					}
					if (@internalbattle && @extramoney>0) {
						if (@amuletcoin) @extramoney*=2;
						if (@doublemoney) @extramoney*=2;
						int oldmoney=this.pbPlayer().Money;
						this.pbPlayer().Money+=@extramoney;
						int moneygained=this.pbPlayer().Money-oldmoney;
						if (moneygained>0) {
							pbDisplayPaused(Game._INTL("{1} picked up ${2}!",this.pbPlayer().name,@extramoney.ToString()));
						}
					}
					foreach (var p in @snaggedpokemon) {
						IPokemon pkmn = this.party2[p];
						pbStorePokemon(pkmn);
						//if (this.pbPlayer().shadowcaught == null) this.pbPlayer().shadowcaught=new Dictionary<Pokemons,bool>();
						if (this.pbPlayer().shadowcaught == null) this.pbPlayer().shadowcaught=new List<Pokemons>();
						//this.pbPlayer().shadowcaught[pkmn.Species]=true;
						if (!this.pbPlayer().shadowcaught.Contains(pkmn.Species)) this.pbPlayer().shadowcaught.Add(pkmn.Species);
					}
					@snaggedpokemon.Clear();
					//break; 
				} 
				//#### LOSE, DRAW ####//
				else if (@decision == BattleResults.LOST || @decision == BattleResults.DRAW) { //case BattleResults.LOST: case BattleResults.DRAW:
					GameDebug.Log($"");
					if (@decision==BattleResults.LOST) GameDebug.Log($"***Player lost***");
					if (@decision==BattleResults.DRAW) GameDebug.Log($"***Player drew with opponent***");
					if (@internalbattle) {
						pbDisplayPaused(Game._INTL("{1} is out of usable Pokémon!",this.pbPlayer().name));
						int moneylost=pbMaxLevelFromIndex(0);   // Player's Pokémon only, not partner's
						int[] multiplier=new int[] { 8, 16, 24, 36, 48, 60, 80, 100, 120 };
						moneylost*=multiplier[(int)Math.Min(multiplier.Length-1,this.pbPlayer().badges.Length)];
						if (moneylost>this.pbPlayer().Money) moneylost=this.pbPlayer().Money;
						if (Core.NO_MONEY_LOSS) moneylost=0;
						int oldmoney=this.pbPlayer().Money;
						this.pbPlayer().Money-=moneylost;
						int lostmoney=oldmoney-this.pbPlayer().Money;
						if (@opponent.Length > 0) {
							if (@opponent.Length > 0) {
								pbDisplayPaused(Game._INTL("{1} lost against {2} and {3}!",this.pbPlayer().name,@opponent[0].name,@opponent[1].name));
							}
							else {
								pbDisplayPaused(Game._INTL("{1} lost against\r\n{2}!",this.pbPlayer().name,@opponent[0].name));
							}
							if (moneylost>0) {
								pbDisplayPaused(Game._INTL("{1} paid ${2}\r\nas the prize money...",this.pbPlayer().name,lostmoney.ToString()));
								if (!canlose) pbDisplayPaused(Game._INTL("..."));
							}
						}
						else {
							if (moneylost>0) {
								pbDisplayPaused(Game._INTL("{1} panicked and lost\r\n${2}...",this.pbPlayer().name,lostmoney.ToString()));
								if (!canlose) pbDisplayPaused(Game._INTL("..."));
							}
						}
						if (!canlose) pbDisplayPaused(Game._INTL("{1} blacked out!",this.pbPlayer().name));
					}
					else if (@decision==BattleResults.LOST) {
						(@scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(0);
						pbDisplayPaused(@endspeechwin.Replace("/\\[Pp][Nn]/",this.pbPlayer().name));
						if (@opponent.Length > 0) {
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbHideOpponent();
							(@scene as IPokeBattle_DebugSceneNoGraphics).pbShowOpponent(1);
							pbDisplayPaused(@endspeechwin2.Replace("/\\[Pp][Nn]/",this.pbPlayer().name));
						}
					}
					//break;
			}
			// Pass on Pokérus within the party
			List<int> infected=new List<int>();
			for (int i = 0; i < Game.GameData.Trainer.party.Length; i++) {
				if (Game.GameData.Trainer.party[i].PokerusStage.HasValue && Game.GameData.Trainer.party[i].PokerusStage.Value) { //Game.GameData.Trainer.party[i].PokerusStage==1
					infected.Add(i);
				}
			}
			if (infected.Count>=1) {
				foreach (var i in infected) {
					int strain=(Game.GameData.Trainer.party[i] as Monster.Pokemon).PokerusStrain;//.pokerus/16;
					if (i>0 && !Game.GameData.Trainer.party[i-1].PokerusStage.HasValue) { //Game.GameData.Trainer.party[i-1].PokerusStage==0
						if (pbRandom(3)==0) Game.GameData.Trainer.party[i-1].GivePokerus(strain);
					}
					if (i<Game.GameData.Trainer.party.Length-1 && !Game.GameData.Trainer.party[i+1].PokerusStage.HasValue) { //Game.GameData.Trainer.party[i-1].PokerusStage==0
						if (pbRandom(3)==0) Game.GameData.Trainer.party[i+1].GivePokerus(strain);
					}
				}
			}
			(@scene as IPokeBattle_DebugSceneNoGraphics).pbEndBattle(@decision);
			foreach (IBattler i in @battlers) {
				i.pbResetForm();
				if (i.hasWorkingAbility(Abilities.NATURAL_CURE)) {
					i.Status=0;
				}
			}
			foreach (IPokemon i in Game.GameData.Trainer.party) {
				i.setItem(i.itemInitial);
				i.itemInitial=i.itemRecycle=0;
				i.belch=false;
			}
			return @decision;
		}
		#endregion

		IEnumerator IBattle.pbDebugUpdate()
		{
			throw new NotImplementedException();
		}

		int IBattle.pbAIRandom(int x)
		{
			throw new NotImplementedException();
		}

		void IBattle.pbAddToPlayerParty(IBattler pokemon)
		{
			throw new NotImplementedException();
		}

		bool IBattle.pbCanShowCommands(int idxPokemon)
		{
			throw new NotImplementedException();
		}

		bool IBattle.pbCanShowFightMenu(int idxPokemon)
		{
			throw new NotImplementedException();
		}

		bool IBattle.pbCanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk)
		{
			throw new NotImplementedException();
		}

		void IBattle.pbSendOut(int index, IBattler pokemon)
		{
			throw new NotImplementedException();
		}

		bool IBattle.pbOnActiveOne(IBattler pkmn, bool onlyabilities, bool moldbreaker)
		{
			throw new NotImplementedException();
		}

		void IBattleCommon.pbThrowPokeBall(int idxPokemon, Items ball, int? rareness, bool showplayer)
		{
			throw new NotImplementedException();
		}
		#endregion
	}

	public class BattleAbortedException : Exception 
	{ 
		public BattleAbortedException(string message) : base(message) { }
	}
}