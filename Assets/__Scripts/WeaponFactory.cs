using UnityEngine;
using System.Collections;

// another main camera appropriate resource
// knows about the various weapon prefabs
public class WeaponFactory : MonoBehaviour {
	static public WeaponFactory S;

	public GameObject defaultWepPrefab;
	public GameObject shotgunPrefab;
	

	void Awake(){
		S = this;
	}

	public GameObject GetWeapon(string name){
		// instantiates (from prefab) and returns reference to
		// the newly created weapon. 
		if (name == "Shotgun") {
			return Instantiate(shotgunPrefab) as GameObject;
		}
		else 
			return Instantiate(defaultWepPrefab) as GameObject;
		// returns default weapon if an invalid name was entered, and writes to debug log
	}

}
