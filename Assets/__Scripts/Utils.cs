using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Bounds and bezier curve code graciously borrowed from prof. Gibson's book - Chapter 30

public enum BoundsTest{
	center, // is the center of the GameObject on screen?
	onScreen,// Are the bounds entirely on screen?
	offScreen // Are the bounds entirely off screen?
}

public class Utils : MonoBehaviour {

	public static int CopLayer(){
		return 9;
	}

	public static int CrimLayer(){
		return 10;
	}

	public static int CopBarrierLayer(){
		return 11;
	}

	public static int CrimBarrierLayer(){
		return 12;
	}

	public static int CopProjectileLayer(){
		return 13;
	}

	public static int CrimProjectileLayer(){
		return 14;
	}
	
	public static int CopWepBarrierLayer(){
		return 17;
	}

	public static int CrimWepBarrierLayer(){
		return 18;
	}

	
		
		// ****************Bounds stuff***********************
		
		public static Bounds BoundsUnion(Bounds b0, Bounds b1){
			// if one of the bounds is Vector3.zero, ignore it
			if (b0.size == Vector3.zero && b1.size != Vector3.zero)
				return (b1);
			else if (b0.size != Vector3.zero && b1.size == Vector3.zero)
				return (b0);
			else if (b0.size == Vector3.zero && b1.size == Vector3.zero)
				return (b0);
			// Stretch b0 to include the b1.min and b1.max
			b0.Encapsulate (b1.min);
			b0.Encapsulate (b1.max);
			return (b0);
		}
		
		public static Bounds CombineBoundsOfChildren(GameObject go){
			// create an empty bounds
			Bounds b = new Bounds (Vector3.zero, Vector3.zero);
			if (go.GetComponent<Renderer>() != null) {
				b = BoundsUnion (b, go.GetComponent<Renderer>().bounds);		
			}
			if (go.GetComponent<Collider>() != null) {
				b = BoundsUnion (b, go.GetComponent<Collider>().bounds);		
			}
			// recursively iterate through each child of this gameObject.transform
			foreach (Transform t in go.transform) {
				// expand b to their bounds
				b = BoundsUnion (b, CombineBoundsOfChildren(t.gameObject));
			}
			return b;
		}
		
		static public Bounds camBounds{
			get{
				// if _camBounds hasn't been set yet
				if (_camBounds.size == Vector3.zero){
					// set camera bounds using the default camera
					SetCameraBounds();
				}
				return (_camBounds);
			}
		}
		
		
		static private Bounds _camBounds;
		
		static public void SetCameraBounds(Camera cam = null){
			// if no camera was passed here, use main camera
			if (cam == null) cam = Camera.main;
			// ASSUMING THAT CAMERA IS ORTHOGRAPHIC AND HAS Vector3.zero rotation
			
			Vector3 topLeft = new Vector3 (0, 0, 0);
			Vector3 bottomRight = new Vector3 (Screen.width, Screen.height, 0);
			
			// convert to world coords
			Vector3 boundTLN = cam.ScreenToWorldPoint (topLeft);
			Vector3 boundBRF = cam.ScreenToWorldPoint (bottomRight);
			
			// Adjust their zs to be at the near and far Camera clipping panes
			boundTLN.z += cam.nearClipPlane;
			boundBRF.z += cam.farClipPlane;
			
			// Find the center of the Bounds
			
			Vector3 center = (boundTLN + boundBRF) / 2f;
			_camBounds = new Bounds (center, Vector3.zero);
			// Expand _camBounds to encapsulate the extents
			_camBounds.Encapsulate (boundTLN);
			_camBounds.Encapsulate (boundBRF);
		}
		
		// checks to see whether the Bounds bnd are within the camBounds
		public static Vector3 ScreenBoundsCheck(Bounds bnd, BoundsTest test = BoundsTest.center){
			return (BoundsInBoundsCheck (camBounds, bnd, test));
		}
		
		public static Vector3 BoundsInBoundsCheck(Bounds bigB, Bounds lilB, 
		                                          BoundsTest test = BoundsTest.onScreen){
			// Get the center of lilB
			Vector3 pos = lilB.center;
			
			// Initialize the offset at 0,0,0
			Vector3 off = Vector3.zero;
			
			switch (test) {
				// the center test determines what offset would have to be applied 
				// to lilB to move its center back inside bigB
			case BoundsTest.center:
				if (bigB.Contains(pos)){
					return (Vector3.zero);
				}
				if (pos.x > bigB.max.x){
					off.x = pos.x - bigB.max.x;
				}
				else if (pos.x < bigB.min.x){
					off.x = pos.x - bigB.min.x;
				}
				if (pos.y > bigB.max.y){
					off.y = pos.y - bigB.max.y;
				}
				else if (pos.y < bigB.min.y){
					off.y = pos.y - bigB.min.y;
				}
				if (pos.z > bigB.max.z){
					off.z = pos.z - bigB.max.z;
				}
				else if (pos.z < bigB.min.z){
					off.z = pos.z - bigB.min.z;
				}
				return off;
				
				// The onScreen test determines what off would have to be applied
				// to keep all of lilB nside bigB
			case BoundsTest.onScreen:
				if (bigB.Contains(lilB.min) && bigB.Contains(lilB.max)){
					return Vector3.zero;
				}
				if (lilB.max.x > bigB.max.x){
					off.x = lilB.max.x - bigB.max.x;
				}
				else if (lilB.min.x < bigB.min.x){
					off.x = lilB.min.x - bigB.min.x;
				}
				if (lilB.max.y > bigB.max.y){
					off.y = lilB.max.y - bigB.max.y;
				}
				else if (lilB.min.y < bigB.min.y){
					off.y = lilB.min.y - bigB.min.y;
				}	
				if (lilB.max.z > bigB.max.z){
					off.z = lilB.max.z - bigB.max.z;
				}
				else if (lilB.min.z < bigB.min.z){
					off.z = lilB.min.z - bigB.min.z;
				}
				return off;
				
				// the offScreen test determines what off would need to be applied to move any tiny part
				// of lilB inside of bigB
			case BoundsTest.offScreen:
				bool cMin = bigB.Contains(lilB.min);
				bool cMax = bigB.Contains(lilB.max);
				if (cMin || cMax){
					return Vector3.zero;
				}
				if (lilB.min.x > bigB.max.x){
					off.x = lilB.min.x - bigB.max.x;
				}
				else if (lilB.max.x < bigB.min.x){
					off.x = lilB.max.x - bigB.min.x;
				}
				if (lilB.min.y > bigB.max.y){
					off.y = lilB.min.y - bigB.max.y;
				}
				else if (lilB.max.y < bigB.min.y){
					off.y = lilB.max.y - bigB.min.y;
				}
				if (lilB.min.z > bigB.max.z){
					off.z = lilB.min.z - bigB.max.z;
				}
				else if (lilB.max.z < bigB.min.z){
					off.z = lilB.max.z - bigB.min.z;
				}
				return off;
			}
			return Vector3.zero;
		}
		
		//===============Bezier curves==================================
		
		// allows for extrapolaiton, (unlike normal Unity Lerp fn)
		static public Vector3 Lerp (Vector3 vFrom, Vector3 vTo, float u){
			Vector3 res = (1 - u) * vFrom + u * vTo;
			return res;
		}
		
		// can use this fn to have any number of Bezier curve points
		static public Vector3 Bezier (float u, List<Vector3> vList){
			// base case
			// if only one element, return it
			if (vList.Count == 1) {
				return vList[0];		
			}
			// create vListR, which is all ComputeBufferType the 0th elt of vList
			List<Vector3> vListR = vList.GetRange (1, vList.Count - 1);
			
			// and vListL, which is all but the last elt of vList
			List<Vector3> vListL = vList.GetRange (0, vList.Count - 1);
			
			// result is the Lerp of the Bezier of these two shorter lists
			Vector3 res = Lerp (Bezier (u, vListL), Bezier (u, vListR), u);
			// recursive calls splitting the lists until there is only one elt in each
			return res;
		}
		
		// this version lets you pass in an array of vector3s as inputs, which are made into a list
		static public Vector3 Bezier(float u, params Vector3[] vecs){
			return (Bezier (u, new List<Vector3> (vecs)));
		}
		

}
