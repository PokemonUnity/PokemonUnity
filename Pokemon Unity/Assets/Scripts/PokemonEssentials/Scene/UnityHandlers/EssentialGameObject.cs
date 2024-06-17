using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Interface;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Universal Base Class for all GameObjects in Unity to Utilize Pokemon Essentials Interfaces
	/// </summary>
	[RequireComponent(typeof(global::UnityEngine.RectTransform))]//,typeof(global::UnityEngine.CanvasGroup),typeof(global::UnityEngine.UI.Image)
	public class EssentialGameObject : MonoBehaviour, IRPGSprite, ISprite, ISpriteWrapper
	{
		[SerializeField] protected global::UnityEngine.CanvasGroup canvasGroup = null;
		[SerializeField] protected global::UnityEngine.RectTransform rect = null;
		[SerializeField] protected global::UnityEngine.UI.Image image = null;
		[SerializeField] protected global::UnityEngine.Canvas canvas = null;
		/// <summary>
		/// The minimum scale of the RectTransform Target
		/// </summary>
		[SerializeField] protected float MinimumScale = 0.5f;
		/// <summary>
		/// The maximum scale of the RectTransform Target
		/// </summary>
		[SerializeField] protected float MaximumScale = 3f;
		/// <summary>
		/// Current scale which is used to keep track whether it is within boundaries
		/// </summary>
		private float scalex = 1f;
		/// <summary>
		/// Current scale which is used to keep track whether it is within boundaries
		/// </summary>
		private float scaley = 1f;

		#region Interface Field Variables
		protected IRect _rect;
		protected IBitmap _bitmap;
		protected IViewport _viewport;
		#endregion

		#region Interface Properties Implementation
		/// <summary>
		/// Refers to the bitmap (Bitmap) used for the sprite's starting point.
		/// </summary>
		/// <remarks>
		/// Uses <see cref="global::UnityEngine.UI.Image"/> component as a wrapper for Bitmap, and assigns sprite to GameObject.
		/// </remarks>
		public virtual IBitmap bitmap {
			//get { return image.sprite as IBitmap; }
			get {
				if (gameObject.GetComponent<global::UnityEngine.UI.Image>() == null)
				{
					image = gameObject.AddComponent<global::UnityEngine.UI.Image>();
				}
				return _bitmap;
			}
			//set { image.sprite = value as global::UnityEngine.Sprite; }
			set {
				//image.sprite = value as global::UnityEngine.Sprite;
				image.sprite = ((object)value as global::UnityEngine.GameObject).GetComponent<global::UnityEngine.UI.Image>().sprite;
				_bitmap = value;
			}
		}
		/// <summary>
		/// The sprite's angle of rotation. Specifies up to 360 degrees of counterclockwise rotation.
		/// </summary>
		public virtual float angle { get; set; }
		/// <summary>
		/// The sprite's blending mode (0: normal, 1: addition, 2: subtraction).
		/// </summary>
		public virtual int blend_type { get; set; }
		/// <summary>
		/// This can be used to represent something like characters' legs being hidden by bushes.
		/// For bush_depth, the number of pixels for the bush section is specified.The default value is 0.
		/// </summary>
		public virtual int bush_depth { get; set; }
		/// <summary>
		/// The color (Color) to be blended with the sprite. Alpha values are used in the blending ratio.
		/// </summary>
		public virtual IColor color { get; set; }
		/// <summary>
		/// Returns TRUE if the sprite has been freed.
		/// </summary>
		public virtual bool disposed { get; private set; }
		/// <summary>
		/// Flag denoting the sprite has been flipped horizontally. If TRUE, the sprite will be drawn flipped.
		/// </summary>
		public virtual bool mirror
		{
			get { return rect.localScale.x > 0; }
			set { rect.localPosition = new Vector3(value ? Math.Abs(rect.localPosition.x) : Math.Abs(rect.localPosition.x) * -1, rect.localPosition.y, rect.localPosition.z); }
		}
		/// <summary>
		/// The sprite's opacity (0-255). Values out of range are automatically corrected.
		/// </summary>
		public virtual float opacity {
			get { return canvasGroup.alpha; }
			set { canvasGroup.alpha = value; }
		}
		/// <summary>
		/// The X-coordinate of the sprite's starting point.
		/// </summary>
		public virtual float ox
		{
			get { return rect.localPosition.x; }
			set { rect.localPosition = new Vector3(value, rect.localPosition.y, rect.localPosition.z); }
		}
		/// <summary>
		/// The Y-coordinate of the sprite's starting point.
		/// </summary>
		public virtual float oy {
			get { return rect.localPosition.y; }
			set { rect.localPosition = new Vector3(rect.localPosition.x, value, rect.localPosition.z); }
		}
		public virtual IRect src_rect
		{
			get { return _rect; }
			set
			{
				//rect = ((object)value as global::UnityEngine.GameObject).GetComponent<global::UnityEngine.RectTransform>();
				rect.rect.Set(value.x, value.y, value.width, value.height);
				_rect = value;
			}
		}
		public virtual ITone tone { get; set; }
		/// <summary>
		/// Refers to the viewport (<see cref="global::UnityEgine.Canvas"/>) associated with the sprite.
		/// </summary>
		public virtual IViewport viewport
		{
			get {
				if (gameObject.GetComponent<Canvas>() == null)
				{
					canvas = gameObject.AddComponent<Canvas>();
				}
				return _viewport;
			}
			set
			{
				gameObject.transform.SetParent(((object)value as global::UnityEngine.GameObject).transform, true); //transform.parent = ((object)value as global::UnityEngine.GameObject).transform;
				canvas = ((object)value as global::UnityEngine.GameObject).GetComponent<global::UnityEngine.Canvas>();
				_viewport = value;
			}
		}
		/// <summary>
		/// The sprite's visibility. If TRUE, the sprite is visible.
		/// </summary>
		public virtual bool visible {
			get { return gameObject.activeInHierarchy; } //gameObject.activeSelf;
			set { gameObject.SetActive(value); }
		}
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		public virtual float x {
			get { return rect.position.x; }
			set { rect.position = new Vector3(value, rect.position.y, rect.position.z); }
		}
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		public virtual float y {
			get { return rect.position.y; }
			set { rect.position = new Vector3(rect.position.x, value, rect.position.z); }
		}
		/// <summary>
		/// The sprite's Z-coordinate. The larger this value, the closer to the player the sprite will be displayed.
		/// </summary>
		public virtual float z {
			get { return canvas.sortingOrder; }
			set { canvas.sortingOrder = (int)value; }
		}
		/// <summary>
		/// The sprite's X-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		public virtual float zoom_x {
			get { return rect.localScale.x; }
			set {
				float newScale = Mathf.Clamp(value, MinimumScale, MaximumScale);
				// If the scale did not change, don't do anything
				if (newScale.Equals(scalex)) return;
				// Set the new scale
				scalex = newScale;
				// Apply the new scale
				rect.localScale = new Vector3(scalex, rect.localScale.y); }
		}
		/// <summary>
		/// The sprite's Y-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		public virtual float zoom_y {
			get { return rect.localScale.y; }
			set {
				float newScale = Mathf.Clamp(value, MinimumScale, MaximumScale);
				// If the scale did not change, don't do anything
				if (newScale.Equals(scaley)) return;
				// Set the new scale
				scaley = newScale;
				// Apply the new scale
				rect.localScale = new Vector3(rect.localScale.x, scaley, rect.localScale.z); }
		}
		/// <summary>
		/// Gets the width of the sprite. Equivalent to src_rect.width.
		/// </summary>
		public virtual float width {
			get { return rect.rect.width; } //rect.sizeDelta.x
			//set { rect.rect.width = value; }
			set { rect.sizeDelta = new Vector2(value, rect.sizeDelta.y); }
		}
		/// <summary>
		/// Gets the height of the sprite. Equivalent to src_rect.height.
		/// </summary>
		public virtual float height {
			get { return rect.rect.height; } //rect.sizeDelta.y
			//set { rect.rect.height = value; }
			set { rect.sizeDelta = new Vector2(rect.sizeDelta.x, value); }
		}

		/// <summary>
		/// The sprite's visibility. If TRUE, the sprite is visible.
		/// </summary>
		public virtual bool blink { get; set; }
		public virtual bool effect { get; set; }
		#endregion

		#region Unity's MonoBehavior Method
		private void Awake()
		{
			rect = GetComponent<global::UnityEngine.RectTransform>();
			//image = GetComponent<UnityEngine.UI.Image>();
			//canvasGroup = GetComponent<global::UnityEngine.CanvasGroup>();
			GameManager.current.onUpdate += delegate { this.update(); };
		}

		private void Start()
		{
			//if (m_CharEntryInstanceRoot == null)
			//{
			//	//Log.Error("You must set HP bar instance root first.");
			//	return;
			//}
			//
			//m_CachedCanvas = m_CharEntryInstanceRoot.GetComponent<Canvas>();
		}

		private void OnDestroy()
		{
			GameManager.current.onUpdate -= delegate { this.update(); };
		}

		private void Update()
		{
		}
		#endregion

		#region Interface Method Implementation
		//public virtual ISprite initialize(IViewport viewport = null);
		public virtual ISpriteWrapper initialize(IViewport viewport = null)
		{
			return this;
		}
		IRPGSprite IRPGSprite.initialize(IViewport viewport)
		{
			return this.initialize(viewport);
		}
		/// <summary>
		/// Begins flashing the sprite. duration specifies the number of frames the flash will last.
		/// If color is set to nil, the sprite will disappear while flashing.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		public virtual void whiten()
		{
		}

		public virtual void appear()
		{
		}

		public virtual void escape()
		{
		}

		public virtual void collapse()
		{
		}

		public virtual void damage(int value, bool critical)
		{
		}

		//public virtual void pushAnimation(List<> array, IAnimation anim);
		//public virtual void animation(animation, hit);
		//public virtual void loop_animation(animation);
		public virtual void dispose_damage()
		{
		}

		public virtual void dispose_animation()
		{
		}

		public virtual void dispose_loop_animation()
		{
		}

		public virtual void blink_on()
		{
		}

		public virtual void blink_off()
		{
		}

		//public virtual IEnumerator update();
		public virtual void update_animation()
		{
		}

		public virtual void update_loop_animation()
		{
		}

		/// <summary>
		/// If a flash or wave is not needed, it is not necessary to call this.
		/// </summary>
		/// <returns></returns>
		public virtual void update()
		{
			//yield return null;
		}
		/// <summary>
		/// Begins flashing the sprite. duration specifies the number of frames the flash will last.
		/// If color is set to nil, the sprite will disappear while flashing.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		public virtual void flash(IColor color, int duration)
		{

		}

		/// <summary>
		/// Begins flashing the sprite. duration specifies the number of frames the flash will last.
		/// If color is set to nil, the sprite will disappear while flashing.
		/// </summary>
		/// <param name="sprite"></param>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		public virtual void flash(global::UnityEngine.UI.Image sprite, IColor color, int duration)
		{

		}
		#endregion

		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		//void dispose();
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					//base.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposed = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~RPGSprite()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		public void Dispose()
		{
			Core.Logger.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
