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
            Camera.main.GetComponent<Manager>().collectedMuffins++;
            c.GetComponent<PlayerController>().powerUpCount++;
            if(c.GetComponent<PlayerController>().powerUpCount<10)
                c.GetComponent<PlayerController>().Body.transform.localScale+=new Vector3(0.025f,0.075f,0f);
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
