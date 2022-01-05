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
	// ===============================================================================
	// Abstraction layer
	// ===============================================================================
	/// <summary>
	/// </summary>
	public interface IMartAdapter {
		int getMoney();

		void setMoney(int value);

		IBag getInventory();

		int getPrice(Items item, bool selling = false);

		ISprite getItemIcon(Items item);

		IRect getItemIconRect(Items item);

		string getDisplayName(Items item);

		string getName(Items item);

		string getDisplayPrice(Items item,bool selling=false);

		string getDescription(Items item);

		bool addItem(Items item);

		int getQuantity(Items item);

		bool canSell(Items item);

		bool showQuantity(Items item);

		bool removeItem(Items item);
	}

	// ===============================================================================
	// Abstraction layer for RPG Maker XP/VX
	// Won't be used if Game.GameData.Bag exists
	// ===============================================================================
	/*public interface IRpgxpMartAdapter : IMartAdapter {
	  void getMoney() {
		return $game_party.gold;
	  }

	  void setMoney(value) {
		$game_party.gain_gold(-$game_party.gold);
		$game_party.gain_gold(value);
	  }

	  void getPrice(Items item,selling=false) {
		return item.price;
	  }

	  void getItemIcon(Items item) {
		if (!item) return null;
		if (Items item==0) {
		  return string.Format("Graphics/Icons/itemBack");
		} else if (Items item.respond_to("icon_index")) {
		  return "Graphics/System/IconSet";
		} else {
		  return string.Format("Graphics/Icons/%s",item.icon_name);
		}
	  }

	  void getItemIconRect(Items item) {
		if (Items item && item.respond_to("icon_index")) {
		  ix=item.icon_index % 16 * 24;
		  iy=item.icon_index / 16 * 24;
		  return new Rect(ix,iy,24,24);
		} else {
		  return new Rect(0,0,32,32);
		}
	  }

	  void getInventory() {
		data = [];
		for (int i = 1; i < $data_items.size; i++) {
		  if (getQuantity($data_items[i]) > 0) {
			data.Add($data_items[i]);
		  }
		}
		for (int i = 1; i < $data_weapons.size; i++) {
		  if (getQuantity($data_weapons[i]) > 0) {
			data.Add($data_weapons[i]);
		 }
		}
		for (int i = 1; i < $data_armors.size; i++) {
		  if (getQuantity($data_armors[i]) > 0) {
			data.Add($data_armors[i]);
		  }
		}
		return data;
	  }

	  bool canSell (Items item) {
		return item ? item.price>0 : false;
	  }

	  void getName(Items item) {
		return item ? item.name : "";
	  }

	  void getDisplayName(Items item) {
		return item ? item.name : "";
	  }

	  void getDisplayPrice(Items item,selling=false) {
		price=item.price;
		return string.Format("{1:d}",price);
	  }

	  void getDescription(Items item) {
		return item ? item.description : "";
	  }

	  void addItem(Items item) {
		ret=(getQuantity(Items item)<99);
		if ($game_party.respond_to("gain_weapon")) {
		  switch (Items item) {
			break;
		  case RPG.Gtem:
			if (ret) $game_party.gain_item(Items item.id, 1);
			break;
		  case RPG.Geapon:
			if (ret) $game_party.gain_weapon(Items item.id, 1);
			break;
		  case RPG.Grmor:
			if (ret) $game_party.gain_armor(Items item.id, 1);
		  }
		} else {
		  if (ret) $game_party.gain_item(Items item,1);
		}
		return ret;
	  }

	  void getQuantity(Items item) {
		ret=0;
		if ($game_party.respond_to("weapon_number")) {
		  switch (Items item) {
			break;
		  case RPG.Gtem:
			ret=$game_party.item_number(Items item.id);
			break;
		  case RPG.Geapon:
			ret=($game_party.weapon_number(Items item.id));
			break;
		  case RPG.Grmor:
			ret=($game_party.armor_number(Items item.id));
		  }
		} else {
		  return $game_party.item_number(Items item);
		}
		return ret;
	  }

	  bool showQuantity (Items item) {
		return true;
	  }

	  void removeItem(Items item) {
		ret=(getQuantity(Items item)>0);
		if ($game_party.respond_to("lose_weapon")) {
		  switch (Items item) {
			break;
		  case RPG.Gtem:
			if (ret) $game_party.lose_item(Items item.id, 1);
			break;
		  case RPG.Geapon:
			if (ret) $game_party.lose_weapon(Items item.id, 1);
			break;
		  case RPG.Grmor:
			if (ret) $game_party.lose_armor(Items item.id, 1);
		  }
		} else {
		  if (ret) $game_party.lose_item(Items item,1);
		}
		return ret;
	  }
	}*/

	// ===============================================================================
	// Buy and Sell adapters
	// ===============================================================================
	public interface IBuyAdapter { // :nodoc:
		IBuyAdapter initialize(IMartAdapter adapter);

		string getDisplayName(Items item);

		int getDisplayPrice(Items item);

		bool isSelling();
	}

	public interface ISellAdapter { // :nodoc:
		ISellAdapter initialize(IMartAdapter adapter);

		string getDisplayName(Items item);

		string getDisplayPrice(Items item);

		bool isSelling();
	}

	// ===============================================================================
	// Pokémon Mart
	// ===============================================================================
	public interface IGameMart {
		void pbPokemonMart(Items[] stock, string speech = null, bool cantsell = false); 
	}

	public interface IGameTempMart {
		int[] mart_prices				{ get; }
		//int[] mart_prices();

		void clear_mart_prices();
	}

	#region Mart Scene
	public interface IMartScene : IScene {
		void update();

		//void pbRefresh();

		void pbStartBuyOrSellScene(bool buying, Items[] stock, IMartAdapter adapter);

		void pbStartBuyScene(Items[] stock, IMartAdapter adapter);

		void pbStartSellScene(IBag bag, IMartAdapter adapter);

		void pbStartSellScene2(IBag bag, IMartAdapter adapter);

		void pbEndBuyScene();

		void pbEndSellScene();

		void pbPrepareWindow(IWindow window);

		void pbShowMoney();

		void pbHideMoney();

		void pbDisplay(string msg, bool brief = false);

		void pbDisplayPaused(string msg);

		bool pbConfirm(string msg);

		int pbChooseNumber(string helptext, Items item, int maximum);

		int pbChooseBuyItem();

		int pbChooseSellItem();
	}

	public interface IMartScreen : IScreen {
		IMartScreen initialize(IMartScene scene, Items[] stock);

		bool pbConfirm(string msg);

		void pbDisplay(string msg);

		void pbDisplayPaused(string msg);

		IEnumerator pbBuyScreen();

		IEnumerator pbSellScreen();
	}
	#endregion

	#region UI Elements
	public interface IWindow_PokemonMart //: IWindow_DrawableCommand 
	{
		IWindow_PokemonMart initialize(Items[] stock, IMartAdapter adapter, float x, float y, int width, int height, IViewport viewport = null);

		int itemCount();

		Items item();

		void drawItem(int index, int count, IRect rect);
	}
	#endregion

	public interface IInterpreterMart {
		//p = parameter[int type,int id]
		Items getItem(KeyValuePair<int, int> p);

		bool command_302();

		void setPrice(Items item, int buyprice = -1, int sellprice = -1);

		void setSellPrice(Items item, int sellprice);
	}

	public interface IGameInterpreterMart {
		//p = parameter[int type,int id]
		Items getItem(KeyValuePair<int, int> p);

		bool command_302();
	}
}