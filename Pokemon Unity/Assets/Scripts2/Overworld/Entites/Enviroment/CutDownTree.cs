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

public class CutDownTree : Entity
{
	public override void UpdateEntity()
	{
		if (this.Rotation.Y != Screen.Camera.Yaw)
		{
			this.Rotation.Y = Screen.Camera.Yaw;
			this.CreatedWorld = false;
		}

		base.UpdateEntity();
	}

	public override void ClickFunction()
	{
		string pName = "";

		foreach (Pokemon p in Core.Player.Pokemons)
		{
			if (p.IsEgg() == false)
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

		if (pName != "" & Badge.CanUseHMMove(Badge.HMMoves.Cut) == true | Core.Player.SandBoxMode == true | GameController.IS_DEBUG_ACTIVE == true)
			text += "~Do you want to~use Cut?%Yes|No%";

		Screen.TextBox.Show(text,
			new CutDownTree()
		{
			this
		});
		SoundManager.PlaySound("select");
	}

	public override void ResultFunction(int Result)
	{
		if (Result == 0)
		{
			string pName = "";

			foreach (Pokemon p in Core.Player.Pokemons)
			{
				if (p.IsEgg() == false)
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