//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpLedge : MonoBehaviour
{
    public int movementDirection = 2;
    private bool active = false;

    private IEnumerator bump()
    {
        if (PlayerMovement.Singleton.direction == movementDirection)
        {
            if (!active)
            {
                active = true;
            
                PlayerMovement.Singleton.pauseInput();
                
                PlayerMovement.Singleton.Follower.canMove = false;
                PlayerMovement.Singleton.forceMoveForward(2);
                
                if (!PlayerMovement.Singleton.running)
                {
                    yield return new WaitForSeconds(PlayerMovement.Singleton.speed * 0.5f);
                    StartCoroutine(PlayerMovement.Singleton.jump());
                    yield return new WaitForSeconds(PlayerMovement.Singleton.speed * 0.5f);
                }
                else
                {
                    StartCoroutine(PlayerMovement.Singleton.jump());
                    yield return new WaitForSeconds(PlayerMovement.Singleton.speed);
                }
                yield return new WaitForSeconds(PlayerMovement.Singleton.speed);

                if (GlobalVariables.Singleton.followerOut)
                {
                    PlayerMovement.Singleton.Follower.canMove = true;
                }
                if (!PlayerMovement.Singleton.busyWith)
                {
                    PlayerMovement.Singleton.unpauseInput();
                }
                active = false;
            }
        }
    }
}