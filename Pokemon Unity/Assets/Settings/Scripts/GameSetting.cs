using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameSetting<T> : GameSetting {
    public T DefaultValue = default(T);
    [HideInInspector] public UnityEvent<T> OnValueChange;

    public abstract T Get();

    public abstract void Set(T value);
}

public class GameSetting : ScriptableObject {
    [SerializeField] protected EGameSettingKey key = EGameSettingKey.NONE;

    public virtual EGameSettingKey Key { get => key; }

    public virtual string KeyString { get => Key.ToString(); }

    class KeyDoesNotHaveThatSetterTypeError : Exception {
        public KeyDoesNotHaveThatSetterTypeError(EGameSettingKey Key, Type intendedType) {
            Debug.LogError("Player preference key '" + Key.ToString() + "' does not have a setter for type " + intendedType.ToString());
        }
    }
}
