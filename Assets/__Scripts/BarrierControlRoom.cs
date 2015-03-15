using UnityEngine;
using System.Collections;

public class BarrierControlRoom : MonoBehaviour {
	public Control control;
	public GameObject[] barriers;
	RoomConsole console;
	bool active;

	Faction_e originalTeam;

	public bool Broken(){
		if (console == null)
			return false;
		return console.IsBroken;
	}

	void Awake(){
		control = GetComponent<Control> ();
		console = GetComponentInChildren<RoomConsole> ();
		originalTeam = control.holds;
		active = true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (control.holds != originalTeam && active) {
			active = false;
			foreach(GameObject go in barriers){
				Destroy (go.gameObject);
			}
		}
	}
}
