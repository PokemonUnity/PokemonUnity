using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Combat
{
	public class PokeBattle_SafariZone : Battle, PokemonEssentials.Interface.PokeBattle.ISafariZone 
	{
		//public Environment environment { get; private set; }
		//public Pokemon[] party1 { get; private set; }
		//public Pokemon[] party2 { get; private set; }
		//public Trainer player { get; private set; }
		//public bool battlescene { get; private set; }
		//public int debugupdate { get; private set; }
		//include PokeBattle_BattleCommon;
		public PokeBattle_SafariZone(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene,PokemonEssentials.Interface.PokeBattle.ITrainer player,PokemonEssentials.Interface.PokeBattle.IPokemon[] p1,PokemonEssentials.Interface.PokeBattle.IPokemon[] p2) : base(scene, p1, p2, player, null)
        {
			initialize(scene, player, p2);
        }
		public PokemonEssentials.Interface.PokeBattle.ISafariZone initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene,ITrainer player,IPokemon[] party) 
		{
			base.scene=scene;
			base.party2=party;
			@peer=Monster.PokeBattle_BattlePeer.create();
			base.player=new ITrainer[] { player };
			@battlers=new IBattler[] {
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

		//public override bool pbIsOpposing (int index) {
		//  return (index%2)==1;
		//}
		//public override bool pbIsDoubleBattler (int index) {
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

		//public Player pbPlayer() {
		//	return @player;
		//}

		//public class BattleAbortedException : Exception{
		//}

		//public override void pbAbort() {
		//	//throw new BattleAbortedException("Battle aborted");
		//	GameDebug.LogError("Battle aborted");
		//}

		public int pbEscapeRate(int rareness) {
			int ret=25;
			if (rareness<200) ret=50;
			if (rareness<150) ret=75;
			if (rareness<100) ret=100;
			if (rareness<25) ret=125;
			return ret;
		}

		public BattleResults pbStartBattle() {
			try { //begin
				PokemonEssentials.Interface.PokeBattle.IPokemon wildpoke=@party2[0];
				this.pbPlayer().seen[wildpoke.Species]=true;
				//Game.GameData.Player.Pokedex[(int)wildpoke.Species,0]=(byte)1;
				//Game.pbSeenForm(wildpoke);
				base.pbSetSeen(wildpoke);
				if (@scene is PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics s0) s0.pbStartBattle(this);
				pbDisplayPaused(Game._INTL("Wild {1} appeared!",wildpoke.Name));
				if (@scene is PokemonEssentials.Interface.Screen.IPokeBattle_Scene s1) s1.pbSafariStart();
				//dexdata=pbOpenDexData;
				//pbDexDataOffset(dexdata,wildpoke.Species,16);
				//rareness=dexdata.fgetb; // Get rareness from dexdata file
				//dexdata.close;
				int rareness = (int)Kernal.PokemonData[wildpoke.Species].Rarity;
				int g=(rareness*100)/1275;
				int e=(pbEscapeRate(rareness)*100)/1275;
				g=(int)Math.Min((int)Math.Max(g,3),20);
				e=(int)Math.Min((int)Math.Max(e,3),20);
				int lastCommand=0;
				do { //begin;
					int cmd=(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_Scene).pbSafariCommandMenu(0);
					switch (cmd) {
						case 0: // Ball
							if (Game.GameData is PokemonEssentials.Interface.IGameUtility pc && pc.pbBoxesFull()) {
							//if (Game.GameData.Player.PC.hasSpace()) {
								pbDisplay(Game._INTL("The boxes are full! You can't catch any more Pokémon!"));
								continue;
							}
							@ballCount-=1;
							int rare=(g*1275)/100;
							Items safariBall=Items.SAFARI_BALL;
							if (safariBall != Items.NONE) {
								base.pbThrowPokeball(1,safariBall,rare,true);
							}
							break;
						case 1: // Bait
							pbDisplayBrief(Game._INTL("{1} threw some bait at the {2}!",this.pbPlayer().name,wildpoke.Name));
							(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_Scene).pbThrowBait();
							g/=2; // Harder to catch
							e/=2; // Less likely to escape
							g=(int)Math.Min((int)Math.Max(g,3),20);
							e=(int)Math.Min((int)Math.Max(e,3),20);
							lastCommand=1;
							break;
						case 2: // Rock
							pbDisplayBrief(Game._INTL("{1} threw a rock at the {2}!",this.pbPlayer().name,wildpoke.Name));
							(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_Scene).pbThrowRock();
							g*=2; // Easier to catch
							e*=2; // More likely to escape
							g=(int)Math.Min((int)Math.Max(g,3),20);
							e=(int)Math.Min((int)Math.Max(e,3),20);
							lastCommand=2;
							break;
						case 3: // Run
							pbDisplayPaused(Game._INTL("Got away safely!"));
							@decision=BattleResults.FORFEIT;
							break;
					}
					if (@decision==0) {
						if (@ballCount<=0) {
							pbDisplay(Game._INTL("PA:  You have no Safari Balls left! Game over!"));
							@decision=BattleResults.LOST;
						} else if (pbRandom(100)<5*e) {
							pbDisplay(Game._INTL("{1} fled!",wildpoke.Name));
							@decision=BattleResults.FORFEIT;
						} else if (lastCommand==1) {
							pbDisplay(Game._INTL("{1} is eating!",wildpoke.Name));
						} else if (lastCommand==2) {
							pbDisplay(Game._INTL("{1} is angry!",wildpoke.Name));
						} else {
							pbDisplay(Game._INTL("{1} is watching carefully!",wildpoke.Name));
						}
					}
				} while (@decision==0);
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).pbEndBattle(@decision);
			} catch { //rescue BattleAbortedException;
				@decision=0;
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).pbEndBattle(@decision);
			}
			return @decision;
		}

		// ############
		public void pbDebugUpdate() {
			@debugupdate+=1;
			if (@debugupdate==30) {
				//Graphics?.update();
				@debugupdate=0;
			}
		}

		public override void pbDisplayPaused(string msg) {
			if (@debug) {
				pbDebugUpdate();
				GameDebug.Log(msg);
			}
			else {
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).pbDisplayPausedMessage(msg);
			}
		}

		public override void pbDisplay(string msg) {
			if (@debug) {
				pbDebugUpdate();
				GameDebug.Log(msg);
			}
			else {
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).pbDisplayMessage(msg);
			}
		}

		public override void pbDisplayBrief(string msg) {
			if (@debug) {
				pbDebugUpdate();
				GameDebug.Log(msg);
			}
			else {
				(@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).pbDisplayMessage(msg,true);
			}
		}

		public override bool pbDisplayConfirm(string msg) {
			if (@debug) {
				pbDebugUpdate();
				GameDebug.Log(msg);
				return true;
			}
			else {
				return (@scene as PokemonEssentials.Interface.Screen.IPokeBattle_DebugSceneNoGraphics).pbDisplayConfirmMessage(msg);
			}
		}

		public override void pbGainEXP() {
		}
	}
}