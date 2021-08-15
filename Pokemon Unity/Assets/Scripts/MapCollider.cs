//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class MapCollider : MonoBehaviour
{
    //Collision Map String provided by DeKay's Collision Map Compiler for Pokémon Essentials
    //See TOOLS folder for details

    public string shorthandCollisionMap;
    public int width;
    public int length;

    public int xModifier;
    public int zModifier;

    private int[,] collisionMap;


    void Awake()
    {
        setCollisionMap();
    }


    //Tile Tags:
    //0 - Default Environment
    //1 - Impassable
    //2 - Surf Water
    //3 - Environment 2

    public int getTileTag(Vector3 position)
    {
        int mapX =
            Mathf.RoundToInt(Mathf.Round(position.x) - Mathf.Round(transform.position.x) +
                             Mathf.Floor((float) width / 2f) - xModifier);
        int mapZ =
            Mathf.RoundToInt(length -
                             (Mathf.Round(position.z) - Mathf.Round(transform.position.z) +
                              Mathf.Floor((float) length / 2f) - zModifier));

        if (mapX < 0 || mapX >= width ||
            mapZ < 0 || mapZ >= length)
        {
//			Debug.Log (mapX +" "+ mapZ +", "+ width +" "+ length);
            return -1;
        }
        int tag = collisionMap[Mathf.FloorToInt(mapX), Mathf.FloorToInt(mapZ)];
        return tag;
    }

    public void setCollisionMap()
    {
        collisionMap = new int[width, length];

        int x = 0;
        int z = 0;
        //contains is an array of every segment of the shorthand
        // (e.g. {"0x10", "2", "1x4", "2", "0x8", "0x10", "2", etc... } )
        string[] contains = shorthandCollisionMap.Split(' ');
        string[] contains2;

        for (int i = 0; i < contains.Length; i++)
        {
            //contains2 is an array of the tag and quantity in the selected segment of the shorthand
            // (breaks "0x10" into {"0", "10"} ready for processing)
            contains2 = contains[i].Split('x');
            if (contains2.Length == 1)
            {
                //if length of contains2 is 1, there is no multiplier attached
                if (x >= this.width)
                {
                    //if x exceeds the map width,
                    x = 0; //move to the first x on the next line down
                    z += 1;
                }
                collisionMap[x, z] = int.Parse(contains2[0]); //add tag to current co-ordinates
                x += 1;
            }
            else
            {
                for (int i2 = 0; i2 < int.Parse(contains2[1]); i2++)
                {
                    //repeat for multiplier amount of times
                    if (x >= this.width)
                    {
                        //same procedure as above
                        x = 0;
                        z += 1;
                    }
                    collisionMap[x, z] = int.Parse(contains2[0]);
                    x += 1;
                }
            }
        }
    }


    /// if bridge was found, returned RaycastHit will have a collider
    public static RaycastHit getBridgeHitOfPosition(Vector3 position)
    {
        //Check for bridges below inputted position
        //cast a ray directly downwards from the position entered
        RaycastHit[] bridgeHitColliders = Physics.RaycastAll(position, Vector3.down, 3f);
        RaycastHit bridgeHit = new RaycastHit();
        //cycle through each of the collisions
        if (bridgeHitColliders.Length > 0)
        {
            for (int i = 0; i < bridgeHitColliders.Length; i++)
            {
                //if a collision's gameObject has a bridgeHandler, it is a bridge.
                if (bridgeHitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null)
                {
                    bridgeHit = bridgeHitColliders[i];
                    i = bridgeHitColliders.Length;
                }
            }
        }

        return bridgeHit;
    }

    /// returns the slope of the map geometry on the tile of the given position (in the given direction) 
    public static float getSlopeOfPosition(Vector3 position, int direction)
    {
        return getSlopeOfPosition(position, direction, true);
    }

    public static float getSlopeOfPosition(Vector3 position, int direction, bool checkForBridge)
    {
        //set vector3 based off of direction
        Vector3 movement = new Vector3(0, 0, 0);
        if (direction == 0)
        {
            movement = new Vector3(0, 0, 1f);
        }
        else if (direction == 1)
        {
            movement = new Vector3(1f, 0, 0);
        }
        else if (direction == 2)
        {
            movement = new Vector3(0, 0, -1f);
        }
        else if (direction == 3)
        {
            movement = new Vector3(-1f, 0, 0);
        }

        //cast a ray directly downwards from the edge of the tile, closest to original position (1.5f height to account for stairs)
        RaycastHit[] mapHitColliders = Physics.RaycastAll(position - (movement * 0.45f) + new Vector3(0, 1.5f, 0),
            Vector3.down);
        RaycastHit map1Hit = new RaycastHit();

        float shortestHit = Mathf.Infinity;
        int shortestHitIndex = -1;
        //cycle through each of the collisions
        for (int i = 0; i < mapHitColliders.Length; i++)
        {
            //if a collision's gameObject has a MapCollider or a BridgeHandler, it is a valid tile.
            if (checkForBridge)
            {
                if (mapHitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null ||
                    mapHitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    //check if distance is shorter than last recorded shortest
                    if (mapHitColliders[i].distance < shortestHit)
                    {
                        shortestHit = mapHitColliders[i].distance;
                        shortestHitIndex = i;
                    }
                }
            }
            else
            {
                if (mapHitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    //check if distance is shorter than last recorded shortest
                    if (mapHitColliders[i].distance < shortestHit)
                    {
                        shortestHit = mapHitColliders[i].distance;
                        shortestHitIndex = i;
                    }
                }
            }
        }
        //if index is not -1, a map/bridge was found
        if (shortestHitIndex != -1)
        {
            map1Hit = mapHitColliders[shortestHitIndex];
        }


        //cast another ray at the edge of the tile, further from original position (1.5f height to account for stairs)
        mapHitColliders = Physics.RaycastAll(position + (movement * 0.45f) + new Vector3(0, 1.5f, 0), Vector3.down);
        RaycastHit map2Hit = new RaycastHit();

        shortestHit = Mathf.Infinity;
        shortestHitIndex = -1;
        //cycle through each of the collisions
        for (int i = 0; i < mapHitColliders.Length; i++)
        {
            //if a collision's gameObject has a MapCollider or a BridgeHandler, it is a valid tile.
            if (checkForBridge)
            {
                if (mapHitColliders[i].collider.gameObject.GetComponent<BridgeHandler>() != null ||
                    mapHitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    //check if distance is shorter than last recorded shortest
                    if (mapHitColliders[i].distance < shortestHit)
                    {
                        shortestHit = mapHitColliders[i].distance;
                        shortestHitIndex = i;
                    }
                }
            }
            else
            {
                if (mapHitColliders[i].collider.gameObject.GetComponent<MapCollider>() != null)
                {
                    //check if distance is shorter than last recorded shortest
                    if (mapHitColliders[i].distance < shortestHit)
                    {
                        shortestHit = mapHitColliders[i].distance;
                        shortestHitIndex = i;
                    }
                }
            }
        }
        //if index is not -1, a map/bridge was found
        if (shortestHitIndex != -1)
        {
            map2Hit = mapHitColliders[shortestHitIndex];
        }


        if (map1Hit.collider == null || map2Hit.collider == null)
        {
            if (map1Hit.collider == null && map2Hit.collider == null)
            {
                Debug.Log("DEBUG: No Map1Hit or Map2Hit!");
            }
            else if (map1Hit.collider == null)
            {
                Debug.Log("DEBUG: No Map1Hit!");
            }
            else
            {
                Debug.Log("DEBUG: No Map1Hit!");
            }

            return 0;
        }

        //flatten the hit.point along the y, so that the distance between them will only calculate using x and z
        Vector3 flatHitPoint1 = new Vector3(map1Hit.point.x, 0, map1Hit.point.z);
        Vector3 flatHitPoint2 = new Vector3(map2Hit.point.x, 0, map2Hit.point.z);

        float rise = Mathf.Abs(map2Hit.point.y - map1Hit.point.y);
        float run = 0.9f;
        float slope = rise / run;

        return Mathf.Round(slope * 100f) / 100f;
    }
}