using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public class PlayerMoveScript : MonoBehaviour
{
    public float movementSpeed = 5.0f;

    private PlayerData playerData;

    private Vector3 screenPoint3D;
    private Vector2 screenPoint;

    private bool isTargetActive;
    private bool lerpToTarget;

    private Vector3 targetPosition3D;

    protected void Start()
    {
        screenPoint3D = new Vector3();
        screenPoint = new Vector2();
    }

    protected void Update()
    {
        if (playerData == null)
        {
            playerData = GetComponent<PlayerScript>().getPlayerData();
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                screenPoint3D = SloverseCoordinateSpaceManager.worldToSloverseSpace(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                screenPoint.x = screenPoint3D.x;
                screenPoint.y = screenPoint3D.y;

                playerData.getTargetPositionData().setTargetPosition(screenPoint, true);
            }
        }

        if (isTargetActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition3D, Time.deltaTime * movementSpeed);

            if ((transform.position - targetPosition3D).magnitude < 0.01f)
            {
                User user = GetComponent<PlayerScript>().getUser();

                if (user.Id == SmartFoxInstanceManager.getInstance().getSmartFox().MySelf.Id)
                {
                    SloverseRequestManager.SendReachedTargetPositionRequest(user);
                }
                
                isTargetActive = false;
            }
        }
    }

    public void SetTargetPosition(Vector2 position, bool hasTarget, bool lerp)
    {
        isTargetActive = hasTarget;
        lerpToTarget = lerp;
        targetPosition3D = new Vector3(position.x, position.y, 0);
    }
}
