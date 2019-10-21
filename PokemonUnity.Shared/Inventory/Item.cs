using System.Collections;
using PokemonUnity;
using PokemonUnity.Inventory;

public class PBItems
{
    public static string getName(PokemonUnity.Inventory.Items item)
    {
        return string.Empty;
    }
    //public static int MaxValue { get { return MoveData.Count; } }
}

namespace PokemonUnity.Inventory
{
    public partial class Item
    {
        #region Variables
        public string Name { get; private set; }
        public string Plural { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public string Use { get; private set; }
        /// <summary>
        /// Should be a description of what it does when in battle,
        /// but might be better as a bool
        /// </summary>
        public bool BattleUse { get { return this.ItemFlag.Useable_In_Battle; } }

        public Items ItemId { get; private set; }
        public ItemFlags ItemFlag { get; private set; }
        public ItemCategory ItemCategory { get; private set; }
        public ItemPockets? ItemPocket
        {
            get
            {
                ItemPockets? itemPocket;
                switch (this.ItemCategory)
                {//([\w]*) = [\d]*, //PocketId = ([\d]*)
                    case ItemCategory.COLLECTIBLES:     //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.EVOLUTION:        //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.SPELUNKING:       //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.HELD_ITEMS:       //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.CHOICE:           //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.EFFORT_TRAINING:  //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.BAD_HELD_ITEMS:   //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.TRAINING:         //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.PLATES:           //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.SPECIES_SPECIFIC: //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.TYPE_ENHANCEMENT: //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.LOOT:             //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.MULCH:            //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.DEX_COMPLETION:   //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.SCARVES:          //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.JEWELS:           //itemPocket = (ItemPockets)1; break;
                    case ItemCategory.MEGA_STONES: itemPocket = (ItemPockets)1; break;

                    case ItemCategory.VITAMINS:         //itemPocket = (ItemPockets)2; break;
                    case ItemCategory.HEALING:          //itemPocket = (ItemPockets)2; break;
                    case ItemCategory.PP_RECOVERY:      //itemPocket = (ItemPockets)2; break;
                    case ItemCategory.REVIVAL:          //itemPocket = (ItemPockets)2; break;
                    case ItemCategory.STATUS_CURES: itemPocket = (ItemPockets)2; break;

                    case ItemCategory.SPECIAL_BALLS:    //itemPocket = (ItemPockets)3; break;
                    case ItemCategory.STANDARD_BALLS:   //itemPocket = (ItemPockets)3; break;
                    case ItemCategory.APRICORN_BALLS: itemPocket = (ItemPockets)3; break;

                    case ItemCategory.ALL_MACHINES: itemPocket = (ItemPockets)4; break;

                    case ItemCategory.EFFORT_DROP:      //itemPocket = (ItemPockets)5; break;
                    case ItemCategory.MEDICINE:         //itemPocket = (ItemPockets)5; break;
                    case ItemCategory.OTHER:            //itemPocket = (ItemPockets)5; break;
                    case ItemCategory.IN_A_PINCH:       //itemPocket = (ItemPockets)5; break;
                    case ItemCategory.PICKY_HEALING:    //itemPocket = (ItemPockets)5; break;
                    case ItemCategory.TYPE_PROTECTION:  //itemPocket = (ItemPockets)5; break;
                    case ItemCategory.BAKING_ONLY: itemPocket = (ItemPockets)5; break;

                    case ItemCategory.ALL_MAIL: itemPocket = (ItemPockets)6; break;

                    case ItemCategory.STAT_BOOSTS:      //itemPocket = (ItemPockets)7; break;
                    case ItemCategory.FLUTES:           //itemPocket = (ItemPockets)7; break;
                    case ItemCategory.MIRACLE_SHOOTER: itemPocket = (ItemPockets)7; break;

                    case ItemCategory.EVENT_ITEMS:      //itemPocket = (ItemPockets)8; break;
                    case ItemCategory.GAMEPLAY:         //itemPocket = (ItemPockets)8; break;
                    case ItemCategory.PLOT_ADVANCEMENT: //itemPocket = (ItemPockets)8; break;
                    case ItemCategory.UNUSED:           //itemPocket = (ItemPockets)8; break;
                    case ItemCategory.APRICORN_BOX:     //itemPocket = (ItemPockets)8; break;
                    case ItemCategory.DATA_CARDS:       //itemPocket = (ItemPockets)8; break;
                    case ItemCategory.XY_UNKNOWN: itemPocket = (ItemPockets)8; break;
                    default:
                        itemPocket = null;
                        break;
                }
                return itemPocket;
            }
        }
        public ItemFlingEffect ItemFlingEffect { get; private set; }

        #region Mail
		public string MailData { get; set; }
        /// <summary>
        /// Mail?...
        /// </summary>
        private Mail mail { get; set; }
		/// <summary>
		/// Returns message stored on this letter, if item <see cref="IsMail"/>;
        /// If anything other than null, there is a message. 
		/// Try <seealso cref="System.String.IsNullOrEmpty(string)"/>
		/// </summary>
        public string MailText
        {
            get
            {
                if (!IsMail) return null; //If empty return null
                //if (mail.Message.Length == 0 || this.Item == 0)//|| this.item.Category != Items.Category.Mail )
                //{
                //	//mail = null;
                //	return null;
                //}
                //if (Item)
                return mail.Message;
            }
            set
            {
                if (IsMail)
                    mail.Message = value;
            }
        }
        /// <summary>
        /// Performs a check to see if this Item belongs in the 
		/// Mail Pocket, <seealso cref="ItemPockets.MAIL"/>;
		/// and if the item is also a letter, <seealso cref="Mail.IsLetter"/>
        /// </summary
        /// ToDo: Item category
        public bool IsMail
        {
            get
            {
                if ((ItemPocket.HasValue && ItemPocket.Value == ItemPockets.MAIL) && mail.IsLetter) return true;
                else return false;
            }
        }
        #endregion
        #endregion

		#region Constructor
		public Item(Items itemId)
        {
            ItemId = itemId;
            Item item = GetItem(itemId);
            this.Price = item.Price;
            this.ItemCategory = item.ItemCategory;
            this.ItemFlag = item.ItemFlag;
            this.ItemFlingEffect = item.ItemFlingEffect;
			//this.Name				= ToDo: load from translation
			//this.Plural			= ToDo: load from translation
			//this.Description		= ToDo: load from translation
			Mail m = new Mail(itemId);
			if (m.IsLetter) this.mail = m;
        }

        public static Item GetItem(Items item)
        {
            for (int i = 0; i < Database.Length; i++)
            {
                if (Database[i].ItemId == item)
                {
                    return Database[i];
                }
            }
            //Zero is masterball; Items in enum dont match order of database.
            //return Database[0]; 
            return new Item(Items.NONE);
        }

        /// <summary>
        /// Returns int value of Pokemon from PokemonData[] <see cref="Database"/>
        /// </summary>
        public int ArrayId
        {//(Pokemon ID)
            get
            {
                //Debug.Log("Get Pokemons");
                /*PokemonData result = null;
                int i = 1;
                while(result == null){
                    if(Database[i].ID == ID){
                        //Debug.Log("Pokemon DB Success");
                        return result = Database[i];
                    }
                    i += 1;
                    if(i >= Database.Length){
                        Debug.Log("Pokemon DB Fail");
                        return null;}
                }
                return result;*/
                /*foreach(PokemonData pokemon in Database)
                {
                    if (pokemon.ID == ID) return pokemon;
                }*/
                for (int i = 0; i < Database.Length; i++)
                {
                    if (Database[i].ItemId == this.ItemId)
                    {
                        return i;
                    }
                }
                //throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
                //Game.DebugLog("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.", true);
                return -1;
            }
        }

        private Item(Items itemId, ItemCategory itemCategory = ItemCategory.UNUSED, /*BattleType battleType, string description,*/ int price = 0, int? flingPower = null,
            ItemFlingEffect? itemEffect = null, /*string stringParameter, float floatParameter,*/ ItemFlags flags = null)
        {
            //this.name = name;
            //this.itemType = itemType;
            //this.battleType = battleType;
            //this.description = description;
            this.Price = price;
            if (itemEffect.HasValue) this.ItemFlingEffect = itemEffect.Value;
            //this.stringParameter = stringParameter;
            //this.floatParameter = floatParameter;
        }

        //private Item(int itemId, int itemCategory, /*BattleType battleType, string description,;*/ int price, int? flingPower,
        //    int? itemEffect, /*string stringParameter, float floatParameter,*/ int[] flags = null) : this((Items)itemId, (ItemCategory)itemCategory, price, flingPower, (ItemFlingEffect)itemEffect, System.Array.ConvertAll(flags, item => (PokemonUnity.Item.ItemFlags)item))
        //{
        //return 
        //new Item((Items)itemId, (ItemCategory)itemCategory, price, flingPower, (ItemFlingEffect)itemEffect, System.Array.ConvertAll(flags, item => (ItemFlags)item));
        //}
        #endregion
    }
}