using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XmasLights : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (Blinki());
	}

	IEnumerator Blinki()
	{
		yield return new WaitForSeconds(Random.Range(1f,2f));
		while(true)
		{
			GetComponent<SpriteRenderer> ().color = new Color (1f, 0f, 0f);
			yield return new WaitForSeconds(Random.Range(1f,2f));
			GetComponent<SpriteRenderer> ().color = new Color (0f, 1f, 0f);
			yield return new WaitForSeconds(Random.Range(1f,2f));
			GetComponent<SpriteRenderer> ().color = new Color (0f, 0f, 1f);
			yield return new WaitForSeconds(Random.Range(1f,2f));
			GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 0f);
			yield return new WaitForSeconds(Random.Range(1f,2f));
		}
	}
}
