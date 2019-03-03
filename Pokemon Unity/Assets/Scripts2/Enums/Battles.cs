using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;

namespace PokemonUnity.Battle
{
	public enum Stats
	{
		ATTACK,
		DEFENSE,
		SPEED,
		SPATK,
		SPDEF,
		/// <summary>
		/// Use minus 2 when reflecting in IV, 
		/// as accuracy and evasion are battle only
		/// </summary>
		/// Should HP be here?
		HP,
		/// <summary>
		/// battle only stat
		/// </summary>
		ACCURACY,
		/// <summary>
		/// battle only stat
		/// </summary>
		EVASION
	}
	#region Battle
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
	#region Effects
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
	#endregion
}