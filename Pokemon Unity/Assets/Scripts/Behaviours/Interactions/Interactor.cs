using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/Interactor")]
public class Interactor : MonoBehaviour
{
    public GameObject PassthroughGameObject;
    Interactable interactable = null;
    public UnityEvent<Interactable> OnPreInteract;
    public UnityEvent<Interactable> OnPostInteract;
    public Vector3 InteractOffset = Vector3.zero;
    public float InteractRadius = 0.4f;
    public bool DrawGizmos = false;

    public Interactable Interactable { get => interactable; }

    void OnDrawGizmos() {
        if (!DrawGizmos) return;
        Gizmos.DrawSphere(InteractPosition, InteractRadius);
    }

    public Vector3 InteractPosition { get => transform.position + InteractOffset; }

    /// <returns>Returns true if the interaction was successful</returns>
    public bool TryInteract() {
        Interactable interactable = GetInteractedObject();

        if (interactable != null) {
            Interact(interactable);
            return true;
        }
        return false;

        Interactable GetInteractedObject() {
            Collider[] hitColliders = Physics.OverlapSphere(InteractPosition, InteractRadius);
            if (hitColliders.Length > 0) {
                for (int i = 0; i < hitColliders.Length; i++) {
                    if (hitColliders[i].TryGetComponent<Interactable>(out var interactable))
                        return interactable;
                }
            }
            return null;
        }
    }

    public bool Interact(Interactable interactable) {
        Debug.Log($"Interacting with {interactable.name}");
        this.interactable = interactable;
        OnPreInteract.Invoke(interactable);
        interactable.Interact(this);
        return true;
    }

    public void FinishInteract() {
        OnPostInteract.Invoke(interactable);
        interactable = null;
    }

    public bool TryInteractLegacy() {
        Collider interactable = GetInteractedObject();

        if (interactable != null) {
            Debug.Log($"Interacting with {interactable.name}");
            //sent interact message to the collider's object's parent object
            //OnPreInteract.Invoke(interactable);
            //interactable.transform.parent.gameObject.SendMessage("interact", SendMessageOptions.DontRequireReceiver);
            //interactable.Interaction(this);
            //OnPostInteract.Invoke(interactable);
            return true;
        }
        return false;

        // TODO return an Interactable
        Collider GetInteractedObject() {
            Collider[] hitColliders = Physics.OverlapSphere(InteractPosition, InteractRadius);
            if (hitColliders.Length > 0) {
                for (int i = 0; i < hitColliders.Length; i++) {
                    //Prioritise a transparent over a solid object.
                    if (hitColliders[i].name.Contains("_Transparent") && hitColliders[i].name != ("Player_Transparent"))
                        return hitColliders[i];
                    else if (hitColliders[i].name.Contains("_Object"))
                        return hitColliders[i];
                }
            }
            return null;
        }
    }
}
