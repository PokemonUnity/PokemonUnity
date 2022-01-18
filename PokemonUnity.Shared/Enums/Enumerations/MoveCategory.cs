namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Is one of the following:
	/// Physical, Special, or Status
	/// </summary>
	public class MoveCategory : Enumeration
	{
		protected MoveCategory(int id, string name) : base(id, name) { }
		public static readonly MoveCategory PHYSICAL		= new MoveCategory(2,	"PHYSICAL");
		public static readonly MoveCategory SPECIAL			= new MoveCategory(3,	"SPECIAL");
		public static readonly MoveCategory STATUS			= new MoveCategory(1,	"STATUS");
	}
}