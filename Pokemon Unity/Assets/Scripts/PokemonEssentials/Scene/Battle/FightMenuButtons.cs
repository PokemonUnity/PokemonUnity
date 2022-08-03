using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;

namespace PokemonUnity
{
	//[RequireComponent(typeof(Image))]
	public class FightMenuButtons : SpriteWrapper, IFightMenuButtons //BitmapSprite
	{
		const int UPPERGAP=46;
		public IAnimatedBitmap bitmap;
		public IAnimatedBitmap buttonbitmap;
		public IAnimatedBitmap typebitmap;
		public IAnimatedBitmap megaevobitmap;
		private bool disposedValue;
		private float x;
		private float y;

		public IFightMenuButtons initialize(int index= 0, IBattleMove[] moves = null, IViewport viewport= null)
		{
			//super((Game.GameData as Game).Graphics.width, 96 + UPPERGAP, viewport);
			//(this as IBitmapSprite).initialize((Game.GameData as Game).Graphics.width, 96 + UPPERGAP, viewport);
			/*this.x = 0;
			this.y = (Game.GameData as Game).Graphics.height - 96 - UPPERGAP;
			pbSetNarrowFont(this.bitmap);*/
			//@buttonbitmap = new AnimatedBitmap("Graphics/Pictures/battleFightButtons");
			//@typebitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/types"));
			//@megaevobitmap = new AnimatedBitmap(Game._INTL("Graphics/Pictures/battleMegaEvo"));
			@buttonbitmap.initialize("Graphics/Pictures/battleFightButtons");
			@typebitmap.initialize(Game._INTL("Graphics/Pictures/types"));
			@megaevobitmap.initialize(Game._INTL("Graphics/Pictures/battleMegaEvo"));
			refresh(index, moves, 0);
			return this;
		}

		public override void Dispose()
		{
			@buttonbitmap.dispose();
			@typebitmap.dispose();
			@megaevobitmap.dispose();
			base.Dispose();//super;
		}

		public void update(int index= 0, IBattleMove[] moves = null, int megaButton= 0)
		{
			refresh(index, moves, megaButton);
		}

		public void refresh(int index, IBattleMove[] moves, int megaButton)
		{
			/*if (moves == null) return;
			this.bitmap.clear();
			string moveboxes = Game._INTL("Graphics/Pictures/battleFightButtons");
			IList<IEnumOption> textpos = new List<IEnumOption>();
			for (int i = 0; i < 4; i++)
			{
				if (i == index) continue;
				if (moves[i].id == 0) continue;
				x = ((i % 2) == 0) ? 4 : 192;
				y = ((i / 2) == 0) ? 6 : 48;
				y += UPPERGAP;
				this.bitmap.blt(x, y, @buttonbitmap.bitmap, new Rect(0, moves[i].Type * 46, 192, 46));
				textpos.Add(new { Game._INTL("{1}", moves[i].Name), x + 96, y + 8, 2,
				   PokeBattle_SceneConstants.MENUBASECOLOR, PokeBattle_SceneConstants.MENUSHADOWCOLOR});
			}
			IColor[] ppcolors=new IColor[] {
			   PokeBattle_SceneConstants.PPTEXTBASECOLOR, PokeBattle_SceneConstants.PPTEXTSHADOWCOLOR,
			   PokeBattle_SceneConstants.PPTEXTBASECOLOR, PokeBattle_SceneConstants.PPTEXTSHADOWCOLOR,
			   PokeBattle_SceneConstants.PPTEXTBASECOLORYELLOW, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORYELLOW,
			   PokeBattle_SceneConstants.PPTEXTBASECOLORORANGE, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORORANGE,
			   PokeBattle_SceneConstants.PPTEXTBASECOLORRED, PokeBattle_SceneConstants.PPTEXTSHADOWCOLORRED
			};
			for (int i = 0; i < 4; i++) {
				if (i!=index) continue;
				if (moves[i].id==0) continue;
				x=((i%2)==0) ? 4 : 192;
				y=((i/2)==0) ? 6 : 48;
				y+=UPPERGAP;
				this.bitmap.blt(x, y, @buttonbitmap.bitmap,new Rect(192, moves[i].Type*46,192,46));
				this.bitmap.blt(416,20+UPPERGAP,@typebitmap.bitmap,new Rect(0, moves[i].Type*28,64,28));
				textpos.Add(new { Game._INTL("{1}", moves[i].name), x + 96, y + 8, 2,
					PokeBattle_SceneConstants.MENUBASECOLOR, PokeBattle_SceneConstants.MENUSHADOWCOLOR});
				if (moves[i].TotalPP>0) {
				int ppfraction=(int)Math.Ceiling(4.0*moves[i].PP/moves[i].TotalPP);
				textpos.Add(new { Game._INTL("PP: {1}/{2}", moves[i].PP, moves[i].TotalPP),
					448, 50 + UPPERGAP, 2, ppcolors[(4 - ppfraction) * 2], ppcolors[(4 - ppfraction) * 2 + 1] });
				}
			}
			pbDrawTextPositions(this.bitmap, textpos);
			if (megaButton > 0)
			{
				this.bitmap.blt(146, 0, @megaevobitmap.bitmap, new Rect(0, (megaButton - 1) * 46, 96, 46));
			}*/
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects)
					Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override finalizer
				// TODO: set large fields to null
				disposedValue = true;
			}
		}

		// // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
		// ~FightMenuDisplay()
		// {
		//     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
		//     Dispose(disposing: false);
		// }

		void IDisposable.Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		#region 
		IBitmapSprite IBitmapSprite.initialize(int width, int height, IViewport viewport)
		{
			//throw new NotImplementedException();
			return this;
		}

		/*ISpriteWrapper ISpriteWrapper.initialize(IViewport viewport)
		{
			throw new NotImplementedException();
		}

		void ISpriteWrapper.flash(IColor color, int duration)
		{
			throw new NotImplementedException();
		}

		void ISpriteWrapper.update()
		{
			throw new NotImplementedException();
		}

		void ISprite.whiten()
		{
			throw new NotImplementedException();
		}

		void ISprite.appear()
		{
			throw new NotImplementedException();
		}

		void ISprite.escape()
		{
			throw new NotImplementedException();
		}

		void ISprite.collapse()
		{
			throw new NotImplementedException();
		}

		void ISprite.damage(int value, bool critical)
		{
			throw new NotImplementedException();
		}

		void ISprite.dispose_damage()
		{
			throw new NotImplementedException();
		}

		void ISprite.dispose_animation()
		{
			throw new NotImplementedException();
		}

		void ISprite.dispose_loop_animation()
		{
			throw new NotImplementedException();
		}

		void ISprite.blink_on()
		{
			throw new NotImplementedException();
		}

		void ISprite.blink_off()
		{
			throw new NotImplementedException();
		}

		void ISprite.update_animation()
		{
			throw new NotImplementedException();
		}

		void ISprite.update_loop_animation()
		{
			throw new NotImplementedException();
		}

		void IRPGSprite.update()
		{
			throw new NotImplementedException();
		}

		void IRPGSprite.flash(IColor color, int duration)
		{
			throw new NotImplementedException();
		}

		IRPGSprite IRPGSprite.initialize(IViewport viewport)
		{
			throw new NotImplementedException();
		}*/
		#endregion
	}
}