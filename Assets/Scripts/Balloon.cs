using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//balloons apply a negative gravity so the collider will start to fly
public class Balloon : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.GetComponent<Rigidbody2D>().gravityScale > 0f)
        {
			if(c.tag == "Player" && !c.GetComponent<PlayerController>().powerUpActivated)
            StartCoroutine(Fly(c));
        }
    }

    IEnumerator Fly(Collider2D Flyer)
    {
        GetComponent<AudioSource>().Play();
        Flyer.GetComponent<Rigidbody2D>().velocity = new Vector2 (0f, 0f);;
        Flyer.GetComponent<Rigidbody2D>().gravityScale = -0.5f;
        Flyer.GetComponent<PlayerController>().Balloons.SetActive(true);
        yield return new WaitForSeconds(2f);
        Flyer.GetComponent<Rigidbody2D>().gravityScale = 8;
        Flyer.GetComponent<PlayerController>().Balloons.SetActive(false);
    }
}
