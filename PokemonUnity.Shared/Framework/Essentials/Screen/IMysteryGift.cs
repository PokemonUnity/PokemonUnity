using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	public interface IMysteryGiftData {
		int id				{ get; }
		int type			{ get; }
		Items item			{ get; }
		string giftname		{ get; }
	}

	public interface ITrainerMysteryGift {
		bool mysterygiftaccess				{ get; set; }		// Whether MG can be used from load screen
		IMysteryGiftData mysterygift		{ get; set; }		// Variable that stores downloaded MG data

		//bool mysterygiftaccess() {
		//	if (!@mysterygiftaccess) @mysterygiftaccess=false;
		//	return @mysterygiftaccess;
		//}

		//byte[] mysterygift() {
		//	if (!@mysterygift) @mysterygift=[];
		//	return @mysterygift;
		//}
	}

	public interface IGameMysteryGift
	{
		// ###############################################################################
		// Mystery Gift system
		// By Maruno
		// ###############################################################################
		// This url is the location of an example Mystery Gift file.
		// You should change it to your file's url once you upload it.
		// ###############################################################################
		string MYSTERYGIFTURL { get; } //= "http://images1.wikia.nocookie.net/pokemonessentials/images/e/e7/MysteryGift.txt"


		// ###############################################################################
		// Creating a new Mystery Gift for the Master file, and editing an existing one.
		// ###############################################################################
		// type: 0=Pokémon; 1 or higher=item (is the item's quantity).
		// item: The thing being turned into a Mystery Gift (Pokémon object or item ID).
		IMysteryGiftData pbEditMysteryGift(int type, Items item, int id = 0, string giftname = "");

		void pbCreateMysteryGift(int type, Items item);



		// ###############################################################################
		// Debug option for managing gifts in the Master file and exporting them to a
		// file to be uploaded.
		// ###############################################################################
		void pbManageMysteryGifts();

		void pbRefreshMGCommands(IMysteryGiftData[] master, int[] online);



		// ###############################################################################
		// Downloads all available Mystery Gifts that haven't been downloaded yet.
		// ###############################################################################
		// Called from the Continue/New Game screen.
		ITrainer pbDownloadMysteryGift(ITrainer trainer);



		// ###############################################################################
		// Converts an array of gifts into a string and back.
		// ###############################################################################
		string pbMysteryGiftEncrypt(IMysteryGiftData gift);

		IMysteryGiftData pbMysteryGiftDecrypt(string gift);



		// ###############################################################################
		// Collecting a Mystery Gift from the deliveryman.
		// ###############################################################################
		int pbNextMysteryGiftID();

		bool pbReceiveMysteryGift(int id);
	}
}