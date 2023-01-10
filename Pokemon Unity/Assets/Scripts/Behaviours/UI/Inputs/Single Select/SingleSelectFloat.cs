using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Float")]
public class SingleSelectFloat : SingleSelect<float> {
    public override bool IsEqual(float value1, float value2) => value1 == value2;
}