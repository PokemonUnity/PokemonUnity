namespace PokemonUnity.Shared.Enums
{
	public class TypeEffectives : Enumeration
	{
		public TypeEffectives(int id, string name) : base(id, name) { }
		//public static readonly TypeEffectives Broken					= new TypeEffectives(0,	"");
		public static readonly TypeEffectives Ineffective				= new TypeEffectives(0,	"TypeEffective_Text_Ineffective");
		public static readonly TypeEffectives NotVeryEffective			= new TypeEffectives(0,	"TypeEffective_Text_NotVeryEffective");
		public static readonly TypeEffectives NormalEffective			= new TypeEffectives(0,	"TypeEffective_Text_NormalEffective");
		public static readonly TypeEffectives SuperEffective			= new TypeEffectives(0,	"TypeEffective_Text_SuperEffective");
	}
}