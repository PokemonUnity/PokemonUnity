//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractTrainerSight : MonoBehaviour
{
    private InteractTrainer trainer;

    private bool positionIsBlocked = false;
    private bool playerAtPosition = false;

    void Awake()
    {
        trainer = transform.parent.GetComponent<InteractTrainer>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player_Transparent")
        {
            if (PlayerMovement.player.busyWith != trainer.gameObject)
            {
                int playerLocation = -1;

                float playerX = Mathf.Round(PlayerMovement.player.hitBox.position.x);
                float playerZ = Mathf.Round(PlayerMovement.player.hitBox.position.z);

                if (playerX == Mathf.Round(transform.position.x))
                {
                    //player is up or down from
                    if (playerZ > transform.position.z)
                    {
                        //up
                        playerLocation = 0;
                    }
                    else
                    {
                        //down
                        playerLocation = 2;
                    }
                }
                else if (playerZ == Mathf.Round(transform.position.z))
                {
//player is right or left of
                    if (playerX > transform.position.x)
                    {
                        //right
                        playerLocation = 1;
                    }
                    else
                    {
                        //left
                        playerLocation = 3;
                    }
                }

                //if running past a random turner
                if (PlayerMovement.player.running && playerLocation != -1 &&
                    trainer.trainerBehaviour == InteractTrainer.TrainerBehaviour.Turn)
                {
                    trainer.direction = playerLocation;
                    Vector3 checkPositionVector = trainer.hitBox.position;
                    for (int i = 0; i < trainer.sightRange; i++)
                    {
                        checkPositionVector = checkPosition(checkPositionVector);
                        if (positionIsBlocked || playerAtPosition)
                        {
                            i = trainer.sightRange;
                        }
                    }
                    if (playerAtPosition)
                    {
                        StartCoroutine(trainer.spotPlayer());
                    }
                }
                else if (trainer.direction == playerLocation)
                {
                    Vector3 checkPositionVector = trainer.hitBox.position;
                    for (int i = 0; i < trainer.sightRange; i++)
                    {
                        checkPositionVector = checkPosition(checkPositionVector);
                        if (positionIsBlocked || playerAtPosition)
                        {
                            i = trainer.sightRange;
                        }
                    }
                    if (playerAtPosition)
                    {
                        StartCoroutine(trainer.spotPlayer());
                    }
                }
            }
        }
    }

    /*/
        /// Checks the position for player and returns the next position to check
        private Vector3 checkPosition(Vector3 position){
            playerAtPosition = false;
            positionIsBlocked = false;
    
            Vector3 nextPosition = position;
    
    
            Vector3 movement = new Vector3(0,0,0);
            if(trainer.direction == 0){
                movement = new Vector3(0,0,1f);}
            else if(trainer.direction == 1){
                movement = new Vector3(1f,0,0);}
            else if(trainer.direction == 2){
                movement = new Vector3(0,0,-1f);}
            else if(trainer.direction == 3){
                movement = new Vector3(-1f,0,0);}
    
            float currentTileTag = trainer.currentMap.getTileTag(position);
            //check current tileTag for stairs(3)
            if(Mathf.Floor(currentTileTag) == 3f){
                //check if stair direction is same as trainer direction
                if(Mathf.Round(((currentTileTag) - 3f)*10) == trainer.direction){
                    movement += new Vector3(0,0.5f,0);}
                else{
                    float flippedDirection = trainer.direction + 2;
                    if(flippedDirection > 3){
                        flippedDirection -= 4;}
                    if(Mathf.Round(((currentTileTag) - 3f)*10) == flippedDirection){
                        movement += new Vector3(0,-0.5f,0);}
                }
            }
            float destinationTileTag = trainer.currentMap.getTileTag(position+movement);
            //check destination tileTag for stairs(3)
            if(Mathf.Floor(destinationTileTag) == 3f){
                if(Mathf.Round(((destinationTileTag) - 3f)*10) == trainer.direction){
                    movement += new Vector3(0,0.5f,0);}
            } //else if its stair-top(4)
            else if(Mathf.Floor(destinationTileTag) == 4f){
                if(Mathf.Round(((destinationTileTag) - 4f)*10) == trainer.direction){
                    movement += new Vector3(0,-0.5f,0);}
            }
    
            destinationTileTag = trainer.currentMap.getTileTag(position+movement);
            //check destination tileTag for impassibles
            if(Mathf.Floor(destinationTileTag) == 0f || Mathf.Floor(destinationTileTag) == 2f){
                positionIsBlocked = true;}
            else if(trainer.trainerSurfing){ //if a surf trainer, normal tiles are impassible
                if(Mathf.Floor(destinationTileTag) == 1f || Mathf.Floor(destinationTileTag) == 5f){
                    positionIsBlocked = true;}
            }
            else if(Mathf.Floor(destinationTileTag) == 6f){
                positionIsBlocked = true;}
    
            nextPosition = position + movement;
            //check nextPosition for objects/followers and check for the player
            Collider[] hitColliders = Physics.OverlapSphere (nextPosition, 0.2f);
            if (hitColliders.Length > 0){
                for (int i = 0; i < hitColliders.Length; i++){
                    if(hitColliders[i].name == "Player_Transparent"){
                        playerAtPosition = true;
                        i = hitColliders.Length;}
                    else if(hitColliders[i].name == "Follower_Transparent" ||
                            hitColliders[i].name.Contains("_Object")){
                        positionIsBlocked = true;
                        i = hitColliders.Length;}
                }
            }
            
            return nextPosition;
        }
        //*/

    /// Checks the position for player and returns the next position to check
    private Vector3 checkPosition(Vector3 position)
    {
        playerAtPosition = false;
        positionIsBlocked = false;

        //Vector3 nextPosition = position;

        Vector3 forwardsVector = new Vector3(0, 0, 0);
        if (trainer.direction == 0)
        {
            forwardsVector = new Vector3(0, 0, 1f);
        }
        else if (trainer.direction == 1)
        {
            forwardsVector = new Vector3(1f, 0, 0);
        }
        else if (trainer.direction == 2)
        {
            forwardsVector = new Vector3(0, 0, -1f);
        }
        else if (trainer.direction == 3)
        {
            forwardsVector = new Vector3(-1f, 0, 0);
        }

        Vector3 movement = forwardsVector;


        //Check destination map																	//0.5f to adjust for stair height
        MapCollider destinationMap = trainer.currentMap;
        //cast a ray directly downwards from the position directly in front of the npc			//1f to check in line with player's head
        RaycastHit[] mapHitColliders = Physics.RaycastAll(position + movement + new Vector3(0, 1.5f, 0), Vector3.down);
        RaycastHit mapHit = new RaycastHit();
        //cycle through each of the collisions
        if (mapHitColliders.Length > 0)
        {
            for (int i = 0; i < mapHitColliders.Length; i++)
            {
                //if a collision's gameObject has a mapCollider, it is a map. set it to be the destination map.
                if (mapHitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    mapHit = mapHitColliders[i];
                    destinationMap = mapHit.collider.gameObject.GetComponent<MapCollider>();
                    i = mapHitColliders.Length;
                }
            }
        }

        //check for a bridge at the destination
        RaycastHit bridgeHit = MapCollider.getBridgeHitOfPosition(position + movement + new Vector3(0, 1.5f, 0));
        if (bridgeHit.collider != null)
        {
            //modify the forwards vector to align to the bridge.
            movement -= new Vector3(0, (position.y - bridgeHit.point.y), 0);
        }
        //if no bridge at destination
        else if (mapHit.collider != null)
        {
            //modify the forwards vector to align to the mapHit.
            movement -= new Vector3(0, (position.y - mapHit.point.y), 0);
        }


        float currentSlope = Mathf.Abs(MapCollider.getSlopeOfPosition(position, trainer.direction));
        float destinationSlope = Mathf.Abs(MapCollider.getSlopeOfPosition(position + forwardsVector, trainer.direction));
        float yDistance = Mathf.Abs((position.y + movement.y) - position.y);
        yDistance = Mathf.Round(yDistance * 100f) / 100f;

        //if either slope is greater than 1 it is too steep.
        if (currentSlope <= 1 && destinationSlope <= 1)
        {
            //if yDistance is greater than both slopes there is a vertical wall between them
            if (yDistance <= currentSlope || yDistance <= destinationSlope)
            {
                //check destination tileTag for impassibles
                int destinationTileTag = destinationMap.getTileTag(position + movement);
                if (destinationTileTag == 1)
                {
                    positionIsBlocked = true;
                }
                else
                {
                    if (trainer.trainerSurfing)
                    {
                        //if a surf trainer, normal tiles are impassible
                        if (destinationTileTag != 2)
                        {
                            positionIsBlocked = true;
                        }
                    }
                    else
                    {
                        //if not a surf trainer, surf tiles are impassible
                        if (destinationTileTag == 2)
                        {
                            positionIsBlocked = true;
                        }
                    }
                }

                //check destination for objects/player/follower
                Collider[] hitColliders = Physics.OverlapSphere(position + movement, 0.4f);
                if (hitColliders.Length > 0)
                {
                    for (int i = 0; i < hitColliders.Length; i++)
                    {
                        if (hitColliders[i].name == "Player_Transparent")
                        {
                            playerAtPosition = true;
                        }
                        else if (hitColliders[i].name == "Follower_Transparent" ||
                                 hitColliders[i].name.ToLowerInvariant().Contains("_object"))
                        {
                            positionIsBlocked = true;
                        }
                    }
                }
            }
        }
        return position + movement;
    }
}