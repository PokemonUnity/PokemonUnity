using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Interface;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Item
{
	/// <summary>
	/// Extension of <seealso cref="Interface.IGlobalMetadata"/>
	/// </summary>
	public interface IGlobalMetadataPokeRadar : Field.IGlobalMetadata
	{
		int pokeradarBattery { get; }
	}
	/// <summary>
	/// Extension of <seealso cref="ITempMetadata"/>
	/// </summary>
	public interface ITempMetadataPokeRadar
	{
		// [species, level, chain count, grasses (x,y,ring,rarity)]
		IPokeRadarMetaData pokeradar { get; }
	}

	public interface IPokeRadarMetaData
	{
		//Tile Grass; x,y,ring,rarity
		//int[] Grass; //x,y,ring,rarity
		IPokeRadarGrassData[] Grass { get; }
		int ChainCount { get; }
		Pokemons Species { get; }
		int Level { get; }
	}
	public interface IPokeRadarGrassData
	{
		int X { get; }
		int Y { get; }
		/// <summary>
		/// (0-3 inner to outer)
		/// </summary>
		int Ring { get; }
		int Rarity { get; }
		//IPokeRadarGrassData(int mapx, int mapy, int ring, int rarity)
	}

	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGamePokeRadar : IGame
	{
		bool CanUsePokeRadar();

		bool UsePokeRadar();

		void PokeRadarHighlightGrass(bool showmessage = true);

		void PokeRadarCancel();

		int PokeRadarGetShakingGrass();

		bool PokeRadarOnShakingGrass();

		Field.IEncounterPokemon PokeRadarGetEncounter(int rarity = 0);
	}
}