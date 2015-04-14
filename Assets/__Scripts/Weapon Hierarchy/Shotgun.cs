using UnityEngine;
using System.Collections;


// there will probably end up being a secondary weapon base class
public class Shotgun : Weapon {

	bool delaying = false;

	public float shotAngle;

	protected override void Start(){
		damage = 4;
		ammunition = startingAmmo;
		clip = clip_size;
	}

	void FixedUpdate(){
		if (ammunition <= 0 && !delaying) {
			StartCoroutine(DestroyWeaponDelay());
		}
	}

	IEnumerator DestroyWeaponDelay(){
		delaying = true;
		yield return new WaitForSeconds (0.25f);
		PlayerStats playerRef = GetComponentInParent<PlayerStats>();
		playerRef.defaultWeapon.enabled = true;
		playerRef.secondaryWeapon = null;
		Destroy (this.gameObject);
	}

	protected override void ShotBehavior(){
		// make three projectiles
		// first projectile (straight) is just base class shot
		base.ShotBehavior ();
		ShotHelper (shotAngle);
		ShotHelper (shotAngle * -1f);
		--ammunition;
	}
}
