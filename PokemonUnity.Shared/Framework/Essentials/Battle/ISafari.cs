using PokemonUnity;
using PokemonUnity.Combat;

namespace PokemonEssentials.Interface.Battle
{
	public interface ISafariState 
	{
		int ballcount				{ get; }
		BattleResults decision      { get; }
		int steps				    { get; }

		ISafariState initialize();

		int pbReceptionMap { get; }

		bool InProgress { get; }

		void pbGoToStart();

		void pbStart(int ballcount);

		void pbEnd();
	}

	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGameSafari
	{
		bool pbInSafari { get; }

		ISafariState pbSafariState { get; }

		BattleResults pbSafariBattle(Pokemons species, int level);

		//Events.onMapChange+=proc{|sender,args|
		//   if (!pbInSafari?) {
		//     pbSafariState.pbEnd;
		//   }
		//}

		//Events.onStepTakenTransferPossible+=delegate(object sender, EventArgs e) {
		//   handled=e[0];
		//   if (handled[0]) continue;
		//   if (pbInSafari? && pbSafariState.decision==0 && SAFARISTEPS>0) {
		//     pbSafariState.steps-=1;
		//     if (pbSafariState.steps<=0) {
		//       Kernel.pbMessage(_INTL("PA:  Ding-dong!\1")) ;
		//       Kernel.pbMessage(_INTL("PA:  Your safari game is over!"));
		//       pbSafariState.decision=1;
		//       pbSafariState.pbGoToStart;
		//       handled[0]=true;
		//     }
		//   }
		//}

		//Events.onWildBattleOverride+= delegate(object sender, EventArgs e) {
		//   species=e[0];
		//   level=e[1];
		//   handled=e[2];
		//   if (handled[0]!=null) continue;
		//   if (!pbInSafari?) continue;
		//   handled[0]=pbSafariBattle(species,level);
		//}
	}
}