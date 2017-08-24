using UnityEngine;
using System.Collections;
using System;

public enum EnumPlayerDirection
{
    NORTH,
    NORTH_EAST,
    EAST,
    SOUTH_EAST,
    SOUTH,
    SOUTH_WEST,
    WEST,
    NORTH_WEST
}

public class EnumPlayerDirectionClass
{
    public static bool isValid(int index)
    {
        return !(index < 0 || index >= Enum.GetValues(typeof(EnumPlayerDirection)).Length);
    }
}
