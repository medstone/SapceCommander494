using UnityEngine;
using System.Collections;

public enum Faction_e {
	spaceCop,
	spaceCrim
}

public enum PlayerNum_e{
	p1,
	p2,
	p3,
	p4
}

// Dealing with the player's health, equipment, etc.
public class PlayerStats : MonoBehaviour {
	PlayerControl control;

	public Faction_e team;
	public PlayerNum_e player;

	public Weapon weapon; 

	public int startingHealth;
	public int health;

	int damageTaken = 0; 
	bool invincible;
	public float invincibleDur; // how long does the player ignore damage after taking a hit


	bool collidingWithWeapon;

	void Awake(){
		control = GetComponent<PlayerControl> ();
		weapon = GetComponentInChildren<Weapon> ();
	}
	// Use this for initialization
	void Start () {
		health = startingHealth;
		invincible = false;
		weapon.owner = transform;
		weapon.canShoot = true;
		collidingWithWeapon = false;
	}


	void FixedUpdate () {
		// maybe this should be handled in PlayerControl
		if (control.triggerDown) {
			weapon.Shoot ();
		}
	}

	void OnTriggerStay(Collider coll){
		if (coll.tag == "WeaponPickup" && control.xButtonDown) {
			collidingWithWeapon = true;
			//StartCoroutine(PickUpWeapon(coll.GetComponent<ItemPickup>()));
		}
	}

	void OnTriggerExit(Collider coll){
		collidingWithWeapon = false;
	}

	IEnumerator PickUpWeapon(GameObject wep){
		float startTime = Time.time;
		while (Time.time - startTime > 0.75f && collidingWithWeapon && control.xButtonDown) {
			yield return null;
		}
		if (wep != null) {
			wep.collider.enabled = false;
			weapon = wep;
		}
		collidingWithWeapon = false;
	}

	void LateUpdate(){
		if (damageTaken > 0) {
			health -= damageTaken;
			damageTaken = 0;
		}
		if (health <= 0) {
			Die ();
		}
	}

	public void TakeHit(int dmg){
		if (invincible)
			return;
		damageTaken += dmg;
		StartCoroutine (Invincibility ());
	}

	void Die(){
		// it would probably be cool to award a kill to whoever did you in.
		StartCoroutine (Death ());
	}

	IEnumerator Invincibility(){
		invincible = true;
		float startTime = Time.time;
		while (Time.time - startTime < invincibleDur) {
			renderer.enabled = !renderer.enabled;
			yield return null;
		}
		renderer.enabled = true;
		invincible = false;
	}

	IEnumerator Death(){
		rigidbody.velocity = Vector3.zero;
		control.enabled = false;
		collider.enabled = false;
		renderer.enabled = false;
		weapon.renderer.enabled = false;
		yield return new WaitForSeconds(3);
		Reset ();
	}

	void Reset(){
		// move to spawn point

		// turn everything back on
		if (control.inDevice != null)
			control.enabled = true;
		collider.enabled = true;
		renderer.enabled = true;
		weapon.renderer.enabled = true;
		health = startingHealth;
	}


}
