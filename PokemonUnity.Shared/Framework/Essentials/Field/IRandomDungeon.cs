using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.UX;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// This class is designed to favor different values more than a uniform
	/// random generator does
	/// </summary>
	public interface IAntiRandom {
		IAntiRandom initialize(int size);

		int get();
	}

	public interface IDungeonMaze {
		//int TILEWIDTH=13;
		//int TILEHEIGHT=13;
		//int MINWIDTH=5;
		//int MINHEIGHT=4;
		//int MAXWIDTH=11;
		//int MAXHEIGHT=10;
		//int None=0;
		//int TurnLeft=1;
		//int TurnRight=2;
		//int Turn180=3;

		/// <summary>
		/// paints a room
		/// </summary>
		/// <param name="tile"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="w">width</param>
		/// <param name="h">height</param>
		void paintRect(int[] tile, int x, int y, int w, int h);

		/// <summary>
		/// paints a tile
		/// </summary>
		/// <param name="dungeon"></param>
		/// <param name="dstX"></param>
		/// <param name="dstY"></param>
		/// <param name="tile"></param>
		/// <param name="rotation"></param>
		void paintTile(IDungeon dungeon, int dstX, int dstY, int[] tile, int rotation);


		bool paintCell(IDungeon dungeon, int xDst, int yDst, int[] tile, float rotation);

		void generateTiles();
	}

		//public interface EdgeMasks { //Moved to overworld enums
		//	int North=1;
		//	int West=2;
		//	int East=4;
		//	int South=8;
		//	int Visited=16;
		//}

	// Dungeon generation algorithm found at:
	// http://members.gamedev.net/potentialwell/ProceduralDungeonGeneration-JTippets.pdf
	public interface IMazeNode {
		IMazeNode initialize();

		void setEdge(int e);
		void clearEdge(int e);
		void clear();
		void set();
		bool getEdge(int e);
		bool isBlocked();
	}

	public interface INodeListElement {
		int x				{ get; }
		int y				{ get; }

		INodeListElement initialize(int x,int y);
	}

	public interface IMaze {
		int cellWidth				{ get; }
		int cellHeight			{ get; }
		int nodeWidth				{ get; }
		int nodeHeight			{ get; }

		EdgeMasks[] dirs { get; }

		IMaze initialize(int cw, int ch);

		IList<INodeListElement> buildNodeList();

		void setEdgeNode(float x, float y, int edge);

		void clearEdgeNode(float x, float y, int edge);

		bool isBlockedNode(float x, float y);

		bool getEdgeNode(float x, float y, int edge);

		EdgeMasks getEdgePattern(float x, float y);

		void setAllEdges();

		void clearAllEdges();

		void clearAllCells();

		void setVisited(float x, float y);

		bool getVisited(float x, float y);

		void clearVisited(float x, float y);

		EdgeMasks randomDir();

		void buildMazeWall(float x, float y, EdgeMasks dir, int len);

		void generateWallGrowthMaze(int? minWall = null, int? maxWall = null);

		void recurseDepthFirst(float x, float y, int depth);

		void generateDepthFirstMaze();
	}

	public interface IDungeonTable {
		IDungeonTable initialize(IDungeon dungeon);
  
		int xsize { get; }
		int ysize { get; }
	
		int this[int x, int y] { get; }
	}
	public interface IDungeon {
		int width { get; }
		int height { get; }
		//XBUFFER=7;
		//YBUFFER=5;


		IDungeon initialize(int width, int height);

		void clear();

		string write();

		int this[int x, int y] { get; set; }

		int value(int x, int y);

		bool get(int x, int y);

		bool isWall(int x, int y);

		bool isRoom(int x, int y);

		void generate();

		void generateMapInPlace(IGameMap map);

		void paint(IRect rect, int offsetX, int offsetY);

		bool intersects(IRect r1, IRect r2);
	}

	/// <summary>
	/// Extension of base Game...
	/// </summary>
	public interface IGameDungeon
	{
		//IPoint is supposed to be an int array of x and y (`[int x,int y]`)
		PokemonUnity.Utility.IPoint pbRandomRoomTile(IDungeon dungeon, ref IList<PokemonUnity.Utility.IPoint> tiles);

		//Events.onMapCreate+=delegate(object sender, EventArgs e)
		//void Events_OnMapCreate(object sender, EventArg.OnMapCreateEventArgs e);
	}
}