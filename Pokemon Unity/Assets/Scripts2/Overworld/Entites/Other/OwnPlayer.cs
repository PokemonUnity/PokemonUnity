using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Misc
{
	public class OwnPlayer : Entity
	{
		public static readonly string[] AllowedSkins = new string[] { "Ethan", "Lyra", "Nate", "Rosa", "Hilbert", "Hilda", "GoldRetro" };

		public Texture2D Texture;
		public string SkinName = "Hilbert";

		public bool HasPokemonTexture = false;

		private Vector4 lastRectangle = new Vector4(0, 0, 0, 0);
		private string lastTexture = "";

		private int AnimationX = 1;
		const float AnimationDelayLenght = 1.1f;
		private float AnimationDelay = AnimationDelayLenght;
		public bool DoAnimation = true;

		public bool UsingGameJoltTexture = false;

		public OwnPlayer(float X, float Y, float Z, Texture2D[] Textures, string TextureID, int Rotation, int ActionValue, string AdditionalValue, string Name, int ID) : base(X, Y, Z, Entities.OwnPlayer, Textures,
			new int[]
			{
			0,
			0
			}, false, 0, new Vector3(1.0f, 1, 1)/*, UnityEngine.Mesh.BillModel*/, 0, "", new Vector3(1.0f, 1, 1))
		{
			SetTexture(TextureID, true);

			this.NeedsUpdate = true;
			this.CreateWorldEveryFrame = true;

			this.DropUpdateUnlessDrawn = false;
		}

		public void SetTexture(string TextureID, bool UseGameJoltID)
		{
			this.SkinName = TextureID;
			HasPokemonTexture = false;

			string texturePath = @"Textures\NPC\";
			bool isPokemon = false;
			if (TextureID.StartsWith("[POKEMON|N]") | TextureID.StartsWith("[Pokémon|N]"))
			{
				TextureID = TextureID.Remove(0, 11);
				isPokemon = true;
				texturePath = @"Pokemon\Overworld\Normal\";
				HasPokemonTexture = true;
			}
			else if (TextureID.StartsWith("[POKEMON|S]") | TextureID.StartsWith("[Pokémon|S]"))
			{
				TextureID = TextureID.Remove(0, 11);
				isPokemon = true;
				texturePath = @"Pokemon\Overworld\Shiny\";
				HasPokemonTexture = true;
			}

			string PokemonAddition = "";
			//if (StringHelper.IsNumeric(TextureID) & texturePath.StartsWith(@"Pokemon\Overworld\"))
			//	PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(System.Convert.ToInt32(TextureID));

			//if (GameVariables.playerTrainer.IsGameJoltSave)
			//{
			//	if (texturePath + TextureID + PokemonAddition == @"Textures\NPC\" + GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(Core.GameJoltSave.Points), Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Gender))
			//		UseGameJoltID = true;
			//}

			//if (UseGameJoltID & GameVariables.playerTrainer.IsGameJoltSave & GameJolt.API.LoggedIn && GameJolt.Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID) != null)
			//{
			//	GameVariables.DebugLog("Change player texture to the online sprite.");
			//	this.Texture = GameJolt.Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID);
			//	UsingGameJoltTexture = true;
			//}
			//else
			//{
			//	GameVariables.DebugLog("Change player texture to [" + texturePath + TextureID + PokemonAddition + "]");
			//
			//	this.Texture = TextureManager.GetTexture(texturePath + TextureID + PokemonAddition);
			//	UsingGameJoltTexture = false;
			//}
		}

		protected override float CalculateCameraDistance(Vector3 CPosition)
		{
			return base.CalculateCameraDistance(CPosition) - 0.2f;
		}

		public override void UpdateEntity()
		{
			//if (Core.CurrentScreen != null)
			//{
			//	if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
			//	{
			//		if (GameVariables.Camera.Name == "Overworld")
			//		{
			//			OverworldCamera c = (OverworldCamera)GameVariables.Camera;
			//			this.Position = new Vector3(c.Position.x, c.Position.y - 0.1f, c.Position.z);
			//		}
			//	}
			//	if (this.Rotation.y != GameVariables.Camera.Yaw)
			//		this.Rotation.y = GameVariables.Camera.Yaw;
			//}

			Move();
			ChangeTexture();

			base.UpdateEntity();
		}

		private void Move()
		{
			if ((GameVariables.Camera.IsMoving & this.DoAnimation) || (GameVariables.Level.OwnPlayer != null && GameVariables.Level.OwnPlayer.isDancing))
			{
				this.AnimationDelay -= 0.13f;
				if (AnimationDelay <= 0.0f)
				{
					AnimationDelay = GetAnimationDelay();
					AnimationX += 1;
					if (HasPokemonTexture)
					{
						if (AnimationX > 2)
							AnimationX = 1;
					}
					else if (AnimationX > 4)
						AnimationX = 1;
				}
			}
			else
			{
				AnimationX = 1;
				AnimationDelay = GetAnimationDelay();
				ChangeTexture();
			}
		}

		public void ChangeTexture()
		{
			//if (this.Texture != null)
			//{
			//	Vector4 r = new Vector4(0, 0, 0, 0);
			//	int cameraRotation = 0;
			//	int spriteIndex = 0;
			//
			//	spriteIndex = 0;
			//
			//	if (GameVariables.Camera.Name == "Overworld")
			//	{
			//		spriteIndex = GameVariables.Camera.GetPlayerFacingDirection() - GameVariables.Camera.GetFacingDirection();
			//		while (spriteIndex > 3)
			//			spriteIndex -= 4;
			//		while (spriteIndex < 0)
			//			spriteIndex += 4;
			//	}
			//
			//	Size frameSize = new Size(System.Convert.ToInt32(this.Texture.width / (double)3), System.Convert.ToInt32(this.Texture.height / (double)4));
			//
			//	int x = 0;
			//	if (GameVariables.Camera.IsMoving())
			//		x = GetAnimationX() * frameSize.width;
			//
			//	r = new Vector4(x, frameSize.width * spriteIndex, frameSize.width, frameSize.height);
			//
			//	if (r != lastRectangle | lastTexture != SkinName)
			//	{
			//		lastRectangle = r;
			//		lastTexture = SkinName;
			//		GameVariables.playerTrainer.Skin = SkinName;
			//
			//		try
			//		{
			//			Texture2D t = TextureManager.GetTexture(this.Texture, r, 1);
			//			Textures[0] = t;
			//		}
			//		catch
			//		{
			//			GameVariables.DebugLog("OwnPlayer.vb: Error assigning a new texture to the player.", false);
			//		}
			//	}
			//}
		}

		private int GetAnimationX()
		{
			if (HasPokemonTexture)
				return AnimationX;
			else
				switch (AnimationX)
				{
					case 1:
						{
							return 0;
						}
					case 2:
						{
							return 1;
						}
					case 3:
						{
							return 0;
						}
					case 4:
						{
							return 2;
						}
				}

			return 1;
		}

		public override void Render()
		{
			if (InCameraFocus())
			{
				//var state = GraphicsDevice.DepthStencilState;
				//GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
				//Draw(this.Model, this.Textures, true);
				//GraphicsDevice.DepthStencilState = state;
			}
		}

		internal bool InCameraFocus()
		{
			//if (GameVariables.Camera.Name == "Overworld")
			//{
			//	var c = (OverworldCamera)GameVariables.Camera;
			//
			//	if (c.CameraFocusType == OverworldCamera.CameraFocusTypes.Player & c.ThirdPerson | c.CameraFocusType != OverworldCamera.CameraFocusTypes.Player)
			//		return true;
			//}
			return false;
		}

		public void ApplyShaders()
		{
			this.Shaders.Clear();
			//foreach (Shader Shader in GameVariables.Level.Shaders)
			//	Shader.ApplyShader(this);
		}

		private float GetAnimationDelay()
		{
			if (GameVariables.playerTrainer.IsRunning())
				return OwnPlayer.AnimationDelayLenght / (float)1.4f;
			return OwnPlayer.AnimationDelayLenght;
		}
	}
}