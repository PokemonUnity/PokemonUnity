using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	/// <summary>
	/// A Pokemon placeholder class to be used while in-battle, 
	/// to prevent changes from being permanent to original pokemon profile
	/// </summary>
	/// ToDo: Create a SaveResults() after battle has ended, to make changes permanent.
	public class Pokemon : PokemonUnity.Monster.Pokemon
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
		public Battle battle { get { return Game.battle; } }
		public bool captured { get; private set; }
		//public bool Fainted { get { return isFainted(); } }
		public Battle.DamageState damagestate { get; set; }
		/// <summary>
		/// Int Buffs and debuffs (gains and loss) affecting this pkmn.
		/// 0: Attack, 1: Defense, 2: Speed, 3: SpAtk, 4: SpDef, 5: Evasion, 6: Accuracy
		/// </summary>
		public int[] stages { get; private set; }//ToDo: sbyte?
		/// <summary>
		/// Returns the position of this pkmn in battle lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == battle.party[this.pkmn.index]
		public sbyte Index { get; private set; }
		//[Obsolete]
		//private int Index { get { return this.battle.battlers.Length; } }
		/// <summary>
		/// Returns the position of this pkmn in party lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == party[this.pkmn.index]
		public byte pokemonIndex { get; private set; }
		public bool IsOwned { get { return Game.Player.Pokedex[_base.ArrayId, 1] == 1; } }
		private PokemonUnity.Monster.Pokemon pokemon { get; set; }
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
		new public Move[] moves { get; set; }

		public int GetWeight(Pokemon attacker = null)
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
		/*public Pokemon(Pokemon replacingPkmn, bool batonpass)
		{
			return replacingPkmn;
		}*/
		//[Obsolete("Don't think this is needed or should be used")]
		public Pokemon() : base() //Battle btl, int idx
		{
			//battle		= btl;
			//Index			= idx;
			//hp			= 0;
			//totalhp		= 0;
			//fainted		= true;
			captured		= false;
			stages			= new int[7];
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
			/*effects = new Effects.Pokemon(batonpass);
			effects = new Effects.Pokemon(batonpass);
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
		private void InitPokemon(PokemonUnity.Monster.Pokemon pkmn, sbyte pkmnIndex)
		{
			if (pkmn.isEgg)
			{
				//Remove/Disable UI for Egg 
				//Pause game to display error message?
				//"An egg can't be an active Pokémon"
				Game.Dialog(LanguageExtension.Translate(Text.Errors, "ActiveEgg").Value);
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
				moves			= new Move[] {
					(Move)base.moves[0],
					(Move)base.moves[1],
					(Move)base.moves[2],
					(Move)base.moves[3]
				};
			}
#if (DEBUG == false || UNITY_EDITOR)
			UpdateUI();
#endif
		}
		public Pokemon Update(bool fullchange = false)
		{
			if(Species != Pokemons.NONE)
			{
				//calcStats(); //Not needed since fetching stats from base ( Pokemon => Pokemon )
				//ToDo: Uncomment and fetch data from baseClass
				//Level		= pokemon.Level;
				//HP		= pokemon.HP;
				//TotalHP	= pokemon.TotalHP;
				//Pokemon	= Pokemon, so not all stats need to be handpicked
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
		public Pokemon Reset()
		{
			pokemon		= new Pokemon();
			Index		= -1;
			InitEffects(false);
			//reset status
			Status		= Status.NONE;
			StatusCount	= 0;
			//IsFainted	= true;
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
			return new PokemonUnity.Inventory.Item(this.Item).ItemPocket == ItemPockets.BERRY;//pbIsBerry?(@item)
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
				//battle.OwnedByPlayer(Index) &&
				Game.Player.BadgesCount >= Core.BADGESBOOSTSPEED)
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
				Game.Dialog(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
			if (HP > TotalHP)
				//"HP greater than total HP"
				Game.Dialog(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
			//ToDo: Pass to UnityEngine
			//if (amt > 0)
			//	battle.scene.HPChanged(Index, oldhp, animate); //Unity takes over
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
				Game.Dialog(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
			if (HP > TotalHP)
				//"HP greater than total HP"
				Game.Dialog(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
			//ToDo: Pass to UnityEngine
			//if(amount > 0)
			//	battle.scene.HPChanged(Index, oldhp, animate); //Unity takes over
			//ToDo: Fix return
			return amount;
		}

		public void Faint(bool showMessage = true)
		{
			if(!isFainted() && HP > 0)
			{
				Game.DebugLog("Can't faint with HP greater than 0", true);
				//return true;
			}
			if(isFainted())
			{
				Game.DebugLog("Can't faint if already fainted", false);
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
			battle.choices[Index] = new Battle.Choice(ChoiceAction.NoAction);
			OwnSide.LastRoundFainted = battle.turncount;
			if (showMessage)
				Game.Dialog(LanguageExtension.Translate(Text.Errors, "Fainted", new string[] { ToString() }).Value);
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
				Game.DebugLog(string.Format("[Form changed] {0} changed to form {1}", ToString(), Form));
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
					Game.DebugLog(string.Format("[Ability triggered] {0}'s Primordial Sea made it rain heavily", ToString()));
				}
				if(hasWorkingAbility(Abilities.DESOLATE_LAND) && battle.Weather != Weather.HARSHSUN)
				{
					battle.SetWeather(Weather.HARSHSUN);
					battle.weatherduration = -1;
					//battle.CommonAnimation("HarshSun", null, null);
					//"{1}'s {2} turned the sunlight extremely harsh!"
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HarshSunStart", ToString(), Ability.ToString().Translate().Value).Value);
					Game.DebugLog(string.Format("[Ability triggered] {0}'s Desolate Land made the sun shine harshly", ToString()));
				}
				if(hasWorkingAbility(Abilities.DELTA_STREAM) && battle.Weather != Weather.STRONGWINDS)
				{
					battle.SetWeather(Weather.STRONGWINDS);
					battle.weatherduration = -1;
					//battle.CommonAnimation("StrongWinds", null, null);
					//"{1}'s {2} caused a mysterious air current that protects Flying-type Pokémon!"
					//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "StrongWindsStart", ToString(), Ability.ToString().Translate().Value).Value);
					Game.DebugLog(string.Format("[Ability triggered] {0}'s Delta Stream made an air current blow", ToString()));
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
						//battle.CommonAnimation("Rain", null, null);
						//"{1}'s {2} made it rain!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "RainStart", ToString(), Ability.ToString().Translate().Value).Value);
						Game.DebugLog(string.Format("[Ability triggered] {0}'s Drizzle made it rain", ToString()));
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
						//battle.CommonAnimation("Sunny", null, null);
						//"{1}'s {2} intensified the sun's rays!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SunnyStart", ToString(), Ability.ToString().Translate().Value).Value);
						Game.DebugLog(string.Format("[Ability triggered] {0}'s Drought made it sunny", ToString()));
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
						//battle.CommonAnimation("Sandstorm", null, null);
						//"{1}'s {2} whipped up a sandstorm!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "SandstormStart", ToString(), Ability.ToString().Translate().Value).Value);
						Game.DebugLog(string.Format("[Ability triggered] {0}'s Sand Stream made it sandstorm", ToString()));
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
						//battle.CommonAnimation("Hail", null, null);
						//"{1}'s {2} madeit hail!"
						//battle.pbDisplay(LanguageExtension.Translate(Text.ScriptTexts, "HailStart", ToString(), Ability.ToString().Translate().Value).Value);
						Game.DebugLog(string.Format("[Ability triggered] {0}'s Snow Warning made it hail", ToString()));
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
					Game.DebugLog(string.Format("[Ability nullified] {0}'s Ability cancelled weather effects", ToString()));
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
					//ToDo: WIP; Finish from here...
				}
			}

		}
		#endregion

		#region ToDo: Everything here needs to be implemented
		new public Items Item { get; set; } //ToDo: Fix this
		new public Types Type1 { get; set; }
		new public Types Type2 { get; set; }
		public byte[] baseStats { get; set; }
		public int gender { get; set; }
		public int level { get; set; }
		public int attack { get; set; }
		public int defense { get; set; }
		public int spatk { get; set; }
		public int spdef { get; set; }
		public int speed { get; set; }
		public int pbSpeed { get; set; }
		public int happiness { get; set; }
		public Moves lastMoveUsedSketch { get; set; }
		/// <summary>
		/// Does faint animation 
		/// (doesn't have to lower hp, as hp will be done by other set of code. 
		/// But just to be safe... do it anyways)
		/// </summary>
		public void pbFaint() { }
		public void pbConsumeItem() { }
		public void pbConsumeItem(bool one, bool two) { }
		public void pbBerryCureCheck() { }
		public void pbAbilityCureCheck() { }
		public void pbActivateBerryEffect(Items item, bool something) { }
		public void pbConfuse() { }
		public void pbSleep() { }
		public void pbSleepSelf(int turns) { }
		public void pbFreeze() { }
		public void pbAttract(Pokemon pkmn, byte? uh = null, bool animate = false) { }
		public void pbFlinch(Pokemon pkmn, byte? uh = null, bool animate = false) { }
		public void pbPoison(Pokemon pkmn, byte? uh = null, bool animate = false) { }
		public void pbParalyze(Pokemon pkmn, byte? uh = null, bool animate = false) { }
		public void pbBurn(Pokemon pkmn, byte? uh = null, bool animate = false) { }
		public void pbCureStatus(bool animate = false) { }
		public void pbCureAttract(bool animate = false) { }
		public void pbAbilitiesOnSwitchIn(bool animate = false) { }
		public void pbUseMoveSimple(Moves move) { }
		public Pokemon pbOppositeOpposing { get; set; }
		public float weight { get; set; }
		public float Weight(Pokemon pkmn)
		{
			return pkmn.weight;
			//return 0f;
		}
		public bool pbHasType(Types type)
		{
			return false;
		}
		public bool pbIsStatus()
		{
			return false;
		}
		public bool pbTooLow(Stats stat)
		{
			return false;
		}
		public bool pbCanAttract(Pokemon pkmn)
		{
			return false;
		}
		public bool pbCanConfuseSelf(bool pkmn)
		{
			return false;
		}
		public bool pbCanConfuse(Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanSleep(Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanSleep(Pokemon pkmn, bool animate, PokeBattle_Move atk, bool uh)
		{
			return false;
		}
		public bool pbCanFreeze(Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanPoison(Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanParalyze(Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanBurn(Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanIncreaseStatStage(Stats stat, Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbCanReduceStatStage(Stats stat, Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbIncreaseStat(Stats stat, int num, Pokemon pkmn, bool animate, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbIncreaseStat(Stats stat, int num, Pokemon pkmn, bool uh, PokeBattle_Move atk, bool animate)
		{
			return false;
		}
		public bool pbReduceStat(Stats stat, int num, Pokemon pkmn, bool uh, PokeBattle_Move atk)
		{
			return false;
		}
		public bool pbReduceStat(Stats stat, int num, Pokemon pkmn, bool uh, PokeBattle_Move atk, bool animate)
		{
			return false;
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
		public Pokemon pbOpposing1 { get; set; }
		public Pokemon pbOpposing2 { get; set; }
		#endregion

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

		public static Pokemon[] GetBattlers(PokemonUnity.Monster.Pokemon[] input)
		{
			Pokemon[] battlers = new Pokemon[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				battlers[i] = (Pokemon)input[i];
			}
			return battlers;
		}
		public void BattlerUI(BattlePokemonHandler gameObject, bool player = false)
		{
			if (player) playerHUD = gameObject;
			else nonplayerHUD = gameObject;
		}
		/// <summary>
		/// Refreshes the HUD of this Pokemon
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
}