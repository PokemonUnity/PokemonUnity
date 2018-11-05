using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

class BattleAnimationHandler : UnityEngine.MonoBehaviour
{
	//private static GameVariables PersistantPlayerData { get { return StartupSceneHandler.PersistantPlayerData; } }
	public static IEnumerator<Battle.BattleResults> BattleCoroutineResults { get; private set; }
	public IEnumerator<Battle.BattleResults> BattleCoroutine { get; private set; }
    public float time;
    AnimationClip anim;

	void Awake()
    {
        //anim = this.GetComponent<Animator>()
        //anim = this.GetComponent<Animator>()
    }

    void OnEnable()
    {

    }

    void Start()
    {
	}

    void Update()
    {
		/* Ping GameNetwork server every 15-45secs
         * If game server is offline or unable to
         * ping connection:
         * Netplay toggle-icon bool equals false
         * otherwise toggle bool to true
         */
		//StartCoroutine(PingServerEveryXsec);
		//Use coroutine that has a while loop instead of using here
		/*while (MainMenu.activeSelf)
        {
            //While scene is enabled, run coroutine to ping server
            break;
        }*/
		//int index = (int)(UnityEngine.Time.timeSinceLevelLoad * Settings.framesPerSecond);
		//index = index % sprites[].Length;string[] dir = AssetDatabase.GetSubFolders("Assets/Pokemons");
	}


    /* Create an animation controller
     * By default, create the battle effects on that controller
     * Faint animation; attack animation; idle animation
     * 
     * When loading a pokemon, check the frame count (or sprite count ...that might eat memory)
     * Adjust the animation length to match the frames (using a 1:1 frames per second)
     */
}
