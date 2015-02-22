using UnityEngine;
using System.Collections;

public class SpawnWeapons : MonoBehaviour {

	public GameObject shotgun;

	// Use this for initialization
	void Start () {
		Instantiate (shotgun, this.transform.position,Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
