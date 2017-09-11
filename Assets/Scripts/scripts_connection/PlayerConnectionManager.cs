using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerConnectionManager : MonoBehaviour
{
    private GameObject playerPrefab;
    
    private Dictionary<int, GameObject> playersInRoom;

    private static PlayerConnectionManager instance;

    public static PlayerConnectionManager Instance
    {
        get
        {
            return instance;
        }
    }

    protected void Awake()
    {
        instance = this;
        playersInRoom = new Dictionary<int, GameObject>();

        playerPrefab = Resources.Load<GameObject>("Prefabs/Sloth/Sloth") as GameObject;
    }

    public GameObject GetPlayerGameObject(int playerId)
    {
        return playersInRoom[playerId];
    }

    //Make sure position is in sloverse space.
    public void SpawnPlayer(int playerId, Vector2 position, bool isClient)
    {
        GameObject player = Instantiate(playerPrefab) as GameObject;

        if (isClient)
        {
            player.AddComponent<ClientPlayerStateScript>();
        }
        else
        {
            player.AddComponent<ServerPlayerStateScript>();
        }
        
        playersInRoom.Add(playerId, player);
        Vector3 position3D = SloverseCoordinateSpaceManager.sloverseToWorldSpace(position);
        player.transform.position = new Vector3(position3D.x, position3D.y, 0);
    }

    /*public void UpdateTargetPosition(int playerId, Vector2 targetPosition, bool hasTarget, bool lerpToTarget)
    {
        GameObject player = playersInRoom[playerId];

        if (player != null && player.GetComponent<PlayerScript>().getUser() != null)
        {
            if (player.GetComponent<PlayerScript>().getUser().Id == playerId)
            {
                ClientPlayerStateScript clientState = player.GetComponent<ClientPlayerStateScript>();
                clientState.SetClientTargetPosition(targetPosition, hasTarget, lerpToTarget);
            }
            else
            {
                player.GetComponent<ServerPlayerStateScript>().SetServerTargetPosition(targetPosition, hasTarget, lerpToTarget);
            }
        }
        else
        {
            Debug.Log("Player is null. Cannot update position.");
        }
    }*/

    public void UpdateServerPlayerDirection(int playerId, EnumPlayerDirection playerDirection)
    {
        GameObject player = playersInRoom[playerId];

        if (player != null)
        {
            player.GetComponent<ServerPlayerStateScript>().SetServerDirectionImage(playerDirection);
        }
        else
        {
            Debug.Log("Server player is null. Cannot update direction.");
        }
    }

    public void RemovePlayer(int playerId)
    {
        GameObject player = playersInRoom[playerId];
        playersInRoom.Remove(playerId);
        Destroy(player);
    }
}
