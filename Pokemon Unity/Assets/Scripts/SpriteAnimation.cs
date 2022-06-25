//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class SpriteAnimation : MonoBehaviour
{
    public float FPS = 8;
    private float secPerFrame;

    public Sprite[] spriteSheet;

    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(animate());
    }

    void Update()
    {
        secPerFrame = 1f / FPS;
    }

    private IEnumerator animate()
    {
        int frame = 0;
        while (true)
        {
            frame += 1;
            if (frame >= spriteSheet.Length)
            {
                frame = 0;
            }

            sprite.sprite = spriteSheet[frame];

            yield return new WaitForSeconds(secPerFrame);
        }
    }
}