using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IDamageState
	{
		int hplost { get; set; }        // HP lost by opponent, inc. HP lost by a substitute
		bool critical { get; set; }      // Critical hit flag
		int calcdamage { get; set; }    // Calculated damage
		double typemod { get; set; }       // Type effectiveness
		bool substitute { get; set; }    // A substitute took the damage
		bool focusband { get; set; }     // Focus Band used
		bool focussash { get; set; }     // Focus Sash used
		bool sturdy { get; set; }        // Sturdy ability used
		bool endured { get; set; }       // Damage was endured
		bool berryweakened { get; set; } // A type-resisting berry was used

		void reset();

		IDamageState initialize();
	}

	public interface ISuccessState
	{
		double typemod { get; set; }
		int useState { get; set; }    // 0 - not used, 1 - failed, 2 - succeeded
		bool @protected				{ get; set; }
		int skill { get; set; }

		ISuccessState initialize();

		void clear();

		void updateSkill();
	}
}