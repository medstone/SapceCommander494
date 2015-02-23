using UnityEngine;
using System.Collections;

public class SpawnWeapons : MonoBehaviour {

	public GameObject shotgunPrefab;
	GameObject shotgun;
	Weapon wepRef;
	Control roomControl;

	void Awake(){
		roomControl = GetComponent<Control> ();
	}

	// Use this for initialization
	void Start () {
		shotgun = Instantiate (shotgunPrefab, this.transform.position,Quaternion.identity) as GameObject;
		wepRef = shotgun.GetComponent<Weapon> ();
		wepRef.allegiance = roomControl.holds;
	}
	
	// Update is called once per frame
	void Update () {
		if (shotgun != null && shotgun.tag == "Weapon") {
			shotgun = null;		
			StartCoroutine(SpawnDelay());
		}
		else if (shotgun != null && wepRef.allegiance == Faction_e.spaceCop && roomControl.holds == Faction_e.spaceCrim) {
			wepRef.allegiance = Faction_e.spaceCrim;
		}
	}

	IEnumerator SpawnDelay(){
		yield return new WaitForSeconds(10f);
		shotgun = Instantiate (shotgunPrefab, this.transform.position,Quaternion.identity) as GameObject;
		wepRef = shotgun.GetComponent<Weapon> ();
	}
}
