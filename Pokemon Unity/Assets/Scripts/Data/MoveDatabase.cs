//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public static class MoveDatabase
{
    //Move Effects Name required. 0 if undefined paramater


    private static MoveData[] moves = new MoveData[]
	{//new MoveData\("([\w\s-]*)"[.,\w\s\[\]{}"\-\\']*[\n| ][."\w]*"\),
        //null,
    };

    public static MoveData getMove(string name)
    {
        MoveData result = null;
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

    public static MoveData getMove(int MoveId)
    {
        return getMove("PlaceHolder"); //just doing this until i can fix it later
    }
}