using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class ThemeAdjustment : MonoBehaviour
{
    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(SceneManager.GetActiveScene().name + "/" + SceneManager.GetActiveScene().name + "_" + gameObject.name.Remove(gameObject.name.LastIndexOf(" (")));        
    }
}