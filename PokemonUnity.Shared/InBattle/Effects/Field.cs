using PokemonUnity;
using PokemonUnity.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	public partial class Effects
	{		   
		/// <summary>
		/// These effects apply to the battle (i.e. both sides)
		/// </summary>
		public class Field { 
			public byte ElectricTerrain	{ get; set; }
			public byte FairyLock		{ get; set; }
			public bool FusionBolt		{ get; set; }
			public bool FusionFlare		{ get; set; }
			public byte GrassyTerrain	{ get; set; }
			public byte Gravity			{ get; set; }
			public bool IonDeluge		{ get; set; }
			public byte MagicRoom		{ get; set; }
			public byte MistyTerrain	{ get; set; }
			public byte MudSportField	{ get; set; }
			public byte TrickRoom		{ get; set; }
			public byte WaterSportField	{ get; set; }
			public byte WonderRoom		{ get; set; }
		}
	}
}