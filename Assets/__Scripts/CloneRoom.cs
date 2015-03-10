using UnityEngine;
using System.Collections;

public class CloneRoom : MonoBehaviour {
	public Control control;
	RoomConsole console;

	public bool Broken(){
		if (console == null)
			return false;
		return console.IsBroken;
	}

	void Awake(){
		control = GetComponent<Control> ();
		console = GetComponentInChildren<RoomConsole> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// This is the kind of thing that should be taken care of with good 
	// class design
	void FixedUpdate () {
		if (control.holds == Faction_e.spaceCop) {
			// DON'T CHANGE THE LAYER OF THE ROOM
			if (console != null)
				console.gameObject.layer = Utils.CopLayer ();
		} else {
			if (console != null)
				console.gameObject.layer = Utils.CrimLayer();
		}
	}
}
