using UnityEngine;
using System.Collections;

public class MaterialScaler : MonoBehaviour {
	
 	public float factor = 1.0f;
 
 	void Start() {
    	MeshFilter mf = GetComponent<MeshFilter>();
			if (mf) {
				Vector3 bounds = mf.mesh.bounds.size;
				Vector3 size = Vector3.Scale(bounds, transform.localScale) * factor;
				size.y = size.z;
				 
				GetComponent<Renderer>().material.mainTextureScale = size;
			}
	 }
	 
}
