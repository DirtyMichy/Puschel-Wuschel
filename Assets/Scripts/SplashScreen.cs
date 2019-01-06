using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using System.IO;

//Initialization Scene, when the game is being started the first time, the playerCount has to be set to 1, because when quitting the game during gameplay it could be still set to 2,3 or 4
public class SplashScreen : MonoBehaviour
{
    private int playerCount;
    public int MAXPLAYER = 4;

    // Use this for initialization
    void Start()
    {
        if (File.Exists(Application.dataPath + "/fluffy.plush"))
        {
            Debug.Log("Savegame found");

            SaveLoad.Load();
            Game.current = SaveLoad.savedGames[0];

            Game.current.firstTimeEntering = true;
            Game.current.playerCount = 1; 
            for (int i = 0; i < MAXPLAYER; i++)
            {
                Game.current.playerActive[i] = false;
            }
            SaveLoad.Save();        
        }
    }
	
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown
            || GamePad.GetButtonDown((GamePad.Button.A), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.B), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.Back), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.LeftShoulder), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.LeftStick), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.RightShoulder), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.RightStick), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.Start), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.X), GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.Y), GamePad.Index.Any))
        {
            SceneManager.LoadScene("menu");            
        }
    }
}
