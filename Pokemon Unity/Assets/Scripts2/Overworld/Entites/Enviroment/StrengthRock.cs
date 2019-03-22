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
        if (Screen.Level.UsedStrength == true)
        {
            string text = "Pokémon with Strength are~able to move this.";
            Screen.TextBox.Show(text, this);
            SoundManager.PlaySound("select");
        }
        else
        {
            string pName = "";

            foreach (Pokemon p in Core.Player.Pokemons)
            {
                if (p.isEgg == false)
                {
                    foreach (BattleSystem.Attack a in p.Attacks)
                    {
                        if (a.Name == "Strength")
                        {
                            pName = p.GetDisplayName();
                            break;
                        }
                    }
                }

                if (pName != "")
                    break;
            }

            string text = "A Pokémon may be~able to move this.";

            if (pName != "" & Badge.CanUseHMMove(Badge.HMMoves.Strength) == true | GameController.IS_DEBUG_ACTIVE == true | Core.Player.SandBoxMode == true)
                text += "~Do you want to~use Strength?%Yes|No%";

            Screen.TextBox.Show(text, this);
            SoundManager.PlaySound("select");
        }
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            Pokemon useP = null/* TODO Change to default(_) if this is not a reference type */;

            foreach (Pokemon p in Core.Player.Pokemons)
            {
                if (p.isEgg == false)
                {
                    foreach (BattleSystem.Attack a in p.Attacks)
                    {
                        if (a.Name == "Strength")
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
                pName = useP.GetDisplayName();
                pNumber = useP.Number;
            }

            Screen.Level.UsedStrength = true;

            SoundManager.PlayPokemonCry(pNumber);
            Screen.TextBox.Show(pName + " used~Strength!", null, true, false);
            PlayerStatistics.Track("Strength used", 1);
        }
    }

    public override bool WalkAgainstFunction()
    {
        if (Screen.Level.UsedStrength == true & this.Moved == 0.0F)
        {
            Vector3 newPosition = Screen.Camera.GetForwardMovedPosition();
            newPosition.Y = newPosition.Y.ToInteger();
            newPosition.X += Screen.Camera.GetMoveDirection().X;
            newPosition.Z += Screen.Camera.GetMoveDirection().Z;

            if (CheckCollision(newPosition) == true)
            {
                this.Moved = 1;
                this.FaceDirection = Screen.Camera.GetPlayerFacingDirection();
                SoundManager.PlaySound("destroy", false);
            }
        }

        return true;
    }

    private bool CheckCollision(Vector3 newPosition)
    {
        newPosition = new Vector3(System.Convert.ToInt32(newPosition.X), System.Convert.ToInt32(newPosition.Y), System.Convert.ToInt32(newPosition.Z));

        bool HasFloor = false;

        Vector3 Position2D = new Vector3(newPosition.X, newPosition.Y - 0.1F, newPosition.Z);
        foreach (Entity Floor in Screen.Level.Floors)
        {
            if (Floor.boundingBox.Contains(Position2D) == ContainmentType.Contains)
                HasFloor = true;
        }

        if (HasFloor == false)
            return false;

        foreach (Entity Entity in Screen.Level.Entities)
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