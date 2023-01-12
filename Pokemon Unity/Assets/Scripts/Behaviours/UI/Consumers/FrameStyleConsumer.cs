using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/Settings/Frame Style Consumer")]
[RequireComponent(typeof(Image))]
public class FrameStyleConsumer : MonoBehaviour
{
    public GameSetting<Sprite> GameSetting;
    Image image;

    void Start()
    {
        image = GetComponent<Image>();
        GameSetting?.OnValueChange.AddListener(UpdateFrame);
        UpdateFrame(GameSetting?.Get());
    }

    public void UpdateFrame(Sprite frame) {
        image.sprite = frame;
    }
}
