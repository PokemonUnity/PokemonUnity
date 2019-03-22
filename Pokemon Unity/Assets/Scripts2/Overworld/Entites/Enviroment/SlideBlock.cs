namespace PokemonUnity.Overworld.Entity.Environment
{
public class SlideBlock : Entity
{
    private ScriptBlock TempScriptEntity = null/* TODO Change to default(_) if this is not a reference type */;

    public override bool WalkAgainstFunction()
    {
        int facing = System.Convert.ToInt32(this.Rotation.Y / (double)MathHelper.PiOver2);
        facing -= 2;
        if (facing < 0)
            facing += 4;

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing & Screen.Camera.IsMoving() == false)
        {
            (OverworldCamera)Screen.Camera.DidWalkAgainst = false;

            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.Y = checkPosition.Y.ToInteger();

            bool foundSteps = true;
            while (foundSteps == true)
            {
                Entity e = GetEntity(Screen.Level.Entities, checkPosition, true, new System.Type[]
                {
                    typeof(SlideBlock),
                    typeof(ScriptBlock),
                    typeof(WarpBlock)
                });
                if (e != null)
                {
                    if (e.EntityID == "SlideBlock")
                    {
                        Steps += 1;
                        checkPosition.X += Screen.Camera.GetMoveDirection().X;
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z;
                        checkPosition.Y += 1;
                    }
                    else
                    {
                        if (e.EntityID == "ScriptBlock")
                            TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == "WarpBlock")
                            (WarpBlock)e.WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string s = "version=2" + Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().X + ",1," + Screen.Camera.GetMoveDirection().Z + ")" + Environment.NewLine + "@player.move(" + Steps + ")" + Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().X + ",0," + Screen.Camera.GetMoveDirection().Z + ")" + Environment.NewLine + "@pokemon.hide" + Environment.NewLine + "@player.move(1)" + Environment.NewLine + "@pokemon.hide" + Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
            return true;
        }

        facing = System.Convert.ToInt32(this.Rotation.Y / (double)MathHelper.PiOver2);
        if (facing < 0)
            facing += 4;
        if (Screen.Camera.GetPlayerFacingDirection() == facing)
            return false;

        return true;
    }

    private string GetScriptStartLine(ScriptBlock ScriptEntity)
    {
        if (ScriptEntity != null)
        {
            if (ScriptEntity.CorrectRotation() == true)
            {
                switch (ScriptEntity.GetActivationID())
                {
                    case 0:
                        {
                            return "@script.start(" + ScriptEntity.ScriptID + ")";
                        }
                    case 1:
                        {
                            return "@script.text(" + ScriptEntity.ScriptID + ")";
                        }
                    case 2:
                        {
                            return "@script.run(" + ScriptEntity.ScriptID + ")";
                        }
                }
            }
        }

        return "";
    }

    public override void WalkOntoFunction()
    {
        int facing = System.Convert.ToInt32(this.Rotation.Y / (double)MathHelper.PiOver2);

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing)
        {
            (OverworldCamera)Screen.Camera.DidWalkAgainst = false;

            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.Y = checkPosition.Y.ToInteger() - 1;

            bool foundSteps = true;
            while (foundSteps == true)
            {
                Entity e = GetEntity(Screen.Level.Entities, checkPosition, true, new System.Type[]
                {
                    typeof(SlideBlock),
                    typeof(ScriptBlock),
                    typeof(WarpBlock)
                });
                if (e != null)
                {
                    if (e.EntityID == "SlideBlock")
                    {
                        Steps += 1;
                        checkPosition.X += Screen.Camera.GetMoveDirection().X;
                        checkPosition.Z += Screen.Camera.GetMoveDirection().Z;
                        checkPosition.Y -= 1;
                    }
                    else
                    {
                        if (e.EntityID == "ScriptBlock")
                            this.TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == "WarpBlock")
                            (WarpBlock)e.WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string s = "version=2" + Environment.NewLine + "@player.move(1)" + Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().X + ",-1," + Screen.Camera.GetMoveDirection().Z + ")" + Environment.NewLine + "@player.move(" + Steps + ")" + Environment.NewLine + "@pokemon.hide" + Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}