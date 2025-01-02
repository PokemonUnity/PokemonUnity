﻿using System.Collections.Generic;
using PokemonUnity.Monster;
using PokemonUnity;
using PokemonUnity.Localization;

namespace PokemonUnity
{
	#region Extras
	public class GameText : XmlFileLocalizationDictionaryProvider
	{
		//static readonly System.Lazy<LocalizationDictionaryProviderBase> _dictionary { get { return new System.Lazy<LocalizationDictionaryProviderBase>} }
		//static readonly System.Lazy<GameText> instanceHolder { get { return new System.Lazy<GameText>(() => new GameText()); } }
		public static GameText Instance { get {
				//if (instanceHolder == null)
				//{
				//	_initialize();
					//instanceHolder = new GameText(filename: null);
				//}
				return instanceHolder; } }
		static readonly GameText instanceHolder;// = new GameText(filename: null); //newInstance();
			/*get { //readonly = null
				if (instanceHolder == null)
				{
					_initialize();
					instanceHolder = new GameText(filename: null);
				}
				return instanceHolder; }
			set { instanceHolder = value; } }*/
		private static string path { get {
				//if(_path == null)_initialize();
				string x = null;
	#if DEBUG
				x = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
				//string filepath = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
				//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
				//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
	#else
				x = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
				//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
	#endif
				return x;// ?? _initialize();
			}
			/*set
			{
				_path = value;
			}*/
		}// = null;
		//private static string file;// = null;
		//string _path;
		static GameText()// : base(path)
		{
			//should return english if player's default language is null
			//if(path == null)_initialize();
			//base.Initialize(file ?? "PokemonUnity");
			//Instance.Initialize(file ?? "PokemonUnity");
			//instanceHolder.Initialize(file ?? "PokemonUnity");
			instanceHolder = new GameText(filename: null);
		}
		/*GameText() : base(path)
		{
			//base.Initialize(file ?? "PokemonUnity");
		}* /
		public GameText(string dirpath = null, string filename = null) : base(dirpath)
		{
			//should return english if player's default language is null
			//if(path == null)_initialize();
			path = dirpath;
			base.Initialize(filename ?? "PokemonUnity");
			//instanceHolder.Initialize(filename ?? "PokemonUnity");
			//file = filename ?? "PokemonUnity";
			instanceHolder = this;
		}*/
		public GameText(string filename = null) : base(path)
		{
			//should return english if player's default language is null
			//if(path == null)_initialize();
			//path = dirpath;
			base.Initialize(filename ?? "PokemonUnity");
			//instanceHolder.Initialize(filename ?? "PokemonUnity");
			//file = filename ?? "PokemonUnity";
			//instanceHolder = this;
		}
		/*public void Initialize(string filename = null, string directorypath = null) //XmlFileLocalizationDictionaryProvider
		{
	#if DEBUG
			path = directorypath ?? @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string filepath = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
	#else
			path = directorypath ?? UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
			//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
	#endif
			//XmlFileLocalizationDictionaryProvider x = new XmlFileLocalizationDictionaryProvider(filepath);
			//Instance = new XmlFileLocalizationDictionaryProvider(filepath);
			//Instance.Initialize(filename);//"PokemonUnity"
			//x.Initialize(filename);
			//return x;
			//GameText(filepath);
		}*/
		public string _initialize()
		{
			//instanceHolder.file = filename ?? instanceHolder.file;
			//GameText x = new GameText(filename ?? "PokemonUnity");
			string x = null;
	#if DEBUG
			x = path ?? @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string filepath = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
	#else
			x = path ?? UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
			//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
	#endif
			//XmlFileLocalizationDictionaryProvider x = new XmlFileLocalizationDictionaryProvider(filepath);
			//Instance = new XmlFileLocalizationDictionaryProvider(filepath);
			//Instance.Initialize(filename);//"PokemonUnity"
			//x.Initialize(filename);
			return x;
			//GameText(filepath);
		}
		/*static void ChangeLanguage(string filename = null, string directorypath = null)
		{
			path = directorypath ?? path;
			file = filename ?? file;
			//_initialize();
			instanceHolder = new GameText(filename: file);
		}*/
		/*public static GameText newInstance(string path = null)
		{
			//instanceHolder.file = filename ?? instanceHolder.file;
			//GameText x = new GameText(filename ?? "PokemonUnity");
			string x = null;
	#if DEBUG
			x = path ?? @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string filepath = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Translations\";//\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
	#else
			x = path ?? UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
			//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
	#endif
			//XmlFileLocalizationDictionaryProvider x = new XmlFileLocalizationDictionaryProvider(filepath);
			//Instance = new XmlFileLocalizationDictionaryProvider(filepath);
			//Instance.Initialize(filename);//"PokemonUnity"
			//x.Initialize(filename);
			//return x;
			//GameText(filepath);
			//insta
			GameText y = new GameText(dirpath: x);
			return y;
		}*/
		/*static void ChangeLanguage(string filename = null, string directorypath = null)
		{
			path = directorypath ?? path;
			file = filename ?? file;
			//_initialize();
			instanceHolder = new GameText(filename: file);
		}*/
	}

	public static class LanguageExtension //: GameText
	{
		static LocalizationDictionaryProviderBase _dictionary { get { return GameText.Instance; } } //new GameText().Instance
		/// <summary>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="textId"></param>
		/// <param name="fieldValues"></param>
		/// <returns></returns>
		/// ToDo: Overload this where TextId can also be an Int?
		public static Translator.Language.LocalizedString Translate(PokemonUnity.Localization.Text text, string textId, params string[] fieldValues)
		{
			var Failure = new Translator.Language.LocalizedString("Failed", "The contents of this dictionary were unable to be found", Core.UserLanguage);
			/*
			if (_pokeTranslations == null) //should return english if player's default language is null
			{
				LoadPokedexTranslations(language);//, form
			}

			int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			if (!_pokeTranslations.ContainsKey(arrayId) && language == Languages.English)
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
				//return new PokedexTranslation();
			}
			//ToDo: Show english text for missing data on foreign languages 
			else if (!_pokeTranslations.ContainsKey(arrayId) && language != Languages.English)
			{
				return _pokeEnglishTranslations[arrayId];
			}*/
			//return string.Format(textId, stringArray.ToArray());
		
			List<string> fieldvalues = new List<string>();
			//For each NODE in TranslationDictionary.XML
			foreach (var node in _dictionary.NodeDictionaries[Core.UserLanguage.ToString()])
			{
				//And the Category.Id the translation string the Text.Id belongs to 
				switch (text)
				{
					//IF the NODE and the Category.Id are the same
					/* Only needed if the label in the .XML is different from Category.Id.ToString().ToUpper()
					case PokemonUnity.Text.Species:
					case PokemonUnity.Text.Kinds:
					case PokemonUnity.Text.Entries:
					case PokemonUnity.Text.FormNames:
					case PokemonUnity.Text.Moves:
					case PokemonUnity.Text.MoveDescriptions:
					case PokemonUnity.Text.Items:
					case PokemonUnity.Text.ItemPlurals:
					case PokemonUnity.Text.ItemDescriptions:
					case PokemonUnity.Text.Abilities:
					case PokemonUnity.Text.AbilityDescs:
					case PokemonUnity.Text.Types:
					case PokemonUnity.Text.TrainerTypes:
					case PokemonUnity.Text.TrainerNames:
					case PokemonUnity.Text.BeginSpeech:
					case PokemonUnity.Text.EndSpeechWin:
					case PokemonUnity.Text.EndSpeechLose:
					case PokemonUnity.Text.RegionNames:
					case PokemonUnity.Text.PlaceNames:
					case PokemonUnity.Text.PlaceDescriptions:
					case PokemonUnity.Text.MapNames:
					case PokemonUnity.Text.PhoneMessages:
					case PokemonUnity.Text.ScriptTexts:*/
					default:
						if (node.Key == System.Enum.GetName(typeof(PokemonUnity.Localization.Text), text).ToUpper())
						{
							foreach (KeyValuePair<string, string> field in node.Value[textId].FieldNames)
							{
								if (field.Key != "id" || field.Key != "identifier" || field.Key != "language")// || field.Key != "name"
								{
									//fieldnames.Add(field);
									/*if (field.Key.Contains("form")){
										//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
										//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
										formvalues.Add(field.Value);
									}*/
									if (field.Key.Contains("field"))
									{
										fieldvalues.Add(field.Value);
									}
								}
							}
							if (fieldvalues.Count > 0)
							{
								node.Value[textId].Value = string.Format(node.Value[textId].Value, fieldValues);
							}
							return node.Value[textId];
						}
						else
							return Failure;
						//break;
				}
			}

			return Failure;
		}

		public static string Translate(this Languages language, string textId, params string[] fieldValues)
		{
			//Uses the langauge class that has the stored user's (current) language value
			//to perform translantion, by matching value from a dictionary stored in the memory 
			/*
			if (_pokeTranslations == null) //should return english if player's default language is null
			{
				LoadPokedexTranslations(language);//, form
			}

			int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			if (!_pokeTranslations.ContainsKey(arrayId) && language == Languages.English)
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
				//return new PokedexTranslation();
			}
			//ToDo: Show english text for missing data on foreign languages 
			else if (!_pokeTranslations.ContainsKey(arrayId) && language != Languages.English)
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
	#if DEBUG
			//Log Translation Text for Debugging purposes...
			//TranslationLogger.LogMessageToFile(text);
	#endif

			//Uses the langauge class that has the stored user's (current) language value
			//to perform translantion, by matching value from a dictionary stored in the memory 
		
			/*if (_dictionary == null) //should return english if player's default language is null
			{
				//LoadPokedexTranslations(language);//, form
				Initialize("PokemonUnity");
			}*/

			//if(_dictionary.Dictionaries[Core.UserLanguage.ToString()].GetAllStrings().Count < _dictionary.Dictionaries[Translator.Languages.English.ToString()].GetAllStrings().Count) _dictionary.Dictionaries[Core.UserLanguage.ToString()]

			//int arrayId = (int)id;// GetPokemon(id).ArrayId; //unless db is set, it'll keep looping null...
			/*if (!_dictionary.Dictionaries[Core.UserLanguage.ToString()].GetAllStrings().Contains(text))//&& language == Languages.English
			{
				//Debug.LogError("Failed to load pokedex translation for pokemon with id: " + (int)id); //ToDo: Throw exception error
				//throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}_{1}", (int)id, id.ToString()));
				throw new System.Exception(string.Format("Failed to load pokedex translation for pokemon with id: {0}", text));
				//return new PokedexTranslation();
			}
			//ToDo: Show english text for missing data on foreign languages 
			/*else if (!_pokeTranslations.ContainsKey(arrayId) && language != Languages.English)
			{
				return _pokeEnglishTranslations[arrayId];
			}*/
			//
		
			//if (_dictionary.Dictionaries[Core.UserLanguage.ToString()].GetAllStrings().Contains(text))
			//{
				//if(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames.Length > 0){
				//List<string> formvalues = new List<string>();
				List<string> fieldvalues = new List<string>();
				//List<KeyValuePair<string,string>> fieldnames = new List<KeyValuePair<string,string>>();
				foreach (KeyValuePair<string, string> field in _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
				{
					if (field.Key != "id" || field.Key != "identifier" || field.Key != "language")// || field.Key != "name"
					{
						//fieldnames.Add(field);
						/*if (field.Key.Contains("form")){
							//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
							//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
							formvalues.Add(field.Value);
						}*/
						if (field.Key.Contains("field"))
						{
							//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, fieldnames.ToArray());//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
							//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = fieldnames.Where() .JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
							fieldvalues.Add(field.Value);
						}
					}
				}
				/*if (formvalues.Count > 0)
				{
					_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = formvalues.JoinAsString("; ") + Environment.NewLine + _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;
				}*/
				//if (_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames.Contains("form")){ //Name vs LocalName?
				if (fieldvalues.Count > 0)
				{ //fieldnames.Contains("form")
					_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = string.Format(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, fieldValues);// + 
					//Adding form value to output string, so that if a pokemon isnt being translated, it would be easy to decide between description or name
					//formvalues.Count > 0? "|" + formvalues.JoinAsString(";"):"";
					//_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value = formvalues.JoinAsString("; ") +"\n"+ _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value;//(_dictionary.Dictionaries[Core.UserLanguage.ToString()][text].Value, _dictionary.Dictionaries[Core.UserLanguage.ToString()][text].FieldNames)
				}

				return _dictionary.Dictionaries[Core.UserLanguage.ToString()][text];
			//} else
				//throw new System.Exception("No Translation Found For: " + text);
				//return "No Translation Found For: " + text;

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

	namespace Localization
	{
		/// <summary>
		/// Different category sections a piece of translated text could belong to
		/// </summary>
		public enum Text
		{
			Species           = 1 ,
			Kinds             = 2 ,
			Entries           = 3 ,
			FormNames         = 4 ,
			Moves             = 5 ,
			MoveDescriptions  = 6 ,
			Items             = 7 ,
			ItemPlurals       = 8 ,
			ItemDescriptions  = 9 ,
			Abilities         = 10,
			AbilityDescs      = 11,
			Types             = 12,
			TrainerTypes      = 13,
			TrainerNames      = 14,
			BeginSpeech       = 15,
			EndSpeechWin      = 16,
			EndSpeechLose     = 17,
			RegionNames       = 18,
			PlaceNames        = 19,
			PlaceDescriptions = 20,
			MapNames          = 21,
			PhoneMessages     = 22,
			ScriptTexts       = 23,
			Errors			  = 24
		}
	}
}