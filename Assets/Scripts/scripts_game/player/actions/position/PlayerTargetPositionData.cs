using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;

public class PlayerTargetPositionData : ActionData
{
    private bool hasTarget;
    private bool lerpToTarget;

    private Vector3 targetPosition;

    public PlayerTargetPositionData()
    {
        hasTarget = false;
        lerpToTarget = false;
        targetPosition = SloverseCoordinateSpaceManager.sloverseToWorldSpace(new Vector3(0.5f, -1.0f, 0.0f));
    }

    public PlayerTargetPositionData(float x, float y, bool lerp) : this(new Vector2(x, y), lerp)
    {

    }

    public PlayerTargetPositionData(Vector2 targetPos, bool lerp)
    {
        targetPosition = new Vector2(targetPos.x, targetPos.y);
        hasTarget = true;
        lerpToTarget = lerp;
    }

    public bool hasTargetPosition()
    {
        return hasTarget;
    }

    public void setHasTargetPosition(bool b)
    {
        hasTarget = b;
    }

    public Vector3 getTargetPosition()
    {
        return targetPosition;
    }

    public void setTargetPosition(Vector3 position, bool lerp, bool sendToServer)
    {
        targetPosition.Set(position.x, position.y, 0.0f);
        hasTarget = true;
        setLerpToTarget(lerp);

        setUpdated(sendToServer);

        //Debug.Log("UPDATED TARGET POSITION");
    }

    public bool shouldLerp()
    {
        return lerpToTarget;
    }

    private void setLerpToTarget(bool lerp)
    {
        lerpToTarget = lerp;
    }

    public static PlayerTargetPositionData fromSFSObject(ISFSObject positionData)
    {
        PlayerTargetPositionData data = new PlayerTargetPositionData();
        data.targetPosition.x = positionData.GetFloat("x");
        data.targetPosition.y = positionData.GetFloat("y");
        data.hasTarget = positionData.GetBool("hasTarget");
        data.lerpToTarget = positionData.GetBool("lerpToTarget");
        return data;
    }

    public ISFSObject toSFSObject()
    {
        ISFSObject targetObj = new SFSObject();
        targetObj.PutFloat("x", targetPosition.x);
        targetObj.PutFloat("y", targetPosition.y);
        targetObj.PutBool("hasTarget", hasTarget);
        targetObj.PutBool("lerpToTarget", lerpToTarget);
        return targetObj;
    }
}
