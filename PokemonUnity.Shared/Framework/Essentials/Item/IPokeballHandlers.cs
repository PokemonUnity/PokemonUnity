using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Item;

namespace PokemonEssentials.Interface
{

	namespace EventArg
	{
		#region PokeBall Handlers EventArgs
		public class OnCatchEventArg : EventArgs
		{
			public readonly int EventId = typeof(OnCatchEventArg).GetHashCode();

			public int Id { get; }
			public Items Ball { get; set; }
			public IBattle Battle { get; set; }
			public IPokemon Pokemon { get; set; }
		}
		public class OnFailCatchEventArg : EventArgs
		{
			public readonly int EventId = typeof(OnCatchEventArg).GetHashCode();

			public int Id { get; }
			public Items Ball { get; set; }
			public IBattle Battle { get; set; }
			public IBattler Battler { get; set; }
		}
		#endregion
	}

	namespace Item
	{
		public interface IBallHandlers {
			//IDictionary<Items, Func<bool>> IsUnconditional { get; }
			//IDictionary<Items, Func<int>> ModifyCatchRate { get; }
			//IDictionary<Items, Action> OnCatch { get; }
			//IDictionary<Items, Action> OnFailCatch { get; }

			event EventHandler IsUnconditional;
			event EventHandler ModifyCatchRate;
			event EventHandler<OnCatchEventArg> OnCatch;
			event EventHandler<OnFailCatchEventArg> OnFailCatch;

			bool isUnconditional(Items ball, IBattle battle, IBattler battler);

			int modifyCatchRate(Items ball, int catchRate, IBattle battle, IBattler battler);

			void onCatch(Items ball, IBattle battle, IPokemon pokemon);
			//void OnCatch(object sender, OnCatchEventArg e)

			void onFailCatch(Items ball, IBattle battle, IBattler battler);
			//void OnFailCatch(object sender, OnFailCatchEventArg e)
		}

		/// <summary>
		/// Extension of <seealso cref="IGame"/>
		/// </summary>
		public interface IGamePokeball
		{
			Items[] BallTypes { get; }

			Items pbBallTypeToBall(int balltype);

			int pbGetBallType(Items ball);
		}
	}
}