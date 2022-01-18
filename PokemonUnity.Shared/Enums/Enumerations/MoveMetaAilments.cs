namespace PokemonUnity.Shared.Enums
{
	public class MoveMetaAilments : Enumeration
	{
		protected MoveMetaAilments(int id, string name) : base(id, name) { }
		public static readonly MoveMetaAilments UNKNOWN				= new MoveMetaAilments(-1,	"UNKNOWN");
		public static readonly MoveMetaAilments NONE				= new MoveMetaAilments(0,	"NONE");
		public static readonly MoveMetaAilments PARALYSIS			= new MoveMetaAilments(1,	"PARALYSIS");
		public static readonly MoveMetaAilments SLEEP				= new MoveMetaAilments(2,	"SLEEP");
		public static readonly MoveMetaAilments FREEZE				= new MoveMetaAilments(3,	"FREEZE");
		public static readonly MoveMetaAilments BURN				= new MoveMetaAilments(4,	"BURN");
		public static readonly MoveMetaAilments POISON				= new MoveMetaAilments(5,	"POISON");
		public static readonly MoveMetaAilments CONFUSION			= new MoveMetaAilments(6,	"CONFUSION");
		public static readonly MoveMetaAilments INFATUATION			= new MoveMetaAilments(7,	"INFATUATION");
		public static readonly MoveMetaAilments TRAP				= new MoveMetaAilments(8,	"TRAP");
		public static readonly MoveMetaAilments NIGHTMARE			= new MoveMetaAilments(9,	"NIGHTMARE");
		public static readonly MoveMetaAilments TORMENT				= new MoveMetaAilments(12,	"TORMENT");
		public static readonly MoveMetaAilments DISABLE				= new MoveMetaAilments(13,	"DISABLE");
		public static readonly MoveMetaAilments YAWN				= new MoveMetaAilments(14,	"YAWN");
		public static readonly MoveMetaAilments HEAL_BLOCK			= new MoveMetaAilments(15,	"HEAL_BLOCK");
		public static readonly MoveMetaAilments NO_TYPE_IMMUNITY	= new MoveMetaAilments(17,	"NO_TYPE_IMMUNITY");
		public static readonly MoveMetaAilments LEECH_SEED			= new MoveMetaAilments(18,	"LEECH_SEED");
		public static readonly MoveMetaAilments EMBARGO				= new MoveMetaAilments(19,	"EMBARGO");
		public static readonly MoveMetaAilments PERISH_SONG			= new MoveMetaAilments(20,	"PERISH_SONG");
		public static readonly MoveMetaAilments INGRAIN				= new MoveMetaAilments(21,	"INGRAIN");
		public static readonly MoveMetaAilments SILENCE				= new MoveMetaAilments(24,	"SILENCE");
	}
}