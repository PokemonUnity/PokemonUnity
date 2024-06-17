using PokemonEssentials.Interface;
using System.Collections;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// </remarks>
	[RequireComponent(typeof(global::UnityEngine.RectTransform),
		typeof(global::UnityEngine.UI.Image))]
	public class Window : EssentialGameObject, ISpriteWindow_Base, ISpriteWindow, IWindow
	{
		#region Unity's MonoBehavior Inspector Properties
		protected global::UnityEngine.UI.Image background;
		#endregion

		#region Interface Private Variables
		protected IBitmap contents;
		protected bool pause;
		#endregion

		#region Interface Properties Implementation
		//public RectTransform Rect { get { return rect; } }
		public global::UnityEngine.UI.Image Background {  get { return background; } }

		bool IWindow.windowskin { get; set; }
		bool IWindow.active {
			get { return gameObject.activeInHierarchy; } //gameObject.activeSelf;
			set { gameObject.SetActive(value); }
		}
		bool IWindow.pause { get { return this.pause; } set { pause = value; } }
		byte IWindow.opacity { get; set; }
		byte IWindow.back_opacity { get; set; }
		byte IWindow.contents_opacity { get; set; }
		byte IWindow.openness { get; set; }
		IBitmap ISpriteWindow.contents { get { return this.contents; } set { contents = value; } }
		int ISpriteWindow.zoom_x { get; set; }
		int ISpriteWindow.zoom_y { get; set; }
		int ISpriteWindow.offset_x { get; set; }
		int ISpriteWindow.offset_y { get; set; }
		IRect ISpriteWindow.cursor_rect { get; set; }
		int ISpriteWindow.contents_blend_type { get; set; }
		#endregion

		#region Unity's MonoBehavior Method
		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			background = GetComponent<global::UnityEngine.UI.Image>();
		}
		#endregion

		#region Interface Method Implementation
		public ISpriteWindow_Base initialize(float x, float y, float width, float height)
		{
			return this;
		}
		#endregion

		#region Interface Method Implementation
		IWindow IWindow.initialize(IViewport viewport)
		{
			throw new System.NotImplementedException();
		}

		IEnumerator IWindow.update()
		{
			yield break;
		}

		ISpriteWindow ISpriteWindow.initialize(IViewport viewport)
		{
			//throw new System.NotImplementedException();
			this.initialize(viewport);
			return this;
		}

		void ISpriteWindow_Base.setSkin(int skin)
		{
			//throw new System.NotImplementedException();
		}

		void ISpriteWindow_Base.setSystemFrame()
		{
			//throw new System.NotImplementedException();
		}

		void ISpriteWindow_Base.__setWindowskin(int skin)
		{
			//throw new System.NotImplementedException();
		}

		void ISpriteWindow_Base.__resolveSystemFrame()
		{
			//throw new System.NotImplementedException();
		}
		#endregion
	}
}