//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.UX;
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
using GameFramework.ObjectPool;
//using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public class SpriteWrapper : Sprite, ISpriteWrapper
	{
		public IBitmap bitmap { get; set; }
		//public float angle { get; set; }
		//public int blend_type { get; set; }
		//public int bush_depth { get; set; }
		//public IColor color { get; set; }
		//public bool disposed { get; }
		//public bool mirror { get; set; }
		//public float opacity { get; set; }
		//public float ox { get; set; }
		//public float oy { get; set; }
		//public IRect src_rect { get; set; }
		//public ITone tone { get; set; }
		//public IViewport viewport { get; set; }
		//public bool visible { get; set; }
		//public float x { get; set; }
		//public float y { get; set; }
		//public float z { get; set; }
		//public float zoom_x { get; set; }
		//public float zoom_y { get; set; }

		new public ISpriteWrapper initialize(IViewport viewport = null)
		{
			return this;
		}
		//public void Dispose()
		//{
		//
		//}
		//public void flash(IColor color, int duration)
		//{
		//
		//}
		//public override void update()
		//{
		//
		//}
	}
	
	public class Sprite : RPGSprite, ISprite
	{
		/// <summary>
		/// The sprite's visibility. If TRUE, the sprite is visible.
		/// </summary>
		public virtual bool blink { get; set; }
		public virtual bool effect { get; set; }
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		//float x { get; set; }
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		//float y { get; set; }
		//public ISprite initialize(IViewport viewport = null);
		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		//void dispose();
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
		//public void pushAnimation(List<> array, IAnimation anim);
		//public void animation(animation, hit);
		//public void loop_animation(animation);
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
		//public IEnumerator update();
		public virtual void update_animation()
		{

		}
		public virtual void update_loop_animation()
		{

		}
	}

	[RequireComponent(typeof(UnityEngine.RectTransform),typeof(UnityEngine.UI.Image))]
	public abstract class RPGSprite : MonoBehaviour, IRPGSprite
	{
		[SerializeField]
		private UnityEngine.RectTransform rect = null;

		[SerializeField]
		private UnityEngine.UI.Image image = null;

		public Canvas m_CachedCanvas = null;

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

		}

		private void Update()
		{
		}


		/// <summary>
		/// The sprite's angle of rotation. Specifies up to 360 degrees of counterclockwise rotation.
		/// </summary>
		public float angle { get; set; }
		/// <summary>
		/// Refers to the bitmap (Bitmap) used for the sprite's starting point.
		/// </summary>
		//public IBitmap bitmap { get; set; }
		/// <summary>
		/// The sprite's blending mode (0: normal, 1: addition, 2: subtraction).
		/// </summary>
		public int blend_type { get; set; }
		/// <summary>
		/// This can be used to represent something like characters' legs being hidden by bushes.
		/// For bush_depth, the number of pixels for the bush section is specified.The default value is 0.
		/// </summary>
		public int bush_depth { get; set; }
		/// <summary>
		/// The color (Color) to be blended with the sprite. Alpha values are used in the blending ratio.
		/// </summary>
		public IColor color { get; set; }
		/// <summary>
		/// Returns TRUE if the sprite has been freed.
		/// </summary>
		public bool disposed { get; }
		/// <summary>
		/// Flag denoting the sprite has been flipped horizontally. If TRUE, the sprite will be drawn flipped.
		/// </summary>
		public bool mirror { get; set; }
		/// <summary>
		/// The sprite's opacity (0-255). Values out of range are automatically corrected.
		/// </summary>
		public float opacity { get; set; }
		/// <summary>
		/// The X-coordinate of the sprite's starting point.
		/// </summary>
		public float ox { get; set; }
		/// <summary>
		/// The Y-coordinate of the sprite's starting point.
		/// </summary>
		public float oy { get; set; }
		public IRect src_rect { get; set; }
		public ITone tone { get; set; }
		/// <summary>
		/// Refers to the viewport (Viewport) associated with the sprite.
		/// </summary>
		public IViewport viewport { get; set; }
		/// <summary>
		/// The sprite's visibility. If TRUE, the sprite is visible.
		/// </summary>
		public bool visible { get; set; }
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		public float x { get; set; }
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		public float y { get; set; }
		/// <summary>
		/// The sprite's Z-coordinate. The larger this value, the closer to the player the sprite will be displayed.
		/// </summary>
		public float z { get; set; }
		/// <summary>
		/// The sprite's X-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		public float zoom_x { get; set; }
		/// <summary>
		/// The sprite's Y-axis zoom level. 1.0 denotes actual pixel size.
		/// </summary>
		public float zoom_y { get; set; }
		/// <summary>
		/// Gets the width of the sprite. Equivalent to src_rect.width.
		/// </summary>
		public float width { get; set; }
		/// <summary>
		/// Gets the height of the sprite. Equivalent to src_rect.height.
		/// </summary>
		public float height { get; set; }
		/// <summary>
		/// If a flash or wave is not needed, it is not necessary to call this.
		/// </summary>
		/// <returns></returns>
		public virtual void update()
		{
			//yield return null;
		}
		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		public virtual void Dispose()
		{

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

		public IRPGSprite initialize(IViewport viewport = null)
		{
			return this;
		}
	}
}
