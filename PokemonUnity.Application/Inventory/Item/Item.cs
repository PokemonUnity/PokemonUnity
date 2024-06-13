using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Character;
using PokemonUnity.Overworld;
using PokemonUnity.Monster;
using PokemonUnity.Monster.Data;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Inventory
{
	//public partial class ItemHandlerHash : HandlerHash {
	//  public void initialize() {
	//    super(:Items);
	//  }
	//}

	//Use `EventHandlerList` and replace dictionary/delegate with events?
	public static partial class ItemHandlers {
		//private static ItemHandlerHash UseFromBag=new ItemHandlerHash();
		//private static ItemHandlerHash UseInField=new ItemHandlerHash();
		//private static ItemHandlerHash UseOnPokemon=new ItemHandlerHash();
		//private static ItemHandlerHash BattleUseOnBattler=new ItemHandlerHash();
		//private static ItemHandlerHash BattleUseOnPokemon=new ItemHandlerHash();
		//private static ItemHandlerHash UseInBattle=new ItemHandlerHash();
		private static Dictionary<Items,Func<ItemUseResults>> UseFromBag=new Dictionary<Items, Func<ItemUseResults>>();
		private static Dictionary<Items,Action> UseInField=new Dictionary<Items, Action>();
		private static Dictionary<Items,UseOnPokemonDelegate> UseOnPokemon=new Dictionary<Items, UseOnPokemonDelegate>();
		private static Dictionary<Items,BattleUseOnBattlerDelegate> BattleUseOnBattler=new Dictionary<Items, BattleUseOnBattlerDelegate>();
		private static Dictionary<Items,BattleUseOnPokemonDelegate> BattleUseOnPokemon=new Dictionary<Items, BattleUseOnPokemonDelegate>();
		private static Dictionary<Items,UseInBattleDelegate> UseInBattle=new Dictionary<Items, UseInBattleDelegate>();
		//public delegate bool UseOnPokemonDelegate(Items item, IPokemon pokemon, IHasDisplayMessage scene);
		//public delegate bool BattleUseOnPokemonDelegate(IPokemon pokemon, IBattler battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
		//public delegate bool BattleUseOnBattlerDelegate(Items item, IBattler battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
		//public delegate void UseInBattleDelegate(Items item, IBattler battler, IBattle battle);

		public static void addUseFromBag(Items item,Func<ItemUseResults> proc) {
			if (!UseFromBag.ContainsKey(item))
				UseFromBag.Add(item,proc);
			else
				UseFromBag[item] = proc;
		}

		public static void addUseInField(Items item,Action proc) {
			if (!UseInField.ContainsKey(item))
				UseInField.Add(item,proc);
			else
				UseInField[item] = proc;
		}

		public static void addUseOnPokemon(Items item, UseOnPokemonDelegate proc) {
			if (!UseOnPokemon.ContainsKey(item))
				UseOnPokemon.Add(item,proc);
			else
				UseOnPokemon[item] = proc;
		}

		public static void addBattleUseOnBattler(Items item, BattleUseOnBattlerDelegate proc) {
			if (!BattleUseOnBattler.ContainsKey(item))
				BattleUseOnBattler.Add(item,proc);
			else
				BattleUseOnBattler[item] = proc;
		}

		public static void addBattleUseOnPokemon(Items item, BattleUseOnPokemonDelegate proc) {
			if (!BattleUseOnPokemon.ContainsKey(item))
				BattleUseOnPokemon.Add(item,proc);
			else
				BattleUseOnPokemon[item] = proc;
		}

		/// <summary>
		/// Shows "Use" option in Bag
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool hasOutHandler(Items item) {
			return (!UseFromBag.ContainsKey(item) && UseFromBag[item]!=null) || (!UseOnPokemon.ContainsKey(item) && UseOnPokemon[item]!=null);
		}

		/// <summary>
		/// Shows "Register" option in Bag
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool hasKeyItemHandler(Items item) {
			return !UseInField.ContainsKey(item) && UseInField[item]!=null;
		}

		public static bool hasUseOnPokemon(Items item) {
			return !UseOnPokemon.ContainsKey(item) && UseOnPokemon[item]!=null;
		}

		public static bool hasBattleUseOnBattler(Items item) {
			return !BattleUseOnBattler.ContainsKey(item) && BattleUseOnBattler[item]!=null;
		}

		public static bool hasBattleUseOnPokemon(Items item) {
			return !BattleUseOnPokemon.ContainsKey(item) && BattleUseOnPokemon[item]!=null;
		}

		public static bool hasUseInBattle(Items item) {
			return !UseInBattle.ContainsKey(item) && UseInBattle[item]!=null;
			//return Kernal.ItemData[item].Flags.Useable_In_Battle;
		}

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Return values: 0 = not used
		///                         1 = used, item not consumed
		///                         2 = close the Bag to use, item not consumed
		///                         3 = used, item consumed
		///                         4 = close the Bag to use, item consumed</returns>
		public static ItemUseResults triggerUseFromBag(Items item) {
			//  Return value:
			//  0 - Item not used
			//  1 - Item used, don't end screen
			//  2 - Item used, end screen
			//  3 - Item used, consume item
			//  4 - Item used, end screen, consume item
			if (!UseFromBag.ContainsKey(item) || UseFromBag[item] == null) { //Search List
				// Check the UseInField handler if present
				if (UseInField[item] != null) {
				//if (!Kernal.ItemData[item].Flags.Useable_Overworld) {
					UseInField[item].Invoke();
					//ItemHandlers.UseInField(item);
					return ItemUseResults.UsedNotConsumed; // item was used
				}
				return 0; // item was not used
			} else {
				return UseFromBag[item].Invoke();
				//return ItemHandlers.UseFromBag(item);
			}
		}

		public static bool triggerUseInField(Items item) {
			// No return value
			if (!UseInField.ContainsKey(item) || UseInField[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_Overworld) {
				return false;
			} else {
				UseInField[item].Invoke();
				//ItemHandlers.UseInField(item);
				return true;
			}
		}

		public static bool triggerUseOnPokemon(Items item,IPokemon pokemon,IHasDisplayMessage scene) {
			// Returns whether item was used
			if (!UseOnPokemon.ContainsKey(item) || UseOnPokemon[item] == null) { //Search List
				return false;
			} else {
				//return UseOnPokemon[item].Invoke(pokemon,scene);
				return (bool)UseOnPokemon[item].Invoke(item,pokemon,scene);
				//return ItemHandlers.UseOnPokemon(item,pokemon,scene);
			}
		}

		//ToDo: scene parameter is throwing errors in battle class, resolve in itemhandler method
		public static bool triggerBattleUseOnBattler(Items item,IBattler battler,IHasDisplayMessage scene) { return false; }
		public static bool triggerBattleUseOnPokemon(Items item,IPokemon pokemon,IBattler battler, IHasDisplayMessage scene) { return false; }

		public static bool triggerBattleUseOnBattler(Items item,IBattler battler,IPokeBattle_Scene scene) {
			// Returns whether item was used
			if (!BattleUseOnBattler.ContainsKey(item) || BattleUseOnBattler[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_In_Battle) {
				return false;
			} else {
				return BattleUseOnBattler[item].Invoke(item,battler,scene);
				//return ItemHandlers.BattleUseOnBattler(item,battler,scene);
			}
		}

		public static bool triggerBattleUseOnPokemon(Items item,IPokemon pokemon,IBattler battler,IPokeBattle_Scene scene) {
			// Returns whether item was used
			if (!BattleUseOnPokemon.ContainsKey(item) || BattleUseOnPokemon[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_In_Battle) {
				return false;
			} else {
				return BattleUseOnPokemon[item].Invoke(pokemon,battler,scene);
				//return ItemHandlers.BattleUseOnPokemon(item,pokemon,battler,scene);
			}
		}

		public static void triggerUseInBattle(Items item,IBattler battler,IBattle battle) {
			// Returns whether item was used
			if (!UseInBattle.ContainsKey(item) || UseInBattle[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_In_Battle) {
				return;
			} else {
				UseInBattle[item].Invoke(item,battler,battle);
				//ItemHandlers.UseInBattle(item,battler,battle);
			}
		}
	}
}