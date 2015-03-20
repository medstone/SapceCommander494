using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlPointUI : MonoBehaviour {
	public Color blue = Color.blue;
	public Color red = Color.red;
	public Control controlRef;
	Slider slider;

	float sliderVal;

	void Awake(){
		slider = GetComponent<Slider> ();
	}
	// Use this for initialization
	void Start () {
		controlRef.CapturedEvent += OnCapture;
		controlRef.CaptureAmountEvent += OnTimeUpdate;
		if (controlRef.holds == Faction_e.spaceCop) {
			slider.GetComponentInChildren<Image>().color = blue;
		} 
		else {
			slider.GetComponentInChildren<Image>().color = red;
		}
	}

	void Update(){
		slider.value = 1f - (sliderVal / controlRef.hack_time);
	}

	void OnTimeUpdate(float value){
		sliderVal = value;
	}

	void OnCapture(Faction_e new_Team){
		if (new_Team == Faction_e.spaceCop) {
			slider.GetComponentInChildren<Image>().color = blue;
		} 
		else {
			slider.GetComponentInChildren<Image>().color = red;
		}
	}
}
