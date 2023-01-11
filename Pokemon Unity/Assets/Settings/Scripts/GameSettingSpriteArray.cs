using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprite Array Setting", menuName = "Pokemon Unity/Settings/Sprite[]")]
public class GameSettingSpriteArray : GameSettingPlayerPrefArray<Sprite> {
    public override bool IsEqual(Sprite value1, Sprite value2) => value1 == value2;
}
