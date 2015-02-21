using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	
	public bool isRunning = true;
	public float speed = 25f;

	Transform startPos;
	Transform endPos;
	Transform rocket;
	Vector3 travelVec = Vector3.zero;
	
	public float matchTime = 25f;
	float currentTime;
	
	// Use this for initialization
	void Awake () {
		startPos = transform.Find("Start");
		endPos = transform.Find("End");
		rocket = transform.Find("Rocket");
	}
	
	void Start () {
		// move rocket to Start
		rocket.position = startPos.position;
		
		// reset the clock
		currentTime = 0f;
		
		// calculate travelPath
		travelVec = endPos.position - startPos.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		// keep track of time that has passed
		if(isRunning){
			currentTime += Time.deltaTime;
		}
		// determine new position for the rocket
		float completed = currentTime / matchTime;
		rocket.position = startPos.position + travelVec * completed;
	}
	
	void FixedUpdate() {
		
		
	}
}
