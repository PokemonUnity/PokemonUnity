using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumExtensions
{
    public static List<InputValue<T>> GetEnumInputValuesList<T>(this T enum_) where T : Enum {
        T[] values = (T[])Enum.GetValues(enum_.GetType());
        string[] names = Enum.GetNames(enum_.GetType());
        List<InputValue<T>> inputValues = new();

        for (int i = 0; i < values.Length; i++) {
            inputValues.Add(new InputValue<T>(names[i], values[i]));
        }

        return inputValues;
    }
}
