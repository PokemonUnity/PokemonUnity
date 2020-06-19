using System.Collections;
using UnityEngine;

namespace PokemonUnity
{
	//Typing State Opens a UI Typing Form
	public class ProcedureTyping : ProcedureBase
	{
		//bool GameEntry.IsCapLockOn = false;
		private bool UseKeyboard = true;

		public override bool UseNativeDialog
		{
			get { return false; }
		}
	}
}
