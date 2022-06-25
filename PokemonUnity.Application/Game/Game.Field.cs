using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.Battle;
using PokemonUnity.Utility;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonUnity
{
	// ===============================================================================
	// Battles
	// ===============================================================================
	public partial class GameTemp {
		public ISprite background_bitmap			{ get; protected set; }
	}

	public partial class PokemonTemp : ITempMetadataField
	{
		public Method? encounterType	{ get; set; } 
		public int evolutionLevels			    { get; set; }


		public bool batterywarning				{ get; protected set; }
		public IAudioBGM cueBGM					{ get; set; }
		public float? cueFrames				    { get; set; }
	}

	/// <summary>
	/// This module stores encounter-modifying events that can happen during the game.
	/// A procedure can subscribe to an event by adding itself to the event.  It will
	/// then be called whenever the event occurs.
	/// </summary>
	public static partial class EncounterModifier {
		public static List<Func<IEncounter,IEncounter>> @procs=new List<Func<IEncounter,IEncounter>>();
		public static List<Action> @procsEnd=new List<Action>();

		public static void register(Func<IEncounter,IEncounter> p) {
			@procs.Add(p);
		}

		public static void registerEncounterEnd(Action p) {
			@procsEnd.Add(p);
		}

		public static IEncounter trigger(IEncounter encounter) {
			foreach (var prc in @procs) {
				//encounter=prc.call(encounter);
				encounter=prc.Invoke(encounter);
			}
			return encounter;
		}

		public static void triggerEncounterEnd() {
			foreach (var prc in @procsEnd) {
				//prc.call();
				prc.Invoke();
			}
		}
	}

	public partial class Game : IGameField
	{
		public IPokeBattle_Scene pbNewBattleScene()
		{
			return null; //new PokeBattle_Scene();
		}

		public System.Collections.IEnumerator pbSceneStandby(Action block = null) {
			if (Scene != null && Scene is ISceneMap s0) {
				s0.disposeSpritesets();
			}
			GC.Collect();
			Graphics.frame_reset();
			yield return null;
			block.Invoke();
			if (Scene != null && Scene is ISceneMap s1) {
				s1.createSpritesets();
			}
		}

		public void pbBattleAnimation(IAudioBGM bgm=null,int trainerid=-1,string trainername="", Action block = null) {
			bool handled=false;
			IAudioBGS playingBGS=null;
			IAudioBGM playingBGM=null;
			if (Game.GameData.GameSystem != null && Game.GameData.GameSystem is IGameSystem s) {
				playingBGS=s.getPlayingBGS();
				playingBGM=s.getPlayingBGM();
				s.bgm_pause();
				s.bgs_pause();
			}
			if (this is IGameAudioPlay a0) a0.pbMEFade(0.25f);
			pbWait(10);
			if (this is IGameAudioPlay a1) a1.pbMEStop();
			if (bgm != null) {
				if (this is IGameAudioPlay a2) a2.pbBGMPlay(bgm);
			} else {
				if (this is IGameAudioPlay a2) a2.pbBGMPlay(pbGetWildBattleBGM(0));
			}
			IViewport viewport=null; //new Viewport(0,0,Graphics.width,Graphics.height);
			//viewport.z=99999;
			// Fade to gray a few times.
			viewport.color=null; //new Color(17*8,17*8,17*8);
			int z = 0; do { //3.times ;
				viewport.color.alpha=0;
				int x = 0; do { //6.times do;
					viewport.color.alpha+=30;
					Graphics?.update();
					Input.update();
					if (this is IGameMessage m) m.pbUpdateSceneMap(); x++;
				} while (x < 6);
				int y = 0; do { //6.times do;
					viewport.color.alpha-=30;
					Graphics?.update();
					Input.update();
					if (this is IGameMessage m) m.pbUpdateSceneMap(); y++;
				} while (y < 6); z++;
			} while (z < 3);
			if (GameTemp.background_bitmap != null) {
				GameTemp.background_bitmap.dispose();
			}
			GameTemp.background_bitmap=Graphics.snap_to_bitmap();
			//  Check for custom battle intro animations
			handled=pbBattleAnimationOverride(viewport,trainerid,trainername);
			//  Default battle intro animation
			if (!handled) {
				//if (Sprite.method_defined(:wave_amp) && Core.Rand.Next(15)==0) {
				//	viewport.color=new Color(0,0,0,255);
				//	sprite = new Sprite();
				//	bitmap=Graphics.snap_to_bitmap;
				//	bm=bitmap.clone();
				//	sprite.z=99999;
				//	sprite.bitmap = bm;
				//	sprite.wave_speed=500;
				//	for (int i = 0; i < 25; i++) {
				//		sprite.opacity-=10;
				//		sprite.wave_amp+=60;
				//		sprite.update();
				//		sprite.wave_speed+=30;
				//		2.times do;
				//			Graphics?.update();
				//		}
				//	}
				//	bitmap.dispose();
				//	bm.dispose();
				//	sprite.dispose();
				//} else if (Bitmap.method_defined(:radial_blur) && Core.Rand.Next(15)==0) {
				//	viewport.color=new Color(0,0,0,255);
				//	sprite = new Sprite();
				//	bitmap=Graphics.snap_to_bitmap;
				//	bm=bitmap.clone();
				//	sprite.z=99999;
				//	sprite.bitmap = bm;
				//	for (int i = 0; i < 15; i++) {
				//		bm.radial_blur(i,2);
				//		sprite.opacity-=15;
				//		2.times do;
				//			Graphics?.update();
				//		}
				//	}
				//	bitmap.dispose();
				//	bm.dispose();
				//	sprite.dispose();
				//} else 
				if (Core.Rand.Next(10)==0) {		// Custom transition method
					string[] scroll=new string[] {"ScrollDown","ScrollLeft","ScrollRight","ScrollUp",
							"ScrollDownRight","ScrollDownLeft","ScrollUpRight","ScrollUpLeft" };
					Graphics.freeze();
					viewport.color=null; //new Color(0,0,0,255);
					Graphics.transition(50,string.Format("Graphics/Transitions/{0}",scroll[Core.Rand.Next(scroll.Length)]));
				} else {
					string[] transitions= new string[] {
						//  Transitions with graphic files
						"021-Normal01","022-Normal02",
						"Battle","battle1","battle2","battle3","battle4",
						"computertr","computertrclose",
						"hexatr","hexatrc","hexatzr",
						"Image1","Image2","Image3","Image4",
						//  Custom transition methods
						"Splash","Random_stripe_v","Random_stripe_h",
						"RotatingPieces","ShrinkingPieces",
						"BreakingGlass","Mosaic","zoomin"
					};
					int rnd=Core.Rand.Next(transitions.Length);
					Graphics.freeze();
					viewport.color=null; //new Color(0,0,0,255);
					Graphics.transition(40,string.Format("Graphics/Transitions/%s",transitions[rnd]));
				}
				int i = 0; do { //5.times do;
					Graphics?.update();
					Input.update();
					if (this is IGameMessage m) m.pbUpdateSceneMap();
				} while (i < 5);
			}
			pbPushFade();
			//if (block_given?) yield;
			if (block != null) block.Invoke();
			pbPopFade();
			if (GameSystem != null && Game.GameData.GameSystem is IGameSystem s1) {
				s1.bgm_resume(playingBGM);
				s1.bgs_resume(playingBGS);
			}
			Global.nextBattleBGM=null;
			Global.nextBattleME=null;
			Global.nextBattleBack=null;
			PokemonEncounters.clearStepCount();
			for (int j = 0; j < 17; j++) {
				viewport.color=null; //new Color(0,0,0,(17-j)*15);
				Graphics?.update();
				Input.update();
				if (this is IGameMessage m) m.pbUpdateSceneMap();
			}
			viewport.dispose();
		}

		// Alias and use this method if you want to add a custom battle intro animation
		// e.g. variants of the Vs. animation.
		// Note that Game.GameData.GameTemp.background_bitmap contains an image of the current game
		// screen.
		// When the custom animation has finished, the screen should have faded to black
		// somehow.
		public bool pbBattleAnimationOverride(IViewport viewport,int trainerid=-1,string trainername="") {
			//  The following example runs a common event that ought to do a custom
			//  animation if some condition is true:
			// 
			//  if (GameMap != null && GameMap.map_id==20) {   // If on map 20
			//		pbCommonEvent(20);
			//		return true;                          // Note that the battle animation is done
			//  }
			// 
			// #### VS. animation, by Luka S.J. #####
			// #### Tweaked by Maruno           #####
			/*if (trainerid>=0) {
				string tbargraphic=string.Format("Graphics/Transitions/vsBar%s",trainerid.ToString()); //getConstantName(PBTrainers,trainerid) rescue null;
				if (pbResolveBitmap(tbargraphic) == null) tbargraphic=string.Format("Graphics/Transitions/vsBar%d",trainerid);
				string tgraphic=string.Format("Graphics/Transitions/vsTrainer%s",trainerid.ToString()); //getConstantName(PBTrainers,trainerid) rescue null;
				if (pbResolveBitmap(tgraphic) == null) tgraphic=string.Format("Graphics/Transitions/vsTrainer%d",trainerid);
				if (pbResolveBitmap(tbargraphic) != null && pbResolveBitmap(tgraphic) != null) {
					int outfit=Trainer != null ? Trainer.outfit??0 : 0;
					//  Set up
					IViewport viewplayer=new Viewport(0,Graphics.height/3,Graphics.width/2,128);
					//viewplayer.z=viewport.z;
					IViewport viewopp=new Viewport(Graphics.width/2,Graphics.height/3,Graphics.width/2,128);
					viewopp.z=viewport.z;
					IViewport viewvs=new Viewport(0,0,Graphics.width,Graphics.height);
					viewvs.z=viewport.z;
					double xoffset=(Graphics.width/2)/10;
					xoffset=Math.Round(xoffset);
					xoffset=xoffset*10;
					ISprite fade=new Sprite(viewport);
					fade.bitmap=BitmapCache.load_bitmap("Graphics/Transitions/vsFlash");
					fade.tone=new Tone(-255,-255,-255);
					fade.opacity=100;
					ISprite overlay=new Sprite(viewport);
					overlay.bitmap=new Bitmap(Graphics.width,Graphics.height);
					pbSetSystemFont(overlay.bitmap);
					ISprite bar1=new Sprite(viewplayer);
					string pbargraphic=string.Format("Graphics/Transitions/vsBar%s_%d",Trainer.trainertype,outfit); //getConstantName(PBTrainers,Trainer.trainertype) rescue null;
					if (pbResolveBitmap(pbargraphic) == null) pbargraphic=string.Format("Graphics/Transitions/vsBar%d_%d",Trainer.trainertype,outfit);
					if (pbResolveBitmap(pbargraphic) == null) {
						pbargraphic=string.Format("Graphics/Transitions/vsBar%s",Trainer.trainertype); //getConstantName(PBTrainers,Trainer.trainertype) rescue null;
					}
					if (pbResolveBitmap(pbargraphic) == null) pbargraphic=string.Format("Graphics/Transitions/vsBar%d",Trainer.trainertype);
					bar1.bitmap=BitmapCache.load_bitmap(pbargraphic);
					bar1.x=-xoffset;
					ISprite bar2=new Sprite(viewopp);
					bar2.bitmap=BitmapCache.load_bitmap(tbargraphic);
					bar2.x=xoffset;
					ISprite vs=new Sprite(viewvs);
					vs.bitmap=BitmapCache.load_bitmap("Graphics/Transitions/vs");
					vs.ox=vs.bitmap.width/2;
					vs.oy=vs.bitmap.height/2;
					vs.x=Graphics.width/2;
					vs.y=Graphics.height/1.5;
					vs.visible=false;
					ISprite flash=new Sprite(viewvs);
					flash.bitmap=BitmapCache.load_bitmap("Graphics/Transitions/vsFlash");
					flash.opacity=0;
					//  Animation
					int i = 0; do { //10.times do;
						bar1.x+=xoffset/10;
						bar2.x-=xoffset/10;
						pbWait(1); i++;
					} while (i < 10);
					pbSEPlay("Flash2");
					pbSEPlay("Sword2");
					flash.opacity=255;
					bar1.dispose();
					bar2.dispose();
					bar1=new AnimatedPlane(viewplayer);
					bar1.bitmap=BitmapCache.load_bitmap(pbargraphic);
					ISprite player=new Sprite(viewplayer);
					string pgraphic=string.Format("Graphics/Transitions/vsTrainer%s_%d",getConstantName(PBTrainers,Game.GameData.Trainer.trainertype),outfit) rescue null;
					if (pbResolveBitmap(pgraphic) == null) pgraphic=string.Format("Graphics/Transitions/vsTrainer%d_%d",Game.GameData.Trainer.trainertype,outfit);
					if (pbResolveBitmap(pgraphic) == null) {
						pgraphic=string.Format("Graphics/Transitions/vsTrainer%s",getConstantName(PBTrainers,Game.GameData.Trainer.trainertype)) rescue null;
					}
					if (pbResolveBitmap(pgraphic) == null) pgraphic=string.Format("Graphics/Transitions/vsTrainer%d",Game.GameData.Trainer.trainertype);
					player.bitmap=BitmapCache.load_bitmap(pgraphic);
					player.x=-xoffset;
					bar2=new AnimatedPlane(viewopp);
					bar2.bitmap=BitmapCache.load_bitmap(tbargraphic);
					trainer=new Sprite(viewopp);
					trainer.bitmap=BitmapCache.load_bitmap(tgraphic);
					trainer.x=xoffset;
					trainer.tone=new Tone(-255,-255,-255);
					i = 0; do { //25.times do;
						if (flash.opacity>0) flash.opacity-=51;
						bar1.ox-=16;
						bar2.ox+=16;
						pbWait(1); i++;
					} while (i < 25);
					i = 0; do { //11.times do;
						bar1.ox-=16;
						bar2.ox+=16;
						player.x+=xoffset/10;
						trainer.x-=xoffset/10;
						pbWait(1); i++;
					} while (i < 11);
					i = 0; do { //2.times do;
						bar1.ox-=16;
						bar2.ox+=16;
						player.x-=xoffset/20;
						trainer.x+=xoffset/20;
						pbWait(1); i++;
					} while (i < 2);
					i = 0; do { //10.times do;
						bar1.ox-=16;
						bar2.ox+=16;
						pbWait(1); i++;
					} while (i < 10);
					int val=2;
					flash.opacity=255;
					vs.visible=true;
					trainer.tone=new Tone(0,0,0);
					textpos=new {
						new { _INTL("{1}",Game.GameData.Trainer.name),Graphics.width/4,(Graphics.height/1.5)+10,2,
						new Color(248,248,248),new Color(12*6,12*6,12*6) },
						new { _INTL("{1}",trainername),(Graphics.width/4)+(Graphics.width/2),(Graphics.height/1.5)+10,2,
						new Color(248,248,248),new Color(12*6,12*6,12*6) };
					};
					pbDrawTextPositions(overlay.bitmap,textpos);
					pbSEPlay("Sword2");
					i = 0; do { //70.times do;
						bar1.ox-=16;
						bar2.ox+=16;
						if (flash.opacity>0) flash.opacity-=25.5;
						vs.x+=val;
						vs.y-=val;
						if (vs.x<=(Graphics.width/2)-2) val=2;
						if (vs.x>=(Graphics.width/2)+2) val=-2;
						pbWait(1); i++;
					} while (i < 70);
					i = 0; do { //30.times do;
						bar1.ox-=16;
						bar2.ox+=16;
						vs.zoom_x+=0.2;
						vs.zoom_y+=0.2;
						pbWait(1); i++;
					} while (i < 30);
					flash.tone=new Tone(-255,-255,-255);
					i = 0; do { //10.times do;
						bar1.ox-=16;
						bar2.ox+=16;
						flash.opacity+=25.5;
						pbWait(1); i++;
					} while (i < 10);
			//  }
					player.dispose();
					trainer.dispose();
					flash.dispose();
					vs.dispose();
					bar1.dispose();
					bar2.dispose();
					overlay.dispose();
					fade.dispose();
					viewvs.dispose();
					viewopp.dispose();
					viewplayer.dispose();
					viewport.color=new Color(0,0,0,255);
					return true;
				}
			}*/
			return false;
		}

		public void pbPrepareBattle(IBattle battle) {
			switch (GameScreen.weather_type) {
				case FieldWeathers.Rain: case FieldWeathers.HeavyRain: case FieldWeathers.Thunderstorm:
					battle.weather=Combat.Weather.RAINDANCE;
					battle.weatherduration=-1;
					break;
				case FieldWeathers.Snow: case FieldWeathers.Blizzard:
					battle.weather=Combat.Weather.HAIL;
					battle.weatherduration=-1;
					break;
				case FieldWeathers.Sandstorm:
					battle.weather=Combat.Weather.SANDSTORM;
					battle.weatherduration=-1;
					break;
				case FieldWeathers.Sunny:
					battle.weather=Combat.Weather.SUNNYDAY;
					battle.weatherduration=-1;
					break;
			}
			battle.shiftStyle=PokemonSystem.battlestyle==0;
			battle.battlescene=PokemonSystem.battlescene==0;
			battle.environment=pbGetEnvironment();
		}

		public Environments pbGetEnvironment() {
			if (GameMap == null) return Environments.None;
			if (Global != null && Global.diving) {
				return Environments.Underwater;
			} else if (PokemonEncounters != null && PokemonEncounters.isCave()) {
				return Environments.Cave;
			} else if (pbGetMetadata(GameMap.map_id,MapMetadatas.MetadataOutdoor) == null) {
				return Environments.None;
			} else {
				switch (GamePlayer.terrain_tag) {
					case Terrains.Grass:
						return Environments.Grass;       // Normal grass
					case Terrains.Sand:
						return Environments.Sand;
					case Terrains.Rock:
						return Environments.Rock;
					case Terrains.DeepWater:
						return Environments.MovingWater;
					case Terrains.StillWater:
						return Environments.StillWater;
					case Terrains.Water:
						return Environments.MovingWater;
					case Terrains.TallGrass:
						return Environments.TallGrass;   // Tall grass
					case Terrains.SootGrass:
						return Environments.Grass;       // Sooty tall grass
					case Terrains.Puddle:
						return Environments.StillWater;
				}
			}
			return Environments.None;
		}

		public IPokemon pbGenerateWildPokemon(Pokemons species,int level,bool isroamer=false) {
			Pokemon genwildpoke=new Monster.Pokemon(species,level: (byte)level);//,Trainer
			//Items items=genwildpoke.wildHoldItems;
			Items[] items=Kernal.PokemonItemsData[species] //ToDo: Return Items[3];
						.OrderByDescending(x => x.Rarirty)
						.Select(x => x.ItemId).ToArray();
			IPokemon firstpoke=Trainer.firstParty;
			int[] chances= new int[]{ 50,5,1 };
			if (firstpoke != null && !firstpoke.isEgg &&
				firstpoke.Ability == Abilities.COMPOUND_EYES) chances= new int[]{ 60,20,5 };
			int itemrnd=Core.Rand.Next(100);
			if (itemrnd<chances[0] || (items[0]==items[1] && items[1]==items[2])) {
				genwildpoke.setItem(items[0]);
			} else if (itemrnd<(chances[0]+chances[1])) {
				genwildpoke.setItem(items[1]);
			} else if (itemrnd<(chances[0]+chances[1]+chances[2])) {
				genwildpoke.setItem(items[2]);
			}
			if (Bag.pbQuantity(Items.SHINY_CHARM)>0) { //hasConst(PBItems,:SHINYCHARM) && 
				for (int i = 0; i < 2; i++) {	// 3 times as likely
					if (genwildpoke.IsShiny) break;
					//genwildpoke.PersonalId=Core.Rand.Next(65536)|(Core.Rand.Next(65536)<<16);
					genwildpoke.shuffleShiny();
				}
			}
			if (Core.Rand.Next(65536)<Core.POKERUSCHANCE) {
				genwildpoke.GivePokerus();
			}
			if (firstpoke != null && !firstpoke.isEgg) {
				if (firstpoke.Ability == Abilities.CUTE_CHARM &&
					!genwildpoke.IsSingleGendered) {
					if (firstpoke.IsMale) {
						if (Core.Rand.Next(3)<2) genwildpoke.makeFemale(); else genwildpoke.makeMale();
					} else if (firstpoke.IsFemale) {
						if (Core.Rand.Next(3)<2) genwildpoke.makeMale(); else genwildpoke.makeFemale();
					}
				} else if (firstpoke.Ability == Abilities.SYNCHRONIZE) {
					if (!isroamer && Core.Rand.Next(10)<5) genwildpoke.setNature(firstpoke.Nature);
				}
			}
			//Events.onWildPokemonCreate.trigger(null,genwildpoke);
			//Events.OnWildPokemonCreateEventArgs eventArgs = new Events.OnWildPokemonCreateEventArgs()
			//{
			//  Pokemon = genwildpoke
			//};
			//Events.OnWildPokemonCreate?.Invoke(null,eventArgs);
			return genwildpoke;
		}

		public Combat.BattleResults pbWildBattle(Pokemons species,int level,int? variable=null,bool canescape=true,bool canlose=false) {
			if ((Input.press(Input.CTRL) && Core.DEBUG) || Trainer.pokemonCount==0) {
				if (Trainer.pokemonCount>0 && this is IGameMessage m) {
					m.pbMessage(Game._INTL("SKIPPING BATTLE..."));
				}
				pbSet(variable,1);
				Global.nextBattleBGM=null;
				Global.nextBattleME=null;
				Global.nextBattleBack=null;
				return Combat.BattleResults.WON; //true;
			}
			//if (species is String || species is Symbol) {
			//  species=getID(PBSpecies,species);
			//}
			bool?[] handled= new bool?[]{ null };
			//Events.onWildBattleOverride.trigger(null,species,level,handled);
			//Events.OnWildBattleOverride?.Invoke(null,species,level,handled);
			if (handled[0]!=null) {
				//return handled[0].Value;
				return handled[0].Value ? Combat.BattleResults.WON : Combat.BattleResults.ABORTED;
			}
			List<int> currentlevels=new List<int>();
			foreach (var i in Trainer.party) {
				currentlevels.Add(i.Level);
			}
			IPokemon genwildpoke=pbGenerateWildPokemon(species,level);
			//Events.onStartBattle.trigger(null,genwildpoke);
			//Events.OnStartBattle.Invoke(null,genwildpoke);
			IPokeBattle_Scene scene=pbNewBattleScene();
			IBattle battle=new Combat.Battle(scene,Trainer.party,new IPokemon[] { genwildpoke },new ITrainer[] { Trainer },null);
			battle.internalbattle=true;
			battle.cantescape=!canescape;
			pbPrepareBattle(battle);
			Combat.BattleResults decision=0;
			pbBattleAnimation(pbGetWildBattleBGM(species), block: () => { 
				pbSceneStandby(() => {
				decision=battle.pbStartBattle(canlose);
				});
				foreach (var i in Trainer.party) { if (i is IPokemonMegaEvolution f) f.makeUnmega(); } //rescue null
				if (Global.partner != null) {
					pbHealAll();
					foreach (IPokemon i in Global.partner.party) { //partner[3]
						i.Heal();
						if (i is IPokemonMegaEvolution f) f.makeUnmega(); //rescue null
					}
				}
				if (decision==Combat.BattleResults.LOST || decision==Combat.BattleResults.DRAW) {		// If loss or draw
					if (canlose) {
						foreach (var i in Trainer.party) { i.Heal(); }
						for (int i = 0; i < 10; i++) {
							Graphics?.update();
						}
//					} else {
//						GameSystem.bgm_unpause();
//						GameSystem.bgs_unpause();
//						Game.pbStartOver();
					}
				}
				//Events.onEndBattle.trigger(null,decision,canlose);
			});
			Input.update();
			pbSet(variable,decision);
			//Events.onWildBattleEnd.trigger(null,species,level,decision);
			//Events.OnWildBattleEnd.Invoke(null,species,level,decision);
			return decision; //!=Combat.BattleResults.LOST;
		}

		public Combat.BattleResults pbDoubleWildBattle(Pokemons species1,int level1,Pokemons species2,int level2,int? variable=null,bool canescape=true,bool canlose=false) {
			if ((Input.press(Input.CTRL) && Core.DEBUG) || Trainer.pokemonCount==0) {
				if (Trainer.pokemonCount>0 && this is IGameMessage m) {
					m.pbMessage(Game._INTL("SKIPPING BATTLE..."));
				}
				pbSet(variable,1);
				Global.nextBattleBGM=null;
				Global.nextBattleME=null;
				Global.nextBattleBack=null;
				return Combat.BattleResults.WON; //true;
			}
			//if (species1 is String || species1 is Symbol) {
			//  species1=getID(PBSpecies,species1);
			//}
			//if (species2 is String || species2 is Symbol) {
			//  species2=getID(PBSpecies,species2);
			//}
			List<int> currentlevels=new List<int>();
			foreach (var i in Trainer.party) {
				currentlevels.Add(i.Level);
			}
			IPokemon genwildpoke=pbGenerateWildPokemon(species1,level1);
			IPokemon genwildpoke2=pbGenerateWildPokemon(species2,level2);
			//Events.onStartBattle.trigger(null,genwildpoke);
			//Events.OnStartBattle.Invoke(null,genwildpoke);
			IPokeBattle_Scene scene=pbNewBattleScene();
			Combat.Battle battle;
			if (Global.partner != null) {
			ITrainer othertrainer=new Trainer(
				Global.partner.trainerTypeName,Global.partner.trainertype);//[1]|[0]
			othertrainer.id=Global.partner.id;//[2]
			othertrainer.party=Global.partner.party;//[3]
			IList<IPokemon> combinedParty=new List<IPokemon>();
			for (int i = 0; i < Trainer.party.Length; i++) {
				combinedParty[i]=Trainer.party[i];
			}
			for (int i = 0; i < othertrainer.party.Length; i++) {
				combinedParty[6+i]=othertrainer.party[i];
			}
			battle=new Combat.Battle(scene,combinedParty.ToArray(),new IPokemon[] { genwildpoke, genwildpoke2 },
				new ITrainer[] { Trainer,othertrainer },null);
			battle.fullparty1=true;
			} else {
			battle=new Combat.Battle(scene,Trainer.party,new IPokemon[] { genwildpoke, genwildpoke2 },
				new ITrainer[] { Trainer },null);
			battle.fullparty1=false;
			}
			battle.internalbattle=true;
			battle.doublebattle=battle.pbDoubleBattleAllowed();
			battle.cantescape=!canescape;
			pbPrepareBattle(battle);
			Combat.BattleResults decision=0;
			pbBattleAnimation(pbGetWildBattleBGM(species1), block: () => { 
				pbSceneStandby(() => {
					decision=battle.pbStartBattle(canlose);
				});
				foreach (var i in Trainer.party) { if (i is IPokemonMegaEvolution f) f.makeUnmega(); } //rescue null
				if (Global.partner != null) {
					pbHealAll();
					foreach (Monster.Pokemon i in Global.partner.party) {//[3]
						i.Heal();
						i.makeUnmega(); //rescue null
					}
				}
				if (decision==Combat.BattleResults.LOST || decision==Combat.BattleResults.DRAW) {
					if (canlose) {
						foreach (var i in Trainer.party) { i.Heal(); }
						for (int i = 0; i < 10; i++) {
							Graphics?.update();
						}
//					} else {
//						GameSystem.bgm_unpause();
//						GameSystem.bgs_unpause();
//						Game.pbStartOver();
					}
				}
				//Events.onEndBattle.trigger(null,decision,canlose);
			});
			Input.update();
			pbSet(variable,decision);
			return decision; //!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW;
		}

		public void pbCheckAllFainted() {
			if (pbAllFainted() && this is IGameMessage m) {
				m.pbMessage(Game._INTL(@"{1} has no usable Pokémon!\1",Trainer.name));
				m.pbMessage(Game._INTL("{1} blacked out!",Trainer.name));
				if (this is IGameAudioPlay a) a.pbBGMFade(1.0f);
				if (this is IGameAudioPlay a1) a1.pbBGSFade(1.0f);
				pbFadeOutIn(99999, block: () => {
					pbStartOver();
				});
			}
		}

		public void pbEvolutionCheck(int[] currentlevels) {
			//  Check conditions for evolution
			for (int i = 0; i < currentlevels.Length; i++) {
				IPokemon pokemon=Trainer.party[i];
				if (pokemon.IsNotNullOrNone() && (currentlevels[i] == null || pokemon.Level!=currentlevels[i])) {
					Pokemons newspecies=EvolutionHelper.pbCheckEvolution(pokemon)[0];
					if (newspecies>0) {
						//  Start evolution scene
						IPokemonEvolutionScene evo=Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution();
						evo.pbEndScreen();
					}
				}
			}
		}

		public Items[] pbDynamicItemList(params Items[] args) {
			List<Items> ret=new List<Items>();
			for (int i = 0; i < args.Length; i++) {
				//if (hasConst(PBItems,args[i])) {
				//  ret.Add(getConst(PBItems,args[i].to_sym));
					ret.Add(args[i]);
				//}
			}
			return ret.ToArray();
		}

		/// <summary>
		/// Runs the Pickup event after a battle if a Pokemon has the ability Pickup.
		/// </summary>
		/// <param name="pokemon"></param>
		public void pbPickup(IPokemon pokemon) {
			if (pokemon.Ability != Abilities.PICKUP || pokemon.isEgg) return;
			if (pokemon.Item!=0) return;
			if (Core.Rand.Next(10)!=0) return;
			Items[] pickupList=new Items[] {
				Items.POTION,
				Items.ANTIDOTE,
				Items.SUPER_POTION,
				Items.GREAT_BALL,
				Items.REPEL,
				Items.ESCAPE_ROPE,
				Items.FULL_HEAL,
				Items.HYPER_POTION,
				Items.ULTRA_BALL,
				Items.REVIVE,
				Items.RARE_CANDY,
				Items.SUN_STONE,
				Items.MOON_STONE,
				Items.HEART_SCALE,
				Items.FULL_RESTORE,
				Items.MAX_REVIVE,
				Items.PP_UP,
				Items.MAX_ELIXIR
			};
			Items[] pickupListRare=new Items[] {
				Items.HYPER_POTION,
				Items.NUGGET,
				Items.KINGS_ROCK,
				Items.FULL_RESTORE,
				Items.ETHER,
				Items.IRON_BALL,
				Items.DESTINY_KNOT,
				Items.ELIXIR,
				Items.DESTINY_KNOT,
				Items.LEFTOVERS,
				Items.DESTINY_KNOT
			};
			if (pickupList.Length!=18) return;
			if (pickupListRare.Length!=11) return;
			int[] randlist= new int[]{ 30,10,10,10,10,10,10,4,4,1,1 };
			List<Items> items=new List<Items>();
			int plevel=Math.Min(100,pokemon.Level);
			int itemstart=(plevel-1)/10;
			if (itemstart<0) itemstart=0;
			for (int i = 0; i < 9; i++) {
				items.Add(pickupList[itemstart+i]);
			}
			items.Add(pickupListRare[itemstart]);
			items.Add(pickupListRare[itemstart+1]);
			int rnd=Core.Rand.Next(100);
			int cumnumber=0;
			for (int i = 0; i < 11; i++) {
				cumnumber+=randlist[i];
				if (rnd<cumnumber) {
					pokemon.setItem(items[i]);
					break;
				}
			}
		}

		public bool pbEncounter(Method enctype) {
			if (Global.partner != null) {
				IEncounter encounter1=PokemonEncounters.pbEncounteredPokemon(enctype);
				if (!encounter1.IsNotNullOrNone()) return false;
				IEncounter encounter2=PokemonEncounters.pbEncounteredPokemon(enctype);
				if (!encounter2.IsNotNullOrNone()) return false;
				if (PokemonTemp is ITempMetadataField f0) f0.encounterType=enctype;
				pbDoubleWildBattle(encounter1.Pokemon,encounter1.Level,encounter2.Pokemon,encounter2.Level); //[0]|[1]
				if (PokemonTemp is ITempMetadataField f1) f1.encounterType=null;
				return true;
			} else {
				IEncounter encounter=PokemonEncounters.pbEncounteredPokemon(enctype);
				if (!encounter.IsNotNullOrNone()) return false;
				if (PokemonTemp is ITempMetadataField f0) f0.encounterType=enctype;
				pbWildBattle(encounter.Pokemon,encounter.Level); //[0]|[1]
				if (PokemonTemp is ITempMetadataField f1) f1.encounterType=null;
				return true;
			}
		}

		//Events.onStartBattle+=delegate(object sender, EventArgs e) {
		//  PokemonTemp.evolutionLevels=new int[6];
		//  for (int i = 0; i < Trainer.party.Length; i++) {
		//    PokemonTemp.evolutionLevels[i]=Trainer.party[i].Level;
		//  }
		//}
		//
		//Events.onEndBattle+=delegate(object sender, EventArgs e) {
		//  int decision=e[0];
		//  bool canlose=e[1];
		//  if (Core.USENEWBATTLEMECHANICS || (decision!=2 && decision!=5)) {		// not a loss or a draw
		//    if (PokemonTemp.evolutionLevels) {
		//      pbEvolutionCheck(PokemonTemp.evolutionLevels);
		//      PokemonTemp.evolutionLevels=null;
		//    }
		//  }
		//  if (decision==1) {
		//    foreach (Pokemon pkmn in Trainer.party) {
		//      Game.pbPickup(pkmn);
		//      if (pkmn.Ability == Abilities.HONEYGATHER && !pkmn.isEgg && !pkmn.hasItem) {
		//        //if (hasConst(PBItems,:HONEY)) {
		//          int chance = 5 + Math.Floor((pkmn.Level-1)/10)*5;
		//          if (Core.Rand.Next(100)<chance) pkmn.setItem(Items.HONEY);
		//        //}
		//      }
		//    }
		//  }
		//  if ((decision==2 || decision==5) && !canlose) {
		//    GameSystem.bgm_unpause();
		//    GameSystem.bgs_unpause();
		//    Game.pbStartOver();
		//  }
		//}

		#region Scene_Map and Spriteset_Map
		public partial class Scene_Map {
			public void createSingleSpriteset(int map) {
				//temp=Game.GameData.Scene.spriteset().getAnimations();
				//@spritesets[map]=new Spriteset_Map(MapFactory.maps[map]);
				//Game.GameData.Scene.spriteset().restoreAnimations(temp);
				//MapFactory.setSceneStarted(this);
				//updateSpritesets();
			}
		}

		public partial class Spriteset_Map {
			private int usersprites;
			public int getAnimations() {
				return @usersprites;
			}

			public void restoreAnimations(int anims) {
				@usersprites=anims;
			}
		}

		//Events.onSpritesetCreate+=delegate(object sender, EventArgs e) {
		//  spriteset=e[0]; // Spriteset being created
		//  viewport=e[1]; // Viewport used for tilemap and characters
		//  map=spriteset.map; // Map associated with the spriteset (not necessarily the current map).
		//  foreach (var i in map.events.keys) {
		//    if (map.events[i].name[/^OutdoorLight\((\w+)\)$/]) {
		//      filename=$~[1].ToString();
		//      spriteset.addUserSprite(new LightEffect_DayNight(map.events[i],viewport,map,filename));
		//    } else if (map.events[i].name=="OutdoorLight") {
		//      spriteset.addUserSprite(new LightEffect_DayNight(map.events[i],viewport,map));
		//    } else if (map.events[i].name[/^Light\((\w+)\)$/]) {
		//      filename=$~[1].ToString();
		//      spriteset.addUserSprite(new LightEffect_Basic(map.events[i],viewport,map,filename));
		//    } else if (map.events[i].name=="Light") {
		//      spriteset.addUserSprite(new LightEffect_Basic(map.events[i],viewport,map));
		//    }
		//  }
		//  spriteset.addUserSprite(new Particle_Engine(viewport,map));
		//}

		public void pbOnSpritesetCreate(ISpritesetMap spriteset,IViewport viewport) {
			//Events.onSpritesetCreate.trigger(null,spriteset,viewport);
			//Events.OnSpritesetCreate.Invoke(null,spriteset,viewport);
		}
		#endregion

		#region Field movement
		public bool pbLedge(float xOffset,float yOffset) {
			if (Terrain.isLedge(pbFacingTerrainTag())) {
				if (pbJumpToward(2,true)) {
					if (Scene.spriteset() is ISpritesetMapAnimation s) s.addUserAnimation(Core.DUST_ANIMATION_ID,GamePlayer.x,GamePlayer.y,true);
					GamePlayer.increase_steps();
					GamePlayer.check_event_trigger_here(new int[] { 1, 2 });
				}
				return true;
			}
			return false;
		}

		public void pbSlideOnIce(IGamePlayer @event=null) {
			if (@event == null) @event=GamePlayer;
			if (@event == null) return;
			if (!Terrain.isIce(pbGetTerrainTag(@event))) return;
			Global.sliding=true;
			int direction=@event.direction;
			bool oldwalkanime=@event.walk_anime;
			@event.straighten();
			@event.pattern=1;
			@event.walk_anime=false;
			do { //;loop
				if (!@event.passable(@event.x,@event.y,direction)) break;
				if (!Terrain.isIce(pbGetTerrainTag(@event))) break;
				@event.move_forward();
				while (@event.moving) {
					Graphics?.update();
					Input.update();
					if (this is IGameMessage s) s.pbUpdateSceneMap();
				}
			} while (true);
			@event.center(@event.x,@event.y);
			@event.straighten();
			@event.walk_anime=oldwalkanime;
			Global.sliding=false;
		}

		// Poison event on each step taken
		//Events.onStepTakenTransferPossible+=delegate(object sender, EventArgs e) {
		//  handled=e[0];
		//  if (handled[0]) continue;
		//  if (Global.stepcount % 4 == 0 && POISONINFIELD) {
		//    flashed=false;
		//    foreach (Pokemon i in Trainer.party) {
		//      if (i.status==Statuses.POISON && i.HP>0 && !i.isEgg? &&
		//         !i.Ability == Abilities.IMMUNITY) {
		//        if (!flashed) {
		//          GameScreen.start_flash(new Color(255,0,0,128), 4);
		//          flashed=true;
		//        }
		//        if (i.HP==1 && !POISONFAINTINFIELD && this is IGameMessage m0) {
		//          i.status=0;
		//          m0.pbMessage(Game._INTL("{1} survived the poisoning.\\nThe poison faded away!\\1",i.name));
		//          continue;
		//        }
		//        i.HP-=1;
		//        if (i.HP==1 && !POISONFAINTINFIELD && this is IGameMessage m1) {
		//          i.status=0;
		//          m1.pbMessage(Game._INTL("{1} survived the poisoning.\\nThe poison faded away!\\1",i.name));
		//        }
		//        if (i.HP==0 && this is IGameMessage m2) {
		//          i.changeHappiness("faint");
		//          i.status=0;
		//          m2.pbMessage(Game._INTL("{1} fainted...\\1",i.name));
		//        }
		//        if (pbAllFainted) handled[0]=true;
		//        pbCheckAllFainted();
		//      }
		//    }
		//  }
		//}
		//
		//Events.onStepTaken+=proc{
		//  if (!Global.happinessSteps) Global.happinessSteps=0;
		//  Global.happinessSteps+=1;
		//  if (Global.happinessSteps==128) {
		//    foreach (var pkmn in Trainer.party) {
		//      if (pkmn.HP>0 && !pkmn.isEgg?) {
		//        if (Core.Rand.Next(2)==0) pkmn.changeHappiness("walking");
		//      }
		//    }
		//    Global.happinessSteps=0;
		//  }
		//}
		//
		//Events.onStepTakenFieldMovement+=delegate(object sender, EventArgs e) {
		//  @event=e[0]; // Get the event affected by field movement
		//  thistile=MapFactory.getRealTilePos(@event.map.map_id,@event.x,@event.y);
		//  map=MapFactory.getMap(thistile[0]);
		//  sootlevel=-1;
		//  foreach (var i in [2, 1, 0]) {
		//    tile_id = map.data[thistile[1],thistile[2],i];
		//    if (tile_id == null) continue;
		//    if (map.terrain_tags[tile_id] &&
		//       map.terrain_tags[tile_id]==Terrains.SootGrass) {
		//      sootlevel=i;
		//      break;
		//    }
		//  }
		//  if (sootlevel>=0 && hasConst(PBItems,:SOOTSACK)) {
		//    if (!Global.sootsack) Global.sootsack=0;
		//    //map.data[thistile[1],thistile[2],sootlevel]=0
		//    if (@event==GamePlayer && Bag.pbQuantity(:SOOTSACK)>0) {
		//      Global.sootsack+=1;
		//    }
		//    //Scene.createSingleSpriteset(map.map_id)
		//  }
		//}
		//
		//Events.onStepTakenFieldMovement+=delegate(object sender, EventArgs e) {
		//  @event=e[0]; // Get the event affected by field movement
		//  if (Scene is Scene_Map) {
		//    currentTag=pbGetTerrainTag(@event);
		//    if (Terrain.isJustGrass(pbGetTerrainTag(@event,true))) {		// Won't show if under bridge
		//      Scene.spriteset.addUserAnimation(GRASS_ANIMATION_ID,@event.x,@event.y,true);
		//    } else if (@event==GamePlayer && currentTag==Terrains.WaterfallCrest) {
		//      //Descend waterfall, but only if this event is the player
		//      Game.pbDescendWaterfall(@event);
		//    } else if (@event==GamePlayer && Terrain.isIce(currentTag) && !Global.sliding) {
		//      Game.pbSlideOnIce(@event);
		//    }
		//  }
		//}

		public void pbBattleOnStepTaken() {
			if (Trainer.party.Length>0) {
				Method? encounterType=PokemonEncounters.pbEncounterType();
				if (encounterType>=0) {
					if (PokemonEncounters.isEncounterPossibleHere()) {
						IEncounter encounter=PokemonEncounters.pbGenerateEncounter(encounterType.Value);
						encounter=EncounterModifier.trigger(encounter);
						if (PokemonEncounters.pbCanEncounter(encounter)) {
							if (Global.partner != null) {
							IEncounter encounter2=PokemonEncounters.pbEncounteredPokemon(encounterType.Value);
								pbDoubleWildBattle(encounter.Pokemon,encounter.Level,encounter2.Pokemon,encounter2.Level);
							} else {
								pbWildBattle(encounter.Pokemon,encounter.Level);
							}
						}
						EncounterModifier.triggerEncounterEnd();
					}
				}
			}
		}

		public void pbOnStepTaken(bool eventTriggered) {
			if (GamePlayer.move_route_forcing || (this is IGameMessage g && g.pbMapInterpreterRunning()) || Trainer == null) {
			//  if forced movement or if no trainer was created yet
			//Events.onStepTakenFieldMovement.trigger(null,GamePlayer);
			return;
			}
			//if (Global.stepcount == null) Global.stepcount=0;
			Global.stepcount+=1;
			Global.stepcount&=0x7FFFFFFF;
			//Events.onStepTaken.trigger(null);
			//  Events.onStepTakenFieldMovement.trigger(null,GamePlayer)
			bool?[] handled= new bool?[]{ null };
			//Events.OnStepTakenTransferPossible.trigger(null,handled);
			if (handled[0]==true) return;
			if (!eventTriggered) {
			pbBattleOnStepTaken();
			}
		}

		// This method causes a lot of lag when the game is encrypted
		public string pbGetPlayerCharset(MetadataPlayer meta,int charset,ITrainer trainer=null) {
			if (trainer == null) trainer=Trainer;
			int outfit=trainer != null ? trainer.outfit.Value : 0;
			string ret=meta[charset];
			if (ret == null || ret=="") ret=meta[1];
		//  if FileTest.image_exist("Graphics/Characters/"+ret+"_"+outfit.ToString())
			if (pbResolveBitmap("Graphics/Characters/"+ret+"_"+outfit.ToString()) != null) {
			ret=ret+"_"+outfit.ToString();
			}
			return ret;
		}

		public void pbUpdateVehicle() {
			//string[] meta=(string[])Game.pbGetMetadata(0, GlobalMetadatas.MetadataPlayerA+Global.playerID);
			MetadataPlayer meta=pbGetMetadata(0).Global.Players[Global.playerID];
			//if (meta != null) {
				if (Global.diving) {
					GamePlayer.character_name=pbGetPlayerCharset(meta,5); // Diving graphic
				} else if (Global.surfing) {
					GamePlayer.character_name=pbGetPlayerCharset(meta,3); // Surfing graphic
				} else if (Global.bicycle) {
					GamePlayer.character_name=pbGetPlayerCharset(meta,2); // Bicycle graphic
				} else {
					GamePlayer.character_name=pbGetPlayerCharset(meta,1); // Regular graphic
				}
			//}
		}

		public void pbCancelVehicles(int? destination=null) {
			Global.surfing=false;
			Global.diving=false;
			if (destination == null || !pbCanUseBike(destination.Value)) {
				Global.bicycle=false;
			}
			pbUpdateVehicle();
		}

		public bool pbCanUseBike (int mapid) {
			//if ((bool)pbGetMetadata(mapid,MapMetadatas.MetadataBicycleAlways)) return true;
			if (pbGetMetadata(mapid).Map.BicycleAlways) return true;
			//bool? val=(bool?)pbGetMetadata(mapid,MapMetadatas.MetadataBicycle);
			bool? val=pbGetMetadata(mapid).Map.Bicycle;
			//if (val==null) val=(bool?)pbGetMetadata(mapid,MapMetadatas.MetadataOutdoor);
			if (val==null) val=pbGetMetadata(mapid).Map.Outdoor;
			return val != null ? true : false;
		}

		public void pbMountBike() {
			if (Global.bicycle) return;
			Global.bicycle=true;
			pbUpdateVehicle();
			//IAudioObject bikebgm=(IAudioObject)pbGetMetadata(0,GlobalMetadatas.MetadataBicycleBGM);
			string bikebgm=pbGetMetadata(0).Global.BicycleBGM;
			if (bikebgm != null) {
				if (this is IGameField a) a.pbCueBGM(bikebgm,0.5f);
			}
		}

		public void pbDismountBike() {
			if (!Global.bicycle) return;
			Global.bicycle=false;
			pbUpdateVehicle();
			GameMap.autoplayAsCue();
		}

		public void pbSetPokemonCenter() {
			Global.pokecenterMapId=GameMap.map_id;
			Global.pokecenterX=GamePlayer.x;
			Global.pokecenterY=GamePlayer.y;
			Global.pokecenterDirection=GamePlayer.direction;
		}
		#endregion

		#region Fishing
		public void pbFishingBegin() {
			Global.fishing=true;
			if (!pbCommonEvent(Core.FISHINGBEGINCOMMONEVENT)) {
				int patternb = 2*GamePlayer.direction - 1;
				//TrainerTypes playertrainer=pbGetPlayerTrainerType();
				//string[] meta=(string[])pbGetMetadata(0,GlobalMetadatas.MetadataPlayerA+Global.playerID);
				MetadataPlayer meta=pbGetMetadata(0).Global.Players[Global.playerID];
				int num=(Global.surfing) ? 7 : 6;
				//if (meta != null && meta[num]!=null && meta[num]!="") {
					string charset=pbGetPlayerCharset(meta,num);
					int pattern = 0; do { //4.times |pattern|
						if (GamePlayer is IGamePlayerRunMovement p) p.setDefaultCharName(charset,patternb-pattern);
						int i = 0; do { //;2.times 
							Graphics?.update();
							Input.update();
							if (this is IGameMessage a) a.pbUpdateSceneMap(); i++;
						} while (i < 2); pattern++;
					} while (pattern < 4);
				//}
			}
		}

		public void pbFishingEnd() {
			if (!pbCommonEvent(Core.FISHINGENDCOMMONEVENT)) {
				int patternb = 2*(GamePlayer.direction - 2);
				//TrainerTypes playertrainer=pbGetPlayerTrainerType();
				//string[] meta=(string[])pbGetMetadata(0,GlobalMetadatas.MetadataPlayerA+Global.playerID);
				MetadataPlayer meta=pbGetMetadata(0).Global.Players[Global.playerID];
				int num=(Global.surfing) ? 7 : 6;
				//if (meta != null && meta[num]!=null && meta[num]!="") {
					string charset=pbGetPlayerCharset(meta,num);
					int pattern = 0; do { //4.times |pattern|
						if (GamePlayer is IGamePlayerRunMovement p) p.setDefaultCharName(charset,patternb+pattern);
						int i = 0; do { //;2.times 
							Graphics?.update();
							Input.update();
							if (this is IGameMessage a) a.pbUpdateSceneMap(); i++;
						} while (i < 2); pattern++;
					} while (pattern < 4);
				//}
			}
			Global.fishing=false;
		}

		public bool pbFishing(bool hasencounter,int rodtype=1) {
			bool speedup=(Trainer.firstParty.IsNotNullOrNone() && !Trainer.firstParty.isEgg &&
				(Trainer.firstParty.Ability == Abilities.STICKY_HOLD ||
				Trainer.firstParty.Ability == Abilities.SUCTION_CUPS));
			float bitechance=20+(25*rodtype);   // 45, 70, 95
			if (speedup) bitechance*=1.5f;
			int hookchance=100;
			int oldpattern=GamePlayer is IGamePlayerRunMovement p ? p.fullPattern() : 0;
			pbFishingBegin();
			IWindow_AdvancedTextPokemon msgwindow=this is IGameMessage m ? m.pbCreateMessageWindow() : null;
			do { //;loop
				int time=2+Core.Rand.Next(10);
				if (speedup) time=Math.Min(time,2+Core.Rand.Next(10));
				string message="";
				int i = 0; do { //;time.times 
					message+=". "; i++;
				} while (i < time);
				if (pbWaitMessage(msgwindow,time)) {
					pbFishingEnd();
					if (GamePlayer is IGamePlayerRunMovement p0) p0.setDefaultCharName(null,oldpattern);
					if (this is IGameMessage m0) m0.pbMessageDisplay(msgwindow,Game._INTL("Not even a nibble..."));
					if (this is IGameMessage m1) m1.pbDisposeMessageWindow(msgwindow);
					return false;
				}
				if (Core.Rand.Next(100)<bitechance && hasencounter) {
					int frames=Core.Rand.Next(21)+20;
					if (!pbWaitForInput(msgwindow,message+Game._INTL("\r\nOh! A bite!"),frames)) {
						pbFishingEnd();
						if (GamePlayer is IGamePlayerRunMovement p0) p0.setDefaultCharName(null,oldpattern);
						if (this is IGameMessage m0) m0.pbMessageDisplay(msgwindow,Game._INTL("The Pokémon got away..."));
						if (this is IGameMessage m1) m1.pbDisposeMessageWindow(msgwindow);
						return false;
					}
					if (Core.Rand.Next(100)<hookchance || Core.FISHINGAUTOHOOK) {
					if (this is IGameMessage m0) m0.pbMessageDisplay(msgwindow,Game._INTL("Landed a Pokémon!"));
					if (this is IGameMessage m1) m1.pbDisposeMessageWindow(msgwindow);
					pbFishingEnd();
					if (GamePlayer is IGamePlayerRunMovement p0) p0.setDefaultCharName(null,oldpattern);
						return true;
					}
			//      bitechance+=15
			//      hookchance+=15
				} else {
					pbFishingEnd();
					if (GamePlayer is IGamePlayerRunMovement p0) p0.setDefaultCharName(null,oldpattern);
					if (this is IGameMessage m0) m0.pbMessageDisplay(msgwindow,Game._INTL("Not even a nibble..."));
					if (this is IGameMessage m1) m1.pbDisposeMessageWindow(msgwindow);
					return false;
				}
			} while (true);
			if (this is IGameMessage m2) m2.pbDisposeMessageWindow(msgwindow);
			return false;
		}

		public bool pbWaitForInput(IWindow msgwindow,string message,int frames) {
			if (Core.FISHINGAUTOHOOK) return true;
			if (this is IGameMessage m0) m0.pbMessageDisplay(msgwindow,message,false);
			int i = 0; do { //;frames.times 
				Graphics?.update();
				Input.update();
				if (this is IGameMessage m1) m1.pbUpdateSceneMap();
				if (Input.trigger((int)Input.C) || Input.trigger((int)Input.B)) {
					return true;
				} i++;
			} while (i < frames);
			return false;
		}

		public bool pbWaitMessage(IWindow msgwindow,int time) {
			string message="";
			int i = 0; do { //(time+1).times |i|
				if (i>0) message+=". ";
				if (this is IGameMessage m0) m0.pbMessageDisplay(msgwindow,message,false);
				int j = 0; do { //20.times ;
					Graphics?.update();
					Input.update();
					if (this is IGameMessage m1) m1.pbUpdateSceneMap();
					if (Input.trigger((int)Input.C) || Input.trigger((int)Input.B)) {
						return true;
					} j++;
				} while (j < 20); i++;
			} while (i < time+1);
			return false;
		}
		#endregion

		#region Moving between maps
		//Events.onMapChange+=delegate(object sender, EventArgs e) {
		//  oldid=e[0]; // previous map ID, 0 if no map ID
		//  healing=pbGetMetadata(GameMap.map_id,MetadataHealingSpot);
		//  if (healing) Global.healingSpot=healing;
		//  if (PokemonMap) PokemonMap.clear;
		//  if (PokemonEncounters) PokemonEncounters.setup(GameMap.map_id);
		//  Global.visitedMaps[GameMap.map_id]=true;
		//  if (oldid!=0 && oldid!=GameMap.map_id) {
		//    mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
		//    weather=pbGetMetadata(GameMap.map_id,MetadataWeather);
		//    if (GameMap.name!=mapinfos[oldid].name) {
		//      if (weather && Core.Rand.Next(100)<weather[1]) GameScreen.weather(weather[0],8,20);
		//    } else {
		//      oldweather=pbGetMetadata(oldid,MetadataWeather);
		//      if (weather && !oldweather && Core.Rand.Next(100)<weather[1]) GameScreen.weather(weather[0],8,20);
		//    }
		//  }
		//}
		//
		//Events.onMapChanging+=delegate(object sender, EventArgs e) {
		//  newmapID=e[0];
		//  newmap=e[1];
		////  Undo the weather (GameMap still refers to the old map)
		//  mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
		//  if (newmapID>0) {
		//    oldweather=pbGetMetadata(GameMap.map_id,MetadataWeather);
		//    if (GameMap.name!=mapinfos[newmapID].name) {
		//      if (oldweather) GameScreen.weather(0,0,0);
		//    } else {
		//      newweather=pbGetMetadata(newmapID,MetadataWeather);
		//      if (oldweather && !newweather) GameScreen.weather(0,0,0);
		//    }
		//  }
		//}
		//
		//Events.onMapSceneChange+=delegate(object sender, EventArgs e) {
		//  scene=e[0];
		//  mapChanged=e[1];
		//  if (!scene || !scene.spriteset) return;
		//  if (GameMap) {
		//    lastmapdetails=Global.mapTrail[0] ?;
		//       pbGetMetadata(Global.mapTrail[0],MetadataMapPosition) : [-1,0,0];
		//    if (!lastmapdetails) lastmapdetails= new []{ -1,0,0 };
		//    newmapdetails=GameMap.map_id ?;
		//       pbGetMetadata(GameMap.map_id,MetadataMapPosition) : [-1,0,0];
		//    if (!newmapdetails) newmapdetails= new []{ -1,0,0 };
		//    if (!Global.mapTrail) Global.mapTrail=[];
		//    if (Global.mapTrail[0]!=GameMap.map_id) {
		//      if (Global.mapTrail[2]) Global.mapTrail[3]=Global.mapTrail[2];
		//      if (Global.mapTrail[1]) Global.mapTrail[2]=Global.mapTrail[1];
		//      if (Global.mapTrail[0]) Global.mapTrail[1]=Global.mapTrail[0];
		//    }
		//    Global.mapTrail[0]=GameMap.map_id;   // Update map trail
		//  }
		//  darkmap=pbGetMetadata(GameMap.map_id,MetadataDarkMap);
		//  if (darkmap) {
		//    if (Global.flashUsed) {
		//      PokemonTemp.darknessSprite=new DarknessSprite();
		//      scene.spriteset.addUserSprite(PokemonTemp.darknessSprite);
		//      darkness=PokemonTemp.darknessSprite;
		//      darkness.radius=176;
		//    } else {
		//      PokemonTemp.darknessSprite=new DarknessSprite();
		//      scene.spriteset.addUserSprite(PokemonTemp.darknessSprite);
		//    }
		//  } else if (!darkmap) {
		//    Global.flashUsed=false;
		//    if (PokemonTemp.darknessSprite) {
		//      PokemonTemp.darknessSprite.dispose();
		//      PokemonTemp.darknessSprite=null;
		//    }
		//  }
		//  if (mapChanged) {
		//    if (pbGetMetadata(GameMap.map_id,MetadataShowArea)) {
		//      nosignpost=false;
		//      if (Global.mapTrail[1]) {
		//        for (int i = 0; i < NOSIGNPOSTS.Length; i++) {/2
		//          if (NOSIGNPOSTS[2*i]==Global.mapTrail[1] && NOSIGNPOSTS[2*i+1]==GameMap.map_id) /nosignpost=true;
		//          if (NOSIGNPOSTS[2*i+1]==Global.mapTrail[1] && NOSIGNPOSTS[2*i]==GameMap.map_id) /nosignpost=true;
		//          if (nosignpost) break;
		//        }
		//        mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
		//        oldmapname=mapinfos[Global.mapTrail[1]].name;
		//        if (GameMap.name==oldmapname) nosignpost=true;
		//      }
		//      if (!nosignpost) scene.spriteset.addUserSprite(new LocationWindow(GameMap.name));
		//    }
		//  }
		//  if (pbGetMetadata(GameMap.map_id,MetadataBicycleAlways)) {
		//    Game.pbMountBike;
		//  } else {
		//    if (!pbCanUseBike(GameMap.map_id)) {
		//      Game.pbDismountBike;
		//    }
		//  }
		//}

		public void pbStartOver(bool gameover=false) {
			if (this is IGameBugContest c && c.pbInBugContest) {
				c.pbBugContestStartOver();
				return;
			}
			pbHealAll();
			if (Global.pokecenterMapId != null && Global.pokecenterMapId>= 0) {
				if (gameover) {
					if (this is IGameMessage m) m.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]After the unfortunate defeat, {1} scurried to a Pokémon Center.",Trainer.name));
				} else {
					if (this is IGameMessage m) m.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]{1} scurried to a Pokémon Center, protecting the exhausted and fainted Pokémon from further harm.",Trainer.name));
				}
				pbCancelVehicles();
				if (this is IGameDependantEvents d) d.pbRemoveDependencies();
				GameSwitches[Core.STARTING_OVER_SWITCH]=true; //ToDo: Use GameData.Feature
				Features.StartingOver();
				GameTemp.player_new_map_id=Global.pokecenterMapId;
				GameTemp.player_new_x=Global.pokecenterX;
				GameTemp.player_new_y=Global.pokecenterY;
				GameTemp.player_new_direction=Global.pokecenterDirection;
				if (Scene is ISceneMap s) s.transfer_player();
				GameMap.refresh();
			} else {
				//int[] homedata=(int[])pbGetMetadata(0,GlobalMetadatas.MetadataHome);
				MetadataPosition? homedata=pbGetMetadata(0).Global.Home;
				//Overworld.TilePosition homedata=(int[])Game.pbGetMetadata(0,GlobalMetadatas.MetadataHome);
				if (homedata != null && !pbRxdataExists(string.Format("Data/Map%03d",homedata.Value.MapId))) { //homedata[0]
					if (Core.DEBUG && this is IGameMessage m) {
						m.pbMessage(string.Format("Can't find the map 'Map{0}' in the Data folder. The game will resume at the player's position.",homedata.Value.MapId)); //homedata[0]
					}
					pbHealAll();
					return;
				}
				if (gameover) {
					if (this is IGameMessage m) m.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]After the unfortunate defeat, {1} scurried home.",Trainer.name));
				} else {
					if (this is IGameMessage m) m.pbMessage(Game._INTL("\\w[]\\wm\\c[8]\\l[3]{1} scurried home, protecting the exhausted and fainted Pokémon from further harm.",Trainer.name));
				}
				if (homedata != null) {
					pbCancelVehicles();
					if (this is IGameDependantEvents d) d.pbRemoveDependencies();
					//GameSwitches[STARTING_OVER_SWITCH]=true;
					Features.StartingOver();
					GameTemp.player_new_map_id=homedata.Value.MapId; //homedata[0]
					GameTemp.player_new_x=homedata.Value.X; //homedata[1]
					GameTemp.player_new_y=homedata.Value.Y; //homedata[2]
					GameTemp.player_new_direction=homedata.Value.Direction; //homedata[3]
					if (Scene is ISceneMap s) s.transfer_player();
					GameMap.refresh();
				} else {
					pbHealAll();
				}
			}
			pbEraseEscapePoint();
		}

		public void pbCaveEntranceEx(bool exiting) {
			//sprite=new BitmapSprite(Graphics.width,Graphics.height);
			//Bitmap sprite=new BitmapSprite(Graphics.width,Graphics.height);
			////sprite.z=100000;
			//int totalBands=15;
			//int totalFrames=15;
			//float bandheight=((Graphics.height/2)-10f)/totalBands;
			//float bandwidth=((Graphics.width/2)-12f)/totalBands;
			//List<double> grays=new List<double>();
			//int tbm1=totalBands-1;
			//for (int i = 0; i < totalBands; i++) {
			//  grays.Add(exiting ? 0 : 255);
			//}
			//int j = 0; do { //totalFrames.times |j|
			//  float x=0;
			//  float y=0;
			//  float rectwidth=Graphics.width;
			//  float rectheight=Graphics.height;
			//  for (int k = 0; k < j; k++) {
			//    double t=(255.0f)/totalFrames;
			//    if (exiting) {
			//      t=1.0-t;
			//      t*=1.0f+((k)/totalFrames);
			//    } else {
			//      t*=1.0+0.3f*Math.Pow(((totalFrames-k)/totalFrames),0.7f);
			//    }
			//    //grays[k]-=t;
			//    grays[k]=grays[k]-t;
			//    if (grays[k]<0) grays[k]=0;
			//  }
			//  for (int i = 0; i < totalBands; i++) {
			//    double currentGray=grays[i];
			//    sprite.bitmap.fill_rect(new Rect(x,y,rectwidth,rectheight),
			//       new Color(currentGray,currentGray,currentGray));
			//    x+=bandwidth;
			//    y+=bandheight;
			//    rectwidth-=bandwidth*2;
			//    rectheight-=bandheight*2;
			//  }
			//  Graphics?.update();
			//  Input.update(); j++;
			//} while (j < totalFrames);
			//if (exiting) {
			//  pbToneChangeAll(new Tone(255,255,255),0);
			//} else {
			//  pbToneChangeAll(new Tone(-255,-255,-255),0);
			//}
			//for (j = 0; j < 15; j++) {
			//  if (exiting) {
			//    sprite.color=new Color(255,255,255,j*255/15);
			//  } else {
			//    sprite.color=new Color(0,0,0,j*255/15) ;
			//  }
			//  Graphics?.update();
			//  Input.update();
			//}
			//pbToneChangeAll(new Tone(0,0,0),8);
			//for (j = 0; j < 5; j++) {
			//  Graphics?.update();
			//  Input.update();
			//}
			//sprite.dispose();
		}

		public void pbCaveEntrance() {
			pbSetEscapePoint();
			pbCaveEntranceEx(false);
		}

		public void pbCaveExit() {
			pbEraseEscapePoint();
			pbCaveEntranceEx(true);
		}

		public void pbSetEscapePoint() {
			if (Global.escapePoint == null) Global.escapePoint=new float[0];
			float xco=GamePlayer.x;
			float yco=GamePlayer.y;
			float dir = 0;
			switch (GamePlayer.direction) {
			case 2:   // Down
			yco-=1; dir=8;
			break;
			case 4:   // Left
			xco+=1; dir=6;
			break;
			case 6:   // Right
			xco-=1; dir=4;
			break;
			case 8:   // Up
			yco+=1; dir=2;
			break;
			}
			Global.escapePoint=new float[] { GameMap.map_id, xco, yco, dir };
		}

		public void pbEraseEscapePoint() {
			Global.escapePoint=new float[0];
		}
		#endregion

		#region Partner trainer
		public void pbRegisterPartner(TrainerTypes trainerid,string trainername,int partyid=0) {
			pbCancelVehicles();
			ITrainer trainer = null; //Trainer.pbLoadTrainer(trainerid,trainername,partyid); //ToDo: Uncomment
			//Events.OnTrainerPartyLoad.trigger(null,trainer);
			ITrainer trainerobject=new Trainer(Game._INTL(trainer.name),trainerid);
			trainerobject.setForeignID(Trainer);
			foreach (IPokemon i in trainer.party) {
				i.trainerID=trainerobject.id;
				i.ot=trainerobject.name;
				//i.SetCatchInfos(trainer:trainerobject);
				i.calcStats();
			}
			//Global.partner=new Trainer(trainerid,trainerobject.name,trainerobject.id,trainer.party);
			ITrainer t=new Trainer(trainertype:trainerid,name:trainerobject.name); t.party = trainer.party;
			Global.partner=t;//new Combat.Trainer(trainerid,trainerobject.name,trainerobject.id,trainer.party);
		}

		public void pbDeregisterPartner() {
			Global.partner=null;
		}
		#endregion

		#region Constant checks
		// ===============================================================================
		//Events.onMapUpdate+=delegate(object sender, EventArgs e) {   // Pokérus check
		//  last=Global.pokerusTime;
		//  now=pbGetTimeNow();
		//  if (!last || last.year!=now.year || last.month!=now.month || last.day!=now.day) {
		//    if (Trainer && Trainer.party) {
		//      foreach (var i in Trainer.pokemonParty) {
		//        i.lowerPokerusCount;
		//      }
		//      Global.pokerusTime=now;
		//    }
		//  }
		//}

		/// <summary>
		/// Returns whether the Poké Center should explain Pokérus to the player, if a
		/// healed Pokémon has it.
		/// </summary>
		/// <returns></returns>
		public bool pbPokerus() {
			if (GameSwitches[Core.SEEN_POKERUS_SWITCH]) return false;
			//if (Core.SEEN_POKERUS_SWITCH) return false;
			if (Features.SeenPokerusSwitch) return false;
			foreach (IPokemon i in Trainer.party) {
				//if (i.PokerusStage==1) return true;
				if (i.PokerusStage==true) return true;
			}
			return false;
		}


		/// <summary>
		/// Connects to User's OS and checks if laptop battery 
		/// life alert should be displayed on screen
		/// </summary>
		/// <returns></returns>
		//public bool pbBatteryLow() {
		//  //int[] power="\0"*12;
		//  int[] power=new int[12];
		//  try {
		//    sps=new Win32API('kernel32.dll','GetSystemPowerStatus','p','l');
		//  } catch (Exception) {
		//    return false;
		//  }
		//  if (sps.call(power)==1) {
		//    status=power;//.unpack("CCCCVV")
		//    //  Battery Flag
		//    if (status[1]!=255 && (status[1]&6)!=0) {		// Low or Critical
		//      return true;
		//    }
		//    //  Battery Life Percent
		//    if (status[2]<3) {		// Less than 3 percent
		//      return true;
		//    }
		//    //  Battery Life Time
		//    if (status[4]<300) {		// Less than 5 minutes
		//      return true;
		//    }
		//  }
		//  return false;
		//}

		//Events.onMapUpdate+=delegate(object sender, EventArgs e) {
		//  DateTime time=pbGetTimeNow();
		//  if (time.Sec==0 && Trainer != null && Global != null && GamePlayer != null && GameMap != null &&
		//     !PokemonTemp.batterywarning && !GamePlayer.move_route_forcing &&
		//     !pbMapInterpreterRunning? && !GameTemp.message_window_showing &&
		//     pbBatteryLow?) {
		//    PokemonTemp.batterywarning=true;
		//    if (this is IGameMessage m) m.pbMessage(Game._INTL("The game has detected that the battery is low. You should save soon to avoid losing your progress."));
		//  }
		//  if (PokemonTemp.cueFrames) {
		//    PokemonTemp.cueFrames-=1;
		//    if (PokemonTemp.cueFrames<=0) {
		//      PokemonTemp.cueFrames=null;
		//      if (GameSystem.getPlayingBGM==null) {
		//        pbBGMPlay(PokemonTemp.cueBGM);
		//      }
		//    }
		//  }
		//}
		#endregion

		#region Audio playing
		public void pbCueBGM(string bgm, float seconds, int? volume = null, float? pitch = null) {
			if (bgm == null) return;
			if (this is IGameAudioPlay a)
				pbCueBGM((IAudioBGM)a.pbResolveAudioFile(bgm,volume,pitch), seconds, volume, pitch);
			//IAudioBGM playingBGM=GameSystem.playing_bgm;
			//if (playingBGM == null || playingBGM.name!=bgm.name || playingBGM.pitch!=bgm.pitch) {
			//	pbBGMFade(seconds);
			//	if (PokemonTemp.cueFrames == null) {
			//		PokemonTemp.cueFrames=(int)((seconds*Graphics.frame_rate)*3/5);
			//	}
			//	PokemonTemp.cueBGM=bgm;
			//} else if (playingBGM != null) {
			//	pbBGMPlay(bgm);
			//}
		}
		
		public void pbCueBGM(IAudioBGM bgm, float seconds, int? volume = null, float? pitch = null) {
			if (bgm == null) return;
			if (this is IGameAudioPlay a)
				bgm=(IAudioBGM)a.pbResolveAudioFile(bgm,volume,pitch);
			IAudioBGM playingBGM=GameSystem.playing_bgm;
			if (playingBGM == null || playingBGM.name!=bgm.name || playingBGM.pitch!=bgm.pitch) {
				if (this is IGameAudioPlay a1) a1.pbBGMFade(seconds);
				if (PokemonTemp is ITempMetadataField f0 && f0.cueFrames == null) {
					f0.cueFrames=(seconds*Graphics.frame_rate)*3/5;
				}
				if (PokemonTemp is ITempMetadataField f1) f1.cueBGM=bgm;
			} else if (playingBGM != null && this is IGameAudioPlay a2) {
				a2.pbBGMPlay(bgm);
			}
		}

		public void pbAutoplayOnTransition() {
			//string surfbgm=pbGetMetadata(0,MetadataSurfBGM);
			string surfbgm=pbGetMetadata(0).Global.SurfBGM;
			if (Global.surfing && surfbgm != null && this is IGameAudioPlay a) {
				a.pbBGMPlay(surfbgm);
			} else {
				GameMap.autoplayAsCue();
			}
		}

		public void pbAutoplayOnSave() {
			//string surfbgm=pbGetMetadata(0,MetadataSurfBGM);
			string surfbgm=pbGetMetadata(0).Global.SurfBGM;
			if (Global.surfing && surfbgm != null && this is IGameAudioPlay a) {
				a.pbBGMPlay(surfbgm);
			} else {
				GameMap.autoplay();
			}
		}
		#endregion

		#region Voice recorder
		public IWaveData pbRecord(string text,float maxtime=30.0f) {
			if (text == null) text="";
			IWindow_UnformattedTextPokemon textwindow = null; //new Window_UnformattedTextPokemon().WithSize(text,
			//	0,0,Graphics.width,Graphics.height-96);
			//textwindow.z=99999;
			if (text=="") {
				textwindow.visible=false;
			}
			IWaveData wave=null;
			IWindow_AdvancedTextPokemon msgwindow=(this as IGameMessage).pbCreateMessageWindow();
			float oldvolume=this is IGameAudio a ? a.Audio_bgm_get_volume : 0;
			if (this is IGameAudio a1) a1.Audio_bgm_set_volume(0);
			int delay=2;
			int i = 0; do { //delay.times |i|
				if (this is IGameMessage m) m.pbMessageDisplay(msgwindow,
					string.Format("Recording in {0} second(s)...\nPress ESC to cancel.",delay-i),false);
				int n = 0; do { //;Graphics.frame_rate.times 
					Graphics?.update();
					Input.update();
					textwindow.update();
					msgwindow.update();
					if (Input.trigger(Input.B)) {
						if (this is IGameAudio a2) a2.Audio_bgm_set_volume(oldvolume);
						(this as IGameMessage).pbDisposeMessageWindow(msgwindow);
						textwindow.dispose();
						return null;
					} n++;
				} while(n < Graphics.frame_rate); i++; 
			} while(i < delay);
			(this as IGameMessage).pbMessageDisplay(msgwindow,
				Game._INTL("NOW RECORDING\nPress ESC to stop recording."),false);
			if (beginRecordUI()) {
				int frames=(int)(maxtime*Graphics.frame_rate);
				i = 0; do { //;frames.times 
					Graphics?.update();
					Input.update();
					textwindow.update();
					msgwindow.update();
					if (Input.trigger(Input.B)) {
						break;
					}
				} while(i < frames);
				string tmpFile="\\record.wav";//ENV["TEMP"]+
				//endRecord(tmpFile); //ToDo: Stops recording and saves the recording to a file.
				wave =getWaveDataUI(tmpFile,true);
				if (wave != null) {
					(this as IGameMessage).pbMessageDisplay(msgwindow,Game._INTL("PLAYING BACK..."),false);
					textwindow.update();
					msgwindow.update();
					Graphics?.update();
					Input.update();
					wave.play();
					i = 0; do { //(Graphics.frame_rate*wave.time).to_i.times 
						Graphics?.update();
						Input.update();
						textwindow.update();
						msgwindow.update();
					} while(i < (Graphics.frame_rate*wave.time()));
				}
			}
			if (this is IGameAudio a3) a3.Audio_bgm_set_volume(oldvolume);
			(this as IGameMessage).pbDisposeMessageWindow(msgwindow);
			textwindow.dispose();
			return wave;
		}

		public bool pbRxdataExists(string file) {
			if (false) { //$RPGVX
				return pbRgssExists(file+".rvdata");
			} else {
				return pbRgssExists(file+".rxdata") ;
			}
		}
		#endregion

		#region Gaining items
		public bool pbItemBall(Items item,int quantity=1) {
			//if (item is String || item is Symbol) {
			//  item=getID(PBItems,item);
			//}
			if (item == null || item<=0 || quantity<1) return false;
			string itemname=(quantity>1) ? Kernal.ItemData[item].Plural : Game._INTL(item.ToString(TextScripts.Name));
			//int pocket=pbGetPocket(item);
			ItemPockets pocket=Kernal.ItemData[item].Pocket??ItemPockets.MISC;
			if (Bag.pbStoreItem(item,quantity)) {		// If item can be picked up
				if (Kernal.ItemData[item].Category==ItemCategory.ALL_MACHINES) { //[ITEMUSE]==3 || Kernal.ItemData[item][ITEMUSE]==4) { If item is TM=>3 or HM=>4
					(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]{1} found \\c[1]{2}\\c[0]!\\nIt contained \\c[1]{3}\\c[0].\\wtnp[30]",
						Trainer.name,itemname,Game._INTL(Kernal.ItemData[item].Id.ToString(TextScripts.Name))));//ToDo:[ITEMMACHINE] param for Machine-to-Move Id
				} else if (item == Items.LEFTOVERS) {
					(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]{1} found some \\c[1]{2}\\c[0]!\\wtnp[30]",Trainer.name,itemname));
				} else if (quantity>1) {
					(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]{1} found {2} \\c[1]{3}\\c[0]!\\wtnp[30]",Trainer.name,quantity,itemname));
				} else {
					(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]{1} found one \\c[1]{2}\\c[0]!\\wtnp[30]",Trainer.name,itemname));
				}
				(this as IGameMessage).pbMessage(Game._INTL("{1} put the \\c[1]{2}\\c[0]\r\nin the <icon=bagPocket#{pocket}>\\c[1]{3}\\c[0] Pocket.",
					Trainer.name,itemname,pocket.ToString())); //PokemonBag.pocketNames()[pocket]
				return true;
			} else {   // Can't add the item
				if (Kernal.ItemData[item].Category==ItemCategory.ALL_MACHINES) { //[ITEMUSE]==3 || Kernal.ItemData[item][ITEMUSE]==4) {
					(this as IGameMessage).pbMessage(Game._INTL("{1} found \\c[1]{2}\\c[0]!\\wtnp[20]",Trainer.name,itemname));
				} else if (item == Items.LEFTOVERS) {
					(this as IGameMessage).pbMessage(Game._INTL("{1} found some \\c[1]{2}\\c[0]!\\wtnp[20]",Trainer.name,itemname));
				} else if (quantity>1) {
					(this as IGameMessage).pbMessage(Game._INTL("{1} found {2} \\c[1]{3}\\c[0]!\\wtnp[20]",Trainer.name,quantity,itemname));
				} else {
					(this as IGameMessage).pbMessage(Game._INTL("{1} found one \\c[1]{2}\\c[0]!\\wtnp[20]",Trainer.name,itemname));
				}
				(this as IGameMessage).pbMessage(Game._INTL("Too bad... The Bag is full..."));
				return false;
			}
		}

		public bool pbReceiveItem(Items item,int quantity=1) {
			//if (item is String || item is Symbol) {
			//  item=getID(PBItems,item);
			//}
			if (item == null || item<=0 || quantity<1) return false;
			string itemname=(quantity>1) ? Kernal.ItemData[item].Plural : Game._INTL(item.ToString(TextScripts.Name));
			//int pocket=pbGetPocket(item);
			ItemPockets pocket=Kernal.ItemData[item].Pocket??ItemPockets.MISC;
			if (Kernal.ItemData[item].Category==ItemCategory.ALL_MACHINES) { //[ITEMUSE]==3 || Kernal.ItemData[item][ITEMUSE]==4) {
				(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]Obtained \\c[1]{1}\\c[0]!\\nIt contained \\c[1]{2}\\c[0].\\wtnp[30]",
					itemname,Game._INTL(Kernal.ItemData[item].Id.ToString(TextScripts.Name))));//ToDo:[ITEMMACHINE] param for Machine-to-Move Id
			} else if (item == Items.LEFTOVERS) {
				(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]Obtained some \\c[1]{1}\\c[0]!\\wtnp[30]",itemname));
			} else if (quantity>1) {
				(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]Obtained \\c[1]{1}\\c[0]!\\wtnp[30]",itemname));
			} else {
				(this as IGameMessage).pbMessage(Game._INTL("\\se[ItemGet]Obtained \\c[1]{1}\\c[0]!\\wtnp[30]",itemname));
			}
			if (Bag.pbStoreItem(item,quantity)) {       // If item can be added
				(this as IGameMessage).pbMessage(Game._INTL("{1} put the \\c[1]{2}\\c[0]\r\nin the <icon=bagPocket#{pocket}>\\c[1]{3}\\c[0] Pocket.",
					Trainer.name,itemname,pocket.ToString())); //PokemonBag.pocketNames()[pocket]
				return true;
			}
			return false;   // Can't add the item
		}

		public void pbUseKeyItem() {
			if (Bag.registeredItem== 0 && this is IGameMessage m) {
				m.pbMessage(Game._INTL("A Key Item in the Bag can be registered to this key for instant use."));
			} else {
				pbUseKeyItemInField(Bag.registeredItem);
			}
		}
		#endregion

		#region Bridges
		public void pbBridgeOn(float height=2) {
			Global.bridge=height;
		}

		public void pbBridgeOff() {
			Global.bridge=0;
		}
		#endregion

		#region Event locations, terrain tags
		public bool pbEventFacesPlayer (IGameCharacter @event,IGamePlayer player,float distance) {
			if (distance<=0) return false;
		//  Event can't reach player if no coordinates coincide
			if (@event.x!=player.x && @event.y!=player.y) return false;
			float deltaX = (@event.direction == 6 ? 1 : @event.direction == 4 ? -1 : 0);
			float deltaY = (@event.direction == 2 ? 1 : @event.direction == 8 ? -1 : 0);
		//  Check for existence of player
			float curx=@event.x;
			float cury=@event.y;
			bool found=false;
			for (int i = 0; i < distance; i++) {
				curx+=deltaX;
				cury+=deltaY;
				if (player.x==curx && player.y==cury) {
					found=true;
					break;
				}
			}
			return found;
		}

		public bool pbEventCanReachPlayer (IGameCharacter @event,IGamePlayer player,float distance) {
			if (distance<=0) return false;
		//  Event can't reach player if no coordinates coincide
			if (@event.x!=player.x && @event.y!=player.y) return false;
			float deltaX = (@event.direction == 6 ? 1 : @event.direction == 4 ? -1 : 0);
			float deltaY =  (@event.direction == 2 ? 1 : @event.direction == 8 ? -1 : 0);
		//  Check for existence of player
			float curx=@event.x;
			float cury=@event.y;
			bool found=false;
			float realdist=0;
			for (int i = 0; i < distance; i++) {
				curx+=deltaX;
				cury+=deltaY;
				if (player.x==curx && player.y==cury) {
					found=true;
					break;
				}
				realdist+=1;
			}
			if (!found) return false;
		//  Check passibility
			curx=@event.x;
			cury=@event.y;
			for (int i = 0; i < realdist; i++) {
				if (!@event.passable(curx,cury,@event.direction)) {
					return false;
				}
				curx+=deltaX;
				cury+=deltaY;
			}
			return true;
		}

		public ITilePosition pbFacingTileRegular(float? direction=null,IGameCharacter @event=null) {
			if (@event == null) @event=GamePlayer;
			if (@event == null) return new TilePosition();
			float x=@event.x;
			float y=@event.y;
			if (direction == null) direction=@event.direction;
			switch (direction) {
				case 1:
					y+=1; x-=1;
					break;
				case 2:
					y+=1;
					break;
				case 3:
					y+=1; x+=1;
					break;
				case 4:
					x-=1;
					break;
				case 6:
					x+=1;
					break;
				case 7:
					y-=1; x-=1;
					break;
				case 8:
					y-=1;
					break;
				case 9:
					y-=1; x+=1;
					break;
			}
			return GameMap != null ? new TilePosition(GameMap.map_id, x, y) : new TilePosition(0, x, y);
		}

		public ITilePosition pbFacingTile(float? direction=null,IGameCharacter @event=null) {
			if (MapFactory != null) {
				return MapFactory.getFacingTile(direction,@event);
			} else {
				return pbFacingTileRegular(direction,@event);
			}
		}

		public bool pbFacingEachOther(IGameCharacter event1,IGameCharacter event2) {
			if (event1 == null || event2 == null) return false; ITilePosition tile1, tile2;
			if (MapFactory != null) {             
				tile1=MapFactory.getFacingTile(null,event1);
				tile2=MapFactory.getFacingTile(null,event2);
				if (tile1 == null || tile2 == null) return false;
				if (tile1.MapId==event2.map.map_id &&
					tile1.X==event2.x && tile1.Y==event2.y &&
					tile2.MapId==event1.map.map_id &&
					tile2.X==event1.x && tile2.Y==event1.y) {
					return true;
				} else {
					return false;
				}
			} else {
				tile1=pbFacingTile(null,event1);
				tile2=pbFacingTile(null,event2);
				if (tile1 == null || tile2 == null) return false;
				if (tile1.X==event2.x && tile1.Y==event2.y &&
					tile2.X==event1.x && tile2.Y==event1.y) {
					return true;
				} else {
					return false;
				}
			}
		}

		public Terrains pbGetTerrainTag(IGameCharacter @event=null,bool countBridge=false) {
			if (@event == null) @event=GamePlayer;
			if (@event == null) return 0;
			if (MapFactory != null) {
				return MapFactory.getTerrainTag(@event.map.map_id,@event.x,@event.y,countBridge).Value;
			} else {
				return GameMap.terrain_tag(@event.x,@event.y,countBridge);
			}
		}

		public Terrains? pbFacingTerrainTag(IGameCharacter @event=null,float? dir=null) {
			if (MapFactory != null) {
				return MapFactory.getFacingTerrainTag(dir,@event);
			} else {
				if (@event == null) @event=GamePlayer;
				if (@event == null) return 0;
				ITilePosition facing=pbFacingTile(dir,@event);
				return GameMap.terrain_tag(facing.X,facing.Y); //(facing[1],facing[2]);
			}
		}
		#endregion

		#region Event movement
		public void pbTurnTowardEvent(IGameCharacter @event,IGameCharacter otherEvent) {
			float sx=0;
			float sy=0;
			if (MapFactory != null) {
                IPoint relativePos=MapFactory.getThisAndOtherEventRelativePos(otherEvent,@event);
				sx = relativePos.x;
				sy = relativePos.y;
			} else {
				sx = @event.x - otherEvent.x;
				sy = @event.y - otherEvent.y;
			}
			if (sx == 0 && sy == 0) {
				return;
			}
			if (Math.Abs(sx) > Math.Abs(sy)) {
				//sx > 0 ? @event.turn_left : @event.turn_right;
				if(sx > 0) @event.turn_left(); else @event.turn_right();
			} else {
				//sy > 0 ? @event.turn_up : @event.turn_down;
				if(sy > 0) @event.turn_up(); else @event.turn_down();
			}
		}

		public void pbMoveTowardPlayer(IGameCharacter @event) {
			int maxsize=Math.Max(GameMap.width,GameMap.height);
			if (!pbEventCanReachPlayer(@event,GamePlayer,maxsize)) return;
			do { //;loop
				float x=@event.x;
				float y=@event.y;
				@event.move_toward_player();
				if (@event.x==x && @event.y==y) break;
				while (@event.moving) {
					Graphics?.update();
					Input.update();
					if (this is IGameMessage m) m.pbUpdateSceneMap();
				}
			} while (true);
			if (PokemonMap != null) PokemonMap.addMovedEvent(@event.id);
		}

		public bool pbJumpToward(int dist=1,bool playSound=false,bool cancelSurf=false) {
			float x=GamePlayer.x;
			float y=GamePlayer.y;
			switch (GamePlayer.direction) {
				case 2: // down
					GamePlayer.jump(0,dist);
					break;
				case 4: // left
					GamePlayer.jump(-dist,0);
					break;
				case 6: // right
					GamePlayer.jump(dist,0);
					break;
				case 8: // up
					GamePlayer.jump(0,-dist);
					break;
			}
			if (GamePlayer.x!=x || GamePlayer.y!=y) {
				if (playSound && this is IGameAudioPlay a) a.pbSEPlay("jump");
				if (cancelSurf) pbCancelVehicles();
				while (GamePlayer.jumping) {
					Graphics?.update();
					Input.update();
					if (this is IGameMessage m) m.pbUpdateSceneMap();
				}
				return true;
			}
			return false;
		}

		public void pbWait(int numframes) {
			if (Core.INTERNAL) return; //if there's no ui connected...
			do { //;numframes.times 
				Graphics?.update();
				Input.update();
				if (this is IGameMessage m) m.pbUpdateSceneMap();
			} while (true);
		}

		public IMoveRoute pbMoveRoute(IGameCharacter @event,int[] commands,bool waitComplete=false) {
			//IMoveRoute route=new RPG.MoveRoute();
			//route.repeat=false;
			//route.skippable=true;
			//route.list.Clear();
			//route.list.Add(new RPG.MoveCommand(MoveRoutes.ThroughOn));
			//int i=0; while (i<commands.Length) { 
			//switch (commands[i]) {
			//case MoveRoutes.Wait: case MoveRoutes.SwitchOn: case MoveRoutes.SwitchOff: case
			//	MoveRoutes.ChangeSpeed: case MoveRoutes.ChangeFreq: case MoveRoutes.Opacity: case
			//	MoveRoutes.Blending: case MoveRoutes.PlaySE: case MoveRoutes.Script:
			//	route.list.Add(new RPG.MoveCommand(commands[i],new int[] { commands[i + 1] }));
			//	i+=1;
			//	break;
			//case MoveRoutes.ScriptAsync:
			//	route.list.Add(new RPG.MoveCommand(MoveRoutes.Script,new int[] { commands[i + 1] }));
			//	route.list.Add(new RPG.MoveCommand(MoveRoutes.Wait,new int[] { 0 }));
			//	i+=1;
			//	break;
			//case MoveRoutes.Jump:
			//	route.list.Add(new RPG.MoveCommand(commands[i],new int[] { commands[i + 1], commands[i + 2] }));
			//	i+=2;
			//	break;
			//case MoveRoutes.Graphic:
			//	route.list.Add(new RPG.MoveCommand(commands[i],
			//		new int[] { commands[i + 1], commands[i + 2], commands[i + 3], commands[i + 4] }));
			//	i+=4;
			//	break;
			//default:
			//	route.list.Add(new RPG.MoveCommand(commands[i]));
			//	break;
			//}
			//i+=1;
			//}
			//route.list.Add(new RPG.MoveCommand(MoveRoutes.ThroughOff));
			//route.list.Add(new RPG.MoveCommand(0));
			//if (@event != null) {
			//	@event.force_move_route(route);
			//}
			//return route;
			return null;
		}
		#endregion

		#region Screen effects
		public void pbToneChangeAll(ITone tone,float duration) {
			GameScreen.start_tone_change(tone,duration * 2);
			foreach (var picture in GameScreen.pictures) {
				if (picture != null) picture.start_tone_change(tone,duration * 2);
			}
		}

		public void pbShake(int power,int speed,int frames) {
			GameScreen.start_shake(power,speed,frames * 2);
		}

		public void pbFlash(IColor color,int frames) {
			GameScreen.start_flash(color,frames * 2);
		}

		public void pbScrollMap(int direction, int distance, float speed) {
			if (GameMap == null) return;
			if (speed==0) {
				switch (direction) {
					case 2:
						GameMap.scroll_down(distance * 128);
						break;
					case 4:
						GameMap.scroll_left(distance * 128);
						break;
					case 6:
						GameMap.scroll_right(distance * 128);
						break;
					case 8:
						GameMap.scroll_up(distance * 128);
						break;
				}
			} else {
				GameMap.start_scroll(direction, distance, speed);
				float oldx=GameMap.display_x;
				float oldy=GameMap.display_y;
				do { //;loop
					Graphics?.update();
					Input.update();
					if (!GameMap.scrolling) {
					break;
					}
					if (this is IGameMessage m) m.pbUpdateSceneMap();
					if (GameMap.display_x==oldx && GameMap.display_y==oldy) {
					break;
					}
					oldx=GameMap.display_x;
					oldy=GameMap.display_y;
				} while (true);
			}
		}
		#endregion
	}

	// ===============================================================================
	// Events
	// ===============================================================================
	//public partial class GameEvent {
	//	public bool cooledDown (int seconds) {
	//		if (!(expired(seconds) && tsOff("A"))) {
	//			this.need_refresh=true;
	//			return false;
	//		} else {
	//			return true;
	//		}
	//	}
	//
	//	public bool cooledDownDays (int days) {
	//		if (!(expiredDays(days) && tsOff("A"))) {
	//			this.need_refresh=true;
	//			return false;
	//		} else {
	//			return true;
	//		}
	//	}
	//}

	public partial class InterpreterFieldMixin : IInterpreterFieldMixin
	{
		private int @event_id;
		private int @map_id;
		private IGameCharacter @event;
		//  Used in boulder events. Allows an event to be pushed. To be used in
		//  a script event command.
		public void pbPushThisEvent() {
			@event=(Game.GameData as Game).Interpreter.get_character(0);
			float oldx=@event.x;
			float oldy=@event.y;
		//  Apply strict version of passable, which makes impassable
		//  tiles that are passable only from certain directions
			if (!@event.passableStrict(@event.x,@event.y,Game.GameData.GamePlayer.direction)) {
				return;
			}
			switch (Game.GameData.GamePlayer.direction) {
				case 2: // down
					@event.move_down();
					break;
				case 4: // left
					@event.move_left();
					break;
				case 6: // right
					@event.move_right();
					break;
				case 8: // up
					@event.move_up();
					break;
			}
			if (Game.GameData.PokemonMap != null) Game.GameData.PokemonMap.addMovedEvent(@event_id);
			if (oldx!=@event.x || oldy!=@event.y) {
					Game.GameData.GamePlayer._lock();
					do {
						(Game.GameData as Game).Graphics?.update();
						Input.update();
						if (Game.GameData is IGameMessage m) m.pbUpdateSceneMap();
					} while (@event.moving);
					Game.GameData.GamePlayer.unlock();
				}
		}

		public bool pbPushThisBoulder() {
			if (Game.GameData.PokemonMap.strengthUsed) {
				pbPushThisEvent();
			}
			return true;
		}

		public bool pbHeadbutt() {
			if (Game.GameData is IGameHiddenMoves f) f.pbHeadbutt((Game.GameData as Game).Interpreter.get_character(0));
			return true;
		}

		public bool pbTrainerIntro(TrainerTypes symbol) {
			//if (Core.DEBUG) {
			//  if (!Game.pbTrainerTypeCheck(symbol)) return false;
			//}
			TrainerTypes trtype=symbol; //PBTrainers.const_get(symbol);
			if (this is IInterpreterMixinMessage m) m.pbGlobalLock();
			if (Game.GameData is IGameUtility a) a.pbPlayTrainerIntroME(trtype);
			return true;
		}

		public void pbTrainerEnd() {
			if (this is IInterpreterMixinMessage m) m.pbGlobalUnlock();
			IGameEvent e=(Game.GameData as Game).Interpreter.get_character(0);
			if (e != null) e.erase_route();
		}

		//public object[] pbParams { get {
		//	return @parameters != null ? @parameters : @params;
		//} }

		public IPokemon pbGetPokemon(int id) {
			return Game.GameData.Trainer.party[Game.GameData is IGameUtility g ? (int)g.pbGet(id) : id];
		}

		public void pbSetEventTime(params int[] arg) {
			if (Game.GameData.Global.eventvars == null) Game.GameData.Global.eventvars=new Dictionary<KeyValuePair<int, int>, long>();
			long time=Game.pbGetTimeNow().Ticks;
			//time=time.to_i;
			if (Game.GameData is IInterpreterMixinMessage i) i.pbSetSelfSwitch(@event_id,"A",true);
			Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,@event_id)]=time;
			foreach (var otherevt in arg) {
				if (Game.GameData is IInterpreterMixinMessage i0) i0.pbSetSelfSwitch(otherevt,"A",true);
				Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,otherevt)]=time;
			}
		}

		public object getVariable(params int[] arg) {
			if (arg.Length==0) {
				if (Game.GameData.Global.eventvars == null) return null;
				return Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,@event_id)];
			} else {
				return Game.GameData.GameVariables[arg[0]];
			}
		}

		public void setVariable(params int[] arg) {
			if (arg.Length==1) {
				if (Game.GameData.Global.eventvars == null) Game.GameData.Global.eventvars=new Dictionary<KeyValuePair<int, int>, long>();
				Game.GameData.Global.eventvars[new KeyValuePair<int, int>(@map_id,@event_id)]=arg[0];
			} else {
				Game.GameData.GameVariables[arg[0]]=arg[1];
				Game.GameData.GameMap.need_refresh=true;
			}
		}

		public bool tsOff (string c) {
			return (Game.GameData as Game).Interpreter.get_character(0).tsOff(c);
		}

		public bool tsOn (string c) {
			return (Game.GameData as Game).Interpreter.get_character(0).tsOn(c);
		}

		//alias isTempSwitchOn? tsOn?;
		//alias isTempSwitchOff? tsOff?;

		public void setTempSwitchOn(string c) {
			(Game.GameData as Game).Interpreter.get_character(0).setTempSwitchOn(c);
		}

		public void setTempSwitchOff(string c) {
			(Game.GameData as Game).Interpreter.get_character(0).setTempSwitchOff(c);
		}

		// Must use this approach to share the methods because the methods already
		// defined in a class override those defined in an included module
		/*CustomEventCommands=<<_END_;

		public void command_352() {
			scene=new PokemonSaveScene();
			screen=new PokemonSave(scene);
			screen.pbSaveScreen;
			return true;
		}

		public void command_125() {
			value = operate_value(pbParams[0], pbParams[1], pbParams[2]);
			Game.GameData.Trainer.money+=value;
			return true;
		}

		public void command_132() {
			Game.GameData.Global.nextBattleBGM=(pbParams[0]) ? pbParams[0].clone : null;
			return true;
		}

		public void command_133() {
			Game.GameData.Global.nextBattleME=(pbParams[0]) ? pbParams[0].clone : null;
			return true;
		}

		public void command_353() {
			pbBGMFade(1.0);
			pbBGSFade(1.0);
			pbFadeOutIn(99999){ Game.pbStartOver(true) }
		}

		public void command_314() {
			if (pbParams[0] == 0 && Game.GameData.Trainer && Game.GameData.Trainer.party) {
				pbHealAll();
			}
			return true;
		}
		_END_;*/
	}

	//public partial class Interpreter : InterpreterFieldMixin {
	//  //include InterpreterFieldMixin;
	//  //eval(InterpreterFieldMixin.CustomEventCommands);
	//}
	//
	//public partial class Game_Interpreter : InterpreterFieldMixin {
	//  //include InterpreterFieldMixin;
	//  //eval(InterpreterFieldMixin.CustomEventCommands);
	//}
}