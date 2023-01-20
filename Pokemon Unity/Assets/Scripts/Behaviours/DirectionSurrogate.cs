using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DirectionSurrogate : MonoBehaviour {
    public Color GizmoColor = Color.gray;
    public float GizmoDrawLength = 1f;
    public Quaternion Direction;
    public UnityEvent<Vector3> OnDirectionUpdated;

    public Vector3 FacingDirection { get => transform.forward; }

    void OnValidate() {
        ResetPosition();
        UpdateDirection(Direction);
    }

    void Start() {
        ResetPosition();
        UpdateDirection(FacingDirection);
    }

    void OnDrawGizmos() {
        Gizmos.color = GizmoColor;
        DrawArrow.ForGizmo(transform.position, FacingDirection * GizmoDrawLength);
    }

    public void UpdateDirection(Quaternion newDirection) {
        transform.rotation = newDirection;
        OnDirectionUpdated.Invoke(FacingDirection);
    }

    public void UpdateDirection(Vector3 newDirection) {
        transform.forward = newDirection;
        OnDirectionUpdated.Invoke(FacingDirection);
    }

    void ResetPosition() {
        if (transform.parent != null) transform.position = transform.parent.position;
    }
}


public interface INeedDirection {
    public DirectionSurrogate DirectionSurrogate { get; }
    public Vector3 FacingDirection { get; }
}