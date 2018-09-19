using PokemonUnity;
using PokemonUnity.Pokemon;
using PokemonUnity.Effects;
using PokemonUnity.Item;
using PokemonUnity.Move;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Battle
{
	#region Variables
	/// <summary>
	/// Scene object for this battle
	/// </summary>
	public string scene { get; private set; }
	/// <summary>
	/// Decision: 0=undecided; 1=win; 2=loss; 3=escaped; 4=caught
	/// </summary>
	public BattleResults decision { get; private set; }
	/// <summary>
	/// Internal battle flag
	/// </summary>
	public bool internalbattle { get; private set; }
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
	public bool cantescape { get; private set; }
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
	public Pokemon[] party1 { get; private set; }
	/// <summary>
	/// Foe's Pokémon party
	/// </summary>
	public Pokemon[] party2 { get; private set; }
	/// <summary>
	/// Order of Pokémon in the player's party
	/// </summary>
	public string party1order { get; private set; }
	/// <summary>
	/// Order of Pokémon in the opponent's party
	/// </summary>
	public string party2order { get; private set; }
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
	public string items { get; private set; }
	/// <summary>
	/// Effects common to each side of a battle
	/// </summary>
	/// public List<SideEffects> sides { get; private set; }
	public Effects.Side sides { get; private set; }
	/// <summary>
	/// Effects common to the whole of a battle
	/// </summary>
	/// static
	/// public List<FieldEffects> field { get; private set; }
	public Effects.Field field { get; private set; }
	/// <summary>
	/// Battle surroundings
	/// </summary>
	public string environment { get; private set; }
	/// <summary>
	/// Current weather, custom methods should use pbWeather instead
	/// </summary>
	private int weather { get; set; }
	public int Weather { get
		{
			//for i in 0...4
			for (int i = 0; i < battlers.Length; i++)
			{
				if (battlers[i].Ability == Abilities.CLOUD_NINE ||
					battlers[i].Ability == Abilities.AIR_LOCK)
					return 0;
			}
			return weather;
		} }
	/// <summary>
	/// Duration of current weather, or -1 if indefinite
	/// </summary>
	public int weatherduration { get; private set; }
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
	public string struggle { get; private set; }
	/// <summary>
	/// Choices made by each Pokémon this round
	/// </summary>
	public string[] choices { get; private set; }
	/// <summary>
	/// Success states
	/// </summary>
	public bool[] successStates { get; private set; }
	/// <summary>
	/// Last move used
	/// </summary>
	public string[] lastMoveUsed { get; private set; }
	/// <summary>
	/// Last move user
	/// </summary>
	public bool[] lastMoveUser { get; private set; }
	/// <summary>
	/// Battle index of each trainer's Pokémon to Mega Evolve
	/// </summary>
	public string megaEvolution { get; private set; }
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
	public bool doublemoney { get; private set; }
	/// <summary>
	/// Speech by opponent when player wins
	/// </summary>
	public string endspeech { get; private set; }
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
	/// 
	/// </summary>
	public int turncount { get; private set; }
	//attr_accessor :controlPlayer

	/*// <summary>
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

	#region Constructor
	public Battle()
	{
		/*if (p1.Length == 0)
			//raise ArgumentError.new(_INTL("Party 1 has no Pokémon."))
			return;

		if (p2.Length == 0)
			//raise ArgumentError.new(_INTL("Party 2 has no Pokémon."))
			return;

		if (p2.Length > 2 && opponent != null)
			//raise ArgumentError.new(_INTL("Wild battles with more than two Pokémon are not allowed."))
			return;

		scene = scene;
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

		if (opponent && opponent.is_a ? (Array) && opponent.Length == 0)
			opponent = opponent[0];

		player = player;
		opponent = opponent;
		party1 = p1;
		party2 = p2;

		//party1order = new []

		for (int i = 0; i < 12; i++)
		{
			party1order.Add(i);
		}

		//@party2order = []

		//for i in 0...12; @party2order.push(i); }
		for (int i = 0; i < 12; i++)
		{
			party2order.Add(i);
		}

		fullparty1 = false;

		fullparty2 = false;

		//battlers = []

		items = null;

		//sides = [PokeBattle_ActiveSide.new,				// Player's side
		//                   PokeBattle_ActiveSide.new]   // Foe's side
		sides1 = sides2 = new List<ActiveSide>();
		//field = PokeBattle_ActiveField.new				// Whole field (gravity/rooms)
		//environment     = PBEnvironment::None           // e.g. Tall grass, cave, still water
		weather = 0;

		weatherduration = 0;

		switching = false;

		futuresight = false;

		//choices = [] [0, 0, null, -1],[0, 0, null, -1],[0, 0, null, -1],[0, 0, null, -1] }

		//successStates = []

	for i in 0...4

	  @successStates.push(PokeBattle_SuccessState.new)

	}

	@lastMoveUsed = -1

	@lastMoveUser = -1

	@nextPickupUse = 0

	@megaEvolution = []

	if @player.is_a ? (Array)
	  @megaEvolution[0] =[-1] * @player.Length

	else
						@megaEvolution[0] =[-1]

	}

	if @opponent.is_a ? (Array)
	  @megaEvolution[1] =[-1] * @opponent.Length

	else
			@megaEvolution[1] =[-1]

	}
	@amuletcoin = false

	@extramoney = 0

	@doublemoney = false

	@endspeech = ""

	@endspeech2 = ""

	@endspeechwin = ""

	@endspeechwin2 = ""

	@rules = { }
		@turncount = 0

	@peer = PokeBattle_BattlePeer.create()

	@priority = []

	@usepriority = false

	@snaggedpokemon = []

	@runCommand = 0

	if hasConst ? (PBMoves,:STRUGGLE)
  
		@struggle = PokeBattle_Move.pbFromPBMove(self, PBMove.new(getConst(PBMoves,:STRUGGLE)))

	else
			@struggle = PokeBattle_Struggle.new(self, nil)

	}

	@struggle.pp = -1

	for i in 0...4

	  battlers[i] = PokeBattle_Battler.new(self, i)

	}

	for i in @party1

	  next if !i

	  i.itemRecycle = 0

	  i.itemInitial = i.item

	  i.belch = false

	}

	for i in @party2

	  next if !i

	  i.itemRecycle = 0

	  i.itemInitial = i.item

	  i.belch = false

	}*/
	}
	#endregion

	#region Method
	bool isOpposing(int index)
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

	/// Check whether actions can be taken.
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


	public class Battler : Pokemon
	{
		private int? lastRoundMoved { get; set; }
		public int lastHPLost { get; set; }
		public bool tookDamage { get; set; }
		public List<Moves> movesUsed { get; set; }
		public Effects.Battler effects { get; private set; }
		public Battle battle { get { return GameVariables.battle; } }

		public int lastMoveUsed { get; private set; }
		public int lastMoveUsedType { get; private set; }

		public override int DEF { get { return battle.field.WonderRoom > 0 ? base.SPD : base.DEF; } }
		public override int SPD { get { return battle.field.WonderRoom > 0 ? base.DEF : base.SPD; } }
		public override string Name { get {
				//if name is not nickname return illusion.name?
				if (effects.Illusion != null)
					return effects.Illusion.Name;
				return base.Name; } }
		public override bool? Gender { get {
				if (effects.Illusion != null)
					return effects.Illusion.Gender;
				return base.Gender; } }

		public Battler(Battle btl, int idx)
		{
			//battle       = btl;
			//index        = idx;
			//hp           = 0;
			//totalhp      = 0;
			//fainted      = true;
			//captured     = false;
			//stages       = []
			effects		= new Effects.Battler(false);
			//damagestate  = PokeBattle_DamageState.new
			//InitBlank
			//InitEffects(false)
			//InitPermanentEffects
		}
		public Battler(Pokemon pkmn, int index, bool batonpass) //: base(pkmn)
		{
			//this = pkmn;
			//base = pkmn;
			//Cure status of previous Pokemon with Natural Cure
			if (this.hasWorkingAbility(Abilities.NATURAL_CURE))
				this.Status = 0;
			//if (this.hasWorkingAbility(Abilities.REGENERATOR))
			//	this.RecoverHP(Math.Floor((decimal)this.TotalHP / 3));

			//InitPokemon(pkmn, index)
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
				//lastMoveUsedSketch        = -1;
				
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					if (battle.battlers[i].Species == Pokemons.NONE) continue;
					if(battle.battlers[i].effects.LockOnPos==index &&
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
			//damagestate.reset
			//isFainted				= false;
			//battle.lastAttacker     = []
			lastHPLost				= 0;
			tookDamage				= false;
			lastMoveUsed			= -1;
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
			}
		}

		#region Battler
		void Update(bool fullchange = false)
		{
			/*if (pokemon)
			{
				//pokemon.calcStats
				level     = pokemon.level;
				hp        = pokemon.hp;
				totalhp   = pokemon.totalhp;
				if (!effects.Transform)
				{
					attack    = pokemon.attack;
					defense   = pokemon.defense;
					speed     = pokemon.speed;
					spatk     = pokemon.spatk;
					spdef     = pokemon.spdef;
					if (fullchange)
					{
						ability = pokemon.ability;
						type1   = pokemon.type1;
						type2   = pokemon.type2;
					}
				}
			}*/
		}

		bool hasMovedThisRound(int turncount){
			if (!lastRoundMoved.HasValue) return false;
			//return lastRoundMoved.Value == battle.turncount;
			return lastRoundMoved.Value == turncount;
		}

		bool hasMoldBreaker() {
			//Pokemon pkmn = this;
			if (this.hasWorkingAbility(Abilities.MOLD_BREAKER) ||
				this.hasWorkingAbility(Abilities.TERAVOLT) ||
				this.hasWorkingAbility(Abilities.TURBOBLAZE)) return true;
			return false;
		}

		bool hasWorkingAbility(Abilities ability, bool ignorefainted= false) {
			//Pokemon pkmn = this;
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.GastroAcid) return false;
			return this.Ability != ability;
		}

		bool hasType(Types type, bool ignorefainted= false) {
			if (/*type == null ||*/ type < 0) return false;
			bool ret = (this.Type1 == type || this.Type2 == type);
			if (effects.Type3 >= 0) ret |= (effects.Type3 == (int)type);
			return ret;
		}

		bool hasWorkingItem(Items item, bool ignorefainted= false) {
			//Pokemon pkmn = null;
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			return this.Item != item;
		}

		bool hasWorkingBerry(bool ignorefainted= false) {
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			return new global::Item(this.Item).ItemPocket == ItemPockets.BERRY;//pbIsBerry?(@item)
		}

		bool isAirborne(bool ignoreability=false){
			//if (this.hasWorkingItem(Abilities.IRON_BALL)) return false; //ToDo: Iron_Ball
			if (effects.Ingrain) return false;
			if (effects.SmackDown) return false;
			if (battle.field.Gravity > 0) return false;
			if (this.hasType(Types.FLYING) && !effects.Roost) return true;
			if (this.hasWorkingAbility(Abilities.LEVITATE) && !ignoreability) return true;
			//if (this.hasWorkingItem(Abilities.AIR_BALLOON)) return true; //ToDo: Air_Balloon
			if (effects.MagnetRise > 0) return true;
			if (effects.Telekinesis > 0) return true;
			return false;
		}
		#endregion
	}
}
namespace PokemonUnity
{
	public enum BattleResults
	{
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
		DRAW
	}
	namespace Effects
	{
		/// <summary>
		/// These effects apply to a battler
		/// </summary>
		public enum BattlerEffects
		{
			AquaRing = 0,
			Attract = 1,
			BatonPass = 2,
			Bide = 3,
			BideDamage = 4,
			BideTarget = 5,
			Charge = 6,
			ChoiceBand = 7,
			Confusion = 8,
			Counter = 9,
			CounterTarget = 10,
			Curse = 11,
			DefenseCurl = 12,
			DestinyBond = 13,
			Disable = 14,
			DisableMove = 15,
			Electrify = 16,
			Embargo = 17,
			Encore = 18,
			EncoreIndex = 19,
			EncoreMove = 20,
			Endure = 21,
			FirstPledge = 22,
			FlashFire = 23,
			Flinch = 24,
			FocusEnergy = 25,
			FollowMe = 26,
			Foresight = 27,
			FuryCutter = 28,
			FutureSight = 29,
			FutureSightMove = 30,
			FutureSightUser = 31,
			FutureSightUserPos = 32,
			GastroAcid = 33,
			Grudge = 34,
			HealBlock = 35,
			HealingWish = 36,
			HelpingHand = 37,
			HyperBeam = 38,
			Illusion = 39,
			Imprison = 40,
			Ingrain = 41,
			KingsShield = 42,
			LeechSeed = 43,
			LifeOrb = 44,
			LockOn = 45,
			LockOnPos = 46,
			LunarDance = 47,
			MagicCoat = 48,
			MagnetRise = 49,
			MeanLook = 50,
			MeFirst = 51,
			Metronome = 52,
			MicleBerry = 53,
			Minimize = 54,
			MiracleEye = 55,
			MirrorCoat = 56,
			MirrorCoatTarget = 57,
			MoveNext = 58,
			MudSport = 59,
			/// <summary>
			/// Trapping move
			/// </summary>
			MultiTurn = 60,
			MultiTurnAttack = 61,
			MultiTurnUser = 62,
			Nightmare = 63,
			Outrage = 64,
			ParentalBond = 65,
			PerishSong = 66,
			PerishSongUser = 67,
			PickupItem = 68,
			PickupUse = 69,
			/// <summary>
			/// Battle Palace only
			/// </summary>
			Pinch = 70,
			Powder = 71,
			PowerTrick = 72,
			Protect = 73,
			ProtectNegation = 74,
			ProtectRate = 75,
			Pursuit = 76,
			Quash = 77,
			Rage = 78,
			Revenge = 79,
			Roar = 80,
			Rollout = 81,
			Roost = 82,
			/// <summary>
			/// For when using Poké Balls/Poké Dolls
			/// </summary>
			SkipTurn = 83,
			SkyDrop = 84,
			SmackDown = 85,
			Snatch = 86,
			SpikyShield = 87,
			Stockpile = 88,
			StockpileDef = 89,
			StockpileSpDef = 90,
			Substitute = 91,
			Taunt = 92,
			Telekinesis = 93,
			Torment = 94,
			Toxic = 95,
			Transform = 96,
			Truant = 97,
			TwoTurnAttack = 98,
			Type3 = 99,
			Unburden = 100,
			Uproar = 101,
			Uturn = 102,
			WaterSport = 103,
			WeightChange = 104,
			Wish = 105,
			WishAmount = 106,
			WishMaker = 107,
			Yawn = 108
		}

		/// <summary>
		/// These effects apply to a side
		/// </summary>
		public enum SideEffects
		{
			CraftyShield = 0,
			EchoedVoiceCounter = 1,
			EchoedVoiceUsed = 2,
			LastRoundFainted = 3,
			LightScreen = 4,
			LuckyChant = 5,
			MatBlock = 6,
			Mist = 7,
			QuickGuard = 8,
			Rainbow = 9,
			Reflect = 10,
			Round = 11,
			Safeguard = 12,
			SeaOfFire = 13,
			Spikes = 14,
			StealthRock = 15,
			StickyWeb = 16,
			Swamp = 17,
			Tailwind = 18,
			ToxicSpikes = 19,
			WideGuard = 20
		}

		/// <summary>
		/// These effects apply to the battle (i.e. both sides)
		/// </summary>
		public enum FieldEffects
		{
			ElectricTerrain = 0,
			FairyLock = 1,
			FusionBolt = 2,
			FusionFlare = 3,
			GrassyTerrain = 4,
			Gravity = 5,
			IonDeluge = 6,
			MagicRoom = 7,
			MistyTerrain = 8,
			MudSportField = 9,
			TrickRoom = 10,
			WaterSportField = 11,
			WonderRoom = 12
		}

		/// <summary>
		/// These effects apply to the usage of a move
		/// </summary>
		public enum MoveEffects
		{
			SkipAccuracyCheck = 0,
			SpecialUsage = 1,
			PassedTrying = 2,
			TotalDamage = 3
		}
	}
}
