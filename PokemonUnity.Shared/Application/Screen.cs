namespace PokemonUnity.UX
{
	/// <summary>
	/// List of all the screens that utilized by frontend platform
	/// </summary>
	public class Screen 
	{
		public PokemonEssentials.Interface.Screen.IPokemonEntryScreen TextEntryScreen { get; set; }
		public PokemonEssentials.Interface.Screen.IIntroEventScreen IntroScreen { get; set; }
		public PokemonEssentials.Interface.Screen.ILoadScreen Load { get; set; }
		public PokemonEssentials.Interface.Screen.ISaveScreen Save { get; set; }
		public PokemonEssentials.Interface.Screen.IOptionScreen OptionScreen { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonMenuScreen PauseMenuScreen { get; set; }
		public PokemonEssentials.Interface.Screen.IPartyDisplayScreen Party { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonSummaryScreen Summary { get; set; }
		public PokemonEssentials.Interface.Screen.IBagScreen Bag { get; set; }
		//public PokemonEssentials.Interface.Screen. RegionMap { get; set; }
		//public PokemonEssentials.Interface.Screen. Phone { get; set; }
		public PokemonEssentials.Interface.Screen.ITrainerCardScreen TrainerCard { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonStorageScreen PokemonStorageScreen { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonPokedexScreen PokedexScreen { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonNestMapScreen PokedexNestScreen { get; set; }
		public PokemonEssentials.Interface.Screen.IPokemonFormScreen PokedexFormScreen { get; set; }
		//public PokemonEssentials.Interface.Screen. EggHatching { get; set; }
		//public PokemonEssentials.Interface.Screen. Trading { get; set; }
		//public PokemonEssentials.Interface.Screen. MoveRelearner { get; set; }
		public PokemonEssentials.Interface.Screen.IMartScreen Mart { get; set; }
		//public PokemonEssentials.Interface.Screen. Debug { get; set; }
		public PokemonEssentials.Interface.Screen.IRelicStoneScreen RelicStone { get; set; } //Shadow Pokemon
		//public PokemonEssentials.Interface.Screen. PurityChamber { get; set; } //Shadow Pokemon
		public PokemonEssentials.Interface.Screen.IBattleSwapScreen BattleSwapScreen { get; set; }

		public Screen(params PokemonEssentials.Interface.Screen.IScreen[] screens)
		{
			foreach (PokemonEssentials.Interface.Screen.IScreen screen in screens)
			{
				if (screen is PokemonEssentials.Interface.Screen.IPokemonEntryScreen		s0) TextEntryScreen			= s0;
				else if (screen is PokemonEssentials.Interface.Screen.IIntroEventScreen		s1) IntroScreen				= s1;
				else if (screen is PokemonEssentials.Interface.Screen.ILoadScreen			s2) Load					= s2;
				else if (screen is PokemonEssentials.Interface.Screen.ISaveScreen			s3) Save					= s3;
				else if (screen is PokemonEssentials.Interface.Screen.IOptionScreen			s4) OptionScreen			= s4;
				else if (screen is PokemonEssentials.Interface.Screen.IPokemonMenuScreen	s5) PauseMenuScreen			= s5;
				else if (screen is PokemonEssentials.Interface.Screen.IPartyDisplayScreen	s6) Party					= s6;
				else if (screen is PokemonEssentials.Interface.Screen.IPokemonSummaryScreen	s7) Summary					= s7;
				else if (screen is PokemonEssentials.Interface.Screen.IBagScreen			s8) Bag						= s8;
				else if (screen is PokemonEssentials.Interface.Screen.ITrainerCardScreen	s9) TrainerCard				= s9;
				else if (screen is PokemonEssentials.Interface.Screen.IPokemonStorageScreen	s10) PokemonStorageScreen	= s10;
				else if (screen is PokemonEssentials.Interface.Screen.IPokemonPokedexScreen	s11) PokedexScreen			= s11;
				else if (screen is PokemonEssentials.Interface.Screen.IPokemonNestMapScreen	s12) PokedexNestScreen		= s12;
				else if (screen is PokemonEssentials.Interface.Screen.IPokemonFormScreen	s13) PokedexFormScreen		= s13;
				else if (screen is PokemonEssentials.Interface.Screen.IMartScreen			s14) Mart					= s14;
				else if (screen is PokemonEssentials.Interface.Screen.IRelicStoneScreen		s15) RelicStone				= s15; 
				else if (screen is PokemonEssentials.Interface.Screen.IBattleSwapScreen		s16) BattleSwapScreen		= s16;
			}																				 
		}																					 
	}																						 
}																							 
																							 
																							 
																							 