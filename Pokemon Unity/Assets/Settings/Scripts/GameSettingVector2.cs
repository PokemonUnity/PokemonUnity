using UnityEngine;

[CreateAssetMenu(fileName = "Vector2 Setting", menuName = "Pokemon Unity/Settings/Vector2")]
public class GameSettingVector2 : GameSetting<Vector2> {
    
    public override Vector2 Get() {
        return new Vector2(PlayerPrefs.GetFloat(KeyString + " x"), PlayerPrefs.GetFloat(KeyString + " y"));
    }

    public override void Set(Vector2 value) {
        PlayerPrefs.SetFloat(KeyString + " x", value.x);
        PlayerPrefs.SetFloat(KeyString + " y", value.y);
    }
}