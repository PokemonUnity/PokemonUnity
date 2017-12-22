//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class BumpLedge : MonoBehaviour
{
    public int movementDirection = 2;


    private IEnumerator bump()
    {
        if (PlayerMovement.player.direction == movementDirection)
        {
            PlayerMovement.player.pauseInput();

            PlayerMovement.player.forceMoveForward(2);
            PlayerMovement.player.followerScript.canMove = false;
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

            PlayerMovement.player.followerScript.canMove = true;
            PlayerMovement.player.unpauseInput();
        }
    }
}