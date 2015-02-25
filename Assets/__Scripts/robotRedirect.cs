using UnityEngine;
using System.Collections;

public class robotRedirect : MonoBehaviour {
	public int crimPaths;
	public Vector3 newCrimDirection1;
	public Vector3 newCrimDirection2;
	public int lastCrim = 1;
	public int copPaths;
	public Vector3 newCopDirection1;
	public Vector3 newCopDirection2;
	public int lastCop;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Robot") {
			if(coll.gameObject.GetComponent<robotAI>().fact == Faction_e.spaceCrim){
				coll.transform.position = this.transform.position;
				if (crimPaths == 1){
					coll.gameObject.GetComponent<robotAI>().direction = newCrimDirection1;
				}
				else{
					if(lastCrim == 2){
						coll.gameObject.GetComponent<robotAI>().direction = newCrimDirection1;
						lastCrim = 1;
					}
					else{
						coll.gameObject.GetComponent<robotAI>().direction = newCrimDirection2;
						lastCrim = 2;
					}
				}
			}
			else if(coll.gameObject.GetComponent<robotAI>().fact == Faction_e.spaceCop){
				coll.transform.position = this.transform.position;
				if (copPaths == 1){
					coll.gameObject.GetComponent<robotAI>().direction = newCopDirection1;
				}
				else{
					if(lastCop == 2){
						coll.gameObject.GetComponent<robotAI>().direction = newCopDirection1;
						lastCop = 1;
					}
					else{
						coll.gameObject.GetComponent<robotAI>().direction = newCopDirection2;
						lastCop = 2;
					}
				}
			}
		}
	}
}
