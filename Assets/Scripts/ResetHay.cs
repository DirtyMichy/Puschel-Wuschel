using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHay : MonoBehaviour 
{
	void OnTriggerEnter2D (Collider2D c) 
	{
		c.transform.position = new Vector3 (204.9751f, -2.649863f, 0f);
	}
}
