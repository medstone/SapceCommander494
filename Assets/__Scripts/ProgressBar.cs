using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour {

	
	public bool isRunning = true;
	public float matchTime = 25f;
	float currentTime;
	public bool ended = false;
	public bool timeRanOut = false;

	Transform startPos;
	Transform endPos;
	Transform rocket;
	Vector3 travelVec = Vector3.zero;
	Text durationText;
	
	
	// Use this for initialization
	void Awake () {
		startPos = transform.Find("Start");
		endPos = transform.Find("End");
		rocket = transform.Find("Rocket");
		durationText = transform.GetComponentInChildren<Text>();
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
			float timeLeft = matchTime - currentTime;
			if(timeLeft < 0) timeLeft = 0;
			string mins = Mathf.Floor(timeLeft / 60).ToString("0");
 			string secs = Mathf.Floor(timeLeft % 60).ToString("00");
 			durationText.text = mins + ":" + secs;
		
			// determine new position for the rocket
			float completed = currentTime / matchTime;
			rocket.position = startPos.position + travelVec * completed;
			
			if(rocket.position.x >= endPos.position.x) {
				// alertTimeUp(); - Not implemented yet
				MatchManager.S.TimeRanOut();
				isRunning = false;
				timeRanOut = true;
				ended = true;
			}
		}
	}
}
