using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HackBar : MonoBehaviour {
	
	Color copColor;
	Color crimColor;
	Color noColor;
	
	RawImage teamGlowBar;
	public Control controlRef;
	Faction_e currFaction;
	
	Transform toCops;
	Transform toCrims;
	float distMagnitude;
	
	Transform slider;
	
	void Awake() {
		teamGlowBar = transform.Find("TeamGlowBar").GetComponent<RawImage>();
		toCops = transform.Find("BarMask/ToCops");
		toCrims = transform.Find("BarMask/ToCrims");
		slider = transform.Find("BarMask/CopBar");
		controlRef = GetComponentInParent<Control>();
	} 
	
	// Use this for initialization
	void Start () {
		// setup colors
		copColor = new Color(0f, 0f, 1f, 1f);
		crimColor = new Color(1f, 0f, 0f, 1f);
		noColor = new Color(0f, 0f, 0f, 0f);
		
		controlRef.CapturedEvent += notifyCapture;
		controlRef.CaptureAmountEvent += notifyAmount;
		
		notifyCapture(controlRef.holds);
		distMagnitude = Vector3.Distance(toCops.position, toCrims.position);
		notifyAmount(0f);
	}
	
	void notifyAmount(float amountHacked) {
		amountHacked = (amountHacked / controlRef.hack_time);
		if(currFaction == Faction_e.spaceCop) {
			slider.position = Vector3.MoveTowards(toCops.position, toCrims.position, amountHacked * distMagnitude);
		}
		else {
			slider.position = Vector3.MoveTowards(toCrims.position, toCops.position, amountHacked * distMagnitude);
		}
	}
	
	void notifyCapture(Faction_e faction) {
		currFaction = controlRef.holds;
		setColor();
	}
	
	void setColor() {
		
		switch(currFaction)
		{
			case Faction_e.spaceCop:
				teamGlowBar.color = copColor;
				break;
			case Faction_e.spaceCrim:
				teamGlowBar.color = crimColor;
				break;
			case Faction_e.neutral:
				teamGlowBar.color = noColor;
				break;
		}
	}
}
