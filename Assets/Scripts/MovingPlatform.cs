using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public float rangeX = 4f;
    public float rangeY = 0f;
    public float speed = 4f;

    // Update is called once per frame
    void Update ()
    {
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
}
