using System;
using System.Collections.Generic;
using PokemonUnity.Attack;
using PokemonUnity.Character;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster
{
	public interface IPokemon : IPokemonBattle
	{
		//Abilities Ability { get; set; }
		//int ATK { get; }
		//Items ballUsed { get; }
		byte[] Contest { get; }
		//int DEF { get; }
		int EggSteps { get; }
		byte[] EV { get; }
		//Pokemon.Experience Exp { get; }
		//int Form { get; set; }
		//bool? Gender { get; }
		//LevelingRate GrowthRate { get; }
		//int Happiness { get; }
		//int HP { get; set; }
		//bool isEgg { get; }
		//bool isHyperMode { get; }
		bool IsNicknamed { get; }
		//bool isShadow { get; }
		//bool IsShiny { get; }
		//Items Item { get; }
		//int[] IV { get; }
		//int Level { get; }
		string Mail { get; }
		bool[] Markings { get; set; }
		//Move[] moves { get; }
		//string Name { get; }
		//Natures Nature { get; }
		Pokemon.ObtainedMethod ObtainedMode { get; }
		int ObtainLevel { get; }
		Locations ObtainMap { get; }
		TrainerId OT { get; }
		int PersonalId { get; }
		//int[] Pokerus { get; }
		//bool? PokerusStage { get; }
		//int PokerusStrain { get; }
		string PublicId { get; }
		List<Ribbon> Ribbons { get; }
		//int? ShadowLevel { get; }
		//int SPA { get; }
		//int SPD { get; }
		//int SPE { get; }
		//Pokemons Species { get; }
		//Status Status { get; set; }
		//int StatusCount { get; }
		DateTimeOffset? TimeEggHatched { get; }
		DateTimeOffset TimeReceived { get; }
		//int TotalHP { get; }
		//Types Type1 { get; }
		//Types Type2 { get; }
	}
	public interface IPokemonBattle
	{
		Abilities Ability { get; set; }
		int ATK { get; }
		Items ballUsed { get; }
		//byte[] Contest { get; }
		int DEF { get; }
		//int EggSteps { get; }
		//byte[] EV { get; }
		Pokemon.Experience Exp { get; }
		int Form { get; set; }
		bool? Gender { get; }
		LevelingRate GrowthRate { get; }
		int Happiness { get; }
		int HP { get; set; }
		bool isEgg { get; }
		bool isHyperMode { get; }
		//bool IsNicknamed { get; }
		bool isShadow { get; }
		bool IsShiny { get; }
		Items Item { get; }
		int[] IV { get; }
		int Level { get; }
		//string Mail { get; }
		//bool[] Markings { get; set; }
		Move[] moves { get; }
		string Name { get; }
		Natures Nature { get; }
		//Pokemon.ObtainedMethod ObtainedMode { get; }
		//int ObtainLevel { get; }
		//Locations ObtainMap { get; }
		//TrainerId OT { get; }
		//int PersonalId { get; }
		int[] Pokerus { get; }
		bool? PokerusStage { get; }
		int PokerusStrain { get; }
		//string PublicId { get; }
		//List<Ribbon> Ribbons { get; }
		int? ShadowLevel { get; }
		int SPA { get; }
		int SPD { get; }
		int SPE { get; }
		Pokemons Species { get; }
		Status Status { get; set; }
		int StatusCount { get; }
		//DateTimeOffset? TimeEggHatched { get; }
		//DateTimeOffset TimeReceived { get; }
		int TotalHP { get; }
		Types Type1 { get; }
		Types Type2 { get; }
	}
}