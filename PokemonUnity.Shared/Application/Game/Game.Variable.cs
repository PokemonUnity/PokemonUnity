using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	public partial class Game
	{
		public GlobalMetadata Global { get; private set; }
		//public PokemonMapFactory MapFactory { get; private set; }
		//public MapMetadata MapData { get; private set; }
		//public PokemonMapMetadata PokemonMap { get; private set; }
		public PokemonSystem PokemonSystem { get; private set; }
		//public PokemonTemp PokemonTemp { get; set; }
		//public PokemonEncounter PokemonEncounters { get; private set; }
		public PokemonStorage PokemonStorage { get; private set; }
		public Character.PokemonBag Bag { get; private set; }
		public ISceneState Scene { get; set; }
		//public GameTemp GameTemp { get; private set; }
		//public Character.Player Player { get; set; }
		public Combat.Trainer Trainer { get; set; }
		//public Avatar.Trainer DataSystem { get; set; }
		//public Avatar.Trainer[] DataTilesets { get; set; }
		//public Game_CommonEvent[] DataCommonEvents { get; set; }
		public IGame_System GameSystem { get; set; }
		public Dictionary<int, bool> GameSwitches { get; set; }
		public Dictionary<int, bool> GameSelfSwitches { get; set; }
		public Dictionary<int, object> GameVariables { get; set; }
		public IGame_Screen GameScreen { get; set; }
		//public Avatar.Player GamePlayer { get; set; }
		public PokemonUnity.Game.Game_Map GameMap { get; set; }
		//public Game_Message GameMessage { get; set; }
		public int SpeechFrame { get; private set; }
		public static PokemonUnity.UX.IFrontEnd UI { get; private set; }
	}
}