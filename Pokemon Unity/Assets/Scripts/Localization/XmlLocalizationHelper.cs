//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Xml;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
    /*// <summary>
    /// XML 格式的本地化辅助器。
    /// </summary>
    public class XmlLocalizationHelper : DefaultLocalizationHelper
    {
        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public override bool ParseDictionary(string text, object userData)
        {
            try
            {
                string currentLanguage = GameEntry.Localization.Language.ToString();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(text);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("Dictionaries");
                XmlNodeList xmlNodeDictionaryList = xmlRoot.ChildNodes;
                for (int i = 0; i < xmlNodeDictionaryList.Count; i++)
                {
                    XmlNode xmlNodeDictionary = xmlNodeDictionaryList.Item(i);
                    if (xmlNodeDictionary.Name != "Dictionary")
                    {
                        continue;
                    }

                    string language = xmlNodeDictionary.Attributes.GetNamedItem("Language").Value;
                    if (language != currentLanguage)
                    {
                        continue;
                    }

                    XmlNodeList xmlNodeStringList = xmlNodeDictionary.ChildNodes;
                    for (int j = 0; j < xmlNodeStringList.Count; j++)
                    {
                        XmlNode xmlNodeString = xmlNodeStringList.Item(j);
                        if (xmlNodeString.Name != "String")
                        {
                            continue;
                        }

                        string key = xmlNodeString.Attributes.GetNamedItem("Key").Value;
                        string value = xmlNodeString.Attributes.GetNamedItem("Value").Value;
                        if (!AddRawString(key, value))
                        {
                            Log.Warning("Can not add raw string with key '{0}' which may be invalid or duplicate.", key);
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary '{0}' with exception '{1}'.", text, exception.ToString());
                return false;
            }
        }
    }*/
}
