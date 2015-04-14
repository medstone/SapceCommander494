using UnityEngine;
using System.Collections;
using InControl;


public class PlayerControl : MonoBehaviour {

	public InputDevice inDevice; 
	
	public float rotationAmount;
	public float moveSpeed;
	public Vector3 bearing;



	public bool triggerDown;
	public bool xButtonDown;
	public bool aButtonDown; // as in the A button
	public bool yButtonDown; 
	public bool dpadUp;
	
	void Awake(){

	}
	 
	// Use this for initialization
	void Start () {
		inDevice = Controller_distributor.S.GetController();
		if (inDevice == null) {
			Debug.Log ("Couldn't connect to a controller!");
			this.enabled = false;
		}
		triggerDown = false;
		inDevice.LeftStickX.LowerDeadZone = .5f;
		inDevice.LeftStickY.LowerDeadZone = .5f;
	}
	
	// Update is called once per frame
	void Update () {
		// handle L stick input
		bearing.x = inDevice.LeftStick.Vector.x;
		bearing.z = inDevice.LeftStick.Vector.y;
		


		// trigger input
		triggerDown = (inDevice.RightBumper || inDevice.RightTrigger);

		xButtonDown = inDevice.Action3;
		aButtonDown = inDevice.Action1;

		yButtonDown = inDevice.Action4;

		dpadUp = inDevice.DPadUp;


		// handle R stick rotation
		float yIn = inDevice.RightStickY;
		float xIn = inDevice.RightStickX;
		if (xIn != 0f || yIn != 0f) {
			float angle = Mathf.Atan2 (yIn, xIn) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler (0f, 90f - angle, 0f);
				//Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (90.0f - angle, Vector3.up), Time.deltaTime * rotationAmount);
		}

	}

	void FixedUpdate(){
		GetComponent<Rigidbody>().velocity = (bearing /*- transform.position*/).normalized * moveSpeed;
	}


	public void AllButtonsOff(){
		triggerDown = false;
		aButtonDown = false;
		xButtonDown = false;
		yButtonDown = false;
	}
	

	void OnTriggerEnter(Collider coll){
		if (coll.tag == "Wall")
			StopMoving ();
	}
	


	void StopMoving(){
		Vector3 vel = GetComponent<Rigidbody> ().velocity;
		vel.x = 0f;
		vel.z = 0f;
		GetComponent<Rigidbody> ().velocity = vel;
	}
}
