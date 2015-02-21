using UnityEngine;
using System.Collections;
using InControl;

public class PlayerControl : MonoBehaviour {

	public InputDevice inDevice; 
	
	public GameObject childSphere; // at the moment, at least a transform is needed here to line up shots with
	
	public float rotationAmount;
	public float moveSpeed;
	public Vector3 bearing;





	public bool triggerDown;
	
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
	
	}
	
	// Update is called once per frame
	void Update () {
		// handle L stick input
		bearing.x = inDevice.LeftStick.Vector.x;
		bearing.z = inDevice.LeftStick.Vector.y;
		


		// trigger input
		triggerDown = inDevice.RightTrigger;


		// handle R stick rotation
		float yIn = inDevice.RightStickY;
		float xIn = inDevice.RightStickX;
		if (xIn != 0f || yIn != 0f) {
			float angle = Mathf.Atan2 (yIn, xIn) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.AngleAxis (90.0f - angle, Vector3.up), Time.deltaTime * rotationAmount);
		}

	}

	void FixedUpdate(){
		rigidbody.velocity = (bearing /*- transform.position*/).normalized * moveSpeed;
	}



	void OnTriggerEnter(Collider coll){
		if (coll.tag == "Wall")
			StopMoving ();
	}
	


	void StopMoving(){
		rigidbody.velocity = Vector3.zero;
	}
}
