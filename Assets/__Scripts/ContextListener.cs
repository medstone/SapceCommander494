﻿using UnityEngine;
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
}