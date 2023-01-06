using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefKeys
{
    public static Dictionary<EPlayerPrefKeys, string> Keys = new() {
        { EPlayerPrefKeys.NONE, "" },
        { EPlayerPrefKeys.TEXTSPEED, "Text Speed" },
        { EPlayerPrefKeys.FULLSCREEN, "Fullscreen" },
    };
}

public enum EPlayerPrefKeys {
    NONE,
    TEXTSPEED,
    FULLSCREEN
}
