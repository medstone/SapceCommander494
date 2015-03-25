using UnityEngine;
using System.Collections;

public class BarrierControlRoom : MonoBehaviour {
	public Control control;
	public GameObject[] barriers;
	public GameObject[] dashes; // sprites that lead to the barriers controlled by this room
	public Sprite blueDash;
	public Sprite redDash;
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

	void Start(){
		control.CapturedEvent += OnCapture;
	}

	void OnCapture(Faction_e new_team){
		foreach(GameObject go in barriers){
			Destroy(go.gameObject); // should it actually destroy it?
		}
		foreach(GameObject go in dashes){
			if (new_team == Faction_e.spaceCrim)
				go.GetComponent<SpriteRenderer>().sprite = redDash;
			else
				go.GetComponent<SpriteRenderer>().sprite = blueDash;
		}
		MatchManager.S.KeyRoomCaptured (); // notify match manager that this key room was captured.
	}
}
