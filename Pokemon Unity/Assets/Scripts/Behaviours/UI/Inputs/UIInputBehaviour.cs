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
        Audio.SelectSoundSource = gameObject.AddComponent<AudioSource>();
        if (Audio.SelectSoundSource is null) Debug.LogError("Failed to add an AudioSource for some reason");
        Audio.SelectSoundSource.playOnAwake = false;
        Audio.SelectSoundSource.clip = Audio.SelectSound;
    }

    void SubscribeToPlayerInputEvents() {
        if (PlayerInputSingleton.Singleton == null) return;
        var navigateEvent = FindNavigateActionEvents();
        navigateEvent.AddListener((CallbackContext context) => Input.OnNavigate.Invoke(context));
        Input.OnNavigate.AddListener(Navigate);
    }

    protected PlayerInput.ActionEvent FindNavigateActionEvents() {
        PlayerInput playerInput = PlayerInputSingleton.Singleton;
        if (playerInput == null) return null;
        if (playerInput == null) throw new PlayerInputSingleton.PlayerInputNotFoundError();
        var uiActionMap = playerInput.actions.FindActionMap("UI").FindAction("Navigate");
        return playerInput.actionEvents.First((actionEvent) => actionEvent.actionId == uiActionMap.id.ToString());
    }

    protected virtual void Navigate(CallbackContext context) {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;
        UpdateValue(context.ReadValue<Vector2>());
    }

    public virtual void UpdateValue(InputValue<T> value) {
        if (!Application.isPlaying) return;
        Audio.SelectSoundSource.Play();
        if (GameSetting != null) { 
            GameSetting.Set(value.Value);
        }
        Events.OnValueChanged.Invoke(value.Value);
    }

    public abstract GameSetting<T> GameSetting { get; }

    public void SetGameSetting(T value) => GameSetting.Set(value);

    public void SetGameSetting() => GameSetting.Set(currentValue);

    public override void OnSelect(BaseEventData eventData) {
        if (Audio.SelectSoundSource == null) return;
        Audio.SelectSoundSource.Play();
        Audio.SelectSoundSource.mute = false;
    }

    public void SilentSelect() {
        Audio.SelectSoundSource.mute = true;
        Select();
    }

    public abstract void UpdateValue(Vector2 navigationDirection);
}

[AddComponentMenu("Pokemon Unity/UI/Generic Input")]
public class UIInputBehaviour : Selectable {
    public string HelpText;
    public UIAudio Audio;
    public UIInput Input;

    #region Subclasses

    [Serializable]
    public class UIInput {
        [Header("Player Input Events Passthrough")]
        [Space]
        public UnityEvent<CallbackContext> OnNavigate;
    }

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
        [HideInInspector] public AudioSource SelectSoundSource;
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