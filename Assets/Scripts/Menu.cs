using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour
{
    public static Menu current;

    public int[] campaignCollectedMuffins;
    public GameObject playerCountText, backGround, charSelectionText;
    public GameObject[] Level;
    public GameObject[] charPreviewers;
    public GameObject[] allCharacters;
    //contains all playable characters
    public int currentLevelSelection = 0;
    //public AudioSource[] UIBeeps;
    public AudioSource SoundPositive;
    public AudioSource SoundNegative;
    //Beeps for ButtonFeedBack
    public bool pressedDpad = false;
    //prevend fast menu scrolling
    public bool pressedButton = false;
    //prevend fast menu selection
    public bool[] playerActive;
    //0 = Player1, ...
    public bool[] playerRDY;
    public int[] playerChosenCharacter;
    //index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
    Vector2[] playerDpad;
    public bool[] pressedPlayerDpad;
    //bool pressedArrow = false;
    public bool charSelection = true;
    public int MAXPLAYER = 4;
    public int MAXLEVELS = 100;
    public int playerCount = 1;
    //player 1 is always avtive at the start
    GameObject[] levelUI, charUI;
    public GameObject CharPreviewArrow;
    public Sprite menuCharSelection, menuLevelSelection;
    int showCharSelection = 1;

    private GamepadInput.GamePad.Index[] gamePadIndex;

    // Toggle between the char- and levelSelectionScreen
    void toggleLevelUI()
    {
        for (int i = 0; i < levelUI.Length; i++)
        {
            levelUI[i].SetActive(!charSelection);
        }
        for (int i = 0; i < charUI.Length; i++)
        {
            charUI[i].SetActive(charSelection);
        }

        charSelection = !levelUI[0].activeSelf;
        if (charSelection)
            for (int i = 0; i < MAXPLAYER; i++)
            {            
                charPreviewers[i].SetActive(playerActive[i]);
            }
        else
            for (int i = 0; i < MAXPLAYER; i++)
            {            
                charPreviewers[i].SetActive(false);
            }

        if (charSelection)
            backGround.GetComponent<SpriteRenderer>().sprite = menuCharSelection;
        else
            backGround.GetComponent<SpriteRenderer>().sprite = menuLevelSelection;

        charSelectionText.SetActive(charSelection);
    }

    // Use this for initialization
    void Awake()
    { 
        gamePadIndex = new GamepadInput.GamePad.Index[4];
        gamePadIndex[0] = GamePad.Index.One;
        gamePadIndex[1] = GamePad.Index.Two;
        gamePadIndex[2] = GamePad.Index.Three;
        gamePadIndex[3] = GamePad.Index.Four;

        Debug.Log(Application.dataPath);

        if (!File.Exists(Application.dataPath + "/fluffy.plush"))
        {
            Game.current = new Game();		
            Game.current.playerChosenCharacter = new int[MAXPLAYER];
            //Errors happen when we increase the amount of levels after a savefile has been created. So we will create an Array with a size of 100 so there will be no problems in the future (atleast when we don't create over 100 levels)
            Game.current.collected = new int[MAXLEVELS]; 
            playerActive = new bool[MAXPLAYER];
            playerChosenCharacter = new int[MAXPLAYER]; 
            Game.current.playerCount = 1; 
        }
        if (File.Exists(Application.dataPath + "/fluffy.plush"))
        {
            Debug.Log("Savegame found");
			
            SaveLoad.Load();
            Game.current = SaveLoad.savedGames[0];  
            playerCount = Game.current.playerCount;
            playerActive = Game.current.playerActive; 
            playerChosenCharacter = Game.current.playerChosenCharacter;
        }
        //Debug.Log("Test: " + Game.current.test + " Collected.Length" + Game.current.collected.Length);
        Debug.Log("Collected[0] " + Game.current.collected[0]);

        //Game.current.test = "ABC";
        //Save the current Game as a new saved Game
        //SaveLoad.Save();

        //Debug.Log(Game.current.test);

        levelUI = GameObject.FindGameObjectsWithTag("LevelUI");
        charUI = GameObject.FindGameObjectsWithTag("CharUI");       

        //Inside the selectionScreen the playable characters are being spawned and modified, this could be improved in the future
        for (int i = 0; i < MAXPLAYER; i++)
        {
            for (int j = 0; j < allCharacters.Length; j++)
            {
                GameObject spawnedChar = Instantiate(allCharacters[j]);
                spawnedChar.GetComponent<PlayerController>().Name.SetActive(true);
                Destroy(spawnedChar.GetComponent<Rigidbody2D>());
                Destroy(spawnedChar.GetComponent<PlayerController>());
					
                spawnedChar.transform.position = charPreviewers[i].transform.position;
                spawnedChar.transform.parent = charPreviewers[i].transform;
                spawnedChar.SetActive(false);

                spawnedChar.transform.Find("ArrowUp").gameObject.SetActive(true);
                spawnedChar.transform.Find("ArrowDown").gameObject.SetActive(true);
            }
        }

        toggleLevelUI();

        int[] campaignCollectedMuffins = new int[Level.Length]; //creating an array with the same size as the Levelarray, to avoid nullpointers in forloop
        int[] loadedCampaignCollectedMuffins = Game.current.collected;

        playerRDY = new bool[MAXPLAYER];
        playerActive[0] = true;
        //Player 1 (int i = 0) is always active and is choosing a character which means he isnt ready at the start
        for (int i = 0; i < playerActive.Length; i++)
            playerRDY[i] = !playerActive[i];

        //get all saved stats, everything else will be 0 so the levelstats can get initialized below
        campaignCollectedMuffins = loadedCampaignCollectedMuffins;

        Debug.Log("Level: " + Level.Length + "campaignCollectedMuffins: " + Game.current.collected.Length);

        //initiliazing levelstats
        if (campaignCollectedMuffins.Length > 0)
            for (int i = 0; i < Level.Length; i++)
            {
                Level[i].GetComponent<LevelStats>().collectedMuffins = campaignCollectedMuffins[i];
            }

        pressedPlayerDpad = new bool[MAXPLAYER];
        playerDpad = new Vector2[MAXPLAYER];
        
        if (showCharSelection == 0)
        {
            charSelection = false;
            toggleLevelUI();
        }

        for (int i = 0; i < MAXPLAYER; i++)
        {            
            charPreviewers[i].SetActive(playerActive[i]);   
            charPreviewers[i].GetComponent<CharPreviewer>().SelectChar(playerChosenCharacter[i]);
        }

        playerCountText.GetComponent<Text>().text = playerCount + "/4 Spielern";
        CharPreviewers();
    }

    void UISoundPositive()
    {
        SoundPositive.Play();
    }

    void UISoundNegative()
    {
        SoundNegative.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Startlevel();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Startlevel();

        MenuNavigation();

//        Debug.Log(Game.current.playerCount);
    }

    void IterateThroughChars_Backward(int i)
    {        
        pressedPlayerDpad[i] = true;
        UISoundPositive();
        if (playerChosenCharacter[i] > 0)
            playerChosenCharacter[i]--;
        else
            playerChosenCharacter[i] = allCharacters.Length - 1;
    }

    void IterateThroughChars_Forward(int i)
    {        
        pressedPlayerDpad[i] = true;
        UISoundPositive();
        if (playerChosenCharacter[i] < allCharacters.Length - 1)
            playerChosenCharacter[i]++;
        else
            playerChosenCharacter[i] = 0; 
    }

    void CharNaviagtion()
    {
        for (int i = 0; i < MAXPLAYER; i++)
        {
            playerDpad[i] = GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex[i]);

            //choosing character with Dpad
            if (!playerRDY[i] && playerActive[i] && !pressedPlayerDpad[i])
            {
                if ((GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex[i]).y < 0f))
                {
                    IterateThroughChars_Forward(i);
                }
                if ((GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex[i]).y > 0f))
                {
                    IterateThroughChars_Backward(i);
                }

                charPreviewers[i].GetComponent<CharPreviewer>().SelectChar(playerChosenCharacter[i]);
            }

            if (playerDpad[i].y == 0f)
            {
                pressedPlayerDpad[i] = false;
            }
        }

        //###################### KeyBoardSupport for player 1 ######################
        if (Input.anyKeyDown)
        {
            if ((Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.Escape)) && playerActive[0])
            {
                Application.Quit();
            }

            if (!pressedPlayerDpad[0] && playerActive[0] && !playerRDY[0])
            {
                if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
                {
                    pressedPlayerDpad[0] = true;
                    int i = 0;
                    IterateThroughChars_Backward(i);
                }

                if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)))
                {
                    pressedPlayerDpad[0] = true;
                    int i = 0;
                    IterateThroughChars_Forward(i);
                }
            }
        }
    }

    void LevelNaviagtion()
    {
        Vector2 playerAnyDpad = GamePad.GetAxis(GamePad.Axis.Dpad, GamePad.Index.Any);

        if ((playerAnyDpad.y < 0f) && !pressedDpad)
        {
            IterateThroughLevels_Forward();                
            DpadLevelIteration();
        }    

        if ((playerAnyDpad.y > 0f) && !pressedDpad)
        {        
            IterateThroughLevels_Backward();
            DpadLevelIteration();
        }

        if ((playerAnyDpad.y == 0f) && pressedDpad)
        {
            pressedDpad = false;
        }

        //###################### KeyBoardSupport for player 1 ######################
        //pressedPlayerDpad[0] is used also for the arrowKeys

        if (Input.anyKeyDown)
        {
            if (((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && !pressedPlayerDpad[0]))
            {                              
                IterateThroughLevels_Forward();                 
                DpadLevelIteration();
            }            

            if (((Input.GetKeyDown(KeyCode.UpArrow)) || Input.GetKeyDown(KeyCode.W)) && !pressedPlayerDpad[0])
            {        
                IterateThroughLevels_Backward();
                DpadLevelIteration();
            }
        }
    }

    void ButtonPressedA(int i)
    {
        if (!playerActive[i])
        {
            playerActive[i] = true;
            Game.current.playerActive = playerActive;
            playerCount++;
            charPreviewers[i].SetActive(true);

            Game.current.playerCount = playerCount;
            SaveLoad.Save();

            playerCountText.GetComponent<Text>().text = playerCount + "/4 Spielern";

            //just to be sure that the new player hasn't chosen a char
            if (charSelection)
                playerRDY[i] = false;

            CharPreviewers();

            UISoundPositive();
        }
        else
        {
            //sound plays only the first time
            if (!playerRDY[i])
                UISoundPositive();
            
            playerRDY[i] = true;

            charPreviewers[i].gameObject.transform.GetChild(playerChosenCharacter[i]).Find("ArrowUp").gameObject.SetActive(!playerRDY[i]);
            charPreviewers[i].gameObject.transform.GetChild(playerChosenCharacter[i]).Find("ArrowDown").gameObject.SetActive(!playerRDY[i]);

            if (playerRDY[0] && playerRDY[1] && playerRDY[2] && playerRDY[3] && playerCount > 0)
            if (charSelection)
            {
                charSelection = false;
                toggleLevelUI();
            }
            else
            {
                playerChosenCharacter = Game.current.playerChosenCharacter;
                SaveLoad.Save();
                Startlevel();
            }
        }
    }

    void ButtonPressedB(int i)
    {
        if (playerActive[i])
        if (charSelection)
        {
            if (playerRDY[i])
            {
                playerRDY[i] = false;

                charPreviewers[i].gameObject.transform.GetChild(playerChosenCharacter[i]).Find("ArrowUp").gameObject.SetActive(!playerRDY[i]);
                charPreviewers[i].gameObject.transform.GetChild(playerChosenCharacter[i]).Find("ArrowDown").gameObject.SetActive(!playerRDY[i]);
            }
            else
            {                       
                playerCount--;
                playerActive[i] = false;
                Game.current.playerActive = playerActive;
                playerRDY[i] = true;

                Game.current.playerCount = playerCount;
                SaveLoad.Save();

                playerCountText.GetComponent<Text>().text = playerCount + "/4 Spielern";

                charPreviewers[i].SetActive(false);
                CharPreviewers();
            }
            UISoundNegative();
        }
        else
        {
            charSelection = true;
            UISoundNegative();
            toggleLevelUI();
        } 
    }

    //Everything for the menu navigation
    void MenuNavigation()
    {
        //############################ Activating Players ############################

        for (int i = 0; i < MAXPLAYER; i++)
        {
            //#################### BUTTON A ####################

            if (GamePad.GetButtonDown(GamePad.Button.A, gamePadIndex[i]))
                ButtonPressedA(i);

            //#################### BUTTON B ####################

            if (GamePad.GetButtonDown(GamePad.Button.B, gamePadIndex[i]))
                ButtonPressedB(i);

            //#################### BUTTON Y ####################

            if ((GamePad.GetButtonDown(GamePad.Button.Y, gamePadIndex[i])) && playerActive[i] && charSelection)
                Application.Quit();                   
        }
        
        //###################### KeyBoardSupport for player 1 ######################

        if ((Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.Return))))
        {
            int i = 0;
            ButtonPressedA(i);
        }
        
        if ((Input.GetKeyDown(KeyCode.B) || (Input.GetKeyDown(KeyCode.Backspace))) && playerActive[0])
        {
            int i = 0;
            ButtonPressedB(i); 
        } 

        if (charSelection)
            CharNaviagtion();
        else
            LevelNaviagtion();        

        //###################### KeyBoardSupport for player 1 ######################
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            pressedPlayerDpad[0] = false;
        }
    }

    //Aligns all previewers on the screen depending on the playerCount
    void CharPreviewers()
    {        
        int row =-1; //-1 because if one player exists, row++ means the previewer will be centered at 0 (center screen)
        if (playerCount > 0)
            for (int i = 0; i < charPreviewers.Length; i++)
            {
                if (playerActive[i])
                    row++;
                
                    Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
                    Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
                    Vector3 pos = new Vector3(0f, 0f, 0f);

                pos.x = min.x + ((max.x * 2) / (playerCount + 1)) + ((max.x * 2) / (playerCount + 1) * row);
                    
                    charPreviewers[i].transform.position = pos;
                
            }
    }

    void IterateThroughLevels_Backward()
    {        
        Level[currentLevelSelection].GetComponent<LevelStats>().currentlySelected = false;
        
        if (currentLevelSelection > 0)
            currentLevelSelection--;
        else
            currentLevelSelection = Level.Length - 1;
        
        Level[currentLevelSelection].GetComponent<LevelStats>().currentlySelected = true;
    }

    void IterateThroughLevels_Forward()
    {        
        Level[currentLevelSelection].GetComponent<LevelStats>().currentlySelected = false;
        
        if (currentLevelSelection < Level.Length - 1)
            currentLevelSelection++;
        else
            currentLevelSelection = 0;

        Level[currentLevelSelection].GetComponent<LevelStats>().currentlySelected = true;
    }

    void Startlevel()
    {
        SceneManager.LoadScene(currentLevelSelection.ToString());
    }
        
    //Makes navigation through the levels inside the menu possible
    void DpadLevelIteration()
    {
        pressedPlayerDpad[0] = true;                            //for keyboardsupport
        pressedDpad = true;
            
        currentLevelSelection %= Level.Length;          //Avoid numbers bigger than the menu options
        if (currentLevelSelection < 0)
        {                                               //and check if the numbers get negativ
            currentLevelSelection = Level.Length - 1;   //if so set the number to the last index
        }

        UISoundPositive();
    }
}