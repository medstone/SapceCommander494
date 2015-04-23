using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {

	AudioSource aud;
	ProgressBar prog;
	public float pitch;

	// Use this for initialization
	void Start () {
		aud = this.gameObject.GetComponent<AudioSource> ();
		prog = GameObject.Find ("ProgressBar").GetComponent<ProgressBar> ();
	}
	
	// Update is called once per frame
	void Update () {
		pitch = 1+((prog.currentTime)/450);
		aud.pitch = pitch;
	}
}
