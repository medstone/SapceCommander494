using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour {
	
	Transform open;
	Transform close;
	Transform door;
	
	public float doorSpeed = 1f;
	public float margin = .1f;
	public bool isClosing = false;
	
	// Use this for initialization
	void Awake () {
		open = transform.Find("OpenPos");
		close = transform.Find("ClosedPos");
		door = transform.Find("Door");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider coll) {
		if(coll.tag != "Actor" && coll.tag != "Robot") return;
		
		StartCoroutine(OpenDoor());
	}
	
	IEnumerator OpenDoor () {
		GetComponent<Collider>().enabled = false;
		isClosing = false;
		
		while (true) {
			
			// Move door towards open position
			float step = doorSpeed * Time.deltaTime;
			door.position = Vector3.MoveTowards(door.position, open.position, step);
			
			// Check if door is sufficiently close enough to open position
			float diff = Mathf.Abs((door.position - open.position).magnitude);
			if(diff < margin) {
				break;
			}
			
			yield return null;
		}
		
		yield return new WaitForSeconds(2f);
		StartCoroutine(CloseDoor());
	}
	
	IEnumerator CloseDoor () {
		
		GetComponent<Collider>().enabled = true;
		isClosing = true;
		while (isClosing) {
			
			// Move door towards open position
			float step = doorSpeed * Time.deltaTime;
			door.position = Vector3.MoveTowards(door.position, close.position, step);
			
			// Check if door is sufficiently close enough to close position
			float diff = Mathf.Abs((door.position - close.position).magnitude);
			if(diff < margin) {
				isClosing = false;
				break;
			}
			
			yield return null;
		}
		
	}
}
