using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;

namespace PokemonUnity.Combat
{
	public class PokeBattle_BattleArena : Battle, PokemonEssentials.Interface.PokeBattle.IBattleArena {
		public bool battlerschanged { get; private set; }
		public int[] mind { get; private set; }
		public int[] skill { get; private set; }
		public int[] starthp { get; private set; }
		public int[] partyindexes { get; private set; }
		public int count { get; private set; }
		public void initialize(PokemonEssentials.Interface.PokeBattle.IPokeBattleArena_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, PokemonEssentials.Interface.PokeBattle.ITrainer[] player, PokemonEssentials.Interface.PokeBattle.ITrainer[] opponent) { return this; } //ToDo: finish this later...
		public PokeBattle_BattleArena(PokemonEssentials.Interface.PokeBattle.IPokeBattleArena_Scene scene, PokemonEssentials.Interface.PokeBattle.IPokemon[] p1, PokemonEssentials.Interface.PokeBattle.IPokemon[] p2, PokemonEssentials.Interface.PokeBattle.ITrainer[] player, PokemonEssentials.Interface.PokeBattle.ITrainer[] opponent) : base (scene, p1, p2, player, opponent) { 
			//base.this() //base.initialize();
			@battlerschanged=true;
			@mind=new int[] { 0, 0 };
			@skill=new int[] { 0, 0 };
			@starthp=new int[] { 0, 0 };
			@count=0;
			@partyindexes=new int[] { 0, 0 };
		}

		PokemonEssentials.Interface.PokeBattle.IBattle pbCreateBattle(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, PokemonEssentials.Interface.PokeBattle.ITrainer[] trainer1, PokemonEssentials.Interface.PokeBattle.ITrainer[] trainer2)
		{
			return new PokeBattle_RecordedBattleArena(scene,
				trainer1[0].party, trainer2[0].party, trainer1, trainer2);
		}

		public override bool pbDoubleBattleAllowed() {
			return false;
		}

		public override bool pbEnemyShouldWithdraw (int index) {
			return false;
		}

		public override bool pbCanSwitchLax (int idxPokemon,int pkmnidxTo,bool showMessages) {
			if (showMessages) {
				PokemonEssentials.Interface.PokeBattle.IBattler thispkmn=@battlers[idxPokemon];
				pbDisplayPaused(Game._INTL("{1} can't be switched out!",thispkmn.ToString()));
			}
			return false;
		}

		public override void pbSwitch(bool favorDraws=false) {
			if (!favorDraws) {
				if (@decision>0) return;
			}
			else {
				if (@decision==BattleResults.DRAW) return;
			}
			pbJudge();
			if (@decision>0) return;
			List<int> switched=new List<int>();
			if (@battlers[0].isFainted() && @partyindexes[0]+1<this.pbParty(0).Length) {
				@partyindexes[0]+=1;
				int newpoke=@partyindexes[0];
				pbMessagesOnReplace(0,newpoke);
				pbReplace(0,newpoke);
				pbOnActiveOne(@battlers[0]);
				switched.Add(0);
			}
			if (@battlers[1].isFainted() && @partyindexes[1]+1<this.pbParty(1).Length) {
				@partyindexes[1]+=1  ;
				int newenemy=@partyindexes[1];
				pbMessagesOnReplace(1,newenemy);
				pbReplace(1,newenemy);
				pbOnActiveOne(@battlers[1]);
				switched.Add(1);
			}
			if (switched.Count>0) {
				PokemonEssentials.Interface.PokeBattle.IBattler[] priority=pbPriority();
				foreach (var i in priority) {
					if (switched.Contains(i.Index)) i.pbAbilitiesOnSwitchIn(true);
				}
			}
		}

		public override void pbOnActiveAll() {
			@battlerschanged=true;
			@mind[0]=0;
			@mind[1]=0;
			@skill[0]=0;
			@skill[1]=0;
			@starthp[0]=battlers[0].HP;
			@starthp[1]=battlers[1].HP;
			@count=0;
			base.pbOnActiveAll(); //return
		}

		public override bool pbOnActiveOne(PokemonEssentials.Interface.PokeBattle.IBattler pkmn, bool onlyabilities = false, bool moldbreaker = false) {
			@battlerschanged=true;
			@mind[0]=0;
			@mind[1]=0;
			@skill[0]=0;
			@skill[1]=0;
			@starthp[0]=battlers[0].HP;
			@starthp[1]=battlers[1].HP;
			@count=0;
			return base.pbOnActiveOne(pkmn, onlyabilities, moldbreaker);
		}

		public int pbMindScore(PokemonEssentials.Interface.PokeBattle.IBattleMove move) {       
			if (move.Effect==Attack.Data.Effects.x070 ||	// Detect/Protect
				move.Effect==Attack.Data.Effects.x075 ||	// Endure
				move.Effect==Attack.Data.Effects.x09F) {	// Fake Out
				return -1;
			}
			if (move.Effect==Attack.Data.Effects.x054 ||	// Counter
				move.Effect==Attack.Data.Effects.x091 ||	// Mirror Coat
				move.Effect==Attack.Data.Effects.x01B) {	// Bide
				return 0;
			}
			if (move.Power==0) {
				return 0;
			}
			else {
				return 1;
			}
		}

		public override void pbCommandPhase() {
			if (@battlerschanged) {
				((IPokeBattleArena_Scene)@scene).pbBattleArenaBattlers(@battlers[0],@battlers[1]);
				@battlerschanged=false;
				@count=0;
			}
			base.pbCommandPhase();
			if (@decision!=0) return;
			// Update mind rating (asserting that a move was chosen)
			// TODO: Actually done at Pokémon's turn
			for (int i = 0; i < 2; i++) {
				if (@choices[i].Move.IsNotNullOrNone() && @choices[i].Action==ChoiceAction.UseMove) {//[0]==1
					@mind[i]+=pbMindScore(@choices[i].Move);
				}
			}
		}

		public override void pbEndOfRoundPhase() {
			base.pbEndOfRoundPhase();
			if (@decision!=0) return;
			@count+=1;
			// Update skill rating
			for (int i = 0; i < 2; i++) {
				@skill[i]+=this.successStates[i].Skill;
			}
			GameDebug.Log($"[Mind: #{@mind.ToString()}, Skill: #{@skill.ToString()}]");
			if (@count==3) {
				int[] points=new int[] { 0, 0 };
				@battlers[0].pbCancelMoves();
				@battlers[1].pbCancelMoves();
				int[] ratings1=new int[] {0,0,0};
				int[] ratings2=new int[] {0,0,0};
				if (@mind[0]==@mind[1]) {
					ratings1[0]=1;
					ratings2[0]=1;
				} else if (@mind[0]>@mind[1]) {
					ratings1[0]=2;
				}
				else {
					ratings2[0]=2;
				}
				if (@skill[0]==@skill[1]) {
					ratings1[1]=1;
					ratings2[1]=1;
				} else if (@skill[0]>@skill[1]) {
					ratings1[1]=2;
				}
				else {
					ratings2[1]=2;
				}
				int[] body=new int[] { 0, 0 };
				body[0]=(int)Math.Floor((@battlers[0].HP*100f)/(int)Math.Max(@starthp[0],1));
				body[1]=(int)Math.Floor((@battlers[1].HP*100f)/(int)Math.Max(@starthp[1],1));
				if (body[0]==body[1]) {
					ratings1[2]=1;
					ratings2[2]=1;
				} else if (body[0]>body[1]) {
					ratings1[2]=2;
				}
				else {
					ratings2[2]=2;
				}
				((IPokeBattleArena_Scene)@scene).pbBattleArenaJudgment(@battlers[0],@battlers[1],ratings1,ratings2);
				points=new int[] { 0, 0 };
				for (int i = 0; i < 3; i++) {
					points[0]+=ratings1[i];
					points[1]+=ratings2[i];
				}
				if (points[0]==points[1]) {
					pbDisplay(Game._INTL("{1} tied the opponent\n{2} in a referee's decision!",
						@battlers[0].Name,@battlers[1].Name)) ;
					@battlers[0].HP=0; // Note: Pokemon doesn't really lose HP, but the effect is mostly the same
					@battlers[0].pbFaint(false);
					@battlers[1].HP=0  ;
					@battlers[1].pbFaint(false);
				} else if (points[0]>points[1]) {
					pbDisplay(Game._INTL("{1} defeated the opponent\n{2} in a referee's decision!",
						@battlers[0].Name,@battlers[1].Name));
					@battlers[1].HP=0  ;
					@battlers[1].pbFaint(false);
				}
				else {
					pbDisplay(Game._INTL("{1} lost to the opponent\n{2} in a referee's decision!",
						@battlers[0].Name,@battlers[1].Name));
					@battlers[0].HP=0  ;
					@battlers[0].pbFaint(false);
				}
				pbGainEXP();
				pbSwitch();
			} 
		}
	}

	/*public partial class PokeBattle_Scene : IPokeBattleArena_Scene {
		public void updateJudgment(window,int phase,PokemonEssentials.Interface.PokeBattle.IBattler battler1,PokemonEssentials.Interface.PokeBattle.IBattler battler2,int[] ratings1,int[] ratings2) {
		int total1=0;
		int total2=0;
		for (int i = 0; i < phase; i++) {
			total1+=ratings1[i];
			total2+=ratings2[i];
		}
		window.contents.clear();
		pbSetSystemFont(window.contents);
		textpos=new {
			//[battler1.Name,64,0,2,new Color(248,0,0),new Color(208,208,200)],
			//[Game._INTL("VS"),144,0,2,new Color(72,72,72),new Color(208,208,200)],
			//[battler2.Name,224,0,2,new Color(72,72,72),new Color(208,208,200)],
			//[Game._INTL("Mind"),144,48,2,new Color(72,72,72),new Color(208,208,200)],
			//[Game._INTL("Skill"),144,80,2,new Color(72,72,72),new Color(208,208,200)],
			//[Game._INTL("Body"),144,112,2,new Color(72,72,72),new Color(208,208,200)],
			//[string.Format("{0}",total1),64,160,2,new Color(72,72,72),new Color(208,208,200)],
			//[Game._INTL("Judgment"),144,160,2,new Color(72,72,72),new Color(208,208,200)],
			//[string.Format("{0}",total2),224,160,2,new Color(72,72,72),new Color(208,208,200)]
		};
		pbDrawTextPositions(window.contents,textpos);
		List<> images=new List<>();
		for (int i = 0; i < phase; i++) {
			float y=[48,80,112][i];
			float x=(ratings1[i]==ratings2[i]) ? 64 : ((ratings1[i]>ratings2[i]) ? 0 : 32);
			images.Add(new { "Graphics/Pictures/judgment", 64 - 16, y, x, 0, 32, 32 });
			x=(ratings1[i]==ratings2[i]) ? 64 : ((ratings1[i]<ratings2[i]) ? 0 : 32);
			images.Add(new { "Graphics/Pictures/judgment", 224 - 16, y, x, 0, 32, 32 });
		}
		pbDrawImagePositions(window.contents,images);
		window.contents.fill_rect(16,150,256,4,new Color(80,80,80));
		}

		public void pbBattleArenaBattlers(PokemonEssentials.Interface.PokeBattle.IBattler battler1,PokemonEssentials.Interface.PokeBattle.IBattler battler2) {
		Game.pbMessage(Game._INTL("REFEREE: {1} VS {2}!\nCommence battling!\\wtnp[20]",
			battler1.Name,battler2.Name)); { pbUpdate(); }
		}

		public void pbBattleArenaJudgment(PokemonEssentials.Interface.PokeBattle.IBattler battler1,PokemonEssentials.Interface.PokeBattle.IBattler battler2,int[] ratings1,int[] ratings2) {
		msgwindow=null;
		dimmingvp=null;
		infowindow=null;
		try { //begin;
			msgwindow=Game.pbCreateMessageWindow();
			dimmingvp=new Viewport(0,0,Graphics.width,Graphics.height-msgwindow.height);
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: That's it! We will now go to judging to determine the winner!\\wtnp[20]")); {
				pbUpdate(); dimmingvp.update(); }
			dimmingvp.z=99999;
			infowindow=new SpriteWindow_Base(80,0,320,224);
			infowindow.contents=new Bitmap(
				infowindow.width-infowindow.borderX,
				infowindow.height-infowindow.borderY);
			infowindow.z=99999;
			infowindow.visible=false;
			for (int i = 0; i < 10; i++) {
			pbGraphicsUpdate();
			pbInputUpdate();  
			msgwindow.update();
			dimmingvp.update();
			dimmingvp.color=new Color(0,0,0,i*128/10);
			}
			updateJudgment(infowindow,0,battler1,battler2,ratings1,ratings2);
			infowindow.visible=true;
			for (int i = 0; i < 10; i++) {
			pbGraphicsUpdate();
			pbInputUpdate();  
			msgwindow.update();
			dimmingvp.update();
			infowindow.update();
			}
			updateJudgment(infowindow,1,battler1,battler2,ratings1,ratings2);
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: Judging category 1, Mind!\nThe Pokemon showing the most guts!\\wtnp[40]")); { 
				pbUpdate(); dimmingvp.update(); infowindow.update(); } 
			updateJudgment(infowindow,2,battler1,battler2,ratings1,ratings2);
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: Judging category 2, Skill!\nThe Pokemon using moves the best!\\wtnp[40]")); { 
				pbUpdate(); dimmingvp.update(); infowindow.update(); } 
			updateJudgment(infowindow,3,battler1,battler2,ratings1,ratings2);
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: Judging category 3, Body!\nThe Pokemon with the most vitality!\\wtnp[40]")); { 
				pbUpdate(); dimmingvp.update(); infowindow.update(); }
			int total1=0;
			int total2=0;
			for (int i = 0; i < 3; i++) {
			total1+=ratings1[i];
			total2+=ratings2[i];
			}
			if (total1==total2) {
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: Judgment: {1} to {2}!\nWe have a draw!\\wtnp[40]",total1,total2)); { 
				pbUpdate(); dimmingvp.update(); infowindow.update(); }
			} else if (total1>total2) {
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: Judgment: {1} to {2}!\nThe winner is {3}'s {4}!\\wtnp[40]",
				total1,total2,@battle.pbGetOwner(battler1.index).Name,battler1.Name)); { 
				pbUpdate(); dimmingvp.update(); infowindow.update(); }
			}
			else {
			Game.pbMessageDisplay(msgwindow,
				Game._INTL("REFEREE: Judgment: {1} to {2}!\nThe winner is {3}!\\wtnp[40]",
				total1,total2,battler2.Name)); { 
				pbUpdate(); dimmingvp.update(); infowindow.update(); }
			}
			infowindow.visible=false;
			msgwindow.visible=false;
			for (int i = 0; i < 10; i++) {
			pbGraphicsUpdate();
			pbInputUpdate();
			msgwindow.update();
			dimmingvp.update();
			dimmingvp.color=new Color(0,0,0,(10-i)*128/10);
			}
		} finally { //ensure;
			Game.pbDisposeMessageWindow(msgwindow);
			dimmingvp.dispose();
			infowindow.contents.dispose();
			infowindow.dispose();
		}
		}
	}*/
}