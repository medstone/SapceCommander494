using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControl : MonoBehaviour {

	InputDevice theInputDevice; 
	
	//GameObject childSphere;
	
	public float rotationAmount;
	public float moveSpeed;
	
	
	public bool triggerDown;
	
	void Awake(){
		//childSphere = GameObject.Find ("lasergun");
	}
	
	// Use this for initialization
	void Start () {
		theInputDevice = InputManager.ActiveDevice;
		triggerDown = false;
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
		
		if (xIn == 0f && yIn == 0f)
			return;
		float angle = Mathf.Atan2 (yIn, xIn) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (90.0f - angle, Vector3.up), Time.deltaTime * rotationAmount);
		
		
		
		
	}
}
