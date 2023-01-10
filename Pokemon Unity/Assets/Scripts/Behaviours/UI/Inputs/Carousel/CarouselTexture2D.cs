using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Carousel/Carousel Texture2D")]
public class CarouselTexture2D : Carousel<Texture2D> {
    public override bool IsEqual(Texture2D value1, Texture2D value2) => value1 == value2;
}
