using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
 
public class EditorHelper : MonoBehaviour
{
    //private static int totalSprites = 2; // change depending on the total sprites in this batch!
 
    [MenuItem("PK Unity/Slice Pokemon Icon Sprites")]
    static void SliceIconSprites()
    {
        //Texture2D[] textures = new Texture2D[totalSprites];
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Sprites/Pokemon/PokemonIcons");

        for (int z = 0; z < textures.Length; z++)
        {
            // path is Assets/Resources/Sprites/Sprite (1).png
            // path is Assets/Resources/Sprites/Pokemon/PokemonIcons/unfixed double size/icon013.png
            //textures[z] = Resources.Load<Texture2D>("Sprites/Sprite (" + (z + 1) + ")");
            string path = AssetDatabase.GetAssetPath(textures[z]);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;
            if (ti.spriteImportMode == SpriteImportMode.Multiple) {
                // Bug? Need to convert to single then back to multiple in order to make changes when it's already sliced
                ti.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
            ti.spriteImportMode = SpriteImportMode.Multiple;

 
            List<SpriteMetaData> newData = new List<SpriteMetaData>();
 
            int SliceWidth = 32;
            int SliceHeight = 32;
            int n = 0;
 
            for (int x = 0; x < textures[z].width; x += SliceWidth)
            {
                //From top to bottom instead of bottom to top.
                //for (int y = 0; y < textures[z].height; y += SliceHeight)
                for (int y = textures[z].height; y > 0; y -= SliceHeight)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0.5f);
                    smd.alignment = 9;
                    smd.name = string.Format("{0}_{1}",textures[z].name, n.ToString());//(textures[z].height - j) / SliceHeight + ", " + i / SliceWidth;
                    smd.rect = new Rect(x * 2, y - SliceHeight, SliceWidth * 2, SliceHeight * 2);
 
                    newData.Add(smd); n++;
                }
            }
 
            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
         }
        //Will only reimport the first icon... will need to reimport the rest if the first is good enough.
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(textures[0]), ImportAssetOptions.ForceUpdate);
        Debug.Log("Done Slicing!");
    }

    [MenuItem("PK Unity/Slice Pokemon Battle Sprites")]
    static void SliceBattleSprites()
    {
        //New Battle Animation Controller if it doesnt exist
        //var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/Animation/BattlePokemon.controller");//WithClip
        //Add state machine (Add layers?)
        //var rootStateMachine = controller.layers[0].stateMachine;

        //UnityEditor.Experimental.AssetImporters.
        //TextureImporterPlatformSettings

        //Texture2D[] textures = new Texture2D[totalSprites];
        Texture2D[] front_regular = Resources.LoadAll<Texture2D>("Sprites/Pokemon/FRONT/[R]");
        //Texture2D[] front_shiny = Resources.LoadAll<Texture2D>("Sprites/Pokemon/FRONT/[S]");
        //Texture2D[] back_regular = Resources.LoadAll<Texture2D>("Sprites/Pokemon/BACK/[R]");
        //Texture2D[] back_shiny = Resources.LoadAll<Texture2D>("Sprites/Pokemon/BACK/[S]");

        for (int z = 0; z < 1; z++)//front_regular.Length
        {
            // path is Assets/Resources/Sprites/Sprite (1).png
            // path is Assets/Resources/Sprites/Pokemon/PokemonIcons/unfixed double size/icon013.png
            //textures[z] = Resources.Load<Texture2D>("Sprites/Sprite (" + (z + 1) + ")");
            string path = AssetDatabase.GetAssetPath(front_regular[z]);
            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            //TextureImporterSettings tis = new TextureImporterSettings();
            //ti.SetTextureSettings(tis);

            if (ti.textureType != TextureImporterType.Sprite) ti.textureType = TextureImporterType.Sprite;
            //Debug.Log("Max Texture Size: " + ti.maxTextureSize);
            //Debug.Log("Compression Quality: " + ti.compressionQuality);
            //Debug.Log("Filter Mode: " + ti.filterMode);
            //new TextureImporterPlatformSettings().
            //TextureResizeAlgorithm.Bilinear;
            ti.isReadable = true;
            if (ti.spriteImportMode == SpriteImportMode.Multiple) {
                // Bug? Need to convert to single then back to multiple in order to make changes when it's already sliced
                ti.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            }
            ti.spriteImportMode = SpriteImportMode.Multiple;

 
            List<SpriteMetaData> newData = new List<SpriteMetaData>();

            //int SliceWidth = ti.maxTextureSize < 2048 ? (front_regular[z].width / 10) * (1600 / ti.maxTextureSize) : front_regular[z].width / 10;
            int Width = 1600; //front_regular[z].width < front_regular[z].height /*ti.maxTextureSize < 2048*/ ? ((front_regular[z].width * front_regular[z].height) * (1600 / ti.maxTextureSize)) / 1600 * (1600 / ti.maxTextureSize) : 1600;
            //Y = (Height times Width) divided by X 
            //(X = 1600; Y = unknown) but only when width is greater than height
            //Y is an unknown regardless if bigger or smaller than X. Math equation shouldnt change
            int Height = /*front_regular[z].width > front_regular[z].height ? /*&& ti.maxTextureSize < 2048 ? front_regular[z].height * (1600 / ti.maxTextureSize) :* / ((front_regular[z].width * front_regular[z].height) * (1600 / ti.maxTextureSize)) / 1600 :*/ 
                (int)((float)(((front_regular[z].width * front_regular[z].height) * (Width / (float)ti.maxTextureSize)) / Width) * (Width / (float)ti.maxTextureSize));
            //Debug.Log("((" + (front_regular[z].width * front_regular[z].height) + " * " + (Width / (float)ti.maxTextureSize) + ") / " + Width + ") * " + (Width / (float)ti.maxTextureSize) + " = h:" +Height);
            int SliceWidth = 160;
            int SliceHeight = 160;
            int n = 0;
 
            //From top to bottom instead of bottom to top.
            //for (int y = 0; y < textures[z].height; y += SliceHeight)
            for (int y = Height; y > 0; y -= SliceHeight)
            {
                for (int x = 0; x < Width; x += SliceWidth)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0f);
                    smd.alignment = 9;
                    smd.name = string.Format("{0}_{1}", front_regular[z].name, n.ToString());//(textures[z].height - j) / SliceHeight + ", " + i / SliceWidth;
                    smd.rect = new Rect(x, y - SliceHeight, SliceWidth, SliceHeight);

                    //if on the last row, check for empty frames
                    if (y == SliceHeight) {
                        //mip level pixelates the image, reducing the amount of colors needed to be scanned... 
                        //not sure what a good number is. but since we're only checking "if empty" it shouldn't matter
                        //UnityEngine.Color[] pix = front_regular[z].GetPixels((front_regular[z].width / 10) * ((n%10)-1), y - SliceHeight, SliceWidth, SliceHeight);
                        UnityEngine.Color[] pix = front_regular[z].GetPixels((front_regular[z].width / 10) * (n%10), y - SliceHeight, (int)(Width / (float)ti.maxTextureSize), (int)(Height / (float)ti.maxTextureSize));
                        Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", (front_regular[z].width / 10) * (n % 10), y - SliceHeight, (int)(Width / (float)ti.maxTextureSize), (int)(Height / (float)ti.maxTextureSize), pix[0].ToString(), pix[0].grayscale, pix[0].linear.ToString(), pix[0].gamma.ToString()));
                        //Debug.Log("length: " + pix.Length + "; rank: " + pix.Rank + "; data: " + pix[0].ToString());
                        if (System.Array.TrueForAll<Color>(pix, e => (e.linear.r > 0.3f || e.linear.g > 0.3f || e.linear.b > 0.3f) & e.a > 0.3f))//(pix != null)
                            newData.Add(smd);
                        //else break;//n += 9;
                    }else newData.Add(smd); n++;
                }
            }

            /*AnimationClip newClip = new AnimationClip();
            AnimationClipSettings acs = AnimationUtility.GetAnimationClipSettings(newClip);

            newClip.wrapMode = WrapMode.Loop;
            acs.loopTime = true;
            acs.startTime = 0f;
            acs.stopTime = 0f;

            AnimationUtility.SetAnimationClipSettings(newClip, acs);*/
 
            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            Debug.Log("Done Slicing Regular FRONT Sprite " + (z+1).ToString());
            //ti.filterMode = FilterMode.Point; ti.textureCompression = TextureImporterCompression.Uncompressed; ti.maxTextureSize = 2048; // ti.compressionQuality = 0;
        }
        //Will only reimport the first icon... will need to reimport the rest if the first is good enough.
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(textures[0]), ImportAssetOptions.ForceUpdate);
        Debug.Log("Done Slicing!");
    }
}