using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour 
{
    public bool unlocked = true;
    public bool currentlySelected = false;
    public int collectedMuffins = 0;
	
	// Update is called once per frame
	void Update () 
    {
        if(currentlySelected)
        {
            iTween.PunchScale(gameObject, iTween.Hash("amount", new Vector3(1f,1f,1f), "time", 1f));
        }
	}
}
