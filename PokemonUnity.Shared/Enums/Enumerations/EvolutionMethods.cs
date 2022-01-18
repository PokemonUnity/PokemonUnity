namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// no parameter,
	/// Positive integer,
	/// Item  = <see cref="Items"/>,
	/// Move = <see cref="Moves"/>,
	/// Species = <see cref="Pokemons"/>,
	/// Type = <see cref="Types"/>
	/// </summary>
	/// <example>
	/// <para>E.G.	Poliwhirl(61)
	///		<code>new int[]{62,186},
	///		new string[]{"Stone,Water Stone","Trade\Item,King's Rock"}),</code></para> 
	/// <para>
	/// E.G. to evolve to sylveon
	///		<code>new int[]{..., 700},
	///		new string[]{..., "Amie\Move,2\Fairy"}),</code>
	/// </para> 
	/// </example>
	public class EvolutionMethods : Enumeration
	{
		protected EvolutionMethods(int id, string name) : base(id, name) { }
	}
}