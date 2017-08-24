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

    public void setPlayerDirection(EnumPlayerDirection direction)
    {
        playerDirection = direction;
        setUpdated(true);
    }

    public ISFSObject toSFSObject()
    {
        ISFSObject directionObj = new SFSObject();
        directionObj.PutInt("dir", (int)playerDirection);
        return directionObj;
    }

    public static PlayerDirectionData fromSFSObject(ISFSObject dirObj)
    {
        PlayerDirectionData data = new PlayerDirectionData();

        int direction = dirObj.GetInt("dir");

        if (EnumPlayerDirectionClass.isValid(direction))
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
