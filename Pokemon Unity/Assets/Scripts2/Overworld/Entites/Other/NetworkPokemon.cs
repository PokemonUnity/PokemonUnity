using System.Linq;

namespace PokemonUnity.Overworld.Entity.Misc
{
public class NetworkPokemon : Entity
{
	public int PlayerID = 0;
	public string LevelFile = "";
	public string PokemonTexture = "";
	public int faceRotation = 0;

	private Texture2D Texture;
	private Rectangle lastRectangle = new Rectangle(0, 0, 0, 0);
	private string loadedTexture = "";

	private int AnimationX = 1;
	private float AnimationDelayLenght = 2.2F;
	private float AnimationDelay = AnimationDelayLenght;

	public NetworkPokemon(Vector3 pos, string PokemonTexture, bool visible) : base(pos.x, pos.y, pos.z, "NetworkPokemon",
		new Texture2D()
	{
		TextureManager.DefaultTexture

	}, new int[]
    {
        0,
        0
    }, false, 0, new Vector3(0.9F), BaseModel.BillModel, 0, "", new Vector3(1))
    {
        this.Visible = visible;

        this.PokemonTexture = PokemonTexture;

        this.Position = new Vector3(System.Convert.ToInt32(this.Position.x), System.Convert.ToSingle(this.Position.y) + 0.001F, System.Convert.ToInt32(this.Position.z));
        this.NeedsUpdate = true;
        this.CreateWorldEveryFrame = true;

        this.NeedsUpdate = true;

        this.DropUpdateUnlessDrawn = false;
    }

	protected override float CalculateCameraDistance(Vector3 CPosition)
	{
		return base.CalculateCameraDistance(CPosition) - 0.2F;
	}

	public override void UpdateEntity()
	{
		if (this.Rotation.y != Screen.Camera.Yaw)
			this.Rotation.y = Screen.Camera.Yaw;

		base.UpdateEntity();
	}

	public override void Update()
	{
		if (this.PokemonTexture != this.loadedTexture)
			this.Texture = null;
		this.loadedTexture = this.PokemonTexture;

		if (this.PokemonTexture != "")
		{
			this.ChangeTexture();

			this.AnimationDelay -= 0.1F;
			if (AnimationDelay <= 0.0F)
			{
				AnimationDelay = AnimationDelayLenght;
				AnimationX += 1;
				if (AnimationX > 2)
					AnimationX = 1;
			}
		}
	}

	public override void Render()
	{
		if (ConnectScreen.Connected == true)
		{
			if (System.Convert.ToBoolean(GameModeManager.GetGameRuleValue("ShowFollowPokemon", "1")) == true)
			{
				if (GameVariables.Level.ShowOverworldPokemon == true)
				{
					if (IsCorrectScreen() == true)
					{
						if (this.PokemonTexture != "")
						{
							if (this.Textures != null)
							{
								var state = GraphicsDevice.DepthStencilState;
								GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
								Draw(this.Model, new Texture2D()
									{
										this.Textures(0)
									}, false);
								GraphicsDevice.DepthStencilState = state;
							}
						}
					}
				}
			}
		}
	}

	private bool IsCorrectScreen()
	{
		Screen.Identifications[] screens = new[] { Screen.Identifications.BattleCatchScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleGrowStatsScreen, Screen.Identifications.BattleScreen, Screen.Identifications.CreditsScreen, Screen.Identifications.BattleAnimationScreen, Screen.Identifications.ViewModelScreen, Screen.Identifications.HallofFameScreen };
		if (screens.Contains(Core.CurrentScreen.Identification) == true)
			return false;
		else if (Core.CurrentScreen.Identification == Screen.Identifications.TransitionScreen)
		{
			if (screens.Contains((TransitionScreen)Core.CurrentScreen.OldScreen.Identification) == true | screens.Contains((TransitionScreen)Core.CurrentScreen.NewScreen.Identification) == true)
				return false;
		}
		else
		{
			Screen c = Core.CurrentScreen;
			while (c.PreScreen != null)
				c = c.PreScreen;
			if (screens.Contains(c.Identification) == true)
				return false;
		}
		return true;
	}

	private void ChangeTexture()
	{
		if (this.Texture == null)
		{
			string path = this.PokemonTexture.Replace("[POKEMON|N]", @"Pokemon\Overworld\Normal\").Replace("[POKEMON|S]", @"Pokemon\Overworld\Shiny\");
			this.Texture = TextureManager.GetTexture(path);
		}

		Rectangle r = new Rectangle(0, 0, 0, 0);
		int cameraRotation = Screen.Camera.GetFacingDirection();
		int spriteIndex = this.faceRotation - cameraRotation;

		spriteIndex = this.faceRotation - cameraRotation;
		if (spriteIndex < 0)
			spriteIndex += 4;

		int dimension = System.Convert.ToInt32(this.Texture.Width / (double)3);

		int x = 0;
		x = AnimationX * dimension;

		int height = dimension;

		int y = dimension * spriteIndex;
		y -= 2;
		if (y < 0)
		{
			y = 0;
			height = dimension - 2;
		}
		y += 2;
		height -= 2;

		r = new Rectangle(x, y, dimension, height);

		if (r != lastRectangle)
		{
			lastRectangle = r;

			Textures(0) = TextureManager.GetTexture(this.Texture, r, 1);
		}
	}

	public void ApplyShaders()
	{
		this.Shaders.Clear();
		foreach (Shader Shader in GameVariables.Level.Shaders)
			Shader.ApplyShader(this);
	}

	public void ApplyPlayerData(Servers.Player p)
	{
		try
		{
			this.PlayerID = p.ServersID;

			this.PokemonTexture = p.PokemonSkin;
			this.Position = p.PokemonPosition;
			this.LevelFile = p.LevelFile;
			this.Visible = p.PokemonVisible;
			this.faceRotation = p.PokemonFacing;
			this.FaceDirection = p.PokemonFacing;

			if (this.Visible == true)
			{
				this.Visible = false;
				if (GameVariables.Level.LevelFile == p.LevelFile)
					this.Visible = true;
				else if (LevelLoader.LoadedOffsetMapNames.Contains(p.LevelFile) == true)
				{
					Offset = LevelLoader.LoadedOffsetMapOffsets(LevelLoader.LoadedOffsetMapNames.IndexOf(p.LevelFile));
					this.Position.x += Offset.x;
					this.Position.y += Offset.y;
					this.Position.z += Offset.z;
					this.Visible = true;
				}
			}
		}
		catch
		{
		}
	}
}
}