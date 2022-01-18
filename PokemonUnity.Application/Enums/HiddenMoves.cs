namespace PokemonUnity.Enums
{
	public class HiddenMoves : PokemonUnity.Shared.Enums.Enumeration
	{
		/// <summary>
		/// Badge needed to acquire in order to use this Move in Overworld Gameplay
		/// </summary>
		public int BadgeId { get; private set; }
		/// <summary>
		/// Number of badges needed to access this Move in Overworld Gameplay
		/// </summary>
		public int BadgeCount { get; private set; }
		public int RegionId { get; private set; }
		public int MoveId { get; private set; }
		protected HiddenMoves(int id, string name) : base(id, name) { }
	}
}