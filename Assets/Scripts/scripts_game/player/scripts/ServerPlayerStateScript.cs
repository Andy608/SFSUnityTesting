using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public class ServerPlayerStateScript : PlayerStateScript
{
    //private bool isTargetPositionActive;
    //private bool lerpToTargetPosition;

    //private Vector3 targetPosition3D;

    override
    protected void OnStart()
    {

    }

    override
    protected void OnUpdate()
    {
        
    }

    //public void SetServerTargetPosition(Vector2 position, bool hasTarget, bool lerp)
    //{
        //isTargetPositionActive = hasTarget;
        //lerpToTargetPosition = lerp;
        //targetPosition3D = new Vector3(position.x, position.y, 0);
    //}

    public void SetServerDirectionImage(EnumPlayerDirection direction)
    {
        UpdateDirectionImage(direction);
    }
}
