using UnityEngine;
using System.Collections;

public class RoomListener : MonoBehaviour {
	
	FadeMessage fm;
	
	// Use this for initialization
	void Start () {
		fm = transform.parent.Find("Camera/PlayerUI/RoomMsg").GetComponent<FadeMessage>();
		if(fm) {
			print("RoomListener found the RoomMsg");
		}
	}
	
	public void Display (string msg) {
		if(fm) {
			print("Player RoomListener called");
			fm.displayMessage(msg);
		}
	}
}
