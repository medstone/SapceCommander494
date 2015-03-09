using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	public Faction_e holds;//which faction controls the room
	public float hack_time;//time it takes for one side to take over the room
	public bool beingHacked = false; //set tp true if player is hacking room
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
	
<<<<<<< HEAD
	// Update is called once per frame
	void Update () {
		if (beingHacked == true && time_hacked < hack_time) {
			time_hacked += Time.deltaTime;
			
			// adjust x scale to percentage of amount hacked
			Vector3 scale = barScale;
			scale.x *= ((hack_time - time_hacked) / hack_time);
			hackBar.localScale = scale;
		}
		if (time_hacked >= hack_time) {
			holds = Faction_e.spaceCrim;//control to Criminals
			beingHacked = false;
			hacked = true;
			
			// change the color to criminal, and reset the x size
			hackBar.GetComponent<Renderer>().material = crimColor;
			hackBar.localScale = barScale;
=======
	// figure out if any hacking is going on
	void FixedUpdate () {
		// !!! conditions for hacking and unhacking should be mutually exclusive 
		if (holds == Faction_e.spaceCrim) {
			if (copsInRoom > 0 && crimsInRoom <= 0){
				if (!beingHacked)
					StartCoroutine(Hacking ());
			}
			else if (copsInRoom <= 0 && time_hacked > 0f){
				if (!beingHacked)
					StartCoroutine(UnHacking());
			}
			else { // nobody is hacking anything otherwise
				beingHacked = false;
			}
		} 
		else if (holds == Faction_e.spaceCop) {
			// check for plurality of crims
			if (crimsInRoom > 0 && copsInRoom <= 0) {
				if (!beingHacked)
					StartCoroutine (Hacking ());
			}
			else if (crimsInRoom <= 0 && time_hacked > 0f){
				if (!beingHacked)
					StartCoroutine(UnHacking());
			}
			else { // nobody is hacking anything otherwise
				beingHacked = false;
			}
>>>>>>> 8ae8ad6cfa6a764118648821f0efec15bfc1a3de
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
		beingHacked = true;
		while (beingHacked && Time.time - startTime < hack_time - time_hacked) {
			// adjust x scale to percentage of amount hacked
			Vector3 scale = barScale;
			
			scale.x *= ((hack_time - time_hacked - (Time.time - startTime )) / hack_time);
			hackBar.localScale = scale;
			yield return null;
		}
		beingHacked = false;
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
		beingHacked = true;
		while (Time.time - startTime < time_hacked) { // up to the time that has already been messed with
			// adjust x scale to percentage of amount unhacked
			Vector3 scale = barScale;
			scale.x *= ((hack_time - time_hacked + (Time.time - startTime )) / hack_time);
			hackBar.localScale = scale;
			yield return null;
		}
		beingHacked = false;
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
