using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Snail Wanted trigger
public class WesternSpecialTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] muffins;

    [SerializeField]
    private Sprite wanted;

    [SerializeField]
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!triggered)
        {
            triggered = true;

            for (int i = 0; i < muffins.Length; i++)
            {
                muffins[i].transform.localScale = new Vector3(0f, 0f, 0f);
                muffins[i].SetActive(true);
                iTween.ScaleTo(muffins[i], new Vector3(1f, 1f, 1f), 1f);
            }

            GetComponent<SpriteRenderer>().sprite = wanted;
        }
    }
}