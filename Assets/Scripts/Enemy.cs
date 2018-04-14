using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour 
{
    public float rangeY = 1f;
    public float speed = 4f;
    public float delay = 1f;
    public GameObject web;
    private Transform origin;
    public bool killable = true;

    void Awake()
	{
		origin = transform;
        if(web)
        {
            web.gameObject.transform.parent = null;
            iTween.MoveAdd(gameObject, iTween.Hash("y", rangeY, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
            iTween.ScaleAdd(web, iTween.Hash("y", 45*Math.Abs(rangeY), "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
        }
    }

    //Some enemies can be killed by superpowers
    void OnTriggerEnter2D(Collider2D c)
    {
            if(c.tag == "Player" && killable)
                if(c.GetComponent<PlayerController>().powerUpActivated)
                    StartCoroutine(Die());
    }

    //Enemy getting killed
    public IEnumerator Die()
    {
        killable = false;
        if(web)
            iTween.Stop(web);
        AudioSource[] sounds = GetComponents<AudioSource>();

        if(sounds.Length > 0)
            sounds[0].Play();

        if(!GetComponent<Rigidbody2D>())
            gameObject.AddComponent<Rigidbody2D>();

        gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 1000f);

        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
    }
}
