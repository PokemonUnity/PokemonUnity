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

		IBitmap pbBushDepthBitmap(IBitmap bitmap, int depth);

		IBitmap pbBushDepthTile(IBitmap bitmap, int depth);

		//void dispose();

		//IEnumerator update();
	}
}