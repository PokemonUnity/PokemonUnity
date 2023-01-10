using UnityEngine;

public class DescriptionAttribute : PropertyAttribute
{
    public readonly string description;
    public readonly EPosition position;

    public DescriptionAttribute(string description, EPosition position = EPosition.Below) {
        this.description = description;
        this.position = position;
    }

    public enum EPosition {
        Below,
        Above
    }
}
