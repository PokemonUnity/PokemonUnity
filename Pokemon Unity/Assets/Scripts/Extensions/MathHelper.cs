using System;

public static class MathHelper
{
    //private MathHelper()
    //{
    //    throw new InvalidOperationException("Cannot initialize static class.");
    //}

	public static int Clamp(float amount, float min, float max)
	{
		if (amount > max)
			return (int)max;
		else if (amount < min)
			return (int)min;
		else
			return (int)amount;
	}

	public static int Clamp(int amount, int min, int max)
	{
		if (amount > max)
			return max;
		else if (amount < min)
			return min;
		else
			return amount;
	}

	public const int PiOver2 = 0;
	public const int TwoPi = 0;
	public const int Pi = 0;
}
