using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Carousel/Carousel Text")]
public class CarouselText : Carousel<string> {
    public override bool IsEqual(string value1, string value2) => value1 == value2;
}
