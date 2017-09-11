using UnityEngine;
using System.Collections;
using Sfs2X.Entities.Data;
using System;

public class PlayerDirectionData : ActionData
{
    private EnumPlayerDirection playerDirection;

    public PlayerDirectionData() : this(EnumPlayerDirection.SOUTH)
    {

    }

    public PlayerDirectionData(EnumPlayerDirection direction)
    {
        playerDirection = direction;
    }

    public EnumPlayerDirection getPlayerDirection()
    {
        return playerDirection;
    }

    public void setPlayerDirection(EnumPlayerDirection direction, bool send)
    {
        playerDirection = direction;
        setUpdated(send);
    }

    //Mostly for client players
    public ISFSObject toSFSObject()
    {
        ISFSObject directionObj = new SFSObject();
        directionObj.PutInt("dir", (int)playerDirection);
        return directionObj;
    }

    //Mostly for server players
    public static PlayerDirectionData fromSFSObject(ISFSObject dirObj)
    {
        PlayerDirectionData data = new PlayerDirectionData();

        int direction = dirObj.GetInt("dir");

        if (PlayerDirectionUtil.isValid(direction))
        {
            data.playerDirection = (EnumPlayerDirection)direction;
        }
        else
        {
            return null;
        }

        return data;
    }
}
