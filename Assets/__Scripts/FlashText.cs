using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashText : MonoBehaviour {
	
	enum Flash_e {
		fadeIn,
		fadeOut,
		none,
	}
	
	public bool flashing;
	float startTime;
	Flash_e state;
	Text flashText;
	Color myColor;
	public float flashTime = 0.3f;
	
	void Awake() {
		flashText = GetComponent<Text>();
	}
	
	// Use this for initialization
	void Start () {
		myColor = new Color(1f, 1f, 1f, 0f);
		flashText.color = myColor;
		state = Flash_e.none;
		flashing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(state != Flash_e.none) {
			fadeFunc();
		}
	}
	
	public void startFlashing() {
		print("Flashing Called");
		// starting off set to white, no alpha
		if(!flashing) {
			print("Flashing with time " + flashTime);
			flashing = true;
			myColor.a = 0f;
			flashText.color = myColor;
			state = Flash_e.fadeIn;
			print("state: " + state);
			startTime = Time.time;
		}
	}
	
	public void stopFlashing() {
		print ("Stopped flashing");
		flashing = false;
		myColor.a = 0f;
		flashText.color = myColor;
		state = Flash_e.none;
	}
	
	// pass it 1 for in or -1 for out
	void fadeFunc() {
		print("state: " + state);
		// time spent fading so far / half total flash time is the percent faded either way
		float diff = Time.time - startTime;
		float percent = diff / flashTime;
		
		// determine the new alpha
		if(state == Flash_e.fadeIn) {
			myColor.a = percent;
			if(percent > 1f){
				state = Flash_e.fadeOut;
				startTime = Time.time;
			}
		}
		else if(state == Flash_e.fadeOut) {
			myColor.a = 1f - percent;
			
			if(percent > 1f)
			{
				state = Flash_e.fadeIn;
				startTime = Time.time;
			}
		}
		
		// set the color
		flashText.color = myColor;
	}
	
}
