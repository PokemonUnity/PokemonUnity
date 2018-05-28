//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractItem : MonoBehaviour
{
    private DialogBoxHandler Dialog;

    public string item;
    public int quantity;

    public bool hidden = false;
    public bool TM = false;

    public Sprite itemBall;
    public Sprite tmBall;

    private SpriteRenderer itemSprite;
    private SpriteRenderer itemShadow;

    void Awake()
    {
        Dialog = GameObject.Find("GUI").GetComponent<DialogBoxHandler>();

        itemSprite = transform.Find("ItemSprite").GetComponent<SpriteRenderer>();
        itemShadow = transform.Find("ItemShadow").GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (!hidden)
        {
            if (TM)
            {
                itemSprite.sprite = tmBall;
            }
            else
            {
                itemSprite.sprite = itemBall;
            }
        }
        else
        {
            itemSprite.enabled = false;
            itemShadow.enabled = false;
        }
    }


    public IEnumerator interact()
    {
        if (PlayerMovement.player.setCheckBusyWith(this.gameObject))
        {
            AudioClip itemGetMFX = (TM)
                ? Resources.Load<AudioClip>("Audio/mfx/GetGood")
                : Resources.Load<AudioClip>("Audio/mfx/GetDecent");
            BgmHandler.main.PlayMFX(itemGetMFX);

            string firstLetter = item.Substring(0, 1).ToLowerInvariant();
            Dialog.drawDialogBox();
            if (TM)
            {
                Dialog.StartCoroutine("drawText",
                    SaveData.currentSave.playerName + " found TM" + ItemDatabaseOld.getItem(item).getTMNo() + ": " + item +
                    "!");
            }
            else
            {
                if (quantity > 1)
                {
                    Dialog.StartCoroutine("drawText", SaveData.currentSave.playerName + " found " + item + "s!");
                }
                else if (firstLetter == "a" || firstLetter == "e" || firstLetter == "i" || firstLetter == "o" ||
                         firstLetter == "u")
                {
                    Dialog.StartCoroutine("drawText", SaveData.currentSave.playerName + " found an " + item + "!");
                }
                else
                {
                    Dialog.StartCoroutine("drawText", SaveData.currentSave.playerName + " found a " + item + "!");
                }
            }
            yield return new WaitForSeconds(itemGetMFX.length);

            bool itemAdd = SaveData.currentSave.Bag.addItem(item, quantity);

            Dialog.drawDialogBox();
            if (itemAdd)
            {
                itemSprite.enabled = false;
                itemShadow.enabled = false;
                transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                if (TM)
                {
                    yield return
                        Dialog.StartCoroutine("drawTextSilent",
                            SaveData.currentSave.playerName + " put the TM" + ItemDatabaseOld.getItem(item).getTMNo() +
                            " \\away into the bag.");
                }
                else
                {
                    if (quantity > 1)
                    {
                        yield return
                            Dialog.StartCoroutine("drawTextSilent",
                                SaveData.currentSave.playerName + " put the " + item + "s \\away into the bag.");
                    }
                    else
                    {
                        yield return
                            Dialog.StartCoroutine("drawTextSilent",
                                SaveData.currentSave.playerName + " put the " + item + " \\away into the bag.");
                    }
                }
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.undrawDialogBox();

                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
                gameObject.SetActive(false);
            }
            else
            {
                yield return Dialog.StartCoroutine("drawTextSilent", "But there was no room...");
                while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
                {
                    yield return null;
                }
                Dialog.undrawDialogBox();

                PlayerMovement.player.unsetCheckBusyWith(this.gameObject);
            }
        }
    }

    public void disableItem()
    {
        itemSprite.enabled = false;
        itemShadow.enabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        gameObject.SetActive(false);
    }
}