using System.Collections.Generic;
using PokemonUnity.Pokemon;

#region Deprecated/Obsolete
[System.Serializable]
[System.Obsolete]
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
[System.Obsolete]
public class MoveTranslation : ITranslation
{
    public string Name { get; set; }
    public string Description { get; set; }
}

[System.Serializable]
[System.Obsolete]
public class ItemTranslation : ITranslation
{
    public string Name { get; set; }
    public string Description { get; set; }
}

[System.Serializable]
[System.Obsolete]
public class AbilityTranslation : ITranslation
{
    public string Name { get; set; }
    public string Description { get; set; }
}

[System.Serializable]
[System.Obsolete]
public class NatureTranslation : ITranslation
{
    public string Name { get; set; }
    public string Description { get; set; }
}

[System.Obsolete]
public interface ITranslation
{
    string Name { get; set; }
    string Description { get; set; }
}

[System.Obsolete]
public interface ITranslationPokedex
{
    string Species { get; set; }
    string[] Forms { get; set; }
}
#endregion

#region Extras
public static class LanguageExtension
{
    private static LocalizationDictionaryProviderBase _dictionary;
    public static void /*XmlFileLocalizationDictionaryProvider*/ Initialize(string filename)
    {
#if DEBUG
        string filepath = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
        //string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
        //string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
        string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
        //XmlFileLocalizationDictionaryProvider x = new XmlFileLocalizationDictionaryProvider(filepath);
        _dictionary = new XmlFileLocalizationDictionaryProvider(filepath);
        _dictionary.Initialize("PokemonUnity");
        //return x;
    }
    public static string Translate(this Settings.Languages language, string textId, params string[] fieldValues)
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

    public static Translator.Language.LocalizedString Translate(this string text, params string[] fieldValues)
    {
        //Uses the langauge class that has the stored user's (current) language value
        //to perform translantion, by matching value from a dictionary stored in the memory 
        
        if (_dictionary == null) //should return english if player's default language is null
        {
            //LoadPokedexTranslations(language);//, form
            Initialize("PokemonUnity");
        }

        //if(_dictionary.Dictionaries[Settings.UserLanguage.ToString()].GetAllStrings().Count < _dictionary.Dictionaries[Translator.Languages.English.ToString()].GetAllStrings().Count) _dictionary.Dictionaries[Settings.UserLanguage.ToString()]

        //int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
        /*if (!_dictionary.Dictionaries[Settings.UserLanguage.ToString()].GetAllStrings().Contains(textId))//&& language == Settings.Languages.English
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
        
        
            //if(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames.Length > 0){
            //List<string> formvalues = new List<string>();
            List<string> fieldvalues = new List<string>();
            //List<KeyValuePair<string,string>> fieldnames = new List<KeyValuePair<string,string>>();
            foreach (KeyValuePair<string, string> field in _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
            {
                if (field.Key != "id" || field.Key != "identifier" || field.Key != "name" || field.Key != "language")
                {
                    //fieldnames.Add(field);
                    /*if (field.Key.Contains("form")){
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        formvalues.Add(field.Value);
                    }*/
                    if (field.Key.Contains("field"))
                    {
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
                        fieldvalues.Add(field.Value);
                    }
                }
            }
            /*if (formvalues.Count > 0)
            {
                _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = formvalues.JoinAsString("; ") + Environment.NewLine + _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;
            }*/
            //if (_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames.Contains("form")){ //Name vs LocalName?
            if (fieldvalues.Count > 0)
            { //fieldnames.Contains("form")
                _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, fieldValues);
                //_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value = formvalues.JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text].FieldNames)
            }

        return _dictionary.Dictionaries[Settings.UserLanguage.ToString()][text];

        /*if (fieldValues == null)
        {
            //ToDo: Method to pull translated string value from Dictionary<string,string> stored in memory cache
            return textId;
        }else { 
            System.Collections.Generic.List<string> stringArray = new System.Collections.Generic.List<string>(fieldValues);
            //Use same Dictionary<string,string> method here
            return string.Format(textId, stringArray.ToArray());
        }*/
    }

    public static Translator.Language.LocalizedString TranslateString(string text, params string[] fieldValues)
    {
        return text.Translate(fieldValues);
    }
}
#endregion

#region 
public static class gameTextExtension //: LanguageExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">string that contains full length message</param>
    /// <param name="text">Text string to that will be displayed in color</param>
    /// <param name="color">rrggbbaa color value</param>
    public static void RichTextAssignColor(ref string message, string text, string color)
    {
        //message.Replace("["+text+"]", string.Format("<color=#{0}>{1}</color>", color, text));
        RichTextAssignColor(ref message, text, text, color);
        //return message;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message">string that contains full length message</param>
    /// <param name="replaceText">Text in message string to replace</param>
    /// <param name="text">Text string to that will be displayed in color</param>
    /// <param name="color">rrggbbaa color value</param>
    public static void RichTextAssignColor(ref string message, string replaceText, string text, string color)
    {
        message.Replace("[" + replaceText + "]", string.Format("<color={0}>{1}</color>", color, text));//#rrggbbaa works as well
        //return message;
    }
    /// <summary>
    /// This might be pretty taxing on system, would not recommend using until a replacement can be made
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string highlightCodexAllTerms(this string text)
    {
        /*if (fieldValues == null)
        {
            //ToDo: Method to pull translated string value from Dictionary<string,string> stored in memory cache
            return textId;
        }
        else
        {
            System.Collections.Generic.List<string> stringArray = new System.Collections.Generic.List<string>(fieldValues);
            //Use same Dictionary<string,string> method here
            return string.Format(textId, stringArray.ToArray());
        }*/
        string[] CodexLibrary = new string[] { "Pokemons", "Items", "Moves", "Abilities", "Types" };//Maybe locations... and npcs
        foreach(string codex in CodexLibrary)
        { 
            switch (codex)
            {
                case "Pokemons":
                    foreach(var pokemon in System.Enum.GetNames(typeof(Pokemons)))
                    {
                        RichTextAssignColor(ref text, pokemon, "blue");
                    }
                    break;
                default:
                    break;
            }
        }
        return text;
    }
    public static string PadNumbers(this string input)
    {
        return System.Text.RegularExpressions.Regex.Replace(input,"[0-9]+", match => match.Value.PadLeft(3,'0'));
    }
}
#endregion