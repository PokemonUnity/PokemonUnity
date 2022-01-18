namespace PokemonUnity.Shared.Enums
{
	public class EncounterOptions : Enumeration
	{
		protected EncounterOptions(int id, string name) : base(id, name) { }
		//Pal_Park, Egg, Hatched, Special_Event,		//= 0x0
		//Tall_Grass,									//= 0x2
		//Plot_Event, //Dialga/Palkia In-Game Event,	//= 0x4
		//Cave, Hall_of_Origin,							//= 0x5
		//Surfing, Fishing,								//= 0x7	
		//Building,										//= 0x9	
		//Great_Marsh, //(Safari Zone)					//= 0xA	
		//Starter, Fossil, Gift, //(Eevee)				//= 0xC	
	}
}