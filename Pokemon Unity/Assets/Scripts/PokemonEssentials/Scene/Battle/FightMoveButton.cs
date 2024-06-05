using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Used to display the moves in the FIGHT menu
	/// </summary>
	/// <remarks>
	/// See <see cref="FightMenuDisplay"/> for the menu class
	/// </remarks>
	public class FightMoveButton : MonoBehaviour
	{
		/// <summary>
		/// Name of the move
		/// </summary>
		//public global::UnityEngine.UI.Text Move;
		//public global::UnityEngine.UI.Text PP;
		public TMPro.TextMeshProUGUI Move;
		public TMPro.TextMeshProUGUI PP;
		public global::UnityEngine.UI.Image Type;
		public global::UnityEngine.UI.Image ButtonBG;
	}
}