namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	/// ToDo: Rename TileTypes or TileCategory?
	public enum MapTiles
	{
		/// <summary>
		/// Spawns random encounter
		/// </summary>
		GRASS,
		/// <summary>
		/// Animated water
		/// </summary>
		SEA,
		/// <summary>
		/// 3-dimensional airborne encounter
		/// </summary>
		SKY,
		/// <summary>
		/// Regular walk-able path
		/// </summary>
		TILE,
		/// <summary>
		/// Impassable (invisible) collision object
		/// </summary>
		WALL,
		/// <summary>
		/// Can only cross with surfing action
		/// </summary>
		WATER,
		/// <summary>
		/// Triggers map change or used for map events
		/// </summary>
		ZONE
	}
}