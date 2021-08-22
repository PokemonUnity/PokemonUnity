using PokemonUnity.Character;

public static class PCExtension
{
	public static bool hasSpace(this PC PC, int box)
	{
		for (int i = 0; i < PC.AllBoxes[box].Length; i++)
        {
			if (!PC.AllBoxes[box][i].IsNotNullOrNone())
            {
				return true;
            }
        }
		return false;
	}
}
