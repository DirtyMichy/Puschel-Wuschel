using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStats : MonoBehaviour 
{
    public bool unlocked = true;            //True if the level is unlocked, first level is always unlocked
    public bool currentlySelected = false;  //if its currently selected it pulses
    public int collectedMuffins = 0;        //counts all collected muffins in this scene
    public GameObject muffinText;           //shows all collected muffins
    private int MAXMUFFINS = 10;            //maximum collectible muffins, maybe always 10?

    void Awake ()
    {
        if(muffinText.GetComponent<Text>())
        muffinText.GetComponent<Text>().text = collectedMuffins + "/" + MAXMUFFINS;
    }

	// Update is called once per frame
	void Update () 
    {
        if(currentlySelected)
        {
            iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(1f,1f,1f), "time", 1f));
        }
	}
}
