using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	
	public bool isRunning = true;
	public float matchTime = 25f;
	float currentTime;

	Transform startPos;
	Transform endPos;
	Transform rocket;
	Vector3 travelVec = Vector3.zero;
	
	
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
		
		if(isRunning){
			// Update the time
			currentTime += Time.deltaTime;
		
			// determine new position for the rocket
			float completed = currentTime / matchTime;
			rocket.position = startPos.position + travelVec * completed;
			
			if(rocket.position.x >= endPos.position.x) {
				// alertTimeUp(); - Not implemented yet
				isRunning = false;
			}
		}
	}
	
	void FixedUpdate() {
		
		
	}
}
