using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum FadeState_e {
	fadeIn,
	stay,
	fadeOut,
	popStay,
	none
}

public class FadeMessage : MonoBehaviour {

	public float fadeTime = 2f;
	public float stayTime = 2f;
	private float popTime = .6f;
	FadeState_e state;
	float startTime;
	Text textField;
	
	void Awake() {
		textField = GetComponent<Text>();
	}
	
	// Use this for initialization
	void Start () {
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
		case FadeState_e.popStay:
			PopStay();
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
		textField.text = msg;
		textField.enabled = true;
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
	
	public void popMessage(string msg) {
		// turn on and enable text
		textField.text = msg;
		textField.enabled = true;
		startTime = Time.time;
		
		// set back to opaque
		Color c = Color.white;
		c.a = 1f;
		textField.color = c;
		
		// set state
		state = FadeState_e.popStay;
	}
	
	void PopStay() {
		if(Time.time > startTime + popTime) {
			state = FadeState_e.none;
			textField.enabled = false;
		}
	}
}
