using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIMoveTo : MonoBehaviour
{
    [SerializeField] GameObject Target;
    [SerializeField] EPosition positionType = EPosition.Anchored;
    [Description("Seconds")]
    [SerializeField] float moveToDuration = 1f;
    public Vector2 Offset;

    LTDescr currentTween;

    void Start()
    {
        if (Target is not null) Move();
    }

    public void Move() {
        if (LeanTween.isTweening(gameObject)) LeanTween.cancel(gameObject);
        RectTransform rectTransform = (RectTransform)Target.transform;
        Vector3 targetPosition;
        switch (positionType) {
            case EPosition.Global:
                targetPosition = rectTransform.position;
                break;
            case EPosition.Local:
                targetPosition = rectTransform.localPosition;
                break;
            case EPosition.Anchored:
                targetPosition = rectTransform.anchoredPosition;
                break;
            default:
                targetPosition = rectTransform.anchoredPosition;
                break;
        }
        //Debug.Log("Target: " + Target.name + " Position: " + targetPosition.ToString() + " Offset: " + Offset.ToString());
        targetPosition = targetPosition + (Vector3)Offset;
        currentTween = LeanTween.move((RectTransform)transform, targetPosition, moveToDuration);
    }

    public void ChangeDuration(float duration) {
        moveToDuration = duration;
    }

    public void MoveTo(GameObject gameObject) {
        MoveTo(gameObject, moveToDuration);
    }

    public void MoveTo(GameObject gameObject, float duration) {
        Target = gameObject;
        moveToDuration = duration;
        Move();
    }

    public enum EPosition {
        Global,
        Local,
        Anchored
    }
}
