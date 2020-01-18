//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIParticleHandler : MonoBehaviour
{
    private GameObject particles;

    private Image[] particle = new Image[32];
    private bool[] particleActive = new bool[32];

    void Awake()
    {
        particles = transform.Find("Particles").gameObject;
        for (int i = 0; i < 32; i++)
        {
            particle[i] = particles.transform.Find("Particle" + i).GetComponent<Image>();
            particle[i].enabled = false;
        }
    }

    private int getFirstInactiveParticle()
    {
        int targetParticle = -1;

        for (int i = 0; i < 32; i++)
        {
            if (!particleActive[i])
            {
                targetParticle = i;
                i = 32;
            }
        }

        return targetParticle;
    }


    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float duration)
    {
        return createParticle(particleSprite, position, size, 0f, duration,
            position, 0f, size);
    }

    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float angle, float duration)
    {
        return createParticle(particleSprite, position, size, angle, duration,
            position, 0f, size);
    }

    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float angle, float duration,
        Vector2 destinationPosition)
    {
        return createParticle(particleSprite, position, size, angle, duration,
            destinationPosition, 0f, size);
    }

    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float angle, float duration,
        float rotationsPerSec)
    {
        return createParticle(particleSprite, position, size, angle, duration,
            position, rotationsPerSec, size);
    }

    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float angle, float duration,
        Vector2 destinationPosition, float rotationsPerSec)
    {
        return createParticle(particleSprite, position, size, angle, duration,
            destinationPosition, rotationsPerSec, size);
    }

    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float angle, float duration,
        float rotationsPerSec, float finalSize)
    {
        return createParticle(particleSprite, position, size, angle, duration,
            position, rotationsPerSec, finalSize);
    }

    public Image createParticle(Sprite particleSprite, Vector2 position, float size, float angle, float duration,
        Vector2 destinationPosition, float rotationsPerSec, float finalSize)
    {
        int targetParticle = getFirstInactiveParticle();
        if (targetParticle > -1)
        {
            particleActive[targetParticle] = true;
            return setUpParticle(targetParticle, particleSprite, position, size, angle, duration, destinationPosition,
                rotationsPerSec, finalSize);
        }
        return null;
    }


    private Image setUpParticle(int targetParticle, Sprite particleSprite, Vector2 position, float size, float angle,
        float duration,
        Vector2 destinationPosition, float rotationsPerSec, float finalSize)
    {
        particle[targetParticle].sprite = particleSprite;
        particle[targetParticle].rectTransform.localPosition = new Vector3(position.x, position.y, 0);
        particle[targetParticle].rectTransform.sizeDelta = new Vector2(size, size);
        particle[targetParticle].rectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        particle[targetParticle].enabled = true;

        StartCoroutine(animateParticle(targetParticle, destinationPosition, rotationsPerSec, finalSize, duration));

        return particle[targetParticle];
    }

    private IEnumerator animateParticle(int targetParticle, Vector2 destinationPosition, float rotationsPerSec,
        float finalSize, float duration)
    {
        bool moving = false;
        bool rotating = false;
        bool scaling = false;

        Vector2 startPosition = new Vector2(particle[targetParticle].transform.localPosition.x,
            particle[targetParticle].transform.localPosition.y);
        Vector2 distance = new Vector2(destinationPosition.x - startPosition.x, destinationPosition.y - startPosition.y);
        float degreesPerSec = rotationsPerSec * 360f;
        float startSize = particle[targetParticle].rectTransform.sizeDelta.x;
        float sizeDifference = finalSize - startSize;

        if (destinationPosition != startPosition)
        {
            //if destination is not the same as current position
            moving = true;
        }
        if (rotationsPerSec > 0)
        {
            //if rotations per sec is greater than 0
            rotating = true;
        }
        if (finalSize != particle[targetParticle].rectTransform.sizeDelta.x)
        {
            //if final size is not the same as current size
            scaling = true;
        }

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / duration) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            if (moving)
            {
                particle[targetParticle].rectTransform.localPosition =
                    new Vector3(startPosition.x + (distance.x * increment), startPosition.y + (distance.y * increment),
                        0);
            }
            if (rotating)
            {
                particle[targetParticle].rectTransform.localRotation =
                    Quaternion.Euler(0, 0,
                        particle[targetParticle].rectTransform.localRotation.z + (degreesPerSec * Time.deltaTime));
            }
            if (scaling)
            {
                particle[targetParticle].rectTransform.sizeDelta = new Vector2(
                    startSize + (sizeDifference * increment), startSize + (sizeDifference * increment));
            }
            yield return null;
        }

        particle[targetParticle].sprite = null;
        particle[targetParticle].enabled = false;
        particleActive[targetParticle] = false;
    }

    public void cancelAllParticles()
    {
        for (int i = 0; i < 32; i++)
        {
            particle[i].sprite = null;
            particle[i].enabled = false;
            particleActive[i] = false;
        }
    }
}