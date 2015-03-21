using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	
	const string copStartText = "Criminals are coming from the <b>LEFT</b>, defend the ship!";
	const string crimStartText = "Make your way <b>RIGHT</b> and take control of the ship!";
	
	// health bar
	Transform healthBar;
	float healthBarMaxWidth;
	
	// health indicator
	Text healthText;
	
	// UI text for beginning message (will turn off after 5 secs)
	// FadeMessage topFadeText; 
	FadeMessage midFadeText;
	
	// stats for health bar
	PlayerStats stats;
	
	void Awake () {
		// topFadeText = transform.Find("RoomMsg").GetComponent<FadeMessage>();
		healthText =  transform.Find("HealthIndicator/Value").GetComponent<Text>();
		
		// grab the stats 
		stats = GetComponentInParent<FollowObject>().target.GetComponent<PlayerStats>();
		
		// display starter text
		midFadeText = transform.Find("MidMsg").GetComponent<FadeMessage>();
		
	}
	
	// Use this for initialization
	void Start () {
		if(!stats) {
			print("PlayerUI: Unable to get player stats");
		}
		if(midFadeText) {
			if(stats.team == Faction_e.spaceCop) {
				midFadeText.displayMessage(copStartText);
			}
			else {
				midFadeText.displayMessage(crimStartText);
			}
		}
	}
	
	void FixedUpdate () {
		// // update health bar - Replaced with health indicator
		// float percentHealthLeft = (float) stats.health / stats.startingHealth;
		// Vector3 sz = healthBar.localScale;
		// sz.x = healthBarMaxWidth * percentHealthLeft;
		// healthBar.localScale = sz;
		
		// update health indicator
		healthText.text = stats.health.ToString();
		
	}
}
