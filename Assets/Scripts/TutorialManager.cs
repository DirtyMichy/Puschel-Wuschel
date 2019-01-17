using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public SpriteRenderer tutorialSpeechBox;
    public Sprite tutorialSpeechBoxNew;
    private bool showTut = true;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player" && showTut)
        {
            showTut = false;
            tutorialSpeechBox.sprite = tutorialSpeechBoxNew;
        }
    }
}
