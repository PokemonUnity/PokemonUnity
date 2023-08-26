using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraHandler : MonoBehaviour
{
	/*private CPC_CameraPath currentPath;
	private CPC_CameraPath intro_path;
	private CPC_CameraPath waiting_path;
	private CPC_CameraPath[] waiting_paths;

	// Use this for initialization
	void Start ()
	{
		currentPath = null;
		intro_path = null;
		waiting_path = GameObject.Find("Global/BattleScene/CameraPaths/Waiting/1").GetComponent<CPC_CameraPath>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator StartTravellingIE(CPC_CameraPath path, float time, int waypoint)
	{
		Debug.Log("Debut Travelling");
		if (currentPath != null)
		{
			currentPath.looped = true;
			currentPath.StopPath();
			currentPath.SetCurrentWayPoint(1);
		}

		if (!path.looped)
		{
			path.looped = true;
		}
		currentPath = path;
		currentPath.SetCurrentWayPoint(waypoint);
		currentPath.PlayPath(time);

		yield return null;
	}

	public IEnumerator StartTravelling()
	{
		yield return new WaitForSeconds(4);
		yield return StartCoroutine(StartTravellingIE(waiting_path, 20f, 1));
	}

	public IEnumerator PauseTravelling()
	{
		currentPath.PausePath();
		yield return null;
	}*/
	
	public void StopTravelling()
	{
		//if (currentPath != null)
		//{
		//	currentPath.SetCurrentWayPoint(1);
		//	currentPath.StopPath();
		//	ResetCameraPosition();
		//}
	}

	public void ResetCameraPosition()
	{
		GameObject.Find("Global/BattleScene/Battle_Camera").transform.localPosition = new Vector3(0,3.89f,-5.24f);
		GameObject.Find("Global/BattleScene/Battle_Camera").transform.rotation = Quaternion.Euler(13.333f,0.118f,0);
	}
}