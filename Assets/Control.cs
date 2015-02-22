using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	public Faction_e holds;//which faction controls the room
	public float hack_time;//time it takes for one side to take over the room
	public bool beingHacked = false; //set tp true if player is hacking room
	public float time_hacked = 0.0f; //counter for time of being hacked
	// Use this for initialization
	void Start () {
		holds = Faction_e.spaceCop;//starts with space cop
	}
	
	// Update is called once per frame
	void Update () {
		if (beingHacked == true && time_hacked < hack_time) {
			time_hacked += Time.deltaTime;
		}
		if (time_hacked >= hack_time) {
			holds = Faction_e.spaceCrim;//control to Criminals
			beingHacked = false;
		}
	}

	void OnTriggerEnter(Collider other){
		GameObject player = GameObject.Find (other.gameObject.name);


		if (player.name == "Player") {
			PlayerStats stats = player.GetComponent("PlayerStats") as PlayerStats;
			if(stats.team == Faction_e.spaceCrim && holds == Faction_e.spaceCop){
				beingHacked = true;
			}
			if(stats.team == Faction_e.spaceCop){

			}
				
		}
	}

	void OnTriggerStay(Collider other){
		OnTriggerEnter (other);
	}

	void OnTriggerExit(Collider other){
		string name = other.gameObject.name;
		if (name == "Player") {
			GameObject player = GameObject.Find (other.gameObject.name);
			PlayerStats stats = player.GetComponent("PlayerStats") as PlayerStats;
			if(stats.team == Faction_e.spaceCrim){
				beingHacked = false;
			}
		}
	}
}
