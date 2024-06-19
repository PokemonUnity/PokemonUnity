using System;
using System.Linq;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Overworld;
using PokemonUnity.Inventory;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface;
using PokemonUnity.EventArg;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity//.Inventory
{
	//public static partial class ItemHandlers
	public partial class Game : IGameItemEffect
	{
		public ItemUseResults Repel(Items item,int steps) {
			if (Global.repel>0) {
				(this as IGameMessage).Message(Game._INTL("But the effects of a Repel lingered from earlier."));
				//return 0;
				return ItemUseResults.NotUsed;
			} else {
				(this as IGameMessage).Message(Game._INTL("{1} used the {2}.",Trainer.name,Game._INTL(item.ToString(TextScripts.Name))));
				Global.repel=steps;
				//return 3;
				return ItemUseResults.UsedItemConsumed;
			}
		}

		//static ItemHandlers() {
		protected virtual void RegisterItemHandlers(object sender, UseFromBagEventArgs args) {
			Terrains? terrain = null;
			bool notCliff = false;
			switch (args.Item) {
				#region UseFromBag handlers
				case Items.REPEL: args.Response = Repel(Items.REPEL, 100); break;
				case Items.SUPER_REPEL: args.Response = Repel(Items.SUPER_REPEL,200); break;
				case Items.MAX_REPEL: args.Response = Repel(Items.MAX_REPEL,250); break;
				case Items.BLACK_FLUTE:
					(this as IGameMessage).Message(Game._INTL("{1} used the {2}.",Trainer.name,Game._INTL(Items.BLACK_FLUTE.ToString(TextScripts.Name))));
					(this as IGameMessage).Message(Game._INTL("Wild Pokémon will be repelled."));
					PokemonMap.blackFluteUsed=true;
					PokemonMap.whiteFluteUsed=false;
					//next 1;
					args.Response = ItemUseResults.UsedNotConsumed;
					break;
				case Items.WHITE_FLUTE:
					(this as IGameMessage).Message(Game._INTL("{1} used the {2}.",Trainer.name,Game._INTL(Items.WHITE_FLUTE.ToString(TextScripts.Name))));
					(this as IGameMessage).Message(Game._INTL("Wild Pokémon will be lured."));
					PokemonMap.blackFluteUsed=false;
					PokemonMap.whiteFluteUsed=true;
					//next 1;
					args.Response = ItemUseResults.UsedNotConsumed;
					break;
				case Items.HONEY: args.Response = ItemUseResults.CloseBagItemConsumed; break;
				case Items.ESCAPE_ROPE:
					if (GamePlayer.HasDependentEvents()) {
						(this as IGameMessage).Message(Game._INTL("It can't be used when you have someone with you."));
						//next 0;
						args.Response = ItemUseResults.NotUsed;
					}
					if (Global.escapePoint != null) { //&& Global.escapePoint.Length>0
						//next 4; // End screen and consume item
						args.Response = ItemUseResults.CloseBagItemConsumed;
					} else {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next 0;
						args.Response = ItemUseResults.NotUsed;
					}
					break;
				case Items.SACRED_ASH:
					int revived=0;
					if (Trainer.pokemonCount==0) {
						(this as IGameMessage).Message(Game._INTL("There is no Pokémon."));
						//next 0;
						args.Response = ItemUseResults.NotUsed;
					}
					//ToDo: Redo below into an Event Listener (Subscribe to on Frontend)
					FadeOutIn(99999, block: () => {
						IPartyDisplayScene scene=Scenes.Party; //new PokemonScreen_Scene();
						IPartyDisplayScreen screen = Screens.Party.initialize(scene,Trainer.party); //new PokemonScreen(scene,Trainer.party);
						screen.StartScene(Game._INTL("Using item..."),false);
						foreach (Pokemon i in Trainer.party) {
							if (i.HP<=0 && !i.isEgg) {
								revived+=1;
								i.Heal();
								screen.Display(Game._INTL("{1}'s HP was restored.",i.Name));
							}
						}
						if (revived==0) {
							screen.Display(Game._INTL("It won't have any effect."));
						}
						screen.EndScene();
					});
					//next (revived==0) ? 0 : 3;
					args.Response = (revived==0) ? ItemUseResults.NotUsed : ItemUseResults.UsedItemConsumed;
					break;
				case Items.BICYCLE:
					//next BikeCheck ? 2 : 0;
					args.Response = BikeCheck() ? ItemUseResults.CloseBagNotConsumed : ItemUseResults.NotUsed;
					//args.Response = ItemUseResults.NotUsed;
					break;
				case Items.MACH_BIKE:
					//next BikeCheck ? 2 : 0;
					args.Response = BikeCheck() ? ItemUseResults.CloseBagNotConsumed : ItemUseResults.NotUsed;
					//args.Response = ItemUseResults.NotUsed;
					break;
				case Items.ACRO_BIKE:
					//next BikeCheck ? 2 : 0;
					args.Response = BikeCheck() ? ItemUseResults.CloseBagNotConsumed : ItemUseResults.NotUsed;
					//args.Response = ItemUseResults.NotUsed;
					break;
				case Items.OLD_ROD:
					terrain=FacingTerrainTag();
					notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
					if ((Terrain.isWater(terrain) && !Global.surfing && notCliff) ||
						(Terrain.isWater(terrain) && Global.surfing)) {
						//next 2;
						args.Response = ItemUseResults.CloseBagNotConsumed;
					} else {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next 0;
						args.Response = ItemUseResults.NotUsed;
					}
					break;
				case Items.GOOD_ROD:
					terrain=FacingTerrainTag();
					notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
					if ((Terrain.isWater(terrain) && !Global.surfing && notCliff) ||
						(Terrain.isWater(terrain) && Global.surfing)) {
						//next 2;
						args.Response = ItemUseResults.CloseBagNotConsumed;
					} else {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next 0;
						args.Response = ItemUseResults.NotUsed;
					}
					break;
				case Items.SUPER_ROD:
					terrain=FacingTerrainTag();
					notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
					if ((Terrain.isWater(terrain) && !Global.surfing && notCliff) ||
						(Terrain.isWater(terrain) && Global.surfing)) {
						//next 2;
						args.Response = ItemUseResults.CloseBagNotConsumed;
					} else {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next 0;
						args.Response = ItemUseResults.NotUsed;
					}
					break; //ToDo: Add items to Enum?...
				//case Items.ITEM_FINDER: args.Response = ItemUseResults.CloseBagNotConsumed; break;
				case Items.DOWSING_MACHINE: args.Response = ItemUseResults.CloseBagNotConsumed; break;
				case Items.TOWN_MAP:
					if (this is IGameRegionMap grm) grm.ShowMap(-1,false);
					//next 1; // Continue
					args.Response = ItemUseResults.UsedNotConsumed;
					break;
				case Items.COIN_CASE:
					(this as IGameMessage).Message(Game._INTL("Coins: {1}",Global.coins));
					//next 1; // Continue
					args.Response = ItemUseResults.UsedNotConsumed;
					break;
				case Items.EXP_ALL:
					Bag.ChangeItem(Items.EXP_ALL, Items.EXP_ALL_OFF);
					(this as IGameMessage).Message(Game._INTL("The Exp Share was turned off."));
					//next 1; // Continue
					args.Response = ItemUseResults.UsedNotConsumed;
					break;
				case Items.EXP_ALL_OFF:
					Bag.ChangeItem(Items.EXP_ALL_OFF, Items.EXP_ALL);
					(this as IGameMessage).Message(Game._INTL("The Exp Share was turned on."));
					//next 1; // Continue
					args.Response = ItemUseResults.UsedNotConsumed;
					break;
				#endregion
			}
		}

		protected virtual void RegisterItemHandlers(object sender, UseInFieldEventArgs args) {
			Items item = args.Item;
			Terrains? terrain = null;
			bool notCliff = false;
			bool encounter = false;
			int pprestored = 0;
			switch (item) {
				#region UseInField handlers
				case Items.HONEY:
					(this as IGameMessage).Message(Game._INTL("{1} used the {2}.",Trainer.name,Game._INTL(Items.HONEY.ToString(TextScripts.Name))));
					if (this is IGameHiddenMoves ghm) ghm.SweetScent();
					break;
				case Items.ESCAPE_ROPE:
					ITilePosition escape=Global.escapePoint; //rescue null
					if (escape == null) { //|| escape.Length==0
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next;
						return;
					}
					if (GamePlayer.HasDependentEvents()) {
						(this as IGameMessage).Message(Game._INTL("It can't be used when you have someone with you."));
						//next;
						return;
					}
					(this as IGameMessage).Message(Game._INTL("{1} used the {2}.",Trainer.name,Game._INTL(item.ToString(TextScripts.Name))));
					FadeOutIn(99999, block: () => {
						CancelVehicles();
						GameTemp.player_new_map_id=escape.MapId;//[0];
						GameTemp.player_new_x=escape.X;//[1];
						GameTemp.player_new_y=escape.Y;//[2];
						GameTemp.player_new_direction=(int)escape.Z;//[3];
						Scene.transfer_player();
						GameMap.autoplay();
						GameMap.refresh();
					});
					EraseEscapePoint();
					break;

				case Items.BICYCLE:
					if (BikeCheck()) {
						if (Global.bicycle) {
							DismountBike();
						} else {
							MountBike();
						}
					}
					break;
				case Items.MACH_BIKE:
					if (BikeCheck()) {
						if (Global.bicycle) {
							DismountBike();
						} else {
							MountBike();
						}
					}
					break;
				case Items.ACRO_BIKE:
					if (BikeCheck()) {
						if (Global.bicycle) {
							DismountBike();
						} else {
							MountBike();
						}
					}
					break;

				case Items.OLD_ROD:
					terrain=FacingTerrainTag();
					notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
					if (!Terrain.isWater(terrain) || (!notCliff && !Global.surfing)) {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next;
						return;
					}
					//encounter=PokemonEncounters.hasEncounter(EncounterTypes.OldRod);
					encounter=PokemonEncounters.hasEncounter(EncounterOptions.OldRod);
					if (Fishing(encounter,1)) {
						//Encounter(EncounterTypes.OldRod);
						Encounter(EncounterOptions.OldRod);
					}
					break;

				case Items.GOOD_ROD:
					terrain=FacingTerrainTag();
					notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
					if (!Terrain.isWater(terrain) || (!notCliff && !Global.surfing)) {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next;
						return;
					}
					//encounter=PokemonEncounters.hasEncounter(EncounterTypes.GoodRod);
					encounter=PokemonEncounters.hasEncounter(EncounterOptions.GoodRod);
					if (Fishing(encounter,2)) {
						//Encounter(EncounterTypes.GoodRod);
						Encounter(EncounterOptions.GoodRod);
					}
					break;

				case Items.SUPER_ROD:
					terrain=FacingTerrainTag();
					notCliff=GameMap.passable(GamePlayer.x,GamePlayer.y,GamePlayer.direction);
					if (!Terrain.isWater(terrain) || (!notCliff && !Global.surfing)) {
						(this as IGameMessage).Message(Game._INTL("Can't use that here."));
						//next;
						return;
					}
					//encounter=PokemonEncounters.hasEncounter(EncounterTypes.SuperRod);
					encounter=PokemonEncounters.hasEncounter(EncounterOptions.SuperRod);
					if (Fishing(encounter,3)) {
						//Encounter(EncounterTypes.SuperRod);
						Encounter(EncounterOptions.SuperRod);
					}
					break;

				case Items.DOWSING_MACHINE://item == Items.ITEM_FINDER || item == Items.DOWSING_MCHN ||
					IGameCharacter @event=ClosestHiddenItem();
					if (@event == null) {
						(this as IGameMessage).Message(Game._INTL("... ... ... ...Nope!\r\nThere's no response."));
					} else {
						float offsetX=@event.x-GamePlayer.x;
						float offsetY=@event.y-GamePlayer.y;
						if (offsetX==0 && offsetY==0) {
							for (int i = 0; i < 32; i++) {
								Graphics?.update();
								Input.update();
								if ((i&7)==0) GamePlayer.turn_right_90();
								if (this is IGameMessage gm) gm.UpdateSceneMap();
							}
							(this as IGameMessage).Message(Game._INTL(@"The {1}'s indicating something right underfoot!\1",Game._INTL(item.ToString(TextScripts.Name))));
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
								if (this is IGameMessage gm) gm.UpdateSceneMap();
							}
							(this as IGameMessage).Message(Game._INTL(@"Huh?\nThe {1}'s responding!\1",Game._INTL(item.ToString(TextScripts.Name))));
							(this as IGameMessage).Message(Game._INTL("There's an item buried around here!"));
						}
					}
					break;

				case Items.TOWN_MAP:
					if (this is IGameRegionMap grm) grm.ShowMap(-1,false);
					break;

				case Items.COIN_CASE:
					(this as IGameMessage).Message(Game._INTL("Coins: {1}",Global.coins));
					//next 1; // Continue
					//return ItemUseResults.UsedNotConsumed;
					break;
				#endregion
			}
		}

		protected virtual void RegisterItemHandlers(object sender, UseOnPokemonEventArgs args) {
			Items item = args.Item;
			IPokemon pokemon = args.Pokemon;
			Pokemons newspecies=Pokemons.NONE;
			//PokemonEssentials.Interface.Screen.IHasDisplayMessage scene = args.Scene;
			PokemonEssentials.Interface.Screen.IPartyDisplayScene scene = args.Scene;
			int move = -1;
			int ppstored = -1;
			//ToDo: If use berry, increase/decrease happiness?
			switch (item) {
				#region UseOnPokemon handlers
				case Items.FIRE_STONE:
					if (pokemon is IPokemonShadowPokemon sp && sp.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies=Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe) newspecies=gpe.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.THUNDER_STONE:
					if (pokemon is IPokemonShadowPokemon sp1 && sp1.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies=Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe1) newspecies=gpe1.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.WATER_STONE:
					if (pokemon is IPokemonShadowPokemon sp2 && sp2.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe2) newspecies=gpe2.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.LEAF_STONE:
					if (pokemon is IPokemonShadowPokemon sp3 && sp3.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe3) newspecies=gpe3.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.MOON_STONE:
					if (pokemon is IPokemonShadowPokemon sp4 && sp4.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe4) newspecies=gpe4.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.SUN_STONE:
					if (pokemon is IPokemonShadowPokemon sp5 && sp5.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe5) newspecies=gpe5.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.DUSK_STONE:
					if (pokemon is IPokemonShadowPokemon sp6 && sp6.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe6) newspecies=gpe6.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.DAWN_STONE:
					if (pokemon is IPokemonShadowPokemon sp7 && sp7.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe7) newspecies=gpe7.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo = Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IBagScene s) { //IPokemonBag_Scene
							if (scene is IPartyDisplayScreen s) { //IPokemonBag_Scene
								//s.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//s.RefreshAnnotations((p) => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								s.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;
				case Items.SHINY_STONE:
					if (pokemon is IPokemonShadowPokemon sp8 && sp8.isShadow) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					}
					newspecies = Pokemons.NONE; //Evolution.CheckEvolution(pokemon,item)[0];
					if (this is IGamePokemonEvolution gpe8) newspecies=gpe8.CheckEvolution(pokemon,item)[0];
					if (newspecies<=0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						//ToDo: Add a check to cycle through all evolves
						FadeOutInWithMusic(99999, block: () => {
							IPokemonEvolutionScene evo= Scenes.EvolvingScene; //new PokemonEvolutionScene();
							evo.StartScreen(pokemon,newspecies);
							evo.Evolution(false);
							evo.EndScreen();
							//if (scene is IPokemonBag_Scene) {
							if (scene is IPartyDisplayScreen s) {
								//scene.RefreshAnnotations(proc{|p| Evolution.CheckEvolution(p,item)[0]>0 }
								//scene.RefreshAnnotations(() => { Evolution.CheckEvolution(p, item)[0] > 0; });
								s.RefreshAnnotations((p) => { if (this is IGamePokemonEvolution pe) return pe.CheckEvolution(p, item)[0] > 0; return false; });
								scene.Refresh();
							}
						});
						//next true;
						args.Response = true;
					}
					break;

				case Items.POTION:
					//next HPItem(pokemon,20,scene);
					args.Response = HPItem(pokemon,20,scene);
					break;

				case Items.SUPER_POTION:
					//next HPItem(pokemon,50,scene);
					args.Response = HPItem(pokemon,50,scene);
					break;

				case Items.HYPER_POTION:
					//next HPItem(pokemon,200,scene);
					args.Response = HPItem(pokemon,200,scene);
					break;

				case Items.MAX_POTION:
					//next HPItem(pokemon,pokemon.TotalHP-pokemon.HP,scene);
					args.Response = HPItem(pokemon,pokemon.TotalHP-pokemon.HP,scene);
					break;

				case Items.BERRY_JUICE:
					//next HPItem(pokemon,20,scene);
					args.Response = HPItem(pokemon,20,scene);
					break;

				case Items.RAGE_CANDY_BAR:
					//next HPItem(pokemon,20,scene);
					args.Response = HPItem(pokemon,20,scene);
					break;

				case Items.SWEET_HEART:
					//next HPItem(pokemon,20,scene);
					args.Response = HPItem(pokemon,20,scene);
					break;

				case Items.FRESH_WATER:
					//next HPItem(pokemon,50,scene);
					args.Response = HPItem(pokemon,50,scene);
					break;

				case Items.SODA_POP:
					//next HPItem(pokemon,60,scene);
					args.Response = HPItem(pokemon,60,scene);
					break;

				case Items.LEMONADE:
					//next HPItem(pokemon,80,scene);
					args.Response = HPItem(pokemon,80,scene);
					break;

				case Items.MOOMOO_MILK:
					//next HPItem(pokemon,100,scene);
					args.Response = HPItem(pokemon,100,scene);
					break;

				case Items.ORAN_BERRY:
					//next HPItem(pokemon,10,scene);
					args.Response = HPItem(pokemon,10,scene);
					break;

				case Items.SITRUS_BERRY:
					//next HPItem(pokemon,Math.Floor(pokemon.TotalHP/4),scene);
					args.Response = HPItem(pokemon,(int)Math.Floor(pokemon.TotalHP/4f),scene);
					break;

				case Items.AWAKENING:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.CHESTO_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.BLUE_FLUTE:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.POKE_FLUTE:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.ANTIDOTE:
					if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.PECHA_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.BURN_HEAL:
					if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s burn was healed.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.RAWST_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s burn was healed.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.PARALYZE_HEAL://item == Items.PARLYZHEAL ||
					if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.CHERI_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.ICE_HEAL:
					if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} was thawed out.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.ASPEAR_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} was thawed out.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.FULL_HEAL:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.LAVA_COOKIE:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.OLD_GATEAU:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.CASTELIACONE:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.LUMIOSE_GALETTE:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.SHALOUR_SABLE:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;
				case Items.LUM_BERRY:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.FULL_RESTORE:
					if (pokemon.HP<=0 || (pokemon.HP==pokemon.TotalHP && pokemon.Status==0)) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						int hpgain=ItemRestoreHP(pokemon,pokemon.TotalHP-pokemon.HP);
						pokemon.HealStatus();
						scene.Refresh();
						if (hpgain>0) {
						scene.Display(Game._INTL("{1}'s HP was restored by {2} points.",pokemon.Name,hpgain));
						} else {
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						}
						//next true;
						args.Response = true;
					}
					break;

				case Items.REVIVE:
					if (pokemon.HP>0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HP=(int)Math.Floor(pokemon.TotalHP/2f);
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP was restored.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.MAX_REVIVE:
					if (pokemon.HP>0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealHP();
						pokemon.HealStatus();
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP was restored.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.ENERGY_POWDER:
					if (HPItem(pokemon,50,scene)) {
						pokemon.ChangeHappiness(HappinessMethods.POWDER);
						//next true;
						args.Response = true;
					}
					//next false;
					args.Response = false;
					break;

				case Items.ENERGY_ROOT:
					if (HPItem(pokemon,200,scene)) {
						pokemon.ChangeHappiness(HappinessMethods.ENERGYROOT);
						//next true;
						args.Response = true;
					}
					//next false;
					args.Response = false;
					break;

				case Items.HEAL_POWDER:
					if (pokemon.HP<=0 || pokemon.Status==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealStatus();
						pokemon.ChangeHappiness(HappinessMethods.POWDER);
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.REVIVAL_HERB:
					if (pokemon.HP>0) {
						scene.Display(Game._INTL("It won't have any effect."));
						//next false;
						args.Response = false;
					} else {
						pokemon.HealHP();
						pokemon.HealStatus();
						pokemon.ChangeHappiness(HappinessMethods.REVIVALHERB);
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP was restored.",pokemon.Name));
						//next true;
						args.Response = true;
					}
					break;

				case Items.ETHER:
					move=scene.ChooseMove(pokemon,Game._INTL("Restore which move?"));
					if (move>=0) {
						if (RestorePP(pokemon,move,10)==0) {
							scene.Display(Game._INTL("It won't have any effect."));
							//next false;
							args.Response = false;
						} else {
							scene.Display(Game._INTL("PP was restored."));
							//next true;
							args.Response = true;
						}
					}
					//next false;
					args.Response = false;
					break;
				case Items.LEPPA_BERRY:
					move=scene.ChooseMove(pokemon,Game._INTL("Restore which move?"));
					if (move>=0) {
						if (RestorePP(pokemon,move,10)==0) {
							scene.Display(Game._INTL("It won't have any effect."));
							//next false;
							args.Response = false;
						} else {
							scene.Display(Game._INTL("PP was restored."));
							//next true;
							args.Response = true;
						}
					}
					//next false;
					args.Response = false;
					break;

				case Items.MAX_ETHER:
					move=scene.ChooseMove(pokemon,Game._INTL("Restore which move?"));
					if (move>=0) {
						if (RestorePP(pokemon,move,pokemon.moves[move].TotalPP-pokemon.moves[move].PP)==0) {
							scene.Display(Game._INTL("It won't have any effect."));
							args.Response = false;
						} else {
							scene.Display(Game._INTL("PP was restored."));
							args.Response = true;
						}
					}
					args.Response = false;
					break;

				case Items.ELIXIR:
					int pprestored=0;
					for (int i = 0; i < pokemon.moves.Length; i++) {
						pprestored+=RestorePP(pokemon,i,10);
					}
					if (pprestored==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("PP was restored."));
						args.Response = true;
					}
					break;

				case Items.MAX_ELIXIR:
					pprestored=0;
					for (int i = 0; i < pokemon.moves.Length; i++) {
						pprestored+=RestorePP(pokemon,i,pokemon.moves[i].TotalPP-pokemon.moves[i].PP);
					}
					if (pprestored==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("PP was restored."));
						args.Response = true;
					}
					break;

				case Items.PP_UP:
					move=scene.ChooseMove(pokemon,Game._INTL("Boost PP of which move?"));
					if (move>=0) {
						if (pokemon.moves[move].TotalPP==0 || pokemon.moves[move].PPups>=3) {
							scene.Display(Game._INTL("It won't have any effect."));
							args.Response = false;
						} else {
							pokemon.moves[move].PPups+=1;
							string movename=Game._INTL(pokemon.moves[move].id.ToString(TextScripts.Name));
							scene.Display(Game._INTL("{1}'s PP increased.",movename));
							args.Response = true;
						}
					}
					args.Response = false;
					break;

				case Items.PP_MAX:
					move=scene.ChooseMove(pokemon,Game._INTL("Boost PP of which move?"));
					if (move>=0) {
						if (pokemon.moves[move].TotalPP==0 || pokemon.moves[move].PPups>=3) {
							scene.Display(Game._INTL("It won't have any effect."));
							args.Response = false;
						} else {
							pokemon.moves[move].PPups=3;
							string movename=Game._INTL(pokemon.moves[move].id.ToString(TextScripts.Name));
							scene.Display(Game._INTL("{1}'s PP increased.",movename));
							args.Response = true;
						}
					}
					args.Response = false;
					break;

				case Items.HP_UP:
					if (RaiseEffortValues(pokemon,Stats.HP)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.PROTEIN:
					if (RaiseEffortValues(pokemon,Stats.ATTACK)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Attack increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.IRON:
					if (RaiseEffortValues(pokemon,Stats.DEFENSE)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Defense increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.CALCIUM:
					if (RaiseEffortValues(pokemon,Stats.SPATK)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Special Attack increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.ZINC:
					if (RaiseEffortValues(pokemon,Stats.SPDEF)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Special Defense increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.CARBOS:
					if (RaiseEffortValues(pokemon,Stats.SPEED)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Speed increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.HEALTH_WING:
					if (RaiseEffortValues(pokemon,Stats.HP,1,false)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.MUSCLE_WING:
					if (RaiseEffortValues(pokemon,Stats.ATTACK,1,false)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Attack increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.RESIST_WING:
					if (RaiseEffortValues(pokemon,Stats.DEFENSE,1,false)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Defense increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.GENIUS_WING:
					if (RaiseEffortValues(pokemon,Stats.SPATK,1,false)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Special Attack increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.CLEVER_WING:
					if (RaiseEffortValues(pokemon,Stats.SPDEF,1,false)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Special Defense increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.SWIFT_WING:
					if (RaiseEffortValues(pokemon,Stats.SPEED,1,false)==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("{1}'s Speed increased.",pokemon.Name));
						pokemon.ChangeHappiness(HappinessMethods.VITAMIN);
						args.Response = true;
					}
					break;

				case Items.RARE_CANDY:
					if (pokemon.Level>=Core.MAXIMUMLEVEL || (pokemon is IPokemonShadowPokemon psp && psp.isShadow)) { //rescue false
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						ChangeLevel(pokemon,pokemon.Level+1,(IScene)scene);
						scene.HardRefresh();
						args.Response = true;
					}
					break;

				case Items.POMEG_BERRY:
					args.Response = RaiseHappinessAndLowerEV(pokemon,scene,Stats.HP,new string[] {
						Game._INTL("{1} adores you! Its base HP fell!",pokemon.Name),
						Game._INTL("{1} became more friendly. Its base HP can't go lower.",pokemon.Name),
						Game._INTL("{1} became more friendly. However, its base HP fell!",pokemon.Name)
					});
					break;

				case Items.KELPSY_BERRY:
					args.Response = RaiseHappinessAndLowerEV(pokemon,scene,Stats.ATTACK,new string[] {
						Game._INTL("{1} adores you! Its base Attack fell!",pokemon.Name),
						Game._INTL("{1} became more friendly. Its base Attack can't go lower.",pokemon.Name),
						Game._INTL("{1} became more friendly. However, its base Attack fell!",pokemon.Name)
					});
					break;

				case Items.QUALOT_BERRY:
					args.Response = RaiseHappinessAndLowerEV(pokemon,scene,Stats.DEFENSE,new string[] {
						Game._INTL("{1} adores you! Its base Defense fell!",pokemon.Name),
						Game._INTL("{1} became more friendly. Its base Defense can't go lower.",pokemon.Name),
						Game._INTL("{1} became more friendly. However, its base Defense fell!",pokemon.Name)
					});
					break;

				case Items.HONDEW_BERRY:
					args.Response = RaiseHappinessAndLowerEV(pokemon,scene,Stats.SPATK,new string[] {
						Game._INTL("{1} adores you! Its base Special Attack fell!",pokemon.Name),
						Game._INTL("{1} became more friendly. Its base Special Attack can't go lower.",pokemon.Name),
						Game._INTL("{1} became more friendly. However, its base Special Attack fell!",pokemon.Name)
					});
					break;

				case Items.GREPA_BERRY:
					args.Response = RaiseHappinessAndLowerEV(pokemon,scene,Stats.SPDEF,new string[] {
						Game._INTL("{1} adores you! Its base Special Defense fell!",pokemon.Name),
						Game._INTL("{1} became more friendly. Its base Special Defense can't go lower.",pokemon.Name),
						Game._INTL("{1} became more friendly. However, its base Special Defense fell!",pokemon.Name)
					});
					break;

				case Items.TAMATO_BERRY:
					args.Response = RaiseHappinessAndLowerEV(pokemon,scene,Stats.SPEED,new string[] {
						Game._INTL("{1} adores you! Its base Speed fell!",pokemon.Name),
						Game._INTL("{1} became more friendly. Its base Speed can't go lower.",pokemon.Name),
						Game._INTL("{1} became more friendly. However, its base Speed fell!",pokemon.Name)
					});
					break;

				case Items.GRACIDEA:
					//if (pokemon.Species == Pokemons.SHAYMIN && ((Pokemon)pokemon).FormId==0 &&
					if (pokemon.Species == Pokemons.SHAYMIN && (pokemon is IPokemonMultipleForms pf && pf.form==0) &&
						pokemon.Status!=Status.FROZEN && !IsNight) {
						if (pokemon.HP>0) {
							//((Pokemon)pokemon).SetForm(1);
							pf.form = 1;
							scene.Refresh();
							scene.Display(Game._INTL("{1} changed Forme!",pokemon.Name));
							args.Response = true;
						} else {
							scene.Display(Game._INTL("This can't be used on the fainted Pokémon."));
						}
					} else {
						scene.Display(Game._INTL("It had no effect."));
						args.Response = false;
					}
					args.Response = false;
					break;

				case Items.REVEAL_GLASS:
					if ((pokemon.Species == Pokemons.TORNADUS ||
						pokemon.Species == Pokemons.THUNDURUS ||
						pokemon.Species == Pokemons.LANDORUS)) {
						if (pokemon.HP>0) {
							((Pokemon)pokemon).SetForm(((Pokemon)pokemon).FormId==0 ? 1 : 0);
							//if (pokemon is IPokemonMultipleForms mf) mf.form=((mf.form==0) ? 1 : 0);
							scene.Refresh();
							scene.Display(Game._INTL("{1} changed Forme!",pokemon.Name));
							args.Response = true;
						} else {
							scene.Display(Game._INTL("This can't be used on the fainted Pokémon."));
						}
					} else {
						scene.Display(Game._INTL("It had no effect."));
						args.Response = false;
					}
					args.Response = false;
					break;

				case Items.DNA_SPLICERS:
					if (pokemon.Species == Pokemons.KYUREM) {
						if (pokemon.HP>0) {
							if (pokemon.fused!=null) {
								if (Trainer.party.Length>=6) { //ToDo: Party count has 2 slots open
									scene.Display(Game._INTL("You have no room to separate the Pokémon."));
									args.Response = false;
								} else {
									Trainer.party[Trainer.party.Length]=pokemon.fused[1];
									pokemon.fused=null;
									((Pokemon)pokemon).SetForm(0);
									//if (pokemon is IPokemonMultipleForms mf) mf.form=0;
									scene.HardRefresh();
									scene.Display(Game._INTL("{1} changed Forme!",pokemon.Name));
									args.Response = true;
								}
							} else {
								int chosen=scene.ChoosePokemon(Game._INTL("Fuse with which Pokémon?"));
								if (chosen>=0) {
									IPokemon poke2=Trainer.party[chosen];
									if ((poke2.Species == Pokemons.RESHIRAM ||
										poke2.Species == Pokemons.ZEKROM) && poke2.HP>0 && !poke2.isEgg) {
										if (poke2.Species == Pokemons.RESHIRAM) ((Pokemon)pokemon).SetForm(1);
										if (poke2.Species == Pokemons.ZEKROM) ((Pokemon)pokemon).SetForm(2);
										pokemon.fused=new IPokemon[] { pokemon, poke2 }; //poke2;
										//ToDo: Combine stats and divide down the middle? (IV/EV)
										RemovePokemonAt(chosen);
										scene.HardRefresh();
										scene.Display(Game._INTL("{1} changed Forme!",pokemon.Name));
										args.Response = true;
									} else if (poke2.isEgg) {
										scene.Display(Game._INTL("It cannot be fused with an Egg."));
									} else if (poke2.HP<=0) {
										scene.Display(Game._INTL("It cannot be fused with that fainted Pokémon."));
									} else if (pokemon==poke2) {
										scene.Display(Game._INTL("It cannot be fused with itself."));
									} else {
										scene.Display(Game._INTL("It cannot be fused with that Pokémon."));
									}
								} else {
									args.Response = false;
								}
							}
						} else {
							scene.Display(Game._INTL("This can't be used on the fainted Pokémon."));
						}
					} else {
						scene.Display(Game._INTL("It had no effect."));
						args.Response = false;
					}
					args.Response = false;
					break;

				case Items.PRISON_BOTTLE:
					if (pokemon.Species == Pokemons.HOOPA) {
						if (pokemon.HP>0) {
							((Pokemon)pokemon).SetForm(((Pokemon)pokemon).FormId == 0 ? 1 : 0);
							scene.Refresh();
							scene.Display(Game._INTL("{1} changed Forme!",pokemon.Name));
							args.Response = true;
						} else {
							scene.Display(Game._INTL("This can't be used on the fainted Pokémon."));
						}
					} else {
						scene.Display(Game._INTL("It had no effect."));
						args.Response = false;
					}
					args.Response = false;
					break;

				case Items.ABILITY_CAPSULE:
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
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					}
					int newabil=(pokemon.abilityIndex+1)%2;
					string newabilname=(newabil==0) ? Game._INTL(abil1.ToString(TextScripts.Name)) : Game._INTL(abil2.ToString(TextScripts.Name));
					if (scene.DisplayConfirm(Game._INTL("Would you like to change {1}'s Ability to {2}?",
					//if (GameMessage.ConfirmMessage(Game._INTL("Would you like to change {1}'s Ability to {2}?",
						pokemon.Name,newabilname))) {
						pokemon.setAbility(newabil);
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s Ability changed to {2}!",pokemon.Name,
						Game._INTL(pokemon.Ability.ToString(TextScripts.Name))));
						args.Response = true;
					}
					args.Response = false;
					break;
				#endregion
			}
		}

		protected virtual void RegisterItemHandlers(object sender, BattleUseOnPokemonEventArgs args) {
			IPokemon pokemon = args.Pokemon;
			IBattler battler = args.Battler;
			int move = -1;
			//PokemonEssentials.Interface.Screen.IScene scene = args.Scene;
			//PokemonEssentials.Interface.Screen.IHasDisplayMessage scene = args.Scene;
			PokemonEssentials.Interface.Screen.IPartyDisplayScene scene = args.Scene;
			switch (args.Item) {
				#region BattleUseOnPokemon handlers
				case Items.POTION:
					args.Response = BattleHPItem(pokemon,battler,20,scene);
					break;

				case Items.SUPER_POTION:
					args.Response = BattleHPItem(pokemon,battler,50,scene);
					break;

				case Items.HYPER_POTION:
					args.Response = BattleHPItem(pokemon,battler,200,scene);
					break;

				case Items.MAX_POTION:
					args.Response = BattleHPItem(pokemon,battler,pokemon.TotalHP-pokemon.HP,scene);
					break;

				case Items.BERRY_JUICE:
					args.Response = BattleHPItem(pokemon,battler,20,scene);
					break;

				case Items.RAGE_CANDY_BAR:
					args.Response = BattleHPItem(pokemon,battler,20,scene);
					break;

				case Items.SWEET_HEART:
					args.Response = BattleHPItem(pokemon,battler,20,scene);
					break;

				case Items.FRESH_WATER:
					args.Response = BattleHPItem(pokemon,battler,50,scene);
					break;

				case Items.SODA_POP:
					args.Response = BattleHPItem(pokemon,battler,60,scene);
					break;

				case Items.LEMONADE:
					args.Response = BattleHPItem(pokemon,battler,80,scene);
					break;

				case Items.MOOMOO_MILK:
					args.Response = BattleHPItem(pokemon,battler,100,scene);
					break;

				case Items.ORAN_BERRY:
					args.Response = BattleHPItem(pokemon,battler,10,scene);
					break;

				case Items.SITRUS_BERRY:
					args.Response = BattleHPItem(pokemon,battler,(int)Math.Floor(pokemon.TotalHP/4f),scene);
					break;

				case Items.AWAKENING:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.CHESTO_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.BLUE_FLUTE:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.POKE_FLUTE:
					if (pokemon.HP<=0 || pokemon.Status!=Status.SLEEP) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} woke up.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.ANTIDOTE:
					if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.PECHA_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.POISON) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of its poisoning.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.BURN_HEAL:
					if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s burn was healed.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.RAWST_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.BURN) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s burn was healed.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.PARALYZE_HEAL://item == Items.PARLYZHEAL ||
					if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.CHERI_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.PARALYSIS) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} was cured of paralysis.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.ICE_HEAL:
					if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} was thawed out.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.ASPEAR_BERRY:
					if (pokemon.HP<=0 || pokemon.Status!=Status.FROZEN) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} was thawed out.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.FULL_HEAL:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.LAVA_COOKIE:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.OLD_GATEAU:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.CASTELIACONE:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.LUMIOSE_GALETTE:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.SHALOUR_SABLE:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;
				case Items.LUM_BERRY:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.FULL_RESTORE:
					if (pokemon.HP<=0 || (pokemon.HP==pokemon.TotalHP && pokemon.Status==0 &&
						(!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						int hpgain=ItemRestoreHP(pokemon,pokemon.TotalHP-pokemon.HP);
						if (battler.IsNotNullOrNone()) battler.HP=pokemon.HP;
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						scene.Refresh();
						if (hpgain>0) {
							scene.Display(Game._INTL("{1}'s HP was restored by {2} points.",pokemon.Name,hpgain));
						} else {
							scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						}
						args.Response = true;
					}
					break;

				case Items.REVIVE:
					if (pokemon.HP>0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HP=(int)Math.Floor(pokemon.TotalHP/2f);
						//ItemRestoreHP(pokemon,(int)Math.Floor(pokemon.TotalHP/2f));
						pokemon.HealStatus();
						for (int i = 0; i < Trainer.party.Length; i++) {
							if (Trainer.party[i]==pokemon) {
								if (battler.IsNotNullOrNone()) battler.Initialize(pokemon,(sbyte)i,false);
								break;
							}
						}
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP was restored.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.MAX_REVIVE:
					if (pokemon.HP>0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealHP();
						pokemon.HealStatus();
						for (int i = 0; i < Trainer.party.Length; i++) {
							if (Trainer.party[i]==pokemon) {
								if (battler.IsNotNullOrNone()) battler.Initialize(pokemon,(sbyte)i,false);
								break;
							}
						}
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP was restored.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.ENERGY_POWDER:
					if (BattleHPItem(pokemon,battler,50,scene)) {
						pokemon.ChangeHappiness(HappinessMethods.POWDER);
						args.Response = true;
					}
					args.Response = false;
					break;

				case Items.ENERGY_ROOT:
					if (BattleHPItem(pokemon,battler,200,scene)) {
						pokemon.ChangeHappiness(HappinessMethods.ENERGYROOT);
						args.Response = true;
					}
					args.Response = false;
					break;

				case Items.HEAL_POWDER:
					if (pokemon.HP<=0 || (pokemon.Status==0 && (!battler.IsNotNullOrNone() || battler.effects.Confusion==0))) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						pokemon.HealStatus();
						if (battler.IsNotNullOrNone()) battler.Status=0;
						if (battler.IsNotNullOrNone()) battler.effects.Confusion=0;
						pokemon.ChangeHappiness(HappinessMethods.POWDER);
						scene.Refresh();
						scene.Display(Game._INTL("{1} became healthy.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.REVIVAL_HERB:
					if (pokemon.HP>0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
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
						scene.Refresh();
						scene.Display(Game._INTL("{1}'s HP was restored.",pokemon.Name));
						args.Response = true;
					}
					break;

				case Items.ETHER:
					move=scene.ChooseMove(pokemon,Game._INTL("Restore which move?"));
					if (move>=0) {
						if (BattleRestorePP(pokemon,battler,move,10)==0) {
							scene.Display(Game._INTL("It won't have any effect."));
							args.Response = false;
						} else {
							scene.Display(Game._INTL("PP was restored."));
							args.Response = true;
						}
					}
					args.Response = false;
					break;
				case Items.LEPPA_BERRY:
					move=scene.ChooseMove(pokemon,Game._INTL("Restore which move?"));
					if (move>=0) {
						if (BattleRestorePP(pokemon,battler,move,10)==0) {
							scene.Display(Game._INTL("It won't have any effect."));
							args.Response = false;
						} else {
							scene.Display(Game._INTL("PP was restored."));
							args.Response = true;
						}
					}
					args.Response = false;
					break;

				case Items.MAX_ETHER:
					move=scene.ChooseMove(pokemon,Game._INTL("Restore which move?"));
					if (move>=0) {
						if (BattleRestorePP(pokemon,battler,move,pokemon.moves[move].TotalPP-pokemon.moves[move].PP)==0) {
							scene.Display(Game._INTL("It won't have any effect."));
							args.Response = false;
						} else {
							scene.Display(Game._INTL("PP was restored."));
							args.Response = true;
						}
					}
					args.Response = false;
					break;

				case Items.ELIXIR:
					int pprestored=0;
					for (int i = 0; i < pokemon.moves.Length; i++) {
						pprestored+=BattleRestorePP(pokemon,battler,i,10);
					}
					if (pprestored==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("PP was restored."));
						args.Response = true;
					}
					break;

				case Items.MAX_ELIXIR:
					pprestored=0;
					for (int i = 0; i < pokemon.moves.Length; i++) {
						pprestored+=BattleRestorePP(pokemon,battler,i,pokemon.moves[i].TotalPP-pokemon.moves[i].PP);
					}
					if (pprestored==0) {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					} else {
						scene.Display(Game._INTL("PP was restored."));
						args.Response = true;
					}
					break;

				case Items.RED_FLUTE:
					if (battler.IsNotNullOrNone() && battler.effects.Attract>=0) {
						battler.effects.Attract=-1;
						scene.Display(Game._INTL("{1} got over its infatuation.",pokemon.Name));
						args.Response = true; // Items.consumed:
					} else {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					}
					break;

				case Items.YELLOW_FLUTE:
					if (battler.IsNotNullOrNone() && battler.effects.Confusion>0) {
						battler.effects.Confusion=0;
						scene.Display(Game._INTL("{1} snapped out of confusion.",pokemon.Name));
						args.Response = true; // Items.consumed:
					} else {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					}
					break;
				case Items.PERSIM_BERRY:
					if (battler.IsNotNullOrNone() && battler.effects.Confusion>0) {
						battler.effects.Confusion=0;
						scene.Display(Game._INTL("{1} snapped out of confusion.",pokemon.Name));
						args.Response = true; // Items.consumed:
					} else {
						scene.Display(Game._INTL("It won't have any effect."));
						args.Response = false;
					}
					break;
				#endregion
			}
		}

		protected virtual void RegisterItemHandlers(object sender, BattleUseOnBattlerEventArgs args) {
			IBattler battler = args.Battler;
			IBattlerEffect battlerE = null;
			if (battler is IBattlerEffect) battlerE = args.Battler as IBattlerEffect;
			IBattle battle = battler.battle;
			Items item = args.Item;
			string playername = string.Empty;
			PokemonEssentials.Interface.Screen.IHasDisplayMessage scene = args.Scene;
			switch (args.Item) {
				#region BattleUseOnBattler handlers
				case Items.X_ATTACK:
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.ATTACK,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.ATTACK,1,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_ATTACK_2:
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.ATTACK,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.ATTACK,2,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_ATTACK_3:
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.ATTACK,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.ATTACK,3,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_ATTACK_6:
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.IncreaseStatWithCause(Combat.Stats.ATTACK,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_DEFENSE: //item == Items.X_DEFEND ||
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.DEFENSE,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.DEFENSE,1,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false  ;
					}
					break;

				case Items.X_DEFENSE_2: //item == Items.XDEFEND2 ||
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.DEFENSE,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.DEFENSE,2,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_DEFENSE_3: //item == Items.XDEFEND3 ||
					playername=battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.DEFENSE,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.DEFENSE,3,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_DEFENSE_6: //item == Items.XDEFEND6 ||
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.IncreaseStatWithCause(Combat.Stats.DEFENSE,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_ATK: //item == Items.X_SPECIAL ||
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPATK,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPATK,1,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_ATK_2: //item == Items.XSPECIAL2 ||
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPATK,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPATK,2,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_ATK_3: //item == Items.XSPECIAL3 ||
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPATK,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPATK,3,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_ATK_6: //item == Items.XSPECIAL6 ||
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.IncreaseStatWithCause(Combat.Stats.SPATK,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_DEF:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPDEF,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPDEF,1,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_DEF_2:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPDEF,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPDEF,2,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_DEF_3:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPDEF,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPDEF,3,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SP_DEF_6:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.IncreaseStatWithCause(Combat.Stats.SPDEF,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SPEED:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPEED,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPEED,1,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SPEED_2:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPEED,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPEED,2,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SPEED_3:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.SPEED,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.SPEED,3,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_SPEED_6:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.IncreaseStatWithCause(Combat.Stats.SPEED,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_ACCURACY:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.ACCURACY,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.ACCURACY,1,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_ACCURACY_2:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.ACCURACY,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.ACCURACY,2,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.X_ACCURACY_3:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.CanIncreaseStatStage(Combat.Stats.ACCURACY,battler,false)) {
						battlerE.IncreaseStat(Combat.Stats.ACCURACY,3,battler,true);
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false  ;
					}
					break;

				case Items.X_ACCURACY_6:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battlerE.IncreaseStatWithCause(Combat.Stats.ACCURACY,6,battler,Game._INTL(item.ToString(TextScripts.Name)))) {
						args.Response = true;
					} else {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					}
					break;

				case Items.DIRE_HIT:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battler.effects.FocusEnergy>=1) {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					} else {
						battler.effects.FocusEnergy=1;
						scene.Display(Game._INTL("{1} is getting pumped!",battler.ToString()));
						args.Response = true;
					}
					break;

				case Items.DIRE_HIT_2:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battler.effects.FocusEnergy>=2) {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					} else {
						battler.effects.FocusEnergy=2;
						scene.Display(Game._INTL("{1} is getting pumped!",battler.ToString()));
						args.Response = true;
					}
					break;

				case Items.DIRE_HIT_3:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battler.effects.FocusEnergy>=3) {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					} else {
						battler.effects.FocusEnergy=3;
						scene.Display(Game._INTL("{1} is getting pumped!",battler.ToString()));
						args.Response = true;
					}
					break;

				case Items.GUARD_SPEC:
					playername = battler.battle.Player().name;
					scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
					if (battler.OwnSide.Mist>0) {
						scene.Display(Game._INTL("But it had no effect!"));
						args.Response = false;
					} else {
						battler.OwnSide.Mist=5;
						//if (!scene.IsOpposing(battler.Index)) { //Create new Delegate for attacker?
						if (!battle.IsOpposing(battler.Index)) { //if player's pokemon...
							scene.Display(Game._INTL("Your team became shrouded in mist!"));
						} else {
							scene.Display(Game._INTL("The foe's team became shrouded in mist!"));
						}
						args.Response = true;
					}
					break;

				case Items.POKE_DOLL:
					battle=battler.battle;
					if (battle.opponent != null) {
						scene.Display(Game._INTL("Can't use that here."));
						args.Response = false;
					} else {
						playername=battle.Player().name;
						scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
						args.Response = true;
					}
					break;
				case Items.FLUFFY_TAIL:
					battle=battler.battle;
					if (battle.opponent != null) {
						scene.Display(Game._INTL("Can't use that here."));
						args.Response = false;
					} else {
						playername=battle.Player().name;
						scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
						args.Response = true;
					}
					break;
				case Items.POKE_TOY:
					battle=battler.battle;
					if (battle.opponent != null) {
						scene.Display(Game._INTL("Can't use that here."));
						args.Response = false;
					} else {
						playername=battle.Player().name;
						scene.Display(Game._INTL("{1} used the {2}.",playername,Game._INTL(item.ToString(TextScripts.Name))));
						args.Response = true;
					}
					break;

				//if (Item.IsPokeBall(item)) {  // Any Poké Ball
				case Items.POKE_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					//if (battle.Player().party.Length>=6 && !PC.full) {
					if (battle.Player().party.GetCount()>=(Global?.Features.LimitPokemonPartySize??Core.MAXPARTYSIZE) && !PokemonStorage.full) {
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.BEAST_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.CHERISH_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.DIVE_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.DREAM_BALL: //ToDo: Only in dreamworld?
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.DUSK_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.FAST_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.FRIEND_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
						scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
						args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.GREAT_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.HEAL_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.HEAVY_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.IRON_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.LEVEL_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.LIGHT_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.LOVE_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.LURE_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.LUXURY_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.MASTER_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.MOON_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.NEST_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.NET_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.PARK_BALL: //ToDo: Only in park?
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.PREMIER_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.QUICK_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.REPEAT_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.SAFARI_BALL: //ToDo: Only during safari contest?
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.SMOKE_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.SPORT_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.TIMER_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				case Items.ULTRA_BALL:
					battle=battler.battle;
					if (!battler.Opposing1.isFainted() && !battler.Opposing2.isFainted()) {
						if (!(this as IItemCheck).IsSnagBall(item)) { //battle.IsSnagBall(item)
							scene.Display(Game._INTL("It's no good! It's impossible to aim when there are two Pokémon!"));
							args.Response = false;
						}
					}
					if (battle.Player().party.Length>=6 && !PokemonStorage.full) { //PC.full
						scene.Display(Game._INTL("There is no room left in the PC!"));
						args.Response = false;
					}
					args.Response = true;
					break;
				#endregion
			}
		}

		protected virtual void RegisterItemHandlers(object sender, UseInBattleEventArgs args) {
			Items item = args.Item;
			IBattle battle = args.Battle;
			IBattler battler = args.Battler;
			//args.Response = false;
			switch (args.Item) {
				#region UseInBattle handlers
				case Items.POKE_DOLL:
					battle.decision=Combat.BattleResults.FORFEIT;
					battle.DisplayPaused(Game._INTL("Got away safely!"));
					break;
				case Items.FLUFFY_TAIL:
					battle.decision=Combat.BattleResults.FORFEIT;
					battle.DisplayPaused(Game._INTL("Got away safely!"));
					break;
				case Items.POKE_TOY:
					battle.decision=Combat.BattleResults.FORFEIT;
					battle.DisplayPaused(Game._INTL("Got away safely!"));
					break;

				//if (Item.IsPokeBall(item))// Any Poké Ball
				case Items.POKE_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.BEAST_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.CHERISH_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.DIVE_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.DREAM_BALL: //ToDo: Only in dreamworld?
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.DUSK_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.FAST_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.FRIEND_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.GREAT_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.HEAL_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.HEAVY_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.IRON_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.LEVEL_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.LIGHT_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.LOVE_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.LURE_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.LUXURY_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.MASTER_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.MOON_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.NEST_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.NET_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.PARK_BALL: //ToDo: Only in park?
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.PREMIER_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.QUICK_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.REPEAT_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.SAFARI_BALL: //ToDo: Only during safari contest?
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.SMOKE_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.SPORT_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.TIMER_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				case Items.ULTRA_BALL:
					battle.ThrowPokeball(battler.Index, item);
					break;
				#endregion
			}
		}

		//Events.onStepTaken+=proc {
		//Events.OnStepTaken+=OnStepTakenEventHandler;
		private void OnStepTakenEventHandler(object sender, OnStepTakenFieldMovementEventArgs e)
		{
			if (!Terrain.isIce(GamePlayer.terrain_tag)) {		// Shouldn't count down if on ice
				if (Global.repel>0) {
					Global.repel-=1;
					if (Global.repel<=0) {
						if (this is IGameMessage gm) gm.Message(Game._INTL("Repel's effect wore off..."));
						Items ret=this is IGameItem g ? g.ChooseItemFromList(Game._INTL("Do you want to use another Repel?"),1,
							Items.REPEL, Items.SUPER_REPEL, Items.MAX_REPEL) : Items.NONE;
						if (ret>0) UseItem(Bag,ret); //Item.UseItem(Bag,ret);
					}
				}
			}
		}
	}

	namespace EventArg
	{
		#region Item Handlers EventArgs
		public class UseFromBagEventArgs : EventArgs, IUseFromBagEventArgs
		{
			public static readonly int EventId = typeof(UseFromBagEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Item { get; set; }
			public ItemUseResults Response { get; set; }
		}
		public class UseInFieldEventArgs : EventArgs, IUseInFieldEventArgs
		{
			public static readonly int EventId = typeof(UseInFieldEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Item { get; set; }
			//public Action Action { get; set; }
			public bool Response { get; set; }
		}
		public class UseOnPokemonEventArgs : EventArgs, IUseOnPokemonEventArgs
		{
			public static readonly int EventId = typeof(UseOnPokemonEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Item { get; set; }
			public IPokemon Pokemon { get; set; }
			//public PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
			public PokemonEssentials.Interface.Screen.IPartyDisplayScene Scene { get; set; }
			public bool Response { get; set; }
		}
		public class BattleUseOnPokemonEventArgs : EventArgs, IBattleUseOnPokemonEventArgs
		{
			public static readonly int EventId = typeof(BattleUseOnPokemonEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Item { get; set; }
			public IPokemon Pokemon { get; set; }
			public IBattler Battler { get; set; }
			//public PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
			public PokemonEssentials.Interface.Screen.IPartyDisplayScene Scene { get; set; }
			public bool Response { get; set; }
		}
		public class BattleUseOnBattlerEventArgs : EventArgs, IBattleUseOnBattlerEventArgs
		{
			public static readonly int EventId = typeof(BattleUseOnBattlerEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Item { get; set; }
			public IBattler Battler { get; set; }
			public PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
			public bool Response { get; set; }
		}
		public class UseInBattleEventArgs : EventArgs, IUseInBattleEventArgs
		{
			public static readonly int EventId = typeof(UseInBattleEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			public Items Item { get; set; }
			public IBattler Battler { get; set; }
			public IBattle Battle { get; set; }
		}
		#endregion
	}
}