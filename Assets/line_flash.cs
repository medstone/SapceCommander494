using UnityEngine;
using System.Collections;

public class line_flash : MonoBehaviour {

	Color start;
	Color black;
	LineRenderer line;
	float timer = 0.0f;

	// Use this for initialization
	void Start () {
		line = this.gameObject.GetComponent<LineRenderer> ();
		start = new Color (1f, 0f, 0f, 1f);
		black = new Color (0f, 0f, 0f, 1f);
		line.SetColors (start, black);
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		float start_shade = Mathf.Cos (5*timer);
		float black_shade = Mathf.Sin (5*timer);
		start = new Color (start_shade, 0f, 0f, 1f);
		black = new Color (black_shade, 0f, 0f, 1f);
		line.SetColors (start, black);
	}
}
