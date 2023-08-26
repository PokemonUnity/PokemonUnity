using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public static class PlayerInputExtensions
{
    public static UnityEvent<InputAction.CallbackContext> GetEvent(this PlayerInput playerInput, string actionMap, string action) {
        var uiActionMap = playerInput.actions.FindActionMap(actionMap).FindAction(action);
        if (uiActionMap == null)
            Debug.LogError($"PlayerInput does not contain the action map {actionMap}");

        try {
            var event_ = playerInput.actionEvents.First((actionEvent) => actionEvent.actionId == uiActionMap.id.ToString());
            return event_;
        } catch (InvalidOperationException) {
            Debug.LogError($"PlayerInput's {actionMap} does not contain the action {action}");
        }
        return null;
    }
}
