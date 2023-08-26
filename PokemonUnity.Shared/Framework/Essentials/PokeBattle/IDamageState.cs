using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IDamageState
	{
		int HPLost { get; set; }        // HP lost by opponent, inc. HP lost by a substitute
		bool Critical { get; set; }      // Critical hit flag
		int CalcDamage { get; set; }    // Calculated damage
		double TypeMod { get; set; }       // Type effectiveness
		bool Substitute { get; set; }    // A substitute took the damage
		bool FocusBand { get; set; }     // Focus Band used
		bool FocusSash { get; set; }     // Focus Sash used
		bool Sturdy { get; set; }        // Sturdy ability used
		bool Endured { get; set; }       // Damage was endured
		bool BerryWeakened { get; set; } // A type-resisting berry was used

		void Reset();

		IDamageState initialize();
	}

	public interface ISuccessState
	{
		double TypeMod		{ get; set; }
		bool? UseState		{ get; set; }    // 0 - not used, 1 - failed, 2 - succeeded
		bool Protected		{ get; set; }
		int Skill			{ get; }

		ISuccessState initialize();

		void Clear();

		void UpdateSkill();
	}
}