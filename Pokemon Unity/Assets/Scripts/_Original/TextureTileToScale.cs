//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class TextureTileToScale : MonoBehaviour
{
    public float scaleSizeReference = 16;

    public enum TextureAnimation
    {
        None,
        Sky,
        Lava,
        Lava2,
        LavaFlow,
        ClearWater,
        OceanWater,
        OceanWater2,
        OceanBorder1,
        OceanBorder2,
        CliffBorder,
        Waterfall_F
    }

    public TextureAnimation texAnimation = TextureAnimation.None;

    public bool tileHorizontally = true;
    public bool tileVertically = true;

    private MeshRenderer mesh;

    private float texWidth;
    private float texHeight;

    private float wMod;
    private float hMod;

    private float xOffsetPixel;
    private float yOffsetPixel;

    private float wScale;
    private float hScale;

    private float xCornerPosition;
    private float yCornerPosition;


    void Awake()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        //get the pixel dimensions of the material's texture
        texWidth = (float) mesh.material.GetTexture("_MainTex").width;
        texHeight = (float) mesh.material.GetTexture("_MainTex").height;

        //get the ratio of texture size to size reference
        wMod = (texWidth / (float) scaleSizeReference);
        hMod = (texHeight / (float) scaleSizeReference);

        //get the offset distance per pixel
        xOffsetPixel = 1f / texWidth;
        yOffsetPixel = 1f / texHeight;

        //use the rendered bounds to get the size
        wScale = mesh.bounds.size.x;
        hScale = mesh.bounds.size.z;

        //calculate the offset relative to the object's position in the game world
        xCornerPosition = 0.5f + transform.position.x - (wScale / 2f);
        yCornerPosition = 0.5f + transform.position.z - (hScale / 2f);


        float xOffset = (tileHorizontally) ? (xCornerPosition * scaleSizeReference) * xOffsetPixel : 0;
        float yOffset = (tileVertically) ? (yCornerPosition * scaleSizeReference) * yOffsetPixel : 0;
        mesh.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
        float xScale = (tileHorizontally) ? wScale / wMod : 1;
        float yScale = (tileVertically) ? hScale / hMod : 1;
        mesh.material.SetTextureScale("_MainTex", new Vector2(xScale, yScale));

        
        if (texAnimation == TextureAnimation.Sky)
        {
            StartCoroutine(animateAsSky());
        }
        else if (texAnimation == TextureAnimation.Lava)
        {
            StartCoroutine(animateAsLava());
        }
        else if (texAnimation == TextureAnimation.Lava2)
        {
            StartCoroutine(animateAsLava(true));
        }
        else if (texAnimation == TextureAnimation.LavaFlow)
        {
            StartCoroutine(animateAsDownFlow(8.7f));
        }
        else if (texAnimation == TextureAnimation.ClearWater)
        {
            StartCoroutine(animateAsClearWater());
        }
        else if (texAnimation == TextureAnimation.OceanWater)
        {
            StartCoroutine(animateAsOcean());
        }
        else if (texAnimation == TextureAnimation.OceanWater2)
        {
            StartCoroutine(animateAsOcean2());
        }
        else if (texAnimation == TextureAnimation.OceanBorder1)
        {
            StartCoroutine(animateAsOceanBorder1(1));
        }
        else if (texAnimation == TextureAnimation.OceanBorder2)
        {
            StartCoroutine(animateAsOceanBorder2());
        }
        else if (texAnimation == TextureAnimation.CliffBorder)
        {
            StartCoroutine(cliffBorder());
        }
        else if (texAnimation == TextureAnimation.Waterfall_F)
        {
            StartCoroutine(animateAsDownFlow(0.5f));
        }
    }
    
    private IEnumerator animateAsSky()
    {
        float speed = 28.0f;

        while (true)
        {
            float yMod = 0;
            float xMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                xMod = texWidth * increment;
                yMod = texHeight * increment;

                mesh.material.SetTextureOffset("_MainTex",
                    new Vector2(((xCornerPosition * scaleSizeReference) + xMod) * xOffsetPixel,
                        ((yCornerPosition * scaleSizeReference) + yMod) * yOffsetPixel));

                yield return null;
            }
        }
    }

    private IEnumerator animateAsLava()
    {
        yield return StartCoroutine(animateAsLava(false));
    }

    private IEnumerator animateAsLava(bool animBackwards)
    {
        float speed = 1.6f;
        int animState = 0; // at 4, it resets
        int scaleState = 0; // at 5, it resets
        while (true)
        {
            float xMod = 0;
            float yMod = 0;
            float sMod = 1;

            float increment = (animBackwards) ? 1 : 0; //count backwards if animBackwards is true
            bool incrementing = true;
            while (incrementing)
            {
                if (!animBackwards)
                {
                    increment += (1 / speed) * Time.deltaTime;
                    if (increment > 1)
                    {
                        increment = 1;
                        incrementing = false;
                    }
                }
                else
                {
                    increment -= (1 / speed) * Time.deltaTime;
                    if (increment < 0)
                    {
                        increment = 0;
                        incrementing = false;
                    }
                }

                if (animState == 0)
                {
                    xMod = 1.4f * increment;
                    yMod = 2 * increment;
                }
                else if (animState == 1)
                {
                    xMod = 1.4f * (1 - increment);
                    yMod = 2 + 2 * increment;
                }
                else if (animState == 2)
                {
                    xMod = -1.4f * increment;
                    yMod = 2 + (2 * (1 - increment));
                }
                else
                {
                    xMod = -1.4f * (1 - increment);
                    yMod = 2 * (1 - increment);
                }

                if (scaleState == 0)
                {
                    sMod = 1 - (0.005f * increment);
                }
                else if (scaleState == 1)
                {
                    sMod = 0.995f - (0.005f * increment);
                }
                else if (scaleState == 2)
                {
                    sMod = 0.99f + (0.01f * increment);
                }
                else if (scaleState == 3)
                {
                    sMod = 1 + (0.0075f * increment);
                }
                else
                {
                    sMod = 1.0075f - (0.0075f * increment);
                }

                //get the ratio of texture size to size reference
                wMod = texWidth / ((float) scaleSizeReference * sMod);
                hMod = texHeight / ((float) scaleSizeReference * sMod);

                //get the offset distance per pixel
                xOffsetPixel = sMod / texWidth;
                yOffsetPixel = sMod / texHeight;

                float xOffset = (tileHorizontally) ? ((xCornerPosition * scaleSizeReference) + xMod) * xOffsetPixel : 0;
                float yOffset = (tileVertically) ? ((yCornerPosition * scaleSizeReference) + yMod) * yOffsetPixel : 0;
                mesh.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
                float xScale = (tileHorizontally) ? wScale / wMod : 1;
                float yScale = (tileVertically) ? hScale / hMod : 1;
                mesh.material.SetTextureScale("_MainTex", new Vector2(xScale, yScale));

                yield return null;
            }
            if (!animBackwards)
            {
                animState = (animState >= 3) ? 0 : animState + 1;
                scaleState = (scaleState >= 4) ? 0 : scaleState + 1;
            }
            else
            {
                animState = (animState <= 0) ? 3 : animState - 1;
                scaleState = (scaleState <= 0) ? 4 : scaleState - 1;
            }
        }
    }

    private IEnumerator animateAsDownFlow(float speed)
    {
        int animState = 0; // at 4, it resets

        float texQuarterHeight = texHeight / 4;

        while (true)
        {
            float yMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                if (animState == 0)
                {
                    yMod = texQuarterHeight * increment;
                }
                else if (animState == 1)
                {
                    yMod = texQuarterHeight + (texQuarterHeight * increment);
                }
                else if (animState == 2)
                {
                    yMod = (texQuarterHeight * 2) + (texQuarterHeight * increment);
                }
                else
                {
                    yMod = (texQuarterHeight * 3) + (texQuarterHeight * increment);
                }

                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    mesh.materials[i].SetTextureOffset("_MainTex",
                        new Vector2(((xCornerPosition * scaleSizeReference)) * xOffsetPixel,
                            ((yCornerPosition * scaleSizeReference) + yMod) * yOffsetPixel));
                }
                yield return null;
            }
            animState = (animState >= 3) ? 0 : animState + 1;
        }
    }


    private IEnumerator animateAsClearWater()
    {
        float speed = 2.2f;
        int animState = 0; // at 4, it resets

        float texQuarterHeight = texHeight / 4;
        while (true)
        {
            float xMod = 0;
            float yMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                if (animState == 0)
                {
                    xMod = 5f * increment;
                    yMod = texQuarterHeight * increment;
                }
                else if (animState == 1)
                {
                    xMod = 5f * (1 - increment);
                    yMod = texQuarterHeight + (texQuarterHeight * increment);
                }
                else if (animState == 2)
                {
                    xMod = -7f * increment;
                    yMod = (texQuarterHeight * 2) + (texQuarterHeight * increment);
                }
                else
                {
                    xMod = -7f * (1 - increment);
                    yMod = (texQuarterHeight * 3) + (texQuarterHeight * increment);
                }

                mesh.material.SetTextureOffset("_MainTex",
                    new Vector2(((xCornerPosition * scaleSizeReference) + xMod) * xOffsetPixel,
                        ((yCornerPosition * scaleSizeReference) + yMod) * yOffsetPixel));
                yield return null;
            }
            animState = (animState >= 3) ? 0 : animState + 1;
        }
    }

    private IEnumerator animateAsOcean()
    {
        float speed = 8.7f;

        while (true)
        {
            float yMod = 0;
            float xMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                xMod = texWidth * increment;
                yMod = texHeight * increment;

                mesh.material.SetTextureOffset("_MainTex",
                    new Vector2(((xCornerPosition * scaleSizeReference) + xMod) * xOffsetPixel,
                        ((yCornerPosition * scaleSizeReference) + yMod) * yOffsetPixel));

                yield return null;
            }
        }
    }
    
    private IEnumerator animateAsOcean2()
    {
        float speed = 8.7f;

        while (true)
        {
            float yMod = 0;
            float xMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                xMod = texWidth * increment;
                yMod = texHeight * increment;

                mesh.material.SetTextureOffset("_MainTex",
                    new Vector2(((xCornerPosition * scaleSizeReference) - xMod) * xOffsetPixel,
                        ((yCornerPosition * scaleSizeReference) - yMod) * yOffsetPixel));

                yield return null;
            }
        }
    }
    
    private IEnumerator animateAsOceanBorder1(float speed)
    {
        int animState = 0; // at 4, it resets

        float texQuarterHeight = texHeight / 4;

        while (true)
        {
            float yMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                if (animState == 0)
                {
                    yMod = texQuarterHeight * increment;
                }
                else if (animState == 1)
                {
                    yMod = texQuarterHeight + (texQuarterHeight * increment);
                }
                else if (animState == 2)
                {
                    yMod = (texQuarterHeight * 2) + (texQuarterHeight * increment);
                }
                else
                {
                    yMod = (texQuarterHeight * 3) + (texQuarterHeight * increment);
                }

                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    mesh.materials[i].SetTextureOffset("_MainTex",
                        new Vector2(((xCornerPosition * scaleSizeReference) + yMod) * xOffsetPixel,
                            0f));
                }
                yield return null;
            }
            animState = (animState >= 3) ? 0 : animState + 1;
        }
    }
    
    private IEnumerator animateAsOceanBorder2(float speed = 1.2f)
    {
        int animState = 0; // at 4, it resets
        int animState2 = 0; // at 4, it resets

        float texQuarterHeight = texHeight / 4;

        while (true)
        {
            float xMod, yMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                if (animState == 0)
                {
                    xMod = -1*texQuarterHeight * increment;
                }
                else if (animState == 1)
                {
                    xMod = texQuarterHeight - (texQuarterHeight * increment);
                }
                else if (animState == 2)
                {
                    xMod = (texQuarterHeight * 2) - (texQuarterHeight * increment);
                }
                else
                {
                    xMod = (texQuarterHeight * 3) - (texQuarterHeight * increment);
                }
                
                float start_pos = -0.05f;
                float coeff = 5;
                
                if (animState2 == 0)
                {
                    yMod = start_pos + increment/coeff;
                }
                else if (animState2 == 1)
                {
                    yMod = start_pos + 1/coeff - increment / coeff /3;
                }
                else if (animState2 == 2)
                {
                    yMod = start_pos + 1/coeff - 1/coeff/3  - increment / coeff /3;
                }
                else if (animState2 == 3)
                {
                    yMod = start_pos + 1/coeff  - (1 / coeff / 3)*2 - increment / coeff /3;
                }

                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    mesh.materials[i].SetTextureOffset("_MainTex",
                        new Vector2(increment,
                            yMod));
                }
                yield return null;
            }
            animState = (animState >= 3) ? 0 : animState + 1;
            animState2 = (animState2 >= 3) ? 0 : animState2 + 1;
        }
    }
    
    private IEnumerator cliffBorder(float speed = 1.2f)
    {
        int animState = 0; // at 4, it resets
        int animState2 = 0; // at 4, it resets

        float texQuarterHeight = texHeight / 4;

        while (true)
        {
            float xMod, yMod = 0;

            float increment = 0;
            bool incrementing = true;
            while (incrementing)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                    incrementing = false;
                }

                if (animState == 0)
                {
                    xMod = -1*texQuarterHeight * increment;
                }
                else if (animState == 1)
                {
                    xMod = texQuarterHeight - (texQuarterHeight * increment);
                }
                else if (animState == 2)
                {
                    xMod = (texQuarterHeight * 2) - (texQuarterHeight * increment);
                }
                else
                {
                    xMod = (texQuarterHeight * 3) - (texQuarterHeight * increment);
                }
                
                float start_pos = -0.05f;
                float coeff = 5;
                
                if (animState2 == 0)
                {
                    yMod = start_pos + increment/coeff;
                }
                else if (animState2 == 1)
                {
                    yMod = start_pos + 1/coeff - increment / coeff /3;
                }
                else if (animState2 == 2)
                {
                    yMod = start_pos + 1/coeff - 1/coeff/3  - increment / coeff /3;
                }
                else if (animState2 == 3)
                {
                    yMod = start_pos + 1/coeff  - (1 / coeff / 3)*2 - increment / coeff /3;
                }

                for (int i = 0; i < mesh.materials.Length; i++)
                {
                    mesh.materials[i].SetTextureOffset("_MainTex",
                        new Vector2(0,
                            yMod));
                }
                yield return null;
            }
            animState = (animState >= 3) ? 0 : animState + 1;
            animState2 = (animState2 >= 3) ? 0 : animState2 + 1;
        }
    }
}