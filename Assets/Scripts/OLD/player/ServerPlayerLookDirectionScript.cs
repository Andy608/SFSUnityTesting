using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerPlayerLookDirectionScript : MonoBehaviour
{
    private EnumLookDirection lookDirection;

    private Sprite[] playerDirections;

    protected void Start()
    {
        initDirectionImages();
    }

    private void initDirectionImages(/*Path of sloth image goes here*/)
    {
        playerDirections = Resources.LoadAll<Sprite>("Sprites/Sloth/sloth");
    }

    public void setLookDirection(EnumLookDirection lookDir)
    {
        lookDirection = lookDir;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = playerDirections[(int)lookDirection];
    }

    public EnumLookDirection getLookDirection()
    {
        return lookDirection;
    }
}