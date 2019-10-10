using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Attack;
using PokemonUnity.Saving;
using System.IO;
//using System.Security.Cryptography.MD5;

/// <summary>
/// Variables that are stored when game is saved, and other temp values used for gameplay.
/// This class should be called once, when the game boots-up.
/// During boot-up, game will check directory for save files and load data.
/// Game class will overwrite all the other class default values when player triggers a load state.
/// </summary>
public partial class Game 
{
	//ToDo: ReadOnly Immutable Dictionaries...
	public static Dictionary<Pokemons,Pokemon.PokemonData> PokemonData { get; private set; }
	public static Dictionary<Moves,Move.MoveData> MoveData { get; private set; }
	public static Dictionary<Items,Item> ItemData { get; private set; }
	public static Dictionary<Natures,Nature> NatureData { get; private set; }
	//public static Dictionary<Berries,Item.Berry> BerryData { get; private set; }
	//ability

	public void InitPokemons()
	{
		PokemonData = new Dictionary<Pokemons, Pokemon.PokemonData>();
		//foreach value in xml
			//PokemonData.Add()
	}
}