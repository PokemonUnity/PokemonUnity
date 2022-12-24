//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpLedge : MonoBehaviour
{
    public int movementDirection = 2;
    private bool active = false;

    private IEnumerator bump()
    {
        if (PlayerMovement.player.direction == movementDirection)
        {
            if (!active)
            {
                active = true;
            
                PlayerMovement.player.pauseInput();
                
                PlayerMovement.player.followerScript.canMove = false;
                PlayerMovement.player.forceMoveForward(2);
                
                if (!PlayerMovement.player.running)
                {
                    yield return new WaitForSeconds(PlayerMovement.player.speed * 0.5f);
                    StartCoroutine(PlayerMovement.player.jump());
                    yield return new WaitForSeconds(PlayerMovement.player.speed * 0.5f);
                }
                else
                {
                    StartCoroutine(PlayerMovement.player.jump());
                    yield return new WaitForSeconds(PlayerMovement.player.speed);
                }
                yield return new WaitForSeconds(PlayerMovement.player.speed);

                if (GlobalVariables.global.followerOut)
                {
                    PlayerMovement.player.followerScript.canMove = true;
                }
                if (!PlayerMovement.player.busyWith)
                {
                    PlayerMovement.player.unpauseInput();
                }
                active = false;
            }
        }
    }
}