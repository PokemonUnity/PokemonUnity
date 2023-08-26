using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Vector3 Setting", menuName = "Pokemon Unity/Settings/Vector3")]
public class GameSettingVector3 : GameSetting<Vector3> {
    protected override Action<Vector3> Setter => CustomSetter;

    void CustomSetter(Vector3 value) {
        PlayerPrefs.SetFloat(Key + " x", value.x);
        PlayerPrefs.SetFloat(Key + " y", value.y);
        PlayerPrefs.SetFloat(Key + " z", value.z);
    }

    protected override Func<string, bool> HasValue { get => (string key) => PlayerPrefs.HasKey(Key + " x") && PlayerPrefs.HasKey(Key + " y") && PlayerPrefs.HasKey(Key + " z"); }

    protected override Func<Vector3> Getter => () => new Vector3(PlayerPrefs.GetFloat(Key + " x"), PlayerPrefs.GetFloat(Key + " y"), PlayerPrefs.GetFloat(Key + " z"));
}