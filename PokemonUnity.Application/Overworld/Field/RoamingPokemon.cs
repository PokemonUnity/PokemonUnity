using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonUnity
{
	public partial class TempData : PokemonEssentials.Interface.Field.ITempMetadataRoaming {
		public int nowRoaming				{ get; protected set; }
		public int roamerIndex				{ get; protected set; }
	}

	public partial class GlobalMetadata : PokemonEssentials.Interface.Field.IGlobalMetadataRoaming {
		public int[] roamPosition				{ get; set; }
		public Queue<int> roamHistory			{ get; set; }
		public bool roamedAlready				{ get; set; }
		public bool roamEncounter				{ get; set; }
		public IList<IPokemon> roamPokemon		{ get; set; }
		public Pokemons[] roamPokemonCaught     { get {
			if (_roamPokemonCaught == null) {
				_roamPokemonCaught=new List<Pokemons>();
			}
			return _roamPokemonCaught.ToArray();
		} }
		private IList<Pokemons> _roamPokemonCaught;
	}

	public partial class Game : PokemonEssentials.Interface.Field.IGameRoaming {
		/// <summary>
		/// Resets all roaming Pokemon that were defeated without having been caught.
		/// </summary>
		public void ResetAllRoamers() {
			if (Game.GameData.Global != null && Game.GameData.Global.roamPokemon != null) {
				for (int i = 0; i < Game.GameData.Global.roamPokemon.Count; i++) {
					if (Game.GameData.Global.roamPokemon[i].IsNotNullOrNone() && Game.GameData.Global is IGlobalMetadataRoaming gmr && gmr.roamPokemonCaught[i]==Pokemons.NONE) { //gmr.roamPokemonCaught[i]!=true
						Game.GameData.Global.roamPokemon[i]=null;
					}
				}
			}
		}

		/// <summary>
		/// Gets the roaming areas for a particular Pokémon.
		/// </summary>
		public IDictionary<int,int[]> RoamingAreas(int index) {
			if (Kernal.RoamingSpecies.Length < index) return new Dictionary<int, int[]>();
			RoamingEncounterData data=Kernal.RoamingSpecies[index];
			//if (data != null && data[5] != null) return data[5].Value;
			if (data.Areas != null) return data.Areas; //data != null &&
			return new Dictionary<int, int[]>(); //RoamingAreas();
		}

		/// <summary>
		/// Puts a roamer in a completely random map available to it.
		/// </summary>
		/// <param name="index"></param>
		public void RandomRoam(int index) {
			if (Game.GameData.Global.roamPosition != null) {
				int[] keys=RoamingAreas(index).Keys.ToArray();
				Game.GameData.Global.roamPosition[index]=keys[Core.Rand.Next(keys.Length)];
			}
		}

		/// <summary>
		/// Roams all roamers, if their Switch is on.
		/// </summary>
		/// <param name="ignoretrail"></param>
		public void RoamPokemon(bool ignoretrail=false) {
			//Start all roamers off in random maps
			if (Game.GameData.Global.roamPosition == null) {
				Game.GameData.Global.roamPosition=new int[Kernal.RoamingSpecies.Length];
				for (int i = 0; i < Kernal.RoamingSpecies.Length; i++) {
					Pokemons? species=Kernal.RoamingSpecies[i].Pokemon;//[0]
					if (species==null || species<=0) continue;
					int[] keys=RoamingAreas(i).Keys.ToArray();
					Game.GameData.Global.roamPosition[i]=keys[Core.Rand.Next(keys.Length)];
				}
			}
			if (Game.GameData.Global.roamHistory == null) Game.GameData.Global.roamHistory=new Queue<int>();
			if (Game.GameData.Global.roamPokemon == null) Game.GameData.Global.roamPokemon=new List<IPokemon>();
			//Roam each Pokémon in turn
			for (int i = 0; i < Kernal.RoamingSpecies.Length; i++) {
				RoamingEncounterData poke=Kernal.RoamingSpecies[i];
				if (Game.GameData.GameSwitches[poke.SwitchId]) {//[2]
					//Pokemons species=getID(Species,poke[0]);
					Pokemons species=poke.Pokemon;
					if (species<=0) continue; //next; //species == null ||
					List<int> choices=new List<int>();
					int[] keys=RoamingAreas(i).Keys.ToArray();
					int? currentArea=Game.GameData.Global.roamPosition[i];
					if (currentArea==null) {
						Game.GameData.Global.roamPosition[i]=keys[Core.Rand.Next(keys.Length)];
						currentArea=Game.GameData.Global.roamPosition[i];
					}
					int[] newAreas=RoamingAreas(i)[currentArea.Value];
					if (newAreas == null) continue; //next;
					foreach (int area in newAreas) {
						bool inhistory=Game.GameData.Global.roamHistory.Contains(area);
						if (ignoretrail) inhistory=false;
						if (!inhistory) choices.Add(area);
					}
					if (Core.Rand.Next(32)==0 && keys.Length>0) {
						int area=keys[Core.Rand.Next(keys.Length)];
						bool inhistory=Game.GameData.Global.roamHistory.Contains(area);
						if (ignoretrail) inhistory=false;
						if (!inhistory) choices.Add(area);
					}
					if (choices.Count>0 ) {
						int area=choices[Core.Rand.Next(choices.Count)];
						Game.GameData.Global.roamPosition[i]=area;
					}
				}
			}
		}

		//Events.OnMapChange+=proc {|sender,e|
		//   if (Game.GameData.Global == null) return;
		//   mapinfosload_data("Data/MapInfos.rxdata");=//$RPGVX ? load_data("Data/MapInfos.rvdata") :
		//   if (Game.GameData.GameMap && mapinfos && e[0]>0 && mapinfos[e[0]] &&
		//             mapinfos[e[0]].Name && Game.GameData.GameMap.Name==mapinfos[e[0]].Name) return;
		//   RoamPokemon();
		//   if (Game.GameData.Global.roamHistory.Length>=2) {
		//     Game.GameData.Global.roamHistory.Dequeue(); //.shift();
		//   }
		//   Game.GameData.Global.roamedAlready=false;
		//   Game.GameData.Global.roamHistory.Add(Game.GameData.GameMap.map_id);
		//}

		//Events.OnWildBattleOverride+=proc { |sender,e|
		//   Pokemons species=e[0];
		//   int level=e[1];
		//   bool?[] handled=e[2];
		//   if (handled[0]!=null) continue;
		//   if (Game.GameData.Temp.nowRoaming == null) continue;
		//   if (Game.GameData.Temp.roamerIndex==null) continue;
		//   if (Game.GameData.Global.roamEncounter == null) continue;
		//   handled[0]=RoamingPokemonBattle(species,level);
		//}

		public bool RoamingPokemonBattle(Pokemons species,int level) {
			IPokemon genwildpoke; int index=0;//Game.GameData.PokemonTemp.roamerIndex;
			if (Game.GameData.PokemonTemp is ITempMetadataRoaming tmr) index=tmr.roamerIndex;
			if (Game.GameData.Global.roamPokemon[index].IsNotNullOrNone()) {
				//&& Game.GameData.Global.roamPokemon[index] is PokeBattle_Pokemon) {
				genwildpoke=Game.GameData.Global.roamPokemon[index];
			} else {
				genwildpoke=GenerateWildPokemon(species,level,true);
			}

			//Events.onStartBattle.trigger(null,genwildpoke);
			PokemonEssentials.Interface.EventArg.IOnWildPokemonCreateEventArgs createWildArgs = new PokemonUnity.EventArg.OnWildPokemonCreateEventArgs()
			{
				Pokemon = genwildpoke
			};
			Events.OnStartBattleTrigger(this);//,genwildpoke
			IScene scene=NewBattleScene();
			//battle=new PokeBattle_Battle(scene,Game.GameData.Player.Party,[genwildpoke],Game.GameData.Player,null);
			IBattle battle=new Combat.Battle(scene,Game.GameData.Trainer.party,new IPokemon[] { genwildpoke },Game.GameData.Trainer,null);
			battle.internalbattle=true;
			battle.cantescape=false;
			battle.rules["alwaysflee"]=true;
			PrepareBattle(battle);
			Combat.BattleResults decision=0;
			BattleAnimation(GetWildBattleBGM(species), block: () => {
				SceneStandby(() => {
					decision=battle.StartBattle();
				});
				foreach (IPokemon i in Game.GameData.Trainer.party) { if (i is PokemonEssentials.Interface.PokeBattle.IPokemonMegaEvolution m) { m.makeUnmega(); m.makeUnprimal(); } } //rescue null);
				if (Game.GameData.Global.partner != null) {
					HealAll();
					foreach (IPokemon i in Game.GameData.Global.partner.party) {//.partner[3]
						i.Heal();
						if (i is PokemonEssentials.Interface.PokeBattle.IPokemonMegaEvolution m) {
							m.makeUnmega();		//rescue null;
							m.makeUnprimal(); } //rescue null;
					}
				}
				//if (decision==2 || decision==5) {
				//	Game.GameData.GameSystem.bgm_unpause();
				//	Game.GameData.GameSystem.bgs_unpause();
				//	Kernel.StartOver();
				//}
				//Events.onEndBattle.trigger(null,decision,false);
				IOnEndBattleEventArgs endBattleEventArgs = new PokemonUnity.EventArg.OnEndBattleEventArgs()
				{
					Decision = decision,
					CanLose = false
				};
				Events.OnEndBattleTrigger(this,endBattleEventArgs);
			});
			Input.update();
			if (decision==Combat.BattleResults.WON || decision==Combat.BattleResults.CAPTURED) {		// Defeated or caught
				Game.GameData.Global.roamPokemon[index]=null; //true;
				//Game.GameData.Global.roamPokemonCaught[index]=(decision==Combat.BattleResults.CAPTURED);
			} else {
				Game.GameData.Global.roamPokemon[index]=genwildpoke;
			}
			_ = Game.GameData.Global.roamEncounter;//=null;
			Game.GameData.Global.roamedAlready=true;
			//Events.onWildBattleEnd.trigger(null,species,level,decision);
			PokemonEssentials.Interface.EventArg.IOnWildBattleEndEventArgs endWildArgs = new PokemonUnity.EventArg.OnWildBattleEndEventArgs()
			{
				Species = genwildpoke.Species,
				Level = level,
				Result = decision
			};
			Events.OnWildBattleEndTrigger(null,species,level,decision);
			return (decision!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW);
		}

		//EncounterModifier.register(proc {|encounter|
		//	Game.GameData.Temp.nowRoaming=false;
		//	Game.GameData.Temp.roamerIndex=null;
		//	if (!encounter) return null;
		//	if (Game.GameData.Global.roamedAlready) return encounter;
		//	if (Game.GameData.Global.partner) return encounter;
		//	if (Game.GameData.Temp.pokeradar) return encounter;
		//	if (Core.Rand.Next(4)!=0) return encounter;
		//	roam=[];
		//	for (int i = 0; i < RoamingSpecies.Length; i++) {
		//		poke=RoamingSpecies[i];
		//		Pokemons species=getID(Species,poke[0]);
		//		if (!species || species<=0) continue;
		//		if (Game.GameData.GameSwitches[poke[2]] && Game.GameData.Global.roamPokemon[i]!=true) {
		//			currentArea=Game.GameData.Global.roamPosition[i];
		//			if (!currentArea) {
		//				Game.GameData.Global.roamPosition[i]=keys[Core.Rand.Next(keys.Length)];
		//				currentArea=Game.GameData.Global.roamPosition[i];
		//			}
		//			roamermeta=GetMetadata(currentArea,MetadataMapPosition);
		//			possiblemaps=[];
		//			mapinfos=$RPGVX ? load_data("Data/MapInfos.rvdata") : load_data("Data/MapInfos.rxdata");
		//			for (int j = 1; j < mapinfos.Length; j++) {
		//				jmeta=GetMetadata(j,MetadataMapPosition);
		//				if (mapinfos[j] && mapinfos[j].Name==Game.GameData.GameMap.Name &&
		//					roamermeta && jmeta && roamermeta[0]==jmeta[0]) {
		//					possiblemaps.Add(j);   // Any map with same name as roamer's current map
		//				}
		//			}
		//			if (possiblemaps.Contains(currentArea) && RoamingMethodAllowed(poke[3])) {
		//				//  Change encounter to species and level, with BGM on end
		//				roam.Add([i,species,poke[1],poke[4]]);
		//			}
		//		}
		//	}
		//	if (roam.Length>0) {
		//		rnd=Core.Rand.Next(roam.Length);
		//		roamEncounter=roam[rnd];
		//		Game.GameData.Global.roamEncounter=roamEncounter;
		//		Game.GameData.Temp.nowRoaming=true;
		//		Game.GameData.Temp.roamerIndex=roamEncounter[0];
		//		if (roamEncounter[3] && roamEncounter[3]!="") {
		//			Game.GameData.Global.nextBattleBGM=roamEncounter[3];
		//		}
		//		return [roamEncounter[1],roamEncounter[2]];
		//	}
		//	return encounter;
		//});
		//
		//EncounterModifier.registerEncounterEnd(proc {
		//	Game.GameData.Temp.nowRoaming=false;
		//	Game.GameData.Temp.roamerIndex=null;
		//});

		public bool RoamingMethodAllowed(EncounterTypes enctype) {
			EncounterOptions? encounter=PokemonEncounters.EncounterType();
			switch (enctype) {
				case EncounterTypes.None:   //0 Any encounter method (except triggered ones and Bug Contest)
					if (encounter==EncounterOptions.Land) return true;
					if (encounter==EncounterOptions.LandMorning) return true;
					if (encounter==EncounterOptions.LandDay) return true;
					if (encounter==EncounterOptions.LandNight) return true;
					if (encounter==EncounterOptions.Water) return true;
					if (encounter==EncounterOptions.Cave) return true;
					break;
				case EncounterTypes.Walking:   //1 Grass (except Bug Contest)/walking in caves only
					if (encounter==EncounterOptions.Land) return true;
					if (encounter==EncounterOptions.LandMorning) return true;
					if (encounter==EncounterOptions.LandDay) return true;
					if (encounter==EncounterOptions.LandNight) return true;
					if (encounter==EncounterOptions.Cave) return true;
					break;
				case EncounterTypes.Surfing:   //2 Surfing only
					if (encounter==EncounterOptions.Water) return true;
					break;
				case EncounterTypes.Fishing:   //3 Fishing only
					if (encounter==EncounterOptions.OldRod) return true;
					if (encounter==EncounterOptions.GoodRod) return true;
					if (encounter==EncounterOptions.SuperRod) return true;
					break;
				case EncounterTypes.AnyWater:   //4 Water-based only
					if (encounter==EncounterOptions.Water) return true;
					if (encounter==EncounterOptions.OldRod) return true;
					if (encounter==EncounterOptions.GoodRod) return true;
					if (encounter==EncounterOptions.SuperRod) return true;
					break;
			}
			return false;
		}
	}
}