using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Array Setting", menuName = "Pokemon Unity/Settings/Float[]")]
public class GameSettingFloatArray : GameSettingPlayerPrefArray<float> {
    public override bool IsEqual(float value1, float value2) => value1 == value2;
}
