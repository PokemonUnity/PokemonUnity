namespace PokemonUnity.Overworld.Entity.Environment
{
public class TurningSign : Entity
{
    private float TurningSpeed = 0.01F;

    public override void Initialize()
    {
        base.Initialize();

        float randomValue = System.Convert.ToSingle(MathHelper.TwoPi * Core.Random.NextDouble());
        switch (this.ActionValue)
        {
            case 1:
                {
                    this.Rotation.X = randomValue;
                    break;
                }
            case 2:
                {
                    this.Rotation.Z = randomValue;
                    break;
                }
            default:
                {
                    this.Rotation.Y = randomValue;
                    break;
                }
        }

        if (StringHelper.IsNumeric(this.AdditionalValue) == true)
            this.TurningSpeed = System.Convert.ToSingle(System.Convert.ToDouble(this.AdditionalValue) / (double)100);
        this.CreateWorldEveryFrame = true;
    }

    public override void UpdateEntity()
    {
        switch (this.ActionValue)
        {
            case 1:
                {
                    this.Rotation.X += TurningSpeed;
                    break;
                }
            case 2:
                {
                    this.Rotation.Z += TurningSpeed;
                    break;
                }
            default:
                {
                    this.Rotation.Y += TurningSpeed;
                    break;
                }
        }

        base.UpdateEntity();
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, true);
    }
}
}