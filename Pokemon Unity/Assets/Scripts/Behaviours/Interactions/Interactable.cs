using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/Interactable")]
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    /// <summary>Higher priority means it gets selected first</summary>
    public GameObject PassthroughGameObject;
    public int Priority = 0;
    public UnityEvent<Interactor> OnPreInteraction;
    public UnityEvent<Interactor> Interaction;
    public UnityEvent<Interactor> OnPostInteraction;
    public bool Interacting = false;
    Interactor interactor;

    public Interactor Interactor { get => interactor; }

    public void Interact(Interactor interactor) {
        if (Interaction == null) {
            Debug.LogError("No Interaction delegate was set. Can't interact", gameObject);
            return;
        }

        if (interactor.Interactable == this) {
            Interacting = true;
            OnPreInteraction.Invoke(interactor);
            Interaction.Invoke(interactor);
            this.interactor = interactor;
        } else {
            Debug.LogError($"Tried to interact with {gameObject.name} but the Interactor is already interacting with {interactor.Interactable.name}", interactor);
        }
    }

    public void FinishInteracting() {
        OnPostInteraction.Invoke(interactor);
        interactor = null;
        Interacting = false;
    }
}
