using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameSetting<T> : GameSetting {
    [InspectorName("Settings Key")]
    public string Key;
    [SerializeField] protected T defaultValue = default(T);
    [HideInInspector] public UnityEvent<T> OnValueChange;

    protected virtual T DefaultValue { get => defaultValue; }

    public virtual bool HasValidKey() => Key != null && Key != "";

    /// <summary>A function that takes in a settings key string, and returns a value of type T</summary>
    protected abstract Func<T> Getter { get; }

    /// <summary>A function that takes in the new value</summary>
    protected abstract Action<T> Setter { get; }

    protected abstract Func<string, bool> HasValue { get; }

    public virtual T Get() => HasValue(Key) ? Getter() : DefaultValue;

    public virtual void Set(T value) {
        if (Compare(value, Get())) return;
        Setter(value);
        OnValueChange.Invoke(value);
    }

    public virtual bool Exists() => HasValue(Key);

    public bool Compare(T x, T y) {
        return EqualityComparer<T>.Default.Equals(x, y);
    }
}

public class GameSetting : ScriptableObject {
    public class NoGameSettingComponentSet : Exception {
        public NoGameSettingComponentSet(Type type, GameObject gameObject) {
            Debug.LogError("Missing Game Setting component of type " + type.ToString(), gameObject);
        }
    }
}
