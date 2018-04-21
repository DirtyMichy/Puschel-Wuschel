using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour {

	public float yForce = 1000f, xForce = 0f, delay = 1f;
	private bool isJumping = false;

	void Update () {
		if(transform.rotation.eulerAngles.z < 1 && transform.rotation.eulerAngles.z > -1 && GetComponent<Rigidbody2D> ().velocity.y == 0f && !isJumping)
		{
			Debug.Log (GetComponent<Rigidbody2D> ().velocity);
			Debug.Log (transform.rotation);
			isJumping = true;
			Invoke ("Jumping", delay);	
		}
	}

	void Jumping () {
		GetComponent<Rigidbody2D> ().AddForce ( new Vector2 (xForce, yForce));
		isJumping = false;
		//Invoke ("Jumping", delay);		
	}
}
