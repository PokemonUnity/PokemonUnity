using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public class HeadbuttTree : Entity
{
    public override void UpdateEntity()
    {
        if (this.Rotation.Y != Screen.Camera.Yaw)
        {
            this.Rotation.Y = Screen.Camera.Yaw;
            CreatedWorld = false;
        }

        base.UpdateEntity();
    }

    public override void ClickFunction()
    {
        if (Screen.Level.Surfing == false)
        {
            string pName = "";

            foreach (Pokemon p in Core.Player.Pokemons)
            {
                if (p.IsEgg() == false)
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

            if (pName != "" & Core.Player.Badges.Contains(10))
            {
                string text = "This tree could have~a Pokémon in it.*Do you want to~use Headbutt?%Yes|No%";
                Screen.TextBox.Show(text,
                {
                    this
                });
                SoundManager.PlaySound("select");
            }
        }
    }

    public override void ResultFunction(int Result)
    {
        if (Result == 0)
        {
            string pName = "";

            foreach (Pokemon p in Core.Player.Pokemons)
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
