using UnityEngine;
using System.Collections;

public class BarrierControlRoom : MonoBehaviour {
	public Control control;
	public GameObject[] barriers;
	public LineRenderer[] lines; // sprites that lead to the barriers controlled by this room
	public Material line_mat;
	public bool cop_cont;
	RoomConsole console;
	Color blue;
	Color black;
	Color red;
	Color blue_adj;
	Color black_adj;
	float timer = 0.0f;

	public bool Broken(){
		if (console == null)
			return false;
		return console.IsBroken;
	}

	void Awake(){
		control = GetComponent<Control> ();
		console = GetComponentInChildren<RoomConsole> ();
		blue = new Color (0f, 0f, 1f, 1f);
		black = new Color (0f, 0f, 0f, 1f);
		red = new Color (0.5f, 0f, 0f, 1f);
		blue_adj = blue;
		black_adj = black;

	}


	void Start(){
		control.CapturedEvent += OnCapture;
		foreach (LineRenderer line in lines) {
			line.material = line_mat;
			if (control.holds == Faction_e.spaceCop){
				line.SetColors(blue,black);
				cop_cont = true;
			}
			else{
				line.SetColors(red,red);
				cop_cont = false;
			}
		}
	}

	void FixedUpdate(){
		if (cop_cont == true) {
			foreach (LineRenderer line in lines) {
				timer += Time.deltaTime;
				float blue_a = Mathf.Cos (3 * timer);
				float black_a = Mathf.Sin (3 * timer);
				blue_adj = new Color (0f, 0f, blue_a, 1f);
				black_adj = new Color (0f, 0f, black_a, 1f);
				line.SetColors (blue_adj, black_adj);
				
			}
		}

	}
	

	void OnCapture(Faction_e new_team){
		foreach(GameObject go in barriers){
			Destroy(go.gameObject); // should it actually destroy it?
		}
		foreach(LineRenderer line in lines){
			if (new_team == Faction_e.spaceCrim){
				line.SetColors(red,red);
				cop_cont = false;
			}
			else{
				line.SetColors(blue,black);
				blue_adj = blue;
				black_adj = black;
				cop_cont = true;
			}
		}
		MatchManager.S.KeyRoomCaptured (); // notify match manager that this key room was captured.
	}
}
