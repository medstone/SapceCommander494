using UnityEngine;
using System.Collections;

// projectiles should destroy themselves when they collide with *something* be it a wall or a player
public class Projectile : MonoBehaviour {

	public float speed;
	public Vector3 bearing;

	public int damage;

	int layermask = ~0;

	// Use this for initialization
	void Start () {
	}

	public void IgnoreLayer(int layer){
		layermask -= (1 << layer);
	}

	public Vector3 GetBearing(){
		return bearing;
	}

	public void SetBearing(Vector3 value){
		bearing = value;
		bearing.Normalize ();
	}

	void FixedUpdate(){

//		RaycastHit hit;
//		if (Physics.Raycast (transform.position, bearing, out hit, 0.5f, layermask)) {
//			RayHit (hit.collider);
//		}

		float dt = Time.fixedDeltaTime;
		Vector3 pos = transform.position;
		pos.x += dt * speed * bearing.x;
		pos.z += dt * speed * bearing.z;
		transform.position = pos;

	
	}


	void RayHit(Collider coll){
//		Debug.Log ("Ray hit");
//		Debug.Log (gameObject.layer);
//		Debug.Log(coll.gameObject.layer);
		if (coll.tag == "Actor") {
						ActorHit (coll.gameObject);
						Destroy (this.gameObject);
				} else if (coll.tag == "Robot") {
						RobotHit (coll.gameObject);
						Destroy (this.gameObject);
				} else if (coll.tag == "Wall") {
						Destroy (this.gameObject);
				} else if (coll.tag == "Turret") {
						TurretHit(coll.gameObject);
						Destroy(this.gameObject);
				}
	}

	void OnTriggerStay(Collider coll){
		if (coll.tag == "Console") {
			ConsoleHit (coll.gameObject);		
			Destroy (this.gameObject);
		}
	}

	// needed because players' colliders are not triggers
	void OnCollisionStay(Collision coll){
//		Debug.Log ("Collision");
//		Debug.Log (gameObject.layer);
//		Debug.Log(coll.gameObject.layer);
		if (coll.gameObject.CompareTag ("Actor")) { // "Player" tag is being used by InControl I think
						ActorHit (coll.gameObject);
						Destroy (this.gameObject);
				} else if (coll.gameObject.tag == "Robot") {
						RobotHit (coll.gameObject);
						Destroy (this.gameObject);
				} else if (coll.gameObject.CompareTag ("Wall")) {
						Destroy (this.gameObject);
				} else if (coll.gameObject.CompareTag ("Turret")) {
						TurretHit(coll.gameObject);
						Destroy (this.gameObject);
				}
	}

	void ActorHit(GameObject player){
		PlayerStats pStats = player.GetComponent<PlayerStats> ();
		// ignore your own bullets, in case you can move faster than they can.
		// alternatively, this could ignore a specific team.
		pStats.TakeHit (damage);
	}

	void RobotHit(GameObject robot){
		Destroy (robot.gameObject);
	}

	void ConsoleHit(GameObject console){
		RoomConsole roomCon = console.GetComponent<RoomConsole> ();
		roomCon.TakeHit (damage);
	}

	void TurretHit(GameObject turret){
		turret_fire turrStats = turret.GetComponent<turret_fire> ();
		turrStats.TakeHit (damage);
	}
}
