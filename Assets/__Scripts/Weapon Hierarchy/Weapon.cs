using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	
	public string weapName = "Pistol";
	
	bool canShoot;
	public int startingAmmo = 1;
	public int ammunition;
	public float shakeAmount = 0.05f;

	public GameObject projectilePrefab;
	protected GameObject projectile;
	public Faction_e allegiance;
	protected int damage;	
	public float rateOfFire;


	public AudioSource aud;

	public int clip_size = 10;
	public int clip;
	public float reload_time = 1.0f;

	public bool reloading = false;

	protected bool fireBasedOnTriggerPress = true; // requires unique trigger pull to fire (can't hold it down)
	protected bool infiniteAmmo = true;

	// used for shaking screen on fire
	public FollowObject cam;

	public void Shoot(bool triggerDown, bool triggerPressed){
		if (ammunition <= 0 || clip <= 0 || !canShoot)
						return;

		if (fireBasedOnTriggerPress) {
			if (triggerPressed)
				StartCoroutine (ShotTimer ());	
		} 			
		else {
			if (triggerDown)
				StartCoroutine(ShotTimer ());
		}
		
	}

	// default behavior
	protected virtual void ShotBehavior(){
		ShotHelper (0f); // straight shot
		--clip;

	}

	// player making request to reload
	public void Reload(){
		if (reloading || !canShoot) return; // coroutine guard
		if (clip == clip_size) return; // full clip - cant reload
		if (clip == ammunition && !infiniteAmmo)return; // all remaining ammo is in clip
		StartCoroutine (ReloadRoutine ());
	}

	IEnumerator ReloadRoutine(){
		float startTime = Time.time;
		canShoot = false;
		reloading = true;
		while (Time.time - startTime < reload_time) {
			yield return null;
		}
		if (ammunition >= clip_size || infiniteAmmo) {
			clip = clip_size;
		} 
		else {
			clip = ammunition;
		}
		canShoot = true;
		reloading = false;
	}


	void Awake () {
		canShoot = true;
		aud = this.gameObject.GetComponent<AudioSource> ();
	}

	protected virtual void Start(){
		ammunition = startingAmmo;
		damage = 1;
		clip = clip_size;
		
		if(transform.parent) {
			cam = transform.parent.GetComponent<PlayerStats>().myCam.GetComponent<FollowObject>();
			if(!cam) {
				print("WEAPON: failed to find a FollowObject script on parent");
			}
		}
	}



	protected void ShotHelper(float angle){
		projectile = Instantiate (projectilePrefab) as GameObject;

		
		Projectile pro = projectile.GetComponent<Projectile> (); // needed to adjust projectile's bearing
		pro.transform.position = transform.position;
		pro.transform.rotation = transform.rotation;

		if (allegiance == Faction_e.spaceCop) {
			projectile.layer = Utils.CopProjectileLayer ();
			//pro.layermask = 1 << LayerMask.NameToLayer("CopProjectile");
			//pro.layermask += 1 << LayerMask.NameToLayer("Cops");
			pro.IgnoreLayer(LayerMask.NameToLayer("CopProjectile"));
			pro.IgnoreLayer(LayerMask.NameToLayer("Cops"));

		} 
		else if (allegiance == Faction_e.spaceCrim) {
			projectile.layer = Utils.CrimProjectileLayer ();
			//pro.layermask = 1 << LayerMask.NameToLayer("CrimProjectile");
			pro.IgnoreLayer(LayerMask.NameToLayer("CrimProjectile"));
			//pro.layermask += 1 << LayerMask.NameToLayer("Crims");
			pro.IgnoreLayer(LayerMask.NameToLayer("Crims"));
		}
		//pro.layermask = ~pro.layermask;
		
		Vector3 offset =  transform.position - GetComponent<Transform>().parent.position;
		offset = Quaternion.Euler (0, angle, 0) * offset;

		pro.SetBearing (offset);
		//pro.SetBearing(transform.position - offset);
		pro.damage = damage;
		

		// shake screen
		if(cam) {
			cam.startShaking(shakeAmount);
		}
		aud.Play ();
	}

	IEnumerator ShotTimer(){
		float startTime = Time.time;
		canShoot = false;
		ShotBehavior ();
		
		while (Time.time - startTime < rateOfFire) {
			yield return null;
		}
		canShoot = true;
		if (clip <= 0) {
			Reload();
		}
	}

	
	void OnTriggerEnter(Collider coll) {
		
		// pickupable
		if(transform.tag != "WeaponPickup") {
			return;
		}
		
		// coll is a player of same faction
		PlayerStats ps = coll.GetComponent<PlayerStats>();
		if(ps && (allegiance == Faction_e.neutral || allegiance == ps.team))
		{
			// don't bother if player already has a secondary
			if(ps.secondaryWeapon) {
				return;
			}
			
			// check if player cares about context
			ContextListener cl = coll.GetComponent<ContextListener>();
			if(cl) {
				print("Trying to pop from weapon");
				cl.PopDisplay("Tap \"X\" to pick up " + weapName);
			}
		}
	}
}
