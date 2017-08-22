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

    private SmartFoxInstanceManager smartFoxManager;

    public SloverseRequestManager(SmartFoxInstanceManager sfsManager)
    {
        smartFoxManager = sfsManager;
    }

    public void SendSpawnRequest()
    {
        ExtensionRequest spawnRequest = new ExtensionRequest(SPAWN_PLAYER_IN_ROOM, new SFSObject());
        smartFoxManager.getSmartFox().Send(spawnRequest);
    }

    public void SendUpdatedTargetPosition(User player, Vector3 targetPosition)
    {
        ISFSObject targetPosData = new SFSObject();
        targetPosData.PutInt("id", player.Id);
        targetPosData.PutFloat("x", targetPosition.x);
        targetPosData.PutFloat("y", targetPosition.y);

        ExtensionRequest moveRequest = new ExtensionRequest(UPDATE_TARGET_POSITION, targetPosData);
        smartFoxManager.getSmartFox().Send(moveRequest);
    }
}
