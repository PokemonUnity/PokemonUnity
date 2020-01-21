using System.Collections;
using PokemonUnity;
using PokemonUnity.Inventory;

namespace PokemonUnity.Inventory//.ItemData
{
	public struct ItemFlag
	{
		/// <summary>
		/// Has a count in the bag
		/// </summary>
		public bool Countable { get; private set; }
		/// <summary>
		/// Consumed when used
		/// </summary>
		public bool Consumable { get; private set; }
		/// <summary>
		/// Usable outside battle
		/// </summary>
		public bool Useable_Overworld { get; private set; }
		/// <summary>
		/// Usable in battle
		/// </summary>
		public bool Useable_In_Battle { get; private set; }
		/// <summary>
		/// Can be held by a pokemon
		/// </summary>
		public bool Holdable { get; private set; }
		/// <summary>
		/// Works passively when held
		/// </summary>
		public bool Holdable_Passive { get; private set; }
		/// <summary>
		/// Usable by a pokemon when held
		/// </summary>
		public bool Holdable_Active { get; private set; }
		/// <summary>
		/// Appears in Sinnoh Underground
		/// </summary>
		public bool Underground { get; private set; }

		public ItemFlag(bool countable = false, bool consumable = false, bool useableOverworld = false,
			bool useableBattle = false, bool holdable = false, bool holdablePassive = false, bool holdableActive = false, bool underground = false)
		{
			Countable = countable;
			Consumable = consumable;
			Useable_Overworld = useableOverworld;
			Useable_In_Battle = useableBattle;
			Holdable = holdable;
			Holdable_Passive = holdablePassive;
			Holdable_Active = holdableActive;
			Underground = underground;
		}
	}
}