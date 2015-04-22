using UnityEngine;
using System.Collections;

public class SpaceToPlayMain : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel("_MainLevel");
		}
	}
}
