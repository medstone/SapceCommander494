using UnityEngine;
using System.Collections;

public class BarrierControlRoom : MonoBehaviour {
	public Control control;
	public GameObject[] barriers;
	public LineRenderer[] lines; // sprites that lead to the barriers controlled by this room
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
		foreach (LineRenderer line in lines) {
			if (control.holds == Faction_e.spaceCop)
				line.material = control.copColor;
			else
				line.material = control.crimColor;
		}
	}
	

	void OnCapture(Faction_e new_team){
		foreach(GameObject go in barriers){
			Destroy(go.gameObject); // should it actually destroy it?
		}
		foreach(LineRenderer line in lines){
			if (new_team == Faction_e.spaceCrim)
				line.material = control.crimColor;
			else
				line.material = control.copColor;
		}
		MatchManager.S.KeyRoomCaptured (); // notify match manager that this key room was captured.
	}
}
