using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Linq;
using static UnityEngine.InputSystem.InputAction;
using System.Reflection;

public class UIInputBehaviour : MonoBehaviour
{
    public UIInput uiInput;

    protected void Start() {
        // TODO: re-hookup all inputs using a subscription to onnavigate
        SubscribeToPlayerInputEvents();
    }

    void SubscribeToPlayerInputEvents() {
        var navigateEvent = FindNavigateActionEvents();
        navigateEvent.AddListener(OnNavigate);
    }

    public void OnNavigate(CallbackContext context) {
        uiInput.OnNavigate.Invoke(context);
    }

    public PlayerInput.ActionEvent FindNavigateActionEvents() {
        PlayerInput t = PlayerInputSingleton.Singleton;
        var uiActionMap = t.actions.FindActionMap("UI").FindAction("Navigate");
        return t.actionEvents.First((actionEvent) => actionEvent.actionId == uiActionMap.id.ToString());
    }

    public UnityEvent<GameObject> OnSelect { get => uiInput.OnSelect; }
    public UnityEvent<string> OnChange { get => uiInput.OnChange; }
}

[Serializable]
public class UIInput 
{
    public UnityEvent<CallbackContext> OnNavigate;
    public UnityEvent<GameObject> OnSelect;
    public UnityEvent<string> OnChange;

    public void ChangeSelection(GameObject gameObject) {
        OnSelect.Invoke(gameObject);
    }
}