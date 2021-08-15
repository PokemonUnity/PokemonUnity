using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using Localization;
//using Localization.Dictionaries;
//using Localization.Dictionaries.Xml;
using System.IO;
using System.Xml;
using System.Collections;
//using System.Collections.Immutable;
using System.Globalization;

/// <summary>
/// </summary>
public class Translator
{
    #region Nested Classes
    /// <summary>
    /// Language class stores the user's current/active language
    /// </summary>
    public class Language
    {
        #region Extra Nested Classes
        /// <summary>
        /// Represents a localized string.
        /// </summary>
        public class LocalizedString
        {
            /// <summary>
            /// Node Type in the XML doc.
            /// </summary>
            public string NodeType { get; internal set; }
            //public System.Reflection.Assembly AssemblyInfo { get; private set; }
            //public System.Reflection.MethodBase MethodClassType { get; private set; }
            //public string[] FieldNames { get; internal set; }
            public KeyValuePair<string, string>[] FieldNames { get; internal set; }
            public int Id { get; set; }

            /// <summary>
            /// Culture info for this string.
            /// </summary>
            public Languages CultureInfo { get; internal set; }
            //public CultureInfo CultureInfo { get; internal set; }

            /// <summary>
            /// Name value of the string.
            /// </summary>
            public string Name { get; internal set; }//{ get; private set; }

            /// <summary>
            /// Unique Id of the string.
            /// </summary>
            public string Identifier { get; internal set; }//{ get; private set; }

            /// <summary>
            /// Value for the <see cref="Identifier"/>.
            /// </summary>
            public string Value { get; set; }//{ get; internal set; } { get; private set; }

            /// <summary>
            /// Creates a localized string instance.
            /// </summary>
            /// <param name="cultureInfo">Culture info for this string</param>
            /// <param name="name">Unique Name of the string</param>
            /// <param name="value">Value for the <paramref name="name"/></param>
            public LocalizedString(string name, string value, Languages cultureInfo)
            {
                Identifier = name;
                Value = value;
                //NodeType = node;
                CultureInfo = cultureInfo;
            }

            public LocalizedString(string name, LocalizedString value, Languages cultureInfo)
            {
                Identifier = name;
                Name = value.Name;
                Value = value.Value;
                NodeType = value.NodeType;
                FieldNames = value.FieldNames ?? new KeyValuePair<string, string>[0];//new string[0];
                CultureInfo = cultureInfo;
            }

            public LocalizedString(string name, Languages cultureInfo)
            {
                Identifier = name;
                CultureInfo = cultureInfo;
            }

            public LocalizedString() { }
        }

        /// <summary>
        /// Represents a simple implementation of <see cref="XmlLocalizationDictionary"/> interface.
        /// </summary>
        public class LocalizationDictionary : IEnumerable<LocalizedString> //ILocalizationDictionary, 
        {
            /// <inheritdoc/>
            public Languages CultureInfo { get; private set; }
            //public string NodeType { get; internal set; }
            //public Dictionary<string FieldNames, string FieldValues> { get; private set; }
            //public string[] FieldNames { get; internal set; }
            //public virtual IEnumerable<LocalizedString> this[string name] FieldNames { get; internal set; }

            /// <inheritdoc/>
            /*public virtual string this[string name]
            {
                get
                {
                    var localizedString = GetOrNull(name);
                    return localizedString == null ? null : localizedString.Value;
                }
                set
                {
                    _dictionary[name] = new LocalizedString(name, value, CultureInfo);
                }
            }*/
            public virtual LocalizedString this[string name]
            {
                get
                {
                    var localizedString = GetOrNull(name);
                    return localizedString == null ? null : localizedString;
                }
                set
                {
                    _dictionary[name] = new LocalizedString(name, CultureInfo);//value; new LocalizedString(name, value, CultureInfo);
                }
            }

            private readonly Dictionary<string, LocalizedString> _dictionary;


            /// <summary>
            /// Creates a new <see cref="LocalizationDictionary"/> object.
            /// </summary>
            /// <param name="cultureInfo">Culture of the dictionary</param>
            public LocalizationDictionary(Languages cultureInfo)
            {
                CultureInfo = cultureInfo;
                _dictionary = new Dictionary<string, LocalizedString>();
            }

            /// <inheritdoc/>
            public virtual LocalizedString GetOrNull(string name)
            {
                LocalizedString localizedString;
                return _dictionary.TryGetValue(name, out localizedString) ? localizedString : null;
            }

            /// <inheritdoc/>
            public virtual /*IReadOnlyList*/List<LocalizedString> GetAllStrings()
            {
                return _dictionary.Values.ToList<LocalizedString>();//.ToImmutableList();
            }

            /// <inheritdoc/>
            public virtual IEnumerator<LocalizedString> GetEnumerator()
            {
                return GetAllStrings().GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetAllStrings().GetEnumerator();
            }

            protected bool Contains(string name)
            {
                return _dictionary.ContainsKey(name);
            }
        }

        /// <summary>
        /// This class is used to build a localization dictionary from XML.
        /// </summary>
        /// <remarks>
        /// Use static Build methods to create instance of this class.
        /// </remarks>
        public class XmlLocalizationDictionary : LocalizationDictionary
        {
			#region XML English Translate Variables
			//private int nodeCount;
			//private static List<string> nodeType = new List<string>(); //nodeType.GroupBy(i => i); foreach(){g.Key, g.Count()}
			public static SortedList<string, IEnumerable<LocalizedString>> nodeType = new SortedList<string, IEnumerable<LocalizedString>>(); //{ get; set; }
            #endregion

            /// <summary>
            /// Private constructor.
            /// </summary>
            /// <param name="cultureInfo">Culture of the dictionary</param>
            private XmlLocalizationDictionary(Languages cultureInfo)
                : base(cultureInfo)
            {

            }

            /// <summary>
            /// Builds an <see cref="XmlLocalizationDictionary"/> from given file.
            /// </summary>
            /// <param name="filePath">Path of the file</param>
            public static XmlLocalizationDictionary BuildFomFile(string filePath)
            {
                try
                {
                    return BuildFomXmlString(File.ReadAllText(filePath));
                }
                catch (Exception ex)
                {
                    throw new Exception("Invalid localization file format! " + filePath, ex);
                }
            }

            /// <summary>
            /// Builds an <see cref="XmlLocalizationDictionary"/> from given xml string.
            /// </summary>
            /// <param name="xmlString">XML string</param>
            public static XmlLocalizationDictionary BuildFomXmlString(string xmlString)
            {
                var xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlString);

                var localizationDictionaryNode = xmlDocument.SelectNodes("/localizationDictionary");
                if (localizationDictionaryNode == null || localizationDictionaryNode.Count <= 0)
                {
                    throw new Exception("A Localization Xml must include localizationDictionary as root node.");
                }

                var cultureName = localizationDictionaryNode[0].GetAttributeValueOrNull("culture");
                if (string.IsNullOrEmpty(cultureName))
                {
                    throw new Exception("culture is not defined in language XML file!");
                }

                var languageInt = localizationDictionaryNode[0].GetAttributeValueOrNull("id");// "culture"
                /*if (languageInt is not an int )
                {
                    throw new Exception("Language int/enum value is not defined in language XML file!");
                }*/

                //var dictionary = new XmlLocalizationDictionary(CultureInfo.GetCultureInfo(cultureName));
                var dictionary = new XmlLocalizationDictionary((Languages)int.Parse(languageInt));

                var dublicateNames = new List<string>();
                //Make a list of all the node types
                //Maybe a dictionary<string,nodeType>?

                var textNodes = xmlDocument.SelectSingleNode("/localizationDictionary/texts").ChildNodes;// /text
                if (textNodes != null)
                {
                    foreach (XmlNode nodes in textNodes)
                    {
                        if (nodes.HasChildNodes)
                        {
							if(!nodeType.ContainsKey(nodes.Name.ToUpperInvariant())) nodeType.Add(nodes.Name.ToUpperInvariant(), new List<LocalizedString>());
                            foreach (XmlNode node in nodes)
                            {
                                if (node.HasChildNodes && node.FirstChild.NodeType == XmlNodeType.Text /*node.NodeType != XmlNodeType.Comment*/)
                                {
                                    var id = node.GetAttributeValueOrNull("identifier");// ?? node.GetAttributeValueOrNull("name");
                                    if (string.IsNullOrEmpty(id))
                                    {
                                        id = node.LocalName.ToString();
                                        //throw new Exception("name attribute of a text is empty in given xml string.");
                                    }

                                    if (dictionary.Contains(id))
                                    {
                                        dublicateNames.Add(id);
                                    }

                                    //dictionary[name] = (node.GetAttributeValueOrNull("value") ?? node.InnerText).NormalizeLineEndings();
                                    dictionary[id] = new LocalizedString() { Identifier = id };
                                    dictionary[id].Value = node.InnerText.TrimStart(new char[] { '\r', '\n' });//.NormalizeLineEndings();
                                    //dictionary[id].Name =  node.GetAttributeValueOrNull("name") ?? id;//.NormalizeLineEndings();

                                    #region MyRegion
                                    //ToDo: Maybe add a forms array, and a new method for single name calls
                                    dictionary[id].FieldNames = new KeyValuePair<string, string>[node.Attributes.Count];//new string
                                    //int n = 0;//dictionary.Forms[0] = node.Attributes["name"].Value;//that or return an empty array T[0]
                                    for (int i = 0; i < node.Attributes.Count; i++)//foreach(System.Xml.XmlAttribute attr in node)
                                    {
                                        //Skipping first 4 values will save processing
                                        /*if (node.Attributes[i].LocalName.Contains("form")) //Name vs LocalName?
                                        {
                                            //translation.Forms[i-4] = node.Attributes[i].Value; //limits xml to only 4 set values 
                                            dictionary.FieldNames[n] = node.Attributes[i].Value; n++;
                                        }*/
                                        //dictionary[name].FieldNames[i] = node.Attributes[i].Value; //n++;
                                        dictionary[id].FieldNames[i] = new KeyValuePair<string, string>(node.Attributes[i].LocalName, node.Attributes[i].Value); //n++;
                                    }

                                    //fieldArray.Add(node.LocalName.ToString());
                                    dictionary[id].NodeType = node.LocalName.ToString();
                                    //nodeType.Add(node.LocalName.ToString());
                                    #endregion
                                }
							}
							nodeType[nodes.Name.ToUpperInvariant()] = dictionary;
						}
                    }
                }

                if (dublicateNames.Count > 0)
                {
                    //throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
                }

                return dictionary;
            }
        }
        #endregion
    }
    #endregion

    #region Enumerators
    public enum Languages 
    {
        /// <summary>
        /// US English
        /// </summary>
        English = 9
	}
	/*ToDo: Convert Enum into a class 
	public class Languages //: IEnumerable
	{
		/// <summary>
		/// US English
		/// </summary>
		public const int English = 9;
	}*/
	#endregion

}

#region Extensions
/// <summary>
/// Extension methods for <see cref="XmlNode"/> class.
/// </summary>
public static class XmlNodeExtensions
{
    /// <summary>
    /// Gets an attribute's value from an Xml node.
    /// </summary>
    /// <param name="node">The Xml node</param>
    /// <param name="attributeName">Attribute name</param>
    /// <returns>Value of the attribute</returns>
    public static string GetAttributeValueOrNull(this XmlNode node, string attributeName)
    {
        if (node.Attributes == null || node.Attributes.Count <= 0)
        {
            throw new Exception(node.Name + " node has not " + attributeName + " attribute");
        }

        return node.Attributes
            .Cast<XmlAttribute>()
            .Where(attr => attr.Name == attributeName)
            .Select(attr => attr.Value)
            .FirstOrDefault();
    }
}

/// <summary> 
/// Extension methods for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Concatenates the members of a constructed <see cref="IEnumerable{T}"/> collection of type System.String, using the specified separator between each member.
    /// This is a shortcut for string.Join(...)
    /// </summary>
    /// <param name="source">A collection that contains the strings to concatenate.</param>
    /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
    /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
    public static string JoinAsString(this IEnumerable<string> source, string separator)
    {
        return string.Join(separator, source.ToArray());
    }

    /*// <summary>
    /// Concatenates the members of a collection, using the specified separator between each member.
    /// This is a shortcut for string.Join(...)
    /// </summary>
    /// <param name="source">A collection that contains the objects to concatenate.</param>
    /// <param name="separator">The string to use as a separator. separator is included in the returned string only if values has more than one element.</param>
    /// <typeparam name="T">The type of the members of values.</typeparam>
    /// <returns>A string that consists of the members of values delimited by the separator string. If values has no members, the method returns System.String.Empty.</returns>
    public static string JoinAsString<T>(this IEnumerable<T> source, string separator)
    {
        return string.Join(separator, source);
    }*/

    /// <summary>
    /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="source">Enumerable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the enumerable</param>
    /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, bool> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }

    /// <summary>
    /// Filters a <see cref="IEnumerable{T}"/> by given predicate if given condition is true.
    /// </summary>
    /// <param name="source">Enumerable to apply filtering</param>
    /// <param name="condition">A boolean value</param>
    /// <param name="predicate">Predicate to filter the enumerable</param>
    /// <returns>Filtered or not filtered enumerable based on <paramref name="condition"/></returns>
    public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, bool condition, Func<T, int, bool> predicate)
    {
        return condition
            ? source.Where(predicate)
            : source;
    }
}

/// <summary>
/// Extension methods for String class.
/// </summary>
public static class StringExtensions
{
    /*// <summary>
    /// indicates whether this string is null, empty, or consists only of white-space characters.
    /// </summary>
    public static bool IsNullOrWhiteSpace(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }*/

    /// <summary>
    /// Gets a substring of a string from beginning of the string.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
    public static string Left(this string str, int len)
    {
        if (str == null)
        {
            throw new ArgumentNullException("str");
        }

        if (str.Length < len)
        {
            throw new ArgumentException("len argument can not be bigger than given string's length!");
        }

        return str.Substring(0, len);
    }

    /// <summary>
    /// Converts line endings in the string to <see cref="Environment.NewLine"/>.
    /// </summary>
    public static string NormalizeLineEndings(this string str)
    {
        return str.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", Environment.NewLine);
    }
}
/*/[DefaultMember("Item")]
//[TypeDependencyAttribute("System.SZArrayHelper")]
public interface IReadOnlyList<out T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
{
    //
    // Summary:
    //     Gets the element at the specified index in the read-only list.
    //
    // Parameters:
    //   index:
    //     The zero-based index of the element to get.
    //
    // Returns:
    //     The element at the specified index in the read-only list.
    T this[int index] { get; }
}
//[TypeDependencyAttribute("System.SZArrayHelper")]
public interface IReadOnlyCollection<out T> : IEnumerable<T>, IEnumerable
{
    //
    // Summary:
    //     Gets the number of elements in the collection.
    //
    // Returns:
    //     The number of elements in the collection.
    int Count { get; }
}*/

/*public static class CollectionEx
{
    public static IReadOnlyList<T> AsReadOnly<T>(this IList<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException("collection");

        // I now check whether it already is IReadOnlyList<T>...
        // ...like casperOne has suggested.
        return collection as IReadOnlyList<T>
            ?? new ReadOnlyWrapper<T>(collection);
    }

    private sealed class ReadOnlyWrapper<T> : IReadOnlyList<T>
    {
        private readonly IList<T> source;

        public int Count { get { return this.source.Count; } }
        public T this[int index] { get { return this.source[index]; } }

        public ReadOnlyWrapper(IList<T> source) { this.source = source; }

        public IEnumerator<T> GetEnumerator() { return this.source.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }
    }
}*/
#endregion

#region MyRegion
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
			var nodeDic = new List<KeyValuePair<string, Translator.Language.XmlLocalizationDictionary>>() { new KeyValuePair<string, Translator.Language.XmlLocalizationDictionary>("", CreateXmlLocalizationDictionary(fileName)) }; 
            if (Dictionaries.ContainsKey(dictionary.CultureInfo.ToString() /*.Name*/))
            {
                throw new Exception(sourceName + " source contains more than one dictionary for the culture: " + dictionary.CultureInfo.ToString() /*.Name*/);
            }

            Dictionaries[dictionary.CultureInfo.ToString() /*.Name*/] = (Translator.Language.XmlLocalizationDictionary)dictionary;
            NodeDictionaries[dictionary.CultureInfo.ToString() /*.Name*/] = nodeDic;

            if (fileName.EndsWith(sourceName + ".xml"))
            {
                if (DefaultDictionary != null)
                {
                    throw new Exception("Only one default localization dictionary can be for source: " + sourceName);
                }

                DefaultDictionary = (Translator.Language.XmlLocalizationDictionary)dictionary;
                DefaultNodeDictionary = nodeDic;
            }
        }
    }

    protected virtual Translator.Language.XmlLocalizationDictionary CreateXmlLocalizationDictionary(string fileName)
    {
        return Translator.Language.XmlLocalizationDictionary.BuildFomFile(fileName);
    }
    /*protected virtual KeyValuePair<string, Translator.Language.XmlLocalizationDictionary> CreateNodeXmlLocalizationDictionary(string fileName)
    {
		//KeyValuePair<string, Translator.Language.XmlLocalizationDictionary> x =
		return new KeyValuePair<string, Translator.Language.XmlLocalizationDictionary>("",
			Translator.Language.XmlLocalizationDictionary.BuildFomFile(fileName));
    }*/
}

public abstract class LocalizationDictionaryProviderBase : ILocalizationDictionaryProvider
{
    public string SourceName { get; private set; }

    public Translator.Language.XmlLocalizationDictionary DefaultDictionary { get; protected set; }
    public IEnumerable<KeyValuePair<string, Translator.Language.XmlLocalizationDictionary>> DefaultNodeDictionary { get; protected set; }

    public IDictionary<string, Translator.Language.XmlLocalizationDictionary> Dictionaries { get; private set; }
    public IDictionary<string, IEnumerable<KeyValuePair<string, Translator.Language.XmlLocalizationDictionary>>> NodeDictionaries { get; private set; }

    protected LocalizationDictionaryProviderBase()
    {
        Dictionaries = new Dictionary<string, Translator.Language.XmlLocalizationDictionary>();
        NodeDictionaries = new Dictionary<string, IEnumerable<KeyValuePair<string, Translator.Language.XmlLocalizationDictionary>>>();
    }

    public virtual void Initialize(string sourceName)
    {
        SourceName = sourceName;
    }

    public void Extend(Translator.Language.XmlLocalizationDictionary dictionary)
    {
        //Add
        Translator.Language.XmlLocalizationDictionary existingDictionary;
        if (!Dictionaries.TryGetValue(dictionary.CultureInfo.ToString() /*.Name*/, out existingDictionary))
        {
            Dictionaries[dictionary.CultureInfo.ToString() /*.Name*/] = dictionary;
            return;
        }

        //Override
        var localizedStrings = dictionary.GetAllStrings();
        foreach (var localizedString in localizedStrings)
        {
            existingDictionary[localizedString.Identifier] = localizedString;//localizedString.Value
        }
    }
}

/// <summary>
/// Used to get localization dictionaries (<see cref="Translator.Language.XmlLocalizationDictionary"/>)
/// for a <see cref="IDictionaryBasedLocalizationSource"/>.
/// </summary>
public interface ILocalizationDictionaryProvider
{
    Translator.Language.XmlLocalizationDictionary DefaultDictionary { get; }

    IDictionary<string, Translator.Language.XmlLocalizationDictionary> Dictionaries { get; }

    void Initialize(string sourceName);

    void Extend(Translator.Language.XmlLocalizationDictionary dictionary);
}

/*class Base<T> where T : struct
{
    private T value;
    public void Foo(T value)
    {
        this.value = value;
    }
}
public interface ILanguage
{
    ILanguage ILanguage { get; set; }
    //ILanguage ILanguage(Enum @Language);
}*/
public interface ILanguage<T>
{
    T ILanguage { get; set; }
}
#endregion