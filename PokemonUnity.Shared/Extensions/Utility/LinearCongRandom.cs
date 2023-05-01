using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace PokemonUnity.Utility
{
	/// <summary>
	/// Linear congruential random number generator
	/// </summary>
	public partial class LinearCongRandom
	{
		public static event EventHandler<OnSeedEventArgs> SeedGenerated;
		public class OnSeedEventArgs : EventArgs
		{
			public static readonly int EventId = typeof(OnSeedEventArgs).GetHashCode();

			public int Id { get { return EventId; } }
			/// <summary>
			/// What is the value seed created
			/// </summary>
			public long Seed { get; set; }
			/// <summary>
			/// What class or object requested for a new seed to be generated
			/// </summary>
			public System.Type Requestor { get; set; }
			/// <summary>
			/// Which entity object from pool of objects 
			/// </summary>
			public int RequestorId { get; set; }
		}
		public long? Seed 
		{ 
			get { return seed; } 
			protected set 
			{ 
				seed = value; 
				OnSeedEventArgs args = new OnSeedEventArgs { Seed = @seed.Value }; 
				SeedGenerated.Invoke(this, args); 
			} 
		}
		private long? seed { get; set; }
		public uint s1 { get; protected set; }
		public int s2 { get; protected set; }
		public LinearCongRandom(uint mul, int add, long? seed = null)
		{
			@s1 = mul;
			@s2 = add;
			this.seed = seed;
			if (@seed == null) @seed = (DateTime.Now.Ticks & 0xffffffff);
			if (@seed.Value < 0) @seed = (@seed.Value + 0xFFFFFFFF) + 1;
		}

		public static long dsSeed()
		{
			DateTime t = DateTime.Now;
			long seed = (((t.Month * t.Day + t.Minute + t.Second) & 0xFF) << 24) | (t.Hour << 16) | (t.Year - 2000);
			if (seed < 0) seed = (seed + 0xFFFFFFFF) + 1;
			return seed;
		}

		public static void pokemonRNG()
		{
			new LinearCongRandom(0x41c64e6d, 0x6073, dsSeed());
		}

		public static void pokemonRNGInverse()
		{
			new LinearCongRandom(0xeeb9eb65, 0xa3561a1, dsSeed());
		}

		public static void pokemonARNG()
		{
			new LinearCongRandom(0x6C078965, 0x01, dsSeed());
		}

		/// <summary>
		/// </summary>
		/// <returns>calculates @seed * @s1 + @s2</returns>
		public long getNext16()
		{   
			@seed = ((((@seed.Value & 0x0000ffff) * (@s1 & 0x0000ffff)) & 0x0000ffff) |
			   (((((((@seed.Value & 0x0000ffff) * (@s1 & 0x0000ffff)) & 0xffff0000) >> 16) +
			   ((((@seed.Value & 0xffff0000) >> 16) * (@s1 & 0x0000ffff)) & 0x0000ffff) +
			   (((@seed.Value & 0x0000ffff) * ((@s1 & 0xffff0000) >> 16)) & 0x0000ffff)) &
			   0x0000ffff) << 16)) + @s2;
			long r = (@seed.Value >> 16);
			if (r < 0) r = (r + 0xFFFFFFFF) + 1;
			return r;
		}

		public long getNext()
		{
			long r = (getNext16() << 16) | (getNext16());
			if (r < 0) r = (r + 0xFFFFFFFF) + 1;
			return r;
		}
	}
}