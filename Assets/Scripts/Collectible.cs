using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour 
{
	private bool collected = false;
	public GameObject spawnParticles;

	// Update is called once per frame
	void Update ()
    {
        iTween.PunchPosition(gameObject, iTween.Hash("y", 2f));
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if(!collected && c.tag == "Player")
        {
			if (c.gameObject.GetComponent<PlayerController> ().alive) {
				collected = true;

				Manager.currentGameManager.GetComponent<Manager> ().collectedMuffins++;

				c.GetComponent<PlayerController> ().powerUpCount++;
                c.transform.Find("PowerUpText").GetComponent<TextMesh>().text = c.GetComponent<PlayerController> ().powerUpCount+"";

				if (c.GetComponent<PlayerController> ().powerUpCount < 10)
					c.GetComponent<PlayerController> ().Body.transform.localScale += new Vector3 (0.05f, 0.15f, 0f);
				GetComponent<AudioSource> ().Play ();

				StartCoroutine (Despawn ());
			}
        }
    }

    IEnumerator Despawn()
	{
		Instantiate(spawnParticles, transform.position, transform.rotation);
        iTween.ScaleTo(gameObject, new Vector3(0f,0f,0f), 1f);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
