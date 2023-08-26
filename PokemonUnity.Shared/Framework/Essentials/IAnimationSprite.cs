using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface
{
	/// <summary>
	/// A sprite whose sole purpose is to display an animation. This sprite
	/// can be displayed anywhere on the map and is disposed
	/// automatically when its animation is finished.
	/// Used for grass rustling and so forth.
	/// </summary>
	public interface IAnimationSprite : ISprite //RPGMaker.Kernal.ISprite 
	{
		void initialize(int animID, IGameMap map, float tileX, float tileY, IViewport viewport = null, bool tinting = false);

		//void dispose();

		//void update();
	}



	public partial interface ISpritesetMapAnimation
	{
		//alias _animationSprite_initialize initialize;
		//alias _animationSprite_update update;
		//alias _animationSprite_dispose dispose;

		ISpritesetMapAnimation initialize(IGameMap map= null);

		IAnimationSprite addUserAnimation(int animID, float x, float y, bool tinting = false);

		void addUserSprite(ISprite sprite);

		void dispose();

		void update();
	}
}