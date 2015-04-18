using UnityEngine;
using System.Collections;

public class SpawnWeapons : MonoBehaviour {

	public GameObject weaponPrefab;
	GameObject weapon;
	Weapon wepRef;
	Control roomControl;
	GameObject barrier; // force field surrounding the weapon

	public float spawnDelay = 10f;

	public Material crimBarrierMat;
	public Material copBarrierMat;

	void Awake(){
		roomControl = GetComponent<Control> ();
		barrier = transform.Find ("WeaponBarrier").gameObject;
	}

	// Use this for initialization
	void Start () {
		weapon = Instantiate (weaponPrefab,new Vector3(this.transform.position.x,1f,this.transform.position.z),Quaternion.identity) as GameObject;
		wepRef = weapon.GetComponent<Weapon> ();
		wepRef.allegiance = roomControl.holds;
		SetBarrierAllegiance (roomControl.holds);
		roomControl.CapturedEvent += OnCapture;
	}
	
	// Update is called once per frame
	void Update () {
		if (weapon != null && weapon.tag == "Weapon") { 
			// when shotgun is picked up, it changes the tag
			weapon = null; // so we forget the reference to the shotgun we previously spawned
			StartCoroutine (SpawnDelay ());
		} else if (weapon != null && wepRef.allegiance == Faction_e.spaceCop && roomControl.holds == Faction_e.spaceCrim) {
			wepRef.allegiance = Faction_e.spaceCrim;
		} else if (weapon != null && wepRef.allegiance == Faction_e.spaceCrim && roomControl.holds == Faction_e.spaceCop) {
			wepRef.allegiance = Faction_e.spaceCop;
		}
	}

	IEnumerator SpawnDelay(){
		yield return new WaitForSeconds(spawnDelay);
		weapon = Instantiate (weaponPrefab, this.transform.position,Quaternion.identity) as GameObject;
		wepRef = weapon.GetComponent<Weapon> ();
	}

	void OnCapture(Faction_e newTeam){
		SetBarrierAllegiance (newTeam);
	}

	void SetBarrierAllegiance(Faction_e team){
		foreach (Transform child in barrier.transform) {
			if (team == Faction_e.spaceCop){
				child.gameObject.layer = Utils.CopWepBarrierLayer();
				child.GetComponent<MeshRenderer>().material = copBarrierMat;
			}
			else {
				child.gameObject.layer = Utils.CrimWepBarrierLayer();
				child.GetComponent<MeshRenderer>().material = crimBarrierMat;
			}
		}
	}
}
