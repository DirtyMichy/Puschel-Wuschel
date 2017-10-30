using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharPreviewer : MonoBehaviour {

    //public int playerNumber = 0;        //Owner of this charPreviewer
    public GameObject[] allCharacters;
    public bool[] isUnlocked;
    //public List<GameObject> unlockedCharacters = new List<GameObject>(); 
	
    public void SelectChar(int selection)
    {   
        if(transform.childCount > 0)
        {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
            transform.GetChild(selection).gameObject.SetActive(true);
        }
    }
}
