using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchManager : MonoBehaviour {
	static public MatchManager S;

	public Transform CopDefaultSpawnPoint;
	public Transform[] CrimDefaultSpawnPoint;

	List <Transform> crimPositions; // used so cop can spawn right of rightmost criminal

	List<CloneRoom> crimSpawnPoints;
	List<CloneRoom> copSpawnPoints;

	public Control[] keyRooms; // key strategic rooms that are locked / unlocked.
	int currentContestedPoint = 0;
	
	Control steeringControl; 	// steering to see if criminals hacked it
	ProgressBar progress;  		// progress bar to see if time is up, and arrived at prison planet
	
	Text winnerText;
	public bool gameEnded = false;

	void Awake(){
		S = this;
		copSpawnPoints = new List<CloneRoom> ();
		crimSpawnPoints = new List<CloneRoom> ();
		crimPositions = new List<Transform> ();
		steeringControl = GameObject.Find("Steering").GetComponent<Control>();
		progress = GameObject.Find("ProgressBar").GetComponent<ProgressBar>();
		winnerText = GameObject.Find("WinnerText").GetComponent<Text>();
	}

	// Use this for initialization
	void Start () {
		winnerText.text = "";
		// register with Steering Room
		steeringControl.CapturedEvent += SteeringCaptured;

		CloneRoom[] rooms = GameObject.FindObjectsOfType<CloneRoom> ();
		foreach (CloneRoom room in rooms) {
			if (room.control.holds == Faction_e.spaceCop)
				copSpawnPoints.Add(room);
			else 
				crimSpawnPoints.Add (room);
		}
		SortCopSpawnPoints ();
		SortCrimSpawnPoints ();
		PlayerStats[] players = GameObject.FindObjectsOfType<PlayerStats> ();
		foreach (PlayerStats stats in players) {
			if (stats.team == Faction_e.spaceCrim)
				crimPositions.Add (stats.GetComponent<Transform>());
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(gameEnded && Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel(0);
		}
	}
	
	public void SteeringCaptured(Faction_e f) {
		if(winnerText.text != "") return;
		progress.isRunning = false;
		winnerText.text = "Criminals Win!\nPress SPACEBAR to play again!";
		gameEnded = true;
		
	}
	
	public void TimeRanOut() {
		winnerText.text = "Cops Win!!\nPress SPACEBAR to play again!";
		gameEnded = true;
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
		SortCrimPositions ();
		if (copSpawnPoints.Count <= 0)
			return CopDefaultSpawnPoint;
		foreach (CloneRoom spawnPoint in copSpawnPoints) {
			// cop will spawn right of rightmost criminal (with a little wiggle room)
			if (!spawnPoint.Broken () && (spawnPoint.GetComponent<Transform>().position.x + 15f) > crimPositions[0].position.x)
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

	void SortCrimPositions(){
		crimPositions.Sort ((a, b) => b.transform.position.x.CompareTo (a.transform.position.x));
	}

	// unlocks next room if possible
	public void KeyRoomCaptured(){
		++currentContestedPoint;
		if (currentContestedPoint < keyRooms.Length)
			keyRooms [currentContestedPoint].locked = false;
		// notify all players that this just went down
		ContextListener[] playerListeners = GameObject.FindObjectsOfType<ContextListener> ();
		foreach (ContextListener listener in playerListeners) {
			listener.Display ("Barrier Shut Down!");
		}
	}
}
