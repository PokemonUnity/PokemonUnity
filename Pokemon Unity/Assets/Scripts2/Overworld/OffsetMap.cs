/// <summary>
/// Represents an Offset Map to be stored by the LevelLoader.
/// </summary>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public class OffsetMap
{

    /// <summary>
    /// The identifier of this offset map, which contains Weather, Time and Season.
    /// </summary>
    public string Identifier { get; }

    /// <summary>
    /// The name of the file relative to *\maps\ with extension.
    /// Like Level.vb-LevelFile
    /// </summary>
    public string MapName { get; }

    /// <summary>
    /// If this map got loaded.
    /// </summary>
    public bool Loaded { get; } = false;

    /// <summary>
    /// The list of entities.
    /// Only filled if LoadMap was performed.
    /// </summary>
    public List<Entity> Entities { get; } = null;

    /// <summary>
    /// The list of floors.
    /// Only filled if LoadMap was performed.
    /// </summary>
    public List<Entity> Floors { get; } = null;

    /// <summary>
    /// Creates a new instance of the OffsetMap class.
    /// </summary>
    public OffsetMap(string MapName)
    {
        MapName = MapName;

        // Set the identifier:
        // Offset Map                   Map Weather                             Region Weather                       Time                   Season
        Identifier = MapName + "|" + Screen.Level.World.CurrentMapWeather + "|" + World.GetCurrentRegionWeather() + "|" + World.GetTime() + "|" + World.CurrentSeason();
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
