//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public static class NatureDatabase
{
    private static NatureData[] natures = new NatureData[]
    {
        new NatureData("HARDY", 1, 1, 1, 1, 1),
        new NatureData("LONELY", 1.1f, 0.9f, 1, 1, 1),
        new NatureData("BRAVE", 1.1f, 1, 1, 1, 0.9f),
        new NatureData("ADAMANT", 1.1f, 1, 0.9f, 1, 1),
        new NatureData("NAUGHTY", 1.1f, 1, 1, 0.9f, 1),
        new NatureData("BOLD", 0.9f, 1.1f, 1, 1, 1),
        new NatureData("DOCILE", 1, 1, 1, 1, 1),
        new NatureData("RELAXED", 1, 1.1f, 1, 1, 0.9f),
        new NatureData("IMPISH", 1, 1.1f, 0.9f, 1, 1),
        new NatureData("LAX", 1, 1.1f, 1, 0.9f, 1),
        new NatureData("TIMID", 0.9f, 1, 1, 1, 1.1f),
        new NatureData("HASTY", 1, 0.9f, 1, 1, 1.1f),
        new NatureData("SERIOUS", 1, 1, 1, 1, 1),
        new NatureData("JOLLY", 1, 1, 0.9f, 1, 1.1f),
        new NatureData("NAIVE", 1, 1, 1, 0.9f, 1.1f),
        new NatureData("MODEST", 0.9f, 1, 1.1f, 1, 1),
        new NatureData("MILD", 1, 0.9f, 1.1f, 1, 1),
        new NatureData("QUIET", 1, 1, 1.1f, 1, 0.9f),
        new NatureData("BASHFUL", 1, 1, 1, 1, 1),
        new NatureData("RASH", 1, 1, 1.1f, 0.9f, 1),
        new NatureData("CALM", 0.9f, 1, 1, 1.1f, 1),
        new NatureData("GENTLE", 1, 0.9f, 1, 1.1f, 1),
        new NatureData("SASSY", 1, 1, 1, 1.1f, 0.9f),
        new NatureData("CAREFUL", 1, 1, 0.9f, 1.1f, 1),
        new NatureData("QUIRKY", 1, 1, 1, 1, 1)
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

    public static NatureData getNature(int index)
    {
        return natures[index];
    }

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