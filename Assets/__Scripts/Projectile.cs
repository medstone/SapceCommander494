﻿using UnityEngine;
using System.Collections;

// projectiles should destroy themselves when they collide with *something* be it a wall or a player
public class Projectile : MonoBehaviour {

	public float speed;
	public Vector3 bearing;

	public int damage;

	public int layermask = 0;

	// Use this for initialization
	void Start () {
	}

	public void SetBearing(Vector3 value){
		bearing = value;
		bearing.Normalize ();
	}

	void FixedUpdate(){
		float dt = Time.fixedDeltaTime;
		Vector3 pos = transform.position;
		pos.x += dt * speed * bearing.x;
		pos.z += dt * speed * bearing.z;
		transform.position = pos;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, bearing, out hit, 1f, layermask)) {
			RayHit (hit.collider);
		}
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
				} 
				else if (coll.gameObject.CompareTag ("Wall")) {
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
}
