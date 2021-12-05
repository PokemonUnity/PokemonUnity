using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Utility;
using PokemonUnity.UX;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// </summary>
	public interface IMapConnection
	{
		/// <summary>
		///Map 1's ID number
		/// </summary>
		int Id1 { get; }
		/// <summary>
		///Map 1's edge (one of N, North, S, South, E, East, W or West)
		/// </summary>
		string Edge1 { get; }
		/// <summary>
		///Map 1's connecting point (a positive integer)
		/// </summary>
		ushort Offset1 { get; }
		/// <summary>
		///Map 2's ID number
		/// </summary>
		int Id2 { get; }
		/// <summary>
		///Map 2's edge (one of N, North, S, South, E, East, W or West)
		/// </summary>
		string Edge2 { get; }
		/// <summary>
		///Map 2's connecting point (a positive integer)
		/// </summary>
		ushort Offset2 { get; }
	}

	public interface IMapChunkUnit
	{
		int MapId { get; }
		int X { get; }
		int Y { get; }

		//MapChunkUnit(PokemonEssentials.Interface.IGame_Map map, int x, int y);
	}

	public interface ITilePosition
	{
		int MapId { get; }
		float X { get; }
		float Y { get; }
		float Z { get; }
		//IVector Vector { get; }
		//Terrains Terrain { get; }

		//TilePosition(int map, float x, float y, float z = 0);
	}

	/// <summary>
	/// Map Factory (allows multiple maps to be loaded at once and connected)
	/// </summary>
	public interface IMapFactoryHelper
	{
		IList<IMapConnection> MapConnections { get; }
		/// <summary>
		/// array of [width, height] for maps, indexed by id
		/// </summary>
		int[][] MapDims { get; }

		void clear();

		/// <summary>
		/// </summary>
		/// <param name="id"></param>
		/// <param name="edge">Considers the special strings "N","W","E","S"</param>
		/// <returns>Returns the X or Y coordinate (width/height) of an edge on the map with id.</returns>
		int getMapEdge(int id, string edge);

		/// <summary>
		/// Gets the height and width of the map with id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		int[] getMapDims(int id);

		IList<IMapConnection> getMapConnections();

		bool hasConnections(int id);

		bool mapInRange(IGameMap map);

		bool mapInRangeById(int id, float dispx, float dispy);
	}

	/// <summary>
	/// Extension of <see cref="PokemonEssentials.Interface.IGameMap"/>
	/// </summary>
	public interface IGameMapFactory
	{
		void updateTileset();
	}

	/// <summary>
	/// Extension of base application class (Game)
	/// </summary>
	public interface IGameFactory
	{
		void updateTilesets();
	}

	public interface IMapFactory
	{
		PokemonEssentials.Interface.IGameMap[] maps { get; }

		//IMapFactory initialize(int id);

		PokemonEssentials.Interface.IGameMap map();

		/// <summary>
		/// Clears all maps and sets up the current map with id.  This function also sets
		/// the positions of neighboring maps and notifies the game system of a map change.
		/// </summary>
		/// <param name="id"></param>
		void setup(int id);

		bool hasMap(int id);
		int getMapIndex(int id);

		void setMapChanging(int newID, PokemonEssentials.Interface.IGameMap newMap);

		void setMapChanged(int prevMap);

		void setSceneStarted(IScene scene);

		/// <summary>
		/// Similar to <see cref="IGameCharacter.passable"/>, but supports map connections
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		bool isPassableFromEdge(float x, float y);

		bool isPassableStrict(int mapID, float x, float y, IGameCharacter thisEvent = null);

		bool isPassable(int mapID, float x, float y, IGameCharacter thisEvent = null);

		PokemonEssentials.Interface.IGameMap getMap(int id);

		PokemonEssentials.Interface.IGameMap getMapNoAdd(int id);

		void updateMaps(IScene scene);

		// :internal:
		void updateMapsInternal();

		bool areConnected(int mapID1, int mapID2);

		IMapChunkUnit getNewMap(float playerX, float playerY);

		void setCurrentMap();

		PokemonUnity.Overworld.Terrains? getTerrainTag(int mapid, float x, float y, bool countBridge = false);

		PokemonUnity.Overworld.Terrains? getFacingTerrainTag(float? dir = null, IGameCharacter @event = null);

		IPoint getRelativePos(int thisMapID, float thisX, float thisY, int otherMapID, float otherX, float otherY);

		/// <summary>
		/// Gets the distance from this event to another event.  Example: If this event's
		/// coordinates are (2,5) and the other event's coordinates are (5,1), returns
		/// the array (3,-4), because (5-2=3) and (1-5=-4).
		/// </summary>
		/// <param name="thisEvent"></param>
		/// <param name="otherEvent"></param>
		/// <returns></returns>
		void getThisAndOtherEventRelativePos(IGameCharacter thisEvent, IGameCharacter otherEvent);

		void getThisAndOtherPosRelativePos(IGameCharacter thisEvent, int otherMapID, float otherX, float otherY);

		ITilePosition getOffsetEventPos(IGameCharacter @event, float xOffset, float yOffset);

		ITilePosition getRealTilePos(int mapID, float x, float y);

		ITilePosition getFacingTileFromPos(int mapID, float x, float y, int direction = 0, int steps = 1);

		ITilePosition getFacingTile(float? direction = null, IGameCharacter @event = null, int steps = 1);

		void setMapsInRange();
	}
}