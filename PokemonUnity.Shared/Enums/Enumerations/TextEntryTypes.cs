namespace PokemonUnity.Shared.Enums
{
	public class TextEntryTypes : Enumeration
	{
		protected TextEntryTypes(int id, string name) : base(id, name) { }
		public static readonly TextEntryTypes NONE				= new TextEntryTypes(0,	"NONE");
		public static readonly TextEntryTypes PLAYER			= new TextEntryTypes(1,	"PLAYER");
		public static readonly TextEntryTypes POKEMON			= new TextEntryTypes(2,	"POKEMON");
		public static readonly TextEntryTypes PC_BOX			= new TextEntryTypes(3,	"PC_BOX");
		public static readonly TextEntryTypes NPC				= new TextEntryTypes(4,	"NPC");
	}
}
