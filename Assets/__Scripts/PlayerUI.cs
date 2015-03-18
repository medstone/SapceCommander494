using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	
	// health bar
	Transform healthBar;
	float healthBarMaxWidth;
	
	// UI text for beginning message (will turn off after 5 secs)
	Text startMsg;
	
	// stats for health bar
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
		
		startMsg = transform.Find("StartMsg").GetComponent<Text>();
		if(startMsg) {
			toggleStartMessgeOnOff();
			Invoke("toggleStartMessgeOnOff", 5);
		}
	}
	
	void FixedUpdate () {
		// update health bar
		float percentHealthLeft = (float) stats.health / stats.startingHealth;
		Vector3 sz = healthBar.localScale;
		sz.x = healthBarMaxWidth * percentHealthLeft;
		healthBar.localScale = sz;
	}
	
	void toggleStartMessgeOnOff() {
		startMsg.enabled = !startMsg.enabled;
	}
}
