using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Utility;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface;

namespace PokemonUnity
{
	public partial class Game : PokemonEssentials.Interface.Battle.IGameOrgBattleRules
	{
		public int pbBaseStatTotal(Pokemons species)
		{
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,species,10);
			//int bst=dexdata.fgetb;
			int bst = PokemonData[species].BaseStatsHP; //dexdata.fgetb;
			bst += PokemonData[species].BaseStatsATK; //dexdata.fgetb;
			bst += PokemonData[species].BaseStatsDEF; //dexdata.fgetb;
			bst += PokemonData[species].BaseStatsSPE; //dexdata.fgetb;
			bst += PokemonData[species].BaseStatsSPA; //dexdata.fgetb;
			bst += PokemonData[species].BaseStatsSPD; //dexdata.fgetb;
			//dexdata.close();
			return bst;
		}

		public int pbBalancedLevelFromBST(Pokemons species)
		{
			return (int)Math.Round(113 - (pbBaseStatTotal(species) * 0.072));
		}

		public bool pbTooTall(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, double maxHeightInMeters)
		{
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,pokemon is Numeric ? pokemon : pokemon.Species,33);
			float height = PokemonData[pokemon.Species].Height; //dexdata.fgetw;
			float weight = PokemonData[pokemon.Species].Weight; //dexdata.fgetw;
			maxHeightInMeters = Math.Round(maxHeightInMeters * 10);
			//dexdata.close();
			return height > maxHeightInMeters;
		}

		public bool pbTooHeavy(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, double maxWeightInKg)
		{
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,pokemon is Numeric ? pokemon : pokemon.Species,33);
			float height = PokemonData[pokemon.Species].Height; //dexdata.fgetw;
			float weight = PokemonData[pokemon.Species].Weight; //dexdata.fgetw;
			maxWeightInKg = Math.Round(maxWeightInKg * 10f);
			//dexdata.close();
			return weight > maxWeightInKg;
		}

		#region Stadium Cups
		// ##########################################
		//  Stadium Cups
		// ##########################################
		public IPokemonChallengeRules pbPikaCupRules(bool @double)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			ret.addPokemonRule(new StandardRestriction());
			ret.addLevelRule(15, 20, 50);
			ret.addTeamRule(new SpeciesClause());
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SleepClause());
			ret.addBattleRule(new FreezeClause());
			ret.addBattleRule(new SelfKOClause());
			ret.setDoubleBattle(@double).setNumber(3);
			return ret;
		}

		public IPokemonChallengeRules pbPokeCupRules(bool @double)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			ret.addPokemonRule(new StandardRestriction());
			ret.addLevelRule(50, 55, 155);
			ret.addTeamRule(new SpeciesClause());
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SleepClause());
			ret.addBattleRule(new FreezeClause());
			ret.addBattleRule(new SelfdestructClause());
			ret.setDoubleBattle(@double).setNumber(3);
			return ret;
		}

		public IPokemonChallengeRules pbPrimeCupRules(bool @double)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			ret.setLevelAdjustment(new OpenLevelAdjustment(Core.MAXIMUMLEVEL));
			ret.addTeamRule(new SpeciesClause());
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SleepClause());
			ret.addBattleRule(new FreezeClause());
			ret.addBattleRule(new SelfdestructClause());
			ret.setDoubleBattle(@double);
			return ret;
		}

		public IPokemonChallengeRules pbFancyCupRules(bool @double)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			ret.addPokemonRule(new StandardRestriction());
			ret.addLevelRule(25, 30, 80);
			ret.addPokemonRule(new HeightRestriction(2));
			ret.addPokemonRule(new WeightRestriction(20));
			ret.addPokemonRule(new BabyRestriction());
			ret.addTeamRule(new SpeciesClause());
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SleepClause());
			ret.addBattleRule(new FreezeClause());
			ret.addBattleRule(new PerishSongClause());
			ret.addBattleRule(new SelfdestructClause());
			ret.setDoubleBattle(@double).setNumber(3);
			return ret;
		}

		public IPokemonChallengeRules pbLittleCupRules(bool @double)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			ret.addPokemonRule(new StandardRestriction());
			ret.addPokemonRule(new UnevolvedFormRestriction());
			ret.setLevelAdjustment(new EnemyLevelAdjustment(5));
			ret.addPokemonRule(new MaximumLevelRestriction(5));
			ret.addTeamRule(new SpeciesClause());
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SleepClause());
			ret.addBattleRule(new FreezeClause());
			ret.addBattleRule(new SelfdestructClause());
			ret.addBattleRule(new PerishSongClause());
			ret.addBattleRule(new SonicBoomClause());
			ret.setDoubleBattle(@double);
			return ret;
		}

		public IPokemonChallengeRules pbStrictLittleCupRules(bool @double)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			ret.addPokemonRule(new StandardRestriction());
			ret.addPokemonRule(new UnevolvedFormRestriction());
			ret.setLevelAdjustment(new EnemyLevelAdjustment(5));
			ret.addPokemonRule(new MaximumLevelRestriction(5));
			ret.addPokemonRule(new LittleCupRestriction());
			ret.addTeamRule(new SpeciesClause());
			ret.addBattleRule(new SleepClause());
			ret.addBattleRule(new EvasionClause());
			ret.addBattleRule(new OHKOClause());
			ret.addBattleRule(new SelfKOClause());
			ret.setDoubleBattle(@double).setNumber(3);
			return ret;
		}
		#endregion

		#region Battle Frontier Rules
		// ##########################################
		//  Battle Frontier Rules
		// ##########################################
		public IPokemonChallengeRules pbBattleTowerRules(bool @double, bool openlevel)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			if (openlevel)
			{
				ret.setLevelAdjustment(new OpenLevelAdjustment(60));
			}
			else
			{
				ret.setLevelAdjustment(new CappedLevelAdjustment(50));
			}
			ret.addPokemonRule(new StandardRestriction());
			ret.addTeamRule(new SpeciesClause());
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SoulDewBattleClause());
			ret.setDoubleBattle(@double);
			return ret;
		}

		public IPokemonChallengeRules pbBattlePalaceRules(bool @double, bool openlevel)
		{
			return pbBattleTowerRules(@double, openlevel).setBattleType(new BattlePalace());
		}

		public IPokemonChallengeRules pbBattleArenaRules(bool openlevel)
		{
			return pbBattleTowerRules(false, openlevel).setBattleType(new BattleArena());
		}

		public IPokemonChallengeRules pbBattleFactoryRules(bool @double, bool openlevel)
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules();
			if (openlevel)
			{
				ret.setLevelAdjustment(new FixedLevelAdjustment(100));
				ret.addPokemonRule(new MaximumLevelRestriction(100));
			}
			else
			{
				ret.setLevelAdjustment(new FixedLevelAdjustment(50));
				ret.addPokemonRule(new MaximumLevelRestriction(50));
			}
			ret.addTeamRule(new SpeciesClause());
			ret.addPokemonRule(new BannedSpeciesRestriction(Pokemons.UNOWN));
			ret.addTeamRule(new ItemClause());
			ret.addBattleRule(new SoulDewBattleClause());
			ret.setDoubleBattle(@double).setNumber(0);
			return ret;
		}
		#endregion

		#region Other Interesting Rulesets
		/*/ ##########################################
		// Other Interesting Rulesets
		// ##########################################

		// Official Species Restriction
		new PokemonChallengeRules()
		.addPokemonRule(new BannedSpeciesRestriction(
		   Pokemons.MEWTWO,Pokemons.MEW,
		   Pokemons.LUGIA,Pokemons.HOOH,Pokemons.CELEBI,
		   Pokemons.KYOGRE,Pokemons.GROUDON,Pokemons.RAYQUAZA,Pokemons.JIRACHI,Pokemons.DEOXYS,
		   Pokemons.DIALGA,Pokemons.PALKIA,Pokemons.GIRATINA,Pokemons.MANAPHY,Pokemons.PHIONE,
		   Pokemons.DARKRAI,Pokemons.SHAYMIN,Pokemons.ARCEUS))
		.addBattleRule(new SoulDewBattleClause());



		// New Official Species Restriction
		new PokemonChallengeRules()
		.addPokemonRule(new BannedSpeciesRestriction(
		   Pokemons.MEW,
		   Pokemons.CELEBI,
		   Pokemons.JIRACHI,Pokemons.DEOXYS,
		   Pokemons.MANAPHY,Pokemons.PHIONE,Pokemons.DARKRAI,Pokemons.SHAYMIN,Pokemons.ARCEUS))
		.addBattleRule(new SoulDewBattleClause());



		// Pocket Monsters Stadium
		new PokemonChallengeRules()
		.addPokemonRule(new SpeciesRestriction(
		   Pokemons.VENUSAUR,Pokemons.CHARIZARD,Pokemons.BLASTOISE,Pokemons.BEEDRILL,Pokemons.FEAROW,
		   Pokemons.PIKACHU,Pokemons.NIDOQUEEN,Pokemons.NIDOKING,Pokemons.DUGTRIO,Pokemons.PRIMEAPE,
		   Pokemons.ARCANINE,Pokemons.ALAKAZAM,Pokemons.MACHAMP,Pokemons.GOLEM,Pokemons.MAGNETON,
		   Pokemons.CLOYSTER,Pokemons.GENGAR,Pokemons.ONIX,Pokemons.HYPNO,Pokemons.ELECTRODE,
		   Pokemons.EXEGGUTOR,Pokemons.CHANSEY,Pokemons.KANGASKHAN,Pokemons.STARMIE,Pokemons.SCYTHER,
		   Pokemons.JYNX,Pokemons.PINSIR,Pokemons.TAUROS,Pokemons.GYARADOS,Pokemons.LAPRAS,
		   Pokemons.DITTO,Pokemons.VAPOREON,Pokemons.JOLTEON,Pokemons.FLAREON,Pokemons.AERODACTYL,
		   Pokemons.SNORLAX,Pokemons.ARTICUNO,Pokemons.ZAPDOS,Pokemons.MOLTRES,Pokemons.DRAGONITE
		));



		// 1999 Tournament Rules
		new PokemonChallengeRules()
		.addTeamRule(new SpeciesClause())
		.addPokemonRule(new ItemsDisallowedClause())
		.addBattleRule(new SleepClause())
		.addBattleRule(new FreezeClause())
		.addBattleRule(new SelfdestructClause())
		.setDoubleBattle(false)
		.setLevelRule(1,50,150)
		.addPokemonRule(new BannedSpeciesRestriction(
		   Pokemons.VENUSAUR,Pokemons.DUGTRIO,Pokemons.ALAKAZAM,Pokemons.GOLEM,Pokemons.MAGNETON,
		   Pokemons.GENGAR,Pokemons.HYPNO,Pokemons.ELECTRODE,Pokemons.EXEGGUTOR,Pokemons.CHANSEY,
		   Pokemons.KANGASKHAN,Pokemons.STARMIE,Pokemons.JYNX,Pokemons.TAUROS,Pokemons.GYARADOS,
		   Pokemons.LAPRAS,Pokemons.DITTO,Pokemons.VAPOREON,Pokemons.JOLTEON,Pokemons.SNORLAX,
		   Pokemons.ARTICUNO,Pokemons.ZAPDOS,Pokemons.DRAGONITE,Pokemons.MEWTWO,Pokemons.MEW));



		// 2005 Tournament Rules
		new PokemonChallengeRules()
		.addPokemonRule(new BannedSpeciesRestriction(
		   Pokemons.DRAGONITE,Pokemons.MEW,Pokemons.MEWTWO,
		   Pokemons.TYRANITAR,Pokemons.LUGIA,Pokemons.CELEBI,Pokemons.HOOH,Pokemons.GROUDON,Pokemons.KYOGRE,Pokemons.RAYQUAZA,
		   Pokemons.JIRACHI,Pokemons.DEOXYS))
		.setDoubleBattle(true)
		.addLevelRule(1,50,200)
		.addTeamRule(new ItemClause())
		.addPokemonRule(new BannedItemRestriction(Items.SOULDEW,Items.ENIGMA_BERRY))
		.addBattleRule(new SleepClause())
		.addBattleRule(new FreezeClause())
		.addBattleRule(new SelfdestructClause())
		.addBattleRule(new PerishSongClause());



		// 2008 Tournament Rules
		new PokemonChallengeRules()
		.addPokemonRule(new BannedSpeciesRestriction(
		   Pokemons.MEWTWO,Pokemons.MEW,Pokemons.TYRANITAR,Pokemons.LUGIA,Pokemons.HOOH,Pokemons.CELEBI,
		   Pokemons.GROUDON,Pokemons.KYOGRE,Pokemons.RAYQUAZA,Pokemons.JIRACHI,Pokemons.DEOXYS,
		   Pokemons.PALKIA,Pokemons.DIALGA,Pokemons.PHIONE,Pokemons.MANAPHY,Pokemons.ROTOM,Pokemons.SHAYMIN,Pokemons.DARKRAI
		.setDoubleBattle(true)
		.addLevelRule(1,50,200)
		.addTeamRule(new NicknameClause())
		.addTeamRule(new ItemClause())
		.addBattleRule(new SoulDewBattleClause());



		// 2010 Tournament Rules
		new PokemonChallengeRules()
		.addPokemonRule(new BannedSpeciesRestriction(
		   Pokemons.MEW,Pokemons.CELEBI,Pokemons.JIRACHI,Pokemons.DEOXYS,
		   Pokemons.PHIONE,Pokemons.MANAPHY,Pokemons.SHAYMIN,Pokemons.DARKRAI,Pokemons.ARCEUS));
		.addSubsetRule(new RestrictedSpeciesSubsetRestriction(
		   Pokemons.MEWTWO,Pokemons.LUGIA,Pokemons.HOOH,
		   Pokemons.GROUDON,Pokemons.KYOGRE,Pokemons.RAYQUAZA,
		   Pokemons.PALKIA,Pokemons.DIALGA,Pokemons.GIRATINA))
		.setDoubleBattle(true)
		.addLevelRule(1,100,600)
		.setLevelAdjustment(new CappedLevelAdjustment(50))
		.addTeamRule(new NicknameClause())
		.addTeamRule(new ItemClause())
		.addPokemonRule(new SoulDewClause());



		// Pokemon Colosseum -- Anything Goes
		new PokemonChallengeRules()
		.addLevelRule(1,100,600)
		.addBattleRule(new SleepClause())
		.addBattleRule(new FreezeClause())
		.addBattleRule(new SelfdestructClause())
		.addBattleRule(new PerishSongClause());



		// Pokemon Colosseum -- Max Lv. 50
		new PokemonChallengeRules()
		.addLevelRule(1,50,300)
		.addTeamRule(new SpeciesClause())
		.addTeamRule(new ItemClause())
		.addBattleRule(new SleepClause())
		.addBattleRule(new FreezeClause())
		.addBattleRule(new SelfdestructClause())
		.addBattleRule(new PerishSongClause());



		// Pokemon Colosseum -- Max Lv. 100
		new PokemonChallengeRules()
		.addLevelRule(1,100,600)
		.addTeamRule(new SpeciesClause())
		.addTeamRule(new ItemClause())
		.addBattleRule(new SleepClause())
		.addBattleRule(new FreezeClause())
		.addBattleRule(new SelfdestructClause())
		.addBattleRule(new PerishSongClause());



		// Battle Time (includes animations)
		If the time runs out, the team with the most Pokemon left wins. If both teams have
		the same number of Pokémon left, total HP remaining breaks the tie. If both HP
		totals are identical, the battle is a draw.

		// Command Time
		If the player is in the process of switching Pokémon when the time runs out, the
		one that can still battle that's closest to the top of the roster is chosen.
		Otherwise, the attack on top of the list is chosen.*/
		#endregion
	}

	#region Rules Class for Inheriting
	public partial class LevelAdjustment : PokemonEssentials.Interface.Battle.ILevelAdjustment
	{
		public const int BothTeams = 0;
		public const int EnemyTeam = 1;
		public const int MyTeam = 2;
		public const int BothTeamsDifferent = 3;
		private int adjustment;

		public virtual int type
		{
			get
			{
				return @adjustment;
			}
		}

		public LevelAdjustment(int adjustment)
		{
			this.adjustment = adjustment;
		}

		//int[] PokemonEssentials.Interface.Battle.ILevelAdjustment.getNullAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		//{
		//	return LevelAdjustment.getNullAdjustment(thisTeam, otherTeam);
		//}

		public static int[] getNullAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int[] ret = new int[6];
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = thisTeam[i].Level;
			}
			return ret;
		}

		public virtual int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			return LevelAdjustment.getNullAdjustment(thisTeam, otherTeam);
		}

		public int?[] getOldExp(PokemonEssentials.Interface.PokeBattle.IPokemon[] team1, PokemonEssentials.Interface.PokeBattle.IPokemon[] team2)
		{
			//IList<int> ret=new List<int>();
			int?[] ret = new int?[team1.Length];
			for (int i = 0; i < team1.Length; i++)
			{
				//ret.Add(team1[i].exp);
				if (team1[i].IsNotNullOrNone()) ret[i] = team1[i].exp;
			}
			//return ret.ToArray();
			return ret;
		}

		public virtual void unadjustLevels(PokemonEssentials.Interface.PokeBattle.IPokemon[] team1, PokemonEssentials.Interface.PokeBattle.IPokemon[] team2, int?[][] adjustments)
		{
			for (int i = 0; i < team1.Length; i++)
			{
				int? exp = adjustments[0][i];
				if (exp != null && team1[i].exp != exp)
				{
					team1[i].exp = exp.Value;
					team1[i].calcStats();
				}
			}
			for (int i = 0; i < team2.Length; i++)
			{
				int? exp = adjustments[1][i];
				if (exp != null && team2[i].exp != exp)
				{
					team2[i].exp = exp.Value;
					team2[i].calcStats();
				}
			}
		}

		public virtual int?[][] adjustLevels(PokemonEssentials.Interface.PokeBattle.IPokemon[] team1, PokemonEssentials.Interface.PokeBattle.IPokemon[] team2)
		{
			int[] adj1 = null;
			int[] adj2 = null;
			int?[][] ret = new int?[][] { getOldExp(team1, team2), getOldExp(team2, team1) };
			if (@adjustment == BothTeams || @adjustment == MyTeam)
			{
				adj1 = getAdjustment(team1, team2);
			}
			else if (@adjustment == BothTeamsDifferent)
			{
				adj1 = getMyAdjustment(team1, team2);
			}
			if (@adjustment == BothTeams || @adjustment == EnemyTeam)
			{
				adj2 = getAdjustment(team2, team1);
			}
			else if (@adjustment == BothTeamsDifferent)
			{
				adj2 = getTheirAdjustment(team2, team1);
			}
			if (adj1 != null)
			{
				for (int i = 0; i < team1.Length; i++)
				{
					if (team1[i].Level != adj1[i])
					{
						//team1[i].Level=adj1[i];
						(team1[i] as Pokemon).SetLevel((byte)adj1[i]);
						team1[i].calcStats();
					}
				}
			}
			if (adj2 != null)
			{
				for (int i = 0; i < team2.Length; i++)
				{
					if (team2[i].Level != adj2[i])
					{
						//team2[i].Level=adj2[i];
						(team2[i] as Pokemon).SetLevel((byte)adj2[i]);
						team2[i].calcStats();
					}
				}
			}
			return ret;
		}
		public virtual int[] getMyAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] myTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] theirTeam)
		{
			return getNullAdjustment(myTeam, theirTeam);
		}
		public virtual int[] getTheirAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] theirTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] myTeam)
		{
			return getNullAdjustment(theirTeam, myTeam);
		}
	}

	public partial class LevelBalanceAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.ILevelBalanceAdjustment
	{
		private int minLevel;
		public LevelBalanceAdjustment(int minLevel) : base(LevelAdjustment.BothTeams)
		{
			//base.initialize(LevelAdjustment.BothTeams);
			this.minLevel = minLevel;
		}

		public override int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int[] ret = new int[6];
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = (Game.GameData as IGameOrgBattleRules).pbBalancedLevelFromBST(thisTeam[i].Species);
			}
			return ret;
		}
	}

	public partial class EnemyLevelAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.IEnemyLevelAdjustment
	{
		private int level;
		public EnemyLevelAdjustment(int level) : base(LevelAdjustment.EnemyTeam)
		{
			//base.initialize(LevelAdjustment.EnemyTeam);
			this.level = Math.Min(Math.Max(1, level), Core.MAXIMUMLEVEL);
		}

		public override int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int[] ret = new int[6];
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = @level;
			}
			return ret;
		}
	}

	public partial class CombinedLevelAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.ICombinedLevelAdjustment
	{
		private ILevelAdjustment my;
		private ILevelAdjustment their;
		public CombinedLevelAdjustment(LevelAdjustment my, LevelAdjustment their) : base(LevelAdjustment.BothTeamsDifferent)
		{
			//base.initialize(LevelAdjustment.BothTeamsDifferent);
			this.my = my;
			this.their = their;
		}

		public override int[] getMyAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] myTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] theirTeam)
		{
			return @my != null ? @my.getAdjustment(myTeam, theirTeam) :
				LevelAdjustment.getNullAdjustment(myTeam, theirTeam);
		}

		public override int[] getTheirAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] theirTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] myTeam)
		{
			return @their != null ? @their.getAdjustment(theirTeam, myTeam) :
				LevelAdjustment.getNullAdjustment(theirTeam, myTeam);
		}
	}

	public partial class SinglePlayerCappedLevelAdjustment : CombinedLevelAdjustment, PokemonEssentials.Interface.Battle.ISinglePlayerCappedLevelAdjustment
	{
		public SinglePlayerCappedLevelAdjustment(int level) : base(new CappedLevelAdjustment(level), new FixedLevelAdjustment(level))
		{
			//base.initialize(new CappedLevelAdjustment(level),new FixedLevelAdjustment(level));
		}
	}

	public partial class CappedLevelAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.ICappedLevelAdjustment
	{
		private int level;
		public CappedLevelAdjustment(int level) : base(LevelAdjustment.BothTeams)
		{
			//base.initialize(LevelAdjustment.BothTeams);
			this.level = Math.Min(Math.Max(1, level), Core.MAXIMUMLEVEL);
		}

		public override int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int[] ret = new int[6];
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = Math.Min(thisTeam[i].Level, @level);
			}
			return ret;
		}
	}

	public partial class FixedLevelAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.IFixedLevelAdjustment
	{
		private int level;
		public FixedLevelAdjustment(int level) : base(LevelAdjustment.BothTeams)
		{
			//base.initialize(LevelAdjustment.BothTeams);
			this.level = Math.Min(Math.Max(1, level), Core.MAXIMUMLEVEL);
		}

		public override int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int[] ret = new int[6];
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = @level;
			}
			return ret;
		}
	}

	public partial class TotalLevelAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.ITotalLevelAdjustment
	{
		private int minLevel;
		private int maxLevel;
		private int totalLevel;
		public TotalLevelAdjustment(int minLevel, int maxLevel, int totalLevel) : base(LevelAdjustment.EnemyTeam)
		{
			//base.initialize(LevelAdjustment.EnemyTeam);
			this.minLevel = Math.Min(Math.Max(1, minLevel), Core.MAXIMUMLEVEL);
			this.maxLevel = Math.Min(Math.Max(1, maxLevel), Core.MAXIMUMLEVEL);
			this.totalLevel = totalLevel;
		}

		public override int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int[] ret = new int[6];
			int total = 0;
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = @minLevel;
				total += @minLevel;
			}
			do //;loop
			{
				bool work = false;
				for (int i = 0; i < thisTeam.Length; i++)
				{
					if (ret[i] >= @maxLevel || total >= @totalLevel)
					{
						continue;
					}
					ret[i] += 1;
					total += 1;
					work = true;
				}
				if (!work) break;
			} while (true);
			return ret;
		}
	}

	public partial class OpenLevelAdjustment : LevelAdjustment, PokemonEssentials.Interface.Battle.IOpenLevelAdjustment
	{
		private int minLevel;
		public OpenLevelAdjustment(int minLevel = 1) : base(LevelAdjustment.EnemyTeam)
		{
			//base.initialize(LevelAdjustment.EnemyTeam);
			this.minLevel = minLevel;
		}

		public override int[] getAdjustment(PokemonEssentials.Interface.PokeBattle.IPokemon[] thisTeam, PokemonEssentials.Interface.PokeBattle.IPokemon[] otherTeam)
		{
			int maxLevel = 1;
			for (int i = 0; i < otherTeam.Length; i++)
			{
				int level = otherTeam[i].Level;
				if (maxLevel < level) maxLevel = level;
			}
			if (maxLevel < @minLevel) maxLevel = @minLevel;
			int[] ret = new int[6];
			for (int i = 0; i < thisTeam.Length; i++)
			{
				ret[i] = maxLevel;
			}
			return ret;
		}
	}

	public partial class NonEggRestriction : PokemonEssentials.Interface.Battle.IBattleRestriction, PokemonEssentials.Interface.Battle.INonEggRestriction
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return pokemon != null && !pokemon.isEgg;
		}
	}

	public partial class AblePokemonRestriction : PokemonEssentials.Interface.Battle.IBattleRestriction, PokemonEssentials.Interface.Battle.IAblePokemonRestriction
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return pokemon != null && !pokemon.isEgg && pokemon.HP > 0;
		}
	}

	public partial class SpeciesRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.ISpeciesRestriction
	{
		private Pokemons[] specieslist;
		public SpeciesRestriction(params Pokemons[] specieslist)
		{
			this.specieslist = specieslist;
		}

		public bool isSpecies(Pokemons species, Pokemons[] specieslist)
		{
			foreach (var s in specieslist)
			{
				if (species == s) return true;
			}
			return false;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			int count = 0;
			if (isSpecies(pokemon.Species, @specieslist))
			{
				count += 1;
			}
			return count != 0;
		}
	}

	public partial class BannedSpeciesRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IBannedSpeciesRestriction
	{
		private Pokemons[] specieslist;
		public BannedSpeciesRestriction(params Pokemons[] specieslist)
		{
			this.specieslist = specieslist;
		}

		public bool isSpecies(Pokemons species, Pokemons[] specieslist)
		{
			foreach (var s in specieslist)
			{
				if (species == s) return true;
			}
			return false;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			int count = 0;
			if (isSpecies(pokemon.Species, @specieslist))
			{
				count += 1;
			}
			return count == 0;
		}
	}

	public partial class BannedItemRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IBannedItemRestriction
	{
		private Items[] specieslist;
		public BannedItemRestriction(params Items[] specieslist)
		{
			this.specieslist = specieslist;
		}

		//public bool isSpecies (Pokemons species,Pokemons[] specieslist) {
		public bool isSpecies(Items species, Items[] specieslist)
		{
			foreach (var s in specieslist)
			{
				if (species == s) return true;
			}
			return false;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			int count = 0;
			if (pokemon.Item != 0 && isSpecies(pokemon.Item, @specieslist))
			{
				count += 1;
			}
			return count == 0;
		}
	}

	public partial class RestrictedSpeciesRestriction : IBattleTeamRestriction, PokemonEssentials.Interface.Battle.IRestrictedSpeciesRestriction
	{
		string IBattleTeamRestriction.errorMessage { get; }
		private int maxValue;
		private Pokemons[] specieslist;
		public RestrictedSpeciesRestriction(int maxValue, params Pokemons[] specieslist)
		{
			this.specieslist = specieslist;
			this.maxValue = maxValue;
		}

		public bool isSpecies(Pokemons species, Pokemons[] specieslist)
		{
			foreach (var s in specieslist)
			{
				if (species == s) return true;
			}
			return false;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			int count = 0;
			for (int i = 0; i < team.Length; i++)
			{
				if (isSpecies(team[i].Species, @specieslist))
				{
					count += 1;
				}
			}
			return count <= @maxValue;
		}
	}

	public partial class RestrictedSpeciesTeamRestriction : RestrictedSpeciesRestriction, PokemonEssentials.Interface.Battle.IRestrictedSpeciesTeamRestriction
	{
		public RestrictedSpeciesTeamRestriction(params Pokemons[] specieslist) : base(4, specieslist)
		{
			//base.initialize(4,*specieslist);
		}
	}

	public partial class RestrictedSpeciesSubsetRestriction : RestrictedSpeciesRestriction, PokemonEssentials.Interface.Battle.IRestrictedSpeciesSubsetRestriction
	{
		public RestrictedSpeciesSubsetRestriction(params Pokemons[] specieslist) : base(2, specieslist)
		{
			//base.initialize(2,*specieslist);
		}
	}

	public partial class StandardRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IStandardRestriction
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			if (pokemon == null || pokemon.isEgg) return false;
			//dexdata=pbOpenDexData();
			//pbDexDataOffset(dexdata,pokemon.Species,10);
			int basestatsum = Kernal.PokemonData[pokemon.Species].BaseStatsHP; //dexdata.fgetb;
			basestatsum += Kernal.PokemonData[pokemon.Species].BaseStatsATK; //dexdata.fgetb;
			basestatsum += Kernal.PokemonData[pokemon.Species].BaseStatsDEF; //dexdata.fgetb;
			basestatsum += Kernal.PokemonData[pokemon.Species].BaseStatsSPE; //dexdata.fgetb;
			basestatsum += Kernal.PokemonData[pokemon.Species].BaseStatsSPD; //dexdata.fgetb;
			basestatsum += Kernal.PokemonData[pokemon.Species].BaseStatsSPA; //dexdata.fgetb;
			//pbDexDataOffset(dexdata,pokemon.Species,2);
			Abilities ability1 = Kernal.PokemonData[pokemon.Species].Ability[0]; //dexdata.fgetw;
			Abilities ability2 = Kernal.PokemonData[pokemon.Species].Ability[1]; //dexdata.fgetw;
			//dexdata.close();
			//  Species with disadvantageous abilities are not banned
			if (ability1 == Abilities.TRUANT ||
				ability2 == Abilities.SLOW_START)
			{
				return true;
			}
			//  Certain named species are banned
			Pokemons[] blacklist = new Pokemons[] { Pokemons.WYNAUT, Pokemons.WOBBUFFET };
			foreach (var i in blacklist)
			{
				if (pokemon.Species == i) return false;
			}
			//  Certain named species are not banned
			Pokemons[] whitelist = new Pokemons[] { Pokemons.DRAGONITE, Pokemons.SALAMENCE, Pokemons.TYRANITAR };
			foreach (Pokemons i in whitelist)
			{
				if (pokemon.Species == i) return true;
			}
			//  Species with total base stat 600 or more are banned
			if (basestatsum >= 600)
			{
				return false;
			}
			return true;
		}
	}

	public partial class LevelRestriction : PokemonEssentials.Interface.Battle.ILevelRestriction { }

	public partial class MinimumLevelRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IMinimumLevelRestriction
	{
		public int level { get; protected set; }

		public MinimumLevelRestriction(int minLevel)
		{
			@level = minLevel;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return pokemon.Level >= @level;
		}
	}

	public partial class MaximumLevelRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IMaximumLevelRestriction
	{
		public int level { get; protected set; }

		public MaximumLevelRestriction(int maxLevel)
		{
			@level = maxLevel;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return pokemon.Level <= @level;
		}
	}

	public partial class HeightRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IHeightRestriction
	{
		private int level;
		public HeightRestriction(int maxHeightInMeters)
		{
			@level = maxHeightInMeters;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return !(Game.GameData as IGameOrgBattleRules).pbTooTall(pokemon, @level);
		}
	}

	public partial class WeightRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IWeightRestriction
	{
		private int level;
		public WeightRestriction(int maxWeightInKg)
		{
			@level = maxWeightInKg;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return !(Game.GameData as IGameOrgBattleRules).pbTooHeavy(pokemon, @level);
		}
	}

	public partial class SoulDewClause : IBattleRestriction, PokemonEssentials.Interface.Battle.ISoulDewClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return pokemon.Item != Items.SOUL_DEW;
		}
	}

	public partial class ItemsDisallowedClause : IBattleRestriction, PokemonEssentials.Interface.Battle.IItemsDisallowedClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			return !pokemon.hasItem();
		}
	}

	public partial class NegativeExtendedGameClause : IBattleRestriction, PokemonEssentials.Interface.Battle.INegativeExtendedGameClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			if (pokemon.Species == Pokemons.ARCEUS) return false;
			if (pokemon.Item == Items.MICLE_BERRY) return false;
			if (pokemon.Item == Items.CUSTAP_BERRY) return false;
			if (pokemon.Item == Items.JABOCA_BERRY) return false;
			if (pokemon.Item == Items.ROWAP_BERRY) return false;
			return true;
		}
	}

	public partial class TotalLevelRestriction : IBattleTeamRestriction, PokemonEssentials.Interface.Battle.ITotalLevelRestriction
	{
		public int level { get; protected set; }

		public TotalLevelRestriction(int level)
		{
			this.level = level;
		}

		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			int totalLevel = 0;
			for (int i = 0; i < team.Length - 1; i++)
			{
				if (team[i].Species == 0) continue;
				totalLevel += team[i].Level;
			}
			return (totalLevel <= @level);
		}

		public string errorMessage
		{
			get
			{
				return string.Format("The combined levels exceed {0}.", @level);
			}
		}
	}

	public partial class SameSpeciesClause : IBattleTeamRestriction, PokemonEssentials.Interface.Battle.ISameSpeciesClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			Pokemons species = 0;
			for (int i = 0; i < team.Length - 1; i++)
			{
				if (team[i].Species == 0) continue;
				if (species == 0)
				{
					species = team[i].Species;
				}
				else
				{
					if (team[i].Species != species) return false;
				}
			}
			return true;
		}

		public string errorMessage
		{
			get
			{
				return Game._INTL("Pokémon can't be the same.");
			}
		}
	}

	public partial class SpeciesClause : IBattleTeamRestriction, PokemonEssentials.Interface.Battle.ISpeciesClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			for (int i = 0; i < team.Length - 1; i++)
			{
				if (team[i].Species == 0) continue;
				for (int j = i + 1; j < team.Length; j++)
				{
					if (team[i].Species == team[j].Species) return false;
				}
			}
			return true;
		}

		public string errorMessage
		{
			get
			{
				return Game._INTL("Pokémon can't be the same.");
			}
		}
	}

	//public static Pokemons[] babySpeciesData = {}
	//public static Pokemons[] canEvolve       = {}

	public partial class BabyRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IBabyRestriction
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			//baby=babySpeciesData[pokemon.Species] != null ? babySpeciesData[pokemon.Species] :
			//   (babySpeciesData[pokemon.Species]=pbGetBabySpecies(pokemon.Species));
			//return baby==pokemon.Species;
			return Game.PokemonData[pokemon.Species].IsBaby;
		}
	}

	public partial class UnevolvedFormRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.IUnevolvedFormRestriction
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			//baby=$babySpeciesData[pokemon.Species] ? $babySpeciesData[pokemon.Species] :
			//   ($babySpeciesData[pokemon.Species]=pbGetBabySpecies(pokemon.Species))
			//if (baby!=pokemon.Species) return false;
			if (!Game.PokemonData[pokemon.Species].IsBaby) return false;
			//bool canEvolve=(canEvolve[pokemon.Species]!=null) ? canEvolve[pokemon.Species] :
			//   (canEvolve[pokemon.Species]=(pbGetEvolvedFormData(pokemon.Species).Length!=0));
			bool canEvolve = Game.PokemonEvolutionsData[pokemon.Species].Length != 0;
			if (!canEvolve) return false;
			return true;
		}
	}

	public partial class LittleCupRestriction : IBattleRestriction, PokemonEssentials.Interface.Battle.ILittleCupRestriction
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			if (pokemon.Item == Items.BERRY_JUICE) return false;
			if (pokemon.Item == Items.DEEP_SEA_TOOTH) return false;
			if (pokemon.hasMove(Moves.SONIC_BOOM)) return false;
			if (pokemon.hasMove(Moves.DRAGON_RAGE)) return false;
			if (pokemon.Species == Pokemons.SCYTHER) return false;
			if (pokemon.Species == Pokemons.SNEASEL) return false;
			if (pokemon.Species == Pokemons.MEDITITE) return false;
			if (pokemon.Species == Pokemons.YANMA) return false;
			if (pokemon.Species == Pokemons.TANGELA) return false;
			if (pokemon.Species == Pokemons.MURKROW) return false;
			return true;
		}
	}

	public partial class ItemClause : IBattleTeamRestriction, PokemonEssentials.Interface.Battle.IItemClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			for (int i = 0; i < team.Length - 1; i++)
			{
				if (!team[i].hasItem()) continue;
				for (int j = i + 1; j < team.Length; j++)
				{
					if (team[i].Item == team[j].Item) return false;
				}
			}
			return true;
		}

		public string errorMessage
		{
			get
			{
				return Game._INTL("No identical hold items.");
			}
		}
	}

	public partial class NicknameChecker //: PokemonEssentials.Interface.Battle.INicknameChecker
	{
		public static IDictionary<Pokemons, string> names = new Dictionary<Pokemons, string>();
		public static int namesMaxValue = 0;

		public static string getName(Pokemons species)
		{
			string n = names[species];
			if (n != null) return n;
			n = species.ToString(TextScripts.Name);
			names[species] = n.ToUpper(); //.upcase;
			return n;
		}

		public static bool check(string name, Pokemons species)
		{
			name = name.ToUpper(); //.upcase;
			if (name == getName(species)) return true;
			if (@names.Values.Contains(name))
			{
				return false;
			}
			//foreach (var i in @@namesMaxValue..PBSpecies.maxValue) {
			foreach (Pokemons i in Kernal.PokemonData.Keys)
			{
				if (i != species)
				{
					string n = getName(i);
					if (n == name) return false;
				}
			}
			return true;
		}
	}

	/// <summary>
	/// No two Pokemon can have the same nickname.
	/// No nickname can be the same as the (real) name of another Pokemon character.
	/// </summary>
	public partial class NicknameClause : IBattleTeamRestriction, PokemonEssentials.Interface.Battle.INicknameClause
	{
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			for (int i = 0; i < team.Length - 1; i++)
			{
				for (int j = i + 1; j < team.Length; j++)
				{
					if (team[i].Name == team[j].Name) return false;
					if (!NicknameChecker.check(team[i].Name, team[i].Species)) return false;
				}
			}
			return true;
		}

		public string errorMessage
		{
			get
			{
				return Game._INTL("No identical nicknames.");
			}
		}
	}

	public partial class PokemonRuleSet : PokemonEssentials.Interface.Battle.IPokemonRuleSet
	{
		public int minTeamLength
		{
			get
			{
				return Math.Min(1, this.minLength);
			}
		}

		public int maxTeamLength
		{
			get
			{
				return Math.Max(6, this.maxLength);
			}
		}

		public int minLength
		{
			get
			{
				return _minLength.HasValue ? _minLength.Value : this.maxLength;
			}
		}

		public int maxLength
		{
			get
			{
				return @number < 0 ? 6 : @number;
			}
		}

		public int number
		{
			get
			{
				return _maxLength;
			}
		}
		private int _maxLength;
		private int? _minLength;
		private IList<IBattleRestriction> pokemonRules;
		private IList<IBattleTeamRestriction> teamRules;
		private IList<IBattleTeamRestriction> subsetRules;

		public PokemonRuleSet(int number = 0)
		{
			@pokemonRules = new List<IBattleRestriction>();
			@teamRules = new List<IBattleTeamRestriction>();
			@subsetRules = new List<IBattleTeamRestriction>();
			_minLength = 1;
			_maxLength = number <= 0 ? (Game.GameData as Game).Features.LimitPokemonPartySize : number;
		}

		public IPokemonRuleSet copy()
		{
			IPokemonRuleSet ret = new PokemonRuleSet(@number);
			foreach (var rule in @pokemonRules)
			{
				ret.addPokemonRule(rule);
			}
			foreach (var rule in @teamRules)
			{
				ret.addTeamRule(rule);
			}
			foreach (var rule in @subsetRules)
			{
				ret.addSubsetRule(rule);
			}
			return ret;
		}

		/// <summary>
		/// Returns the length of a valid subset of a Pokemon team.
		/// </summary>
		public int suggestedNumber
		{
			get
			{
				return this.maxLength;
			}
		}

		/// <summary>
		/// Returns a valid level to assign to each member of a valid Pokemon team.
		/// </summary>
		/// <returns></returns>
		public int suggestedLevel
		{ get {  
			int minLevel = 1;
			int maxLevel = Core.MAXIMUMLEVEL;
			int num = this.suggestedNumber;
			foreach (var rule in @pokemonRules)
			{
				if (rule is IMinimumLevelRestriction r1)
				{
					minLevel = r1.level; //rule.level;
				}
				else if (rule is IMaximumLevelRestriction r2)
				{
					maxLevel = r2.level; //rule.level;
				}
			}
			int totalLevel = maxLevel * num;
			foreach (var rule in @subsetRules)
			{
				if (rule is ITotalLevelRestriction r)
				{
					totalLevel = r.level; //rule.level;
				}
			}
			if (totalLevel >= maxLevel * num)
			{
				return Math.Max(maxLevel, minLevel);
			}
			else
			{
				return Math.Max((totalLevel / this.suggestedNumber), minLevel);
			}
		} }

		public IPokemonRuleSet setNumberRange(int minValue, int maxValue)
		{
			_minLength = Math.Max(1, minValue);
			_maxLength = Math.Min(maxValue, 6);
			return this;
		}

		public IPokemonRuleSet setNumber(int value)
		{
			return setNumberRange(value, value);
		}

		public IPokemonRuleSet addPokemonRule(IBattleRestriction rule)
		{
			@pokemonRules.Add(rule);
			return this;
		}

		/// <summary>
		///  This rule checks
		///  - the entire team to determine whether a subset of the team meets the rule, or 
		///  - a list of Pokemon whose length is equal to the suggested number. For an
		///    entire team, the condition must hold for at least one possible subset of
		///    the team, but not necessarily for the entire team.
		///  A subset rule is "number-dependent", that is, whether the condition is likely
		///  to hold depends on the number of Pokemon in the subset.
		/// </summary>
		/// <example>
		///  Example of a subset rule:
		///  - The combined level of X Pokemon can't exceed Y.
		/// </example>
		/// <param name="rule"></param>
		/// <returns>
		/// </returns>
		public IPokemonRuleSet addSubsetRule(IBattleTeamRestriction rule)
		{
			@teamRules.Add(rule);
			return this;
		}

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
		public IPokemonRuleSet addTeamRule(IBattleTeamRestriction rule)
		{
			@teamRules.Add(rule);
			return this;
		}

		public IPokemonRuleSet clearPokemonRules()
		{
			@pokemonRules.Clear();
			return this;
		}

		public IPokemonRuleSet clearTeamRules()
		{
			@teamRules.Clear();
			return this;
		}

		public IPokemonRuleSet clearSubsetRules()
		{
			@subsetRules.Clear();
			return this;
		}

		public bool isPokemonValid(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			if (!pokemon.IsNotNullOrNone()) return false;
			foreach (var rule in @pokemonRules)
			{
				if (!rule.isValid(pokemon))
				{
					return false;
				}
			}
			return true;
		}

		public bool hasRegistrableTeam(PokemonEssentials.Interface.PokeBattle.IPokemon[] list)
		{
			if (list == null || list.Length < this.minTeamLength) return false;
			//Array.ForEach<PokemonEssentials.Interface.PokeBattle.IPokemon[]>( //PokemonEssentials.Interface.PokeBattle.IPokemon[] |comb|
			//	(Game.GameData as IGameUtility).pbEachCombination(list, this.maxTeamLength), (comb) => {
			foreach ( PokemonEssentials.Interface.PokeBattle.IPokemon[] comb in
				(Game.GameData as IGameUtility).pbEachCombination(list, this.maxTeamLength)) {
				if (canRegisterTeam(comb)) return true;
			};
			return false;
		}

		/// <summary>
		///  Returns true if the team's length is greater or equal to the suggested number
		///  and is 6 or less, the team as a whole meets the requirements of any team
		///  rules, and at least one subset of the team meets the requirements of any
		///  subset rules. Each Pokemon in the team must be valid.
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
		public bool canRegisterTeam(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			if (team == null || team.Length < this.minTeamLength)
			{
				return false;
			}
			if (team.Length > this.maxTeamLength)
			{
				return false;
			}
			int teamNumber = Math.Min(this.maxLength, team.Length);
			foreach (var pokemon in team)
			{
				if (!isPokemonValid(pokemon))
				{
					return false;
				}
			}
			foreach (var rule in @teamRules)
			{
				if (!rule.isValid(team))
				{
					return false;
				}
			}
			if (@subsetRules.Count > 0)
			{
				//pbEachCombination(team,teamNumber){|comb|
				//   bool isValid=true;
				//   foreach (var rule in @subsetRules) {
				//     if (!rule.isValid(comb)) {
				//       isValid=false;
				//       break;
				//     }
				//   }
				//   if (isValid) return true;
				//}
				return false;
			}
			return true;
		}

		/// <summary>
		///  Returns true if the team's length is greater or equal to the suggested number
		///  and at least one subset of the team meets the requirements of any team rules
		///  and subset rules. Not all Pokemon in the team have to be valid.
		/// </summary>
		/// <param name="team"></param>
		/// <returns></returns>
		public bool hasValidTeam(PokemonEssentials.Interface.PokeBattle.IPokemon[] team)
		{
			if (team == null || team.Length < this.minTeamLength)
			{
				return false;
			}
			int teamNumber = Math.Min(this.maxLength, team.Length);
			IList<PokemonEssentials.Interface.PokeBattle.IPokemon> validPokemon = new List<PokemonEssentials.Interface.PokeBattle.IPokemon>();
			foreach (var pokemon in team)
			{
				if (isPokemonValid(pokemon))
				{
					validPokemon.Add(pokemon);
				}
			}
			if (validPokemon.Count < teamNumber)
			{
				return false;
			}
			if (@teamRules.Count > 0)
			{
				//pbEachCombination(team,teamNumber){|comb|
				//   if (isValid(comb)) {
				//     return true;
				//   }
				//}
				return false;
			}
			return true;
		}

		/// <summary>
		///  Returns true if the team's length meets the subset length range requirements
		///  and the team meets the requirements of any team rules and subset rules. Each
		///  Pokemon in the team must be valid.
		/// </summary>
		/// <param name="team"></param>
		/// <param name="error"></param>
		/// <returns></returns>
		public bool isValid(PokemonEssentials.Interface.PokeBattle.IPokemon[] team, IList<string> error = null)
		{
			if (team.Length < this.minLength)
			{
				if (error != null && this.minLength == 1) error.Add(Game._INTL("Choose a Pokémon."));
				if (error != null && this.minLength > 1) error.Add(Game._INTL("{1} Pokémon are needed.", this.minLength));
				return false;
			}
			else if (team.Length > this.maxLength)
			{
				if (error != null) error.Add(Game._INTL("No more than {1} Pokémon may enter.", this.maxLength));
				return false;
			}
			foreach (IPokemon pokemon in team)
			{
				if (!isPokemonValid(pokemon))
				{
					if (pokemon.IsNotNullOrNone())
					{
						if (error != null) error.Add(Game._INTL("This team is not allowed.", pokemon.Name));
					}
					else
					{
						if (error != null) error.Add(Game._INTL("{1} is not allowed.", pokemon.Name));
					}
					return false;
				}
			}
			foreach (var rule in @teamRules)
			{
				if (!rule.isValid(team))
				{
					if (error != null) error.Add(rule.errorMessage);
					return false;
				}
			}
			foreach (var rule in @subsetRules)
			{
				if (!rule.isValid(team))
				{
					if (error != null) error.Add(rule.errorMessage);
					return false;
				}
			}
			return true;
		}
	}

	public partial class BattleType : IBattleType
	{
		public virtual PokemonEssentials.Interface.PokeBattle.IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2)
		{
			return (PokemonEssentials.Interface.PokeBattle.IBattle)new Combat.Battle(scene,
				trainer1[0].party, trainer2[0].party, trainer1, trainer2);
		}
	}

	public partial class BattleTower : BattleType, PokemonEssentials.Interface.Battle.IBattleTower
	{
		public override PokemonEssentials.Interface.PokeBattle.IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2)
		{
			//ToDo: Uncomment... After adding recorded battles classes
			//return (PokemonEssentials.Interface.PokeBattle.IBattle)new Combat.PokeBattle_RecordedBattle(scene,
			//	trainer1[0].party, trainer2[0].party, trainer1, trainer2);
			throw new NotImplementedException("Removed because didnt finish setting up recorded battle classes");
		}
	}

	public partial class BattlePalace : BattleType, PokemonEssentials.Interface.Battle.IBattlePalace
	{
		public override PokemonEssentials.Interface.PokeBattle.IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2)
		{
			//ToDo: Uncomment... After adding recorded battles classes
			//return (PokemonEssentials.Interface.PokeBattle.IBattle)new Combat.PokeBattle_RecordedBattlePalace(scene,
			//	trainer1[0].party, trainer2[0].party, trainer1, trainer2);
			throw new NotImplementedException("Removed because didnt finish setting up recorded battle classes");
		}
	}

	public partial class BattleArena : BattleType, PokemonEssentials.Interface.Battle.IBattleArena
	{
		public override PokemonEssentials.Interface.PokeBattle.IBattle pbCreateBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2)
		{
			//ToDo: Uncomment... After adding recorded battles classes
			//return (PokemonEssentials.Interface.PokeBattle.IBattle)new Combat.PokeBattle_RecordedBattleArena(scene,
			//	trainer1[0].party, trainer2[0].party, trainer1, trainer2);
			throw new NotImplementedException("Removed because didnt finish setting up recorded battle classes");
		}
	}

	public abstract partial class BattleRule : IBattleRule
	{
		public const string SOULDEWCLAUSE = "souldewclause";
		public const string SLEEPCLAUSE = "sleepclause";
		public const string FREEZECLAUSE = "freezeclause";
		public const string EVASIONCLAUSE = "evasionclause";
		public const string OHKOCLAUSE = "ohkoclause";
		public const string PERISHSONG = "perishsong";
		public const string PERISHSONGCLAUSE = "perishsongclause";
		public const string SELFKOCLAUSE = "selfkoclause";
		public const string SELFDESTRUCTCLAUSE = "selfdestructclause";
		public const string SONICBOOMCLAUSE = "sonicboomclause";
		public const string MODIFIEDSLEEPCLAUSE = "modifiedsleepclause";
		public const string SKILLSWAPCLAUSE = "skillswapclause";
		public const string SUDDENDEATH = "suddendeath";
		public const string MODIFIEDSELFDESTRUCTCLAUSE = "modifiedselfdestructclause";
		public virtual void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { }
	}

	public partial class DoubleBattle : BattleRule, PokemonEssentials.Interface.Battle.IDoubleBattle
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle)
		{
			battle.doublebattle = battle.pbDoubleBattleAllowed();
		}
	}

	public partial class SingleBattle : BattleRule, PokemonEssentials.Interface.Battle.ISingleBattle
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle)
		{
			battle.doublebattle = false;
		}
	}

	public partial class SoulDewBattleClause : BattleRule, PokemonEssentials.Interface.Battle.ISoulDewBattleClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.SOULDEWCLAUSE] = true; }
	}

	public partial class SleepClause : BattleRule, PokemonEssentials.Interface.Battle.ISleepClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.SLEEPCLAUSE] = true; }
	}

	public partial class FreezeClause : BattleRule, PokemonEssentials.Interface.Battle.IFreezeClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.FREEZECLAUSE] = true; }
	}

	public partial class EvasionClause : BattleRule, PokemonEssentials.Interface.Battle.IEvasionClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.EVASIONCLAUSE] = true; }
	}

	public partial class OHKOClause : BattleRule, PokemonEssentials.Interface.Battle.IOHKOClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.OHKOCLAUSE] = true; }
	}

	public partial class PerishSongClause : BattleRule, PokemonEssentials.Interface.Battle.IPerishSongClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.PERISHSONG] = true; }
	}

	public partial class SelfKOClause : BattleRule, PokemonEssentials.Interface.Battle.ISelfKOClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.SELFKOCLAUSE] = true; }
	}

	public partial class SelfdestructClause : BattleRule, PokemonEssentials.Interface.Battle.ISelfdestructClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.SELFDESTRUCTCLAUSE] = true; }
	}

	public partial class SonicBoomClause : BattleRule, PokemonEssentials.Interface.Battle.ISonicBoomClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.SONICBOOMCLAUSE] = true; }
	}

	public partial class ModifiedSleepClause : BattleRule, PokemonEssentials.Interface.Battle.IModifiedSleepClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.MODIFIEDSLEEPCLAUSE] = true; }
	}

	public partial class SkillSwapClause : BattleRule, PokemonEssentials.Interface.Battle.ISkillSwapClause
	{
		public override void setRule(PokemonEssentials.Interface.PokeBattle.IBattle battle) { battle.rules[BattleRule.SKILLSWAPCLAUSE] = true; }
	}

	public partial class PokemonChallengeRules : PokemonEssentials.Interface.Battle.IPokemonChallengeRules
	{
		public IPokemonRuleSet ruleset { get; protected set; }
		public IBattleType battletype { get; protected set; }
		public ILevelAdjustment levelAdjustment { get; protected set; }
		private IList<IBattleRule> battlerules;

		public PokemonChallengeRules(IPokemonRuleSet ruleset = null)
		{
			@ruleset = ruleset != null ? ruleset : new PokemonRuleSet();
			@battletype = new BattleTower();
			@levelAdjustment = null;
			@battlerules = new List<IBattleRule>();
		}

		public IPokemonChallengeRules copy()
		{
			IPokemonChallengeRules ret = new PokemonChallengeRules(@ruleset.copy());
			ret.setBattleType(@battletype);
			ret.setLevelAdjustment(@levelAdjustment);
			foreach (var rule in @battlerules)
			{
				ret.addBattleRule(rule);
			}
			return ret;
		}

		public int number
		{
			get
			{
				return this.ruleset.number;
			}
		}

		public IPokemonChallengeRules setNumber(int number)
		{
			this.ruleset.setNumber(number);
			return this;
		}

		public IPokemonChallengeRules setDoubleBattle(bool value)
		{
			if (value)
			{
				this.ruleset.setNumber(4);
				this.addBattleRule(new DoubleBattle());
			}
			else
			{
				this.ruleset.setNumber(3);
				this.addBattleRule(new SingleBattle());
			}
			return this;
		}

		public int?[][] adjustLevelsBilateral(PokemonEssentials.Interface.PokeBattle.IPokemon[] party1, PokemonEssentials.Interface.PokeBattle.IPokemon[] party2)
		{
			if (@levelAdjustment != null && @levelAdjustment.type == LevelAdjustment.BothTeams)
			{
				return @levelAdjustment.adjustLevels(party1, party2);
			}
			else
			{
				return null;
			}
		}

		public void unadjustLevelsBilateral(PokemonEssentials.Interface.PokeBattle.IPokemon[] party1, PokemonEssentials.Interface.PokeBattle.IPokemon[] party2, int?[][] adjusts)
		{
			if (@levelAdjustment != null && adjusts != null && @levelAdjustment.type == LevelAdjustment.BothTeams)
			{
				@levelAdjustment.unadjustLevels(party1, party2, adjusts);
			}
		}

		public int?[][] adjustLevels(PokemonEssentials.Interface.PokeBattle.IPokemon[] party1, PokemonEssentials.Interface.PokeBattle.IPokemon[] party2)
		{
			if (@levelAdjustment != null)
			{
				return @levelAdjustment.adjustLevels(party1, party2);
			}
			else
			{
				return null;
			}
		}

		public void unadjustLevels(PokemonEssentials.Interface.PokeBattle.IPokemon[] party1, PokemonEssentials.Interface.PokeBattle.IPokemon[] party2, int?[][] adjusts)
		{
			if (@levelAdjustment != null && adjusts != null)
			{
				@levelAdjustment.unadjustLevels(party1, party2, adjusts);
			}
		}

		public IPokemonChallengeRules addPokemonRule(IBattleRestriction rule)
		{
			this.ruleset.addPokemonRule(rule);
			return this;
		}

		public IPokemonChallengeRules addLevelRule(int minLevel, int maxLevel, int totalLevel)
		{
			this.addPokemonRule(new MinimumLevelRestriction(minLevel));
			this.addPokemonRule(new MaximumLevelRestriction(maxLevel));
			this.addSubsetRule(new TotalLevelRestriction(totalLevel));
			this.setLevelAdjustment(new TotalLevelAdjustment(minLevel, maxLevel, totalLevel));
			return this;
		}

		public IPokemonChallengeRules addSubsetRule(IBattleTeamRestriction rule)
		{
			this.ruleset.addSubsetRule(rule);
			return this;
		}

		public IPokemonChallengeRules addTeamRule(IBattleTeamRestriction rule)
		{
			this.ruleset.addTeamRule(rule);
			return this;
		}

		public IPokemonChallengeRules addBattleRule(IBattleRule rule)
		{
			@battlerules.Add(rule);
			return this;
		}

		public PokemonEssentials.Interface.PokeBattle.IBattle createBattle(IPokeBattle_Scene scene, ITrainer[] trainer1, ITrainer[] trainer2)
		{
			PokemonEssentials.Interface.PokeBattle.IBattle battle = @battletype.pbCreateBattle(scene, trainer1, trainer2);
			foreach (var p in @battlerules)
			{
				p.setRule(battle);
			}
			return battle;
		}

		public IPokemonChallengeRules setRuleset(IPokemonRuleSet rule)
		{
			@ruleset = rule;
			return this;
		}

		public IPokemonChallengeRules setBattleType(IBattleType rule)
		{
			@battletype = rule;
			return this;
		}

		public IPokemonChallengeRules setLevelAdjustment(ILevelAdjustment rule)
		{
			@levelAdjustment = rule;
			return this;
		}
	}
	#endregion

	#region Generation IV Cups
	// ##########################################
	//  Generation IV Cups
	// ##########################################
	public partial class StandardRules : PokemonRuleSet, PokemonEssentials.Interface.Battle.IStandardRules
	{
		//public int number				{ get; protected set; }

		public StandardRules(int number, int? level = null) : base(number)
		{
			//base.initialize(number);
			addPokemonRule(new StandardRestriction());
			addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
			addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
			if (level != null)
			{
				addPokemonRule(new MaximumLevelRestriction(level.Value));
			}
		}
	}

	public partial class StandardCup : StandardRules, PokemonEssentials.Interface.Battle.IStandardCup
	{
		public StandardCup() : base(3, 50)
		{
			//base.initialize(3,50);
		}

		public string name
		{
			get
			{
				return Game._INTL("STANDARD Cup");
			}
		}
	}

	public partial class DoubleCup : StandardRules, PokemonEssentials.Interface.Battle.IDoubleCup
	{
		public DoubleCup() : base(4, 50)
		{
			//base.initialize(4,50);
		}

		public string name
		{
			get
			{
				return Game._INTL("DOUBLE Cup");
			}
		}
	}

	public partial class FancyCup : PokemonRuleSet, PokemonEssentials.Interface.Battle.IFancyCup
	{
		public FancyCup() : base(3)
		{
			//base.initialize(3);
			addPokemonRule(new StandardRestriction());
			addPokemonRule(new MaximumLevelRestriction(30));
			addSubsetRule(new TotalLevelRestriction(80));
			addPokemonRule(new HeightRestriction(2));
			addPokemonRule(new WeightRestriction(20));
			addPokemonRule(new BabyRestriction());
			addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
			addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
		}

		public string name
		{
			get
			{
				return Game._INTL("FANCY Cup");
			}
		}
	}

	public partial class LittleCup : PokemonRuleSet, PokemonEssentials.Interface.Battle.ILittleCup
	{
		public LittleCup() : base(3)
		{
			//base.initialize(3);
			addPokemonRule(new StandardRestriction());
			addPokemonRule(new MaximumLevelRestriction(5));
			addPokemonRule(new BabyRestriction());
			addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
			addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
		}

		public string name
		{
			get
			{
				return Game._INTL("LITTLE Cup");
			}
		}
	}

	public partial class LightCup : PokemonRuleSet, PokemonEssentials.Interface.Battle.ILightCup
	{
		public LightCup() : base(3)
		{
			//base.initialize(3);
			addPokemonRule(new StandardRestriction());
			addPokemonRule(new MaximumLevelRestriction(50));
			addPokemonRule(new WeightRestriction(99));
			addPokemonRule(new BabyRestriction());
			addTeamRule(new SpeciesClause()); //addPokemonRule(new SpeciesClause());
			addTeamRule(new ItemClause()); //addPokemonRule(new ItemClause());
		}
		public string name
		{
			get
			{
				return Game._INTL("LIGHT Cup");
			}
		}
	}
	#endregion
}