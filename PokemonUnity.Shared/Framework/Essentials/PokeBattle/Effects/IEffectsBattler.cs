using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle.Effects
{
	/// <summary>
	/// These effects apply to a battler
	/// </summary>
	public interface IEffectsBattler { 
		bool AquaRing			{ get; set; }
		int Attract				{ get; set; }
		bool BatonPass			{ get; set; }
		int Bide					{ get; set; }
		int BideDamage			{ get; set; }
		int BideTarget			{ get; set; }
		int Charge				{ get; set; }
		Moves ChoiceBand			{ get; set; }
		int Confusion			{ get; set; }
		int Counter				{ get; set; }
		sbyte CounterTarget		{ get; set; } //ToDo: maybe `byte?` and do `.HasValue`?
		bool Curse				{ get; set; }
		bool DefenseCurl			{ get; set; }
		bool DestinyBond			{ get; set; }
		int Disable				{ get; set; }
		Moves DisableMove		{ get; set; }
		bool Electrify			{ get; set; }
		int Embargo				{ get; set; }
		int Encore				{ get; set; }
		int EncoreIndex			{ get; set; }
		Moves EncoreMove			{ get; set; }
		bool Endure				{ get; set; }
		PokemonUnity.Attack.Data.Effects FirstPledge	{ get; set; }
		bool FlashFire			{ get; set; }
		bool Flinch				{ get; set; }
		int FocusEnergy			{ get; set; }
		int FollowMe				{ get; set; }
		bool Foresight			{ get; set; }
		int FuryCutter			{ get; set; }
		int FutureSight			{ get; set; }
		Moves FutureSightMove	{ get; set; }
		int FutureSightUser		{ get; set; }
		int FutureSightUserPos	{ get; set; }
		bool GastroAcid			{ get; set; }
		bool Grudge				{ get; set; }
		int HealBlock			{ get; set; }
		bool HealingWish			{ get; set; }
		bool HelpingHand			{ get; set; }
		int HyperBeam			{ get; set; }
		/// <summary>
		/// Takes image of last person on team
		/// </summary>
		PokemonUnity.Monster.Pokemon Illusion		{ get; set; }
		bool Imprison			{ get; set; }
		bool Ingrain				{ get; set; }
		bool KingsShield			{ get; set; }
		int LeechSeed			{ get; set; }
		bool LifeOrb				{ get; set; }
		int LockOn				{ get; set; }
		int LockOnPos			{ get; set; }
		bool LunarDance			{ get; set; }
		bool MagicCoat			{ get; set; }
		int MagnetRise			{ get; set; }
		int MeanLook				{ get; set; }
		bool MeFirst				{ get; set; }
		int Metronome			{ get; set; }
		bool MicleBerry			{ get; set; }
		bool Minimize			{ get; set; }
		bool MiracleEye			{ get; set; }
		int MirrorCoat			{ get; set; }
		int MirrorCoatTarget		{ get; set; }
		bool MoveNext			{ get; set; }
		bool MudSport			{ get; set; }
		/// <summary>
		/// Trapping move
		/// </summary>
		int MultiTurn			{ get; set; } 
		Moves MultiTurnAttack	{ get; set; }
		int MultiTurnUser		{ get; set; }
		bool Nightmare			{ get; set; }
		int Outrage				{ get; set; }
		int ParentalBond			{ get; set; }
		int PerishSong			{ get; set; }
		int PerishSongUser		{ get; set; }
		Items PickupItem			{ get; set; }
		int PickupUse			{ get; set; }
		/// <summary>
		/// Battle Palace only
		/// </summary>
		bool Pinch				{ get; set; }
		bool Powder				{ get; set; }
		bool PowerTrick			{ get; set; }
		bool Protect				{ get; set; }
		bool ProtectNegation		{ get; set; }
		short ProtectRate		{ get; set; }
		bool Pursuit				{ get; set; }
		bool Quash				{ get; set; }
		bool Rage				{ get; set; }
		int Revenge				{ get; set; }
		bool Roar				{ get; set; }
		byte Rollout				{ get; set; }
		bool Roost				{ get; set; }
		/// <summary>
		/// For when using Poké Balls/Poké Dolls
		/// </summary>
		bool SkipTurn			{ get; set; }
		bool SkyDrop				{ get; set; }
		bool SmackDown			{ get; set; }
		bool Snatch				{ get; set; }
		bool SpikyShield			{ get; set; }
		int Stockpile			{ get; set; }
		int StockpileDef			{ get; set; }
		int StockpileSpDef		{ get; set; }
		int Substitute			{ get; set; }
		int Taunt				{ get; set; }
		int Telekinesis			{ get; set; }
		bool Torment				{ get; set; }
		int Toxic				{ get; set; }
		bool Transform			{ get; set; }
		bool Truant				{ get; set; }
		Moves TwoTurnAttack		{ get; set; }
		PokemonUnity.Types Type3	{ get; set; }
		bool Unburden			{ get; set; }
		int Uproar				{ get; set; }
		bool Uturn				{ get; set; }
		bool WaterSport			{ get; set; }
		int WeightChange			{ get; set; }
		int Wish					{ get; set; }
		int WishAmount			{ get; set; }
		int WishMaker			{ get; set; }
		int Yawn					{ get; set; }

		IEffectsBattler initialize(bool batonpass);
	}
}