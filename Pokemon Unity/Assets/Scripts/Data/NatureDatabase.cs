//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public static class NatureDatabase
{
    public enum Nature
    {
        HARDY,
        LONELY,
        BRAVE,
        ADAMANT,
        NAUGHTY,
        BOLD,
        DOCILE,
        RELAXED,
        IMPISH,
        LAX,
        TIMID,
        HASTY,
        SERIOUS,
        JOLLY,
        NAIVE,
        MODEST,
        MILD,
        QUIET,
        BASHFUL,
        RASH,
        CALM,
        GENTLE,
        SASSY,
        CAREFUL,
        QUIRKY
    }
    private static NatureData[] natures = new NatureData[]
    {
        new NatureData(Nature.HARDY, 1, 1, 1, 1, 1),
        new NatureData(Nature.LONELY, 1.1f, 0.9f, 1, 1, 1),
        new NatureData(Nature.BRAVE, 1.1f, 1, 1, 1, 0.9f),
        new NatureData(Nature.ADAMANT, 1.1f, 1, 0.9f, 1, 1),
        new NatureData(Nature.NAUGHTY, 1.1f, 1, 1, 0.9f, 1),
        new NatureData(Nature.BOLD, 0.9f, 1.1f, 1, 1, 1),
        new NatureData(Nature.DOCILE, 1, 1, 1, 1, 1),
        new NatureData(Nature.RELAXED, 1, 1.1f, 1, 1, 0.9f),
        new NatureData(Nature.IMPISH, 1, 1.1f, 0.9f, 1, 1),
        new NatureData(Nature.LAX, 1, 1.1f, 1, 0.9f, 1),
        new NatureData(Nature.TIMID, 0.9f, 1, 1, 1, 1.1f),
        new NatureData(Nature.HASTY, 1, 0.9f, 1, 1, 1.1f),
        new NatureData(Nature.SERIOUS, 1, 1, 1, 1, 1),
        new NatureData(Nature.JOLLY, 1, 1, 0.9f, 1, 1.1f),
        new NatureData(Nature.NAIVE, 1, 1, 1, 0.9f, 1.1f),
        new NatureData(Nature.MODEST, 0.9f, 1, 1.1f, 1, 1),
        new NatureData(Nature.MILD, 1, 0.9f, 1.1f, 1, 1),
        new NatureData(Nature.QUIET, 1, 1, 1.1f, 1, 0.9f),
        new NatureData(Nature.BASHFUL, 1, 1, 1, 1, 1),
        new NatureData(Nature.RASH, 1, 1, 1.1f, 0.9f, 1),
        new NatureData(Nature.CALM, 0.9f, 1, 1, 1.1f, 1),
        new NatureData(Nature.GENTLE, 1, 0.9f, 1, 1.1f, 1),
        new NatureData(Nature.SASSY, 1, 1, 1, 1.1f, 0.9f),
        new NatureData(Nature.CAREFUL, 1, 1, 0.9f, 1.1f, 1),
        new NatureData(Nature.QUIRKY, 1, 1, 1, 1, 1)
    };

    public static NatureData getNature(string name)
    {
        NatureData result = null;
        name = name.ToUpper();
        int i = 0;
        while (result == null)
        {
            if (i >= natures.Length)
            {
                return null;
            }
            else if (natures[i].getName() == name)
            {
                result = natures[i];
            }
            i += 1;
        }
        return result;
    }

    public static NatureData getNature(Nature name)
    {
        return getNature((int)name);
    }

    public static NatureData getNature(int index)
    {
        return natures[index];
    }

    /// <summary>
    /// not needed; use <see cref="ToString()"/>
    /// </summary>
    /// <returns></returns>
    public static string[] getNatureNames()
    {
        string[] names = new string[natures.Length];
        for (int i = 0; i < names.Length; i++)
        {
            names[i] = natures[i].getName();
        }
        return names;
    }

    public static NatureData getRandomNature()
    {
        return natures[Random.Range(0, natures.Length)];
    }
}