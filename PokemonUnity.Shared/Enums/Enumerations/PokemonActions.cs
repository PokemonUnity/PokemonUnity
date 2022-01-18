namespace PokemonUnity.Shared.Enums
{
	public class PokemonActions : Enumeration
	{
		public PokemonActions(int id, string name) : base(id, name) { }
		public static readonly PokemonActions Battle			= new PokemonActions(0,	"Battle"); 
		public static readonly PokemonActions CallTo			= new PokemonActions(0,	"CallTo"); 
		public static readonly PokemonActions Party			= new PokemonActions(0,	"Party"); 
		public static readonly PokemonActions DayCare			= new PokemonActions(0,	"DayCare"); 
		public static readonly PokemonActions MysteryAction			= new PokemonActions(0,	"MysteryAction"); 
		public static readonly PokemonActions Scent			= new PokemonActions(0,	"Scent");
	}
}