public static class FloatExtensions 
{
    /// <summary>
    /// Checks if value is between lowerbound and upperbound (both inclusive)
    /// </summary>
    public static bool IsBetween(this float value, float lb, float ub) => lb <= value && value <= ub;

    /// <summary>
    /// Clamps value to lowerbound and upperbound (both inclusive)
    /// </summary>
    public static float ClampTo(this float value, float lb, float ub) 
    {
        if (value < lb) return lb;
        else if (value > ub) return ub;
        else return value;
    }

    /// <summary>
    /// Checks if value is close to zero within tolerance of 0.005f
    /// </summary>
    public static bool IsBasicallyZero(this float value) => value.IsBetween(-0.005f, 0.005f);

    /// <summary>
    /// Checks if the absolute difference of the two numbers is within tolerance of 0.005f
    /// </summary>
    public static bool BasicallyEquals(this float value, float other) => (value - other).IsBasicallyZero();
}
