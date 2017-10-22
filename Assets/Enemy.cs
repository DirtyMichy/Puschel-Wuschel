using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public float rangeY = 1f;
    public float speed = 4f;
    public float delay = 1f;
    public GameObject web;
    private Transform origin;
    
    void Awake()
    {
        origin = transform;
        web.gameObject.transform.parent = null;
        iTween.MoveAdd(gameObject, iTween.Hash("y", rangeY, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
        StartCoroutine(WebScaler());
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            Destroy(c.transform.root.gameObject);
        }        
    }

    IEnumerator WebScaler()
    {
        for(int i = 0)
        {

        }
    }
}
