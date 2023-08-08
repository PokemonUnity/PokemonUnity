using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonEssentials.Interface
{
	public interface IBushBitmap
	{
		//BushBitmap(Bitmap bitmap, bool isTile, int depth);

		void dispose();

		IBitmap bitmap { get; set; }
	}

	public interface ISpriteCharacter : ISprite
	{

		ISpriteCharacter initialize(IViewport viewport, IGameCharacter character = null);

		int groundY { get; }

		IBitmap BushDepthBitmap(IBitmap bitmap, int depth);

		IBitmap BushDepthTile(IBitmap bitmap, int depth);

		//void dispose();

		//IEnumerator update();
	}
}