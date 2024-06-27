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
using PokemonUnity.Combat;
using PokemonEssentials.Interface.Item;

namespace PokemonUnity
{
	//public partial class TempData : PokemonEssentials.Interface.Field.ITempMetadataRoaming {
	//	public bool nowRoaming				{ get; set; }
	//	public int? roamerIndex				{ get; set; }
	//}

	public partial class GlobalMetadata : PokemonEssentials.Interface.Field.IGlobalMetadataRoaming {
		public int[] roamPosition						{ get; set; }
		public Queue<int> roamHistory					{ get; set; }
		public bool roamedAlready						{ get; set; }
		public IEncounterPokemonRoaming roamEncounter	{ get; set; }
		public IList<IPokemon> roamPokemon				{ get; set; }
		public Pokemons[] roamPokemonCaught				{ get {
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
			if (Global != null && Global is IGlobalMetadataRoaming gmr && gmr.roamPokemon != null) {
				for (int i = 0; i < gmr.roamPokemon.Count; i++) {
					if (gmr.roamPokemon[i].IsNotNullOrNone() && gmr.roamPokemonCaught[i]==Pokemons.NONE) { //gmr.roamPokemonCaught[i]!=true
						gmr.roamPokemon[i]=null;
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
			if (Global is IGlobalMetadataRoaming gmr && gmr.roamPosition != null) {
				int[] keys=RoamingAreas(index).Keys.ToArray();
				gmr.roamPosition[index]=keys[Core.Rand.Next(keys.Length)];
			}
		}

		/// <summary>
		/// Roams all roamers, if their Switch is on.
		/// </summary>
		/// <param name="ignoretrail"></param>
		public void RoamPokemon(bool ignoretrail=false) {
			if (Global is IGlobalMetadataRoaming gmr) {
				//Start all roamers off in random maps
				if (gmr.roamPosition == null) {
					gmr.roamPosition=new int[Kernal.RoamingSpecies.Length];
					for (int i = 0; i < Kernal.RoamingSpecies.Length; i++) {
						Pokemons? species=Kernal.RoamingSpecies[i].Pokemon;//[0]
						if (species==null || species<=0) continue;
						int[] keys=RoamingAreas(i).Keys.ToArray();
						gmr.roamPosition[i]=keys[Core.Rand.Next(keys.Length)];
					}
				}
				if (gmr.roamHistory == null) gmr.roamHistory=new Queue<int>();
				if (gmr.roamPokemon == null) gmr.roamPokemon=new List<IPokemon>();
				//Roam each Pokémon in turn
				for (int i = 0; i < Kernal.RoamingSpecies.Length; i++) {
					RoamingEncounterData poke=Kernal.RoamingSpecies[i];
					if (GameSwitches[poke.SwitchId]) {//[2]
						//Pokemons species=getID(Species,poke[0]);
						Pokemons species=poke.Pokemon;
						if (species<=0) continue; //next; //species == null ||
						List<int> choices=new List<int>();
						int[] keys=RoamingAreas(i).Keys.ToArray();
						int? currentArea=gmr.roamPosition[i];
						if (currentArea==null) {
							gmr.roamPosition[i]=keys[Core.Rand.Next(keys.Length)];
							currentArea=gmr.roamPosition[i];
						}
						int[] newAreas=RoamingAreas(i)[currentArea.Value];
						if (newAreas == null) continue; //next;
						foreach (int area in newAreas) {
							bool inhistory=gmr.roamHistory.Contains(area);
							if (ignoretrail) inhistory=false;
							if (!inhistory) choices.Add(area);
						}
						if (Core.Rand.Next(32)==0 && keys.Length>0) {
							int area=keys[Core.Rand.Next(keys.Length)];
							bool inhistory=gmr.roamHistory.Contains(area);
							if (ignoretrail) inhistory=false;
							if (!inhistory) choices.Add(area);
						}
						if (choices.Count>0 ) {
							int area=choices[Core.Rand.Next(choices.Count)];
							gmr.roamPosition[i]=area;
						}
					}
				}
			}
		}

		//Events.OnMapChange+=proc {|sender,e|
		protected virtual void Events_OnMapChangeRoaming (object sender, EventArg.OnMapChangeEventArgs e) {
			if (Global == null) return;
			IDictionary<int,IMapInfo> mapinfos = null;//$RPGVX ? load_data("Data/MapInfos.rvdata") :load_data("Data/MapInfos.rxdata");
			Kernal.load_data(mapinfos,"Data/MapInfos.rxdata");
			//if (GameMap != null && mapinfos != null && e[0]>0 && mapinfos[e[0]] &&
			//	mapinfos[e[0]].Name && GameMap.Name==mapinfos[e[0]].Name) return;
			if (GameMap != null && mapinfos != null && e.MapId>0 && mapinfos.ContainsKey(e.MapId) &&
				!string.IsNullOrEmpty(mapinfos[e.MapId].name.Trim()) && GameMap.name==mapinfos[e.MapId].name) return;
			RoamPokemon();
			if (Global is IGlobalMetadataRoaming gmr) {
				if (gmr.roamHistory.Count>=2) {
					gmr.roamHistory.Dequeue(); //.shift();
				}
				gmr.roamedAlready=false;
				if (GameMap is IGameMapOrgBattle gmo)
					gmr.roamHistory.Enqueue(gmo.map_id);
			}
		}

		//Events.OnWildBattleOverride+=proc { |sender,e|
		protected virtual void Events_OnWildBattleOverride (object sender, EventArg.OnWildBattleOverrideEventArgs e) {
			Pokemons species=e.Species;		//[0]
			int level=e.Level;				//[1]
			BattleResults? handled=e.Result;//[2]
			if (handled!=null) return;		//[0]
			if (GameTemp is ITempMetadataRoaming tmr0 && tmr0.nowRoaming == null) return;
			if (GameTemp is ITempMetadataRoaming tmr1 && tmr1.roamerIndex==null) return;
			if (Global is IGlobalMetadataRoaming gmr && gmr.roamEncounter == null) return;
			handled=RoamingPokemonBattle(species,level);//[0]
		}

		//public bool RoamingPokemonBattle(Pokemons species,int level) {
		public BattleResults RoamingPokemonBattle(Pokemons species,int level) {
			IPokemon genwildpoke; int index=0;//PokemonTemp.roamerIndex;
			if (PokemonTemp is ITempMetadataRoaming tmr) index=tmr.roamerIndex??0;
			if (Global is IGlobalMetadataRoaming gmr && gmr.roamPokemon.Count>0 && gmr.roamPokemon[index].IsNotNullOrNone()) {
				//&& Global.roamPokemon[index] is PokeBattle_Pokemon) {
				genwildpoke=gmr.roamPokemon[index];
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
			//battle=new PokeBattle_Battle(scene,Player.Party,[genwildpoke],Player,null);
			IBattle battle=new Combat.Battle(scene,Trainer.party,new IPokemon[] { genwildpoke },Trainer,null);
			battle.internalbattle=true;
			battle.cantescape=false;
			battle.rules["alwaysflee"]=true;
			PrepareBattle(battle);
			Combat.BattleResults decision=0;
			BattleAnimation(GetWildBattleBGM(species), block: () => {
				SceneStandby(() => {
					decision=battle.StartBattle();
				});
				foreach (IPokemon i in Trainer.party) { if (i is PokemonEssentials.Interface.PokeBattle.IPokemonMegaEvolution m) { m.makeUnmega(); m.makeUnprimal(); } } //rescue null);
				if (Global.partner != null) {
					HealAll();
					foreach (IPokemon i in Global.partner.party) {//.partner[3]
						i.Heal();
						if (i is PokemonEssentials.Interface.PokeBattle.IPokemonMegaEvolution m) {
							m.makeUnmega();		//rescue null;
							m.makeUnprimal(); } //rescue null;
					}
				}
				//if (decision==2 || decision==5) {
				//	GameSystem.bgm_unpause();
				//	GameSystem.bgs_unpause();
				//	Kernel.StartOver();
				//}
				IOnEndBattleEventArgs endBattleEventArgs = new PokemonUnity.EventArg.OnEndBattleEventArgs()
				{
					Decision = decision,
					CanLose = false
				};
				//Events.onEndBattle.trigger(null,decision,false);
				Events.OnEndBattleTrigger(this,endBattleEventArgs);
			});
			Input.update();
			if (Global is IGlobalMetadataRoaming gmr1) {
				if (decision==Combat.BattleResults.WON || decision==Combat.BattleResults.CAPTURED) {		// Defeated or caught
					gmr1.roamPokemon[index]=null; //true;
					//Global.roamPokemonCaught[index]=(decision==Combat.BattleResults.CAPTURED);
				} else {
					gmr1.roamPokemon[index]=genwildpoke;
				}
				gmr1.roamEncounter=null;
				gmr1.roamedAlready=true;
			}
			PokemonEssentials.Interface.EventArg.IOnWildBattleEndEventArgs endWildArgs = new PokemonUnity.EventArg.OnWildBattleEndEventArgs()
			{
				Species = genwildpoke.Species,
				Level = level,
				Result = decision
			};
			//Events.onWildBattleEnd.trigger(null,species,level,decision);
			Events.OnWildBattleEndTrigger(this,species,level,decision);
			return decision; //(decision!=Combat.BattleResults.LOST && decision!=Combat.BattleResults.DRAW);
		}

		//EncounterModifier.register(proc {|encounter|
		protected virtual void Events_EncounterModifiers(object sender, EventArg.OnEncounterCreateEventArgs encounter) {
			if (GameTemp is ITempMetadataRoaming tmr) {
				tmr.nowRoaming=false;
				tmr.roamerIndex=null;
				if (encounter==null) return; //null;
				if (Global is IGlobalMetadataRoaming gmr) {
					if (gmr.roamedAlready) return; // encounter;
					if (Global is IGlobalMetadata gm && gm.partner != null) return; // encounter;
					if (GameTemp is ITempMetadataPokeRadar tmpr && tmpr.pokeradar!=null) return; // encounter;
					if (Core.Rand.Next(4)!=0) return; // encounter;
					IList<IEncounterPokemonRoaming> roam=new List<IEncounterPokemonRoaming>();
					for (int i = 0; i < Kernal.RoamingSpecies.Length; i++) {
						RoamingEncounterData poke=Kernal.RoamingSpecies[i];
						Pokemons species=poke.Pokemon;//getID(Species,poke[0]);
						if (species<=0) continue; //species==null ||
						//if (GameSwitches[poke[2]] && gmr.roamPokemon[i]!=true) {
						if (GameSwitches[poke.SwitchId] && gmr.roamPokemon[i].IsNotNullOrNone()) { //[2] | switchId
							int? currentArea=gmr.roamPosition[i];
							if (currentArea==null) {
								//Global.roamPosition[i]=keys[Core.Rand.Next(keys.Length)];
								gmr.roamPosition[i]=Kernal.RoamingAreas[Core.Rand.Next(Kernal.RoamingAreas.Length)].Key;
								currentArea=gmr.roamPosition[i];
							}
							//roamermeta=GetMetadata(currentArea,MetadataMapPosition);
							ITilePosition roamermeta=GetMetadata(currentArea.Value).Map.MapPosition;
							IList<int> possiblemaps=new List<int>();
							IDictionary<int,IMapInfo> mapinfos=new Dictionary<int,IMapInfo>();//$RPGVX ? load_data("Data/MapInfos.rvdata") :load_data("Data/MapInfos.rxdata");
							Kernal.load_data(mapinfos,"Data/MapInfos.rxdata");
							for (int j = 1; j < mapinfos.Count; j++) {
								//jmeta=GetMetadata(j,MetadataMapPosition);
								ITilePosition jmeta=GetMetadata(j).Map.MapPosition;
								if (mapinfos[j]!=null && mapinfos[j].name==GameMap.name &&
									roamermeta!=null && jmeta!=null && roamermeta.MapId==jmeta.MapId) { //roamermeta[0]==jmeta[0]
									possiblemaps.Add(j);   // Any map with same name as roamer's current map
								}
							}
							if (possiblemaps.Contains(currentArea.Value) && RoamingMethodAllowed(poke.EncounterType)) { //[3]
								//  Change encounter to species and level, with BGM on end
								//roam.Add([i,species,poke[1],poke[4]]); | [i,species,level,IAudioBGM]
								//roam.Add([i,species,poke[1],poke[4]]);
							}
						}
					}
					if (roam.Count>0) {
						int rnd=Core.Rand.Next(roam.Count);
						IEncounterPokemonRoaming roamEncounter=roam[rnd];
						gmr.roamEncounter=roamEncounter;
						tmr.nowRoaming=true;
						tmr.roamerIndex=roamEncounter.roamerIndex;//[0]
						if (roamEncounter.battleBGM!=null && roamEncounter.battleBGM.name!="") {//[3]
							Global.nextBattleBGM=roamEncounter.battleBGM;//[3]
						}
						//return [roamEncounter[1],roamEncounter[2]];
						encounter.Pokemon=new PokemonUnity.Monster.Pokemon(pkmn:roamEncounter.Pokemon,level:(byte)roamEncounter.MaxLevel);
						return; //[roamEncounter[1],roamEncounter[2]];
					}
				}
			}
			return; // encounter;
		}

		//EncounterModifier.registerEncounterEnd(proc {
		protected virtual void Events_EncounterEnd(object sender, EventArgs e) {
			if (GameTemp is ITempMetadataRoaming tmr) {
				tmr.nowRoaming=false;
				tmr.roamerIndex=null;
			}
		}

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