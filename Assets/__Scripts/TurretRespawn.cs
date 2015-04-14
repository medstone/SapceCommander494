using UnityEngine;
using System.Collections;

public class TurretRespawn : MonoBehaviour {

	public float respawn_time = 5.0f;
	public float timer = 0.0f;
	public bool turret_spawned = false;
	GameObject curr_turret;
	public GameObject turret;
	public GameObject turr_spawn_point;

	// Use this for initialization
	void Start () {
		Vector3 pos = turr_spawn_point.transform.position;
		curr_turret = Instantiate (turret, pos, this.transform.rotation) as GameObject;
		turret_fire turr = curr_turret.GetComponent ("turret_fire") as turret_fire;
		turr.Turret_room = this.gameObject;
		turret_spawned = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (turret_spawned == false && timer >= respawn_time) {
			Vector3 pos = turr_spawn_point.transform.position;
			curr_turret = Instantiate (turret, pos, this.transform.rotation) as GameObject;
			turret_fire turr = curr_turret.GetComponent ("turret_fire") as turret_fire;
			turr.Turret_room = this.gameObject;
			turret_spawned = true;
			timer = 0.0f;
		} else if (turret_spawned == false) {
			timer += Time.deltaTime;
		}
	
	}
}
