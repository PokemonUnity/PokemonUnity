using System;

namespace Data
{
    [Serializable]
    public class Pokedex
    {
        public bool[] seen;
        public bool[] caught;

        private static int[] pokedex =
        {
            1,2,3,4,5,6,7,8,9,10,11,12
        };

        public Pokedex()
        {
            seen = new bool[pokedex.Length]; // each value is set to false by default
            caught = new bool[pokedex.Length];
        }
        
        public static int findPokedexIndex(int id)
        {
            int i = 0;
            while (i < pokedex.Length)
            {
                if (id == pokedex[i])
                {
                    return i;
                }
                i++;
            }

            return -1;
        }

        public int[] getList()
        {
            return pokedex;
        }

        public void SetSeen(int index)
        {
            if (index >= 0 && index < pokedex.Length)
            {
                seen[index] = true;
            }
        }

        public void SetCaught(int index)
        {
            if (index >= 0 && index < pokedex.Length)
            {
                caught[index] = true;
            }
        }

        public void UpdateList()
        {
            if (seen.Length < pokedex.Length)
            {
                Array.Resize(ref seen, pokedex.Length);
                Array.Resize(ref caught, pokedex.Length);
            }
        }
    }
}

