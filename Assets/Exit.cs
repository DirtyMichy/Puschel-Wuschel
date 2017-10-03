using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour 
{
    private bool exited = false;

    void OnTriggerEnter2D(Collider2D c)
    {
        if(!exited && c.tag == "Player")
        {
            exited = true;
            int[] campaignCollectedMuffins = PlayerPrefsX.GetIntArray("collectedMuffins");
            int sceneNameAsInt = int.Parse(SceneManager.GetActiveScene().name);

            campaignCollectedMuffins[sceneNameAsInt]=Camera.main.GetComponent<Manager>().collectedMuffins;
            PlayerPrefsX.SetIntArray("collectedMuffins", campaignCollectedMuffins);

            PlayerPrefs.Save();
            SceneManager.LoadScene("Menu");
        }
    }
}
