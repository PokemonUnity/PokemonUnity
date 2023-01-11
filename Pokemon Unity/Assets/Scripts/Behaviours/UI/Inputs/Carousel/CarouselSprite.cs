using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Pokemon Unity/UI/Carousel/Carousel Sprite")]
public class CarouselSprite : Carousel<Sprite> {
    public override bool IsEqual(Sprite value1, Sprite value2) => value1 == value2;
}
