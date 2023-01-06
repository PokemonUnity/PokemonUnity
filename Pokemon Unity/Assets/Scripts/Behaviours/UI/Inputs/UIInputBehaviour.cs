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

public interface PlayerPrefSetter<T> {
    public abstract void SetPlayerPref(T value);
    public abstract void SetPlayerPref();
}

public abstract class UIInputBehaviour<T> : UIInputBehaviour, PlayerPrefSetter<T> {
    [Description("Automatically updates the player pref if selected option is not NONE")]
    public EPlayerPrefKeys PlayerPreferenceKey = EPlayerPrefKeys.NONE;

    protected new void Start() {
        base.Start();
        if (PlayerPreferenceKey != EPlayerPrefKeys.NONE) OnValueChange.AddListener(SetPlayerPref);
    }

    public abstract void SetPlayerPref(T value);

    public abstract void SetPlayerPref();

    public virtual void UpdateValue(InputValue<T> value) {
        OnValueChange.Invoke(value.Value);
    }

    public abstract UnityEvent<T> OnValueChange { get; }
}

public class UIInputBehaviour : Selectable {
    public UIAudio Audio;
    public UIInput Input;
    public UIEvents Events;

    public string HelpText;

    protected new void Start() {
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

    public override void OnSelect(BaseEventData eventData) {
        if (Audio.SelectSoundSource == null) return;
        Audio.SelectSoundSource.Play();
        Audio.SelectSoundSource.mute = false;
    }

    public void SilentSelect() {
        Audio.SelectSoundSource.mute = true;
        Select();
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

    public virtual void UpdateValue(Vector2 navigationDirection) { }

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
    public class UIAudio {
        public AudioClip SelectSound;
        [HideInInspector] public AudioSource SelectSoundSource;
    }

    [Serializable]
    public class InputValue<T> {
        public string DisplayText;
        public T Value;
    }
}
