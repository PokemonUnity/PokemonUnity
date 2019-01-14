using System.Collections;
using PokemonUnity.Pokemon;

namespace PokemonUnity.Pokemon
{
public class Nature
{

    public Natures Natures { get; private set; }
    float[] Stat_mod { get; set; }//= new float[5];
    public float ATK { get { return Stat_mod[0]; } } 
    public float DEF { get { return Stat_mod[1]; } } 
    public float SPA { get { return Stat_mod[2]; } } 
    public float SPD { get { return Stat_mod[3]; } } 
    public float SPE { get { return Stat_mod[4]; } } 
    //private string[] Stat_name = new string[] { "ATK", "DEF", "SPA", "SPD", "SPE" };

    public Nature()
    {
        this.Natures = getRandomNature();
    }

    public Nature(Natures name, float ATK_mod, float DEF_mod, float SPA_mod, float SPD_mod, float SPE_mod)
    {
        this.Natures = name;
        this.Stat_mod = new float[5];
        this.Stat_mod[0] = ATK_mod;
        this.Stat_mod[1] = DEF_mod;
        this.Stat_mod[2] = SPA_mod;
        this.Stat_mod[3] = SPD_mod;
        this.Stat_mod[4] = SPE_mod;
    }    

    private static Nature[] natures = new Nature[]
    {
        new Nature(Natures.HARDY, 1, 1, 1, 1, 1),
        new Nature(Natures.LONELY, 1.1f, 0.9f, 1, 1, 1),
        new Nature(Natures.BRAVE, 1.1f, 1, 1, 1, 0.9f),
        new Nature(Natures.ADAMANT, 1.1f, 1, 0.9f, 1, 1),
        new Nature(Natures.NAUGHTY, 1.1f, 1, 1, 0.9f, 1),
        new Nature(Natures.BOLD, 0.9f, 1.1f, 1, 1, 1),
        new Nature(Natures.DOCILE, 1, 1, 1, 1, 1),
        new Nature(Natures.RELAXED, 1, 1.1f, 1, 1, 0.9f),
        new Nature(Natures.IMPISH, 1, 1.1f, 0.9f, 1, 1),
        new Nature(Natures.LAX, 1, 1.1f, 1, 0.9f, 1),
        new Nature(Natures.TIMID, 0.9f, 1, 1, 1, 1.1f),
        new Nature(Natures.HASTY, 1, 0.9f, 1, 1, 1.1f),
        new Nature(Natures.SERIOUS, 1, 1, 1, 1, 1),
        new Nature(Natures.JOLLY, 1, 1, 0.9f, 1, 1.1f),
        new Nature(Natures.NAIVE, 1, 1, 1, 0.9f, 1.1f),
        new Nature(Natures.MODEST, 0.9f, 1, 1.1f, 1, 1),
        new Nature(Natures.MILD, 1, 0.9f, 1.1f, 1, 1),
        new Nature(Natures.QUIET, 1, 1, 1.1f, 1, 0.9f),
        new Nature(Natures.BASHFUL, 1, 1, 1, 1, 1),
        new Nature(Natures.RASH, 1, 1, 1.1f, 0.9f, 1),
        new Nature(Natures.CALM, 0.9f, 1, 1, 1.1f, 1),
        new Nature(Natures.GENTLE, 1, 0.9f, 1, 1.1f, 1),
        new Nature(Natures.SASSY, 1, 1, 1, 1.1f, 0.9f),
        new Nature(Natures.CAREFUL, 1, 1, 0.9f, 1.1f, 1),
        new Nature(Natures.QUIRKY, 1, 1, 1, 1, 1)
    };

    private static Natures getRandomNature()
    {
        return natures[new System.Random(Settings.Seed()).Next(0, natures.Length)].Natures;
    }
}
}