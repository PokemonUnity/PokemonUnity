using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonUnity.ConsoleApp
{
	class Program
	{
		public static void ResetSqlConnection(string db) 
		{
			Game.con = (System.Data.IDbConnection)new System.Data.SQLite.SQLiteConnection(db); 
			Game.ResetSqlConnection(db); 
		}

		static void Main(string[] args)
		{
			GameDebug.OnLog += GameDebug_OnLog;
			string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
			//System.Console.WriteLine(System.IO.Directory.GetParent(englishLocalization).FullName);
			Game.LocalizationDictionary = new XmlStringRes(null); //new Debugger());
			Game.LocalizationDictionary.Initialize(englishLocalization, (int)Languages.English);

			//Game.ResetAndOpenSql(@"Data\veekun-pokedex.sqlite");
			ResetSqlConnection(Game.DatabasePath);//@"Data\veekun-pokedex.sqlite"
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("######################################");
			System.Console.WriteLine("# Hello - Welcome to Console Battle! #");
			System.Console.WriteLine("######################################");

			Battle battle;
			IPokeBattle_DebugSceneNoGraphics pokeBattle = new PokeBattleScene();
			//pokeBattle.initialize();


			IPokemon[] p1 = new IPokemon[2] { new PokemonUnity.Monster.Pokemon(Pokemons.ABRA), new PokemonUnity.Monster.Pokemon(Pokemons.EEVEE) };
			IPokemon[] p2 = new IPokemon[2] { new PokemonUnity.Monster.Pokemon(Pokemons.MONFERNO), new PokemonUnity.Monster.Pokemon(Pokemons.SEEDOT) };

			p1[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
			p1[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

			p2[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
			p2[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

			//PokemonUnity.Character.TrainerData trainerData = new PokemonUnity.Character.TrainerData("FlakTester", true, 120, 002);
			//Game.GameData.Player = new PokemonUnity.Character.Player(trainerData, p1);
			//Game.GameData.Trainer = new Trainer("FlakTester", true, 120, 002);

			(p1[0] as PokemonUnity.Monster.Pokemon).SetNickname("Test1");
			(p1[1] as PokemonUnity.Monster.Pokemon).SetNickname("Test2");

			(p2[0] as PokemonUnity.Monster.Pokemon).SetNickname("OppTest1");
			(p2[1] as PokemonUnity.Monster.Pokemon).SetNickname("OppTest2");

			//ITrainer player = new Trainer(Game.GameData.Trainer.name, TrainerTypes.PLAYER);
			ITrainer player = new Trainer("FlakTester",TrainerTypes.CHAMPION);
			//ITrainer pokemon = new Trainer("Wild Pokemon", TrainerTypes.WildPokemon);
			Game.GameData.Trainer = player;

			//battle = new Battle(pokeBattle, Game.GameData.Trainer.party, p2, Game.GameData.Trainer, null, 2);
			battle = new Battle(pokeBattle, p1, p2, Game.GameData.Trainer, null);

			battle.rules.Add(BattleRule.SUDDENDEATH, false);
			battle.rules.Add("drawclause", false);
			battle.rules.Add(BattleRule.MODIFIEDSELFDESTRUCTCLAUSE, false);

			battle.weather = Weather.SUNNYDAY;

			battle.pbStartBattle(true);
		}

		private static void GameDebug_OnLog(object sender, OnDebugEventArgs e)
		{
			if (e != null || e != System.EventArgs.Empty)
				if (e.Error == true)
					System.Console.WriteLine("[ERR]: " + e.Message);
				else if (e.Error == false)
					System.Console.WriteLine("[WARN]: " + e.Message);
				else
					System.Console.WriteLine("[LOG]: " + e.Message);
		}
	}

	public class PokeBattleScene : IPokeBattle_DebugSceneNoGraphics, IPokeBattle_SceneNonInteractive //IPokeBattle_Scene,
	{
		private PokemonEssentials.Interface.PokeBattle.IBattle battle;
		private bool aborted;
		private bool abortable;
		private MenuCommands[] lastcmd;
		private int[] lastmove;
		private int messageCount = 0;

		public int Id { get { return 0; } }

		public PokeBattleScene()
		{
			initialize();
		}

		public void initialize()
		{
			battle = null;
			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			lastmove = new int[] { 0, 0, 0, 0 };
			//@pkmnwindows = new GameObject[] { null, null, null, null };
			//@sprites = new Dictionary<string, GameObject>();
			//@battlestart = true;
			//@messagemode = false;
			abortable = true;
			aborted = false;
		}

		public void pbDisplay(string v)
		//void IHasDisplayMessage.pbDisplay(string v)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//GameDebug.Log(v);
			System.Console.WriteLine(v);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbDisplayMessage(string msg, bool brief)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbDisplay(msg);
			@messageCount += 1;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbDisplayPausedMessage(string msg)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbDisplay(msg);
			@messageCount += 1;
		}

		bool IPokeBattle_DebugSceneNoGraphics.pbDisplayConfirmMessage(string msg)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pbDisplay(msg);
			@messageCount += 1;

			System.Console.WriteLine("Y/N?");
			bool appearing = true;
			bool result = false;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.Y)
				{
					appearing = false;
					result = true;
				}
				else if (fs.Key == ConsoleKey.N)
				{
					appearing = false;
					result = false;
				}
			} while (appearing);
			return result;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbShowCommands(string msg, string[] commands, bool defaultValue)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			GameDebug.Log(msg);
			@messageCount += 1;
			return 0;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBeginCommandPhase()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (@messageCount > 0)
			{
				GameDebug.Log($"[message count: #{@messageCount}]");
			}
			@messageCount = 0;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbStartBattle(PokemonEssentials.Interface.PokeBattle.IBattle battle)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			this.battle = battle;
			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			lastmove = new int[] { 0, 0, 0, 0 };
			@messageCount = 0;

			if (battle.player?.Length == 1)
			{
				GameDebug.Log("One player battle!");
			}

			if (battle.opponent != null)
			{
				GameDebug.Log("Opponent found!");
				if (battle.opponent.Length == 1)
				{
					GameDebug.Log("One opponent battle!");
				}
				if (battle.opponent.Length > 1)
				{
					GameDebug.Log("Multiple opponents battle!");
				}
				else
					GameDebug.Log("Wild Pokemon battle!");
			}

			if (battle.player?.Length > 0 && battle.opponent?.Length > 0 && !battle.doublebattle)
			{
				GameDebug.Log("Single Battle");
				System.Console.WriteLine("Player: {0} has {1} in their party", battle.player[0].name, battle.party1.Length);
				System.Console.WriteLine("Opponent: {0} has {1} in their party", battle.opponent?[0].name, battle.party2.Length);
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.pbEndBattle(BattleResults result)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbTrainerSendOut(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbSendOut(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbTrainerWithdraw(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbWithdraw(IBattle battle, IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		int IPokeBattle_DebugSceneNoGraphics.pbForgetMove(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, Moves moveToLearn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IMove[] moves = pokemon.moves;
			string[] commands = new string[4] {
			   pbMoveString(moves[0], 1),
			   pbMoveString(moves[1], 2),
			   pbMoveString(moves[2], 3),
			   pbMoveString(moves[3], 4) };
			for (int i = 0; i < commands.Length; i++)
			{
				System.Console.WriteLine(commands[i]);
			}
			System.Console.WriteLine("Press 0 to Cancel");
			bool appearing = true;
			do 
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D0)
				{
					appearing = false;
					return -1;
				}
				else if (fs.Key == ConsoleKey.D1)
				{
					appearing = false;
					return 0;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					appearing = false;
					return 1;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					appearing = false;
					return 2;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					appearing = false;
					return 3;
				}
			} while (appearing);

			return -1;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBeginAttackPhase()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		}

		int IPokeBattle_DebugSceneNoGraphics.pbCommandMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool shadowTrainer = //(hasConst(PBTypes,:SHADOW) && //Game has shadow pokemons
				//@battle.opponent != null;
				battle.battlers[index] is IPokemonShadowPokemon p && p.hypermode;

			System.Console.WriteLine("Enemy: {0} HP: {1}/{2}", battle.battlers[index].pbOpposing1.Name, battle.battlers[index].pbOpposing1.HP, battle.battlers[index].pbOpposing1.TotalHP);
			if (battle.battlers[index].pbOpposing2.IsNotNullOrNone()) 
				System.Console.WriteLine("Enemy: {0} HP: {1}/{2}", battle.battlers[index].pbOpposing2.Name, battle.battlers[index].pbOpposing2.HP, battle.battlers[index].pbOpposing2.TotalHP);

			System.Console.WriteLine("What will {0} do?", battle.battlers[index].Name);
			System.Console.WriteLine("Fight - 0");
			System.Console.WriteLine("Bag - 1");
			System.Console.WriteLine("Pokémon - 2");
			System.Console.WriteLine(shadowTrainer ? "Call - 3" : "Run - 3");

			bool appearing = true;
			int result = -1;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);
				if (fs.Key == ConsoleKey.D0)
				{
					result = 0;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D1)
				{
					result = 1;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					result = 2;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					if (shadowTrainer)
						result = 4;
					else
						result = 3;
					appearing = false;
				}
			}
			while (appearing);

			//GameDebug.LogError("Invalid Input!");

			return result;
			//if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
			//return ret;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbFightMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IBattleMove[] moves = @battle.battlers[index].moves;
			string[] commands = new string[4] {
			   pbMoveString(moves[0].thismove, 1),
			   pbMoveString(moves[1].thismove, 2),
			   pbMoveString(moves[2].thismove, 3),
			   pbMoveString(moves[3].thismove, 4) };
			int index_ = @lastmove[index];
			for (int i = 0; i < commands.Length; i++)
			{
				System.Console.WriteLine(commands[i]);
			}
			bool appearing = true;
			int result = 0;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D1)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 0;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 1;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 2;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 3;
				}
			} while (appearing && battle.battlers[index].moves[result].id == Moves.NONE);

			return result;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbItemMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//System.Console.WriteLine("Need to implment item system in textbased-line");
			return -1;
		}

		int IPokeBattle_DebugSceneNoGraphics.pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			List<int> targets = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				//if (@battle.battlers[index].pbIsOpposing(i) &&
				//   !@battle.battlers[i].isFainted()) targets.Add(i);
				if (!@battle.battlers[i].isFainted())
					if ((targettype == Targets.ALL_OPPONENTS
						//|| targettype == Targets.OPPONENTS_FIELD
						|| targettype == Targets.RANDOM_OPPONENT
						|| targettype == Targets.SELECTED_POKEMON
						|| targettype == Targets.SELECTED_POKEMON_ME_FIRST) &&
						@battle.battlers[index].pbIsOpposing(i))
						targets.Add(i);
					else if ((targettype == Targets.ALLY
						//|| targettype == Targets.USERS_FIELD
						//|| targettype == Targets.USER_AND_ALLIES
						|| targettype == Targets.USER_OR_ALLY) &&
						!@battle.battlers[index].pbIsOpposing(i))
						targets.Add(i);
			}
			//Doesnt include multiple targets...
			if (targets.Count == 0) return -1;
			return targets[Core.Rand.Next(targets.Count)];
		}

		public void pbRefresh()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//int IPokeBattle_DebugSceneNoGraphics.pbSwitch(int index, bool lax, bool cancancel)
		int IPokeBattle_SceneNonInteractive.pbSwitch(int index, bool lax, bool cancancel)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			for (int i = 0; i < @battle.pbParty(index).Length - 1; i++)
			{
				if (lax)
				{
					if (@battle.pbCanSwitchLax(index, i, false)) return i;
				}
				else
				{
					if (@battle.pbCanSwitch(index, i, false)) return i;
				}
			}
			return -1;
		}

		//public IEnumerator pbHPChanged(PokemonEssentials.Interface.PokeBattle.IBattler pkmn, int oldhp, bool animate)
		void IPokeBattle_DebugSceneNoGraphics.pbHPChanged(IBattler pkmn, int oldhp, bool anim)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			int hpchange = pkmn.HP - oldhp;
			if (hpchange < 0)
			{
				hpchange = -hpchange;
				GameDebug.Log($"[HP change] #{pkmn.ToString()} lost #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
			}
			else
			{
				GameDebug.Log($"[HP change] #{pkmn.ToString()} gained #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
			}
			pbRefresh();

			//System.Console.WriteLine("[HP Changed] {0}: oldhp: {1} and animate: {2}", pkmn.Name, oldhp, animate.ToString());
			//System.Console.WriteLine("[HP Changed] {0}: CurrentHP: {1}", pkmn.Name, pkmn.HP);

			//yield return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbFainted(IBattler pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.pbChooseEnemyCommand(int index)
		void IPokeBattle_SceneNonInteractive.pbChooseEnemyCommand(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) b.pbDefaultChooseEnemyCommand(index);
		}

		//void IPokeBattle_DebugSceneNoGraphics.pbChooseNewEnemy(int index, IPokemon[] party)
		int IPokeBattle_SceneNonInteractive.pbChooseNewEnemy(int index, IPokemon[] party)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) return b.pbDefaultChooseNewEnemy(index, party);
			return -1;
		}

		void IPokeBattle_DebugSceneNoGraphics.pbWildBattleSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbTrainerBattleSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbEXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbLevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		int IPokeBattle_DebugSceneNoGraphics.pbBlitz(int keys)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			return battle.pbRandom(30);
		}

		void ISceneHasChatter.pbChatter(PokemonEssentials.Interface.PokeBattle.IBattler attacker, PokemonEssentials.Interface.PokeBattle.IBattler opponent)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.pbChatter(IBattler attacker, IBattler opponent)
		//{
		//	GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	(this as ISceneHasChatter).pbChatter(attacker, opponent);
		//}

		void IPokeBattle_DebugSceneNoGraphics.pbShowOpponent(int opp)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbHideOpponent()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbRecall(int battlerindex)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbDamageAnimation(IBattler pkmn, TypeEffective effectiveness)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//GameDebug.Log($"[Judgment] #{b1.ToString()}:#{r1.Inspect()}, #{b2.ToString()}:#{r2.Inspect()}");
			GameDebug.Log($"[Judgment] #{b1.ToString()}:#[{r1.JoinAsString(", ")}], #{b2.ToString()}:#[{r2.JoinAsString(", ")}]");
		}

		void IPokeBattle_DebugSceneNoGraphics.pbBattleArenaBattlers(IBattle b1, IBattle b2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			GameDebug.Log($"[#{b1.ToString()} VS #{b2.ToString()}]");
		}

		void IPokeBattle_DebugSceneNoGraphics.pbCommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (attacker.IsNotNullOrNone())
			{
				if (opponent.IsNotNullOrNone())
				{
					GameDebug.Log($"[pbCommonAnimation] #{moveid}, #{attacker.ToString()}, #{opponent.ToString()}");
				}
				else
				{
					GameDebug.Log($"[pbCommonAnimation] #{moveid}, #{attacker.ToString()}");
				}
			}
			else
			{
				GameDebug.Log($"[pbCommonAnimation] #{moveid}");
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.pbAnimation(Moves moveid, PokemonEssentials.Interface.PokeBattle.IBattler user, PokemonEssentials.Interface.PokeBattle.IBattler target, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("{0} attack {1} With {2} for {3} hit times", user.Name, target.Name, moveid.ToString(), hitnum);

			if (user.IsNotNullOrNone())
			{
				if (target.IsNotNullOrNone())
				{
					GameDebug.Log($"[pbAnimation] #{user.ToString()}, #{target.ToString()}");
				}
				else
				{
					GameDebug.Log($"[pbAnimation] #{user.ToString()}");
				}
			}
			else
			{
				GameDebug.Log($"[pbAnimation]");
			}
		}

		#region Non Interactive Battle Scene
		int IPokeBattle_SceneNonInteractive.pbCommandMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (battle.pbRandom(15) == 0) return 1;
			//return 0;
			return (this as IPokeBattle_DebugSceneNoGraphics).pbCommandMenu(index);
		}

		int IPokeBattle_SceneNonInteractive.pbFightMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//IBattler battler = @battle.battlers[index];
			//int i = 0;
			//do {
			//	i = Core.Rand.Next(4);
			//} while (battler.moves[i].id==0);
			//GameDebug.Log($"i=#{i}, pp=#{battler.moves[i].PP}");
			////PBDebug.flush;
			//return i;
			return (this as IPokeBattle_DebugSceneNoGraphics).pbFightMenu(index);
		}

		int IPokeBattle_SceneNonInteractive.pbItemMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//return -1;
			return (this as IPokeBattle_DebugSceneNoGraphics).pbItemMenu(index);
		}

		int IPokeBattle_SceneNonInteractive.pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//List<int> targets = new List<int>();
			//for (int i = 0; i < 4; i++)
			//{
			//	if (@battle.battlers[index].pbIsOpposing(i) &&
			//	   !@battle.battlers[i].isFainted())
			//	{
			//		targets.Add(i);
			//	}
			//}
			//if (targets.Count == 0) return -1;
			//return targets[Core.Rand.Next(targets.Count)];
			return (this as IPokeBattle_DebugSceneNoGraphics).pbChooseTarget(index, targettype);
		}

		/*int IPokeBattle_SceneNonInteractive.pbSwitch(int index, bool lax, bool cancancel)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_SceneNonInteractive.pbChooseEnemyCommand(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_SceneNonInteractive.pbChooseNewEnemy(int index, IPokemon[] party)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}*/
		#endregion

		private string pbMoveString(IMove move, int index)
		{
			string ret = string.Format("{0} - Press {1}", Game._INTL(move.id.ToString(TextScripts.Name)), index);
			string typename = Game._INTL(move.Type.ToString(TextScripts.Name));
			if (move.id > 0)
			{
				ret += string.Format(" ({0}) PP: {1}/{2}", typename, move.PP, move.TotalPP);
			}
			return ret;
		}
	}
}