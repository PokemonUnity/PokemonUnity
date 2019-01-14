using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ReSkinAnimation : UnityEngine.MonoBehaviour
{
    public string spriteSheetName;

    SpriteRenderer gameObjectSprite;
    public List<SpriteCollection> skins = new List<SpriteCollection>();
    public int skin = 0;

    void Start()
    {
        gameObjectSprite = this.GetComponent<SpriteRenderer>();

        //load all sprites for the pokemon
        foreach (SpriteCollection coll in skins)
            coll.sprites = Resources.LoadAll<Sprite>(coll.sheet.name);
    }

    /// <summary>
    /// Switches out animation/sprite frames for others from source with matching names
    /// </summary>
    /// Must refactor, will eat a lot of resources if used like this
    void LateUpdate()
    {
        //var subSprite = Resources.LoadAll<Sprite>("Sprite/Pokemon/Path" + spriteSheetName);
        SpriteCollection coll = skins[this.skin];

            string spriteName = gameObjectSprite.sprite.name;

            Sprite newSprite = //Array.Find(subSprite, item => item.name == spriteName);
                //Gotta figure out a way to sort out that 0...
                coll.sprites.Where(item => item.name == spriteName).ToArray()[0];

            if (newSprite) gameObjectSprite.sprite = newSprite;
    }

    [System.Serializable]
    public class SpriteCollection
    {
        public string name;
        public Texture sheet;

        [System.NonSerialized]
        public Sprite[] sprites;
    }
}


/// <summary>
/// Pixel-Perfect camera resolution-display.
/// </summary>
[ExecuteInEditMode] //runs the code without needing to press "Play"
class PixelDensityCamera : UnityEngine.MonoBehaviour
{
    public float pixelsToUnit = 100;

    void Update()
    {
        gameObject.GetComponent<Camera>().orthographicSize = Screen.height / pixelsToUnit / 2;
    }
}

/// <summary>
/// Changes camera resolution to adjust for different platforms
/// </summary>
[ExecuteInEditMode] //runs the code without needing to press "Play"
class ScaleWidthCamera : UnityEngine.MonoBehaviour
{
    public float targetWidth = 640;
    public float pixelsToUnit = 100;

    void Update()
    {
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);

        gameObject.GetComponent<Camera>().orthographicSize = height / pixelsToUnit / 2;
    }
}