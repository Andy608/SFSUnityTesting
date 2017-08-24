using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayerLookDirectionScript : MonoBehaviour
{
    private EnumLookDirection lastLookDirection;
    private EnumLookDirection lookDirection;

    private float lookAngle;

    private bool directionChanged;

    private Sprite[] playerDirections;

    private Vector3 rotationPointOrigin;
    private Vector2 playerToMouse;

	protected void Start ()
    {
        initDirectionImages();

        rotationPointOrigin = new Vector3();
        playerToMouse = new Vector2();

        lastLookDirection = lookDirection = EnumLookDirection.SOUTH;
        directionChanged = false;
    }

    protected void Update ()
    {
        updatePlayerDirection();

        if (directionChanged)
        {
            updateClientDirectionImage();
            updateServerDirectionImage();
            directionChanged = false;
        }
    }

    private void initDirectionImages(/*Path of sloth image goes here*/)
    {
        playerDirections = Resources.LoadAll<Sprite>("Sprites/Sloth/sloth");
    }

    public void updatePlayerDirection()
    {
        updateRotationOrigin();
        Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition) - rotationPointOrigin;
        playerToMouse.x = v.x;
        playerToMouse.y = v.y;

        if (playerToMouse.magnitude == 0)
        {
            return;
        }

        playerToMouse.Normalize();

        lookAngle = Mathf.Rad2Deg * Mathf.Acos((Vector2.Dot(playerToMouse, Vector2.right)) / (playerToMouse.magnitude * Vector2.right.magnitude));

        if ((playerToMouse.x <= 0 && playerToMouse.y <= 0) || (playerToMouse.x >= 0 && playerToMouse.y <= 0))
        {
            lookAngle = 360.0f - lookAngle;
        }
        
        //Debug.Log(lookAngle);

        if (lookAngle > 60.0f && lookAngle < 120.0f)
        {
            lookDirection = EnumLookDirection.NORTH;
        }
        else if (lookAngle >= 10.0f && lookAngle <= 60.0f)
        {
            lookDirection = EnumLookDirection.NORTH_EAST;
        }
        else if (lookAngle > 330.0f || lookAngle < 10.0f)
        {
            lookDirection = EnumLookDirection.EAST;
        }
        else if (lookAngle >= 290.0f && lookAngle <= 330.0f)
        {
            lookDirection = EnumLookDirection.SOUTH_EAST;
        }
        else if (lookAngle > 250.0f && lookAngle < 290.0f)
        {
            lookDirection = EnumLookDirection.SOUTH;
        }
        else if (lookAngle >= 210.0f && lookAngle <= 250.0f)
        {
            lookDirection = EnumLookDirection.SOUTH_WEST;
        }
        else if (lookAngle > 170.0f && lookAngle < 210.0f)
        {
            lookDirection = EnumLookDirection.WEST;
        }
        else
        {
            lookDirection = EnumLookDirection.NORTH_WEST;
        }

        if (lastLookDirection != lookDirection)
        {
            directionChanged = true;
            Debug.Log("DIRECTION CHANGED!");
            lastLookDirection = lookDirection;
        }
    }

    private void updateClientDirectionImage()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerDirections[(int)lookDirection];
    }

    private void updateServerDirectionImage()
    {
        SloverseRequestManager.SendDirectionChangeRequest(SmartFoxInstanceManager.getInstance().getSmartFox().MySelf, lookDirection);
    }

    private void updateRotationOrigin()
    {
        rotationPointOrigin.x = transform.position.x;
        rotationPointOrigin.y = transform.position.y + 1.1f;
    }
}
