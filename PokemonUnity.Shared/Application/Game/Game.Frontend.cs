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
		#region Scenes //Set once at boot cycle, and forget about it
		public static PokemonUnity.IPokemonEvolutionScene PokemonEvolutionScene		{ get; set; }
		public static PokemonUnity.IPokemonScreen_Scene PokemonScreenScene			{ get; set; }
		public static PokemonUnity.IPokemonScreen PokemonScreen						{ get; set; }
		public static PokemonUnity.IPokemonBag_Scene PokemonBagScene				{ get; set; }
		public static PokemonUnity.IPokemonBagScreen PokemonBagScreen				{ get; set; }
		public static PokemonUnity.IPokemonSummaryScene PokemonSummaryScene			{ get; set; }
		public static PokemonUnity.IPokemonSummary PokemonSummary					{ get; set; }
		#endregion
	}
}