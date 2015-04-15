using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	
	const string copStartText = "Criminals are coming from the <b>LEFT</b>, defend the ship!";
	const string crimStartText = "Make your way <b>RIGHT</b> and take control of the ship!";
	
	
	// health indicator
	Text healthText;
	
	// Weapon indicators
	Text weaponName;
	Text weaponAmmo;
	
	// UI text for beginning message (will turn off after 5 secs)
	// FadeMessage topFadeText; 
	FadeMessage midFadeText;
	FlashText reloadText;
	
	// stats for health bar
	PlayerStats stats;
	
	void Awake () {
		// topFadeText = transform.Find("RoomMsg").GetComponent<FadeMessage>();
		healthText =  transform.Find("HealthIndicator/Value").GetComponent<Text>();
		weaponName = transform.Find("WeaponIndicator/Name").GetComponent<Text>();
		weaponAmmo = transform.Find("WeaponIndicator/Ammo").GetComponent<Text>();
		reloadText = GetComponentInChildren<FlashText>();
		
		
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
		
		// update the health to a value between 0 and startingHealth
		healthText.text = Mathf.Clamp(stats.health, 0, stats.startingHealth).ToString();
		
		if(stats.secondaryWeapon) {
			weaponName.text = stats.secondaryWeapon.weapName;
			weaponAmmo.text = stats.secondaryWeapon.clip.ToString() + " | " +
								(stats.secondaryWeapon.ammunition - stats.secondaryWeapon.clip).ToString() ;
			
			reloadBit(stats.secondaryWeapon.reloading);
		}
		else {
			weaponName.text = stats.defaultWeapon.weapName;
			weaponAmmo.text = stats.defaultWeapon.clip.ToString() + " | " + "Unlimited";
			
			reloadBit(stats.defaultWeapon.reloading);
		}
		
	}
	
	void reloadBit(bool isReloading) {
		if(!reloadText) return;
		if(isReloading){
			reloadText.startFlashing();
		}
		else {
			reloadText.stopFlashing();
		}
	}
	
}
