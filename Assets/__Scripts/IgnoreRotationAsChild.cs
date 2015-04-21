using UnityEngine;
using System.Collections;

public class IgnoreRotationAsChild : MonoBehaviour {
	private Quaternion rotation;
	
	void Awake()
	{
		rotation = transform.rotation;
	}

	void LateUpdate()
	{
		transform.rotation = rotation;
	}
}