using PokemonUnity;
using PokemonUnity.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle.Effects
{
	/// <summary>
	/// These effects apply to the battle (i.e. both sides)
	/// </summary>
	public interface IEffectsField { 
		byte ElectricTerrain	{ get; set; }
		byte FairyLock		{ get; set; }
		bool FusionBolt		{ get; set; }
		bool FusionFlare		{ get; set; }
		byte GrassyTerrain	{ get; set; }
		byte Gravity			{ get; set; }
		bool IonDeluge		{ get; set; }
		byte MagicRoom		{ get; set; }
		byte MistyTerrain	{ get; set; }
		byte MudSportField	{ get; set; }
		byte TrickRoom		{ get; set; }
		byte WaterSportField	{ get; set; }
		byte WonderRoom		{ get; set; }
	}
}