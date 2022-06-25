using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraView : MonoBehaviour {
	
	private Camera battleCam;
	
	// Use this for initialization
	void Start () {
		battleCam = GameObject.Find("Battle_Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.rotation = battleCam.transform.rotation;
	}
}
