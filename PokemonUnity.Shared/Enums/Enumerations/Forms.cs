namespace PokemonUnity.Shared.Enums
{
	public class Forms : Enumeration
	{
		public int Index { get; set; }
		public Forms(int id, string name) : base(id, name) { }
		public static readonly Forms NONE			= new Forms(0,	"NONE");
	}
}