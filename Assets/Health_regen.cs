using UnityEngine;
using System.Collections;

public class Health_regen : MonoBehaviour {

	public GameObject healthroom;
	public float regen_time;//time to regenerate 1 health;
	public float time_elap = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay(Collider other){
		if (other.gameObject.tag == "HealthRoom") {
			Control hr = other.gameObject.GetComponent("Control") as Control;
			PlayerStats p_stat = this.GetComponent("PlayerStats") as PlayerStats;
			if(time_elap >= regen_time && hr.holds == p_stat.team && p_stat.health < p_stat.startingHealth){
				p_stat.health++;
				time_elap = 0.0f;
			} else if(hr.holds == p_stat.team && p_stat.health < p_stat.startingHealth){
				time_elap += Time.deltaTime;
			}else{
				time_elap = 0.0f;
			}
		}
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.tag == "HealthRoom") {
			time_elap = 0.0f;
		}
	}
}
