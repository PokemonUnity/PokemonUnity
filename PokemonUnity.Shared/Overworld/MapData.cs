using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
	[Serializable] //ToDo: Should be a class and not a struct?
	public struct MapData : IMapData
	{
		public int tileset_id									{ get; private set; }
		public int width										{ get; private set; }
		public int height										{ get; private set; }
		public bool autoplay_bgm								{ get; private set; }
		public IAudioBGM bgm									{ get; private set; }
		public bool autoplay_bgs								{ get; private set; }
		public IAudioBGS bgs									{ get; private set; }
		public IList<IEncounterPokemon> encounter_list			{ get; private set; }
		//public IList<IMapEncounterMetadata> encounter_list	{ get; private set; }
		public int encounter_step								{ get; private set; }
		public int?[,,] data									{ get; private set; }
		//public IDictionary<int,int> events					{ get; private set; }
		public IDictionary<int,IGameCharacter> events			{ get; private set; }

		public MapData(int width, int height)
		{
			@tileset_id = 1;
			this.width = width;
			this.height = height;
			@autoplay_bgm = false;
			@bgm = null; //new RPG.AudioFile();
			@autoplay_bgs = false;
			@bgs = null; //new RPG.AudioFile("", 80);
			@encounter_list = new List<IEncounterPokemon>();
			@encounter_step = 30;
			@data = new int?[width, height, 3];
			//@events = new Dictionary<int, int>();
			@events = new Dictionary<int, IGameCharacter>();
		}

		public MapData(int width, int height, IAudioBGM bgmusic, IAudioBGS bgsound)
		{
			@tileset_id = 1;
			this.width = width;
			this.height = height;
			@autoplay_bgm = false;
			@bgm = bgmusic; //new AudioFile().initialize();
			@autoplay_bgs = false;
			@bgs = bgsound; //new AudioFile().initialize("", 80);
			@encounter_list = new List<IEncounterPokemon>();
			@encounter_step = 30;
			@data = new int?[width, height, 3];
			//@events = new Dictionary<int, int>();
			@events = new Dictionary<int, IGameCharacter>();
		}
	}
}