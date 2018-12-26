//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PC
{
    //index 0: Party,    index >= 1: PC box
    public Pokemon[][] boxes = new Pokemon[][]
    {
        new Pokemon[6], //Party
        new Pokemon[30], //Boxes
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30],
        new Pokemon[30]
    }; //create an array of arrays of Pokemon. A List of Boxes.

    public string[] boxName = new string[13];

    public int[] boxTexture = new int[13];


    public PC()
    {
    }


    public bool hasSpace(int box)
    {
        for (int i = 0; i < boxes[box].Length; i++)
        {
            if (boxes[box][i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public int getBoxLength(int box)
    {
        int result = 0;
        for (int i = 0; i < boxes[box].Length; i++)
        {
            if (boxes[box][i] != null)
            {
                result += 1;
            }
        }
        return result;
    }

    public void packParty()
    {
        Pokemon[] packedArray = new Pokemon[6];
        int i2 = 0; //counter for packed array
        for (int i = 0; i < 6; i++)
        {
            if (boxes[0][i] != null)
            {
                //if next object in box has a value
                packedArray[i2] = boxes[0][i]; //add to packed array
                i2 += 1; //ready packed array's next position
            }
        }
        boxes[0] = packedArray;
    }

    //Add a new pokemon. If pokemon could not be added return false.
    //public bool addPokemon(PokemonOld acquiredPokemon)
    //{
    //    //attempt to add to party first. pack the party array if space available.
    //    if (hasSpace(0))
    //    {
    //        packParty();
    //        boxes[0][boxes[0].Length - 1] = acquiredPokemon;
    //        packParty();
    //        return true;
    //    }
    //    //attempt to add to the earliest available PC box. no array packing needed.
    //    else
    //    {
    //        for (int i = 1; i < boxes.Length; i++)
    //        {
    //            if (hasSpace(i))
    //            {
    //                for (int i2 = 0; i2 < boxes[i].Length; i2++)
    //                {
    //                    if (boxes[i][i2] == null)
    //                    {
    //                        boxes[i][i2] = acquiredPokemon;
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    //if could not add a pokemon, return false. Party and PC are both full.
    //    return false;
    //}

    //public void swapPokemon(int box1, int pos1, int box2, int pos2)
    //{
    //    PokemonOld temp = boxes[box1][pos1];
    //    boxes[box1][pos1] = boxes[box2][pos2];
    //    boxes[box2][pos2] = temp;
    //}

    public string boxToString(int box)
    {
        string result = "";
        if (box == 0)
        {
            result = "(Party) ";
            packParty();
            for (int i = 0; i < boxes[box].Length; i++)
            {
                if (boxes[box][i] == null)
                {
                    result += "null, ";
                }
                else
                {
                    result += //boxes[box][i].getID() + 
						": " + boxes[box][i].Name + ", ";
                }
            }
            result.Remove(result.Length - 2, 2);
        }
        else
        {
            result = "(Box " + box + ") ";
            for (int i = 0; i < boxes[box].Length; i++)
            {
                if (boxes[box][i] == null)
                {
                    result += "null, ";
                }
                else
                {
                    result += //boxes[box][i].getID() + 
						": " + boxes[box][i].Name + ", ";
                }
            }
        }
        return result;
    }

    //public PokemonOld getFirstFEUserInParty(string moveName)
    //{
    //    for (int i = 0; i < 6; i++)
    //    {
    //        if (boxes[0][i] != null)
    //        {
    //            string[] moveset = boxes[0][i].getMoveset();
    //            for (int i2 = 0; i2 < moveset.Length; i2++)
    //            {
    //                if (moveset[i2] != null)
    //                {
    //                    if (MoveDatabase.getMove(moveset[i2]).getFieldEffect() == moveName)
    //                    {
    //                        return boxes[0][i];
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return null;
    //}
}