using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class TransformExtension
{
	public static UnityEngine.Transform ClearChildren(this UnityEngine.Transform transform)
	{
		foreach (UnityEngine.Transform child in transform)
		{
			UnityEngine.GameObject.Destroy(child.gameObject);
		}
		return transform;
	}
}