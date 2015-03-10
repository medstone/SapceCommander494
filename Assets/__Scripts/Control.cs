using UnityEngine;
using System.Collections;

public enum HackState_e{
	none,
	hack,
	unhack
}

public class Control : MonoBehaviour {
	public Faction_e holds;//which faction controls the room
	public float hack_time;//time it takes for one side to take over the room
	public HackState_e hackState = HackState_e.none;
	public float time_hacked = 0.0f; //counter for time of being hacked
	
	Transform hackBar;
	Vector3 barScale;
	public Material copColor;
	public Material crimColor;

	public int copsInRoom;
	public int crimsInRoom;
	
	void Awake () { 
		hackBar = transform.Find("HackBar");
		barScale = hackBar.localScale;
	}
	
	// Use this for initialization
	void Start () {
		copsInRoom = 0;
		crimsInRoom = 0;
		holds = Faction_e.spaceCop;//starts with space cop
	}
	

	// figure out if any hacking is going on
	void FixedUpdate () {
		// !!! conditions for hacking and unhacking should be mutually exclusive 
		if (holds == Faction_e.spaceCrim) {
			if (copsInRoom > 0 && crimsInRoom <= 0){
				if (hackState == HackState_e.none)
					StartCoroutine(Hacking ());
				else if (hackState == HackState_e.unhack)
					hackState = HackState_e.none;
			}
			else if (copsInRoom <= 0 && time_hacked > 0f){
				if (hackState == HackState_e.none)
					StartCoroutine(UnHacking());
				else if (hackState == HackState_e.hack)
					hackState = HackState_e.none;
			}
			else { // nobody is hacking anything otherwise
				hackState = HackState_e.none;
			}
		} 
		else if (holds == Faction_e.spaceCop) {
			// check for plurality of crims
			if (crimsInRoom > 0 && copsInRoom <= 0) {
				if (hackState == HackState_e.none)
					StartCoroutine (Hacking ());
				else if (hackState == HackState_e.unhack)
					hackState = HackState_e.none;
			}
			else if (crimsInRoom <= 0 && time_hacked > 0f){
				if (hackState == HackState_e.none)
					StartCoroutine(UnHacking());
				else if (hackState == HackState_e.hack)
					hackState = HackState_e.none;
			}
			else { // nobody is hacking anything otherwise
				hackState = HackState_e.none;
			}

		}

	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Actor") {
			PlayerStats stats = other.GetComponent("PlayerStats") as PlayerStats;
			if(stats.team == Faction_e.spaceCrim){
				++crimsInRoom;
			}
			if(stats.team == Faction_e.spaceCop){
				++copsInRoom;
			}
				
		}
	}

	IEnumerator Hacking(){
		float startTime = Time.time;
		hackState = HackState_e.hack;
		while (hackState == HackState_e.hack && Time.time - startTime < hack_time - time_hacked) {
			// adjust x scale to percentage of amount hacked
			Vector3 scale = barScale;
			
			scale.x *= ((hack_time - time_hacked - (Time.time - startTime )) / hack_time);
			hackBar.localScale = scale;
			yield return null;
		}
		hackState = HackState_e.none;
		// only do the following if the hacking went all the way!
		if (Time.time - startTime >= hack_time - time_hacked) {
			CloneRoom cloneRef = GetComponent<CloneRoom>();
			if (cloneRef != null)
				MatchManager.S.CapturedSpawnPoint (cloneRef);
			if (holds == Faction_e.spaceCop) {
				holds = Faction_e.spaceCrim;//control to Criminals
				hackBar.GetComponent<Renderer> ().material = crimColor;

			} else {
				holds = Faction_e.spaceCop;
				hackBar.GetComponent<Renderer> ().material = copColor;
			}
			hackBar.localScale = barScale;
			time_hacked = 0f;
		} 
		else { // hang onto the amount of hacking time accrued 
			time_hacked += Time.time - startTime;
		}
	}

	// reversing the process -> making time_hacked cruise down to 0.0f
	IEnumerator UnHacking(){
		float startTime = Time.time;
		hackState = HackState_e.unhack;
		while (hackState == HackState_e.unhack && Time.time - startTime < time_hacked) { // up to the time that has already been messed with
			// adjust x scale to percentage of amount unhacked
			Vector3 scale = barScale;
			scale.x *= ((hack_time - time_hacked + (Time.time - startTime )) / hack_time);
			hackBar.localScale = scale;
			yield return null;
		}
		hackState = HackState_e.none;
		// save the amount that has been unhacked
		time_hacked -= Time.time - startTime;
		// adjust back to zero just in case b/c floating point stuff
		if (time_hacked < 0f) time_hacked = 0f;
	}


	void OnTriggerExit(Collider other){
		if (other.tag == "Actor") {
			PlayerStats stats = other.GetComponent("PlayerStats") as PlayerStats;
			if(stats.team == Faction_e.spaceCrim){
				--crimsInRoom;
			}
			if(stats.team == Faction_e.spaceCop){
				--copsInRoom;
			}
			
		}
	}
}
