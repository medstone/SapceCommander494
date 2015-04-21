using UnityEngine;
using System.Collections;

public class ReloadLevel : CommandObject {
	
	public override void setTransform (Transform trans) {
		
	}
	
	public override bool doingWork () {
		Application.LoadLevel(Application.loadedLevel);
		return false;
	} 
}
