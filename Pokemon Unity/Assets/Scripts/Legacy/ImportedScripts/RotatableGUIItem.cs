//Original Scripts by http://answers.unity3d.com/users/82/duck.html 

using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class RotatableGUIItem : MonoBehaviour
{
    //Avoid using this if possible. Unity's Canvas UI system does this with much less trouble.

    public Texture texture = null;
    public float angle = 0;
    public Vector2 size = new Vector2(128, 128);
    public bool hide = false;
    public Color color = Color.white;
    public Material material;
    private Vector2 pos = new Vector2(0, 0);
    private Rect rect;
    private Vector2 pivot;


    public RenderTexture m_TargetTexture;


    private static bool textureCleared = false;

    void Start()
    {
        UpdateSettings();
    }

    void Update()
    {
        textureCleared = false;
    }

    void UpdateSettings()
    {
        pos = new Vector2(transform.localPosition.x, transform.localPosition.y);
        if (hide)
        {
            rect = new Rect(0, 0, 0, 0);
        }
        else
        {
            rect = new Rect(Mathf.RoundToInt(pos.x * 342f) - size.x * 0.5f,
                Mathf.RoundToInt(pos.y * 192f) - size.y * 0.5f, size.x, size.y);
        }
        pivot = new Vector2(rect.xMin + rect.width * 0.5f, rect.yMin + rect.height * 0.5f);
    }

    void OnGUI()
    {
        BeginRenderTextureGUI(m_TargetTexture);

        if (Application.isEditor)
        {
            UpdateSettings();
        }
        if (texture != null)
        {
            UpdateSettings();
            Matrix4x4 matrixBackup = GUI.matrix;
            GUIUtility.RotateAroundPivot(angle, pivot);
            if (material == null)
            {
                GUI.color = color;
                GUI.DrawTexture(rect, texture);
            }
            else
            {
                Graphics.DrawTexture(rect, texture, new Rect(0, 0, 1, 1), 0, 0, 0, 0, color, material);
            }
            GUI.matrix = matrixBackup;
            GUI.depth = Mathf.FloorToInt(transform.localPosition.z) * -1;
        }

        EndRenderTextureGUI();
    }

    RenderTexture m_PreviousActiveTexture = null;

    protected void BeginRenderTextureGUI(RenderTexture targetTexture)
    {
        if (Event.current.type == EventType.Repaint)
        {
            m_PreviousActiveTexture = RenderTexture.active;
            if (targetTexture != null)
            {
                RenderTexture.active = targetTexture;
                if (!textureCleared)
                {
                    textureCleared = true;
                    GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
                }
            }
        }
    }

    protected void EndRenderTextureGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            RenderTexture.active = m_PreviousActiveTexture;
        }
    }

    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int) sprite.rect.width, (int) sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int) sprite.textureRect.x,
                (int) sprite.textureRect.y,
                (int) sprite.textureRect.width,
                (int) sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }
}