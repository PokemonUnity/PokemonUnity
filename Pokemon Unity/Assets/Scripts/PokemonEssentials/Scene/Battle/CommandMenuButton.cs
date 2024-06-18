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
		private RectTransform rect;
		private global::UnityEngine.UI.Image background;
		/// <summary>
		/// Name of the move
		/// </summary>
		public TMPro.TextMeshProUGUI Text;
		public global::UnityEngine.UI.Image Logo;
		public RectTransform Rect {  get { return rect; } }
		public global::UnityEngine.UI.Image Background {  get { return background; } }

		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			background = GetComponent<global::UnityEngine.UI.Image>();
			if (Logo == null) Logo = GetComponentInChildren<global::UnityEngine.UI.Image>();
			if (Text == null) Text = GetComponentInChildren<global::TMPro.TextMeshProUGUI>();
		}
	}
}