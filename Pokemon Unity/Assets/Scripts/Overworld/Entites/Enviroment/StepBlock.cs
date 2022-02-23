using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class StepBlock : Entity
	{
		public override bool WalkAgainstFunction()
		{
			int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
			facing -= 2;
			if (facing < 0)
				facing += 4;

			if (Game.Camera.GetPlayerFacingDirection() == facing)
			{
				// Dim newPos As New Vector3(Game.Camera.Position.x + Game.Camera.moveDirectionX * 2, Game.Camera.Position.y, Game.Camera.Position.z + Game.Camera.moveDirectionZ * 2)
				// If CType(Game.Camera, OverworldCamera).CheckCollision(newPos) = True Then

				// End If
				Game.Camera.AddToPlannedMovement(new Vector3(0, 0.15f, 0));
				Game.Camera.Move(1.0f);
				Game.Level.OverworldPokemon.Visible = false;
				Game.Level.OverworldPokemon.warped = true;

				SoundManager.PlaySound("jump_ledge", false);

				return false;
			}
			return true;
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}
	}
}