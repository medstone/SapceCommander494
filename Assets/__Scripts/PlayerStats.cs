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
	bool pickingUpWep;
	public bool repairing;

	bool dead;

	public Material copColor;
	public Material crimColor;
	public Material damageMat;
	
	public GameObject myCam;
	int defaultOrthoSize = 10;
	int zoomedOutOrthoSize = 30;

	LineRenderer laserSightRef;
	
	// FadeMessage contextNotify;

	void Awake(){
		control = GetComponent<PlayerControl> ();
		defaultWeapon = GetComponentInChildren<Weapon> ();
		secondaryWeapon = null;
		laserSightRef = GetComponent<LineRenderer> ();
		// contextNotify = transform.parent.Find("Camera/PlayerUI/MidMsg").GetComponent<FadeMessage>();
	}
	// Use this for initialization
	void Start () {
		health = startingHealth;
		damaged = false;
		defaultWeapon.allegiance = team;
		collidingWithWeapon = false;
		repairing = false;
		dead = false;
		pickingUpWep = false;
		StartCoroutine (CheckForDebugTeamChange ());
	}

	IEnumerator CheckForDebugTeamChange(){
		while (true) {
			yield return new WaitForSeconds (2f); // so it's not super sensitive
			if (control.yButtonDown && control.aButtonDown && control.dpadUp)
				DebugTeamChange ();
		}
	}

	void DebugTeamChange(){
		if (team == Faction_e.spaceCop) {
			team = Faction_e.spaceCrim;
			gameObject.layer = Utils.CrimLayer();
			GetComponent<Renderer>().material = crimColor;
			defaultWeapon.allegiance = Faction_e.spaceCrim;
			if (secondaryWeapon != null)
				secondaryWeapon.allegiance = Faction_e.spaceCrim;
		} else {
			team = Faction_e.spaceCop;
			gameObject.layer = Utils.CopLayer();
			GetComponent<Renderer>().material = copColor;
			defaultWeapon.allegiance = Faction_e.spaceCop;
			if (secondaryWeapon != null)
				secondaryWeapon.allegiance = Faction_e.spaceCop;
		}
	}

	void Update () {
//		if (repairing) {
//			return; // don't allow shooting if repairing
//		}
		// maybe this should be handled in PlayerControl

		if (secondaryWeapon != null)
			secondaryWeapon.Shoot (control.triggerDown, control.triggerPressed);
		else
			defaultWeapon.Shoot (control.triggerDown, control.triggerPressed);
		if (control.bButtonDown) {
			if (secondaryWeapon != null)
				secondaryWeapon.Reload();
			else
				defaultWeapon.Reload();
		}
		if (control.LtriggerPressed) { // toggle laser sight
			laserSightRef.enabled = !laserSightRef.enabled;
		}
	}
	
	void OnTriggerEnter(Collider coll) {
		Weapon w = coll.GetComponent<Weapon>();
		if(w) {
			print("player collided with " + w.weapName);
		}
	}
	
	void OnTriggerStay(Collider coll){
		if (coll.tag == "WeaponPickup" && control.xButtonPressed){
			Weapon wepRef = coll.GetComponent<Weapon>();
			if (!collidingWithWeapon && !pickingUpWep){// so we don't start the coroutine a bunch of times
				if (wepRef.allegiance == team || wepRef.allegiance == Faction_e.neutral){ 
					PickUpWeapon(coll.gameObject);
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

	void PickUpWeapon(GameObject item){
		collidingWithWeapon = true;
		pickingUpWep = true;
	
		if (item != null && collidingWithWeapon) {
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
			
			pickup.cam = myCam.GetComponent<FollowObject>();
			

		}
		collidingWithWeapon = false;
		pickingUpWep = false;
	}

	void LateUpdate(){
		if (damageTaken > 0) {
			health -= damageTaken;
			damageTaken = 0;
		}
		if (health <= 0 && !dead) {
			dead = true;
			Death ();
		}
	}

	public void TakeHit(int dmg){

		damageTaken += dmg;
		if (!damaged)
			StartCoroutine (DamageAnimation ());
	}


	IEnumerator DamageAnimation(){
		damaged = true;
		float startTime = Time.time;
		Renderer renderRef = GetComponent<Renderer> ();
		Material defaultMat = renderRef.material;
		while (Time.time - startTime < damageAnimDur) {
			if (renderRef.material == defaultMat)
				renderRef.material = damageMat;
			else
				renderRef.material = defaultMat;
			yield return new WaitForSeconds(0.05f);
		}
		renderRef.material = defaultMat;
		damaged = false;

	}

	void Death(){
		GetComponent<Rigidbody>().velocity = Vector3.zero;
		control.enabled = false;

		GetComponent<laser_sights> ().enabled = false; // code from laser_sights.cs could probably just be here
		laserSightRef.enabled = false;

		GetComponent<Renderer>().enabled = false;
		if (secondaryWeapon != null) {
			// drop secondary weapon
			secondaryWeapon.transform.parent = null;
			secondaryWeapon.tag = "WeaponPickup";
			secondaryWeapon.allegiance = Faction_e.neutral;
			secondaryWeapon = null;
			defaultWeapon.enabled = true;
		}


		defaultWeapon.GetComponent<Renderer>().enabled = false;

		control.AllButtonsOff ();

		defaultWeapon.GetComponent<Renderer>().enabled = false;
		// rather than turning off the collider, move the dead player to some faraway place
		Vector3 offscreen = new Vector3 (transform.position.x, -500f, transform.position.z);
		transform.position = offscreen;
		StartCoroutine (DeathDelay (offscreen));
	}

	IEnumerator DeathDelay(Vector3 deadPos){
		// so that they can see that they died
		yield return new WaitForSeconds (1);
		Vector3 roomPos = new Vector3 (0f, -500f);
		// have camera look at currently contested keyRoom
		foreach (Control keyRoom in MatchManager.S.keyRooms) {
			if (!keyRoom.locked){
				roomPos.x = keyRoom.transform.position.x;
				roomPos.z = keyRoom.transform.position.z;
				break;
			}		
		}
		// lerp both cam position and zoomage to the key location
		Camera camRef = myCam.GetComponentInChildren<Camera> ();
		camRef.orthographicSize = zoomedOutOrthoSize;
		transform.position = roomPos;
		float startTime = Time.time;
		while (Time.time - startTime < 2f) {
			float frac = (Time.time - startTime) / 2f;
			transform.position = Vector3.Lerp(deadPos, roomPos, frac);
			camRef.orthographicSize = Mathf.Lerp(defaultOrthoSize, zoomedOutOrthoSize, frac);
			yield return null;
		}
		// hang out at the location of interest
		yield return new WaitForSeconds(2);
		Reset ();
	}

	void Reset(){
		dead = false;
		// move to spawn point
		if (team == Faction_e.spaceCop) {
			transform.position = MatchManager.S.GetCopSpawnPoint().position;
		} else if (team == Faction_e.spaceCrim) {
			transform.position = MatchManager.S.GetCrimSpawnPoint().position;
		}
		// revert camera back to original size
		myCam.GetComponentInChildren<Camera> ().orthographicSize = defaultOrthoSize;
		// turn everything back on
		if (control.inDevice != null)
			control.enabled = true;

		
		GetComponent<laser_sights> ().enabled = true;
		GetComponent<Collider>().enabled = true;
		GetComponent<Renderer>().enabled = true;
		defaultWeapon.GetComponent<Renderer>().enabled = true;
		defaultWeapon.clip = defaultWeapon.clip_size;



		//defaultWeapon.canShoot = true;

		health = startingHealth;

	}


}
