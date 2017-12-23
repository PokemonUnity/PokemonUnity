//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class TextureTileAnimation : MonoBehaviour
{
    public int matID = 0;

    public int width = 16;
    public int height = 16;

    public float FPS = 8;
    private float secPerFrame;

    private float xScale;
    private float yScale;

    private int xTiles;
    private int yTiles;

    private MeshRenderer mesh;

    public bool[] applyToMaterials = new bool[0];

    void Awake()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        //get the pixel dimensions of the material's texture
        float texWidth = (float) mesh.materials[matID].GetTexture("_MainTex").width;
        float texHeight = (float) mesh.materials[matID].GetTexture("_MainTex").height;

        xScale = (float) width / texWidth;
        yScale = (float) height / texHeight;
        xTiles = Mathf.RoundToInt(1 / xScale);
        yTiles = Mathf.RoundToInt(1 / yScale);


        if (applyToMaterials.Length > 0)
        {
            for (int i = 0; i < mesh.materials.Length && i < applyToMaterials.Length; i++)
            {
                if (applyToMaterials[i])
                {
                    mesh.materials[i].SetTextureOffset("_MainTex", new Vector2(0, 0));
                    mesh.materials[i].SetTextureScale("_MainTex", new Vector2(xScale, yScale));
                }
            }
        }
        else
        {
            mesh.materials[matID].SetTextureOffset("_MainTex", new Vector2(0, 0));
            mesh.materials[matID].SetTextureScale("_MainTex", new Vector2(xScale, yScale));
        }

        StartCoroutine(animate());
    }

    void Update()
    {
        secPerFrame = 1f / FPS;
    }

    private IEnumerator animate()
    {
        int currentXTile = 0;
        int currentYTile = 0;
        while (true)
        {
            currentXTile += 1;
            if (currentXTile >= xTiles)
            {
                currentXTile = 0;
                currentYTile += 1;
                if (currentYTile >= yTiles)
                {
                    currentYTile = 0;
                }
            }

            if (applyToMaterials.Length > 0)
            {
                for (int i = 0; i < mesh.materials.Length && i < applyToMaterials.Length; i++)
                {
                    if (applyToMaterials[i])
                    {
                        mesh.materials[i].SetTextureOffset("_MainTex",
                            new Vector2(xScale * currentXTile, yScale * (yTiles - 1 - currentYTile)));
                    }
                }
            }
            else
            {
                mesh.materials[matID].SetTextureOffset("_MainTex",
                    new Vector2(xScale * currentXTile, yScale * (yTiles - 1 - currentYTile)));
            }

            yield return new WaitForSeconds(secPerFrame);
        }
    }
}