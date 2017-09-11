using UnityEngine;
using System.Collections;
using System;

public enum EnumPlayerDirection
{
    SOUTH,
    SOUTH_WEST,
    WEST,
    NORTH_WEST,
    NORTH,
    NORTH_EAST,
    EAST,
    SOUTH_EAST
}

public class PlayerDirectionUtil
{
    private const float NORTH_EAST_AND_EAST_ANGLE = 10.0f;
    private const float NORTH_AND_NORTH_EAST_ANGLE = 60.0f;
    private const float NORTH_WEST_AND_NORTH_ANGLE = 120.0f;
    private const float WEST_AND_NORTH_WEST_ANGLE = 170.0f;
    private const float SOUTH_WEST_AND_WEST_ANGLE = 210.0f;
    private const float SOUTH_AND_SOUTH_WEST_ANGLE = 250.0f;
    private const float SOUTH_EAST_AND_SOUTH_ANGLE = 290.0f;
    private const float EAST_AND_SOUTH_EAST_ANGLE = 330.0f;

    public static bool isValid(int index)
    {
        return !(index < 0 || index >= Enum.GetValues(typeof(EnumPlayerDirection)).Length);
    }

    public static EnumPlayerDirection getDirectionFromAngle(float rotationAngle)
    {
        if (rotationAngle > NORTH_AND_NORTH_EAST_ANGLE && rotationAngle < NORTH_WEST_AND_NORTH_ANGLE)
        {
            return EnumPlayerDirection.NORTH;
        }
        else if (rotationAngle >= NORTH_EAST_AND_EAST_ANGLE && rotationAngle <= NORTH_AND_NORTH_EAST_ANGLE)
        {
            return EnumPlayerDirection.NORTH_EAST;
        }
        else if (rotationAngle > EAST_AND_SOUTH_EAST_ANGLE || rotationAngle < NORTH_EAST_AND_EAST_ANGLE)
        {
            return EnumPlayerDirection.EAST;
        }
        else if (rotationAngle >= SOUTH_EAST_AND_SOUTH_ANGLE && rotationAngle <= EAST_AND_SOUTH_EAST_ANGLE)
        {
            return EnumPlayerDirection.SOUTH_EAST;
        }
        else if (rotationAngle > SOUTH_AND_SOUTH_WEST_ANGLE && rotationAngle < SOUTH_EAST_AND_SOUTH_ANGLE)
        {
            return EnumPlayerDirection.SOUTH;
        }
        else if (rotationAngle >= SOUTH_WEST_AND_WEST_ANGLE && rotationAngle <= SOUTH_AND_SOUTH_WEST_ANGLE)
        {
            return EnumPlayerDirection.SOUTH_WEST;
        }
        else if (rotationAngle > WEST_AND_NORTH_WEST_ANGLE && rotationAngle < SOUTH_WEST_AND_WEST_ANGLE)
        {
            return EnumPlayerDirection.WEST;
        }
        else
        {
            return EnumPlayerDirection.NORTH_WEST;
        }
    }
}
