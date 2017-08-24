using UnityEngine;
using System.Collections;
using System;

public enum EnumPlayerState
{
    STANDING,
    WALKING,
    SITTING,
    DANCING
}

public class EnumPlayerStateClass
{
    public static bool isValid(int index)
    {
        return !(index < 0 || index >= Enum.GetValues(typeof(EnumPlayerState)).Length);
    }
}
