using UnityEngine;
using System.Collections;

public class robotSpawn : MonoBehaviour {
	public Vector3 spawnDirection;
	public GameObject robotPrefab;

	// Use this for initialization
	void Start () {
		InvokeRepeating ("MakeRobots", 1f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeRobots(){
		GameObject robot = Instantiate (robotPrefab) as GameObject;
		robot.transform.position = this.transform.position;
		robot.GetComponent<robotAI> ().direction = spawnDirection;
	}
}
