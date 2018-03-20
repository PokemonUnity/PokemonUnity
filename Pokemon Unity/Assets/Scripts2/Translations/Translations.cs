[System.Serializable]
public class PokedexTranslation //: ITranslation, ITranslationPokedex
{
    ITranslation _translate;
    public string Name;
    public string PokedexEntry
    {
        get { return _translate.Description; }
        set { _translate.Description = value; }
    }
    /// <summary>
    /// "Seed", "Rat", "Plant", etc... Pokemon.
    /// </summary>
    public string Species;
    public string[] Forms;
}

[System.Serializable]
public class MoveTranslation : ITranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class ItemTranslation : ITranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class AbilityTranslation : ITranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class NatureTranslation : ITranslation
{
    public string Name;
    public string Description;
}

public interface ITranslation
{
    string Name { get; set; }
    string Description { get; set; }
}

public interface ITranslationPokedex
{
    string Species { get; set; }
    string[] Forms { get; set; }
}

public static class Class1
{
    public static string Translate(this Settings.Language language, string textId, params string[] fieldValues)
    {
        //Uses the langauge class that has the stored user's (current) language value
        //to perform translantion, by matching value from a dictionary stored in the memory 
        /*
        if (_pokeTranslations == null) //should return english if player's default language is null
        {
            LoadPokedexTranslations(language);//, form
        }

        int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
        if (!_pokeTranslations.ContainsKey(arrayId) && language == Settings.Languages.English)
        {
            //Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
            throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
            //return new PokedexTranslation();
        }
        //ToDo: Show english text for missing data on foreign languages 
        else if (!_pokeTranslations.ContainsKey(arrayId) && language != Settings.Languages.English)
        {
            return _pokeEnglishTranslations[arrayId];
        }*/
        //
        if (fieldValues == null)
        {
            //ToDo: Method to pull translated string value from Dictionary<string,string> stored in memory cache
            return textId;
        }else { 
            System.Collections.Generic.List<string> stringArray = new System.Collections.Generic.List<string>(fieldValues);
            //Use same Dictionary<string,string> method here
            return string.Format(textId, stringArray.ToArray());
        }
    }
}