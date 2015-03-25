using UnityEngine;
using System.Collections;

public class turret_fire : MonoBehaviour {

	public GameObject bullet;
	public GameObject Turret_room;
	public float radius = 5.0f;
	public GameObject[] butts;
	public float rate_of_fire = 0.5f;
	public float timer = 0.0f;
	public bool destroyed = false;
	public int health = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		butts = GameObject.FindGameObjectsWithTag ("Actor");
		Control turr = Turret_room.GetComponent ("Control") as Control;
		int closest = -1;
		float min_dist = 1000000.0f;//arbitrarily high
		float dist = 0.0f;
		for (int i = 0; i < butts.Length; i++) {
			dist = Vector3.Distance(this.transform.position,butts[i].transform.position);
			PlayerStats play = butts[i].GetComponent("PlayerStats") as PlayerStats;
			if(dist < min_dist && play.team != turr.holds && destroyed == false){
				min_dist = dist;
				closest = i;
			}
		}
		if (closest >= 0) {
					this.transform.LookAt (butts [closest].transform.position);
					if (min_dist < radius && timer > rate_of_fire) {
							//Quaternion rot = this.transform.rotation;
							//rot.x += 180;
							//this.transform.rotation = rot;
							Vector3 pos = this.transform.position;
							pos.y += this.transform.lossyScale.y / 2f;
							Instantiate (bullet, pos, this.transform.rotation);
							timer = 0.0f;
						} else {
							timer += Time.deltaTime;
						}
				}

		}
}
