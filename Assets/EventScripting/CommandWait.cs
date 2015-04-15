using UnityEngine;
using System.Collections;

public class CommandWait : CommandObject {

	public float waitTime = 2f;
	private float startTime;

	public override void setTransform (Transform trans) {
		startTime = Time.time;
	}
	
	public override bool doingWork() {
		
		// doing work until waitTime has passed since start time
		if(Time.time < startTime + waitTime) {
			return true;
		}
		return false;
	}
}
