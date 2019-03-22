using PokemonUnity.Pokemon;
using System;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class CutDownTree : Entity
{
	public override void UpdateEntity()
	{
		if (this.Rotation.y != Screen.Camera.Yaw)
		{
			this.Rotation.y = Screen.Camera.Yaw;
			this.CreatedWorld = false;
		}

		base.UpdateEntity();
	}

	public override void ClickFunction()
	{
		string pName = "";

		foreach (Pokemon p in GameVariables.playerTrainer.Party)
		{
			if (p.isEgg == false)
			{
				foreach (BattleSystem.Attack a in p.Attacks)
				{
					if (a.Name == "Cut")
					{
						pName = p.GetDisplayName();
						break;
					}
				}
			}

			if (pName != "")
				break;
		}

		string text = "This tree looks like it~can be Cut down!";

		if (pName != "" & Badge.CanUseHMMove(Badge.HMMoves.Cut) == true | GameVariables.playerTrainer.SandBoxMode == true | GameController.IS_DEBUG_ACTIVE == true)
			text += "~Do you want to~use Cut?%Yes|No%";

		Screen.TextBox.Show(text, this);
		SoundManager.PlaySound("select");
	}

	public override void ResultFunction(int Result)
	{
		if (Result == 0)
		{
			string pName = "";

			foreach (Pokemon p in GameVariables.playerTrainer.Party)
			{
				if (p.isEgg == false)
				{
					foreach (BattleSystem.Attack a in p.Attacks)
					{
						if (a.Name == "Cut")
						{
							pName = p.GetDisplayName();
							break;
						}
					}
				}

				if (pName != "")
					break;
			}

			string Text = pName + " used~Cut!";
			this.CanBeRemoved = true;

			string s = "version=2" + Environment.NewLine + "@text.show(" + Text + ")" + Environment.NewLine + "@sound.play(destroy,0)" + Environment.NewLine + ":end";

			PlayerStatistics.Track("Cut used", 1);
			(OverworldScreen)Core.CurrentScreen.ActionScript.StartScript(s, 2, false);
		}
	}

	public override void Render()
	{
		this.Draw(this.Model, Textures, false);
	}
}
}