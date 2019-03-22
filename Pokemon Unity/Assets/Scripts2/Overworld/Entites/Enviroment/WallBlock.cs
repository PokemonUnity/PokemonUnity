namespace PokemonUnity.Overworld.Entity.Environment
{
public class WallBlock : Entity
{
    protected override float CalculateCameraDistance(Vector3 CPosition)
    {
        return base.CalculateCameraDistance(CPosition) - 0.2F;
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}