using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;

namespace SandBox
{
    class Program
    {
        static void Main(string[] args)
        {
            Game.ResetAndOpenSql(@"Data\veekun-pokedex.sqlite");
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            
            Console.WriteLine("######################################");
            Console.WriteLine("# Hello - Welcome to Console Battle! #");
            Console.WriteLine("######################################");

            Battle battle;
            PokeBattle pokeBattle = new PokeBattle();
            pokeBattle.initialize();


            PokemonUnity.Monster.Pokemon[] p1 = new PokemonUnity.Monster.Pokemon[2] { new PokemonUnity.Monster.Pokemon(Pokemons.ABRA), new PokemonUnity.Monster.Pokemon(Pokemons.EEVEE) };
            PokemonUnity.Monster.Pokemon[] p2 = new PokemonUnity.Monster.Pokemon[2] { new PokemonUnity.Monster.Pokemon(Pokemons.MONFERNO), new PokemonUnity.Monster.Pokemon(Pokemons.SEEDOT) };

            p1[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
            p1[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

            p2[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
            p2[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

            PokemonUnity.Character.TrainerData trainerData = new PokemonUnity.Character.TrainerData("FlakTester", true, 120, 002);
            Game.GameData.Player = new PokemonUnity.Character.Player(trainerData, p1);
            
            p1[0].SetNickname("Test1");
            p1[1].SetNickname("Test2");
            
            p2[0].SetNickname("OppTest1");
            p2[1].SetNickname("OppTest2");

            Trainer player = new Trainer(Game.GameData.Player.Name, TrainerTypes.PLAYER);
            Trainer pokemon = new Trainer("Wild Pokemon", TrainerTypes.WildPokemon);
            Game.GameData.Trainer = player;

            battle = new Battle(pokeBattle, Game.GameData.Player.Party, p2, new Trainer[1] { Game.GameData.Trainer }, new Trainer[1]
                { pokemon }, 2);

            battle.rules.Add("suddendeath", false);
            battle.rules.Add("drawclause", false);
            battle.rules.Add("modifiedselfdestructclause", false);

            battle.SetWeather(Weather.SUNNYDAY);

            battle.pbStartBattle(true);
        }
    }

    class BattleScene
    {

    }

    class PokeBattle : IPokeBattle_Scene
    {
        public Battle battle;
        public bool aborted;
        public bool abortable;
        public int[] lastcmd;
        public int[] lastmove;

        public void initialize()
        {
            battle = null;
            lastcmd = new int[] { 0, 0, 0, 0 };
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

            throw new NotImplementedException();
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

        void IPokeBattle_Scene.pbAnimation(Moves moveid, PokemonUnity.Combat.Pokemon user, PokemonUnity.Combat.Pokemon target, int hitnum)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("{0} attack {1} With {2} for {3} hit times", user.Name, target.Name, moveid.ToString(), hitnum);

            //throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbAnimationCore(string animation, PokemonUnity.Combat.Pokemon user, PokemonUnity.Combat.Pokemon target, bool oppmove)
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

        void IPokeBattle_Scene.pbChangePokemon(PokemonUnity.Combat.Pokemon attacker, Forms pokemon)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbChangeSpecies(PokemonUnity.Combat.Pokemon attacker, Pokemons species)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        void IHasChatter.pbChatter(PokemonUnity.Combat.Pokemon attacker, Trainer opponent)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbChooseEnemyCommand(int index)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            battle.pbDefaultChooseEnemyCommand(index);

            //throw new NotImplementedException();
        }

        int IPokeBattle_Scene.pbChooseMove(PokemonUnity.Monster.Pokemon pokemon, string message)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        int IPokeBattle_Scene.pbChooseNewEnemy(int index, PokemonUnity.Monster.Pokemon[] party)
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

            Console.WriteLine("Enemy: {0} HP: {1}/{2}", battle.battlers[0].pbOpposing1.Name, battle.battlers[0].pbOpposing1.HP, battle.battlers[0].pbOpposing1.TotalHP);

            Console.WriteLine("What will\n{0} do?", battle.battlers[index].Name);
            Console.WriteLine("Fight - 0");
            Console.WriteLine("Bag - 1");
            Console.WriteLine("Pokémon - 2");
            Console.WriteLine(!shadowTrainer ? "Call - 3" : "Run - 3");

            bool appearing = true;
            do
            {
                ConsoleKeyInfo fs = Console.ReadKey(true);
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

            //throw new NotImplementedException();
        }

        int IPokeBattle_Scene.pbCommandMenuEx(int index, string[] texts, int mode)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        // Not going use this because console
        void IPokeBattle_Scene.pbCommonAnimation(string animname, PokemonUnity.Combat.Pokemon user, PokemonUnity.Combat.Pokemon target, int hitnum)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            //Console.WriteLine(animname);
            //throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbDamageAnimation(PokemonUnity.Combat.Pokemon pkmn, float effectiveness)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("[Damage Animation] {0} for effectiveness: {1}", pkmn.Name, effectiveness);

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

            Console.WriteLine(msg);

            //throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbDisplayPausedMessage(string msg)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Console.WriteLine(msg);

            //throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbDisposeSprites()
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbEndBattle(BattleResults result)
        {
            Console.WriteLine("BattleResults: {0}", result.ToString());

            //throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbEXPBar(PokemonUnity.Monster.Pokemon pokemon, PokemonUnity.Combat.Pokemon battler, int startexp, int endexp, int tempexp1, int tempexp2)
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
            string ret = Game._INTL("{0} - Press {1}", move.Name, index);
            string typename = move.Type.ToString(TextScripts.Name);
            if (move.MoveId > 0)
            {
                ret += Game._INTL(" ({0}) PP: {1}/{2}", typename, move.PP, move.TotalPP);
            }
            return ret;
        }

        int IPokeBattle_Scene.pbFightMenu(int index)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            IMove[] moves = @battle.battlers[index].moves;
            string[] commands = new string[4] { 
               pbMoveString(moves[0], 1),
               pbMoveString(moves[1], 2),
               pbMoveString(moves[2], 3),
               pbMoveString(moves[3], 4) };
            int index_ = @lastmove[index];
            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine(commands[i]);
            }
            bool appearing = true;
            do
            { //;loop
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
                ConsoleKeyInfo fs = Console.ReadKey(true);

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

        int IPokeBattle_Scene.pbForgetMove(PokemonUnity.Monster.Pokemon pokemon, Moves moveToLearn)
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

        public IEnumerator pbHPChanged(PokemonUnity.Combat.Pokemon pkmn, int oldhp, bool animate)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
            pkmn.HP -= oldhp;

            Console.WriteLine("[HP Changed] {0}: oldhp: {1} and animate: {2}", pkmn.Name, oldhp, animate.ToString());
            Console.WriteLine("[HP Changed] {0}: CurrentHP: {1}", pkmn.Name, pkmn.HP);
            
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

            Console.WriteLine("Need to implment item system in textbased-line");

            throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbLevelUp(PokemonUnity.Monster.Pokemon pokemon, PokemonUnity.Combat.Pokemon battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        string IPokeBattle_Scene.pbMoveString(string move)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }

        string IPokeBattle_Scene.pbNameEntry(string helptext, PokemonUnity.Monster.Pokemon pokemon)
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

            lastcmd = new int[] { 0, 0, 0, 0 };

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

        void IPokeBattle_Scene.pbSendOut(int battlerindex, PokemonUnity.Monster.Pokemon pkmn)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            Console.WriteLine("Trainer: {0}. Pokemon: {1}", battlerindex, pkmn.ToString());

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

        void IPokeBattle_Scene.pbStartBattle(Battle battle)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            this.battle = battle;

            if (battle.player?.Length == 1)
            {
                GameDebug.Log("One player battle!");
            }

            if (battle.opponent != null)
            {
                GameDebug.Log("opponent founded!");
                if (battle.opponent.Length == 1)
                {
                    GameDebug.Log("One opponent battle!");
                }
            }

            if (!battle.doublebattle)
            {
                GameDebug.Log("Single Battle");
                Console.WriteLine("Player: {0} has {1} in their party", battle.player[0].name, battle.party1.Length);
                Console.WriteLine("Opponent: {0} has {1} in their party", battle.opponent[0].name, battle.party2.Length);
                //bool appearing = true;
                //do
                //{
                //    ConsoleKeyInfo fs = Console.ReadKey(true);
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

        void IPokeBattle_Scene.pbTrainerSendOut(int battlerindex, PokemonUnity.Monster.Pokemon pkmn)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            if (battle.opponent != null)
                Console.WriteLine("Trainer: {0}. Pokemon: {1}", battle.opponent[0].name, pkmn.ToString());

            //throw new NotImplementedException();
        }

        void IPokeBattle_Scene.pbTrainerWithdraw(Battle battle, PokemonUnity.Combat.Pokemon pkmn)
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

        void IPokeBattle_Scene.pbWithdraw(Battle battle, PokemonUnity.Combat.Pokemon pkmn)
        {
            GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

            throw new NotImplementedException();
        }
    }
}
