﻿namespace PokemonUnity.Combat.Data
{
	public class DamageState : PokemonEssentials.Interface.PokeBattle.IDamageState
	{
		/// <summary>
		/// HP lost by opponent, inc. HP lost by a substitute
		/// </summary>
		public int HPLost { get; set; }
		/// <summary>
		/// Critical hit flag
		/// </summary>
		public bool Critical { get; set; }
		/// <summary>
		/// Calculated damage
		/// </summary>
		public int CalcDamage { get; set; }
		/// <summary>
		/// Type effectiveness
		/// </summary>
		public double TypeMod { get; set; }
		/// <summary>
		/// A substitute took the damage
		/// </summary>
		public bool Substitute { get; set; }
		/// <summary>
		/// Focus Band used
		/// </summary>
		public bool FocusBand { get; set; }
		/// <summary>
		/// Focus Sash used
		/// </summary>
		public bool FocusSash { get; set; }
		/// <summary>
		/// Sturdy ability used
		/// </summary>
		public bool Sturdy { get; set; }
		/// <summary>
		/// Damage was endured
		/// </summary>
		public bool Endured { get; set; }
		/// <summary>
		/// A type-resisting berry was used
		/// </summary>
		public bool BerryWeakened { get; set; }

		public PokemonEssentials.Interface.PokeBattle.IDamageState initialize()
		{
			return this;
		}

		public void Reset()
		{
			HPLost        = 0;
			Critical      = false;
			CalcDamage    = 0;
			TypeMod       = 0;
			Substitute    = false;
			FocusBand     = false;
			FocusSash     = false;
			Sturdy        = false;
			Endured       = false;
			BerryWeakened = false;
		}
	}
}