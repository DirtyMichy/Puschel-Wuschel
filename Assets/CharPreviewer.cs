using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharPreviewer : MonoBehaviour {

    public int playerNumber = 0;
    public List<GameObject> unlockedCharacters = new List<GameObject>(); 

	// Use this for initialization
	void Start () {
//Beim Awake werden alle spielbaren Chars (bool unlockedChars im menu) als Kinder und disabled erstellt, rigidbody und control müssen deaktiviert werden, 
        //list wird alle unlocked chars erhalten und man iteriert hier durch. Gamepadstuerung in Menu ruft hier methoden auf

        foreach(Transform t in transform)
            unlockedCharacters.Add(t.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
