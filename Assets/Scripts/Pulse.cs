using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
	public float time = 0.25f;
    public float size = 1;

	public void Awake ()
    {
        iTween.ScaleBy (gameObject, iTween.Hash ("amount", new Vector3 (size, size, size), "easeType", "easeInOutExpo", "looptype", "pingpong", "time", time));
	}
}