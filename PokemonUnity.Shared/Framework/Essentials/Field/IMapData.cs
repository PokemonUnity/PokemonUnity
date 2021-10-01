using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.UX;
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
		List<Pokemons> encounter_list	{ get; }
		int encounter_step				{ get; }
		int?[,,] data					{ get; }
		Dictionary<int,int> events		{ get; }

		//MapData(int width, int height);
	}
}