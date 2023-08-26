using System.Collections.Generic;
using System.Xml;
using PokemonUnity.Monster;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Utility;

namespace PokemonUnity.Localization
{
	/// <summary>
	/// </summary>
	public class XmlStringRes
	{
		//private static SortedList<string, IEnumerable<string>> nodeType = new SortedList<string, IEnumerable<string>>();
		private IDictionary<string, string> stringMap;
		private string nodeElement;
		private string nodeAttribute;
		private IDebugger _logger;

		public XmlStringRes(IDebugger logger, string element = "STR", string attribute = "id")
		{
			stringMap = new Dictionary<string, string>();
			nodeElement = element;
			nodeAttribute = attribute;
			_logger = logger;
		}

		public bool Initialize(string fileName, int languageId = 0)
		{
			if (string.IsNullOrEmpty(fileName)) return false;

			//var fileNames = System.IO.Directory.GetFiles(_directoryPath, "*.xml", SearchOption.TopDirectoryOnly);
			if (!System.IO.File.Exists(fileName)) return false;

			_logger?.Log("Load XML to memory : {0} ({1})", fileName, languageId);
			string xmlString = System.IO.File.ReadAllText(fileName);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xmlString);
			_logger?.Log("SUCCESS");

			var dublicateNames = new List<string>();

			//Make a list of all the node types
			//XmlNodeList textNodes = xmlDocument.SelectSingleNode("/localizationDictionary/texts").ChildNodes; // returns `/text` nodes
			XmlNodeList textNodes = xmlDocument.ChildNodes;
			//XmlNodeList textNodes = xmlDocument.GetElementsByTagName(nodeElement);
			if (textNodes != null)
			{
				foreach (XmlNode nodes in textNodes)
				{
					if (nodes.HasChildNodes)
					{
						//if (!nodeType.ContainsKey(nodes.Name.ToUpperInvariant())) nodeType.Add(nodes.Name.ToUpperInvariant(), new List<string>());
						foreach (XmlNode node in nodes)
						{
							if ((nodeElement == node.LocalName.ToString() || "MSG" == node.LocalName.ToString() || "MESSAGE" == node.LocalName.ToString()) &&
								node.HasChildNodes && node.FirstChild.NodeType == XmlNodeType.Text /*node.NodeType != XmlNodeType.Comment*/)
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

								#region MyRegion
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
							}
						}
						//nodeType[nodes.Name.ToUpperInvariant()] = stringMap;
					}
				}
			}

			if (dublicateNames.Count > 0)
			{
				//throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
				_logger.LogWarning("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));
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
			_logger?.Log("Identifier `{0}` was not found in Localization dictionary", code);
			return code;
		}
	}
}