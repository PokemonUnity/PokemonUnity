using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PokemonUnity.Overworld.Entity.Environment
{
public class WarpBlock : Entity
{
    public override void Render()
    {
        this.Draw(this.Model, Textures, false);
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
                if (!validRotations.Contains(GameVariables.Camera.GetPlayerFacingDirection()))
                    return true;
            }

            if (System.IO.File.Exists(GameController.GamePath + @"\" + GameModeManager.ActiveGameMode.MapPath + destination) | System.IO.File.Exists(GameController.GamePath + @"\Content\Data\maps\" + destination))
            {
                if (!MapViewMode)
                {
                    GameVariables.Level.WarpData.WarpDestination = this.AdditionalValue.GetSplit(0);
                    GameVariables.Level.WarpData.WarpPosition = new Vector3(System.Convert.ToSingle(this.AdditionalValue.GetSplit(1)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(2).Replace(".", StringHelper.DecSeparator)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(3)));
                    GameVariables.Level.WarpData.WarpRotations = System.Convert.ToInt32(this.AdditionalValue.GetSplit(4));
                    GameVariables.Level.WarpData.DoWarpInNextTick = true;
                    GameVariables.Level.WarpData.CorrectCameraYaw = GameVariables.Camera.Yaw;
                    GameVariables.Level.WarpData.IsWarpBlock = true;
                    GameVariables.DebugLog("Lock Camera");
                    ((OverworldCamera)GameVariables.Camera).YawLocked = true;
                }
                else
                {
                    GameVariables.Level = new Level();
                    GameVariables.Level.Load(this.AdditionalValue.GetSplit(0));
                    GameVariables.Level.World.Initialize(GameVariables.Level.EnvironmentType, GameVariables.Level.WeatherType);

                    GameVariables.Camera.Position = new Vector3(System.Convert.ToSingle(this.AdditionalValue.GetSplit(1)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(2).Replace(".", StringHelper.DecSeparator)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(3)));
                }
            }
            else
                CallError("Map file \"" + GameModeManager.ActiveGameMode.MapPath + destination + "\" does not exist.");
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
        GameVariables.DebugLog("WarpBlock.vb: Invalid warp! More information:" + ex, true);
    }
}
}