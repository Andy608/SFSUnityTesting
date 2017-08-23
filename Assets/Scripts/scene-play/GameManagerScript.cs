using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;

public class GameManagerScript : MonoBehaviour
{
    private SmartFoxInstanceManager smartFoxManager;

	protected void Start ()
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

        if (Input.GetMouseButtonDown(0))
        {
            SloverseRequestManager.SendUpdatedTargetPositionRequest(smartFoxManager.getSmartFox().MySelf, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void OnExtensionResponse(BaseEvent e)
    {
        string command = (string)e.Params["cmd"];
        ISFSObject cmdParams = (SFSObject)e.Params["params"];

        if (command == SloverseCommandList.SPAWN_PLAYER_COMMAND)
        {
            CreateNewPlayerInRoom(cmdParams);
        }
        else if (command == SloverseCommandList.UPDATE_PLAYER_POSITION)
        {
            UpdatePlayerPosition(cmdParams);
        }
        else if (command == SloverseCommandList.UPDATE_PLAYERS_POSITION_BUNDLE)
        {
            UpdatePlayersPosition(cmdParams);
        }
        else if (command == SloverseCommandList.REMOVE_DUPLICATE_PLAYER)
        {
            RemoveDuplicatePlayer(cmdParams);
        }
        else if (command == SloverseCommandList.UPDATE_PLAYER_DIRECTION)
        {
            UpdatePlayerDirection(cmdParams);
        }
    }

    private void CreateNewPlayerInRoom(ISFSObject data)
    {
        ISFSObject playerData = data.GetSFSObject("player");
        ISFSObject playerPositionData = playerData.GetSFSObject("playerPosition");
        int userID = playerData.GetInt("id");
        float posX = playerPositionData.GetFloat("x");
        float posY = playerPositionData.GetFloat("y");

        User user = smartFoxManager.getSmartFox().UserManager.GetUserById(userID);
        string name = user.Name;
        
        if (userID == smartFoxManager.getSmartFox().MySelf.Id)
        {
            Debug.Log("CREATING CLIENT USER " + name + ": " + posX.ToString() + ", " + posY.ToString());
            PlayerManagerScript.Instance.SpawnClientPlayer(userID, posX, posY);
        }
        else
        {
            Debug.Log("CREATING SERVER USER " + name + ": " + posX.ToString() + ", " + posY.ToString());

            bool hasTarget = playerData.GetBool("hasTarget");

            Vector3 targetPosition = new Vector3();

            if (hasTarget)
            {
                float targetPosX = playerData.GetFloat("targetX");
                float targetPosY = playerData.GetFloat("targetY");

                targetPosition.x = targetPosX;
                targetPosition.y = targetPosY;
            }

            PlayerManagerScript.Instance.SpawnServerPlayer(userID, posX, posY);
            PlayerManagerScript.Instance.UpdatePlayerPosition(userID, SloverseCoordinateSpaceManager.sloverseToWorldSpace(targetPosition), SloverseCoordinateSpaceManager.sloverseToWorldSpace(new Vector3(posX, posY, 0)), false, hasTarget);
        }
    }

    private void UpdatePlayerPosition(ISFSObject data)
    {
        ISFSObject playerData = data.GetSFSObject("player");
        ISFSObject playerPositionData = playerData.GetSFSObject("playerPosition");
        int userID = playerData.GetInt("id");
        float posX = playerPositionData.GetFloat("x");
        float posY = playerPositionData.GetFloat("y");
        bool lerp = playerData.GetBool("lerp");

        User user = smartFoxManager.getSmartFox().UserManager.GetUserById(userID);
        string name = user.Name;

        Debug.Log("UPDATING USER " + name + ": " + posX.ToString() + ", " + posY.ToString() + " LERP: " + lerp);

        bool hasTarget = playerData.GetBool("hasTarget");

        Vector3 targetPosition = new Vector3();

        if (hasTarget)
        {
            float targetPosX = playerData.GetFloat("targetX");
            float targetPosY = playerData.GetFloat("targetY");

            targetPosition.x = targetPosX;
            targetPosition.y = targetPosY;
        }

        PlayerManagerScript.Instance.UpdatePlayerPosition(userID, SloverseCoordinateSpaceManager.sloverseToWorldSpace(targetPosition), SloverseCoordinateSpaceManager.sloverseToWorldSpace(new Vector3(posX, posY, 0)), lerp, hasTarget);
    }

    private void UpdatePlayersPosition(ISFSObject playerPositionsBundle)
    {
        int playerAmount = playerPositionsBundle.GetInt("playerAmount");
        ISFSArray playerArray = playerPositionsBundle.GetSFSArray("playerDataArray");

        for (int i = 0; i < playerAmount; ++i)
        {
            ISFSObject playerDataWrapper = (ISFSObject)playerArray.GetElementAt(i);
            ISFSObject playerData = playerDataWrapper.GetSFSObject("player");
            ISFSObject playerPositionData = playerData.GetSFSObject("playerPosition");
            int userID = playerData.GetInt("id");
            float posX = playerPositionData.GetFloat("x");
            float posY = playerPositionData.GetFloat("y");
            bool lerp = playerData.GetBool("lerp");

            User user = smartFoxManager.getSmartFox().UserManager.GetUserById(userID);
            string name = user.Name;
            
            Debug.Log("UPDATING USER FROM BUNDLE " + name + ": " + posX.ToString() + ", " + posY.ToString() + " LERP: " + lerp);

            bool hasTarget = playerData.GetBool("hasTarget");

            Vector3 targetPosition = new Vector3();

            if (hasTarget)
            {
                float targetPosX = playerData.GetFloat("targetX");
                float targetPosY = playerData.GetFloat("targetY");

                targetPosition.x = targetPosX;
                targetPosition.y = targetPosY;
            }

            PlayerManagerScript.Instance.UpdatePlayerPosition(userID, SloverseCoordinateSpaceManager.sloverseToWorldSpace(targetPosition), SloverseCoordinateSpaceManager.sloverseToWorldSpace(new Vector3(posX, posY, 0)), lerp, hasTarget);
        }
    }

    private void UpdatePlayerDirection(ISFSObject directionData)
    {
        int userID = directionData.GetInt("id");
        EnumLookDirection direction = (EnumLookDirection)directionData.GetInt("dir");

        PlayerManagerScript.Instance.UpdatePlayerDirection(userID, direction);
    }

    private void RemoveDuplicatePlayer(ISFSObject data)
    {
        Debug.Log("SOMEONE LOGGED IN TO THIS ACCOUNT SOMEWHERE ELSE.");

        int userID = data.GetInt("id");
        User user = smartFoxManager.getSmartFox().UserManager.GetUserById(userID);
        PlayerManagerScript.Instance.RemovePlayer(user.Id);

        SmartFoxInstanceManager.getInstance().disconnect();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }

    private void OnUserLeaveRoom(BaseEvent e)
    {
        User user = (User)e.Params["user"];
        Room room = (Room)e.Params["room"];

        PlayerManagerScript.Instance.RemovePlayer(user.Id);
        Debug.Log("Player: " + user.Name + " left.");
    }

    private void OnConnectionLost(BaseEvent e)
    {
        Debug.Log("CONNECTION IS LOST!");

        SmartFoxInstanceManager.getInstance().disconnect();
        SceneManager.LoadScene(SloverseSceneUtil.START_SCENE);
    }
}
