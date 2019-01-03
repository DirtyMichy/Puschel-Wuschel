using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHide : MonoBehaviour
{
    public GameObject objectToShow;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            objectToShow.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.tag == "Player")
        {
            objectToShow.SetActive(false);
        }
    }
}
