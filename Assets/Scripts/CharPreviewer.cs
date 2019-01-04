using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used in menu scrren to show the chosen character
public class CharPreviewer : MonoBehaviour
{
    public GameObject[] allCharacters;

    public void SelectChar(int selection)
    {  
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i != selection)
                    transform.GetChild(i).gameObject.SetActive(false);
                else
                    transform.GetChild(selection).gameObject.SetActive(true);
            }
        }
    }
}
