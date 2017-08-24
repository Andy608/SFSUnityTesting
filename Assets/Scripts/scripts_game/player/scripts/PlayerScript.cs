using UnityEngine;
using System.Collections;
using Sfs2X.Entities;

public class PlayerScript : MonoBehaviour
{
    private User sfsUser;
    private PlayerData playerData;

    protected void Start()
    {
        sfsUser = SmartFoxInstanceManager.getInstance().getSmartFox().MySelf;
    }

    protected void Update()
    {
        if (playerData != null)
        {
            if (playerData.areUpdatesAvailable())
            {
                if (playerData.getTargetPositionData().hasUpdate())
                {
                    SloverseRequestManager.SendTargetPositionChangeRequest(sfsUser, playerData.getTargetPositionData().getTargetPosition());
                }

                if (playerData.getDirectionData().hasUpdate())
                {
                    SloverseRequestManager.SendDirectionChangeRequest(sfsUser, playerData.getDirectionData().getPlayerDirection());
                }

                if (playerData.getStateData().hasUpdate())
                {
                    SloverseRequestManager.SendStateChangeRequest(sfsUser, playerData.getStateData().getPlayerState());
                }
            }
        }
    }

    public void setPlayerData(PlayerData data)
    {
        playerData = data;
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
