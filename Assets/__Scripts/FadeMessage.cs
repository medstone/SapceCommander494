using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum FadeState_e {
	fadeIn,
	stay,
	fadeOut,
	none
}

public class FadeMessage : MonoBehaviour {

	public float fadeTime = 2f;
	public float stayTime = 2f;
	FadeState_e state;
	float startTime;
	Text textField;
	
	// Use this for initialization
	void Start () {
		textField = GetComponent<Text>();
		if(!textField) 	print("No Text Component brah");
		else 			print("This thing has a component");
		textField.enabled = false;
		state = FadeState_e.none;
	}
	
	void Update () {
		switch (state) {
		case FadeState_e.fadeIn:
			FadeIn();
			break;
		case FadeState_e.stay:
			Stay();
			break;
		case FadeState_e.fadeOut:
			FadeOut();
			break;
		default: break;
		}
	}
	
	// Pass function a string and it will immediately fade it in
	// then fade out after a few seconds
	public void displayMessage(string msg) {
		textField.enabled = true;
		textField.text = msg;
		state = FadeState_e.fadeIn;
		Color c = Color.white;
		c.a = 0f;
		textField.color = c;
	}
	
	void FadeIn() {
		Color c = textField.color;
		c.a += Time.deltaTime / fadeTime;
		
		// switch to stay state if we are opaque
		if(c.a > 1f) {
			c.a = 1f;
			state = FadeState_e.stay;
			startTime = Time.time;
		}
		
		textField.color = c;
	}
	
	void Stay() {
		if(Time.time > startTime + stayTime) {
			state = FadeState_e.fadeOut;
		}
	}
	
	void FadeOut() {
		Color c = textField.color;
		c.a -= Time.deltaTime / fadeTime;
		
		// switch to stay state if we are opaque
		if(c.a < 0f) {
			c.a = 0f;
			state = FadeState_e.none;
			textField.enabled = false;
		}
		
		textField.color = c;
	}
}
