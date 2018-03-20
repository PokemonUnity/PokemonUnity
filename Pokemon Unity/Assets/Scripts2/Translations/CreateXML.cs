using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

class CreateXML
{
}

/// <summary>
/// Provides localization dictionaries from XML files in a directory.
/// </summary>
public class XmlFileLocalizationDictionaryProvider : LocalizationDictionaryProviderBase
{
    private readonly string _directoryPath;

    /// <summary>
    /// Creates a new <see cref="XmlFileLocalizationDictionaryProvider"/>.
    /// </summary>
    /// <param name="directoryPath">Path of the dictionary that contains all related XML files</param>
    public XmlFileLocalizationDictionaryProvider(string directoryPath)
    {
        _directoryPath = directoryPath;
    }

    public override void Initialize(string sourceName)
    {
        var fileNames = Directory.GetFiles(_directoryPath, "*.xml", SearchOption.TopDirectoryOnly);

        foreach (var fileName in fileNames)
        {
            var dictionary = CreateXmlLocalizationDictionary(fileName);
            if (Dictionaries.ContainsKey(dictionary.CultureInfo.ToString() /*.Name*/))
            {
                throw new Exception(sourceName + " source contains more than one dictionary for the culture: " + dictionary.CultureInfo.ToString() /*.Name*/);
            }

            Dictionaries[dictionary.CultureInfo.ToString() /*.Name*/] = (Settings.Language.ILocalizationDictionary)dictionary;

            if (fileName.EndsWith(sourceName + ".xml"))
            {
                if (DefaultDictionary != null)
                {
                    throw new Exception("Only one default localization dictionary can be for source: " + sourceName);
                }

                DefaultDictionary = (Settings.Language.ILocalizationDictionary)dictionary;
            }
        }
    }

    protected virtual Settings.Language.XmlLocalizationDictionary CreateXmlLocalizationDictionary(string fileName)
    {
        return Settings.Language.XmlLocalizationDictionary.BuildFomFile(fileName);
    }
}

public abstract class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
{
    public string SourceName { get; private set; }

    public Settings.Language.ILocalizationDictionary DefaultDictionary { get; protected set; }

    public IDictionary<string, Settings.Language.ILocalizationDictionary> Dictionaries { get; private set; }

    protected LocalizationDictionaryProviderBase()
    {
        Dictionaries = new Dictionary<string, Settings.Language.ILocalizationDictionary>();
    }

    public virtual void Initialize(string sourceName)
    {
        SourceName = sourceName;
    }

    public void Extend(Settings.Language.ILocalizationDictionary dictionary)
    {
        //Add
        Settings.Language.ILocalizationDictionary existingDictionary;
        if (!Dictionaries.TryGetValue(dictionary.CultureInfo.ToString() /*.Name*/, out existingDictionary))
        {
            Dictionaries[dictionary.CultureInfo.ToString() /*.Name*/] = dictionary;
            return;
        }

        /*/Override
        var localizedStrings = dictionary.GetAllStrings();
        foreach (var localizedString in localizedStrings)
        {
            existingDictionary[localizedString.Name] = localizedString.Value;
        }*/
    }
}

/// <summary>
/// Used to get localization dictionaries (<see cref="Settings.Language.ILocalizationDictionary"/>)
/// for a <see cref="IDictionaryBasedLocalizationSource"/>.
/// </summary>
public interface ILocalizationDictionaryProvider
{
    Settings.Language.ILocalizationDictionary DefaultDictionary { get; }

    IDictionary<string, Settings.Language.ILocalizationDictionary> Dictionaries { get; }

    void Initialize(string sourceName);

    void Extend(Settings.Language.ILocalizationDictionary dictionary);
}