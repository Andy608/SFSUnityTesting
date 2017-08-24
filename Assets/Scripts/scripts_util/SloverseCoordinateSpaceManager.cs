using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SloverseCoordinateSpaceManager
{
    public static Vector3 worldToSloverseSpace(Vector3 vec)
    {
        vec.z += 10;
        return Camera.main.WorldToViewportPoint(vec);
    }

    public static Vector3 sloverseToWorldSpace(Vector3 vec)
    {
        vec.z += 10;
        return Camera.main.ViewportToWorldPoint(vec);
    }
}
