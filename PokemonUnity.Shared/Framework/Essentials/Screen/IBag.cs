using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	/// <summary>
	/// The Bag object, which actually contains all the items
	/// </summary>
	public interface IBag 
	{
		Items registeredItem			{ get; }
		int lastpocket				{ get; }
		//ItemPockets pockets		{ get; }
		Items[][] pockets			{ get; }

		string[] pocketNames		{ get; }

		int numPockets				{ get; }

		//IBag initialize();

		//int pockets { get; }

		void rearrange();

		/// <summary>
		/// Gets the index of the current selected item in the pocket
		/// </summary>
		/// <param name="pocket"></param>
		/// <returns></returns>
		int getChoice(ItemPockets pocket);

		/// <summary>
		/// Clears the entire bag
		/// </summary>
		void clear();

		/// <summary>
		/// Sets the index of the current selected item in the pocket
		/// </summary>
		/// <param name="pocket"></param>
		/// <param name="value"></param>
		void setChoice(ItemPockets pocket, int value);

		/// <summary>
		/// Registers the item as a key item.  Can be retrieved with Game.GameData.Bag.registeredItem
		/// </summary>
		/// <param name="item"></param>
		void pbRegisterKeyItem(Items item);

		int maxPocketSize(int pocket);

		int pbQuantity(Items item);

		bool pbHasItem(Items item);

		bool pbDeleteItem(Items item, int qty = 1);

		bool pbCanStore(Items item, int qty = 1);

		bool pbStoreAllOrNone(Items item, int qty = 1);

		bool pbStoreItem(Items item, int qty = 1);

		bool pbChangeItem(Items olditem, Items newitem);
	}

	#region Bag Scene
	public interface IBagScene : IScene
	{
		IBagScene initialize();
		void update();
		void pbStartScene(IBag bag);
		void pbEndScene();
		int pbChooseNumber(string helptext, int maximum);
		void pbDisplay(string msg, bool brief = false);
		void pbConfirm(string msg);
		int pbShowCommands(string helptext, IList<string> commands);
		//void pbRefresh();

		/// <summary>
		/// Called when the item screen wants an item to be chosen from the screen
		/// </summary>
		/// <param name="lockpocket"></param>
		/// <returns></returns>
		Items pbChooseItem(bool lockpocket = false);
	}

	/// <summary>
	/// Functions and Menu options for Bag Component.
	/// Can share the same GameObject entity as <seealso cref="IWindow_PokemonBag"/>
	/// </summary>
	public interface IBagScreen : IScreen
	{
		IBagScreen initialize(IBagScene scene, IBag bag);
		void pbDisplay(string text);
		void pbConfirm(string text);

		/// <summary>
		/// UI logic for the item screen when an item is to be held by a Pokémon.
		/// </summary>
		/// <returns></returns>
		Items pbGiveItemScreen();

		/// <summary>
		/// UI logic for the item screen when an item is used on a Pokémon from the party screen.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <returns></returns>
		Items pbUseItemScreen(Pokemons pokemon);

		/// <summary>
		/// UI logic for the item screen for choosing an item
		/// </summary>
		/// <returns></returns>
		Items pbChooseItemScreen();

		/// <summary>
		/// UI logic for the item screen for choosing a Berry
		/// </summary>
		/// <returns></returns>
		Items pbChooseBerryScreen();

		/// <summary>
		/// UI logic for tossing an item in the item screen.
		/// </summary>
		void pbTossItemScreen();

		/// <summary>
		/// UI logic for withdrawing an item in the item screen.
		/// </summary>
		void pbWithdrawItemScreen();

		/// <summary>
		/// UI logic for depositing an item in the item screen.
		/// </summary>
		void pbDepositItemScreen();
		Items pbStartScreen();
	}
	#endregion

	#region Bag Screen UI Elements
	/// <summary>
	/// UI Elements for the Bag screen
	/// </summary>
	public interface IWindow_PokemonBag : IWindow_DrawableCommand
	{
		int pocket { get; set; }
		int sortIndex { get; set; }

		IWindow_PokemonBag initialize(IBag bag, int pocket, float x, float y, int width, int height);

		// public int pocket { set {
		//@pocket=value;
		//thispocket=@bag.pockets[@pocket];
		//@item_max=thispocket.Length+1;
		//this.index=@bag.getChoice(@pocket);
		//refresh();
		// } }

		// public int sortIndex { set {
		//@sortIndex=value;
		//refresh();
		// } }

		int page_row_max();
		int page_item_max();

		IRect itemRect(int item);

		IRect drawCursor(int index, IRect rect);

		Items item();

		int itemCount { get; }

		void drawItem(int index, int count, IRect rect);

		void refresh();
	}
	#endregion
}