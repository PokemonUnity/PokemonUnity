using PokemonUnity.Character;
using PokemonUnity.Monster;
using System;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class CutDownTree : Entity
	{
		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
			{
				this.Rotation.y = Game.Camera.Yaw;
				this.CreatedWorld = false;
			}

			base.UpdateEntity();
		}

		public override void ClickFunction()
		{
			string pName = "";

			foreach (Monster.Pokemon p in Game.playerTrainer.Party)
			{
				if (!p.isEgg)
				{
					foreach (Attack.Move a in p.moves)
					{
						if (a.MoveId == Moves.CUT)
						{
							pName = p.Name;
							break;
						}
					}
				}

				if (pName != "")
					break;
			}

			string text = "This tree looks like it~can be Cut down!";

			if (pName != "" & Badge.CanUseHMMove(Badge.HMMoves.Cut) | Game.playerTrainer.SandBoxMode | Game.IS_DEBUG_ACTIVE)
				text += "~Do you want to~use Cut?%Yes|No%";

			Game.TextBox.Show(text, new Entity[] { this });
			SoundManager.PlaySound("select");
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				string pName = "";

				foreach (Monster.Pokemon p in Game.playerTrainer.Party)
				{
					if (!p.isEgg)
					{
						foreach (Attack.Move a in p.moves)
						{
							if (a.MoveId == Moves.CUT)
							{
								pName = p.Name;
								break;
							}
						}
					}

					if (pName != "")
						break;
				}

				string Text = pName + " used~Cut!";
				this.CanBeRemoved = true;

				string s = "version=2" + System.Environment.NewLine + "@text.show(" + Text + ")" + System.Environment.NewLine + "@sound.play(destroy,0)" + System.Environment.NewLine + ":end";

				//PlayerStatistics.Track("Cut used", 1);
				//((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(s, 2, false);
			}
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}
	}
}