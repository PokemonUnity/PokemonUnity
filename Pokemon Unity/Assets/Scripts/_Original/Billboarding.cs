using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{

	public Camera camera;
	private Camera battleCam;

	private float defaultRotationX;
	private float defaultRotationY;
	private float defaultRotationZ;
	
	public bool turnX = true;
	public bool turnY = true;
	public bool turnZ = true;

	// Use this for initialization
	void Start ()
	{
		battleCam = GameObject.Find("Battle_Camera").GetComponent<Camera>();

		if (camera == null)
		{
			camera = battleCam;
		}

		defaultRotationX = transform.rotation.x;
		defaultRotationY = transform.rotation.y;
		defaultRotationZ = transform.rotation.z;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.LookAt(camera.transform);
		transform.rotation = Quaternion.Euler(
			(turnX ? transform.rotation.eulerAngles.x : defaultRotationX),
			(turnY ? transform.rotation.eulerAngles.y : defaultRotationY),
			(turnZ ? transform.rotation.eulerAngles.z : defaultRotationZ)
			);
	}
}
