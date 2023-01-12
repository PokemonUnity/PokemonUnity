using TMPro;
using UnityEngine;

public abstract class Carousel<T> : UIListInput<T> {
    [SerializeField] TextMeshProUGUI TargetText;

    public override GameSetting<T> GameSetting { get => gameSetting; }

    new void Start() {
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided", gameObject);
        base.Start();
    }

    protected override void SyncChildren() {
        if (activeIndex < 0 || activeIndex >= Choices.Count) return;
        TargetText.text = Choices[activeIndex].DisplayText;
    }

    public void UpdateValueUp() => UpdateValueIndex(activeIndex + 1);
    public void UpdateValueDown() => UpdateValueIndex(activeIndex - 1);

    public override void UpdateValueIndex(int index) {
        if (index == activeIndex) return;
        if (index < 0 || index >= Choices.Count) return;
        activeIndex = index;
        TargetText.text = Choices[index].DisplayText;
        currentValue = Choices[index].Value;
        base.UpdateValue(Choices[index]);
    }

    public override void UpdateValue(Vector2 navigationDirection) {
        if (navigationDirection.magnitude == 0) return;
        Audio.Play();
        UpdateValueIndex(activeIndex + (int)navigationDirection.x);
    }
}
