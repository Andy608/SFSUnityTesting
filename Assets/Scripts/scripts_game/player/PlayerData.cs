using UnityEngine;
using Sfs2X.Entities.Data;

public class PlayerData
{
    private PlayerTargetPositionData targetPositionData;
    private PlayerDirectionData directionData;
    private PlayerStateData stateData;

    public PlayerData()
    {

    }

    public PlayerData(PlayerTargetPositionData targetPos, PlayerDirectionData dir, PlayerStateData state)
    {
        targetPositionData = targetPos;
        directionData = dir;
        stateData = state;
    }

    public bool areUpdatesAvailable()
    {
        return (targetPositionData.hasNewUpdate() || directionData.hasNewUpdate() || stateData.hasNewUpdate());
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
        if (targetPositionData.hasNewUpdate())
        {
            targetPositionData.resetUpdate();
        }

        if (directionData.hasNewUpdate())
        {
            directionData.resetUpdate();
        }

        if (stateData.hasNewUpdate())
        {
            stateData.resetUpdate();
        }
    }

    public static PlayerData fromSFSObject(ISFSObject playerActionsUpdateBundle)
    {
        PlayerData data = new PlayerData();

        if (playerActionsUpdateBundle.GetBool("isTargetPos"))
        {
            data.targetPositionData = new PlayerTargetPositionData();
            ISFSObject targetPositionObj = playerActionsUpdateBundle.GetSFSObject("targetPosObj");

            Vector3 targetPosition = new Vector3();
            targetPosition.x = targetPositionObj.GetFloat("x");
            targetPosition.y = targetPositionObj.GetFloat("y");
            targetPosition = SloverseCoordinateSpaceManager.sloverseToWorldSpace(targetPosition);
            data.targetPositionData.setTargetPosition(targetPosition, targetPositionObj.GetBool("lerpToTarget"), false);
        }

        if (playerActionsUpdateBundle.GetBool("isPlayerDir"))
        {
            data.directionData = new PlayerDirectionData();
            ISFSObject directionObj = playerActionsUpdateBundle.GetSFSObject("playerDirObj");

            data.directionData.setPlayerDirection((EnumPlayerDirection)directionObj.GetInt("dir"), false);
        }

        if (playerActionsUpdateBundle.GetBool("isPlayerState"))
        {
            data.stateData = new PlayerStateData();
            ISFSObject stateObj = playerActionsUpdateBundle.GetSFSObject("playerStateObj");
            data.stateData.setPlayerState((EnumPlayerState)stateObj.GetInt("state"), false);
        }

        return data;
    }
}