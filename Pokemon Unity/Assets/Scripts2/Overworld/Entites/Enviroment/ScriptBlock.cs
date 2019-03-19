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

public class ScriptBlock : Entity
{
    public static bool TriggeredScriptBlock = false;

    private int TriggerID = 0;
    private string _scriptID = "0";
    private List<int> AcceptedRotations = new List<int>();

    private bool ActivateScript = false;
    private bool clickedToActivate = false;

    public override void Initialize()
    {
        base.Initialize();

        this.TriggerID = this.ActionValue;
        if (this.AdditionalValue.Contains(","))
        {
            string[] Data = this.AdditionalValue.Split(System.Convert.ToChar(","));
            for (var i = 0; i <= Data.Count() - 2; i++)
                AcceptedRotations.Add(System.Convert.ToInt32(Data[i]));
            this._scriptID = Data[Data.Count() - 1];
        }
        else
            this._scriptID = this.AdditionalValue;

        this.NeedsUpdate = true;

        if (this.TriggerID == 0)
            this.Visible = false;
    }

    public override bool WalkIntoFunction()
    {
        if (this.TriggerID == 0 | this.TriggerID == 4)
        {
            ActivateScript = true;
            TriggeredScriptBlock = true;
            if (ActionScript.TempInputDirection == -1)
                ActionScript.TempInputDirection = Screen.Camera.GetPlayerFacingDirection();

            if (Screen.Camera.Name == "Overworld")
            {
                if ((OverworldCamera)Screen.Camera.FreeCameraMode == false)
                    (OverworldCamera)Screen.Camera.YawLocked = true;
            }

            Screen.Level.WalkedSteps = 0;
            Screen.Level.PokemonEncounterData.EncounteredPokemon = false;
        }

        return false;
    }

    public override void ClickFunction()
    {
        if (this.TriggerID == 1)
        {
            ActionScript.TempInputDirection = -1;
            this.clickedToActivate = true;
            TriggerScript(false);
        }
    }

    public override void Update()
    {
        if (this.ActivateScript == true & Screen.Camera.Position.X == this.Position.X & Screen.Camera.Position.Z == this.Position.Z & System.Convert.ToInt32(Screen.Camera.Position.Y) == System.Convert.ToInt32(this.Position.Y))
        {
            Screen.Camera.StopMovement();
            ActivateScript = false;
            TriggerScript(false);
        }

        base.Update();
    }

    public void TriggerScript(bool canAttach)
    {
        if (Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
        {
            OverworldScreen oS = (OverworldScreen)Core.CurrentScreen;

            if (oS.ActionScript.IsReady == true | canAttach == true)
            {
                if (this.CorrectRotation() == true)
                {
                    if (this.clickedToActivate == true)
                    {
                        this.clickedToActivate = false;
                        SoundManager.PlaySound("select");
                    }

                    oS.ActionScript.StartScript(this._scriptID, GetActivationID());
                    ActionScript.TempSpin = true;
                }
            }
        }
        TriggeredScriptBlock = false;
    }

    public int GetActivationID()
    {
        int activationID = 0;
        switch (this.TriggerID)
        {
            case 0:
            case 1:
            case 4:
                {
                    activationID = 0;
                    break;
                }

            case 2:
                {
                    activationID = 1;
                    break;
                }

            case 3:
                {
                    activationID = 2;
                    break;
                }
        }
        return activationID;
    }

    public bool CorrectRotation()
    {
        bool activate = false;
        if (AcceptedRotations.Count > 0)
        {
            if (AcceptedRotations.Contains(Screen.Camera.GetPlayerFacingDirection()))
                activate = true;
        }
        else
            activate = true;
        return activate;
    }

    public override void Render()
    {
        this.Draw(this.Model, Textures, true);
    }

    public string ScriptID
    {
        get
        {
            return this._scriptID;
        }
    }
}
