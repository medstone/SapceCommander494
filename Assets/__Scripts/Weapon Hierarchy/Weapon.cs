using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public bool canShoot;
	public int startingAmmo = 1;
	protected int ammunition;

	public GameObject projectilePrefab;
	protected GameObject projectile;
	public Faction_e allegiance;
	protected int damage;	
	public float rateOfFire;
	
	// used for shaking screen on fire
	FollowObject cam;

	public void Shoot(){
		if (canShoot && ammunition > 0)
			StartCoroutine (ShotTimer ());
	}

	// default behavior
	protected virtual void ShotBehavior(){
		ShotHelper (0f); // straight shot
	}


	void Awake () {
		canShoot = true;
	}

	protected virtual void Start(){
		ammunition = startingAmmo;
		damage = 2;
		
		cam = GameObject.Find("Camera1").GetComponent<FollowObject>();
	}

	protected void ShotHelper(float angle){
		projectile = Instantiate (projectilePrefab) as GameObject;
		
		Projectile pro = projectile.GetComponent<Projectile> (); // needed to adjust projectile's bearing
		pro.transform.position = transform.position;
		pro.transform.rotation = transform.rotation;
		
		Vector3 offset = transform.parent.position;
		offset = Quaternion.Euler (0, angle, 0) * offset;
		
		pro.bearing = transform.position - offset;
		pro.bearing.Normalize ();
		pro.damage = damage;
		
		// shake screen
		if(cam) {
			cam.startShaking();
		}
		
		// Just don't have a collider...
		Physics.IgnoreCollision (pro.GetComponent<Collider>(), this.GetComponentInParent<Collider>());
	}

	IEnumerator ShotTimer(){
		float startTime = Time.time;
		canShoot = false;
		ShotBehavior ();
		
		while (Time.time - startTime < rateOfFire) {
			yield return null;
		}
		canShoot = true;
	}
}
