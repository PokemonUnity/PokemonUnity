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

namespace PokemonUnity
{	
	public partial class Game
	{
		/// <summary>
		/// Collection of KeyValuePairs grouped by Pockets.
		/// Used in combination with quantity[], 
		/// one holds the itemId (Key) and the other 
		/// has amount (value).
		/// </summary>
		/// I'd feel more comfortable if instead of {get;set;}
		/// it was a {get;} only, that returned an iqueryable
		/// ToDo: remove static <see cref="GameVariables.Bag_Items"/>
		/// so trainerbag can load multiple players (for netplay)
		///ToDo: Pokemonunity.Character.Bag?...
		public class TrainerBag
		{
			private Player trainer { get; set; }
			/*// <remarks>if use <see cref="Items"/> might be less on memory</remarks>
			/// <see cref="Items"/> stores quantity value*/
			//public List<Item> Items { get { return trainer.Bag_Items; } }
			//public SortedList<Item, byte> Misc { get; private set; }
			//public SortedList<Item, byte> Medicine { get; private set; }
			//public SortedList<Item, byte> Pokeball { get; private set; }
			//public SortedList<Item, byte> Machine { get; private set; }
			//public SortedList<Item, byte> Berry { get; private set; }
			//public SortedList<Item, byte> Mail { get; private set; }
			//public SortedList<Item, byte> Battle { get; private set; }
			//public SortedList<Item, byte> Key { get; private set; }
			public SortedList<Items, byte> this[ItemPockets pocket]  {
				get
				{
					//All items found in the bag
					List<Items> bag = new List<Items>();
					//List of items that belong in this pocket, with quantity found
					SortedList<Items, byte> items = new SortedList<Items, byte>();
					//foreach (KeyValuePair<Items, byte> Item in Game.Bag_Items)
					foreach (Items Item in Game.Bag_Items)
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
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.MEDICINE:
								if (pocket == ItemPockets.MEDICINE)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.POKEBALL:
								if (pocket == ItemPockets.POKEBALL)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.MACHINE:
								if (pocket == ItemPockets.MACHINE)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.BERRY:
								if (pocket == ItemPockets.BERRY)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.MAIL:
								if (pocket == ItemPockets.MAIL)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.BATTLE:
								if (pocket == ItemPockets.BATTLE)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
										bag.Add(Item);
									}
								break;
							case ItemPockets.KEY:
								if (pocket == ItemPockets.KEY)
									if (!bag.Contains(Item))
									{
										items.Add(Item, (byte)Game.Bag_Items.Count(i => i == Item));
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
			//private int[] quantity { get; set; }
			private ItemPockets pocket { get; set; }
			private bool reverseOrder { get; set; }

			/// <summary>
			/// 
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			/// ToDo: Return KeyValuePair<Item, Quantity>?
			public ItemData this[Items item]
			{
				get
				{
					return Game.ItemData[Game.Bag_Items.DefaultIfEmpty(Items.NONE).FirstOrDefault(i => i == item)];
				}
			}

			public ItemData this[ItemData item]
			{
				get
				{
					return this[item.Id];
				}
			}

			public TrainerBag(Player t) //: this()
			{
				//quantity = new int[Bag_Items.Count];
				trainer = t;
			}

			//public TrainerBag()
			//{
			//	Misc = Medicine = Pokeball = Machine = Berry = Mail = Battle = Key = new SortedList<Item, byte>();
			//	GetBag();
			//}
			//
			//public void GetBag()
			//{
			//	Misc = Medicine = Pokeball = Machine = Berry = Mail = Battle = Key = new SortedList<Item, byte>();
			//	List<Items> misc, medicine, pokeball, machine, berry, mail, battle, key;// = new List<Item>();
			//	misc = medicine = pokeball = machine = berry = mail = battle = key = new List<Items>();
			//	//orderString = new string[ItemDatabaseOld.getItemsLength()];
			//	quantity = new int[Bag_Items.Count];
			//	//foreach (KeyValuePair<Items, byte> Item in Game.Bag_Items)
			//	foreach (Items Item in Game.Bag_Items)
			//	{
			//		//Item item = global::Item.GetItem(Item.Key);
			//		Item item = PokemonUnity.Item.Item.GetItem(Item);
			//		switch (item.ItemPocket)
			//		{
			//			case ItemPockets.MISC:
			//				if (!misc.Contains(Item))
			//				{
			//					Misc.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					misc.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.MEDICINE:
			//				if (!machine.Contains(Item))
			//				{
			//					Medicine.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					machine.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.POKEBALL:
			//				if (!pokeball.Contains(Item))
			//				{
			//					Pokeball.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					pokeball.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.MACHINE:
			//				if (!machine.Contains(Item))
			//				{
			//					Machine.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					machine.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.BERRY:
			//				if (!berry.Contains(Item))
			//				{
			//					Berry.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					berry.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.MAIL:
			//				if (!mail.Contains(Item))
			//				{
			//					Mail.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					mail.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.BATTLE:
			//				if (!battle.Contains(Item))
			//				{
			//					Battle.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					battle.Add(Item);
			//				}
			//				break;
			//			case ItemPockets.KEY:
			//				if (!key.Contains(Item))
			//				{
			//					Key.Add(item, (byte)Game.Bag_Items.Count(i => i == Item));
			//					key.Add(Item);
			//				}
			//				break;
			//			default:
			//				break;
			//		}
			//	}
			//}

			/*public int getIndexOf(Item name)
			{
				for (int i = 0; i < order.Length; i++)
				{
					if (order[i] == name)
					{
						return i;
					}
				}
				return -1;
			}

			public void moveBehind(int targetIndex, int destinationIndex)
			{
				string temp = orderString[targetIndex];
				string[] packedOrder = new string[orderString.Length];

				orderString[targetIndex] = null;

				if (Mathf.Abs(targetIndex - destinationIndex) == 1)
				{
					orderString[targetIndex] = orderString[destinationIndex];
					orderString[destinationIndex] = temp;
				}
				else
				{
					int packedIndex = 0;
					for (int i = 0; i < orderString.Length; i++)
					{
						if (i == destinationIndex)
						{
							packedOrder[packedIndex] = temp;
							packedIndex += 1;
						}
						if (orderString[i] != null)
						{
							packedOrder[packedIndex] = orderString[i];
							packedIndex += 1;
						}
					}
					orderString = packedOrder;
				}
			}

			public void packOrder()
			{
				string[] packedOrder = new string[orderString.Length];
				int packedIndex = 0;
				for (int i = 0; i < orderString.Length; i++)
				{
					if (orderString[i] != null)
					{
						packedOrder[packedIndex] = orderString[i];
						packedIndex += 1;
					}
				}
				orderString = packedOrder;
			}

			///ToDo: Add to global bag, then use GetBag to pack and sort
			/// Max int count for a byte value is 255... Bag can hold more, but will only display max of 255?
			public bool addItem(Item itemName)
			{
				//returns false if will exceed the quantity limit (999)
				//packOrder();
				//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
				int index = getIndexOf(itemName);
				if (index == -1)
				{
					//item does not exist in bag, add it to the end
					index = getLength();
					order[index] = itemName;
				}
				index = Item.getIndexOf(order[index]);
				if (quantity[index] + itemName.Quantity > 999)
				{
					return false;
				}
				quantity[index] += amount;
				return true;
			}

			public bool addItem(Items itemName, int amount = 1)
			{
				//returns false if will exceed the quantity limit (999)
				packOrder();
				//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
				int index = getIndexOf(itemName);
				if (index == -1)
				{
					//item does not exist in bag, add it to the end
					index = getLength();
					order[index] = ItemDatabaseOld.getItem(itemName);
				}
				index = ItemDatabaseOld.getIndexOf(order[index]);
				if (quantity[index] + amount > 999)
				{
					return false;
				}
				quantity[index] += amount;
				return true;
			}

			public bool removeItem(Item itemName, int amount = 1) //ToDo: amount should default to 'ALL'
			{
				//returns false if trying to remove more items than exist
				packOrder();
				//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
				int index = getIndexOf(itemName);
				if (index == -1)
				{
					//item does not exist in bag
					return false;
				}
				index = ItemDatabaseOld.getIndexOf(orderString[index]);
				if (quantity[index] - amount < 0)
				{
					return false;
				}
				quantity[index] -= amount;
				if (quantity[index] == 0)
				{
					orderString[getIndexOf(itemName)] = null;
					packOrder();
				}
				return true;
			}

			public bool removeItem(Items itemName, int amount = 1)  //ToDo: amount should default to 'ALL'
			{
				//returns false if trying to remove more items than exist
				packOrder();
				//string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
				int index = getIndexOf(itemName);
				if (index == -1)
				{
					//item does not exist in bag
					return false;
				}
				index = ItemDatabaseOld.getIndexOf(orderString[index]);
				if (quantity[index] - amount < 0)
				{
					return false;
				}
				quantity[index] -= amount;
				if (quantity[index] == 0)
				{
					orderString[getIndexOf(itemName)] = null;
					packOrder();
				}
				return true;
			}*/

			public KeyValuePair<Items, byte>[] Sort(Order by)
			{
				IQueryable<KeyValuePair<Items, byte>> items = this[pocket].AsQueryable();
				switch (by)
				{
					case Order.Alphabet:
						reverseOrder = !reverseOrder;
						//ToDo: Connect to text and string localization
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
				//if (Game.Player.SandBoxMode || Game.IS_DEBUG_ACTIVE)
				//	return true;
				//else {
					if(Game.Bag_Items.Contains(Items.MEGA_BRACELET)) 
						return true;
				//}

				return false;
			}

			/// <summary>
			/// If the player has the <see cref="Items.RUNNING_SHOES"/> in their inventory.
			/// </summary>
			/// <returns></returns>
			/// newer gens automatically give at start, so default is true...
			public bool HasRunningShoes()
			{
				//if (Game.Bag_Items.Contains(Items.RUNNING_SHOES)) //ToDo: Does not exist?...
				//	return true;

				return true;
			}

			/// <summary>
			/// Adds items to the inventory.
			/// </summary>
			/// <param name="item">The ID of the item.</param>
			/// <param name="quantity">Amount of items to add.</param>
			public void AddItem(Items item, int quantity = 1)
			{
				//Dim newItem As Item = P3D.Item.GetItemByID(ID)
				//
				//For Each c As ItemContainer In Me
				//    If c.ItemID = ID Then
				//        c.Amount += Amount
				//        Exit Sub
				//    End If
				//Next
				//
				//Me.Add(New ItemContainer(ID, Amount))
				for (int i = 0; i < quantity; i++)
				{
					Game.Bag_Items.Add(item);
				}
			}

			/// <summary>
			/// Removes items from the inventory.
			/// </summary>
			/// <param name="item">The ID of the item to remove.</param>
			/// <param name="quantity">The amount of items to remove.</param>
			public void RemoveItem(Items item, int quantity = 1)
			{
				//If Amount > 0 Then
				//    For Each c As ItemContainer In Me
				//        If c.ItemID = ID Then
				//            If c.Amount - Amount <= 0 Then
				//                Me.Remove(c)
				//                Exit Sub
				//            Else
				//                c.Amount -= Amount
				//            End If
				//        End If
				//    Next
				//End If
				if(quantity > 0)
					for (int i = 0; i < quantity; i++)
					{
						if(Game.Bag_Items.Contains(item))
							Game.Bag_Items.Remove(item);
					}
			}

			/// <summary>
			/// Returns the count of the item in the inventory.
			/// </summary>
			/// <param name="item">The ID of the item to be counted.</param>
			/// <returns></returns>
			public int GetItemAmount(Items item)
			{
				//For Each c As ItemContainer In Me
				//    If c.ItemID = ID Then
				//        Return c.Amount
				//    End If
				//Next
				//
				//Return 0
				return Game.Bag_Items.Where(x => x == item).Count();
			}

			/// <summary>
			/// Returns a message that displays the event of putting an item into the inventory.
			/// </summary>
			/// <param name="item">The Item to store in the inventory.</param>
			/// <param name="quantity">The amount.</param>
			/// <returns></returns>
			public string GetMessageReceive(Items item, int quantity = 1)
			{
				string Message = "";
				//if (quantity == 1)
				//	Message = Game.Player.Name + " stored it in the~" + Game.Player.Inventory.GetItemPocketChar(item) + item.ItemPocket.ToString() + " pocket.";
				//else
				//	Message = Game.Player.Name + " stored them~in the " + Game.Player.Inventory.GetItemPocketChar(item) + item.ItemPocket.ToString() + " pocket.";
				return Message;
			}

			public enum Order
			{
				Alphabet,
				Quantity, 
				Category,
				Price
			}
		}
	}
}

namespace PokemonUnity.Character
{	
}