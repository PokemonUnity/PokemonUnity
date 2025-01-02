﻿namespace PokemonUnity.Combat.Data
{
	/// <summary>
	/// Success state (used for Battle Arena)
	/// </summary>
	public class SuccessState : PokemonEssentials.Interface.PokeBattle.ISuccessState
	{
		/// <summary>
		/// Type effectiveness
		/// </summary>
		public double TypeMod { get; set; }
		/// <summary>
		/// null - not used, 0 - failed, 1 - succeeded
		/// </summary>
		/// instead of an int or enum
		/// 0 - not used, 1 - failed, 2 - succeeded
		public bool? UseState { get; set; }
		public bool Protected { get; set; }
		public int Skill { get; private set; }

		public SuccessState()
		{
			initialize();
		}

		public PokemonEssentials.Interface.PokeBattle.ISuccessState initialize()
		{
			Clear();
			return this;
		}

		public void Clear()
		{
			TypeMod		= 4;
			UseState	= null;
			Protected	= false;
			Skill		= 0;
		}

		public void UpdateSkill()
		{
			if (UseState != true && !Protected)
				Skill -= 2;
			else if (UseState == true)
			{
				if (TypeMod > 4)
					Skill += 2; // "Super effective"
				else if (TypeMod >= 1 && TypeMod < 4)
					Skill -= 1; // "Not very effective"
				else if (TypeMod == 0)
					Skill -= 2; // Ineffective
				else
					Skill += 1;
			}
			TypeMod = 4;
			UseState = false;
			Protected = false;
		}
	}
}