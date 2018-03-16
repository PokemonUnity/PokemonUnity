[System.Serializable]
public class PokedexTranslation : ITranslation, ITranslationPokedex
{
    ITranslation _translate;
    public string Name;
    public string PokedexEntry
    {
        get { return _translate.Description; }
        set { _translate.Description = value; }
    }
    /// <summary>
    /// "Seed", "Rat", "Plant", etc... Pokemon.
    /// </summary>
    public string Species;
    public string[] Forms;
}

[System.Serializable]
public class MoveTranslation : ITranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class ItemTranslation : ITranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class AbilityTranslation : ITranslation
{
    public string Name;
    public string Description;
}

[System.Serializable]
public class NatureTranslation : ITranslation
{
    public string Name;
    public string Description;
}

public interface ITranslation
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public interface ITranslationPokedex
{
    public string Species { get; set; }
    public string[] Forms { get; set; }
}