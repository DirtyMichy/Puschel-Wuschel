using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float rangeX = 4f;
    public float rangeY = 0f;
    public float speed = 4f;
    public float delay = 1f;
    public bool doesFall = false;
    private Transform origin;

    void Awake()
    {
        origin = transform;
    }
    // Update is called once per frame
    void Update ()
    {
        if(!doesFall)
        iTween.MoveBy(gameObject, iTween.Hash("y", rangeY, "x", rangeX, "loopType", "pingPong", "easeType", "easeInOutExpo", "speed", speed));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

        private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player" && doesFall)
        {
            StartCoroutine(ColliderCooldown());
            iTween.PunchPosition(gameObject, iTween.Hash("y", rangeY, "easeType", "easeInOutExpo", "time", speed, "delay", delay));
        }
        else
            iTween.PunchPosition(gameObject, iTween.Hash("y", .5f, "time", 1f));
    }

    IEnumerator ColliderCooldown()
    {
        yield return new WaitForSeconds(1f);        
        GetComponent<EdgeCollider2D>().enabled=false;
        yield return new WaitForSeconds(1f);        
        GetComponent<EdgeCollider2D>().enabled=true;
        transform.position = origin.position;
    }
}
