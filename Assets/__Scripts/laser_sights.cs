using UnityEngine;
using System.Collections;

public class laser_sights : MonoBehaviour {

	LineRenderer line;
	RaycastHit hit;
	public Material line_mat;
	int layermask = ~0; // hit everything by default

	// Use this for initialization
	void Start () {

		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (2);
		//line.renderer.material = line_mat;
		line.SetWidth (0.1f, 0.1f);
		line.SetColors (Color.red, Color.red);
		line.enabled = true;

		// ignore all projectiles
		IgnoreLayer (LayerMask.NameToLayer ("CrimProjectile"));
		IgnoreLayer (LayerMask.NameToLayer ("CopProjectile"));
		IgnoreLayer (LayerMask.NameToLayer ("CrimWepBarrier"));
		IgnoreLayer (LayerMask.NameToLayer ("CopWepBarrier"));
		IgnoreLayer (LayerMask.NameToLayer ("Ignore Raycast")); // yes, there is a space

		PlayerStats playerRef = GetComponent<PlayerStats> ();
		if (playerRef != null){ 
			if (playerRef.team == Faction_e.spaceCop) {
			IgnoreLayer (LayerMask.NameToLayer ("Cops"));
		} else {
			IgnoreLayer(LayerMask.NameToLayer("Crims"));
			}
		}
	}

	void IgnoreLayer(int layer){
		layermask -= (1 << layer);
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (ray, out hit, 1000f, layermask)) {
			line.SetPosition(0,this.transform.position);
			line.SetPosition(1, hit.point);
		}
	
	}
}
