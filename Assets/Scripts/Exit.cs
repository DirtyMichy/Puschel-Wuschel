using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Exit : MonoBehaviour 
{
    private bool exited = false;
    //private int[] campaignCollectedMuffins;

    void OnTriggerEnter2D(Collider2D c)
    {
        if(!exited && c.tag == "Player")
        {
            exited = true;

			//A saveFile should exist because its being created in the menu before a mission
			if (File.Exists (Application.dataPath + "/fluffy.plush")) 
			{
				Debug.Log ("Savegame found");

				SaveLoad.Load ();
				Game.current = SaveLoad.savedGames[0];
			}

			int sceneNameAsInt = int.Parse(SceneManager.GetActiveScene().name);

			Game.current.collected[sceneNameAsInt]=Camera.main.GetComponent<Manager>().collectedMuffins; 

			/*
            if(PlayerPrefsX.GetIntArray("collectedMuffins").Length > 0)
                campaignCollectedMuffins = PlayerPrefsX.GetIntArray("collectedMuffins");
            else
            {
                campaignCollectedMuffins = new int[SceneManager.sceneCountInBuildSettings];
            }
			*/

            //campaignCollectedMuffins[sceneNameAsInt]=Camera.main.GetComponent<Manager>().collectedMuffins;
            //PlayerPrefsX.SetIntArray("collectedMuffins", campaignCollectedMuffins);

            //PlayerPrefs.Save();
			SaveLoad.Save();
            if((sceneNameAsInt+1) < ((SceneManager.sceneCountInBuildSettings)-1))
                SceneManager.LoadScene((sceneNameAsInt+1).ToString());
            else                
                SceneManager.LoadScene("Menu");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && !exited)
        {
            exited = true;

			//A saveFile should exist because its being created in the menu before a mission
			if (File.Exists (Application.dataPath + "/fluffy.plush")) 
			{
				Debug.Log ("Savegame found");

				SaveLoad.Load ();
				Game.current = SaveLoad.savedGames[0];
			}

			int sceneNameAsInt = int.Parse(SceneManager.GetActiveScene().name);

			Game.current.collected[sceneNameAsInt]=Camera.main.GetComponent<Manager>().collectedMuffins; 
            /*
            if(PlayerPrefsX.GetIntArray("collectedMuffins").Length > 0)
                campaignCollectedMuffins = PlayerPrefsX.GetIntArray("collectedMuffins");
            else
            {
                campaignCollectedMuffins = new int[SceneManager.sceneCountInBuildSettings];
            }
            
            int sceneNameAsInt = int.Parse(SceneManager.GetActiveScene().name);
            
            campaignCollectedMuffins[sceneNameAsInt]=Camera.main.GetComponent<Manager>().collectedMuffins;
            PlayerPrefsX.SetIntArray("collectedMuffins", campaignCollectedMuffins);
            
            PlayerPrefs.Save();
            SceneManager.LoadScene("Menu");
            */
        }  
    }
}
