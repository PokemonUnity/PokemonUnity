using System;
using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Overworld.Entity.Environment
{
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
                ActionScript.TempInputDirection = GameVariables.Camera.GetPlayerFacingDirection();

            if (GameVariables.Camera.Name == "Overworld")
            {
                if (!(OverworldCamera)GameVariables.Camera.FreeCameraMode)
                    (OverworldCamera)GameVariables.Camera.YawLocked = true;
            }

            GameVariables.Level.WalkedSteps = 0;
            GameVariables.Level.PokemonEncounterData.EncounteredPokemon = false;
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
        if (this.ActivateScript & GameVariables.Camera.Position.x == this.Position.x & GameVariables.Camera.Position.z == this.Position.z & System.Convert.ToInt32(GameVariables.Camera.Position.y) == System.Convert.ToInt32(this.Position.y))
        {
            GameVariables.Camera.StopMovement();
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

            if (oS.ActionScript.IsReady | canAttach)
            {
                if (this.CorrectRotation())
                {
                    if (this.clickedToActivate)
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
            if (AcceptedRotations.Contains(GameVariables.Camera.GetPlayerFacingDirection()))
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
}