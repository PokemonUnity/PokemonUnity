using UnityEngine;

public class GameSettingArray<T> : GameSetting<int> {
    public T[] Choices;

    public override int Get() => PlayerPrefs.GetInt(KeyString);
    public override void Set(int index) => PlayerPrefs.SetInt(KeyString, index);
}