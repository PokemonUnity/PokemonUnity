namespace PokemonUnity.Overworld.Entity.Environment
{
public class SlideBlock : Entity
{
    private ScriptBlock TempScriptEntity = null;// TODO Change to default(_) if this is not a reference type 

    public override bool WalkAgainstFunction()
    {
        int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
        facing -= 2;
        if (facing < 0)
            facing += 4;

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing & Screen.Camera.IsMoving() == false)
        {
            (OverworldCamera)Screen.Camera.DidWalkAgainst = false;

            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.y = checkPosition.y.ToInteger();

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
                    if (e.EntityID == Entities.SlideBlock)
                    {
                        Steps += 1;
                        checkPosition.x += Screen.Camera.GetMoveDirection().x;
                        checkPosition.z += Screen.Camera.GetMoveDirection().z;
                        checkPosition.y += 1;
                    }
                    else
                    {
                        if (e.EntityID == Entities.ScriptBlock)
                            TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == Entities.WarpBlock)
                            ((WarpBlock)e).WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string s = "version=2" + System.Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().x + ",1," + Screen.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@player.move(" + Steps + ")" + System.Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().x + ",0," + Screen.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.move(1)" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + System.Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
            return true;
        }

        facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
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
        int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing)
        {
            (OverworldCamera)Screen.Camera.DidWalkAgainst = false;

            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.y = checkPosition.y.ToInteger() - 1;

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
                    if (e.EntityID == Entities.SlideBlock)
                    {
                        Steps += 1;
                        checkPosition.x += Screen.Camera.GetMoveDirection().x;
                        checkPosition.z += Screen.Camera.GetMoveDirection().z;
                        checkPosition.y -= 1;
                    }
                    else
                    {
                        if (e.EntityID == Entities.ScriptBlock)
                            this.TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == Entities.WarpBlock)
                            ((WarpBlock)e).WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string s = "version=2" + System.Environment.NewLine + "@player.move(1)" + System.Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().x + ",-1," + Screen.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@player.move(" + Steps + ")" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + System.Environment.NewLine;
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