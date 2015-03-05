using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {

	// target to follow
	public GameObject target;
	
	// CAMERA SHAKE CODE
	Vector3 originalCameraPosition;
    public float shakeAmt = 0;
    public bool shaking = false;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 pos = transform.position;
		pos.x = target.transform.position.x;
		pos.z = target.transform.position.z - 5f;
		transform.position = pos;
	}
	
    
    // CAMERA SHAKE CODE
	public void startShaking() 
    {
    	// Don't double shake
    	if(shaking) {
    		return;
		}
		
		// commence the shakedown
		shaking = true;
		// remember old camera position
		originalCameraPosition = transform.position;
        
        InvokeRepeating("CameraShake", 0, .01f);
        Invoke("StopShaking", 0.1f);

    }
    void CameraShake()
    {
    	shaking = true;
        if(shakeAmt>0) 
        {
            // float quakeAmt = Random.value*shakeAmt*2 - shakeAmt;
            Vector3 pp = transform.position;
            pp.x += Random.value*shakeAmt*2 - shakeAmt;
            pp.z += Random.value*shakeAmt*2 - shakeAmt;
            transform.position = pp;
        }
    }
    void StopShaking()
    {
        CancelInvoke("CameraShake");
        transform.position = originalCameraPosition;
    	shaking = false;
    }
	
}
