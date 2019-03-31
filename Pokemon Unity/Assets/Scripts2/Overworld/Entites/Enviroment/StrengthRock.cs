using PokemonUnity.Character;
using PokemonUnity.Monster;
using UnityEngine;

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
			if (Game.Level.UsedStrength)
			{
				string text = "Pokémon with Strength are~able to move this.";
				Game.TextBox.Show(text, new Entity[] { this });
				SoundManager.PlaySound("select");
			}
			else
			{
				string pName = "";

				foreach (Monster.Pokemon p in Game.Player.Party)
				{
					if (!p.isEgg)
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

				if (pName != "" & Badge.CanUseHMMove(Badge.HMMoves.Strength) | Game.IS_DEBUG_ACTIVE | Game.Player.SandBoxMode)
					text += "~Do you want to~use Strength?%Yes|No%";

				Game.TextBox.Show(text, new Entity[] { this });
				SoundManager.PlaySound("select");
			}
		}

		public override void ResultFunction(int Result)
		{
			if (Result == 0)
			{
				Monster.Pokemon useP = null;// TODO Change to default(_) if this is not a reference type 

				foreach (Monster.Pokemon p in Game.Player.Party)
				{
					if (!p.isEgg)
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

				Game.Level.UsedStrength = true;

				SoundManager.PlayPokemonCry(pNumber);
				Game.TextBox.Show(pName + " used~Strength!", null, true, false);
				//PlayerStatistics.Track("Strength used", 1);
			}
		}

		public override bool WalkAgainstFunction()
		{
			if (Game.Level.UsedStrength & this.Moved == 0.0f)
			{
				Vector3 newPosition = Game.Camera.GetForwardMovedPosition();
				newPosition.y = newPosition.y.ToInteger();
				newPosition.x += Game.Camera.GetMoveDirection().x;
				newPosition.z += Game.Camera.GetMoveDirection().z;

				if (CheckCollision(newPosition))
				{
					this.Moved = 1;
					this.FaceDirection = Game.Camera.GetPlayerFacingDirection();
					SoundManager.PlaySound("destroy", false);
				}
			}

			return true;
		}

		private bool CheckCollision(Vector3 newPosition)
		{
			newPosition = new Vector3(System.Convert.ToInt32(newPosition.x), System.Convert.ToInt32(newPosition.y), System.Convert.ToInt32(newPosition.z));

			bool HasFloor = false;

			Vector3 Position2D = new Vector3(newPosition.x, newPosition.y - 0.1f, newPosition.z);
			foreach (Entity Floor in Game.Level.Floors)
			{
				if (Floor.boundingBox.Contains(Position2D))// == ContainmentType.Contains
					HasFloor = true;
			}

			if (!HasFloor)
				return false;

			foreach (Entity Entity in Game.Level.Entities)
			{
				if (Entity.boundingBox.Contains(newPosition))// == ContainmentType.Contains
				{
					if (Entity.Collision)
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
			//this.Draw(this.Model, Textures, false);
		}
	}
}