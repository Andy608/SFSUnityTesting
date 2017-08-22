using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerScript : MonoBehaviour 
{
	public float movementSpeed = 5.0f;
	public float dampTime = 0.5f;
	public bool isMoving;

	private Vector3 targetPosition;
	private Vector3 velocity;

	void Start () 
	{
		isMoving = false;
		targetPosition = this.transform.position;
	}
	
	void Update () 
	{
		if (Input.GetMouseButton(0))
		{
			targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			targetPosition.z = this.transform.position.z;
		}

		if (targetPosition == this.transform.position)
		{
			isMoving = false;
		}
		else
		{
			isMoving = true;
			this.transform.Translate((Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, dampTime, movementSpeed, Time.deltaTime) - this.transform.position));
		}
	}
}
