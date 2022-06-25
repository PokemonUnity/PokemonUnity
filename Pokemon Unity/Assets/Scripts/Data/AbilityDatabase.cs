using Data.Abilities;
using UnityEngine;

public static class AbilityDatabase
{
    private static AbilityData[] abilities =
    {
        new Pressure(),
        new Teravolt(),
        new Turboblaze()
        
    };

    public static AbilityData getAbility(string name)
    {
        foreach (AbilityData ab in abilities)
        {
            if (ab.GetName() == name) return ab;
        }
        return null;
    }
}
