using PokemonUnity;
using PokemonUnity.Combat;

namespace PokemonEssentials.Interface.Field
{
	public interface ISafariState 
	{
		int ballcount				{ get; set; }
		BattleResults decision      { get; set; }
		int steps				    { get; set; }

		ISafariState initialize();

		int ReceptionMap { get; }

		bool InProgress { get; }

		void GoToStart();

		void Start(int ballcount);

		void End();
	}

	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGameSafari
	{
		bool InSafari { get; }

		ISafariState SafariState { get; }

		BattleResults SafariBattle(Pokemons species, int level);

		/// <summary>
		/// Fires whenever the player moves to a new map. Event handler receives the old
		/// map ID or 0 if none.  Also fires when the first map of the game is loaded
		/// </summary>
		event System.EventHandler OnMapChange;
		//Events.onMapChange+=proc{|sender,args|
		//   if (!InSafari?) {
		//     SafariState.End;
		//   }
		//}

		/// <summary>
		/// Fires whenever the player takes a step. The event handler may possibly move
		/// the player elsewhere.
		/// </summary>
		//event EventHandler<IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		event System.Action<object, EventArg.IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		//Events.onStepTakenTransferPossible+=delegate(object sender, EventArgs e) {
		//   handled=e[0];
		//   if (handled[0]) continue;
		//   if (InSafari? && SafariState.decision==0 && SAFARISTEPS>0) {
		//     SafariState.steps-=1;
		//     if (SafariState.steps<=0) {
		//       Kernel.Message(_INTL("PA:  Ding-dong!\1")) ;
		//       Kernel.Message(_INTL("PA:  Your safari game is over!"));
		//       SafariState.decision=1;
		//       SafariState.GoToStart;
		//       handled[0]=true;
		//     }
		//   }
		//}

		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		//event EventHandler<IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		event System.Action<object, EventArg.IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		//Events.onWildBattleOverride+= delegate(object sender, EventArgs e) {
		//   species=e[0];
		//   level=e[1];
		//   handled=e[2];
		//   if (handled[0]!=null) continue;
		//   if (!InSafari?) continue;
		//   handled[0]=SafariBattle(species,level);
		//}
	}
}