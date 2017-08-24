using UnityEngine;
using System.Collections;
using Sfs2X.Core;
using UnityEngine.SceneManagement;
using Sfs2X.Entities.Data;
using Sfs2X.Entities;

public class EventListenerManager : MonoBehaviour
{
    private SmartFoxInstanceManager smartFoxManager;

    protected void Start()
    {
        smartFoxManager = SmartFoxInstanceManager.getInstance();

        if (!smartFoxManager.isSmartFoxInitialized())
        {
            SceneManager.LoadScene(SloverseSceneUtil.LOGIN_SCENE);
            return;
        }

        smartFoxManager.getSmartFox().AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);
        smartFoxManager.getSmartFox().AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserLeaveRoom);
        smartFoxManager.getSmartFox().AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);

        SloverseRequestManager.SendSpawnRequest();
    }

    protected void Update()
    {
        if (smartFoxManager.isSmartFoxInitialized())
        {
            smartFoxManager.getSmartFox().ProcessEvents();
        }
    }

    private void OnExtensionResponse(BaseEvent e)
    {
        string command = (string)e.Params["cmd"];
        ISFSObject cmdParams = (SFSObject)e.Params["params"];

        if (command == SloverseCommandList.SPAWN_PLAYER_COMMAND)
        {
            SpawnPlayerInRoom(cmdParams);
        }
        else if (command == SloverseCommandList.REMOVE_DUPLICATE_PLAYER)
        {
            RemoveDuplicatePlayer(cmdParams);
        }
        else if (command == SloverseCommandList.UPDATE_PLAYER_ACTIONS)
        {
            UpdatePlayerActions(cmdParams);
        }
    }

    private void SpawnPlayerInRoom(ISFSObject playerData)
    {
        ISFSObject playerDataObj = playerData.GetSFSObject("playerDataObj");
        ISFSObject playerPositionObj = playerDataObj.GetSFSObject("positionObj");
        ISFSObject playerUpdateObj = playerDataObj.GetSFSObject("updateDataObj");

        int playerId = playerDataObj.GetInt("id");
        Vector2 position = new Vector2(playerPositionObj.GetFloat("x"), playerPositionObj.GetFloat("y"));

        User user = smartFoxManager.getSmartFox().UserManager.GetUserById(playerId);
        string name = user.Name;

        if (playerId == smartFoxManager.getSmartFox().MySelf.Id)
        {
            Debug.Log("Creating client user: " + name + " at: (" + position.x + ", " + position.y + ")");
            PlayerConnectionManager.Instance.SpawnPlayer(playerId, position, true);
        }
        else
        {
            Debug.Log("Creating remote user: " + name + " at: (" + position.x + ", " + position.y + ")");
            PlayerConnectionManager.Instance.SpawnPlayer(playerId, position, false);
        }
    }

    private void RemoveDuplicatePlayer(ISFSObject data)
    {
        Debug.Log("Someone logged into this account from another location.");

        int userID = data.GetInt("id");
        User user = smartFoxManager.getSmartFox().UserManager.GetUserById(userID);
        PlayerManagerScript.Instance.RemovePlayer(user.Id);

        SmartFoxInstanceManager.getInstance().disconnect();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }

    private void UpdatePlayerActions(ISFSObject playerActionDataListObj)
    {
        int playersInList = playerActionDataListObj.GetInt("playerAmount");
        ISFSArray playerActionsArray = playerActionDataListObj.GetSFSArray("playerActionsArray");

        for (int i = 0; i < playersInList; ++i)
        {
            ISFSObject actionsArrayIndexObj = (ISFSObject)playerActionsArray.GetElementAt(i);
            ISFSObject actionsObj = actionsArrayIndexObj.GetSFSObject("actionsObj");

            int playerId = actionsArrayIndexObj.GetInt("id");

            User user = smartFoxManager.getSmartFox().UserManager.GetUserById(playerId);
            string name = user.Name;
            Debug.Log("Updating actions for: " + name);

            bool isTargetPositionUpdated = actionsObj.GetBool("isTargetPos");

            if (isTargetPositionUpdated)
            {
                ISFSObject targetPosObj = actionsObj.GetSFSObject("targetPosObj");
                Vector2 targetPosition = new Vector2(targetPosObj.GetFloat("x"), targetPosObj.GetFloat("y"));
                bool hasTarget = targetPosObj.GetBool("hasTarget");
                bool lerpToTarget = targetPosObj.GetBool("lerpToTarget");

                PlayerConnectionManager.Instance.UpdateTargetPosition(playerId, targetPosition, hasTarget, lerpToTarget);
            }

            //Is direction

            //Is state
        }
    }

    private void OnUserLeaveRoom(BaseEvent e)
    {
        User user = (User)e.Params["user"];
        Room room = (Room)e.Params["room"];

        PlayerManagerScript.Instance.RemovePlayer(user.Id);
        Debug.Log(user.Name + " left the room.");
    }

    private void OnConnectionLost(BaseEvent e)
    {
        Debug.Log("CONNECTION LOST");

        SmartFoxInstanceManager.getInstance().disconnect();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }
}
