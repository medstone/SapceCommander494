using UnityEngine;
using System.Collections;

public class SpawnRobots : MonoBehaviour {

	public GameObject robot;

	// Use this for initialization
	void Start () {
		Instantiate (robot, this.transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
