using System.Linq;
using UnityEngine;

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

	public float MoveSpeed = 0.01f;
	public float Delay = 10.0f;
	public Behaviors Behavior = Behaviors.Falling;

	public float Destination = 999.0f;

	private Vector3 Realtive = new Vector3(0,0,0);
	private Vector3 LastPosition;
	private float time = 0;

	public Particle(Vector3 Position, Texture2D[] Textures, int[] TextureIndex, int Rotation, Vector3 Scale, UnityEngine.Mesh Model, Vector3 Shader) : base(Position.x, Position.y, Position.z, Entities.Particle, Textures, TextureIndex, false, Rotation, Scale, Model, 0, "", Shader)
	{
		this.NeedsUpdate = true;
		this.CreateWorldEveryFrame = true;

		if (Destination == 999.0f)
			this.Destination = this.Position.y - 2.8f;

		this.DropUpdateUnlessDrawn = false;
		this.NormalOpacity = 0F;
	}

	public override void Update()
	{
		//Screen.Identifications[] identifications = new[] { Screen.Identifications.OverworldScreen, Screen.Identifications.MainMenuScreen, Screen.Identifications.BattleScreen, Screen.Identifications.BattleCatchScreen };
		//if (identifications.Contains(Core.CurrentScreen.Identification))
		//{
		//	switch (this.Behavior)
		//	{
		//		case Behaviors.Falling:
		//			{
		//				this.Position.y -= this.MoveSpeed;
		//				if (this.Position.y <= this.Destination)
		//					this.CanBeRemoved = true;
		//				break;
		//			}
		//		case Behaviors.Floating:
		//			{
		//				break;
		//			}
		//		case Behaviors.Rising:
		//			{
		//				this.Position.y -= this.MoveSpeed;
		//				if (this.Position.y >= this.Destination)
		//					this.CanBeRemoved = true;
		//				break;
		//			}
		//		case Behaviors.LeftToRight:
		//			{
		//				this.Position.x += this.MoveSpeed;
		//				this.Position.y -= this.MoveSpeed / (float)4;
		//
		//				if (this.Position.x >= this.Destination)
		//					this.CanBeRemoved = true;
		//				break;
		//			}
		//		case Behaviors.RightToLeft:
		//			{
		//				this.Position.x += this.MoveSpeed;
		//				this.Position.y += this.MoveSpeed / (float)4;
		//
		//				if (this.Position.x >= this.Destination)
		//					this.CanBeRemoved = true;
		//				break;
		//			}
		//	}
		//
		//	if (this.NormalOpacity < 1.0f)
		//	{
		//		this.NormalOpacity += 0.05f;
		//		if (this.NormalOpacity >= 1)
		//			this.NormalOpacity = 1.0f;
		//	}
		//}
	}

	public void MoveWithCamera(Vector3 diff)
	{
		this.Position -= diff;
		switch (this.Behavior)
		{
			case Behaviors.Falling:
			//case Behaviors.Floating: // y
				{
					this.Destination -= diff.y;
					break;
				}
			case Behaviors.Floating:
				{
					this.Destination += diff.y;
					break;
				}
			default:
				{
					this.Destination -= diff.x;
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
		if (this.Rotation.y != GameVariables.Camera.Yaw)
			this.Rotation.y = GameVariables.Camera.Yaw;

		float c_pitch = GameVariables.Camera.Pitch;
		this.Rotation.x = c_pitch / (float)2.0f;

		base.UpdateEntity();
	}

	public override void Render()
	{
		base.Draw(this.Model, this.Textures, false);
	}
}
}