using UnityEngine;
using System.Collections;

public class SpawnWeapons : MonoBehaviour {

	public GameObject shotgunPrefab;
	GameObject shotgun;
	Weapon wepRef;
	Control roomControl;
	GameObject barrier; // force field surrounding the weapon

	public Material crimBarrierMat;
	public Material copBarrierMat;

	void Awake(){
		roomControl = GetComponent<Control> ();
		barrier = transform.Find ("WeaponBarrier").gameObject;
	}

	// Use this for initialization
	void Start () {
		shotgun = Instantiate (shotgunPrefab, this.transform.position,Quaternion.identity) as GameObject;
		wepRef = shotgun.GetComponent<Weapon> ();
		wepRef.allegiance = roomControl.holds;
		SetBarrierAllegiance (roomControl.holds);
		roomControl.CapturedEvent += OnCapture;
	}
	
	// Update is called once per frame
	void Update () {
		if (shotgun != null && shotgun.tag == "Weapon") { 
			// when shotgun is picked up, it changes the tag
			shotgun = null; // so we forget the reference to the shotgun we previously spawned
			StartCoroutine (SpawnDelay ());
		} else if (shotgun != null && wepRef.allegiance == Faction_e.spaceCop && roomControl.holds == Faction_e.spaceCrim) {
			wepRef.allegiance = Faction_e.spaceCrim;
		} else if (shotgun != null && wepRef.allegiance == Faction_e.spaceCrim && roomControl.holds == Faction_e.spaceCop) {
			wepRef.allegiance = Faction_e.spaceCop;
		}
	}

	IEnumerator SpawnDelay(){
		yield return new WaitForSeconds(10f);
		shotgun = Instantiate (shotgunPrefab, this.transform.position,Quaternion.identity) as GameObject;
		wepRef = shotgun.GetComponent<Weapon> ();
	}

	void OnCapture(Faction_e newTeam){
		SetBarrierAllegiance (newTeam);
	}

	void SetBarrierAllegiance(Faction_e team){
		foreach (Transform child in barrier.transform) {
			if (team == Faction_e.spaceCop){
				child.gameObject.layer = Utils.CopBarrierLayer();
				child.GetComponent<MeshRenderer>().material = copBarrierMat;
			}
			else {
				child.gameObject.layer = Utils.CrimBarrierLayer();
				child.GetComponent<MeshRenderer>().material = crimBarrierMat;
			}
		}
	}
}
