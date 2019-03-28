using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace PokemonUnity
{
	public class TextureManager
	{
		public static Texture2D DefaultTexture;

		public static void InitializeTextures()
		{
			DefaultTexture = LoadDirect(@"GUI\no_texture.png");
		}

		public static Dictionary<string, Texture2D> TextureList = new Dictionary<string, Texture2D>();
		public static Dictionary<KeyValuePair<Int32, Rectangle>, Texture2D> TextureRectList = new Dictionary<KeyValuePair<Int32, Rectangle>, Texture2D>();

		public static Texture2D LoadDirect(string textureFile)
		{
			using (Stream stream = File.Open(GameController.GamePath + @"\Content\" + textureFile, System.IO.FileMode.OpenOrCreate))
			{
				return Texture2D.FromStream(Core.GraphicsDevice, stream);
			}
		}

		/// <summary>
		/// Returns a texture.
		/// </summary>
		/// <param name="Name">The name of the texture.</param>
		public static Texture2D GetTexture(string Name)
		{
			ContentManager cContent = ContentPackManager.GetContentManager(Name, ".xnb,.png");

			string tKey = cContent.RootDirectory + @"\" + Name + ",FULL_IMAGE";

			if (TextureList.ContainsKey(tKey) == false)
			{
				Texture2D t = null;// TODO Change to default(_) if this is not a reference type

				if (System.IO.File.Exists(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + ".xnb") == false)
				{
					if (System.IO.File.Exists(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + ".png") == true)
					{
						using (System.IO.Stream stream = System.IO.File.Open(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + ".png", System.IO.FileMode.OpenOrCreate))
						{
							try
							{
								t = Texture2D.FromStream(Core.GraphicsDevice, stream);
							}
							catch (Exception ex)
							{
								GameVariables.DebugLog("Something went wrong while XNA tried to load a texture. Return default.", true);
								return DefaultTexture;
							}
						}
					}
					else
					{
						GameVariables.DebugLog(GameVariables.DebugLogTypes.ErrorMessage, "Texures.vb: Texture \"" + GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + "\" was not found!");
						return DefaultTexture;
					}
				}
				else
					t = cContent.Load<Texture2D>(Name);

				TextureList.Add(tKey, ApplyEffect(TextureRectangle(t, new Rectangle(0, 0, t.Width, t.Height), 1)));

				cContent.Unload();
			}

			return TextureList[tKey];
		}

		private static Texture2D ApplyEffect(Texture2D t)
		{
			if (GameController.Hacker == true)
			{
				Texture2D newT = new Texture2D(Core.GraphicsDevice, t.Width, t.Height);

				List<Color> newC = new List<Color>();
				Color[] oldC = new Color[t.Width * t.Height - 1 + 1];
				t.GetData(oldC);

				foreach (Color c in oldC)
					newC.Add(c.Invert());

				newT.SetData(newC.ToArray());
				return newT;
			}
			else
				return t;
		}

		/// <summary>
		/// Returns a texture.
		/// </summary>
		/// <param name="Name">The name of the texture.</param>
		/// <param name="r">The rectangle to get the texture from.</param>
		/// <param name="TexturePath">The texturepath to load a texture from.</param>
		public static Texture2D GetTexture(string Name, Rectangle r, string TexturePath)
		{
			TextureSource tSource = ContentPackManager.GetTextureReplacement(TexturePath + Name, r);

			ContentManager cContent = ContentPackManager.GetContentManager(tSource.TexturePath, ".xnb,.png");
			int resolution = ContentPackManager.GetTextureResolution(TexturePath + Name);

			string tKey = cContent.RootDirectory + @"\" + TexturePath + Name + "," + r.X + "," + r.Y + "," + r.Width + "," + r.Height + "," + resolution;
			if (TextureList.ContainsKey(tKey) == false)
			{
				Texture2D t = null;// TODO Change to default(_) if this is not a reference type
				bool doApplyEffect = true;

				if (TextureList.ContainsKey(cContent.RootDirectory + @"\" + TexturePath + Name) == true)
				{
					t = TextureList[cContent.RootDirectory + @"\" + TexturePath + Name];
					doApplyEffect = false;
				}
				else
				{
					if (System.IO.File.Exists(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + tSource.TexturePath + ".xnb") == false)
					{
						if (System.IO.File.Exists(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + tSource.TexturePath + ".png") == true)
						{
							using (System.IO.Stream stream = System.IO.File.Open(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + tSource.TexturePath + ".png", System.IO.FileMode.OpenOrCreate))
							{
								try
								{
									t = Texture2D.FromStream(Core.GraphicsDevice, stream);
								}
								catch (Exception ex)
								{
									GameVariables.DebugLog("Something went wrong while XNA tried to load a texture. Return default.", true);
									return DefaultTexture;
								}
							}
						}
						else
						{
							GameVariables.DebugLog("Texures.vb: Texture \"" + GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + "\" was not found!", true);
							return DefaultTexture;
						}
					}
					else
						t = cContent.Load<Texture2D>(tSource.TexturePath);

					TextureList.Add(cContent.RootDirectory + @"\" + TexturePath + Name, ApplyEffect(t.Copy()));
				}

				if (doApplyEffect == true)
				{
					if (TextureList.ContainsKey(tKey) == false)
						TextureList.Add(tKey, ApplyEffect(TextureRectangle(t, tSource.TextureRectangle, resolution)));
				}
				else if (TextureList.ContainsKey(tKey) == false)
					TextureList.Add(tKey, TextureRectangle(t, tSource.TextureRectangle, resolution));

				cContent.Unload();
			}

			return TextureList[tKey];
		}

		/// <summary>
		/// Returns the texture. The default texture path is "Textures\".
		/// </summary>
		/// <param name="Name">The name of the texture.</param>
		/// <param name="r">The rectangle to get from the texture.</param>
		public static Texture2D GetTexture(string Name, Rectangle r)
		{
			return GetTexture(Name, r, @"Textures\");
		}

		public static Texture2D GetTexture(Texture2D Texture, Rectangle Rectangle, int Factor = 1)
		{
			Texture2D tex = null;// TODO Change to default(_) if this is not a reference type

			if (TextureRectList.TryGetValue(new KeyValuePair<Int32, Rectangle>(Texture.GetHashCode(), out Rectangle), ref tex))
				return tex;

			tex = TextureRectangle(Texture, Rectangle, Factor);
			TextureRectList.Add(new KeyValuePair<int, Rectangle>(Texture.GetHashCode(), Rectangle), tex);

			return tex;
		}

		private static Texture2D TextureRectangle(Texture2D Texture, Rectangle Rectangle, int Factor = 1)
		{
			if (Rectangle == Rectangle.Empty)
				return Texture;
			else
			{
				Rectangle = new Rectangle(Rectangle.X * Factor, Rectangle.Y * Factor, Rectangle.Width * Factor, Rectangle.Height * Factor);

				Rectangle tRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
				if (tRectangle.Contains(Rectangle) == false)
				{
					GameVariables.DebugLog(GameVariables.DebugLogTypes.ErrorMessage, "Textures.vb: The rectangle for a texture was out of bounds!");
					return DefaultTexture;
				}

				Color[] Data = new Color[Rectangle.Width * Rectangle.Height - 1 + 1];
				Texture.GetData(0, Rectangle, Data, 0, Rectangle.Width * Rectangle.Height);

				Texture2D newTex = new Texture2D(Core.GraphicsDevice, Rectangle.Width, Rectangle.Height);
				newTex.SetData(Data);

				return newTex;
			}
		}

		public static bool TextureExist(string Name)
		{
			ContentManager cContent = ContentPackManager.GetContentManager(Name, ".xnb,.png");
			if (System.IO.File.Exists(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + ".xnb") == true)
				return true;
			else if (System.IO.File.Exists(GameController.GamePath + @"\" + cContent.RootDirectory + @"\" + Name + ".png") == true)
				return true;
			return false;
		}
	}
}