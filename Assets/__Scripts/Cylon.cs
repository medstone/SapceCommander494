using UnityEngine;
using System.Collections;

/* Okay so this borrows a lot from the robotAI code but broken up into more functions + 
 the navigation choice-making is greatly simplified. */

public class Cylon : MonoBehaviour {
	public Faction_e faction;
	public Vector3 direction;
	public float speed = 5f;
	public float range = 10f; // how far can the robot shoot?
	public GameObject projectilePrefab;
	public float shotDelay = 1f; // rate of fire
	public robotSpawn spawnerRef; // needed to inform spawner of death.
	int raylayer;
	bool canShoot;
	bool stopped;

	int shotCheckRaylayer; // used to raycast to check if a wall is blocking a shot

	// Use this for initialization
	void Start () {
		canShoot = true;
		if (faction == Faction_e.spaceCop) {
			gameObject.layer = Utils.CopLayer ();
			raylayer = 1 << LayerMask.NameToLayer("CrimBarrier");
		} else if (faction == Faction_e.spaceCrim) {
			gameObject.layer = Utils.CrimLayer ();
			raylayer = 1 << LayerMask.NameToLayer("CopBarrier");
		}
		raylayer += 1 << LayerMask.NameToLayer ("Wall");

		shotCheckRaylayer = raylayer;
		if (faction == Faction_e.spaceCop) {
			shotCheckRaylayer += 1 << LayerMask.NameToLayer ("Crims");
			shotCheckRaylayer += 1 << LayerMask.NameToLayer("CopBarrier"); // all barriers block shots
		} else {
			shotCheckRaylayer += 1 << LayerMask.NameToLayer ("Cops");
			shotCheckRaylayer += 1 << LayerMask.NameToLayer("CrimBarrier");
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (canShoot) { 
			FindTargets();
		}
		if (stopped) { // stopped is set when a robot is laying the hurt on someone
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
		} 
		else {
			UpdateFacing();
			GetComponent<Rigidbody> ().velocity = direction.normalized * speed;
		}
	}

	// look around the robot, find all potential targets, ignore those that have walls or barriers in the way
	void FindTargets(){
		Collider[] colls = Physics.OverlapSphere(this.transform.position, range);
		foreach(Collider targ in colls){
			if ((targ.gameObject.tag == "Actor" && faction != targ.GetComponent<PlayerStats>().team) || 
			(targ.gameObject.tag == "Robot" && faction != targ.GetComponent<Cylon>().faction)){
				if (!IsTargHittable(targ)) continue;
				if (canShoot)
					StartCoroutine(ShotTimer (targ));
				break;
			}
			else
				continue;
		}
	}
	// determines if there is a wall or barrier in between the robot and a potential target
	bool IsTargHittable(Collider targ){
		RaycastHit rayHit;
		if (Physics.Raycast (transform.position, targ.transform.position - transform.position, out rayHit, range, shotCheckRaylayer)) {
			if (rayHit.collider.gameObject.tag == targ.gameObject.tag)
				return true;
			return false; // hit a wall or a barrier or something instead
		}
		return false; // somehow didn't hit anything
	}

	// timing mechanism for robot's shooting. also sets a toggle that stops the robot while it is shooting
	IEnumerator ShotTimer(Collider targ){
		float startTime = Time.time;
		canShoot = false;
		stopped = true;
		Shoot (targ);
		while (Time.time - startTime < stopDuration) {
			yield return null;
			// this presumes that stopduration is less than shotdelay
		}
		stopped = false;
		while (Time.time - startTime < shotDelay) {
			yield return null;
		}
		canShoot = true;
	}

	// actually deals with the projectile, should only be called by ShotTimer
	void Shoot(Collider targ){
		GameObject proj = Instantiate(projectilePrefab) as GameObject;
		proj.layer = gameObject.layer;
		proj.gameObject.transform.position = transform.position;
		proj.GetComponent<Projectile> ().damage = 1;
		proj.GetComponent<Projectile>().SetBearing(new Vector3((
			targ.transform.position.x - transform.position.x)/3f,(targ.transform.position.y - transform.position.y)/3f,(targ.transform.position.z - transform.position.z)/3f));
	}

	// figure out if the robot needs to alter it's course
	void UpdateFacing(){
		RaycastHit rayHit;
		if (Physics.Raycast (transform.position, direction, out rayHit, 1f, raylayer)) { // close to a wall?
			// make a choice of which way to go
			float rand = Random.Range(0f, 1f);
			if (rand < 0.5f){
				CheckRight();
				CheckLeft ();
			}
			else {
				CheckLeft ();
				CheckRight ();
			}
		}
	}

	void CheckRight(){
		RaycastHit rayHit;
		if (!Physics.Raycast(transform.position, Quaternion.Euler(0f, 90f, 0f) * direction, out rayHit, 1f, raylayer)){
			// clockwise is clear
			direction = Quaternion.Euler(0f, 90f, 0f) * direction;
		}
	}

	void CheckLeft(){
		RaycastHit rayHit;
		if (!Physics.Raycast(transform.position, Quaternion.Euler(0f, -90f, 0f) * direction, out rayHit, 1f, raylayer)){
			direction = Quaternion.Euler(0f, -90f, 0f) * direction;
		}
	}

	void OnDestroy(){
		--spawnerRef.numSpawned;
	}
}
