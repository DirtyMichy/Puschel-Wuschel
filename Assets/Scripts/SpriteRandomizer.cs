using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
    public Sprite[] AllSprites;

	// Use this for initialization
	void Start ()
    {
        GetComponent<SpriteRenderer>().sprite = AllSprites[Random.Range(0, AllSprites.Length)];
    }	
}
