[System.Serializable]
public class PokedexTranslation
{
    public string Name;
    /// <summary>
    /// "Seed", "Rat", "Plant", etc... Pokemon.
    /// </summary>
    public string Species;
    public string PokedexEntry;
    public string[] Forms;
}

[System.Serializable]
public class MoveTranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class ItemTranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class AbilityTranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class NatureTranslation
{
    public string Name;
    public string Description;
}