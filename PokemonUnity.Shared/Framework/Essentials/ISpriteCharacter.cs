using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface
{
	public interface ISprite : IDisposable, IRPGSprite
	{
		/// <summary>
		/// The sprite's visibility. If TRUE, the sprite is visible.
		/// </summary>
		bool blink { get; set; }
		bool effect { get; set; }
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		float y { get; set; }
		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		void dispose();
		/// <summary>
		/// Begins flashing the sprite. duration specifies the number of frames the flash will last.
		/// If color is set to nil, the sprite will disappear while flashing.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="duration"></param>
		void whiten();
		void appear();
		void escape();
		void collapse();
		void damage(int value, bool critical);
		//void pushAnimation(List<> array, IAnimation anim);
		//void animation(animation, hit);
		//void loop_animation(animation);
		void dispose_damage();
		void dispose_animation();
		void dispose_loop_animation();
		void blink_on();
		void blink_off();
		IEnumerator update();
		void update_animation();
		void update_loop_animation();
		ISprite initialize(IViewport viewport = null);
	}

	public interface ISpriteAnimation : IDisposable
	{
		
		/// <summary>
		/// The sprite's X-coordinate.
		/// </summary>
		float x { get; set; }
		/// <summary>
		/// The sprite's Y-coordinate.
		/// </summary>
		float y { get; set; }
		bool active { get; set; }
		bool effect { get; set; }
		/// <summary>
		/// </summary>
		void clear();
		/// <summary>
		/// Frees the sprite. If the sprite has already been freed, does nothing.
		/// </summary>
		void dispose();
		/// <summary>
		/// </summary>
		/// <param name="animation"></param>
		/// <param name="hit"></param>
		void animation(IColor animation, int hit);
		/// <summary>
		/// </summary>
		/// <param name="animation"></param>
		void loop_animation(IColor animation);
		void dispose_animation();
		void dispose_loop_animation();
		ISpriteAnimation initialize(ISprite sprite);
		IEnumerator update();
		void update_animation();
		void update_loop_animation();
		//void animation_set_sprites(ISprite[] sprites, cell_data, int position);
		//void animation_process_timing(timing, hit);
	}
}