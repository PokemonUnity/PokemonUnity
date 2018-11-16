//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
//using Assets.Scripts.ImportedScripts;//.ColorExtensions;
using System.Collections;
using System.Collections.Generic;
//using System.Data;
//using System.Data.Common;
//using Mono.Data.Sqlite;
//using System.Data.SQLite;
using System.IO;
using System;
//using System.Threading.Task;

/// <summary>
/// all of the values are hard coded
/// first index value is null
/// returns [0,1,2...] as order in pokedex
/// </summary>
[Obsolete("Use `Pokemon.cs` in `/Scripts2` folder...")]
public static class PokemonDatabaseOld
{

	/// <summary>
	/// 
	/// </summary>
	/// <param name="ID"></param>
	/// <returns></returns>
	/// <remarks>Should rely on <see cref="ePokemons.Pokemon"/> for this</remarks>
	//[Obsolete("Use `Pokemon.cs` in `/Scripts2` folder...")]
	public static PokemonDataOld getPokemon(int ID){
		Debug.Log("Get Pokemons");
		/*PokemonData result = null;
		int i = 1;
		while(result == null){
			if(Pokedex[i].getID() == ID){
				Debug.Log("Pokemon DB Success");
				result = Pokedex[i];
			}
			i += 1;
			if(i >= Pokedex.Length){
				Debug.Log("Pokemon DB Fail");
				return null;}
		}*/
        return new PokemonDataOld();// result;
	}

	public static int getLevelExp(PokemonDataOld.LevelingRate levelingRate, int currentLevel){
		return 0;
	}
}