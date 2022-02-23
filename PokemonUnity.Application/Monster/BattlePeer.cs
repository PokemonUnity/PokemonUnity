using System;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Character;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Monster
{
	public partial class PokeBattle_NullBattlePeer : PokemonEssentials.Interface.PokeBattle.IBattlePeer, PokemonEssentials.Interface.PokeBattle.IBattlePeerMultipleForms
	{ 
		public void pbOnEnteringBattle(PokemonEssentials.Interface.PokeBattle.IBattle battle, PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
		}

		public int pbStorePokemon(PokemonEssentials.Interface.PokeBattle.ITrainer player, PokemonEssentials.Interface.PokeBattle.IPokemon pokemon) {
			if (player.party.GetCount()<6) {
				player.party[player.party.GetCount()]=pokemon;
			}
			return -1;
		}

		public string pbGetStorageCreator() {
			return null;
		}

		public int pbCurrentBox() {
			return -1;
		}

		public string pbBoxName(int box) {
			return "";
		}
	}
	   
	public partial class PokeBattle_RealBattlePeer : PokemonEssentials.Interface.PokeBattle.IBattlePeer {
		public int pbStorePokemon(ITrainer player, IPokemon pokemon) {
			if (player.party.GetCount()<6) {
				player.party[player.party.GetCount()]=pokemon;
				return -1;
			} else {
				(pokemon as IPokemon).Heal();
				int oldcurbox=Game.GameData.PokemonStorage.currentBox;
				int storedbox=Game.GameData.PokemonStorage.pbStoreCaught(pokemon);
				//int oldcurbox=Game.GameData.Player.PC.ActiveBox;
				//int? storedbox = Game.GameData.Player.PC.getIndexOfFirstEmpty();
				if (storedbox<0) {
				//if (!storedbox.HasValue) {
					//Game.GameData.pbDisplayPaused(Game._INTL("Can't catch any more...")); //ToDo: Relationship to ui from here?
					return oldcurbox;
				} else {
					return storedbox;
				}
			}
		}

		public string pbGetStorageCreator() {
			string creator=null;
			if (Game.GameData != null && Game.GameData.Global.seenStorageCreator) {
			//if (Game.GameData != null && Game.GameData.Player.IsCreator) {
				creator=(Game.GameData as PokemonEssentials.Interface.Screen.IGamePCStorage).pbGetStorageCreator();
				//creator="someone"; //ToDo...
			}
			return creator;
		}

		public int pbCurrentBox() {
			return Game.GameData.PokemonStorage.currentBox;
			//return Game.GameData.Player.PC.ActiveBox;
		}

		public string pbBoxName(int box) {
			return box<0 ? "" : Game.GameData.PokemonStorage[box].name;
			//return box<0 ? "" : Game.GameData.Player.PC.BoxNames[box];
		}
	}
	
	public partial class PokeBattle_BattlePeer {
		public static IBattlePeer create() {
			return (IBattlePeer)new PokeBattle_RealBattlePeer();
		}
	}
}