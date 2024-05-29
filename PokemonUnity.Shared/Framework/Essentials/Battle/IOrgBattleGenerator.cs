using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Battle
{
	/*=begin;
	[3/10];
	0-266 - 0-500;
	[106];
	267-372 - 380-500;
	[95];
	373-467 - 400-555 (nonlegendary);
	468-563 - 400-555 (nonlegendary);
	564-659 - 400-555 (nonlegendary);
	660-755 - 400-555 (nonlegendary);
	756-799 - 580-600 [legendary] (compat1==15 or compat2==15, genderbyte=255);
	800-849 - 500-;
	850-881 - 580- ;
	=end;*/

	public interface IBaseStatRestriction : IBattleRestriction
	{
		//IBaseStatRestriction initialize(int mn,int mx);

		//bool isValid(IPokemon pkmn);
	}

	public interface INonlegendaryRestriction : IBattleRestriction
	{
		//bool isValid (IPokemon pkmn);
	}

	public interface IInverseRestriction : IBattleRestriction
	{
		IBattleRestriction initialize(IBattleRestriction r);

		//bool isValid (pkmn);
	}

	public interface ISingleMatch {
		float  opponentRating				{ get; }
		float  opponentDeviation				{ get; }
		int score				{ get; }
		int kValue				{ get; }

		ISingleMatch initialize(float opponentRating, float opponentDev, int score, int kValue = 16);
	}

	public interface IMatchHistory : IEnumerable<ISingleMatch>
	{
		//include Enumerable;

		IEnumerable<ISingleMatch> each();

		int length { get; }

		ISingleMatch this[int i] { get; }

		IMatchHistory initialize(IPlayerRating thisPlayer);

		//  1=I won; 0=Other player won; -1: Draw
		void addMatch(IPlayerRating otherPlayer, int result);

		void updateAndClear();
	}

	public interface IPlayerRatingElo : IPlayerRating {
		//float rating				{ get; }
		//K_VALUE = 16;

		new IPlayerRatingElo initialize();

		//float winChancePercent { get; }

		void update(IList<ISingleMatch> matches);
	}

	public interface IPlayerRating {
		float volatility				{ get; }
		float deviation				{ get; }
		float rating				    { get; }

		IPlayerRating initialize();

		float winChancePercent { get; }

		void update(IList<ISingleMatch> matches, float system = 1.2f);

		//  private int volatility				{ get; }

		//  private void rating2() {
		//    return (@rating-1500.0)/173.7178;
		//  }

		//  private void deviation2() {
		//    return (@deviation)/173.7178;
		//  }

		//  private void getGFactor(deviation) {
		////  deviation is not yet in glicko2
		//    deviation/=173.7178;
		//    return 1.0 / Math.sqrt(1.0 + (3.0*deviation*deviation) / (Math.PI*Math.PI));
		//  }

		//  private void getEFactor(rating,opponentRating, g) {
		////  rating is already in glicko2
		////  opponentRating is not yet in glicko2
		//    opponentRating=(opponentRating-1500.0)/173.7178;
		//    return 1.0 / (1.0 + Math.exp(-g * (rating - opponentRating)));
		//  }

		//  alias volatility2 volatility;

		//  private void setVolatility2(float value) {
		//    @volatility=value;
		//  }

		//  private void setRating2(float value) {
		//    @estimatedRating=null;
		//    @rating=(value*173.7178)+1500.0;
		//  }

		//  private void setDeviation2(float value) {
		//    @estimatedRating=null;
		//    @deviation=(value*173.7178);
		//  }

		//  private void getUpdatedVolatility(float volatility, float deviation, float variance, float improvementSum, float system) {
		//    improvement = improvementSum * variance;
		//    a = Math.log(volatility * volatility);
		//    squSystem = system * system;
		//    squDeviation = deviation * deviation;
		//    squVariance = variance + variance;
		//    squDevplusVar = squDeviation + variance;
		//    x0 = a;
		//    100.times { // Up to 100 iterations to avoid potentially infinite loops
		//       e = Math.exp(x0);
		//       d = squDevplusVar + e;
		//       squD = d * d;
		//       i = improvement / d;
		//       h1 = -(x0 - a) / squSystem - 0.5 * e * i * i;
		//       h2 = -1.0 / squSystem - 0.5 * e * squDevplusVar / squD;
		//       h2 += 0.5 * squVariance * e * (squDevplusVar - e) / (squD * d);
		//       x1 = x0;
		//       x0 -= h1 / h2;
		//       if (((x1 - x0).abs < 0.000001)) {
		//         break;
		//       }
		//    }
		//    return Math.exp(x0 / 2.0);
		//  }
	}

	public interface IRuledTeam {
		//IPlayerRating rating { get; }
		float rating { get; }

		float[] ratingRaw { get; }

		IPlayerRating ratingData { get; }

		int totalGames { get; }

		void updateRating();

		//void compare(IRuledTeam other);
		bool compare(IRuledTeam other);

		//void addMatch(IRuledTeam other, bool? score);
		void addMatch(IRuledTeam other, int score);

		int games { get; }

		IList<IPokemon> team				{ get; }

		IRuledTeam initialize(IList<IPokemon> party, IPokemonChallengeRules rule);

		IPokemon this[int i] { get; }

		//string toStr();
		string ToString();

		int length { get; }

		IList<IPokemon> load(IList<IPokemon> party);
	}

	public interface IGameOrgBattleGenerator
	{
		//$tmData          = null;
		//$legalMoves      = [];
		//$legalMovesLevel = 0;
		//$moveData        = [];
		//$baseStatTotal   = [];
		//$minimumLevel    = [];
		//$babySpecies     = [];
		//$evolutions      = [];
		//$tmMoves         = null;

		Moves RandomMove();

		void addMove(IList<Moves> moves, Moves move, int @base);

		IList<Moves> GetLegalMoves2(Pokemons species, int maxlevel);

		int baseStatTotal(Pokemons species);

		Pokemons babySpecies(Pokemons species);

		int minimumLevel(Pokemons species);

		IList<Pokemons> evolutions(Pokemons pkmn);

		PokemonUnity.Attack.Data.MoveData moveData(Moves move);


		IPokemonChallengeRules withRestr(IPokemonChallengeRules rule, int minbs, int maxbs, int legendary);

		// The Pokemon list is already roughly arranged by rank from weakest to strongest
		IList<IPokemon> ArrangeByTier(IList<IPokemon> pokemonlist, IPokemonChallengeRules rule);

		bool hasMorePowerfulMove(IList<Moves> moves, Moves thismove);

		IPokemon RandomPokemonFromRule(IPokemonChallengeRules rule, ITrainer trainer);



		int DecideWinnerEffectiveness(Moves move, Types otype1, Types otype2, Abilities ability, int[] scores);

		double DecideWinnerScore(IList<IPokemon> party0, IList<IPokemon> party1, double rating);

		int DecideWinner(IList<IPokemon> party0, IList<IPokemon> party1, double rating0, double rating1);

		void RuledBattle(IRuledTeam team1, IRuledTeam team2, IPokemonChallengeRules rule);

		Types[] getTypes(Pokemons species);

		//void TrainerInfo(IList<IPokemonSerialized> pokemonlist, string trfile, IPokemonChallengeRules rules);
		IEnumerator TrainerInfo(IList<IPokemonSerialized> pokemonlist, int trfile, IPokemonChallengeRules rules, Action block_given = null);

		//#if FAKERGSS
		//public void Kernel.MessageDisplay(mw,txt,lbl) {
		//  puts txt;
		//}

		//public void _INTL(*arg) {
		//  return arg[0];
		//}

		//public void string.Format(*arg) {
		//  return arg[0];
		//}
		//#endif

		bool isBattlePokemonDuplicate(IPokemon pk, IPokemon pk2);

		IList<IPokemon> RemoveDuplicates(IList<IPokemon> party);
		//IPokemon[] RemoveDuplicates(IPokemon[] party);

		void ReplenishBattlePokemon(IList<IPokemon> party, IPokemonChallengeRules rule);

		IEnumerator<string> GenerateChallenge(IPokemonChallengeRules rule, int tag);

		void WriteCup(int id, IPokemonChallengeRules rules);
	}
}