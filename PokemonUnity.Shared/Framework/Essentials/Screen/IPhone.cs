using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Interface;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.RPGMaker;
using PokemonEssentials.Interface.RPGMaker.Kernal;

namespace PokemonEssentials.Interface.Screen
{
	/// <summary>
	/// Extension of <see cref="IGame"/>
	/// </summary>
	public interface IGamePhone : IGame
	{
		int pbRandomPhoneTrainer();

		int pbFindPhoneTrainer(TrainerTypes trtype,string trname);

		bool pbHasPhoneTrainer(TrainerTypes trtype, string trname);

		void pbPhoneRegisterNPC(int ident,string name,int mapid,bool showmessage=true);

		void pbPhoneRegister(IGameEvent @event,TrainerTypes trainertype,string trainername);

		void pbPhoneDeleteContact(int index);

		void pbPhoneRegisterBattle(string message,IGameEvent @event,TrainerTypes trainertype,string trainername,int maxbattles);

		bool pbPhoneReadyToBattle(TrainerTypes trtype, string trname);

		void pbPhoneBattleCount(TrainerTypes trtype, string trname);

		void pbPhoneIncrement(TrainerTypes trtype, string trname, int maxbattles);

		void pbPhoneReset(TrainerTypes trtype, string trname);

		void pbCallTrainer(TrainerTypes trtype, string trname);

		void pbSetReadyToBattle(IPhoneTrainerContact num);

		//Events.onMapUpdate+=delegate(object sender, EventArgs e) {
		//	if (!Game.GameData.Trainer || !Game.GameData.Global || !Game.GameData.GamePlayer ||
		//		!Game.GameData.GameMap || !Game.GameData.Trainer.pokegear) {
		//		//do nothing
		//		continue;
		//	} else if (!Game.GameData.Global.phoneTime || Game.GameData.Global.phoneTime<=0) {
		//		Game.GameData.Global.phoneTime=20*60*Graphics.frame_rate;
		//		Game.GameData.Global.phoneTime+=Core.Rand.Next(20*60*Graphics.frame_rate);
		//	}
		//	if (!Game.GameData.Global.phoneNumbers) {
		//		Game.GameData.Global.phoneNumbers=[];
		//	}
		//	if (!Game.GameData.GamePlayer.move_route_forcing && !pbMapInterpreterRunning? &&
		//		!Game.GameData.GameTemp.message_window_showing) {
		//		Game.GameData.Global.phoneTime-=1;
		//		if (Game.GameData.Global.phoneTime%10==0) {
		//			foreach (var num in Game.GameData.Global.phoneNumbers) {
		//				if (num[0] && num.Length==8) {				// if visible and a trainer
		//					if (num[4]==0) {						// needs resetting
		//						num[3]=2000+Core.Rand.Next(2000);	// set time to can-battle
		//						num[4]=1;
		//					}
		//					num[3]-=1;
		//					if (num[3]<=0 && num[4]==1) {
		//						num[4]=2; // set ready-to-battle flag
		//						pbSetReadyToBattle(num);
		//					}
		//				}
		//			}
		//		}
		//		if (Game.GameData.Global.phoneTime<=0) {
		//			//find all trainer phone numbers
		//			phonenum=pbRandomPhoneTrainer;
		//			if (phonenum) {
		//				call=pbPhoneGenerateCall(phonenum);
		//				pbPhoneCall(call,phonenum);
		//			}
		//		}
		//	}
		//}

		void pbRandomPhoneItem(IList<string> array);

		void pbPhoneGenerateCall(IPhoneContact phonenum);

		Pokemons pbRandomEncounterSpecies(EncounterOptions enctype);

		string pbEncounterSpecies(IPhoneContact phonenum);

		void pbLoadTrainerData(TrainerTypes trainerid,string trainername,int partyid=0);

		void pbTrainerMapName(IPhoneContact phonenum);

		void pbTrainerSpecies(IPhoneContact phonenum);



		void pbPhoneCall(string call,IPhoneContact phonenum);
	}

	public interface IWindow_PhoneList : IWindow_CommandPokemon {
		//void drawCursor(int index,IRect rect);

		//void drawItem(int index, int count,IRect rect);
	}

	public interface IPokemonPhoneScene : IScene
	{
		void start();
	}
}