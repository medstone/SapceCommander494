using UnityEngine;
using System.Collections;

public class RoomListener : MonoBehaviour {
	
	FadeMessage fm;
	
	// Use this for initialization
	void Start () {
		fm = transform.parent.Find("Camera/PlayerUI/RoomMsg").GetComponent<FadeMessage>();
		if(fm) {
		}
	}
	
	public void Display (string msg) {
		if(fm) {
			fm.displayMessage(msg);
		}
	}
}
