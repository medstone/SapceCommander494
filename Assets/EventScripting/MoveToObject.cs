using UnityEngine;
using System.Collections;

public class MoveToObject : CommandObject {
	
	public float speed = 10f;
	public float stopMargin = .1f;
	
	public Transform target;
	
	public override void setTransform (Transform trans) {
		target = trans;
		target.LookAt(transform.position);
	}
	
	public override bool doingWork () {
		// print("DOIN WORK");
        target.position = Vector3.MoveTowards(target.position, transform.position, speed * Time.deltaTime);
        if(Vector3.Distance(target.position, transform.position) > stopMargin) {
        	// print("Still Working");
        	return true;
        }
        
        // print("I'm done with my command");
        return false;
	}
	
}