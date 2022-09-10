using System;
using System.Linq;

namespace PokemonUnity.Utility
{
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
		public static string GetAttributeValueOrNull(this System.Xml.XmlNode node, string attributeName)
		{
			if (node.Attributes == null || node.Attributes.Count <= 0)
			{
				throw new Exception(node.Name + " node has not " + attributeName + " attribute");
			}

			return node.Attributes
				.Cast<System.Xml.XmlAttribute>()
				.Where(attr => attr.Name == attributeName)
				.Select(attr => attr.Value)
				.FirstOrDefault();
		}
	}
}