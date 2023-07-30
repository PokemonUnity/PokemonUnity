using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reticle control for when the aiming is inaccurate. Inaccuracy is shown by pulling apart the aim reticle.
/// </summary>
public class AimReticle : MonoBehaviour
{
    [Tooltip("Maximum radius of the aim reticle, when aiming is inaccurate. ")]
    [Range(0, 100f)]
    public float MaxRadius;

    [Tooltip("The time is takes for the aim reticle to adjust, when inaccurate.")]
    [Range(0, 1f)]
    public float BlendTime;
    
    [Tooltip("Top piece of the aim reticle.")]
    public Image Top;
    [Tooltip("Bottom piece of the aim reticle.")]
    public Image Bottom;
    [Tooltip("Left piece of the aim reticle.")]
    public Image Left;
    [Tooltip("Right piece of the aim reticle.")]
    public Image Right;
    
    [Tooltip("This 2D object will be positioned in the game view over the raycast hit point, if any, "
        + "or will remain in the center of the screen if no hit point is detected.  "
        + "May be null, in which case no on-screen indicator will appear. Same as Cinemachine3rdPersonAim's")]
    public RectTransform AimTargetReticle;

    void Reset()
    {
        MaxRadius = 30f;
        BlendTime = 0.05f;
    }
    
    float m_BlendVelocity;
    float m_CurrentRadius;
    void Update()
    {
        var screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        float distanceFromCenter = 0;
        if (AimTargetReticle != null)
        {
            var hitPoint = (Vector2) AimTargetReticle.position;
            distanceFromCenter = (screenCenterPoint - hitPoint).magnitude;
        }
        m_CurrentRadius = Mathf.SmoothDamp(m_CurrentRadius, distanceFromCenter * 2f, ref m_BlendVelocity, BlendTime);
        m_CurrentRadius = Mathf.Min(MaxRadius, m_CurrentRadius);

        Left.rectTransform.position = screenCenterPoint + (Vector2.left * m_CurrentRadius);
        Right.rectTransform.position = screenCenterPoint + (Vector2.right * m_CurrentRadius);
        Top.rectTransform.position = screenCenterPoint + (Vector2.up * m_CurrentRadius);
        Bottom.rectTransform.position = screenCenterPoint + (Vector2.down * m_CurrentRadius);
    }
}
