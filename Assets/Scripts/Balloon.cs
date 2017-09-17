using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.GetComponent<Rigidbody2D>().gravityScale > 0f)
        {
            //StartCoroutine(c.GetComponent<PlayerController>().BalloonFly());
            if(c.tag == "Player")
            StartCoroutine(Fly(c));
        }
    }

    IEnumerator Fly(Collider2D Flyer)
    {
        GetComponent<AudioSource>().Play();
        Flyer.GetComponent<Rigidbody2D>().gravityScale = -0.5f;
        Flyer.GetComponent<PlayerController>().Balloons.SetActive(true);
        yield return new WaitForSeconds(2f);
        Flyer.GetComponent<Rigidbody2D>().gravityScale = 8;
        Flyer.GetComponent<PlayerController>().Balloons.SetActive(false);
    }
}
