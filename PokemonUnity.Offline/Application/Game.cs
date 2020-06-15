using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonUnity.Utility;
using PokemonUnity.Application;
using PokemonUnity.Character;

namespace PokemonUnity.Offline
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public partial class Game : PokemonUnity.Game//Application
	{
		#region Constructor
		public Game() : base()
		{
		}

		public Game(Character.Player player, Feature? features = null, Challenges challenge = Challenges.Classic, Locations checkpoint = Locations.PALLET_TOWN, int area = 0, int repelSteps = 0, string[] rival = null, 
			string playerItemData = null, string playerDayCareData = null, string playerBerryData = null, string playerNPCData = null, string playerApricornData = null)
			: base (player, features, challenge, checkpoint, area, repelSteps, rival)
		{
		}
		#endregion
	}
}