using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using UnityEngine;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	public struct Tile
	{
		#region Variables
		/// <summary>
		/// Y measurement of this Tile's Node in Scene; 
		/// starts at Bottom and stretches Up
		/// </summary>
		public int Length 
		{
			get
			{
				switch (Shape)
				{
					case Shape.WalkPath:
						return 2;
					default:
						return 1;
				}
			}
		}
		/// <summary>
		/// X measurement of this Tile's Node in Scene;
		/// Starts at Left and stretches Right
		/// </summary>
		public int Width
		{
			get
			{
				switch (Shape)
				{
					case Shape.WalkPath:
						return 2;
					default:
						return 1;
				}
			}
		}
		/// <summary>
		/// X position of this Tile's Node in Scene
		/// </summary>
		public int X { get; private set; }
		/// <summary>
		/// Y position of this Tile's Node in Scene
		/// </summary>
		public int Y { get; private set; }
		/// <summary>
		/// Z position of this Tile's Node in Scene
		/// </summary>
		public float Z { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		/// ToDo: Enum?
		//public float[] Position {
		//	get{
		//		
		//	}
		// }
		public int Map { get; private set; } 
		//public bool Passable {
		//	get
		//	{
		//		switch (Shape)
		//		{
		//			default:
		//				return true;
		//		}
		//	}
		//}
		//public bool Encounter {
		//	get
		//	{
		//		switch (Terrain)
		//		{
		//			case Terrain.Grass:
		//			case Terrain.Sand:
		//			case Terrain.Rock:
		//			case Terrain.DeepWater:
		//			case Terrain.StillWater:
		//			case Terrain.Water:
		//			case Terrain.TallGrass:
		//			case Terrain.SootGrass:
		//				return true;
		//			case Terrain.Puddle:
		//			default:
		//				return false;
		//		}
		//	}
		//}
		//public bool NeedsSurf {
		//	get
		//	{
		//		switch (Terrain)
		//		{
		//			default:
		//				return false;
		//		}
		//	}
		//}
		public bool hasSnow { get; set; }
		public bool hasSand { get; set; }
		public bool isIce { get; set; }
		#endregion
		#region Enums
		public Terrain Terrain { get; set; }
		//public Environment Environment { get; set; }
		///// <summary>
		///// What Season Texture to apply
		///// </summary>
		//public Season Texture { get; set; }
		public Shape Shape { get; set; }
		/// <summary>
		/// Direction this Tile Node is facing;
		/// Rotation to use on this Tile's Node
		/// </summary>
		public Direction Direction { get; set; }
		#endregion

		//public Tile(int x, int y, int z) { }

		#region Explicit Operators
		/// <summary>
		/// Two Tile objects are equal if they each occupy by the same physical location.
		/// </summary>
		/// <param name="tile1">Tile Location 1</param>
		/// <param name="tile2">Tile Attempting to Occupy 2</param>
		/// <returns>Returns whether or not the two Tile entities are overlapping the same physical location</returns>
		public static bool operator == (Tile tile1, Tile tile2)
		{
			for (int x = 0; x < tile1.Width; x++)
			{
				if (tile1.X + x == tile2.X && (/*tile1.Y == tile2.Y &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
					//return true;
					for (int y = 0; y < tile1.Length; y++)
					{
						if (tile1.Y + y == tile2.Y && (/*tile1.X == tile2.X &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
							return true;
					}
			}
			return tile1.X == tile2.X && tile1.Y == tile2.Y /*&& tile1.Z == tile2.Z*/ && tile1.Map == tile2.Map;
		}
		public static bool operator != (Tile tile1, Tile tile2)
		{
			if (/*tile1.Z != tile2.Z ||*/ tile1.Map != tile2.Map)
				return true;
			for (int x = 0; x < tile1.Width; x++)
			{
				if (tile1.X + x == tile2.X && (/*tile1.Y == tile2.Y &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
					//return true;
					for (int y = 0; y < tile1.Length; y++)
					{
						if (tile1.Y + y == tile2.Y && (/*tile1.X == tile2.X &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
							return false;
					}
			}
			return tile1.X != tile2.X && tile1.Y != tile2.Y /*&& tile1.Z != tile2.Z*/ && tile1.Map != tile2.Map;
		}
		//public bool Equals(Tile obj)
		//{
		//	return this == obj;
		//}
		public bool Equals(int x, int y)
		{
			//return this.X == x && this.Y == y;
			Tile obj = new Tile() { X = x, Y = y };
			return base.Equals(obj);
		}
		public override bool Equals(object obj)
		{
			if (!(obj is Tile)) return false;
			return this == (Tile)obj;
		}
		public override int GetHashCode()
		{
			//ToDo: Test HashCode? Also, MapId should also be included...
			//Math was copied from Unity's Vector3
			//return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
			//return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Map.GetHashCode() >> 2;
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				return hash;
			}
		}
		#endregion
	}
}