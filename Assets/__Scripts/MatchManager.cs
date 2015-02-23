using UnityEngine;
using System.Collections;

public class MatchManager : MonoBehaviour {
	static public MatchManager S;

	public Transform CopSpawnPoint;
	public Transform CrimSpawnPoint;

	public GameObject[] rooms;
	
	Control steeringControl; 	// steering to see if criminals hacked it
	ProgressBar progress;  		// progress bar to see if time is up, and arrived at prison planet
	

	void Awake(){
		S = this;
	}

	// Use this for initialization
	void Start () {
		steeringControl = GameObject.Find("Steering").GetComponent<Control>();
		progress = GameObject.Find("ProgressBar").GetComponent<ProgressBar>();
	}
	
	// Update is called once per frame
	void Update () {
		
		// check if criminals hacked the steering
		if(steeringControl.hacked) {
			// Criminals WIN!!
		}
		// check if they arrived at the prison planet
		else if(progress.ended) {
			// Cops WIN!!
		}
	}
}
