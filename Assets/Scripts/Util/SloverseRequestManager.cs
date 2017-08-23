using System.Collections;
using System.Collections.Generic;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using Sfs2X.Entities;

public class SloverseRequestManager
{
    public const string SPAWN_PLAYER_IN_ROOM = "SpawnPlayerInRoom";
    public const string UPDATE_TARGET_POSITION = "UpdateTargetPosition";
    public const string UPDATE_PLAYER_DIRECTION = "UpdatePlayerDirection";

    public static void SendSpawnRequest()
    {
        ExtensionRequest spawnRequest = new ExtensionRequest(SPAWN_PLAYER_IN_ROOM, new SFSObject());
        SmartFoxInstanceManager.getInstance().getSmartFox().Send(spawnRequest);
    }

    public static void SendUpdatedTargetPositionRequest(User player, Vector3 targetPosition)
    {
        targetPosition = SloverseCoordinateSpaceManager.worldToSloverseSpace(targetPosition);

        ISFSObject targetPosData = new SFSObject();
        targetPosData.PutInt("id", player.Id);
        targetPosData.PutFloat("x", targetPosition.x);
        targetPosData.PutFloat("y", targetPosition.y);

        ExtensionRequest moveRequest = new ExtensionRequest(UPDATE_TARGET_POSITION, targetPosData);
        SmartFoxInstanceManager.getInstance().getSmartFox().Send(moveRequest);
    }

    public static void SendDirectionChangeRequest(User player, EnumLookDirection lookDirection)
    {
        ISFSObject playerDirectionData = new SFSObject();
        playerDirectionData.PutInt("id", player.Id);
        playerDirectionData.PutInt("dir", (int)lookDirection);

        ExtensionRequest directionRequest = new ExtensionRequest(UPDATE_PLAYER_DIRECTION, playerDirectionData);
        SmartFoxInstanceManager.getInstance().getSmartFox().Send(directionRequest);
    }
}
