using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : MonoBehaviour {
	static public MatchManager S;

	public Transform CopDefaultSpawnPoint;
	public Transform[] CrimDefaultSpawnPoint;

	List<CloneRoom> crimSpawnPoints;
	List<CloneRoom> copSpawnPoints;



	public GameObject[] rooms;
	
	Control steeringControl; 	// steering to see if criminals hacked it
	ProgressBar progress;  		// progress bar to see if time is up, and arrived at prison planet
	
	Text winnerText;

	void Awake(){
		S = this;
		copSpawnPoints = new List<CloneRoom> ();
		crimSpawnPoints = new List<CloneRoom> ();
	}

	// Use this for initialization
	void Start () {
		steeringControl = GameObject.Find("Steering").GetComponent<Control>();
		progress = GameObject.Find("ProgressBar").GetComponent<ProgressBar>();
		winnerText = GameObject.Find("WinnerText").GetComponent<Text>();

		CloneRoom[] rooms = GameObject.FindObjectsOfType<CloneRoom> ();
		foreach (CloneRoom room in rooms) {
			if (room.control.holds == Faction_e.spaceCop)
				copSpawnPoints.Add(room);
			else 
				crimSpawnPoints.Add (room);
		}
		SortCopSpawnPoints ();
		SortCrimSpawnPoints ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// check if criminals hacked the steering
		if(steeringControl.holds == Faction_e.spaceCrim) {
			// Criminals WIN!!
			winnerText.text = "Criminals Win!!";
		}
		// check if they arrived at the prison planet
		else if(progress.ended) {
			// Cops WIN!!
			winnerText.text = "Cops Win!!";
		}
	}

	// reorganizes the spawn point lists to reflect room's capture by newAllegiance
	public void CapturedSpawnPoint(CloneRoom room){
		if (room.control.holds == Faction_e.spaceCop) {
			CloneRoom roomRef = copSpawnPoints.Find(room.Equals);
			copSpawnPoints.Remove (room);
			crimSpawnPoints.Add(roomRef);
			SortCrimSpawnPoints();
		} else {
			CloneRoom roomRef = crimSpawnPoints.Find (room.Equals);
			crimSpawnPoints.Remove (room);
			copSpawnPoints.Add(roomRef);
			SortCopSpawnPoints();
		}
	}
	
	public Transform GetCopSpawnPoint(){
		if (copSpawnPoints.Count <= 0)
			return CopDefaultSpawnPoint;
		foreach (CloneRoom spawnPoint in copSpawnPoints) {
			if (!spawnPoint.Broken ())
				return spawnPoint.transform;
		}
		return CopDefaultSpawnPoint; // if all are broken
	}
	
	public Transform GetCrimSpawnPoint(){
		if (crimSpawnPoints.Count <= 0) {
			float randomChoice = Random.Range(0, CrimDefaultSpawnPoint.Length);
			int randomNum = (int)randomChoice;
			return CrimDefaultSpawnPoint[randomNum];
		}

		foreach (CloneRoom spawnPoint in crimSpawnPoints) {
			if (!spawnPoint.Broken ())
				return spawnPoint.transform;
		}
		return CrimDefaultSpawnPoint[0]; 
	}

	void SortCrimSpawnPoints(){
		crimSpawnPoints.Sort ((a, b) => b.transform.position.x.CompareTo (a.transform.position.x));
	}

	void SortCopSpawnPoints(){
		copSpawnPoints.Sort ((a, b) => a.transform.position.x.CompareTo (b.transform.position.x));
		// not sure what the best sorting would be for cops, or if their list should just be combed
		// whenever the cop spawns.
	}
}
