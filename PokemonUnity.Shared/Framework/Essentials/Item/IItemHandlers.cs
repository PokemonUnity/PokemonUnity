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
		public interface IUseFromBagEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(UseFromBagEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Item { get; set; }
			ItemUseResults Response { get; set; }
		}
		public interface IUseInFieldEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(UseInFieldEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Item { get; set; }
			//Action Action { get; set; }
			bool Response { get; set; }
		}
		public interface IUseOnPokemonEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(UseOnPokemonEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Item { get; set; }
			IPokemon Pokemon { get; set; }
			PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
			bool Response { get; set; }
		}
		public interface IBattleUseOnPokemonEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(BattleUseOnPokemonEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Item { get; set; }
			IPokemon Pokemon { get; set; }
			IBattler Battler { get; set; }
			PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
			bool Response { get; set; }
		}
		public interface IBattleUseOnBattlerEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(BattleUseOnBattlerEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Item { get; set; }
			IBattler Battler { get; set; }
			PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
			bool Response { get; set; }
		}
		public interface IUseInBattleEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(UseInBattleEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Item { get; set; }
			IBattler Battler { get; set; }
			IBattle Battle { get; set; }
		}
		#endregion

		public delegate bool UseOnPokemonDelegate(Items item, IPokemon pokemon, PokemonEssentials.Interface.Screen.IHasDisplayMessage scene);
		public delegate bool BattleUseOnPokemonDelegate(IPokemon pokemon, IBattler battler, PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene); //or Pokemon Party Scene
		public delegate bool BattleUseOnBattlerDelegate(Items item, IBattler battler, PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene); //or Pokemon Party Scene
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
			
			//event EventHandler<IUseFromBagEventArgs> OnUseFromBag;
			event Action<object,IUseFromBagEventArgs> OnUseFromBag;
			//event EventHandler<IUseInFieldEventArgs> OnUseInField;
			event Action<object,IUseInFieldEventArgs> OnUseInField;
			//event EventHandler<IUseOnPokemonEventArgs> OnUseOnPokemon;
			event Action<object,IUseOnPokemonEventArgs> OnUseOnPokemon;
			//event EventHandler<IBattleUseOnBattlerEventArgs> OnBattleUseOnBattler;
			event Action<object,IBattleUseOnBattlerEventArgs> OnBattleUseOnBattler;
			//event EventHandler<IBattleUseOnPokemonEventArgs> OnBattleUseOnPokemon;
			event Action<object,IBattleUseOnPokemonEventArgs> OnBattleUseOnPokemon;
			//event EventHandler<IUseInBattleEventArgs> OnUseInBattle;
			event Action<object,IUseInBattleEventArgs> OnUseInBattle;

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

			bool triggerUseOnPokemon(Items item, IPokemon pokemon, PokemonEssentials.Interface.Screen.IHasDisplayMessage scene);

			//bool triggerBattleUseOnBattler(Items item,IBattler battler,IPokeBattle_Scene scene);
			bool triggerBattleUseOnBattler(Items item, IBattler battler, PokemonEssentials.Interface.Screen.IHasDisplayMessage scene);

			//bool triggerBattleUseOnPokemon(Items item, IPokemon pokemon, IBattler battler, IPokeBattle_Scene scene);
			bool triggerBattleUseOnPokemon(Items item, IPokemon pokemon, IBattler battler, PokemonEssentials.Interface.Screen.IHasDisplayMessage scene);

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
			bool IsHiddenMove(Moves move);

			int GetPrice(Items item);

			ItemPockets? GetPocket(Items item);

			/// <summary>
			/// Important items can't be sold, given to hold, or tossed.
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			bool IsImportantItem(Items item);

			bool IsMachine(Items item);

			bool IsTechnicalMachine(Items item);

			bool IsHiddenMachine(Items item);

			bool IsMail(Items item);

			bool IsSnagBall(Items item);

			bool IsPokeBall(Items item);

			bool IsBerry(Items item);

			bool IsKeyItem(Items item);

			bool IsGem(Items item);

			bool IsEvolutionStone(Items item);

			/// <summary>
			/// Does NOT include Red Orb/Blue Orb
			/// </summary>
			/// <param name="item"></param>
			/// <returns></returns>
			bool IsMegaStone(Items item);

			bool IsMulch(Items item);
		}

		/// <summary>
		/// Extension of <seealso cref="IGameItem"/>
		/// </summary>
		public interface IGameItemEffect
		{
			ItemUseResults Repel(Items item, int steps);
		}

		/// <summary>
		/// Extension of <seealso cref="IGame"/>
		/// </summary>
		public interface IGameItem 
		{
			void TopRightWindow(string text);

			void ChangeLevel(IPokemon pokemon, int newlevel, PokemonEssentials.Interface.Screen.IScene scene);

			int ItemRestoreHP(IPokemon pokemon, int restorehp);

			bool HPItem(IPokemon pokemon, int restorehp, PokemonEssentials.Interface.Screen.IScene scene);

			bool BattleHPItem(IPokemon pokemon, IBattler battler, int restorehp, PokemonEssentials.Interface.Screen.IScene scene);

			int JustRaiseEffortValues(IPokemon pokemon, PokemonUnity.Monster.Stats ev, int evgain);

			int RaiseEffortValues(IPokemon pokemon, PokemonUnity.Monster.Stats ev, int evgain = 10, bool evlimit = true);

			bool RaiseHappinessAndLowerEV(IPokemon pokemon, PokemonEssentials.Interface.Screen.IScene scene, PokemonUnity.Monster.Stats ev, string[] messages);

			int RestorePP(IPokemon pokemon, int move, int pp);

			int BattleRestorePP(IPokemon pokemon, IBattler battler, int move, int pp);

			bool BikeCheck();

			IGameCharacter ClosestHiddenItem();

			void UseKeyItemInField(Items item);

			bool SpeciesCompatible(Pokemons species, Moves move);

			int ForgetMove(IPokemon pokemon, Moves moveToLearn);

			bool LearnMove(IPokemon pokemon, Moves move, bool ignoreifknown = false, bool bymachine = false);

			bool CheckUseOnPokemon(Items item, IPokemon pokemon, PokemonEssentials.Interface.Screen.IScreen screen);

			bool ConsumeItemInBattle(IBag bag, Items item);

			// Only called when in the party screen and having chosen an item to be used on
			// the selected Pokémon
			bool UseItemOnPokemon(Items item, IPokemon pokemon, PokemonEssentials.Interface.Screen.IPartyDisplayScreen scene);

			int UseItem(IBag bag, Items item, PokemonEssentials.Interface.Screen.IScene bagscene = null);

			Items ChooseItem(int var = 0, params Items[] args);

			/// <summary>
			/// Shows a list of items to choose from, with the chosen item's ID being stored
			/// in the given Global Variable. Only items which the player has are listed.
			/// </summary>
			/// <param name="message"></param>
			/// <param name="variable"></param>
			/// <param name="args"></param>
			/// <returns></returns>
			Items ChooseItemFromList(string message, int variable, params Items[] args);
		}
	}
}