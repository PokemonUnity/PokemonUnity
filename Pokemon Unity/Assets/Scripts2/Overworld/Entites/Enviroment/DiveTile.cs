using PokemonUnity.Pokemon;
using System;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class DiveTile : Entity
{
    private int diveUp = 0;

    public override void Initialize()
    {
        base.Initialize();
        this.diveUp = this.ActionValue;
        this.NeedsUpdate = true;
    }

    public override void Update()
    {
        if (Screen.Level.Surfing == true)
        {
            if (System.Convert.ToInt32(this.Position.x) == System.Convert.ToInt32(Screen.Camera.Position.x) & System.Convert.ToInt32(this.Position.y) == System.Convert.ToInt32(Screen.Camera.Position.y) & System.Convert.ToInt32(this.Position.z) == System.Convert.ToInt32(Screen.Camera.Position.z))
            {
                if (Controls.Accept(true, true, true) == true)
                {
                    if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
                    {
                        if ((OverworldScreen)Core.CurrentScreen.ActionScript.IsReady)
                            this.StartDive();
                    }
                }
            }
        }
    }

    private void StartDive()
    {
        if (diveUp == 0)
        {
            // Down
            string t = "The water seems to be~really deep at this point.";
            string d = GetDivePokemon();

            if (d != "" & Badge.CanUseHMMove(Badge.HMMoves.Dive) == true | GameVariables.playerTrainer.SandBoxMode == true | GameController.IS_DEBUG_ACTIVE == true)
                t += "*Do you want to~use Dive?%Yes|No%";
            Screen.TextBox.Show(t, (DiveTile)this);
            SoundManager.PlaySound("select");
        }
        else if (diveUp == 1)
        {
            // Up
            string t = "Light shines down from~the surface.*Do you want to~use Dive?%Yes|No%";

            Screen.TextBox.Show(t, (DiveTile)this);
            SoundManager.PlaySound("select");
        }
        else if (diveUp == 2)
        {
            // Up
            string t = "The boat's shadow is cast~upon the ocean floor.*Do you want to~use Dive?%Yes|No%";

            Screen.TextBox.Show(t, (DiveTile)this);
            SoundManager.PlaySound("select");
        }
    }

    public override void ResultFunction(int result)
    {
        if (result == 0)
        {
            if (diveUp == 0)
            {
                // Down
                string s = "version=2" + Environment.NewLine + "@text.show(" + GetDivePokemon() + "~used Dive!)" + Environment.NewLine + "@screen.fadeout" + Environment.NewLine + "@player.warp(" + this.AdditionalValue + ")" + Environment.NewLine + "@level.update" + Environment.NewLine + "@player.setmovement(0,-0.5,0)" + Environment.NewLine + "@screen.fadein" + Environment.NewLine + "@player.move(8)" + Environment.NewLine + "@player.resetmovement" + Environment.NewLine + ":end";

                (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
            }
            else if (diveUp == 1)
            {
                // Up
                string s = "version=2" + Environment.NewLine + "@text.show(" + GetDivePokemon() + "~used Dive!)" + Environment.NewLine + "@player.setmovement(0,0.5,0)" + Environment.NewLine + "@player.move(8)" + Environment.NewLine + "@player.resetmovement" + Environment.NewLine + "@screen.fadeout" + Environment.NewLine + "@player.warp(" + this.AdditionalValue + ")" + Environment.NewLine + "@level.update" + Environment.NewLine + "@screen.fadein" + Environment.NewLine + ":end";

                (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
            }
            else if (diveUp == 2)
            {
                // Up
                string s = "version=2" + Environment.NewLine + "@text.show(" + GetDivePokemon() + "~used Dive!)" + Environment.NewLine + "@player.setmovement(0,0.5,0)" + Environment.NewLine + "@player.move(6)" + Environment.NewLine + "@player.resetmovement" + Environment.NewLine + "@screen.fadeout" + Environment.NewLine + "@player.warp(" + this.AdditionalValue + ")" + Environment.NewLine + "@level.update" + Environment.NewLine + "@screen.fadein" + Environment.NewLine + ":end";

                (OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2);
            }
        }
    }

    private string GetDivePokemon()
    {
        foreach (Pokemon p in GameVariables.playerTrainer.Pokemons)
        {
            foreach (BattleSystem.Attack a in p.Attacks)
            {
                if (a.Name.ToLower() == "dive")
                    return p.GetDisplayName();
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