//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class NatureData
{
    private string name;
    private float[] Stat_mod = new float[5];
    private string[] Stat_name = new string[] {"ATK", "DEF", "SPA", "SPD", "SPE"};

    public NatureData(string name, float ATK_mod, float DEF_mod, float SPA_mod, float SPD_mod, float SPE_mod)
    {
        this.name = name;
        this.Stat_mod[0] = ATK_mod;
        this.Stat_mod[1] = DEF_mod;
        this.Stat_mod[2] = SPA_mod;
        this.Stat_mod[3] = SPD_mod;
        this.Stat_mod[4] = SPE_mod;
    }

    public string getName()
    {
        return name;
    }


    public float getATK()
    {
        return Stat_mod[0];
    }

    public float getDEF()
    {
        return Stat_mod[1];
    }

    public float getSPA()
    {
        return Stat_mod[2];
    }

    public float getSPD()
    {
        return Stat_mod[3];
    }

    public float getSPE()
    {
        return Stat_mod[4];
    }

    public string getUpStat()
    {
        for (int i = 0; i < Stat_mod.Length; i++)
        {
            if (Stat_mod[i] > 1)
            {
                return Stat_name[i];
            }
        }
        return "None";
    }

    public string getDownStat()
    {
        for (int i = 0; i < Stat_mod.Length; i++)
        {
            if (Stat_mod[i] < 1)
            {
                return Stat_name[i];
            }
        }
        return "None";
    }
}