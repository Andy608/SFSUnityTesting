using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverButtonScript : MonoBehaviour
{
    private Color backButtonDefaultColor;
    public Color backButtonHoveredColor;

    protected void Start ()
    {
        backButtonDefaultColor = gameObject.GetComponentInChildren<Text>().color;
    }

    public void onBackButtonEntered()
    {
        gameObject.GetComponentInChildren<Text>().color = backButtonHoveredColor;
    }

    public void onBackButtonExited()
    {
        gameObject.GetComponentInChildren<Text>().color = backButtonDefaultColor;
    }
}
