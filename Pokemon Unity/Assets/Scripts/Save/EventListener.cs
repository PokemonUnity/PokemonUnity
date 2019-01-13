using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class EventListener : MonoBehaviour
{
    public GameObject Player;
    private PlayerMovement playerMovement;
    private bool RegisteredEvent = false;

    private void Awake()
    {
        GlobalSaveManager.Load(0);

        if (null == Player)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            playerMovement = Player.GetComponent<PlayerMovement>();
        }
        GlobalSaveManager.RegisterPlayer(Player);

        List<CustomSaveEvent> Events = GlobalSaveManager.GetRelaventSaveData(SceneManager.GetActiveScene().buildIndex);

        foreach(CustomSaveEvent Event in Events)
        {
            var objects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => obj.name == Event.ObjectName);
            foreach (GameObject saveObject in objects)
            {
                if (saveObject.name == Event.ObjectName && saveObject.transform.position == Event.ObjectPosition)
                {
                    switch (Event.EventType)
                    {
                        case SaveEventType.ITEM:
                            saveObject.SetActive(false);
                            break;
                        case SaveEventType.INTERACTION:
                            saveObject.SetActive(false);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void Update()
    {
        if (playerMovement.busyWith != null && RegisteredEvent == false)
        {
            //Getting the Object the Player's busy with
            GameObject eventObject = playerMovement.busyWith;

            //Getting the events from the object
            CustomEvent interactionEvent = eventObject.GetComponent<CustomEvent>();
            InteractItem itemEvent = eventObject.GetComponent<InteractItem>();

            if (null != interactionEvent)
            {
                RegisteredEvent = true;
                GlobalSaveManager.RegisterEvent(new CustomSaveEvent(DateTime.Now, SaveEventType.INTERACTION, eventObject, SceneManager.GetActiveScene().buildIndex));
            }
            else if (null != itemEvent)
            {
                RegisteredEvent = true;
                GlobalSaveManager.RegisterEvent(new CustomSaveEvent(DateTime.Now, SaveEventType.ITEM, eventObject, SceneManager.GetActiveScene().buildIndex));
                GlobalSaveManager.Save();
            }
        }
        else if(playerMovement.busyWith == null)
        {
            RegisteredEvent = false;
        }
    }
}
