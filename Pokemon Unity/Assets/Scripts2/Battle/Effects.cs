using PokemonUnity;
using PokemonUnity.Move;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Effects
{
	/// <summary>
	/// These effects apply to a battler
	/// </summary>
	public class Battler { 
		public bool AquaRing          { get; set; }
		public int Attract            { get; set; }
		public bool BatonPass          { get; set; }
		public int Bide               { get; set; }
		public int BideDamage         { get; set; }
		public int BideTarget         { get; set; }
		public int Charge             { get; set; }
		public PokemonUnity.Move.Moves? ChoiceBand         { get; set; }
		public int Confusion          { get; set; }
		public int Counter            { get; set; }
		public sbyte CounterTarget      { get; set; } //ToDo: maybe `byte?` and do `.HasValue`?
		public bool Curse              { get; set; }
		public bool DefenseCurl        { get; set; }
		public bool DestinyBond        { get; set; }
		public int Disable            { get; set; }
		public PokemonUnity.Move.Moves DisableMove        { get; set; }
		public bool Electrify          { get; set; }
		public int Embargo            { get; set; }
		public int Encore             { get; set; }
		public int EncoreIndex        { get; set; }
		public int EncoreMove         { get; set; }
		public bool Endure             { get; set; }
		public int FirstPledge        { get; set; }
		public bool FlashFire          { get; set; }
		public bool Flinch             { get; set; }
		public int FocusEnergy        { get; set; }
		public int FollowMe           { get; set; }
		public bool Foresight          { get; set; }
		public int FuryCutter         { get; set; }
		public int FutureSight        { get; set; }
		public Moves FutureSightMove    { get; set; }
		public int FutureSightUser    { get; set; }
		public int FutureSightUserPos { get; set; }
		public bool GastroAcid         { get; set; }
		public bool Grudge             { get; set; }
		public int HealBlock          { get; set; }
		public bool HealingWish        { get; set; }
		public bool HelpingHand        { get; set; }
		public int HyperBeam          { get; set; }
		/// <summary>
		/// Takes image of last person on team
		/// </summary>
		public Pokemon Illusion           { get; set; }
		public bool Imprison           { get; set; }
		public bool Ingrain            { get; set; }
		public bool KingsShield        { get; set; }
		public int LeechSeed          { get; set; }
		public bool LifeOrb            { get; set; }
		public int LockOn             { get; set; }
		public int LockOnPos          { get; set; }
		public bool LunarDance         { get; set; }
		public bool MagicCoat          { get; set; }
		public int MagnetRise         { get; set; }
		public int MeanLook           { get; set; }
		public bool MeFirst            { get; set; }
		public int Metronome          { get; set; }
		public bool MicleBerry         { get; set; }
		public bool Minimize           { get; set; }
		public bool MiracleEye         { get; set; }
		public int MirrorCoat         { get; set; }
		public int MirrorCoatTarget   { get; set; }
		public bool MoveNext           { get; set; }
		public bool MudSport           { get; set; }
		/// <summary>
		/// Trapping move
		/// </summary>
		public int MultiTurn          { get; set; } 
		public int MultiTurnAttack    { get; set; }
		public int MultiTurnUser      { get; set; }
		public bool Nightmare          { get; set; }
		public int Outrage            { get; set; }
		public int ParentalBond       { get; set; }
		public int PerishSong         { get; set; }
		public int PerishSongUser     { get; set; }
		public int PickupItem         { get; set; }
		public int PickupUse          { get; set; }
		/// <summary>
		/// Battle Palace only
		/// </summary>
		public bool Pinch              { get; set; }
		public bool Powder             { get; set; }
		public bool PowerTrick         { get; set; }
		public bool Protect            { get; set; }
		public bool ProtectNegation    { get; set; }
		public short ProtectRate        { get; set; }
		public bool Pursuit            { get; set; }
		public bool Quash              { get; set; }
		public bool Rage               { get; set; }
		public int Revenge            { get; set; }
		public bool Roar               { get; set; }
		public byte Rollout            { get; set; }
		public bool Roost              { get; set; }
		/// <summary>
		/// For when using Poké Balls/Poké Dolls
		/// </summary>
		public bool SkipTurn           { get; set; }
		public bool SkyDrop            { get; set; }
		public bool SmackDown          { get; set; }
		public bool Snatch             { get; set; }
		public bool SpikyShield        { get; set; }
		public int Stockpile          { get; set; }
		public int StockpileDef       { get; set; }
		public int StockpileSpDef     { get; set; }
		public int Substitute         { get; set; }
		public int Taunt              { get; set; }
		public int Telekinesis        { get; set; }
		public bool Torment            { get; set; }
		public int Toxic              { get; set; }
		public bool Transform          { get; set; }
		public bool Truant             { get; set; }
		public int TwoTurnAttack      { get; set; }
		public Types Type3              { get; set; }
		public bool Unburden           { get; set; }
		public int Uproar             { get; set; }
		public bool Uturn              { get; set; }
		public bool WaterSport         { get; set; }
		public int WeightChange       { get; set; }
		public int Wish               { get; set; }
		public int WishAmount         { get; set; }
		public int WishMaker          { get; set; }
		public int Yawn               { get; set; }

		public Battler() : this (false) { }
		public Battler(bool batonpass)
		{
			if (!batonpass)
			{
			//  # These effects are retained if Baton Pass is used
			//  @stages[PBStats::ATTACK]   = 0
			//  @stages[PBStats::DEFENSE]  = 0
			//  @stages[PBStats::SPEED]    = 0
			//  @stages[PBStats::SPATK]    = 0
			//  @stages[PBStats::SPDEF]    = 0
			//  @stages[PBStats::EVASION]  = 0
			//  @stages[PBStats::ACCURACY] = 0
			//  @lastMoveUsedSketch        = -1
				this.AquaRing    = false;
				this.Confusion   = 0;
				this.Curse       = false;
				this.Embargo     = 0;
				this.FocusEnergy = 0;
				this.GastroAcid  = false;
				this.HealBlock   = 0;
				this.Ingrain     = false;
				this.LeechSeed   = -1;
				this.LockOn      = 0;
				this.LockOnPos   = -1;
			//  for i in 0...4
			//    next if !@battle.battlers[i]
			//    if @battle.battlers[i].effects.LockOnPos==@index &&
			//       @battle.battlers[i].effects.LockOn>0
			//      @battle.battlers[i].effects.LockOn=0
			//      @battle.battlers[i].effects.LockOnPos=-1
			//    end
			//  end
				this.MagnetRise     = 0;
				this.PerishSong     = 0;
				this.PerishSongUser = -1;
				this.PowerTrick     = false;
				this.Substitute     = 0;
				this.Telekinesis    = 0;
			}
			else
			{
				if (this.LockOn>0){
				  this.LockOn=2;
				}else{
				  this.LockOn=0;
				}
				//if (this.PowerTrick){
				//	attack = defense;
				//	defense = attack;
				//}
			}
			//@damagestate.reset
			//@fainted          = false
			//@lastAttacker     = []
			//@lastHPLost       = 0
			//@tookDamage       = false
			//@lastMoveUsed     = -1
			//@lastMoveUsedType = -1
			//@lastRoundMoved   = -1
			//@movesUsed        = []
			//@turncount        = 0
			this.Attract          = -1;
			//for i in 0...4
			//  next if !@battle.battlers[i]
			//  if @battle.battlers[i].effects.Attract==@index
			//    @battle.battlers[i].effects.Attract=-1
			//  end
			//end
			this.BatonPass        = false;
			this.Bide             = 0;
			this.BideDamage       = 0;
			this.BideTarget       = -1;
			this.Charge           = 0;
			this.ChoiceBand       = null;
			this.Counter          = -1;
			this.CounterTarget    = -1;
			this.DefenseCurl      = false;
			this.DestinyBond      = false;
			this.Disable          = 0;
			this.DisableMove      = 0;
			this.Electrify        = false;
			this.Encore           = 0;
			this.EncoreIndex      = 0;
			this.EncoreMove       = 0;
			this.Endure           = false;
			this.FirstPledge      = 0;
			this.FlashFire        = false;
			this.Flinch           = false;
			this.FollowMe         = 0;
			this.Foresight        = false;
			this.FuryCutter       = 0;
			this.Grudge           = false;
			this.HelpingHand      = false;
			this.HyperBeam        = 0;
			this.Illusion         = null;
			//if self.hasWorkingAbility(:ILLUSION)
			//  lastpoke=@battle.pbGetLastPokeInTeam(@index)
			//  if lastpoke!=@pokemonIndex
			//    this.Illusion     = @battle.pbParty(@index)[lastpoke]
			//  end
			//end
			this.Imprison         = false;
			this.KingsShield      = false;
			this.LifeOrb          = false;
			this.MagicCoat        = false;
			this.MeanLook         = -1;
			//for i in 0...4
			//  next if !@battle.battlers[i]
			//  if @battle.battlers[i].effects.MeanLook==@index
			//    @battle.battlers[i].effects.MeanLook=-1
			//  end
			//end
			this.MeFirst          = false;
			this.Metronome        = 0;
			this.MicleBerry       = false;
			this.Minimize         = false;
			this.MiracleEye       = false;
			this.MirrorCoat       = -1;
			this.MirrorCoatTarget = -1;
			this.MoveNext         = false;
			this.MudSport         = false;
			this.MultiTurn        = 0;
			this.MultiTurnAttack  = 0;
			this.MultiTurnUser    = -1;
			//for i in 0...4
			//  next if !@battle.battlers[i]
			//  if @battle.battlers[i].effects.MultiTurnUser==@index
			//    @battle.battlers[i].effects.MultiTurn=0
			//    @battle.battlers[i].effects.MultiTurnUser=-1
			//  end
			//end
			this.Nightmare        = false;
			this.Outrage          = 0;
			this.ParentalBond     = 0;
			this.PickupItem       = 0;
			this.PickupUse        = 0;
			this.Pinch            = false;
			this.Powder           = false;
			this.Protect          = false;
			this.ProtectNegation  = false;
			this.ProtectRate      = 1;
			this.Pursuit          = false;
			this.Quash            = false;
			this.Rage             = false;
			this.Revenge          = 0;
			this.Roar             = false;
			this.Rollout          = 0;
			this.Roost            = false;
			this.SkipTurn         = false;
			this.SkyDrop          = false;
			this.SmackDown        = false;
			this.Snatch           = false;
			this.SpikyShield      = false;
			this.Stockpile        = 0;
			this.StockpileDef     = 0;
			this.StockpileSpDef   = 0;
			this.Taunt            = 0;
			this.Torment          = false;
			this.Toxic            = 0;
			this.Transform        = false;
			this.Truant           = false;
			this.TwoTurnAttack    = 0;
			this.Type3            = PokemonUnity.Types.NONE;//-1; ToDo: What's difference between "None" and "null"?
			this.Unburden         = false;
			this.Uproar           = 0;
			this.Uturn            = false;
			this.WaterSport       = false;
			this.WeightChange     = 0;
			this.Yawn             = 0;
		}
	}
    					   
	/// <summary>
	/// These effects apply to a side
	/// </summary>
	public class Side { 
		public bool CraftyShield		{ get; set; }
		public byte EchoedVoiceCounter	{ get; set; }
		public bool EchoedVoiceUsed		{ get; set; }
		public byte LastRoundFainted	{ get; set; }
		public byte LightScreen			{ get; set; }
		public byte LuckyChant			{ get; set; }
		public bool MatBlock			{ get; set; }
		public byte Mist				{ get; set; }
		public bool QuickGuard			{ get; set; }
		public byte Rainbow				{ get; set; }
		public byte Reflect				{ get; set; }
		public byte Round				{ get; set; }
		public byte Safeguard			{ get; set; }
		public bool SeaOfFire			{ get; set; }
		public byte Spikes				{ get; set; }
		public bool StealthRock			{ get; set; }
		public bool StickyWeb			{ get; set; }
		public byte Swamp				{ get; set; }
		public byte Tailwind			{ get; set; }
		public byte ToxicSpikes			{ get; set; }
		public bool WideGuard			{ get; set; }
	}
    					   
	/// <summary>
	/// These effects apply to the battle (i.e. both sides)
	/// </summary>
	public class Field { 
		public byte ElectricTerrain	{ get; set; }
		public byte FairyLock		{ get; set; }
		public bool FusionBolt		{ get; set; }
		public bool FusionFlare		{ get; set; }
		public byte GrassyTerrain	{ get; set; }
		public byte Gravity			{ get; set; }
		public byte IonDeluge		{ get; set; }
		public byte MagicRoom		{ get; set; }
		public byte MistyTerrain		{ get; set; }
		public byte MudSportField	{ get; set; }
		public byte TrickRoom		{ get; set; }
		public byte WaterSportField	{ get; set; }
		public byte WonderRoom		{ get; set; }
	}
    					   
	/// <summary>
	/// These effects apply to the usage of a move
	/// </summary>
	public class Move { 
		public int SkipAccuracyCheck { get; set; }
		public int SpecialUsage      { get; set; }
		public int PassedTrying      { get; set; }
		public int TotalDamage       { get; set; }
	}
}
namespace PokemonUnity
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
