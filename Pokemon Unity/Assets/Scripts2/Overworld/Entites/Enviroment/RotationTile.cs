using UnityEngine;

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
        //if (this.RotationType == RotationTypes.StartSpin)
        //{
        //    if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
        //    {
        //        if ((OverworldScreen)Core.CurrentScreen.ActionScript.IsReady)
        //        {
        //            if (this.Position.x == GameVariables.Camera.Position.x & System.Convert.ToInt32(this.Position.y) == System.Convert.ToInt32(GameVariables.Camera.Position.y) & this.Position.z == GameVariables.Camera.Position.z)
        //            {
        //                int steps = GetSteps();
		//	
        //                string s = "version=2" + System.Environment.NewLine + "@player.move(0)" + System.Environment.NewLine + "@player.turnto(" + this.RotateTo.ToString() + ")" + System.Environment.NewLine + "@player.move(" + steps + ")" + System.Environment.NewLine + ":end";
		//	
        //                (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
        //            }
        //        }
        //    }
        //}
    }

    private int GetSteps()
    {
        int steps = 0;
        Vector2 direction = new Vector2(0,0);
        switch (this.RotateTo)
        {
            case 0:
                {
                    direction.y = -1;
                    break;
                }
            case 1:
                {
                    direction.x = -1;
                    break;
                }
            case 2:
                {
                    direction.y = 1;
                    break;
                }
            case 3:
                {
                    direction.x = 1;
                    break;
                }
        }

        int stepY = System.Convert.ToInt32(direction.y);
        if (stepY == 0)
            stepY = 1;

        for (float x = 0; x <= direction.x * 100; x += direction.x)
        {
            for (var y = 0; y <= direction.y * 100; y += stepY)
            {
                Vector3 p = new Vector3(x, 0, y) + this.Position;
                foreach (Entity e in GameVariables.Level.Entities)
                {
                    if (!e.Equals(this))
                    {
                        if (e.EntityID == Entities.RotationTile)
                        {
                            if (System.Convert.ToInt32(e.Position.x) == System.Convert.ToInt32(p.x) & System.Convert.ToInt32(e.Position.y) == System.Convert.ToInt32(p.y) & System.Convert.ToInt32(e.Position.z) == System.Convert.ToInt32(p.z))
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
 //this.Draw(this.Model, Textures, false);
    }

    public override bool LetPlayerMove()
    {
        return this.RotationType == RotationTypes.StopSpin;
    }

    public override bool WalkIntoFunction()
    {
        //if (this.RotationType == RotationTypes.StartSpin)
        //    ((OverworldCamera)GameVariables.Camera).YawLocked = true;
        return false;
    }
}
}