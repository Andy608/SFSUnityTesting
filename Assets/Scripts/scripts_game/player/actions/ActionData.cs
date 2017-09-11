using UnityEngine;
using System.Collections;

public class ActionData
{
    private bool updated;

    public ActionData()
    {
        updated = false;
    }

    public bool hasNewUpdate()
    {
        return updated;
    }

    public void resetUpdate()
    {
        updated = false;
    }

    protected void setUpdated(bool value)
    {
        updated = value;
    }
}
