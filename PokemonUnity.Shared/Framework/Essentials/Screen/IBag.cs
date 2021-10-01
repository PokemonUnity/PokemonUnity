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
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	/// <summary>
	/// The Bag object, which actually contains all the items
	/// </summary>
	public interface IBag 
	{
		int registeredItem			{ get; }
		int lastpocket				{ get; }
		int pockets				    { get; }

		string[] pocketNames		{ get; }

		int numPockets				{ get; }

		//PokemonBag();

		//int pockets { get; }

		void rearrange();

		// Gets the index of the current selected item in the pocket
		int getChoice(int pocket);

		// Clears the entire bag
		void clear();

		// Sets the index of the current selected item in the pocket
		void setChoice(int pocket, int value);

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
	public interface IPokemonBag_Scene : IScene
	{
		void update();
		void pbStartScene(Bag bag);
		void pbEndScene();
		int pbChooseNumber(string helptext, int maximum);
		void pbDisplay(string msg, bool brief = false);
		void pbConfirm(string msg);
		void pbShowCommands(string helptext, int commands);
		//void pbRefresh();

		// Called when the item screen wants an item to be chosen from the screen
		Items pbChooseItem(bool lockpocket = false);
	}

	public interface IPokemonBagScreen : IScreen
	{
		IPokemonBagScreen initialize(IPokemonBag_Scene scene, Bag bag);
		void pbDisplay(string text);
		void pbConfirm(string text);

		// UI logic for the item screen when an item is to be held by a Pokémon.
		Items pbGiveItemScreen();

		// UI logic for the item screen when an item is used on a Pokémon from the party screen.
		Items pbUseItemScreen(Pokemons pokemon);

		// UI logic for the item screen for choosing an item
		Items pbChooseItemScreen();

		// UI logic for the item screen for choosing a Berry
		Items pbChooseBerryScreen();

		// UI logic for tossing an item in the item screen.
		void pbTossItemScreen();

		// UI logic for withdrawing an item in the item screen.
		void pbWithdrawItemScreen();

		// UI logic for depositing an item in the item screen.
		void pbDepositItemScreen();
		Items pbStartScreen();
	}
	#endregion
}