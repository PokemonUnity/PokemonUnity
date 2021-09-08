using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public interface IGameVariable
	{
		int this[int variable_id] { get; set; }
	}
}