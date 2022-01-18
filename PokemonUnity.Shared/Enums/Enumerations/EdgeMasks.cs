namespace PokemonUnity.Shared.Enums
{
	public class EdgeMasks : Enumeration
	{
		protected EdgeMasks(int id, string name) : base(id, name) { }
		public static readonly EdgeMasks North				= new EdgeMasks(1,	"North");
		public static readonly EdgeMasks West				= new EdgeMasks(2,	"West");
		public static readonly EdgeMasks East				= new EdgeMasks(4,	"East");
		public static readonly EdgeMasks South				= new EdgeMasks(8,	"South");
		public static readonly EdgeMasks Visited			= new EdgeMasks(16,	"Visited");
	}
}