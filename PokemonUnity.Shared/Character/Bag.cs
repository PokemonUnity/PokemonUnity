using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;

namespace PokemonUnity.Character
{	
	/// <summary>
	/// Collection of KeyValuePairs grouped by Items.
	/// Used in combination with quantity[], 
	/// one holds the itemId (Key) and the other 
	/// has amount (value).
	/// </summary>
	/// I'd feel more comfortable if instead of {get;set;}
	/// it was a {get;} only, that returned an iqueryable
	/// ToDo: remove static <see cref="GameVariables.Bag_Items"/>
	/// so trainerbag can load multiple players (for netplay)
	public class Bag
	{
		#region Variables
		private List<Items> items { get; set; }
		private ItemPockets pocket { get; set; }
		private bool reverseOrder { get; set; }
		//private Items[] Items { get; set; }
		public Items[] Contents { get { return items.ToArray(); } }
		public SortedList<Items, byte> this[ItemPockets pocket]
		{
			get
			{
				//All items found in the bag
				List<Items> bag = new List<Items>();
				//List of items that belong in this pocket, with quantity found
				SortedList<Items, byte> items = new SortedList<Items, byte>();
				//foreach (KeyValuePair<Items, byte> Item in Game.GameData.Bag_Items)
				foreach (Items Item in this.items)
				{
					//Item item = global::Item.GetItem(Item.Key);
					ItemData item = Game.ItemData[Item];
					if(Item != Items.NONE)
					switch (item.Pocket)
					{
						case ItemPockets.MISC:
							if(pocket == ItemPockets.MISC)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.MEDICINE:
							if (pocket == ItemPockets.MEDICINE)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.POKEBALL:
							if (pocket == ItemPockets.POKEBALL)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.MACHINE:
							if (pocket == ItemPockets.MACHINE)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.BERRY:
							if (pocket == ItemPockets.BERRY)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.MAIL:
							if (pocket == ItemPockets.MAIL)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.BATTLE:
							if (pocket == ItemPockets.BATTLE)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						case ItemPockets.KEY:
							if (pocket == ItemPockets.KEY)
								if (!bag.Contains(Item))
								{
									items.Add(Item, (byte)this.items.Count(i => i == Item));
									bag.Add(Item);
								}
							break;
						default:
							break;
					}
				};
				return items;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		/// ToDo: Return KeyValuePair<Item, Quantity>?
		public ItemData this[Items item]
		{
			get
			{
				return Game.ItemData[items.DefaultIfEmpty(Items.NONE).FirstOrDefault(i => i == item)];
			}
		}
		#endregion

		#region Constructors
		public Bag() //: this()
		{
			//items = new Items[0];
			items = new List<Items>();
		}

		public Bag(Items[] content) : this()
		{
			//items = content;
			items.AddRange(content);
		}
		#endregion

		#region Methods
		//public void GetBag()
		//{
		//	Misc = Medicine = Pokeball = Machine = Berry = Mail = Battle = Key = new SortedList<Item, byte>();
		//	List<Items> misc, medicine, pokeball, machine, berry, mail, battle, key;// = new List<Item>();
		//	misc = medicine = pokeball = machine = berry = mail = battle = key = new List<Items>();
		//	//orderString = new string[ItemDatabaseOld.getItemsLength()];
		//	quantity = new int[Bag_Items.Count];
		//	//foreach (KeyValuePair<Items, byte> Item in Game.GameData.Bag_Items)
		//	foreach (Items Item in Game.GameData.Bag_Items)
		//	{
		//		//Item item = global::Item.GetItem(Item.Key);
		//		Item item = PokemonUnity.Item.Item.GetItem(Item);
		//		switch (item.ItemPocket)
		//		{
		//			case ItemPockets.MISC:
		//				if (!misc.Contains(Item))
		//				{
		//					Misc.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					misc.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.MEDICINE:
		//				if (!machine.Contains(Item))
		//				{
		//					Medicine.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					machine.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.POKEBALL:
		//				if (!pokeball.Contains(Item))
		//				{
		//					Pokeball.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					pokeball.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.MACHINE:
		//				if (!machine.Contains(Item))
		//				{
		//					Machine.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					machine.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.BERRY:
		//				if (!berry.Contains(Item))
		//				{
		//					Berry.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					berry.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.MAIL:
		//				if (!mail.Contains(Item))
		//				{
		//					Mail.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					mail.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.BATTLE:
		//				if (!battle.Contains(Item))
		//				{
		//					Battle.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					battle.Add(Item);
		//				}
		//				break;
		//			case ItemPockets.KEY:
		//				if (!key.Contains(Item))
		//				{
		//					Key.Add(item, (byte)Game.GameData.Bag_Items.Count(i => i == Item));
		//					key.Add(Item);
		//				}
		//				break;
		//			default:
		//				break;
		//		}
		//	}
		//}

		public KeyValuePair<Items, byte>[] Sort(Order by)
		{
			IQueryable<KeyValuePair<Items, byte>> items = this[pocket].AsQueryable();
			switch (by)
			{
				case Order.Alphabet:
					reverseOrder = !reverseOrder;
					//return this.reverseOrder? items.OrderBy(x => Game.ItemData[x.Key].Name).ToArray() : items.OrderByDescending(x => Game.ItemData[x.Key].Name).ToArray();
					return this.reverseOrder? items.OrderBy(x => Game.ItemData[x.Key].Id.ToString()).ToArray() : items.OrderByDescending(x => Game.ItemData[x.Key].Id.ToString()).ToArray();
				case Order.Quantity:
					reverseOrder = !reverseOrder;
					return this.reverseOrder? items.OrderBy(x => x.Value).ToArray() : items.OrderByDescending(x => x.Value).ToArray();
				case Order.Price:
					reverseOrder = !reverseOrder;
					return this.reverseOrder? items.OrderBy(x => Game.ItemData[x.Key].Price).ToArray() : items.OrderByDescending(x => Game.ItemData[x.Key].Price).ToArray();
				case Order.Category:
					reverseOrder = !reverseOrder;
					return this.reverseOrder? items.OrderBy(x => Game.ItemData[x.Key].Category).ToArray() : items.OrderByDescending(x => Game.ItemData[x.Key].Category).ToArray();
				default:
					break;
			}
			return items.ToArray();
		}
		 
		/// <summary>
		/// If the player has the <see cref="Items.MEGA_BRACELET"/> in their inventory.
		/// </summary>
		/// <returns></returns>
		public bool HasMegaBracelet()
		{
			if (Game.GameData.Features.SandBoxMode || Core.DEBUG)
				return true;
			else
				if (items.Contains(Items.MEGA_BRACELET))
					return true;
			return false;
		}

		/// <summary>
		/// If the player has the <see cref="Items.RUNNING_SHOES"/> in their inventory.
		/// </summary>
		/// <returns></returns>
		/// `Items.RUNNING_SHOES` Does not exist... Use => `Game.GameData.HasRunningShoes()` instead?
		/// newer gens automatically give at start, so default is true...
		public bool HasRunningShoes()
		{
			//if (!items.Contains(Items.RUNNING_SHOES)) 
			//	return false;

			return true;
		}

		/// <summary>
		/// Adds items to the inventory.
		/// </summary>
		/// <param name="item">The ID of the item.</param>
		/// <param name="quantity">Amount of items to add.</param>
		public void AddItem(Items item, int quantity = 1)
		{
			for (int i = 0; i < quantity; i++)
			{
				//Game.GameData.Bag_Items.Add(item);
				items.Add(item);
			}
		}

		/// <summary>
		/// Removes items from the inventory.
		/// </summary>
		/// <param name="item">The ID of the item to remove.</param>
		/// <param name="quantity">The amount of items to remove.</param>
		public void RemoveItem(Items item, int quantity = 1)
		{
			if(quantity > 0)
				for (int i = 0; i < quantity; i++)
				{
					//if(Game.GameData.Bag_Items.Contains(item))
					//	Game.GameData.Bag_Items.Remove(item);
					if(items.Contains(item))
						items.Remove(item);
				}
		}

		/// <summary>
		/// Returns the count of the item in the inventory.
		/// </summary>
		/// <param name="item">The ID of the item to be counted.</param>
		/// <returns></returns>
		public int GetItemAmount(Items item)
		{
			return items.Where(x => x == item).Count();
		}
		/// <summary>
		/// Separates Items in Misc Pocket into groups of [slot limit]
		/// and counts it.
		/// </summary>
		/// <returns># of active item slots in misc pocket</returns>
		public int GetMiscItems()
		{
			double total = 0;
			foreach (Items item in items.Distinct().Where(x => Game.ItemData[x].Pocket == ItemPockets.MISC))
			{
				int count = GetItemAmount(item);
				int groups = (int)Math.Floor(total / Core.BAGMAXPERSLOT);
				int leftovers = (int)total % Core.BAGMAXPERSLOT > 0 ? 1 : 0;
				total += groups + leftovers;
			}
			return (int)total;
		}
		public bool CanStore(Items item)
		{
			//Sort items into groups
			//Check if item has storage limit
			//Check if storage limit is met
			//ToDo: If pocket is full, cannot hold any more items.
			//ToDo: if individual item capicity is reached, reject add
			if (Game.ItemData[item].Pocket == ItemPockets.MISC)
			{
				if (Core.MAXPOCKETSIZE[(int)ItemPockets.MISC] == -1) return true;
				//get item slots used / out of / item slots available
				int slotsUsed = GetMiscItems();
				//If item is already in bag, and slots are full...
				int count = GetItemAmount(item);
				//If pocket does not have anymore space...
				int spaceLeft = count % Core.MAXPOCKETSIZE[(int)ItemPockets.MISC];
				return spaceLeft > 0;
			}
			return false;
		}

		/// <summary>
		/// Returns a message that displays the event of putting an item into the inventory.
		/// </summary>
		/// <param name="item">The Item to store in the inventory.</param>
		/// <param name="quantity">The amount.</param>
		/// <returns></returns>
		public void GetMessageReceive(Items item, IPokeBattle_Scene scene, int quantity = 1)
		{
			string Message = "";
			if (quantity == 1)
				Message = Game.GameData.Player.Name + " stored it in the~" + Game.ItemData[item].Pocket.ToString() + " pocket.";
			else
				Message = Game.GameData.Player.Name + " stored them~in the " + Game.ItemData[item].Pocket.ToString() + " pocket.";
			scene.pbDisplayMessage(Message);
		}

		public enum Order
		{
			Alphabet,
			Quantity, 
			Category,
			Price
		}
		#endregion
	}
}