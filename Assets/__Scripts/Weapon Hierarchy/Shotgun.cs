using UnityEngine;
using System.Collections;

public class Shotgun : Weapon {

	protected override void Start(){
		damage = 4;
		ammunition = startingAmmo;
	}

	protected override void ShotBehavior(){
		// make three projectiles

		// first projectile (straight) is just base class shot
		base.ShotBehavior ();

		// shot 2
		projectile = Instantiate (projectilePrefab) as GameObject;

		Projectile pro = projectile.GetComponent<Projectile> (); // needed to adjust projectile's bearing
		pro.transform.position = transform.position;
		pro.transform.rotation = transform.rotation;

		Vector3 offset = owner.transform.position;
		offset = Quaternion.Euler (0, -1, 0) * offset;

		pro.bearing = transform.position - offset;
		pro.bearing.Normalize ();
		pro.damage = damage;

		Physics.IgnoreCollision (pro.collider, this.collider);

		// shot 3 - blatant copy-paste
		projectile = Instantiate (projectilePrefab) as GameObject;
		
		pro = projectile.GetComponent<Projectile> (); // needed to adjust projectile's bearing
		pro.transform.position = transform.position;
		pro.transform.rotation = transform.rotation;
		
		offset = owner.transform.position;
		offset = Quaternion.Euler (0, 1, 0) * offset;
		
		pro.bearing = transform.position - offset;
		pro.bearing.Normalize ();
		pro.damage = damage;
		
		Physics.IgnoreCollision (pro.collider, this.collider);

		--ammunition;
	}
}
