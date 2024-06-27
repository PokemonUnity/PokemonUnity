using System;
using System.Linq;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Combat.Data;
using PokemonUnity.Inventory;
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
	/// A Pokemon placeholder class to be used while in-battle,
	/// to prevent changes from being permanent to original pokemon profile
	/// </summary>
	/// <remarks>
	/// This class is called once during battle and persist until end.
	/// Values and variables are overwritten using <see cref="IBattler.InitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon, int)"/>
	/// </remarks>
	public partial class Pokemon : PokemonEssentials.Interface.PokeBattle.IBattler, IEquatable<Pokemon>, IEqualityComparer<Pokemon>, IEquatable<IBattler>, IEqualityComparer<IBattler>, ICloneable
	{
#pragma warning disable 0162 //Warning CS0162  Unreachable code detected
		#region Variables
		#region Battle Related
		public IBattle battle					{ get; protected set; }//{ return Game.battle; }
		/// <summary>
		/// Returns the position of this pkmn in battle lineup
		/// </summary>
		public int Index						{ get; protected set; }
		/// <summary>
		/// Index list of all pokemons who attacked this battler on this/previous turn
		/// </summary>
		public IList<int> lastAttacker			{ get; protected set; }
		public int turncount					{ get; set; } //ToDo: Private set?
		public IEffectsBattler effects			{ get; protected set; }
		/// <summary>
		/// Int Buffs and debuffs (gains and loss) affecting this pkmn.
		/// </summary>
		/// <remarks>
		/// 0: Attack, 1: Defense, 2: Speed, 3: SpAtk, 4: SpDef, 5: Evasion, 6: Accuracy
		/// </remarks>
		public int[] stages						{ get; protected set; }
		/// <summary>
		/// Participants will earn Exp. Points if this battler is defeated
		/// </summary>
		public IList<int> participants			{ get; set; }
		public bool tookDamage					{ get; set; }
		public int lastHPLost					{ get; set; }
		public Moves lastMoveUsed				{ get; set; }
		public Types lastMoveUsedType			{ get { return Kernal.MoveData[lastMoveUsed].Type; } }
		public Moves lastMoveUsedSketch			{ get; set; }
		public Moves lastRegularMoveUsed		{ get; set; }
		public int? lastRoundMoved				{ get; set; }
		public IList<Moves> movesUsed			{ get; set; }
		public Moves currentMove				{ get; set; }
		public IDamageState damagestate			{ get; set; }
		public bool captured					{ get; set; }
		#endregion
		#region Inherit Base Pokemon Data
		public int HP							{ get { return hp; } set { if (value > 0) { hp = value; if (status == Status.FAINT) { Status = Status.NONE; fainted = false; } } else Status = Status.FAINT;} }
		private int hp;
		public int TotalHP						{ get; protected set; }
		public int ATK							{ get { return effects.PowerTrick ? DEF : attack; } set { attack = value; } }
		protected int attack;
		public int DEF							{ get
			{
				if (effects.PowerTrick) return attack;
				return battle.field.WonderRoom > 0 ? spdef : defense;
			}
			set { defense = value; }
		}
		protected int defense;
		public int SPD							{ get { return battle.field.WonderRoom > 0 ? DEF : spdef; } set { spdef = value; } }
		protected int spdef;
		public int SPA							{ get { return spatk; } set { spatk = value; } }
		protected int spatk;
		protected int speed;
		public int SPE							{ get //ToDo: Revert to mirror pokemon essential source?
			{
				int[] stagemul = new int[] { 10, 10, 10, 10, 10, 10, 10, 15, 20, 25, 30, 35, 40 };
				int[] stagediv = new int[] { 40, 35, 30, 25, 20, 15, 10, 10, 10, 10, 10, 10, 10 };
				int spe = this.speed;
				int stage = stages[2] + 6;
				spe = (int)Math.Floor(spe * (decimal)stagemul[stage] / stagediv[stage]);
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
					Game.GameData.Trainer.badges.Count(b => b == true) >= Core.BADGESBOOSTSPEED)
					speedmult = (int)Math.Round(speedmult * 1.1f);
				spe = (int)Math.Round(spe * speedmult * 1f / 0x1000);
				return Math.Max(spe, 1);
			}
			set { speed = value; }
		}
		public int Level						{
													get { return level; } //pokemon.IsNotNullOrNone() ? pokemon.Level : 0; }
													set { level = value; } //if (pokemon.IsNotNullOrNone()) pokemon.SetLevel((byte)value); }
												}
		private int level; //ToDo: Do a null check for base.pokemon, and default to 0 if none?
		public Monster.Natures Nature			{ get { return pokemon.Nature; } }
		public int Happiness					{ get { return pokemon.Happiness; } } //set { happiness = value; } }
		//private int happiness					{ get; set; } //{ return pokemon.IsNotNullOrNone() ? pokemon.Happiness : 0; } }
		public bool? PokerusStage				{ get { return pokemon.PokerusStage; } }
		public string Name { get {
				//if name is not nickname return illusion.Name?
				if (effects.Illusion != null)
					return Game._INTL(effects.Illusion.Species.ToString(TextScripts.Name));
				return name; } set { name = value; } }
		private string name;					//{ get { return pokemon.Name; } }
		public bool? Gender { get {
				if (effects.Illusion != null)
					return effects.Illusion.Gender;
				return this.gender; } set { gender = value; } }
		protected bool? gender;
		public bool IsShiny { get {
				if (effects.Illusion != null)
					return effects.Illusion.IsShiny;
				if (pokemon.IsNotNullOrNone()) return pokemon.IsShiny;
				return false; }
		}
		public Pokemons Species					{ get { return pokemon == null ? Pokemons.NONE : (effects.Illusion != null ? effects.Illusion.Species : Kernal.PokemonFormsData[pokemon.Species][form].Base); } }
		public int StatusCount					{ get { return statusCount; } set { statusCount = value; } }
		private int statusCount;
		public Status Status
		{
			get
			{
				return status;
			}
			set
			{
				if (status == Status.SLEEP && value == 0)
					effects.Truant = false;
				status = value;
				if (value != Status.POISON)
					effects.Toxic = 0;
				if (value != Status.POISON && value != Status.SLEEP)
					statusCount = 0;
				if (value == Status.FAINT)
				{ hp = 0; fainted = true; }
			}
		}
		private Status status;
		public Items Item						{ get { return item; } set { item = value; } }
		private Items item;
		public Types Type1						{ get; set; }
		public Types Type2						{ get; set; }
		public int[] IV							{ get; protected set; }
		//public int[] IV						{ get { return pokemon.IV; } }
		public Abilities Ability				{ get { return ability; } set { ability = value; } }
		private Abilities ability;
		public IBattleMove[] moves				{ get; set; }
		#endregion
		#region Move to PokemonBattle Class
		/// <summary>
		/// Shadow Pokémon in the first game sometimes enter Hyper Mode, but in XD they enter Reverse Mode instead. (Battle Only).
		/// </summary>
		/// <remarks>
		/// In Hyper Mode, a Pokémon may attack its Trainer, but in Reverse Mode, they will not.
		/// While in Reverse Mode, a Pokémon hurts itself after every turn, whereas a Pokémon in Hyper Mode incurs no self-damage
		/// </remarks>
		protected bool isHyperMode { get; set; }
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
		protected bool fainted;	//ToDo: Remove because redundancy of `this.Status == Status.FAINT`?
		public bool isFainted() { return hp <= 0 || Status == Status.FAINT; } //|| fainted
		public bool isEgg { get { return pokemon?.isEgg??true; } }
		/// <summary>
		/// Returns the position of this pkmn in party lineup
		/// </summary>
		/// ToDo: Where this.pkmn.index == party[this.pkmn.index]
		public int pokemonIndex { get; set; }
		public bool IsOwned
		{
			get
			{
				return (pokemon.IsNotNullOrNone()) ? Game.GameData.Trainer.owned.ContainsKey(@pokemon.Species) && Game.GameData.Trainer.owned[@pokemon.Species] && !@battle.opponent.IsNotNullOrNone() : false;
				//return Game.GameData.Player.Pokedex[(byte)Species, 1] == 1;
			}
		}
		public PokemonEssentials.Interface.PokeBattle.IPokemon pokemon { get; protected set; }

		public float Weight(IBattler attacker = null)
		{
			float w = Kernal.PokemonData[Form.Pokemon].Weight;
			if (attacker.IsNotNullOrNone() && !attacker.hasMoldBreaker())
			//if (this.IsNotNullOrNone() || !hasMoldBreaker())
			{
				if (hasWorkingAbility(Abilities.HEAVY_METAL)) w *= 2;
				if (hasWorkingAbility(Abilities.LIGHT_METAL)) w /= 2;
			}
			if (hasWorkingItem(Items.FLOAT_STONE)) w /= 2;
			w += effects.WeightChange;
			w = (float)Math.Floor((decimal)w);
			if (w < 1) w = 1;
			return w;
		}

		public bool IsHyperMode { get {
				if (effects.Illusion != null)
					return false;
				return isHyperMode; }
		}
		public int form { get; set; } //ToDo: Rename to FormId and set `form` private get/set
		public Monster.Data.Form Form { get { return Kernal.PokemonFormsData[Species][form]; } }
		public bool hasMega { get {
			if (@effects.Transform) return false;
			if (@pokemon.IsNotNullOrNone())
			{
				//return !(@pokemon is IPokemonMegaEvolution m) ? false : m.hasMegaForm();// ? rescue false
				return @pokemon is IPokemonMegaEvolution m && m.hasMegaForm();
			}
			return false;
		} }

		public bool isMega { get {
			if (@pokemon.IsNotNullOrNone())
			{
				//return !(@pokemon is IPokemonMegaEvolution m) ? false : m.isMega();// ? rescue false
				return @pokemon is IPokemonMegaEvolution m && m.isMega();
			}
			return false;
		} }

		public bool hasPrimal { get {
			if (@effects.Transform) return false;
			if (@pokemon.IsNotNullOrNone())
			{
				//return !(@pokemon is IPokemonMegaEvolution m) ? false : m.hasPrimalForm();// ? rescue false
				return @pokemon is IPokemonMegaEvolution m && m.hasPrimalForm();
			}
			return false;
		} }

		public bool isPrimal { get {
			if (@pokemon.IsNotNullOrNone())
			{
				//return !(@pokemon is IPokemonMegaEvolution m) ? false : m.isPrimal();// ? rescue false
				return @pokemon is IPokemonMegaEvolution m && m.isPrimal();
			}
			return false;
		} }
		#endregion

		#region Constructors
		protected Pokemon() { }
		public Pokemon(IBattle btl, int idx) //: base()
		{
			(this as IBattler).initialize(btl, idx);
		}
		IBattler IBattler.initialize(IBattle btl, int idx) //because it's meant to be protected...
		{
			battle			= btl;
			Index			= idx;
			hp				= 0;
			TotalHP			= 0;
			fainted			= true;
			captured		= false;
			stages			= new int[7]; //int[Enum.GetValues(typeof(PokemonUnity.Combat.Stats)).Length];
			effects			= new Effects.Battler(false);
			damagestate		= new DamageState();
			InitBlank();
			InitEffects(false);
			InitPermanentEffects();
			return this;
		}
		/// <summary>
		/// Used when switching a pokemon in from standby (in pokeball) TO battle (in match)
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="index"></param>
		/// <param name="batonpass"></param>
		/// <returns></returns>
		public virtual IBattler Initialize(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, int index, bool batonpass = false) //: base(pkmn)
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
		public virtual void InitBlank()
		{
			@name			= "";
			//@Species		= 0;
			@level			= 0;
			@HP				= 0;
			@TotalHP		= 0;
			@gender			= null; //0;
			@ability		= 0;
			@Type1			= 0;
			@Type2			= 0;
			@form			= 0;
			@attack			= 0;
			@defense		= 0;
			@speed			= 0;
			@spatk			= 0;
			@spdef			= 0;
			@status			= 0;
			@statusCount	= 0;
			@pokemon		= null;
			pokemonIndex	= -1;
			participants	= new List<int>();
			@moves			= new IBattleMove[]{ null,null,null,null };
			@IV				= new int[]{ 0,0,0,0,0,0 };
			@item			= 0;
			//@weight			= null;
		}
		public virtual void InitEffects(bool batonpass)
		{
			if (!batonpass)
			{
				//These effects are retained if Baton Pass is used
				//stages[0]			= 0; // [ATTACK]
				//stages[1]			= 0; // [DEFENSE]
				//stages[2]			= 0; // [SPEED]
				//stages[3]			= 0; // [SPATK]
				//stages[4]			= 0; // [SPDEF]
				//stages[5]			= 0; // [EVASION]
				//stages[6]			= 0; // [ACCURACY]
				stages				= new int[7]; //int[Enum.GetValues(typeof(PokemonUnity.Combat.Stats)).Length];
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
					//if (battle.battlers[i].Species == Pokemons.NONE) continue;
					if (battle.battlers[i].IsNotNullOrNone() && //) continue;
						battle.battlers[i].effects.LockOnPos == Index &&
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
			fainted						= false;
			lastAttacker				= new List<int>();
			lastHPLost					= 0;
			tookDamage					= false;
			lastMoveUsed				= Moves.NONE;
			//lastMoveUsedType			= Types.NONE;
			lastRoundMoved				= -1;
			movesUsed					= new List<Moves>();
			turncount					= 0; //number of turns for battler, not the match battle
			effects.Attract				= -1;
			effects.BatonPass			= false;
			effects.Bide				= 0;
			effects.BideDamage			= 0;
			effects.BideTarget			= -1;
			effects.Charge				= 0;
			effects.ChoiceBand			= Moves.NONE;
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
				//if (battle.battlers[i].Species == Pokemons.NONE) continue;
				if (!battle.battlers[i].IsNotNullOrNone()) continue;
				if (battle.battlers[i].effects == null) continue; //ToDo: Initialize on all battlers before here?...
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
				int lastpoke = battle.GetLastPokeInTeam(Index);
				if (lastpoke != pokemonIndex){
					effects.Illusion = battle.Party(Index)[lastpoke];
				}
			}
		}
		public virtual void InitPermanentEffects()
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
		//protected void _InitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, sbyte pkmnIndex)
		void IBattler.InitPokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pkmn, int pkmnIndex)
		{
			if (pkmn.isEgg)
			{
				Core.Logger.LogWarning(Game._INTL("An egg can't be an active Pokémon"));
				//Core.Logger.LogWarning(LanguageExtension.Translate(Text.Errors, "ActiveEgg").Value);
			}
			else
			{
				name			= pkmn.Name;
				//Species		= pkmn.Species;
				level			= pkmn.Level;
				HP				= pkmn.HP;
				TotalHP			= pkmn.TotalHP;
				gender			= pkmn.Gender;
				ability			= pkmn.Ability;
				item			= pkmn.Item;
				Type1			= pkmn.Type1;
				Type2			= pkmn.Type2;
				form			= pkmn is IPokemonMultipleForms f ? f.form : 0;
				attack			= pkmn.ATK;
				defense			= pkmn.DEF;
				speed			= pkmn.SPE;
				spatk			= pkmn.SPA;
				spdef			= pkmn.SPD;
				status			= pkmn.Status;
				statusCount		= pkmn.StatusCount;
				pokemon			= pkmn;
				pokemonIndex	= pkmnIndex;
				participants	= new List<int>();
				moves			= new IBattleMove[] {
					PokemonUnity.Combat.Move.FromMove(@battle,pkmn.moves[0]),
					PokemonUnity.Combat.Move.FromMove(@battle,pkmn.moves[1]),
					PokemonUnity.Combat.Move.FromMove(@battle,pkmn.moves[2]),
					PokemonUnity.Combat.Move.FromMove(@battle,pkmn.moves[3])
				};
				IV				= new int[] { //pkmn.IV;
					pkmn.IV[0],
					pkmn.IV[1],
					pkmn.IV[2],
					pkmn.IV[3],
					pkmn.IV[4],
					pkmn.IV[5]
				};
			}
		}
		public void Update(bool fullchange = false)
		{
			if(Species != Pokemons.NONE)
			{
				pokemon.calcStats(); //Not needed since fetching stats from base ( Pokemon => Battler )
				level		= pokemon.Level;
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
		public IBattler Reset()
		{
			pokemon		= new PokemonUnity.Monster.Pokemon();
			Index		= -1;
			InitEffects(false);
			//reset status
			status		= Status.NONE; //ToDo: Status.FAINT?
			statusCount	= 0;
			fainted		= true;
			//captured	= false; //ToDo: null=default,false=snagged,true=captured?
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
				if (battle.IsOpposing(Index))
				{
					bool found1, found2; found1 = found2 = false;
					for (int i = 0; i < participants.Count; i++)
					{
						if (participants[i] == battle.battlers[2].Index) found1 = true;
						if (participants[i] == battle.battlers[3].Index) found2 = true;
					}
					if (!found1 && !battle.battlers[2].isFainted())
						participants.Add((byte)battle.battlers[2].Index);
					if (!found2 && !battle.battlers[3].isFainted())
						participants.Add((byte)battle.battlers[3].Index);
				}
			}
		}
		#endregion

		#region About this Battler
		public string ToString(bool lowercase = false)
		{
			if (battle.IsOpposing(Index))
			{
				if (battle.opponent.Length == 0)//.ID == TrainerTypes.WildPokemon
					//return string.Format("The wild {0}", Name);
					//return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "WildPokemonL" : "WildPokemon", Name).Value;
					return Game._INTL("The wild {0}", lowercase ? Game._INTL(Species.ToString(TextScripts.Name)).ToLowerInvariant() : Game._INTL(Species.ToString(TextScripts.Name)));
				else
					//return string.Format("The opposing {0}", Name);
					//return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "OpponentPokemonL" : "OpponentPokemon", Name).Value;
					return Game._INTL("The opposing {0}", lowercase ? Game._INTL(Species.ToString(TextScripts.Name)).ToLowerInvariant() : Game._INTL(Species.ToString(TextScripts.Name)));
			}
			else if (battle.OwnedByPlayer(Index))
				return Name;
			else
				//return string.Format("The ally {0}", Name);
				//return LanguageExtension.Translate(Text.ScriptTexts, lowercase ? "AllyPokemonL" : "AllyPokemon", Name).Value;
				return Game._INTL("The ally {0}", lowercase ? Game._INTL(Species.ToString(TextScripts.Name)).ToLowerInvariant() : Game._INTL(Species.ToString(TextScripts.Name)));
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
			return this.Ability == ability;
		}

		public bool HasType(Types type) {
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

		public bool hasMovedThisRound() {
			if (!lastRoundMoved.HasValue) return false;
			return lastRoundMoved.Value == battle.turncount;
		}
		//ToDo: Double check this
		public bool hasMovedThisRound(int? turncount){
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
			return this.Item == item;
		}

		public bool hasWorkingBerry(bool ignorefainted= false) {
			if (this.isFainted() && !ignorefainted) return false;
			if (effects.Embargo > 0) return false;
			if (battle.field.MagicRoom > 0) return false;
			if (this.hasWorkingAbility(Abilities.KLUTZ, ignorefainted)) return false;
			//return Kernal.ItemData[this.Item].IsBerry;//.Pocket == ItemPockets.BERRY;//IsBerry?(@item)
			return ItemData.IsBerry(this.Item);
		}

		public bool isAirborne(bool ignoreability=false){
			if (this.hasWorkingItem(Items.IRON_BALL)) return false;
			if (effects.Ingrain) return false;
			if (effects.SmackDown) return false;
			if (battle.field.Gravity > 0) return false;
			if (this.HasType(Types.FLYING) && !effects.Roost) return true;
			if (this.hasWorkingAbility(Abilities.LEVITATE) && !ignoreability) return true;
			if (this.hasWorkingItem(Items.AIR_BALLOON)) return true;
			if (effects.MagnetRise > 0) return true;
			if (effects.Telekinesis > 0) return true;
			return false;
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
				//battle.Display(Game._INTL("HP less than 0"));
				//battle.Display(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
				Core.Logger.LogWarning("HP less than 0");
			if (HP > TotalHP)
				//battle.Display(Game._INTL("HP greater than total HP"));
				//battle.Display(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
				Core.Logger.LogWarning("HP greater than total HP");
			if (amt > 0)
				//battle.scene.HPChanged(Index, oldhp, animate);
				if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.HPChanged(this, oldhp, animate);
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
				//battle.Display(Game._INTL("HP less than 0"));
				//battle.Display(LanguageExtension.Translate(Text.Errors, "HpLessThanZero").Value);
				Core.Logger.LogWarning("HP less than 0");
			if (HP > TotalHP)
				//battle.Display(Game._INTL("HP greater than total HP"));
				//battle.Display(LanguageExtension.Translate(Text.Errors, "HpGreaterThanTotal").Value);
				Core.Logger.LogWarning("HP greater than total HP");
			if(amount > 0)
				//battle.scene.HPChanged(Index, oldhp, animate);
				if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.HPChanged(this, oldhp, animate);
			//ToDo: Fix return
			return amount;
		}

		public bool Faint(bool showMessage = true)
		{
			if(!isFainted() && HP > 0)
			{
				Core.Logger.LogWarning("Can't faint with HP greater than 0");
				return true;
			}
			if(fainted)
			{
				Core.Logger.LogWarning("Can't faint if already fainted");
				return true;
			}
			if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.Fainted(this);
			InitEffects(false);
			// Reset status
			//status = 0;
			statusCount = 0;
			if (pokemon != null && battle.internalbattle)
				(pokemon as Monster.Pokemon).ChangeHappiness(HappinessMethods.FAINT);
			if (isMega) {
				//Change form to before transformation
				if (pokemon is IPokemonMegaEvolution m) m.makeUnmega();
				form = pokemon is IPokemonMultipleForms f ? f.form : 0;
			}
			if (isPrimal) {
				//Change form to before transformation
				if (pokemon is IPokemonMegaEvolution m) m.makeUnprimal();
				form = pokemon is IPokemonMultipleForms f ? f.form : 0;
			}
			hp = 0;
			fainted = true;
			status = Status.FAINT;
			//reset choice
			battle.choices[Index] = new Choice(ChoiceAction.NoAction);
			OwnSide.LastRoundFainted = battle.turncount;
			if (showMessage)
				battle.Display(Game._INTL("{1} fainted!", ToString()));
			return true;
		}
		#endregion

		#region Find other battlers/sides in relation to this battler
		/// <summary>
		/// Returns the data structure for this battler's side
		/// </summary>
		/// Player: 0 and 2; Foe: 1 and 3
		public IEffectsSide OwnSide { get { return battle.sides[Index&1]; } }
		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// Player: 1 and 3; Foe: 0 and 2
		public IEffectsSide OpposingSide { get { return battle.sides[(Index&1)^1]; } }
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
		public IBattler Partner { get { return battle.battlers[(Index & 1) | ((Index & 2) ^ 2)]; } }
		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		public IBattler OppositeOpposing { get { return battle.battlers[(Index ^ 1)]; } }
		//ToDo: If not double battle return null?
		public IBattler OppositeOpposing2 { get { return battle.battlers[(Index ^ 1) | ((Index & 2) ^ 2)]; } }
		public IBattler Opposing1 { get { return battle.battlers[((Index & 1) ^ 1)]; } }
		//ToDo: If not double battle return null?
		public IBattler Opposing2 { get { return battle.battlers[((Index & 1) ^ 1) + 2]; } }
		/// <summary>
		/// Returns the battler's first opposing Pokémon Index
		/// </summary>
		public int OpposingIndex { get { return (Index ^ 1) | ((Index & 2) ^ 2); } }
		public int NonActivePokemonCount
		{
			get
			{
				int count = 0;
				PokemonEssentials.Interface.PokeBattle.IPokemon[] party = battle.Party(Index);
				for (int i = 0; i < party.Length; i++)
				{
					if (battle.doublebattle)
						if ((isFainted() || i != pokemonIndex) &&
							(Partner.isFainted() || i != Partner.pokemonIndex) &&
							party[i].IsNotNullOrNone() && !party[i].isEgg && party[i].HP > 0)
								count += 1;
					else
						if ((isFainted() || i != pokemonIndex) &&
							party[i].IsNotNullOrNone() && !party[i].isEgg && party[i].HP > 0)
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
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.GIRATINA)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.ARCEUS && Ability == Abilities.MULTITYPE)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
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
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (Species == Pokemons.GENESECT)
			{
				if (form != (pokemon is IPokemonMultipleForms f ? f.form : 0))
				{
					form = pokemon is IPokemonMultipleForms f1 ? f1.form : 0;
					transformed = true;
				}
			}
			if (transformed)
			{
				Update(true);
				if (@battle.scene is IPokeBattle_Scene s0)
					//s0.ChangePokemon();
					s0.ChangePokemon(this, pokemon);
				battle.Display(Game._INTL("{1} transformed!", ToString()));
				//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "Transformed", ToString()).Value);
				Core.Logger.Log(string.Format("[Form changed] {0} changed to form {1}", ToString(), Game._INTL(Form.Id.ToString(TextScripts.Name))));
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
					battle.weather = (Weather.HEAVYRAIN);
					battle.weatherduration = -1;
					battle.CommonAnimation("HeavyRain", null, null);
					battle.Display(Game._INTL("{1}'s {2} made a heavy rain begin to fall!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "HeavyRainStart", ToString(), Ability.ToString().Translate().Value).Value);
					Core.Logger.Log(string.Format("[Ability triggered] {0}'s Primordial Sea made it rain heavily", ToString()));
				}
				if(hasWorkingAbility(Abilities.DESOLATE_LAND) && battle.Weather != Weather.HARSHSUN)
				{
					battle.weather = (Weather.HARSHSUN);
					battle.weatherduration = -1;
					battle.CommonAnimation("HarshSun", null, null);
					battle.Display(Game._INTL("{1}'s {2} turned the sunlight extremely harsh!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "HarshSunStart", ToString(), Ability.ToString().Translate().Value).Value);
					Core.Logger.Log(string.Format("[Ability triggered] {0}'s Desolate Land made the sun shine harshly", ToString()));
				}
				if(hasWorkingAbility(Abilities.DELTA_STREAM) && battle.Weather != Weather.STRONGWINDS)
				{
					battle.weather = (Weather.STRONGWINDS);
					battle.weatherduration = -1;
					battle.CommonAnimation("StrongWinds", null, null);
					battle.Display(Game._INTL("{1}'s {2} caused a mysterious air current that protects Flying-type Pokémon!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "StrongWindsStart", ToString(), Ability.ToString().Translate().Value).Value);
					Core.Logger.Log(string.Format("[Ability triggered] {0}'s Delta Stream made an air current blow", ToString()));
				}
				if (battle.Weather != Weather.HEAVYRAIN &&
					battle.Weather != Weather.HARSHSUN &&
					battle.Weather != Weather.STRONGWINDS)
				{
					if (hasWorkingAbility(Abilities.DRIZZLE) &&
						(battle.Weather != Weather.RAINDANCE || battle.weatherduration != -1))
					{
						battle.weather = (Weather.RAINDANCE);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.DAMP_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.CommonAnimation("Rain", null, null);
						battle.Display(Game._INTL("{1}'s {2} made it rain!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "RainStart", ToString(), Ability.ToString().Translate().Value).Value);
						Core.Logger.Log(string.Format("[Ability triggered] {0}'s Drizzle made it rain", ToString()));
					}
					if (hasWorkingAbility(Abilities.DROUGHT) &&
						(battle.Weather != Weather.SUNNYDAY || battle.weatherduration != -1))
					{
						battle.weather = (Weather.SUNNYDAY);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.HEAT_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.CommonAnimation("Sunny", null, null);
						//Output Below:
						battle.Display(Game._INTL("{1}'s {2} intensified the sun's rays!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "SunnyStart", ToString(), Ability.ToString().Translate().Value).Value);
						Core.Logger.Log(string.Format("[Ability triggered] {0}'s Drought made it sunny", ToString()));
					}
					if (hasWorkingAbility(Abilities.SAND_STREAM) &&
						(battle.Weather != Weather.SANDSTORM || battle.weatherduration != -1))
					{
						battle.weather = (Weather.SANDSTORM);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.SMOOTH_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.CommonAnimation("Sandstorm", null, null);
						battle.Display(Game._INTL("{1}'s {2} whipped up a sandstorm!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "SandstormStart", ToString(), Ability.ToString().Translate().Value).Value);
						Core.Logger.Log(string.Format("[Ability triggered] {0}'s Sand Stream made it sandstorm", ToString()));
					}
					if (hasWorkingAbility(Abilities.SNOW_WARNING) &&
						(battle.Weather != Weather.HAIL || battle.weatherduration != -1))
					{
						battle.weather = (Weather.HAIL);
						if (Core.USENEWBATTLEMECHANICS)
						{
							battle.weatherduration = 5;
							if (hasWorkingItem(Items.ICY_ROCK))
								battle.weatherduration = 8;
						}
						else
							battle.weatherduration = -1;
						battle.CommonAnimation("Hail", null, null);
						battle.Display(Game._INTL("{1}'s {2} made it hail!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
						//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "HailStart", ToString(), Ability.ToString().Translate().Value).Value);
						Core.Logger.Log(string.Format("[Ability triggered] {0}'s Snow Warning made it hail", ToString()));
					}
				}
				if(hasWorkingAbility(Abilities.AIR_LOCK) || hasWorkingAbility(Abilities.CLOUD_NINE))
				{
					battle.weather = (Weather.NONE);
					battle.weatherduration = 0;
					battle.Display(Game._INTL("{1} has {2}!", ToString(), Game._INTL(Ability.ToString(TextScripts.Name))));
					//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "HasAbility", ToString(), Ability.ToString().Translate().Value).Value);
					battle.Display(Game._INTL("The effects of the weather disappeared."));
					//battle.Display(LanguageExtension.Translate(Text.ScriptTexts, "WeatherNullified").Value);
					Core.Logger.Log(string.Format("[Ability nullified] {0}'s Ability cancelled weather effects", ToString()));
				}
			}
			//battle.PrimordialWeather();
			#endregion Weather
			#region Trace
			if (hasWorkingAbility(Abilities.TRACE))
			{
				//IBattleChoice[] choices = new IBattleChoice[4];
				List<int> choices = new List<int>();
				for (int i = 0; i < battle.battlers.Length; i++)
				{
					IBattler foe = battle.battlers[i];
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
					int choice = choices[@battle.Random(choices.Count)];
					string battlername = @battle.battlers[choice].ToString(true);
					Abilities battlerability = @battle.battlers[choice].Ability;
					@ability = battlerability;
					string abilityname = battlerability.ToString();
					@battle.Display(Game._INTL("{1} traced {2}'s {3}!", ToString(), battlername, abilityname));
					Core.Logger.Log($"[Ability triggered] #{ToString()}'s Trace turned into #{abilityname} from #{battlername}");
				}
			}
			#endregion Trace
			#region Intimidate
			if (this.hasWorkingAbility(Abilities.INTIMIDATE) && onactive) {
				Core.Logger.Log($"[Ability triggered] #{ToString()}'s Intimidate");
				for (int i = 0; i < 4; i++)
				if (IsOpposing(i) && !@battle.battlers[i].isFainted() && @battle.battlers[i] is IBattlerEffect b)
					b.ReduceAttackStatIntimidate(this);
			}
			#endregion Intimidate
			#region Download
			if (this.hasWorkingAbility(Abilities.DOWNLOAD) && onactive) {
				int odef = 0; int ospdef = 0;
				if (Opposing1.IsNotNullOrNone() && !Opposing1.isFainted()) {
					odef+=Opposing1.DEF;
					ospdef+=Opposing1.SPD;
				}
				if (Opposing2.IsNotNullOrNone() && !Opposing2.isFainted()) {
					odef+=Opposing2.DEF;
					ospdef+=Opposing1.SPD;
				}
				if (ospdef>odef)
					if (IncreaseStatWithCause(Stats.ATTACK,1,this,Game._INTL(ability.ToString(TextScripts.Name))))
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s Download (raising Attack)");
					else
						if (IncreaseStatWithCause(Stats.SPATK,1,this,Game._INTL(ability.ToString(TextScripts.Name))))
							Core.Logger.Log($"[Ability triggered] #{ToString()}'s Download (raising Special Attack)");
			}
			#endregion Download
			#region Frisk
			if (this.hasWorkingAbility(Abilities.FRISK) && @battle.OwnedByPlayer(@Index) && onactive) {
				List<IBattler> foes= new List<IBattler>();
				if (Opposing1.Item>0 && !Opposing1.isFainted()) foes.Add(Opposing1);
				if (Opposing2.Item>0 && !Opposing2.isFainted()) foes.Add(Opposing2);
				if (Core.USENEWBATTLEMECHANICS) {
				if (foes.Count>0) Core.Logger.Log($"[Ability triggered] #{ToString()}'s Frisk");
					foreach (var i in foes) {
						string itemname=Game._INTL(i.Item.ToString(TextScripts.Name));
						@battle.Display(Game._INTL("{1} frisked {2} and found its {3}!",ToString(),i.ToString(true),itemname));
					}
				} else if (foes.Count>0) {
					Core.Logger.Log($"[Ability triggered] #{ToString()}'s Frisk");
					IBattler foe=foes[@battle.Random(foes.Count)];
					string itemname=Game._INTL(foe.Item.ToString(TextScripts.Name));
					@battle.Display(Game._INTL("{1} frisked the foe and found one {2}!",ToString(),itemname));
				}
			}
			#endregion Frisk
			#region Anticipation
			if (this.hasWorkingAbility(Abilities.ANTICIPATION) && @battle.OwnedByPlayer(@Index) && onactive) {
				Core.Logger.Log($"[Ability triggered] #{ToString()} has Anticipation");
				bool found=false;
				foreach (var foe in new IBattler[] { Opposing1, Opposing2 }) {
					if (foe.isFainted()) continue;
					foreach (var j in foe.moves) {
						Attack.Data.MoveData movedata=Kernal.MoveData[j.id];
						TypeEffective eff=movedata.Type.GetCombinedEffectiveness(Type1,Type2,@effects.Type3);
						if ((movedata.Power>0 && eff == TypeEffective.SuperEffective) ||
							(movedata.Effect== Attack.Effects.x027 && eff != TypeEffective.Ineffective)) { // OHKO
							found=true;
							break;
						}
					}
					if (found) break;
				}
				if (found) @battle.Display(Game._INTL("{1} shuddered with anticipation!",ToString()));
			}
			#endregion Anticipation
			#region Forewarn
			if (this.hasWorkingAbility(Abilities.FOREWARN) && @battle.OwnedByPlayer(@Index) && onactive) {
				Core.Logger.Log($"[Ability triggered] #{ToString()} has Forewarn");
				int highpower=0;
				List<Moves> fwmoves= new List<Moves>();
				foreach (var foe in new IBattler[] { Opposing1, Opposing2 }) {
					if (foe.isFainted()) continue;
					foreach (var j in foe.moves) {
						Attack.Data.MoveData movedata=Kernal.MoveData[j.id];
						int power=movedata.Power??0;
						if (movedata.Effect == Attack.Effects.x027) power=160;    // OHKO
						if (movedata.Effect == Attack.Effects.x0BF) power=150;    // Eruption
						if (movedata.Effect == Attack.Effects.x05A || // Counter
									movedata.Effect == Attack.Effects.x091 || // Mirror Coat
									movedata.Effect == Attack.Effects.x0E4) power=120;// || // Metal Burst
						if (movedata.Effect == Attack.Effects.x083 ||  // SonicBoom
									movedata.Effect == Attack.Effects.x02A ||  // Dragon Rage
									movedata.Effect == Attack.Effects.x058 ||  // Night Shade
									movedata.Effect == Attack.Effects.x0BE ||  // Endeavor
									movedata.Effect == Attack.Effects.x059 ||  // Psywave
									movedata.Effect == Attack.Effects.x07A ||  // Return
									movedata.Effect == Attack.Effects.x07C ||  // Frustration
									movedata.Effect == Attack.Effects.x0EE ||  // Crush Grip
									movedata.Effect == Attack.Effects.x0DC ||  // Gyro Ball
									movedata.Effect == Attack.Effects.x088 ||  // Hidden Power
									movedata.Effect == Attack.Effects.x0DF ||  // Natural Gift
									movedata.Effect == Attack.Effects.x0EC ||  // Trump Card
									movedata.Effect == Attack.Effects.x064 ||  // Flail
									movedata.Effect == Attack.Effects.x0C5) power=80;     // Grass Knot
						if (power > highpower) {
							fwmoves=new List<Moves>() { j.id }; highpower=power;
						} else if (power==highpower)
							fwmoves.Add(j.id);
					}
				}
				if (fwmoves.Count>0) {
					Moves fwmove=fwmoves[@battle.Random(fwmoves.Count)];
					string movename=Game._INTL(fwmove.ToString(TextScripts.Name));
					@battle.Display(Game._INTL("{1}'s Forewarn alerted it to {2}!",ToString(),movename));
				}
			}
			#endregion Forewarn
			// Pressure message
			if (this.hasWorkingAbility(Abilities.PRESSURE) && onactive)
				@battle.Display(Game._INTL("{1} is exerting its pressure!",ToString()));
			// Mold Breaker message
			if (this.hasWorkingAbility(Abilities.MOLD_BREAKER) && onactive)
				@battle.Display(Game._INTL("{1} breaks the mold!",ToString()));
			// Turboblaze message
			if (this.hasWorkingAbility(Abilities.TURBOBLAZE) && onactive)
				@battle.Display(Game._INTL("{1} is radiating a blazing aura!",ToString()));
			// Teravolt message
			if (this.hasWorkingAbility(Abilities.TERAVOLT) && onactive)
				@battle.Display(Game._INTL("{1} is radiating a bursting aura!",ToString()));
			// Dark Aura message
			if (this.hasWorkingAbility(Abilities.DARK_AURA) && onactive)
				@battle.Display(Game._INTL("{1} is radiating a dark aura!",ToString()));
			// Fairy Aura message
			if (this.hasWorkingAbility(Abilities.FAIRY_AURA) && onactive)
				@battle.Display(Game._INTL("{1} is radiating a fairy aura!",ToString()));
			// Aura Break message
			if (this.hasWorkingAbility(Abilities.AURA_BREAK) && onactive)
				@battle.Display(Game._INTL("{1} reversed all other Pokémon's auras!",ToString()));
			// Imposter
			if (this.hasWorkingAbility(Abilities.IMPOSTER) && !@effects.Transform && onactive) {
				IBattler choice=OppositeOpposing;
				List<Attack.Effects> blacklist=new List<Attack.Effects>() {
					Attack.Effects.x09C,	// Fly
					Attack.Effects.x101,	// Dig
					Attack.Effects.x100,	// Dive
					Attack.Effects.x108,	// Bounce
					Attack.Effects.x138,	// Sky Drop
					//Attack.Effects.x111,	// Shadow Force
					Attack.Effects.x111	// Phantom Force
				};
				if (choice.effects.Transform ||
					choice.effects.Illusion.IsNotNullOrNone() ||
					choice.effects.Substitute>0 ||
					choice.effects.SkyDrop ||
					blacklist.Contains(Kernal.MoveData[choice.effects.TwoTurnAttack].Effect))
					Core.Logger.Log($"[Ability triggered] #{ToString()}'s Imposter couldn't transform");
				else {
					Core.Logger.Log($"[Ability triggered] #{ToString()}'s Imposter");
					@battle.Animation(Moves.TRANSFORM,this,choice);
					@effects.Transform=true;
					@Type1=choice.Type1;
					@Type2=choice.Type2;
					@effects.Type3=Types.NONE;
					@ability=choice.Ability;
					@attack=choice.ATK;
					@defense=choice.DEF;
					@speed=choice.SPE;
					@spatk=choice.SPA;
					@spdef=choice.SPD;
					foreach (var i in new Stats[] { Stats.ATTACK,Stats.DEFENSE,Stats.SPEED,
								Stats.SPATK,Stats.SPDEF,Stats.ACCURACY,Stats.EVASION })
						@stages[(int)i]=choice.stages[(int)i];
					for (int i = 0; i < 4; i++) {
						@moves[i]=Move.FromMove(@battle,new Attack.Move(choice.moves[i].id));
						@moves[i].PP=5;
						//@moves[i].TotalPP=5;
					}
					@effects.Disable=0;
					@effects.DisableMove=0;
					@battle.Display(Game._INTL("{1} transformed into {2}!",ToString(),choice.ToString(true)));
					Core.Logger.Log($"[Pokémon transformed] #{ToString()} transformed into #{choice.ToString(true)}");
				}
			}
			// Air Balloon message
			if (this.hasWorkingItem(Items.AIR_BALLOON) && onactive)
				@battle.Display(Game._INTL("{1} floats in the air with its {2}!",ToString(),Game._INTL(this.Item.ToString(TextScripts.Name))));
		}
		public void EffectsOnDealingDamage(IBattleMove move, IBattler user,IBattler target,int damage) {
			Types movetype=move.GetType(move.Type,user,target);
			if (damage>0 && move.Flags.Contact)
				if (!target.damagestate.Substitute) {
					if (target.hasWorkingItem(Items.STICKY_BARB,true) && user.Item==0 && !user.isFainted()) {
						user.Item=target.Item;
						target.Item=0;
						target.effects.Unburden=true;
						if (@battle.opponent.Length == 0 && !@battle.IsOpposing(user.Index))
						if (user.pokemon.itemInitial==Items.NONE && target.pokemon.itemInitial==user.Item) {
							user.pokemon.itemInitial=user.Item;
							target.pokemon.itemInitial=Items.NONE;
						}
						@battle.Display(Game._INTL("{1}'s {2} was transferred to {3}!",
							target.ToString(),Game._INTL(user.Item.ToString(TextScripts.Name)),user.ToString(true)));
						Core.Logger.Log($"[Item triggered] #{target.ToString()}'s Sticky Barb moved to #{user.ToString(true)}");
					}
					if (target.hasWorkingItem(Items.ROCKY_HELMET,true) && !user.isFainted())
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s Rocky Helmet");
							if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.DamageAnimation(user,0);
							user.ReduceHP((int)Math.Floor(user.TotalHP/6d));
							@battle.Display(Game._INTL("{1} was hurt by the {2}!",user.ToString(),
								Game._INTL(target.Item.ToString(TextScripts.Name))));
						}
					if (target.hasWorkingAbility(Abilities.AFTERMATH,true) && target.isFainted() &&
						!user.isFainted())
						if (!@battle.CheckGlobalAbility(Abilities.DAMP).IsNotNullOrNone() &&
							!user.hasMoldBreaker() && !user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Aftermath");
							if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.DamageAnimation(user,0);
							user.ReduceHP((int)Math.Floor(user.TotalHP/4d));
							@battle.Display(Game._INTL("{1} was caught in the aftermath!",user.ToString()));
						}
					if (target.hasWorkingAbility(Abilities.CUTE_CHARM) && @battle.Random(10)<3)
						if (!user.isFainted() && user is IBattlerEffect u && u.CanAttract(target,false)) {
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Cute Charm");
							u.Attract(target,Game._INTL("{1}'s {2} made {3} fall in love!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
						}
					if (target.hasWorkingAbility(Abilities.EFFECT_SPORE,true) && @battle.Random(10)<3)
						if (Core.USENEWBATTLEMECHANICS &&
							(user.HasType(Types.GRASS) ||
							user.hasWorkingAbility(Abilities.OVERCOAT) ||
							user.hasWorkingItem(Items.SAFETY_GOGGLES))) { //Not sure what goes here
						} else {
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Effect Spore");
							switch (@battle.Random(3)) {
								case 0:
									if (user is IBattlerEffect b0 && b0.CanPoison(null,false))
										b0.Poison(target,Game._INTL("{1}'s {2} poisoned {3}!",target.ToString(),
											Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
									break;
								case 1:
									if (user is IBattlerClause b1 && b1.CanSleep(null,false))
										if (b1 is IBattlerEffect b1a) b1a.Sleep(Game._INTL("{1}'s {2} made {3} fall asleep!",target.ToString(),
											Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
									break;
								case 2:
									if (user is IBattlerEffect b2 && b2.CanParalyze(null,false))
										b2.Paralyze(target,Game._INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
											target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
									break;
							}
						}
					if (target.hasWorkingAbility(Abilities.FLAME_BODY,true) && @battle.Random(10)<3 &&
						user is IBattlerEffect u0 && u0.CanBurn(null,false)) {
						Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Flame Body");
						u0.Burn(target,Game._INTL("{1}'s {2} burned {3}!",target.ToString(),
							Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
					}
					if (target.hasWorkingAbility(Abilities.MUMMY,true) && !user.isFainted())
						if (user.Ability != Abilities.MULTITYPE &&
							user.Ability != Abilities.STANCE_CHANGE &&
							user.Ability != Abilities.MUMMY) {
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Mummy copied onto #{user.ToString(true)}");
							user.Ability=Abilities.MUMMY;// || 0;
							@battle.Display(Game._INTL("{1} was mummified by {2}!",
								user.ToString(),target.ToString(true)));
						}
					if (target.hasWorkingAbility(Abilities.POISON_POINT,true) && @battle.Random(10)<3 &&
						user is IBattlerEffect u1 && u1.CanPoison(null,false)) {
						Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Poison Point");
						u1.Poison(target,Game._INTL("{1}'s {2} poisoned {3}!",target.ToString(),
							Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
					}
					if ((target.hasWorkingAbility(Abilities.ROUGH_SKIN,true) ||
						target.hasWorkingAbility(Abilities.IRON_BARBS,true)) && !user.isFainted())
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s #{target.Ability.ToString()}");
							if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.DamageAnimation(user,0);
							user.ReduceHP((int)Math.Floor(user.TotalHP/8d));
							@battle.Display(Game._INTL("{1}'s {2} hurt {3}!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
						}
					if (target.hasWorkingAbility(Abilities.STATIC,true) && @battle.Random(10)<3 &&
						user is IBattlerEffect u2 && u2.CanParalyze(null,false)) {
						Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Static");
						u2.Paralyze(target,Game._INTL("{1}'s {2} paralyzed {3}! It may be unable to move!",
							target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
					}
					if (target.hasWorkingAbility(Abilities.GOOEY,true))
						if (user is IBattlerEffect u3 && u3.ReduceStatWithCause(Stats.SPEED,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name))))
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Gooey");
					if (user.hasWorkingAbility(Abilities.POISON_TOUCH,true) &&
						target is IBattlerEffect t && t.CanPoison(null,false) && @battle.Random(10)<3) {
						Core.Logger.Log($"[Ability triggered] #{user.ToString()}'s Poison Touch");
						t.Poison(user,Game._INTL("{1}'s {2} poisoned {3}!",user.ToString(),
							Game._INTL(user.Ability.ToString(TextScripts.Name)),target.ToString(true)));
					}
				}
			if (damage>0) {
				if (!target.damagestate.Substitute) {
					if (target.hasWorkingAbility(Abilities.CURSED_BODY,true) && @battle.Random(10)<3)
						if (user.effects.Disable<=0 && move.PP>0 && !user.isFainted()) {
							user.effects.Disable=3;
							user.effects.DisableMove=move.id;
							@battle.Display(Game._INTL("{1}'s {2} disabled {3}!",target.ToString(),
								Game._INTL(target.Ability.ToString(TextScripts.Name)),user.ToString(true)));
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Cursed Body disabled #{user.ToString(true)}");
						}
					if (target.hasWorkingAbility(Abilities.JUSTIFIED) && movetype == Types.DARK)
						if (target is IBattlerEffect t && t.IncreaseStatWithCause(Stats.ATTACK,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name))))
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Justified");
					if (target.hasWorkingAbility(Abilities.RATTLED) &&
						(movetype == Types.BUG ||
						movetype == Types.DARK ||
						movetype == Types.GHOST))
						if (target is IBattlerEffect t && t.IncreaseStatWithCause(Stats.SPEED,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name))))
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Rattled");
					if (target.hasWorkingAbility(Abilities.WEAK_ARMOR) && move.IsPhysical(movetype)) {
						if (target is IBattlerEffect t0 && t0.ReduceStatWithCause(Stats.DEFENSE,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name))))
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Weak Armor (lower Defense)");
						if (target is IBattlerEffect t1 && t1.IncreaseStatWithCause(Stats.SPEED,1,target,Game._INTL(target.Ability.ToString(TextScripts.Name))))
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Weak Armor (raise Speed)");
					}
					if (target.hasWorkingItem(Items.AIR_BALLOON,true)) {
						Core.Logger.Log($"[Item triggered] #{target.ToString()}'s Air Balloon popped");
						@battle.Display(Game._INTL("{1}'s Air Balloon popped!",target.ToString()));
						target.ConsumeItem(true,false);
					} else if (target.hasWorkingItem(Items.ABSORB_BULB) && movetype == Types.WATER)
						if (target is IBattlerEffect t0 && t0.IncreaseStatWithCause(Stats.SPATK,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)))) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString()}");
							target.ConsumeItem();
						}
					else if (target.hasWorkingItem(Items.LUMINOUS_MOSS) && movetype == Types.WATER)
						if (target is IBattlerEffect t1 && t1.IncreaseStatWithCause(Stats.SPDEF,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)))) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString()}");
							target.ConsumeItem();
						}
					else if (target.hasWorkingItem(Items.CELL_BATTERY) && movetype == Types.ELECTRIC)
						if (target is IBattlerEffect t2 && t2.IncreaseStatWithCause(Stats.ATTACK,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)))) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString()}");
							target.ConsumeItem();
						}
					else if (target.hasWorkingItem(Items.SNOWBALL) && movetype == Types.ICE)
						if (target is IBattlerEffect t3 && t3.IncreaseStatWithCause(Stats.ATTACK,1,target,Game._INTL(target.Item.ToString(TextScripts.Name)))) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString()}");
							target.ConsumeItem();
						}
					else if (target.hasWorkingItem(Items.WEAKNESS_POLICY) && target.damagestate.TypeMod>8) {
						bool showanim=true;
						if (target is IBattlerEffect t4 && t4.IncreaseStatWithCause(Stats.ATTACK,2,target,Game._INTL(target.Item.ToString(TextScripts.Name)),showanim)) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s Weakness Policy (Attack)");
							showanim=false;
						}
						if (target is IBattlerEffect t5 && t5.IncreaseStatWithCause(Stats.SPATK,2,target,Game._INTL(target.Item.ToString(TextScripts.Name)),showanim)) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s Weakness Policy (Special Attack)");
							showanim=false;
						}
						if (!showanim) target.ConsumeItem();
					} else if (target.hasWorkingItem(Items.ENIGMA_BERRY) && target.damagestate.TypeMod>8)
						target.ActivateBerryEffect();
					else if ((target.hasWorkingItem(Items.JABOCA_BERRY) && move.IsPhysical(movetype)) ||
							(target.hasWorkingItem(Items.ROWAP_BERRY) && move.IsSpecial(movetype)))
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD) && !user.isFainted()) {
							Core.Logger.Log($"[Item triggered] #{target.ToString()}'s #{target.Item.ToString()}");
							if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.DamageAnimation(user,0);
							user.ReduceHP((int)Math.Floor(user.TotalHP/8d));
							@battle.Display(Game._INTL("{1} consumed its {2} and hurt {3}!",target.ToString(),
								Game._INTL(target.Item.ToString(TextScripts.Name)),user.ToString(true)));
							target.ConsumeItem();
						}
					else if (target.hasWorkingItem(Items.KEE_BERRY) && move.IsPhysical(movetype))
						target.ActivateBerryEffect();
					else if (target.hasWorkingItem(Items.MARANGA_BERRY) && move.IsSpecial(movetype))
						target.ActivateBerryEffect();
				}
				if (target.hasWorkingAbility(Abilities.ANGER_POINT))
					if (target.damagestate.Critical && !target.damagestate.Substitute &&
						target is IBattlerEffect t && t.CanIncreaseStatStage(Stats.ATTACK,target)) {
						Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Anger Point");
						target.stages[(int)Stats.ATTACK]=6;
						@battle.CommonAnimation("StatUp",target,null);
						@battle.Display(Game._INTL("{1}'s {2} maxed its {3}!",
							target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name)),Game._INTL(Stats.ATTACK.ToString(TextScripts.Name))));
					}
			}
			user.AbilityCureCheck();
			target.AbilityCureCheck();
		}
		public void EffectsAfterHit(IBattler user,IBattler target,IBattleMove thismove,IEffectsMove turneffects) {
			if (turneffects.TotalDamage==0) return;
			if (!(user.hasWorkingAbility(Abilities.SHEER_FORCE) && thismove.AddlEffect>0)) {
				// Target's held items:
				// Red Card
				if (target.hasWorkingItem(Items.RED_CARD) && @battle.CanSwitch(user.Index,-1,false)) {
					user.effects.Roar=true;
					@battle.Display(Game._INTL("{1} held up its {2} against the {3}!",
						target.ToString(),Game._INTL(target.Item.ToString(TextScripts.Name)),user.ToString(true)));
					target.ConsumeItem();
					// Eject Button
				} else if (target.hasWorkingItem(Items.EJECT_BUTTON) && @battle.CanChooseNonActive(target.Index)) {
					target.effects.Uturn=true;
					@battle.Display(Game._INTL("{1} is switched out with the {2}!",
						target.ToString(),Game._INTL(target.Item.ToString(TextScripts.Name))));
					target.ConsumeItem();
				}
				// User's held items:
				// Shell Bell
				if (user.hasWorkingItem(Items.SHELL_BELL) && user.effects.HealBlock==0) {
					Core.Logger.Log($"[Item triggered] #{user.ToString()}'s Shell Bell (total damage=#{turneffects.TotalDamage})");
					int hpgain=user.RecoverHP((int)Math.Floor(turneffects.TotalDamage/8d),true);
					if (hpgain>0)
						@battle.Display(Game._INTL("{1} restored a little HP using its {2}!",
							user.ToString(),Game._INTL(user.Item.ToString(TextScripts.Name))));
				}
				// Life Orb
				if (user.effects.LifeOrb && !user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					Core.Logger.Log($"[Item triggered] #{user.ToString()}'s Life Orb (recoil)");
					int hploss=user.ReduceHP((int)Math.Floor(user.TotalHP/10d),true);
					if (hploss>0)
						@battle.Display(Game._INTL("{1} lost some of its HP!",user.ToString()));
				}
				if (user.isFainted()) user.Faint(); // no return
				// Color Change
				Types movetype=thismove.GetType(thismove.Type,user,target);
				if (target.hasWorkingAbility(Abilities.COLOR_CHANGE) &&
					!target.HasType(movetype)) {//!Types.isPseudoType(movetype) &&
					Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Color Change made it #{movetype.ToString()}-type");
					target.Type1=movetype;
					target.Type2=movetype;
					target.effects.Type3=Types.NONE;
					@battle.Display(Game._INTL("{1}'s {2} made it the {3} type!",target.ToString(),
						Game._INTL(target.Ability.ToString(TextScripts.Name)),Game._INTL(movetype.ToString(TextScripts.Name))));
				}
			}
			// Moxie
			if (user.hasWorkingAbility(Abilities.MOXIE) && target.isFainted())
				if (user is IBattlerEffect u && u.IncreaseStatWithCause(Stats.ATTACK,1,user,Game._INTL(user.Ability.ToString(TextScripts.Name))))
					Core.Logger.Log($"[Ability triggered] #{user.ToString()}'s Moxie");
			// Magician
			if (user.hasWorkingAbility(Abilities.MAGICIAN))
				if (target.Item>0 && user.Item==0 &&
					user.effects.Substitute==0 &&
					target.effects.Substitute==0 &&
					!target.hasWorkingAbility(Abilities.STICKY_HOLD) &&
					!@battle.IsUnlosableItem(target,target.Item) &&
					!@battle.IsUnlosableItem(user,target.Item) &&
					(@battle.opponent.Length > 0 || !@battle.IsOpposing(user.Index))) {
					user.Item=target.Item;
					target.Item=0;
					target.effects.Unburden=true;
					if (@battle.opponent.Length != 0 &&   // In a wild battle
						user.pokemon.itemInitial==Items.NONE &&
						target.pokemon.itemInitial==user.Item) {
						user.pokemon.itemInitial=user.Item;
						target.pokemon.itemInitial=Items.NONE;
					}
					@battle.Display(Game._INTL("{1} stole {2}'s {3} with {4}!",user.ToString(),
						target.ToString(true),Game._INTL(user.Item.ToString(TextScripts.Name)),Game._INTL(user.Ability.ToString(TextScripts.Name))));
					Core.Logger.Log($"[Ability triggered] #{user.ToString()}'s Magician stole #{target.ToString(true)}'s #{user.Item.ToString()}");
				}
			// Pickpocket
			if (target.hasWorkingAbility(Abilities.PICKPOCKET))
				if (target.Item==0 && user.Item>0 &&
					user.effects.Substitute==0 &&
					target.effects.Substitute==0 &&
					!user.hasWorkingAbility(Abilities.STICKY_HOLD) &&
					!@battle.IsUnlosableItem(user,user.Item) &&
					!@battle.IsUnlosableItem(target,user.Item) &&
					(@battle.opponent.Length > 0 || !@battle.IsOpposing(target.Index))) {
					target.Item=user.Item;
					user.Item=0;
					user.effects.Unburden=true;
					if (@battle.opponent.Length != 0 &&   // In a wild battle
						target.pokemon.itemInitial==Items.NONE &&
						user.pokemon.itemInitial==target.Item) {
						target.pokemon.itemInitial=target.Item;
						user.pokemon.itemInitial=Items.NONE;
					}
					@battle.Display(Game._INTL("{1} pickpocketed {2}'s {3}!",target.ToString(),
						user.ToString(true),Game._INTL(target.Item.ToString(TextScripts.Name))));
					Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Pickpocket stole #{user.ToString(true)}'s #{target.Item.ToString()}");
				}
			}
		public void AbilityCureCheck() {
			if (this.isFainted()) return;
			switch (this.status) {
				case Status.SLEEP:
					if (this.hasWorkingAbility(Abilities.VITAL_SPIRIT) || this.hasWorkingAbility(Abilities.INSOMNIA)) {
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))}");
						CureStatus(false);
						@battle.Display(Game._INTL("{1}'s {2} woke it up!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.POISON:
					if (this.hasWorkingAbility(Abilities.IMMUNITY)) {
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))}");
						CureStatus(false);
						@battle.Display(Game._INTL("{1}'s {2} cured its poisoning!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.BURN:
					if (this.hasWorkingAbility(Abilities.WATER_VEIL)) {
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))}");
						CureStatus(false);
						@battle.Display(Game._INTL("{1}'s {2} healed its burn!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.PARALYSIS:
					if (this.hasWorkingAbility(Abilities.LIMBER)) {
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))}");
						CureStatus(false);
						@battle.Display(Game._INTL("{1}'s {2} cured its paralysis!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
					}
					break;
				case Status.FROZEN:
					if (this.hasWorkingAbility(Abilities.MAGMA_ARMOR)) {
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))}");
						CureStatus(false);
						@battle.Display(Game._INTL("{1}'s {2} defrosted it!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
					}
					break;
			}
			if (@effects.Confusion>0 && this.hasWorkingAbility(Abilities.OWN_TEMPO)) {
				Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))} (attract)");
				CureConfusion(false);
				@battle.Display(Game._INTL("{1}'s {2} snapped it out of its confusion!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
			}
			if (@effects.Attract>=0 && this.hasWorkingAbility(Abilities.OBLIVIOUS)) {
				Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))}");
				CureAttract();
				@battle.Display(Game._INTL("{1}'s {2} cured its infatuation status!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
			}
			if (Core.USENEWBATTLEMECHANICS && @effects.Taunt>0 && this.hasWorkingAbility(Abilities.OBLIVIOUS)) {
				Core.Logger.Log($"[Ability triggered] #{ToString()}'s #{Game._INTL(@ability.ToString(TextScripts.Name))} (taunt)");
				@effects.Taunt=0;
				@battle.Display(Game._INTL("{1}'s {2} made its taunt wear off!",ToString(),Game._INTL(@ability.ToString(TextScripts.Name))));
			}
		}
		#endregion

		#region Held Item effects
		public void ConsumeItem(bool recycle=true,bool pickup=true) {
			string itemname=Game._INTL(this.Item.ToString(TextScripts.Name));
			if (recycle) itemRecycle=this.Item;
			if (itemInitial==this.Item) itemInitial=0;
			if (pickup) {
				@effects.PickupItem=this.Item;
				@effects.PickupUse=@battle.nextPickupUse;
			}
			this.Item=0;
			this.effects.Unburden=true;
			// Symbiosis
			if (Partner.IsNotNullOrNone() && Partner.hasWorkingAbility(Abilities.SYMBIOSIS) && recycle)
				if (Partner.Item>0 &&
					!@battle.IsUnlosableItem(Partner,Partner.Item) &&
					!@battle.IsUnlosableItem(this,Partner.Item)) {
					@battle.Display(Game._INTL("{1}'s {2} let it share its {3} with {4}!",
						Partner.ToString(),Game._INTL(Partner.Ability.ToString(TextScripts.Name)),
						Game._INTL(Partner.Item.ToString(TextScripts.Name)),ToString(true)));
					this.Item=Partner.Item;
					Partner.Item=0;
					Partner.effects.Unburden=true;
					BerryCureCheck();
				}
		}
		public bool ConfusionBerry(Inventory.Plants.Flavours flavor,string message1,string message2) {
			int amt=this.RecoverHP((int)Math.Floor(this.TotalHP/8d),true);
			if (amt>0) {
				@battle.Display(message1);
				//if ((this.Nature%5)==flavor && (int)Math.Floor(this.Nature/5d)!=(this.Nature%5)) {
				if (Kernal.NatureData[pokemon.Nature].Dislikes==flavor) {
					@battle.Display(message2);
					ConfuseSelf();
				}
				return true;
			}
			return false;
		}
		public bool StatIncreasingBerry(Stats stat,string berryname) {
			return IncreaseStatWithCause(stat,1,this,berryname);
		}
		public void ActivateBerryEffect(Items berry=Items.NONE,bool consume=true) {
			if (berry==0) berry=this.Item;
			string berryname=(berry==0) ? "" : Game._INTL(berry.ToString(TextScripts.Name));
			Core.Logger.Log($"[Item triggered] #{ToString()}'s #{berryname}");
			bool consumed=false;
			if (berry == Items.ORAN_BERRY) {
				int amt=this.RecoverHP(10,true);
				if (amt>0) {
					@battle.Display(Game._INTL("{1} restored its health using its {2}!",ToString(),berryname));
					consumed=true;
				}
			} else if (berry == Items.SITRUS_BERRY ||
					berry == Items.ENIGMA_BERRY) {
				int amt=this.RecoverHP((int)Math.Floor(this.TotalHP/4d),true);
				if (amt>0) {
					@battle.Display(Game._INTL("{1} restored its health using its {2}!",ToString(),berryname));
					consumed=true;
				}
			} else if (berry == Items.CHESTO_BERRY)
				if (this.status==Status.SLEEP) {
					CureStatus(false);
					@battle.Display(Game._INTL("{1}'s {2} cured its sleep problem.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.PECHA_BERRY)
				if (this.status==Status.POISON) {
					CureStatus(false);
					@battle.Display(Game._INTL("{1}'s {2} cured its poisoning.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.RAWST_BERRY)
				if (this.status==Status.BURN) {
					CureStatus(false);
					@battle.Display(Game._INTL("{1}'s {2} healed its burn.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.CHERI_BERRY)
				if (this.status==Status.PARALYSIS) {
					CureStatus(false);
					@battle.Display(Game._INTL("{1}'s {2} cured its paralysis.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.ASPEAR_BERRY)
				if (this.status==Status.FROZEN) {
					CureStatus(false);
					@battle.Display(Game._INTL("{1}'s {2} thawed it out.",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.LEPPA_BERRY) {
				List<int> found= new List<int>();
				for (int i = 0; i < @pokemon.moves.Length; i++)
					if (@pokemon.moves[i].id!=0)
						if ((consume && @pokemon.moves[i].PP==0) ||
							(!consume && @pokemon.moves[i].PP<@pokemon.moves[i].TotalPP))
							found.Add(i);
				if (found.Count>0) {
					int choice=(consume) ? found[0] : found[@battle.Random(found.Count)];
					IMove pokemove=@pokemon.moves[choice];
					pokemove.PP+=10;
					if (pokemove.PP > pokemove.TotalPP) pokemove.PP=pokemove.TotalPP;
					this.moves[choice].PP=pokemove.PP;
					string movename=Game._INTL(pokemove.id.ToString(TextScripts.Name));
					@battle.Display(Game._INTL("{1}'s {2} restored {3}'s PP!",ToString(),berryname,movename)) ;
					consumed=true;
				}
			} else if (berry == Items.PERSIM_BERRY)
				if (@effects.Confusion>0) {
					CureConfusion(false);
					@battle.Display(Game._INTL("{1}'s {2} snapped it out of its confusion!",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.LUM_BERRY)
				if (this.status>0 || @effects.Confusion>0) {
					Status st=this.status; bool conf=(@effects.Confusion>0);
					CureStatus(false);
					CureConfusion(false);
					switch (st) {
						case Status.SLEEP:
							@battle.Display(Game._INTL("{1}'s {2} woke it up!",ToString(),berryname));
							break;
						case Status.POISON:
							@battle.Display(Game._INTL("{1}'s {2} cured its poisoning!",ToString(),berryname));
							break;
						case Status.BURN:
							@battle.Display(Game._INTL("{1}'s {2} healed its burn!",ToString(),berryname));
							break;
						case Status.PARALYSIS:
							@battle.Display(Game._INTL("{1}'s {2} cured its paralysis!",ToString(),berryname));
							break;
						case Status.FROZEN:
							@battle.Display(Game._INTL("{1}'s {2} defrosted it!",ToString(),berryname));
							break;
					}
					if (conf)
						@battle.Display(Game._INTL("{1}'s {2} snapped it out of its confusion!",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.FIGY_BERRY)
				consumed=ConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too spicy!",ToString(true),berryname));
			else if (berry == Items.WIKI_BERRY)
				consumed=ConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too dry!",ToString(true),berryname));
			else if (berry == Items.MAGO_BERRY)
				consumed=ConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too sweet!",ToString(true),berryname));
			else if (berry == Items.AGUAV_BERRY)
				consumed=ConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too bitter!",ToString(true),berryname));
			else if (berry == Items.IAPAPA_BERRY)
				consumed=ConfusionBerry(Kernal.BerryData[berry].Flavour,
					Game._INTL("{1}'s {2} restored health!",ToString(),berryname),
					Game._INTL("For {1}, the {2} was too sour!",ToString(true),berryname));
			else if (berry == Items.LIECHI_BERRY)
				consumed=StatIncreasingBerry(Stats.ATTACK,berryname);
			else if (berry == Items.GANLON_BERRY ||
					berry == Items.KEE_BERRY)
				consumed=StatIncreasingBerry(Stats.DEFENSE,berryname);
			else if (berry == Items.SALAC_BERRY)
				consumed=StatIncreasingBerry(Stats.SPEED,berryname);
			else if (berry == Items.PETAYA_BERRY)
				consumed=StatIncreasingBerry(Stats.SPATK,berryname);
			else if (berry == Items.APICOT_BERRY ||
					berry == Items.MARANGA_BERRY)
				consumed=StatIncreasingBerry(Stats.SPDEF,berryname);
			else if (berry == Items.LANSAT_BERRY)
				if (@effects.FocusEnergy<2) {
					@effects.FocusEnergy=2;
					@battle.Display(Game._INTL("{1} used its {2} to get pumped!",ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.MICLE_BERRY)
				if (!@effects.MicleBerry) {
					@effects.MicleBerry=true;
					@battle.Display(Game._INTL("{1} boosted the accuracy of its next move using its {2}!",
						ToString(),berryname));
					consumed=true;
				}
			else if (berry == Items.STARF_BERRY) {
				List<Stats> stats= new List<Stats>();
				foreach (Stats i in new Stats[] { Stats.ATTACK, Stats.DEFENSE, Stats.SPATK, Stats.SPDEF, Stats.SPEED })
					if (CanIncreaseStatStage(i,this)) stats.Add(i);
				if (stats.Count>0) {
					Stats stat=stats[@battle.Random(stats.Count)];
					consumed=IncreaseStatWithCause(stat,2,this,berryname);
				}
			}
			if (consumed) {
				// Cheek Pouch
				if (hasWorkingAbility(Abilities.CHEEK_POUCH)) {
					int amt=this.RecoverHP((int)Math.Floor(@TotalHP/3d),true);
					if (amt>0)
						@battle.Display(Game._INTL("{1}'s {2} restored its health!",
							ToString(),Game._INTL(ability.ToString(TextScripts.Name))));
				}
				if (consume) ConsumeItem();
				if (this.pokemon.IsNotNullOrNone()) this.belch=true;
			}
		}
		public void BerryCureCheck(bool hpcure=false) {
			if (this.isFainted()) return;
			bool unnerver = false;
			if (battle.doublebattle)
				unnerver = (Opposing1.hasWorkingAbility(Abilities.UNNERVE) || Opposing2.hasWorkingAbility(Abilities.UNNERVE));
			else
				unnerver = (Opposing1.hasWorkingAbility(Abilities.UNNERVE));
			string itemname=(this.Item==0) ? "" : Game._INTL(this.Item.ToString(TextScripts.Name));
			if (hpcure)
				if (this.hasWorkingItem(Items.BERRY_JUICE) && this.HP<= (int)Math.Floor(this.TotalHP/2d)) {
					int amt=this.RecoverHP(20,true);
					if (amt>0) {
						@battle.CommonAnimation("UseItem",this,null);
						@battle.Display(Game._INTL("{1} restored its health using its {2}!",ToString(),itemname));
						ConsumeItem();
						return;
					}
				}
			if (!unnerver) {
				if (hpcure) {
					if (this.HP<= (int)Math.Floor(this.TotalHP/2d)) {
						if (this.hasWorkingItem(Items.ORAN_BERRY) ||
							this.hasWorkingItem(Items.SITRUS_BERRY)) {
							ActivateBerryEffect();
							return;
						}
						if (this.hasWorkingItem(Items.FIGY_BERRY) ||
							this.hasWorkingItem(Items.WIKI_BERRY) ||
							this.hasWorkingItem(Items.MAGO_BERRY) ||
							this.hasWorkingItem(Items.AGUAV_BERRY) ||
							this.hasWorkingItem(Items.IAPAPA_BERRY)) {
							ActivateBerryEffect();
							return;
						}
					}
				} //<= Not sure if this should move to 1930
				if ((this.hasWorkingAbility(Abilities.GLUTTONY) && this.HP<= (int)Math.Floor(this.TotalHP/2d)) ||
					this.HP<= (int)Math.Floor(this.TotalHP/4d)) {
					if (this.hasWorkingItem(Items.LIECHI_BERRY) ||
						this.hasWorkingItem(Items.GANLON_BERRY) ||
						this.hasWorkingItem(Items.SALAC_BERRY) ||
						this.hasWorkingItem(Items.PETAYA_BERRY) ||
						this.hasWorkingItem(Items.APICOT_BERRY)) {
						ActivateBerryEffect();
						return;
					}
					if (this.hasWorkingItem(Items.LANSAT_BERRY) ||
						this.hasWorkingItem(Items.STARF_BERRY)) {
						ActivateBerryEffect();
						return;
					}
					if (this.hasWorkingItem(Items.MICLE_BERRY)) {
						ActivateBerryEffect();
						return;
					}
				}
				if (this.hasWorkingItem(Items.LEPPA_BERRY)) {
					ActivateBerryEffect();
					return;
				}
				if (this.hasWorkingItem(Items.CHESTO_BERRY) ||
					this.hasWorkingItem(Items.PECHA_BERRY) ||
					this.hasWorkingItem(Items.RAWST_BERRY) ||
					this.hasWorkingItem(Items.CHERI_BERRY) ||
					this.hasWorkingItem(Items.ASPEAR_BERRY) ||
					this.hasWorkingItem(Items.PERSIM_BERRY) ||
					this.hasWorkingItem(Items.LUM_BERRY)) {
					ActivateBerryEffect();
					return;
				}
			}
			if (this.hasWorkingItem(Items.WHITE_HERB)) {
				bool reducedstats=false;
				foreach (Stats i in new Stats[]{ Stats.ATTACK,Stats.DEFENSE,
						Stats.SPEED,Stats.SPATK,Stats.SPDEF,
						Stats.ACCURACY,Stats.EVASION })
					if (@stages[(int)i]<0) {
						@stages[(int)i]=0; reducedstats=true;
					}
				if (reducedstats) {
					Core.Logger.Log($"[Item triggered] #{ToString()}'s #{itemname}");
					@battle.CommonAnimation("UseItem",this,null);
					@battle.Display(Game._INTL("{1} restored its status using its {2}!",ToString(),itemname));
					ConsumeItem();
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
				Core.Logger.Log($"[Item triggered] #{ToString()}'s #{itemname}");
				@battle.CommonAnimation("UseItem",this,null);
				if (@effects.Attract>=0) @battle.Display(Game._INTL("{1} cured its infatuation status using its {2}.",ToString(),itemname));
				if (@effects.Taunt>0) @battle.Display(Game._INTL("{1}'s taunt wore off!",ToString()));
				if (@effects.Encore>0) @battle.Display(Game._INTL("{1}'s encore ended!",ToString()));
				if (@effects.Torment) @battle.Display(Game._INTL("{1}'s torment wore off!",ToString()));
				if (@effects.Disable>0) @battle.Display(Game._INTL("{1} is no longer disabled!",ToString()));
				if (@effects.HealBlock>0) @battle.Display(Game._INTL("{1}'s Heal Block wore off!",ToString()));
				this.CureAttract();
				@effects.Taunt=0;
				@effects.Encore=0;
				@effects.EncoreMove=0;
				@effects.EncoreIndex=0;
				@effects.Torment=false;
				@effects.Disable=0;
				@effects.HealBlock=0;
				ConsumeItem();
				return;
			}
			if (hpcure && this.hasWorkingItem(Items.LEFTOVERS) && this.HP!=this.TotalHP &&
				@effects.HealBlock==0) {
				Core.Logger.Log($"[Item triggered] #{ToString()}'s Leftovers");
				@battle.CommonAnimation("UseItem",this,null);
				RecoverHP((int)Math.Floor(this.TotalHP/16d),true);
				@battle.Display(Game._INTL("{1} restored a little HP using its {2}!",ToString(),itemname));
			}
			if (hpcure && this.hasWorkingItem(Items.BLACK_SLUDGE)) {
				if (HasType(Types.POISON))
					if (this.HP!=this.TotalHP &&
						(!Core.USENEWBATTLEMECHANICS || @effects.HealBlock==0)) {
						Core.Logger.Log($"[Item triggered] #{ToString()}'s Black Sludge (heal)");
						@battle.CommonAnimation("UseItem",this,null);
						RecoverHP((int)Math.Floor(this.TotalHP/16d),true);
						@battle.Display(Game._INTL("{1} restored a little HP using its {2}!",ToString(),itemname));
					}
				else if (!this.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
					Core.Logger.Log($"[Item triggered] #{ToString()}'s Black Sludge (damage)");
					@battle.CommonAnimation("UseItem",this,null);
					ReduceHP((int)Math.Floor(this.TotalHP/8d),true);
					@battle.Display(Game._INTL("{1} was hurt by its {2}!",ToString(),itemname));
				}
				if (this.isFainted()) Faint();
			}
		}
		#endregion

		#region Move user and targets
		public IBattler FindUser(IBattleChoice choice,IList<IBattler> targets) {
			IBattleMove move=choice.Move;
			int target=choice.Target;
			IBattler user = this;   // Normally, the user is this
			// Targets in normal cases
			switch (Target(move)) { //ToDo: Missing `Select everyone` (including user)
				case Attack.Targets.SELECTED_POKEMON: //Attack.Target.SingleNonUser:
				case Attack.Targets.SELECTED_POKEMON_ME_FIRST:
					if (target>=0) {
						IBattler targetBattler=@battle.battlers[target];
						if (!IsOpposing(targetBattler.Index))
							if (!AddTarget(targets,targetBattler))
								if (!AddTarget(targets,Opposing1)) AddTarget(targets,Opposing2);
						else
							if (!AddTarget(targets,targetBattler)) AddTarget(targets,targetBattler.Partner);
					}
					else
						RandomTarget(targets);
					break;
				//case Attack.Targets.SELECTED_POKEMON: //Attack.Target.SingleOpposing:
				//case Attack.Targets.SELECTED_POKEMON_ME_FIRST:
				//	if (target>=0) {
				//		IBattler targetBattler=@battle.battlers[target];
				//		if (!IsOpposing(targetBattler.Index))
				//			if (!AddTarget(targets,targetBattler))
				//				if (!AddTarget(targets,Opposing1)) AddTarget(targets,Opposing2);
				//		else
				//			if (!AddTarget(targets,targetBattler)) AddTarget(targets,targetBattler.Partner);
				//	}
				//	else
				//		RandomTarget(targets);
				//	break;
				case Attack.Targets.OPPONENTS_FIELD: //Attack.Target.OppositeOpposing:
					if (!AddTarget(targets,OppositeOpposing2)) AddTarget(targets,OppositeOpposing);
					break;
				case Attack.Targets.RANDOM_OPPONENT: //Attack.Target.RandomOpposing:
					RandomTarget(targets);
					break;
				case Attack.Targets.ALL_OPPONENTS: //Attack.Target.AllOpposing:
					// Just Opposing1 because partner is determined late
					if (!AddTarget(targets,Opposing1)) AddTarget(targets,Opposing2);
					break;
				case Attack.Targets.ALL_OTHER_POKEMON: //Attack.Target.AllNonUsers:
					for (int i = 0; i < 4; i++) // not ordered by priority
					if (i!=@Index) AddTarget(targets,@battle.battlers[i]);
					break;
				case Attack.Targets.USER_OR_ALLY: //Attack.Target.UserOrPartner:
					if (target>=0) { // Pre-chosen target
						IBattler targetBattler=@battle.battlers[target];
						if (!AddTarget(targets,targetBattler)) AddTarget(targets,targetBattler.Partner);
					}
					else
						AddTarget(targets,this);
					break;
				case Attack.Targets.ALLY: //Attack.TargetPartner:
					AddTarget(targets,Partner);
					break;
				default:
					move.AddTarget(targets,this);
					break;
			}
			return user;
		}
		public IBattler ChangeUser(IBattleMove thismove,IBattler user) {
			IBattler[] priority=@battle.Priority();
			// Change user to user of Snatch
			if (thismove.Flags.Snatch)
				foreach (var i in priority)
					if (i.effects.Snatch) {
						@battle.Display(Game._INTL("{1} snatched {2}'s move!",i.ToString(),user.ToString(true)));
						Core.Logger.Log($"[Lingering effect triggered] #{i.ToString()}'s Snatch made it use #{user.ToString(true)}'s #{thismove.id.ToString()}");
						i.effects.Snatch=false;
						IBattler target=user;
						user=i;
						// Snatch's PP is reduced if old user has Pressure
						IBattleChoice userchoice=@battle.choices[user.Index];
						if (target.hasWorkingAbility(Abilities.PRESSURE) && user.IsOpposing(target.Index) && userchoice.Action>=0) {
							IBattleMove pressuremove=user.moves[(int)userchoice.Move.id]; //ToDo: Change Choice.Move to int
							if (pressuremove.PP>0) SetPP(pressuremove,(byte)(pressuremove.PP-1));
						}
						if (Core.USENEWBATTLEMECHANICS) break;
					}
			return user;
		}
		public Attack.Targets Target(IBattleMove move) {
			//Attack.Target target=move.Targets;
			Attack.Targets target=move.Target;
			if (move.Effect == Attack.Effects.x06E && HasType(Types.GHOST)) // Curse
				//target=Attack.Target.OppositeOpposing;
				target=Attack.Targets.OPPONENTS_FIELD;
			return target;
		}
		//public bool AddTarget(IList<IBattler> targets,IBattler target) {
		//	if (!target.isFainted()) {
		//		targets[targets.Count - 1]=target; // Add target to end of the list...
		//		return true;
		//	}
		//	return false;
		//}
		public bool AddTarget(IList<IBattler> targets,IBattler target) {
			if (!target.isFainted()) {
				targets.Add(target);
				return true;
			}
			return false;
		}
		public void RandomTarget(IList<IBattler> targets) {
			IList<IBattler> choices= new List<IBattler>();
			AddTarget(choices,Opposing1);
			//if (battle.doublebattle) //Added a null conditional to below function
				AddTarget(choices,Opposing2);
			if (choices.Count>0)
				AddTarget(targets,choices[@battle.Random(choices.Count)]);
		}
		public bool ChangeTarget(IBattleMove thismove,IBattler[] userandtarget,IBattler[] targets) {
			IBattler[] priority=@battle.Priority();
			int changeeffect=0;
			IBattler user=userandtarget[0];
			IBattler target=userandtarget[1];
			// Lightningrod
			if (targets.Length==1 && thismove.GetType(thismove.Type,user,target) == Types.ELECTRIC &&
				!target.hasWorkingAbility(Abilities.LIGHTNING_ROD))
				foreach (var i in priority) { // use Pokémon earliest in priority
					if (user.Index==i.Index || target.Index==i.Index) continue;
					if (i.hasWorkingAbility(Abilities.LIGHTNING_ROD)) {
						Core.Logger.Log($"[Ability triggered] #{i.ToString()}'s Lightningrod (change target)");
						target=i; // X's Lightningrod took the attack!
						changeeffect=1;
						break;
					}
				}
			// Storm Drain
			if (targets.Length==1 && thismove.GetType(thismove.Type,user,target) == Types.WATER &&
				!target.hasWorkingAbility(Abilities.STORM_DRAIN))
				foreach (var i in priority) { // use Pokémon earliest in priority
					if (user.Index==i.Index || target.Index==i.Index) continue;
					if (i.hasWorkingAbility(Abilities.STORM_DRAIN)) {
						Core.Logger.Log($"[Ability triggered] #{i.ToString()}'s Storm Drain (change target)");
						target=i; // X's Storm Drain took the attack!
						changeeffect=1;
						break;
					}
				}
			// Change target to user of Follow Me (overrides Magic Coat
			// because check for Magic Coat below uses this target)
			if (thismove.Target.TargetsOneOpponent()) {
				IBattler newtarget=null; int strength=100;
				foreach (var i in priority) { // use Pokémon latest in priority
					if (!user.IsOpposing(i.Index)) continue;
					if (!i.isFainted() && !@battle.switching && !i.effects.SkyDrop &&
						i.effects.FollowMe>0 && i.effects.FollowMe<strength) {
						Core.Logger.Log($"[Lingering effect triggered] #{i.ToString()}'s Follow Me");
						newtarget=i; strength=i.effects.FollowMe;
						changeeffect=0;
					}
				}
				if (newtarget.IsNotNullOrNone()) target=newtarget;
			}
			// TODO: Pressure here is incorrect if Magic Coat redirects target
			if (user.IsOpposing(target.Index) && target.hasWorkingAbility(Abilities.PRESSURE)) {
				Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Pressure (in ChangeTarget)");
				user.ReducePP(thismove); // Reduce PP
			}
			// Change user to user of Snatch
			if (thismove.Flags.Snatch)
				foreach (var i in priority)
					if (i.effects.Snatch) {
						@battle.Display(Game._INTL("{1} Snatched {2}'s move!",i.ToString(),user.ToString(true)));
						Core.Logger.Log($"[Lingering effect triggered] #{i.ToString()}'s Snatch made it use #{user.ToString(true)}'s #{thismove.id.ToString()}");
						i.effects.Snatch=false;
						target=user;
						user=i;
						// Snatch's PP is reduced if old user has Pressure
						int userchoice=@battle.choices[user.Index].Index;
						if (target.hasWorkingAbility(Abilities.PRESSURE) && user.IsOpposing(target.Index) && userchoice>=0) {
							Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Pressure (part of Snatch)");
							IBattleMove pressuremove=user.moves[userchoice];
							if (pressuremove.PP>0) SetPP(pressuremove,(byte)(pressuremove.PP-1));
						}
					}
			if (thismove.Flags.Reflectable)//canMagicCoat()
				if (target.effects.MagicCoat) {
					// switch user and target
					Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Magic Coat made it use #{user.ToString(true)}'s #{thismove.id.ToString()}");
					changeeffect=3;
					IBattler tmp=user;
					user=target;
					target=tmp;
					// Magic Coat's PP is reduced if old user has Pressure
					int userchoice=@battle.choices[user.Index].Index;
					if (target.hasWorkingAbility(Abilities.PRESSURE) && user.IsOpposing(target.Index) && userchoice>=0) {
						Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Pressure (part of Magic Coat)");
						IBattleMove pressuremove=user.moves[userchoice];
						if (pressuremove.PP>0) SetPP(pressuremove,(byte)(pressuremove.PP-1));
					}
				} else if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.MAGIC_BOUNCE)) {
					// switch user and target
					Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Magic Bounce made it use #{user.ToString(true)}'s #{thismove.id.ToString()}");
					changeeffect=3;
					IBattler tmp=user;
					user=target;
					target=tmp;
				}
			if (changeeffect==1)
				@battle.Display(Game._INTL("{1}'s {2} took the move!",target.ToString(),Game._INTL(target.Ability.ToString(TextScripts.Name))));
			else if (changeeffect==3)
				@battle.Display(Game._INTL("{1} bounced the {2} back!",user.ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
			userandtarget[0]=user;
			userandtarget[1]=target;
			if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.SOUNDPROOF) &&
				thismove.Flags.SoundBased && //isSoundBased()
				thismove.Effect != Attack.Effects.x073 &&	// Perish Song handled elsewhere
				thismove.Effect != Attack.Effects.x15B) {	// Parting Shot handled elsewhere
				Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Soundproof blocked #{user.ToString(true)}'s #{thismove.id.ToString()}");
				@battle.Display(Game._INTL("{1}'s {2} blocks {3}!",target.ToString(),
					Game._INTL(target.Ability.ToString(TextScripts.Name)),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				return false;
			}
			return true;
		}
		#endregion

		#region Move PP
		//ToDo: Revisit SetPP, and maybe try (int move, byte pp) or ref move
		public void SetPP(IBattleMove move,int pp) {
			move.PP=pp;
			// Not effects.Mimic, since Mimic can't copy Mimic
			if (move.thismove.IsNotNullOrNone() && move.id==move.thismove.id && !@effects.Transform)
				move.thismove.PP=pp;
		}
		//public void SetPP(Attack.Move move,byte pp) {
		//	move.PP=pp;
		//	// Not effects.Mimic, since Mimic can't copy Mimic
		//	if (move.IsNotNullOrNone() &&
		//		//move.id==move.id && //For-loop, on Pokemon.Moves[int].id == move.id
		//		!@effects.Transform)
		//	  //move.thismove.PP=pp;
		//	  pokemon.moves[0].PP=pp;
		//}
		public bool ReducePP(IBattleMove move) {
			if (@effects.TwoTurnAttack>0 ||
				@effects.Bide>0 ||
				@effects.Outrage>0 ||
				@effects.Rollout>0 ||
				@effects.HyperBeam>0 ||
				@effects.Uproar>0)
					// No need to reduce PP if two-turn attack
					return true;
			if (move.PP<0) return true;			// No need to reduce PP for special calls of moves
			if (move.TotalPP==0) return true;   // Infinite PP, can always be used
			if (move.PP==0) return false;
			if (move.PP>0)
				SetPP(move,(byte)(move.PP-1));
			return true;
		}
		public void ReducePPOther(IBattleMove move) {
			if (move.PP>0) SetPP(move,(byte)(move.PP-1));
		}
		#endregion

		#region Using a move
		public bool ObedienceCheck(IBattleChoice choice) {
			if (choice.Action!=ChoiceAction.UseMove) return true;
			if (@battle.OwnedByPlayer(@Index) && @battle.internalbattle) {
				int badgelevel=10;
				int badgeCount = @battle.Player().badges.Count(b => b == true);
				if (badgeCount>=1) badgelevel=20 ;
				if (badgeCount>=2) badgelevel=30 ;
				if (badgeCount>=3) badgelevel=40 ;
				if (badgeCount>=4) badgelevel=50 ;
				if (badgeCount>=5) badgelevel=60 ;
				if (badgeCount>=6) badgelevel=70 ;
				if (badgeCount>=7) badgelevel=80 ;
				if (badgeCount>=8) badgelevel=100;
				IBattleMove move=choice.Move;
				bool disobedient=false;
				if (@pokemon.isForeign(@battle.player[0]) && @level>badgelevel) {
					int a=(int)Math.Floor((@level+badgelevel)*@battle.Random(256)/255d);
					disobedient|=a<badgelevel;
				}
				if (this is IBattlerShadowPokemon s) //this.respond_to("HyperModeObedience")
					disobedient|=!s.HyperModeObedience(move);
				if (disobedient) {
					Core.Logger.Log($"[Disobedience] #{ToString()} disobeyed");
					@effects.Rage=false;
					if (this.status==Status.SLEEP &&
						(move.Effect == Attack.Effects.x05D || move.Effect == Attack.Effects.x062)) { // Snore, Sleep Talk
						@battle.Display(Game._INTL("{1} ignored orders while asleep!",ToString()));
						return false;
					}
					int b=(int)Math.Floor((@level+badgelevel)*@battle.Random(256)/255d);
					if (b<badgelevel) {
						if (!@battle.CanShowFightMenu(@Index)) return false;
						List<int> othermoves= new List<int>();
						for (int i = 0; i < 4; i++) {
							if (i==choice.Index) continue;
							if (@battle.CanChooseMove(@Index,i,false)) othermoves.Add(i);
						}
						if (othermoves.Count>0) {
							@battle.Display(Game._INTL("{1} ignored orders!",ToString()));
							int newchoice=othermoves[@battle.Random(othermoves.Count)];
							//choice.Action=ChoiceAction.UseMove;
							//choice.Move=@moves[newchoice];
							//choice.Target=-1;
							choice = new Choice(action: ChoiceAction.UseMove, moveIndex: newchoice, move: @moves[newchoice]);
						}
						return true;
					}
					else if (this.status!=Status.SLEEP) {
						int c=@level-b;
						int r=@battle.Random(256);
						if (r<c && CanSleep(this,false)) {
							SleepSelf();
							@battle.Display(Game._INTL("{1} took a nap!",ToString()));
							return false;
						}
						r-=c;
						if (r<c) {
							@battle.Display(Game._INTL("It hurt itself in its confusion!"));
							ConfusionDamage();
						}
						else {
							int message=@battle.Random(4);
							if (message==0) @battle.Display(Game._INTL("{1} ignored orders!",ToString()));
							if (message==1) @battle.Display(Game._INTL("{1} turned away!",ToString()));
							if (message==2) @battle.Display(Game._INTL("{1} is loafing around!",ToString()));
							if (message==3) @battle.Display(Game._INTL("{1} pretended not to notice!",ToString()));
						}
						return false;
					}
				}
				return true;
			}
			else
				return true;
		}
		public bool SuccessCheck(IBattleMove thismove,IBattler user,IBattler target,IEffectsMove turneffects,bool accuracy=true) {
			if (user.effects.TwoTurnAttack>0)
				return true;
			// TODO: "Before Protect" applies to Counter/Mirror Coat
			if (thismove.Effect == Attack.Effects.x009 && target.Status!=Status.SLEEP) { // Dream Eater
				@battle.Display(Game._INTL("{1} wasn't affected!",target.ToString()));
				Core.Logger.Log($"[Move failed] #{user.ToString()}'s Dream Eater's target isn't asleep");
				return false;
			}
			if (thismove.Effect == Attack.Effects.x0A2 && user.effects.Stockpile==0) { // Spit Up
				@battle.Display(Game._INTL("But it failed to spit up a thing!"));
				Core.Logger.Log($"[Move failed] #{user.ToString()}'s Spit Up did nothing as Stockpile's count is 0");
				return false;
			}
			if (target.effects.Protect && thismove.Flags.Protectable &&
				!target.effects.ProtectNegation) {
				@battle.Display(Game._INTL("{1} protected itself!",target.ToString()));
				@battle.successStates[user.Index].Protected=true;
				Core.Logger.Log($"[Move failed] #{target.ToString()}'s Protect stopped the attack");
				return false;
			}
			int p=thismove.Priority;
			if (Core.USENEWBATTLEMECHANICS) {
				if (user.hasWorkingAbility(Abilities.PRANKSTER) && thismove.Category == Attack.Category.STATUS) p+=1;
				if (user.hasWorkingAbility(Abilities.GALE_WINGS) && thismove.Type == Types.FLYING) p+=1;
			}
			if (target.OwnSide.QuickGuard && thismove.Flags.Protectable &&
				p>0 && !target.effects.ProtectNegation) {
				@battle.Display(Game._INTL("{1} was protected by Quick Guard!",target.ToString()));
				Core.Logger.Log($"[Move failed] The opposing side's Quick Guard stopped the attack");
				return false;
			}
			if (target.OwnSide.WideGuard &&
				thismove.Target.HasMultipleTargets() && thismove.Category != Attack.Category.STATUS &&
				!target.effects.ProtectNegation) {
				@battle.Display(Game._INTL("{1} was protected by Wide Guard!",target.ToString()));
				Core.Logger.Log($"[Move failed] The opposing side's Wide Guard stopped the attack");
				return false;
			}
			if (target.OwnSide.CraftyShield && thismove.Category == Attack.Category.STATUS &&
				thismove.Effect != Attack.Effects.x073) { // Perish Song
				@battle.Display(Game._INTL("Crafty Shield protected {1}!",target.ToString(true)));
				Core.Logger.Log($"[Move failed] The opposing side's Crafty Shield stopped the attack");
				return false;
			}
			if (target.OwnSide.MatBlock && thismove.Category != Attack.Category.STATUS &&
				thismove.Flags.Protectable && !target.effects.ProtectNegation) {
				@battle.Display(Game._INTL("{1} was blocked by the kicked-up mat!",Game._INTL(thismove.id.ToString(TextScripts.Name))));
				Core.Logger.Log($"[Move failed] The opposing side's Mat Block stopped the attack");
				return false;
			}
			// TODO: Mind Reader/Lock-On
			// --Sketch/FutureSight/PsychUp work even on Fly/Bounce/Dive/Dig
			if (thismove.MoveFailed(user,target)) { // TODO: Applies to Snore/Fake Out
				@battle.Display(Game._INTL("But it failed!"));
				Core.Logger.Log(string.Format("[Move failed] Failed MoveFailed (function code %02X)",thismove.Effect));
				return false;
			}
			// King's Shield (purposely after MoveFailed)
			if (target.effects.KingsShield && thismove.Category != Attack.Category.STATUS &&
				thismove.Flags.Protectable && !target.effects.ProtectNegation) {
				@battle.Display(Game._INTL("{1} protected itself!",target.ToString()));
				@battle.successStates[user.Index].Protected=true;
				Core.Logger.Log($"[Move failed] #{target.ToString()}'s King's Shield stopped the attack");
				if (thismove.Flags.Contact && user is IBattlerEffect u)
					u.ReduceStat(Stats.ATTACK,2,null,false);
				return false;
			}
			// Spiky Shield
			if (target.effects.SpikyShield && thismove.Flags.Protectable &&
				!target.effects.ProtectNegation) {
				@battle.Display(Game._INTL("{1} protected itself!",target.ToString()));
				@battle.successStates[user.Index].Protected=true;
				Core.Logger.Log($"[Move failed] #{user.ToString()}'s Spiky Shield stopped the attack");
				if (thismove.Flags.Contact && !user.isFainted()) {
					if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.DamageAnimation(user,0);
					int amt=user.ReduceHP((int)Math.Floor(user.TotalHP/8d));
					if (amt>0) @battle.Display(Game._INTL("{1} was hurt!",user.ToString()));
				}
				return false;
			}
			// Immunity to powder-based moves
			if (Core.USENEWBATTLEMECHANICS && thismove.Flags.PowderBased &&
				(target.HasType(Types.GRASS) ||
				(!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.OVERCOAT)) ||
				target.hasWorkingItem(Items.SAFETY_GOGGLES))) {
				@battle.Display(Game._INTL("It doesn't affect\r\n{1}...",target.ToString(true)));
				Core.Logger.Log($"[Move failed] #{target.ToString()} is immune to powder-based moves somehow");
				return false;
			}
			if (thismove.basedamage>0 && thismove.Effect != Attack.Effects.x0FF && // Struggle
				thismove.Effect != Attack.Effects.x095) { // Future Sight
				Types type=thismove.GetType(thismove.Type,user,target);
				float typemod=thismove.TypeModifier(type,user,target);
				// Airborne-based immunity to Ground moves
				if (type == Types.GROUND && target.isAirborne(user.hasMoldBreaker()) &&
					!target.hasWorkingItem(Items.RING_TARGET) && thismove.Effect != Attack.Effects.x120) { // Smack Down
					if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.LEVITATE)) {
						@battle.Display(Game._INTL("{1} makes Ground moves miss with Levitate!",target.ToString()));
						Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Levitate made the Ground-type move miss");
						return false;
					}
					if (target.hasWorkingItem(Items.AIR_BALLOON)) {
						@battle.Display(Game._INTL("{1}'s Air Balloon makes Ground moves miss!",target.ToString()));
						Core.Logger.Log($"[Item triggered] #{target.ToString()}'s Air Balloon made the Ground-type move miss");
						return false;
					}
					if (target.effects.MagnetRise>0) {
						@battle.Display(Game._INTL("{1} makes Ground moves miss with Magnet Rise!",target.ToString()));
						Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Magnet Rise made the Ground-type move miss");
						return false;
					}
					if (target.effects.Telekinesis>0) {
						@battle.Display(Game._INTL("{1} makes Ground moves miss with Telekinesis!",target.ToString()));
						Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Telekinesis made the Ground-type move miss");
						return false;
					}
				}
				if (!user.hasMoldBreaker() && target.hasWorkingAbility(Abilities.WONDER_GUARD) &&
					type>=0 && typemod<=8) {
					@battle.Display(Game._INTL("{1} avoided damage with Wonder Guard!",target.ToString()));
					Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Wonder Guard");
					return false;
				}
				if (typemod==0) {
					@battle.Display(Game._INTL("It doesn't affect\r\n{1}...",target.ToString(true)));
					Core.Logger.Log($"[Move failed] Type immunity");
					return false;
				}
			}
			if (accuracy) {
				if (target.effects.LockOn>0 && target.effects.LockOnPos==user.Index) {
					Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Lock-On");
					return true;
				}
				bool miss=false; bool _override=false;
				Attack.Effects invulmove=Kernal.MoveData[target.effects.TwoTurnAttack].Effect;
				switch (invulmove) {
					case Attack.Effects.x09C: case Attack.Effects.x108:	// Fly, Bounce
						if (thismove.Effect != Attack.Effects.x099 ||			// Thunder
							thismove.Effect != Attack.Effects.x14E ||			// Hurricane
							thismove.Effect != Attack.Effects.x096 ||			// Gust
							thismove.Effect != Attack.Effects.x093 ||			// Twister
							thismove.Effect != Attack.Effects.x0D0 ||			// Sky Uppercut
							thismove.Effect != Attack.Effects.x120 ||			// Smack Down
							thismove.id != Moves.WHIRLWIND)miss=true;
						break;
					case Attack.Effects.x101:									// Dig
						if (thismove.Effect != Attack.Effects.x094 ||			// Earthquake
							thismove.Effect != Attack.Effects.x07F)			// Magnitude
							miss=true;
						break;
					case Attack.Effects.x100:									// Dive
						if (thismove.Effect != Attack.Effects.x102 ||			// Surf
							thismove.Effect != Attack.Effects.x106)			// Whirlpool
							miss=true;
						break;
					//case Attack.Effects.x111:								// Shadow Force
					case Attack.Effects.x111:									// Phantom Force
						miss=true;
						break;
					case Attack.Effects.x138:									// Sky Drop
						if (thismove.Effect != Attack.Effects.x099 ||			// Thunder
							thismove.Effect != Attack.Effects.x14E ||			// Hurricane
							thismove.Effect != Attack.Effects.x096 ||			// Gust
							thismove.Effect != Attack.Effects.x093 ||			// Twister
							thismove.Effect != Attack.Effects.x0D0 ||			// Sky Uppercut
							thismove.Effect != Attack.Effects.x120)			// Smack Down
							miss=true;
						break;
				}
				if (target.effects.SkyDrop)
				if (thismove.Effect != Attack.Effects.x099 ||					// Thunder
					thismove.Effect != Attack.Effects.x14E ||					// Hurricane
					thismove.Effect != Attack.Effects.x096 ||					// Gust
					thismove.Effect != Attack.Effects.x093 ||					// Twister
					thismove.Effect != Attack.Effects.x138 ||					// Sky Drop
					thismove.Effect != Attack.Effects.x0D0 ||					// Sky Uppercut
					thismove.Effect != Attack.Effects.x120)					// Smack Down
					miss=true;
				if (user.hasWorkingAbility(Abilities.NO_GUARD) ||
							target.hasWorkingAbility(Abilities.NO_GUARD) ||
							@battle.futuresight) miss=false;
				if (Core.USENEWBATTLEMECHANICS && thismove.Effect == Attack.Effects.x022 && // Toxic
							thismove.basedamage==0 && user.HasType(Types.POISON)) _override=true;
				if (!miss && turneffects.SkipAccuracyCheck) _override=true; // Called by another move
				if (!_override && (miss || !thismove.AccuracyCheck(user,target))) { // Includes Counter/Mirror Coat
					Core.Logger.Log(string.Format("[Move failed] Failed AccuracyCheck (Effect Id: {0}) or target is semi-invulnerable",thismove.Effect));
					if (thismove.Target==Attack.Targets.ALL_OPPONENTS && //thismove.Targets==Attack.Target.AllOpposing
						(!user.Opposing1.isFainted() ? 1 : 0) + (!user.Opposing2.isFainted() ? 1 : 0) > 1)
						@battle.Display(Game._INTL("{1} avoided the attack!",target.ToString()));
					else if (thismove.Target==Attack.Targets.ALL_OTHER_POKEMON && //thismove.Targets==Attack.Target.AllNonUsers
						(!user.Opposing1.isFainted() ? 1 : 0) + (!user.Opposing2.isFainted() ? 1 : 0) + (!user.Partner.isFainted() ? 1 : 0) > 1)
						@battle.Display(Game._INTL("{1} avoided the attack!",target.ToString()));
					else if (target.effects.TwoTurnAttack>0)
						@battle.Display(Game._INTL("{1} avoided the attack!",target.ToString()));
					else if (thismove.Effect == Attack.Effects.x055) // Leech Seed
						@battle.Display(Game._INTL("{1} evaded the attack!",target.ToString()));
					else
						@battle.Display(Game._INTL("{1}'s attack missed!",user.ToString()));
					return false;
				}
			}
			return true;
		}
		public bool TryUseMove(IBattleChoice choice,IBattleMove thismove,IEffectsMove turneffects) {
			if (turneffects.PassedTrying) return true;
			// TODO: Return true if attack has been Mirror Coated once already
			//if (!turneffects.SkipAccuracyCheck)
			//  if (!ObedienceCheck(choice)) return false;
			if (@effects.SkyDrop) { // Intentionally no message here
				Core.Logger.Log($"[Move failed] #{ToString()} can't use #{thismove.id.ToString()} because of being Sky Dropped");
				return false;
			}
			if (@battle.field.Gravity>0 && thismove.Flags.Gravity) {
				@battle.Display(Game._INTL("{1} can't use {2} because of gravity!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				Core.Logger.Log($"[Move failed] #{ToString()} can't use #{thismove.id.ToString()} because of Gravity");
				return false;
			}
			if (@effects.Taunt>0 && thismove.basedamage==0) {
				@battle.Display(Game._INTL("{1} can't use {2} after the taunt!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				Core.Logger.Log($"[Move failed] #{ToString()} can't use #{thismove.id.ToString()} because of Taunt");
				return false;
			}
			if (@effects.HealBlock>0 && thismove.isHealingMove()) {
				@battle.Display(Game._INTL("{1} can't use {2} because of Heal Block!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				Core.Logger.Log($"[Move failed] #{ToString()} can't use #{thismove.id.ToString()} because of Heal Block");
				return false;
			}
			if (@effects.Torment && thismove.id==@lastMoveUsed &&
				thismove.id!=@battle.struggle.id && @effects.TwoTurnAttack==0) {
				@battle.DisplayPaused(Game._INTL("{1} can't use the same move in a row due to the torment!",ToString()));
				Core.Logger.Log($"[Move failed] #{ToString()} can't use #{thismove.id.ToString()} because of Torment");
				return false;
			}
			if (Opposing1.effects.Imprison && !Opposing1.isFainted())
				if(thismove.id==Opposing1.moves[0].id ||
					thismove.id==Opposing1.moves[1].id ||
					thismove.id==Opposing1.moves[2].id ||
					thismove.id==Opposing1.moves[3].id) {
					@battle.Display(Game._INTL("{1} can't use the sealed {2}!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					Core.Logger.Log($"[Move failed] #{thismove.id.ToString()} can't use #{thismove.id.ToString()} because of #{Opposing1.ToString(true)}'s Imprison");
					return false;
				}
			if (battle.doublebattle && Opposing2.effects.Imprison && !Opposing2.isFainted())
				if(thismove.id==Opposing2.moves[0].id ||
					thismove.id==Opposing2.moves[1].id ||
					thismove.id==Opposing2.moves[2].id ||
					thismove.id==Opposing2.moves[3].id) {
					@battle.Display(Game._INTL("{1} can't use the sealed {2}!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					Core.Logger.Log($"[Move failed] #{thismove.id.ToString()} can't use #{thismove.id.ToString()} because of #{Opposing2.ToString(true)}'s Imprison");
					return false;
				}
			if (@effects.Disable>0 && thismove.id==@effects.DisableMove &&
				!@battle.switching) { // Pursuit ignores if it's disabled
				@battle.DisplayPaused(Game._INTL("{1}'s {2} is disabled!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
				Core.Logger.Log($"[Move failed] #{ToString()}'s #{thismove.id.ToString()} is disabled");
				return false;
			}
			if (choice.Index==-2) { // Battle Palace
				@battle.Display(Game._INTL("{1} appears incapable of using its power!",ToString()));
				Core.Logger.Log($"[Move failed] Battle Palace: #{ToString()} is incapable of using its power");
				return false;
			}
			if (@effects.HyperBeam>0) {
				@battle.Display(Game._INTL("{1} must recharge!",ToString()));
				Core.Logger.Log($"[Move failed] #{ToString()} must recharge after using #{Combat.Move.FromMove(@battle,new Attack.Move(@currentMove)).ToString()}");
				return false;
			}
			if (this.hasWorkingAbility(Abilities.TRUANT) && @effects.Truant) {
				@battle.Display(Game._INTL("{1} is loafing around!",ToString()));
				Core.Logger.Log($"[Ability triggered] #{ToString()}'s Truant");
				return false;
			}
			if (!turneffects.SkipAccuracyCheck)
				if (this.status==Status.SLEEP) {
					this.StatusCount-=1;
					if (this.StatusCount<=0)
						this.CureStatus();
					else {
						this.ContinueStatus();
						Core.Logger.Log($"[Status] #{ToString()} remained asleep (count: #{this.StatusCount.ToString()})");
						if (!thismove.CanUseWhileAsleep()) { // Snore/Sleep Talk/Outrage
							Core.Logger.Log($"[Move failed] #{ToString()} couldn't use #{thismove.id.ToString()} while asleep");
							return false;
						}
					}
				}
			if (this.status==Status.FROZEN) {
				if (thismove.Flags.Defrost) {
					Core.Logger.Log($"[Move effect triggered] #{ToString()} was defrosted by using #{thismove.id.ToString()}");
					this.CureStatus(false);
					@battle.Display(Game._INTL("{1} melted the ice!",ToString()));
					CheckForm();
				}
				else if (@battle.Random(10)<2 && !turneffects.SkipAccuracyCheck) {
					this.CureStatus();
					CheckForm();
				}
				else if (!thismove.Flags.Defrost) {
					this.ContinueStatus();
					Core.Logger.Log($"[Status] #{ToString()} remained frozen and couldn't move");
					return false;
				}
			}
			if (!turneffects.SkipAccuracyCheck)
				if (@effects.Confusion>0) {
					@effects.Confusion-=1;
					if (@effects.Confusion<=0)
						CureConfusion();
					else {
						ContinueConfusion();
						Core.Logger.Log($"[Status] #{ToString()} remained confused (count: #{@effects.Confusion})");
						if (@battle.Random(2)==0) {
							ConfusionDamage();
							@battle.Display(Game._INTL("It hurt itself in its confusion!")) ;
							Core.Logger.Log($"[Status] #{ToString()} hurt itself in its confusion and couldn't move");
							return false;
						}
					}
				}
			if (@effects.Flinch) {
				@effects.Flinch=false;
				@battle.Display(Game._INTL("{1} flinched and couldn't move!",this.ToString()));
				Core.Logger.Log($"[Lingering effect triggered] #{ToString()} flinched");
				if (this.hasWorkingAbility(Abilities.STEADFAST))
					if (IncreaseStatWithCause(Stats.SPEED,1,this,Game._INTL(this.ability.ToString(TextScripts.Name))))
						Core.Logger.Log($"[Ability triggered] #{ToString()}'s Steadfast");
				return false;
			}
			if (!turneffects.SkipAccuracyCheck) {
				if (@effects.Attract>=0) {
					AnnounceAttract(@battle.battlers[@effects.Attract]);
					if (@battle.Random(2)==0) {
						ContinueAttract();
						Core.Logger.Log($"[Lingering effect triggered] #{ToString()} was infatuated and couldn't move");
						return false;
					}
				}
				if (this.status==Status.PARALYSIS)
					if (@battle.Random(4)==0) {
						ContinueStatus();
						Core.Logger.Log($"[Status] #{ToString()} was fully paralysed and couldn't move");
						return false;
					}
			}
			turneffects.PassedTrying=true;
			return true;
		}
		public void ConfusionDamage() {
			this.damagestate.Reset();
			//PokeBattle_Confusion confmove=new PokeBattle_Confusion(@battle,null);
			IBattleMove confmove=new PokeBattle_Confusion().Initialize(@battle,null);
			confmove.GetEffect(this,this);
			if (this.isFainted()) Faint();
		}
		public void UpdateTargetedMove(IBattleMove thismove,IBattler user) {
			// TODO: Snatch, moves that use other moves
			// TODO: All targeting cases
			// Two-turn attacks, Magic Coat, Future Sight, Counter/MirrorCoat/Bide handled
		}
		public void ProcessMoveAgainstTarget(IBattleMove thismove,IBattler user,IBattler target,int numhits,IEffectsMove turneffects,bool nocheck=false,int[] alltargets=null,bool showanimation=true) {
			int realnumhits=0;
			int totaldamage=0;
			bool destinybond=false;
			for (int i = 0; i < numhits; i++) {
				target.damagestate.Reset();
				// Check success (accuracy/evasion calculation)
				if (!nocheck &&
					!SuccessCheck(thismove,user,target,turneffects,i==0 || thismove.successCheckPerHit())) {
					if (thismove.Effect == Attack.Effects.x069 && realnumhits>0)   // Triple Kick
						break;   // Considered a success if Triple Kick hits at least once
					else if (thismove.Effect == Attack.Effects.x02E)  // Hi Jump Kick, Jump Kick
						if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) {
							Core.Logger.Log($"[Move effect triggered] #{user.ToString()} took crash damage");
							//TODO: Not shown if message is "It doesn't affect XXX..."
							@battle.Display(Game._INTL("{1} kept going and crashed!",user.ToString()));
							int dmg=(int)Math.Floor(user.TotalHP/2d);
							if (dmg>0) {
								if (@battle.scene is IPokeBattle_DebugSceneNoGraphics s0) s0.DamageAnimation(user,0);
								user.ReduceHP(dmg);
							}
							if (user.isFainted()) user.Faint();
						}
					if (thismove.Effect == Attack.Effects.x01C) user.effects.Outrage=0;    // Outrage
					if (thismove.Effect == Attack.Effects.x076) user.effects.Rollout=0;    // Rollout
					if (thismove.Effect == Attack.Effects.x078) user.effects.FuryCutter=0; // Fury Cutter
					if (thismove.Effect == Attack.Effects.x0A2) user.effects.Stockpile=0;  // Spit Up
					return;
				}
				// Add to counters for moves which increase them when used in succession
				if (thismove.Effect == Attack.Effects.x078) // Fury Cutter
					if (user.effects.FuryCutter<4) user.effects.FuryCutter+=1;
				else
					user.effects.FuryCutter=0;
				if (thismove.Effect == Attack.Effects.x12F) { // Echoed Voice
					if (!user.OwnSide.EchoedVoiceUsed &&
						user.OwnSide.EchoedVoiceCounter<5)
						user.OwnSide.EchoedVoiceCounter+=1;
					user.OwnSide.EchoedVoiceUsed=true;
				}
				// Count a hit for Parental Bond if it applies
				if (user.effects.ParentalBond>0) user.effects.ParentalBond-=1;
				// This hit will happen; count it
				realnumhits+=1;
				// Damage calculation and/or main effect
				int damage=thismove.GetEffect(user,target,(byte)i+1,alltargets,showanimation); // Recoil/drain, etc. are applied here
				if (damage>0) totaldamage+=damage;
				// Message and consume for type-weakening berries
				if (target.damagestate.BerryWeakened) {
					@battle.Display(Game._INTL("The {1} weakened the damage to {2}!",
						Game._INTL(target.Item.ToString(TextScripts.Name)),target.ToString(true)));
					target.ConsumeItem();
				}
				// Illusion
				if (target.effects.Illusion.IsNotNullOrNone() && target.hasWorkingAbility(Abilities.ILLUSION) &&
					damage>0 && !target.damagestate.Substitute) {
					Core.Logger.Log($"[Ability triggered] #{target.ToString()}'s Illusion ended");
					target.effects.Illusion=null;
					if (@battle.scene is IPokeBattle_Scene s0) s0.ChangePokemon(target,target.pokemon);
					@battle.Display(Game._INTL("{1}'s {2} wore off!",target.ToString(),
						Game._INTL(target.Ability.ToString(TextScripts.Name))));
				}
				if (user.isFainted()) user.Faint(); // no return
				if (numhits>1 && target.damagestate.CalcDamage<=0) return;
				@battle.JudgeCheckpoint(user,thismove);
				// Additional effect
				if (target.damagestate.CalcDamage>0 &&
					!user.hasWorkingAbility(Abilities.SHEER_FORCE) &&
					(user.hasMoldBreaker() || !target.hasWorkingAbility(Abilities.SHIELD_DUST))) {
					int addleffect=thismove.AddlEffect;
					if ((user.hasWorkingAbility(Abilities.SERENE_GRACE) ||
						user.OwnSide.Rainbow>0) &&
						thismove.Effect != Attack.Effects.x0C6) addleffect*=2; // Secret Power
					if (Core.DEBUG && Input.press((int)PokemonUnity.Interface.InputKeys.DEBUG)) addleffect=100;
					if (@battle.Random(100)<addleffect) {
						Core.Logger.Log($"[Move effect triggered] #{thismove.id.ToString()}'s added effect");
						thismove.AdditionalEffect(user,target);
					}
				}
				// Ability effects
				EffectsOnDealingDamage(thismove,user,target,damage);
				// Grudge
				if (!user.isFainted() && target.isFainted())
				if (target.effects.Grudge && target.IsOpposing(user.Index)) {
					thismove.PP=0;
					@battle.Display(Game._INTL("{1}'s {2} lost all its PP due to the grudge!",
						user.ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Grudge made #{thismove.id.ToString()} lose all its PP");
				}
				if (target.isFainted())
				destinybond=destinybond || target.effects.DestinyBond;
				if (user.isFainted()) user.Faint(); // no return
				if (user.isFainted()) break;
				if (target.isFainted()) break;
				// Make the target flinch
				if (target.damagestate.CalcDamage>0 && !target.damagestate.Substitute)
				if (user.hasMoldBreaker() || !target.hasWorkingAbility(Abilities.SHIELD_DUST)) {
					bool canflinch=false;
					if (user.hasWorkingItem(Items.KINGS_ROCK) || user.hasWorkingItem(Items.RAZOR_FANG)
						&& thismove.canKingsRock())
					canflinch=true;
					if (user.hasWorkingAbility(Abilities.STENCH) &&
						thismove.Effect != Attack.Effects.x114 &&		// Thunder Fang
						thismove.Effect != Attack.Effects.x112 &&		// Fire Fang
						thismove.Effect != Attack.Effects.x113 &&		// Ice Fang
						thismove.Effect != Attack.Effects.x097 &&		// Stomp
						thismove.Effect != Attack.Effects.x05D &&		// Snore
						thismove.Effect != Attack.Effects.x09F &&		// Fake Out
						thismove.Effect != Attack.Effects.x093 &&		// Twister
						thismove.Effect != Attack.Effects.x04C &&		// Sky Attack
						//thismove.Effect != Attack.Effects.   &&		// flinch-inducing moves
						Kernal.MoveMetaData[thismove.id].FlinchChance == 0)	// flinch-inducing moves
						canflinch=true;
					if (canflinch && @battle.Random(10)==0) {
						Core.Logger.Log($"[Item/ability triggered] #{user.ToString()}'s King's Rock/Razor Fang or Stench");
						if (target is IBattlerEffect t0) t0.Flinch(user);
					}
				}
				if (target.damagestate.CalcDamage>0 && !target.isFainted() && target is IBattlerEffect t) {
					// Defrost
					if (target.Status==Status.FROZEN &&
						(thismove.GetType(thismove.Type,user,target) == Types.FIRE ||
						(Core.USENEWBATTLEMECHANICS && thismove.id == Moves.SCALD)))
						t.CureStatus();
					// Rage
					if (target.effects.Rage && target.IsOpposing(user.Index))
						// TODO: Apparently triggers if opposing Pokémon uses Future Sight after a Future Sight attack
						if (t.IncreaseStatWithCause(Stats.ATTACK,1,target,"",true,false)) {
							Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Rage");
							@battle.Display(Game._INTL("{1}'s rage is building!",target.ToString()));
						}
				}
				if (target.isFainted()) target.Faint();		// no return
				if (user.isFainted()) user.Faint();			// no return
				if (user.isFainted() || target.isFainted()) break;
				// Berry check (maybe just called by ability effect, since only necessary Berries are checked)
				for (int j = 0; j < battle.battlers.Length; j++)
					@battle.battlers[j].BerryCureCheck();
				if (user.isFainted() || target.isFainted()) break;
				target.UpdateTargetedMove(thismove,user);
				if (target.damagestate.CalcDamage<=0) break;
			}
			if (totaldamage>0) turneffects.TotalDamage+=totaldamage;
			// Battle Arena only - attack is successful
			@battle.successStates[user.Index].UseState=null;
			@battle.successStates[user.Index].TypeMod=target.damagestate.TypeMod;
			// Type effectiveness
			if (numhits>1) {
				if (target.damagestate.TypeMod>8)
				if (alltargets.Length>1)
					@battle.Display(Game._INTL("It's super effective on {1}!",target.ToString(true)));
				else
					@battle.Display(Game._INTL("It's super effective!"));
				else if (target.damagestate.TypeMod>=1 && target.damagestate.TypeMod<8)
				if (alltargets.Length>1)
					@battle.Display(Game._INTL("It's not very effective on {1}...",target.ToString(true)));
				else
					@battle.Display(Game._INTL("It's not very effective..."));
				if (realnumhits==1)
					@battle.Display(Game._INTL("Hit {1} time!",realnumhits.ToString()));
				else
					@battle.Display(Game._INTL("Hit {1} times!",realnumhits.ToString()));
			}
			Core.Logger.Log("Move did #{0} hit(s), total damage=#{1}",numhits,turneffects.TotalDamage);
			// Faint if 0 HP
			if (target.isFainted()) target.Faint();	// no return
			if (user.isFainted()) user.Faint();		// no return
			thismove.EffectAfterHit(user,target,turneffects);  //ToDo: CONFIRM IF `Faint()` IS ASSIGNING `isFaint()` AS TRUE!~
			if (target.isFainted()) target.Faint();	// no return
			if (user.isFainted()) user.Faint();		// no return
			// Destiny Bond
			if (!user.isFainted() && target.isFainted())
				if (destinybond && target.IsOpposing(user.Index)) {
					Core.Logger.Log($"[Lingering effect triggered] #{target.ToString()}'s Destiny Bond");
					@battle.Display(Game._INTL("{1} took its attacker down with it!",target.ToString()));
					user.ReduceHP(user.HP);
					user.Faint(); // no return
					@battle.JudgeCheckpoint(user);
				}
			EffectsAfterHit(user,target,thismove,turneffects);
			// Berry check
			for (int j = 0; j < battle.battlers.Length; j++)
				@battle.battlers[j].BerryCureCheck();
			target.UpdateTargetedMove(thismove,user);
		}

		/// <summary>
		/// </summary>
		/// <param name="moveid">"Use move"</param>
		/// <param name="index">Index of move to be used in user's moveset</param>
		/// <param name="target">Target (-1 means no target yet)</param>
		public void UseMoveSimple(Moves moveid,int index=-1,int target=-1) {
			//IBattleChoice choice= new Choice();
			//choice[0]=1;           // "Use move"
			//choice.Action=index;   // Index of move to be used in user's moveset
			//choice.Move=PokeBattle_Move.FromMove(@battle,new Attack.Move(moveid)); // PokeBattle_Move object of the move
			//choice.Move.PP=-1;
			//choice.Target=target;  // Target (-1 means no target yet)
			IBattleMove move = Combat.Move.FromMove(@battle, new Attack.Move(moveid));
			move.PP = -1;
			IBattleChoice choice = new Choice(action: ChoiceAction.UseMove, moveIndex: index, move: move, target: target);
			if (index>=0)
				//@battle.choices[@Index].Index=index;
				@battle.choices[@Index]=new Choice(action: @battle.choices[@Index].Action, moveIndex: index, move: @battle.choices[@Index].Move, target: @battle.choices[@Index].Target);
			//Core.Logger.Log($"#{ToString()} used simple move #{choice.Move.id.ToString()}");
			Core.Logger.Log("#{0} used simple move #{1}",ToString(),choice.Move.id.ToString());
			UseMove(choice,true);
			return;
		}

		public void UseMove(IBattleChoice choice,bool specialusage=false) {
			// TODO: lastMoveUsed is not to be updated on nested calls
			// Note: user.lastMoveUsedType IS to be updated on nested calls; is used for Conversion 2
			IEffectsMove turneffects= new Effects.Move(); //ToDo: ParentalBond is supposed to deal less damage on subsequent hits, use variable for implementation
			turneffects.SpecialUsage=specialusage;
			turneffects.SkipAccuracyCheck=specialusage;
			turneffects.PassedTrying=false;
			turneffects.TotalDamage=0;
			// Start using the move
			BeginTurn(choice);
			// Force the use of certain moves if they're already being used
			if (@effects.TwoTurnAttack>0 ||
				@effects.HyperBeam>0 ||
				@effects.Outrage>0 ||
				@effects.Rollout>0 ||
				@effects.Uproar>0 ||
				@effects.Bide>0) {
				//choice.Move=Combat.Move.FromMove(@battle,new Attack.Move(@currentMove));
				choice=new Choice(action: choice.Action, moveIndex: choice.Index, move: Combat.Move.FromMove(@battle, new Attack.Move(@currentMove)));
				turneffects.SpecialUsage=true;
				Core.Logger.Log($"Continuing multi-turn move #{choice.Move.id.ToString()}");
			}
			else if (@effects.Encore>0) {
				if (@battle.CanShowCommands(@Index) &&
					@battle.CanChooseMove(@Index,@effects.EncoreIndex,false)) {
					if (choice.Index!=@effects.EncoreIndex) { // Was Encored mid-round
						//choice.Index=@effects.EncoreIndex;
						//choice.Move=@moves[@effects.EncoreIndex];
						//choice.Target=-1; // No target chosen
						choice = new Choice(action: ChoiceAction.UseMove, moveIndex: @effects.EncoreIndex, move: @moves[@effects.EncoreIndex]);
					}
					Core.Logger.Log($"Using Encored move #{choice.Move.id.ToString()}");
				}
			}
			IBattleMove thismove=choice.Move;
			if (!thismove.IsNotNullOrNone() || thismove.id<=0) return; //if move was not chosen
			if (!turneffects.SpecialUsage) {
				// TODO: Quick Claw message
			}
			// Stance Change
			if (hasWorkingAbility(Abilities.STANCE_CHANGE) && Species == Pokemons.AEGISLASH &&
				!@effects.Transform)
				if (thismove.IsDamaging() && this.form!=1) {
					this.form=1;
					Update(true);
					if (@battle.scene is IPokeBattle_Scene s0) s0.ChangePokemon(this,@pokemon);
					@battle.Display(Game._INTL("{1} changed to Blade Forme!",ToString()));
					Core.Logger.Log($"[Form changed] #{ToString()} changed to Blade Forme");
				}
				else if (thismove.id == Moves.KINGS_SHIELD && this.form!=0) {
					this.form=0;
					Update(true);
					if (@battle.scene is IPokeBattle_Scene s0) s0.ChangePokemon(this,@pokemon);
					@battle.Display(Game._INTL("{1} changed to Shield Forme!",ToString()));
					Core.Logger.Log($"[Form changed] #{ToString()} changed to Shield Forme");
				}
			// Record that user has used a move this round (or at least tried to)
			this.lastRoundMoved=@battle.turncount;
			// Try to use the move
			if (!TryUseMove(choice,thismove,turneffects)) {
				this.lastMoveUsed=Moves.NONE;
				//this.lastMoveUsedType=Types.NONE;
				if (!turneffects.SpecialUsage) {
					if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=Moves.NONE;
					this.lastRegularMoveUsed=Moves.NONE;
				}
				CancelMoves();
				@battle.GainEXP();
				EndTurn(choice);
				@battle.Judge();       //@battle.Switch
				return;
			}
			if (!turneffects.SpecialUsage)
				if (!ReducePP(thismove)) {
					@battle.Display(Game._INTL("{1} used\r\n{2}!",ToString(),Game._INTL(thismove.id.ToString(TextScripts.Name))));
					@battle.Display(Game._INTL("But there was no PP left for the move!"));
					this.lastMoveUsed=Moves.NONE;
					//this.lastMoveUsedType=Types.NONE;
					if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=Moves.NONE;
					this.lastRegularMoveUsed=Moves.NONE;
					EndTurn(choice);
					@battle.Judge();         //@battle.Switch
					Core.Logger.Log($"[Move failed] #{thismove.id.ToString()} has no PP left");
					return;
				}
			// Remember that user chose a two-turn move
			if (thismove.TwoTurnAttack(this)) {
				// Beginning use of two-turn attack
				@effects.TwoTurnAttack=thismove.id;
				@currentMove=thismove.id;
			}
			else
				@effects.TwoTurnAttack=0; // Cancel use of two-turn attack
			// Charge up Metronome Item
			if (this.lastMoveUsed==thismove.id)
				this.effects.Metronome+=1;
			else
				this.effects.Metronome=0;
			// "X used Y!" message
			int zxy = thismove.DisplayUseMessage(this); //switch (thismove.DisplayUseMessage(this)) {
				if (zxy == 2) //case 2:   // Continuing Bide
					return;
				else if (zxy == 1) { //case 1:   // Starting Bide
					this.lastMoveUsed=thismove.id;
					//this.lastMoveUsedType=thismove.Type(thismove.Type,this,null);
					if (!turneffects.SpecialUsage) {
						if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=thismove.id;
						this.lastRegularMoveUsed=thismove.id;
					}
					@battle.lastMoveUsed=thismove.id;
					@battle.lastMoveUser=this.Index;
					@battle.successStates[this.Index].UseState=null;
					@battle.successStates[this.Index].TypeMod=8;
					return;
				} else if (zxy == -1) { //case -1:   // Was hurt while readying Focus Punch, fails use
					this.lastMoveUsed=thismove.id;
					//this.lastMoveUsedType=thismove.Type(thismove.Type,this,null);
					if (!turneffects.SpecialUsage) {
						if (this.effects.TwoTurnAttack==0) this.lastMoveUsedSketch=thismove.id;
						this.lastRegularMoveUsed=thismove.id;
					}
					@battle.lastMoveUsed=thismove.id;
					@battle.lastMoveUser=this.Index;
					@battle.successStates[this.Index].UseState =null;// somehow treated as a success
					@battle.successStates[this.Index].TypeMod=8;
					Core.Logger.Log($"[Move failed] #{ToString()} was hurt while readying Focus Punch");
					return;
			}
			// Find the user and target(s)
			IList<IBattler> targets= new List<IBattler>();
			IBattler user=FindUser(choice,targets);
			// Battle Arena only - assume failure
			@battle.successStates[user.Index].UseState=true;
			@battle.successStates[user.Index].TypeMod=8;
			// Check whether Selfdestruct works
			if (!thismove.OnStartUse(user)) { // Selfdestruct, Natural Gift, Beat Up can return false here
				//Core.Logger.Log(string.Format("[Move failed] Failed OnStartUse (function code %02X)",thismove.Effect));
				Core.Logger.Log(string.Format("[Move failed] Failed `OnStartUse` (Move Effect Code: {0})",thismove.Effect.ToString()));
				user.lastMoveUsed=thismove.id;
				//user.lastMoveUsedType=thismove.Type(thismove.Type,user,null);
				if (!turneffects.SpecialUsage) {
					if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
					user.lastRegularMoveUsed=thismove.id;
				}
				@battle.lastMoveUsed=thismove.id;
				@battle.lastMoveUser=user.Index;
				return;
			}
			// Primordial Sea, Desolate Land
			if (thismove.IsDamaging())
				switch (@battle.Weather) { //ToDo: Make this an if-statement?
					case Weather.HEAVYRAIN:
						if (thismove.GetType(thismove.Type,user,null) == Types.FIRE) {
							Core.Logger.Log($"[Move failed] Primordial Sea's rain cancelled the Fire-type #{thismove.id.ToString()}");
							@battle.Display(Game._INTL("The Fire-type attack fizzled out in the heavy rain!"));
							user.lastMoveUsed=thismove.id;
							//user.lastMoveUsedType=thismove.Type(thismove.Type,user,null);
							if (!turneffects.SpecialUsage) {
								if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
								user.lastRegularMoveUsed=thismove.id;
							}
							@battle.lastMoveUsed=thismove.id;
							@battle.lastMoveUser=user.Index;
							return;
						}
						break;
					case Weather.HARSHSUN:
						if (thismove.GetType(thismove.Type,user,null) == Types.WATER) {
							Core.Logger.Log($"[Move failed] Desolate Land's sun cancelled the Water-type #{thismove.id.ToString()}");
							@battle.Display(Game._INTL("The Water-type attack evaporated in the harsh sunlight!"));
							user.lastMoveUsed=thismove.id;
							//user.lastMoveUsedType=thismove.Type(thismove.Type,user,null);
							if (!turneffects.SpecialUsage) {
								if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
								user.lastRegularMoveUsed=thismove.id;
							}
							@battle.lastMoveUsed=thismove.id;
							@battle.lastMoveUser=user.Index;
							return;
						}
						break;
				}
			// Powder
			if (user.effects.Powder && thismove.GetType(thismove.Type,user,null) == Types.FIRE) {
				Core.Logger.Log($"[Lingering effect triggered] #{ToString()}'s Powder cancelled the Fire move");
				@battle.CommonAnimation("Powder",user,null);
				@battle.Display(Game._INTL("When the flame touched the powder on the Pokémon, it exploded!"));
				if (!user.hasWorkingAbility(Abilities.MAGIC_GUARD)) user.ReduceHP(1+(int)Math.Floor(user.TotalHP/4d));
				user.lastMoveUsed=thismove.id;
				//user.lastMoveUsedType=thismove.Type(thismove.Type,user,null);
				if (!turneffects.SpecialUsage) {
					if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
					user.lastRegularMoveUsed=thismove.id;
				}
				@battle.lastMoveUsed=thismove.id;
				@battle.lastMoveUser=user.Index;
				if (user.isFainted()) user.Faint();
				EndTurn(choice);
				return;
			}
			// Protean
			if (user.hasWorkingAbility(Abilities.PROTEAN) &&
				thismove.Effect != Attack.Effects.x00A &&   // Mirror Move
				thismove.Effect != Attack.Effects.x0F3 &&   // Copycat
				thismove.Effect != Attack.Effects.x073 &&   // Me First
				thismove.Effect != Attack.Effects.x0AE &&   // Nature Power
				thismove.Effect != Attack.Effects.x062 &&   // Sleep Talk
				thismove.Effect != Attack.Effects.x0B5 &&   // Assist
				thismove.Effect != Attack.Effects.x054) {   // Metronome
				Types movetype=thismove.GetType(thismove.Type,user,null);
				if (!user.HasType(movetype)) {
					string typename=Game._INTL(movetype.ToString(TextScripts.Name));
					Core.Logger.Log($"[Ability triggered] #{ToString()}'s Protean made it #{typename}-type");
					user.Type1=movetype;
					user.Type2=movetype;
					user.effects.Type3=Types.NONE;
					@battle.Display(Game._INTL("{1} transformed into the {2} type!",user.ToString(),typename));
				}
			}
			// Try to use move against user if there aren't any targets
			if (targets.Count==0) {
				user=ChangeUser(thismove,user);
				if(thismove.Target==Attack.Targets.SELECTED_POKEMON ||		//.Targets==Attack.Target.SingleNonUser ||
					thismove.Target==Attack.Targets.RANDOM_OPPONENT ||		//.Targets==Attack.Target.RandomOpposing ||
					thismove.Target==Attack.Targets.ALL_OPPONENTS ||		//.Targets==Attack.Target.AllOpposing ||
					thismove.Target==Attack.Targets.ALL_OTHER_POKEMON ||	//.Targets==Attack.Target.AllNonUsers ||
					thismove.Target==Attack.Targets.ALLY ||				//.Targets==Attack.TargetPartner ||
					thismove.Target==Attack.Targets.USER_OR_ALLY ||		//.Targets==Attack.Target.UserOrPartner ||
					//thismove.Target==Attack.Targets.SingleOpposing ||	//.Targets==Attack.Target.SingleOpposing ||
					thismove.Target==Attack.Targets.OPPONENTS_FIELD)		//.Targets==Attack.Target.OppositeOpposing)
					@battle.Display(Game._INTL("But there was no target..."));
				else
					//Core.Logger.logonerr(thismove.Effect(user,null));
					try{ thismove.GetEffect(user, null); } catch { Core.Logger.Log(""); }
			}
			else {
				// We have targets
				bool showanimation=true;
				IList<int> alltargets= new List<int>();
				for (int x = 0; x < targets.Count; x++)
					if (!targets.Contains(targets[x])) alltargets.Add(targets[x].Index);
				// For each target in turn
				int i=0;
				do {
					// Get next target
					IBattler[] userandtarget=new IBattler[] { user, targets[i] };
					bool success=ChangeTarget(thismove,userandtarget,targets.ToArray());
					user=userandtarget[0];
					IBattler target=userandtarget[1];
					if (battle.doublebattle && i==0 && thismove.Target==Attack.Targets.ALL_OPPONENTS) //thismove.Targets==Attack.Target.AllOpposing
						// Add target's partner to list of targets
						AddTarget(targets,target.Partner);
					// If couldn't get the next target
					if (!success) {
						i+=1;
						continue;
					}
					// Get the number of hits
					int numhits=thismove.NumHits(user);
					// Reset damage state, set Focus Band/Focus Sash to available
					target.damagestate.Reset();
					// Use move against the current target
					ProcessMoveAgainstTarget(thismove,user,target,numhits,turneffects,false,alltargets.ToArray(),showanimation);
					showanimation=false;
					i+=1;
				} while (i<targets.Count); //break if i>=targets.length
			}
			List<int> switched= new List<int>();
			// Pokémon switching caused by Roar, Whirlwind, Circle Throw, Dragon Tail, Red Card
			if (!user.isFainted()) {
				switched= new List<int>();
				for (int i = 0; i < battle.battlers.Length; i++) {
					if (@battle.battlers[i].effects.Roar) {
						@battle.battlers[i].effects.Roar=false;
						@battle.battlers[i].effects.Uturn=false;
						if (@battle.battlers[i].isFainted()) continue;
						if (!@battle.CanSwitch(i,-1,false)) continue;
						List<int> choices= new List<int>();
						PokemonEssentials.Interface.PokeBattle.IPokemon[] party=@battle.Party(i);
						for (int j = 0; j< party.Length; j++)
							if (@battle.CanSwitchLax(i,j,false)) choices.Add(j);
						if (choices.Count>0) {
							int newpoke=choices[@battle.Random(choices.Count)];
							int newpokename=newpoke;
							if (party[newpoke].Ability == Abilities.ILLUSION)
								newpokename=@battle.GetLastPokeInTeam(i);
							switched.Add(i);
							@battle.battlers[i].ResetForm();
							@battle.RecallAndReplace(i,newpoke,newpokename,false,user.hasMoldBreaker());
							@battle.Display(Game._INTL("{1} was dragged out!",@battle.battlers[i].ToString()));
							//@battle.choices[i]=[0,0,null,-1];		// Replacement Pokémon does nothing this round
							@battle.choices[i]=new Choice();		// Replacement Pokémon does nothing this round
						}
					}
				}
				foreach(IBattler i in @battle.Priority()) {
					if (!switched.Contains(i.Index)) continue;
					i.AbilitiesOnSwitchIn(true);
				}
			}
			// Pokémon switching caused by U-Turn, Volt Switch, Eject Button
			switched= new List<int>();
			for (int i = 0; i < battle.battlers.Length; i++)
				if (@battle.battlers[i].effects.Uturn) {
					@battle.battlers[i].effects.Uturn=false;
					@battle.battlers[i].effects.Roar=false;
					if (!@battle.battlers[i].isFainted() && @battle.CanChooseNonActive(i) &&
						!@battle.AllFainted(@battle.OpposingParty(i))) {
						// TODO: Pursuit should go here, and negate this effect if it KO's attacker
						@battle.Display(Game._INTL("{1} went back to {2}!",@battle.battlers[i].ToString(),@battle.GetOwner(i).name));
						int newpoke=0;
						newpoke=@battle.SwitchInBetween(i,true,false);
						int newpokename=newpoke;
						if (@battle.Party(i)[newpoke].Ability == Abilities.ILLUSION)
							newpokename=@battle.GetLastPokeInTeam(i);
						switched.Add(i);
						@battle.battlers[i].ResetForm();
						@battle.RecallAndReplace(i,newpoke,newpokename,@battle.battlers[i].effects.BatonPass);
						//@battle.choices[i]=[0,0,null,-1];		// Replacement Pokémon does nothing this round
						@battle.choices[i]= new Choice();		// Replacement Pokémon does nothing this round
					}
				}
			foreach(IBattler i in @battle.Priority()) {
				if (!switched.Contains(i.Index)) continue;
				i.AbilitiesOnSwitchIn(true);
			}
			// Baton Pass
			if (user.effects.BatonPass) {
				user.effects.BatonPass=false;
				if (!user.isFainted() && @battle.CanChooseNonActive(user.Index) &&
					!@battle.AllFainted(@battle.Party(user.Index))) {
					int newpoke=0;
					newpoke=@battle.SwitchInBetween(user.Index,true,false);
					int newpokename=newpoke;
					if (@battle.Party(user.Index)[newpoke].Ability == Abilities.ILLUSION)
						newpokename=@battle.GetLastPokeInTeam(user.Index);
					user.ResetForm();
					@battle.RecallAndReplace(user.Index,newpoke,newpokename,true);
					//@battle.choices[user.Index]=new Battle.Choice(0,0,null,-1);	// Replacement Pokémon does nothing this round
					@battle.choices[user.Index]=new Choice(ChoiceAction.NoAction);	// Replacement Pokémon does nothing this round
					user.AbilitiesOnSwitchIn(true);
				}
			}
			// Record move as having been used
			user.lastMoveUsed=thismove.id;
			//user.lastMoveUsedType=thismove.Type(thismove.Type,user,null);
			if (!turneffects.SpecialUsage) {
				if (user.effects.TwoTurnAttack==0) user.lastMoveUsedSketch=thismove.id;
				user.lastRegularMoveUsed=thismove.id;
				if (!user.movesUsed.Contains(thismove.id)) user.movesUsed.Add(thismove.id); // For Last Resort
			}
			@battle.lastMoveUsed=thismove.id;
			@battle.lastMoveUser=user.Index;
			// Gain Exp
			@battle.GainEXP();
			// Battle Arena only - update skills
			for (int i = 0; i < battle.battlers.Length; i++)
				@battle.successStates[i].UpdateSkill();
			// End of move usage
			EndTurn(choice);
			@battle.Judge();		//@battle.Switch();
			return;
		}
		public void CancelMoves() {
			// If failed TryUseMove or have already used Pursuit to chase a switching foe
			// Cancel multi-turn attacks (note: Hyper Beam effect is not canceled here)
			if (@effects.TwoTurnAttack>0) @effects.TwoTurnAttack=0;
			@effects.Outrage=0;
			@effects.Rollout=0;
			@effects.Uproar=0;
			@effects.Bide=0;
			@currentMove=0;
			// Reset counters for moves which increase them when used in succession
			@effects.FuryCutter=0;
			Core.Logger.Log("Cancelled using the move");
		}
		#endregion

		#region Turn processing
		public void BeginTurn(IBattleChoice choice) {
			// Cancel some lingering effects which only apply until the user next moves
			@effects.DestinyBond=false;
			@effects.Grudge=false;
			// Reset Parental Bond's count
			@effects.ParentalBond=0;
			// Encore's effect ends if the encored move is no longer available
			if (@effects.Encore>0 &&
				@moves[@effects.EncoreIndex].id!=@effects.EncoreMove) {
				Core.Logger.Log("Resetting Encore effect");
				@effects.Encore=0;
				@effects.EncoreIndex=0;
				@effects.EncoreMove=0;
			}
			// Wake up in an uproar
			if (this.status==Status.SLEEP && !this.hasWorkingAbility(Abilities.SOUNDPROOF))
				for (int i = 0; i < 4; i++)
					if (@battle.battlers[i].effects.Uproar>0) {
						CureStatus(false);
						@battle.Display(Game._INTL("{1} woke up in the uproar!",ToString()));
					}
		}
		//protected void _EndTurn(IBattleChoice choice) {
		void IBattler.EndTurn(IBattleChoice choice) {
			// True end(?)
			if (@effects.ChoiceBand<0 && @lastMoveUsed>=0 && !this.isFainted() &&
				(this.hasWorkingItem(Items.CHOICE_BAND) ||
				this.hasWorkingItem(Items.CHOICE_SPECS) ||
				this.hasWorkingItem(Items.CHOICE_SCARF)))
				@effects.ChoiceBand=@lastMoveUsed;
			@battle.PrimordialWeather();
			for (int i = 0; i < battle.battlers.Length; i++)
				@battle.battlers[i].BerryCureCheck();
			for (int i = 0; i < battle.battlers.Length; i++)
				@battle.battlers[i].AbilityCureCheck();
			for (int i = 0; i < battle.battlers.Length; i++)
				@battle.battlers[i].AbilitiesOnSwitchIn(false);
			for (int i = 0; i < battle.battlers.Length; i++)
				@battle.battlers[i].CheckForm();
		}
		public bool ProcessTurn(IBattleChoice choice) {
			// Can't use a move if fainted
			if (this.isFainted()) return false;
			// Wild roaming Pokémon always flee if possible
			if (@battle.opponent.Length != 0 && @battle.IsOpposing(this.Index) &&
				@battle.rules["alwaysflee"] && @battle.CanRun(this.Index)) {
				BeginTurn(choice);
				@battle.Display(Game._INTL("{1} fled!",this.ToString()));
				@battle.decision=BattleResults.FORFEIT;
				EndTurn(choice);
				Core.Logger.Log($"[Escape] #{ToString()} fled");
				return true;
			}
			// If this battler's action for this round wasn't "use a move"
			if (choice.Action!=ChoiceAction.UseMove) {
				// Clean up effects that end at battler's turn
				BeginTurn(choice);
				EndTurn(choice);
				return false;
			}
			// Turn is skipped if Pursuit was used during switch
			if (@effects.Pursuit) {
				@effects.Pursuit=false;
				CancelMoves();
				EndTurn(choice);
				@battle.Judge();		//@battle.Switch
				return false;
			}
			// Use the move
			//   @battle.DisplayPaused("Before: [#{@lastMoveUsedSketch},#{@lastMoveUsed}]"); //Log instead?
			Core.Logger.Log("#{0} used #{1}",ToString(),choice.Move.id.ToString());
			//try{ UseMove(choice, choice.Move == @battle.struggle); } catch (Exception e) { Core.Logger.Log(e.Message); } //Core.Logger.Log(e.StackTrace);
			UseMove(choice, choice.Move == @battle.struggle);
			//   @battle.DisplayPaused("After: [#{@lastMoveUsedSketch},#{@lastMoveUsed}]");
			return true;
		}
		#endregion

		#region Explicit Interface Implementation
		bool IBattler.inHyperMode { get { return inHyperMode(); } }
		bool IBattler.isShadow { get { return isShadow(); } }
		int IBattler.displayGender { get { if (Gender == true) return 1; else if (Gender == false) return 0; else return -1; } }
		bool IBattler.owned { get { return IsOwned; } }
		int IBattler.Speed { get { return SPE; } } //Should be referenced about 12 times throughout the project...

		void IBattler.InitDummyPokemon(IPokemon pkmn, int pkmnIndex)
		{
			//throw new NotImplementedException();
		}

		bool IBattler.HasMove(Moves id)
		{
			throw new NotImplementedException();
		}

		bool IBattler.HasMoveFunction(int code)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Explicit Operators
		public static bool operator ==(Pokemon x, Pokemon y)
		{
			if ((object)x == null && (object)y == null) return true;
			if ((object)x == null || (object)y == null) return false;
			return ((x.pokemon.PersonalId == y.pokemon.PersonalId) && ((x.pokemon as PokemonUnity.Monster.Pokemon).TrainerId == (y.pokemon as PokemonUnity.Monster.Pokemon).TrainerId) && ((x.pokemon as PokemonUnity.Monster.Pokemon).OT == (y.pokemon as PokemonUnity.Monster.Pokemon).OT)) & (x.pokemon.Name == y.pokemon.Name); //ToDo: If Gender is different, are pokemons the same? Check Date/Age of Pokemon?
		}
		public static bool operator !=(Pokemon x, Pokemon y)
		{
			if ((object)x == null && (object)y == null) return false;
			if ((object)x == null || (object)y == null) return true;
			return ((x.pokemon.PersonalId != y.pokemon.PersonalId) || ((x.pokemon as PokemonUnity.Monster.Pokemon).TrainerId != (y.pokemon as PokemonUnity.Monster.Pokemon).TrainerId) || ((x.pokemon as PokemonUnity.Monster.Pokemon).OT != (y.pokemon as PokemonUnity.Monster.Pokemon).OT)) | (x.pokemon.Name == y.pokemon.Name);
		}
		public bool Equals(Pokemon obj)
		{
			if (obj == null) return false;
			return this == obj;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj.GetType() == typeof(IBattler) || obj.GetType() == typeof(Pokemon))
				return Equals(obj as Pokemon);
			if (obj.GetType() == typeof(IPokemon) || obj.GetType() == typeof(Monster.Pokemon))
				return pokemon.Equals(obj as Monster.Pokemon);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return pokemon.PersonalId.GetHashCode();
		}
		bool IEquatable<IBattler>.Equals(IBattler other)
		{
			return Equals(obj: (object)other);
		}
		bool IEquatable<Pokemon>.Equals(Pokemon other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<IBattler>.Equals(IBattler x, IBattler y)
		{
			return x == y;
		}
		bool IEqualityComparer<Pokemon>.Equals(Pokemon x, Pokemon y)
		{
			return x == y;
		}
		int IEqualityComparer<IBattler>.GetHashCode(IBattler obj)
		{
			return obj.GetHashCode();
		}
		int IEqualityComparer<Pokemon>.GetHashCode(Pokemon obj)
		{
			return obj.GetHashCode();
		}
		object ICloneable.Clone()
		{
			return MemberwiseClone();
		}

		public static IBattler[] GetBattlers(PokemonEssentials.Interface.PokeBattle.IPokemon[] input, IBattle btl)
		{
			IBattler[] battlers = new IBattler[input.Length];
			for (int i = 0; i < input.Length; i++)
			{
				//battlers[i] = (IBattler)input[i];
				battlers[i] = (IBattler)new Pokemon(btl, (sbyte)i);
				battlers[i].InitPokemon(input[i], (sbyte)i);
			}
			return battlers;
		}

		//public static implicit operator PokemonEssentials.Interface.PokeBattle.IPokemon(PokemonUnity.Combat.Pokemon pkmn)
		//{
		//	if (pkmn == null) return new PokemonEssentials.Interface.PokeBattle.IPokemon();
		//	PokemonEssentials.Interface.PokeBattle.IPokemon pokemon = pkmn.pokemon;
		//	//if (pokemon == null) return null;
		//	if ((Pokemons)pokemon.Species == Pokemons.NONE) return new PokemonEssentials.Interface.PokeBattle.IPokemon(Pokemons.NONE);
		//	Ribbon[] ribbons = pokemon.Ribbons; //new Ribbon[pokemon.Ribbons.Length];
		//	//for (int i = 0; i < ribbons.Length; i++)
		//	//{
		//	//	ribbons[i] = (Ribbon)pokemon.Ribbons[i];
		//	//}
		//
		//	Attack.Move[] moves = new Attack.Move[pokemon.moves.Length];
		//	for (int i = 0; i < moves.Length; i++)
		//	{
		//		moves[i] = pokemon.moves[i];
		//	}
		//
		//	PokemonEssentials.Interface.PokeBattle.IPokemon normalPokemon = //pokemon;
		//		new PokemonEssentials.Interface.PokeBattle.IPokemon
		//		(
		//			(Pokemons)pokemon.Species, pokemon.OT,
		//			pokemon.Name, pokemon.FormId, (Abilities)pokemon.Ability,
		//			(Monster.Natures)pokemon.Nature, pokemon.IsShiny, pokemon.Gender,
		//			pokemon.Pokerus, pokemon.HeartGuageSize, /*pokemon.IsHyperMode,*/ pokemon.ShadowLevel,
		//			pokemon.HP, (Items)pokemon.Item, pokemon.IV, pokemon.EV,
		//			pokemon.ObtainLevel, /*pokemon.CurrentLevel,*/ pokemon.exp,
		//			pokemon.Happiness, (Status)pokemon.Status, pokemon.StatusCount,
		//			pokemon.EggSteps, (Items)pokemon.ballUsed, pokemon.Mail, moves,
		//			pokemon.MoveArchive, ribbons, pokemon.Markings, pokemon.PersonalId,
		//			(PokemonEssentials.Interface.PokeBattle.IPokemon.ObtainedMethod)pokemon.ObtainedMode,
		//			pokemon.TimeReceived, pokemon.TimeEggHatched
		//		);
		//	return normalPokemon;
		//}

		//public static implicit operator PokemonUnity.Combat.Pokemon(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		//{
		//	Pokemon pkmn = new Pokemon();
		//	if (pokemon == null) return pkmn;
		//
		//	if (pokemon != null && pokemon.Species != Pokemons.NONE)
		//	{
		//		pkmn.PersonalId = pokemon.PersonalId;
		//		//PublicId in pokemon is null, so Pokemon returns null
		//		//pkmn.PublicId				= pokemon.TrainerId;
		//
		//		if (!pokemon.OT.Equals((object)null))
		//		{
		//			pkmn.TrainerName = pokemon.OT.Name;
		//			pkmn.TrainerIsMale = pokemon.OT.Gender;
		//			pkmn.TrainerTrainerId = pokemon.OT.TrainerID;
		//			pkmn.TrainerSecretId = pokemon.OT.SecretID;
		//		}
		//
		//		pkmn.Species = (int)pokemon.Species;
		//		pkmn.form = pokemon.Form;
		//		pkmn.NickName = pokemon.Name;
		//
		//		pkmn.Ability = (int)pokemon.Ability;
		//
		//		//pkmn.Nature				= pokemon.getNature();
		//		pkmn.Nature = (int)pokemon.Nature;
		//		pkmn.IsShiny = pokemon.IsShiny;
		//		pkmn.Gender = pokemon.Gender;
		//
		//		//pkmn.PokerusStage			= pokemon.PokerusStage;
		//		pkmn.Pokerus = pokemon.Pokerus;
		//		//pkmn.PokerusStrain		= pokemon.PokerusStrain;
		//
		//		pkmn.IsHyperMode = pokemon.isHyperMode;
		//		//pkmn.IsShadow				= pokemon.isShadow;
		//		pkmn.ShadowLevel = pokemon.ShadowLevel;
		//
		//		pkmn.CurrentHP = pokemon.HP;
		//		pkmn.Item = (int)pokemon.Item;
		//
		//		pkmn.IV = pokemon.IV;
		//		pkmn.EV = pokemon.EV;
		//
		//		pkmn.ObtainedLevel = pokemon.ObtainLevel;
		//		//pkmn.CurrentLevel			= pokemon.Level;
		//		pkmn.CurrentExp = pokemon.Exp;
		//
		//		pkmn.Happines = pokemon.Happiness;
		//
		//		pkmn.Status = (int)pokemon.Status;
		//		pkmn.StatusCount = pokemon.StatusCount;
		//
		//		pkmn.EggSteps = pokemon.EggSteps;
		//
		//		pkmn.BallUsed = (int)pokemon.ballUsed;
		//		if (pokemon.Item != Items.NONE && Kernal.ItemData[pokemon.Item].IsLetter)//PokemonUnity.Inventory.Mail.IsMail(pokemon.Item))
		//		{
		//			pkmn.Mail = new SeriMail(pokemon.Item, pokemon.Mail);
		//		}
		//
		//		pkmn.Moves = new SeriMove[4];
		//		for (int i = 0; i < 4; i++)
		//		{
		//			pkmn.Moves[i] = pokemon.moves[i];
		//		}
		//
		//		//Ribbons is also null, we add a null check
		//		if (pokemon.Ribbons != null)
		//		{
		//			pkmn.Ribbons = new int[pokemon.Ribbons.Count];
		//			for (int i = 0; i < pkmn.Ribbons.Length; i++)
		//			{
		//				pkmn.Ribbons[i] = (int)pokemon.Ribbons[i];
		//			}
		//		}
		//		//else //Dont need else, should copy whatever value is given, even if null...
		//		//{
		//		//	pkmn.Ribbons			= new int[0];
		//		//}
		//		pkmn.Markings = pokemon.Markings;
		//
		//		pkmn.ObtainedMethod = (int)pokemon.ObtainedMode;
		//		pkmn.TimeReceived = pokemon.TimeReceived;
		//		//try
		//		//{
		//		pkmn.TimeEggHatched = pokemon.TimeEggHatched;
		//		//}
		//		//catch (Exception) { seriPokemon.TimeEggHatched = new DateTimeOffset(); }
		//	}
		//
		//	return pkmn;
		//}
		#endregion
#pragma warning restore 0162
	}
}