using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCameraHandler : MonoBehaviour
{
	private bool isFree = false;
	
	// Local position
	private Vector3 defaultCameraPosition = new Vector3(0, 12.07f, -9.79f);
	private Vector3 startCameraPosition;
	private Quaternion startCameraRotation;
	
	void Start()
	{
		startCameraPosition = transform.localPosition;
		startCameraRotation = transform.localRotation;
	}

	public void setCamera(Transform view)
	{
		freeCamera();
		transform.position = view.position;
		transform.rotation = view.rotation;
	}

	public void resetCamera()
	{
		resetPosition();
	}

	public void freeCamera()
	{
		isFree = true;
		transform.parent = null;
	}
	
	public void unfreeCamera()
	{
		isFree = false;
		transform.parent = PlayerMovement.player.transform;
		resetPosition();
	}
	
	void resetPosition()
	{
		transform.localPosition = startCameraPosition;
		transform.localRotation = startCameraRotation;
	}
}
