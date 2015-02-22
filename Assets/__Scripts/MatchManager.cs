using UnityEngine;
using System.Collections;

public class MatchManager : MonoBehaviour {
	static public MatchManager S;

	public Transform CopSpawnPoint;
	public Transform CrimSpawnPoint;

	public GameObject[] rooms;


	void Awake(){
		S = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
