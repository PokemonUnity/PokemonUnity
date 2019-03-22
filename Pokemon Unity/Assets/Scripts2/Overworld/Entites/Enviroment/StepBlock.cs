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

        if (Screen.Camera.GetPlayerFacingDirection() == facing)
        {
            // Dim newPos As New Vector3(Screen.Camera.Position.x + Screen.Camera.moveDirectionX * 2, Screen.Camera.Position.y, Screen.Camera.Position.z + Screen.Camera.moveDirectionZ * 2)
            // If CType(Screen.Camera, OverworldCamera).CheckCollision(newPos) = True Then

            // End If
            Screen.Camera.AddToPlannedMovement(new Vector3(0, 0.15F, 0));
            Screen.Camera.Move(1.0F);
            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

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