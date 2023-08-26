using System;

public static class StringExtensions
{
    public static string ToLowerAndTrim(this string string_) => string_.Trim().ToLower();
}
