using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Carousel/Carousel Int")]
public class CarouselInt : Carousel<int> {
    public override bool IsEqual(int value1, int value2) => value1 == value2;
}
