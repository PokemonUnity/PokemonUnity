using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;

namespace PokemonUnity.Combat
{
	//ToDo: Remove `Battle` inheritance?... Maybe this class and battle scene should be refactored...
	public class PokeBattle_SafariZone : Battle, PokemonEssentials.Interface.Screen.ISafariZone_Scene
	{
		//public Environment environment { get; private set; }
		//public Pokemon[] party1 { get; private set; }
		//public Pokemon[] party2 { get; private set; }
		//public Trainer player { get; private set; }
		//public bool battlescene { get; private set; }
		//public int debugupdate { get; private set; }
		//include PokeBattle_BattleCommon;
		/// <summary>
		/// Scene Id; Match against unity's scene loader management, and use this value as input parameter
		/// </summary>
		public virtual int Id { get { return 0; } }

		public PokeBattle_SafariZone(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene,PokemonEssentials.Interface.PokeBattle.ITrainer player,PokemonEssentials.Interface.PokeBattle.IPokemon[] p1,PokemonEssentials.Interface.PokeBattle.IPokemon[] p2) : base(scene, p1, p2, player, null)
		{
			initialize(scene, player, p2);
		}
		public PokemonEssentials.Interface.Screen.ISafariZone_Scene initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene,ITrainer player,IPokemon[] party)
		{
			base.scene=scene;
			base.party2=party;
			@peer=Monster.PokeBattle_BattlePeer.create();
			base.player=new ITrainer[] { player };
			_battlers=new IBattler[] {
			   new Pokemon(this,0), //PokeBattle_FakeBattler(party[0],0),
			   new Pokemon(this,1), //PokeBattle_FakeBattler(party[0],1),
			   new Pokemon(this,2), //PokeBattle_FakeBattler(party[0],2),
			   new Pokemon(this,3)  //PokeBattle_FakeBattler(party[0],3)
			};
			@environment=Overworld.Environments.None;
			@battlescene=true;
			@decision=BattleResults.InProgress;
			@ballcount=0;
			return this;
		}

		//public override bool IsOpposing (int index) {
		//  return (index%2)==1;
		//}
		//public override bool IsDoubleBattler (int index) {
		//  return (index>=2);
		//}
		//public override IBattler[] battlers { get; private set; }
		//  return @battlers; }
		//public override Trainer[] opponent { get {
		//  return null; } }
		//public override bool doublebattle { get {
		//  return false; } set { } }
		private int ballcount { get; set; }
		public int ballCount{ get {
				return (@ballcount<0) ? 0 : @ballcount;
			}
			set {
				@ballcount=(value<0) ? 0 : value;
		} }

		//public Player Player() {
		//	return @player;
		//}

		//public class BattleAbortedException : Exception{
		//}

		//public override void Abort() {
		//	//throw new BattleAbortedException("Battle aborted");
		//	GameDebug.LogError("Battle aborted");
		//}

		public int EscapeRate(int rareness) {
			int ret=25;
			if (rareness<200) ret=50;
			if (rareness<150) ret=75;
			if (rareness<100) ret=100;
			if (rareness<25) ret=125;
			return ret;
		}

		public BattleResults StartBattle() {
			try { //begin
				PokemonEssentials.Interface.PokeBattle.IPokemon wildpoke=@party2[0];
				this.Player().seen[wildpoke.Species]=true;
				//Game.GameData.Player.Pokedex[(int)wildpoke.Species,0]=(byte)1;
				//Game.SeenForm(wildpoke);
				base.SetSeen(wildpoke);
				if (@scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics s0) s0.StartBattle(this);
				DisplayPaused(Game._INTL("Wild {1} appeared!",wildpoke.Name));
				if (@scene is PokemonEssentials.Interface.Screen.IPokeBattle_Scene s1) s1.SafariStart();
				//dexdata=OpenDexData;
				//DexDataOffset(dexdata,wildpoke.Species,16);
				//rareness=dexdata.fgetb; // Get rareness from dexdata file
				//dexdata.close;
				int rareness = (int)Kernal.PokemonData[wildpoke.Species].Rarity;
				int g=(rareness*100)/1275;
				int e=(EscapeRate(rareness)*100)/1275;
				g=(int)Math.Min((int)Math.Max(g,3),20);
				e=(int)Math.Min((int)Math.Max(e,3),20);
				int lastCommand=0;
				do { //begin;
					int cmd=(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_Scene).SafariCommandMenu(0);
					switch (cmd) {
						case 0: // Ball
							if (Game.GameData is PokemonEssentials.Interface.IGameUtility pc && pc.BoxesFull()) {
							//if (Game.GameData.Player.PC.hasSpace()) {
								Display(Game._INTL("The boxes are full! You can't catch any more Pokémon!"));
								continue;
							}
							@ballCount-=1;
							int rare=(g*1275)/100;
							Items safariBall=Items.SAFARI_BALL;
							if (safariBall != Items.NONE) {
								base.ThrowPokeball(1,safariBall,rare,true);
							}
							break;
						case 1: // Bait
							DisplayBrief(Game._INTL("{1} threw some bait at the {2}!",this.Player().name,wildpoke.Name));
							(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_Scene).ThrowBait();
							g/=2; // Harder to catch
							e/=2; // Less likely to escape
							g=(int)Math.Min((int)Math.Max(g,3),20);
							e=(int)Math.Min((int)Math.Max(e,3),20);
							lastCommand=1;
							break;
						case 2: // Rock
							DisplayBrief(Game._INTL("{1} threw a rock at the {2}!",this.Player().name,wildpoke.Name));
							(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_Scene).ThrowRock();
							g*=2; // Easier to catch
							e*=2; // More likely to escape
							g=(int)Math.Min((int)Math.Max(g,3),20);
							e=(int)Math.Min((int)Math.Max(e,3),20);
							lastCommand=2;
							break;
						case 3: // Run
							DisplayPaused(Game._INTL("Got away safely!"));
							@decision=BattleResults.FORFEIT;
							break;
					}
					if (@decision==0) {
						if (@ballCount<=0) {
							Display(Game._INTL("PA:  You have no Safari Balls left! Game over!"));
							@decision=BattleResults.LOST;
						} else if (Random(100)<5*e) {
							Display(Game._INTL("{1} fled!",wildpoke.Name));
							@decision=BattleResults.FORFEIT;
						} else if (lastCommand==1) {
							Display(Game._INTL("{1} is eating!",wildpoke.Name));
						} else if (lastCommand==2) {
							Display(Game._INTL("{1} is angry!",wildpoke.Name));
						} else {
							Display(Game._INTL("{1} is watching carefully!",wildpoke.Name));
						}
					}
				} while (@decision==0);
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).EndBattle(@decision);
			} catch { //rescue BattleAbortedException;
				@decision=0;
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).EndBattle(@decision);
			}
			return @decision;
		}

		// ############
		public void DebugUpdate() {
			@debugupdate+=1;
			if (@debugupdate==30) {
				//Graphics?.update();
				@debugupdate=0;
			}
		}

		public override void DisplayPaused(string msg) {
			if (@debug) {
				DebugUpdate();
				GameDebug.Log(msg);
			}
			else {
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).DisplayPausedMessage(msg);
			}
		}

		public override void Display(string msg) {
			if (@debug) {
				DebugUpdate();
				GameDebug.Log(msg);
			}
			else {
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).DisplayMessage(msg);
			}
		}

		public override void DisplayBrief(string msg) {
			if (@debug) {
				DebugUpdate();
				GameDebug.Log(msg);
			}
			else {
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).DisplayMessage(msg,true);
			}
		}

		public override bool DisplayConfirm(string msg) {
			if (@debug) {
				DebugUpdate();
				GameDebug.Log(msg);
				return true;
			}
			else {
				return (@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).DisplayConfirmMessage(msg);
			}
		}

		public override void GainEXP() {
		}

		void IScene.Refresh() { }
	}
}