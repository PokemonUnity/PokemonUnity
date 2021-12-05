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
	public class Scene
	{
		public PokemonEssentials.Interface.Screen.IPokemonEntryScene TextEntryScene { get; set; }
		public PokemonEssentials.Interface.Screen.IIntroEventScene IntroScene { get; set; }
		public PokemonEssentials.Interface.Screen.ILoadScene Load { get; set; }
		public PokemonEssentials.Interface.Screen.ISaveScene Save { get; set; }
		public PokemonEssentials.Interface.Screen.IOptionScene OptionScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonMenuScene PauseMenuScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPartyDisplayScene Party { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonSummaryScene Summary { get; set; }
		public PokemonEssentials.Interface.Screen.IBagScene Bag { get; set; }
		public PokemonEssentials.Interface.Screen.IItemStorageScene Bag_ItemStore { get; set; }
		public PokemonEssentials.Interface.Screen.IWithdrawItemScene Bag_ItemWithdraw { get; set; }
		public PokemonEssentials.Interface.Screen.ITossItemScene Bag_ItemToss { get; set; }
		//public PokemonEssentials.Interface.Screen. RegionMap { get; set; }
		//public PokemonEssentials.Interface.Screen. Phone { get; set; }
		public PokemonEssentials.Interface.Screen.IPokegearScene PokeGear { get; set; }
		public PokemonEssentials.Interface.Screen.ITrainerCardScene TrainerCard { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonStorageScene PokemonStorageScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonPokedexScene PokedexScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonNestMapScene PokedexNestScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonFormScene PokedexFormScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonEvolutionScene EvolvingScene { get; set; }
		//public PokemonEssentials.Interface.Screen. EggHatching { get; set; }
		//public PokemonEssentials.Interface.Screen. Trading { get; set; }
		//public PokemonEssentials.Interface.Screen. MoveRelearner { get; set; }
		public PokemonEssentials.Interface.Screen.IMartScene Mart { get; set; }
		//public PokemonEssentials.Interface.Screen. Debug { get; set; }
		public PokemonEssentials.Interface.Screen.IRelicStoneScene RelicStone { get; set; } //Shadow Pokemon
		//public PokemonEssentials.Interface.Screen. PurityChamber { get; set; } //Shadow Pokemon


		public PokemonEssentials.Interface.Screen.IPokeBattle_Scene BattleScene { get; set; }
		public PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene BattleArenaScene { get; set; }
		public PokemonEssentials.Interface.Screen.IBattleSwapScene BattleSwapScene { get; set; }
		//public PokemonEssentials.Interface.Screen. Battle { get; set; }
		//public PokemonEssentials.Interface.Screen. BattleDebug { get; set; }

		public Scene(params PokemonEssentials.Interface.Screen.IScene[] scenes)
		{
			foreach (PokemonEssentials.Interface.Screen.IScene scene in scenes)
			{
				if (scene is PokemonEssentials.Interface.Screen.IPokemonEntryScene			s0) TextEntryScene		= s0;
				else if (scene is PokemonEssentials.Interface.Screen.IIntroEventScene		s1) IntroScene			= s1;
				else if (scene is PokemonEssentials.Interface.Screen.ILoadScene				s2) Load				= s2;
				else if (scene is PokemonEssentials.Interface.Screen.ISaveScene				s3) Save				= s3;
				else if (scene is PokemonEssentials.Interface.Screen.IOptionScene			s4) OptionScene			= s4;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonMenuScene		s5) PauseMenuScene		= s5;
				else if (scene is PokemonEssentials.Interface.Screen.IPartyDisplayScene		s6) Party				= s6;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonSummaryScene	s7) Summary				= s7;
				else if (scene is PokemonEssentials.Interface.Screen.IBagScene				s8) Bag					= s8;
				else if (scene is PokemonEssentials.Interface.Screen.IItemStorageScene		s9) Bag_ItemStore		= s9;
				else if (scene is PokemonEssentials.Interface.Screen.IWithdrawItemScene		s10) Bag_ItemWithdraw	= s10;
				else if (scene is PokemonEssentials.Interface.Screen.ITossItemScene			s11) Bag_ItemToss		= s11;
				else if (scene is PokemonEssentials.Interface.Screen.IPokegearScene			s12) PokeGear			= s12;
				else if (scene is PokemonEssentials.Interface.Screen.ITrainerCardScene		s13) TrainerCard		= s13;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonStorageScene	s14) PokemonStorageScene= s14;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonPokedexScene	s15) PokedexScene		= s15;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonNestMapScene	s16) PokedexNestScene	= s16;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonFormScene		s17) PokedexFormScene	= s17;
				else if (scene is PokemonEssentials.Interface.Screen.IPokemonEvolutionScene	s18) EvolvingScene		= s18;
				else if (scene is PokemonEssentials.Interface.Screen.IMartScene				s19) Mart				= s19;
				else if (scene is PokemonEssentials.Interface.Screen.IRelicStoneScene		s20) RelicStone			= s20;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattle_Scene		s21) BattleScene		= s21;
				else if (scene is PokemonEssentials.Interface.Screen.IPokeBattleArena_Scene	s22) BattleArenaScene	= s22;
				else if (scene is PokemonEssentials.Interface.Screen.IBattleSwapScene		s23) BattleSwapScene	= s23;
			}
		}
	}
}