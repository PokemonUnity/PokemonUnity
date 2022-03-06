using System.Collections;

namespace PokemonUnity
{
	public static class ItemExtension
	{
		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		/// <param name="text"></param>
		/// <returns>Returns the lookup identifier for localization dictionary</returns>
		/// <remarks>Requires <seealso cref="Game._INTL(string, object[])"/> to retrieve actual value</remarks>
		public static string ToString(this PokemonUnity.Inventory.Items item, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("ITEM_NAME_{0}", (int)item);
			if (text == TextScripts.Description)
				return string.Format("ITEM_DESC_{0}", (int)item);
			//create a switch, and return Locale Name, Plural, or Description
			return item.ToString();
		}
		/// <summary>
		/// </summary>
		/// <param name="pocket"></param>
		/// <param name="text"></param>
		/// <returns>Returns the lookup identifier for localization dictionary</returns>
		/// <remarks>Requires <seealso cref="Game._INTL(string, object[])"/> to retrieve actual value</remarks>
		public static string ToString(this PokemonUnity.Inventory.ItemPockets pocket, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("POCKET_NAME_{0}", (int)pocket);
			if (text == TextScripts.Description)
				return string.Format("POCKET_DESC_{0}", (int)pocket);
			//create a switch, and return Locale Name, Plural, or Description
			//GetItemPocketChar + string?
			return pocket.ToString();
		}
	}
}