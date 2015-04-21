using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum BlinkState_e {
	fin, 
	fout, 
	none
}

public class HackBar : MonoBehaviour {
	
	
	Color copColor;
	Color crimColor;
	Color noColor;
	
	private float blinkSpeed = 5f;
	private bool isBlinking = false;
	private BlinkState_e bstate = BlinkState_e.none;
	
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
	
	void Update() {
		if(controlRef.hackState != HackState_e.none) {	
			if(!isBlinking) {
				isBlinking = true;
				bstate = BlinkState_e.fout;
			}
			switch(bstate) {
				case BlinkState_e.fin:
					FadeIn();
					break;
				case BlinkState_e.fout:
					FadeOut();
					break;
				default:
					break;
				}
		}
		else {
			setColor();
			isBlinking = false;
			bstate = BlinkState_e.none;
		}
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
	
	void FadeOut () {
		teamGlowBar.color = Color.Lerp(teamGlowBar.color, Color.clear, Time.deltaTime * blinkSpeed);
		if(teamGlowBar.color.a <= 0.05f){
			bstate = BlinkState_e.fin;
		}
	}
	
	void FadeIn () {
		Color toColor = copColor;
		if(currFaction == Faction_e.spaceCrim){
			toColor = crimColor;
		}
		
		
		teamGlowBar.color = Color.Lerp(teamGlowBar.color, toColor, Time.deltaTime * blinkSpeed);
		if(teamGlowBar.color.a >= 0.95f){
			bstate = BlinkState_e.fout;
		}
	}
}
