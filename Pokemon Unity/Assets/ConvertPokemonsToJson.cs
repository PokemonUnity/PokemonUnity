using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConvertPokemonsToJson : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    var data = PokemonDatabase.Pokedex;

	    //foreach (PokemonData pokemon in data.Values)
	    //{
     //       string dataAsJson = JsonUtility.ToJson(pokemon, true);
	    //    string filePath =  Application.dataPath + "/Resources/Database/Pokemons/" + pokemon.getID() + ".json"; 
     //       File.WriteAllText(filePath, dataAsJson);
     //   } 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
