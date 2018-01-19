using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D c)
    {
            if (c.tag != "Player")
            {
            //    GetComponent<AudioSource>().Play();
                Destroy(c.transform.root.gameObject);
            }
    }
}
