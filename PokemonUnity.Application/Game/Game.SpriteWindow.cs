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
            IColor col = null; //new Color(0, 0, 0, 0);
            IViewport viewport = null; //new Viewport(0, 0, Graphics.width, Graphics.height);
			//viewport.z = z;
			for (int j = 0; j < 17; j++)
			{
				col.set(0, 0, 0, j * 15);
				viewport.color = col;
				Graphics.update();
				Input.update();
			}
			pbPushFade();
			try { //begin;
				if (block != null) block.Invoke(); //(block_given ?) yield;
			} finally { //ensure;
				pbPopFade();
				if (!nofadeout)
				{
					for (int j = 0; j < 17; j++)
					{
						col.set(0, 0, 0, (17 - j) * 15);
						viewport.color = col;
						Graphics.update();
						Input.update();
					}
				}
				viewport.dispose();
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

        public bool safeIsDirectory(string f)
        {
            throw new NotImplementedException();
        }

        public bool safeExists(string f)
        {
            throw new NotImplementedException();
        }

        public string[] safeGlob(string dir, string wildcard)
        {
            throw new NotImplementedException();
        }

        public IBitmap pbGetTileBitmap(string filename, int tile_id, int hue)
        {
            throw new NotImplementedException();
        }

        public IBitmap[] pbGetAnimation(string name, int hue = 0)
        {
            throw new NotImplementedException();
        }

        public IBitmap[] pbGetTileset(string name, int hue = 0)
        {
            throw new NotImplementedException();
        }

        public IBitmap[] pbGetAutotile(string name, int hue = 0)
        {
            throw new NotImplementedException();
        }

        public void pbDrawShadow(IBitmap bitmap, float x, float y, int width, int height, string text)
        {
            throw new NotImplementedException();
        }

        public void pbDrawShadowText(IBitmap bitmap, float x, float y, int width, int height, string text, IColor baseColor, IColor shadowColor = null, int align = 0)
        {
            throw new NotImplementedException();
        }

        public void pbDrawOutlineText(IBitmap bitmap, float x, float y, int width, int height, string text, IColor baseColor, IColor shadowColor = null, int align = 0)
        {
            throw new NotImplementedException();
        }

        public void pbBottomRight(IWindow window)
        {
            throw new NotImplementedException();
        }

        public void pbBottomLeft(IWindow window)
        {
            throw new NotImplementedException();
        }

        public void pbBottomLeftLines(IWindow window, int lines, int? width = null)
        {
            throw new NotImplementedException();
        }

        public bool pbDisposed(IViewport x)
        {
            throw new NotImplementedException();
        }

        public bool isDarkBackground(int background, IRect rect = null)
        {
            throw new NotImplementedException();
        }

        public bool isDarkWindowskin(int windowskin)
        {
            throw new NotImplementedException();
        }

        public IColor getDefaultTextColors(int windowskin)
        {
            throw new NotImplementedException();
        }

        public IBitmap pbDoEnsureBitmap(IBitmap bitmap, int dwidth, int dheight)
        {
            throw new NotImplementedException();
        }

        public void pbUpdateSpriteHash(IWindow[] windows)
        {
            throw new NotImplementedException();
        }

        public void pbDisposeSpriteHash(ISprite[] sprites)
        {
            throw new NotImplementedException();
        }

        public void pbDisposeSprite(ISprite[] sprites, int id)
        {
            throw new NotImplementedException();
        }

        public void pbDrawTextPositions(IBitmap bitmap, object[] textpos)
        {
            throw new NotImplementedException();
        }

        public void pbDrawImagePositions(IBitmap bitmap, object[] textpos)
        {
            throw new NotImplementedException();
        }

        public void pbFadeOutAndHide(ISprite[] sprites)
        {
            throw new NotImplementedException();
        }

        public void pbFadeInAndShow(ISprite[] sprites, IList<ISprite> visiblesprites = null, Action block = null)
        {
            throw new NotImplementedException();
        }

        public void pbRestoreActivations(ISprite[] sprites, object activeStatuses)
        {
            throw new NotImplementedException();
        }

        public void pbDeactivateWindows(IWindow[] sprites, Action action = null)
        {
            throw new NotImplementedException();
        }

        public void pbActivateWindow(IWindow[] sprites, int key, Action action = null)
        {
            throw new NotImplementedException();
        }

        public IColor pbAlphaBlend(IColor dstColor, IColor srcColor)
        {
            throw new NotImplementedException();
        }

        public IColor pbSrcOver(IColor dstColor, IColor srcColor)
        {
            throw new NotImplementedException();
        }

        public void pbSetSpritesToColor(ISprite[] sprites, IColor color)
        {
            throw new NotImplementedException();
        }

        public string pbTryString(string x)
        {
            throw new NotImplementedException();
        }

        public string pbBitmapName(string x)
        {
            throw new NotImplementedException();
        }

        public string pbResolveBitmap(string x)
        {
            throw new NotImplementedException();
        }

        public void addBackgroundPlane(ISprite[] sprites, string planename, string background, IViewport viewport = null)
        {
            throw new NotImplementedException();
        }

        public void addBackgroundOrColoredPlane(ISprite[] sprites, string planename, string background, IColor color, IViewport viewport = null)
        {
            throw new NotImplementedException();
        }

        public void pbSetSystemFont(IBitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public string pbSmallFontName()
        {
            throw new NotImplementedException();
        }

        public string pbNarrowFontName()
        {
            throw new NotImplementedException();
        }

        public void pbSetSmallFont(IBitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public void pbSetNarrowFont(IBitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public bool pbRgssExists(string filename)
        {
            throw new NotImplementedException();
        }

        public IDisposable pbRgssOpen(string file, int? mode = null, Action action = null)
        {
            throw new NotImplementedException();
        }

        public string pbGetFileChar(string file)
        {
            throw new NotImplementedException();
        }

        public string pbGetFileString(string file)
        {
            throw new NotImplementedException();
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