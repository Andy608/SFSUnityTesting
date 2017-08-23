using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionLerpScript : MonoBehaviour
{
    private Vector3 serverPosition;
    private Vector3 targetPosition;

    private float dampingFactor;

    private bool hasTarget;
    
	protected void Start ()
    {
        dampingFactor = 1.0f;
    }

    public void updateServerPosition(Vector3 targetPos, Vector3 serverPos, bool interpolate/*, float speed*/, bool hasTarget)
    {
        serverPosition = serverPos;

        this.hasTarget = hasTarget;
        
        if (this.hasTarget)
        {
            targetPosition = targetPos;
        }
        

        if (!interpolate)
        {
            transform.position = serverPosition;
        }
    }

    protected void Update ()
    {
        transform.position = Vector3.MoveTowards(transform.position, serverPosition, Time.deltaTime * 5.0f);
	}
}
