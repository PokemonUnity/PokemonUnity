using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Language : MonoBehaviour {

    private static Country lang = Country.ENGLISH;

    private static readonly List<Dictionary<string, object>> names = CSVReader.Read("DataCSV/names");

	public enum Country
        {
            ENGLISH,
            FRANCAIS
        }

    public static Country getLang()
    {
        return lang;
    }

    public static string getPokemonName(int id)
    {
        if (id < names.Count)
        {
            switch (lang)
            {
                default:
                    return names[id]["en"].ToString();
                case Country.FRANCAIS:
                    return names[id]["fr"].ToString();
            }
            
        }
        
        return PokemonDatabase.getPokemon(id).getName();
    }
}
