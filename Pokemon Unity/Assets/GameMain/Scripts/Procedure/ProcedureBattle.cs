using System.Collections;
using UnityEngine;

namespace PokemonUnity
{
	public class ProcedureBattle : ProcedureBase
	{
		public PokemonUnity.Combat.Battle Battle;
		
		public override bool UseNativeDialog
		{
			get { return false; }
		}
	}
}
