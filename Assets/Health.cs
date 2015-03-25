using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	public int healthrate;
	public GameObject controlroom;
	public float time = 0.0f;
	public float max_time = 1.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		if (other.tag == "Actor") {
			PlayerStats stuff = other.GetComponent("PlayerStats") as PlayerStats;
			Control stiff = controlroom.GetComponent("Control") as Control;
			if(stuff.health < stuff.startingHealth && stiff.holds == stuff.team){
				if(time >= max_time){
					stuff.health++;
					time = 0.0f;
				}
				else{
					time += Time.deltaTime;
				}
			}
		}
	}
}
