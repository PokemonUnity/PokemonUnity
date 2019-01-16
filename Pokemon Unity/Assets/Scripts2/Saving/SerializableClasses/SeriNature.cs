using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Pokemon;

namespace PokemonUnity.Saving.SerializableClasses
{
    public class SeriNature
    {
        public int Nature { get; private set; }
        float[] Stat_mod { get; set; }

        public float ATK { get { return Stat_mod[0]; } }
        public float DEF { get { return Stat_mod[1]; } }
        public float SPA { get { return Stat_mod[2]; } }
        public float SPD { get { return Stat_mod[3]; } }
        public float SPE { get { return Stat_mod[4]; } }

        public SeriNature(int name, float atk_mod, float def_mod, float spa_mod, float spd_mod, float spe_mod)
        {
            Nature = name;
            Stat_mod = new float[5];
            Stat_mod[0] = atk_mod;
            Stat_mod[1] = def_mod;
            Stat_mod[2] = spa_mod;
            Stat_mod[3] = spd_mod;
            Stat_mod[4] = spe_mod;
        }

        public static implicit operator Nature(SeriNature nature)
        {
            Nature newNature = new Nature((Natures)nature.Nature, nature.ATK, nature.DEF, nature.SPA, nature.SPD, nature.SPE);
            return newNature;
        }

        public static implicit operator SeriNature(Nature nature)
        {
            SeriNature newNature = new SeriNature((int)nature.Natures, nature.ATK, nature.DEF, nature.SPA, nature.SPD, nature.SPE);
            return newNature;
        }
    }
}
