using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

public class Menu : MonoBehaviour {

    
    public int[] campaignCollectedMuffins; //campaignCollectedMuffins[0] =0; 0= E1M1 0= not finished, 1= finished, 2= everythingFound
    public GameObject[] Level;

	// Use this for initialization
	void Start () {
        
        campaignCollectedMuffins = new int[1];
        PlayerPrefsX.SetIntArray("missionProgress", campaignCollectedMuffins);
        PlayerPrefs.Save();        
        //If this isn't the first play, then there is a saveFile to load
        if (PlayerPrefs.GetInt("startedCampaign") > 0)
        {
            campaignCollectedMuffins = PlayerPrefsX.GetIntArray("missionProgress");
        }
	}
	
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            Startlevel();

        //Everything for the menu navigation
        void MenuNavigation()
        {
            //Menu Navigations -1 = playing
            if (currentMenu > -1)
            {
                GamepadInput.GamePad.Index[] gamePadIndex;
                gamePadIndex = new GamepadInput.GamePad.Index[4];
                gamePadIndex[0] = GamePad.Index.One;
                gamePadIndex[1] = GamePad.Index.Two;
                gamePadIndex[2] = GamePad.Index.Three;
                gamePadIndex[3] = GamePad.Index.Four;
                
                //CharacterSelection
                if (currentMenu == 3)
                {
                    for (int i = 0; i < player.Length; i++)
                    {
                        playerDpad[i] = GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex[i]);
                        
                        if ((GamePad.GetButton(GamePad.Button.A, gamePadIndex[i])) && !playerActive[i])
                        {
                            playerCount++;
                            playerActive[i] = true;
                            playerActiveText[i].text = "Spieler " + (i + 1) + ": \n Aktiv";
                            UIBeepSounds();
                        }
                        if ((GamePad.GetButton(GamePad.Button.B, gamePadIndex[i])) && playerActive[i])
                        {
                            playerCount--;
                            playerActive[i] = false;
                            playerActiveText[i].text = "Spieler " + (i + 1) + ": \n Inaktiv";
                            UIBeepSounds();
                        }                    
                        if (playerDpad[i].y == 0f)
                        {
                            pressedPlayerDpad[i] = false;
                        }
                    }
                    
                    //Keyboard
                    if ((Input.GetKey(KeyCode.A)) && !playerActive[0])
                    {
                        playerCount++;
                        playerActive[0] = true;
                        playerActiveText[0].text = "Spieler " + (1) + ": \n Aktiv";
                        UIBeepSounds();
                    }
                    if ((Input.GetKey(KeyCode.B)) && playerActive[0])
                    {
                        playerCount--;
                        playerActive[0] = false;
                        playerActiveText[0].text = "Spieler " + (1) + ": \n Inaktiv";
                        UIBeepSounds();
                    }
                    if (Input.GetKeyUp(KeyCode.DownArrow))
                    {
                        pressedArrow = false;
                    }
                }
                
                //PlayerAny          
                Vector2 playerAnyDpad = GamePad.GetAxis(GamePad.Axis.Dpad, GamePad.Index.Any);
                
                //################################Navigate down the MainMenu################################
                if (currentMenu == 1 || currentMenu == 2)
                {
                    if ((playerAnyDpad.y < 0f) && !pressedDpad) //&& currentMenu==1 ?
                    {
                        if (currentMenu == 1)
                            currentMainMenuSelection++;
                        if (currentMenu == 2)
                            
                        {
                            if (currentMissionSelection < MissionIcons.Length - 1)
                                currentMissionSelection++;
                            else
                                currentMissionSelection = 0;
                            Debug.Log("Searching: " + currentMissionSelection);
                        }
                        Dpad();
                    }
                    //Navigate up the MainMenu
                    if ((playerAnyDpad.y > 0f) && !pressedDpad)
                    {
                        if (currentMenu == 1)
                            currentMainMenuSelection--;
                        if (currentMenu == 2)
                            
                        {
                            if (currentMissionSelection > 0)
                                currentMissionSelection--;
                            else
                                currentMissionSelection = MissionIcons.Length - 1;
                        }
                        
                        Dpad();
                    }
                    if ((playerAnyDpad.y == 0f) && pressedDpad)
                    {
                        pressedDpad = false;
                    }
                    
                    //################################Keyboardsupport################################
                    if (Input.GetKey(KeyCode.DownArrow) && !pressedArrow)
                    {
                        if (currentMenu == 1)
                            currentMainMenuSelection++;
                        if (currentMenu == 2)
                            
                        {
                            if (currentMissionSelection > 0)
                                currentMissionSelection--;
                            else
                                currentMissionSelection = MissionIcons.Length - 1;
                        }
                        
                        
                        Dpad();
                    }
                    //Keyboardsupport
                    //Navigate up the MainMenu
                    if (Input.GetKey(KeyCode.UpArrow) && !pressedArrow) //&& currentMenu==1 ?
                    {
                        if (currentMenu == 1)
                            currentMainMenuSelection--;
                        if (currentMenu == 2)
                            
                        {
                            if (currentMissionSelection < MissionIcons.Length - 1)
                                currentMissionSelection++;
                            else
                                currentMissionSelection = 0;
                            //Debug.Log("Searching: " + currentMissionSelection);
                        }
                        Dpad();
                    }
                }
                
                //Keyboardsupport
                if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
                {
                    pressedArrow = false;
                }
                
                //Menuselection 0 = StoryMode, 1 = Endless, 2 = Survival, 3 = Exit
                //Modes 1 = StoryMode, 2 = Endless, 3 = Survival
                if ((GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) || Input.GetKey(KeyCode.A)) && currentMenu == 1 && !pressedButton)
                {
                    pressedButton = true;
                    //StoryMode
                    if (currentMainMenuSelection == 0)
                    {
                        GotoMissionMenu();
                    }
                    //Endless
                    if (currentMainMenuSelection == 1)
                    {
                        GotoSelectionScreen();
                    }
                    //SurvivalMode
                    if (currentMainMenuSelection == 2)
                    {
                        GotoSelectionScreen();
                    }
                    //Exit to desktop
                    if (currentMainMenuSelection == 3)
                    {
                        Application.Quit();
                    }
                }
                
                //missionSelection
                if ((GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) || Input.GetKey(KeyCode.A)) && currentMenu == 2 && !pressedButton)
                {
                    pressedButton = true;
                    GotoSelectionScreen();
                }
                
                //Setting to false after Button pressed to prefend fast menu scrolling
                if (GamePad.GetButtonUp(GamePad.Button.A, GamePad.Index.Any) || GamePad.GetButtonUp(GamePad.Button.Y, GamePad.Index.Any) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.Y))
                {
                    pressedButton = false;
                }
            }
            
            //Continue Mission or try again
            if (currentMenu == 4 && (GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) || Input.GetKey(KeyCode.A)) && !pressedButton && fadeFinished)
            {
                pressedButton = true;
                
                if (playingCampaign)
                    GotoMissionMenu();
                else
                    GotoSelectionScreen();
            }
            
            //Get into the MainMenu
            if (currentMenu == 0 && !pressedButton && (GamePad.GetButton(GamePad.Button.A, GamePad.Index.Any) || Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.A)))
            {
                GotoMainMenu();
            }
            
            //Start the game if it isn't already going and the player presses the key
            if (((GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Any) || Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) && currentMenu == 3 && menuActive && (playerActive[0] || playerActive[1] || playerActive[2] || playerActive[3])))
            {
                currentMenu = -1;
                menuActive = false;
                GameStart();
            }
        }
    }

	// Update is called once per frame
	public void Startlevel () {
        SceneManager.LoadScene("E1M1");
	}
}
