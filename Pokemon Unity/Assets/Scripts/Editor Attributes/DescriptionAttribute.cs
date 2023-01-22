using UnityEngine;

public class DescriptionAttribute : PropertyAttribute
{
    public readonly string Description;
    public readonly EPosition Position;
    public readonly bool HasSpace;

    public DescriptionAttribute(string description, EPosition position = EPosition.Below, bool hasSpace = false) {
        Description = description;
        Position = position;
        HasSpace = hasSpace;
    }

    public enum EPosition {
        Below,
        Above
    }
}
