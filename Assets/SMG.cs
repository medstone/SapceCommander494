using UnityEngine;
using System.Collections;

public class SMG : Weapon {

	bool delaying = false;

	// Use this for initialization
	protected override void Start () {
		fireBasedOnTriggerPress = false;
		infiniteAmmo = false;
		damage = 1;
		ammunition = startingAmmo;
		clip = clip_size;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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

		--ammunition;
	}
}
