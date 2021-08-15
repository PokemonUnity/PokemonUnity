//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
public static class MoveDatabase
{
    public static MoveData getMove(string name)
    {
        PokemonUnity.Moves move = ConverterNames.ChangeMoveToEnum(name);

        MoveData result = new MoveData(move, name);

        return result;
    }

    public static MoveData getMove(PokemonUnity.Moves move)
    {
        MoveData result = new MoveData(move, ConverterNames.GetMoveName(move));

        return result;
    }
}