using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameSettingPlayerPrefArray<T> : GameSetting<T>, IGameSettingArray<T> {
    public List<InputValue<T>> choices;

    public List<InputValue<T>> Choices => choices;

    public int GetIndex() => Exists() ? PlayerPrefs.GetInt(Key) : -1;

    protected override Func<T> Getter => () => Choices[PlayerPrefs.GetInt(Key)].Value;

    public InputValue<T> GetInputValue() => Choices[PlayerPrefs.GetInt(Key)];

    protected override Action<T> Setter => setter;

    protected override Func<string, bool> HasValue => hasValue;

    public abstract bool IsEqual(T value1, T value2);

    void setter(T value) {
        int index = Choices.FindIndex((InputValue<T> value2) => IsEqual(value2.Value, value));
        if (index == -1) throw new InvalidValueGivenToSetter<T>(value, Choices);
        PlayerPrefs.SetInt(Key, index);
    }

    bool hasValue(string key) {
        if (!PlayerPrefs.HasKey(key)) return false;
        int index = PlayerPrefs.GetInt(key);
        return index >= 0 && index < Choices.Count;
    }

    class InvalidValueGivenToSetter<U> : Exception {
        public InvalidValueGivenToSetter(U value, List<InputValue<U>> options) {
            Debug.LogError("Setting provided does not exist as a possible option.\nProvided option: " + value.ToString() + "\nOptions: " + options.ToString());
        }
    }
}