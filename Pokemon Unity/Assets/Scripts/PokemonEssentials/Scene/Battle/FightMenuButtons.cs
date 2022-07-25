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
	public class FightMenuButtons : MonoBehaviour, IFightMenuButtons //BitmapSprite
	{
		const int UPPERGAP=46;
		public IAnimatedBitmap bitmap;
		public IAnimatedBitmap buttonbitmap;
		public IAnimatedBitmap typebitmap;
		public IAnimatedBitmap megaevobitmap;
		private bool disposedValue;
		private float x;
		private float y;

		#region Need to Implement using Unity Engine
		IBitmap IBitmapSprite.bitmap { set => throw new NotImplementedException(); }
		float ISpriteWrapper.angle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IBitmap ISpriteWrapper.bitmap { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		int ISpriteWrapper.blend_type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		int ISpriteWrapper.bush_depth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IColor ISpriteWrapper.color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool ISpriteWrapper.disposed => throw new NotImplementedException();

		bool ISpriteWrapper.mirror { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.opacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.ox { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.oy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IRect ISpriteWrapper.src_rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		ITone ISpriteWrapper.tone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IViewport ISpriteWrapper.viewport { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		bool ISpriteWrapper.visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.x { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.z { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.zoom_x { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float ISpriteWrapper.zoom_y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		bool ISprite.blink { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		bool ISprite.effect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.angle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		int IRPGSprite.blend_type { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		int IRPGSprite.bush_depth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IColor IRPGSprite.color { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		bool IRPGSprite.disposed => throw new NotImplementedException();

		bool IRPGSprite.mirror { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.opacity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.ox { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.oy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IRect IRPGSprite.src_rect { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		ITone IRPGSprite.tone { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		IViewport IRPGSprite.viewport { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		bool IRPGSprite.visible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.x { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.z { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.zoom_x { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.zoom_y { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		float IRPGSprite.height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		#endregion

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

		public void dispose()
		{
			@buttonbitmap.dispose();
			@typebitmap.dispose();
			@megaevobitmap.dispose();
			//super;
		}

		public void update(int index= 0, IBattleMove[] moves = null, int megaButton= 0)
		{
			refresh(index, moves, megaButton);
		}

		public void refresh(int index, IBattleMove[] moves, int megaButton)
		{
			/*
			if (moves == null) return;
			this.bitmap.clear();
			string moveboxes = Game._INTL("Graphics/Pictures/battleFightButtons");
			IList<> textpos = new List<>();
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
			   PokeBattle_SceneConstants.HPTEXTBASECOLOR, PokeBattle_SceneConstants.HPTEXTSHADOWCOLOR,
			   PokeBattle_SceneConstants.HPTEXTBASECOLORYELLOW, PokeBattle_SceneConstants.HPTEXTSHADOWCOLORYELLOW,
			   PokeBattle_SceneConstants.HPTEXTBASECOLORORANGE, PokeBattle_SceneConstants.HPTEXTSHADOWCOLORORANGE,
			   PokeBattle_SceneConstants.HPTEXTBASECOLORRED, PokeBattle_SceneConstants.HPTEXTSHADOWCOLORRED
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
				int ppfraction=Math.Ceiling(4.0*moves[i].PP/moves[i].TotalPP);
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
					dispose();
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
			throw new NotImplementedException();
		}

		ISpriteWrapper ISpriteWrapper.initialize(IViewport viewport)
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

		IEnumerator IRPGSprite.update()
		{
			throw new NotImplementedException();
		}

		void IRPGSprite.flash(IColor color, int duration)
		{
			throw new NotImplementedException();
		}

		ISprite IRPGSprite.initialize(IViewport viewport)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}