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
		StartCoroutine (MakeRobots ());
		GetComponent<Control> ().CapturedEvent += TeamSwap; // gets called when room changes hands
		//copdir = spawnDirection;
	}
	


	IEnumerator MakeRobots(){
		while (true) {
			if (numSpawned == maxSpawned) {
				yield return null;
			} 
			else {
				yield return new WaitForSeconds(2f); // so there's a bit of delay
				numSpawned++;
				GameObject robot = Instantiate (robotPrefab) as GameObject;
				robot.transform.position = this.transform.position;
				robot.GetComponent<Cylon> ().direction = spawnDirection;
				robot.GetComponent<Cylon> ().faction = setFact;
				robot.GetComponent<Cylon> ().spawnerRef = this;
			}
			yield return null;
		}
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
