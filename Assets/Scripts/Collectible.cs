using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private bool collected = false;

	// Update is called once per frame
	void Update ()
    {
        iTween.PunchPosition(gameObject, iTween.Hash("y", 2f));
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if(!collected && c.tag == "Player")
        {
            collected = true;
            c.GetComponent<PlayerController>().powerUpCount++;
            GetComponent<AudioSource>().Play();
            StartCoroutine(Despawn());
        }
    }

    IEnumerator Despawn()
    {
        iTween.ScaleTo(gameObject, new Vector3(0f,0f,0f), 1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
