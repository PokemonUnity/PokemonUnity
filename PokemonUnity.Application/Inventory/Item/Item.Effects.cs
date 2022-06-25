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
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface;

namespace PokemonUnity//.Inventory
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
		public PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
		public bool Response { get; set; }
	}
	public class BattleUseOnPokemonEventArgs : EventArgs, IBattleUseOnPokemonEventArgs
	{
		public static readonly int EventId = typeof(BattleUseOnPokemonEventArgs).GetHashCode();

		public int Id { get { return EventId; } }
		public Items Item { get; set; }
		public IPokemon Pokemon { get; set; }
		public IBattler Battler { get; set; }
		public PokemonEssentials.Interface.Screen.IHasDisplayMessage Scene { get; set; }
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

	//public static partial class ItemHandlers
	public partial class Game : IGameItemEffect
	{
		public ItemUseResults pbRepel(Items item,int steps) {
			if (RepelSteps>0) {
				(this as IGameMessage).pbMessage(Game._INTL("But the effects of a Repel lingered from earlier."));
				//return 0;
				return ItemUseResults.NotUsed;
			} else {
				(this as IGameMessage).pbMessage(Game._INTL("{1} used the {2}.",Trainer.name,Game._INTL(item.ToString(TextScripts.Name))));
				RepelSteps=steps;
				//return 3;
				return ItemUseResults.UsedItemConsumed;
			}
		}

		//static ItemHandlers() {
		/*private void RegisterItemHandlers() {
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
				@event=Item.pbClosestHiddenItem();
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
				return Item.pbHPItem(pokemon,20,scene);
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
					if (!scene.pbIsOpposing(battler.Index)) { //Create new Delegate for attacker?
					//if (!scene.pbIsOpposing(battler.Index)) { //if player's pokemon...
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


		//Events.onStepTaken+=proc {
		private void OnStepTakenEventHandler(object sender, EventArgs e)
		{
			if (!Terrain.isIce(Player.terrain_tag)) {		// Shouldn't count down if on ice
				if (RepelSteps>0) {
					RepelSteps-=1;
					if (RepelSteps<=0) {
						(this as IGameMessage).pbMessage(Game._INTL("Repel's effect wore off..."));
						Items ret=this is IGameItem g ? g.pbChooseItemFromList(Game._INTL("Do you want to use another Repel?"),1,
							Items.REPEL, Items.SUPER_REPEL, Items.MAX_REPEL) : Items.NONE;
						if (ret>0) pbUseItem(Bag,ret); //Item.pbUseItem(Bag,ret);
					}
				}
			}
		}
	}
}