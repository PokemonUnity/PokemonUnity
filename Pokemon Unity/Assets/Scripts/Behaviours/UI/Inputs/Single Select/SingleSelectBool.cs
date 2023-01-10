using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Bool")]
public class SingleSelectBool : SingleSelect<bool> {
    public override bool IsEqual(bool value1, bool value2) => value1 == value2;
}
