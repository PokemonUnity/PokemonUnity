using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Text")]
public class SingleSelectText : SingleSelect<string> {
    public override bool IsEqual(string value1, string value2) => value1 == value2;
}
