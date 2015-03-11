using UnityEngine;
using System.Collections;

public class robotSpawn : MonoBehaviour {
	public Faction_e setFact;
	public Vector3 spawnDirection;
	public GameObject robotPrefab;
	public Vector3 crimdir;
	public Vector3 copdir;
	public int numSpawned = 0;
	public int maxSpawned = 4;

	// Use this for initialization
	void Start () {
		setFact = GameObject.Find ("RobotRoom").GetComponent<Control> ().holds;
		InvokeRepeating ("MakeRobots", 1f, 2f);
		copdir = spawnDirection;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeRobots(){
		if (numSpawned == maxSpawned) {
			return;
		}
		if (setFact != GameObject.Find ("RobotRoom").GetComponent<Control> ().holds) {
			setFact = GameObject.Find ("RobotRoom").GetComponent<Control> ().holds;
			if(setFact == Faction_e.spaceCrim){
				spawnDirection = crimdir;
			}
			if(setFact == Faction_e.spaceCop){
				spawnDirection = copdir;
			}
		}
		numSpawned++;
		GameObject robot = Instantiate (robotPrefab) as GameObject;
		robot.transform.position = this.transform.position;
		robot.GetComponent<robotAI> ().direction = spawnDirection;
		robot.GetComponent<robotAI> ().fact = setFact;
		robot.GetComponent<robotAI> ().spawn = this;
	}
}
