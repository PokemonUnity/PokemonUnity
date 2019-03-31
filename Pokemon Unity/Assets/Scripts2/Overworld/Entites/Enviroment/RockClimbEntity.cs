using PokemonUnity.Character;
using PokemonUnity.Monster;
using System;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class RockClimbEntity : Entity
	{
		private ScriptBlock TempScriptEntity = null;// TODO Change to default(_) if this is not a reference type 
		private bool TempClicked = false; // If true, walk up.

		public override void ClickFunction()
		{
			if (Badge.CanUseHMMove(Badge.HMMoves.RockClimb) | GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
			{
				TempClicked = true;
				if (GetRockClimbPokemon() == null)
					GameVariables.TextBox.Show("A Pokémon could~climb this rock...", new Entity[] { this }, true, true);
				else
					GameVariables.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", new Entity[] { this }, true, true);
			}
		}

		public override void WalkOntoFunction()
		{
			if (Badge.CanUseHMMove(Badge.HMMoves.RockClimb) | GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
			{
				TempClicked = false;
				if (GetRockClimbPokemon() == null)
					GameVariables.TextBox.Show("A Pokémon could~climb this rock...", new Entity[] { this }, true, true);
				else
					GameVariables.TextBox.Show("A Pokémon could~climb this rock.*Do you want to~use Rock Climb?%Yes|No%", new Entity[] { this }, true, true);
				SoundManager.PlaySound("select");
			}
			else
				GameVariables.TextBox.Show("A path is engraved~into this rock...", new Entity[] { this }, true, true);
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				if (this.TempClicked)
					this.WalkUp();
				else
					this.WalkDown();
			}
		}

		private Monster.Pokemon GetRockClimbPokemon()
		{
			foreach (Monster.Pokemon teamPokemon in GameVariables.playerTrainer.Party)
			{
				if (!teamPokemon.isEgg)
				{
					foreach (Attack.Move a in teamPokemon.moves)
					{
						if (a.MoveId == Moves.ROCK_CLIMB)
							return teamPokemon;
					}
				}
			}

			// No rock climb in team:
			if (GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
			{
				if (GameVariables.playerTrainer.Party.GetCount() > 0)
					return GameVariables.playerTrainer.Party[0];
				else
				{
					Monster.Pokemon p = new Monster.Pokemon((Pokemons)10, false);//Pokemon.GetPokemonByID(10);
																				 //p.Generate(10, true);
					return p;
				}
			}
			else
				return null;// TODO Change to default(_) if this is not a reference type 
		}

		private void WalkUp()
		{
			int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
			facing -= 2;
			if (facing < 0)
				facing += 4;

			GameVariables.Camera.PlannedMovement = Vector3.zero;

			if (GameVariables.Camera.GetPlayerFacingDirection() == facing & !GameVariables.Camera.IsMoving)
			{
				int Steps = 0;

				Vector3 checkPosition = GameVariables.Camera.GetForwardMovedPosition();
				checkPosition.y = checkPosition.y.ToInteger();

				bool foundSteps = true;
				while (foundSteps)
				{
					Entity e = GetEntity(GameVariables.Level.Entities, checkPosition, true, new System.Type[]
					{
					typeof(RockClimbEntity),
					typeof(ScriptBlock),
					typeof(WarpBlock)
					});
					if (e != null)
					{
						if (e.EntityID == Entities.RockClimbEntity)
						{
							Steps += 1;
							checkPosition.x += GameVariables.Camera.GetMoveDirection().x;
							checkPosition.z += GameVariables.Camera.GetMoveDirection().z;
							checkPosition.y += 1;
						}
						else
						{
							if (e.EntityID == Entities.ScriptBlock)
								TempScriptEntity = (ScriptBlock)e;
							else if (e.EntityID == Entities.WarpBlock)
								((WarpBlock)e).WalkAgainstFunction();
							foundSteps = false;
						}
					}
					else
						foundSteps = false;
				}

				GameVariables.Level.OverworldPokemon.Visible = false;
				GameVariables.Level.OverworldPokemon.warped = true;

				string tempSkin = "";//GameVariables.playerTrainer.Skin;

				Monster.Pokemon RockClimbPokemon = GetRockClimbPokemon();

				//GameVariables.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture();
				GameVariables.Level.OwnPlayer.ChangeTexture();

				string s = "version=2" + System.Environment.NewLine + "@pokemon.cry(" + (int)RockClimbPokemon.Species + ")" + System.Environment.NewLine + "@player.setmovement(" + GameVariables.Camera.GetMoveDirection().x + ",1," + GameVariables.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@sound.play(destroy)" + System.Environment.NewLine + "@player.move(" + Steps + ")" + System.Environment.NewLine + "@player.setmovement(" + GameVariables.Camera.GetMoveDirection().x + ",0," + GameVariables.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.move(1)" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.wearskin(" + tempSkin + ")" + System.Environment.NewLine;

				if (this.TempScriptEntity != null)
				{
					s += GetScriptStartLine(this.TempScriptEntity) + System.Environment.NewLine;
					this.TempScriptEntity = null;
				}

				s += ":end";

				// Reset the player's transparency:
				GameVariables.Level.OwnPlayer.Opacity = 1.0f;

				//((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(s, 2, false);
			}

			facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);
			if (facing < 0)
				facing += 4;
		}

		private void WalkDown()
		{
			int facing = System.Convert.ToInt32(this.Rotation.y / (double)MathHelper.PiOver2);

			GameVariables.Camera.PlannedMovement = Vector3.zero;

			if (GameVariables.Camera.GetPlayerFacingDirection() == facing)
			{
				int Steps = 0;

				Vector3 checkPosition = GameVariables.Camera.GetForwardMovedPosition();
				checkPosition.y = checkPosition.y.ToInteger() - 1;

				bool foundSteps = true;
				while (foundSteps)
				{
					Entity e = GetEntity(GameVariables.Level.Entities, checkPosition, true, new System.Type[]
					{
					typeof(RockClimbEntity),
					typeof(ScriptBlock),
					typeof(WarpBlock)
					});
					if (e != null)
					{
						if (e.EntityID == Entities.RockClimbEntity)
						{
							Steps += 1;
							checkPosition.x += GameVariables.Camera.GetMoveDirection().x;
							checkPosition.z += GameVariables.Camera.GetMoveDirection().z;
							checkPosition.y -= 1;
						}
						else
						{
							if (e.EntityID == Entities.ScriptBlock)
								this.TempScriptEntity = (ScriptBlock)e;
							else if (e.EntityID == Entities.WarpBlock)
								((WarpBlock)e).WalkAgainstFunction();
							foundSteps = false;
						}
					}
					else
						foundSteps = false;
				}

				GameVariables.Level.OverworldPokemon.Visible = false;
				GameVariables.Level.OverworldPokemon.warped = true;

				string tempSkin = "";//GameVariables.playerTrainer.Skin;

				Monster.Pokemon RockClimbPokemon = GetRockClimbPokemon();

				//GameVariables.Level.OwnPlayer.Texture = RockClimbPokemon.GetOverworldTexture();
				GameVariables.Level.OwnPlayer.ChangeTexture();

				string s = "version=2" + System.Environment.NewLine + "@pokemon.cry(" + (int)RockClimbPokemon.Species + ")" + System.Environment.NewLine + "@player.move(1)" + System.Environment.NewLine + "@player.setmovement(" + GameVariables.Camera.GetMoveDirection().x + ",-1," + GameVariables.Camera.GetMoveDirection().z + ")" + System.Environment.NewLine + "@sound.play(destroy)" + System.Environment.NewLine + "@player.move(" + Steps + ")" + System.Environment.NewLine + "@pokemon.hide" + System.Environment.NewLine + "@player.wearskin(" + tempSkin + ")" + System.Environment.NewLine;

				if (this.TempScriptEntity != null)
				{
					s += GetScriptStartLine(this.TempScriptEntity) + System.Environment.NewLine;
					this.TempScriptEntity = null;
				}

				s += ":end";

				// Reset the player's transparency:
				GameVariables.Level.OwnPlayer.Opacity = 1.0f;

				//((OverworldScreen)Core.CurrentScreen).ActionScript.StartScript(s, 2, false);
			}
		}

		private string GetScriptStartLine(ScriptBlock ScriptEntity)
		{
			if (ScriptEntity != null)
			{
				if (ScriptEntity.CorrectRotation())
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
			//this.Draw(this.Model, Textures, false);
		}
	}
}