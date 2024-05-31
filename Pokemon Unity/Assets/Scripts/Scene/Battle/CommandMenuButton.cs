using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Used to display the command options in the command menu
	/// </summary>
	/// <remarks>
	/// See <see cref="CommandMenuDisplay"/> for the menu class
	/// </remarks>
	[RequireComponent(typeof(global::UnityEngine.RectTransform),
		typeof(global::UnityEngine.UI.Image))]
	public class CommandMenuButton : MonoBehaviour
	{
		public RectTransform Rect;
		/// <summary>
		/// Name of the move
		/// </summary>
		public TMPro.TextMeshProUGUI Text;
		public global::UnityEngine.UI.Image Logo;
		public global::UnityEngine.UI.Image Background;

		private void Awake()
		{
			Rect = GetComponent<RectTransform>();
			Background = GetComponent<global::UnityEngine.UI.Image>();
		}
	}
}