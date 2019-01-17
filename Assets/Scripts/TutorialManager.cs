using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public Sprite tutorialSpeechBox;
    public Sprite tutorialSpeechBoxNew;
    private bool showTut = true;

	// Use this for initialization
	void Start () {
		
	}
	
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player" && showTut)
        {
            showTut = true;
            tutorialSpeechBox = tutorialSpeechBoxNew;
        }
    }
}
