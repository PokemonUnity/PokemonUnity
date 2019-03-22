using System;
using System.Linq;
using PokemonUnity.Pokemon;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class HeadbuttTree : Entity
{
    public override void UpdateEntity()
    {
        if (this.Rotation.y != Screen.Camera.Yaw)
        {
            this.Rotation.y = Screen.Camera.Yaw;
            CreatedWorld = false;
        }

        base.UpdateEntity();
    }

    public override void ClickFunction()
    {
        if (Screen.Level.Surfing == false)
        {
            string pName = "";

            foreach (Pokemon p in GameVariables.playerTrainer.Party)
            {
                if (p.isEgg == false)
                {
                    foreach (BattleSystem.Attack a in p.Attacks)
                    {
                        if (a.Name == "Headbutt")
                        {
                            pName = p.GetDisplayName();
                            break;
                        }
                    }

                    if (pName != "")
                        break;
                }
            }

            if (pName != "" & GameVariables.playerTrainer.Badges.Contains(10))
            {
                string text = "This tree could have~a Pokémon in it.*Do you want to~use Headbutt?%Yes|No%";
                Screen.TextBox.Show(text, this);
                SoundManager.PlaySound("select");
            }
        }
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            string pName = "";

            foreach (Pokemon p in GameVariables.playerTrainer.Party)
            {
                foreach (BattleSystem.Attack a in p.Attacks)
                {
                    if (a.Name == "Headbutt")
                    {
                        pName = p.GetDisplayName();
                        break;
                    }
                }

                if (pName != "")
                    break;
            }

            Pokemon spawnedPokemon = Spawner.GetPokemon(Screen.Level.LevelFile, Spawner.EncounterMethods.Headbutt, false);
            if (spawnedPokemon == null)
            {
                string s = "version=2" + Environment.NewLine + "@text.show(" + pName + " used~Headbutt!)" + Environment.NewLine + "@sound.play(destroy,0)" + Environment.NewLine + "@level.wait(20)" + Environment.NewLine + "@text.show(Nothing happened...)" + Environment.NewLine + ":end";
                (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
            }
            else
            {
                string s = "version=2" + Environment.NewLine + "@text.show(" + pName + " used~Headbutt!)" + Environment.NewLine + "@sound.play(destroy,0)" + Environment.NewLine + "@level.wait(20)" + Environment.NewLine + "@text.show(A wild Pokémon~appeared!)" + Environment.NewLine + "@battle.wild(" + spawnedPokemon.Number + "," + spawnedPokemon.Level + ")" + Environment.NewLine + ":end";
                (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
            }
        }
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
    }
}
}