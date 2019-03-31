using System;
using System.Linq;
using PokemonUnity.Monster;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class HeadbuttTree : Entity
	{
		public override void UpdateEntity()
		{
			if (this.Rotation.y != Game.Camera.Yaw)
			{
				this.Rotation.y = Game.Camera.Yaw;
				CreatedWorld = false;
			}

			base.UpdateEntity();
		}

		public override void ClickFunction()
		{
			if (!Game.Level.Surfing)
			{
				string pName = "";

				foreach (Monster.Pokemon p in Game.Player.Party)
				{
					if (!p.isEgg)
					{
						foreach (Attack.Move a in p.moves)
						{
							if (a.MoveId == Moves.HEADBUTT)
							{
								pName = p.Name;
								break;
							}
						}

						if (pName != "")
							break;
					}
				}

				if (pName != "" & Game.Player.Badges.Contains(10))
				{
					string text = "This tree could have~a Pokémon in it.*Do you want to~use Headbutt?%Yes|No%";
					Game.TextBox.Show(text, new Entity[] { this });
					SoundManager.PlaySound("select");
				}
			}
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				string pName = "";

				foreach (Monster.Pokemon p in Game.Player.Party)
				{
					foreach (Attack.Move a in p.moves)
					{
						if (a.MoveId == Moves.HEADBUTT)
						{
							pName = p.Name;
							break;
						}
					}

					if (pName != "")
						break;
				}

				Monster.Pokemon spawnedPokemon = null;
				//Pokemon.Pokemon spawnedPokemon = Spawner.GetPokemon(Game.Level.LevelFile, EncounterTypes.Headbutt, false);
				if (spawnedPokemon == null)
				{
					string s = "version=2" + System.Environment.NewLine + "@text.show(" + pName + " used~Headbutt!)" + System.Environment.NewLine + "@sound.play(destroy,0)" + System.Environment.NewLine + "@level.wait(20)" + System.Environment.NewLine + "@text.show(Nothing happened...)" + System.Environment.NewLine + ":end";
					//((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(s, 2);
				}
				else
				{
					string s = "version=2" + System.Environment.NewLine + "@text.show(" + pName + " used~Headbutt!)" + System.Environment.NewLine + "@sound.play(destroy,0)" + System.Environment.NewLine + "@level.wait(20)" + System.Environment.NewLine + "@text.show(A wild Pokémon~appeared!)" + System.Environment.NewLine + "@battle.wild(" + (int)spawnedPokemon.Species + "," + spawnedPokemon.Level + ")" + System.Environment.NewLine + ":end";
					//((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(s, 2);
				}
			}
		}

		public override void Render()
		{
			//this.Draw(this.Model, Textures, false);
		}
	}
}