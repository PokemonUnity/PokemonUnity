using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 
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
        Texture2D[] textures = Resources.LoadAll<Texture2D>("Sprites/Pokemon/FRONT/[R]").OrderBy(t => t.name.PadNumbers()).ToArray();
        //Texture2D[] textures = AssetDatabase.IsValidFolder.Resources.LoadAll<Texture2D>("Sprites/Pokemon/FRONT/[R]");
        //Texture2D[] front_regular = Resources.LoadAll<Texture2D>("Sprites/Pokemon/FRONT/[R]");
        //Texture2D[] front_shiny = Resources.LoadAll<Texture2D>("Sprites/Pokemon/FRONT/[S]");
        //Texture2D[] back_regular = Resources.LoadAll<Texture2D>("Sprites/Pokemon/BACK/[R]");
        //Texture2D[] back_shiny = Resources.LoadAll<Texture2D>("Sprites/Pokemon/BACK/[S]");

        int[] frames = new int[] {
            //gen1
            99,111,167,167,107,89,143,51,87,244,81,111,89,89,123,79,60,41,35,71,101,101,119,119,61,21,71,74,112,112,167,167,123,111,89,49,123,121,99,97,133,102,71,53,111,109,54,54,16,16,109,95,95,71,71,73,71,95,21,108,83,139,107,44,76,120,56,141,71,79,107,89,113,119,119,105,105,149,98,59,95,97,71,95,80,103,47,91,91,47,89,113,105,143,65,189,189,155,155,49,121,116,103,87,101,110,25,88,99,153,125,125,73,97,71,49,53,89,157,136,112,81,70,87,99,157,153,193,193,116,63,253,80,64,114,114,96,96,79,159,127,137,137,107,95,65,143,79,120,120,87,87,65,80,125,101,99,119,60,68,155,67,47,17,173,51,17,53,49,127,102,125,119,
            //gen2
            51,119,91,91,128,82,112,87,179,83,71,111,135,72,97,97,40,40,65,37,95,63,126,113,62,161,101,104,129,97,97,97,111,73,107,135,145,67,67,95,95,79,95,95,78,78,65,90,42,97,97,73,73,150,65,61,61,101,95,95,90,95,95,95,95,95,95,95,95,95,95,95,95,93,95,95,95,95,95,95,95,95,95,95,95,95,95,121,121,105,105,95,54,95,67,67,95,95,104,125,65,105,105,103,84,84,113,113,78,124,124,64,63,63,71,71,114,91,33,33,119,167,96,119,106,106,54,109,127,127,95,85,95,74,129,95,95,67,95,96,175,111,95,86,63,85,120,83,64,
            //gen3
            108,151,125,149,149,125,125,113,113,139,122,92,93,89,112,114,71,90,154,154,93,164,164,111,85,76,76,259,95,95,107,107,65,136,151,142,131,101,243,120,171,53,51,48,88,110,107,74,17,59,114,117,115,135,92,15,153,150,92,155,31,115,109,287,287,89,89,83,77,144,144,55,45,122,122,98,98,99,99,131,127,107,111,49,49,106,106,129,125,29,104,33,85,125,94,114,114,23,101,72,85,167,223,143,191,153,99,81,101,141,132,105,153,91,191,191,85,85,102,78,85,84,127,84,116,88,108,137,107,53,99,86,112,109,85,113,107,113,113,77,53,95,125,69,173,105,101,83,81,107,76,249,99,115,146,160,126,169,100,
            //gen4
            77,113,115,140,127,133,149,125,131,99,99,113,113,93,93,103,103,161,161,149,149,101,101,127,127,192,192,171,171,108,203,203,125,175,116,122,163,163,163,225,225,225,18,32,32,224,112,112,119,119,168,168,117,79,95,103,103,121,109,191,191,127,71,183,105,29,85,124,121,163,95,95,125,109,155,159,56,160,223,125,125,54,54,89,89,143,125,143,188,188,91,91,105,177,155,155,127,127,115,173,173,171,171,96,111,111,81,81,131,131,95,130,251,251,191,191,103,160,95,112,137,171,131,165,165,95,132,237,95,131,171,171,160,163,213,192,191,145,263,137,127,203,145,26,157,191,143,119,143,173,113,84,84,84,84,84,84,84,84,84,84,84,84,84,84,84,84,165,
            //gen5
            98,145,96,147,117,125,165,295,101,193,105,167,127,117,151,65,117,174,155,95,95,74,101,181,60,85,149,150,150,159,153,129,117,197,160,110,103,167,121,133,143,139,184,97,192,111,127,156,127,99,179,138,233,127,117,107,120,185,185,104,118,104,158,133,144,144,81,119,143,167,117,144,144,127,183,127,80,203,135,118,179,141,101,139,159,191,121,159,143,83,125,107,145,214,131,131,131,131,139,139,139,139,188,175,238,99,95,165,165,137,137,117,63,143,163,131,107,171,171,210,85,126,113,105,155,131,167,150,106,182,185,224,143,123,218,123,133,135,127,81,160,134,157,153,142,41,139,141,96,72,126,113,75,117,48,135,161,183,307,235,188,197,223,191,317,191,172,357,113,105,144,152,299,179,179,179,179,179
        };

        object[] args = new object[2] { 0, 0 };
        System.Reflection.MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        for (int z = 0; z < textures.Length; z++)//
        {
            // path is Assets/Resources/Sprites/Sprite (1).png
            // path is Assets/Resources/Sprites/Pokemon/PokemonIcons/unfixed double size/icon013.png
            //textures[z] = Resources.Load<Texture2D>("Sprites/Sprite (" + (z + 1) + ")");
            string path = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(textures[z]));
            //Debug.Log(path + "; " + AssetDatabase.GetAssetPath(textures[z]));
            TextureImporter ti_FR = AssetImporter.GetAtPath("Assets/Resources/Sprites/Pokemon/FRONT/[R]/" + path) as TextureImporter;
            //TextureImporter ti_BR = AssetDatabase.IsValidFolder("Sprites/Pokemon/BACK/[R]") ? AssetImporter.GetAtPath("Sprites/Pokemon/BACK/[R]/"+path) as TextureImporter:null;
            TextureImporter ti_BR = AssetImporter.GetAtPath("Assets/Resources/Sprites/Pokemon/BACK/[R]/" + path) as TextureImporter;
            //Shiny
            TextureImporter ti_FS = AssetImporter.GetAtPath("Assets/Resources/Sprites/Pokemon/FRONT/[S]/" + path) as TextureImporter;
            TextureImporter ti_BS = AssetImporter.GetAtPath("Assets/Resources/Sprites/Pokemon/BACK/[S]/" + path) as TextureImporter;

            //TextureImporterSettings tis = new TextureImporterSettings();
            //ti.SetTextureSettings(tis);

            if (ti_FR.textureType != TextureImporterType.Sprite) ti_FR.textureType = TextureImporterType.Sprite;
            if (ti_BR.textureType != TextureImporterType.Sprite) ti_BR.textureType = TextureImporterType.Sprite;
            //Shiny
            if (ti_FS.textureType != TextureImporterType.Sprite) ti_FS.textureType = TextureImporterType.Sprite;
            if (ti_BS.textureType != TextureImporterType.Sprite) ti_BS.textureType = TextureImporterType.Sprite;

            //Debug.Log("Max Texture Size: " + ti.maxTextureSize);
            //Debug.Log("Compression Quality: " + ti.compressionQuality);
            //Debug.Log("Filter Mode: " + ti.filterMode);

            //new TextureImporterPlatformSettings().
            //TextureResizeAlgorithm.Bilinear;
            ti_FR.isReadable = ti_BR.isReadable = ti_FS.isReadable = ti_BS.isReadable = true;
            if (ti_FR.spriteImportMode == SpriteImportMode.Multiple) {
                // Bug? Need to convert to single then back to multiple in order to make changes when it's already sliced
                ti_FR.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset("Assets/Resources/Sprites/Pokemon/FRONT/[R]/" + path, ImportAssetOptions.ForceUpdate);
            }
            if (ti_BR.spriteImportMode == SpriteImportMode.Multiple) {
                // Bug? Need to convert to single then back to multiple in order to make changes when it's already sliced
                ti_BR.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset("Assets/Resources/Sprites/Pokemon/BACK/[R]/" + path, ImportAssetOptions.ForceUpdate);
            }
            if (ti_FS.spriteImportMode == SpriteImportMode.Multiple) {
                // Bug? Need to convert to single then back to multiple in order to make changes when it's already sliced
                ti_FS.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset("Assets/Resources/Sprites/Pokemon/FRONT/[S]/" + path, ImportAssetOptions.ForceUpdate);
            }
            if (ti_BS.spriteImportMode == SpriteImportMode.Multiple) {
                // Bug? Need to convert to single then back to multiple in order to make changes when it's already sliced
                ti_BS.spriteImportMode = SpriteImportMode.Single;
                AssetDatabase.ImportAsset("Assets/Resources/Sprites/Pokemon/BACK/[S]/" + path, ImportAssetOptions.ForceUpdate);
            }
            ti_FR.spriteImportMode = ti_BR.spriteImportMode = ti_FS.spriteImportMode = ti_BS.spriteImportMode = SpriteImportMode.Multiple;

 
            List<SpriteMetaData> newData = new List<SpriteMetaData>();

            mi.Invoke(ti_FR, args);//textures[z]
            
            //int SliceWidth = ti.maxTextureSize < 2048 ? (front_regular[z].width / 10) * (1600 / ti.maxTextureSize) : front_regular[z].width / 10;
            int Width = (int)args[0]; //1600; //front_regular[z].width < front_regular[z].height /*ti.maxTextureSize < 2048*/ ? ((front_regular[z].width * front_regular[z].height) * (1600 / ti.maxTextureSize)) / 1600 * (1600 / ti.maxTextureSize) : 1600;
            //Y = (Height times Width) divided by X 
            //(X = 1600; Y = unknown) but only when width is greater than height
            //Y is an unknown regardless if bigger or smaller than X. Math equation shouldnt change
            int Height = (int)args[1];/*front_regular[z].width > front_regular[z].height ? /*&& ti.maxTextureSize < 2048 ? front_regular[z].height * (1600 / ti.maxTextureSize) :* / ((front_regular[z].width * front_regular[z].height) * (1600 / ti.maxTextureSize)) / 1600 :*/ 
                //(int)((float)(((textures[z].width * textures[z].height) * (Width / (float)ti_FR.maxTextureSize)) / Width) * (Width / (float)ti_FR.maxTextureSize));
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
                    if (n == frames[z]) break;// continue;
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = new Vector2(0.5f, 0f);
                    smd.alignment = 9;
                    smd.name = string.Format("{0}_{1}", textures[z].name, n.ToString());//(textures[z].height - j) / SliceHeight + ", " + i / SliceWidth;
                    smd.rect = new Rect(x, y - SliceHeight, SliceWidth, SliceHeight);

                    //if on the last row, check for empty frames
                    /*if (y == SliceHeight) {
                        //mip level pixelates the image, reducing the amount of colors needed to be scanned... 
                        //not sure what a good number is. but since we're only checking "if empty" it shouldn't matter
                        //UnityEngine.Color[] pix = front_regular[z].GetPixels((front_regular[z].width / 10) * ((n%10)-1), y - SliceHeight, SliceWidth, SliceHeight);
                        UnityEngine.Color[] pix = front_regular[z].GetPixels((front_regular[z].width / 10) * (n%10), y - SliceHeight, (int)(Width / (float)ti.maxTextureSize), (int)(Height / (float)ti.maxTextureSize));
                        Debug.Log(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", (front_regular[z].width / 10) * (n % 10), y - SliceHeight, (int)(Width / (float)ti.maxTextureSize), (int)(Height / (float)ti.maxTextureSize), pix[0].ToString(), pix[0].grayscale, pix[0].linear.ToString(), pix[0].gamma.ToString()));
                        //Debug.Log("length: " + pix.Length + "; rank: " + pix.Rank + "; data: " + pix[0].ToString());
                        if (System.Array.TrueForAll<Color>(pix, e => (e.linear.r > 0.3f || e.linear.g > 0.3f || e.linear.b > 0.3f) & e.a > 0.3f))//(pix != null)
                            newData.Add(smd);
                        //else break;//n += 9;
                    }else*/ newData.Add(smd); n++;
                }
            }
            //Debug.Assert(newData.Count == frames[z]);
            if(newData.Count == frames[z])
            Debug.Log(string.Format("{0}; Frame Count: {1}; Frame Slices: {2}",newData.Count == frames[z], newData.Count, frames[z]));
            else Debug.LogWarning(string.Format("{0}; Frame Count: {1}; Frame Slices: {2}; Image Name: {3}; #: {4}",newData.Count == frames[z], newData.Count, frames[z], textures[z].name, z+1));

            /*AnimationClip newClip = new AnimationClip();
            AnimationClipSettings acs = AnimationUtility.GetAnimationClipSettings(newClip);

            newClip.wrapMode = WrapMode.Loop;
            acs.loopTime = true;
            acs.startTime = 0f;
            acs.stopTime = 0f;

            AnimationUtility.SetAnimationClipSettings(newClip, acs);*/

            ti_FR.spritesheet = ti_BR.spritesheet = ti_FS.spritesheet = ti_BS.spritesheet = newData.ToArray();
            //AssetDatabase.ImportAsset("Sprites/Pokemon/FRONT/[R]/" + path, ImportAssetOptions.ForceUpdate);
            //AssetDatabase.ImportAsset("Sprites/Pokemon/BACK/[R]/" + path, ImportAssetOptions.ForceUpdate);
            //AssetDatabase.ImportAsset("Sprites/Pokemon/FRONT/[S]/" + path, ImportAssetOptions.ForceUpdate);
            //AssetDatabase.ImportAsset("Sprites/Pokemon/BACK/[S]/" + path, ImportAssetOptions.ForceUpdate);
            //ti_FR.filterMode = FilterMode.Point; ti_FR.textureCompression = TextureImporterCompression.Uncompressed; ti_FR.maxTextureSize = 2048; // ti_FR.compressionQuality = 0;
            //ti_BR.filterMode = FilterMode.Point; ti_BR.textureCompression = TextureImporterCompression.Uncompressed; ti_BR.maxTextureSize = 2048; // ti_BR.compressionQuality = 0;
            //ti_FS.filterMode = FilterMode.Point; ti_FS.textureCompression = TextureImporterCompression.Uncompressed; ti_FS.maxTextureSize = 2048; // ti_FS.compressionQuality = 0;
            //ti_BS.filterMode = FilterMode.Point; ti_BS.textureCompression = TextureImporterCompression.Uncompressed; ti_BS.maxTextureSize = 2048; // ti_BS.compressionQuality = 0;
        }
        //Will only reimport the first icon... will need to reimport the rest if the first is good enough.
        //AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(textures[0]), ImportAssetOptions.ForceUpdate);
        //This should update everything one time. but it doesn't do anything...
        AssetDatabase.Refresh();
        Debug.Log("Done Slicing!");
    }
}