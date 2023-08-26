using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputConsumer : MonoBehaviour
{
    public PlayerInputEvents Events;

    void Start() {
        Events.Subscribe(gameObject);
    }

    void OnDestroy() {
        Events.Unsubscribe();
    }

    public void SwitchActionMap(string actionMapName) {
        int index = Events.PlayerInput.actions.actionMaps.IndexOf((InputActionMap actionMap) => actionMap.name == actionMapName);
        if (index == -1) {
            Debug.LogError($"PlayerInput does not have the Action Map named '{actionMapName}'");
            return;
        }
        Events.PlayerInput.SwitchCurrentActionMap(actionMapName);
    }
}

[Serializable]
public class PlayerInputEvents {
    [Description("If null, tries to \n" +
        "1. Grab a PlayerInput off of the attached GameObject\n" +
        "2. Grab the PlayerInputSingleton\n\n" +
        "Events only invoke when the PlayerInput's behaviour must be set to 'Invoke Unity Events'")]
    PlayerInput playerInput;
    public List<PlayerInputEvent> Events;
    bool hasSubscribed = false;

    public PlayerInput PlayerInput => GetPlayerInput();

    public void Unsubscribe() {
        if (!hasSubscribed || playerInput == null || !Application.isPlaying) return;

        foreach (PlayerInputEvent inputEvent in Events) {
            UnityEvent<InputAction.CallbackContext> event_ = playerInput.GetEvent(inputEvent.ActionMap, inputEvent.Action);
            if (event_ != null)
                event_.RemoveListener(inputEvent.Callbacks.Invoke);
        }
        hasSubscribed = false;
    }

    public void Subscribe(GameObject gameObject) {
        playerInput = GetPlayerInput(gameObject);
        
        foreach (PlayerInputEvent inputEvent in Events) {
            UnityEvent<InputAction.CallbackContext> event_ = playerInput.GetEvent(inputEvent.ActionMap, inputEvent.Action);
            if (event_ != null) event_.AddListener(inputEvent.Callbacks.Invoke);
        }
        hasSubscribed = true;
    }

    PlayerInput GetPlayerInput(GameObject gameObject) {
        if (playerInput != null)
            return playerInput;

        playerInput = gameObject.GetComponent<PlayerInput>();

        if (playerInput == null)
            playerInput = PlayerInputSingleton.Singleton;

        if (playerInput == null)
            throw new NoPlayerInputFound(gameObject);

        return playerInput;
    }

    PlayerInput GetPlayerInput() {
        if (playerInput != null)
            return playerInput;

        if (playerInput == null)
            playerInput = PlayerInputSingleton.Singleton;

        if (playerInput == null)
            throw new NoPlayerInputFound();

        return playerInput;
    }

    class NoPlayerInputFound : Exception {
        public NoPlayerInputFound(GameObject gameObject) {
            Debug.LogError("Could not find a PlayerInput component to use", gameObject);
        }

        public NoPlayerInputFound() {
            Debug.LogError("Could not find a PlayerInput component to use");
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