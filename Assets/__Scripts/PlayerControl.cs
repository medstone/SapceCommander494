using UnityEngine;
using System.Collections;
using InControl;


public class PlayerControl : MonoBehaviour {

	public InputDevice inDevice; 
	
	public float rotationAmount;
	public float moveSpeed;
	public Vector3 bearing;



	public bool triggerDown;
	public bool triggerPressed; //differentiate between the two
	public bool xButtonDown;
	public bool aButtonDown; // as in the A button
	public bool bButtonDown;
	public bool yButtonDown; 
	public bool dpadUp;


	int wallLayerMask; 

	 
	// Use this for initialization
	void Start () {
		inDevice = Controller_distributor.S.GetController();
		if (inDevice == null) {
			Debug.Log ("Couldn't connect to a controller!");
			this.enabled = false;
		} else {
			inDevice.LeftStickX.LowerDeadZone = .5f;
			inDevice.LeftStickY.LowerDeadZone = .5f;
			inDevice.RightStickX.LowerDeadZone = .8f;
			inDevice.RightStickY.LowerDeadZone = .8f;
		}
		wallLayerMask = 1 << LayerMask.NameToLayer("Wall");
	}
	
	// Update is called once per frame
	void Update () {
		// handle L stick input
		bearing.x = inDevice.LeftStick.Vector.x;
		bearing.z = inDevice.LeftStick.Vector.y;
		


		// trigger input
		triggerDown = (inDevice.RightBumper || inDevice.RightTrigger);
		triggerPressed = (inDevice.RightBumper.WasPressed || inDevice.RightTrigger.WasPressed);

		xButtonDown = inDevice.Action3;
		aButtonDown = inDevice.Action1;
		bButtonDown = inDevice.Action2;
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
		Vector3 curBearing = bearing;
		RaycastHit hit;
		if (Physics.Raycast (transform.position, curBearing, out hit, 1.25f, wallLayerMask)) {
			//Vector3 temp = (Vector3.Cross(Vector3.Cross(hit.normal, curBearing.normalized), hit.normal)).normalized;
			//temp.Scale(bearing);
			//curBearing = temp;
			if (hit.normal == Vector3.right || hit.normal == Vector3.left){
				if (curBearing.z > 0){
					curBearing = new Vector3(0, 0, 1);
				}
				else {
					curBearing = new Vector3(0, 0, -1);
				}
			}
			else if (hit.normal == Vector3.forward || hit.normal == Vector3.back) { // normal is on z axis
				if (curBearing.x > 0){
					curBearing = new Vector3(1, 0, 0);
				}
				else
					curBearing = new Vector3(-1, 0, 0);
			}
		}
		GetComponent<Rigidbody>().velocity = (curBearing /*- transform.position*/).normalized * moveSpeed;
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
