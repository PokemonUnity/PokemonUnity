using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSetting<T> : GameSetting {
    public T DefaultValue = default(T);

    public abstract T Get();

    public abstract void Set(T value);
}

[Serializable]
public class GameSetting : ScriptableObject {
    [SerializeField] protected EPlayerPrefKeys key = EPlayerPrefKeys.NONE;

    public virtual EPlayerPrefKeys Key { get => key; }

    public virtual string KeyString { get => GameSettings.KeyStrings[Key]; }

    class KeyDoesNotHaveThatSetterTypeError : Exception {
        public KeyDoesNotHaveThatSetterTypeError(EPlayerPrefKeys Key, Type intendedType) {
            Debug.LogError("Player preference key '" + Key.ToString() + "' does not have a setter for type " + intendedType.ToString());
        }
    }
}
