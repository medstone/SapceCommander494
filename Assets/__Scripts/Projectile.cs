using UnityEngine;
using System.Collections;

// projectiles should destroy themselves when they collide with *something* be it a wall or a player
public class Projectile : MonoBehaviour {

	public float speed;
	public Vector3 bearing;
	public int damage;


	// Use this for initialization
	void Start () {
	}

	void FixedUpdate(){
		float dt = Time.fixedDeltaTime;
		Vector3 pos = transform.position;
		pos.x += dt * speed * bearing.x;
		pos.z += dt * speed * bearing.z;
		transform.position = pos;
	}



	// needed because players' colliders are not triggers
	void OnCollisionEnter(Collision coll){
		if (coll.gameObject.CompareTag ("Actor")) { // "Player" tag is being used by InControl I think
			PlayerStats pStats = coll.gameObject.GetComponent<PlayerStats> ();
			// ignore your own bullets, in case you can move faster than they can.
			// alternatively, this could ignore a specific team.
			pStats.TakeHit (damage);
			Destroy (this.gameObject);
		} else if (coll.gameObject.CompareTag ("Wall")) {
			Destroy (this.gameObject);
		} else if (coll.gameObject.tag == "Robot") {
			Destroy(coll.gameObject);
			Destroy(this.gameObject);
		}
	}
	
}
