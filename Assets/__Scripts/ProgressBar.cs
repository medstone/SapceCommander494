using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProgressBar : MonoBehaviour {

	
	public bool isRunning = true;
	public float matchTime = 25f;
	float currentTime;
	public bool ended = false;
	public bool timeRanOut = false;

	Transform startPos;
	Transform endPos;
	Transform rocket;
	Image rocketImg;
	public List<Vector2> TimeLeftAndInterval;
	float flashTime = .4f;
	Vector3 travelVec = Vector3.zero;
	Text durationText;
	
	// Use this for initialization
	void Awake () {
		startPos = transform.Find("Start");
		endPos = transform.Find("End");
		rocket = transform.Find("Rocket");
		rocketImg = transform.Find("Rocket").GetComponent<Image>();
		durationText = transform.GetComponentInChildren<Text>();
	}
	
	void Start () {
		
		// reset the clock
		currentTime = 0f;
		
		// calculate travelPath
		travelVec = endPos.position - startPos.position;
		
		// coroutine starts when there are  x seconds remaining
		// blink happens every y seconds
		foreach(Vector2 times in TimeLeftAndInterval) {
			StartCoroutine(DelayAndFlashOnInterval(Mathf.Clamp(matchTime - times.x, 0f, matchTime), times.y));
			print("added " + Mathf.Clamp(matchTime - times.x, 0f, matchTime) + " " + times.y);
		}
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
			// rocket.position = startPos.position + travelVec * completed;
			
			if(timeLeft == 0f) {
				// alertTimeUp(); - Not implemented yet
				MatchManager.S.TimeRanOut();
				isRunning = false;
				timeRanOut = true;
				ended = true;
			}
		}
	}
	
	IEnumerator SingleFlash(float delayTime) {
		// Kindly wait
		yield return new WaitForSeconds(delayTime);
		if(ended) yield break;
		
		// Play a sound here if you'd like
		// SOUNDPLAYING
		
		
		// fade in the red
		float startTime = Time.time;
		while((startTime + (flashTime/2)) > Time.time) {
			// amount is percent it should be one color or the other
			float amount = (Time.time - startTime) / (flashTime/2);
			print(amount);
			rocketImg.color = Color.Lerp(Color.white, Color.red, amount);
			yield return null;
		}
		
		// fade it out - same as above but lerpin in reverse
		startTime = Time.time;
		while((startTime + (flashTime/2)) > Time.time) {
			float amount = (Time.time - startTime) / (flashTime/2);
			rocketImg.color = Color.Lerp(Color.red, Color.white, amount);
			yield return null;
		}
	}
	
	// This function starts a blink
	// SingleFlash routine takes less than a second to run so it's kewl
	IEnumerator DelayAndFlashOnInterval(float delayTime, float interval) {
		yield return new WaitForSeconds(delayTime);
		
		// repeat if match has not ended
		// and there is enough time left to to another flash
		float count = Time.time;
		while(!ended && (matchTime - currentTime) > interval) {
			
			count += Time.deltaTime;
			
			// flash every second
			if(count > interval){
				StartCoroutine(SingleFlash(0f));
				count = 0f;
			}
			
			yield return null;
		}
	}
}
