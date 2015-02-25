using UnityEngine;
using System.Collections;

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


	// Use this for initialization
	void Start () {
	
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
				   	this.rigidbody.velocity = Vector3.zero;
					if(shoot > 0){
						return;
					}
					GameObject proj = Instantiate(project) as GameObject;
					proj.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y+(this.transform.lossyScale.y/2f),this.transform.position.z);
					proj.GetComponent<Projectile>().bearing = new Vector3((coll.transform.position.x-this.transform.position.x)/3f,(coll.transform.position.y-this.transform.position.y)/3f,(coll.transform.position.z-this.transform.position.z)/3f);
					shoot = 1f;
					return;
				}
			}
			else if(coll.gameObject.tag == "Actor" && this.fact != coll.gameObject.GetComponent<PlayerStats>().team){
				if(Physics.Raycast(this.transform.position,new Vector3(coll.transform.position.x-this.transform.position.x,coll.transform.position.y-this.transform.position.y,coll.transform.position.z-this.transform.position.z),out targ)){
					if(targ.collider.gameObject.tag != "Actor"){
						continue;
					}
					this.rigidbody.velocity = Vector3.zero;
					if(shoot > 0){
						return;
					}
					GameObject proj = Instantiate(project) as GameObject;
					proj.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y+(this.transform.lossyScale.y/2f),this.transform.position.z);
					proj.GetComponent<Projectile>().bearing = new Vector3((coll.transform.position.x-this.transform.position.x)/3f,(coll.transform.position.y-this.transform.position.y)/3f,(coll.transform.position.z-this.transform.position.z)/3f);
					shoot = 1f;
					return;
				}
			}
		}
		Vector3 vel;
		vel.x = direction.x * speed;
		vel.y = rigidbody.velocity.y;
		vel.z = direction.z * speed;
		this.rigidbody.velocity = vel;
	}
}
