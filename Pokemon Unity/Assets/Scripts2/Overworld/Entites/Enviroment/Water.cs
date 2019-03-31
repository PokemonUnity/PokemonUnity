using PokemonUnity.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class Water : Entity
	{
		private static Dictionary<string, Texture2D> WaterTexturesTemp = new Dictionary<string, Texture2D>();
		private string waterTextureName = "";

		private Animation WaterAnimation;
		private Vector4 currentRectangle = new Vector4(0, 0, 0, 0);

		public override void Initialize()
		{
			base.Initialize();

			//WaterAnimation = new Animation(TextureManager.GetTexture(@"Textures\Routes"), 1, 3, 16, 16, 9, 15, 0);

			CreateWaterTextureTemp();
		}

		public static void ClearAnimationResources()
		{
			WaterTexturesTemp.Clear();
		}

		private void CreateWaterTextureTemp()
		{
			//if (Core.GameOptions.GraphicStyle == 1)
			//{
			//    List<string> textureData = this.AdditionalValue.Split(System.Convert.ToChar(",")).ToList();
			//    if (textureData.Count >= 5)
			//    {
			//        Vector4 r = new Vector4(System.Convert.ToInt32(textureData[1]), System.Convert.ToInt32(textureData[2]), System.Convert.ToInt32(textureData[3]), System.Convert.ToInt32(textureData[4]));
			//        string texturePath = textureData[0];
			//        this.waterTextureName = AdditionalValue;
			//        if (!Water.WaterTexturesTemp.ContainsKey(AdditionalValue + "_0"))
			//        {
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_0", TextureManager.GetTexture(texturePath, new Vector4(r.x, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_1", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_2", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 2, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_3", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 3, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_4", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 4, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_5", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 5, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_6", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 6, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_7", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 7, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_8", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 8, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_9", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 9, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_10", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 10, r.y, r.width, r.height)));
			//            Water.WaterTexturesTemp.Add(AdditionalValue + "_11", TextureManager.GetTexture(texturePath, new Vector4(r.x + r.width * 11, r.y, r.width, r.height)));
			//        }
			//    }
			//    else if (!Water.WaterTexturesTemp.ContainsKey("_0"))
			//    {
			//        Water.WaterTexturesTemp.Add("_0", TextureManager.GetTexture("Routes", new Vector4(0, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_1", TextureManager.GetTexture("Routes", new Vector4(20, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_2", TextureManager.GetTexture("Routes", new Vector4(40, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_3", TextureManager.GetTexture("Routes", new Vector4(60, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_4", TextureManager.GetTexture("Routes", new Vector4(80, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_5", TextureManager.GetTexture("Routes", new Vector4(100, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_6", TextureManager.GetTexture("Routes", new Vector4(120, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_7", TextureManager.GetTexture("Routes", new Vector4(140, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_8", TextureManager.GetTexture("Routes", new Vector4(160, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_9", TextureManager.GetTexture("Routes", new Vector4(180, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_10", TextureManager.GetTexture("Routes", new Vector4(200, 220, 20, 20)));
			//        Water.WaterTexturesTemp.Add("_11", TextureManager.GetTexture("Routes", new Vector4(220, 220, 20, 20)));
			//    }
			//}
		}

		public override void ClickFunction()
		{
			this.Surf();
		}

		public override bool WalkAgainstFunction()
		{
			WalkOntoFunction();
			return base.WalkAgainstFunction();
		}

		public override bool WalkIntoFunction()
		{
			WalkOntoFunction();
			return base.WalkIntoFunction();
		}

		public override void WalkOntoFunction()
		{
			if (Game.Level.Surfing)
			{
				bool canSurf = false;

				foreach (Entity Entity in Game.Level.Entities)
				{
					if (Entity.boundingBox.Contains(Game.Camera.GetForwardMovedPosition()))// == ContainmentType.Contains
					{
						if (Entity.EntityID == Entities.Water)
							canSurf = true;
						else if (Entity.Collision)
						{
							canSurf = false;
							break;
						}
					}
				}

				if (canSurf)
				{
					Game.Camera.Move(1);

					Game.Level.PokemonEncounter.TryEncounterWildPokemon(this.Position, EncounterTypes.Surfing, "");
				}
			}
		}

		private void Surf()
		{
			if (!Game.Camera.Turning)
			{
				if (!Game.Level.Surfing)
				{
					if (Badge.CanUseHMMove(Badge.HMMoves.Surf) | Game.IS_DEBUG_ACTIVE | Game.Player.SandBoxMode)
					{
						if (!new ChooseBox().Showing)
						{
							bool canSurf = false;

							if (this.ActionValue == 0)
							{
								foreach (Entity Entity in Game.Level.Entities)
								{
									if (Entity.boundingBox.Contains(Game.Camera.GetForwardMovedPosition()))// == ContainmentType.Contains
									{
										if (Entity.EntityID == Entities.Water)
										{
											if (Game.Player.SurfPokemon > -1)
												canSurf = true;
										}
										else if (Entity.Collision)
										{
											canSurf = false;
											break;
										}
									}
								}
							}

							if (Game.Level.Riding)
								canSurf = false;

							if (canSurf)
							{
								string message = "Do you want to Surf?%Yes|No%";
								string waterType = "";
								if (this.AdditionalValue.CountSeperators(",") >= 6)
									waterType = this.AdditionalValue.GetSplit(5);
								else
									waterType = this.AdditionalValue;
								switch (waterType.ToLower())
								{
									case "0":
									case "":
										{
											message = "Do you want to Surf?%Yes|No%";
											break;
										}
									case "1":
									case "sea":
									case "water":
										{
											message = "The water looks still~and deep.~Do you want to Surf?%Yes|No%";
											break;
										}
									case "2":
									case "lake":
									case "pond":
										{
											message = "This lake is~calm and shallow~Do you want to Surf?%Yes|No%";
											break;
										}
								}

								Game.TextBox.Show(message, new Entity[] { this }, true, true);
								SoundManager.PlaySound("select");
							}
						}
					}
				}
			}
		}

		protected override float CalculateCameraDistance(Vector3 CPosition)
		{
			return base.CalculateCameraDistance(CPosition) - 0.2f;
		}

		public override void UpdateEntity()
		{
			if (WaterAnimation != null)
			{
				//WaterAnimation.Update(0.01f);
				//if (currentRectangle != WaterAnimation.TextureRectangle)
				//{
				//    ChangeTexture();
				//
				//    currentRectangle = WaterAnimation.TextureRectangle;
				//}
			}

			base.UpdateEntity();
		}

		private void ChangeTexture()
		{
			//if (Core.GameOptions.GraphicStyle == 1)
			//{
			//    if (WaterTexturesTemp.Count == 0)
			//    {
			//        ClearAnimationResources();
			//        CreateWaterTextureTemp();
			//    }
			//
			//    switch (this.Rotation.y)
			//    {
			//        case 0:
			//        case (object)MathHelper.TwoPi:
			//            {
			//                switch (WaterAnimation.CurrentColumn)
			//                {
			//                    case 0:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_0"];
			//                            break;
			//                        }
			//                    case 1:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_1"];
			//                            break;
			//                        }
			//                    case 2:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_2"];
			//                            break;
			//                        }
			//                }
			//
			//                break;
			//            }
			//        case (object)MathHelper.Pi * 0.5f:
			//            {
			//                switch (WaterAnimation.CurrentColumn)
			//                {
			//                    case 0:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_3"];
			//                            break;
			//                        }
			//                    case 1:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_4"];
			//                            break;
			//                        }
			//                    case 2:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_5"];
			//                            break;
			//                        }
			//                }
			//
			//                break;
			//            }
			//        case (object)MathHelper.Pi:
			//            {
			//                switch (WaterAnimation.CurrentColumn)
			//                {
			//                    case 0:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_6"];
			//                            break;
			//                        }
			//                    case 1:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_7"];
			//                            break;
			//                        }
			//                    case 2:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_8"];
			//                            break;
			//                        }
			//                }
			//
			//                break;
			//            }
			//        case (object)MathHelper.Pi * 1.5f:
			//            {
			//                switch (WaterAnimation.CurrentColumn)
			//                {
			//                    case 0:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_9"];
			//                            break;
			//                        }
			//                    case 1:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_10"];
			//                            break;
			//                        }
			//                    case 2:
			//                        {
			//                            this.Textures[0] = Water.WaterTexturesTemp[waterTextureName + "_11"];
			//                            break;
			//                        }
			//                }
			//
			//                break;
			//            }
			//    }
			//}
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				Game.TextBox.Show(Game.Player.Party[Game.Player.SurfPokemon].Name + " used~Surf!", new Entity[] { this });
				Game.Level.Surfing = true;
				Game.Camera.Move(1);
				//PlayerStatistics.Track("Surf used", 1);

				{
					var withBlock = Game.Level.OwnPlayer;
					//Game.Player.TempSurfSkin = withBlock.SkinName;

					//int pokemonNumber = Game.Player.Party[Game.Player.SurfPokemon].Species;
					//string SkinName = "[POKEMON|N]" + pokemonNumber + PokemonForms.GetOverworldAddition(Game.Player.Party[Game.Player.SurfPokemon]);
					//if (Game.Player.Party[Game.Player.SurfPokemon].IsShiny)
					//    SkinName = "[POKEMON|S]" + pokemonNumber + PokemonForms.GetOverworldAddition(Game.Player.Party[Game.Player.SurfPokemon]);

					//withBlock.SetTexture(SkinName, false);
					//
					//withBlock.UpdateEntity();
					//
					//SoundManager.PlayPokemonCry(pokemonNumber);
					//
					//if (!Game.Level.IsRadioOn || !GameJolt.PokegearScreen.StationCanPlay(Game.Level.SelectedRadioStation))
					//    MusicManager.Play("surf", true);
				}
			}
		}

		public override void Render()
		{
			//bool setRasterizerState = this.Model.ID != 0;
			//
			//this.Draw(this.Model, Textures, setRasterizerState);
		}
	}
}