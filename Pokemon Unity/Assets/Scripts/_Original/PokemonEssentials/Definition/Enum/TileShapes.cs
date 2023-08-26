//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace PokemonUnity
{
	public enum TileShapes
	{
		/// <summary>
		/// Flat surface. 
		/// </summary>
		/// Stairs use flats too
		Flat,
		/// <summary>
		/// Box(five sides)
		/// </summary>
		Entrance,
		/// <summary>
		/// 45 degree slant.
		/// </summary>
		Slant,
		/// <summary>
		/// Flat vertical surface
		/// </summary>
		CliffSide,
		/// <summary>
		/// "In" corner.
		/// </summary>
		CliffCornerIn,
		/// <summary>
		/// "Out" corner.
		/// </summary>
		CliffCornerOut,
		/// <summary>
		/// Ledge.
		/// </summary>
		LedgeJump,
		/// <summary>
		/// Ledge Corner.
		/// </summary>
		LedgeJumpCorner,
		/// <summary>
		/// Water's edge.
		/// </summary>
		LedgeWater,
		/// <summary>
		/// Water's edge "in" corner.
		/// </summary>
		LedgeWaterIn,
		/// <summary>
		/// Water's edge "out" corner.
		/// </summary>
		LedgeWaterOut,
		/// <summary>
		/// Only here because 3ds using a 2x2 dirt path
		/// </summary>
		WalkPath,
		/// <summary>
		/// Thin block. 4 Pixels wide.
		/// </summary>
		Fence,
		/// <summary>
		/// Double floor (a floor with a floor above it)
		/// </summary>
		Bridge,
		/// <summary>
		/// Two Flat pieces intersecting in their centers at 90 degree angles.
		/// </summary>
		Crossway,
		/// <summary>
		/// Box (Six Sides).
		/// </summary>
		NULL
	}
}
