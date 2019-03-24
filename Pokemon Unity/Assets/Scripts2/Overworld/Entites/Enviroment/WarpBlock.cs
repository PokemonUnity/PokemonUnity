using System.Collections.Generic;
using System.Linq;

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
        if (IsValidLink(this.AdditionalValue) == true & ScriptBlock.TriggeredScriptBlock == false)
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
                if (validRotations.Contains(Screen.Camera.GetPlayerFacingDirection()) == false)
                    return true;
            }

            if (System.IO.File.Exists(GameController.GamePath + @"\" + GameModeManager.ActiveGameMode.MapPath + destination) == true | System.IO.File.Exists(GameController.GamePath + @"\Content\Data\maps\" + destination) == true)
            {
                if (MapViewMode == false)
                {
                    Screen.Level.WarpData.WarpDestination = this.AdditionalValue.GetSplit(0);
                    Screen.Level.WarpData.WarpPosition = new Vector3(System.Convert.ToSingle(this.AdditionalValue.GetSplit(1)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(2).Replace(".", GameController.DecSeparator)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(3)));
                    Screen.Level.WarpData.WarpRotations = System.Convert.ToInt32(this.AdditionalValue.GetSplit(4));
                    Screen.Level.WarpData.DoWarpInNextTick = true;
                    Screen.Level.WarpData.CorrectCameraYaw = Screen.Camera.Yaw;
                    Screen.Level.WarpData.IsWarpBlock = true;
                    GameVariables.DebugLog("Lock Camera");
                    (OverworldCamera)Screen.Camera.YawLocked = true;
                }
                else
                {
                    Screen.Level = new Level();
                    Screen.Level.Load(this.AdditionalValue.GetSplit(0));
                    Screen.Level.World.Initialize(Screen.Level.EnvironmentType, Screen.Level.WeatherType);

                    Screen.Camera.Position = new Vector3(System.Convert.ToSingle(this.AdditionalValue.GetSplit(1)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(2).Replace(".", GameController.DecSeparator)), System.Convert.ToSingle(this.AdditionalValue.GetSplit(3)));
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
            if (link.Contains(",") == true)
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
                    if (destination.EndsWith(".dat") == true)
                    {
                        string x = link.GetSplit(1);
                        string y = link.GetSplit(2).Replace(".", GameController.DecSeparator);
                        string z = link.GetSplit(3);
                        string l = link.GetSplit(4);

                        if (StringHelper.IsNumeric(x) == true & StringHelper.IsNumeric(y) == true & StringHelper.IsNumeric(z) == true & StringHelper.IsNumeric(l) == true)
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