using System;
using System.Collections.Generic;
using PokemonUnity.Attack;
using PokemonUnity.Character;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster
{
	public interface IPokemon
	{
		int TotalHP { get; }
		int HP { get; }
		int ATK { get; }
		int DEF { get; }
		int SPA { get; }
		int SPD { get; }
		int SPE { get; }
		int[] IV { get; }
		byte[] EV { get; }
		Pokemons Species { get; }
		TrainerData OT { get; }
		string TrainerId { get; }
		int PersonalId { get; }
		int[] Pokerus { get; }
		Items Item { get; }
		Items itemRecycle { get; }
		Items itemInitial { get; }
		bool belch { get; }
		string Name { get; }
		int Exp { get; }
		int Happiness { get; }
		Status Status { get; }
		int StatusCount { get; }
		int EggSteps { get; }
		Move[] moves { get; }
		Items ballUsed { get; }
		bool[] Markings { get; set; }
		Pokemon.ObtainedMethod ObtainedMode { get; }
		Locations ObtainMap { get; }
		int ObtainLevel { get; }
		Abilities Ability { get; }
		bool? Gender { get; }
		Natures Nature { get; }
		//bool IsShiny { get; }
		Ribbon[] Ribbons { get; }
		byte[] Contest { get; }

		//int Form { get; set; }
		//LevelingRate GrowthRate { get; }
		//bool isEgg { get; }
		//bool isHyperMode { get; }
		bool IsNicknamed { get; }
		//bool isShadow { get; }
		//int Level { get; }
		//string Mail { get; }
		//bool? PokerusStage { get; }
		//int PokerusStrain { get; }
		//int? ShadowLevel { get; }
		DateTimeOffset? TimeEggHatched { get; }
		DateTimeOffset TimeReceived { get; }
		Types Type1 { get; }
		Types Type2 { get; }
	}

	public interface IPokemonMultipleForms
	{
		int? forcedform { get; set; }
		int form { get; set; }
		//Data.Form Form { get; }
		int formNoCall { set; }
		DateTime? formTime { get; set; }

		void forceForm(int value);
	}

	/// <summary>
	/// Mega Evolutions and Primal Reversions are treated as form changes in
	/// Essentials. The code below is just more of what's in the Pokemon_MultipleForms
	/// script section, but specifically and only for the Mega Evolution and Primal
	/// Reversion forms.
	/// </summary>
	public interface IPokemonMegaEvolution
	{
		//bool IsMega { get; }
		//bool IsPrimal { get; }

		bool hasMegaForm();
		bool hasPrimalForm();
		bool isMega();
		bool isPrimal();
		void makeMega();
		void makePrimal();
		void makeUnmega();
		void makeUnprimal();
		int megaMessage();
		string megaName();
	}
	public interface IShadowPokemon
	{
		//int exp { set; }
		int Exp { set; }
		int? heartgauge { get; }
		int heartStage { get; }
		//int hp { set; }
		//int HP { set; }
		bool hypermode { get; }
		bool isShadow { get; }
		int[] savedev { get; }
		int savedexp { get; }
		bool shadow { get; }
		int shadowmovenum { get; }
		Moves[] shadowmoves { get; }

		void adjustHeart(int value);
		void makeShadow();
		void pbUpdateShadowMoves(bool allmoves = false);
	}
}