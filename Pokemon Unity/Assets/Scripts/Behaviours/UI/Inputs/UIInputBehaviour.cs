using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class UIInputBehaviour<T> : UIInputBehaviour {
    protected T currentValue;
    public UIEvents<T> Events;

    protected virtual new void Start() {
        if (!Application.isPlaying) return;
        // PlayerInput events
        SubscribeToPlayerInputEvents();
        // EventTrigger events
        if (!TryGetComponent(out EventTrigger eventTrigger)) eventTrigger = gameObject.AddComponent<EventTrigger>();
        Events.EventTrigger = eventTrigger;
        Events.EventTrigger.triggers = Events.Triggers;
        // Audio
        Audio.Initialize(gameObject);        
    }

    protected override void OnDestroy() {
        Input.Unsubscribe();
    }

    void SubscribeToPlayerInputEvents() {
        if (PlayerInputSingleton.Singleton == null) return;
        PlayerInputEvent event_ = new PlayerInputEvent("UI", "Navigate");
        event_.Callbacks.AddListener(Navigate);
        Input.Events.Add(event_);
        Input.Subscribe();
    }

    public abstract GameSetting<T> GameSetting { get; }

    public void SetGameSetting(T value) => GameSetting.Set(value);

    public void SetGameSetting() => GameSetting.Set(currentValue);

    public override void OnSelect(BaseEventData eventData) {
        if (Audio.SelectAudioSource == null) return;
        Audio.Play();
        Audio.SelectAudioSource.mute = false;
    }

    public void SelectSilently() {
        Audio.SelectAudioSource.mute = true;
        Select();
    }

    protected virtual void Navigate(CallbackContext context) {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;
        UpdateValue(context.ReadValue<Vector2>());
    }

    public virtual void UpdateValue(T value) {
        if (!Application.isPlaying) return;
        Audio.Play();
        GameSetting?.Set(value);
        currentValue = value;
        Events.OnValueChanged.Invoke(value);
    }

    public abstract void UpdateValue(Vector2 navigationDirection);
}

[AddComponentMenu("Pokemon Unity/UI/Generic Input")]
public class UIInputBehaviour : Selectable {
    [TextArea]
    public string HelpText;
    public UIAudio Audio;
    public PlayerInputEvents Input;

    #region Subclasses

    [Serializable]
    public class UIEvents {
        [HideInInspector]
        public EventTrigger EventTrigger;
        public List<Entry> Triggers;
    }

    [Serializable]
    public class UIEvents<T> : UIEvents {
        public UnityEvent<T> OnValueChanged;
    }

    [Serializable]
    public class UIAudio {
        public AudioClip SelectSound;
        [Description("if not provided, an audio source is created")]
        public AudioSource SelectAudioSource;
        float originalVolume;

        public void Initialize(GameObject gameObject) {
            if (SelectAudioSource == null) SelectAudioSource = gameObject.AddComponent<AudioSource>();
            if (SelectAudioSource == null) Debug.LogError("Failed to add an AudioSource for some reason");
            SelectAudioSource.clip = SelectSound;
            SelectAudioSource.playOnAwake = false;
            originalVolume = SelectAudioSource.volume;
        }

        public void Play() {
            AdjustVolume();
            SelectAudioSource.Play();
        }

        public void AdjustVolume() {
            SelectAudioSource.volume = originalVolume;
            SelectAudioSource.volume *= GlobalVariables.SFXVolumeSetting.Get();
        }
    }

    #endregion
}

[Serializable]
public class InputValue<T> {
    public string DisplayText;
    public T Value;

    public InputValue(string displayText, T value) {
        DisplayText = displayText;
        Value = value;
    }

    public InputValue(InputValue<T> inputValue) {
        DisplayText = inputValue.DisplayText;
        Value = inputValue.Value;
    }
}