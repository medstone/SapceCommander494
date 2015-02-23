using UnityEngine;
using System.Collections;

public enum faction{
	cop,
	crim
}

public class robotAI : MonoBehaviour {
	public faction fact;
	public Vector3 direction;
	public float speed = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 vel;
		vel.x = direction.x * speed;
		vel.y = rigidbody.velocity.y;
		vel.z = direction.z * speed;
		this.rigidbody.velocity = vel;
	}
}
