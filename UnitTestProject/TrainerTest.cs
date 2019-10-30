using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokemonUnity;


namespace Tests
{
    [TestClass]
    public class TrainerTest
    {
        #region TrainerProperties
        //public void Trainer_() { 
        //	/*SaveDataOld.currentSave.playerName = name;
        //	SaveDataOld.currentSave.playerID = 29482; //not implemented
        //	SaveDataOld.currentSave.isMale = isMale;
        //	SaveDataOld.currentSave.playerMoney = 2481; 
        //	SaveDataOld.currentSave.playerLanguage = Language.English;
        //
        //	SaveDataOld.currentSave.playerOutfit = "hgss";
        //
        //	SaveDataOld.currentSave.playerShirt = "Ethan's Shirt";
        //	SaveDataOld.currentSave.playerMisc = null;
        //	SaveDataOld.currentSave.playerHat = "Ethan's Hat";
        //	//customizables not implemented
        //
        //	if(isMale == true){
        //		SaveDataOld.currentSave.setCVariable("male",1); //custom events can check if the player is male or female, 1 meaning male, 0 meaning female
        //	} else {
        //		SaveDataOld.currentSave.setCVariable("male",0);
        //	}*/
        //}
        #endregion

        #region TrainerPokemon
        [TestMethod]
        public void Trainer_Party_AddPokemon()
		{
			Assert.Inconclusive();
			string playerName = "Red";
			int trainerID = 55323;
			int secretID = 64123;
			bool isMale = false;
			Player player = new Player(playerName, isMale/*, playerParty*/);
			Trainer trainer = new Trainer(player, tID: trainerID, sID: secretID);
			player = new Player(trainer);
			//Trainer trainer = new Trainer(TrainerTypes.PLAYER);
			player.addPokemon(new PokemonUnity.Monster.Pokemon(Pokemons.CHARMANDER, trainer));

			/*SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(006, null, PokemonOld.Gender.CALCULATE, 3, true, "Poké Ball", "",
				name,
				Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
				Random.Range(0, 32),
				0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabaseOld.getPokemon(6).GenerateMoveset(42), new int[4]));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(197, PokemonOld.Gender.CALCULATE, 34, "Great Ball", "", name, 0));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(393, PokemonOld.Gender.CALCULATE, 6, "Poké Ball", "", name, 0));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(197, PokemonOld.Gender.CALCULATE, 28, "Great Ball", "", name, -1));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(68, PokemonOld.Gender.CALCULATE, 37, "Ultra Ball", "", name, -1));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(448, PokemonOld.Gender.CALCULATE, 56, "Great Ball", "", name, 0));

			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(006, PokemonOld.Gender.CALCULATE, 37, "Poké Ball", "", name, 0));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(607, PokemonOld.Gender.CALCULATE, 48, "Poké Ball", "", "Bob", 0));
			SaveDataOld.currentSave.PC.boxes[1][1].addExp(7100);
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(157, PokemonOld.Gender.CALCULATE, 51, "Poké Ball", "", name, 0));
			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(300, PokemonOld.Gender.CALCULATE, 51, "Poké Ball", "", name, 0));

			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(393, "Surf Bloke", PokemonOld.Gender.MALE, 15, false, "Ultra Ball", //starters not implemented
				"", name,
				31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
				new string[] {"Drill Peck", "Surf", "Growl", "Dragon Rage"}, new int[] {0, 0, 0, 3}));*/
			CollectionAssert.AreNotEqual(new Pokemons[] { Pokemons.CHARMANDER, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE, Pokemons.NONE }, new Pokemons[] { trainer.Party[0].Species, trainer.Party[1].Species, trainer.Party[2].Species, trainer.Party[3].Species, trainer.Party[4].Species, trainer.Party[5].Species } );
		}

        [TestMethod]
        public void Trainer_Party_AlterPokemon_InsideBox() {
			/*SaveDataOld.currentSave.PC.boxes[0][1].setNickname("Greg");

			SaveDataOld.currentSave.PC.boxes[0][1].setStatus(PokemonOld.Status.POISONED);
			SaveDataOld.currentSave.PC.boxes[0][1].addExp(420);

			SaveDataOld.currentSave.PC.boxes[0][0].swapHeldItem("Ultra Ball");

			SaveDataOld.currentSave.PC.boxes[0][1].removeHP(56);
			SaveDataOld.currentSave.PC.boxes[0][4].removeHP(64);

			SaveDataOld.currentSave.PC.boxes[0][4].removePP(0, 5);
			SaveDataOld.currentSave.PC.boxes[0][4].removePP(1, 5);
			SaveDataOld.currentSave.PC.boxes[0][3].removePP(0, 6);
			SaveDataOld.currentSave.PC.boxes[0][0].removePP(2, 11);

			//PC.boxes[0][0].setStatus(Pokemon.Status.FROZEN);
			SaveDataOld.currentSave.PC.boxes[0][2].setStatus(PokemonOld.Status.PARALYZED);
			SaveDataOld.currentSave.PC.boxes[0][3].setStatus(PokemonOld.Status.BURNED);
			SaveDataOld.currentSave.PC.boxes[0][4].setStatus(PokemonOld.Status.ASLEEP);*/
			Assert.Inconclusive();
		}

        //[TestMethod]
        //public void Trainer_Trade_Pokemons()
        //{
        //	//Swap pokemons between owners, and confirm original pokemon owners are different
        //	Assert.Inconclusive();
        //}

        [TestMethod]
        public void Trainer_Party_SwapPokemon() {
			/*SaveDataOld.currentSave.PC.swapPokemon(0, 5, 1, 5);
			SaveDataOld.currentSave.PC.swapPokemon(0, 3, 1, 11);
			SaveDataOld.currentSave.PC.swapPokemon(1, 1, 1, 12);
			SaveDataOld.currentSave.PC.swapPokemon(1, 2, 1, 21);
			SaveDataOld.currentSave.PC.swapPokemon(0, 5, 1, 3);

			SaveDataOld.currentSave.PC.swapPokemon(0, 2, 1, 4);

			SaveDataOld.currentSave.PC.packParty();

			SaveDataOld.currentSave.PC.swapPokemon(0, 0, 0, 2);


			SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(012, null, PokemonOld.Gender.CALCULATE, 35, false, "Great Ball", "",
				name,
				31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
				new string[] {"Ominous Wind", "Sunny Day", "Gust", "Sleep Powder"}, new int[] {0, 0, 0, 0}));

			//SaveData.currentSave.PC.swapPokemon(0,1,3,1);
			SaveDataOld.currentSave.PC.swapPokemon(0, 2, 3, 2);
			SaveDataOld.currentSave.PC.swapPokemon(0, 3, 3, 3);
			SaveDataOld.currentSave.PC.swapPokemon(0, 4, 3, 4);
			SaveDataOld.currentSave.PC.swapPokemon(0, 5, 3, 5);


			SaveDataOld.currentSave.PC.packParty();*/
			Assert.Inconclusive();
		}
        #endregion

        #region TrainerItemsBags&&PC
        [TestMethod]
        public void Trainer_RideBike_Fail_If_Item_NotInInventory()
        {
			//if(
			//	Game.Player.Bag[PokemonUnity.Inventory.Items.BICYCLE] != null ||
			//	Game.Player.Bag[PokemonUnity.Inventory.Items.ACRO_BIKE] != null ||
			//	Game.Player.Bag[PokemonUnity.Inventory.Items.MACH_BIKE] != null
			//)
            Assert.Inconclusive();
        }

        [TestMethod]
        public void Trainer_Bag_AddItems() {
			/*SaveDataOld.currentSave.Bag.addItem("Poké Ball", 9);
			SaveDataOld.currentSave.Bag.addItem("Miracle Seed", 1);
			SaveDataOld.currentSave.Bag.addItem("Poké Ball", 3);
			SaveDataOld.currentSave.Bag.addItem("Charcoal", 1);
			SaveDataOld.currentSave.Bag.addItem("Potion", 4);
			SaveDataOld.currentSave.Bag.addItem("Poké Doll", 13);
			SaveDataOld.currentSave.Bag.addItem("Escape Rope", 4);
			SaveDataOld.currentSave.Bag.addItem("Fire Stone", 2);
			SaveDataOld.currentSave.Bag.removeItem("Poké Doll", 10);
			SaveDataOld.currentSave.Bag.addItem("Stardust", 1);
			SaveDataOld.currentSave.Bag.addItem("Water Stone", 1);
			SaveDataOld.currentSave.Bag.addItem("Moon Stone", 1);
			SaveDataOld.currentSave.Bag.addItem("Super Potion", 2);
			SaveDataOld.currentSave.Bag.addItem("Great Ball", 4);
			SaveDataOld.currentSave.Bag.addItem("Psyshock", 1);
			SaveDataOld.currentSave.Bag.addItem("Bulk Up", 1);
			SaveDataOld.currentSave.Bag.addItem("Elixir", 2);
			SaveDataOld.currentSave.Bag.addItem("Ether", 1);
			SaveDataOld.currentSave.Bag.addItem("Antidote", 1);
			SaveDataOld.currentSave.Bag.addItem("Full Heal", 1);
			SaveDataOld.currentSave.Bag.addItem("Rare Candy", 100);
			SaveDataOld.currentSave.Bag.addItem("Paralyze Heal", 1);
			SaveDataOld.currentSave.Bag.addItem("Awakening", 1);
			SaveDataOld.currentSave.Bag.addItem("Burn Heal", 1);
			SaveDataOld.currentSave.Bag.addItem("Ice Heal", 1);
			SaveDataOld.currentSave.Bag.addItem("Max Potion", 1);
			SaveDataOld.currentSave.Bag.addItem("Hyper Potion", 1);*/
			Assert.Inconclusive();
		}
        
        //[TestMethod]
        //public void Trainer_PC_RenameBox() {
		//	//debug code to test custom box names/textures
		//	/*PC.boxName[1] = "Grassy Box";
		//	PC.boxTexture[2] = 12;*/
		//	Assert.Inconclusive();
		//}
        #endregion

        #region TrainerSave
        //[TestMethod] //Not needed anymore, being saved from Player
        //public void Trainer_Save() {
		//	////////////////////////////////////////////////////////////////////////////////////////////////////
		//	/* None of this is needed...
		//	//set file creation date
		//	string day = "";
		//	string month = "";
		//	if(System.DateTime.Now.Day == 1){day = "1st, ";}
		//	else if(System.DateTime.Now.Day == 2){day = "2nd, ";}
		//	else if(System.DateTime.Now.Day == 3){day = "3rd, ";}
		//	else if(System.DateTime.Now.Day == 21){day = "21st, ";}
		//	else if(System.DateTime.Now.Day == 22){day = "22nd, ";}
		//	else if(System.DateTime.Now.Day == 23){day = "23rd, ";}
		//	else if(System.DateTime.Now.Day == 31){day = "31st, ";}
		//	else{day = System.DateTime.Now.Day.ToString("D") + "th, ";}
		//
		//	if(System.DateTime.Now.Month == 1){month = "Jan. ";}
		//	else if(System.DateTime.Now.Month == 2){month = "Feb. ";}
		//	else if(System.DateTime.Now.Month == 3){month = "Mar. ";}
		//	else if(System.DateTime.Now.Month == 4){month = "Apr. ";}
		//	else if(System.DateTime.Now.Month == 5){month = "May ";}
		//	else if(System.DateTime.Now.Month == 6){month = "June ";}
		//	else if(System.DateTime.Now.Month == 7){month = "July ";}
		//	else if(System.DateTime.Now.Month == 8){month = "Aug. ";}
		//	else if(System.DateTime.Now.Month == 9){month = "Sep. ";}
		//	else if(System.DateTime.Now.Month == 10){month = "Oct. ";}
		//	else if(System.DateTime.Now.Month == 11){month = "Nov. ";}
		//	else if(System.DateTime.Now.Month == 12){month = "Dec. ";} //probably the worst way to do this but I have no idea why ToString("MMM") doesn't work
		//
		//	string date = month + day + System.DateTime.Now.Year;
		//
		//	//SaveData.currentSave.fileCreationDate = "Aug. 2nd, 2017";
		//	SaveData.currentSave.fileCreationDate = date;*/
		//
		//	////////////////////////////////////////////////////////////////////////////////////////////////////
		//	/*
		//	//debug code to test trainer card/save
		//	SaveDataOld.currentSave.fileCreationDate = new System.DateTime(System.DateTime.Now.Year, 2, 14); //"Feb. 14th, 2015";
		//	SaveDataOld.currentSave.playerMoney = 2481;
		//	SaveDataOld.currentSave.playerScore = SaveDataOld.currentSave.pokedexCaught + "/" + SaveDataOld.currentSave.pokedexSeen;// PokemonDatabase.LoadPokedex().Length;//481;
		//	//SaveData.currentSave.pokeDex = 0;
        //
		//	SaveDataOld.currentSave.playerHours = 0;
		//	SaveDataOld.currentSave.playerMinutes = 7;
		//	SaveDataOld.currentSave.playerSeconds = 12;
		//	SaveDataOld.currentSave.PlayTime = new System.TimeSpan(0,7,12);
		//
		//	////////////////////////////////////////////////////////////////////////////////////////////////////
		//
		//	//debug code to test badge box
		//	SaveDataOld.currentSave.gymsEncountered = new bool[]
		//	{
		//		true, true, false, true, true, true,
		//		false, false, false, false, false, false
		//	};
		//	SaveDataOld.currentSave.gymsBeaten = new bool[]
		//	{
		//		true, true, false, false, false, true,
		//		false, false, false, false, false, false
		//	};*/
		//	//SaveDataOld.currentSave.gymsBeatTime = new System.DateTime?[]
		//	//{
		//	//	new System.DateTime(System.DateTime.Now.Year, 4, 27) /*"Apr. 27th, 2015"*/, new System.DateTime(System.DateTime.Now.Year, 4, 30) /*"Apr. 30th, 2015"*/, null, null, null, new System.DateTime(System.DateTime.Now.Year, 5,1) /*"May. 1st, 2015"*/,
		//	//	null, null, null, null, null, null
		//	//};
		//	Assert.Inconclusive();
		//}
        #endregion        
    }
}