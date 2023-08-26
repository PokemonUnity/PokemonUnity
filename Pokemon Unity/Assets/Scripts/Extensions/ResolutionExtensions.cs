using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResolutionExtensions
{
    public static string GetDisplayText(this Resolution resolution) {
        return $"{resolution.width} x {resolution.height} {resolution.refreshRate}Hz";
    }
}
