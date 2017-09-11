using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public class ClientPlayerStateScript : PlayerStateScript
{
    private const float DIRECTION_ORIGIN_OFFSET = 1.1f;

    private Vector3 screenPoint;
    private Vector3 lastMousePos;

    private float rotationAngle;
    private bool directionChanged;

    private Vector3 rotationPointOrigin;
    private Vector2 originToMouse;

    override
    protected void OnStart()
    {
        screenPoint = new Vector3();
    }

    override
    protected void OnUpdate()
    {
        lastMousePos = screenPoint;
        screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        screenPoint.z = 0.0f;

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("CLICK: " + ((screenPoint - transform.position).magnitude));
            UpdateClientPlayerDirection();

            if ((screenPoint - transform.position).magnitude < 0.02f)
            {
                Debug.Log("ALREADY THERE");
            }
            else
            {
                playerData.getTargetPositionData().setTargetPosition(screenPoint, true, true);
            }
        }

        PlayerTargetPositionData targetPosData = playerData.getTargetPositionData();
        if (!targetPosData.hasTargetPosition() && lastMousePos != screenPoint)
        {
            UpdateClientPlayerDirection();
        }
        
        if (directionChanged)
        {
            directionChanged = false;
        }
    }

    public void UpdateClientPlayerDirection()
    {
        UpdateClientRotationOrigin();
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rotationPointOrigin;
        originToMouse.x = v.x;
        originToMouse.y = v.y;

        if (originToMouse.magnitude < 0.01f)
        {
            return;
        }

        originToMouse.Normalize();

        rotationAngle = Mathf.Rad2Deg * Mathf.Acos((Vector2.Dot(originToMouse, Vector2.right)) / (originToMouse.magnitude * Vector2.right.magnitude));

        if ((originToMouse.x <= 0 && originToMouse.y <= 0) || (originToMouse.x >= 0 && originToMouse.y <= 0))
        {
            rotationAngle = 360.0f - rotationAngle;
        }

        EnumPlayerDirection updatedDirection = PlayerDirectionUtil.getDirectionFromAngle(rotationAngle);

        if (updatedDirection != playerData.getDirectionData().getPlayerDirection())
        { 
            directionChanged = true;
            playerData.getDirectionData().setPlayerDirection(updatedDirection, directionChanged);
            UpdateDirectionImage(updatedDirection);
        }
    }

    private void UpdateClientRotationOrigin()
    {
        rotationPointOrigin.x = transform.position.x;
        rotationPointOrigin.y = transform.position.y + DIRECTION_ORIGIN_OFFSET;
    }

    //public void SetClientTargetPosition(Vector2 position, bool hasTarget, bool lerp)
    //{
        //isTargetPositionActive = hasTarget;
        //lerpToTargetPosition = lerp;
        //targetPosition = new Vector3(position.x, position.y, 0);
    //}
}
