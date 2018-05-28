//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[System.Obsolete]
public static class NatureDatabaseOld
{
    public enum Nature
    {
		UNSET,
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
    private static NatureDataOld[] natures = new NatureDataOld[]
    {
        new NatureDataOld(Nature.HARDY, 1, 1, 1, 1, 1),
        new NatureDataOld(Nature.LONELY, 1.1f, 0.9f, 1, 1, 1),
        new NatureDataOld(Nature.BRAVE, 1.1f, 1, 1, 1, 0.9f),
        new NatureDataOld(Nature.ADAMANT, 1.1f, 1, 0.9f, 1, 1),
        new NatureDataOld(Nature.NAUGHTY, 1.1f, 1, 1, 0.9f, 1),
        new NatureDataOld(Nature.BOLD, 0.9f, 1.1f, 1, 1, 1),
        new NatureDataOld(Nature.DOCILE, 1, 1, 1, 1, 1),
        new NatureDataOld(Nature.RELAXED, 1, 1.1f, 1, 1, 0.9f),
        new NatureDataOld(Nature.IMPISH, 1, 1.1f, 0.9f, 1, 1),
        new NatureDataOld(Nature.LAX, 1, 1.1f, 1, 0.9f, 1),
        new NatureDataOld(Nature.TIMID, 0.9f, 1, 1, 1, 1.1f),
        new NatureDataOld(Nature.HASTY, 1, 0.9f, 1, 1, 1.1f),
        new NatureDataOld(Nature.SERIOUS, 1, 1, 1, 1, 1),
        new NatureDataOld(Nature.JOLLY, 1, 1, 0.9f, 1, 1.1f),
        new NatureDataOld(Nature.NAIVE, 1, 1, 1, 0.9f, 1.1f),
        new NatureDataOld(Nature.MODEST, 0.9f, 1, 1.1f, 1, 1),
        new NatureDataOld(Nature.MILD, 1, 0.9f, 1.1f, 1, 1),
        new NatureDataOld(Nature.QUIET, 1, 1, 1.1f, 1, 0.9f),
        new NatureDataOld(Nature.BASHFUL, 1, 1, 1, 1, 1),
        new NatureDataOld(Nature.RASH, 1, 1, 1.1f, 0.9f, 1),
        new NatureDataOld(Nature.CALM, 0.9f, 1, 1, 1.1f, 1),
        new NatureDataOld(Nature.GENTLE, 1, 0.9f, 1, 1.1f, 1),
        new NatureDataOld(Nature.SASSY, 1, 1, 1, 1.1f, 0.9f),
        new NatureDataOld(Nature.CAREFUL, 1, 1, 0.9f, 1.1f, 1),
        new NatureDataOld(Nature.QUIRKY, 1, 1, 1, 1, 1)
    };

    public static NatureDataOld getNature(string name)
    {
        NatureDataOld result = null;
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

    public static NatureDataOld getNature(Nature name)
    {
        return getNature((int)name);
    }

    public static NatureDataOld getNature(int index)
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

    public static NatureDataOld getRandomNature()
    {
        return natures[Random.Range(0, natures.Length)];
    }
}