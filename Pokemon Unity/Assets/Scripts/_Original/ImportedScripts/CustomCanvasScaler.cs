using UnityEngine;
using UnityEngine.UI;

public class CustomCanvasScaler : CanvasScaler
{
    private Canvas m_RootCanvas;
    private const float kLogBase = 2;

    protected override void OnEnable()
    {
        m_RootCanvas = GetComponent<Canvas>();
        base.OnEnable();
    }

    protected override void HandleScaleWithScreenSize()
    {
        Vector2 screenSize;
        if (m_RootCanvas.worldCamera != null)
        {
            screenSize = new Vector2(m_RootCanvas.worldCamera.pixelWidth, m_RootCanvas.worldCamera.pixelHeight);
        }
        else
        {
            screenSize = new Vector2(Screen.width, Screen.height);
        }

        // Multiple display support only when not the main display. For display 0 the reported
        // resolution is always the desktops resolution since its part of the display API,
        // so we use the standard none multiple display method. (case 741751)
        int displayIndex = m_RootCanvas.targetDisplay;
        if (displayIndex > 0 && displayIndex < Display.displays.Length)
        {
            Display disp = Display.displays[displayIndex];
            screenSize = new Vector2(disp.renderingWidth, disp.renderingHeight);
        }

        float scaleFactor = 0;
        switch (m_ScreenMatchMode)
        {
            case ScreenMatchMode.MatchWidthOrHeight:
                {
                    // We take the log of the relative width and height before taking the average.
                    // Then we transform it back in the original space.
                    // the reason to transform in and out of logarithmic space is to have better behavior.
                    // If one axis has twice resolution and the other has half, it should even out if widthOrHeight value is at 0.5.
                    // In normal space the average would be (0.5 + 2) / 2 = 1.25
                    // In logarithmic space the average is (-1 + 1) / 2 = 0
                    float logWidth = Mathf.Log(screenSize.x / m_ReferenceResolution.x, kLogBase);
                    float logHeight = Mathf.Log(screenSize.y / m_ReferenceResolution.y, kLogBase);
                    float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, m_MatchWidthOrHeight);
                    scaleFactor = Mathf.Pow(kLogBase, logWeightedAverage);
                    break;
                }
            case ScreenMatchMode.Expand:
                {
                    scaleFactor = Mathf.Min(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
                    break;
                }
            case ScreenMatchMode.Shrink:
                {
                    scaleFactor = Mathf.Max(screenSize.x / m_ReferenceResolution.x, screenSize.y / m_ReferenceResolution.y);
                    break;
                }
        }

        SetScaleFactor(scaleFactor);
        SetReferencePixelsPerUnit(m_ReferencePixelsPerUnit);
    }
}