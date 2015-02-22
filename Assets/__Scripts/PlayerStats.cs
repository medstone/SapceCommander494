﻿using UnityEngine;
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

	public Weapon defaultWeapon;
	public Weapon secondaryWeapon; 

	public int startingHealth;
	public int health;

	int damageTaken = 0; 
	bool invincible;
	public float invincibleDur; // how long does the player ignore damage after taking a hit


	bool collidingWithWeapon;
	GameObject pickupRef;

	void Awake(){
		control = GetComponent<PlayerControl> ();
		defaultWeapon = GetComponentInChildren<Weapon> ();
		secondaryWeapon = null;
	}
	// Use this for initialization
	void Start () {
		health = startingHealth;
		invincible = false;
		defaultWeapon.canShoot = true;
		collidingWithWeapon = false;
	}


	void FixedUpdate () {
		// maybe this should be handled in PlayerControl
		if (control.triggerDown) {
			if (secondaryWeapon != null)
				secondaryWeapon.Shoot ();
			else
				defaultWeapon.Shoot ();
		}
	}

	void OnTriggerStay(Collider coll){
		if (coll.tag == "WeaponPickup" && control.xButtonDown) {
			if (!collidingWithWeapon) // so we don't start the coroutine a bunch of times
				StartCoroutine(PickUpWeapon(coll.gameObject));
		}
	}

	void OnTriggerExit(Collider coll){
		collidingWithWeapon = false;
	}

	IEnumerator PickUpWeapon(GameObject item){
		collidingWithWeapon = true;
		float startTime = Time.time;
		while (Time.time - startTime < 0.25f && collidingWithWeapon && control.xButtonDown) {
			yield return null;
		}
		if (item != null && collidingWithWeapon && control.xButtonDown) {
			// picking up the weapon
			ItemPickup pickup = item.GetComponent<ItemPickup>();
			pickupRef = WeaponFactory.S.GetWeapon(pickup.itemName);
			// set to location and rotation of current weapon
			pickupRef.transform.position = defaultWeapon.transform.position;
			pickupRef.transform.rotation = defaultWeapon.transform.rotation;
			pickupRef.transform.SetParent(transform);
			if (secondaryWeapon != null){
				// already have a secondary weapon
				Destroy (secondaryWeapon.gameObject);
			}
			else {
				// turn off primary weapon
				defaultWeapon.enabled = false;
			}
			secondaryWeapon = pickupRef.GetComponent<Weapon>();
			// destroy item pickup
			Destroy (item.gameObject);
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
		if (secondaryWeapon != null) {
			Destroy (secondaryWeapon.gameObject);
			defaultWeapon.enabled = true;
		}
		defaultWeapon.renderer.enabled = false;
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
		defaultWeapon.renderer.enabled = true;
		health = startingHealth;
	}


}
