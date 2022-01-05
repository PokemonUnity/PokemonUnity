using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Battle
{
	/// <summary>
	/// Extension of <seealso cref="IGame"/>
	/// </summary>
	public interface IGameOrgBattleRules
	{
		int pbBaseStatTotal(Pokemons species);

		int pbBalancedLevelFromBST(Pokemons species);

		bool pbTooTall(IPokemon pokemon, double maxHeightInMeters);

		bool pbTooHeavy(IPokemon pokemon, double maxWeightInKg);

		#region Stadium Cups
		// ##########################################
		//  Stadium Cups
		// ##########################################
		IPokemonChallengeRules pbPikaCupRules(bool @double);
		IPokemonChallengeRules pbPokeCupRules(bool @double);
		IPokemonChallengeRules pbPrimeCupRules(bool @double);
		IPokemonChallengeRules pbFancyCupRules(bool @double);
		IPokemonChallengeRules pbLittleCupRules(bool @double);
		IPokemonChallengeRules pbStrictLittleCupRules(bool @double);
		#endregion

		#region Battle Frontier Rules
		// ##########################################
		//  Battle Frontier Rules
		// ##########################################
		IPokemonChallengeRules pbBattleTowerRules(bool @double, bool openlevel);
		IPokemonChallengeRules pbBattlePalaceRules(bool @double, bool openlevel);
		IPokemonChallengeRules pbBattleArenaRules(bool openlevel);
		IPokemonChallengeRules pbBattleFactoryRules(bool @double, bool openlevel);
		#endregion
	}

	public interface ILevelAdjustment
	{
		//int BothTeams { get; } //= 0;
		//int EnemyTeam { get; } //= 1;
		//int MyTeam { get; } //= 2;
		//int BothTeamsDifferent { get; } //= 3;

		int type { get; }

		//ILevelAdjustment(int adjustment);

		//int[] getNullAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);

		int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);

		int?[] getOldExp(IPokemon[] team1, IPokemon[] team2);

		void unadjustLevels(IPokemon[] team1, IPokemon[] team2, int?[][] adjustments);

		int?[][] adjustLevels(IPokemon[] team1, IPokemon[] team2);
		int[] getMyAdjustment(IPokemon[] myTeam, IPokemon[] theirTeam);
		int[] getTheirAdjustment(IPokemon[] theirTeam, IPokemon[] myTeam);
	}

	public interface ILevelBalanceAdjustment : ILevelAdjustment
	{
		//ILevelBalanceAdjustment(int minLevel);
		//int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);
	}

	public interface IEnemyLevelAdjustment : ILevelAdjustment
	{
		//IEnemyLevelAdjustment(int level);
		//int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);
	}

	public interface ICombinedLevelAdjustment : ILevelAdjustment
	{
		//ICombinedLevelAdjustment(ILevelAdjustment my, ILevelAdjustment their);
		//int[] getMyAdjustment(IPokemon[] myTeam, IPokemon[] theirTeam);
		//int[] getTheirAdjustment(IPokemon[] theirTeam, IPokemon[] myTeam);
	}

	public interface ISinglePlayerCappedLevelAdjustment : ICombinedLevelAdjustment
	{
		//ISinglePlayerCappedLevelAdjustment(int level);
	}

	public interface ICappedLevelAdjustment : ILevelAdjustment
	{
		//ICappedLevelAdjustment(int level);
		//int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);
	}

	public interface IFixedLevelAdjustment : ILevelAdjustment
	{
		//IFixedLevelAdjustment(int level);
		//int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);
	}

	public interface ITotalLevelAdjustment : ILevelAdjustment
	{
		//ITotalLevelAdjustment(int minLevel, int maxLevel, int totalLevel);
		//int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);
	}

	public interface IOpenLevelAdjustment : ILevelAdjustment
	{
		//IOpenLevelAdjustment(int minLevel = 1);
		//int[] getAdjustment(IPokemon[] thisTeam, IPokemon[] otherTeam);
	}

	public interface INonEggRestriction : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface IAblePokemonRestriction : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface ISpeciesRestriction : IBattleRestriction
	{
		//ISpeciesRestriction(params Pokemons[] specieslist);
		//bool isValid(IPokemon pokemon);
		bool isSpecies(Pokemons species, Pokemons[] specieslist);
	}

	public interface IBannedSpeciesRestriction : IBattleRestriction
	{
		//IBannedSpeciesRestriction(params Pokemons[] specieslist);
		//bool isValid(IPokemon pokemon);
		bool isSpecies(Pokemons species, Pokemons[] specieslist);
	}

	public interface IBannedItemRestriction : IBattleRestriction
	{
		//IBannedItemRestriction(params Items[] specieslist);
		//bool isValid(IPokemon pokemon);
		//bool isSpecies (Pokemons species,Pokemons[] specieslist);
		bool isSpecies(Items species, Items[] specieslist);
	}

	public interface IRestrictedSpeciesRestriction : IBattleTeamRestriction
	{
		//IRestrictedSpeciesRestriction(int maxValue, params Pokemons[] specieslist);
		//bool isValid(IPokemon[] team);
		bool isSpecies(Pokemons species, Pokemons[] specieslist);
	}

	public interface IRestrictedSpeciesTeamRestriction : IRestrictedSpeciesRestriction
	{
		//IRestrictedSpeciesTeamRestriction(params Pokemons[] specieslist);
	}

	public interface IRestrictedSpeciesSubsetRestriction : IRestrictedSpeciesRestriction
	{
		//IRestrictedSpeciesSubsetRestriction(params Pokemons[] specieslist);
	}

	public interface IStandardRestriction : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface ILevelRestriction { }

	public interface IMinimumLevelRestriction : IBattleRestriction
	{
		int level { get; }

		//IMinimumLevelRestriction(int minLevel);
		//bool isValid(IPokemon pokemon);
	}

	public interface IMaximumLevelRestriction : IBattleRestriction
	{
		int level { get; }

		//IMaximumLevelRestriction(int maxLevel);
		//bool isValid(IPokemon pokemon);
	}

	public interface IHeightRestriction : IBattleRestriction
	{
		//IHeightRestriction(int maxHeightInMeters);
		//bool isValid(IPokemon pokemon);
	}

	public interface IWeightRestriction : IBattleRestriction
	{
		//IWeightRestriction(int maxWeightInKg);
		//bool isValid(IPokemon pokemon);
	}

	public interface ISoulDewClause : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface IItemsDisallowedClause : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface INegativeExtendedGameClause : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface ITotalLevelRestriction : IBattleTeamRestriction
	{
		int level { get; }

		//ITotalLevelRestriction(int level);
		//bool isValid(IPokemon[] team);
		//string errorMessage { get; }
	}

	public interface ISameSpeciesClause : IBattleTeamRestriction
	{
		//bool isValid(IPokemon[] team);
		//string errorMessage { get; }
	}

	public interface ISpeciesClause : IBattleTeamRestriction
	{
		//bool isValid(IPokemon[] team);
		//string errorMessage { get; }
	}

	//Pokemons[] babySpeciesData = {}
	//Pokemons[] canEvolve       = {}

	public interface IBabyRestriction : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface IUnevolvedFormRestriction : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface ILittleCupRestriction : IBattleRestriction
	{
		//bool isValid(IPokemon pokemon);
	}

	public interface IItemClause : IBattleTeamRestriction
	{
		//bool isValid(IPokemon[] team);
		//string errorMessage { get; }
	}

	public interface INicknameChecker
	{
		Dictionary<Pokemons, string> names { get; }
		int namesMaxValue { get; }

		string getName(Pokemons species);
		bool check(string name, Pokemons species);
	}

	/// <summary>
	/// No two Pokemon can have the same nickname.
	/// No nickname can be the same as the (real) name of another Pokemon character.
	/// </summary>
	public interface INicknameClause : IBattleTeamRestriction
	{
		//bool isValid(IPokemon[] team);
		//string errorMessage { get; }
	}

	public interface IPokemonRuleSet
	{
		int minTeamLength { get; }
		int maxTeamLength { get; }
		int minLength { get; }
		int maxLength { get; }
		int number { get; }

		//IPokemonRuleSet(int number = 0);

		IPokemonRuleSet copy();

		/// <summary>
		/// Returns the length of a valid subset of a Pokemon team.
		/// </summary>
		int suggestedNumber { get; }

		/// <summary>
		/// Returns a valid level to assign to each member of a valid Pokemon team.
		/// </summary>
		/// <returns></returns>
		int suggestedLevel { get; }

		IPokemonRuleSet setNumberRange(int minValue, int maxValue);

		IPokemonRuleSet setNumber(int value);

		IPokemonRuleSet addPokemonRule(IBattleRestriction rule);

		//  This rule checks
		//  - the entire team to determine whether a subset of the team meets the rule, or 
		//  - a list of Pokemon whose length is equal to the suggested number. For an
		//    entire team, the condition must hold for at least one possible subset of
		//    the team, but not necessarily for the entire team.
		//  A subset rule is "number-dependent", that is, whether the condition is likely
		//  to hold depends on the number of Pokemon in the subset.
		//  Example of a subset rule:
		//  - The combined level of X Pokemon can't exceed Y.
		IPokemonRuleSet addSubsetRule(IBattleTeamRestriction rule);

		/// <summary>
		///  This rule checks either<para>
		///  - the entire team to determine whether a subset of the team meets the rule, or </para>
		///  - whether the entire team meets the rule. If the condition holds for the
		///    entire team, the condition must also hold for any possible subset of the
		///    team with the suggested number.
		/// </summary>
		/// <example>
		///  Examples of team rules:
		///  - No two Pokemon can be the same species.
		///  - No two Pokemon can hold the same items.
		/// </example>
		/// <param name="rule"></param>
		/// <returns></returns>
		IPokemonRuleSet addTeamRule(IBattleTeamRestriction rule);

		IPokemonRuleSet clearPokemonRules();

		IPokemonRuleSet clearTeamRules();

		IPokemonRuleSet clearSubsetRules();

		bool isPokemonValid(IPokemon pokemon);

		bool hasRegistrableTeam(IPokemon[] list);

		/// <summary>
		///  Returns true if the team's length is greater or equal to the suggested number
		///  and is 6 or less, the team as a whole meets the requirements of any team
		///  rules, and at least one subset of the team meets the requirements of any
		///  subset rules. Each Pokemon in the team must be valid.
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
		bool canRegisterTeam(IPokemon[] team);

		/// <summary>
		///  Returns true if the team's length is greater or equal to the suggested number
		///  and at least one subset of the team meets the requirements of any team rules
		///  and subset rules. Not all Pokemon in the team have to be valid.
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
		bool hasValidTeam(IPokemon[] team);

		/// <summary>
		///  Returns true if the team's length meets the subset length range requirements
		///  and the team meets the requirements of any team rules and subset rules. Each
		///  Pokemon in the team must be valid.
		/// </summary>
		/// <param name="team"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		bool isValid(IPokemon[] team, IList<string> error = null);
	}

	public interface IBattleType //: IBattleType
	{
		IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2);
	}

	public interface IBattleTower : IBattleType
	{
		//IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2);
	}

	public interface IBattlePalace : IBattleType
	{
		//IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2);
	}

	public interface IBattleArena : IBattleType
	{
		//IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2);
	}

	public interface IBattleRule //: IBattleRule
	{
		//const string SOULDEWCLAUSE = "souldewclause";
		//const string SLEEPCLAUSE = "sleepclause";
		//const string FREEZECLAUSE = "freezeclause";
		//const string EVASIONCLAUSE = "evasionclause";
		//const string OHKOCLAUSE = "ohkoclause";
		//const string PERISHSONG = "perishsong";
		//const string SELFKOCLAUSE = "selfkoclause";
		//const string SELFDESTRUCTCLAUSE = "selfdestructclause";
		//const string SONICBOOMCLAUSE = "sonicboomclause";
		//const string MODIFIEDSLEEPCLAUSE = "modifiedsleepclause";
		//const string SKILLSWAPCLAUSE = "skillswapclause";
		void setRule(IBattle battle);
	}

	public interface IDoubleBattle : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISingleBattle : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISoulDewBattleClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISleepClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface IFreezeClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface IEvasionClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface IOHKOClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface IPerishSongClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISelfKOClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISelfdestructClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISonicBoomClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface IModifiedSleepClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface ISkillSwapClause : IBattleRule
	{
		//void setRule(IBattle battle);
	}

	public interface IPokemonChallengeRules
	{
		IPokemonRuleSet ruleset				{ get; }
		IBattleType battletype				{ get; }
		ILevelAdjustment levelAdjustment	{ get; }

		//IPokemonChallengeRules(IPokemonRuleSet ruleset = null);

		IPokemonChallengeRules copy();

		int number { get; }

		IPokemonChallengeRules setNumber(int number);

		IPokemonChallengeRules setDoubleBattle(bool value);

		int?[][] adjustLevelsBilateral(IPokemon[] party1, IPokemon[] party2);

		void unadjustLevelsBilateral(IPokemon[] party1, IPokemon[] party2, int?[][] adjusts);

		int?[][] adjustLevels(IPokemon[] party1, IPokemon[] party2);

		void unadjustLevels(IPokemon[] party1, IPokemon[] party2, int?[][] adjusts);

		IPokemonChallengeRules addPokemonRule(IBattleRestriction rule);

		IPokemonChallengeRules addLevelRule(int minLevel, int maxLevel, int totalLevel);

		IPokemonChallengeRules addSubsetRule(IBattleTeamRestriction rule);

		IPokemonChallengeRules addTeamRule(IBattleTeamRestriction rule);

		IPokemonChallengeRules addBattleRule(IBattleRule rule);

		IBattle createBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2);

		IPokemonChallengeRules setRuleset(IPokemonRuleSet rule);

		IPokemonChallengeRules setBattleType(IBattleType rule);

		IPokemonChallengeRules setLevelAdjustment(ILevelAdjustment rule);
	}

	#region Generation IV Cups
	// ##########################################
	//  Generation IV Cups
	// ##########################################
	public interface IStandardRules : IPokemonRuleSet
	{
		//int number				{ get; set; }

		//IStandardRules(int number, int? level = null);
	}

	public interface IStandardCup : IStandardRules
	{
		string name { get; }
		//IStandardCup();
	}

	public interface IDoubleCup : IStandardRules
	{
		string name { get; }
		//IDoubleCup();
	}

	public interface IFancyCup : IPokemonRuleSet
	{
		string name { get; }
		//IFancyCup();
	}

	public interface ILittleCup : IPokemonRuleSet
	{
		string name { get; }
		//ILittleCup();
	}

	public interface ILightCup : IPokemonRuleSet
	{
		//ILightCup();
		string name { get; }
	}
	#endregion	

	public interface IBattleRestriction
	{
		//string errorMessage { get; }
		bool isValid(IPokemon pokemon);
	}
	public interface IBattleTeamRestriction
	{
		string errorMessage { get; }
		bool isValid(IPokemon[] team);
	}
}