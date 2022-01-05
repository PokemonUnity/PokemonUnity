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
		public interface IOnCatchEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnCatchEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Ball { get; set; }
			IBattle Battle { get; set; }
			IPokemon Pokemon { get; set; }
		}
		public interface IOnFailCatchEventArgs : IEventArgs
		{
			//readonly int EventId = typeof(OnCatchEventArgs).GetHashCode();

			//int Id { get { return EventId; } }
			Items Ball { get; set; }
			IBattle Battle { get; set; }
			IBattler Battler { get; set; }
		}

		public delegate bool IsUnconditional(Items ball, IBattle battle, IBattler battler);
		public delegate int ModifyCatchRate(Items ball, int catchRate, IBattle battle, IBattler battler);
		public delegate void OnCatch(Items ball, IBattle battle, IPokemon pokemon);
		public delegate void OnFailCatch(Items ball, IBattle battle, IBattler battler);
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
			//event EventHandler<IOnCatchEventArgs> OnCatch;
			event Action<object, IOnCatchEventArgs> OnCatch;
			//event EventHandler<IOnFailCatchEventArgs> OnFailCatch;
			event Action<object, IOnFailCatchEventArgs> OnFailCatch;

			bool isUnconditional(Items ball, IBattle battle, IBattler battler);

			int modifyCatchRate(Items ball, int catchRate, IBattle battle, IBattler battler);

			void onCatch(Items ball, IBattle battle, IPokemon pokemon);
			//void OnCatch(object sender, OnCatchEventArgs e)

			void onFailCatch(Items ball, IBattle battle, IBattler battler);
			//void OnFailCatch(object sender, OnFailCatchEventArgs e)
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