using UnityEngine;
using System.Collections;

public class laser_sights : MonoBehaviour {

	LineRenderer line;
	RaycastHit hit;
	public Material line_mat;

	// Use this for initialization
	void Start () {

		line = GetComponent<LineRenderer> ();
		line.SetVertexCount (2);
		//line.renderer.material = line_mat;
		line.SetWidth (0.1f, 0.1f);
		line.SetColors (Color.red, Color.red);
		line.enabled = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		Ray ray = new Ray (transform.position, transform.forward);
		if (Physics.Raycast (ray, out hit, 1000f)) {
			line.SetPosition(0,this.transform.position);
			line.SetPosition(1, hit.point + hit.normal);

		}
	
	}
}
