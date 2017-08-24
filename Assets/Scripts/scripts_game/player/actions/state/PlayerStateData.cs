using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;

public class PlayerStateData :ActionData
{
    private EnumPlayerState playerState;

    public PlayerStateData() : this(EnumPlayerState.STANDING)
    {

    }

    public PlayerStateData(EnumPlayerState state)
    {
        playerState = state;
    }

    public EnumPlayerState getPlayerState()
    {
        return playerState;
    }

    public void setPlayerState(EnumPlayerState state)
    {
        playerState = state;
        setUpdated(true);
    }

    public ISFSObject toSFSObject()
    {
        ISFSObject stateObj = new SFSObject();
        stateObj.PutInt("state", (int)playerState);
        return stateObj;
    }

    public static PlayerStateData fromSFSObject(ISFSObject dirObj)
    {
        PlayerStateData data = new PlayerStateData();

        int state = dirObj.GetInt("state");

        if (EnumPlayerStateClass.isValid(state))
        {
            data.playerState = (EnumPlayerState)state;
        }
        else
        {
            return null;
        }

        return data;
    }
}
