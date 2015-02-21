using UnityEngine;
using System.Collections;

// projectiles should destroy themselves when they collide with *something* be it a wall or a player
public class Projectile : MonoBehaviour {

	public float speed;
	public Vector3 bearing;

	// Use this for initialization
	void Start () {
	
	}


	void FixedUpdate () {
		float dt = Time.fixedDeltaTime;
		Vector3 pos = transform.position;
		pos.x += dt * speed * bearing.x;
		pos.z += dt * speed * bearing.z;
		transform.position = pos;
	}

	void OnTriggerEnter(Collider coll){
		// this could possibly be bad if bullets could ever spawn inside of some collider. 
	}

	void OnTriggerStay(Collider coll){
		if (coll.tag == "Player") {
			//get player component, .TakeHit();
		}
	}
	
}
