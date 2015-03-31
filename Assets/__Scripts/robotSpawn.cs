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
		setFact = GetComponent<Control> ().holds;
		InvokeRepeating ("MakeRobots", 1f, 2f);
		GetComponent<Control> ().CapturedEvent += TeamSwap; // gets called when room changes hands
		//copdir = spawnDirection;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void MakeRobots(){
		if (numSpawned == maxSpawned) {
			return;
		}
		numSpawned++;
		GameObject robot = Instantiate (robotPrefab) as GameObject;
		robot.transform.position = this.transform.position;
		robot.GetComponent<Cylon> ().direction = spawnDirection;
		robot.GetComponent<Cylon> ().faction = setFact;
		robot.GetComponent<Cylon> ().spawnerRef = this;
	}

	void TeamSwap(Faction_e new_team){
		setFact = new_team;
		if(setFact == Faction_e.spaceCrim){
			spawnDirection = crimdir;
		}
		if(setFact == Faction_e.spaceCop){
			spawnDirection = copdir;
		}
	}
}
