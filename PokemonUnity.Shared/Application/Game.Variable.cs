using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using System.IO;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	public partial class Game
	{
		public ISceneState Scene { get; set; }
		public Avatar.Trainer Trainer { get; set; }
		public Dictionary<int, bool> GameSwitches { get; set; }
		public Dictionary<int, bool> GameSelfSwitches { get; set; }
		public Dictionary<int, object> GameVariables { get; set; }
		public IGame_Screen GameScreen { get; set; }
		public int SpeechFrame { get; private set; }
	}
}