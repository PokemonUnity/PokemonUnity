using System.Collections;

namespace PokemonUnity.Inventory
{
	public static class ItemExtension
	{		
		public static string ToString(this PokemonUnity.Inventory.Items item, TextScripts text)
		{
			//create a switch, and return Locale Name, Plural, or Description
			return item.ToString();
		}
	}
}