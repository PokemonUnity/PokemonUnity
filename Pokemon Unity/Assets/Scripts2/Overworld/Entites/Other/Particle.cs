using System.Linq;

namespace PokemonUnity.Overworld.Entity.Misc
{
public class Particle : Entity
{
	public enum Behaviors
	{
		Floating,
		Falling,
		Rising,
		LeftToRight,
		RightToLeft
	}

	public float MoveSpeed = 0.01F;
	public float Delay = 10.0F;
	public Behaviors Behavior = Behaviors.Falling;

	public float Destination = 999.0F;

	private Vector3 Realtive = new Vector3(0);
	private Vector3 LastPosition;
	private float time = 0;

	public Particle(Vector3 Position, Texture2D[] Textures, int[] TextureIndex, int Rotation, Vector3 Scale, BaseModel Model, Vector3 Shader) : base(Position.X, Position.Y, Position.Z, "Particle", Textures, TextureIndex, false, Rotation, Scale, Model, 0, "", Shader)
	{
		this.NeedsUpdate = true;
		this.CreateWorldEveryFrame = true;

		if (Destination == 999.0F)
			this.Destination = this.Position.Y - 2.8F;

		this.DropUpdateUnlessDrawn = false;
		this.NormalOpacity = 0F;
	}

	public override void Update()
	{
		Screen.Identifications[] identifications = new[] { Screen.Identifications.OverworldScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen };
		if (identifications.Contains(Core.CurrentScreen.Identification) == true)
		{
			switch (this.Behavior)
			{
				case Behaviors.Falling:
					{
						this.Position.Y -= this.MoveSpeed;
						if (this.Position.Y <= this.Destination)
							this.CanBeRemoved = true;
						break;
					}
				case Behaviors.Floating:
					{
						break;
					}
				case Behaviors.Rising:
					{
						this.Position.Y -= this.MoveSpeed;
						if (this.Position.Y >= this.Destination)
							this.CanBeRemoved = true;
						break;
					}
				case Behaviors.LeftToRight:
					{
						this.Position.X += this.MoveSpeed;
						this.Position.Y -= this.MoveSpeed / (double)4;

						if (this.Position.X >= this.Destination)
							this.CanBeRemoved = true;
						break;
					}
				case Behaviors.RightToLeft:
					{
						this.Position.X += this.MoveSpeed;
						this.Position.Y += this.MoveSpeed / (double)4;

						if (this.Position.X >= this.Destination)
							this.CanBeRemoved = true;
						break;
					}
			}

			if (this.NormalOpacity < 1.0F)
			{
				this.NormalOpacity += 0.05F;
				if (this.NormalOpacity >= 1)
					this.NormalOpacity = 1.0F;
			}
		}
	}

	public void MoveWithCamera(Vector3 diff)
	{
		this.Position -= diff;
		switch (this.Behavior)
		{
			case Behaviors.Falling:
			//case Behaviors.Floating: // y
				{
					this.Destination -= diff.Y;
					break;
				}
			case Behaviors.Floating:
				{
					this.Destination += diff.Y;
					break;
				}
			default:
				{
					this.Destination -= diff.X;
					break;
				}
		}
	}

	protected override float CalculateCameraDistance(Vector3 CPosition)
	{
		return base.CalculateCameraDistance(CPosition) - 1000000F;
	}

	public override void UpdateEntity()
	{
		if (this.Rotation.Y != Screen.Camera.Yaw)
			this.Rotation.Y = Screen.Camera.Yaw;

		float c_pitch = Screen.Camera.Pitch;
		this.Rotation.X = c_pitch / (double)2.0F;

		base.UpdateEntity();
	}

	public override void Render()
	{
		base.Draw(this.Model, this.Textures, false);
	}
}
}