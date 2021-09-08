using PokemonUnity;
using PokemonUnity.Combat;

namespace PokemonEssentials.Interface
{
	public interface ISafariState
	{
		int ballcount { get; set; }
		int steps { get; set; }
		BattleResults decision { get; set; }

		ISafariState initialize();

		int pbReceptionMap { get; }

		bool inprogress { get; }

		void pbGoToStart();

		void pbStart(int ballcount);

		void pbEnd();
	}

	public interface IGameSafari
	{
		bool pbInSafari { get; }

		ISafariState pbSafariState { get; }

		BattleResults pbSafariBattle(Pokemons species, int level);
	}
}