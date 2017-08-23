using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerScript : MonoBehaviour
{
    private GameObject clientPlayerPrefab;
    private GameObject serverPlayerPrefab;

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

        clientPlayerPrefab = Resources.Load<GameObject>("Prefabs/Sloth/ClientSloth") as GameObject;
        serverPlayerPrefab = Resources.Load<GameObject>("Prefabs/Sloth/ServerSloth") as GameObject;
    }

    public GameObject GetPlayerObject(int playerID)
    {
        return playersInRoom[playerID];
    }

    public void SpawnClientPlayer(int playerID, float x, float y)
    {
        clientPlayer = Instantiate(clientPlayerPrefab) as GameObject;
        playersInRoom.Add(playerID, clientPlayer);

        clientPlayer.transform.position = SloverseCoordinateSpaceManager.sloverseToWorldSpace(new Vector3(x, y, 0));
    }

    public void SpawnServerPlayer(int playerID, float x, float y)
    {
        GameObject serverPlayerObj = Instantiate(serverPlayerPrefab) as GameObject;
        playersInRoom.Add(playerID, serverPlayerObj);

        serverPlayerObj.transform.position = SloverseCoordinateSpaceManager.sloverseToWorldSpace(new Vector3(x, y, 0));
    }

    public void UpdatePlayerPosition(int playerID, Vector3 targetPosition, Vector3 serverPosition, bool interpolate, bool hasTarget)
    {
        GameObject player = playersInRoom[playerID];

        if (player != null)
        {
            PlayerPositionLerpScript playerScript = player.GetComponent<PlayerPositionLerpScript>();
            playerScript.updateServerPosition(targetPosition, serverPosition, interpolate, hasTarget);
        }
        else
        {
            Debug.Log("PLAYER IS NULL. CAN'T UPDATE POSITION. AHHHHHH");
        }
    }

    public void UpdatePlayerDirection(int playerID, EnumLookDirection lookDirection)
    {
        if (playerID != SmartFoxInstanceManager.getInstance().getSmartFox().MySelf.Id)
        {
            GameObject player = playersInRoom[playerID];//instead of id check, check if gameobject is server or client.
            player.GetComponent<ServerPlayerLookDirectionScript>().setLookDirection(lookDirection);
        }
    }

    public void RemovePlayer(int playerID)
    {
        GameObject player = playersInRoom[playerID];
        playersInRoom.Remove(playerID);
        Destroy(player);
    }
}
