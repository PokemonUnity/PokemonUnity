using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector2 Setting", menuName = "Pokemon Unity/Settings/Vector2")]
public class GameSettingVector2 : GameSetting<Vector2> {
    protected override Action<Vector2> Setter => CustomSetter;

    void CustomSetter(Vector2 value) {
        PlayerPrefs.SetFloat(Key + " x", value.x);
        PlayerPrefs.SetFloat(Key + " y", value.y);
    }

    protected override Func<string, bool> HasValue { get => (string key) => PlayerPrefs.HasKey(Key + " x") && PlayerPrefs.HasKey(Key + " y"); }

    protected override Func<Vector2> Getter => () => new Vector2(PlayerPrefs.GetFloat(Key + " x"), PlayerPrefs.GetFloat(Key + " y"));
}