using UnityEngine;
using System.Collections;

public class turret_fire : MonoBehaviour {

	public GameObject bullet;
	public GameObject Turret_room;
	public float radius = 5.0f;
	public GameObject[] players;
	public float rate_of_fire = 0.5f;
	public float timer = 0.0f;
	public bool destroyed = false;
	public int health = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		players = GameObject.FindGameObjectsWithTag ("Actor");
		Control turr = Turret_room.GetComponent ("Control") as Control;

		int closest = -1;
		float min_dist = 1000000.0f;//arbitrarily high
		float dist = 0.0f;
		for (int i = 0; i < players.Length; i++) {
			dist = Vector3.Distance(this.transform.position,players[i].transform.position);
			PlayerStats play = players[i].GetComponent("PlayerStats") as PlayerStats;
			if(dist < min_dist && play.team != turr.holds && destroyed == false){
				min_dist = dist;
				closest = i;
			}
		}
		if (closest >= 0) {
			this.transform.LookAt (players [closest].transform.position);
			if (min_dist < radius && timer > rate_of_fire) {
							//Quaternion rot = this.transform.rotation;
							//rot.x += 180;
							//this.transform.rotation = rot;
					Vector3 pos = this.transform.position;
					pos.y += this.transform.lossyScale.y / 2f;
					GameObject cur_bullet = Instantiate (bullet, pos, this.transform.rotation) as GameObject;
					Projectile cur_bull = cur_bullet.GetComponent("Projectile") as Projectile;
					if(turr.holds == Faction_e.spaceCop){
						cur_bullet.layer = Utils.CopProjectileLayer();
						cur_bull.IgnoreLayer(LayerMask.NameToLayer("CopProjectile"));
						cur_bull.IgnoreLayer(LayerMask.NameToLayer("Cops"));
						cur_bull.IgnoreLayer(LayerMask.NameToLayer("Turret"));
					}
					else if(turr.holds == Faction_e.spaceCrim){
					cur_bullet.layer = Utils.CrimProjectileLayer();
					cur_bull.IgnoreLayer(LayerMask.NameToLayer("CrimProjectile"));
					cur_bull.IgnoreLayer(LayerMask.NameToLayer("Crims"));
					cur_bull.IgnoreLayer(LayerMask.NameToLayer("Turret"));
					}

				cur_bull.damage = 1;
					Vector3 direction = new Vector3(0f,0f,0f);
				direction.x = (players[closest].transform.position.x - this.transform.position.x)/3f;
				direction.y = (players[closest].transform.position.y - this.transform.position.y)/3f;
				direction.z = (players[closest].transform.position.z - this.transform.position.z)/3f;
					print (direction);
					cur_bull.SetBearing(direction);
					timer = 0.0f;
				} else {
					timer += Time.deltaTime;
				}
		}

	}
}
