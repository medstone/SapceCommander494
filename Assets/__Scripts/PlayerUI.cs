using UnityEngine;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	
	Transform healthBar;
	float healthBarMaxWidth;
	
	PlayerStats stats;
	
	// Use this for initialization
	void Start () {
		// grab the stats 
		stats = GetComponentInParent<FollowObject>().target.GetComponent<PlayerStats>();
		if(!stats) {
			print("PlayerUI: Unable to get player stats");
		}
		
		healthBar = transform.Find("HealthBar");
		if(!stats) {
			print("PlayerUI: Unable to healthbar child");
		}
		else {
			healthBarMaxWidth = healthBar.localScale.x;
		}
	}
	
	void FixedUpdate () {
		// update health bar
		float percentHealthLeft = (float) stats.health / stats.startingHealth;
		Vector3 sz = healthBar.localScale;
		sz.x = healthBarMaxWidth * percentHealthLeft;
		healthBar.localScale = sz;
	}
}
