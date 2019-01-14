using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

namespace PokemonUnity.Saving
{
    public class EventListener : UnityEngine.MonoBehaviour
    {
        public UnityEngine.GameObject Player;
        private PlayerMovement playerMovement;
        private bool RegisteredEvent = false;

        private void Awake()
        {
            GlobalSaveManager.Load(0);
            if (null == Player)
            {
                Player = UnityEngine.GameObject.FindGameObjectWithTag("Player");
                playerMovement = Player.GetComponent<PlayerMovement>();
            }
            else
            {
                Player.tag = "Player";
                playerMovement = Player.GetComponent<PlayerMovement>();
            }
            GlobalSaveManager.RegisterPlayer(Player);

            List<SaveEvent> Events = GlobalSaveManager.GetRelaventSaveData(SceneManager.GetActiveScene().buildIndex);

            foreach (SaveEvent Event in Events)
            {
                var objects = UnityEngine.Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>().Where(obj => obj.name == Event.ObjectName);
                foreach (UnityEngine.GameObject saveObject in objects)
                {
                    if (saveObject.name == Event.ObjectName && saveObject.transform.position == Event.ObjectPosition)
                    {
                        switch (Event.EventType)
                        {
                            case SaveEventType.ITEM:
                                saveObject.SetActive(false);
                                break;
                            case SaveEventType.INTERACTION:
                                //Perform action for Interaction
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
                UnityEngine.GameObject eventObject = playerMovement.busyWith;

                //Getting the events from the object
                //CustomEvent interactionEvent = eventObject.GetComponent<CustomEvent>();
                InteractItem itemEvent = eventObject.GetComponent<InteractItem>();

                /*if (null != interactionEvent)
                {
                    RegisteredEvent = true;
                    GlobalSaveManager.RegisterEvent(new CustomSaveEvent(SaveEventType.INTERACTION, eventObject, SceneManager.GetActiveScene().buildIndex));
                }
                else*/ if (null != itemEvent)
                {
                    RegisteredEvent = true;
                    GlobalSaveManager.RegisterEvent(new SaveEvent(SaveEventType.ITEM, eventObject, SceneManager.GetActiveScene().buildIndex));
                }
            }
            else if (playerMovement.busyWith == null)
            {
                RegisteredEvent = false;
            }
        }
    }
}
