using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
	public class WarpBlock : Entity
	{
		public override void Render()
		{
	 //this.Draw(this.Model, Textures, false);
		}

		public override bool WalkAgainstFunction()
		{
			return Warp(false);
		}

		public bool Warp(bool MapViewMode)
		{
			if (IsValidLink(this.AdditionalValue) & !ScriptBlock.TriggeredScriptBlock)
			{
				string destination = this.AdditionalValue.GetSplit(0);

				string link = this.AdditionalValue;
				int c = 0;
				for (var e = 0; e <= link.Length - 1; e++)
				{
					if (link[e] == System.Convert.ToChar(","))
						c += 1;
				}
				if (c >= 5)
				{
					List<int> validRotations = new List<int>();

					string[] rotationData = link.GetSplit(5, ",").Split(System.Convert.ToChar("|"));
					foreach (string Element in rotationData)
						validRotations.Add(System.Convert.ToInt32(Element));
					if (!validRotations.Contains(Game.Camera.GetPlayerFacingDirection()))
						return true;
				}

				if (System.IO.File.Exists(Saving.SaveManager.saveLocation + @"\" + /*GameModeManager.ActiveGameMode.*/"" + destination) | System.IO.File.Exists(Saving.SaveManager.saveLocation + @"\Content\Data\maps\" + destination))
				{
					if (!MapViewMode)
					{
						Game.Level.WarpData.WarpDestination = this.AdditionalValue.GetSplit(0);
						Game.Level.WarpData.WarpPosition = new Vector3(System.Convert.ToSingle(this.AdditionalValue.GetSplit(1)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(2).Replace(".", StringHelper.DecSeparator)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(3)));
						Game.Level.WarpData.WarpRotations = System.Convert.ToInt32(this.AdditionalValue.GetSplit(4));
						Game.Level.WarpData.DoWarpInNextTick = true;
						Game.Level.WarpData.CorrectCameraYaw = Game.Camera.Yaw;
						Game.Level.WarpData.IsWarpBlock = true;
						Game.DebugLog("Lock Camera");
						//((OverworldCamera)Game.Camera).YawLocked = true;
					}
					else
					{
						Game.Level = new Level();
						Game.Level.Load(this.AdditionalValue.GetSplit(0));
						Game.Level.World.Initialize(Game.Level.EnvironmentType, Game.Level.WeatherType);

						Game.Camera.Position = new Vector3(System.Convert.ToSingle(this.AdditionalValue.GetSplit(1)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(2).Replace(".", StringHelper.DecSeparator)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(3)));
					}
				}
				else
					CallError("Map file \"" + /*GameModeManager.ActiveGameMode.MapPath*/"" + destination + "\" does not exist.");
			}

			return false;
		}

		public static bool IsValidLink(string link)
		{
			if (link != "")
			{
				if (link.Contains(","))
				{
					int c = 0;
					for (var e = 0; e <= link.Length - 1; e++)
					{
						if (link[e] == System.Convert.ToChar(","))
							c += 1;
					}
					if (c >= 4)
					{
						string destination = link.GetSplit(0);
						if (destination.EndsWith(".dat"))
						{
							string x = link.GetSplit(1);
							string y = link.GetSplit(2).Replace(".", StringHelper.DecSeparator);
							string z = link.GetSplit(3);
							string l = link.GetSplit(4);

							if (StringHelper.IsNumeric(x) & StringHelper.IsNumeric(y) & StringHelper.IsNumeric(z) & StringHelper.IsNumeric(l))
								return true;
							else
							{
								CallError("Position values are not numeric.");
								return false;
							}
						}
						else
						{
							CallError("Destination file is not a valid map file.");
							return false;
						}
					}
					else
					{
						CallError("Not enough or too much arguments to resolve the link.");
						return false;
					}
				}
				else
				{
					CallError("Link does not contain seperators or has wrong seperators.");
					return false;
				}
			}
			else
			{
				CallError("Link is empty.");
				return false;
			}
		}

		private static void CallError(string ex)
		{
			Game.DebugLog("WarpBlock.vb: Invalid warp! More information:" + ex, true);
		}
	}
}