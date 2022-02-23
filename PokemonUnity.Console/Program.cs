using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
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
			//Game.ResetAndOpenSql(@"Data\veekun-pokedex.sqlite");
			ResetSqlConnection(Game.DatabasePath);//@"Data\veekun-pokedex.sqlite"
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("######################################");
			System.Console.WriteLine("# Hello - Welcome to Console Battle! #");
			System.Console.WriteLine("######################################");

			Battle battle;
			IPokeBattle_Scene pokeBattle = new PokeBattleScene();
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

	public class PokeBattleScene : IPokeBattle_Scene
	{
		public PokemonEssentials.Interface.PokeBattle.IBattle battle;
		public bool aborted;
		public bool abortable;
		public MenuCommands[] lastcmd;
		public int[] lastmove;

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

		bool IPokeBattle_Scene.inPartyAnimation => throw new NotImplementedException();

		int IScene.Id => throw new NotImplementedException();

		void IPokeBattle_Scene.ChangePokemon()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.initialize()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.partyAnimationUpdate()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbAddPlane(int id, string filename, int viewport)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbAddSprite(string id, double x, double y, string filename, int viewport)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbAnimation(Moves moveid, PokemonEssentials.Interface.PokeBattle.IBattler user, PokemonEssentials.Interface.PokeBattle.IBattler target, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("{0} attack {1} With {2} for {3} hit times", user.Name, target.Name, moveid.ToString(), hitnum);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbAnimationCore(string animation, PokemonEssentials.Interface.PokeBattle.IBattler user, PokemonEssentials.Interface.PokeBattle.IBattler target, bool oppmove)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbBackdrop()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbBeginAttackPhase()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbBeginCommandPhase()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbChangePokemon(PokemonEssentials.Interface.PokeBattle.IBattler attacker, Forms pokemon)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbChangeSpecies(PokemonEssentials.Interface.PokeBattle.IBattler attacker, Pokemons species)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void ISceneHasChatter.pbChatter(PokemonEssentials.Interface.PokeBattle.IBattler attacker, PokemonEssentials.Interface.PokeBattle.ITrainer opponent)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbChooseEnemyCommand(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) b.pbDefaultChooseEnemyCommand(index);

			else throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbChooseMove(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, string message)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbChooseNewEnemy(int index, PokemonEssentials.Interface.PokeBattle.IPokemon[] party)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbChooseTarget(int index, Targets targettype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbCommandMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool shadowTrainer = //(hasConst(PBTypes,:SHADOW) && //Game has shadow pokemons
				@battle.opponent != null;

			System.Console.WriteLine("Enemy: {0} HP: {1}/{2}", battle.battlers[0].pbOpposing1.Name, battle.battlers[0].pbOpposing1.HP, battle.battlers[0].pbOpposing1.TotalHP);

			System.Console.WriteLine("What will\n{0} do?", battle.battlers[index].Name);
			System.Console.WriteLine("Fight - 0");
			System.Console.WriteLine("Bag - 1");
			System.Console.WriteLine("Pokémon - 2");
			System.Console.WriteLine(!shadowTrainer ? "Call - 3" : "Run - 3");

			bool appearing = true;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);
				if (fs.Key == ConsoleKey.D0)
				{
					return 0;
				}
				else if (fs.Key == ConsoleKey.D1)
				{
					return 1;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					return 2;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					if (shadowTrainer)
						return 4;
					return 3;
				}
			}
			while (appearing);

			GameDebug.LogError("Invalid Input!");

			return -1;
			//if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
			//return ret;
		}

		int IPokeBattle_Scene.pbCommandMenuEx(int index, string[] texts, int mode)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		// Not going use this because console
		void IPokeBattle_Scene.pbCommonAnimation(string animname, PokemonEssentials.Interface.PokeBattle.IBattler user, PokemonEssentials.Interface.PokeBattle.IBattler target, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//System.Console.WriteLine(animname);
			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbDamageAnimation(PokemonEssentials.Interface.PokeBattle.IBattler pkmn, float effectiveness)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("[Damage Animation] {0} for effectiveness: {1}", pkmn.Name, effectiveness);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbDisplay(string msg, bool brief)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IHasDisplayMessage.pbDisplay(string v)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		bool IPokeBattle_Scene.pbDisplayConfirmMessage(string msg)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbDisplayMessage(string msg, bool brief)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine(msg);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbDisplayPausedMessage(string msg)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine(msg);

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbDisposeSprites()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbEndBattle(BattleResults result)
		{
			System.Console.WriteLine("BattleResults: {0}", result.ToString());

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbEXPBar(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, PokemonEssentials.Interface.PokeBattle.IBattler battler, int startexp, int endexp, int tempexp1, int tempexp2)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		IEnumerator IPokeBattle_Scene.pbFainted(int pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		public string pbMoveString(IMove move, int index)
		{
			string ret = Game._INTL("{0} - Press {1}", move.id.ToString(TextScripts.Name), index);
			string typename = move.Type.ToString(TextScripts.Name);
			if (move.id > 0)
			{
				ret += Game._INTL(" ({0}) PP: {1}/{2}", typename, move.PP, move.TotalPP);
			}
			return ret;
		}

		int IPokeBattle_Scene.pbFightMenu(int index)
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
			do //;loop
			{   
				//Graphics.update();
				//Input.update();
				//pbFrameUpdate(cw);
				//if (Input.trigger(Input.t))
				//{
				//    lastmove[index] = index_;
				//    //cw.dispose();
				//    return -1;
				//}
				//if (Input.trigger(Input.t))
				//{
				//    @lastmove[index] = index_;
				//    return index_;
				//}
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.T)
				{
					lastmove[index] = index_;
					appearing = false;
					return -1;
				}
				else if (fs.Key == ConsoleKey.D1)
				{
					lastmove[index] = index_;
					appearing = false;
					return 0;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					lastmove[index] = index_;
					appearing = false;
					return 1;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					lastmove[index] = index_;
					appearing = false;
					return 2;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					lastmove[index] = index_;
					appearing = false;
					return 3;
				}
			} while (appearing);

			return -1;
		}

		void IPokeBattle_Scene.pbFindAnimation(Moves moveid, int userIndex, int hitnum)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbFirstTarget(int index, Targets targettype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbForgetMove(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, Moves moveToLearn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbFrameUpdate(object cw)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbGraphicsUpdate()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbHideCaptureBall()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbHideHelp()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		IEnumerator IPokeBattle_Scene.pbHideOpponent()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		public IEnumerator pbHPChanged(PokemonEssentials.Interface.PokeBattle.IBattler pkmn, int oldhp, bool animate)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			pkmn.HP -= oldhp;

			System.Console.WriteLine("[HP Changed] {0}: oldhp: {1} and animate: {2}", pkmn.Name, oldhp, animate.ToString());
			System.Console.WriteLine("[HP Changed] {0}: CurrentHP: {1}", pkmn.Name, pkmn.HP);

			//throw new NotImplementedException();
			yield return null;
		}

		void IPokeBattle_Scene.pbInputUpdate()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		KeyValuePair<Items, int> IPokeBattle_Scene.pbItemMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("Need to implment item system in textbased-line");

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbLevelUp(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, PokemonEssentials.Interface.PokeBattle.IBattler battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		string IPokeBattle_Scene.pbMoveString(string move)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		string IPokeBattle_Scene.pbNameEntry(string helptext, PokemonEssentials.Interface.PokeBattle.IPokemon pokemon)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbRecall(int battlerindex)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IScene.pbRefresh()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbResetCommandIndices()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbResetMoveIndex(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			lastmove[index] = 0;

			//throw new NotImplementedException();
		}

		int IPokeBattle_Scene.pbSafariCommandMenu(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbSafariStart()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbSaveShadows()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbSelectBattler(int index, int selectmode)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbSendOut(int battlerindex, PokemonEssentials.Interface.PokeBattle.IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			System.Console.WriteLine("Trainer: {0}. Pokemon: {1}", battlerindex, pkmn.ToString());

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbSetMessageMode(bool mode)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbShowCommands(string msg, string[] commands, bool canCancel)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbShowHelp(string text)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		IEnumerator IPokeBattle_Scene.pbShowOpponent(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbShowPokedex(Pokemons species, int form)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbShowWindow(int windowtype)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbStartBattle(PokemonEssentials.Interface.PokeBattle.IBattle battle)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			this.battle = battle;

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
				//bool appearing = true;
				//do
				//{
				//    ConsoleKeyInfo fs = System.Console.ReadKey(true);
				//    if (fs.Key == ConsoleKey.T && abortable && !aborted)
				//    {
				//        aborted = true;
				//        battle.pbAbort();
				//        appearing = false;
				//    }
				//}
				//while (appearing);
			}
		}

		int IPokeBattle_Scene.pbSwitch(int index, bool lax, bool cancancel)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbThrowAndDeflect(Items ball, int targetBattler)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbThrowBait()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbThrowRock()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbThrowSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbTrainerBattleSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbTrainerSendOut(int battlerindex, PokemonEssentials.Interface.PokeBattle.IPokemon pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle.opponent != null)
				System.Console.WriteLine("Trainer: {0}. Pokemon: {1}", battle.opponent[0].name, pkmn.ToString());

			//throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbTrainerWithdraw(PokemonEssentials.Interface.PokeBattle.IBattle battle, PokemonEssentials.Interface.PokeBattle.IBattler pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbUpdate()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbUpdateSelected(int index)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbWaitMessage()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbWildBattleSuccess()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_Scene.pbWithdraw(PokemonEssentials.Interface.PokeBattle.IBattle battle, PokemonEssentials.Interface.PokeBattle.IBattler pkmn)
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}
	}
}