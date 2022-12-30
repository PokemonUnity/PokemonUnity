using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIInputBehaviour : MonoBehaviour
{
    public UIInput uiInput;

    public UnityEvent<GameObject> OnSelect { get => uiInput.OnSelect; }
    public UnityEvent<string> OnChange { get => uiInput.OnChange; }
}

[Serializable]
public class UIInput 
{
    public UnityEvent<GameObject> OnSelect;
    public UnityEvent<string> OnChange;

    public void ChangeSelection(GameObject gameObject) {
        OnSelect.Invoke(gameObject);
    }
}