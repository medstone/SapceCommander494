using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControl : MonoBehaviour {

	InputDevice theInputDevice; 
	
	public GameObject childSphere;
	
	public float rotationAmount;
	public float moveSpeed;

	public GameObject projectilePrefab;
	GameObject projectile;

	public float rateOfFire;
	bool canShoot;
	bool triggerDown;
	
	void Awake(){

	}
	
	// Use this for initialization
	void Start () {
		theInputDevice = InputManager.ActiveDevice;
		triggerDown = false;
		canShoot = true;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = transform.position;
		pos.x += Time.deltaTime * theInputDevice.LeftStickX * moveSpeed;
		pos.z += Time.deltaTime * theInputDevice.LeftStickY * moveSpeed; 
		transform.position = pos;
		
		float yIn = theInputDevice.RightStickY;
		float xIn = theInputDevice.RightStickX;
		
		triggerDown = theInputDevice.RightTrigger;
		if (triggerDown && canShoot)
			StartCoroutine (ShotTimer ());
		
		if (xIn == 0f && yIn == 0f)
			return;
		float angle = Mathf.Atan2 (yIn, xIn) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (90.0f - angle, Vector3.up), Time.deltaTime * rotationAmount);


	}

	IEnumerator ShotTimer(){
		float startTime = Time.time;
		canShoot = false;
		// make a projectile 
		projectile = Instantiate (projectilePrefab) as GameObject;
		Projectile pro = projectile.GetComponent<Projectile> ();
		pro.transform.position = childSphere.transform.position;
		pro.transform.rotation = childSphere.transform.rotation;
		pro.bearing = childSphere.transform.position - transform.position;
		pro.bearing.Normalize ();
		// give it a bearing as is appropriate for the way the weapon is facing.

		while (Time.time - startTime < rateOfFire) {
			yield return null;
		}
		canShoot = true;
	}
}
