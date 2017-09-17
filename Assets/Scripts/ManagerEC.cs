using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GamepadInput;

//This is the manager for the game
public class ManagerEC : MonoBehaviour
{    
    int startedCampaign = 0;                    //0 means no saveFile exists
    
    public int playerCount = 0;                 //PlayerCounter 0 = no active Player    
    public int currentMainMenuSelection = 0;    //0 = StoryMode, 1 = Survival, 2 = Exit
    public int currentMissionSelection = 0;
    
    public int currentMenu = 0;                 //-1 = None, 0 = TitleScreen, 1 = ModeSelection, 2 = MissionSelection, 3 = CharacterSelection, 4 = Highscore
    
    public int[] missionProgress;     //[0] = E1M1, 0 = Disabled, 1 = Enabled, 2 = Won 
    public int endlessProgress;                 //Every x Mission the enemytypes get mixed again

    public static Manager current;              //A public static reference to itself (make's it visible to other objects without a reference)

    public GameObject[] MissionIcons;
                                                      //The player ship
    public GameObject[] player;                 //0 = Player1, ...
    public GameObject characterScreen;          //The game object containing the title text
    public GameObject startScreen;
    public GameObject mainMenuScreen;
    public GameObject highScoreScreen;
    public GameObject ingameScoreScreen;
    public GameObject missionScreen;
    
    public GameObject menuCam, ingameCam;       //the menu uses other effects than ingame
    
    public GUIText[] ingameScoreGUIText;        //The score text
    public GUIText[] highScoreGUIText;          //The high score text
    public GUIText[] playerActiveText;
    public GUIText[] mainMenuGuiText;           //Start, Options, Exit...
    public GUIText objectiveText;               //ingame status in upper right corner
    public GUIText missionText;                 //e1m1
    public GUIText QuestText;
    public GUIText chosenEpisode;

    public bool menuActive = true;              //True if any Menu is active
    public bool[] playerActive;                 //0 = Player1, ...
    public bool pressedDpad = false;            //prevend fast menu scrolling
    public bool pressedButton = false;          //prefend fast menu selection
    bool[] pressedPlayerDpad;
    public bool objectiveComplete = true;
    public bool fadeFinished = false;
    public bool playingCampaign = false;        //If the player plays the campaign he shall return to the missionMenu after the Highscore

    bool pressedArrow = false;

    int fadeDirection = -1;                     //-1 fadeIn (transparent), 1 fadeOut (darken)

    public AudioClip[] UImusic, BattleMusic;    //Music
    public AudioSource[] UIBeeps;               //Beeps for ButtonFeedBack
    public AudioClip[] DeathSounds;           //Dying laughter sounds

    Vector2[] playerDpad;

    public string missionName = "";             //Current Mission like E1M1
    public string[] mainMenuText;               //0 = StoryMode, 1 = Survival, 2 = Exit

    public Sprite[] CharPreviews;               //Character Previews
    public Sprite logoSprite, menuSprite, victorySprite, loseSprite, worldMap, MissionSpriteA, MissionSpriteB, MissionSpriteC;
    Vector2 min;                                //Viewport
    Vector2 max;                                //Viewport
            
    public void PlayDeathSound()
    {
        UIBeeps[3].clip = DeathSounds[Random.Range(0, DeathSounds.Length)];
        UIBeeps[3].Play();
    }

    void Awake()
    {
        //PlayerPrefs.DeleteAll();

        missionProgress = new int[MissionIcons.Length];

        //If this isn't the first play, then there is a saveFile to load
        if (PlayerPrefs.GetInt("startedCampaign") > 0)
        {
            missionProgress = PlayerPrefsX.GetIntArray("missionProgress");
        }

        fadeDirection = -1;     //-1 fadeIn (transparent), 1 fadeOut (darken)
        StartCoroutine("Fade");

        UIBeeps = GetComponents<AudioSource>();
        //Raise Volume
        StartCoroutine("VolumeOn");
        /*
        //Ensure that there is only one manager
        if (current == null)
            current = this;
        else
            Destroy(gameObject);
            */
        //Avoiding index out of bounds
        playerActive = new bool[4];
        player = new GameObject[4];
        pressedPlayerDpad = new bool[4];
        playerDpad = new Vector2[4];

        for (int i = 0; i < 5; i++)
        {            
            pressedPlayerDpad[i] = false;
            playerActive[i] = false;
            playerDpad[i] = new Vector2(0, 0);
        }
    }

    void Update()
    {
        //Back to MainMenu
        if (currentMenu != -1 && (GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any) || Input.GetKey(KeyCode.Y)) && !pressedButton && fadeFinished)
        {
            GotoMainMenu();
        }
        
        MenuNavigation();
    }

    void UIBeepSounds()
    {
        UIBeeps[1].Play();
    }

    //Makes navigation through the main menu possible
    void Dpad()
    {
        pressedArrow = true;             //for keyboardsupport
        pressedDpad = true;

        //-1 = None, 0 = TitleScreen, 1 = ModeSelection, 2 = MissionSelection, 3 = CharacterSelection, 4 = Highscore
        if (currentMenu == 1)
        {
            currentMainMenuSelection %= mainMenuText.Length;                //Avoid numbers bigger than the menu options
            if (currentMainMenuSelection < 0)                               //and check if the numbers get negativ
            {
                currentMainMenuSelection = mainMenuText.Length - 1;         //if so set the number to the last index
            }
            UIBeeps[2].Play();

            for (int i = 0; i < mainMenuGuiText.Length; i++)
            {
                mainMenuGuiText[i].color = new Color(255f, 255f, 255f, 255f);
                string tempString = mainMenuGuiText[i].text;
                tempString = tempString.Replace("✠", " ");
                mainMenuGuiText[i].text = tempString;
            }

            mainMenuGuiText[currentMainMenuSelection].color = new Color(255f, 0f, 0f, 255f);
            mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Remove(0, 1);
            mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Remove(mainMenuGuiText[currentMainMenuSelection].text.Length - 1, 1);
            mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Insert(0, "✠");
            mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Insert(mainMenuGuiText[currentMainMenuSelection].text.Length, "✠");
        }

        if (currentMenu == 2)
        {
            UIBeeps[1].Play();

            setMissionMarker(currentMissionSelection);
        }

        if (currentMenu == 3)
        {
            UIBeeps[2].Play();
        }
    }
    //Setting the missionMode if gameMode is 1 and start spawncoroutines
    void GameStart()
    {
        menuCam.SetActive(false);
        ingameCam.SetActive(true);

        objectiveComplete = false;

        fadeDirection = -1;
        StartCoroutine("Fade");
        GetComponent<SpriteRenderer>().sprite = null;

        menuActive = false;

        //Deactivate the title and activate the player
        ingameScoreScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
        characterScreen.SetActive(false);

        //0 = None, 1 = Mission, 2 = Survive, 3 Endless Mission
        
        GetComponent<AudioSource>().clip = BattleMusic[Random.Range(1, BattleMusic.Length)];
        GetComponent<AudioSource>().Play();
        StartCoroutine("VolumeOn");

        //Count player
        int playerCount = 0;
        for (int i = 0; i < player.Length; i++)
        {
            if (playerActive[i])
            {
                playerCount++;
            }
        }
        //Spawn the playerPlanes
        for (int i = 0; i < player.Length; i++)
        {
            if (playerActive[i])
            {
                //player[i] = (GameObject)Instantiate(Characters[Mathf.Abs(playerChoice[i])], Characters[Mathf.Abs(playerChoice[i])].transform.position = new Vector2(1f, 0f), Characters[Mathf.Abs(playerChoice[i])].transform.rotation);
                player[i].SendMessage("SetPlayer", (i + 1));
                player[i].SendMessage("SetPlaneValue", (playerCount));
            }
        }

        objectiveComplete = false;              //Must be after the players are spawned    
    }

    //Show the missionObjective at start
    IEnumerator ShowQuest()
    {
        QuestText.fontSize = 100;
        yield return new WaitForSeconds(2f);
        for (int i = 100; i > 0; i--)
        {
            QuestText.fontSize = i;
            yield return new WaitForSeconds(0.001f);
        }
        QuestText.text = "";
        StopCoroutine("ShowQuest");
    }

    //Fade in and out
    IEnumerator Fade()
    {
        fadeFinished = false;
        for (float i = 0; i != 100; i++)
        {
            yield return new WaitForSeconds(0.025f);

            if (fadeDirection == -1)
            {
                //fader.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f - i / 100f);
            }
            else
            {
                //fader.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, i / 100f);
            }
        }
        fadeFinished = true;
    }
    
    IEnumerator VolumeOn()
    {
        for (float i = 0f; i <= 100f; i++)
        {
            GetComponent<AudioSource>().volume = i / 100f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator VolumeOff()
    {
        for (float i = 100f; i >= 0f; i--)
        {
            GetComponent<AudioSource>().volume = i / 100f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    
    int CountPlayersAlive()
    {
        GameObject[] playerAlive = GameObject.FindGameObjectsWithTag("Player");
        return playerAlive.Length;
    }

    IEnumerator TriggerBossBattle()
    {
        Debug.Log("TriggerBossBattle");

        objectiveComplete = false; //Otherwise the winscreeen would load upon death
        
        yield return new WaitForSeconds(5f);
        
        if (CountPlayersAlive() > 0)
        {
            for (float i = 100f; i >= 0f; i--)
            {
                GetComponent<AudioSource>().volume = i / 100f;
                yield return new WaitForSeconds(0.01f);
            }

            GetComponent<AudioSource>().clip = BattleMusic[0];
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().volume = 100f;            
        }
        if (CountPlayersAlive() > 0)
        {
            yield return new WaitForSeconds(10f);
        }
        //the player can die within the last 10 seconds, so we have to check for bosses        
    }
    
    public void Save()
    {
        Debug.Log(missionProgress.Length);

        for (int i = 0; i < MissionIcons.Length; i++)
        {
            //missionProgress[i] = MissionIcons[i].GetComponent<Mission>().status;
        }
        //Save the progress to the player prefs
        PlayerPrefsX.SetIntArray("missionProgress", missionProgress);


        if (startedCampaign == 0)
            PlayerPrefs.SetInt("startedCampaign", (startedCampaign + 1));

        PlayerPrefs.Save();
    }

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

    //Setting the selected Mission in campaign
    void setMissionMarker(int selectedCampaignMission)
    {
        //iTween.MoveTo(missionMarker, iTween.Hash("position", MissionIcons[selectedCampaignMission].transform.position, "easeType", "linear", "time", .5f));

        int selectedCampaignEpisode = selectedCampaignMission / 5 + 1;

        chosenEpisode.text = "Episode: " + selectedCampaignEpisode + "   Mission: " + ((selectedCampaignMission % 5) + 1);
        missionText.text = "e" + selectedCampaignEpisode + "m" + ((selectedCampaignMission % 5) + 1);
    }

    //Navigating to the main menu
    void GotoMainMenu()
    {
        playingCampaign = false;
        if (currentMenu == 4)
        {
            GetComponent<AudioSource>().clip = UImusic[2];
            GetComponent<AudioSource>().Play();
        }
        pressedButton = true;   //Prefend accidental menu navigation
        currentMenu = 1;        //-1 = None, 0 = TitleScreen, 1 = ModeSelection, 2 = MissionSelection, 3 = CharacterSelection, 4 = Highscore
        currentMainMenuSelection = 0; //Default is 1 -> Misson

        mainMenuScreen.SetActive(true); //Activate GUI Text
        startScreen.SetActive(false);   //Deactivate old GUI
        characterScreen.SetActive(false);
        highScoreScreen.SetActive(false);
        missionScreen.SetActive(false);

        GetComponent<SpriteRenderer>().sprite = menuSprite;
        
        //Reset menu colorization
        for (int i = 0; i < mainMenuGuiText.Length; i++)
        {
            mainMenuGuiText[i].color = new Color(255f, 255f, 255f, 255f);
            string tempString = mainMenuGuiText[i].text;
            tempString = tempString.Replace("✠", " ");
            mainMenuGuiText[i].text = tempString;
        }

        mainMenuGuiText[currentMainMenuSelection].color = new Color(255f, 0f, 0f, 255f);
        mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Remove(0, 1);
        mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Remove(mainMenuGuiText[currentMainMenuSelection].text.Length - 1, 1);
        mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Insert(0, "✠");
        mainMenuGuiText[currentMainMenuSelection].text = mainMenuGuiText[currentMainMenuSelection].text.Insert(mainMenuGuiText[currentMainMenuSelection].text.Length, "✠");
    }

    //Navigating to the campaign menu
    void GotoMissionMenu()
    {
        bool sieg = true;

        if (currentMenu == 4 && !sieg)
        {
            GetComponent<AudioSource>().clip = UImusic[2];
            GetComponent<AudioSource>().Play();
        }
        if (sieg)
        {
            GetComponent<AudioSource>().clip = UImusic[1];
            GetComponent<AudioSource>().Play();
        }

        mainMenuScreen.SetActive(false);
        highScoreScreen.SetActive(false);
        missionScreen.SetActive(true);
        currentMenu = 2;



        //setMissionMarker(currentMissionSelection); //Last unlocked mission shall be the default chosen
        Dpad();

        GetComponent<SpriteRenderer>().sprite = worldMap;
    }

    //Navigating to the selection menu
    void GotoSelectionScreen()
    {
        if (currentMenu == 4)
        {
            GetComponent<AudioSource>().clip = UImusic[2];
            GetComponent<AudioSource>().Play();
        }
        highScoreScreen.SetActive(false);
        mainMenuScreen.SetActive(false);
        missionScreen.SetActive(false);

        characterScreen.SetActive(true);

        currentMenu = 3;

        GetComponent<SpriteRenderer>().sprite = menuSprite;
    }

}