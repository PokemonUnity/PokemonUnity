using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public struct SelfSwitchVariable
	{
		public int MapId	{ get; private set; }
		public int EventId	{ get; private set; }
		public string Name	{ get; private set; }

		public SelfSwitchVariable(int mapId, int eventId, string name)
		{
			MapId = mapId;
			EventId = eventId;
			Name = name;
		}
	}
	public interface ISelfSwitchVariable
	{
		int MapId	{ get; }
		int EventId { get; }
		string Name { get; }
	}
	public interface IGameSelfSwitches
	{
		bool this[ISelfSwitchVariable key] { get; set; }
	}
}