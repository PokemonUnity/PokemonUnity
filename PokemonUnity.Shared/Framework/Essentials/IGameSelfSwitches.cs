using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public interface IGameSelfSwitches
	{
		bool this[int key] { get; set; }
	}
}