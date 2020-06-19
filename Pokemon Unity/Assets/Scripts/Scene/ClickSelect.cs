using System;
using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace PokemonUnity
{
	public class ClickSelect : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;
		public static event GameFrameworkAction<Entity> OnSelectedEntityChanged;
		public static Entity SelectedEntity { get; private set; }

		private void Update()
		{
			if (Input.GetMouseButtonDown(0)) //Change to "Select"
			{
				float distance = 25f;
				Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
				Debug.DrawRay(ray.origin, ray.direction * distance, UnityEngine.Color.red, duration: 1f, true);
				if(Physics.Raycast(ray, out RaycastHit hitinfo, maxDistance: distance))
				{
					Entity entity = hitinfo.collider.GetComponent<Entity>();
					SelectedEntity = entity;
					if (OnSelectedEntityChanged != null) OnSelectedEntityChanged(entity);
				}
			}
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SelectedEntity = null;
				if (OnSelectedEntityChanged != null) OnSelectedEntityChanged(null);
			}
		}
	}
}
