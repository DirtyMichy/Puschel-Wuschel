using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used in menu scrren to show the chosen character
public class CharPreviewer : MonoBehaviour {

    //public int playerNumber = 0;        //Owner of this charPreviewer
    public GameObject[] allCharacters;
    public bool[] isUnlocked; 
	
    public void SelectChar(int selection)
    {  
        if(transform.childCount > 0)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if(i != selection)
                    transform.GetChild(i).gameObject.SetActive(false);
                else
                    transform.GetChild(selection).gameObject.SetActive(true);
            }
        }
    }
}
