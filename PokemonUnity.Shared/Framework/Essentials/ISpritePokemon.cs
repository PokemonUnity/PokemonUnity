using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;

namespace PokemonEssentials.Interface
{
	public interface ISpritePicture : IDisposable
	{
		ISpritePicture initialize(IViewport viewport, ISprite picture);

		void dispose();

		IEnumerator update();
	}
}