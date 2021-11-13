using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Overworld
{
	public static class Terrain
	{
		public static bool isSurfable (Terrains tag) {
			return isWater(tag);
		}

		public static bool isWater (Terrains tag) {
			return tag == Terrains.Water ||
				   tag == Terrains.StillWater ||
				   tag == Terrains.DeepWater ||
				   tag == Terrains.WaterfallCrest ||
				   tag == Terrains.Waterfall;
		}

		public static bool isPassableWater (Terrains tag) {
			return tag == Terrains.Water ||
				   tag == Terrains.StillWater ||
				   tag == Terrains.DeepWater ||
				   tag == Terrains.WaterfallCrest;
		}

		public static bool isJustWater (Terrains tag) {
			return tag == Terrains.Water ||
				   tag == Terrains.StillWater ||
				   tag == Terrains.DeepWater;
		}

		public static bool isGrass (Terrains tag) {
			return tag == Terrains.Grass ||
				   tag == Terrains.TallGrass ||
				   tag == Terrains.UnderwaterGrass ||
				   tag == Terrains.SootGrass;
		}

		/// <summary>
		/// The Poké Radar only works in these tiles
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public static bool isJustGrass (Terrains tag) {
			return tag == Terrains.Grass ||
				   tag == Terrains.SootGrass;
		}

		public static bool isLedge (Terrains tag) {
			return tag == Terrains.Ledge;
		}

		public static bool isIce (Terrains tag) {
			return tag == Terrains.Ice;
		}

		public static bool isBridge (Terrains tag) {
			return tag == Terrains.Bridge;
		}

		public static bool hasReflections (Terrains tag) {
			return tag == Terrains.StillWater ||
				   tag == Terrains.Puddle;
		}

		public static bool onlyWalk (Terrains tag) {
			return tag == Terrains.TallGrass ||
				   tag == Terrains.Ice;
		}
	}
}