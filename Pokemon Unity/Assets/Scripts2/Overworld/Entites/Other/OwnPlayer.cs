using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Misc
{
public class OwnPlayer : Entity
{
	public static readonly string[] AllowedSkins = new string[] { "Ethan", "Lyra", "Nate", "Rosa", "Hilbert", "Hilda", "GoldRetro" };

	public Texture2D Texture;
	public string SkinName = "Hilbert";

	public bool HasPokemonTexture = false;

	private Rectangle lastRectangle = new Rectangle(0, 0, 0, 0);
	private string lastTexture = "";

	private int AnimationX = 1;
	const float AnimationDelayLenght = 1.1F;
	private float AnimationDelay = AnimationDelayLenght;
	public bool DoAnimation = true;

	public bool UsingGameJoltTexture = false;

	public OwnPlayer(float X, float Y, float Z, Texture2D[] Textures, string TextureID, int Rotation, int ActionValue, string AdditionalValue, string Name, int ID) : base(X, Y, Z, "OwnPlayer", Textures,
		new int[]
		{
			0,
			0
		}, false, 0, new Vector3(1.0F), BaseModel.BillModel, 0, "", new Vector3(1.0F))
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
		if (StringHelper.IsNumeric(TextureID) & texturePath.StartsWith(@"Pokemon\Overworld\"))
			PokemonAddition = PokemonForms.GetDefaultOverworldSpriteAddition(System.Convert.ToInt32(TextureID));

		if (GameVariables.playerTrainer.IsGameJoltSave)
		{
			if (texturePath + TextureID + PokemonAddition == @"Textures\NPC\" + GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(Core.GameJoltSave.Points), Core.GameJoltSave.GameJoltID, Core.GameJoltSave.Gender))
				UseGameJoltID = true;
		}

		if (UseGameJoltID & GameVariables.playerTrainer.IsGameJoltSave & GameJolt.API.LoggedIn && !GameJolt.Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID) == null)
		{
			Logger.Debug("Change player texture to the online sprite.");
			this.Texture = GameJolt.Emblem.GetOnlineSprite(Core.GameJoltSave.GameJoltID);
			UsingGameJoltTexture = true;
		}
		else
		{
			Logger.Debug("Change player texture to [" + texturePath + TextureID + PokemonAddition + "]");

			this.Texture = P3D.TextureManager.GetTexture(texturePath + TextureID + PokemonAddition);
			UsingGameJoltTexture = false;
		}
	}

	protected override float CalculateCameraDistance(Vector3 CPosition)
	{
		return base.CalculateCameraDistance(CPosition) - 0.2F;
	}

	public override void UpdateEntity()
	{
		if (Core.CurrentScreen != null)
		{
			if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
			{
				if (Screen.Camera.Name == "Overworld")
				{
					OverworldCamera c = (OverworldCamera)Screen.Camera;
					this.Position = new Vector3(c.Position.x, c.Position.y - 0.1F, c.Position.z);
				}
			}
			if (this.Rotation.y != Screen.Camera.Yaw)
				this.Rotation.y = Screen.Camera.Yaw;
		}

		Move();
		ChangeTexture();

		base.UpdateEntity();
	}

	private void Move()
	{
		if ((Screen.Camera.IsMoving() == true & this.DoAnimation == true) || (Screen.Level.OwnPlayer != null && Screen.Level.OwnPlayer.isDancing))
		{
			this.AnimationDelay -= 0.13F;
			if (AnimationDelay <= 0.0F)
			{
				AnimationDelay = GetAnimationDelay();
				AnimationX += 1;
				if (HasPokemonTexture == true)
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
		if (this.Texture != null)
		{
			Rectangle r = new Rectangle(0, 0, 0, 0);
			int cameraRotation = 0;
			int spriteIndex = 0;

			spriteIndex = 0;

			if (Screen.Camera.Name == "Overworld")
			{
				spriteIndex = Screen.Camera.GetPlayerFacingDirection() - Screen.Camera.GetFacingDirection();
				while (spriteIndex > 3)
					spriteIndex -= 4;
				while (spriteIndex < 0)
					spriteIndex += 4;
			}

			Size frameSize = new Size(System.Convert.ToInt32(this.Texture.Width / (double)3), System.Convert.ToInt32(this.Texture.Height / (double)4));

			int x = 0;
			if (Screen.Camera.IsMoving() == true)
				x = GetAnimationX() * frameSize.Width;

			r = new Rectangle(x, frameSize.Width * spriteIndex, frameSize.Width, frameSize.Height);

			if (r != lastRectangle | lastTexture != SkinName)
			{
				lastRectangle = r;
				lastTexture = SkinName;
				GameVariables.playerTrainer.Skin = SkinName;

				try
				{
					Texture2D t = TextureManager.GetTexture(this.Texture, r, 1);
					Textures(0) = t;
				}
				catch
				{
					Logger.Log(Logger.LogTypes.Warning, "OwnPlayer.vb: Error assigning a new texture to the player.");
				}
			}
		}
	}

	private int GetAnimationX()
	{
		if (HasPokemonTexture == true)
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
		if (InCameraFocus() == true)
		{
			var state = GraphicsDevice.DepthStencilState;
			GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
			Draw(this.Model, this.Textures, true);
			GraphicsDevice.DepthStencilState = state;
		}
	}

	internal bool InCameraFocus()
	{
		if (Screen.Camera.Name == "Overworld")
		{
			var c = (OverworldCamera)Screen.Camera;

			if (c.CameraFocusType == OverworldCamera.CameraFocusTypes.Player & c.ThirdPerson == true | c.CameraFocusType != OverworldCamera.CameraFocusTypes.Player)
				return true;
		}
		return false;
	}

	public void ApplyShaders()
	{
		this.Shaders.Clear();
		foreach (Shader Shader in Screen.Level.Shaders)
			Shader.ApplyShader(this);
	}

	private float GetAnimationDelay()
	{
		if (GameVariables.playerTrainer.IsRunning() == true)
			return OwnPlayer.AnimationDelayLenght / (double)1.4F;
		return OwnPlayer.AnimationDelayLenght;
	}
}
}