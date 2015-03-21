﻿using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

	bool canShoot;
	public int startingAmmo = 1;
	protected int ammunition;
	public float shakeAmount = 0.05f;

	public GameObject projectilePrefab;
	protected GameObject projectile;
	public Faction_e allegiance;
	protected int damage;	
	public float rateOfFire;
	
	// used for shaking screen on fire
	public FollowObject cam;

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
