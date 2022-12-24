//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class SandTileHandler : MonoBehaviour
{
    private GameObject foot_print;
    private Sprite[] foot_sprites;
    private AudioSource walkSound;
    public AudioClip walkClip;

    private Coroutine currentCoroutine;

    void Awake()
    {
        foot_print = transform.Find("foot_print").gameObject;
        walkSound = transform.GetComponent<AudioSource>();
        foot_sprites = Resources.LoadAll<Sprite>("foot_print");
    }

    void Start()
    {
        foot_print.SetActive(true);
        foot_print.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("_Object") || other.name.Contains("_Transparent"))
        {

            if (other.transform.parent.name == "Player")
            {
                SfxHandler.Play(walkClip, Random.Range(0.85f, 1.1f));
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.name.Contains("map"))
        {
            Collider[] hitColliders =
                Physics.OverlapSphere(
                    new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 0.15f);
            bool deactivate = true;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].name.Contains("Transparent") || hitColliders[i].name.Contains("Object"))
                {
                    //deactivate = false;
                    i = hitColliders.Length;
                }
            }
            if (deactivate)
            {
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }
                SpriteRenderer foot = foot_print.GetComponent<SpriteRenderer>();
                if (other.transform.parent.GetComponent<PlayerMovement>() != null)
                {
                    switch (other.transform.parent.GetComponent<PlayerMovement>().direction)
                    {
                        //0 = up, 1 = right, 2 = down, 3 = left
                        case 0:
                            foot.sprite = foot_sprites[0];
                            break;
                        case 1:
                            foot.sprite = foot_sprites[1];
                            break;
                        case 2:
                            foot.sprite = foot_sprites[3];
                            break;
                        case 3:
                            foot.sprite = foot_sprites[2];
                            break;
                    }
                }
                else if (other.transform.parent.GetComponent<FollowerMovement>() != null)
                {
                    if (!other.transform.parent.GetComponent<FollowerMovement>().hide)
                    {
                        switch (other.transform.parent.GetComponent<FollowerMovement>().direction)
                        {
                            //0 = up, 1 = right, 2 = down, 3 = left
                            case 0:
                                foot.sprite = foot_sprites[0];
                                break;
                            case 1:
                                foot.sprite = foot_sprites[1];
                                break;
                            case 2:
                                foot.sprite = foot_sprites[3];
                                break;
                            case 3:
                                foot.sprite = foot_sprites[2];
                                break;
                        }
                    }
                }
                else if (other.transform.parent.GetComponent<NPCHandler>() != null)
                {
                    switch (other.transform.parent.GetComponent<NPCHandler>().direction)
                    {
                        //0 = up, 1 = right, 2 = down, 3 = left
                        case 0:
                            foot.sprite = foot_sprites[0];
                            break;
                        case 1:
                            foot.sprite = foot_sprites[1];
                            break;
                        case 2:
                            foot.sprite = foot_sprites[3];
                            break;
                        case 3:
                            foot.sprite = foot_sprites[2];
                            break;
                    }
                }

                currentCoroutine = StartCoroutine(footStep());
            }
        }
    }

    private IEnumerator footStep(float time = 1f)
    {
        SpriteRenderer foot = foot_print.GetComponent<SpriteRenderer>();

        foot.color = new Color(1, 1, 1, 0.5f);
        float increment = 0;

        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            foot.color = new Color(0, 0, 0, 0.5f - increment / 2);

            yield return null;
        }
        foot.color = new Color(1, 1, 1, 0);
    }
}