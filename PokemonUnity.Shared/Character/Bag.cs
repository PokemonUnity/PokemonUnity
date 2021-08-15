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
		public void GetMessageReceive(Items item, IScene scene, int quantity = 1)
		{
			string Message = "";
			if (quantity == 1)
				Message = Game.GameData.Player.Name + " stored it in the~" + Game.ItemData[item].Pocket.ToString() + " pocket.";
			else
				Message = Game.GameData.Player.Name + " stored them~in the " + Game.ItemData[item].Pocket.ToString() + " pocket.";
			//scene.pbDisplayMessage(Message);
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

/// <summary>
/// The Bag object, which actually contains all the items
/// </summary>
public partial class PokemonBag {
  public Items registeredItem			{ get; protected set; }
  public int lastpocket					{ get; protected set; }
  public Items[][] pockets				{ get; protected set; }
  //public Dictionary<ItemPockets, Items[]> pockets				{ get; protected set; }
		private Dictionary<ItemPockets, int> choices;

  //public static string[] pocketNames { get {
  //  return Game.pbPocketNames;
  //} }

  public static int numPockets { get {
    //return PokemonBag.pocketNames.Length-1;
    return Enum.GetNames(typeof(PokemonUnity.Inventory.ItemPockets)).Length-1;
  } }

  public PokemonBag() {
    @lastpocket=1;
    @pockets=new Items[PokemonBag.numPockets][];
    //@choices=new int[PokemonBag.numPockets];
    @choices=new Dictionary<ItemPockets, int>();
	//  Initialize each pocket of the array
    for (int i = 0; i < PokemonBag.numPockets; i++) {
      @pockets[i]=new Items[0];
      //@choices[i]=0;
    }
    @registeredItem=0;
  }

  //public ItemPockets pockets { get {
  //  rearrange();
  //  return @pockets;
  //} }

  public void rearrange() {
    if ((@pockets.Length-1)!=PokemonBag.numPockets) {
      List<Items>[] newpockets=new List<Items>[PokemonBag.numPockets];
      Items[][] n=new Items[PokemonBag.numPockets][];
      for (int i = 0; i < PokemonBag.numPockets; i++) {
        newpockets[i]=new List<Items>();
        if (@choices[(ItemPockets)i] == null) @choices[(ItemPockets)i]=0;
      }
      int nump=PokemonBag.numPockets;
      for (int i = 0; i < @pockets.Length; i++) {
        foreach (Items item in @pockets[i]) {
          //ItemPockets p=pbGetPocket(item[0]);
          ItemPockets p=Game.ItemData[item].Pocket??ItemPockets.MISC;
          if ((int)p<=nump) newpockets[(int)p].Add(item);
        }
		n[i] =newpockets[(int)i].ToArray();
      }
      @pockets=n; //newpockets;
    }
  }

/// <summary>
/// Gets the index of the current selected item in the pocket
/// </summary>
/// <param name="pocket"></param>
/// <returns></returns>
  public int getChoice(ItemPockets pocket) {
    if (pocket<0 || (int)pocket>PokemonBag.numPockets) {
      //throw new Exception(Game._INTL("Invalid pocket: {1}",pocket.ToString()));
      GameDebug.LogError(Game._INTL("Invalid pocket: {1}",pocket.ToString()));
    }
    rearrange();
    return Math.Min(@choices[pocket],@pockets[(int)pocket].Length); //|| 0
  }

/// <summary>
/// Clears the entire bag
/// </summary>
  public void clear() {
    //foreach (var pocket in @pockets) {
    //  //pocket.clear();
    //}
    for (int p = 0; p < @pockets.Length; p++) {
      pockets[p] = new Items[0];
    }
  }

/// <summary>
/// Sets the index of the current selected item in the pocket
/// </summary>
/// <param name="pocket"></param>
/// <param name="value"></param>
  public void setChoice(ItemPockets pocket,int value) {
    if (pocket<=0 || (int)pocket>PokemonBag.numPockets) {
      //throw new Exception(Game._INTL("Invalid pocket: {1}",pocket.ToString()));
      GameDebug.LogError(Game._INTL("Invalid pocket: {1}",pocket.ToString()));
    }
    rearrange();
    if (value<=@pockets[(int)pocket].Length) @choices[pocket]=value;
  }

/// <summary>
/// Registers the item as a key item.
/// Can be retrieved with <see cref="Game.GameData.Bag.registeredItem"/>
/// </summary>
/// <param name="item"></param>
  public void pbRegisterKeyItem(Items item) {
    //if (item is String || item is Symbol) {
    //  item=getID(PBItems,item);
    //}
    //if (item == null || item<1) {
    //  //throw new Exception(Game._INTL("The item number is invalid."));
    //  return;
    //}
    @registeredItem=(item!=@registeredItem) ? item : 0;
  }

  public int maxPocketSize(int pocket) {
    int? maxsize=Core.MAXPOCKETSIZE[pocket];
    if (maxsize == null) return -1;
    return maxsize.Value;
  }

  public int pbQuantity(Items item) {
    //if (item is String || item is Symbol) {
    //  item=getID(PBItems,item);
    //}
    //if (item == null || item<1) {
    //  //throw new Exception(Game._INTL("The item number is invalid."));
    //  return 0;
    //}
    //ItemPockets pocket=pbGetPocket(item);
    int pocket=(int)(Game.ItemData[item].Pocket??ItemPockets.MISC);
    int maxsize=maxPocketSize(pocket);
    if (maxsize<0) maxsize=@pockets[(int)pocket].Length;
    return ItemStorageHelper.pbQuantity(@pockets[pocket],maxsize,item);
  }

  public bool pbHasItem (Items item) {
    return pbQuantity(item)>0;
  }

  public bool pbDeleteItem(Items item,int qty=1) {
    //if (item is String || item is Symbol) {
    //  item=getID(PBItems,item);
    //}
    //if (item == null || item<1) {
    //  //throw new Exception(Game._INTL("The item number is invalid."));
    //  return false;
    //}
    //ItemPockets pocket=pbGetPocket(item);
    int pocket=(int)(Game.ItemData[item].Pocket??ItemPockets.MISC);
    int maxsize=maxPocketSize(pocket);
    if (maxsize<0) maxsize=@pockets[pocket].Length;
    bool ret=ItemStorageHelper.pbDeleteItem(ref @pockets[pocket],maxsize,item,qty);
    if (ret) {
      if (@registeredItem==item && !pbHasItem(item)) @registeredItem=0;
    }
    return ret;
  }

  public bool pbCanStore (Items item,int qty=1) {
    //if (item is String || item is Symbol) {
    //  item=getID(PBItems,item);
    //}
    //if (!item || item<1) {
    //  //throw new Exception(Game._INTL("The item number is invalid."));
    //  return false;
    //}
    //ItemPockets pocket=pbGetPocket(item);
    int pocket=(int)(Game.ItemData[item].Pocket??ItemPockets.MISC);
    int maxsize=maxPocketSize(pocket);
    if (maxsize<0) maxsize=@pockets[pocket].Length+1;
    return ItemStorageHelper.pbCanStore(
       @pockets[pocket],maxsize,Core.BAGMAXPERSLOT,item,qty);
  }

  //public bool pbStoreAllOrNone(Items item,int qty=1) {
  //  //if (item is String || item is Symbol) {
  //  //  item=getID(PBItems,item);
  //  //}
  //  //if (item == null || item<1) {
  //  //  //throw new Exception(Game._INTL("The item number is invalid."));
  //  //  return false;
  //  //}
  //  //ItemPockets pocket=pbGetPocket(item);
  //  int pocket=(int)(Game.ItemData[item].Pocket??ItemPockets.MISC);
  //  int maxsize=maxPocketSize(pocket);
  //  if (maxsize<0) maxsize=@pockets[pocket].Length+1;
  //  return ItemStorageHelper.pbStoreAllOrNone(ref
  //     @pockets[pocket],maxsize,Core.BAGMAXPERSLOT,item,qty);
  //}

  public bool pbStoreItem(Items item,int qty=1) {
    //if (item is String || item is Symbol) {
    //  item=getID(PBItems,item);
    //}
    //if (item == null || item<1) {
    //  //throw new Exception(Game._INTL("The item number is invalid."));
    //  return false;
    //}
    //ItemPockets pocket=pbGetPocket(item);
    int pocket=(int)(Game.ItemData[item].Pocket??ItemPockets.MISC);
    int maxsize=maxPocketSize(pocket);
    if (maxsize<0) maxsize=@pockets[pocket].Length+1;
    return ItemStorageHelper.pbStoreItem(ref
       @pockets[pocket],maxsize,Core.BAGMAXPERSLOT,item,qty,true);
  }

  public bool pbChangeItem(Items olditem,Items newitem) {
    //if (olditem is String || olditem is Symbol) {
    //  olditem=getID(PBItems,olditem);
    //}
    //if (newitem is String || newitem is Symbol) {
    //  newitem=getID(PBItems,newitem);
    //}
    //if (olditem == null || olditem<1 || newitem == null || newitem<1) {
    if ((int)olditem<1 || (int)newitem<1) {
      //throw new Exception(Game._INTL("The item number is invalid."));
      return false;
    }
    //ItemPockets pocket=pbGetPocket(olditem);
    int pocket=(int)(Game.ItemData[olditem].Pocket??ItemPockets.MISC);
    int maxsize=maxPocketSize(pocket);
    if (maxsize<0) maxsize=@pockets[pocket].Length;
    bool ret=false;
    for (int i = 0; i < maxsize; i++) {
      Items[] itemslot=@pockets[pocket];//[i];
      if (itemslot != null && itemslot[i]==olditem) {
        itemslot[i]=newitem;//itemslot[0] => key
        ret=true;
      }
    }
    return ret;
  }
}
}