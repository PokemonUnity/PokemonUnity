using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
    public struct TilePosition : ITilePosition
	{
		public int MapId { get; }
		public float X { get; }
		public float Y { get; }
		public float Z { get; }
		//IVector Vector { get; }
		//Terrains Terrain { get; }

		public TilePosition(int map, float x, float y, float z = 0)
        {
			MapId = map;
			X = x;
			Y = y;
			Z = z;
        }
	}
}