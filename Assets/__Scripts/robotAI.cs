using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*public enum faction{
	cop,
	crim
}*/

public class robotAI : MonoBehaviour {
	public Faction_e fact;
	public Vector3 direction;
	public float speed = 5f;
	public GameObject project;
	public float shoot = 1f;
	public robotSpawn spawn;
	int raylayer;

	// Use this for initialization
	void Start () {
		if (fact == Faction_e.spaceCop) {
			gameObject.layer = Utils.CopLayer ();
		} else if (fact == Faction_e.spaceCrim) {
			gameObject.layer = Utils.CrimLayer ();
		}
		raylayer = 1 << LayerMask.NameToLayer ("Wall");
	}

	
	// Update is called once per frame
	void Update () {
		if (shoot > 0) {
			shoot -= Time.deltaTime;
		}
		RaycastHit targ = new RaycastHit();
		Collider[] colls = Physics.OverlapSphere(this.transform.position, 10f);
		foreach( Collider coll in colls){
			if(coll.gameObject.tag == "Robot" && this.fact != coll.gameObject.GetComponent<robotAI>().fact){
				if(Physics.Raycast(this.transform.position,new Vector3(coll.transform.position.x-this.transform.position.x,coll.transform.position.y-this.transform.position.y,coll.transform.position.z-this.transform.position.z),out targ)){
					if(targ.collider.gameObject.tag != "Robot"){
						continue;
					}
				   	this.GetComponent<Rigidbody>().velocity = Vector3.zero;
					if(shoot > 0){
						return;
					}
					GameObject proj = Instantiate(project) as GameObject;
					proj.layer = gameObject.layer;
					proj.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y+(this.transform.lossyScale.y/2f),this.transform.position.z);
					proj.GetComponent<Projectile>().SetBearing(new Vector3((coll.transform.position.x-this.transform.position.x)/3f,(coll.transform.position.y-this.transform.position.y)/3f,(coll.transform.position.z-this.transform.position.z)/3f));
					shoot = 1f;
					return;
				}
			}
			else if(coll.gameObject.tag == "Actor" && this.fact != coll.gameObject.GetComponent<PlayerStats>().team){
				if(Physics.Raycast(this.transform.position,new Vector3(coll.transform.position.x-this.transform.position.x,coll.transform.position.y-this.transform.position.y,coll.transform.position.z-this.transform.position.z),out targ)){
					if(targ.collider.gameObject.tag != "Actor"){
						continue;
					}
					this.GetComponent<Rigidbody>().velocity = Vector3.zero;
					if(shoot > 0){
						return;
					}
					GameObject proj = Instantiate(project) as GameObject;
					proj.layer = gameObject.layer;
					proj.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y+(this.transform.lossyScale.y/2f),this.transform.position.z);
					proj.GetComponent<Projectile>().SetBearing(new Vector3((coll.transform.position.x-this.transform.position.x)/3f,(coll.transform.position.y-this.transform.position.y)/3f,(coll.transform.position.z-this.transform.position.z)/3f));
					proj.GetComponent<Projectile>().damage = 1;
					shoot = 1f;
					return;
				}
			}
		}
		Vector3 vel;
		vel.x = direction.x * speed;
		vel.y = GetComponent<Rigidbody>().velocity.y;
		vel.z = direction.z * speed;
		List<Vector3> paths = new List<Vector3>();
		targ = new RaycastHit ();
		if (Physics.Raycast (this.transform.position, new Vector3(vel.x,0f,vel.z), out targ, Mathf.Sqrt (vel.x * vel.x + vel.z * vel.z)/10, raylayer)) {
			if(targ.collider.gameObject.tag == "Robot");
			else if(targ.collider.gameObject.tag == "Wall" && targ.collider.gameObject.GetComponent<Renderer>().enabled == true){
				if(!Physics.Raycast (this.transform.position, new Vector3(0f,0f,1f), out targ, Mathf.Sqrt (vel.x * vel.x + vel.z * vel.z), raylayer)){
					paths.Add(new Vector3(0f,0f,1f));
				}
				if(!Physics.Raycast (this.transform.position, new Vector3(1f,0f,0f), out targ, Mathf.Sqrt (vel.x * vel.x + vel.z * vel.z), raylayer)){
					paths.Add(new Vector3(1f,0f,0f));
				}
				if(!Physics.Raycast (this.transform.position, new Vector3(0f,0f,-1f), out targ, Mathf.Sqrt (vel.x * vel.x + vel.z * vel.z), raylayer)){
					paths.Add(new Vector3(0f,0f,-1f));
				}
				if(!Physics.Raycast (this.transform.position, new Vector3(-1f,0f,0f), out targ, Mathf.Sqrt (vel.x * vel.x + vel.z * vel.z), raylayer)){
					paths.Add(new Vector3(-1f,0f,0f));
				}
				float val = Random.value * paths.Count;
				for(int i = 0; i < paths.Count; ++i){
					if(val >= i && val < i+1){
						direction = paths[i];
					}
				}
			}
		}
		vel.x = direction.x * speed;
		vel.y = GetComponent<Rigidbody>().velocity.y;
		vel.z = direction.z * speed;
		this.GetComponent<Rigidbody>().velocity = vel;
	}

	void OnDestroy(){
		spawn.numSpawned--;
	}
}
