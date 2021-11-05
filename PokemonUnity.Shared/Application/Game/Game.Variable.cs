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
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
	public partial class Game
	{
		public PokemonEssentials.Interface.IGlobalMetadata Global { get; private set; }
		//public IPokemonMapFactory MapFactory { get; private set; }
		public IMapFactory MapFactory { get; private set; }
		//public IPokemonMapMetadata PokemonMap { get; private set; }
		public IMapMetadata MapData { get; private set; }
		public PokemonSystem PokemonSystem { get; private set; }
		public PokemonEssentials.Interface.ITempMetadata PokemonTemp { get; set; }
		//public PokemonEncounter PokemonEncounters { get; private set; }
		public IEncounters PokemonEncounters { get; private set; }
		public PokemonStorage PokemonStorage { get; private set; }
		public Character.PokemonBag Bag { get; private set; }
		public ISceneState Scene { get; set; }
		public IGameTemp GameTemp { get; private set; }
		//public IGamePlayer Player { get; set; }
		public Combat.Trainer Trainer { get; set; }
		//public Avatar.Trainer DataSystem { get; set; }
		//public Avatar.Trainer[] DataTilesets { get; set; }
		public IGameCommonEvent[] DataCommonEvents { get; set; }
		public IGameSystem GameSystem { get; set; }
		public Dictionary<int, bool> GameSwitches { get; set; }
		public Dictionary<int, bool> GameSelfSwitches { get; set; }
		public Dictionary<int, object> GameVariables { get; set; }
		public IGameScreen GameScreen { get; set; }
		public IGamePlayer GamePlayer { get; set; }
		public IGameMap GameMap { get; set; }
		//public IGameMessage GameMessage { get; set; }
		public int SpeechFrame { get; private set; }
		public static PokemonUnity.UX.IFrontEnd UI { get; private set; }
	}
}