using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool activated = false;
    public GameObject flag;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player" && !activated)
        {
            iTween.MoveBy(flag, iTween.Hash("y", 2f, "easeType", "linear", "speed", 1f));
            activated = true;
            GetComponent<AudioSource>().Play();

			Manager.currentGameManager.GetComponent<Manager>().setCheckPoint(gameObject);
        }
    }
}
