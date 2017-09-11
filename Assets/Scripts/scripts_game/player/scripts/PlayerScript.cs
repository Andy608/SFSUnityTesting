using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public class PlayerScript : MonoBehaviour
{
    private User sfsUser;
    private PlayerData playerData;

    protected void Awake()
    {
        sfsUser = SmartFoxInstanceManager.getInstance().getSmartFox().MySelf;
    }

    protected void Update()
    {
        if (playerData != null)
        {
            if (playerData.areUpdatesAvailable())
            {
                if (playerData.getTargetPositionData().hasNewUpdate())
                {
                    SloverseRequestManager.SendTargetPositionChangeRequest(sfsUser, playerData.getTargetPositionData().getTargetPosition());
                    playerData.getTargetPositionData().resetUpdate();
                }

                if (playerData.getDirectionData().hasNewUpdate())
                {
                    SloverseRequestManager.SendDirectionChangeRequest(sfsUser, playerData.getDirectionData().getPlayerDirection());
                    playerData.getDirectionData().resetUpdate();
                }

                if (playerData.getStateData().hasNewUpdate())
                {
                    SloverseRequestManager.SendStateChangeRequest(sfsUser, playerData.getStateData().getPlayerState());
                    playerData.getStateData().resetUpdate();
                }

                playerData.resetUpdates();
            }
        }
    }

    public void updatePlayerData(PlayerData newData)
    {
        if (playerData == null)
        {
            playerData = new PlayerData(new PlayerTargetPositionData(), new PlayerDirectionData(), new PlayerStateData());
        }

        if (newData.getTargetPositionData() != null)
        {
            PlayerTargetPositionData targetPosData = newData.getTargetPositionData();
            Debug.Log(targetPosData.getTargetPosition());
            playerData.getTargetPositionData().setTargetPosition(targetPosData.getTargetPosition(), targetPosData.shouldLerp(), false);
        }

        if (newData.getDirectionData() != null)
        {
            playerData.getDirectionData().setPlayerDirection(newData.getDirectionData().getPlayerDirection(), false);
        }

        if (newData.getStateData() != null)
        {
            playerData.getStateData().setPlayerState(newData.getStateData().getPlayerState(), false);
        }
    }

    public PlayerData getPlayerData()
    {
        return playerData;
    }

    public User getUser()
    {
        return sfsUser;
    }
}
