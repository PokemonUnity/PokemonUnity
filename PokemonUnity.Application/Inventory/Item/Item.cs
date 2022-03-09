using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Character;
using PokemonUnity.Overworld;
using PokemonUnity.Monster;
using PokemonUnity.Monster.Data;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Inventory
{
	[Obsolete("Move Everything over to Game class using interface `IItemChecker` and `IGameItem`")]
	public static partial class Item { 
		public const int ITEMID        = 0;
		public const int ITEMNAME      = 1;
		public const int ITEMPLURAL    = 2;
		public const int ITEMPOCKET    = 3;
		public const int ITEMPRICE     = 4;
		public const int ITEMDESC      = 5;
		public const int ITEMUSE       = 6;
		public const int ITEMBATTLEUSE = 7;
		public const int ITEMTYPE      = 8;
		public const int ITEMMACHINE   = 9;

		/// <summary>
		/// </summary>
		/// <param name="move"></param>
		/// <returns></returns>
		/// <seealso cref="HiddenMoves"/>
		public static bool pbIsHiddenMove (Moves move) { //ToDo: Move to Game Class
			//if (Kernal.ItemData == null) return false;
			////for (int i = 0; i < Kernal.ItemData.Count; i++) {
			//for (int i = 0; i < Kernal.MachineData.Count; i++) {
			//  //if (!pbIsHiddenMachine(i)) continue;
			//  //if(Kernal.ItemData[i].Pocket == ItemPockets.MACHINE)
			//  //atk=Kernal.ItemData[i][ITEMMACHINE];
			//  MachineData atk = Kernal.MachineData[i]; //HiddenMachine is not HiddenMove
			//  if (atk.Type != MachineData.MachineType.HiddenMachine && move==atk.Move) return true;
			//}
			Moves[] hidden = new Moves[] {
				Moves.SURF,
				Moves.CUT,
				Moves.STRENGTH,
				Moves.FLASH,
				Moves.FLY,
				Moves.WHIRLPOOL,
				Moves.WATERFALL,
				//Moves.RIDE,
				Moves.DIVE,
				Moves.ROCK_CLIMB,
				Moves.ROCK_SMASH,
				Moves.HEADBUTT,
				Moves.DEFOG }; 
			//return false;
			return hidden.Contains(move);
		}

		public static int pbGetPrice(Items item) {
			return Kernal.ItemData[item].Price; //[ITEMPRICE];
		}

		public static ItemPockets? pbGetPocket(Items item) {
			return Kernal.ItemData[item].Pocket; //[ITEMPOCKET];
		}

		/// <summary>
		/// Important items can't be sold, given to hold, or tossed.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool pbIsImportantItem (Items item) {
			return Kernal.ItemData.ContainsKey(item) && (pbIsKeyItem(item) ||
				pbIsHiddenMachine(item) ||
				(Core.INFINITETMS && pbIsTechnicalMachine(item)));
		}

		public static bool pbIsMachine (Items item) {
			return Kernal.ItemData[item].Category == ItemCategory.ALL_MACHINES || (pbIsTechnicalMachine(item) || pbIsHiddenMachine(item));
		}

		public static bool pbIsTechnicalMachine (Items item) {
			//return Kernal.ItemData.ContainsKey(item) && (Kernal.ItemData[item][ITEMUSE]==3);
			Items[] TMs = new Items[] { Items.TM_ALL }; //Items.TM01, Items.TM02, Items.TM03, Items.TM04, Items.TM05, Items.TM06, Items.TM07, Items.TM08, Items.TM09, Items.TM10, Items.TM11, Items.TM12, Items.TM13, Items.TM14, Items.TM15, Items.TM16, Items.TM17, Items.TM18, Items.TM19, Items.TM20, Items.TM21, Items.TM22, Items.TM23, Items.TM24, Items.TM25, Items.TM26, Items.TM27, Items.TM28, Items.TM29, Items.TM30, Items.TM31, Items.TM32, Items.TM33, Items.TM34, Items.TM35, Items.TM36, Items.TM37, Items.TM38, Items.TM39, Items.TM40, Items.TM41, Items.TM42, Items.TM43, Items.TM44, Items.TM45, Items.TM46, Items.TM47, Items.TM48, Items.TM49, Items.TM50, Items.TM51, Items.TM52, Items.TM53, Items.TM54, Items.TM55, Items.TM56, Items.TM57, Items.TM58, Items.TM59, Items.TM60, Items.TM61, Items.TM62, Items.TM63, Items.TM64, Items.TM65, Items.TM66, Items.TM67, Items.TM68, Items.TM69, Items.TM70, Items.TM71, Items.TM72, Items.TM73, Items.TM74, Items.TM75, Items.TM76, Items.TM77, Items.TM78, Items.TM79, Items.TM80, Items.TM81, Items.TM82, Items.TM83, Items.TM84, Items.TM85, Items.TM86, Items.TM87, Items.TM88, Items.TM89, Items.TM90, Items.TM91, Items.TM92, Items.TM93, Items.TM94, Items.TM95, Items.TM96, Items.TM97, Items.TM98, Items.TM99, Items.TM100 };
			return TMs.Contains(item);
		}

		public static bool pbIsHiddenMachine (Items item) {
			//return Kernal.ItemData.ContainsKey(item) && (Kernal.ItemData[item][ITEMUSE]==4);
			Items[] HMs = new Items[] { Items.HM01, Items.HM02, Items.HM03, Items.HM04, Items.HM05, Items.HM06, Items.HM07, Items.HM08 };
			return HMs.Contains(item);
		}

		public static bool pbIsMail (Items item) {
			return Kernal.ItemData.ContainsKey(item) && (ItemData.pbIsLetter(item)); //[ITEMTYPE]==1 || Kernal.ItemData[item][ITEMTYPE]==2
		}

		public static bool pbIsSnagBall (Items item) {
			return Kernal.ItemData.ContainsKey(item) && (ItemData.pbIsPokeBall(item) || Kernal.ItemData[item].Pocket == ItemPockets.POKEBALL);// || //[ITEMTYPE]==3
				//(Game.GameData.Global.snagMachine)); //Kernal.ItemData[item][ITEMTYPE]==4 && 4: SnagBall Item
		}

		public static bool pbIsPokeBall (Items item) {
			return Kernal.ItemData.ContainsKey(item) && (ItemData.pbIsPokeBall(item) || Kernal.ItemData[item].Pocket == ItemPockets.POKEBALL);//[ITEMTYPE]==4
		}

		public static bool pbIsBerry (Items item) {
			return Kernal.ItemData.ContainsKey(item) && ItemData.pbIsBerry(item); //[ITEMTYPE]==5 
		}

		public static bool pbIsKeyItem (Items item) {
			return Kernal.ItemData.ContainsKey(item) && (Kernal.ItemData[item].Pocket == ItemPockets.KEY);//[ITEMTYPE]==6
		}

		public static bool pbIsGem (Items item) {
			Items[] gems=new Items[] {Items.FIRE_GEM,Items.WATER_GEM,Items.ELECTRIC_GEM,Items.GRASS_GEM,Items.ICE_GEM,
				Items.FIGHTING_GEM,Items.POISON_GEM,Items.GROUND_GEM,Items.FLYING_GEM,Items.PSYCHIC_GEM,
				Items.BUG_GEM,Items.ROCK_GEM,Items.GHOST_GEM,Items.DRAGON_GEM,Items.DARK_GEM,
				Items.STEEL_GEM,Items.NORMAL_GEM,Items.FAIRY_GEM};
			//foreach (Items i in gems) {
			//  if (item == i) return true;
			//}
			//return false;
			return gems.Contains(item) || Kernal.ItemData[item].Category == ItemCategory.JEWELS;
		}

		public static bool pbIsEvolutionStone (Items item) {
			Items[] stones=new Items[] {Items.FIRE_STONE,Items.THUNDER_STONE,Items.WATER_STONE,Items.LEAF_STONE,Items.MOON_STONE,
					Items.SUN_STONE,Items.DUSK_STONE,Items.DAWN_STONE,Items.SHINY_STONE};
			//foreach (Items i in stones) {
			//  if (item == i) return true;
			//}
			//return false;
			return stones.Contains(item) || Kernal.ItemData[item].Category == ItemCategory.EVOLUTION;
		}

		public static bool pbIsMegaStone (Items item) {   // Does NOT include Red Orb/Blue Orb
			Items[] stones=new Items[] { Items.ABOMASITE,Items.ABSOLITE,Items.AERODACTYLITE,Items.AGGRONITE,Items.ALAKAZITE,
					Items.ALTARIANITE,Items.AMPHAROSITE,Items.AUDINITE,Items.BANETTITE,Items.BEEDRILLITE,
					Items.BLASTOISINITE,Items.BLAZIKENITE,Items.CAMERUPTITE,Items.CHARIZARDITE_X,Items.CHARIZARDITE_Y,
					Items.DIANCITE,Items.GALLADITE,Items.GARCHOMPITE,Items.GARDEVOIRITE,Items.GENGARITE,
					Items.GLALITITE,Items.GYARADOSITE,Items.HERACRONITE,Items.HOUNDOOMINITE,Items.KANGASKHANITE,
					Items.LATIASITE,Items.LATIOSITE,Items.LOPUNNITE,Items.LUCARIONITE,Items.MANECTITE,
					Items.MAWILITE,Items.MEDICHAMITE,Items.METAGROSSITE,Items.MEWTWONITE_X,Items.MEWTWONITE_Y,
					Items.PIDGEOTITE,Items.PINSIRITE,Items.SABLENITE,Items.SALAMENCITE,Items.SCEPTILITE,
					Items.SCIZORITE,Items.SHARPEDONITE,Items.SLOWBRONITE,Items.STEELIXITE,Items.SWAMPERTITE,
					Items.TYRANITARITE,Items.VENUSAURITE};
			//foreach (Items i in stones) {
			//  if (item == i) return true;
			//}
			//return false;
			return stones.Contains(item) || Kernal.ItemData[item].Category == ItemCategory.MEGA_STONES;
		}

		public static bool pbIsMulch (Items item) {
			Items[] mulches= new Items[] { Items.GROWTH_MULCH,Items.DAMP_MULCH,Items.STABLE_MULCH,Items.GOOEY_MULCH };
			//foreach (Items i in mulches) {
			//  if (item == i) return true;
			//}
			//return false;
			return mulches.Contains(item) || Kernal.ItemData[item].Category == ItemCategory.MULCH;
		}

		#region Move to Game class?
		public static void pbChangeLevel(IPokemon pokemon,int newlevel,IScene scene) {
			if (newlevel<1) newlevel=1;
			if (newlevel>Core.MAXIMUMLEVEL) newlevel=Core.MAXIMUMLEVEL;
			if (pokemon.Level>newlevel) {
				int attackdiff=pokemon.ATK;
				int defensediff=pokemon.DEF;
				int speeddiff=pokemon.SPE;
				int spatkdiff=pokemon.SPA;
				int spdefdiff=pokemon.SPD;
				int totalhpdiff=pokemon.TotalHP;
				//pokemon.Level=newlevel;
				(pokemon as Pokemon).SetLevel((byte)newlevel);
				//pokemon.Exp=Experience.GetStartExperience(pokemon.GrowthRate, newlevel);
				pokemon.calcStats();
				scene.pbRefresh();
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("{1} was downgraded to Level {2}!",pokemon.Name,pokemon.Level));
				attackdiff=pokemon.ATK-attackdiff;
				defensediff=pokemon.DEF-defensediff;
				speeddiff=pokemon.SPE-speeddiff;
				spatkdiff=pokemon.SPA-spatkdiff;
				spdefdiff=pokemon.SPD-spdefdiff;
				totalhpdiff=pokemon.TotalHP-totalhpdiff;
				pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
					totalhpdiff,attackdiff,defensediff,spatkdiff,spdefdiff,speeddiff));
				pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
					pokemon.TotalHP,pokemon.ATK,pokemon.DEF,pokemon.SPA,pokemon.SPD,pokemon.SPE));
			} else if (pokemon.Level==newlevel) {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("{1}'s level remained unchanged.",pokemon.Name));
			} else {
				int attackdiff=pokemon.ATK;
				int defensediff=pokemon.DEF;
				int speeddiff=pokemon.SPE;
				int spatkdiff=pokemon.SPA;
				int spdefdiff=pokemon.SPD;
				int totalhpdiff=pokemon.TotalHP;
				int oldlevel=pokemon.Level;
				//pokemon.Level=newlevel;
				(pokemon as Pokemon).SetLevel((byte)newlevel);
				//pokemon.Exp = Experience.GetStartExperience(pokemon.GrowthRate, newlevel);
				(pokemon as Pokemon).ChangeHappiness(HappinessMethods.LEVELUP);
				pokemon.calcStats();
				scene.pbRefresh();
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("{1} was elevated to Level {2}!",pokemon.Name,pokemon.Level));
				attackdiff=pokemon.ATK-attackdiff;
				defensediff=pokemon.DEF-defensediff;
				speeddiff=pokemon.SPE-speeddiff;
				spatkdiff=pokemon.SPA-spatkdiff;
				spdefdiff=pokemon.SPD-spdefdiff;
				totalhpdiff=pokemon.TotalHP-totalhpdiff;
				pbTopRightWindow(Game._INTL("Max. HP<r>+{1}\r\nAttack<r>+{2}\r\nDefense<r>+{3}\r\nSp. Atk<r>+{4}\r\nSp. Def<r>+{5}\r\nSpeed<r>+{6}",
					totalhpdiff,attackdiff,defensediff,spatkdiff,spdefdiff,speeddiff));
				pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
					pokemon.TotalHP,pokemon.ATK,pokemon.DEF,pokemon.SPA,pokemon.SPD,pokemon.SPE));
				//Moves[] movelist=pokemon.getMoveList();
				//foreach (Moves i in pokemon.getMoveList(LearnMethod.levelup)) { //movelist
				foreach (KeyValuePair<Moves,int> i in Kernal.PokemonMovesData[pokemon.Species].LevelUp) { 
					if (i.Value==pokemon.Level) {		// Learned a new move
						pbLearnMove(pokemon,i.Key,true);
					}
				}
				Pokemons newspecies=EvolutionHelper.pbCheckEvolution(pokemon)[0];
				if (newspecies>0 && Game.GameData is IGameUtility u) {
					u.pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo=(Game.GameData as Game).Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution();
						evo.pbEndScreen();
					});
				}
			}
		}

		public static int pbItemRestoreHP(IPokemon pokemon,int restorehp) {
			int newhp=pokemon.HP+restorehp;
			if (newhp>pokemon.TotalHP) newhp=pokemon.TotalHP;
			int hpgain=newhp-pokemon.HP;
			pokemon.HP=newhp;
			return hpgain;
		}

		public static bool pbHPItem(IPokemon pokemon,int restorehp,IScene scene) {
			if (pokemon.HP<=0 || pokemon.HP==pokemon.TotalHP || pokemon.isEgg) {
				scene.pbDisplay(Game._INTL("It won't have any effect."));
				return false;
			} else {
				int hpgain=pbItemRestoreHP(pokemon,restorehp);
				scene.pbRefresh();
				scene.pbDisplay(Game._INTL("{1}'s HP was restored by {2} points.",pokemon.Name,hpgain));
				return true;
			}
		}

		public static bool pbBattleHPItem(IPokemon pokemon,IBattler battler,int restorehp,IScene scene) {
			if (pokemon.HP<=0 || pokemon.HP==pokemon.TotalHP || pokemon.isEgg) {
				scene.pbDisplay(Game._INTL("But it had no effect!"));
				return false;
			} else {
				int hpgain=pbItemRestoreHP(pokemon,restorehp);
				if (battler.IsNotNullOrNone()) battler.HP=pokemon.HP;
				scene.pbRefresh();
				scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name,hpgain));
				return true;
			}
		}

		public static int pbJustRaiseEffortValues(IPokemon pokemon,Stats ev,int evgain) {
			int totalev=0;
			for (int i = 0; i < 6; i++) {
				totalev+=pokemon.EV[i];
			}
			if (totalev+evgain>Pokemon.EVLIMIT) {
				//  Bug Fix: must use "-=" instead of "="
				evgain-=totalev+evgain-Pokemon.EVLIMIT;
			}
			if (pokemon.EV[(int)ev]+evgain>Pokemon.EVSTATLIMIT) {
				//  Bug Fix: must use "-=" instead of "="
				evgain-=pokemon.EV[(int)ev]+evgain-Pokemon.EVSTATLIMIT;
			}
			if (evgain>0) {
				//pokemon.EV[ev]+=evgain;
				pokemon.EV[(int)ev]=(byte)(pokemon.EV[(int)ev]+evgain);
				pokemon.calcStats();
			}
			return evgain;
		}

		public static int pbRaiseEffortValues(IPokemon pokemon,Stats ev,int evgain=10,bool evlimit=true) {
			if (pokemon.EV[(int)ev]>=100 && evlimit) {
				return 0;
			}
			int totalev=0;
			for (int i = 0; i < 6; i++) {
				totalev+=pokemon.EV[i];
			}
			if (totalev+evgain>Pokemon.EVLIMIT) {
				evgain=Pokemon.EVLIMIT-totalev;
			}
			if (pokemon.EV[(int)ev]+evgain>Pokemon.EVSTATLIMIT) {
				evgain=Pokemon.EVSTATLIMIT-pokemon.EV[(int)ev];
			}
			if (evlimit && pokemon.EV[(int)ev]+evgain>100) {
				evgain=100-pokemon.EV[(int)ev];
			}
			if (evgain>0) {
				//pokemon.EV[ev]+=evgain;
				pokemon.EV[(int)ev]=(byte)(pokemon.EV[(int)ev]+evgain);
				pokemon.calcStats();
			}
			return evgain;
		}

		public static bool pbRaiseHappinessAndLowerEV(IPokemon pokemon,IScene scene,Stats ev,string[] messages) {
			bool h=(pokemon.Happiness<255);
			bool e=(pokemon.EV[(int)ev]>0);
			if (!h && !e) {
				scene.pbDisplay(Game._INTL("It won't have any effect."));
				return false;
			}
			if (h) {
				(pokemon as Pokemon).ChangeHappiness(HappinessMethods.EVBERRY);
			}
			if (e) {
				pokemon.EV[(int)ev]-=10;
				if (pokemon.EV[(int)ev]<0) pokemon.EV[(int)ev]=0;
				pokemon.calcStats();
			}
			scene.pbRefresh();
			scene.pbDisplay(messages[2-(h ? 0 : 1)-(e ? 0 : 2)]);
			return true;
		}

		public static int pbRestorePP(IPokemon pokemon,int move,int pp) {
			if (pokemon.moves[move].id==0) return 0;
			if (pokemon.moves[move].TotalPP==0) return 0;
			int newpp=pokemon.moves[move].PP+pp;
			if (newpp>pokemon.moves[move].TotalPP) {
				newpp=pokemon.moves[move].TotalPP;
			}
			int oldpp=pokemon.moves[move].PP;
			pokemon.moves[move].PP=(byte)newpp;
			return newpp-oldpp;
		}

		public static int pbBattleRestorePP(IPokemon pokemon,IBattler battler,int move,int pp) {
			int ret=pbRestorePP(pokemon,move,pp);
			if (ret>0) {
				if (battler.IsNotNullOrNone()) battler.pbSetPP(battler.moves[move],pokemon.moves[move].PP);
			}
			return ret;
		}

		public static bool pbBikeCheck() {
			if (Game.GameData.Global.surfing ||
				(!Game.GameData.Global.bicycle && Terrain.onlyWalk(Game.GameData is PokemonEssentials.Interface.Field.IGameField f ? f.pbGetTerrainTag() : (Terrains?)null))) {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("Can't use that here."));
				return false;
			}
			if (Game.GameData.GamePlayer.pbHasDependentEvents()) {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("It can't be used when you have someone with you."));
				return false;
			}
			if (Game.GameData.Global.bicycle) {
				//if (Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataBicycleAlways)) {
				if (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e ? e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.BicycleAlways : false) {
					if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("You can't dismount your Bike here."));
					return false;
				}
				return true;
			} else {
				//bool? val=Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataBicycle);
				bool? val=Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e0 ? e0.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Bicycle : (bool?)null;
				//if (val == null) val=Game.GameData.pbGetMetadata(Game.GameData.GameMap.map_id,MetadataOutdoor);
				if (val == null) val=Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e1 ? e1.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor : false;
				if (val == null) {
					if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("Can't use that here."));
					return false;
				}
				return true;
			}
		}

		public static IGameCharacter pbClosestHiddenItem() {
			List<IGameEvent> result = new List<IGameEvent>();
			float playerX=Game.GameData.GamePlayer.x;
			float playerY=Game.GameData.GamePlayer.y;
			foreach (IGameEvent @event in Game.GameData.GameMap.events.Values) {
				if (@event.name!="HiddenItem") continue;
				if (Math.Abs(playerX-@event.x)>=8) continue;
				if (Math.Abs(playerY-@event.y)>=6) continue;
				if (Game.GameData.GameSelfSwitches[new SelfSwitchVariable(Game.GameData.GameMap.map_id,@event.id,"A")]) continue;
				result.Add(@event);
			}
			if (result.Count==0) return null;
			IGameCharacter ret=null;
			float retmin=0;
			foreach (IGameEvent @event in result) {
				float dist=Math.Abs(playerX-@event.x)+Math.Abs(playerY-@event.y);
				if (ret == null || retmin>dist) {
					ret=@event;
					retmin=dist;
				}
			}
			return ret;
		}

		public static void pbUseKeyItemInField(Items item) { //Move to game class
			if (!ItemHandlers.triggerUseInField(item)) {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("Can't use that here."));
			}
		}

		public static bool pbSpeciesCompatible (Pokemons species,Moves move) {
			//bool ret=false;
			//if (species<=0) return false;
			//data=load_data("Data/tm.dat");
			//if (!data[move]) return false;
			//return data[move].Any(item => item==species);
			return Kernal.PokemonMovesData[species].Machine.Contains(move);
		}

		public static int pbForgetMove(IPokemon pokemon,Moves moveToLearn) {
			int ret=-1;
			if (Game.GameData is IGameSpriteWindow w) w.pbFadeOutIn(99999, block: () => {
				IPokemonSummaryScene scene= (Game.GameData as Game).Scenes.Summary; //new PokemonSummaryScene();
				IPokemonSummaryScreen screen=(Game.GameData as Game).Screens.Summary.initialize(scene); //new PokemonSummary(scene);
				ret=screen.pbStartForgetScreen(pokemon,0,moveToLearn);
			});
			return ret;
		}

		public static bool pbLearnMove(IPokemon pokemon,Moves move,bool ignoreifknown=false,bool bymachine=false) {
			if (!pokemon.IsNotNullOrNone()) return false;
			string movename=Game._INTL(move.ToString(TextScripts.Name));
			if (pokemon.isEgg && !Core.DEBUG) {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("{1} can't be taught to an Egg.",movename));
				return false;
			}
			if (pokemon is IPokemonShadowPokemon p && p.isShadow) {
				if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("{1} can't be taught to this Pokémon.",movename));
				return false;
			}
			string pkmnname=pokemon.Name;
			for (int i = 0; i < 4; i++) {
				if (pokemon.moves[i].id==move) {
					if (!ignoreifknown && Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("{1} already knows {2}.",pkmnname,movename));
					return false;
				}
				if (pokemon.moves[i].id==0) {
					pokemon.moves[i]=new Attack.Move(move);
					if (Game.GameData is IGameMessage m) m.pbMessage(Game._INTL("\\se[]{1} learned {2}!\\se[MoveLearnt]",pkmnname,movename));
					return true;
				}
			}
			do { //;loop
				if (Game.GameData is IGameMessage m0) m0.pbMessage(Game._INTL("{1} wants to learn the move {2}.",pkmnname,movename));
				if (Game.GameData is IGameMessage m1) m1.pbMessage(Game._INTL("However, {1} already knows four moves.",pkmnname));
				if (Game.GameData is IGameMessage m2 && m2.pbConfirmMessage(Game._INTL("Should a move be deleted and replaced with {1}?",movename))) {
					m2.pbMessage(Game._INTL("Which move should be forgotten?"));
					int forgetmove=pbForgetMove(pokemon,move);
					if (forgetmove>=0) {
						string oldmovename=Game._INTL(pokemon.moves[forgetmove].id.ToString(TextScripts.Name));
						int oldmovepp=pokemon.moves[forgetmove].PP;
						pokemon.moves[forgetmove]=new Attack.Move(move); // Replaces current/total PP
						if (bymachine) pokemon.moves[forgetmove].PP=Math.Min(oldmovepp,pokemon.moves[forgetmove].TotalPP);
						m2.pbMessage(Game._INTL("\\se[]1,\\wt[16] 2, and\\wt[16]...\\wt[16] ...\\wt[16] ... Ta-da!\\se[balldrop]"));
						m2.pbMessage(Game._INTL("\\se[]{1} forgot how to use {2}. And... {1} learned {3}!\\se[MoveLearnt]",pkmnname,oldmovename,movename));
						return true;
					} else if (m2.pbConfirmMessage(Game._INTL("Give up on learning the move {1}?",movename))) {
						m2.pbMessage(Game._INTL("{1} did not learn {2}.",pkmnname,movename));
						return false;
					}
				} else if (Game.GameData is IGameMessage m3 && m3.pbConfirmMessage(Game._INTL("Give up on learning the move {1}?",movename))) {
					m3.pbMessage(Game._INTL("{1} did not learn {2}.",pkmnname,movename));
					return false;
				}
			} while (true);
		}

		public static bool pbCheckUseOnPokemon(Items item,IPokemon pokemon,IScreen screen) {
			return pokemon.IsNotNullOrNone() && !pokemon.isEgg;
		}

		public static bool pbConsumeItemInBattle(PokemonBag bag,Items item) {
			if (item!=0 && Kernal.ItemData[item].Flags.Consumable && //!=3 disappear after use
				//Kernal.ItemData[item].Flags!=4 && //used on enemy and disappears after use (i.e. pokeball)
				Kernal.ItemData[item].Flags.Useable_In_Battle) { //!=0 cannot be used in battle
				//  Delete the item just used from stock
				return Game.GameData.Bag.pbDeleteItem(item);
			}
			return false;
		}

		// Only called when in the party screen and having chosen an item to be used on
		// the selected Pokémon
		/*public static bool pbUseItemOnPokemon(Items item,IPokemon pokemon,IScene scene) {
			//if (Kernal.ItemData[item][ITEMUSE]==3 || Kernal.ItemData[item][ITEMUSE]==4) {		// TM or HM
			if (Item.pbIsMachine(item)) {
				Moves machine=Kernal.MachineData[(int)item].Move;
				if (machine==Moves.NONE) return false;
				string movename=Game._INTL(machine.ToString(TextScripts.Name));
				if (pokemon.isShadow) { //? rescue false
					Game.GameData.pbMessage(Game._INTL("Shadow Pokémon can't be taught any moves."));
				} else if (!pokemon.isCompatibleWithMove(machine)) {
					Game.GameData.pbMessage(Game._INTL("{1} and {2} are not compatible.",pokemon.Name,movename));
					Game.GameData.pbMessage(Game._INTL("{1} can't be learned.",movename));
				} else {
					if (pbIsHiddenMachine(item)) {
						Game.GameData.pbMessage(Game._INTL("\\se[accesspc]Booted up an HM."));
						Game.GameData.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
					} else {
						Game.GameData.pbMessage(Game._INTL("\\se[accesspc]Booted up a TM."));
						Game.GameData.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
					}
					if (Game.GameData.pbConfirmMessage(Game._INTL("Teach {1} to {2}?",movename,pokemon.Name))) {
						if (pbLearnMove(pokemon,machine,false,true)) {
							if (pbIsTechnicalMachine(item) && !Core.INFINITETMS) Game.GameData.Bag.pbDeleteItem(item);
							return true;
						}
					}
				}
				return false;
			} else {
				bool ret=ItemHandlers.triggerUseOnPokemon(item,pokemon,scene);
				scene.pbClearAnnotations();
				scene.pbHardRefresh();
				if (ret && Kernal.ItemData[item].Flags.Consumable) {		//[ITEMUSE]==1 Usable on Pokémon, consumed
					Game.GameData.Bag.pbDeleteItem(item);
				}
				if (Game.GameData.Bag.pbQuantity(item)<=0) {
					Game.GameData.pbMessage(Game._INTL("You used your last {1}.",Game._INTL(item.ToString(TextScripts.Name))));
				}
				return ret;
			}
			Game.GameData.pbMessage(Game._INTL("Can't use that on {1}.",pokemon.Name));
			return false;
		}

		public static int pbUseItem(PokemonBag bag,Items item,IScene bagscene=null) {
			//bool found=false;
			//if (Kernal.ItemData[item][ITEMUSE]==3 || Kernal.ItemData[item][ITEMUSE]==4) {		// TM or HM
			if (Item.pbIsMachine(item)) {
			Moves machine=Kernal.MachineData[(int)item].Move;
			if (machine==Moves.NONE) return 0;
			if (Game.GameData.Trainer.pokemonCount==0) {
				Game.GameData.pbMessage(Game._INTL("There is no Pokémon."));
				return 0;
			}
			string movename=Game._INTL(machine.ToString(TextScripts.Name));
			if (pbIsHiddenMachine(item)) {
				Game.GameData.pbMessage(Game._INTL("\\se[accesspc]Booted up an HM."));
				Game.GameData.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
			} else {
				Game.GameData.pbMessage(Game._INTL("\\se[accesspc]Booted up a TM."));
				Game.GameData.pbMessage(Game._INTL(@"It contained {1}.\1",movename));
			}
			if (!Game.GameData.pbConfirmMessage(Game._INTL("Teach {1} to a Pokémon?",movename))) {
				return 0;
			} else if (Game.pbMoveTutorChoose(machine,null,true)) {
				if (pbIsTechnicalMachine(item) && !Core.INFINITETMS) bag.pbDeleteItem(item);
				return 1;
			} else {
				return 0;
			}
			} else if (Kernal.ItemData[item].Flags.Consumable || Kernal.ItemData[item][ITEMUSE]==5) {		//[ITEMUSE]==1| Item is usable on a Pokémon
			if (Game.GameData.Trainer.pokemonCount==0) {
				Game.GameData.pbMessage(Game._INTL("There is no Pokémon."));
				return 0;
			}
			bool ret=false;
			List<string> annot=null;
			if (pbIsEvolutionStone(item)) {
				annot=new List<string>();
				foreach (var pkmn in Game.GameData.Trainer.party) {
				bool elig=(Evolution.pbCheckEvolution(pkmn,item).Length>0);
				annot.Add(elig ? Game._INTL("ABLE") : Game._INTL("NOT ABLE"));
				}
			}
			if (Game.GameData is IGameSpriteWindow w) w.pbFadeOutIn(99999, block: () => {
				IPokemonScreen_Scene scene=Game.PokemonScreenScene.initialize(); //new PokemonScreen_Scene();
				IPokemonScreen screen=Game.PokemonScreen.initialize(scene,Game.GameData.Trainer.party); //new PokemonScreen(scene,Game.GameData.Trainer.party);
				screen.pbStartScene(Game._INTL("Use on which Pokémon?"),false,annot);
				do { //;loop
					scene.pbSetHelpText(Game._INTL("Use on which Pokémon?"));
					int chosen=screen.pbChoosePokemon();
					if (chosen>=0) {
					IPokemon pokemon=Game.GameData.Trainer.party[chosen];
					if (!pbCheckUseOnPokemon(item,pokemon,screen)) {
						Game.UI.pbPlayBuzzerSE();
					} else {
						ret=ItemHandlers.triggerUseOnPokemon(item,pokemon,screen);
						if (ret && Kernal.ItemData[item].Flags.Consumable) {		//[ITEMUSE]==1 Usable on Pokémon, consumed
							bag.pbDeleteItem(item);
						}
						if (bag.pbQuantity(item)<=0) {
							Game.GameData.pbMessage(Game._INTL("You used your last {1}.",Game._INTL(item.ToString(TextScripts.Name))));
							break;
						}
					}
					} else {
					ret=false;
					break;
					}
				} while (true);
				screen.pbEndScene();
				if (bagscene!=null) bagscene.pbRefresh();
			});
			return ret ? 1 : 0;
			} else if (Kernal.ItemData[item][ITEMUSE]==2) {		// Item is usable from bag
				int intret=(int)ItemHandlers.triggerUseFromBag(item);
				switch (intret) {
					case 0:
						return 0;
						break;
					case 1: // Item used
						return 1;
						break;
					case 2: // Item used, end screen
						return 2;
						break;
					case 3: // Item used, consume item
						bag.pbDeleteItem(item);
						return 1;
						break;
					case 4: // Item used, end screen and consume item
						bag.pbDeleteItem(item);
						return 2;
						break;
					default:
						Game.GameData.pbMessage(Game._INTL("Can't use that here."));
						return 0;
				}
			} else {
				Game.GameData.pbMessage(Game._INTL("Can't use that here."));
				return 0;
			}
		}

		public static Items pbChooseItem(int var=0,params Items[] args) {
			Items ret=0; //int?
			IPokemonBag_Scene scene=Game.PokemonBagScene; //new PokemonBag_Scene();
			IPokemonBagScreen screen=Game.PokemonBagScreen.initialize(scene,Game.GameData.Bag); //new PokemonBagScreen(scene,Game.GameData.Bag);
			Game.UI.pbFadeOutIn(99999, block: () => { 
				ret=screen.pbChooseItemScreen();
			});
			if (var>0) Game.GameData.GameVariables[var]=ret;
			return ret;
		}*/

		/// <summary>
		/// Shows a list of items to choose from, with the chosen item's ID being stored
		/// in the given Global Variable. Only items which the player has are listed.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="variable"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static Items pbChooseItemFromList(string message,int variable,params Items[] args) {
			List<string> commands=new List<string>();
			List<Items> itemid=new List<Items>();
			foreach (Items item in args) {
				//if (hasConst(PBItems,item)) {
					Items id=(Items)item;
					if (Game.GameData.Bag.pbQuantity(id)>0) {
						commands.Add(Game._INTL(id.ToString(TextScripts.Name)));
						itemid.Add(id);
					}
				//}
			}
			if (commands.Count==0) {
				Game.GameData.GameVariables[variable]=0;
				return 0;
			}
			commands.Add(Game._INTL("Cancel"));
			itemid.Add(0);
			int ret=Game.GameData is IGameMessage m ? m.pbMessage(message,commands.ToArray(),-1) : -1;
			if (ret<0 || ret>=commands.Count-1) {
				Game.GameData.GameVariables[variable]=-1;
				return Items.NONE;
			} else {
				Game.GameData.GameVariables[variable]=itemid[ret];
				return itemid[ret];
			}
		}

		private static void pbTopRightWindow(string text) {
			IWindow_AdvancedTextPokemon window = null; //new Window_AdvancedTextPokemon(text);
			window.z=99999;
			window.width=198;
			window.y=0;
			window.x=(Game.GameData as Game).Graphics.width-window.width;
			if (Game.GameData is IGameAudioPlay a) a.pbPlayDecisionSE();
			do { //;loop
				//Graphics?.update();
				Input.update();
				window.update();
				if (Input.trigger(Input.A)) {
					break;
				}
			} while (true);
			window.dispose();
		}
		#endregion
	}

	//public partial class ItemHandlerHash : HandlerHash {
	//  public void initialize() {
	//    super(:PBItems);
	//  }
	//}

	//Use `EventHandlerList` and replace dictionary/delegate with events? 
	public static partial class ItemHandlers {
		//private static ItemHandlerHash UseFromBag=new ItemHandlerHash();
		//private static ItemHandlerHash UseInField=new ItemHandlerHash();
		//private static ItemHandlerHash UseOnPokemon=new ItemHandlerHash();
		//private static ItemHandlerHash BattleUseOnBattler=new ItemHandlerHash();
		//private static ItemHandlerHash BattleUseOnPokemon=new ItemHandlerHash();
		//private static ItemHandlerHash UseInBattle=new ItemHandlerHash();
		private static Dictionary<Items,Func<ItemUseResults>> UseFromBag=new Dictionary<Items, Func<ItemUseResults>>();
		private static Dictionary<Items,Action> UseInField=new Dictionary<Items, Action>();
		private static Dictionary<Items,UseOnPokemonDelegate> UseOnPokemon=new Dictionary<Items, UseOnPokemonDelegate>();
		private static Dictionary<Items,BattleUseOnBattlerDelegate> BattleUseOnBattler=new Dictionary<Items, BattleUseOnBattlerDelegate>();
		private static Dictionary<Items,BattleUseOnPokemonDelegate> BattleUseOnPokemon=new Dictionary<Items, BattleUseOnPokemonDelegate>();
		private static Dictionary<Items,UseInBattleDelegate> UseInBattle=new Dictionary<Items, UseInBattleDelegate>();
		//public delegate bool UseOnPokemonDelegate(Items item, IPokemon pokemon, IHasDisplayMessage scene);
		//public delegate bool BattleUseOnPokemonDelegate(IPokemon pokemon, IBattler battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
		//public delegate bool BattleUseOnBattlerDelegate(Items item, IBattler battler, IPokeBattle_Scene scene); //or Pokemon Party Scene
		//public delegate void UseInBattleDelegate(Items item, IBattler battler, IBattle battle);

		public static void addUseFromBag(Items item,Func<ItemUseResults> proc) {
			if (!UseFromBag.ContainsKey(item))
				UseFromBag.Add(item,proc);
			else
				UseFromBag[item] = proc;
		}

		public static void addUseInField(Items item,Action proc) {
			if (!UseInField.ContainsKey(item))
				UseInField.Add(item,proc);
			else
				UseInField[item] = proc;
		}

		public static void addUseOnPokemon(Items item, UseOnPokemonDelegate proc) {
			if (!UseOnPokemon.ContainsKey(item))
				UseOnPokemon.Add(item,proc);
			else
				UseOnPokemon[item] = proc;
		}

		public static void addBattleUseOnBattler(Items item, BattleUseOnBattlerDelegate proc) {
			if (!BattleUseOnBattler.ContainsKey(item))
				BattleUseOnBattler.Add(item,proc);
			else
				BattleUseOnBattler[item] = proc;
		}

		public static void addBattleUseOnPokemon(Items item, BattleUseOnPokemonDelegate proc) {
			if (!BattleUseOnPokemon.ContainsKey(item))
				BattleUseOnPokemon.Add(item,proc);
			else
				BattleUseOnPokemon[item] = proc;
		}

		/// <summary>
		/// Shows "Use" option in Bag
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool hasOutHandler(Items item) {                       
			return (!UseFromBag.ContainsKey(item) && UseFromBag[item]!=null) || (!UseOnPokemon.ContainsKey(item) && UseOnPokemon[item]!=null);
		}

		/// <summary>
		/// Shows "Register" option in Bag
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool hasKeyItemHandler(Items item) {              
			return !UseInField.ContainsKey(item) && UseInField[item]!=null;
		}

		public static bool hasUseOnPokemon(Items item) {
			return !UseOnPokemon.ContainsKey(item) && UseOnPokemon[item]!=null;
		}

		public static bool hasBattleUseOnBattler(Items item) {
			return !BattleUseOnBattler.ContainsKey(item) && BattleUseOnBattler[item]!=null;
		}

		public static bool hasBattleUseOnPokemon(Items item) {
			return !BattleUseOnPokemon.ContainsKey(item) && BattleUseOnPokemon[item]!=null;
		}

		public static bool hasUseInBattle(Items item) {
			return !UseInBattle.ContainsKey(item) && UseInBattle[item]!=null;
			//return Kernal.ItemData[item].Flags.Useable_In_Battle;
		}

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		/// <returns>Return values: 0 = not used
		///                         1 = used, item not consumed
		///                         2 = close the Bag to use, item not consumed
		///                         3 = used, item consumed
		///                         4 = close the Bag to use, item consumed</returns>
		public static ItemUseResults triggerUseFromBag(Items item) {
			//  Return value:
			//  0 - Item not used
			//  1 - Item used, don't end screen
			//  2 - Item used, end screen
			//  3 - Item used, consume item
			//  4 - Item used, end screen, consume item
			if (!UseFromBag.ContainsKey(item) || UseFromBag[item] == null) { //Search List
				// Check the UseInField handler if present
				if (UseInField[item] != null) {
				//if (!Kernal.ItemData[item].Flags.Useable_Overworld) {
					UseInField[item].Invoke();
					//ItemHandlers.UseInField(item);
					return ItemUseResults.UsedNotConsumed; // item was used
				}
				return 0; // item was not used
			} else {
				return UseFromBag[item].Invoke();
				//return ItemHandlers.UseFromBag(item);
			}
		}

		public static bool triggerUseInField(Items item) {
			// No return value
			if (!UseInField.ContainsKey(item) || UseInField[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_Overworld) {
				return false;
			} else {
				UseInField[item].Invoke();
				//ItemHandlers.UseInField(item);
				return true;
			}
		}

		public static bool triggerUseOnPokemon(Items item,IPokemon pokemon,IHasDisplayMessage scene) {
			// Returns whether item was used
			if (!UseOnPokemon.ContainsKey(item) || UseOnPokemon[item] == null) { //Search List
				return false;
			} else {
				//return UseOnPokemon[item].Invoke(pokemon,scene);
				return (bool)UseOnPokemon[item].Invoke(item,pokemon,scene);
				//return ItemHandlers.UseOnPokemon(item,pokemon,scene);
			}
		}

		//ToDo: scene parameter is throwing errors in battle class, resolve in itemhandler method
		public static bool triggerBattleUseOnBattler(Items item,IBattler battler,IHasDisplayMessage scene) { return false; }
		public static bool triggerBattleUseOnPokemon(Items item,IPokemon pokemon,IBattler battler, IHasDisplayMessage scene) { return false; }

		public static bool triggerBattleUseOnBattler(Items item,IBattler battler,IPokeBattle_Scene scene) {
			// Returns whether item was used
			if (!BattleUseOnBattler.ContainsKey(item) || BattleUseOnBattler[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_In_Battle) {
				return false;
			} else {
				return BattleUseOnBattler[item].Invoke(item,battler,scene);
				//return ItemHandlers.BattleUseOnBattler(item,battler,scene);
			}
		}

		public static bool triggerBattleUseOnPokemon(Items item,IPokemon pokemon,IBattler battler,IPokeBattle_Scene scene) {
			// Returns whether item was used
			if (!BattleUseOnPokemon.ContainsKey(item) || BattleUseOnPokemon[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_In_Battle) {
				return false;
			} else {
				return BattleUseOnPokemon[item].Invoke(pokemon,battler,scene);
				//return ItemHandlers.BattleUseOnPokemon(item,pokemon,battler,scene);
			}
		}

		public static void triggerUseInBattle(Items item,IBattler battler,PokemonUnity.Combat.Battle battle) {
			// Returns whether item was used
			if (!UseInBattle.ContainsKey(item) || UseInBattle[item] == null) {
			//if (!Kernal.ItemData[item].Flags.Useable_In_Battle) {
				return;
			} else {
				UseInBattle[item].Invoke(item,battler,battle);
				//ItemHandlers.UseInBattle(item,battler,battle);
			}
		}

		//static ItemHandlers() {
		/*private static void RegisterItemHandlers() {
			//Events.OnStepTaken+=OnStepTakenEventHandler;
			#region UseFromBag handlers
			UseFromBag.Add(Items.REPEL, () => { return pbRepel(Items.REPEL, 100); });
			UseFromBag.Add(Items.SUPER_REPEL, () => { return pbRepel(Items.SUPER_REPEL,200); });
			UseFromBag.Add(Items.MAX_REPEL, () => { return pbRepel(Items.MAX_REPEL,250); });
			UseFromBag.Add(Items.BLACK_FLUTE, () => {
				(this as IGameMessage).pbMessage(Game._INTL("{1} used the {2}.",Player.Name,Game._INTL(Items.BLACK_FLUTE.ToString(TextScripts.Name))));
				(this as IGameMessage).pbMessage(Game._INTL("Wild Pokémon will be repelled."));
				MapData.blackFluteUsed=true;
				MapData.whiteFluteUsed=false;
				//next 1;
				return ItemUseResults.UsedNotConsumed;
			});
			UseFromBag.Add(Items.WHITE_FLUTE, () => {
				(this as IGameMessage).pbMessage(Game._INTL("{1} used the {2}.",Player.Name,Game._INTL(Items.WHITE_FLUTE.ToString(TextScripts.Name))));
				(this as IGameMessage).pbMessage(Game._INTL("Wild Pokémon will be lured."));
				MapData.blackFluteUsed=false;
				MapData.whiteFluteUsed=true;
				//next 1;
				return ItemUseResults.UsedNotConsumed;
			});
			UseFromBag.Add(Items.HONEY, () => { return ItemUseResults.CloseBagItemConsumed; });
			UseFromBag.Add(Items.ESCAPE_ROPE, () => {
				if (GamePlayer.pbHasDependentEvents()) {
					(this as IGameMessage).pbMessage(Game._INTL("It can't be used when you have someone with you."));
					//next 0;
					return ItemUseResults.NotUsed;
				}
				if ((Global.escapePoint != null) && Global.escapePoint.Length>0) {
					//next 4; // End screen and consume item
					return ItemUseResults.CloseBagItemConsumed;
				} else {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next 0;
					return ItemUseResults.NotUsed;
				}
			});
			UseFromBag.Add(Items.SACRED_ASH, () => {
				int revived=0;
				if (Trainer.pokemonCount==0) {
					(this as IGameMessage).pbMessage(Game._INTL("There is no Pokémon."));
					//next 0;
					return ItemUseResults.NotUsed;
				}
				//ToDo: Redo below into an Event Listener (Subscribe to on Frontend)
				pbFadeOutIn(99999, block: () => {
					IPartyDsplayScene scene=Scenes.Party; //new PokemonScreen_Scene();
					IPartyDsplayScreen screen = Screens.Party.initialize(scene,Trainer.party); //new PokemonScreen(scene,Trainer.party);
					screen.pbStartScene(Game._INTL("Using item..."),false);
					foreach (Pokemon i in Trainer.party) {
						if (i.HP<=0 && !i.isEgg) {
							revived+=1;
							i.Heal();
							screen.pbDisplay(Game._INTL("{1}'s HP was restored.",i.Name));
						}
					}
					if (revived==0) {
						screen.pbDisplay(Game._INTL("It won't have any effect."));
					}
					screen.pbEndScene();
				});
				//next (revived==0) ? 0 : 3;
				return (revived==0) ? ItemUseResults.NotUsed : ItemUseResults.UsedItemConsumed;
			});
			UseFromBag.Add(Items.BICYCLE, () => {
				//next pbBikeCheck ? 2 : 0;
				return pbBikeCheck() ? ItemUseResults.CloseBagNotConsumed : ItemUseResults.NotUsed;
				//return ItemUseResults.NotUsed;
			});
			UseFromBag.Add(Items.MACH_BIKE, () => {
				//next pbBikeCheck ? 2 : 0;
				return pbBikeCheck() ? ItemUseResults.CloseBagNotConsumed : ItemUseResults.NotUsed;
				//return ItemUseResults.NotUsed;
			});
			UseFromBag.Add(Items.ACRO_BIKE, () => {
				//next pbBikeCheck ? 2 : 0;
				return pbBikeCheck() ? ItemUseResults.CloseBagNotConsumed : ItemUseResults.NotUsed;
				//return ItemUseResults.NotUsed;
			});
			UseFromBag.Add(Items.OLD_ROD, () => {
				Terrains terrain=pbFacingTerrainTag();
				bool notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
				if ((Terrain.isWater(terrain) && !Global.surfing && notCliff) ||
					(Terrain.isWater(terrain) && Global.surfing)) {
					//next 2;
					return ItemUseResults.CloseBagNotConsumed;
				} else {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next 0;
					return ItemUseResults.NotUsed;
				}
			});
			UseFromBag.Add(Items.GOOD_ROD, () => {
				Terrains terrain=pbFacingTerrainTag();
				bool notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
				if ((Terrain.isWater(terrain) && !Global.surfing && notCliff) ||
					(Terrain.isWater(terrain) && Global.surfing)) {
					//next 2;
					return ItemUseResults.CloseBagNotConsumed;
				} else {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next 0;
					return ItemUseResults.NotUsed;
				}
			});
			UseFromBag.Add(Items.SUPER_ROD, () => {
				Terrains terrain=pbFacingTerrainTag();
				bool notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
				if ((Terrain.isWater(terrain) && !Global.surfing && notCliff) ||
					(Terrain.isWater(terrain) && Global.surfing)) {
					//next 2;
					return ItemUseResults.CloseBagNotConsumed;
				} else {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next 0;
					return ItemUseResults.NotUsed;
				}
			}); //ToDo: Add items to Enum?...
			//UseFromBag.Add(Items.ITEM_FINDER, () => { return ItemUseResults.CloseBagNotConsumed; });
			UseFromBag.Add(Items.DOWSING_MACHINE, () => { return ItemUseResults.CloseBagNotConsumed; });
			UseFromBag.Add(Items.TOWN_MAP, () => {
				pbShowMap(-1,false);
				//next 1; // Continue
				return ItemUseResults.UsedNotConsumed; 
			});
			UseFromBag.Add(Items.COIN_CASE, () => {
				(this as IGameMessage).pbMessage(Game._INTL("Coins: {1}",Player.Coins));
				//next 1; // Continue
				return ItemUseResults.UsedNotConsumed; 
			});
			UseFromBag.Add(Items.EXP_ALL, () => {
				Bag.pbChangeItem(Items.EXP_ALL, Items.EXP_ALL_OFF);
				(this as IGameMessage).pbMessage(Game._INTL("The Exp Share was turned off."));
				//next 1; // Continue
				return ItemUseResults.UsedNotConsumed; 
			});
			UseFromBag.Add(Items.EXP_ALL_OFF, () => {
				Bag.pbChangeItem(Items.EXP_ALL_OFF, Items.EXP_ALL);
				(this as IGameMessage).pbMessage(Game._INTL("The Exp Share was turned on."));
				//next 1; // Continue
				return ItemUseResults.UsedNotConsumed; 
			});
			#endregion

			#region UseInField handlers
			UseInField.Add(Items.HONEY, () => {  
				(this as IGameMessage).pbMessage(Game._INTL("{1} used the {2}.",Player.Name,Game._INTL(Items.HONEY.ToString(TextScripts.Name))));
				pbSweetScent();
			});
			UseInField.Add(Items.ESCAPE_ROPE, () => {
				int[] escape=Global.escapePoint; //rescue null
				if (escape == null || escape.Length==0) {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next;
					return;
				}
				if (GamePlayer.pbHasDependentEvents()) {
					(this as IGameMessage).pbMessage(Game._INTL("It can't be used when you have someone with you."));
					//next;
					return;
				}
				(this as IGameMessage).pbMessage(Game._INTL("{1} used the {2}.",Player.Name,Game._INTL(item.ToString(TextScripts.Name))));
				pbFadeOutIn(99999, block: () => {
					pbCancelVehicles();
					GameTemp.player_new_map_id=escape[0];
					GameTemp.player_new_x=escape[1];
					GameTemp.player_new_y=escape[2];
					GameTemp.player_new_direction=escape[3];
					Scene.transfer_player();
					GameMap.autoplay();
					GameMap.refresh();
				});
				pbEraseEscapePoint();
			});

			UseInField.Add(Items.BICYCLE, () => {
				if (pbBikeCheck()) {
					if (Global.bicycle) {
						pbDismountBike();
					} else {
						pbMountBike();
					}
				}
			});
			UseInField.Add(Items.MACH_BIKE, () => {
				if (pbBikeCheck()) {
					if (Global.bicycle) {
						pbDismountBike();
					} else {
						pbMountBike();
					}
				}
			});
			UseInField.Add(Items.ACRO_BIKE, () => {
				if (pbBikeCheck()) {
					if (Global.bicycle) {
						pbDismountBike();
					} else {
						pbMountBike();
					}
				}
			});

			UseInField.Add(Items.OLD_ROD, () => {
				Terrains terrain=pbFacingTerrainTag();
				bool notCliff=GameMap.passable/(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
				if (!Terrain.isWater(terrain) || (!notCliff && !Global.surfing)) {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next;
					return;
				}
				bool encounter=PokemonEncounters.hasEncounter(EncounterTypes.OldRod);
				if (pbFishing(encounter,1)) {
					pbEncounter(EncounterTypes.OldRod);
				}
			});

			UseInField.Add(Items.GOOD_ROD, () => {
				Terrains terrain=pbFacingTerrainTag();
				bool notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
				if (!Terrain.isWater(terrain) || (!notCliff && !Global.surfing)) {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next;
					return;
				}
				bool encounter=PokemonEncounters.hasEncounter(EncounterTypes.GoodRod);
				if (pbFishing(encounter,2)) {
					pbEncounter(EncounterTypes.GoodRod);
				}
			});

			UseInField.Add(Items.SUPER_ROD, () => {
				Terrains terrain=pbFacingTerrainTag();
				bool notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
				if (!Terrain.isWater(terrain) || (!notCliff && !Global.surfing)) {
					(this as IGameMessage).pbMessage(Game._INTL("Can't use that here."));
					//next;
					return;
				}
				bool encounter=PokemonEncounters.hasEncounter(EncounterTypes.SuperRod);
				if (pbFishing(encounter,3)) {
					pbEncounter(EncounterTypes.SuperRod);
				}
			});

			UseInField.Add(Items.DOWSING_MACHINE, () => {//item == Items.ITEM_FINDER || item == Items.DOWSING_MCHN || 
				@event=Game.GameData is IItemCheck i && i.pbClosestHiddenItem();
				if (@event == null) {
					(this as IGameMessage).pbMessage(Game._INTL("... ... ... ...Nope!\r\nThere's no response."));
				} else {
					int offsetX=@event.x-GamePlayer.x;
					int offsetY=@event.y-GamePlayer.y;
					if (offsetX==0 && offsetY==0) {
						for (int i = 0; i < 32; i++) {
							Graphics?.update();
							Input.update();
							if ((i&7)==0) GamePlayer.turn_right_90();
							pbUpdateSceneMap();
						}
						(this as IGameMessage).pbMessage(Game._INTL(@"The {1}'s indicating something right underfoot!\1",Game._INTL(item.ToString(TextScripts.Name))));
					} else {
						int direction=GamePlayer.direction;
						if (Math.Abs(offsetX)>Math.Abs(offsetY)) {
							direction=(offsetX<0) ? 4 : 6;
						} else {
							direction=(offsetY<0) ? 8 : 2;
						}
						for (int i = 0; i < 8; i++) {
							Graphics?.update();
							Input.update();
							if (i==0) {
								if (direction==2) GamePlayer.turn_down();
								if (direction==4) GamePlayer.turn_left();
								if (direction==6) GamePlayer.turn_right();
								if (direction==8) GamePlayer.turn_up();
							}
							pbUpdateSceneMap();
						}
						(this as IGameMessage).pbMessage(Game._INTL(@"Huh?\nThe {1}'s responding!\1",Game._INTL(item.ToString(TextScripts.Name))));
						(this as IGameMessage).pbMessage(Game._INTL("There's an item buried around here!"));
					}
				}
			});

			UseInField.Add(Items.TOWN_MAP, () => {
				pbShowMap(-1,false);
			});

			UseInField.Add(Items.COIN_CASE, () => {
				(this as IGameMessage).pbMessage(Game._INTL("Coins: {1}",Player.Coins));
				//next 1; // Continue
				//return ItemUseResults.UsedNotConsumed;
			});
			#endregion

			//ToDo: If use berry, increase/decrease happiness?
			#region UseOnPokemon handlers
			UseOnPokemon.Add(Items.FIRE_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.THUNDER_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.WATER_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.LEAF_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.MOON_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.SUN_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.DUSK_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.DAWN_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IBagScene s) { //IPokemonBag_Scene
							//s.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							s.pbRefreshAnnotations((p) => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							s.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.SHINY_STONE, (item, pokemon, scene) => {
				if (pokemon.isShadow) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				}
				Pokemons newspecies=Pokemons.NONE; //Evolution.pbCheckEvolution(pokemon,item)[0];
				if (newspecies<=0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					//ToDo: Add a check to cycle through all evolves
					pbFadeOutInWithMusic(99999, block: () => {
						IPokemonEvolutionScene evo= Scenes.EvolvingScene; //new PokemonEvolutionScene();
						evo.pbStartScreen(pokemon,newspecies);
						evo.pbEvolution(false);
						evo.pbEndScreen();
						if (scene is IPokemonBag_Scene) {
							//scene.pbRefreshAnnotations(proc{|p| Evolution.pbCheckEvolution(p,item)[0]>0 }
							scene.pbRefreshAnnotations(() => { Evolution.pbCheckEvolution(p, item)[0] > 0; });
							scene.pbRefresh();
						}
					});
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.POTION, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,20,scene);
				return Game.GameData is IGameItem i && i.pbHPItem(pokemon,20,scene);
			});

			UseOnPokemon.Add(Items.SUPER_POTION, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,50,scene);
				return Item.pbHPItem(pokemon,50,scene);
			});

			UseOnPokemon.Add(Items.HYPER_POTION, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,200,scene);
				return Item.pbHPItem(pokemon,200,scene);
			});

			UseOnPokemon.Add(Items.MAX_POTION, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,pokemon.TotalHP-pokemon.HP,scene);
				return Item.pbHPItem(pokemon,pokemon.TotalHP-pokemon.HP,scene);
			});

			UseOnPokemon.Add(Items.BERRY_JUICE, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,20,scene);
				return Item.pbHPItem(pokemon,20,scene);
			});

			UseOnPokemon.Add(Items.RAGE_CANDY_BAR, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,20,scene);
				return Item.pbHPItem(pokemon,20,scene);
			});

			UseOnPokemon.Add(Items.SWEET_HEART, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,20,scene);
				return Item.pbHPItem(pokemon,20,scene);
			});

			UseOnPokemon.Add(Items.FRESH_WATER, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,50,scene);
				return Item.pbHPItem(pokemon,50,scene);
			});

			UseOnPokemon.Add(Items.SODA_POP, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,60,scene);
				return Item.pbHPItem(pokemon,60,scene);
			});

			UseOnPokemon.Add(Items.LEMONADE, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,80,scene);
				return Item.pbHPItem(pokemon,80,scene);
			});

			UseOnPokemon.Add(Items.MOOMOO_MILK, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,100,scene);
				return Item.pbHPItem(pokemon,100,scene);
			});

			UseOnPokemon.Add(Items.ORAN_BERRY, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,10,scene);
				return Item.pbHPItem(pokemon,10,scene);
			});

			UseOnPokemon.Add(Items.SITRUS_BERRY, (item, pokemon, scene) => {
				//next pbHPItem(pokemon,Math.Floor(pokemon.TotalHP/4),scene);
				return Item.pbHPItem(pokemon,(int)Math.Floor(pokemon.TotalHP/4f),scene);
			});

			UseOnPokemon.Add(Items.AWAKENING, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.CHESTO_BERRY, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.BLUE_FLUTE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.POKE_FLUTE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.ANTIDOTE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.PECHA_BERRY, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.BURN_HEAL, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s burn was healed.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.RAWST_BERRY, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s burn was healed.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.PARALYZE_HEAL, (item, pokemon, scene) => {//item == Items.PARLYZHEAL || 
				if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.CHERI_BERRY, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.ICE_HEAL, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was thawed out.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.ASPEAR_BERRY, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was thawed out.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.FULL_HEAL, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.LAVA_COOKIE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.OLD_GATEAU, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.CASTELIACONE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.LUMIOSE_GALETTE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.SHALOUR_SABLE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});
			UseOnPokemon.Add(Items.LUM_BERRY, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.FULL_RESTORE, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || (pokemon.HP==pokemon.TotalHP && pokemon.Status==0)) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					int hpgain=Item.pbItemRestoreHP(pokemon,pokemon.TotalHP-pokemon.HP);
					pokemon.HealStatus();
					scene.pbRefresh();
					if (hpgain>0) {
					scene.pbDisplay(Game._INTL("{1}'s HP was restored by {2} points.",pokemon.Name,hpgain));
					} else {
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					}
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.REVIVE, (item, pokemon, scene) => {
				if (pokemon.HP>0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HP=(int)Math.Floor(pokemon.TotalHP/2f);
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.MAX_REVIVE, (item, pokemon, scene) => {
				if (pokemon.HP>0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealHP();
					pokemon.HealStatus();
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.ENERGY_POWDER, (item, pokemon, scene) => {
				if (Item.pbHPItem(pokemon,50,scene)) {
					pokemon.ChangeHappiness(HappinessMethods.POWDER);
					//next true;
					return true;
				}
				//next false;
				return false;
			});

			UseOnPokemon.Add(Items.ENERGY_ROOT, (item, pokemon, scene) => {
				if (Item.pbHPItem(pokemon,200,scene)) {
					pokemon.ChangeHappiness(HappinessMethods.ENERGYROOT);
					//next true;
					return true;
				}
				//next false;
				return false;
			});

			UseOnPokemon.Add(Items.HEAL_POWDER, (item, pokemon, scene) => {
				if (pokemon.HP<=0 || pokemon.Status==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealStatus();
					pokemon.ChangeHappiness(HappinessMethods.POWDER);
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.REVIVAL_HERB, (item, pokemon, scene) => {
				if (pokemon.HP>0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					//next false;
					return false;
				} else {
					pokemon.HealHP();
					pokemon.HealStatus();
					pokemon.ChangeHappiness(HappinessMethods.REVIVALHERB);
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name));
					//next true;
					return true;
				}
			});

			UseOnPokemon.Add(Items.ETHER, (item, pokemon, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Restore which move?"));
				if (move>=0) {
					if (Item.pbRestorePP(pokemon,move,10)==0) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						//next false;
						return false;
					} else {
						scene.pbDisplay(Game._INTL("PP was restored."));
						//next true;
						return true;
					}
				}
				//next false;
				return false;
			});
			UseOnPokemon.Add(Items.LEPPA_BERRY, (item, pokemon, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Restore which move?"));
				if (move>=0) {
					if (Item.pbRestorePP(pokemon,move,10)==0) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						//next false;
						return false;
					} else {
						scene.pbDisplay(Game._INTL("PP was restored."));
						//next true;
						return true;
					}
				}
				//next false;
				return false;
			});

			UseOnPokemon.Add(Items.MAX_ETHER, (item, pokemon, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Restore which move?"));
				if (move>=0) {
					if (Item.pbRestorePP(pokemon,move,pokemon.moves[move].TotalPP-pokemon.moves[move].PP)==0) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						return false;
					} else {
						scene.pbDisplay(Game._INTL("PP was restored."));
						return true;
					}
				}
				return false;
			});

			UseOnPokemon.Add(Items.ELIXIR, (item, pokemon, scene) => {
				int pprestored=0;
				for (int i = 0; i < pokemon.moves.Length; i++) {
					pprestored+=Item.pbRestorePP(pokemon,i,10);
				}
				if (pprestored==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("PP was restored."));
					return true;
				}
			});

			UseOnPokemon.Add(Items.MAX_ELIXIR, (item, pokemon, scene) => {
				int pprestored=0;
				for (int i = 0; i < pokemon.moves.Length; i++) {
					pprestored+=Item.pbRestorePP(pokemon,i,pokemon.moves[i].TotalPP-pokemon.moves[i].PP);
				}
				if (pprestored==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("PP was restored."));
					return true;
				}
			});

			UseOnPokemon.Add(Items.PP_UP, (item, pokemon, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Boost PP of which move?"));
				if (move>=0) {
					if (pokemon.moves[move].TotalPP==0 || pokemon.moves[move].PPups>=3) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						return false;
					} else {
						pokemon.moves[move].PPups+=1;
						string movename=Game._INTL(pokemon.moves[move].id.ToString(TextScripts.Name));
						scene.pbDisplay(Game._INTL("{1}'s PP increased.",movename));
						return true;
					}
				}
				return false;
			});

			UseOnPokemon.Add(Items.PP_MAX, (item, pokemon, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Boost PP of which move?"));
				if (move>=0) {
					if (pokemon.moves[move].TotalPP==0 || pokemon.moves[move].PPups>=3) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						return false;
					} else {
						pokemon.moves[move].PPups=3;
						string movename=Game._INTL(pokemon.moves[move].id.ToString(TextScripts.Name));
						scene.pbDisplay(Game._INTL("{1}'s PP increased.",movename));
						return true;
					}
				}
				return false;
			});

			UseOnPokemon.Add(Items.HP_UP, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.HP)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.PROTEIN, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.ATTACK)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Attack increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.IRON, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.DEFENSE)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Defense increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.CALCIUM, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.SPATK)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Special Attack increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.ZINC, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.SPDEF)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Special Defense increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.CARBOS, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.SPEED)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Speed increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.HEALTH_WING, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.HP,1,false)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.MUSCLE_WING, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.ATTACK,1,false)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Attack increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.RESIST_WING, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.DEFENSE,1,false)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Defense increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.GENIUS_WING, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.SPATK,1,false)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Special Attack increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.CLEVER_WING, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.SPDEF,1,false)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Special Defense increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.SWIFT_WING, (item, pokemon, scene) => {
				if (Item.pbRaiseEffortValues(pokemon,Stats.SPEED,1,false)==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("{1}'s Speed increased.",pokemon.Name));
					pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
					return true;
				}
			});

			UseOnPokemon.Add(Items.RARE_CANDY, (item, pokemon, scene) => {
				if (pokemon.Level>=Core.MAXIMUMLEVEL || (pokemon.isShadow)) { //rescue false
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					Item.pbChangeLevel(pokemon,pokemon.Level+1,scene);
					scene.pbHardRefresh();
					return true;
				}
			});

			UseOnPokemon.Add(Items.POMEG_BERRY, (item, pokemon, scene) => {
				return Item.pbRaiseHappinessAndLowerEV(pokemon,scene,Stats.HP,new string[] {
					Game._INTL("{1} adores you! Its base HP fell!",pokemon.Name),
					Game._INTL("{1} became more friendly. Its base HP can't go lower.",pokemon.Name),
					Game._INTL("{1} became more friendly. However, its base HP fell!",pokemon.Name)
				});
			});

			UseOnPokemon.Add(Items.KELPSY_BERRY, (item, pokemon, scene) => {
				return Item.pbRaiseHappinessAndLowerEV(pokemon,scene,Stats.ATTACK,new string[] {
					Game._INTL("{1} adores you! Its base Attack fell!",pokemon.Name),
					Game._INTL("{1} became more friendly. Its base Attack can't go lower.",pokemon.Name),
					Game._INTL("{1} became more friendly. However, its base Attack fell!",pokemon.Name)
				});
			});

			UseOnPokemon.Add(Items.QUALOT_BERRY, (item, pokemon, scene) => {
				return Item.pbRaiseHappinessAndLowerEV(pokemon,scene,Stats.DEFENSE,new string[] {
					Game._INTL("{1} adores you! Its base Defense fell!",pokemon.Name),
					Game._INTL("{1} became more friendly. Its base Defense can't go lower.",pokemon.Name),
					Game._INTL("{1} became more friendly. However, its base Defense fell!",pokemon.Name)
				});
			});

			UseOnPokemon.Add(Items.HONDEW_BERRY, (item, pokemon, scene) => {
				return Item.pbRaiseHappinessAndLowerEV(pokemon,scene,Stats.SPATK,new string[] {
					Game._INTL("{1} adores you! Its base Special Attack fell!",pokemon.Name),
					Game._INTL("{1} became more friendly. Its base Special Attack can't go lower.",pokemon.Name),
					Game._INTL("{1} became more friendly. However, its base Special Attack fell!",pokemon.Name)
				});
			});

			UseOnPokemon.Add(Items.GREPA_BERRY, (item, pokemon, scene) => {
				return Item.pbRaiseHappinessAndLowerEV(pokemon,scene,Stats.SPDEF,new string[] {
					Game._INTL("{1} adores you! Its base Special Defense fell!",pokemon.Name),
					Game._INTL("{1} became more friendly. Its base Special Defense can't go lower.",pokemon.Name),
					Game._INTL("{1} became more friendly. However, its base Special Defense fell!",pokemon.Name)
				});
			});

			UseOnPokemon.Add(Items.TAMATO_BERRY, (item, pokemon, scene) => {
				return Item.pbRaiseHappinessAndLowerEV(pokemon,scene,Stats.SPEED,new string[] {
					Game._INTL("{1} adores you! Its base Speed fell!",pokemon.Name),
					Game._INTL("{1} became more friendly. Its base Speed can't go lower.",pokemon.Name),
					Game._INTL("{1} became more friendly. However, its base Speed fell!",pokemon.Name)
				});
			});

			UseOnPokemon.Add(Items.GRACIDEA, (item, pokemon, scene) => {
				if (pokemon.Species == Pokemons.SHAYMIN && pokemon.FormId==0 &&
					pokemon.Status!=Status.FROZEN && !IsNight) {
					if (pokemon.HP>0) {
						pokemon.SetForm(1);
						scene.pbRefresh();
						scene.pbDisplay(Game._INTL("{1} changed Forme!",pokemon.Name));
						return true;
					} else {
						scene.pbDisplay(Game._INTL("This can't be used on the fainted Pokémon."));
					}
				} else {
					scene.pbDisplay(Game._INTL("It had no effect."));
					return false;
				}
				return false;
			});

			UseOnPokemon.Add(Items.REVEAL_GLASS, (item, pokemon, scene) => {
				if ((pokemon.Species == Pokemons.TORNADUS ||
					pokemon.Species == Pokemons.THUNDURUS ||
					pokemon.Species == Pokemons.LANDORUS)) {
					if (pokemon.HP>0) {
						pokemon.SetForm((pokemon.FormId==0) ? 1 : 0);
						scene.pbRefresh();
						scene.pbDisplay(Game._INTL("{1} changed Forme!",pokemon.Name));
						return true;
					} else {
						scene.pbDisplay(Game._INTL("This can't be used on the fainted Pokémon."));
					}
				} else {
					scene.pbDisplay(Game._INTL("It had no effect."));
					return false;
				}
				return false;
			});

			UseOnPokemon.Add(Items.DNA_SPLICERS, (item, pokemon, scene) => {
				if (pokemon.Species == Pokemons.KYUREM) {
					if (pokemon.HP>0) {
						if (pokemon.fused!=null) {
							if (Trainer.party.Length>=6) { //ToDo: Party count has 2 slots open
								scene.pbDisplay(Game._INTL("You have no room to separate the Pokémon."));
								return false;
							} else {
								Trainer.party[Trainer.party.Length]=pokemon.fused[1];
								pokemon.fused=null;
								pokemon.SetForm(0);
								scene.pbHardRefresh();
								scene.pbDisplay(Game._INTL("{1} changed Forme!",pokemon.Name));
								return true;
							}
						} else {
							int chosen=scene.pbChoosePokemon(Game._INTL("Fuse with which Pokémon?"));
							if (chosen>=0) {
								Pokemon poke2=Trainer.party[chosen];
								if ((poke2.Species == Pokemons.RESHIRAM ||
									poke2.Species == Pokemons.ZEKROM) && poke2.HP>0 && !poke2.isEgg) {
									if (poke2.Species == Pokemons.RESHIRAM) pokemon.SetForm(1);
									if (poke2.Species == Pokemons.ZEKROM) pokemon.SetForm(2);
									pokemon.fused=new Pokemon[] { pokemon, poke2 }; //poke2;
									//ToDo: Combine stats and divide down the middle? (IV/EV)
									pbRemovePokemonAt(chosen); 
									scene.pbHardRefresh();
									scene.pbDisplay(Game._INTL("{1} changed Forme!",pokemon.Name));
									return true;
								} else if (poke2.isEgg) {
									scene.pbDisplay(Game._INTL("It cannot be fused with an Egg."));
								} else if (poke2.HP<=0) {
									scene.pbDisplay(Game._INTL("It cannot be fused with that fainted Pokémon."));
								} else if (pokemon==poke2) {
									scene.pbDisplay(Game._INTL("It cannot be fused with itself."));
								} else {
									scene.pbDisplay(Game._INTL("It cannot be fused with that Pokémon."));
								}
							} else {
								return false;
							}
						}
					} else {
						scene.pbDisplay(Game._INTL("This can't be used on the fainted Pokémon."));
					}
				} else {
					scene.pbDisplay(Game._INTL("It had no effect."));
					return false;
				}
				return false;
			});

			UseOnPokemon.Add(Items.PRISON_BOTTLE, (item, pokemon, scene) => {
				if (pokemon.Species == Pokemons.HOOPA) {
					if (pokemon.HP>0) {
						pokemon.SetForm((pokemon.FormId==0) ? 1 : 0);
						scene.pbRefresh();
						scene.pbDisplay(Game._INTL("{1} changed Forme!",pokemon.Name));
						return true;
					} else {
						scene.pbDisplay(Game._INTL("This can't be used on the fainted Pokémon."));
					}
				} else {
					scene.pbDisplay(Game._INTL("It had no effect."));
					return false;
				}
				return false;
			});

			UseOnPokemon.Add(Items.ABILITY_CAPSULE, (item, pokemon, scene) => {
				Abilities[] abils=pokemon.getAbilityList();
				Abilities abil1=0;Abilities abil2=0; int n = 0;
				foreach (Abilities i in abils) {
					//if (i[1]==0) abil1=i[0];
					if (n==0) abil1=i;
					//if (i[1]==1) abil2=i[0];
					if (n==1) abil2=i;
					n++;
				}
				if (abil1<=0 || abil2<=0 || pokemon.hasHiddenAbility()) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				}
				int newabil=(pokemon.abilityIndex+1)%2;
				string newabilname=(newabil==0) ? Game._INTL(abil1.ToString(TextScripts.Name)) : Game._INTL(abil2.ToString(TextScripts.Name));
				if (scene.pbConfirm(Game._INTL("Would you like to change {1}'s Ability to {2}?",
					pokemon.Name,newabilname))) {
					pokemon.setAbility(newabil);
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s Ability changed to {2}!",pokemon.Name,
					Game._INTL(pokemon.Ability.ToString(TextScripts.Name))));
					return true;
				}
				return false;
			});
			#endregion

			#region BattleUseOnPokemon handlers
			BattleUseOnPokemon.Add(Items.POTION, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,20,scene);
			});

			BattleUseOnPokemon.Add(Items.SUPER_POTION, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,50,scene);
			});

			BattleUseOnPokemon.Add(Items.HYPER_POTION, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,200,scene);
			});

			BattleUseOnPokemon.Add(Items.MAX_POTION, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,pokemon.TotalHP-pokemon.HP,scene);
			});

			BattleUseOnPokemon.Add(Items.BERRY_JUICE, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,20,scene);
			});

			BattleUseOnPokemon.Add(Items.RAGE_CANDY_BAR, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,20,scene);
			});

			BattleUseOnPokemon.Add(Items.SWEET_HEART, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,20,scene);
			});

			BattleUseOnPokemon.Add(Items.FRESH_WATER, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,50,scene);
			});

			BattleUseOnPokemon.Add(Items.SODA_POP, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,60,scene);
			});

			BattleUseOnPokemon.Add(Items.LEMONADE, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,80,scene);
			});

			BattleUseOnPokemon.Add(Items.MOOMOO_MILK, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,100,scene);
			});

			BattleUseOnPokemon.Add(Items.ORAN_BERRY, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,10,scene);
			});

			BattleUseOnPokemon.Add(Items.SITRUS_BERRY, (pokemon, battler, scene) => {
				return Item.pbBattleHPItem(pokemon,battler,(int)Math.Floor(pokemon.TotalHP/4f),scene);
			});

			BattleUseOnPokemon.Add(Items.AWAKENING, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.CHESTO_BERRY, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.BLUE_FLUTE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.POKE_FLUTE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} woke up.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.ANTIDOTE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.PECHA_BERRY, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.BURN_HEAL, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s burn was healed.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.RAWST_BERRY, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s burn was healed.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.PARALYZE_HEAL, (pokemon, battler, scene) => {//item == Items.PARLYZHEAL || 
				if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.CHERI_BERRY, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.ICE_HEAL, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was thawed out.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.ASPEAR_BERRY, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} was thawed out.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.FULL_HEAL, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.LAVA_COOKIE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.OLD_GATEAU, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.CASTELIACONE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.LUMIOSE_GALETTE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.SHALOUR_SABLE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});
			BattleUseOnPokemon.Add(Items.LUM_BERRY, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.FULL_RESTORE, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.HP==pokemon.TotalHP && pokemon.Status==0 &&
					(!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					int hpgain=Item.pbItemRestoreHP(pokemon,pokemon.TotalHP-pokemon.HP);
					if (battler.IsNotNullOrNone()) battler.HP=pokemon.HP;
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					scene.pbRefresh();
					if (hpgain>0) {
						scene.pbDisplay(Game._INTL("{1}'s HP was restored by {2} points.",pokemon.Name,hpgain));
					} else {
						scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					}
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.REVIVE, (pokemon, battler, scene) => {
				if (pokemon.HP>0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HP=(int)Math.Floor(pokemon.TotalHP/2f);
					//Item.pbItemRestoreHP(pokemon,(int)Math.Floor(pokemon.TotalHP/2f));
					pokemon.HealStatus();
					for (int i = 0; i < Trainer.party.Length; i++) {
						if (Trainer.party[i]==pokemon) {
							if (battler.IsNotNullOrNone()) battler.Initialize(pokemon,(sbyte)i,false);
							break;
						}
					}
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.MAX_REVIVE, (pokemon, battler, scene) => {
				if (pokemon.HP>0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealHP();
					pokemon.HealStatus();
					for (int i = 0; i < Trainer.party.Length; i++) {
						if (Trainer.party[i]==pokemon) {
							if (battler.IsNotNullOrNone()) battler.Initialize(pokemon,(sbyte)i,false);
							break;
						}
					}
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.ENERGY_POWDER, (pokemon, battler, scene) => {
				if (Item.pbBattleHPItem(pokemon,battler,50,scene)) {
					pokemon.ChangeHappiness(HappinessMethods.POWDER);
					return true;
				}
				return false;
			});

			BattleUseOnPokemon.Add(Items.ENERGY_ROOT, (pokemon, battler, scene) => {
				if (Item.pbBattleHPItem(pokemon,battler,200,scene)) {
					pokemon.ChangeHappiness(HappinessMethods.ENERGYROOT);
					return true;
				}
				return false;
			});

			BattleUseOnPokemon.Add(Items.HEAL_POWDER, (pokemon, battler, scene) => {
				if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					if (battler.IsNotNullOrNone()) battler.Status=0;
					if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
					pokemon.ChangeHappiness(HappinessMethods.POWDER);
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1} became healthy.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.REVIVAL_HERB, (pokemon, battler, scene) => {
				if (pokemon.HP>0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					pokemon.HealStatus();
					//pokemon.HP=pokemon.TotalHP;
					pokemon.HealHP();
					for (int i = 0; i < Trainer.party.Length; i++) {
						if (Trainer.party[i]==pokemon) {
							if (battler.IsNotNullOrNone()) battler.Initialize(pokemon,(sbyte)i,false);
							break;
						}
					}
					pokemon.ChangeHappiness(HappinessMethods.REVIVALHERB);
					scene.pbRefresh();
					scene.pbDisplay(Game._INTL("{1}'s HP was restored.",pokemon.Name));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.ETHER, (pokemon, battler, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Restore which move?"));
				if (move>=0) {
					if (Item.pbBattleRestorePP(pokemon,battler,move,10)==0) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						return false;
					} else {
						scene.pbDisplay(Game._INTL("PP was restored."));
						return true;
					}
				}
				return false;
			});
			BattleUseOnPokemon.Add(Items.LEPPA_BERRY, (pokemon, battler, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Restore which move?"));
				if (move>=0) {
					if (Item.pbBattleRestorePP(pokemon,battler,move,10)==0) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						return false;
					} else {
						scene.pbDisplay(Game._INTL("PP was restored."));
						return true;
					}
				}
				return false;
			});

			BattleUseOnPokemon.Add(Items.MAX_ETHER, (pokemon, battler, scene) => {
				int move=scene.pbChooseMove(pokemon,Game._INTL("Restore which move?"));
				if (move>=0) {
					if (Item.pbBattleRestorePP(pokemon,battler,move,pokemon.moves[move].TotalPP-pokemon.moves[move].PP)==0) {
						scene.pbDisplay(Game._INTL("It won't have any effect."));
						return false;
					} else {
						scene.pbDisplay(Game._INTL("PP was restored."));
						return true;
					}
				}
				return false;
			});

			BattleUseOnPokemon.Add(Items.ELIXIR, (pokemon, battler, scene) => {
				int pprestored=0;
				for (int i = 0; i < pokemon.moves.Length; i++) {
					pprestored+=Item.pbBattleRestorePP(pokemon,battler,i,10);
				}
				if (pprestored==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("PP was restored."));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.MAX_ELIXIR, (pokemon, battler, scene) => {
				int pprestored=0;
				for (int i = 0; i < pokemon.moves.Length; i++) {
					pprestored+=Item.pbBattleRestorePP(pokemon,battler,i,pokemon.moves[i].TotalPP-pokemon.moves[i].PP);
				}
				if (pprestored==0) {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				} else {
					scene.pbDisplay(Game._INTL("PP was restored."));
					return true;
				}
			});

			BattleUseOnPokemon.Add(Items.RED_FLUTE, (pokemon, battler, scene) => {
				if (battler.IsNotNullOrNone() && battler.effects.Attract>=0) {
					battler.effects.Attract=-1;
					scene.pbDisplay(Game._INTL("{1} got over its infatuation.",pokemon.Name));
					return true; // Items.consumed:
				} else {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				}
			});

			BattleUseOnPokemon.Add(Items.YELLOW_FLUTE, (pokemon, battler, scene) => {
				if (battler.IsNotNullOrNone() && battler.effects.Confusion>0) {
					battler.effects.Confusion=0;
					scene.pbDisplay(Game._INTL("{1} snapped out of confusion.",pokemon.Name));
					return true; // Items.consumed:
				} else {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				}
			});
			BattleUseOnPokemon.Add(Items.PERSIM_BERRY, (pokemon, battler, scene) => {
				if (battler.IsNotNullOrNone() && battler.effects.Confusion>0) {
					battler.effects.Confusion=0;
					scene.pbDisplay(Game._INTL("{1} snapped out of confusion.",pokemon.Name));
					return true; // Items.consumed:
				} else {
					scene.pbDisplay(Game._INTL("It won't have any effect."));
					return false;
				}
			});
			#endregion

			#region BattleUseOnBattler handlers
			BattleUseOnBattler.Add(Items.X_ATTACK, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.ATTACK,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.ATTACK,1,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_ATTACK_2, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.ATTACK,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.ATTACK,2,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_ATTACK_3, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.ATTACK,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.ATTACK,3,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_ATTACK_6, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbIncreaseStatWithCause(Combat.Stats.ATTACK,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_DEFENSE, (item, battler, scene) => { //item == Items.X_DEFEND ||
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.DEFENSE,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.DEFENSE,1,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false  ;
				}
			});

			BattleUseOnBattler.Add(Items.X_DEFENSE_2, (item, battler, scene) => { //item == Items.XDEFEND2 || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.DEFENSE,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.DEFENSE,2,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_DEFENSE_3, (item, battler, scene) => { //item == Items.XDEFEND3 || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.DEFENSE,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.DEFENSE,3,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_DEFENSE_6, (item, battler, scene) => { //item == Items.XDEFEND6 || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbIncreaseStatWithCause(Combat.Stats.DEFENSE,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_ATK, (item, battler, scene) => { //item == Items.X_SPECIAL || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPATK,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPATK,1,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_ATK_2, (item, battler, scene) => { //item == Items.XSPECIAL2 || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPATK,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPATK,2,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_ATK_3, (item, battler, scene) => { //item == Items.XSPECIAL3 || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPATK,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPATK,3,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_ATK_6, (item, battler, scene) => { //item == Items.XSPECIAL6 || 
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbIncreaseStatWithCause(Combat.Stats.SPATK,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_DEF, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPDEF,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPDEF,1,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_DEF_2, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPDEF,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPDEF,2,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_DEF_3, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPDEF,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPDEF,3,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SP_DEF_6, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbIncreaseStatWithCause(Combat.Stats.SPDEF,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SPEED, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPEED,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPEED,1,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SPEED_2, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPEED,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPEED,2,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SPEED_3, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.SPEED,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.SPEED,3,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_SPEED_6, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbIncreaseStatWithCause(Combat.Stats.SPEED,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_ACCURACY, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.ACCURACY,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.ACCURACY,1,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_ACCURACY_2, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.ACCURACY,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.ACCURACY,2,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.X_ACCURACY_3, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbCanIncreaseStatStage(Combat.Stats.ACCURACY,battler,false)) {
					battler.pbIncreaseStat(Combat.Stats.ACCURACY,3,battler,true);
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false  ;
				}
			});

			BattleUseOnBattler.Add(Items.X_ACCURACY_6, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbIncreaseStatWithCause(Combat.Stats.ACCURACY,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
					return true;
				} else {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				}
			});

			BattleUseOnBattler.Add(Items.DIRE_HIT, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.effects.FocusEnergy>=1) {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				} else {
					battler.effects.FocusEnergy=1;
					scene.pbDisplay(Game._INTL("{1} is getting pumped!",battler.ToString()));
					return true;
				}
			});

			BattleUseOnBattler.Add(Items.DIRE_HIT_2, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.effects.FocusEnergy>=2) {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				} else {
					battler.effects.FocusEnergy=2;
					scene.pbDisplay(Game._INTL("{1} is getting pumped!",battler.ToString()));
					return true;
				}
			});

			BattleUseOnBattler.Add(Items.DIRE_HIT_3, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.effects.FocusEnergy>=3) {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				} else {
					battler.effects.FocusEnergy=3;
					scene.pbDisplay(Game._INTL("{1} is getting pumped!",battler.ToString()));
					return true;
				}
			});

			BattleUseOnBattler.Add(Items.GUARD_SPEC, (item, battler, scene) => {
				string playername=battler.battle.pbPlayer().name;
				scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
				if (battler.pbOwnSide.Mist>0) {
					scene.pbDisplay(Game._INTL("But it had no effect!"));
					return false;
				} else { 
					battler.pbOwnSide.Mist=5;
					//if (!scene.pbIsOpposing(battler.Index)) { //Create new Delegate for attacker?
					if (!battler.battle.pbOwnedByPlayer(battler.Index)) { //if player's pokemon...
						scene.pbDisplay(Game._INTL("Your team became shrouded in mist!"));
					} else {
						scene.pbDisplay(Game._INTL("The foe's team became shrouded in mist!"));
					}
					return true;
				}
			});

			BattleUseOnBattler.Add(Items.POKE_DOLL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (battle.opponent != null) {
					scene.pbDisplay(Game._INTL("Can't use that here."));
					return false;
				} else {
					string playername=battle.pbPlayer().name;
					scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					return true;
				}
			});
			BattleUseOnBattler.Add(Items.FLUFFY_TAIL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (battle.opponent != null) {
					scene.pbDisplay(Game._INTL("Can't use that here."));
					return false;
				} else {
					string playername=battle.pbPlayer().name;
					scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					return true;
				}
			});
			BattleUseOnBattler.Add(Items.POKE_TOY, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (battle.opponent != null) {
					scene.pbDisplay(Game._INTL("Can't use that here."));
					return false;
				} else {
					string playername=battle.pbPlayer().name;
					scene.pbDisplay(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					return true;
				}
			});

			//if (Item.pbIsPokeBall(item)) {  // Any Poké Ball
			BattleUseOnBattler.Add(Items.POKE_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				//if (battle.pbPlayer().party.Length>=6 && !PC.full) {
				if (battle.pbPlayer().party.GetCount()>=Features.LimitPokemonPartySize && !Player.PC.hasSpace()) {
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.BEAST_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.CHERISH_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.DIVE_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.DREAM_BALL, (item, battler, scene) => { //ToDo: Only in dreamworld?
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.DUSK_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.FAST_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.FRIEND_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
					scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
					return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.GREAT_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.HEAL_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.HEAVY_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.IRON_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.LEVEL_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.LIGHT_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.LOVE_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.LURE_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.LUXURY_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.MASTER_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.MOON_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.NEST_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.NET_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.PARK_BALL, (item, battler, scene) => { //ToDo: Only in park?
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.PREMIER_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.QUICK_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.REPEAT_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.SAFARI_BALL, (item, battler, scene) => { //ToDo: Only during safari contest?
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.SMOKE_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.SPORT_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.TIMER_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			BattleUseOnBattler.Add(Items.ULTRA_BALL, (item, battler, scene) => {
				IBattle battle=battler.battle;
				if (!battler.pbOpposing1.isFainted() && !battler.pbOpposing2.isFainted()) {
					if (!(Game.GameData as IItemCheck).pbIsSnagBall(item)) { //battle.pbIsSnagBall(item)
						scene.pbDisplay(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						return false;
					}
				}
				if (battle.pbPlayer().party.Length>=6 && !Player.PC.hasSpace()) { //PC.full
					scene.pbDisplay(Game._INTL("There is no room left in the PC!"));
					return false;
				}
				return true;
			});
			#endregion

			#region UseInBattle handlers
			UseInBattle.Add(Items.POKE_DOLL, (item, battler, battle) => {
				battle.decision=Combat.BattleResults.FORFEIT;
				battle.pbDisplayPaused(Game._INTL("Got away safely!"));
			});
			UseInBattle.Add(Items.FLUFFY_TAIL, (item, battler, battle) => {
				battle.decision=Combat.BattleResults.FORFEIT;
				battle.pbDisplayPaused(Game._INTL("Got away safely!"));
			});
			UseInBattle.Add(Items.POKE_TOY, (item, battler, battle) => {
				battle.decision=Combat.BattleResults.FORFEIT;
				battle.pbDisplayPaused(Game._INTL("Got away safely!"));
			});

			//if (Item.pbIsPokeBall(item))// Any Poké Ball 
			UseInBattle.Add(Items.POKE_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.BEAST_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.CHERISH_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.DIVE_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.DREAM_BALL, (item, battler, battle) => { //ToDo: Only in dreamworld?
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.DUSK_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.FAST_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.FRIEND_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.GREAT_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.HEAL_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.HEAVY_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.IRON_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.LEVEL_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.LIGHT_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.LOVE_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.LURE_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.LUXURY_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.MASTER_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.MOON_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.NEST_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.NET_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.PARK_BALL, (item, battler, battle) => { //ToDo: Only in park?
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.PREMIER_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.QUICK_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.REPEAT_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.SAFARI_BALL, (item, battler, battle) => { //ToDo: Only during safari contest?
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.SMOKE_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.SPORT_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.TIMER_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			UseInBattle.Add(Items.ULTRA_BALL, (item, battler, battle) => {
				battle.pbThrowPokeball(battler.Index, item);
			});
			#endregion
		}*/
	}
}