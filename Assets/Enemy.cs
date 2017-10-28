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
    
    void Awake()
    {
        if(web)
        {
            origin = transform;
            web.gameObject.transform.parent = null;
            iTween.MoveAdd(gameObject, iTween.Hash("y", rangeY, "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
            iTween.ScaleAdd(web, iTween.Hash("y", 45*Math.Abs(rangeY), "easeType", "easeInOutExpo", "loopType", "pingPong", "time", speed, "delay", delay));
        }
    }
    /*
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            Destroy(c.transform.root.gameObject);
        }        
    }
    */
}
