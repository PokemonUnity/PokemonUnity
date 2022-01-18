namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// The likelihood of a Pokémon of the species being a certain gender.
	/// </summary>
	public class GenderRatio : Enumeration
	{
		//public System.Predicate<double> Predicate { get; private set; }
		public System.Func<bool> Func { get; private set; }
		protected GenderRatio(int id, string name, System.Func<bool> func) : base(id, name) { Func = func; }
		public static readonly GenderRatio AlwaysMale				= new GenderRatio(0,	"AlwaysMale"		, () => true); //(float genderRate = this._base.MaleRatio)
		public static readonly GenderRatio FemaleOneEighth			= new GenderRatio(1,	"FemaleOneEighth"	, () => { double n = (Core.Rand.NextDouble() * 100) + 1; return !(n >= 87.5f && n < 100f); });
		public static readonly GenderRatio Female25Percent			= new GenderRatio(2,	"Female25Percent"	, () => { double n = (Core.Rand.NextDouble() * 100) + 1; return !(n >= 75f && n < 87.5f); });
		public static readonly GenderRatio Female50Percent			= new GenderRatio(4,	"Female50Percent"	, () => { double n = (Core.Rand.NextDouble() * 100) + 1; return !(n >= 62.5f && n < 75f) || !(n >= 50f && n < 62.5f); });
		public static readonly GenderRatio Female75Percent			= new GenderRatio(6,	"Female75Percent"	, () => { double n = (Core.Rand.NextDouble() * 100) + 1; return !(n >= 37.5f && n < 50f) || !(n >= 25f && n < 37.5f); });
		public static readonly GenderRatio FemaleSevenEighths		= new GenderRatio(7,	"FemaleSevenEighths", () => { double n = (Core.Rand.NextDouble() * 100) + 1; return !(n >= 12.5f && n < 25f); });
		public static readonly GenderRatio AlwaysFemale				= new GenderRatio(8,	"AlwaysFemale"		, () => false); //{ double n = (Core.Rand.NextDouble() * 100) + 1; return !(n > 0f && n < 12.5f); });
		public static readonly GenderRatio Genderless				= new GenderRatio(-1,	"Genderless"		, null);
	}
}