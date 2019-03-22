using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class AnimatedBlock : Entity
{
	private static Dictionary<string, Texture2D> BlockTexturesTemp = new Dictionary<string, Texture2D>();


	private List<string> AnimationNames;

	private List<Animation> Animations;
	private List<Rectangle> currentRectangle;

	private List<int> X, Y, width, height, rows, columns, animationSpeed, startRow, startColumn;

	private int AnimCount = 0;


	public AnimatedBlock()
	{
		X = new List<int>();
		Y = new List<int>();
		width = new List<int>();
		height = new List<int>();
		rows = new List<int>();
		columns = new List<int>();
		animationSpeed = new List<int>();
		startRow = new List<int>();
		startColumn = new List<int>();

		AnimationNames = new List<string>();
		currentRectangle = new List<Rectangle>();
		Animations = new List<Animation>();
	}

	public new void Initialize(List<List<int>> AnimationData)
	{
		base.Initialize();
		for (var i = 0; i <= AnimationData.Count - 1; i++)
		{
			X.Add(AnimationData[i][0]);
			Y.Add(AnimationData[i][1]);
			width.Add(AnimationData[i][2]);
			height.Add(AnimationData[i][3]);
			rows.Add(AnimationData[i][4]);
			columns.Add(AnimationData[i][5]);
			animationSpeed.Add(AnimationData[i][6]);
			startRow.Add(AnimationData[i][7]);
			startColumn.Add(AnimationData[i][8]);

			AnimationNames.Add("");
			currentRectangle.Add(new Rectangle(0, 0, 0, 0));

			Animations.Add(new Animation(TextureManager.GetTexture(@"Textures\Routes"), rows[i], columns[i], 16, 16, animationSpeed[i], startRow[i], startColumn[i]));

			AnimCount += 1;
		}

		CreateBlockTextureTemp();
	}

	public static void ClearAnimationResources()
	{
		BlockTexturesTemp.Clear();
	}

	private void CreateBlockTextureTemp()
	{
		// If Core.GameOptions.GraphicStyle = 1 Then

		for (var n = 0; n <= Animations.Count - 1; n++)
		{
			Rectangle r = new Rectangle(X[n], Y[n], width[n], height[n]);
			this.AnimationNames[n] = AdditionalValue + "," + X[n] + "," + Y[n] + "," + height[n] + "," + width[n];
			if (BlockTexturesTemp.ContainsKey(AnimationNames[n] + "_0") == false)
			{
				for (var i = 0; i <= this.rows[n] - 1; i++)
				{
					for (var j = 0; j <= this.columns[n] - 1; j++)
						BlockTexturesTemp.Add(AnimationNames[n] + "_" + (j + columns[n] * i).ToString(), TextureManager.GetTexture(AdditionalValue, new Rectangle(r.x + r.Width * j, r.y + r.Height * i, r.Width, r.Height)));
				}
			}
		}
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
		if (Screen.Level.Surfing == true)
		{
			bool canSurf = false;

			foreach (Entity Entity in Screen.Level.Entities)
			{
				if (Entity.boundingBox.Contains(Screen.Camera.GetForwardMovedPosition()) == ContainmentType.Contains)
				{
					if (Entity.ActionValue == 0 && (Entity.EntityID == "AnimatedBlock" || Entity.EntityID == "Water"))
						canSurf = true;
					else if (Entity.Collision == true)
					{
						canSurf = false;
						break;
					}
				}
			}

			if (canSurf == true)
			{
				Screen.Camera.Move(1);

				Screen.Level.PokemonEncounter.TryEncounterWildPokemon(this.Position, Spawner.EncounterMethods.Surfing, "");
			}
		}
	}

	private void Surf()
	{
		if (Screen.Camera.Turning == false)
		{
			if (Screen.Level.Surfing == false)
			{
				if (Badge.CanUseHMMove(Badge.HMMoves.Surf) == true | GameController.IS_DEBUG_ACTIVE == true | GameVariables.playerTrainer.SandBoxMode == true)
				{
					if (Screen.ChooseBox.Showing == false)
					{
						bool canSurf = false;

						if (this.ActionValue == 0)
						{
							foreach (Entity Entity in Screen.Level.Entities)
							{
								if (Entity.boundingBox.Contains(Screen.Camera.GetForwardMovedPosition()) == ContainmentType.Contains)
								{
									if (Entity.EntityID == "AnimatedBlock")
									{
										if (GameVariables.playerTrainer.SurfPokemon > -1)
											canSurf = true;
									}
									else if (Entity.Collision == true)
									{
										canSurf = false;
										break;
									}
								}
							}
						}

						if (Screen.Level.Riding == true)
							canSurf = false;

						if (canSurf == true)
						{
							string message = "Do you want to Surf?%Yes|No%";
							Screen.TextBox.Show(message, this, true, true);
							SoundManager.PlaySound("select");
						}
					}
				}
			}
		}
	}


	protected override float CalculateCameraDistance(Vector3 CPosition)
	{
		return base.CalculateCameraDistance(CPosition) - 0.25F;
	}

	public override void UpdateEntity()
	{
		if (!Animations == null)
		{
			for (var n = 0; n <= Animations.Count - 1; n++)
			{
				Animations[n].Update(0.01);
				if (currentRectangle[n] != Animations[n].TextureRectangle)
				{
					ChangeTexture(n);

					currentRectangle[n] = Animations[n].TextureRectangle;
				}
			}
		}
		base.UpdateEntity();
	}

	private void ChangeTexture(int n)
	{
		// If Core.GameOptions.GraphicStyle = 1 Then

		if (BlockTexturesTemp.Count == 0)
		{
			ClearAnimationResources();
			CreateBlockTextureTemp();
		}
		var i = Animations[n].CurrentRow;
		var j = Animations[n].CurrentColumn;
		this.Textures(n) = AnimatedBlock.BlockTexturesTemp[AnimationNames[n] + "_" + (j + columns[n] * i)];
	}

	public override void ResultFunction(int Result)
	{
		if (Result == 0)
		{
			Screen.TextBox.Show(GameVariables.playerTrainer.Pokemons(GameVariables.playerTrainer.SurfPokemon).GetDisplayName() + " used~Surf!", this);
			Screen.Level.Surfing = true;
			Screen.Camera.Move(1);
			PlayerStatistics.Track("Surf used", 1);

			{
				var withBlock = Screen.Level.OwnPlayer;
				GameVariables.playerTrainer.TempSurfSkin = withBlock.SkinName;

				int pokemonNumber = GameVariables.playerTrainer.Pokemons(GameVariables.playerTrainer.SurfPokemon).Number;
				string SkinName = "[POKEMON|N]" + pokemonNumber + PokemonForms.GetOverworldAddition(GameVariables.playerTrainer.Pokemons(GameVariables.playerTrainer.SurfPokemon));
				if (GameVariables.playerTrainer.Pokemons(GameVariables.playerTrainer.SurfPokemon).IsShiny == true)
					SkinName = "[POKEMON|S]" + pokemonNumber + PokemonForms.GetOverworldAddition(GameVariables.playerTrainer.Pokemons(GameVariables.playerTrainer.SurfPokemon));

				withBlock.SetTexture(SkinName, false);

				withBlock.UpdateEntity();

				SoundManager.PlayPokemonCry(pokemonNumber);

				if (Screen.Level.IsRadioOn == false || GameJolt.PokegearScreen.StationCanPlay(Screen.Level.SelectedRadioStation) == false)
					MusicManager.Play("surf", true);
			}
		}
	}

	public override void Render()
	{
		bool setRasterizerState = this.Model.ID != 0;

		this.Draw(this.Model, Textures, setRasterizerState);
	}
}
}