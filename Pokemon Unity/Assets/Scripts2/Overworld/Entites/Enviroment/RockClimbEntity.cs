using PokemonUnity.Pokemon;
using System;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class RockClimbEntity : Entity
{
    private ScriptBlock TempScriptEntity = null/* TODO Change to default(_) if this is not a reference type */;
    private bool TempClicked = false; // If true, walk up.

    public override void ClickFunction()
    {
        if (Badge.CanUseHMMove(Badge.HMMoves.RockClimb) == true | GameController.IS_DEBUG_ACTIVE == true | GameVariables.playerTrainer.SandBoxMode == true)
        {
            TempClicked = true;
            if (GetRockClimbPokemon() == null)
                Screen.TextBox.Show("A Pokémon could~climb this rock...", this, true, true);
            else
                Screen.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", this, true, true);
        }
    }

    public override void WalkOntoFunction()
    {
        if (Badge.CanUseHMMove(Badge.HMMoves.RockClimb) == true | GameController.IS_DEBUG_ACTIVE == true | GameVariables.playerTrainer.SandBoxMode == true)
        {
            TempClicked = false;
            if (GetRockClimbPokemon() == null)
                Screen.TextBox.Show("A Pokémon could~climb this rock...", this, true, true);
            else
                Screen.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", this, true, true);
            SoundManager.PlaySound("select");
        }
        else
            Screen.TextBox.Show("A path is engraved~into this rock...", this, true, true);
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            if (this.TempClicked == true)
                this.WalkUp();
            else
                this.WalkDown();
        }
    }

    private Pokemon GetRockClimbPokemon()
    {
        foreach (Pokemon teamPokemon in GameVariables.playerTrainer.Party)
        {
            if (teamPokemon.isEgg == false)
            {
                foreach (Attack.Move a in teamPokemon.moves)
                {
                    if (a.Name.ToLower() == "rock climb")
                        return teamPokemon;
                }
            }
        }

        // No rock climb in team:
        if (GameController.IS_DEBUG_ACTIVE == true | GameVariables.playerTrainer.SandBoxMode == true)
        {
            if (GameVariables.playerTrainer.Party.GetCount() > 0)
                return GameVariables.playerTrainer.Party[0];
            else
            {
                Pokemon p = Pokemon.GetPokemonByID(10);
                p.Generate(10, true);
                return p;
            }
        }
        else
            return null/* TODO Change to default(_) if this is not a reference type */;
    }

    private void WalkUp()
    {
        int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
        facing -= 2;
        if (facing < 0)
            facing += 4;

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing & Screen.Camera.IsMoving == false)
        {
            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.y = checkPosition.y.ToInteger();

            bool foundSteps = true;
            while (foundSteps == true)
            {
                Entity e = GetEntity(Screen.Level.Entities, checkPosition, true, new System.Type[]
                {
                    typeof(RockClimbEntity),
                    typeof(ScriptBlock),
                    typeof(WarpBlock)
                });
                if (e != null)
                {
                    if (e.EntityID.ToLower() == "rockclimbentity")
                    {
                        Steps += 1;
                        checkPosition.x += Screen.Camera.GetMoveDirection().x;
                        checkPosition.z += Screen.Camera.GetMoveDirection().z;
                        checkPosition.y += 1;
                    }
                    else
                    {
                        if (e.EntityID == "ScriptBlock")
                            TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == "WarpBlock")
                            ((WarpBlock)e).WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string tempSkin = GameVariables.playerTrainer.Skin;

            Pokemon RockClimbPokemon = GetRockClimbPokemon();

            Screen.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture();
            Screen.Level.OwnPlayer.ChangeTexture();

            string s = "version=2" + System.Environment.NewLine + "@pokemon.cry(" + RockClimbPokemon.Number + ")" + System.Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().x + ",1," + Screen.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@sound.play(destroy)" + System.Environment.NewLine + "@player.move(" + Steps + ")" + System.Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().x + ",0," + Screen.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.move(1)" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.wearskin(" + tempSkin + ")" + System.Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + System.Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            // Reset the player's transparency:
            Screen.Level.OwnPlayer.Opacity = 1.0F;

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
        }

        facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
        if (facing < 0)
            facing += 4;
    }

    private void WalkDown()
    {
        int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);

        Screen.Camera.PlannedMovement = Vector3.Zero;

        if (Screen.Camera.GetPlayerFacingDirection() == facing)
        {
            int Steps = 0;

            Vector3 checkPosition = Screen.Camera.GetForwardMovedPosition();
            checkPosition.y = checkPosition.y.ToInteger() - 1;

            bool foundSteps = true;
            while (foundSteps == true)
            {
                Entity e = GetEntity(Screen.Level.Entities, checkPosition, true, new System.Type[]
                {
                    typeof(RockClimbEntity),
                    typeof(ScriptBlock),
                    typeof(WarpBlock)
                });
                if (e != null)
                {
                    if (e.EntityID == "RockClimbEntity")
                    {
                        Steps += 1;
                        checkPosition.x += Screen.Camera.GetMoveDirection().x;
                        checkPosition.z += Screen.Camera.GetMoveDirection().z;
                        checkPosition.y -= 1;
                    }
                    else
                    {
                        if (e.EntityID == "ScriptBlock")
                            this.TempScriptEntity = (ScriptBlock)e;
                        else if (e.EntityID == "WarpBlock")
                            ((WarpBlock)e).WalkAgainstFunction();
                        foundSteps = false;
                    }
                }
                else
                    foundSteps = false;
            }

            Screen.Level.OverworldPokemon.Visible = false;
            Screen.Level.OverworldPokemon.warped = true;

            string tempSkin = GameVariables.playerTrainer.Skin;

            Pokemon RockClimbPokemon = GetRockClimbPokemon();

            Screen.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture();
            Screen.Level.OwnPlayer.ChangeTexture();

            string s = "version=2" + System.Environment.NewLine + "@pokemon.cry(" + RockClimbPokemon.Number + ")" + System.Environment.NewLine + "@player.move(1)" + System.Environment.NewLine + "@player.setmovement(" + Screen.Camera.GetMoveDirection().x + ",-1," + Screen.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@sound.play(destroy)" + System.Environment.NewLine + "@player.move(" + Steps + ")" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.wearskin(" + tempSkin + ")" + System.Environment.NewLine;

            if (this.TempScriptEntity != null)
            {
                s += GetScriptStartLine(this.TempScriptEntity) + System.Environment.NewLine;
                this.TempScriptEntity = null;
            }

            s += ":end";

            // Reset the player's transparency:
            Screen.Level.OwnPlayer.Opacity = 1.0F;

            (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
        }
    }

    private string GetScriptStartLine(ScriptBlock ScriptEntity)
    {
        if (!ScriptEntity == null)
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

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}