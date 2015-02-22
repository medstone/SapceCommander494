using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public bool canShoot;
	public int startingAmmo = 1;
	protected int ammunition;

	public GameObject projectilePrefab;
	protected GameObject projectile;

	protected int damage;

	public Transform owner; // holder of the weapon.

	
	public float rateOfFire;

	public void Shoot(){
		if (canShoot && ammunition > 0)
			StartCoroutine (ShotTimer ());
	}

	// default behavior
	protected virtual void ShotBehavior(){
		// make a projectile 
		projectile = Instantiate (projectilePrefab) as GameObject;
		Projectile pro = projectile.GetComponent<Projectile> ();
		pro.transform.position = transform.position;
		pro.transform.rotation = transform.rotation;
		pro.bearing = transform.position - owner.transform.position;
		pro.bearing.Normalize ();
		pro.damage = damage;
		// this would need to be more intense to eliminate all friendly-fire
		// doing so via layers might be necessary?
		Physics.IgnoreCollision (pro.collider, this.collider);
		// give it a bearing as is appropriate for the way the weapon is facing.
	}


	void Awake () {
		canShoot = true;
	}

	protected virtual void Start(){
		ammunition = startingAmmo;
		damage = 2;
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
