using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGameSettingArray<T> {
    public List<InputValue<T>> Choices { get; }
    public int GetIndex();
    public InputValue<T> GetInputValue();
}

//public abstract class GameSettingArray<T> : GameSetting<T>
//{
//    public abstract List<T> Choices { get; }

//    public abstract int GetIndex();
//}
