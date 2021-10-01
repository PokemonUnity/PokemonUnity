using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;

namespace PokemonEssentials.Interface
{
	namespace EventArg
	{
		#region Item Handlers EventArgs
		public class UseFromBagEventArgs : EventArgs
		{
			public readonly int EventId = typeof(UseFromBagEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			public ItemUseResults Response { get; set; }
		}
		public class UseInFieldEventArgs : EventArgs
		{
			public readonly int EventId = typeof(UseInFieldEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			//public Action Action { get; set; }
			public bool Response { get; set; }
		}
		public class UseOnPokemonEventArgs : EventArgs
		{
			public readonly int EventId = typeof(UseOnPokemonEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			public IPokemon Pokemon { get; set; }
			public IHasDisplayMessage Scene { get; set; }
			public bool Response { get; set; }
		}
		public class BattleUseOnPokemonEventArgs : EventArgs
		{
			public readonly int EventId = typeof(BattleUseOnPokemonEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			public IPokemon Pokemon { get; set; }
			public IBattler Battler { get; set; }
			public IHasDisplayMessage Scene { get; set; }
			public bool Response { get; set; }
		}
		public class BattleUseOnBattlerEventArgs : EventArgs
		{
			public readonly int EventId = typeof(BattleUseOnBattlerEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			public IBattler Battler { get; set; }
			public IHasDisplayMessage Scene { get; set; }
			public bool Response { get; set; }
		}
		public class UseInBattleEventArgs : EventArgs
		{
			public readonly int EventId = typeof(UseInBattleEventArgs).GetHashCode();

			public int Id { get; }
			public Items Item { get; set; }
			public IBattler Battler { get; set; }
			public IBattle Battle { get; set; }
		}
		#endregion

		public delegate bool UseOnPokemonDelegate(Items item, IPokemon pokemon, IHasDisplayMessage scene);
		public delegate bool BattleUseOnPokemonDelegate(IPokemon pokemon, IBattler battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
		public delegate bool BattleUseOnBattlerDelegate(Items item, IBattler battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
		public delegate void UseInBattleDelegate(Items item, IBattler battler, IBattle battle);
	}

	namespace Item
	{
		public interface IItemHandlers {
			//IDictionary<Items, Func<ItemUseResults>> UseFromBag { get; }
			//IDictionary<Items, Action> UseInField { get; }
			//IDictionary<Items, UseOnPokemonDelegate> UseOnPokemon { get; }
			//IDictionary<Items, BattleUseOnBattlerDelegate> BattleUseOnBattler { get; }
			//IDictionary<Items, BattleUseOnPokemonDelegate> BattleUseOnPokemon { get; }
			//IDictionary<Items, UseInBattleDelegate> UseInBattle { get; }
			
			event EventHandler<UseFromBagEventArgs> OnUseFromBag;
			event EventHandler<UseInFieldEventArgs> OnUseInField;
			event EventHandler<UseOnPokemonEventArgs> OnUseOnPokemon;
			event EventHandler<BattleUseOnBattlerEventArgs> OnBattleUseOnBattler;
			event EventHandler<BattleUseOnPokemonEventArgs> OnBattleUseOnPokemon;
			event EventHandler<UseInBattleEventArgs> OnUseInBattle;

			void addUseFromBag(Items item, Func<ItemUseResults> proc);

			void addUseInField(Items item, Action proc);

			void addUseOnPokemon(Items item, UseOnPokemonDelegate proc);

			void addBattleUseOnBattler(Items item, BattleUseOnBattlerDelegate proc);

			void addBattleUseOnPokemon(Items item, BattleUseOnPokemonDelegate proc);

			/// <summary>
			/// Shows "Use" option in Bag
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			bool hasOutHandler(Items item);

			/// <summary>
			/// Shows "Register" option in Bag
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			bool hasKeyItemHandler(Items item);

			bool hasUseOnPokemon(Items item);

			bool hasBattleUseOnBattler(Items item);

			bool hasBattleUseOnPokemon(Items item);

			bool hasUseInBattle(Items item);

			/// <summary>
			/// </summary>
			/// <param name="item"></param>
			/// <returns>Return values: 0 = not used
			///                         1 = used, item not consumed
			///                         2 = close the Bag to use, item not consumed
			///                         3 = used, item consumed
			///                         4 = close the Bag to use, item consumed</returns>
			ItemUseResults triggerUseFromBag(Items item);

			bool triggerUseInField(Items item);

			bool triggerUseOnPokemon(Items item, IPokemon pokemon, IHasDisplayMessage scene);

			//bool triggerBattleUseOnBattler(Items item,IBattler battler,IPokeBattle_Scene scene);
			bool triggerBattleUseOnBattler(Items item, IBattler battler, IHasDisplayMessage scene);

			//bool triggerBattleUseOnPokemon(Items item, IPokemon pokemon, IBattler battler, IPokeBattle_Scene scene);
			bool triggerBattleUseOnPokemon(Items item, IPokemon pokemon, IBattler battler, IHasDisplayMessage scene);

			void triggerUseInBattle(Items item, IBattler battler, IBattle battle);
		}

		/// <summary>
		/// Extension of <seealso cref="IGameItem"/>.
		/// Can be own Item Class
		/// </summary>
		public interface IItemCheck
		{
			//int ITEMID        = 0;
			//int ITEMNAME      = 1;
			//int ITEMPLURAL    = 2;
			//int ITEMPOCKET    = 3;
			//int ITEMPRICE     = 4;
			//int ITEMDESC      = 5;
			//int ITEMUSE       = 6;
			//int ITEMBATTLEUSE = 7;
			//int ITEMTYPE      = 8;
			//int ITEMMACHINE   = 9;

			/// <summary>
			/// </summary>
			/// <param name="move"></param>
			/// <returns></returns>
			/// <seealso cref="HiddenMoves"/>
			bool pbIsHiddenMove(Moves move);

			int pbGetPrice(Items item);

			ItemPockets? pbGetPocket(Items item);

			/// <summary>
			/// Important items can't be sold, given to hold, or tossed.
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			bool pbIsImportantItem(Items item);

			bool pbIsMachine(Items item);

			bool pbIsTechnicalMachine(Items item);

			bool pbIsHiddenMachine(Items item);

			bool pbIsMail(Items item);

			bool pbIsSnagBall(Items item);

			bool pbIsPokeBall(Items item);

			bool pbIsBerry(Items item);

			bool pbIsKeyItem(Items item);

			bool pbIsGem(Items item);

			bool pbIsEvolutionStone(Items item);

			/// <summary>
			/// Does NOT include Red Orb/Blue Orb
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			bool pbIsMegaStone(Items item);

			bool pbIsMulch(Items item);
		}

		/// <summary>
		/// Extension of <seealso cref="IGameItem"/>
		/// </summary>
		public interface IGameItemEffect
		{
			ItemUseResults pbRepel(Items item, int steps);
		}

		/// <summary>
		/// Extension of <seealso cref="IGame"/>
		/// </summary>
		public interface IGameItem 
		{
			void pbTopRightWindow(string text);

			void pbChangeLevel(IPokemon pokemon, int newlevel, IScene scene);

			int pbItemRestoreHP(IPokemon pokemon, int restorehp);

			bool pbHPItem(IPokemon pokemon, int restorehp, IScene scene);

			bool pbBattleHPItem(IPokemon pokemon, IBattler battler, int restorehp, IScene scene);

			int pbJustRaiseEffortValues(IPokemon pokemon, PokemonUnity.Monster.Stats ev, int evgain);

			int pbRaiseEffortValues(IPokemon pokemon, PokemonUnity.Monster.Stats ev, int evgain = 10, bool evlimit = true);

			bool pbRaiseHappinessAndLowerEV(IPokemon pokemon, IScene scene, PokemonUnity.Monster.Stats ev, string[] messages);

			int pbRestorePP(IPokemon pokemon, int move, int pp);

			int pbBattleRestorePP(IPokemon pokemon, IBattler battler, int move, int pp);

			bool pbBikeCheck();

			IGameCharacter pbClosestHiddenItem();

			void pbUseKeyItemInField(Items item);

			bool pbSpeciesCompatible(Pokemons species, Moves move);

			int pbForgetMove(IPokemon pokemon, Moves moveToLearn);

			bool pbLearnMove(IPokemon pokemon, Moves move, bool ignoreifknown = false, bool bymachine = false);

			bool pbCheckUseOnPokemon(Items item, IPokemon pokemon, IScreen screen);

			bool pbConsumeItemInBattle(IBag bag, Items item);

			// Only called when in the party screen and having chosen an item to be used on
			// the selected Pokémon
			bool pbUseItemOnPokemon(Items item, IPokemon pokemon, IScene scene);

			int pbUseItem(IBag bag, Items item, IScene bagscene = null);

			Items pbChooseItem(int var = 0, params Items[] args);

			/// <summary>
			/// Shows a list of items to choose from, with the chosen item's ID being stored
			/// in the given Global Variable. Only items which the player has are listed.
			/// </summary>
			/// <param name="message"></param>
			/// <param name="variable"></param>
			/// <param name="args"></param>
			/// <returns></returns>
			Items pbChooseItemFromList(string message, int variable, params Items[] args);
		}
	}
}