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
			new PokemonUnity.ConsoleApp.Debugger().Init("..\\..\\..\\Logs", "ConsoleBattle");
			//PokemonUnity.ConsoleApp.Debugger.Instance.OnLog += GameDebug_OnLog;
			Core.Logger.LogDebug(message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			Core.Logger.Log("######################################");
			Core.Logger.Log("# Hello - Welcome to Console Battle! #");
			Core.Logger.Log("######################################");

			string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
			//Core.Logger.Log(System.IO.Directory.GetParent(englishLocalization).FullName);
			Game.LocalizationDictionary = new XmlStringRes(null); //new Debugger());
			Game.LocalizationDictionary.Initialize(englishLocalization, (int)Languages.English);

			//Game.ResetAndOpenSql(@"Data\veekun-pokedex.sqlite");
			ResetSqlConnection(Game.DatabasePath);//@"Data\veekun-pokedex.sqlite"

			//IPokeBattle_DebugSceneNoGraphics pokeBattle = new PokeBattleScene();
			//pokeBattle.initialize();


			IPokemon[] p1 = new IPokemon[] { new PokemonUnity.Monster.Pokemon(Pokemons.ABRA), new PokemonUnity.Monster.Pokemon(Pokemons.EEVEE) };
			IPokemon[] p2 = new IPokemon[] { new PokemonUnity.Monster.Pokemon(Pokemons.MONFERNO) }; //, new PokemonUnity.Monster.Pokemon(Pokemons.SEEDOT) };

			//p1[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
			//p1[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

			//p2[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
			//p2[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

			//PokemonUnity.Character.TrainerData trainerData = new PokemonUnity.Character.TrainerData("FlakTester", true, 120, 002);
			//Game.GameData.Player = new PokemonUnity.Character.Player(trainerData, p1);
			//Game.GameData.Trainer = new Trainer("FlakTester", true, 120, 002);

			//(p1[0] as PokemonUnity.Monster.Pokemon).SetNickname("Test1");
			//(p1[1] as PokemonUnity.Monster.Pokemon).SetNickname("Test2");

			//(p2[0] as PokemonUnity.Monster.Pokemon).SetNickname("OppTest1");
			//(p2[1] as PokemonUnity.Monster.Pokemon).SetNickname("OppTest2");

			//ITrainer player = new Trainer(Game.GameData.Trainer.name, TrainerTypes.PLAYER);
			ITrainer player = new Trainer("FlakTester",TrainerTypes.CHAMPION);
			//ITrainer pokemon = new Trainer("Wild Pokemon", TrainerTypes.WildPokemon);
			Game.GameData.Trainer = player;
			Game.GameData.Trainer.party = p1;

			//IBattle battle = new Battle(pokeBattle, Game.GameData.Trainer.party, p2, Game.GameData.Trainer, null, 2);
			IBattle battle = new Battle(new PokeBattleScene(), p1, p2, Game.GameData.Trainer, null);

			battle.rules.Add(BattleRule.SUDDENDEATH, false);
			battle.rules.Add("drawclause", false);
			battle.rules.Add(BattleRule.MODIFIEDSELFDESTRUCTCLAUSE, false);

			battle.weather = Weather.SUNNYDAY;

			battle.StartBattle(true);
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

		public IPokeBattle_DebugSceneNoGraphics initialize()
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

			return this;
		}

		public void Display(string v)
		//void IHasDisplayMessage.Display(string v)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//Core.Logger.Log(v);
			Core.Logger.Log(v);
		}

		void IPokeBattle_DebugSceneNoGraphics.DisplayMessage(string msg, bool brief)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Display(msg);
			@messageCount += 1;
		}

		void IPokeBattle_DebugSceneNoGraphics.DisplayPausedMessage(string msg)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Display(msg);
			@messageCount += 1;
		}

		bool IPokeBattle_DebugSceneNoGraphics.DisplayConfirmMessage(string msg)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Display(msg);
			@messageCount += 1;

			Core.Logger.Log("Y/N?");
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

		bool IHasDisplayMessage.DisplayConfirm(string v)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			return (this as IPokeBattle_DebugSceneNoGraphics).DisplayConfirmMessage(v);
		}

		bool IPokeBattle_DebugSceneNoGraphics.ShowCommands(string msg, string[] commands, bool defaultValue)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log(msg);
			@messageCount += 1;
			return false;
		}

		int IPokeBattle_DebugSceneNoGraphics.ShowCommands(string msg, string[] commands, int defaultValue)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log(msg);
			@messageCount += 1;
			return 0;
		}

		void IPokeBattle_DebugSceneNoGraphics.BeginCommandPhase()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (@messageCount > 0)
			{
				Core.Logger.Log("[message count: #{0}]", @messageCount);
			}
			@messageCount = 0;
		}

		void IPokeBattle_DebugSceneNoGraphics.StartBattle(PokemonEssentials.Interface.PokeBattle.IBattle battle)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			this.battle = battle;
			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			lastmove = new int[] { 0, 0, 0, 0 };
			@messageCount = 0;

			if (battle.player?.Length == 1)
			{
				Core.Logger.Log("One player battle!");
			}

			if (battle.opponent != null)
			{
				Core.Logger.Log("Opponent found!");
				if (battle.opponent.Length == 1)
				{
					Core.Logger.Log("One opponent battle!");
				}
				if (battle.opponent.Length > 1)
				{
					Core.Logger.Log("Multiple opponents battle!");
				}
				else
					Core.Logger.Log("Wild Pokemon battle!");
			}

			if (battle.player?.Length > 0 && battle.opponent?.Length > 0 && !battle.doublebattle)
			{
				Core.Logger.Log("Single Battle");
				Core.Logger.Log("Player: {0} has {1} in their party", battle.player[0].name, battle.party1.Length);
				Core.Logger.Log("Opponent: {0} has {1} in their party", battle.opponent?[0].name, battle.party2.Length);
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.EndBattle(BattleResults result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.TrainerSendOut(IBattle battle, IPokemon pkmn)
		//{
		//	Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//}

		void IPokeBattle_DebugSceneNoGraphics.TrainerWithdraw(IBattle battle, IBattler pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.Withdraw(IBattle battle, IBattler pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		int IPokeBattle_DebugSceneNoGraphics.ForgetMove(PokemonEssentials.Interface.PokeBattle.IPokemon pokemon, Moves moveToLearn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IMove[] moves = pokemon.moves;
			string[] commands = new string[4] {
			   MoveString(moves[0], 1),
			   MoveString(moves[1], 2),
			   MoveString(moves[2], 3),
			   MoveString(moves[3], 4) };
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

		void IPokeBattle_DebugSceneNoGraphics.BeginAttackPhase()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		}

		int IPokeBattle_DebugSceneNoGraphics.CommandMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool shadowTrainer = //(hasConst(Types,:SHADOW) && //Game has shadow pokemons
				(@battle.opponent != null && battle.opponent.Length>0) && //Opponent is a trainer
				battle.battlers[index] is IPokemonShadowPokemon p && p.hypermode;

			Core.Logger.Log("Enemy: {0} HP: {1}/{2}", battle.battlers[index].Opposing1.Name, battle.battlers[index].Opposing1.HP, battle.battlers[index].Opposing1.TotalHP);
			if (battle.battlers[index].Opposing2.IsNotNullOrNone())
				Core.Logger.Log("Enemy: {0} HP: {1}/{2}", battle.battlers[index].Opposing2.Name, battle.battlers[index].Opposing2.HP, battle.battlers[index].Opposing2.TotalHP);
			Core.Logger.Log("Player: {0} HP: {1}/{2}", battle.battlers[index].Name, battle.battlers[index].HP, battle.battlers[index].TotalHP);
			if (battle.battlers[index].Partner.IsNotNullOrNone())
				Core.Logger.Log("Player: {0} HP: {1}/{2}", battle.battlers[index].Partner.Name, battle.battlers[index].Partner.HP, battle.battlers[index].Partner.TotalHP);

			System.Console.WriteLine("Fight - 1");
			System.Console.WriteLine("Bag - 2");
			System.Console.WriteLine("Pokémon - 3");
			System.Console.WriteLine(shadowTrainer ? "Call - 4" : "Run - 4");
			Core.Logger.Log("What will {0} do?", battle.battlers[index].Name);

			bool appearing = true;
			int result = -1;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);
				if (fs.Key == ConsoleKey.D1)
				{
					result = 0;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					result = 1;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					result = 2;
					appearing = false;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					if (shadowTrainer)
						result = 4;
					else
						result = 3;
					appearing = false;
				}
			}
			while (appearing);

			//Core.Logger.LogError("Invalid Input!");

			if (result == 3 && shadowTrainer) result = 4; // Convert "Run" to "Call"
			return result;
		}

		int IPokeBattle_DebugSceneNoGraphics.FightMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IBattleMove[] moves = @battle.battlers[index].moves;
			string[] commands = new string[4] {
			   MoveString(moves[0].thismove, 1),
			   MoveString(moves[1].thismove, 2),
			   MoveString(moves[2].thismove, 3),
			   MoveString(moves[3].thismove, 4) };
			int index_ = @lastmove[index];
			for (int i = 0; i < commands.Length; i++)
			{
				System.Console.WriteLine(commands[i]);
			}
			System.Console.WriteLine("Press Q to return back to Command Menu");
			bool appearing = true;
			int result = -2;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D1)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 0;
					//Core.Logger.Log($"int=#{result}, pp=#{moves[result].PP}");
					Core.Logger.Log("int=#{0}, pp=#{1}", result, moves[result].PP);
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 1;
					//Core.Logger.Log($"int=#{result}, pp=#{moves[result].PP}");
					Core.Logger.Log("int=#{0}, pp=#{1}", result, moves[result].PP);
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 2;
					//Core.Logger.Log($"int=#{result}, pp=#{moves[result].PP}");
					Core.Logger.Log("int=#{0}, pp=#{1}", result, moves[result].PP);
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					lastmove[index] = index_;
					appearing = false;
					result = 3;
					//Core.Logger.Log($"int=#{result}, pp=#{moves[result].PP}");
					Core.Logger.Log("int=#{0}, pp=#{1}", result, moves[result].PP);
				}
				else if (fs.Key == ConsoleKey.Q)
				{
					appearing = false;
					result = -1; //CANCEL FIGHT MENU
				}
			} while (appearing && (result == -2 || battle.battlers[index].moves[result].id == Moves.NONE));

			return result;
		}

		Items IPokeBattle_DebugSceneNoGraphics.ItemMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//Core.Logger.Log("Need to implement item system in textbased-line");
			return Items.NONE;
		}

		int IPokeBattle_DebugSceneNoGraphics.ChooseTarget(int index, PokemonUnity.Attack.Targets targettype)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//Doesnt include multiple targets at once...
			List<int> targets = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				//if (@battle.battlers[index].IsOpposing(i) &&
				//   !@battle.battlers[i].isFainted()) targets.Add(i);
				if (!@battle.battlers[i].isFainted())
					if ((targettype == PokemonUnity.Attack.Targets.RANDOM_OPPONENT
						//|| targettype == PokemonUnity.Attack.Targets.ALL_OPPONENTS
						//|| targettype == PokemonUnity.Attack.Targets.OPPONENTS_FIELD
						|| targettype == PokemonUnity.Attack.Targets.SELECTED_POKEMON
						|| targettype == PokemonUnity.Attack.Targets.SELECTED_POKEMON_ME_FIRST) &&
						@battle.battlers[index].IsOpposing(i))
						targets.Add(i);
					else if ((targettype == PokemonUnity.Attack.Targets.ALLY
						//|| targettype == PokemonUnity.Attack.Targets.USERS_FIELD
						//|| targettype == PokemonUnity.Attack.Targets.USER_AND_ALLIES
						|| targettype == PokemonUnity.Attack.Targets.USER_OR_ALLY) &&
						!@battle.battlers[index].IsOpposing(i))
						targets.Add(i);
			}
			if (targets.Count == 0) return -1;
			//return targets[Core.Rand.Next(targets.Count)];

			for (int i = 0; i < targets.Count; i++)
			{
				Core.Logger.Log("Target {0}: {1} HP: {2}/{3} => {4}", targets[i] % 2 == 1 ? "Enemy" : "Ally", battle.battlers[targets[i]].Name, battle.battlers[targets[i]].HP, battle.battlers[targets[i]].TotalHP, i);
			}
			bool appearing = true;
			int result = 0;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);

				if (fs.Key == ConsoleKey.D1)
				{
					appearing = false;
					result = 0;
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					appearing = false;
					result = 1;
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					appearing = false;
					result = 2;
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					appearing = false;
					result = 3;
				}
			} while (appearing && targets.Contains(result));

			return result;
		}

		public void Refresh()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//int IPokeBattle_DebugSceneNoGraphics.Switch(int index, bool lax, bool cancancel)
		int IPokeBattle_SceneNonInteractive.Switch(int index, bool lax, bool cancancel)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			IPokemon[] party = @battle.Party(index);
			IList<string> commands = new List<string>();
			bool[] inactives = new bool[] { true, true, true, true, true, true };
			IList<int> partypos = new List<int>();
			//int activecmd = 0; //if cursor is on first or second pokemon when viewing ui
			int numactive = battle.doublebattle ? 2 : 1;
			IBattler battler = @battle.battlers[0];
			//commands[commands.Count] = PokemonString(party[battler.pokemonIndex]);
			commands.Add(PokemonString(party[battler.pokemonIndex]));
			//if (battler.Index == index) activecmd = 0;
			inactives[battler.pokemonIndex] = false;
			//partypos[partypos.Count] = battler.pokemonIndex;
			partypos.Add(battler.pokemonIndex);
			if (@battle.doublebattle)
			{
				battler = @battle.battlers[2];
				//commands[commands.Count] = PokemonString(party[battler.pokemonIndex]);
				commands.Add(PokemonString(party[battler.pokemonIndex]));
				//if (battler.Index == index) activecmd = 1;
				inactives[battler.pokemonIndex] = false;
				//partypos[partypos.Count] = battler.pokemonIndex;
				partypos.Add(battler.pokemonIndex);
			}
			for (int i = 0; i < party.Length; i++)
			{
				if (inactives[i])
				{
					//commands[commands.Count] = PokemonString(party[i]);
					commands.Add(PokemonString(party[i]));
					//Core.Logger.Log(PokemonString(party[i]));
					//partypos[partypos.Count] = i;
					partypos.Add(i);
				}
			}
			for (int i = 0; i < commands.Count; i++)
			{
				System.Console.WriteLine("Press {0} => {1}",i+1,commands[i]);
			}
			System.Console.WriteLine("Press Q to return back to Command Menu");
			bool appearing = true;
			int ret = -2;
			do
			{
				ConsoleKeyInfo fs = System.Console.ReadKey(true);
				bool canswitch = false; int pkmnindex = -1;
				if (fs.Key == ConsoleKey.D1)
				{
					pkmnindex = partypos[0];
					canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
					   @battle.CanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D2)
				{
					pkmnindex = partypos[1];
					canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
					   @battle.CanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D3)
				{
					pkmnindex = partypos[2];
					canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
					   @battle.CanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D4)
				{
					pkmnindex = partypos[3];
					canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
					   @battle.CanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D5)
				{
					pkmnindex = partypos[4];
					canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
					   @battle.CanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.D6)
				{
					pkmnindex = partypos[5];
					canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
					   @battle.CanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						appearing = false;
						//break;
					}
				}
				else if (fs.Key == ConsoleKey.Q && cancancel)
				{
					appearing = false;
					ret = -1; //CANCEL POKEMON MENU
				}
			} while (appearing && (ret == -2 || ret == -2 || inactives[ret]));//!battle.Party(index)[ret].IsNotNullOrNone()

			return ret;
		}

		//public IEnumerator HPChanged(PokemonEssentials.Interface.PokeBattle.IBattler pkmn, int oldhp, bool animate)
		void IPokeBattle_DebugSceneNoGraphics.HPChanged(IBattler pkmn, int oldhp, bool anim)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			int hpchange = pkmn.HP - oldhp;
			if (hpchange < 0)
			{
				hpchange = -hpchange;
				//Core.Logger.Log($"[HP change] #{pkmn.Name} lost #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
				Core.Logger.Log("[HP change] #{0} lost #{1} HP (#{2}=>#{3})", pkmn.Name, hpchange, oldhp, pkmn.HP);
			}
			else
			{
				//Core.Logger.Log($"[HP change] #{pkmn.Name} gained #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
				Core.Logger.Log("[HP change] #{0} gained #{1} HP (#{2}=>#{3})", pkmn.Name, hpchange, oldhp, pkmn.HP);
			}
			Refresh();

			Core.Logger.LogDebug("[HP Changed] {0}: oldhp: {1} and animate: {2}", pkmn.Name, oldhp, anim.ToString());
			//Core.Logger.LogDebug("[HP Changed] {0}: CurrentHP: {1}", pkmn.Name, pkmn.HP);

			//yield return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.Fainted(IBattler pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.ChooseEnemyCommand(int index)
		void IPokeBattle_SceneNonInteractive.ChooseEnemyCommand(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) b.DefaultChooseEnemyCommand(index);
		}

		//void IPokeBattle_DebugSceneNoGraphics.ChooseNewEnemy(int index, IPokemon[] party)
		int IPokeBattle_SceneNonInteractive.ChooseNewEnemy(int index, IPokemon[] party)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battle is IBattleAI b) return b.DefaultChooseNewEnemy(index, party);
			return -1;
		}

		void IPokeBattle_DebugSceneNoGraphics.WildBattleSuccess()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.TrainerBattleSuccess()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.EXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.LevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		int IPokeBattle_DebugSceneNoGraphics.Blitz(int keys)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			return battle.Random(30);
		}

		void ISceneHasChatter.Chatter(PokemonEssentials.Interface.PokeBattle.IBattler attacker, PokemonEssentials.Interface.PokeBattle.IBattler opponent)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//void IPokeBattle_DebugSceneNoGraphics.Chatter(IBattler attacker, IBattler opponent)
		//{
		//	Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	(this as ISceneHasChatter).Chatter(attacker, opponent);
		//}

		void IPokeBattle_DebugSceneNoGraphics.ShowOpponent(int opp)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.HideOpponent()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.Recall(int battlerindex)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.DamageAnimation(IBattler pkmn, TypeEffective effectiveness)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		void IPokeBattle_DebugSceneNoGraphics.BattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//Core.Logger.Log($"[Judgment] #{b1.ToString()}:#{r1.Inspect()}, #{b2.ToString()}:#{r2.Inspect()}");
			Core.Logger.Log($"[Judgment] #{b1.ToString()}:#[{r1.JoinAsString(", ")}], #{b2.ToString()}:#[{r2.JoinAsString(", ")}]");
		}

		void IPokeBattle_DebugSceneNoGraphics.BattleArenaBattlers(IBattle b1, IBattle b2)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log($"[#{b1.ToString()} VS #{b2.ToString()}]");
		}

		void IPokeBattle_DebugSceneNoGraphics.CommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (attacker.IsNotNullOrNone())
			{
				if (opponent.IsNotNullOrNone())
				{
					//Core.Logger.Log($"[CommonAnimation] #{moveid}, #{attacker.Name}, #{opponent.Name}");
					Core.Logger.Log("[CommonAnimation] #{0}, #{1}, #{2}",moveid.ToString(), attacker.Name, opponent.Name);
				}
				else
				{
					//Core.Logger.Log($"[CommonAnimation] #{moveid}, #{attacker.Name}");
					Core.Logger.Log("[CommonAnimation] #{0}, #{1}",moveid.ToString(), attacker.Name);
				}
			}
			else
			{
				//Core.Logger.Log($"[CommonAnimation] #{moveid}");
				Core.Logger.Log("[CommonAnimation] #{0}",moveid.ToString());
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.Animation(Moves moveid, PokemonEssentials.Interface.PokeBattle.IBattler user, PokemonEssentials.Interface.PokeBattle.IBattler target, int hitnum)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log("{0} attack {1} With {2} for {3} hit times", user.Name, target.Name, moveid.ToString(), hitnum);

			if (user.IsNotNullOrNone())
			{
				if (target.IsNotNullOrNone())
				{
					//Core.Logger.Log($"[Animation] #{user.Name}, #{target.Name}");
					Core.Logger.Log("[Animation] #{0}, #{1}", user.Name.ToString(), target.Name);
				}
				else
				{
					//Core.Logger.Log($"[Animation] #{user.Name}");
					Core.Logger.Log("[Animation] #{0}", user.Name);
				}
			}
			else
			{
				Core.Logger.Log("[Animation]");
			}
		}

		#region Non Interactive Battle Scene
		int IPokeBattle_SceneNonInteractive.CommandMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

#if INTERNAL
			if (battle.Random(15) == 0) return 1;
			return 0;
#else
			return (this as IPokeBattle_DebugSceneNoGraphics).CommandMenu(index);
#endif
		}

		int IPokeBattle_SceneNonInteractive.FightMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

#if INTERNAL
			IBattler battler = @battle.battlers[index];
			int i = 0;
			do {
				i = Core.Rand.Next(4);
			} while (battler.moves[i].id==0);
			Core.Logger.Log("i=#{0}, pp=#{1}",i,battler.moves[i].PP);
			//Debug.flush;
			return i;
#else
			return (this as IPokeBattle_DebugSceneNoGraphics).FightMenu(index);
#endif
		}

		Items IPokeBattle_SceneNonInteractive.ItemMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

#if INTERNAL
			return -1;
#else
			return (this as IPokeBattle_DebugSceneNoGraphics).ItemMenu(index);
#endif
		}

		int IPokeBattle_SceneNonInteractive.ChooseTarget(int index, PokemonUnity.Attack.Targets targettype)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

#if INTERNAL
			List<int> targets = new List<int>();
			for (int i = 0; i < 4; i++)
			{
				if (@battle.battlers[index].IsOpposing(i) &&
				   !@battle.battlers[i].isFainted())
				{
					targets.Add(i);
				}
			}
			if (targets.Count == 0) return -1;
			return targets[Core.Rand.Next(targets.Count)];
#else
			return (this as IPokeBattle_DebugSceneNoGraphics).ChooseTarget(index, targettype);
#endif
		}

		/*int IPokeBattle_SceneNonInteractive.Switch(int index, bool lax, bool cancancel)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			for (int i = 0; i < @battle.Party(index).Length - 1; i++)
			{
				if (lax)
				{
					if (@battle.CanSwitchLax(index, i, false)) return i;
				}
				else
				{
					if (@battle.CanSwitch(index, i, false)) return i;
				}
			}
			return -1;
		}

		void IPokeBattle_SceneNonInteractive.ChooseEnemyCommand(int index)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}

		void IPokeBattle_SceneNonInteractive.ChooseNewEnemy(int index, IPokemon[] party)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new NotImplementedException();
		}*/
		#endregion

		private string PokemonString(IPokemon pkmn)
		{
			string status = string.Empty;
			if (pkmn.HP <= 0)
			{
				status = " [FNT]";
			}
			else
			{
				switch (pkmn.Status)
				{
					case Status.SLEEP:
						status = " [SLP]";
						break;
					case Status.FROZEN:
						status = " [FRZ]";
						break;
					case Status.BURN:
						status = " [BRN]";
						break;
					case Status.PARALYSIS:
						status = " [PAR]";
						break;
					case Status.POISON:
						status = " [PSN]";
						break;
				}
			}
			Core.Logger.LogDebug("#{0} (Lv. #{1})#{2} HP: #{3}/#{4}",pkmn.Name, pkmn.Level, status, pkmn.HP, pkmn.TotalHP);
			return $"#{pkmn.Name} (Lv. #{pkmn.Level})#{status} HP: #{pkmn.HP}/#{pkmn.TotalHP}";
		}

		private string PokemonString(IBattler pkmn)
		{
			if (!pkmn.pokemon.IsNotNullOrNone())
			{
				return "";
			}
			string status = string.Empty;
			if (pkmn.HP <= 0)
			{
				status = " [FNT]";
			}
			else
			{
				switch (pkmn.Status)
				{
					case Status.SLEEP:
						status = " [SLP]";
						break;
					case Status.FROZEN:
						status = " [FRZ]";
						break;
					case Status.BURN:
						status = " [BRN]";
						break;
					case Status.PARALYSIS:
						status = " [PAR]";
						break;
					case Status.POISON:
						status = " [PSN]";
						break;
				}
			}
			Core.Logger.LogDebug("#{0} (Lv. #{1})#{2} HP: #{3}/#{4}",pkmn.Name, pkmn.Level, status, pkmn.HP, pkmn.TotalHP);
			return $"#{pkmn.Name} (Lv. #{pkmn.Level})#{status} HP: #{pkmn.HP}/#{pkmn.TotalHP}";
		}

		private string MoveString(IMove move, int index)
		{
			string ret = string.Format("{0} - Press {1}", Game._INTL(move.id.ToString(TextScripts.Name)), index);
			string typename = Game._INTL(move.Type.ToString(TextScripts.Name));
			if (move.id > 0)
			{
				ret += string.Format(" ({0}) PP: {1}/{2}", typename, move.PP, move.TotalPP);
				Core.Logger.LogDebug("{0} - Press {1} ({2}) PP: {3}/{4}", move.id.ToString(), index, typename, move.PP, move.TotalPP);
			}
			else Core.Logger.LogDebug("{0} - Press {1}", move.id.ToString(), index);
			return ret;
		}
	}
}