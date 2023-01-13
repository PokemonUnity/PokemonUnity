using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputConsumer : MonoBehaviour
{
    public PlayerInputEvents Events;

    void Start() {
        Events.Subscribe();
    }

    void OnDestroy() {
        Events.Unsubscribe();
    }
}

[Serializable]
public class PlayerInputEvents {
    public List<PlayerInputEvent> Events;
    bool hasSubscribed = false;

    public void Unsubscribe() {
        if (!hasSubscribed) return;
        if (PlayerInputSingleton.Singleton == null)
            throw new PlayerInputSingleton.PlayerInputNotFoundError();

        PlayerInput playerInput = PlayerInputSingleton.Singleton;

        foreach (PlayerInputEvent inputEvent in Events) {
            UnityEvent<InputAction.CallbackContext> event_ = playerInput.GetEvent(inputEvent.ActionMap, inputEvent.Action);
            event_.RemoveListener(inputEvent.Callbacks.Invoke);
        }
        hasSubscribed = false;
    }

    public void Subscribe() {
        hasSubscribed = true;
        if (PlayerInputSingleton.Singleton == null)
            throw new PlayerInputSingleton.PlayerInputNotFoundError();

        PlayerInput playerInput = PlayerInputSingleton.Singleton;
        foreach (PlayerInputEvent inputEvent in Events) {
            UnityEvent<InputAction.CallbackContext> event_ = playerInput.GetEvent(inputEvent.ActionMap, inputEvent.Action);
            event_.AddListener(inputEvent.Callbacks.Invoke);
        }
    }
}

[Serializable]
public class PlayerInputEvent {
    public string ActionMap;
    public string Action;
    public UnityEvent<InputAction.CallbackContext> Callbacks;

    public PlayerInputEvent(string actionMap, string action) {
        Action = action;
        ActionMap = actionMap;
        Callbacks = new();
    }
}