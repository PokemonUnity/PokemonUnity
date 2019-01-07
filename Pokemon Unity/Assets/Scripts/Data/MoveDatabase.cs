//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public static class MoveDatabase
{
    //Move Effects Name required. 0 if undefined paramater


    private static MoveDataOld[] moves = new MoveDataOld[]
	{//new MoveDataOld\("([\w\s-]*)"[.,\w\s\[\]{}"\-\\']*[\n| ][."\w]*"\),
        //null,
    };

    public static MoveDataOld getMove(string name)
    {
        MoveDataOld result = null;
        int i = 1;
        while (result == null && i < moves.Length)
        {
            if (moves[i].getName() == name)
            {
                result = moves[i];
            }
            i += 1;
        }
        return result;
    }

    public static MoveDataOld getMove(int MoveId)
    {
        return getMove("PlaceHolder"); //just doing this until i can fix it later
    }
}