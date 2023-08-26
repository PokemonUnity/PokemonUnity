using System;
using UnityEngine;

public abstract class GameSettingPlayerPref<T> : GameSetting<T> {
    [SerializeField] T value;

    void OnValidate() {
        Set(value);
    }

    private int myVar;

    protected override Func<string, bool> HasValue { get => PlayerPrefs.HasKey; }
    protected override Action<T> Setter { get => setter; }

    void setter(T value) {
        this.value = value;
        PrefSetter(Key, value);
    }

    protected override Func<T> Getter { get => () => PrefGetter(Key); }
    protected abstract Action<string, T> PrefSetter { get; }
    protected abstract Func<string, T> PrefGetter { get; }
}