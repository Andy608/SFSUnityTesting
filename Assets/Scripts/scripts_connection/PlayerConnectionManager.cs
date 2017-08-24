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
        playersInRoom.Add(playerId, player);
        player.transform.position = new Vector3(position.x, position.y, 0);
    }

    public void UpdateTargetPosition(int playerId, Vector2 targetPosition, bool hasTarget, bool lerpToTarget)
    {
        GameObject player = playersInRoom[playerId];

        if (player != null)
        {
            //Get movement script for player and update target position.
            player.GetComponent<PlayerMoveScript>().SetTargetPosition(targetPosition, hasTarget, lerpToTarget);
        }
        else
        {
            Debug.Log("Player is null. Cannot update position.");
        }
    }
}
