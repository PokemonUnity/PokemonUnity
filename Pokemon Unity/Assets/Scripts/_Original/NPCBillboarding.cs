using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBillboarding : MonoBehaviour
{

	public Camera camera;

	private float defaultRotationX;
	private float defaultRotationY;
	private float defaultRotationZ;
	
	public bool turnX = true;
	public bool turnY = true;
	public bool turnZ = true;

	// Use this for initialization
	void Start ()
	{
		defaultRotationX = transform.rotation.x;
		defaultRotationY = transform.rotation.y;
		defaultRotationZ = transform.rotation.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt(camera.transform);
		transform.rotation = Quaternion.Euler(
			(turnX ? transform.localRotation.eulerAngles.x : defaultRotationX),
			(turnY ? transform.localRotation.eulerAngles.y : defaultRotationY),
			(turnZ ? transform.localRotation.eulerAngles.z : defaultRotationZ)
			);
		if (PlayerMovement.player.transform.Find("Camera") == null)
			transform.localRotation = Quaternion.Euler(camera.transform.rotation.x-50, 180, 0);
		
		transform.Rotate( new Vector3(0, 180, 0), Space.Self );
	}
}
