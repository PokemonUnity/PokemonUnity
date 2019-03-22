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
                    this.Rotation.x = randomValue;
                    break;
                }
            case 2:
                {
                    this.Rotation.z = randomValue;
                    break;
                }
            default:
                {
                    this.Rotation.y = randomValue;
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
                    this.Rotation.x += TurningSpeed;
                    break;
                }
            case 2:
                {
                    this.Rotation.z += TurningSpeed;
                    break;
                }
            default:
                {
                    this.Rotation.y += TurningSpeed;
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