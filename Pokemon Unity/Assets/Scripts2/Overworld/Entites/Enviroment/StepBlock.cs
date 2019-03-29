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

        if (GameVariables.Camera.GetPlayerFacingDirection() == facing)
        {
            // Dim newPos As New Vector3(GameVariables.Camera.Position.x + GameVariables.Camera.moveDirectionX * 2, GameVariables.Camera.Position.y, GameVariables.Camera.Position.z + GameVariables.Camera.moveDirectionZ * 2)
            // If CType(GameVariables.Camera, OverworldCamera).CheckCollision(newPos) = True Then

            // End If
            GameVariables.Camera.AddToPlannedMovement(new Vector3(0, 0.15f, 0));
            GameVariables.Camera.Move(1.0f);
            GameVariables.Level.OverworldPokemon.Visible = false;
            GameVariables.Level.OverworldPokemon.warped = true;

            SoundManager.PlaySound("jump_ledge", false);

            return false;
        }
        return true;
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}