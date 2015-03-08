using UnityEngine;
using System.Collections;

public enum Faction_e {
	spaceCop,
	spaceCrim,
	neutral
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

	public float damageAnimDur;
	bool damaged;

	bool collidingWithWeapon;
	public bool repairing;

	void Awake(){
		control = GetComponent<PlayerControl> ();
		defaultWeapon = GetComponentInChildren<Weapon> ();
		secondaryWeapon = null;
	}
	// Use this for initialization
	void Start () {
		health = startingHealth;
		damaged = false;
		defaultWeapon.canShoot = true;
		defaultWeapon.allegiance = team;
		collidingWithWeapon = false;
		repairing = false;
	}


	void FixedUpdate () {
		if (repairing) {
			return; // don't allow shooting if repairing
		}
		// maybe this should be handled in PlayerControl
		if (control.triggerDown) {
			if (secondaryWeapon != null)
				secondaryWeapon.Shoot ();
			else
				defaultWeapon.Shoot ();
		}
	}

	void OnTriggerStay(Collider coll){
		if (coll.tag == "WeaponPickup" && control.xButtonDown){
			Weapon wepRef = coll.GetComponent<Weapon>();
			if (!collidingWithWeapon){// so we don't start the coroutine a bunch of times
				if (wepRef.allegiance == team || wepRef.allegiance == Faction_e.neutral){ 
					StartCoroutine(PickUpWeapon(coll.gameObject));
				}
			}
		}
		else if (coll.tag == "Console" && control.aButtonDown) {
			RoomConsole console = coll.GetComponent<RoomConsole>();
			if (!repairing && console.IsDamaged){
				StartCoroutine(RepairStation(console));
			}
		}
	}

	void OnTriggerExit(Collider coll){
		if (coll.tag == "WeaponPickup")
			collidingWithWeapon = false;
		else if (coll.tag == "Console")
			repairing = false;
	}

	IEnumerator RepairStation(RoomConsole console){
		repairing = true;
		while (repairing && control.aButtonDown) {
			repairing = console.Repair();		
			yield return null;
		}
		console.Repair (false); // tell console we're stoppin
		repairing = false;
	}

	IEnumerator PickUpWeapon(GameObject item){
		collidingWithWeapon = true;
		float startTime = Time.time;
		while (Time.time - startTime < 0.25f && collidingWithWeapon && control.xButtonDown) {
			yield return null;
		}
		if (item != null && collidingWithWeapon && control.xButtonDown) {
			// picking up the weapon
			Weapon pickup = item.GetComponent<Weapon>();
			// set to location and rotation of current weapon
			pickup.transform.position = defaultWeapon.transform.position;
			pickup.transform.rotation = defaultWeapon.transform.rotation;
			pickup.transform.SetParent(transform);
			pickup.allegiance = team;
			pickup.tag = "Weapon";
			if (secondaryWeapon != null){
				// already have a secondary weapon
				secondaryWeapon.transform.parent = null;
				secondaryWeapon.tag = "WeaponPickup";
				secondaryWeapon.allegiance = Faction_e.neutral;
			}
			else {
				// turn off primary weapon
				defaultWeapon.enabled = false;
			}
			secondaryWeapon = pickup;

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

		damageTaken += dmg;
		if (!damaged)
			StartCoroutine (DamageAnimation ());
	}

	void Die(){
		// it would probably be cool to award a kill to whoever did you in.
		StartCoroutine (Death ());
	}

	IEnumerator DamageAnimation(){
		damaged = true;
		float startTime = Time.time;
		while (Time.time - startTime < damageAnimDur) {
			GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
			yield return null;
		}
		GetComponent<Renderer>().enabled = true;
		damaged = false;
	}

	IEnumerator Death(){
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		control.enabled = false;
		GetComponent<Renderer>().enabled = false;
		if (secondaryWeapon != null) {
			// drop secondary weapon
			secondaryWeapon.transform.parent = null;
			secondaryWeapon.tag = "WeaponPickup";
			secondaryWeapon.allegiance = Faction_e.neutral;
			secondaryWeapon = null;
			defaultWeapon.enabled = true;
		}
		defaultWeapon.canShoot = false;
		defaultWeapon.GetComponent<Renderer>().enabled = false;
		// rather than turning off the collider, move the dead player to some faraway place
		Vector3 offScreen = new Vector3 (0f, -500f);
		transform.position = offScreen;
		yield return new WaitForSeconds(3);
		Reset ();
	}

	void Reset(){
		// move to spawn point
		if (team == Faction_e.spaceCop) {
			transform.position = MatchManager.S.GetCopSpawnPoint().position;
		} else if (team == Faction_e.spaceCrim) {
			transform.position = MatchManager.S.GetCrimSpawnPoint().position;
		}
		// turn everything back on
		if (control.inDevice != null)
			control.enabled = true;
		GetComponent<Renderer>().enabled = true;
		defaultWeapon.GetComponent<Renderer>().enabled = true;
		defaultWeapon.canShoot = true;
		health = startingHealth;

	}


}
