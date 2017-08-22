using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteInterpolationScript : MonoBehaviour 
{
	private Vector3 desiredPosition;
	private float dampingFactor = 5.0f;

	void Start () 
	{
		desiredPosition = this.transform.position;
	}
	
	void Update () 
	{
		this.transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * dampingFactor);
	}

	public void SetTransform(Vector3 position, bool interpolate)
	{
		if (interpolate)
		{
			desiredPosition = position;
		}
		else
		{
			this.transform.position = position;
		}
	}
}
