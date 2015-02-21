using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	public bool canShoot;
	public int startingAmmo = 1;
	int ammunition;

	public GameObject projectilePrefab;
	GameObject projectile;

	public Transform owner; // holder of the weapon.

	
	public float rateOfFire;

	public void Shoot(){
		if (canShoot && ammunition > 0)
			StartCoroutine (ShotTimer ());
	}

	protected virtual void ShotBehavior(){
		// make a projectile 
		projectile = Instantiate (projectilePrefab) as GameObject;
		Projectile pro = projectile.GetComponent<Projectile> ();
		pro.transform.position = transform.position;
		pro.transform.rotation = transform.rotation;
		pro.bearing = transform.position - owner.transform.position;
		pro.bearing.Normalize ();
		// this would need to be more intense to eliminate all friendly-fire
		// doing so via layers might be necessary?
		Physics.IgnoreCollision (pro.collider, this.collider);
		// give it a bearing as is appropriate for the way the weapon is facing.
	}


	void Awake () {
		canShoot = true;
	}

	void Start(){
		ammunition = startingAmmo;
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
