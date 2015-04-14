using UnityEngine;
using System.Collections;

public abstract class CommandObject : MonoBehaviour {
	
	public void Start() {
		GetComponent<Renderer>().enabled = false;
	}
	
	public abstract void setTransform (Transform trans);
	
	public abstract bool doingWork ();
	
}