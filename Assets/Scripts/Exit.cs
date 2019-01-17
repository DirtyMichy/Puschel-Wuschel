using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Exit : MonoBehaviour
{
    private bool exited = false;
    private bool loadNextLevel = false;

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Player")
            ExitLevel();
    }

    void ExitLevel()
    {
        if (!exited)
        {
            exited = true;

            //A saveFile should exist because its being created in the menu before a mission
            if (File.Exists(Application.dataPath + "/fluffy.plush"))
            {
                Debug.Log("Savegame found");

                SaveLoad.Load();
                Game.current = SaveLoad.savedGames[0];
            }

            int sceneNameAsInt = int.Parse(SceneManager.GetActiveScene().name);

            Game.current.collected[sceneNameAsInt] = Manager.currentGameManager.GetComponent<Manager>().collectedMuffins; 

            SaveLoad.Save();
            if ((sceneNameAsInt + 1) < ((SceneManager.sceneCountInBuildSettings) - 1) && loadNextLevel)
            {
                Debug.Log((sceneNameAsInt + 1) + " less than " + (SceneManager.sceneCountInBuildSettings - 1));
                SceneManager.LoadScene((sceneNameAsInt + 1).ToString());
            }
            else
            {   
                SceneManager.LoadScene("Menu");
            }
        }
    }

    void Update()
    {
        //Cheat for debugging
        if (Input.GetKeyDown(KeyCode.O) && !exited)
        {
            ExitLevel();
        }
    }
}