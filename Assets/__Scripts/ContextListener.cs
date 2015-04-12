using UnityEngine;
using System.Collections;

public class ContextListener : MonoBehaviour {

	FadeMessage fm;
	
	// Use this for initialization
	void Start () {
		fm = transform.parent.Find("Camera/PlayerUI/MidMsg").GetComponent<FadeMessage>();
	}
	
	public void Display (string msg) {
		if(fm) {
			fm.displayMessage(msg);
		}
	}
	
	public void PopDisplay(string msg) {
		if(!fm) return;
		print("Fade message exists");
		fm.popMessage(msg);
	}
}
