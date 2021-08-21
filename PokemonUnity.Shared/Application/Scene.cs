using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Monster;

namespace PokemonUnity.UX
{
	/// <summary>
	/// List of all the scenes that utilized by frontend platform
	/// </summary>
	public static class Scene
	{
		public static IPokemonEntryScene TextEntryScene { get; set; }
		public static IIntroEventScene IntroScene { get; set; }
		//public static IIntroEventScene Load { get; set; }
		//public static IIntroEventScene Save { get; set; }
		public static IPokemonOptionScene OptionScene { get; set; }
		public static IPokemonMenu_Scene PauseMenuScene { get; set; }
		//public static  Party { get; set; }
		//public static  Summary { get; set; }
		//public static  Bag { get; set; }
		//public static  Bag_ItemStore { get; set; }
		//public static  Bag_ItemWithdraw { get; set; }
		//public static  Bag_ItemToss { get; set; }
		//public static  RegionMap { get; set; }
		//public static  Phone { get; set; }
		//public static  TrainerCard { get; set; }
		public static IPokemonStorageScene StorageScene { get; set; }
		public static IPokemonStorageScreen StorageScreen { get; set; }
		public static IPokemonPokedexScene PokedexScene { get; set; }
		//public static  PokedexNestForm { get; set; }
		//public static  EggHatching { get; set; }
		//public static  Trading { get; set; }
		//public static  MoveRelearner { get; set; }
		//public static  Mart { get; set; }
		//public static  Debug { get; set; }
		//public static  RelicStone { get; set; } //Shadow Pokemon
		//public static  PurityChamber { get; set; } //Shadow Pokemon


		public static IPokeBattle_Scene BattleScene { get; set; }
		public static IPokeBattleArena_Scene BattleArenaScene { get; set; }
		//public static  Battle { get; set; }
		//public static  BattleDebug { get; set; }

		public static IFrontEnd UI { get; set; }
	}
}