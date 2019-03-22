namespace PokemonUnity.Overworld.Entity.Environment
{
public class RotationTile : Entity
{
    public enum RotationTypes
    {
        StartSpin,
        StopSpin
    }

    private RotationTypes RotationType;
    private int RotateTo = 0;

    public override void Initialize()
    {
        base.Initialize();

        switch (this.ActionValue)
        {
            case 0:
                {
                    this.RotationType = RotationTypes.StartSpin;
                    break;
                }

            case 1:
                {
                    this.RotationType = RotationTypes.StopSpin;
                    break;
                }
        }

        this.RotateTo = System.Convert.ToInt32(this.AdditionalValue);
        this.NeedsUpdate = true;
    }

    public override void Update()
    {
        if (this.RotationType == RotationTypes.StartSpin)
        {
            if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
            {
                if ((OverworldScreen)Core.CurrentScreen.ActionScript.IsReady == true)
                {
                    if (this.Position.X == Screen.Camera.Position.X & System.Convert.ToInt32(this.Position.Y) == System.Convert.ToInt32(Screen.Camera.Position.Y) & this.Position.Z == Screen.Camera.Position.Z)
                    {
                        int steps = GetSteps();

                        string s = "version=2" + Environment.NewLine + "@player.move(0)" + Environment.NewLine + "@player.turnto(" + this.RotateTo.ToString() + ")" + Environment.NewLine + "@player.move(" + steps + ")" + Environment.NewLine + ":end";

                        (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
                    }
                }
            }
        }
    }

    private int GetSteps()
    {
        int steps = 0;
        Vector2 direction = new Vector2(0);
        switch (this.RotateTo)
        {
            case 0:
                {
                    direction.Y = -1;
                    break;
                }

            case 1:
                {
                    direction.X = -1;
                    break;
                }

            case 2:
                {
                    direction.Y = 1;
                    break;
                }

            case 3:
                {
                    direction.X = 1;
                    break;
                }
        }

        int stepY = System.Convert.ToInt32(direction.Y);
        if (stepY == 0)
            stepY = 1;

        for (var x = 0; x <= direction.X * 100; x += direction.X)
        {
            for (var y = 0; y <= direction.Y * 100; y += stepY)
            {
                Vector3 p = new Vector3(x, 0, y) + this.Position;
                foreach (Entity e in Screen.Level.Entities)
                {
                    if (e.Equals(this) == false)
                    {
                        if (e.EntityID.ToLower() == "rotationtile")
                        {
                            if (System.Convert.ToInt32(e.Position.X) == System.Convert.ToInt32(p.X) & System.Convert.ToInt32(e.Position.Y) == System.Convert.ToInt32(p.Y) & System.Convert.ToInt32(e.Position.Z) == System.Convert.ToInt32(p.Z))
                                goto theend;
                        }
                    }
                }
                steps += 1;
            }
        }

    theend:
        ;
        return steps;
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }

    public override bool LetPlayerMove()
    {
        return this.RotationType == RotationTypes.StopSpin;
    }

    public override bool WalkIntoFunction()
    {
        if (this.RotationType == RotationTypes.StartSpin)
            (OverworldCamera)Screen.Camera.YawLocked = true;
        return false;
    }
}
}