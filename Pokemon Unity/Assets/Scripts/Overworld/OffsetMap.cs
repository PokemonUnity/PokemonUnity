using PokemonUnity.Overworld.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld
{
	/// <summary>
	/// Represents an Offset Map to be stored by the LevelLoader.
	/// </summary>
	public class OffsetMap
	{

		/// <summary>
		/// The identifier of this offset map, which contains Weather, Time and Season.
		/// </summary>
		public string Identifier { get; private set; }

		/// <summary>
		/// The name of the file relative to *\maps\ with extension.
		/// Like Level.vb-LevelFile
		/// </summary>
		public string MapName { get; private set; }

		/// <summary>
		/// If this map got loaded.
		/// </summary>
		public bool Loaded { get; private set; } 

		/// <summary>
		/// The list of entities.
		/// Only filled if LoadMap was performed.
		/// </summary>
		public List<PokemonUnity.Overworld.Entity.Entity> Entities { get; private set; }

		/// <summary>
		/// The list of floors.
		/// Only filled if LoadMap was performed.
		/// </summary>
		public List<PokemonUnity.Overworld.Entity.Entity> Floors { get; private set; }

		/// <summary>
		/// Creates a new instance of the OffsetMap class.
		/// </summary>
		public OffsetMap(string MapName)
		{
			this.MapName = MapName;

			// Set the identifier:
			// Offset Map                   Map Weather										 Region Weather                          Time                   Season
			//Identifier = MapName + "|" + Game.Level.World.CurrentMapWeather + "|" + World.GetCurrentRegionWeather() + "|" + World.GetTime() + "|" + World.CurrentSeason();
		}

		/// <summary>
		/// Loads the offset map.
		/// </summary>
		public void LoadMap(Vector3 Offset)
		{
			Loaded = true;
		}

		public void ApplyToLevel(Level Level)
		{
		}
	}
}