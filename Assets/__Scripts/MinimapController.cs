using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BlipTuple {	
	public BlipTuple(Transform _playerTransform, GameObject _blipObj)
	{
		this.playerTransform = _playerTransform;
		this.blipObj = _blipObj;
	}
	
	public Transform playerTransform;
	public GameObject blipObj;
};

public class MinimapController : MonoBehaviour {
	
	// need some list of other players
	PlayerStats myStats;
	Transform myPlayer;
	LinkedList<Transform> otherTrans;
	LinkedList<Transform> other;
	
	public List<BlipTuple> blips;
	
	public GameObject copBlipPrefab;
	public GameObject crimBlipPrefab;
	
	SpriteRenderer sprite;
	float actualRadius;
	public float mapViewRadius = 10f;
		
	void Awake() {
		blips = new List<BlipTuple> ();
		myStats = transform.root.GetComponentInChildren<PlayerStats>();
	}
	
	void Start () {
		
		actualRadius = GetComponent<RectTransform>().rect.width / 2;
		print("Actual Radious: " + actualRadius);
		
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
				print("Found my blip");
			}
		}
		print(blips.Count);
		
	}
	
	void FixedUpdate () {
		foreach(BlipTuple tuple in blips) {
			
			// get vector from myPlayer to other player
			Vector3 toOther = tuple.playerTransform.position - myPlayer.position;
			print("raw: " + toOther);
			
			// magnitude = distance
			// limit between 0 and mapViewRadius
			float magnitude = Mathf.Clamp(toOther.magnitude, 0f, Mathf.Abs(mapViewRadius));
			// print("raw magnitude" + magnitude);
			
			// determine minimap adjusted magnitude
			magnitude /= mapViewRadius;
			magnitude *=  actualRadius;
			// print("Normalized magnitude: " + magnitude);
			toOther.Normalize();
			// print("normalized dir: " + toOther);
			
			// apply minimap-adjusted magnitude
			toOther *= magnitude;
			
			// print("Minimap adjusted:" + toOther);
			
			// adjust from x-z to x-y
			Vector3 newPos = new Vector3(toOther.x, toOther.z, 0f);
			// set position
			tuple.blipObj.GetComponent<RectTransform>().anchoredPosition = newPos;
		}
	}
	
}