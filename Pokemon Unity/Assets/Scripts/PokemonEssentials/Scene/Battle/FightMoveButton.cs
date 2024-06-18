using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Used to display the moves in the FIGHT menu
	/// </summary>
	/// <remarks>
	/// See <see cref="FightMenuDisplay"/> for the menu class
	/// </remarks>
	[RequireComponent(typeof(global::UnityEngine.RectTransform),
		typeof(global::UnityEngine.UI.Image))]
	public class FightMoveButton : MonoBehaviour
	{
		private RectTransform rect;
		/// <summary>
		/// Name of the move
		/// </summary>
		//public global::UnityEngine.UI.Text Move;
		//public global::UnityEngine.UI.Text PP;
		public TMPro.TextMeshProUGUI Move;
		public TMPro.TextMeshProUGUI PP;
		public global::UnityEngine.UI.Image Type;
		public global::UnityEngine.UI.Image ButtonBG;

		private void Awake()
		{
			if (rect == null) rect = GetComponent<RectTransform>();
			if (ButtonBG == null) ButtonBG = GetComponent<global::UnityEngine.UI.Image>();
		}
	}
}