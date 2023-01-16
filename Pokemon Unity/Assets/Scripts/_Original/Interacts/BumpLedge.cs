//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpLedge : MonoBehaviour
{
    public int movementDirection = 2;
    private bool active = false;

    private IEnumerator bump()
    {
        if (PlayerMovement.Singleton.Direction == movementDirection)
        {
            if (!active)
            {
                active = true;
            
                PlayerMovement.Singleton.PauseInput();
                
                PlayerMovement.Singleton.Follower.canMove = false;
                PlayerMovement.Singleton.forceMoveForward(2);
                
                if (!PlayerMovement.Singleton.IsRunning)
                {
                    yield return new WaitForSeconds(PlayerMovement.Singleton.Speed * 0.5f);
                    StartCoroutine(PlayerMovement.Singleton.jump());
                    yield return new WaitForSeconds(PlayerMovement.Singleton.Speed * 0.5f);
                }
                else
                {
                    StartCoroutine(PlayerMovement.Singleton.jump());
                    yield return new WaitForSeconds(PlayerMovement.Singleton.Speed);
                }
                yield return new WaitForSeconds(PlayerMovement.Singleton.Speed);

                if (GlobalVariables.Singleton.followerOut)
                {
                    PlayerMovement.Singleton.Follower.canMove = true;
                }
                if (!PlayerMovement.Singleton.busyWith)
                {
                    PlayerMovement.Singleton.UnpauseInput();
                }
                active = false;
            }
        }
    }
}