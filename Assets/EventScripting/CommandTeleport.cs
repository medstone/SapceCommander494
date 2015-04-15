using UnityEngine;
using System.Collections;

public class CommandTeleport : CommandObject {
	
	public Transform target;
	
	public override void setTransform (Transform trans) {
		target = trans;
	}
	
	public override bool doingWork () {
		target.transform.position = transform.position;
		return false;
	} 
}
