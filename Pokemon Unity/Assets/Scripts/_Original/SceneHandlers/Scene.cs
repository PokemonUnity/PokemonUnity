//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using PokemonUnity.Legacy;

public class Scene : MonoBehaviour
{
    public static Scene main;

    public BagHandler Bag;
    public BattleHandler Battle;
    public EvolutionHandler Evolution;
    public PartyHandler Party;
    public PauseHandler Pause;
    public PCHandler PC;
    public MenuBehaviour Settings;
    public SummaryHandler Summary;
    public TrainerHandler Trainer;
    public TypingHandler Typing;
    public StarterChoiceHandler StarterChoice;


    void Awake()
    {
        if (main == null)
        {
            main = this;
        }

        Bag.gameObject.SetActive(true);
        Battle.gameObject.SetActive(true);
        Evolution.gameObject.SetActive(true);
        Party.gameObject.SetActive(true);
        Pause.gameObject.SetActive(true);
        PC.gameObject.SetActive(true);
        Settings.gameObject.SetActive(true);
        Summary.gameObject.SetActive(true);
        Trainer.gameObject.SetActive(false); //TODO Trainer Handler
        Typing.gameObject.SetActive(true);
        StarterChoice?.gameObject.SetActive(true);
    }
}