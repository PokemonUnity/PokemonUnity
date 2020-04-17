using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Localization;
using PokemonUnity.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
#pragma warning disable 0162 //Warning CS0162  Unreachable code detected 

	/// <summary>
	/// A Pokemon placeholder class to be used while in-battle, 
	/// to prevent changes from being permanent to original pokemon profile
	/// </summary>
	/// ToDo: Create a SaveResults() after battle has ended, to make changes permanent.
	public partial class Pokemon //: PokemonUnity.Monster.IPokemonBattle //PokemonUnity.Monster.Pokemon
	{
		#region Variables
		#region Battle Related
		public Battle battle					{ get; private set; }//{ return Game.battle; }
		/// <summary>
		/// Returns the position of this pkmn in battle lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == battle.party[this.pkmn.index]
		public sbyte Index						{ get; private set; }
		/// <summary>
		/// Index list of all pokemons who attacked this battler on this/previous turn
		/// </summary>
		public List<sbyte> lastAttacker			{ get; private set; }
		public int turncount					{ get; private set; }
		public Effects.Battler effects			{ get; private set; }
		/// <summary>
		/// Int Buffs and debuffs (gains and loss) affecting this pkmn.
		/// </summary>
		/// <remarks>
		/// 0: Attack, 1: Defense, 2: Speed, 3: SpAtk, 4: SpDef, 5: Evasion, 6: Accuracy
		/// </remarks>
		public int[] stages						{ get; private set; }//ToDo: sbyte?
		/// <summary>
		/// Participants will earn Exp. Points if this battler is defeated
		/// </summary>
		public List<byte> participants			{ get; private set; }
		public bool tookDamage					{ get; set; }
		public int lastHPLost					{ get; set; }
		public Moves lastMoveUsed				{ get; private set; }
		public Types lastMoveUsedType			{ get; private set; }
		public Moves lastMoveUsedSketch			{ get; private set; }
		public Moves lastRegularMoveUsed		{ get; private set; }
		private int? lastRoundMoved				{ get; set; }
		public List<Moves> movesUsed			{ get; set; }
		public Moves currentMove				{ get; set; }
		public Battle.DamageState damagestate	{ get; set; }
		public bool captured					{ get; private set; }
		#endregion
		#region Inherit Base Pokemon Data
		public int HP							{ get; set; }
		public int TotalHP						{ get; private set; }
		public int ATK							{ get { return effects.PowerTrick ? defense : attack; } }
		public int attack						{ get; set; }
		public int DEF {
			get
			{
				if (effects.PowerTrick) return attack;
				return battle.field.WonderRoom > 0 ? spdef : defense;
			}
		}
		public int defense						{ get; set; }
		public int SPD							{ get { return battle.field.WonderRoom > 0 ? defense : spdef; } }
		public int spdef						{ get; set; }
		public int SPA							{ get; private set; }
		public int SPE							{ get; private set; }
		public int spatk						{ get; set; }
		public int speed						{ get; set; }
		public int pbSpeed						{ get; set; }
		public byte[] baseStats					{ get; private set; }
		public int Level						{ get { return pokemon.Level; } }
		public int level						{ get; set; }
		public int happiness					{ get { return pokemon.Happiness; } }
		public string Name { get {
				//if name is not nickname return illusion.name?
				if (effects.Illusion != null)
					return effects.Illusion.Name;
				return name; } }
		private string name						{ get { return pokemon.Name; } }
		public bool? Gender { get {
				if (effects.Illusion != null)
					return effects.Illusion.Gender;
				return this.gender; } }
		private bool? gender { get; set; }
		public bool isHyperMode { get {
				if (effects.Illusion != null)
					return false; //effects.Illusion.IsShiny;
				return pokemon.isHyperMode; }
		}
		public bool IsShiny { get {
				if (effects.Illusion != null)
					return effects.Illusion.IsShiny;
				return isShiny; }
		}
		private bool isShiny { get; set; }
		public Pokemons Species { get { return Game.PokemonFormsData[pokemon.Species][form].Base; } }//ToDo: What about Illusion?
		public Monster.Data.Form Form { get { return Game.PokemonFormsData[Species][form]; } }// set { if (value >= 0 && value <= _base.Forms) _base.Form = value; } }
		public int form { get; internal set; } //ToDo: private get
		public int StatusCount { get; internal set; }
		public Status Status
		{
			get
			{
				return status; //ToDo: pokemon.Status
			}
			internal set
			{
				//ToDo: pokemon.Status = value
				if (status == Status.SLEEP && value == 0)
					effects.Truant = false;
				status = value;
				if (value != Status.POISON)
					effects.Toxic = 0;
				if (value != Status.POISON && value != Status.SLEEP)
					StatusCount = 0;
			}
		}
		private Status status { get; set; }
		public Items Item { get; set; } //ToDo: Fix this
		public Types Type1 { get; set; }
		public Types Type2 { get; set; }
		public byte[] IV { get; set; } //{ get { return pokemon.IV; } } //ToDo: Evasion?
		public Abilities Ability { get { return ability; } }
		internal Abilities ability { private get; set; }
		public PokemonUnity.Attack.Move[] moves { get; set; }
		#endregion
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
		public bool Fainted { get; private set; }
		public bool isFainted() { return true; } //HP == 0 || Status.FAINT || Fainted?
		public bool isEgg { get { return pokemon.isEgg; } }
		/// <summary>
		/// Returns the position of this pkmn in party lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == party[this.pkmn.index]
		public sbyte pokemonIndex { get; private set; }
		public bool IsOwned { get { return Game.GameData.Player.Pokedex[(byte)Species, 1] == 1; } }
		private PokemonUnity.Monster.Pokemon pokemon { get; set; }

		public int GetWeight(Pokemon attacker = null)
		{
			float w = Game.PokemonData[Form.Pokemon].Weight;
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

		public bool IsMega { get { return Game.PokemonFormsData[Species][form].IsMega; } }
		//public override bool hasMegaForm { get { if (effects.Transform) return false; return base.hasMegaForm; } }
		public bool IsPrimal { get; private set; }
		//public override bool hasPrimalForm { get { if (effects.Transform) return false; return base.hasPrimalForm; } }
		#endregion

		#region Constructors
		//public Pokemon(PokemonUnity.Monster.Pokemon replacingPkmn, bool batonpass)
		//{
		//	Initialize(replacingPkmn, replacingPkmn.Index, batonpass);
		//}
		//[Obsolete("Don't think this is needed or should be used")]
		public Pokemon(Battle btl, sbyte idx) //: base() 
		{
			battle			= btl;
			Index			= idx;
			HP				= 0;
			TotalHP			= 0;
			Fainted			= true;
			captured		= false;
			stages			= new int[Enum.GetValues(typeof(PokemonUnity.Battle.Stats)).Length];
			effects			= new Effects.Battler(false);
			damagestate		= new Battle.DamageState();
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
		public Pokemon Initialize(PokemonUnity.Monster.Pokemon pkmn, sbyte index, bool batonpass = false) //: base(pkmn)
		{
			//Cure status of previous Pokemon with Natural Cure
			if (this.hasWorkingAbility(Abilities.NATURAL_CURE))
				this.Status = 0;
			if (this.hasWorkingAbility(Abilities.REGENERATOR))
				this.RecoverHP((int)Math.Floor((decimal)this.TotalHP / 3));
			InitPokemon(pkmn, index);
			InitEffects(batonpass);
			return this;
		}
		private void InitBlank()
		{
			//Pokemon blank = new Pokemon();
			//Level = 0;
			pokemonIndex = -1;
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
				stages = new int[7];//Enum.GetValues(typeof(PokemonUnity.Battle.Stats)).Length
				lastMoveUsedSketch 	= Moves.NONE; //-1;
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
				
				//Moved to Property Getter
				//if (effects.PowerTrick)
				//{
				//	int a = this.ATK;
				//	this.ATK = this.DEF;
				//	this.DEF = this.ATK;
				//	this.DEF = a;
				//}
			}			
			damagestate.Reset();
			Fainted						= false;
			lastAttacker				= new List<sbyte>();
			lastHPLost					= 0;
			tookDamage					= false;
			lastMoveUsed				= Moves.NONE;
			lastMoveUsedType			= Types.NONE; //-1;
			lastRoundMoved				= -1;
			movesUsed					= new List<Moves>();
			battle.turncount			= 0;
			effects.Attract				= -1;
			effects.BatonPass			= false;
			effects.Bide				= 0;
			effects.BideDamage			= 0;
			effects.BideTarget			= -1;
			effects.Charge				= 0;
			effects.ChoiceBand			= null;
			effects.Counter				= -1;
			effects.CounterTarget		= -1;
			effects.DefenseCurl			= false;
			effects.DestinyBond			= false;
			effects.Disable				= 0;
			effects.DisableMove			= 0;
			effects.Electrify			= false;
			effects.Encore				= 0;
			effects.EncoreIndex			= 0;
			effects.EncoreMove			= 0;
			effects.Endure				= false;
			effects.FirstPledge			= 0;
			effects.FlashFire			= false;
			effects.Flinch				= false;
			effects.FollowMe			= 0;
			effects.Foresight			= false;
			effects.FuryCutter			= 0;
			effects.Grudge				= false;
			effects.HelpingHand			= false;
			effects.HyperBeam			= 0;
			effects.Illusion			= null;
			effects.Imprison			= false;
			effects.KingsShield			= false;
			effects.LifeOrb				= false;
			effects.MagicCoat			= false;
			effects.MeanLook			= -1;
			effects.MeFirst				= false;
			effects.Metronome			= 0;
			effects.MicleBerry			= false;
			effects.Minimize			= false;
			effects.MiracleEye			= false;
			effects.MirrorCoat			= -1;
			effects.MirrorCoatTarget	= -1;
			effects.MoveNext			= false;
			effects.MudSport			= false;
			effects.MultiTurn			= 0;
			effects.MultiTurnAttack		= 0;
			effects.MultiTurnUser		= -1;
			effects.Nightmare			= false;
			effects.Outrage				= 0;
			effects.ParentalBond		= 0;
			effects.PickupItem			= 0;
			effects.PickupUse			= 0;
			effects.Pinch				= false;
			effects.Powder				= false;
			effects.Protect				= false;
			effects.ProtectNegation		= false;
			effects.ProtectRate			= 1;
			effects.Pursuit				= false;
			effects.Quash				= false;
			effects.Rage				= false;
			effects.Revenge				= 0;
			effects.Roar				= false;
			effects.Rollout				= 0;
			effects.Roost				= false;
			effects.SkipTurn			= false;
			effects.SkyDrop				= false;
			effects.SmackDown			= false;
			effects.Snatch				= false;
			effects.SpikyShield			= false;
			effects.Stockpile			= 0;
			effects.StockpileDef		= 0;
			effects.StockpileSpDef		= 0;
			effects.Taunt				= 0;
			effects.Torment				= false;
			effects.Toxic				= 0;
			effects.Transform			= false;
			effects.Truant				= false;
			effects.TwoTurnAttack		= 0;
			effects.Type3				= Types.NONE; //-1;
			effects.Unburden			= false;
			effects.Uproar				= 0;
			effects.Uturn				= false;
			effects.WaterSport			= false;
			effects.WeightChange		= 0;
			effects.Yawn				= 0;
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
				//ToDo: Uncomment below
				int lastpoke = 0; //battle.GetLastPokeInTeam(Index);
				if (lastpoke != pokemonIndex){
					effects.Illusion = battle.pbParty(Index)[lastpoke];
				}
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
		private void InitPokemon(PokemonUnity.Monster.Pokemon pkmn, sbyte pkmnIndex)
		{
			if (pkmn.isEgg)
			{
				//Remove/Disable UI for Egg 
				//Pause game to display error message?
				//"An egg can't be an active Pokémon"
				//Game.Dialog(LanguageExtension.Translate(Text.Errors, "ActiveEgg").Value);
			}
			else
			{
				//name			= pkmn.Name;
				//Species		= pkmn.Species;
				//Level			= pkmn.Level;
				HP				= pkmn.HP;
				TotalHP			= pkmn.TotalHP;
				gender			= pkmn.Gender;
				ability			= pkmn.Ability;
				Item			= pkmn.Item;
				Type1			= pkmn.Type1;
				Type2			= pkmn.Type2;
				form			= pkmn.Form;
				attack			= pkmn.ATK;
				defense			= pkmn.DEF;
				speed			= pkmn.SPE;
				spatk			= pkmn.SPA;
				spdef			= pkmn.SPD;
				Status			= pkmn.Status;
				StatusCount		= pkmn.StatusCount;
				pokemon			= pkmn;
				Index			= pkmnIndex;
				participants	= new List<byte>();
				moves			= new PokemonUnity.Attack.Move[] {
					(PokemonUnity.Attack.Move)pkmn.moves[0],
					(PokemonUnity.Attack.Move)pkmn.moves[1],
					(PokemonUnity.Attack.Move)pkmn.moves[2],
					(PokemonUnity.Attack.Move)pkmn.moves[3]
				};
			}
		}
		public void Update(bool fullchange = false)
		{
			if(Species != Pokemons.NONE)
			{
				//calcStats(); //Not needed since fetching stats from base ( Pokemon => Battler )
				//ToDo: Uncomment and fetch data from baseClass
				//Level		= pokemon.Level;
				HP			= pokemon.HP;
				TotalHP		= pokemon.TotalHP;
				//Pokemon	= Pokemon; //so not all stats need to be handpicked
				if (!effects.Transform) //Changed forms but did not transform?
				{
					attack		= pokemon.ATK;
					defense		= pokemon.DEF;
					speed		= pokemon.SPE;
					spatk		= pokemon.SPA;
					spdef		= pokemon.SPD;
					if (fullchange)
					{
						ability		= pokemon.Ability;
						Type1		= pokemon.Type1;
						Type2		= pokemon.Type2;
					}
				}
			}
			//return this;
		}
		/// <summary>
		/// Used only to erase the battler of a Shadow Pokémon that has been snagged.
		/// </summary>
		public Pokemon Reset()
		{
			pokemon		= new PokemonUnity.Monster.Pokemon();
			Index		= -1;
			InitEffects(false);
			//reset status
			Status		= Status.NONE; //ToDo: Status.FAINT?
			StatusCount	= 0;
			Fainted		= true;
			//reset choice
			battle.choices[Index] = new Battle.Choice(ChoiceAction.NoAction);
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

		public bool hasType(Types type) {
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

		//public bool HasMoveFunction(string code) {
		//	if (string.IsNullOrEmpty(code)) return false;
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		if (moves[i].FunctionAsString == code) return true;
		//	}
		//	return false;
		//}
		//
		//public bool HasMoveFunction(short code) {
		//	//if (string.IsNullOrEmpty(code)) return false;
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		if ((short)moves[i].Function == code) return true;
		//	}
		//	return false;
		//}
		//
		//public bool HasMoveFunction(Move.Effect code) {
		//	//if (string.IsNullOrEmpty(code)) return false;
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		if ((Move.Effect)moves[i].Function == code) return true;
		//	}
		//	return false;
		//}

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
			return Game.ItemData[this.Item].IsBerry;//.Pocket == ItemPockets.BERRY;//pbIsBerry?(@item)
		}

		public bool isAirborne(bool ignoreability=false){
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

		public int Speed()
		{
			int[] stagemul = new int[] { 10, 10, 10, 10, 10, 10, 10, 15, 20, 25, 30, 35, 40 };
			int[] stagediv = new int[] { 40, 35, 30, 25, 20, 15, 10, 10, 10, 10, 10, 10, 10 };
			int speed = 0;
			int stage = stages[2] + 6;
			speed = (int)Math.Floor(speed * (decimal)stagemul[stage] / stagediv[stage]);
			int speedmult = 0x1000;
			switch (battle.weather)
			{
				case Weather.RAINDANCE:
				case Weather.HEAVYRAIN:
					speedmult = hasWorkingAbility(Abilities.SWIFT_SWIM) ? speedmult * 2 : speedmult;
					break;
				case Weather.SUNNYDAY:
				case Weather.HARSHSUN:
					speedmult = hasWorkingAbility(Abilities.CHLOROPHYLL) ? speedmult * 2 : speedmult;
					break;
				case Weather.SANDSTORM:
					speedmult = hasWorkingAbility(Abilities.SAND_RUSH) ? speedmult * 2 : speedmult;
					break;
				default:
					break;
			}
			if (hasWorkingAbility(Abilities.QUICK_FEET) && Status > 0)
				speedmult = (int)Math.Round(speedmult * 1.5f);
			if (hasWorkingAbility(Abilities.UNBURDEN) && effects.Unburden && Item == Items.NONE)
				speedmult = speedmult * 2;
			if (hasWorkingAbility(Abilities.SLOW_START) && battle.turncount > 0)
				speedmult = (int)Math.Round(speedmult * 1.5f);
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
				battle.OwnedByPlayer(Index) &&
				Game.GameData.Player.BadgesCount >= Core.BADGESBOOSTSPEED)
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
				battle.pbDisplay//Game.Dialog
					(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
			if (HP > TotalHP)
				//"HP greater than total HP"
				battle.pbDisplay//Game.Dialog
					(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
			//ToDo: Pass to UnityEngine
			if (amt > 0)
				battle.scene.HPChanged(Index, oldhp, animate); //Unity takes over
			if (amt > 0 && registerDamage)
				tookDamage = true;
			return amt;
		}

		public int RecoverHP(int amount, bool animate = false)
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
				battle.pbDisplay//Game.Dialog
					(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
			if (HP > TotalHP)
				//"HP greater than total HP"
				battle.pbDisplay//Game.Dialog
					(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
			//ToDo: Pass to UnityEngine
			if(amount > 0)
				battle.scene.HPChanged(Index, oldhp, animate); //Unity takes over
			//ToDo: Fix return
			return amount;
		}

		public void Faint(bool showMessage = true)
		{
			if(!isFainted() && HP > 0)
			{
				GameDebug.LogWarning("Can't faint with HP greater than 0");
				return; //true;
			}
			if(isFainted())
			{
				GameDebug.LogWarning("Can't faint if already fainted");
				return; //true;
			}
			battle.scene.Fainted(Index);
			InitEffects(false);
			// Reset status
			//Status = 0;
			StatusCount = 0;
			if (pokemon != null && battle.internalbattle)
				pokemon.ChangeHappiness(HappinessMethods.FAINT);
			if (IsMega)
				//Change form to before transformation
				//pokemon.MakeUnmega;
				form = pokemon.Form;
			if (IsPrimal)
				//Change form to before transformation
				//pokemon.MakeUnprimal;
				form = pokemon.Form;
			//Fainted = true;
			Status = Status.FAINT;
			//reset choice
			battle.choices[Index] = new Battle.Choice(ChoiceAction.NoAction);
			OwnSide.LastRoundFainted = battle.turncount;
			if (showMessage)
				battle.pbDisplay//Game.Dialog
					(LanguageExtension.Translate(Text.Errors, "Fainted", new string[] { ToString() }).Value);
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
		public Pokemon Partner { get { return battle.battlers[(Index & 1) | ((Index & 2) ^ 2)]; } }
		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		public Pokemon OppositeOpposing { get { return battle.battlers[(Index ^ 1)]; } }
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
		/// </summary>
		/// ToDo: Changes stats on form changes here?
		/// ToDo: Use PokemonUnity.Battle.Form to modify Pokemon._base, 
		/// Which will override and modify base stats for values that inherit it
		/// Castform and Unown should use (int)Form, and others will use (PokemonData)Form
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
							if(Form.Id != Monster.Forms.CASTFORM_SUNNY)
							{
								form = 1;
								transformed = true;
							}
							break;
						case Weather.RAINDANCE:
						case Weather.HEAVYRAIN:
							if(Form.Id != Monster.Forms.CASTFORM_RAINY)
							{
								form = 2;
								transformed = true;
							}
							break;
						case Weather.HAIL:
							if(Form.Id != Monster.Forms.CASTFORM_SNOWY)
							{
								form = 3;
								transformed = true;
							}
							break;
						case Weather.NONE:
						default:
							if(Form.Id != Monster.Forms.CASTFORM)
							{
								form = 0;
								transformed = true; //Shouldn't normal be false?
							}
							break;
					}
				} else
					if(Form.Id != Monster.Forms.CASTFORM)
					{
						form = 0;
						transformed = true; //Shouldn't normal be false?
					}
			}
			if (Species == Pokemons.SHAYMIN)
			{
				if (form != pokemon.Form)
				{
					form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.GIRATINA)
			{
				if (form != pokemon.Form)
				{
					form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.ARCEUS && Ability == Abilities.MULTITYPE)
			{
				if (form != pokemon.Form)
				{
					form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.DARMANITAN)
			{
				if(hasWorkingAbility(Abilities.ZEN_MODE) && HP <= Math.Floor(TotalHP/2f))
					if (Form.Id != Monster.Forms.DARMANITAN_ZEN)
					{
						form = 1;
						transformed = true;
					}
				else
					if (Form.Id != Monster.Forms.DARMANITAN_STANDARD)
					{
						form = 0;
						transformed = true;
					}
			}
			if (Species == Pokemons.KELDEO)
			{
				if (form != pokemon.Form)
				{
					form = pokemon.Form;
					transformed = true;
				}
			}
			if (Species == Pokemons.GENESECT)
			{
				if (form != pokemon.Form)
				{
					form = pokemon.Form;
					transformed = true;
				}
			}
			if (transformed)
			{
				Update(true);
				battle.scene.ChangePokemon();
				battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "Transformed", ToString()).Value);
				GameDebug.Log(string.Format("[Form changed] {0} changed to form {1}", ToString(), Form.Id.ToString(TextScripts.Name)));
			}
		}
		public void ResetForm()
		{
			if (!effects.Transform){
				if (Species == Pokemons.CASTFORM ||
					Species == Pokemons.CHERRIM ||
					Species == Pokemons.DARMANITAN ||
					Species == Pokemons.MELOETTA ||
					Species == Pokemons.AEGISLASH ||
					Species == Pokemons.XERNEAS)
					form = 0;
			}
			Update(true);
		}
		#endregion

		#region Ability Effects
		public void AbilitiesOnSwitchIn(bool onactive)
		{
			if (isFainted()) return;
			//if (onactive)
			//	battle.PrimalReversion(Index);
			#region Weather
			if (onactive)
			{
				if(hasWorkingAbility(Abilities.PRIMORDIAL_SEA) && battle.Weather != Weather.HEAVYRAIN)
				{
					battle.SetWeather(Weather.HEAVYRAIN);
					battle.weatherduration = -1;
					battle.pbCommonAnimation("HeavyRain", null, null);
					//Output Below: "{1}'s {2} made a heavy rain begin to fall!"
					battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HeavyRainStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameDebug.Log(string.Format("[Ability triggered] {0}'s Primordial Sea made it rain heavily", ToString()));
				}
				if(hasWorkingAbility(Abilities.DESOLATE_LAND) && battle.Weather != Weather.HARSHSUN)
				{
					battle.SetWeather(Weather.HARSHSUN);
					battle.weatherduration = -1;
					battle.pbCommonAnimation("HarshSun", null, null);
					//Output Below: "{1}'s {2} turned the sunlight extremely harsh!"
					battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HarshSunStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameDebug.Log(string.Format("[Ability triggered] {0}'s Desolate Land made the sun shine harshly", ToString()));
				}
				if(hasWorkingAbility(Abilities.DELTA_STREAM) && battle.Weather != Weather.STRONGWINDS)
				{
					battle.SetWeather(Weather.STRONGWINDS);
					battle.weatherduration = -1;
					battle.pbCommonAnimation("StrongWinds", null, null);
					//Output Below: "{1}'s {2} caused a mysterious air current that protects Flying-type Pokémon!"
					battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "StrongWindsStart", ToString(), Ability.ToString().Translate().Value).Value);
					GameDebug.Log(string.Format("[Ability triggered] {0}'s Delta Stream made an air current blow", ToString()));
				}
				if (battle.Weather != Weather.HEAVYRAIN &&
					battle.Weather != Weather.HARSHSUN &&
					battle.Weather != Weather.STRONGWINDS)
				{
					if (hasWorkingAbility(Abilities.DRIZZLE) && 
						(battle.Weather != Weather.RAINDANCE || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.RAINDANCE);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.DAMP_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Rain", null, null);
						//Output Below: "{1}'s {2} made it rain!"
						battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "RainStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Drizzle made it rain", ToString()));
					}
					if (hasWorkingAbility(Abilities.DROUGHT) && 
						(battle.Weather != Weather.SUNNYDAY || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.SUNNYDAY);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.HEAT_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Sunny", null, null);
						//Output Below: "{1}'s {2} intensified the sun's rays!"
						battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SunnyStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Drought made it sunny", ToString()));
					}
					if (hasWorkingAbility(Abilities.SAND_STREAM) && 
						(battle.Weather != Weather.SANDSTORM || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.SANDSTORM);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.SMOOTH_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Sandstorm", null, null);
						//Output Below: "{1}'s {2} whipped up a sandstorm!"
						battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SandstormStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Sand Stream made it sandstorm", ToString()));
					}
					if (hasWorkingAbility(Abilities.SNOW_WARNING) && 
						(battle.Weather != Weather.HAIL || battle.weatherduration != -1))
					{
						battle.SetWeather(Weather.HAIL);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.ICY_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.pbCommonAnimation("Hail", null, null);
						//Output Below: "{1}'s {2} madeit hail!"
						battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HailStart", ToString(), Ability.ToString().Translate().Value).Value);
						GameDebug.Log(string.Format("[Ability triggered] {0}'s Snow Warning made it hail", ToString()));
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
					GameDebug.Log(string.Format("[Ability nullified] {0}'s Ability cancelled weather effects", ToString()));
				}
			}
			#endregion Weather
			//battle.PrimordialWeather();
			#region Trace
			if (hasWorkingAbility(Abilities.TRACE))
			{
				//Choice[] choices = new Choice[4];
				List<int> choices = new List<int>();
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					Pokemon foe = battle.battlers[i];
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
					int choice = choices[@battle.pbRandom(choices.Count)];
					string battlername = @battle.battlers[choice].ToString(true);
					Abilities battlerability = @battle.battlers[choice].ability;
					@ability = battlerability;
					string abilityname = battlerability.ToString();
					@battle.pbDisplay(_INTL("{1} traced {2}'s {3}!", ToString(), battlername, abilityname));
					GameDebug.Log($"[Ability triggered] #{ToString()}'s Trace turned into #{abilityname} from #{battlername}");
				}
			}
			#endregion Trace
            #region Intimidate
            #endregion Intimidate
    if (this.hasWorkingAbility(Abilities.INTIMIDATE) && onactive) {
      GameDebug.Log($"[Ability triggered] #{ToString()}'s Intimidate");
      for (int i = 0; i < 4; i++)
        if (pbIsOpposing(i) && !@battle.battlers[i].isFainted())
          @battle.battlers[i].pbReduceAttackStatIntimidate(this);
    }
    // Download
    if (this.hasWorkingAbility(Abilities.DOWNLOAD) && onactive) {
      int odef = 0; int ospdef = 0;
      if (pbOpposing1 && !pbOpposing1.isFainted()) {
        odef+=pbOpposing1.defense;
        ospdef+=pbOpposing1.spdef;
      }
      if (pbOpposing2 && !pbOpposing2.isFainted()) {
        odef+=pbOpposing2.defense;
        ospdef+=pbOpposing1.spdef;
      }
      if (ospdef>odef) 
        if (pbIncreaseStatWithCause(Stats.ATTACK,1,this,ability.ToString(TextScripts.Name)))
          GameDebug.Log($"[Ability triggered] #{ToString()}'s Download (raising Attack)");
      else
        if (pbIncreaseStatWithCause(Stats.SPATK,1,this,ability.ToString(TextScripts.Name)))
          GameDebug.Log($"[Ability triggered] #{ToString()}'s Download (raising Special Attack)");
    }
    // Frisk
    if (this.hasWorkingAbility(Abilities.FRISK) && @battle.pbOwnedByPlayer(@index) && onactive) {
      List<Pokemon> foes= new List<Pokemon>();
      if (pbOpposing1.Item>0 && !pbOpposing1.isFainted()) foes.Add(pbOpposing1);
      if (pbOpposing2.Item>0 && !pbOpposing2.isFainted()) foes.Add(pbOpposing2);
      if (Core.USENEWBATTLEMECHANICS) {
        if (foes.Length>0) GameDebug.Log($"[Ability triggered] #{ToString()}'s Frisk");
        foreach (var i in foes) {
          string itemname=i.Item.ToString(TextScripts.Name);
          @battle.pbDisplay(_INTL("{1} frisked {2} and found its {3}!",ToString(),i.ToString(true),itemname));
        }
      }else if (foes.Length>0) {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s Frisk");
        Pokemon foe=foes[@battle.pbRandom(foes.Length)];
        string itemname=foe.Item.ToString(TextScripts.Name);
        @battle.pbDisplay(_INTL("{1} frisked the foe and found one {2}!",ToString(),itemname));
      }
    }
    // Anticipation
    if (this.hasWorkingAbility(Abilities.ANTICIPATION) && @battle.pbOwnedByPlayer(@index) && onactive) {
      GameDebug.Log($"[Ability triggered] #{ToString()} has Anticipation");
      bool found=false;
      foreach (var foe in [pbOpposing1,pbOpposing2]) {
        if (foe.isFainted()) continue; 
        foreach (var j in foe.moves) {
          Attack.Data.MoveData movedata=Game.MoveData[j.id];
          float eff=movedata.Type.GetCombinedEffectiveness(Type1,Type2,@effects.Type3);
          if ((movedata.Power>0 && eff>8) ||
             ((int)movedata.Effect==0x70 && eff>0)) { // OHKO
            found=true;
            break;
          }
        }
        if (found) break;
      }
      if (found) @battle.pbDisplay(_INTL("{1} shuddered with anticipation!",ToString()));
    }
    // Forewarn
    if (this.hasWorkingAbility(Abilities.FOREWARN) && @battle.pbOwnedByPlayer(@index) && onactive) {
      GameDebug.Log($"[Ability triggered] #{ToString()} has Forewarn");
      int highpower=0;
      List<Moves> fwmoves= new List<Moves>();
      foreach (var foe in new Pokemon[] { pbOpposing1, pbOpposing2 }) {
        if (foe.isFainted()) continue; 
        foreach (var j in foe.moves) {
          Attack.Data.MoveData movedata=Game.MoveData[j.MoveId];
          int power=movedata.Power??0;
          if ((int)movedata.Effect==0x70) power=160;    // OHKO
          if ((int)movedata.Effect==0x8B) power=150;    // Eruption
          if ((int)movedata.Effect==0x71 || // Counter
                       (int)movedata.Effect==0x72 || // Mirror Coat
                       (int)movedata.Effect==0x73) power=120;// || // Metal Burst
          if ((int)movedata.Effect==0x6A ||  // SonicBoom
                      (int)movedata.Effect==0x6B ||  // Dragon Rage
                      (int)movedata.Effect==0x6D ||  // Night Shade
                      (int)movedata.Effect==0x6E ||  // Endeavor
                      (int)movedata.Effect==0x6F ||  // Psywave
                      (int)movedata.Effect==0x89 ||  // Return
                      (int)movedata.Effect==0x8A ||  // Frustration
                      (int)movedata.Effect==0x8C ||  // Crush Grip
                      (int)movedata.Effect==0x8D ||  // Gyro Ball
                      (int)movedata.Effect==0x90 ||  // Hidden Power
                      (int)movedata.Effect==0x96 ||  // Natural Gift
                      (int)movedata.Effect==0x97 ||  // Trump Card
                      (int)movedata.Effect==0x98 ||  // Flail
                      (int)movedata.Effect==0x9A) power=80;     // Grass Knot
          if (power > highpower) { 
            fwmoves=new List<Moves>() { j.MoveId }; highpower=power;
          }else if (power==highpower) 
            fwmoves.Add(j.MoveId);
        }
      }
      if (fwmoves.Length>0) {
        Moves fwmove=fwmoves[@battle.pbRandom(fwmoves.Length)];
        string movename=fwmove.ToString(TextScripts.Name);
        @battle.pbDisplay(_INTL("{1}'s Forewarn alerted it to {2}!",ToString(),movename));
      }
    }
    // Pressure message
    if (this.hasWorkingAbility(Abilities.PRESSURE) && onactive)
      @battle.pbDisplay(_INTL("{1} is exerting its pressure!",ToString()));
    // Mold Breaker message
    if (this.hasWorkingAbility(Abilities.MOLD_BREAKER) && onactive)
      @battle.pbDisplay(_INTL("{1} breaks the mold!",ToString()));
    // Turboblaze message
    if (this.hasWorkingAbility(Abilities.TURBOBLAZE) && onactive)
      @battle.pbDisplay(_INTL("{1} is radiating a blazing aura!",ToString()));
    // Teravolt message
    if (this.hasWorkingAbility(Abilities.TERAVOLT) && onactive)
      @battle.pbDisplay(_INTL("{1} is radiating a bursting aura!",ToString()));
    // Dark Aura message
    if (this.hasWorkingAbility(Abilities.DARK_AURA) && onactive)
      @battle.pbDisplay(_INTL("{1} is radiating a dark aura!",ToString()));
    // Fairy Aura message
    if (this.hasWorkingAbility(Abilities.FAIRY_AURA) && onactive)
      @battle.pbDisplay(_INTL("{1} is radiating a fairy aura!",ToString()));
    // Aura Break message
    if (this.hasWorkingAbility(Abilities.AURA_BREAK) && onactive)
      @battle.pbDisplay(_INTL("{1} reversed all other Pokémon's auras!",ToString()));
    // Imposter
    if (this.hasWorkingAbility(Abilities.IMPOSTER) && !@effects.Transform && onactive) {
      Pokemon choice=pbOppositeOpposing;
      List<int> blacklist=  new List<int>() {
         0xC9,    // Fly
         0xCA,    // Dig
         0xCB,    // Dive
         0xCC,    // Bounce
         0xCD,    // Shadow Force
         0xCE,    // Sky Drop
         0x14D    // Phantom Force
      };
      if (choice.effects.Transform ||
         choice.effects.Illusion.IsNotNullOrNone() ||
         choice.effects.Substitute>0 ||
         choice.effects.SkyDrop ||
         blacklist.Contains((int)Game.MoveData[choice.effects.TwoTurnAttack].Effect))
        GameDebug.Log($"[Ability triggered] #{ToString()}'s Imposter couldn't transform");
      else {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s Imposter");
        @battle.pbAnimation(Moves.TRANSFORM,this,choice);
        @effects.Transform=true;
        @Type1=choice.Type1;
        @Type2=choice.Type2;
        @effects.Type3=Types.NONE;
        @ability=choice.ability;
        @attack=choice.attack;
        @defense=choice.defense;
        @speed=choice.speed;
        @spatk=choice.spatk;
        @spdef=choice.spdef;
        foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
                  Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION })
          @stages[(int)i]=choice.stages[(int)i];
        for (int i = 0; i < 4; i++) {
          @moves[i]=PokeBattle_Move.pbFromPBMove(@battle,new Move(choice.moves[i].MoveId));
          @moves[i].PP=5;
          //@moves[i].TotalPP=5;
        }
        @effects.Disable=0;
        @effects.DisableMove=0;
        @battle.pbDisplay(_INTL("{1} transformed into {2}!",ToString(),choice.ToString(true)));
        GameDebug.Log($"[Pokémon transformed] #{ToString()} transformed into #{choice.ToString(true)}");
      }
    }
    // Air Balloon message
    if (this.hasWorkingItem(Items.AIR_BALLOON) && onactive)
      @battle.pbDisplay(_INTL("{1} floats in the air with its {2}!",ToString(),this.Item.ToString(TextScripts.Name)));
  }
  public void pbEffectsOnDealingDamage(Move move, Pokemon user,Pokemon target,int damage) {
    Types movetype=move.pbType(move.Type,user,target);
    if (damage>0 && move.isContactMove())
      if (!target.damagestate.Substitute) {
        if (target.hasWorkingItem(Items.STICKY_BARB,true) && user.Item==0 && !user.isFainted()) {
          user.Item=target.Item;
          target.Item=0;
          target.effects.Unburden=true;
          if (!@battle.opponent && !@battle.pbIsOpposing(user.Index))
            if (user.pokemon.itemInitial==0 && target.pokemon.itemInitial==user.Item) {
              user.pokemon.itemInitial=user.Item;
              target.pokemon.itemInitial=0;
            }
          @battle.pbDisplay(_INTL("{1}'s {2} was transferred to {3}!",
             target.ToString(),user.Item.ToString(TextScripts.Name),user.ToString(true)));
          GameDebug.Log($"[Item triggered] #{target.ToString()}'s Sticky Barb moved to #{user.ToString(true)}");
        }
        if (target.hasWorkingItem(Items.ROCKYHELMET,true) && !user.isFainted())
          if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s Rocky Helmet");
            @battle.scene.pbDamageAnimation(user,0);
            user.ReduceHP((user.TotalHP/6).floor);
            @battle.pbDisplay(_INTL("{1} was hurt by the {2}!",user.ToString(),
               target.Item.ToString(TextScripts.Name)));
          }
        if (target.hasWorkingAbility(Abilities.AFTERMATH,true) && target.isFainted() &&
           !user.isFainted())
          if (!@battle.pbCheckGlobalAbility(Abilities.DAMP) &&
             !user.hasMoldBreaker() && !user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Aftermath");
            @battle.scene.pbDamageAnimation(user,0);
            user.ReduceHP((user.TotalHP/4).floor);
            @battle.pbDisplay(_INTL("{1} was caught in the aftermath!",user.ToString()));
          }
        if (target.hasWorkingAbility(Abilities.CUTE_CHARM) && @battle.pbRandom(10)<3)
          if (!user.isFainted() && user.pbCanAttract(target,false)) {
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Cute Charm");
            user.pbAttract(target,_INTL("{1}'s {2} made {3} fall in love!",target.ToString(),
               target.ability.ToString(TextScripts.Name),user.ToString(true)));
          }
        if (target.hasWorkingAbility(Abilities.EFFECT_SPORE,true) && @battle.pbRandom(10)<3)
          if (Core.USENEWBATTLEMECHANICS &&
             (user.hasType(Types.GRASS) ||
             user.hasWorkingAbility(Abilities.OVERCOAT) ||
             user.hasWorkingItem(Items.SAFETY_GOGGLES))) { //Not sure what goes here
          } else {
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Effect Spore");
            switch (@battle.pbRandom(3)) {
            case 0:
              if (user.pbCanPoison(null,false))
                user.pbPoison(target,_INTL("{1}'s {2} poisoned {3}!",target.ToString(),
                   target.ability.ToString(TextScripts.Name),user.ToString(true)));
              break;
            case 1:
              if (user.pbCanSleep(null,false))
                user.pbSleep(_INTL("{1}'s {2} made {3} fall asleep!",target.ToString(),
                   target.ability.ToString(TextScripts.Name),user.ToString(true)));
              break;
            case 2:
              if (user.pbCanParalyze(null,false))
                user.pbParalyze(target,_INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
                   target.ToString(),target.ability.ToString(TextScripts.Name),user.ToString(true)));
              break;
            }
          }
        if (target.hasWorkingAbility(Abilities.FLAME_BODY,true) && @battle.pbRandom(10)<3 &&
           user.pbCanBurn(null,false)) {
          GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Flame Body");
          user.pbBurn(target,_INTL("{1}'s {2} burned {3}!",target.ToString(),
             target.ability.ToString(TextScripts.Name),user.ToString(true)));
        }
        if (target.hasWorkingAbility(Abilities.MUMMY,true) && !user.isFainted()) 
          if (user.ability != Abilities.MULTITYPE &&
             user.ability != Abilities.STANCE_CHANGE &&
             user.ability != Abilities.MUMMY) {
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Mummy copied onto #{user.ToString(true)}");
            user.ability=Abilities.MUMMY;// || 0;
            @battle.pbDisplay(_INTL("{1} was mummified by {2}!",
               user.ToString(),target.ToString(true)));
          }
        if (target.hasWorkingAbility(Abilities.POISON_POINT,true) && @battle.pbRandom(10)<3 &&
           user.pbCanPoison(null,false)) {
          GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Poison Point");
          user.pbPoison(target,_INTL("{1}'s {2} poisoned {3}!",target.ToString(),
             target.ability.ToString(TextScripts.Name),user.ToString(true)));
        }
        if ((target.hasWorkingAbility(Abilities.ROUGH_SKIN,true) ||
           target.hasWorkingAbility(Abilities.IRON_BARBS,true)) && !user.isFainted())
          if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s #{target.ability.ToString(TextScripts.Name)}");
            @battle.scene.pbDamageAnimation(user,0);
            user.ReduceHP((user.TotalHP/8).floor);
            @battle.pbDisplay(_INTL("{1}'s {2} hurt {3}!",target.ToString(),
               target.ability.ToString(TextScripts.Name),user.ToString(true)));
          }
        if (target.hasWorkingAbility(Abilities.STATIC,true) && @battle.pbRandom(10)<3 &&
           user.pbCanParalyze(null,false)) {
          GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Static");
          user.pbParalyze(target,_INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
             target.ToString(),target.ability.ToString(TextScripts.Name),user.ToString(true)));
        }
        if (target.hasWorkingAbility(Abilities.GOOEY,true))
          if (user.pbReduceStatWithCause(Stats.SPEED,1,target,target.ability.ToString(TextScripts.Name)))
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Gooey");
        if (user.hasWorkingAbility(Abilities.POISON_TOUCH,true) &&
           target.pbCanPoison(null,false) && @battle.pbRandom(10)<3) {
          GameDebug.Log($"[Ability triggered] #{user.ToString()}'s Poison Touch");
          target.pbPoison(user,_INTL("{1}'s {2} poisoned {3}!",user.ToString(),
             user.ability.ToString(TextScripts.Name),target.ToString(true)));
        }
      }
    if (damage>0) {
      if (!target.damagestate.Substitute) {
        if (target.hasWorkingAbility(Abilities.CURSED_BODY,true) && @battle.pbRandom(10)<3)
          if (user.effects.Disable<=0 && move.PP>0 && !user.isFainted()) {
            user.effects.Disable=3;
            user.effects.DisableMove=move.MoveId;
            @battle.pbDisplay(_INTL("{1}'s {2} disabled {3}!",target.ToString(),
               target.ability.ToString(TextScripts.Name),user.ToString(true)));
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Cursed Body disabled #{user.ToString(true)}");
          }
        if (target.hasWorkingAbility(Abilities.JUSTIFIED) && movetype == Types.DARK)
          if (target.pbIncreaseStatWithCause(Stats.ATTACK,1,target,target.ability.ToString(TextScripts.Name)))
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Justified");
        if (target.hasWorkingAbility(Abilities.RATTLED) &&
           (movetype == Types.BUG ||
            movetype == Types.DARK ||
            movetype == Types.GHOST))
          if (target.pbIncreaseStatWithCause(Stats.SPEED,1,target,target.ability.ToString(TextScripts.Name)))
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Rattled");
        if (target.hasWorkingAbility(Abilities.WEAK_ARMOR) && move.pbIsPhysical(movetype)) {
          if (target.pbReduceStatWithCause(Stats.DEFENSE,1,target,target.ability.ToString(TextScripts.Name)))
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Weak Armor (lower Defense)");
          if (target.pbIncreaseStatWithCause(Stats.SPEED,1,target,target.ability.ToString(TextScripts.Name)))
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Weak Armor (raise Speed)");
        }
        if (target.hasWorkingItem(Items.AIR_BALLOON,true)) {
          GameDebug.Log($"[Item triggered] #{target.ToString()}'s Air Balloon popped");
          @battle.pbDisplay(_INTL("{1}'s Air Balloon popped!",target.ToString()));
          target.pbConsumeItem(true,false);
        }else if (target.hasWorkingItem(Items.ABSORB_BULB) && movetype == Types.WATER) 
          if (target.pbIncreaseStatWithCause(Stats.SPATK,1,target,target.Item.ToString(TextScripts.Name))) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString(TextScripts.Name)}");
            target.pbConsumeItem();
          }
        else if (target.hasWorkingItem(Items.LUMINOUS_MOSS) && movetype == Types.WATER) 
          if (target.pbIncreaseStatWithCause(Stats.SPDEF,1,target,target.Item.ToString(TextScripts.Name))) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString(TextScripts.Name)}");
            target.pbConsumeItem();
          }
        else if (target.hasWorkingItem(Items.CELL_BATTERY) && movetype == Types.ELECTRIC) 
          if (target.pbIncreaseStatWithCause(Stats.ATTACK,1,target,target.Item.ToString(TextScripts.Name))) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString(TextScripts.Name)}");
            target.pbConsumeItem();
          }
        else if (target.hasWorkingItem(Items.SNOWBALL) && movetype == Types.ICE) 
          if (target.pbIncreaseStatWithCause(Stats.ATTACK,1,target,target.Item.ToString(TextScripts.Name))) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString(TextScripts.Name)}");
            target.pbConsumeItem();
          }
        else if (target.hasWorkingItem(Items.WEAKNESS_POLICY) && target.damagestate.TypeMod>8) {
          bool showanim=true;
          if (target.pbIncreaseStatWithCause(Stats.ATTACK,2,target,target.Item.ToString(TextScripts.Name),showanim)) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s Weakness Policy (Attack)");
            showanim=false;
          }
          if (target.pbIncreaseStatWithCause(Stats.SPATK,2,target,target.Item.ToString(TextScripts.Name),showanim)) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s Weakness Policy (Special Attack)");
            showanim=false;
          }
          if (!showanim) target.pbConsumeItem();
        }else if (target.hasWorkingItem(Items.ENIGMA_BERRY) && target.damagestate.TypeMod>8) 
          target.pbActivateBerryEffect();
        else if ((target.hasWorkingItem(Items.JABOCA_BERRY) && move.pbIsPhysical(movetype)) ||
              (target.hasWorkingItem(Items.ROWAP_BERRY) && move.pbIsSpecial(movetype)));
          if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD) && !user.isFainted()) {
            GameDebug.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString(TextScripts.Name)}");
            @battle.scene.pbDamageAnimation(user,0);
            user.ReduceHP((user.TotalHP/8).floor);
            @battle.pbDisplay(_INTL("{1} consumed its {2} and hurt {3}!",target.ToString(),
               target.Item.ToString(TextScripts.Name),user.ToString(true)));
            target.pbConsumeItem();
          }
        else if (target.hasWorkingItem(Items.KEE_BERRY) && move.pbIsPhysical(movetype)) 
          target.pbActivateBerryEffect();
        else if (target.hasWorkingItem(Items.MARANGA_BERRY) && move.pbIsSpecial(movetype)) 
          target.pbActivateBerryEffect();
      }
      if (target.hasWorkingAbility(Abilities.ANGER_POINT))
        if (target.damagestate.Critical && !target.damagestate.Substitute &&
           target.pbCanIncreaseStatStage(Stats.ATTACK,target)) {
          GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Anger Point");
          target.stages[(int)Stats.ATTACK]=6;
          @battle.pbCommonAnimation("StatUp",target,null);
          @battle.pbDisplay(_INTL("{1}'s {2} maxed its {3}!",
             target.ToString(),target.ability.ToString(TextScripts.Name),Stats.ATTACK.ToString(TextScripts.Name)));
        }
    }
    user.pbAbilityCureCheck();
    target.pbAbilityCureCheck();
  }
  public void pbEffectsAfterHit(Pokemon user,Pokemon target,Move thismove,turneffects) {
    if (turneffects.TotalDamage==0) return;
    if (!(user.hasWorkingAbility(Abilities.SHEER_FORCE) && thismove.addlEffect>0)) {
      // Target's held items:
      // Red Card
      if (target.hasWorkingItem(Items.RED_CARD) && @battle.pbCanSwitch(user.Index,-1,false)) {
        user.effects.Roar=true;
        @battle.pbDisplay(_INTL("{1} held up its {2} against the {3}!",
           target.ToString(),target.Item.ToString(TextScripts.Name),user.ToString(true)));
        target.pbConsumeItem();
      // Eject Button
      }else if (target.hasWorkingItem(Items.EJECT_BUTTON) && @battle.pbCanChooseNonActive(target.Index)) {
        target.effects.Uturn=true;
        @battle.pbDisplay(_INTL("{1} is switched out with the {2}!",
           target.ToString(),target.Item.ToString(TextScripts.Name)));
        target.pbConsumeItem();
      }
      // User's held items:
      // Shell Bell
      if (user.hasWorkingItem(Items.SHELL_BELL) && user.effects.HealBlock==0) {
        GameDebug.Log($"[Item triggered] #{user.ToString()}'s Shell Bell (total damage=#{turneffects.TotalDamage})");
        int hpgain=user.RecoverHP((turneffects.TotalDamage/8).floor,true);
        if (hpgain>0)
          @battle.pbDisplay(_INTL("{1} restored a little HP using its {2}!",
             user.ToString(),user.Item.ToString(TextScripts.Name)));
      }
      // Life Orb
      if (user.effects.LifeOrb && !user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
        GameDebug.Log($"[Item triggered] #{user.ToString()}'s Life Orb (recoil)");
        int hploss=user.ReduceHP((user.TotalHP/10).floor,true);
        if (hploss>0)
          @battle.pbDisplay(_INTL("{1} lost some of its HP!",user.ToString()));
      }
      if (user.isFainted()) user.pbFaint(); // no return
      // Color Change
      Types movetype=thismove.pbType(thismove.Type,user,target);
      if (target.hasWorkingAbility(Abilities.COLOR_CHANGE) &&
         !target.hasType(movetype)) {//!PBTypes.isPseudoType(movetype) && 
        GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Color Change made it #{movetype.ToString(TextScripts.Name)}-type");
        target.Type1=movetype;
        target.Type2=movetype;
        target.effects.Type3=-1;
        @battle.pbDisplay(_INTL("{1}'s {2} made it the {3} type!",target.ToString(),
           target.ability.ToString(TextScripts.Name),movetype.ToString(TextScripts.Name)));
      }
    }
    // Moxie
    if (user.hasWorkingAbility(Abilities.MOXIE) && target.isFainted())
      if (user.pbIncreaseStatWithCause(Stats.ATTACK,1,user,user.ability.ToString(TextScripts.Name)))
        GameDebug.Log($"[Ability triggered] #{user.ToString()}'s Moxie");
    // Magician
    if (user.hasWorkingAbility(Abilities.MAGICIAN))
      if (target.Item>0 && user.Item==0 &&
         user.effects.Substitute==0 &&
         target.effects.Substitute==0 &&
         !target.hasWorkingAbility(Abilities.STICKY_HOLD) &&
         !@battle.pbIsUnlosableItem(target,target.Item) &&
         !@battle.pbIsUnlosableItem(user,target.Item) &&
         (@battle.opponent || !@battle.pbIsOpposing(user.Index))) {
        user.Item=target.Item;
        target.Item=0;
        target.effects.Unburden=true;
        if (!@battle.opponent &&   // In a wild battle
           user.pokemon.itemInitial==0 &&
           target.pokemon.itemInitial==user.Item) {
          user.pokemon.itemInitial=user.Item;
          target.pokemon.itemInitial=0;
        }
        @battle.pbDisplay(_INTL("{1} stole {2}'s {3} with {4}!",user.ToString(),
           target.ToString(true),user.Item.ToString(TextScripts.Name),user.ability.ToString(TextScripts.Name)));
        GameDebug.Log($"[Ability triggered] #{user.ToString()}'s Magician stole #{target.ToString(true)}'s #{user.Item.ToString(TextScripts.Name)}");
      }
    // Pickpocket
    if (target.hasWorkingAbility(Abilities.PICKPOCKET))
      if (target.Item==0 && user.Item>0 &&
         user.effects.Substitute==0 &&
         target.effects.Substitute==0 &&
         !user.hasWorkingAbility(Abilities.STICKY_HOLD) &&
         !@battle.pbIsUnlosableItem(user,user.Item) &&
         !@battle.pbIsUnlosableItem(target,user.Item) &&
         (@battle.opponent || !@battle.pbIsOpposing(target.Index))) {
        target.Item=user.Item;
        user.Item=0;
        user.effects.Unburden=true;
        if (!@battle.opponent &&   // In a wild battle
           target.pokemon.itemInitial==0 &&
           user.pokemon.itemInitial==target.Item) {
          target.pokemon.itemInitial=target.Item;
          user.pokemon.itemInitial=0;
        }
        @battle.pbDisplay(_INTL("{1} pickpocketed {2}'s {3}!",target.ToString(),
           user.ToString(true),target.Item.ToString(TextScripts.Name)));
        GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pickpocket stole #{user.ToString(true)}'s #{target.Item.ToString(TextScripts.Name)}");
      }
  }
  public void pbAbilityCureCheck() {
    if (this.isFainted()) return;
    switch (this.status) {
    case Status.SLEEP:
      if (this.hasWorkingAbility(Abilities.VITAL_SPIRIT) || this.hasWorkingAbility(Abilities.INSOMNIA)) {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)}");
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} woke it up!",ToString(),@ability.ToString(TextScripts.Name)));
      }
      break;
    case Status.POISON:
      if (this.hasWorkingAbility(Abilities.IMMUNITY)) {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)}");
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} cured its poisoning!",ToString(),@ability.ToString(TextScripts.Name)));
      }
      break;
    case Status.BURN:
      if (this.hasWorkingAbility(Abilities.WATER_VEIL)) {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)}");
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} healed its burn!",ToString(),@ability.ToString(TextScripts.Name)));
      }
      break;
    case Status.PARALYSIS:
      if (this.hasWorkingAbility(Abilities.LIMBER)) {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)}");
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} cured its paralysis!",ToString(),@ability.ToString(TextScripts.Name)));
      }
      break;
    case Status.FROZEN:
      if (this.hasWorkingAbility(Abilities.MAGMA_ARMOR)) {
        GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)}");
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} defrosted it!",ToString(),@ability.ToString(TextScripts.Name)));
      }
      break;
    }
    if (@effects.Confusion>0 && this.hasWorkingAbility(Abilities.OWN_TEMPO)) {
      GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)} (attract)");
      pbCureConfusion(false);
      @battle.pbDisplay(_INTL("{1}'s {2} snapped it out of its confusion!",ToString(),@ability.ToString(TextScripts.Name)));
    }
    if (@effects.Attract>=0 && this.hasWorkingAbility(Abilities.OBLIVIOUS)) {
      GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)}");
      pbCureAttract();
      @battle.pbDisplay(_INTL("{1}'s {2} cured its infatuation status!",ToString(),@ability.ToString(TextScripts.Name)));
    }
    if (Core.USENEWBATTLEMECHANICS && @effects.Taunt>0 && this.hasWorkingAbility(Abilities.OBLIVIOUS)) {
      GameDebug.Log($"[Ability triggered] #{ToString()}'s #{@ability.ToString(TextScripts.Name)} (taunt)");
      @effects.Taunt=0;
      @battle.pbDisplay(_INTL("{1}'s {2} made its taunt wear off!",ToString(),@ability.ToString(TextScripts.Name)));
    }
  }
#endregion

#region Held Item effects
  public void pbConsumeItem(bool recycle=true,bool pickup=true) {
    string itemname=this.Item.ToString(TextScripts.Name);
    if (recycle) @pokemon.itemRecycle=this.Item;
    if (@pokemon.itemInitial==this.Item) @pokemon.itemInitial=0;
    if (pickup) {
      @effects.PickupItem=this.Item;
      @effects.PickupUse=@battle.nextPickupUse;
    }
    this.Item=0;
    this.effects.Unburden=true;
    // Symbiosis
    if (pbPartner && pbPartner.hasWorkingAbility(Abilities.SYMBIOSIS) && recycle)
      if (pbPartner.Item>0 &&
         !@battle.pbIsUnlosableItem(pbPartner,pbPartner.Item) &&
         !@battle.pbIsUnlosableItem(this,pbPartner.Item)) {
        @battle.pbDisplay(_INTL("{1}'s {2} let it share its {3} with {4}!",
           pbPartner.ToString(),pbPartner.ability.ToString(TextScripts.Name),
           pbPartner.Item.ToString(TextScripts.Name),ToString(true)));
        this.Item=pbPartner.Item;
        pbPartner.Item=0;
        pbPartner.effects.Unburden=true;
        pbBerryCureCheck();
      }
  }
  public bool pbConfusionBerry(int flavor,string message1,string message2) {
    int amt=this.RecoverHP((this.TotalHP/8).floor,true);
    if (amt>0) {
      @battle.pbDisplay(message1);
      if ((this.Nature%5)==flavor && (this.Nature/5).floor!=(this.Nature%5)) {
        @battle.pbDisplay(message2);
        pbConfuseSelf();
      }
      return true;
    }
    return false;
  }
  public bool pbStatIncreasingBerry(Stats stat,string berryname) {
    return pbIncreaseStatWithCause(stat,1,this,berryname);
  }
  public pbActivateBerryEffect(Items berry=Items.NONE,bool consume=true) {
    if (berry==0) berry=this.Item;
    string berryname=(berry==0) ? "" : berry.ToString(TextScripts.Name);
    GameDebug.Log($"[Item triggered] #{ToString()}'s #{berryname}");
    bool consumed=false;
    if (berry == Items.ORAN_BERRY) {
      int amt=this.RecoverHP(10,true);
      if (amt>0) {
        @battle.pbDisplay(_INTL("{1} restored its health using its {2}!",ToString(),berryname));
        consumed=true;
      }
    }else if (berry == Items.SITRUS_BERRY ||
          berry == Items.ENIGMA_BERRY) {
      int amt=this.RecoverHP((this.TotalHP/4).floor,true);
      if (amt>0) {
        @battle.pbDisplay(_INTL("{1} restored its health using its {2}!",ToString(),berryname));
        consumed=true;
      }
    }else if (berry == Items.CHESTO_BERRY)
      if (this.status==Status.SLEEP) {
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} cured its sleep problem.",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.PECHA_BERRY)
      if (this.status==Status.POISON) {
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} cured its poisoning.",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.RAWST_BERRY)
      if (this.status==Status.BURN) {
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} healed its burn.",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.CHERI_BERRY)
      if (this.status==Status.PARALYSIS) {
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} cured its paralysis.",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.ASPEAR_BERRY)
      if (this.status==Status.FROZEN) {
        pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1}'s {2} thawed it out.",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.LEPPA_BERRY) {
      List<int> found= new List<int>();
      for (int i = 0; i < @pokemon.moves.Length; i++)
        if (@pokemon.moves[i].MoveId!=0)
          if ((consume && @pokemon.moves[i].PP==0) ||
             (!consume && @pokemon.moves[i].PP<@pokemon.moves[i].TotalPP))
            found.Add(i);
      if (found.Length>0) {
        int choice=(consume) ? found[0] : found[@battle.pbRandom(found.Length)];
        Attack.Move pokemove=@pokemon.moves[choice];
        pokemove.PP+=10;
        if (pokemove.PP>pokemove.TotalPP ) pokemove.PP=pokemove.TotalPP;
        this.moves[choice].PP=pokemove.PP;
        string movename=pokemove.MoveId.ToString(TextScripts.Name);
        @battle.pbDisplay(_INTL("{1}'s {2} restored {3}'s PP!",ToString(),berryname,movename)) ;
        consumed=true;
      }
    }else if (berry == Items.PERSIM_BERRY)
      if (@effects.Confusion>0) {
        pbCureConfusion(false);
        @battle.pbDisplay(_INTL("{1}'s {2} snapped it out of its confusion!",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.LUM_BERRY)
      if (this.status>0 || @effects.Confusion>0) {
        Status st=this.status; bool conf=(@effects.Confusion>0);
        pbCureStatus(false);
        pbCureConfusion(false);
        switch (st) {
        case Status.SLEEP:
          @battle.pbDisplay(_INTL("{1}'s {2} woke it up!",ToString(),berryname));
          break;
        case Status.POISON:
          @battle.pbDisplay(_INTL("{1}'s {2} cured its poisoning!",ToString(),berryname));
          break;
        case Status.BURN:
          @battle.pbDisplay(_INTL("{1}'s {2} healed its burn!",ToString(),berryname));
          break;
        case Status.PARALYSIS:
          @battle.pbDisplay(_INTL("{1}'s {2} cured its paralysis!",ToString(),berryname));
          break;
        case Status.FROZEN:
          @battle.pbDisplay(_INTL("{1}'s {2} defrosted it!",ToString(),berryname));
          break;
        }
        if (conf)
          @battle.pbDisplay(_INTL("{1}'s {2} snapped it out of its confusion!",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.FIGY_BERRY)
      consumed=pbConfusionBerry(0,
         _INTL("{1}'s {2} restored health!",ToString(),berryname),
         _INTL("For {1}, the {2} was too spicy!",ToString(true),berryname));
    else if (berry == Items.WIKI_BERRY)
      consumed=pbConfusionBerry(3,
         _INTL("{1}'s {2} restored health!",ToString(),berryname),
         _INTL("For {1}, the {2} was too dry!",ToString(true),berryname));
    else if (berry == Items.MAGO_BERRY)
      consumed=pbConfusionBerry(2,
         _INTL("{1}'s {2} restored health!",ToString(),berryname),
         _INTL("For {1}, the {2} was too sweet!",ToString(true),berryname));
    else if (berry == Items.AGUAV_BERRY)
      consumed=pbConfusionBerry(4,
         _INTL("{1}'s {2} restored health!",ToString(),berryname),
         _INTL("For {1}, the {2} was too bitter!",ToString(true),berryname));
    else if (berry == Items.IAPAPA_BERRY)
      consumed=pbConfusionBerry(1,
         _INTL("{1}'s {2} restored health!",ToString(),berryname),
         _INTL("For {1}, the {2} was too sour!",ToString(true),berryname));
    else if (berry == Items.LIECHI_BERRY)
      consumed=pbStatIncreasingBerry(Stats.ATTACK,berryname);
    else if (berry == Items.GANLON_BERRY ||
          berry == Items.KEE_BERRY)
      consumed=pbStatIncreasingBerry(Stats.DEFENSE,berryname);
    else if (berry == Items.SALAC_BERRY)
      consumed=pbStatIncreasingBerry(Stats.SPEED,berryname);
    else if (berry == Items.PETAYA_BERRY)
      consumed=pbStatIncreasingBerry(Stats.SPATK,berryname);
    else if (berry == Items.APICOT_BERRY ||
          berry == Items.MARANGA_BERRY)
      consumed=pbStatIncreasingBerry(Stats.SPDEF,berryname);
    else if (berry == Items.LANSAT_BERRY)
      if (@effects.FocusEnergy<2) {
        @effects.FocusEnergy=2;
        @battle.pbDisplay(_INTL("{1} used its {2} to get pumped!",ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.MICLE_BERRY)
      if (!@effects.MicleBerry) {
        @effects.MicleBerry=true;
        @battle.pbDisplay(_INTL("{1} boosted the accuracy of its next move using its {2}!",
           ToString(),berryname));
        consumed=true;
      }
    else if (berry == Items.STARF_BERRY) {
      List<Stats> stats= new List<Stats>();
      foreach (Stats i in new Stats[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPATK, Stats.SPDEF, Stats.SPEED })
        if (pbCanIncreaseStatStage(i,this)) stats.Add(i);
      if (stats.Length>0) {
        Stats stat=stats[@battle.pbRandom(stats.Length)];
        consumed=pbIncreaseStatWithCause(stat,2,this,berryname);
      }
    }
    if (consumed) {
      // Cheek Pouch
      if (hasWorkingAbility(Abilities.CHEEK_POUCH)) {
        int amt=this.RecoverHP((@TotalHP/3).floor,true);
        if (amt>0)
          @battle.pbDisplay(_INTL("{1}'s {2} restored its health!",
             ToString(),ability.ToString(TextScripts.Name)));
      }
      if (consume) pbConsumeItem();
      if (this.pokemon) this.pokemon.belch=true;
    }
  }
  public void pbBerryCureCheck(bool hpcure=false) {
    if (this.isFainted()) return;
    bool unnerver=(pbOpposing1.hasWorkingAbility(Abilities.UNNERVE) ||
              pbOpposing2.hasWorkingAbility(Abilities.UNNERVE));
    string itemname=(this.Item==0) ? "" : this.Item.ToString(TextScripts.Name);
    if (hpcure)
      if (this.hasWorkingItem(Items.BERRY_JUICE) && this.HP<=(this.TotalHP/2).floor) {
        int amt=this.RecoverHP(20,true);
        if (amt>0) {
          @battle.pbCommonAnimation("UseItem",this,null);
          @battle.pbDisplay(_INTL("{1} restored its health using its {2}!",ToString(),itemname));
          pbConsumeItem();
          return;
        }
      }
    if (!unnerver) {
      if (hpcure) {
        if (this.HP<=(this.TotalHP/2).floor) {
          if (this.hasWorkingItem(Items.ORAN_BERRY) ||
             this.hasWorkingItem(Items.SITRUS_BERRY)) {
            pbActivateBerryEffect();
            return;
          }
          if (this.hasWorkingItem(Items.FIGY_BERRY) ||
             this.hasWorkingItem(Items.WIKI_BERRY) ||
             this.hasWorkingItem(Items.MAGO_BERRY) ||
             this.hasWorkingItem(Items.AGUAV_BERRY) ||
             this.hasWorkingItem(Items.IAPAPA_BERRY)) {
            pbActivateBerryEffect();
            return;
          }
        }
      } //<= Not sure if this should move to 1930
        if ((this.hasWorkingAbility(Abilities.GLUTTONY) && this.HP<=(this.TotalHP/2).floor) ||
           this.HP<=(this.TotalHP/4).floor) {
          if (this.hasWorkingItem(Items.LIECHI_BERRY) ||
             this.hasWorkingItem(Items.GANLON_BERRY) ||
             this.hasWorkingItem(Items.SALAC_BERRY) ||
             this.hasWorkingItem(Items.PETAYA_BERRY) ||
             this.hasWorkingItem(Items.APICOT_BERRY)) {
            pbActivateBerryEffect();
            return;
          }
          if (this.hasWorkingItem(Items.LANSAT_BERRY) ||
             this.hasWorkingItem(Items.STARF_BERRY)) {
            pbActivateBerryEffect();
            return;
          }
          if (this.hasWorkingItem(Items.MICLE_BERRY)) {
            pbActivateBerryEffect();
            return;
          }
        }
        if (this.hasWorkingItem(Items.LEPPA_BERRY)) {
          pbActivateBerryEffect();
          return;
        }
      if (this.hasWorkingItem(Items.CHESTO_BERRY) ||
         this.hasWorkingItem(Items.PECHA_BERRY) ||
         this.hasWorkingItem(Items.RAWST_BERRY) ||
         this.hasWorkingItem(Items.CHERI_BERRY) ||
         this.hasWorkingItem(Items.ASPEAR_BERRY) ||
         this.hasWorkingItem(Items.PERSIM_BERRY) ||
         this.hasWorkingItem(Items.LUM_BERRY)) {
        pbActivateBerryEffect();
        return;
      }
    }
    if (this.hasWorkingItem(Items.WHITE_HERB)) {
      bool reducedstats=false;
      foreach (Stats i in new Stats[]{ Stats.ATTACK,Stats.DEFENSE,
                Stats.SPEED,Stats.SPATK,Stats.SPDEF,
                Stats.ACCURACY,Stats.EVASION }) 
        if (@stages[(int)i]<0)
          @stages[(int)i]=0; reducedstats=true;
      if (reducedstats) {
        GameDebug.Log($"[Item triggered] #{ToString()}'s #{itemname}");
        @battle.pbCommonAnimation("UseItem",this,null);
        @battle.pbDisplay(_INTL("{1} restored its status using its {2}!",ToString(),itemname));
        pbConsumeItem();
        return;
      }
    }
    if (this.hasWorkingItem(Items.MENTAL_HERB) &&
       (@effects.Attract>=0 ||
       @effects.Taunt>0 ||
       @effects.Encore>0 ||
       @effects.Torment ||
       @effects.Disable>0 ||
       @effects.HealBlock>0)) {
      GameDebug.Log($"[Item triggered] #{ToString()}'s #{itemname}");
      @battle.pbCommonAnimation("UseItem",this,null);
      if (@effects.Attract>=0) @battle.pbDisplay(_INTL("{1} cured its infatuation status using its {2}.",ToString(),itemname));
      if (@effects.Taunt>0) @battle.pbDisplay(_INTL("{1}'s taunt wore off!",ToString()));
      if (@effects.Encore>0) @battle.pbDisplay(_INTL("{1}'s encore ended!",ToString()));
      if (@effects.Torment) @battle.pbDisplay(_INTL("{1}'s torment wore off!",ToString()));
      if (@effects.Disable>0) @battle.pbDisplay(_INTL("{1} is no longer disabled!",ToString()));
      if (@effects.HealBlock>0) @battle.pbDisplay(_INTL("{1}'s Heal Block wore off!",ToString()));
      this.pbCureAttract();
      @effects.Taunt=0;
      @effects.Encore=0;
      @effects.EncoreMove=0;
      @effects.EncoreIndex=0;
      @effects.Torment=false;
      @effects.Disable=0;
      @effects.HealBlock=0;
      pbConsumeItem();
      return;
    }
    if (hpcure && this.hasWorkingItem(Items.LEFTOVERS) && this.HP!=this.TotalHP &&
       @effects.HealBlock==0) {
      GameDebug.Log($"[Item triggered] #{ToString()}'s Leftovers");
      @battle.pbCommonAnimation("UseItem",this,null);
      RecoverHP((this.TotalHP/16).floor,true);
      @battle.pbDisplay(_INTL("{1} restored a little HP using its {2}!",ToString(),itemname));
    }
    if (hpcure && this.hasWorkingItem(Items.BLACK_SLUDGE)) {
      if (hasType(Types.POISON))
        if (this.HP!=this.TotalHP &&
           (!Core.USENEWBATTLEMECHANICS || @effects.HealBlock==0)) {
          GameDebug.Log($"[Item triggered] #{ToString()}'s Black Sludge (heal)");
          @battle.pbCommonAnimation("UseItem",this,null);
          RecoverHP((this.TotalHP/16).floor,true);
          @battle.pbDisplay(_INTL("{1} restored a little HP using its {2}!",ToString(),itemname));
        }
      else if (!this.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
        GameDebug.Log($"[Item triggered] #{ToString()}'s Black Sludge (damage)");
        @battle.pbCommonAnimation("UseItem",this,null);
        ReduceHP((this.TotalHP/8).floor,true);
        @battle.pbDisplay(_INTL("{1} was hurt by its {2}!",ToString(),itemname));
      }
      if (this.isFainted()) pbFaint();
    }
  }
#endregion

#region Move user and targets
  public pbFindUser(choice,Pokemon[] targets) {
    move=choice[2];
    target=choice[3];
    user = this;   // Normally, the user is this
    // Targets in normal cases
    switch (pbTarget(move)) {
    case PBTargets::SingleNonUser:
      if (target>=0) {
        targetBattler=@battle.battlers[target];
        if (!pbIsOpposing(targetBattler.Index))
          if (!pbAddTarget(targets,targetBattler))
            if (!pbAddTarget(targets,pbOpposing1)) pbAddTarget(targets,pbOpposing2);
        else
          if (!pbAddTarget(targets,targetBattler)) pbAddTarget(targets,targetBattler.pbPartner);
      }
      else
        pbRandomTarget(targets)
      break;
    case PBTargets::SingleOpposing:
      if (target>=0) {
        targetBattler=@battle.battlers[target];
        if (!pbIsOpposing(targetBattler.Index))
          if (!pbAddTarget(targets,targetBattler))
            if (!pbAddTarget(targets,pbOpposing1)) pbAddTarget(targets,pbOpposing2);
        else
          if (!pbAddTarget(targets,targetBattler)) pbAddTarget(targets,targetBattler.pbPartner);
      }
      else
        pbRandomTarget(targets)
      break;
    case PBTargets::OppositeOpposing:
      if (!pbAddTarget(targets,pbOppositeOpposing2)) pbAddTarget(targets,pbOppositeOpposing);
      break;
    case PBTargets::RandomOpposing:
      pbRandomTarget(targets);
      break;
    case PBTargets::AllOpposing:
      // Just pbOpposing1 because partner is determined late
      if (!pbAddTarget(targets,pbOpposing1)) pbAddTarget(targets,pbOpposing2);
      break;
    case PBTargets::AllNonUsers:
      for (int i = 0; i < 4; i++) // not ordered by priority
        if (i!=@index) pbAddTarget(targets,@battle.battlers[i]);
      break;
    case PBTargets::UserOrPartner:
      if (target>=0) { // Pre-chosen target
        targetBattler=@battle.battlers[target];
        if (!pbAddTarget(targets,targetBattler)) pbAddTarget(targets,targetBattler.pbPartner);
      }
      else
        pbAddTarget(targets,this);
      break;
    case PBTargets::Partner:
      pbAddTarget(targets,pbPartner);
      break;
    default:
      move.pbAddTarget(targets,this);
      break;
    }
    return user;
  }
  public pbChangeUser(Move thismove,Pokemon user) {
    priority=@battle.pbPriority;
    // Change user to user of Snatch
    if (thismove.canSnatch())
      foreach (var i in priority)
        if (i.effects.Snatch) {
          @battle.pbDisplay(_INTL("{1} snatched {2}'s move!",i.ToString(),user.ToString(true)));
          GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Snatch made it use #{user.ToString(true)}'s #{thismove.name}");
          i.effects.Snatch=false;
          target=user;
          user=i;
          // Snatch's PP is reduced if old user has Pressure
          userchoice=@battle.choices[user.Index][1];
          if (target.hasWorkingAbility(Abilities.PRESSURE) && user.pbIsOpposing(target.Index) && userchoice>=0) {
            pressuremove=user.moves[userchoice];
            if (pressuremove.PP>0) pbSetPP(pressuremove,pressuremove.PP-1);
          }
          if (Core.USENEWBATTLEMECHANICS) break;
        }
    return user;
  }
  public pbTarget(Move move) {
    target=move.target;
    if (move.Effect==0x10D && hasType(Types.GHOST) // Curse)
      target=PBTargets::OppositeOpposing;
    return target;
  }
  public pbAddTarget(Pokemon[] targets,Pokemon target) {
    if (!target.isFainted()) {
      targets[targets.Length]=target;
      return true;
    }
    return false;
  }
  public pbRandomTarget(Pokemon[] targets) {
    choices= new List<>();
    pbAddTarget(choices,pbOpposing1);
    pbAddTarget(choices,pbOpposing2);
    if (choices.Length>0)
      pbAddTarget(targets,choices[@battle.pbRandom(choices.Length)]);
  }
  public pbChangeTarget(Move thismove,Pokemon[] userandtarget,Pokemon[] targets) {
    priority=@battle.pbPriority;
    changeeffect=0;
    user=userandtarget[0];
    target=userandtarget[1];
    // Lightningrod
    if (targets.Length==1 && thismove.pbType(thismove.Type,user,target) == Types.ELECTRIC && 
       !target.hasWorkingAbility(Abilities.LIGHTNINGROD))
      foreach (var i in priority) { // use Pokémon earliest in priority
        if (user.Index==i.Index || target.Index==i.Index) continue; 
        if (i.hasWorkingAbility(Abilities.LIGHTNINGROD)) {
          GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Lightningrod (change target)");
          target=i; // X's Lightningrod took the attack!
          changeeffect=1;
          break;
        }
      }
    // Storm Drain
    if (targets.Length==1 && thismove.pbType(thismove.Type,user,target) == Types.WATER && 
       !target.hasWorkingAbility(Abilities.STORMDRAIN))
      foreach (var i in priority) { // use Pokémon earliest in priority
        if (user.Index==i.Index || target.Index==i.Index) continue; 
        if (i.hasWorkingAbility(Abilities.STORMDRAIN)) {
          GameDebug.Log($"[Ability triggered] #{i.ToString()}'s Storm Drain (change target)");
          target=i; // X's Storm Drain took the attack!
          changeeffect=1;
          break;
        }
      }
    // Change target to user of Follow Me (overrides Magic Coat
    // because check for Magic Coat below uses this target)
    if (PBTargets.targetsOneOpponent(thismove)) {
      newtarget=null; strength=100;
      foreach (var i in priority) { // use Pokémon latest in priority
        if (!user.pbIsOpposing(i.Index)) continue; 
        if (!i.isFainted() && !@battle.switching && !i.effects.SkyDrop &&
           i.effects.FollowMe>0 && i.effects.FollowMe<strength) {
          GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Follow Me");
          newtarget=i; strength=i.effects.FollowMe;
          changeeffect=0;
        }
      }
      if (newtarget) target=newtarget;
    }
    // TODO: Pressure here is incorrect if Magic Coat redirects target
    if (user.pbIsOpposing(target.Index) && target.hasWorkingAbility(Abilities.PRESSURE)) {
      GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pressure (in pbChangeTarget)");
      user.pbReducePP(thismove); // Reduce PP
    }
    // Change user to user of Snatch
    if (thismove.canSnatch())
      foreach (var i in priority)
        if (i.effects.Snatch) {
          @battle.pbDisplay(_INTL("{1} Snatched {2}'s move!",i.ToString(),user.ToString(true)));
          GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Snatch made it use #{user.ToString(true)}'s #{thismove.name}");
          i.effects.Snatch=false;
          target=user;
          user=i;
          // Snatch's PP is reduced if old user has Pressure
          userchoice=@battle.choices[user.Index][1];
          if (target.hasWorkingAbility(Abilities.PRESSURE) && user.pbIsOpposing(target.Index) && userchoice>=0) {
            GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pressure (part of Snatch)");
            pressuremove=user.moves[userchoice];
            if (pressuremove.PP>0) pbSetPP(pressuremove,pressuremove.PP-1);
          }
        }
    if (thismove.canMagicCoat())
      if (target.effects.MagicCoat) {
        // switch user and target
        GameDebug.Log($"[Lingering effect triggered] #{i.ToString()}'s Magic Coat made it use #{user.ToString(true)}'s #{thismove.name}");
        changeeffect=3;
        tmp=user;
        user=target;
        target=tmp;
        // Magic Coat's PP is reduced if old user has Pressure
        userchoice=@battle.choices[user.Index][1];
        if (target.hasWorkingAbility(Abilities.PRESSURE) && user.pbIsOpposing(target.Index) && userchoice>=0) {
          GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Pressure (part of Magic Coat)");
          pressuremove=user.moves[userchoice];
          if (pressuremove.PP>0) pbSetPP(pressuremove,pressuremove.PP-1);
        }
      }else if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.MAGICBOUNCE)) {
        // switch user and target
        GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Magic Bounce made it use #{user.ToString(true)}'s #{thismove.name}");
        changeeffect=3;
        tmp=user;
        user=target;
        target=tmp;
      }
    if (changeeffect==1)
      @battle.pbDisplay(_INTL("{1}'s {2} took the move!",target.ToString(),target.ability.ToString(TextScripts.Name)));
    else if (changeeffect==3) 
      @battle.pbDisplay(_INTL("{1} bounced the {2} back!",user.ToString(),thismove.name));
    userandtarget[0]=user;
    userandtarget[1]=target;
    if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.SOUNDPROOF) &&
       thismove.isSoundBased() &&
       (int)thismove.Effect!=0xE5 &&   // Perish Song handled elsewhere
       (int)thismove.Effect!=0x151) {     // Parting Shot handled elsewhere
      GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Soundproof blocked #{user.ToString(true)}'s #{thismove.name}");
      @battle.pbDisplay(_INTL("{1}'s {2} blocks {3}!",target.ToString(),
         target.ability.ToString(TextScripts.Name),thismove.name));
      return false;
    }
    return true;
  }
#endregion
#region Move PP
  public void pbSetPP(Move move,byte pp) {
    move.PP=pp;
    // Not effects.Mimic, since Mimic can't copy Mimic
    if (move.thismove && move.MoveId==move.thismove.MoveId && !@effects.Transform)
      move.thismove.PP=pp;
  }
  public bool pbReducePP(Move move) {
    if (@effects.TwoTurnAttack>0 ||
       @effects.Bide>0 || 
       @effects.Outrage>0 ||
       @effects.Rollout>0 ||
       @effects.HyperBeam>0 ||
       @effects.Uproar>0)
      // No need to reduce PP if two-turn attack
      return true;
    if (move.PP<0) return true;   // No need to reduce PP for special calls of moves
    if (move.TotalPP==0) return true;   // Infinite PP, can always be used
    if (move.PP==0) return false;
    if (move.PP>0)
      pbSetPP(move,(byte)(move.PP-1));
    return true;
  }
  public void pbReducePPOther(Move move) {
    if (move.PP>0) pbSetPP(move,(byte)(move.PP-1));
  }
#endregion
#region Using a move
  public bool pbObedienceCheck(choice) {
    if (choice[0]!=1) return true;
    if (@battle.pbOwnedByPlayer(@index) && @battle.internalbattle) {
      int badgelevel=10;
      if (@battle.pbPlayer.numbadges>=1) badgelevel=20 ;
      if (@battle.pbPlayer.numbadges>=2) badgelevel=30 ;
      if (@battle.pbPlayer.numbadges>=3) badgelevel=40 ;
      if (@battle.pbPlayer.numbadges>=4) badgelevel=50 ;
      if (@battle.pbPlayer.numbadges>=5) badgelevel=60 ;
      if (@battle.pbPlayer.numbadges>=6) badgelevel=70 ;
      if (@battle.pbPlayer.numbadges>=7) badgelevel=80 ;
      if (@battle.pbPlayer.numbadges>=8) badgelevel=100;
      move=choice[2];
      bool disobedient=false;
      if (@pokemon.isForeign(@battle.pbPlayer) && @level>badgelevel) {
        int a=((@level+badgelevel)*@battle.pbRandom(256)/255).floor;
        disobedient|=a<badgelevel;
      }
      if (this.respond_to("pbHyperModeObedience"))
        disobedient|=!this.pbHyperModeObedience(move)
      if (disobedient) {
        GameDebug.Log($"[Disobedience] #{ToString()} disobeyed");
        @effects.Rage=false;
        if (this.status==Status.SLEEP && 
           (move.Effect==0x11 || move.Effect==0xB4)) { // Snore, Sleep Talk
          @battle.pbDisplay(_INTL("{1} ignored orders while asleep!",ToString()));
          return false;
        }
        int b=((@level+badgelevel)*@battle.pbRandom(256)/255).floor;
        if (b<badgelevel) {
          if (!@battle.pbCanShowFightMenu(@index)) return false;
          othermoves= new List<>();
          for (int i = 0; i < 4; i++) {
            if (i==choice[1]) continue; 
            if (@battle.pbCanChooseMove(@index,i,false)) othermoves[othermoves.Length]=i;
          }
          if (othermoves.Length>0) {
            @battle.pbDisplay(_INTL("{1} ignored orders!",ToString()));
            newchoice=othermoves[@battle.pbRandom(othermoves.Length)];
            choice[1]=newchoice;
            choice[2]=@moves[newchoice];
            choice[3]=-1;
          }
          return true;
        }
        else if (this.status!=Status.SLEEP) {
          int c=@level-b;
          int r=@battle.pbRandom(256);
          if (r<c && pbCanSleep(this,false)) {
            pbSleepSelf();
            @battle.pbDisplay(_INTL("{1} took a nap!",ToString()));
            return false;
          }
          r-=c;
          if (r<c) {
            @battle.pbDisplay(_INTL("It hurt itself in its confusion!"));
            pbConfusionDamage;
          }
          else {
            int message=@battle.pbRandom(4);
            if (message==0) @battle.pbDisplay(_INTL("{1} ignored orders!",ToString()));
            if (message==1) @battle.pbDisplay(_INTL("{1} turned away!",ToString()));
            if (message==2) @battle.pbDisplay(_INTL("{1} is loafing around!",ToString()));
            if (message==3) @battle.pbDisplay(_INTL("{1} pretended not to notice!",ToString()));
          }
          return false;
        }
      }
      return true;
    }
    else
      return true;
  }
  public bool pbSuccessCheck(Move thismove,Pokemon user,Pokemon target,turneffects,bool accuracy=true) {
    if (user.effects.TwoTurnAttack>0)
      return true;
    // TODO: "Before Protect" applies to Counter/Mirror Coat
    if ((int)thismove.Effect==0xDE && target.status!=Status.SLEEP) { // Dream Eater
      @battle.pbDisplay(_INTL("{1} wasn't affected!",target.ToString()));
      GameDebug.Log($"[Move failed] #{user.ToString()}'s Dream Eater's target isn't asleep");
      return false;
    }
    if ((int)thismove.Effect==0x113 && user.effects.Stockpile==0) { // Spit Up
      @battle.pbDisplay(_INTL("But it failed to spit up a thing!"));
      GameDebug.Log($"[Move failed] #{user.ToString()}'s Spit Up did nothing as Stockpile's count is 0");
      return false;
    }
    if (target.effects.Protect && thismove.canProtectAgainst() &&
       !target.effects.ProtectNegation) {
      @battle.pbDisplay(_INTL("{1} protected itself!",target.ToString()));
      //@battle.successStates[user.Index].protected=true; //ToDo: Uncomment here
      GameDebug.Log($"[Move failed] #{target.ToString()}'s Protect stopped the attack");
      return false;
    }
    int p=thismove.Priority;
    if (Core.USENEWBATTLEMECHANICS) {
      if (user.hasWorkingAbility(Abilities.PRANKSTER) && thismove.pbIsStatus()) p+=1;
      if (user.hasWorkingAbility(Abilities.GALE_WINGS) && thismove.Type == Types.FLYING) p+=1;
    }
    if (target.pbOwnSide.effects.QuickGuard && thismove.canProtectAgainst() &&
       p>0 && !target.effects.ProtectNegation) {
      @battle.pbDisplay(_INTL("{1} was protected by Quick Guard!",target.ToString()));
      GameDebug.Log($"[Move failed] The opposing side's Quick Guard stopped the attack");
      return false;
    }
    if (target.pbOwnSide.effects.WideGuard &&
       thismove.Target.HasMultipleTargets() && !thismove.pbIsStatus() &&
       !target.effects.ProtectNegation) {
      @battle.pbDisplay(_INTL("{1} was protected by Wide Guard!",target.ToString()));
      GameDebug.Log($"[Move failed] The opposing side's Wide Guard stopped the attack");
      return false;
    }
    if (target.pbOwnSide.effects.CraftyShield && thismove.pbIsStatus() &&
       (int)thismove.Effect!=0xE5) { // Perish Song
      @battle.pbDisplay(_INTL("Crafty Shield protected {1}!",target.ToString(true)));
      GameDebug.Log($"[Move failed] The opposing side's Crafty Shield stopped the attack");
      return false;
    }
    if (target.pbOwnSide.effects.MatBlock && !thismove.pbIsStatus() &&
       thismove.canProtectAgainst() && !target.effects.ProtectNegation) {
      @battle.pbDisplay(_INTL("{1} was blocked by the kicked-up mat!",thismove.name));
      GameDebug.Log($"[Move failed] The opposing side's Mat Block stopped the attack");
      return false;
    }
    // TODO: Mind Reader/Lock-On
    // --Sketch/FutureSight/PsychUp work even on Fly/Bounce/Dive/Dig
    if (thismove.pbMoveFailed(user,target)) { // TODO: Applies to Snore/Fake Out
      @battle.pbDisplay(_INTL("But it failed!"));
      GameDebug.Log(string.Format("[Move failed] Failed pbMoveFailed (function code %02X)",thismove.Effect));
      return false;
    }
    // King's Shield (purposely after pbMoveFailed)
    if (target.effects.KingsShield && !thismove.pbIsStatus() &&
       thismove.canProtectAgainst() && !target.effects.ProtectNegation) {
      @battle.pbDisplay(_INTL("{1} protected itself!",target.ToString()));
      //@battle.successStates[user.Index].protected=true; //ToDo: Uncomment here
      GameDebug.Log($"[Move failed] #{target.ToString()}'s King's Shield stopped the attack");
      if (thismove.isContactMove())
        user.pbReduceStat(Stats.ATTACK,2,null,false);
      return false;
    }
    // Spiky Shield
    if (target.effects.SpikyShield && thismove.canProtectAgainst() &&
       !target.effects.ProtectNegation) {
      @battle.pbDisplay(_INTL("{1} protected itself!",target.ToString()));
      //@battle.successStates[user.Index].protected=true; //ToDo: Uncomment here
      GameDebug.Log($"[Move failed] #{user.ToString()}'s Spiky Shield stopped the attack");
      if (thismove.isContactMove() && !user.isFainted()) {
        @battle.scene.pbDamageAnimation(user,0);
        int amt=user.ReduceHP((user.TotalHP/8).floor);
        if (amt>0) @battle.pbDisplay(_INTL("{1} was hurt!",user.ToString()));
      }
      return false;
    }
    // Immunity to powder-based moves
    if (Core.USENEWBATTLEMECHANICS && thismove.isPowderMove() &&
       (target.hasType(Types.GRASS) ||
       (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.OVERCOAT)) ||
       target.hasWorkingItem(Items.SAFETY_GOGGLES))) {
      @battle.pbDisplay(_INTL("It doesn't affect\r\n{1}...",target.ToString(true)));
      GameDebug.Log($"[Move failed] #{target.ToString()} is immune to powder-based moves somehow");
      return false;
    }
    if (thismove.basedamage>0 && (int)thismove.Effect!=0x02 && // Struggle
       (int)thismove.Effect!=0x111) { // Future Sight
      Types type=thismove.pbType(thismove.Type,user,target);
      float typemod=thismove.pbTypeModifier(type,user,target);
      // Airborne-based immunity to Ground moves
      if (type == Types.GROUND && target.isAirborne(user.hasMoldBreaker()) &&
         !target.hasWorkingItem(Items.RING_TARGET) && (int)thismove.Effect!=0x11C) { // Smack Down
        if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.LEVITATE)) {
          @battle.pbDisplay(_INTL("{1} makes Ground moves miss with Levitate!",target.ToString()));
          GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Levitate made the Ground-type move miss");
          return false;
        }
        if (target.hasWorkingItem(Items.AIR_BALLOON)) {
          @battle.pbDisplay(_INTL("{1}'s Air Balloon makes Ground moves miss!",target.ToString()));
          GameDebug.Log($"[Item triggered] #{target.ToString()}'s Air Balloon made the Ground-type move miss");
          return false;
        }
        if (target.effects.MagnetRise>0) {
          @battle.pbDisplay(_INTL("{1} makes Ground moves miss with Magnet Rise!",target.ToString()));
          GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Magnet Rise made the Ground-type move miss");
          return false;
        }
        if (target.effects.Telekinesis>0) {
          @battle.pbDisplay(_INTL("{1} makes Ground moves miss with Telekinesis!",target.ToString()));
          GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Telekinesis made the Ground-type move miss");
          return false;
        }
      }
      if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.WONDER_GUARD) &&
         type>=0 && typemod<=8) {
        @battle.pbDisplay(_INTL("{1} avoided damage with Wonder Guard!",target.ToString()));
        GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Wonder Guard");
        return false;
      }
      if (typemod==0) {
        @battle.pbDisplay(_INTL("It doesn't affect\r\n{1}...",target.ToString(true)));
        GameDebug.Log($"[Move failed] Type immunity");
        return false;
      }
    }
    if (accuracy) {
      if (target.effects.LockOn>0 && target.effects.LockOnPos==user.Index) {
        GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Lock-On");
        return true;
      }
      bool miss=false; bool _override=false;
      int invulmove=Game.MoveData[target.effects.TwoTurnAttack].Effect;
      switch (invulmove) {
      case 0xC9: case 0xCC: // Fly, Bounce
        if ((int)thismove.Effect!=0x08 ||  // Thunder
                         (int)thismove.Effect!=0x15 ||  // Hurricane
                         (int)thismove.Effect!=0x77 ||  // Gust
                         (int)thismove.Effect!=0x78 ||  // Twister
                         (int)thismove.Effect!=0x11B || // Sky Uppercut
                         (int)thismove.Effect!=0x11C || // Smack Down
                         thismove.MoveId != Moves.WHIRLWIND)miss=true ;
        break;
      case 0xCA: // Dig
        if ((int)thismove.Effect!=0x76 || // Earthquake
                         (int)thismove.Effect!=0x95)    // Magnitude
            miss=true ;
        break;
      case 0xCB: // Dive
        if ((int)thismove.Effect!=0x75 || // Surf
                         (int)thismove.Effect!=0xD0)    // Whirlpool
            miss=true ;
        break;
      case 0xCD: // Shadow Force
        miss=true;
        break;
      case 0xCE: // Sky Drop
        if ((int)thismove.Effect!=0x08 ||  // Thunder
                         (int)thismove.Effect!=0x15 ||  // Hurricane
                         (int)thismove.Effect!=0x77 ||  // Gust
                         (int)thismove.Effect!=0x78 ||  // Twister
                         (int)thismove.Effect!=0x11B || // Sky Uppercut
                         (int)thismove.Effect!=0x11C)    // Smack Down
           miss=true ;   
        break;
      case 0x14D: // Phantom Force
        miss=true;
        break;
      }
      if (target.effects.SkyDrop)
        if ((int)thismove.Effect!=0x08 ||  // Thunder
                         (int)thismove.Effect!=0x15 ||  // Hurricane
                         (int)thismove.Effect!=0x77 ||  // Gust
                         (int)thismove.Effect!=0x78 ||  // Twister
                         (int)thismove.Effect!=0xCE ||  // Sky Drop
                         (int)thismove.Effect!=0x11B || // Sky Uppercut
                         (int)thismove.Effect!=0x11C)    // Smack Down
            miss=true;
      if (user.hasWorkingAbility(Abilities.NO_GUARD) ||
                    target.hasWorkingAbility(Abilities.NO_GUARD) ||
                    @battle.futuresight) miss=false;
      if (Core.USENEWBATTLEMECHANICS && (int)thismove.Effect==0x06 && // Toxic
                    thismove.basedamage==0 && user.hasType(Types.POISON)) _override=true;
      if (!miss && turneffects.SkipAccuracyCheck) _override=true; // Called by another move
      if (!_override && (miss || !thismove.pbAccuracyCheck(user,target))) { // Includes Counter/Mirror Coat
        GameDebug.Log(string.Format("[Move failed] Failed pbAccuracyCheck (function code %02X) or target is semi-invulnerable",thismove.Effect));
        if (thismove.target==PBTargets::AllOpposing && 
           (!user.pbOpposing1.isFainted() ? 1 : 0) + (!user.pbOpposing2.isFainted() ? 1 : 0) > 1)
          @battle.pbDisplay(_INTL("{1} avoided the attack!",target.ToString()));
        else if (thismove.target==PBTargets::AllNonUsers && 
           (!user.pbOpposing1.isFainted() ? 1 : 0) + (!user.pbOpposing2.isFainted() ? 1 : 0) + (!user.pbPartner.isFainted() ? 1 : 0) > 1) 
          @battle.pbDisplay(_INTL("{1} avoided the attack!",target.ToString()));
        else if (target.effects.TwoTurnAttack>0) 
          @battle.pbDisplay(_INTL("{1} avoided the attack!",target.ToString()));
        else if (thismove.Effect==0xDC) // Leech Seed
          @battle.pbDisplay(_INTL("{1} evaded the attack!",target.ToString()));
        else
          @battle.pbDisplay(_INTL("{1}'s attack missed!",user.ToString()));
        return false;
      }
    }
    return true;
  }
  public bool pbTryUseMove(choice,Move thismove,turneffects) {
    if (turneffects.PassedTrying) return true;
    // TODO: Return true if attack has been Mirror Coated once already
    if (!turneffects.SkipAccuracyCheck)
      if (!pbObedienceCheck(choice)) return false;
    if (@effects.SkyDrop) { // Intentionally no message here
      GameDebug.Log($"[Move failed] #{ToString()} can't use #{thismove.name} because of being Sky Dropped");
      return false;
    }
    if (@battle.field.effects.Gravity>0 && thismove.unusableInGravity()) {
      @battle.pbDisplay(_INTL("{1} can't use {2} because of gravity!",ToString(),thismove.name));
      GameDebug.Log($"[Move failed] #{ToString()} can't use #{thismove.name} because of Gravity");
      return false;
    }
    if (@effects.Taunt>0 && thismove.basedamage==0) {
      @battle.pbDisplay(_INTL("{1} can't use {2} after the taunt!",ToString(),thismove.name));
      GameDebug.Log($"[Move failed] #{ToString()} can't use #{thismove.name} because of Taunt");
      return false;
    }
    if (@effects.HealBlock>0 && thismove.isHealingMove()) {
      @battle.pbDisplay(_INTL("{1} can't use {2} because of Heal Block!",ToString(),thismove.name));
      GameDebug.Log($"[Move failed] #{ToString()} can't use #{thismove.name} because of Heal Block");
      return false;
    }
    if (@effects.Torment && thismove.MoveId==@lastMoveUsed &&
       thismove.MoveId!=@battle.struggle.id && @effects.TwoTurnAttack==0) {
      @battle.pbDisplayPaused(_INTL("{1} can't use the same move in a row due to the torment!",ToString()));
      GameDebug.Log($"[Move failed] #{ToString()} can't use #{thismove.name} because of Torment");
      return false;
    }
    if (pbOpposing1.effects.Imprison && !pbOpposing1.isFainted())
      if(thismove.MoveId==pbOpposing1.moves[0].MoveId ||
         thismove.MoveId==pbOpposing1.moves[1].MoveId ||
         thismove.MoveId==pbOpposing1.moves[2].MoveId ||
         thismove.MoveId==pbOpposing1.moves[3].MoveId) {
        @battle.pbDisplay(_INTL("{1} can't use the sealed {2}!",ToString(),thismove.name));
        GameDebug.Log($"[Move failed] #{thismove.name} can't use #{thismove.name} because of #{pbOpposing1.ToString(true)}'s Imprison");
        return false;
      }
    if (pbOpposing2.effects.Imprison && !pbOpposing2.isFainted())
      if(thismove.MoveId==pbOpposing2.moves[0].MoveId ||
         thismove.MoveId==pbOpposing2.moves[1].MoveId ||
         thismove.MoveId==pbOpposing2.moves[2].MoveId ||
         thismove.MoveId==pbOpposing2.moves[3].MoveId) {
        @battle.pbDisplay(_INTL("{1} can't use the sealed {2}!",ToString(),thismove.name));
        GameDebug.Log($"[Move failed] #{thismove.name} can't use #{thismove.name} because of #{pbOpposing2.ToString(true)}'s Imprison");
        return false;
      }
    if (@effects.Disable>0 && thismove.MoveId==@effects.DisableMove &&
       !@battle.switching) { // Pursuit ignores if it's disabled
      @battle.pbDisplayPaused(_INTL("{1}'s {2} is disabled!",ToString(),thismove.name));
      GameDebug.Log($"[Move failed] #{ToString()}'s #{thismove.name} is disabled");
      return false;
    }
    if (choice[1]==-2) { // Battle Palace
      @battle.pbDisplay(_INTL("{1} appears incapable of using its power!",ToString()));
      GameDebug.Log($"[Move failed] Battle Palace: #{ToString()} is incapable of using its power");
      return false;
    }
    if (@effects.HyperBeam>0) {
      @battle.pbDisplay(_INTL("{1} must recharge!",ToString()));
      GameDebug.Log($"[Move failed] #{ToString()} must recharge after using #{PokeBattle_Move.pbFromPBMove(@battle,new PBMove(@currentMove)).name}");
      return false;
    }
    if (this.hasWorkingAbility(Abilities.TRUANT) && @effects.Truant) {
      @battle.pbDisplay(_INTL("{1} is loafing around!",ToString()));
      GameDebug.Log($"[Ability triggered] #{ToString()}'s Truant");
      return false;
    }
    if (!turneffects.SkipAccuracyCheck)
      if (this.status==Status.SLEEP) {
        this.statusCount-=1;
        if (this.statusCount<=0)
          this.pbCureStatus();
        else {
          this.pbContinueStatus;
          GameDebug.Log($"[Status] #{ToString()} remained asleep (count: #{this.statusCount})");
          if (!thismove.pbCanUseWhileAsleep()) { // Snore/Sleep Talk/Outrage
            GameDebug.Log($"[Move failed] #{ToString()} couldn't use #{thismove.name} while asleep");
            return false;
          }
        }
      }
    if (this.status==Status.FROZEN) {
      if (thismove.canThawUser()) {
        GameDebug.Log($"[Move effect triggered] #{ToString()} was defrosted by using #{thismove.name}");
        this.pbCureStatus(false);
        @battle.pbDisplay(_INTL("{1} melted the ice!",ToString()));
        pbCheckForm;
      }
      else if (@battle.pbRandom(10)<2 && !turneffects.SkipAccuracyCheck) {
        this.pbCureStatus();
        pbCheckForm;
      }
      else if (!thismove.canThawUser()) {
        this.pbContinueStatus;
        GameDebug.Log($"[Status] #{ToString()} remained frozen and couldn't move");
        return false;
      }
    }
    if (!turneffects.SkipAccuracyCheck)
      if (@effects.Confusion>0) {
        @effects.Confusion-=1;
        if (@effects.Confusion<=0)
          pbCureConfusion;
        else {
          pbContinueConfusion;
          GameDebug.Log($"[Status] #{ToString()} remained confused (count: #{@effects.Confusion})");
          if (@battle.pbRandom(2)==0) {
            pbConfusionDamage;
            @battle.pbDisplay(_INTL("It hurt itself in its confusion!")) ;
            GameDebug.Log($"[Status] #{ToString()} hurt itself in its confusion and couldn't move");
            return false;
          }
        }
      }
    if (@effects.Flinch) {
      @effects.Flinch=false;
      @battle.pbDisplay(_INTL("{1} flinched and couldn't move!",this.ToString()));
      GameDebug.Log($"[Lingering effect triggered] #{ToString()} flinched");
      if (this.hasWorkingAbility(Abilities.STEADFAST))
        if (pbIncreaseStatWithCause(Stats.SPEED,1,this,this.ability.ToString(TextScripts.Name)))
          GameDebug.Log($"[Ability triggered] #{ToString()}'s Steadfast");
      return false;
    }
    if (!turneffects.SkipAccuracyCheck) {
      if (@effects.Attract>=0) {
        pbAnnounceAttract(@battle.battlers[@effects.Attract]);
        if (@battle.pbRandom(2)==0) {
          pbContinueAttract;
          GameDebug.Log($"[Lingering effect triggered] #{ToString()} was infatuated and couldn't move");
          return false;
        }
      }
      if (this.status==Status.PARALYSIS)
        if (@battle.pbRandom(4)==0) {
          pbContinueStatus;
          GameDebug.Log($"[Status] #{ToString()} was fully paralysed and couldn't move");
          return false;
        }
    }
    turneffects.PassedTrying=true;
    return true;
  }
  public void pbConfusionDamage() {
    this.damagestate.Reset();
    confmove=new PokeBattle_Confusion(@battle,null);
    confmove.pbEffect(this,this);
    if (this.isFainted()) pbFaint();
  }
  public void pbUpdateTargetedMove(Move thismove,Pokemon user) {
    // TODO: Snatch, moves that use other moves
    // TODO: All targeting cases
    // Two-turn attacks, Magic Coat, Future Sight, Counter/MirrorCoat/Bide handled
  }
  public void pbProcessMoveAgainstTarget(Move thismove,Pokemon user,Pokemon target,int numhits,turneffects,bool nocheck=false,alltargets=null,bool showanimation=true) {
    int realnumhits=0;
    int totaldamage=0;
    bool destinybond=false;
    for (int i = 0; i < numhits; i++) {
      target.damagestate.Reset();
      // Check success (accuracy/evasion calculation)
      if (!nocheck &&
         !pbSuccessCheck(thismove,user,target,turneffects,i==0 || thismove.successCheckPerHit())) {
        if ((int)thismove.Effect==0xBF && realnumhits>0)   // Triple Kick
          break;   // Considered a success if Triple Kick hits at least once
        else if ((int)thismove.Effect==0x10B) {   // Hi Jump Kick, Jump Kick
          if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
            GameDebug.Log($"[Move effect triggered] #{user.ToString()} took crash damage");
            //TODO: Not shown if message is "It doesn't affect XXX..."
            @battle.pbDisplay(_INTL("{1} kept going and crashed!",user.ToString()));
            int damage=(user.TotalHP/2).floor;
            if (damage>0) {
              @battle.scene.pbDamageAnimation(user,0);
              user.ReduceHP(damage);
            }
            if (user.isFainted()) user.pbFaint();
          }
        }
        if ((int)thismove.Effect==0xD2) user.effects.Outrage=0; // Outrage
        if ((int)thismove.Effect==0xD3) user.effects.Rollout=0; // Rollout
        if ((int)thismove.Effect==0x91) user.effects.FuryCutter=0; // Fury Cutter
        if ((int)thismove.Effect==0x113) user.effects.Stockpile=0; // Spit Up
        return;
      }
      // Add to counters for moves which increase them when used in succession
      if ((int)thismove.Effect==0x91) // Fury Cutter
        if (user.effects.FuryCutter<4) user.effects.FuryCutter+=1;
      else
        user.effects.FuryCutter=0;
      if ((int)thismove.Effect==0x92) { // Echoed Voice
        if (!user.pbOwnSide.effects.EchoedVoiceUsed &&
           user.pbOwnSide.effects.EchoedVoiceCounter<5)
          user.pbOwnSide.effects.EchoedVoiceCounter+=1;
        user.pbOwnSide.effects.EchoedVoiceUsed=true;
      }
      // Count a hit for Parental Bond if it applies
      if (user.effects.ParentalBond>0) user.effects.ParentalBond-=1;
      // This hit will happen; count it
      realnumhits+=1;
      // Damage calculation and/or main effect
      int damage=thismove.pbEffect(user,target,i,alltargets,showanimation); // Recoil/drain, etc. are applied here
      if (damage>0) totaldamage+=damage;
      // Message and consume for type-weakening berries
      if (target.damagestate.BerryWeakened) {
        @battle.pbDisplay(_INTL("The {1} weakened the damage to {2}!",
           target.Item.ToString(TextScripts.Name),target.ToString(true)));
        target.pbConsumeItem();
      }
      // Illusion
      if (target.effects.Illusion.IsNotNullOrNone() && target.hasWorkingAbility(Abilities.ILLUSION) &&
         damage>0 && !target.damagestate.Substitute) {
        GameDebug.Log($"[Ability triggered] #{target.ToString()}'s Illusion ended");
        target.effects.Illusion=null;
        @battle.scene.pbChangePokemon(target,target.pokemon);
        @battle.pbDisplay(_INTL("{1}'s {2} wore off!",target.ToString(),
            target.ability.ToString(TextScripts.Name)));
      }
      if (user.isFainted())
        user.pbFaint(); // no return
      if (numhits>1 && target.damagestate.CalcDamage<=0) return;
      @battle.pbJudgeCheckpoint(user,thismove);
      // Additional effect
      if (target.damagestate.CalcDamage>0 &&
         !user.hasWorkingAbility(Abilities.SHEER_FORCE) &&
         (user.hasMoldBreaker() || !target.hasWorkingAbility(Abilities.SHIELD_DUST))) {
        addleffect=thismove.addlEffect;
        if ((user.hasWorkingAbility(Abilities.SERENE_GRACE) ||
                         user.pbOwnSide.effects.Rainbow>0) &&
                         (int)thismove.Effect!=0xA4) addleffect*=2; // Secret Power
        //if ($DEBUG && Input.press(Input::CTRL)) addleffect=100;
        if (@battle.pbRandom(100)<addleffect) {
          GameDebug.Log($"[Move effect triggered] #{thismove.name}'s added effect");
          thismove.pbAdditionalEffect(user,target);
        }
      }
      // Ability effects
      pbEffectsOnDealingDamage(thismove,user,target,damage);
      // Grudge
      if (!user.isFainted() && target.isFainted())
        if (target.effects.Grudge && target.pbIsOpposing(user.Index)) {
          thismove.PP=0;
          @battle.pbDisplay(_INTL("{1}'s {2} lost all its PP due to the grudge!",
             user.ToString(),thismove.name));
          GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Grudge made #{thismove.name} lose all its PP");
        }
      if (target.isFainted())
        destinybond=destinybond || target.effects.DestinyBond;
      if (user.isFainted()) user.pbFaint(); // no return
      if (user.isFainted()) break;
      if (target.isFainted()) break;
      // Make the target flinch
      if (target.damagestate.CalcDamage>0 && !target.damagestate.Substitute)
        if (user.hasMoldBreaker() || !target.hasWorkingAbility(Abilities.SHIELD_DUST)) {
          bool canflinch=false;
          if (user.hasWorkingItem(Items.KINGS_ROCK) || user.hasWorkingItem(Items.RAZOR_FANG)) 
             //&& thismove.canKingsRock()) //ToDo: Uncommont; Check if in Veekun's Database
            canflinch=true;
          if (user.hasWorkingAbility(Abilities.STENCH) &&
             (int)thismove.Effect!=0x09 && // Thunder Fang
             (int)thismove.Effect!=0x0B && // Fire Fang
             (int)thismove.Effect!=0x0E && // Ice Fang
             (int)thismove.Effect!=0x0F && // flinch-inducing moves
             (int)thismove.Effect!=0x10 && // Stomp
             (int)thismove.Effect!=0x11 && // Snore
             (int)thismove.Effect!=0x12 && // Fake Out
             (int)thismove.Effect!=0x78 && // Twister
             (int)thismove.Effect!=0xC7)   // Sky Attack
            canflinch=true;
          if (canflinch && @battle.pbRandom(10)==0) {
            GameDebug.Log($"[Item/ability triggered] #{user.ToString()}'s King's Rock/Razor Fang or Stench");
            target.pbFlinch(user);
          }
        }
      if (target.damagestate.CalcDamage>0 && !target.isFainted()) {
        // Defrost
        if (target.status==Status.FROZEN &&
           (thismove.pbType(thismove.Type,user,target) == Types.FIRE ||
           (Core.USENEWBATTLEMECHANICS && thismove.MoveId == Moves.SCALD)))
          target.pbCureStatus();
        // Rage
        if (target.effects.Rage && target.pbIsOpposing(user.Index))
          // TODO: Apparently triggers if opposing Pokémon uses Future Sight after a Future Sight attack
          if (target.pbIncreaseStatWithCause(Stats.ATTACK,1,target,"",true,false)) {
            GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Rage");
            @battle.pbDisplay(_INTL("{1}'s rage is building!",target.ToString()));
          }
      }
      if (target.isFainted()) target.pbFaint(); // no return
      if (user.isFainted()) user.pbFaint(); // no return
      if (user.isFainted() || target.isFainted()) break;
      // Berry check (maybe just called by ability effect, since only necessary Berries are checked)
      for (int j = 0; j< 4; j++)
        @battle.battlers[j].pbBerryCureCheck();
      if (user.isFainted() || target.isFainted()) break;
      target.pbUpdateTargetedMove(thismove,user);
      if (target.damagestate.CalcDamage<=0) break;
    }
    if (totaldamage>0) turneffects.TotalDamage+=totaldamage;
    // Battle Arena only - attack is successful
    @battle.successStates[user.Index].UseState=2;
    @battle.successStates[user.Index].TypeMod=target.damagestate.TypeMod;
    // Type effectiveness
    if (numhits>1) {
      if (target.damagestate.TypeMod>8)
        if (alltargets.Length>1)
          @battle.pbDisplay(_INTL("It's super effective on {1}!",target.ToString(true)));
        else
          @battle.pbDisplay(_INTL("It's super effective!"));
      else if (target.damagestate.TypeMod>=1 && target.damagestate.TypeMod<8)
        if (alltargets.Length>1)
          @battle.pbDisplay(_INTL("It's not very effective on {1}...",target.ToString(true)));
        else
          @battle.pbDisplay(_INTL("It's not very effective..."));
      if (realnumhits==1)
        @battle.pbDisplay(_INTL("Hit {1} time!",realnumhits.ToString()));
      else
        @battle.pbDisplay(_INTL("Hit {1} times!",realnumhits.ToString()));
    }
    GameDebug.Log($"Move did #{numhits} hit(s), total damage=#{turneffects.TotalDamage}");
    // Faint if 0 HP
    if (target.isFainted()) target.pbFaint(); // no return
    if (user.isFainted()) user.pbFaint(); // no return
    thismove.pbEffectAfterHit(user,target,turneffects);
    if (target.isFainted()) target.pbFaint(); // no return
    if (user.isFainted()) user.pbFaint(); // no return
    // Destiny Bond
    if (!user.isFainted() && target.isFainted())
      if (destinybond && target.pbIsOpposing(user.Index)) {
        GameDebug.Log($"[Lingering effect triggered] #{target.ToString()}'s Destiny Bond");
        @battle.pbDisplay(_INTL("{1} took its attacker down with it!",target.ToString()));
        user.ReduceHP(user.HP);
        user.pbFaint(); // no return
        @battle.pbJudgeCheckpoint(user);
      }
    pbEffectsAfterHit(user,target,thismove,turneffects);
    // Berry check
    for (int j = 0; j< 4; j++)
      @battle.battlers[j].pbBerryCureCheck();
    target.pbUpdateTargetedMove(thismove,user);
  }

  //public void pbUseMoveSimple(Moves moveid,sbyte index=-1,int target=-1) {
  //  choice= new List<>(); 
  //  choice[0]=1;       // "Use move"
  //  choice[1]=index;   // Index of move to be used in user's moveset
  //  choice[2]=PokeBattle_Move.pbFromPBMove(@battle,new Attack.Move(moveid)); // PokeBattle_Move object of the move
  //  choice[2].PP=-1;
  //  choice[3]=target;  // Target (-1 means no target yet)
  //  if (index>=0)
  //    @battle.choices[@index][1]=index;
  //  GameDebug.Log($"#{ToString()} used simple move #{choice[2].name}");
  //  pbUseMove(choice,true);
  //  return;
  //}

  public void pbUseMove(choice,bool specialusage=false) {
    // TODO: lastMoveUsed is not to be updated on nested calls
    // Note: user.lastMoveUsedType IS to be updated on nested calls; is used for Conversion 2
    turneffects= new List<>();
    turneffects.SpecialUsage=specialusage;
    turneffects.SkipAccuracyCheck=specialusage;
    turneffects.PassedTrying=false;
    turneffects.TotalDamage=0;
    // Start using the move
    pbBeginTurn(choice);
    // Force the use of certain moves if they're already being used
    if (@effects.TwoTurnAttack>0 ||
       @effects.HyperBeam>0 ||
       @effects.Outrage>0 ||
       @effects.Rollout>0 ||
       @effects.Uproar>0 ||
       @effects.Bide>0) {
      choice[2]=PokeBattle_Move.pbFromPBMove(@battle,new PBMove(@currentMove));
      turneffects.SpecialUsage=true;
      GameDebug.Log($"Continuing multi-turn move #{choice[2].name}");
    }
    else if (@effects.Encore>0) {
      if (@battle.pbCanShowCommands(@index) &&
         @battle.pbCanChooseMove(@index,@effects.EncoreIndex,false)) {
        if (choice[1]!=@effects.EncoreIndex) { // Was Encored mid-round
          choice[1]=@effects.EncoreIndex;
          choice[2]=@moves[@effects.EncoreIndex];
          choice[3]=-1; // No target chosen
        }
        GameDebug.Log($"Using Encored move #{choice[2].name}");
      }
    }
    thismove=choice[2];
    if (!thismove || thismove.MoveId==0) return; //if move was not chosen
    if (!turneffects.SpecialUsage) {
      // TODO: Quick Claw message
    }
    // Stance Change
    if (hasWorkingAbility(Abilities.STANCE_CHANGE) && species == Species.AEGISLASH &&
       !@effects.Transform)
      if (thismove.pbIsDamaging() && this.form!=1) {
        this.form=1;
        pbUpdate(true);
        @battle.scene.pbChangePokemon(this,@pokemon);
        @battle.pbDisplay(_INTL("{1} changed to Blade Forme!",ToString()));
        GameDebug.Log($"[Form changed] #{ToString()} changed to Blade Forme");
      }
      else if (thismove.MoveId == Moves.KINGSSHIELD && this.form!=0) {
        this.form=0;
        pbUpdate(true);
        @battle.scene.pbChangePokemon(this,@pokemon);
        @battle.pbDisplay(_INTL("{1} changed to Shield Forme!",ToString()));
        GameDebug.Log($"[Form changed] #{ToString()} changed to Shield Forme");
      }
    // Record that user has used a move this round (ot at least tried to)
    this.lastRoundMoved=@battle.turncount;
    // Try to use the move
    if (!pbTryUseMove(choice,thismove,turneffects)) {
      this.lastMoveUsed=-1;
      this.lastMoveUsedType=-1;
      if (!turneffects.SpecialUsage) {
        if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=-1;
        this.lastRegularMoveUsed=-1;
      }
      pbCancelMoves;
      @battle.pbGainEXP;
      pbEndTurn(choice);
      @battle.pbJudge; //      @battle.pbSwitch
      return;
    }
    if (!turneffects.SpecialUsage)
      if (!pbReducePP(thismove)) {
        @battle.pbDisplay(_INTL("{1} used\r\n{2}!",ToString(),thismove.name));
        @battle.pbDisplay(_INTL("But there was no PP left for the move!"));
        this.lastMoveUsed=-1;
        this.lastMoveUsedType=-1;
        if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=-1;
        this.lastRegularMoveUsed=-1;
        pbEndTurn(choice);
        @battle.pbJudge; //        @battle.pbSwitch
        GameDebug.Log($"[Move failed] #{thismove.name} has no PP left");
        return;
      }
    // Remember that user chose a two-turn move
    if (thismove.pbTwoTurnAttack(this)) {
      // Beginning use of two-turn attack
      @effects.TwoTurnAttack=thismove.MoveId;
      @currentMove=thismove.MoveId;
    }
    else
      @effects.TwoTurnAttack=0; // Cancel use of two-turn attack
    // Charge up Metronome Item
    if (this.lastMoveUsed==thismove.MoveId)
      this.effects.Metronome+=1;
    else
      this.effects.Metronome=0;
    // "X used Y!" message
    switch (thismove.pbDisplayUseMessage(this)) {
    case 2:   // Continuing Bide
      return;
    case 1:   // Starting Bide
      this.lastMoveUsed=thismove.MoveId;
      this.lastMoveUsedType=thismove.pbType(thismove.Type,this,null);
      if (!turneffects.SpecialUsage) {
        if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=thismove.MoveId;
        this.lastRegularMoveUsed=thismove.MoveId;
      }
      @battle.lastMoveUsed=thismove.MoveId;
      @battle.lastMoveUser=this.Index;
      @battle.successStates[this.Index].UseState=2;
      @battle.successStates[this.Index].TypeMod=8;
      return;
    case -1:   // Was hurt while readying Focus Punch, fails use
      this.lastMoveUsed=thismove.MoveId;
      this.lastMoveUsedType=thismove.pbType(thismove.Type,this,null);
      if (!turneffects.SpecialUsage) {
        if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=thismove.MoveId;
        this.lastRegularMoveUsed=thismove.MoveId;
      }
      @battle.lastMoveUsed=thismove.MoveId;
      @battle.lastMoveUser=this.Index;
      @battle.successStates[this.Index].UseState = 2;// somehow treated as a success
      @battle.successStates[this.Index].TypeMod=8;
      GameDebug.Log($"[Move failed] #{ToString()} was hurt while readying Focus Punch");
      return;
    }
    // Find the user and target(s)
    targets= new List<>();
    user=pbFindUser(choice,targets);
    // Battle Arena only - assume failure 
    @battle.successStates[user.Index].UseState=1;
    @battle.successStates[user.Index].TypeMod=8;
    // Check whether Selfdestruct works
    if (!thismove.pbOnStartUse(user)) { // Selfdestruct, Natural Gift, Beat Up can return false here
      GameDebug.Log(string.Format("[Move failed] Failed pbOnStartUse (function code %02X)",thismove.Effect));
      user.lastMoveUsed=thismove.MoveId;
      user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
      if (!turneffects.SpecialUsage) {
        if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.MoveId;
        user.lastRegularMoveUsed=thismove.MoveId;
      }
      @battle.lastMoveUsed=thismove.MoveId;
      @battle.lastMoveUser=user.Index;
      return;
    }
    // Primordial Sea, Desolate Land
    if (thismove.pbIsDamaging())
      switch (@battle.pbWeather) {
      case Weather.HEAVYRAIN:
        if (thismove.pbType(thismove.Type,user,null) == Types.FIRE) {
          GameDebug.Log($"[Move failed] Primordial Sea's rain cancelled the Fire-type #{thismove.name}");
          @battle.pbDisplay(_INTL("The Fire-type attack fizzled out in the heavy rain!"));
          user.lastMoveUsed=thismove.MoveId;
          user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
          if (!turneffects.SpecialUsage) {
            if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.MoveId;
            user.lastRegularMoveUsed=thismove.MoveId;
          }
          @battle.lastMoveUsed=thismove.MoveId;
          @battle.lastMoveUser=user.Index;
          return;
        }
        break;
      case Weather.HARSHSUN:
        if (thismove.pbType(thismove.Type,user,null) == Types.WATER) {
          GameDebug.Log($"[Move failed] Desolate Land's sun cancelled the Water-type #{thismove.name}");
          @battle.pbDisplay(_INTL("The Water-type attack evaporated in the harsh sunlight!"));
          user.lastMoveUsed=thismove.MoveId;
          user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
          if (!turneffects.SpecialUsage) {
            if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.MoveId;
            user.lastRegularMoveUsed=thismove.MoveId;
          }
          @battle.lastMoveUsed=thismove.MoveId;
          @battle.lastMoveUser=user.Index;
          return;
        }
        break;
      }
    // Powder
    if (user.effects.Powder && thismove.pbType(thismove.Type,user,null) == Types.FIRE) {
      GameDebug.Log($"[Lingering effect triggered] #{ToString()}'s Powder cancelled the Fire move");
      @battle.pbCommonAnimation("Powder",user,null);
      @battle.pbDisplay(_INTL("When the flame touched the powder on the Pokémon, it exploded!"));
      if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) user.ReduceHP(1+(user.TotalHP/4).floor);
      user.lastMoveUsed=thismove.MoveId;
      user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
      if (!turneffects.SpecialUsage) {
        if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.MoveId;
        user.lastRegularMoveUsed=thismove.MoveId;
      }
      @battle.lastMoveUsed=thismove.MoveId;
      @battle.lastMoveUser=user.Index;
      if (user.isFainted()) user.pbFaint();
      pbEndTurn(choice);
      return;
    }
    // Protean
    if (user.hasWorkingAbility(Abilities.PROTEAN) &&
       (int)thismove.Effect!=0xAE &&   // Mirror Move
       (int)thismove.Effect!=0xAF &&   // Copycat
       (int)thismove.Effect!=0xB0 &&   // Me First
       (int)thismove.Effect!=0xB3 &&   // Nature Power
       (int)thismove.Effect!=0xB4 &&   // Sleep Talk
       (int)thismove.Effect!=0xB5 &&   // Assist
       (int)thismove.Effect!=0xB6) {    // Metronome
      Types movetype=thismove.pbType(thismove.Type,user,null);
      if (!user.hasType(movetype)) {
        typename=movetype.ToString(TextScripts.Name);
        GameDebug.Log($"[Ability triggered] #{ToString()}'s Protean made it #{typename}-type");
        user.Type1=movetype;
        user.Type2=movetype;
        user.effects.Type3=-1;
        @battle.pbDisplay(_INTL("{1} transformed into the {2} type!",user.ToString(),typename))  ;
      }
    }
    // Try to use move against user if there aren't any targets
    if (targets.Length==0) {
      user=pbChangeUser(thismove,user);
      if(thismove.target==Targets.SingleNonUser ||
         thismove.target==Targets.RandomOpposing ||
         thismove.target==Targets.AllOpposing ||
         thismove.target==Targets.AllNonUsers ||
         thismove.target==Targets.Partner ||
         thismove.target==Targets.UserOrPartner ||
         thismove.target==Targets.SingleOpposing ||
         thismove.target==Targets.OppositeOpposing)
        @battle.pbDisplay(_INTL("But there was no target..."));
      else
        //GameDebug.logonerr(thismove.pbEffect(user,null));
        try{ thismove.pbEffect(user, null); } catch { GameDebug.LogError(""); }
    }
    else {
      // We have targets
      bool showanimation=true;
      alltargets= new List<>();
      for (int i = 0; i < targets.Length; i++)
        if (!targets.Contains(targets[i].Index)) alltargets.Add(targets[i].Index);
      // For each target in turn
      if (i>=targets.Length) { i=0; //loop do break; //ToDo: Make a part of for-loop or as a do-while loop?
        // Get next target
        userandtarget=[user,targets[i]];
        success=pbChangeTarget(thismove,userandtarget,targets);
        user=userandtarget[0];
        target=userandtarget[1];
        if (i==0 && thismove.target==PBTargets::AllOpposing)
          // Add target's partner to list of targets
          pbAddTarget(targets,target.pbPartner);
        // If couldn't get the next target
        if (!success) {
          i+=1;
          continue; 
        }
        // Get the number of hits
        numhits=thismove.pbNumHits(user);
        // Reset damage state, set Focus Band/Focus Sash to available
        target.damagestate.Reset();
        // Use move against the current target
        pbProcessMoveAgainstTarget(thismove,user,target,numhits,turneffects,false,alltargets,showanimation);
        showanimation=false;
        i+=1;
      }
    }
    // Pokémon switching caused by Roar, Whirlwind, Circle Throw, Dragon Tail, Red Card
    if (!user.isFainted()) {
      switched= new List<>();
      for (int i = 0; i < 4; i++)
        if (@battle.battlers[i].effects.Roar) {
          @battle.battlers[i].effects.Roar=false;
          @battle.battlers[i].effects.Uturn=false;
          if (@battle.battlers[i].isFainted()) continue; 
          if (!@battle.pbCanSwitch(i,-1,false)) continue; 
          choices= new List<>();
          party=@battle.pbParty(i);
          for (int j = 0; j< party.Length; j++)
            if (@battle.pbCanSwitchLax(i,j,false)) choices.Add(j);
          if (choices.Length>0) {
            newpoke=choices[@battle.pbRandom(choices.Length)];
            newpokename=newpoke;
            if (party[newpoke].ability == Abilities.ILLUSION)
              newpokename=pbGetLastPokeInTeam(i);
            switched.Add(i);
            @battle.battlers[i].pbResetForm;
            @battle.pbRecallAndReplace(i,newpoke,newpokename,false,user.hasMoldBreaker());
            @battle.pbDisplay(_INTL("{1} was dragged out!",@battle.battlers[i].ToString()));
            @battle.choices[i]=[0,0,null,-1];   // Replacement Pokémon does nothing this round
          }
        }
      for (int i = 0; i< @battle.pbPriority; i++) {
        if (!switched.include(i.Index)) continue; 
        i.pbAbilitiesOnSwitchIn(true);
      }
    }
    // Pokémon switching caused by U-Turn, Volt Switch, Eject Button
    switched= new List<>();
    for (int i = 0; i < 4; i++)
      if (@battle.battlers[i].effects.Uturn) {
        @battle.battlers[i].effects.Uturn=false;
        @battle.battlers[i].effects.Roar=false;
        if (!@battle.battlers[i].isFainted() && @battle.pbCanChooseNonActive(i) &&
           !@battle.pbAllFainted(@battle.pbOpposingParty(i))) {
          // TODO: Pursuit should go here, and negate this effect if it KO's attacker
          @battle.pbDisplay(_INTL("{1} went back to {2}!",@battle.battlers[i].ToString(),@battle.pbGetOwner(i).name));
          newpoke=0;
          newpoke=@battle.pbSwitchInBetween(i,true,false);
          newpokename=newpoke;
          if (@battle.pbParty(i)[newpoke].ability == Abilities.ILLUSION)
            newpokename=pbGetLastPokeInTeam(i);
          switched.Add(i);
          @battle.battlers[i].pbResetForm;
          @battle.pbRecallAndReplace(i,newpoke,newpokename,@battle.battlers[i].effects.BatonPass);
          @battle.choices[i]=[0,0,null,-1];   // Replacement Pokémon does nothing this round
        }
      }
    for (int i = 0; i< @battle.pbPriority; i++) {
      if (!switched.include(i.Index)) continue; 
      i.pbAbilitiesOnSwitchIn(true);
    }
    // Baton Pass
    if (user.effects.BatonPass) {
      user.effects.BatonPass=false;
      if (!user.isFainted() && @battle.pbCanChooseNonActive(user.Index) &&
         !@battle.pbAllFainted(@battle.pbParty(user.Index))) {
        newpoke=0;
        newpoke=@battle.pbSwitchInBetween(user.Index,true,false);
        newpokename=newpoke;
        if (@battle.pbParty(user.Index)[newpoke].ability == Abilities.ILLUSION)
          newpokename=pbGetLastPokeInTeam(user.Index);
        user.pbResetForm;
        @battle.pbRecallAndReplace(user.Index,newpoke,newpokename,true);
        @battle.choices[user.Index]=[0,0,null,-1];   // Replacement Pokémon does nothing this round
        user.pbAbilitiesOnSwitchIn(true);
      }
    }
    // Record move as having been used
    user.lastMoveUsed=thismove.MoveId;
    user.lastMoveUsedType=thismove.pbType(thismove.Type,user,null);
    if (!turneffects.SpecialUsage) {
      if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.MoveId;
      user.lastRegularMoveUsed=thismove.MoveId;
      if (!user.movesUsed.include(thismove.MoveId)) user.movesUsed.Add(thismove.MoveId); // For Last Resort
    }
    @battle.lastMoveUsed=thismove.MoveId;
    @battle.lastMoveUser=user.Index;
    // Gain Exp
    @battle.pbGainEXP;
    // Battle Arena only - update skills
    for (int i = 0; i < 4; i++)
      @battle.successStates[i].updateSkill;
    // End of move usage
    pbEndTurn(choice);
    @battle.pbJudge; //    @battle.pbSwitch
    return;
  }
  public pbCancelMoves() {
    // If failed pbTryUseMove or have already used Pursuit to chase a switching foe
    // Cancel multi-turn attacks (note: Hyper Beam effect is not canceled here)
    if (@effects.TwoTurnAttack>0) @effects.TwoTurnAttack=0;
    @effects.Outrage=0;
    @effects.Rollout=0;
    @effects.Uproar=0;
    @effects.Bide=0;
    @currentMove=0;
    // Reset counters for moves which increase them when used in succession
    @effects.FuryCutter=0;
    GameDebug.Log($"Cancelled using the move");
  }
#endregion
#region Turn processing
  public pbBeginTurn(choice) {
    // Cancel some lingering effects which only apply until the user next moves
    @effects.DestinyBond=false;
    @effects.Grudge=false;
    // Reset Parental Bond's count
    @effects.ParentalBond=0;
    // Encore's effect ends if the encored move is no longer available
    if (@effects.Encore>0 &&
       @moves[@effects.EncoreIndex].id!=@effects.EncoreMove) {
      GameDebug.Log($"Resetting Encore effect");
      @effects.Encore=0;
      @effects.EncoreIndex=0;
      @effects.EncoreMove=0;
    }
    // Wake up in an uproar
    if (this.status==Status.SLEEP && !this.hasWorkingAbility(Abilities.SOUNDPROOF))
      for (int i = 0; i < 4; i++)
        if (@battle.battlers[i].effects.Uproar>0) {
          pbCureStatus(false);
          @battle.pbDisplay(_INTL("{1} woke up in the uproar!",ToString()));
        }
  }
  public void pbEndTurn(choice) {
    // True end(?)
    if (@effects.ChoiceBand<0 && @lastMoveUsed>=0 && !this.isFainted() && 
       (this.hasWorkingItem(Items.CHOICEBAND) ||
       this.hasWorkingItem(Items.CHOICESPECS) ||
       this.hasWorkingItem(Items.CHOICESCARF)))
      @effects.ChoiceBand=@lastMoveUsed;
    @battle.pbPrimordialWeather;
    for (int i = 0; i < 4; i++)
      @battle.battlers[i].pbBerryCureCheck();
    for (int i = 0; i < 4; i++)
      @battle.battlers[i].pbAbilityCureCheck();
    for (int i = 0; i < 4; i++)
      @battle.battlers[i].pbAbilitiesOnSwitchIn(false);
    for (int i = 0; i < 4; i++)
      @battle.battlers[i].pbCheckForm;
  }
  public bool pbProcessTurn(choice) {
    // Can't use a move if fainted
    if (this.isFainted()) return false;
    // Wild roaming Pokémon always flee if possible
    if (!@battle.opponent && @battle.pbIsOpposing(this.Index) &&
       @battle.rules["alwaysflee"] && @battle.pbCanRun(this.Index)) {
      pbBeginTurn(choice);
      @battle.pbDisplay(_INTL("{1} fled!",this.ToString()));
      @battle.decision=3;
      pbEndTurn(choice);
      GameDebug.Log($"[Escape] #{ToString()} fled");
      return true;
    }
    // If this battler's action for this round wasn't "use a move"
    if (choice[0]!=1) {
      // Clean up effects that end at battler's turn
      pbBeginTurn(choice);
      pbEndTurn(choice);
      return false;
    }
    // Turn is skipped if Pursuit was used during switch
    if (@effects.Pursuit) {
      @effects.Pursuit=false;
      pbCancelMoves;
      pbEndTurn(choice);
      @battle.pbJudge; //      @battle.pbSwitch
      return false;
    }
    // Use the move //   @battle.pbDisplayPaused("Before: [#{@lastMoveUsedSketch},#{@lastMoveUsed}]");
    GameDebug.Log($"#{ToString()} used #{choice[2].name}");
    GameDebug.logonerr(pbUseMove(choice,choice[2]==@battle.struggle));//ToDo: try(move)/catch(logerror) //   @battle.pbDisplayPaused("After: [#{@lastMoveUsedSketch},#{@lastMoveUsed}]");
    return true;
  }
		#endregion

		#region ToDo: Everything here needs to be implemented
		/// <summary>
		/// Set item to none, if it can only be used once
		/// </summary>
		public void pbConsumeItem() { }
		public void pbConsumeItem(bool one, bool two) { }
		public void pbCancelMoves() { }
		public void pbHyperMode() { }
		public void pbFaint() { }
		public void pbAbilityCureCheck() { }
		public void ReduceHP(int amount) { }
		public void pbBerryCureCheck() { }
		public void pbActivateBerryEffect(Items item, bool something) { }
		public void pbAbilitiesOnSwitchIn(bool animate = false) { }
		public void pbUseMoveSimple(Moves move) { }
		public float Weight(Pokemon pkmn)
		{
			return pkmn.weight;
			//return 0f;
		}
		public void pbUseMoveSimple(Moves pkmn, int uhh, int index)
		{
			//return false;
		}
		public bool pbAddTarget(byte index, Pokemon pkmn)
		{
			return false;
		}
		public void pbRandomTarget(byte index)
		{
			//return false;
		}
		public void pbSetPP(Moves move, int index)
		{
			//return false;
		}
		public Pokemon pbOppositeOpposing { get; set; }
		public float weight { get; set; }
		public Pokemon pbOpposing1 { get; set; }
		public Pokemon pbOpposing2 { get; set; }
		#endregion

#region
		/*public static implicit operator Pokemon[](Pokemon[] input)
		{
			Pokemon[] battlers = new Pokemon[input.Length];
			return battlers;
		}

		public static implicit operator Pokemon(Pokemon input)
		{
			Pokemon[] battlers = new Pokemon[input.Length];
			return battlers;
		}*/

		public static Pokemon[] GetBattlers(PokemonUnity.Monster.Pokemon[] input, Battle btl)
		{
			Pokemon[] battlers = new Pokemon[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				//battlers[i] = (Pokemon)input[i];
				battlers[i] = (Pokemon)new Pokemon(btl, (sbyte)i);
				battlers[i].InitPokemon(input[i], (sbyte)i);
			}
			return battlers;
		}
		/* ToDo: Move to mono class...
		public void BattlerUI(BattlePokemonHandler gameObject, bool player = false)
		{
			if (player) playerHUD = gameObject;
			else nonplayerHUD = gameObject;
		}
		/// <summary>
		/// Refreshes the HUD of this Pokemon
		/// </summary>
		/// ToDo: Make this into a MonoBehaviour Class?
		private void UpdateUI()
		{
			playerHUD.HP 				= nonplayerHUD.HP 				= HP;
			playerHUD.TotalHP 			= nonplayerHUD.TotalHP 			= TotalHP;
			playerHUD.expSlider.value 	= nonplayerHUD.expSlider.value 	= Exp.Current;
			playerHUD.expSlider.maxValue = nonplayerHUD.expSlider.maxValue = Exp.NextLevel;
			playerHUD.Level.text 		= nonplayerHUD.Level.text 		= Level.ToString();
			playerHUD.Name.text 		= nonplayerHUD.Name.text 		= Name;
			playerHUD.Gender 			= nonplayerHUD.Gender 			= Gender;
			playerHUD.Status 			= nonplayerHUD.Status 			= Status;
			playerHUD.Item 				= nonplayerHUD.Item 			= Item != Items.NONE;
		}*/

		public static implicit operator Monster.Pokemon(PokemonUnity.Battle.Pokemon pkmn)
		{
			Monster.Pokemon pokemon = pkmn.pokemon;
			//if (pokemon == null) return null;
			if ((Pokemons)pokemon.Species == Pokemons.NONE) return new Monster.Pokemon(Pokemons.NONE);
			Ribbon[] ribbons = new Ribbon[pokemon.Ribbons.Count];
			for (int i = 0; i < ribbons.Length; i++)
			{
				ribbons[i] = (Ribbon)pokemon.Ribbons[i];
			}

			Attack.Move[] moves = new Attack.Move[pokemon.moves.Length];
			for (int i = 0; i < moves.Length; i++)
			{
				moves[i] = pokemon.moves[i];
			}

			Monster.Pokemon normalPokemon = pokemon;
				//new Monster.Pokemon
				//(
				//	(Pokemons)pokemon.Species, new Character.TrainerId(pokemon.TrainerName, pokemon.TrainerIsMale,
				//	tID: pokemon.TrainerTrainerId, sID: pokemon.TrainerSecretId),
				//	pokemon.NickName, pokemon.Form, (Abilities)pokemon.Ability,
				//	(Monster.Natures)pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
				//	pokemon.Pokerus, pokemon.IsHyperMode, pokemon.ShadowLevel,
				//	pokemon.CurrentHP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
				//	pokemon.ObtainedLevel, /*pokemon.CurrentLevel,*/ pokemon.CurrentExp,
				//	pokemon.Happines, (Status)pokemon.Status, pokemon.StatusCount,
				//	pokemon.EggSteps, (Items)pokemon.BallUsed, pokemon.Mail.Message,
				//	(Attack.Move[])moves, ribbons, pokemon.Markings, pokemon.PersonalId,
				//	(Monster.Pokemon.ObtainedMethod)pokemon.ObtainedMethod,
				//	pokemon.TimeReceived, pokemon.TimeEggHatched
				//);
			return normalPokemon;
		}

		//public static implicit operator PokemonUnity.Battle.Pokemon(Monster.Pokemon pokemon)
		//{
		//	Pokemon pkmn = new Pokemon();
		//	if (pokemon == null) return pkmn;

		//	if (pokemon != null && pokemon.Species != Pokemons.NONE)
		//	{
		//		pkmn.PersonalId				= pokemon.PersonalId;
		//		//PublicId in pokemon is null, so Pokemon returns null
		//		//pkmn.PublicId				= pokemon.PublicId;

		//		if (!pokemon.OT.Equals((object)null))
		//		{
		//			pkmn.TrainerName		= pokemon.OT.Name;
		//			pkmn.TrainerIsMale		= pokemon.OT.Gender;
		//			pkmn.TrainerTrainerId	= pokemon.OT.TrainerID;
		//			pkmn.TrainerSecretId	= pokemon.OT.SecretID;
		//		}

		//		pkmn.Species				= (int)pokemon.Species;
		//		pkmn.form					= pokemon.Form;
		//		//Creates an error System OutOfBounds inside Pokemon
		//		pkmn.NickName				= pokemon.Name;

		//		pkmn.Ability				= (int)pokemon.Ability;

		//		//pkmn.Nature				= pokemon.getNature();
		//		pkmn.Nature					= (int)pokemon.Nature;
		//		pkmn.IsShiny				= pokemon.IsShiny;
		//		pkmn.Gender					= pokemon.Gender;

		//		//pkmn.PokerusStage			= pokemon.PokerusStage;
		//		pkmn.Pokerus				= pokemon.Pokerus;
		//		//pkmn.PokerusStrain		= pokemon.PokerusStrain;

		//		pkmn.IsHyperMode			= pokemon.isHyperMode;
		//		//pkmn.IsShadow				= pokemon.isShadow;
		//		pkmn.ShadowLevel			= pokemon.ShadowLevel;
				
		//		pkmn.CurrentHP				= pokemon.HP;
		//		pkmn.Item					= (int)pokemon.Item;
				
		//		pkmn.IV						= pokemon.IV;
		//		pkmn.EV						= pokemon.EV;
				
		//		pkmn.ObtainedLevel			= pokemon.ObtainLevel;
		//		//pkmn.CurrentLevel			= pokemon.Level;
		//		pkmn.CurrentExp				= pokemon.Exp.Current;
				
		//		pkmn.Happines				= pokemon.Happiness;
				
		//		pkmn.Status					= (int)pokemon.Status;
		//		pkmn.StatusCount			= pokemon.StatusCount;
				
		//		pkmn.EggSteps				= pokemon.EggSteps;
				
		//		pkmn.BallUsed				= (int)pokemon.ballUsed;
		//		if (pokemon.Item != Items.NONE && Game.ItemData[pokemon.Item].IsLetter)//PokemonUnity.Inventory.Mail.IsMail(pokemon.Item))
		//		{
		//			pkmn.Mail				= new SeriMail(pokemon.Item, pokemon.Mail);
		//		}

		//		pkmn.Moves					= new SeriMove[4];
		//		for (int i = 0; i < 4; i++)
		//		{
		//			pkmn.Moves[i]			= pokemon.moves[i];
		//		}

		//		//Ribbons is also null, we add a null check
		//		if (pokemon.Ribbons != null)
		//		{
		//			pkmn.Ribbons			= new int[pokemon.Ribbons.Count];
		//			for (int i = 0; i < pkmn.Ribbons.Length; i++)
		//			{
		//				pkmn.Ribbons[i]		= (int)pokemon.Ribbons[i];
		//			}
		//		}
		//		//else //Dont need else, should copy whatever value is given, even if null...
		//		//{
		//		//	pkmn.Ribbons			= new int[0];
		//		//}
		//		pkmn.Markings				= pokemon.Markings;
				
		//		pkmn.ObtainedMethod			= (int)pokemon.ObtainedMode;
		//		pkmn.TimeReceived			= pokemon.TimeReceived;
		//		//try
		//		//{
		//			pkmn.TimeEggHatched		= pokemon.TimeEggHatched;
		//		//}
		//		//catch (Exception) { seriPokemon.TimeEggHatched = new DateTimeOffset(); }
		//	}

		//	return pkmn;
		//}
        #endregion
	}
#pragma warning restore 0162
}