using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Resolution Setting", menuName = "Pokemon Unity/Settings/Resolution")]
public class GameSettingResolution : GameSettingPlayerPrefArray<Resolution> {
    //public static List<Resolution> Resolutions = new() { 
    //    new Resolution() { height }
    //};

    public override List<InputValue<Resolution>> Choices {
        get {
            SyncChoicesToResolution();
            return choices; 
        }
    }

    private void OnValidate() {
        SyncChoicesToResolution();
    }

    private void Awake() {
        SyncChoicesToResolution();
    }

    protected void SyncChoicesToResolution() {
        choices.Clear();
        foreach (Resolution resolution in Screen.resolutions) {
            choices.Add(new InputValue<Resolution>(resolution.ToString(), resolution));
        }
    }
}