using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public abstract class PlayerStateScript : MonoBehaviour
{
    protected Sprite[] playerDirectionSprites;
    private int DIRECTIONS = 8;
    private const string STATE_PATH = "Sprites/Sloth/sloth_state";
    private const string STAND_STATE = "stand";

    protected PlayerData playerData;
    protected float movementSpeed = 5.0f;

    protected void Start()
    {
        initDirectionImages();
        OnStart();
    }

    protected void Update()
    {
        playerData = GetComponent<PlayerScript>().getPlayerData();

        if (playerData == null)
        {
            return;
        }
        else
        {
            PlayerTargetPositionData targetPosData = playerData.getTargetPositionData();
            if (targetPosData.hasTargetPosition())
            {
                if (targetPosData.shouldLerp())
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosData.getTargetPosition(), Time.deltaTime * movementSpeed);
                }
                else
                {
                    transform.position = targetPosData.getTargetPosition();
                }

                if ((transform.position - targetPosData.getTargetPosition()).magnitude < 0.01f)
                {
                    User user = GetComponent<PlayerScript>().getUser();
                    if (user != null && user.Id == SmartFoxInstanceManager.getInstance().getSmartFox().MySelf.Id)
                    {
                        SloverseRequestManager.SendReachedTargetPositionRequest(user);
                        targetPosData.setHasTargetPosition(false);
                    }
                }
            }

            OnUpdate();
        }
    }

    private void initDirectionImages()
    {
        Sprite[] playerStates = Resources.LoadAll<Sprite>(STATE_PATH);
        playerDirectionSprites = new Sprite[DIRECTIONS];

        int counter = 0;
        foreach (Sprite s in playerStates)
        {
            if (s.name.Contains(STAND_STATE))
            {
                playerDirectionSprites[counter] = s;
                ++counter;
            }
        }
    }

    protected void UpdateDirectionImage(EnumPlayerDirection direction)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerDirectionSprites[(int)direction];
    }

    protected abstract void OnStart();

    protected abstract void OnUpdate();
}
