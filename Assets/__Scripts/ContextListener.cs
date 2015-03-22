using UnityEngine;
using System.Collections;

public class ContextListener : MonoBehaviour {

	FadeMessage fm;
	
	// Use this for initialization
	void Start () {
		fm = transform.parent.Find("Camera/PlayerUI/MidMsg").GetComponent<FadeMessage>();
		if(fm) {
			print("RoomListener found the MidMsg");
		}
	}
	
	public void Display (string msg) {
		if(fm) {
			print("Player ContextListener called");
			fm.displayMessage(msg);
		}
	}
}
