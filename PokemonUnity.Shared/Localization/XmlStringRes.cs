using System.Collections.Generic;
using System.Xml;
using PokemonUnity.Monster;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Utility;
using System.Linq;

namespace PokemonUnity.Localization
{
	/// <summary>
	/// Localization Dictionary of Text Strings
	/// </summary>
	public class XmlStringRes //ToDo: Rename to XmlStringResource
	{
		public Languages Language { get { return language; } }
		/// <summary>
		/// </summary>
		/// <remarks>
		/// Assigned based on the file loaded.
		/// </remarks>
		private Languages language;
		//private static SortedList<string, IEnumerable<string>> nodeType = new SortedList<string, IEnumerable<string>>();
		private IDictionary<string, string> stringMap;
		private string nodeElement;
		private string nodeAttribute;

		public XmlStringRes(string element = "STR", string attribute = "id")
		{
			stringMap = new Dictionary<string, string>();
			nodeElement = element;
			nodeAttribute = attribute;
		}

		public bool Initialize(string fileName, int languageId = 0)
		{
			if (string.IsNullOrEmpty(fileName)) return false;

			if (System.IO.Path.GetInvalidPathChars().Any(invalid => fileName.Contains(invalid))) return false;

			if (System.IO.Path.GetInvalidFileNameChars().Any(invalid => fileName.Contains(invalid))) return false;

			//var fileNames = System.IO.Directory.GetFiles(_directoryPath, "*.xml", SearchOption.TopDirectoryOnly);
			if (!System.IO.File.Exists(fileName)) return false;

			Core.Logger.Log("Load XML to memory : {0} ({1}:{2})", System.IO.Path.GetFullPath(fileName), languageId, ((Languages)languageId).ToString());
			string xmlString = System.IO.File.ReadAllText(fileName);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlString);
			Core.Logger.Log("Localization text was successfully found and contents scanned.");

			var dublicateNames = new List<string>();

			//Make a list of all the node types
			//XmlNodeList textNodes = xmlDocument.SelectSingleNode("/localizationDictionary/texts").ChildNodes; // returns `/text` nodes
			XmlNodeList textNodes = xmlDocument.ChildNodes;
			//XmlNodeList textNodes = xmlDocument.GetElementsByTagName(nodeElement);
			if (textNodes != null)
			{
				Core.Logger.Log("Localization file contains text strings...");
				foreach (XmlNode nodes in textNodes)
				{
					Core.Logger.Log("Localization file text contains header: `{0}` ", nodes.LocalName);
					if (nodes.HasChildNodes)
					{
						//if (!nodeType.ContainsKey(nodes.Name.ToUpperInvariant())) nodeType.Add(nodes.Name.ToUpperInvariant(), new List<string>());
						foreach (XmlNode node in nodes)
						{
							if ((nodeElement == node.LocalName.ToString() || "MSG" == node.LocalName.ToString() || "MESSAGE" == node.LocalName.ToString()) &&
								node.HasChildNodes && node.FirstChild.NodeType == XmlNodeType.Text) //node.NodeType != XmlNodeType.Comment
							{
								var id = node.GetAttributeValueOrNull(nodeAttribute);// ?? node.GetAttributeValueOrNull("name");
								if (string.IsNullOrEmpty(id))
								{
									continue;
									//id = node.LocalName.ToString();
									//throw new Exception("name attribute of a text is empty in given xml string.");
								}

								if (stringMap.ContainsKey(id))
								{
									dublicateNames.Add(id);
								}

								//stringMap[name] = (node.GetAttributeValueOrNull("value") ?? node.InnerText).NormalizeLineEndings();
								//stringMap[id] = new LocalizedString() { Identifier = id };
								//stringMap[id].Value = node.InnerText.TrimStart(new char[] { '\r', '\n' });//.NormalizeLineEndings();
								//stringMap[id].Name =  node.GetAttributeValueOrNull("name") ?? id;//.NormalizeLineEndings();
								stringMap.Add(id, node.InnerText.TrimStart(new char[] { '\r', '\n' }));//.NormalizeLineEndings();

								#region Localization Text for Pokemon Alternate Forms
								/* ToDo: Maybe add a forms array, and a new method for single name calls
								stringMap[id].FieldNames = new KeyValuePair<string, string>[node.Attributes.Count];//new string
								//int n = 0;//stringMap.Forms[0] = node.Attributes["name"].Value;//that or return an empty array T[0]
								for (int i = 0; i < node.Attributes.Count; i++)//foreach(System.Xml.XmlAttribute attr in node)
								{
									//Skipping first 4 values will save processing
									//if (node.Attributes[i].LocalName.Contains("form")) //Name vs LocalName?
									//{
									//	//translation.Forms[i-4] = node.Attributes[i].Value; //limits xml to only 4 set values
									//	stringMap.FieldNames[n] = node.Attributes[i].Value; n++;
									//}
									//stringMap[name].FieldNames[i] = node.Attributes[i].Value; //n++;
									stringMap[id].FieldNames[i] = new KeyValuePair<string, string>(node.Attributes[i].LocalName, node.Attributes[i].Value); //n++;
								}

								//fieldArray.Add(node.LocalName.ToString());
								stringMap[id].NodeType = node.LocalName.ToString();
								//nodeType.Add(node.LocalName.ToString());*/
								#endregion

								//ToDo: Moves, Types, Nature, Abilities, Items, Game Text and Resource Files (Images=>Different Paths) etc
							}
						}
						//nodeType[nodes.Name.ToUpperInvariant()] = stringMap;
					}
				}
			}

			if (dublicateNames.Count > 0)
			{
				//throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
				Core.Logger.LogWarning("A dictionary can not contain same key twice. There are some duplicated names: {0}", dublicateNames.JoinAsString(", "));
			}

			return true;
		}

		public void Release()
		{
			stringMap.Clear();
		}

		public string GetStr(string code)
		{
			if (stringMap.ContainsKey(code))
				return stringMap[code];
			Core.Logger.LogDebug("Identifier `{0}` was not found in Localization dictionary", code); //Log as Warning?
			return code;
		}
	}
}