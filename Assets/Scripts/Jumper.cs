using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used by jumping dudes in the xmas level
public class Jumper : MonoBehaviour {

	public float yForce = 1000f, xForce = 0f, delay = 1f;
	private bool isJumping = false;

	void Update () {
		if(GetComponent<Rigidbody2D> ().velocity.y == 0f && !isJumping)
		{
			isJumping = true;
			Invoke ("Jumping", delay);	
		}
	}

	void Jumping () {
		GetComponent<Rigidbody2D> ().AddForce ( new Vector2 (xForce, yForce));
		isJumping = false;		
	}
}
