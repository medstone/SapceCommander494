using UnityEngine;
using System.Collections;

public class RoomConsole : MonoBehaviour {

	public int maxHealth;
	public int health;
	int damageTaken;
	public float repairDuration; // time needed to fix this station when broken
	float amountRepaired; // time that station has been repaired (before it becomes non-broken)

	bool beingRepaired;

	public bool IsBroken{
		get{return (health <= 0);}
	}

	public bool IsDamaged{
		get{return (health < maxHealth);}
	}

	// returns true if fixing is gonna happen, false otherwise
	public bool Repair(bool trigger = true){  // to start and stop the repairing?
		if (!IsDamaged)
						return false;
		if (!beingRepaired) {
			StartCoroutine(RepairRoutine());
		}
		if (beingRepaired && !trigger)
			beingRepaired = trigger;
		return beingRepaired;
	}

	public void TakeHit(int dmg){
		damageTaken += dmg;
	}

	IEnumerator RepairRoutine(){
		beingRepaired = true;
		float startTime = Time.time;
		while (beingRepaired) {
			if (IsBroken){
				if (Time.time - startTime >= repairDuration - amountRepaired){
					Repaired ();
					startTime = Time.time; // so that damaged counter can start up
				}
				yield return null;
			}
			else if (IsDamaged){ // recover some amount of health per second
				if (Time.time - startTime > 1f){
					health += 1;
					startTime += 1f;
				}
				yield return null;
			}
			else { // nothing more to be done here
				beingRepaired = false;
			}
		}
		amountRepaired = Time.time - startTime; // if we break out early.
	}

	void Awake(){

	}

	// Use this for initialization
	void Start () {
		beingRepaired = false;
		damageTaken = 0;
		health = maxHealth;
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	void LateUpdate(){
		int startHealth = health;
		health -= damageTaken;
		damageTaken = 0;
		if (startHealth > 0 && health <= 0) // if it broke this update
						Broken ();
	}
	
	void Broken(){
		amountRepaired = 0f;
	}

	void Repaired(){
		health = maxHealth / 2;
	}

}
