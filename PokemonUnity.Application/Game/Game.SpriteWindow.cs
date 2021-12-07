using System;
using System.Collections;
using System.Collections.Generic;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity
{
	public partial class Game //: IGameSpriteWindow
	{
		// pbFadeOutIn(z) { block }
		/// <summary>
		/// Fades out the screen before a <paramref name="block"/> is run and fades it back in after the
		/// block exits.
		/// </summary>
		/// <param name="z">indicates the z-coordinate of the viewport used for this effect</param>
		/// <param name="nofadeout"></param>
		/// <param name="block"></param>
		public void pbFadeOutIn(int z, bool nofadeout= false, Action block = null)
		{
			//IColor col = new Color(0, 0, 0, 0);
			//IViewport viewport = new Viewport(0, 0, Graphics.width, Graphics.height);
			//viewport.z = z;
			//for (int j = 0; j < 17; j++)
			//{
			//	col.set(0, 0, 0, j * 15);
			//	viewport.color = col;
			//	Graphics.update();
			//	Input.update();
			//}
			pbPushFade();
			try { //begin;
				if (block != null) block.Invoke(); //(block_given ?) yield;
			} finally { //ensure;
				pbPopFade();
				if (!nofadeout)
				{
					//for (int j = 0; j < 17; j++)
					//{
					//	col.set(0, 0, 0, (17 - j) * 15);
					//	viewport.color = col;
					//	Graphics.update();
					//	Input.update();
					//}
				}
				//viewport.dispose();
			}
		}

		public void pbPushFade()
		{
			if (GameTemp != null)
			{
				GameTemp.fadestate = Math.Max(GameTemp.fadestate + 1, 0);
			}
		}

		public void pbPopFade()
		{
			if (GameTemp != null)
			{
				GameTemp.fadestate = Math.Max(GameTemp.fadestate - 1, 0);
			}
		}

		public bool pbIsFaded
		{
			get
			{
				if (GameTemp != null)
				{
					return GameTemp.fadestate > 0;
				}
				else
				{
					return false;
				}
			}
		}
	}
}