﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    bool activated = false;
	public GameObject flag;
	public GameObject spawnParticles;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player" && !activated)
        {
            iTween.MoveBy(flag, iTween.Hash("y", 2f, "easeType", "linear", "speed", 1f));
            activated = true;
			GetComponent<AudioSource>().Play();
			Instantiate(spawnParticles, transform.position, transform.rotation);

			Manager.currentGameManager.GetComponent<Manager>().setCheckPoint(gameObject);
        }
    }
}
