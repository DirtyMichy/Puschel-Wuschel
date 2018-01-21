using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour {

	// Use this for initialization
	void Start () {
		iTween.MoveBy(gameObject, iTween.Hash("x", 160f, "easeType", "linear", "loopType", "loop", "time", 30f ));
	}
}
