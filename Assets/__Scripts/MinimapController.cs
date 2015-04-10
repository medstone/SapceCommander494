using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BlipTuple {	
	public BlipTuple(Transform _targetTransform, GameObject _blipObj)
	{
		this.playerTransform = _targetTransform;
		this.blipObj = _blipObj;
	}
	
	public Transform playerTransform;
	public GameObject blipObj;
};

public class MinimapController : MonoBehaviour {
	
	// need some list of other players
	PlayerStats myStats;
	Transform myPlayer;
	
	public List<Transform> controlPoints;
	public List<BlipTuple> blips;
	
	public GameObject copBlipPrefab;
	public GameObject crimBlipPrefab;
	public GameObject controlBlip;
	
	SpriteRenderer sprite;
	float actualRadius;
	public float mapViewRadius = 10f;
		
	void Awake() {
		blips = new List<BlipTuple> ();
		myStats = transform.root.GetComponentInChildren<PlayerStats>();
	}
	
	void Start () {
		
		actualRadius = GetComponent<RectTransform>().rect.width / 2;
		
		// create a blip for every player
		GameObject[] gos = GameObject.FindGameObjectsWithTag("Actor");
		foreach(GameObject go in gos) {
			
			// get the player's stats
			PlayerStats p = go.GetComponent<PlayerStats>();
			GameObject blip;
			
			// make copBlip or crimBlip
			if(p.team == Faction_e.spaceCop) {
				blip = Instantiate(copBlipPrefab) as GameObject;
			}
			else {
				blip = Instantiate(crimBlipPrefab) as GameObject;
			}
			blip.transform.SetParent(transform, false);
			
			// add other player's blips to list
			if(myStats.player != p.player) {
				blips.Add(new BlipTuple(go.transform, blip));
			}
			else {
				// hold onto this for reference later on
				myPlayer = go.transform;
			}
		}
		
		// Create blips for control points
		// NOTE: EXPECTS ITEMS WERE PLACED IN LIST IN INSPECTOR IN ORDER A->B->STEERING
		char pointChar = 'A';
		foreach(Transform trans in controlPoints) {
			// create the blip
			GameObject cblip = Instantiate(controlBlip) as GameObject;
			cblip.transform.SetParent(transform, false);
			
			// set it's text to be A B or C
			Text blipText = cblip.GetComponent<Text>();
			if(!blipText) continue; // safety
			blipText.text = "" + pointChar;
			++pointChar;
			
			blips.Add(new BlipTuple(trans, cblip));
			
			
		}
	}
	
	void FixedUpdate () {
		foreach(BlipTuple tuple in blips) {
			
			// get vector from myPlayer to other player
			Vector3 toOther = tuple.playerTransform.position - myPlayer.position;
			
			// magnitude = distance
			// limit between 0 and mapViewRadius
			float magnitude = Mathf.Clamp(toOther.magnitude, 0f, Mathf.Abs(mapViewRadius));
			// print("raw magnitude" + magnitude);
			
			// percentage of radius to use
			magnitude /= mapViewRadius;
			
			// radius used
			magnitude *=  actualRadius;
			
			// normalize to get just direction
			toOther.Normalize();
			
			// apply minimap-adjusted magnitude
			toOther *= magnitude;
			
			// adjust from x-z to x-y
			Vector3 newPos = new Vector3(toOther.x, toOther.z, 0f);
			
			// set position
			tuple.blipObj.GetComponent<RectTransform>().anchoredPosition = newPos;
		}
	}
	
}