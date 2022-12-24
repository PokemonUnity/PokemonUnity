using UnityEngine;

public class DescriptionAttribute : PropertyAttribute
{
    public readonly string description;

    public DescriptionAttribute(string description) {
        this.description = description;
    }
}
