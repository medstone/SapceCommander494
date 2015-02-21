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

	public int startingHealth;
	public int health;

	int damageTaken = 0; 
	bool invincible;
	public float invincibleDur; // how long does the player ignore damage after taking a hit

	public float rateOfFire;
	bool canShoot;

	public GameObject projectilePrefab;
	GameObject projectile;

	void Awake(){
		control = GetComponent<PlayerControl> ();
	}
	// Use this for initialization
	void Start () {
		health = startingHealth;
		invincible = false;
		canShoot = true;
	}

	//*** not sure if this should be update
	void FixedUpdate () {
		if (control.triggerDown && canShoot)
			StartCoroutine (ShotTimer ());
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
		control.childSphere.renderer.enabled = false;
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
		control.childSphere.renderer.enabled = true;
		health = startingHealth;
	}

	IEnumerator ShotTimer(){
		float startTime = Time.time;
		canShoot = false;
		// make a projectile 
		projectile = Instantiate (projectilePrefab) as GameObject;
		Projectile pro = projectile.GetComponent<Projectile> ();
		pro.transform.position = control.childSphere.transform.position;
		pro.transform.rotation = control.childSphere.transform.rotation;
		pro.bearing = control.childSphere.transform.position - transform.position;
		pro.bearing.Normalize ();
		// this would need to be more intense to eliminate all friendly-fire
		// doing so via layers might be necessary?
		Physics.IgnoreCollision (pro.collider, this.collider);
		// give it a bearing as is appropriate for the way the weapon is facing.
		
		while (Time.time - startTime < rateOfFire) {
			yield return null;
		}
		canShoot = true;
	}
}
