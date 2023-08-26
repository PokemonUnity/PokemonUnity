using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIListInput<T> : UIInputBehaviour<T>
{
    [Description("This value is not used if a GameSetting is provided")]
    [SerializeField] protected int defaultChoiceIndex = 0;
    [SerializeField]
    [Description("Choices is synced with the Game Settings if it implements IGameSettingArray")]
    protected GameSetting<T> gameSetting;
    public List<InputValue<T>> Choices;
    protected List<InputValue<T>> savedChoices = new();
    protected int activeIndex = -1;
    bool hasSyncedChoices = false;

    public override GameSetting<T> GameSetting { get => gameSetting; }

    protected new virtual void OnValidate() {
        SyncChoices();
    }

    protected override void Start() {
        base.Start();
        savedChoices = Choices.ConvertAll(choice => new InputValue<T>(choice));
        SyncChoices();
        activeIndex = GetInitialIndex();
        SyncChildren();
        UpdateValueIndex(activeIndex);
    }

    public int GetInitialIndex() {
        if (GameSetting == null) return defaultChoiceIndex;
        else return Choices.FindIndex((InputValue<T> value) => Compare(value.Value, gameSetting.Get()));
    }

    public virtual void SyncChoices() {
        if (GameSetting is IGameSettingArray<T>) {
            // deepcopy the values from the Game Setting
            hasSyncedChoices = true;
            Choices = (gameSetting as IGameSettingArray<T>).Choices.ConvertAll(choice => new InputValue<T>(choice));
        } else if (hasSyncedChoices) {
            Choices = savedChoices;
        }
    }

    protected abstract void SyncChildren();

    public bool Compare(T x, T y) {
        return EqualityComparer<T>.Default.Equals(x, y);
    }

    public abstract void UpdateValueIndex(int index);

    public override void UpdateValue(Vector2 navigationDirection) {
        if (navigationDirection.magnitude == 0) return;
        //Audio.Play();
        UpdateValueIndex(activeIndex + (int)navigationDirection.x);
    }

    public virtual void UpdateValue(InputValue<T> value) {
        UpdateValue(value.Value);
    }
}
