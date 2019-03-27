using PokemonUnity.Player;
using PokemonUnity.Pokemon;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class StrengthRock : Entity
{
    public override void Initialize()
    {
        base.Initialize();

        this.CanMove = true;
    }

    public override void ClickFunction()
    {
        if (GameVariables.Level.UsedStrength == true)
        {
            string text = "Pokémon with Strength are~able to move this.";
            Screen.TextBox.Show(text, this);
            SoundManager.PlaySound("select");
        }
        else
        {
            string pName = "";

            foreach (Pokemon.Pokemon p in GameVariables.playerTrainer.Party)
            {
                if (p.isEgg == false)
                {
                    foreach (Attack.Move a in p.moves)
                    {
                        if (a.MoveId == Moves.STRENGTH)
                        {
                            pName = p.Name;
                            break;
                        }
                    }
                }

                if (pName != "")
                    break;
            }

            string text = "A Pokémon may be~able to move this.";

            if (pName != "" & Badge.CanUseHMMove(Badge.HMMoves.Strength) == true | GameVariables.IS_DEBUG_ACTIVE == true | GameVariables.playerTrainer.SandBoxMode == true)
                text += "~Do you want to~use Strength?%Yes|No%";

            Screen.TextBox.Show(text, this);
            SoundManager.PlaySound("select");
        }
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            Pokemon.Pokemon useP = null;// TODO Change to default(_) if this is not a reference type 

            foreach (Pokemon.Pokemon p in GameVariables.playerTrainer.Party)
            {
                if (p.isEgg == false)
                {
                    foreach (Attack.Move a in p.moves)
                    {
                        if (a.MoveId == Moves.STRENGTH)
                        {
                            useP = p;
                            break;
                        }
                    }
                }

                if (useP != null)
                    break;
            }

            string pName = "MissignNo.";
            int pNumber = 23;

            if (useP != null)
            {
                pName = useP.Name;
                pNumber = (int)useP.Species;
            }

            GameVariables.Level.UsedStrength = true;

            SoundManager.PlayPokemonCry(pNumber);
            Screen.TextBox.Show(pName + " used~Strength!", null, true, false);
            PlayerStatistics.Track("Strength used", 1);
        }
    }

    public override bool WalkAgainstFunction()
    {
        if (GameVariables.Level.UsedStrength == true & this.Moved == 0.0F)
        {
            Vector3 newPosition = GameVariables.Camera.GetForwardMovedPosition();
            newPosition.y = newPosition.y.ToInteger();
            newPosition.x += GameVariables.Camera.GetMoveDirection().x;
            newPosition.z += GameVariables.Camera.GetMoveDirection().z;

            if (CheckCollision(newPosition) == true)
            {
                this.Moved = 1;
                this.FaceDirection = GameVariables.Camera.GetPlayerFacingDirection();
                SoundManager.PlaySound("destroy", false);
            }
        }

        return true;
    }

    private bool CheckCollision(Vector3 newPosition)
    {
        newPosition = new Vector3(System.Convert.ToInt32(newPosition.x), System.Convert.ToInt32(newPosition.y), System.Convert.ToInt32(newPosition.z));

        bool HasFloor = false;

        Vector3 Position2D = new Vector3(newPosition.x, newPosition.y - 0.1F, newPosition.z);
        foreach (Entity Floor in GameVariables.Level.Floors)
        {
            if (Floor.boundingBox.Contains(Position2D) == ContainmentType.Contains)
                HasFloor = true;
        }

        if (HasFloor == false)
            return false;

        foreach (Entity Entity in GameVariables.Level.Entities)
        {
            if (Entity.boundingBox.Contains(newPosition) == ContainmentType.Contains)
            {
                if (Entity.Collision == true)
                    return false;
            }
        }

        return true;
    }

    public override void UpdateEntity()
    {
        base.UpdateEntity();
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}