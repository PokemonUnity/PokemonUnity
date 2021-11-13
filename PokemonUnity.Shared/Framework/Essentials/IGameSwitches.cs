using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public interface IGameSwitches
	{
		bool this[int switch_id] { get; set; }
	}
}