using UnityEngine;
using System.Collections;

public class robotRedirect : MonoBehaviour {
	public int paths;
	public Vector3 newDirection1;
	public Vector3 newDirection2;
	public int last = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == "Robot") {
			if (paths == 1){
				coll.gameObject.GetComponent<robotAI>().direction = newDirection1;
			}
			else{
				if(last == 2){
					coll.gameObject.GetComponent<robotAI>().direction = newDirection1;
					last = 1;
				}
				else{
					coll.gameObject.GetComponent<robotAI>().direction = newDirection2;
					last = 2;
				}
			}
		}
	}
}
