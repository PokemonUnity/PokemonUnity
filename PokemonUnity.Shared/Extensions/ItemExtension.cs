using System.Collections;

namespace PokemonUnity
{
	public static class ItemExtension
	{
		public static string ToString(this PokemonUnity.Inventory.Items item, TextScripts text)
		{
			//create a switch, and return Locale Name, Plural, or Description
			return item.ToString();
		}
		public static string ToString(this PokemonUnity.Inventory.ItemPockets item, TextScripts text)
		{
			//create a switch, and return Locale Name, Plural, or Description
			//GetItemPocketChar + string?
			return item.ToString();
		}
	}
}