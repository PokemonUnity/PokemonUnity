using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonEssentials.Interface.Field
{
	/// <summary>
	/// </summary>
	public interface IMapData
	{
		int tileset_id					{ get; }
		int width						{ get; }
		int height						{ get; }
		bool autoplay_bgm				{ get; }
		IAudioBGM bgm					{ get; }
		bool autoplay_bgs				{ get; }
		IAudioBGS bgs					{ get; }
		IList<Pokemons> encounter_list	{ get; }
		int encounter_step				{ get; }
		int?[,,] data					{ get; }
		IDictionary<int,int> events		{ get; }

		//MapData(int width, int height);
	}

	/// <summary>
	/// The size and shape of this map in <see cref="IRegionMap"/> squares.
	/// Maps with this metadata calculate the relative position of the player on that map, 
	/// and show the player's head icon in the appropriate place depending on this position.
	/// </summary>
	/// <example>
	/// For example, if the width of the map is 2, and the layout is "1011", then the map is a 2x2 L-shaped map with the gap in the top right corner.
	/// </example>
	public interface IRegionMapMetadata
	{
		/// <summary>
		/// The width of the map, in squares. This can be 1.
		/// </summary>
		int Width						{ get; }
		/// <summary>
		/// An array of 1s and 0s, which describe the shape of the map. 
		/// Can use a utility to convert from bool[] to bool[,]
		/// </summary>
		/// <remarks>
		/// It is more easily visualised by breaking it up into chunks with "width" number of digits each; 
		/// each successive chunk comes underneath the previous chunk. 
		/// A 1 represents a square that is part of the map, 
		/// and a 0 represents a square that isn't.
		/// </remarks>
		bool[] Shape					{ get; }
	}

	/// <summary>
	/// Each square of the <see cref="IRegionMap"/> can be given a variety of information. 
	/// The information each square of a region map can have is a <see cref="IRegionMapData"/> node.
	/// </summary>
	public interface IRegionMapData
	{
		/// <summary>
		/// The coordinates of the point on the region map, in squares. Every point must have these coordinates. 
		/// You cannot have more than one line in the same region with the same coordinates.
		/// </summary>
		ITilePosition Position			{ get; }
		/// <summary>
		/// The location's name (e.g. Route 101, Rustboro City, Mt. Moon).
		/// Several squares can have the same name (e.g. you can give both squares of a large city that takes up 2 squares the same name).
		/// </summary>
		string Name						{ get; }
		/// <summary>
		/// Optional. 
		/// The name of a particular building or cave or other feature within that location 
		/// (e.g. Pokémon Tower, Desert, Safari Zone). 
		/// You can only have one Point of Interest per square.
		/// </summary>
		string PointOfInterest			{ get; }
		/// <summary>
		/// Optional. 
		/// This is a Fly destination, and is a <seealso cref="ITilePosition"/>: 
		/// the map ID followed by X and Y coordinates of the location Fly will take you to when you choose this square to Fly to. 
		/// This location is usually directly in front of a Poké Center building, hence its old name "Healing Spot".
		/// </summary>
		ITilePosition FlyDestination	{ get; }
		/// <summary>
		/// Optional. 
		/// If this is a number, then the Game Switch of that number must be ON in order to show the square's information. 
		/// That is, the map's name and Point of Interest will not be shown unless that Game Switch is ON. 
		/// This only affects the player's version of the region map; the public version never shows the information for these squares.
		/// Multiple squares can depend on the same Game Switch.
		/// </summary>
		/// <remarks>
		/// Typically used for special locations outside of the storyline. 
		/// Note that only the name and Point of Interest are not displayed; 
		/// you can still Fly there (if "Fly Destination" is defined). 
		/// However, since you can only Fly to maps you have already visited, 
		/// this shouldn't usually matter as you would set this Game Switch ON at the same time as visiting the map.
		/// </remarks>
		int? Switch						{ get; }
	}

	/// <summary>
	/// The region map (also known as the Town Map) is the picture that shows the entire region.
	/// </summary>
	public interface IRegionMap
	{
		/// <summary>
		/// Each region defined has its own section, which begins with the region number in square brackets (the first/default region number is 0). 
		/// </summary>
		int Id							{ get; }
		/// <summary>
		/// The name of the region
		/// </summary>
		string Name						{ get; }
		/// <summary>
		/// The name of the graphic file depicting the region
		/// </summary>
		string Filename					{ get; }
		/// <summary>
		/// The information about each square (or "point").
		/// </summary>
		IRegionMapData[] Points			{ get; }
	}
}