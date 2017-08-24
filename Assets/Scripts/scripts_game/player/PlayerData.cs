using UnityEngine;
using UnityEditor;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;

public class PlayerData
{
    private PlayerTargetPositionData targetPositionData;
    private PlayerDirectionData directionData;
    private PlayerStateData stateData;

    public PlayerData()
    {
        targetPositionData = new PlayerTargetPositionData();
        directionData = new PlayerDirectionData();
        stateData = new PlayerStateData();
    }

    public bool areUpdatesAvailable()
    {
        return (targetPositionData.hasUpdate() || directionData.hasUpdate() || stateData.hasUpdate());
    }

    public PlayerTargetPositionData getTargetPositionData()
    {
        return targetPositionData;
    }

    public PlayerDirectionData getDirectionData()
    {
        return directionData;
    }

    public PlayerStateData getStateData()
    {
        return stateData;
    }

    public void resetUpdates()
    {
        if (targetPositionData.hasUpdate())
        {
            targetPositionData.resetUpdate();
        }

        if (directionData.hasUpdate())
        {
            directionData.resetUpdate();
        }

        if (stateData.hasUpdate())
        {
            stateData.resetUpdate();
        }
    }

    public static PlayerData fromSFSObject(ISFSObject playerActionsUpdateBundle)
    {
        PlayerData data = new PlayerData();
        
        if (playerActionsUpdateBundle.GetBool("isTargetPos"))
        {
            ISFSObject targetPositionObj = playerActionsUpdateBundle.GetSFSObject("targetPosObj");

            Vector2 targetPosition = new Vector2();
            targetPosition.x = targetPositionObj.GetFloat("x");
            targetPosition.y = targetPositionObj.GetFloat("y");
            data.targetPositionData.setTargetPosition(targetPosition, targetPositionObj.GetBool("lerpToTarget"));
        }

        if (playerActionsUpdateBundle.GetBool("isPlayerDir"))
        {
            ISFSObject directionObj = playerActionsUpdateBundle.GetSFSObject("playerDirObj");
            data.directionData.setPlayerDirection((EnumPlayerDirection)directionObj.GetInt("dir"));
        }

        if (playerActionsUpdateBundle.GetBool("isPlayerState"))
        {
            ISFSObject stateObj = playerActionsUpdateBundle.GetSFSObject("playerStateObj");
            data.stateData.setPlayerState((EnumPlayerState)stateObj.GetInt("state"));
        }

        return data;
    }
}