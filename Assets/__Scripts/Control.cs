﻿using UnityEngine;
using System.Collections;

public enum HackState_e{
	none,
	hack,
	unhack
}

public delegate void CapturedRoomHandler(Faction_e new_team);
public delegate void CaptureAmountHandler(float amountCaptured);

public class Control : MonoBehaviour {
	public Faction_e holds;//which faction controls the room
	public float hack_time;//time it takes for one side to take over the room
	public HackState_e hackState = HackState_e.none;
	public float time_hacked = 0.0f; //counter for time of being hacked
	public bool locked; // if the station is locked, it cannot be captured
	public bool lockOnCapture; // if true, station will become locked after first capture.
	
	public string roomName = "Untitled Room";

	AudioSource aud;
	
	// Transform hackBar;
	// Vector3 barScale;
	public Material copColor;
	public Material crimColor;

	public int copsInRoom;
	public int crimsInRoom;

	public event CapturedRoomHandler CapturedEvent;
	public event CaptureAmountHandler CaptureAmountEvent;
	
	void Awake () { 
		// hackBar = transform.Find("HackBar");
		// barScale = hackBar.localScale;
	}
	
	// Use this for initialization
	void Start () {
		copsInRoom = 0;
		crimsInRoom = 0;
		aud = this.gameObject.GetComponent<AudioSource> ();
	}
	

	// figure out if any hacking is going on
	void FixedUpdate () {
		if (locked) {
			hackState = HackState_e.none;
			aud.Stop();
			return;
		}
		if (hackState != HackState_e.hack && aud.isPlaying) {
			aud.Stop ();
		} else if(!aud.isPlaying && hackState == HackState_e.hack) {
			aud.Play();
		}
		// !!! conditions for hacking and unhacking should be mutually exclusive 
		if (holds == Faction_e.spaceCrim) {
			if (copsInRoom > 0 && crimsInRoom <= 0){
				if (hackState == HackState_e.none)
					StartCoroutine(Hacking (Faction_e.spaceCop));
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
					StartCoroutine (Hacking (Faction_e.spaceCrim));
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
		
		RoomListener rm = other.GetComponent<RoomListener>();
		if(rm) {
			rm.Display(roomName);
		}
	}

	float Multiplier(int numCapturers){
		switch (numCapturers) {
		case 1:
			return 1.0f;
		case 2:
			return 2f;
		case 3:
			return 3.0f;
		default:
			return 1.0f;
		}
	}

	IEnumerator Hacking(Faction_e team_hacking){
		hackState = HackState_e.hack;
		float counter = 0.0f;
		float mult = 1.0f;
		while (hackState == HackState_e.hack && counter < hack_time - time_hacked) {
			// adjust x scale to percentage of amount hacked
			// Vector3 scale = barScale;
			// scale.x *= ((hack_time - time_hacked - (counter)) / hack_time);
			// hackBar.localScale = scale;
			counter += Time.deltaTime * mult;
			if (team_hacking == Faction_e.spaceCop)
				mult = Multiplier (copsInRoom); // maybe they should have SOME bonus
			else
				mult = Multiplier (crimsInRoom);
			if (CaptureAmountEvent != null)
				CaptureAmountEvent(counter + time_hacked);
			//aud.Play();
			yield return null;
		}
		hackState = HackState_e.none;
		//aud.Stop ();
		// only do the following if the hacking went all the way!
		if (counter >= hack_time - time_hacked) {
			CloneRoom cloneRef = GetComponent<CloneRoom>();
			if (cloneRef != null)
				MatchManager.S.CapturedSpawnPoint (cloneRef);
			if (holds == Faction_e.spaceCop) {
				holds = Faction_e.spaceCrim;//control to Criminals
				// hackBar.GetComponent<Renderer> ().material = crimColor;

			} else {
				holds = Faction_e.spaceCop;
				// hackBar.GetComponent<Renderer> ().material = copColor;
			}
			// hackBar.localScale = barScale;
			time_hacked = 0f;
			if (lockOnCapture)
				locked = true;
			if (CapturedEvent != null){
				CapturedEvent(holds);
			}
			if (CaptureAmountEvent != null){
				CaptureAmountEvent(0f);
			}
		} 
		else { // hang onto the amount of hacking time accrued 
			time_hacked += counter;
		}
	}

	// reversing the process -> making time_hacked cruise down to 0.0f
	IEnumerator UnHacking(){
		float startTime = Time.time;
		hackState = HackState_e.unhack;
		while (hackState == HackState_e.unhack && Time.time - startTime < time_hacked) { // up to the time that has already been messed with
			// adjust x scale to percentage of amount unhacked
			// Vector3 scale = barScale;
			// scale.x *= ((hack_time - time_hacked + (Time.time - startTime )) / hack_time);
			// hackBar.localScale = scale;
			if (CaptureAmountEvent != null)
				CaptureAmountEvent(time_hacked - (Time.time - startTime));
			//aud.Play();
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
