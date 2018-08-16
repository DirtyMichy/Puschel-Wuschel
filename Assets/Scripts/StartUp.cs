using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif 

#if UNITY_EDITOR
[InitializeOnLoad]
public class Startup 
{
	static Startup()
	{
		Debug.Log("Up and running");
	}
}
#endif 
