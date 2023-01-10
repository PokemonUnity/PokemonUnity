using UnityEngine;

[CreateAssetMenu(fileName = "Texture2D Array Setting", menuName = "Pokemon Unity/Settings/Texture2D[]")]
public class GameSettingTexture2DArray : GameSettingPlayerPrefArray<Texture2D> {
    public override bool IsEqual(Texture2D value1, Texture2D value2) => value1 == value2;
}