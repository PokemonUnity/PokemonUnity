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
	public partial class Game : IGameSpriteWindow
	{
		// FadeOutIn(z) { block }
		/// <summary>
		/// Fades out the screen before a <paramref name="block"/> is run and fades it back in after the
		/// block exits.
		/// </summary>
		/// <param name="z">indicates the z-coordinate of the viewport used for this effect</param>
		/// <param name="nofadeout"></param>
		/// <param name="block"></param>
		public void FadeOutIn(int z, bool nofadeout= false, Action block = null)
		{
			IColor col = null; //new Color(0, 0, 0, 0);
			IViewport viewport = null; //new Viewport(0, 0, Graphics.width, Graphics.height);
			//viewport.z = z;
			for (int j = 0; j < 17; j++)
			{
				col.set(0, 0, 0, j * 15);
				viewport.color = col;
				Graphics?.update();
				Input.update();
			}
			PushFade();
			try { //begin;
				if (block != null) block.Invoke(); //(block_given ?) yield;
			} finally { //ensure;
				PopFade();
				if (!nofadeout)
				{
					for (int j = 0; j < 17; j++)
					{
						col.set(0, 0, 0, (17 - j) * 15);
						viewport.color = col;
						Graphics?.update();
						Input.update();
					}
				}
				viewport.Dispose();
			}
		}

		public void PushFade()
		{
			if (GameTemp != null)
			{
				GameTemp.fadestate = Math.Max(GameTemp.fadestate + 1, 0);
			}
		}

		public void PopFade()
		{
			if (GameTemp != null)
			{
				GameTemp.fadestate = Math.Max(GameTemp.fadestate - 1, 0);
			}
		}

		public virtual bool safeIsDirectory(string f)
		{
			throw new NotImplementedException();
		}

		public virtual bool safeExists(string f)
		{
			throw new NotImplementedException();
		}

		public virtual string[] safeGlob(string dir, string wildcard)
		{
			throw new NotImplementedException();
		}

		public virtual IBitmap GetTileBitmap(string filename, int tile_id, int hue)
		{
			throw new NotImplementedException();
		}

		public virtual IBitmap[] GetAnimation(string name, int hue = 0)
		{
			throw new NotImplementedException();
		}

		public virtual IBitmap[] GetTileset(string name, int hue = 0)
		{
			throw new NotImplementedException();
		}

		public virtual IBitmap[] GetAutotile(string name, int hue = 0)
		{
			throw new NotImplementedException();
		}

		public virtual void DrawShadow(IBitmap bitmap, float x, float y, int width, int height, string text)
		{
			throw new NotImplementedException();
		}

		public virtual void DrawShadowText(IBitmap bitmap, float x, float y, int width, int height, string text, IColor baseColor, IColor shadowColor = null, int align = 0)
		{
			throw new NotImplementedException();
		}

		public virtual void DrawOutlineText(IBitmap bitmap, float x, float y, int width, int height, string text, IColor baseColor, IColor shadowColor = null, int align = 0)
		{
			throw new NotImplementedException();
		}

		public virtual void BottomRight(IWindow window)
		{
			throw new NotImplementedException();
		}

		public virtual void BottomLeft(IWindow window)
		{
			throw new NotImplementedException();
		}

		public virtual void BottomLeftLines(IWindow window, int lines, int? width = null)
		{
			throw new NotImplementedException();
		}

		public virtual bool Disposed(IViewport x)
		{
			throw new NotImplementedException();
		}

		public virtual bool isDarkBackground(int background, IRect rect = null)
		{
			throw new NotImplementedException();
		}

		public virtual bool isDarkWindowskin(int windowskin)
		{
			throw new NotImplementedException();
		}

		public virtual IColor getDefaultTextColors(int windowskin)
		{
			throw new NotImplementedException();
		}

		public virtual IBitmap DoEnsureBitmap(IBitmap bitmap, int dwidth, int dheight)
		{
			throw new NotImplementedException();
		}

		public virtual void UpdateSpriteHash(IWindow[] windows)
		{
			throw new NotImplementedException();
		}

		public virtual void DisposeSpriteHash(IDictionary<string,ISprite> sprites)
		{
			throw new NotImplementedException();
		}

		public virtual void DisposeSprite(ISprite[] sprites, int id)
		{
			throw new NotImplementedException();
		}

		public virtual void DrawTextPositions(IBitmap bitmap, IList<ITextPosition> textpos)
		{
			throw new NotImplementedException();
		}

		public virtual void DrawImagePositions(IBitmap bitmap, IList<ITextPosition> textpos)
		{
			throw new NotImplementedException();
		}

		public virtual void FadeOutAndHide(IDictionary<string,ISprite> sprites)
		{
			throw new NotImplementedException();
		}

		public virtual void FadeInAndShow(IDictionary<string,ISprite> sprites, IList<ISprite> visiblesprites = null, Action block = null)
		{
			throw new NotImplementedException();
		}

		public virtual void RestoreActivations(ISprite[] sprites, object activeStatuses)
		{
			throw new NotImplementedException();
		}

		public virtual void DeactivateWindows(IWindow[] sprites, Action action = null)
		{
			throw new NotImplementedException();
		}

		public virtual void ActivateWindow(IWindow[] sprites, int key, Action action = null)
		{
			throw new NotImplementedException();
		}

		public virtual IColor AlphaBlend(IColor dstColor, IColor srcColor)
		{
			throw new NotImplementedException();
		}

		public virtual IColor SrcOver(IColor dstColor, IColor srcColor)
		{
			throw new NotImplementedException();
		}

		public virtual void SetSpritesToColor(ISprite[] sprites, IColor color)
		{
			throw new NotImplementedException();
		}

		public virtual string TryString(string x)
		{
			throw new NotImplementedException();
		}

		public virtual string BitmapName(string x)
		{
			throw new NotImplementedException();
		}

		public virtual string ResolveBitmap(string x)
		{
			throw new NotImplementedException();
		}

		public virtual void addBackgroundPlane(ISprite[] sprites, string planename, string background, IViewport viewport = null)
		{
			throw new NotImplementedException();
		}

		public virtual void addBackgroundOrColoredPlane(ISprite[] sprites, string planename, string background, IColor color, IViewport viewport = null)
		{
			throw new NotImplementedException();
		}

		public virtual void SetSystemFont(IBitmap bitmap)
		{
			throw new NotImplementedException();
		}

		public virtual string SmallFontName()
		{
			throw new NotImplementedException();
		}

		public virtual string NarrowFontName()
		{
			throw new NotImplementedException();
		}

		public virtual void SetSmallFont(IBitmap bitmap)
		{
			throw new NotImplementedException();
		}

		public virtual void SetNarrowFont(IBitmap bitmap)
		{
			throw new NotImplementedException();
		}

		public virtual bool RgssExists(string filename)
		{
			throw new NotImplementedException();
		}

		public virtual IDisposable RgssOpen(string file, int? mode = null, Action action = null)
		{
			throw new NotImplementedException();
		}

		public virtual string GetFileChar(string file)
		{
			throw new NotImplementedException();
		}

		public virtual string GetFileString(string file)
		{
			throw new NotImplementedException();
		}

		public bool IsFaded
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