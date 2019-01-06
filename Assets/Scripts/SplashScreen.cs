using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using UnityEngine.UI;
using System.IO;

public class SplashScreen : MonoBehaviour {

    //Erste Szene die beim Start geladen wird, hier werden Dinge resettet, nachher kommt man hier nichtmehr wieder rein außer man startet das Spiel neu. 

    int playerCount;

	// Use this for initialization
	void Start () {
        if (File.Exists(Application.dataPath + "/fluffy.plush"))
        {
            Debug.Log("Savegame found");

            SaveLoad.Load();
            Game.current = SaveLoad.savedGames[0];

            Game.current.firstTimeEntering = true;
            Game.current.playerCount = 1;         
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown 
            || GamePad.GetButtonDown((GamePad.Button.A) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.B) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.Back) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.LeftShoulder) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.LeftStick) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.RightShoulder) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.RightStick) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.Start) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.X) , GamePad.Index.Any)
            || GamePad.GetButtonDown((GamePad.Button.Y) , GamePad.Index.Any))
        {
            SceneManager.LoadScene("menu");            
        }
	}
}
