using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{
    private GameObject playerPrefab;

    private GameObject clientPlayer;
    private Dictionary<int, GameObject> playersInRoom;

    private static PlayerManagerScript instance;
    public static PlayerManagerScript Instance
    {
        get
        {
            return instance;
        }
    }

    protected void Awake ()
    {
        instance = this;
        playersInRoom = new Dictionary<int, GameObject>();

        playerPrefab = Resources.Load<GameObject>("Sloth/Sloth") as GameObject;

        Debug.Log("player prefab = " + playerPrefab);
    }

    public GameObject GetPlayerObject(int playerID)
    {
        return playersInRoom[playerID];
    }

    public void SpawnClientPlayer(int playerID, float x, float y)
    {
        clientPlayer = Instantiate(playerPrefab) as GameObject;
        playersInRoom.Add(playerID, clientPlayer);

        clientPlayer.transform.position = new Vector3(x, y, 0);
    }

    public void SpawnServerPlayer(int playerID, float x, float y)
    {
        GameObject serverPlayerObj = Instantiate(playerPrefab) as GameObject;
        playersInRoom.Add(playerID, serverPlayerObj);

        serverPlayerObj.transform.position = new Vector3(x, y, 0);
    }

    public void UpdatePlayerPosition(int playerID, float x, float y)
    {
        GameObject player = playersInRoom[playerID];

        if (player != null)
        {
            player.transform.position = new Vector3(x, y, 0);
        }
        else
        {
            Debug.Log("PLAYER IS NULL. CAN'T UPDATE POSITION. AHHHHHH");
        }
    }

    public void RemovePlayer(int playerID)
    {
        GameObject player = playersInRoom[playerID];
        playersInRoom.Remove(playerID);
        Destroy(player);
    }
}
