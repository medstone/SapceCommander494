using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : MonoBehaviour {
	static public MatchManager S;

	public Transform CopSpawnPoint;
	public Transform CrimSpawnPoint;

	List<Control> crimSpawnPoints;
	List<Control> copSpawnPoints;



	public GameObject[] rooms;
	
	Control steeringControl; 	// steering to see if criminals hacked it
	ProgressBar progress;  		// progress bar to see if time is up, and arrived at prison planet
	
	Text winnerText;

	void Awake(){
		S = this;
		copSpawnPoints = new List<Control> ();
		crimSpawnPoints = new List<Control> ();
	}

	// Use this for initialization
	void Start () {
		steeringControl = GameObject.Find("Steering").GetComponent<Control>();
		progress = GameObject.Find("ProgressBar").GetComponent<ProgressBar>();
		winnerText = GameObject.Find("WinnerText").GetComponent<Text>();

		Control[] rooms = GameObject.FindObjectsOfType<Control> ();
		foreach (Control room in rooms) {
			if (room.holds == Faction_e.spaceCop)
				copSpawnPoints.Add(room);
			else 
				crimSpawnPoints.Add (room);
		}
		SortCopSpawnPoints ();
		SortCrimSpawnPoints ();
		print (copSpawnPoints.Count); print (" cop points registered");
	}
	
	// Update is called once per frame
	void Update () {
		
//		// check if criminals hacked the steering
//		if(steeringControl.hacked) {
//			// Criminals WIN!!
//			winnerText.text = "Criminals Win!!";
//		}
//		// check if they arrived at the prison planet
//		else if(progress.ended) {
//			// Cops WIN!!
//			winnerText.text = "Cops Win!!";
//		}
	}

	// reorganizes the spawn point lists to reflect room's capture by newAllegiance
	public void CapturedSpawnPoint(Control room){
		print ("trynna rearrange spawn points");
		if (room.holds == Faction_e.spaceCop) {
			Control roomRef = copSpawnPoints.Find(room.Equals);
			copSpawnPoints.Remove (room);
			crimSpawnPoints.Add(roomRef);
			SortCrimSpawnPoints();
			print ("Added a crim spawn point");
		} else {
			Control roomRef = crimSpawnPoints.Find (room.Equals);
			crimSpawnPoints.Remove (room);
			copSpawnPoints.Add(roomRef);
			SortCopSpawnPoints();
		}
	}
	
	public Transform GetCopSpawnPoint(){
		if (copSpawnPoints.Count <= 0)
			return CopSpawnPoint;
		return copSpawnPoints [0].transform;
	}
	
	public Transform GetCrimSpawnPoint(){
		if (crimSpawnPoints.Count <= 0)
			return CrimSpawnPoint; // default point
		return crimSpawnPoints [crimSpawnPoints.Count - 1].transform;
	}

	void SortCrimSpawnPoints(){
		crimSpawnPoints.Sort ((a, b) => a.transform.position.x.CompareTo (b.transform.position.x));
	}

	void SortCopSpawnPoints(){
		// not sure what the best sorting would be for cops, or if their list should just be combed
		// whenever the cop spawns.
	}
}
