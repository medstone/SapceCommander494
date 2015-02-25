using UnityEngine;
using System.Collections;

public class robotSpawn : MonoBehaviour {
	public Faction_e setFact;
	public Vector3 spawnDirection;
	public GameObject robotPrefab;
	public Vector3 crimdir;

	// Use this for initialization
	void Start () {
		setFact = GameObject.Find ("RobotRoom").GetComponent<Control> ().holds;
		InvokeRepeating ("MakeRobots", 1f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeRobots(){
		if (setFact != GameObject.Find ("RobotRoom").GetComponent<Control> ().holds) {
			setFact = GameObject.Find ("RobotRoom").GetComponent<Control> ().holds;
			spawnDirection = crimdir;
		}
		GameObject robot = Instantiate (robotPrefab) as GameObject;
		robot.transform.position = this.transform.position;
		robot.GetComponent<robotAI> ().direction = spawnDirection;
		robot.GetComponent<robotAI> ().fact = setFact;
	}
}
