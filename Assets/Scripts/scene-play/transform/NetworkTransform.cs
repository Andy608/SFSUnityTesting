using System;
using System.Collections;
using UnityEngine;
using Sfs2X.Entities.Data;

public class NetworkTransform
{
    private NetworkTransform() { }

    private Vector3 position;
    private Vector3 rotation;

    private double timeStamp;

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public Vector3 Rotation
    {
        get
        {
            return rotation;
        }
    }

    public double TimeStamp
    {
        get
        {
            return timeStamp;
        }

        set
        {
            timeStamp = value;
        }
    }

    public void ToSFSObject(ISFSObject data)
    {
        ISFSObject transformObj = new SFSObject();

        transformObj.PutFloat("x", this.position.x);
        transformObj.PutFloat("y", this.position.y);
        transformObj.PutFloat("z", this.position.z);

        transformObj.PutFloat("rx", this.rotation.x);
        transformObj.PutFloat("ry", this.rotation.y);
        transformObj.PutFloat("rz", this.rotation.z);

        transformObj.PutLong("t", Convert.ToInt64(this.timeStamp));

        data.PutSFSObject("transform", transformObj);
    }

    public void Override(NetworkTransform other)
    {
        this.position = other.position;
        this.rotation = other.rotation;
        this.timeStamp = other.timeStamp;
    }

    public static NetworkTransform Clone(NetworkTransform other)
    {
        NetworkTransform another = new NetworkTransform();
        another.Override(other);
        return another;
    }

    public void Update(Transform transform)
    {
        transform.position = this.position;
        transform.localEulerAngles = this.rotation;
    }

    public static NetworkTransform FromSFSObject(ISFSObject data)
    {
        NetworkTransform transform = new NetworkTransform();
        ISFSObject transformData = data.GetSFSObject("transform");

        float x = transformData.GetFloat("x");
        float y = transformData.GetFloat("y");
        float z = transformData.GetFloat("z");

        transform.position = new Vector3(x, y, z);

        float rx = transformData.GetFloat("rx");
        float ry = transformData.GetFloat("ry");
        float rz = transformData.GetFloat("rz");

        transform.rotation = new Vector3(rx, ry, rz);

        if (transformData.ContainsKey("t"))
        {
            transform.TimeStamp = Convert.ToDouble(transformData.GetLong("t"));
        }
        else
        {
            transform.TimeStamp = 0;
        }

        return transform;
    }

    public static NetworkTransform FromTransform(Transform transform)
    {
        NetworkTransform trans = new NetworkTransform();
        trans.position = transform.position;
        trans.rotation = transform.localEulerAngles;
        return trans;
    }
}