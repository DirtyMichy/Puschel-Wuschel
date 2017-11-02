using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    //public int[] campaignCollectedMuffins;      //campaignCollectedMuffins[0] =0; 0= E1M1 0= not finished, 1= finished, 2= everythingFound
    public GameObject playerCountText, backGround;
    public GameObject[] Level;
    public GameObject[] charPreviewers;
    public GameObject[] allCharacters;            //contains all playable characters
    //public bool[] unlockedCharacters;           //Array of gameobjects which contain playable characters
    public int currentLevelSelection = 0;
    public AudioSource[] UIBeeps;               //Beeps for ButtonFeedBack
    public bool pressedDpad = false;            //prevend fast menu scrolling
    public bool pressedButton = false;          //prevend fast menu selection
    public bool[] playerActive;                 //0 = Player1, ...
    public bool[] playerRDY;
    public int[] playerChosenCharacter;         //index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
    Vector2[] playerDpad;
    public bool[] pressedPlayerDpad;
    bool pressedArrow = false;
    public bool charSelection = true;
    int MAXPLAYER = 4;
    public int playerCount = 1; //player 1 is always avtive at the start
    public bool[] isUnlocked;
    GameObject[] levelUI, charUI;
    public GameObject Arrow;
    public Sprite menuCharSelection, menuLevelSelection;

    void toggleLevelUI()
    {
        for (int i = 0; i < levelUI.Length; i++)
        {
            levelUI [i].SetActive(!charSelection);
        }
        for (int i = 0; i < charUI.Length; i++)
        {
            charUI [i].SetActive(charSelection);
        }
        charSelection = !levelUI [0].activeSelf;
        if(charSelection)
            backGround.GetComponent<SpriteRenderer>().sprite = menuCharSelection;
        else
            backGround.GetComponent<SpriteRenderer>().sprite = menuLevelSelection;
    }

    // Use this for initialization
    void Awake()
    { 
        levelUI = GameObject.FindGameObjectsWithTag("LevelUI");
        charUI = GameObject.FindGameObjectsWithTag("CharUI");
        
        if (PlayerPrefsX.GetBoolArray("unlockedCharacters").Length > 0)
            isUnlocked = PlayerPrefsX.GetBoolArray("unlockedCharacters");
        else
        {
            isUnlocked = new bool[allCharacters.Length];
            for (int i = 0; i < allCharacters.Length; i++)
                isUnlocked [i] = true;
        }

        for (int i = 0; i < MAXPLAYER; i++)
        {
            for (int j = 0; j < allCharacters.Length; j++)
            {
                if (isUnlocked [j])
                {
                    GameObject spawnedChar = Instantiate(allCharacters [j]);
                    Destroy(spawnedChar.GetComponent<Rigidbody2D>());
                    Destroy(spawnedChar.GetComponent<PlayerController>());
                    spawnedChar.transform.position = charPreviewers [i].transform.position;
                    spawnedChar.transform.parent = charPreviewers [i].transform;
                    spawnedChar.SetActive(false);

                    //DownArrow
                    GameObject temp = (GameObject)Instantiate(Arrow);
                    Vector3 pos = temp.transform.position;
                    pos.y-=1;
                    temp.transform.position = pos;
                    temp.gameObject.transform.parent = spawnedChar.gameObject.transform;
                    
                    //UpArrow
                    temp = (GameObject)Instantiate(Arrow);
                    pos.y+=2;
                    temp.transform.position = pos;
                    temp.GetComponent<MovingPlatform>().rangeY = 1;
                    Vector3 scale = temp.transform.localScale;
                    scale.y*=-1;
                    temp.transform.localScale = scale;
                    temp.gameObject.transform.parent = spawnedChar.gameObject.transform;
                }
            }
        }

        toggleLevelUI();
        int[] campaignCollectedMuffins = new int[Level.Length]; //creating an array with the same size as the Levelarray, to avoid nullpointers in forloop
        int[] loadedCampaignCollectedMuffins = PlayerPrefsX.GetIntArray("collectedMuffins"); //

        playerRDY = new bool[MAXPLAYER];
        playerActive = new bool[MAXPLAYER];
        playerChosenCharacter = new int[MAXPLAYER];

        //Player 1 (int i = 0) is always active and is choosing a character which means he isnt ready at the start
        for (int i = 1; i < playerRDY.Length; i++)
            playerRDY [i] = true;
        if (playerActive.Length > 0)
            playerActive [0] = true;

        if (PlayerPrefsX.GetIntArray("playerChosenCharacter").Length > 0)
            playerChosenCharacter = PlayerPrefsX.GetIntArray("playerChosenCharacter");

        //get all saved stats, everything else will be 0 so the levelstats can get initialized below
        campaignCollectedMuffins = loadedCampaignCollectedMuffins;
        /*
        for (int i = 0; i < loadedCampaignCollectedMuffins.Length; i++)
        {
            campaignCollectedMuffins [i] = loadedCampaignCollectedMuffins [i];
        }
*/
        Debug.Log("Level: " + Level.Length + "campaignCollectedMuffins: " + campaignCollectedMuffins.Length);

        //initiliazing levelstats
        if( campaignCollectedMuffins.Length > 0)
        for (int i = 0; i < Level.Length; i++)
        {
            Level [i].GetComponent<LevelStats>().collectedMuffins = campaignCollectedMuffins [i];
        }

        pressedPlayerDpad = new bool[MAXPLAYER];
        playerDpad = new Vector2[MAXPLAYER];
        UIBeeps = GetComponents<AudioSource>();
    }

    void UIBeepSounds()
    {
        UIBeeps [1].Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Startlevel();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Startlevel();

        MenuNavigation();
        playerCountText.GetComponent<Text>().text = playerCount + "/4 Spielern";
    }

    //Everything for the menu navigation
    void MenuNavigation()
    {
        //############################ Activating Players ############################
        GamepadInput.GamePad.Index[] gamePadIndex;
        gamePadIndex = new GamepadInput.GamePad.Index[4];
        gamePadIndex [0] = GamePad.Index.One;
        gamePadIndex [1] = GamePad.Index.Two;
        gamePadIndex [2] = GamePad.Index.Three;
        gamePadIndex [3] = GamePad.Index.Four;
            
        for (int i = 0; i < MAXPLAYER; i++)
        {
            playerDpad [i] = GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex [i]);      

            if ((GamePad.GetButton(GamePad.Button.A, gamePadIndex [i])) && !playerActive [i])
            {
                UIBeepSounds();
                if (charSelection)
                {                    
                    if (!playerActive [i])
                    {
                        playerCount++;
                        playerActive [i] = true;
                    }                    
                }
            }

            if ((GamePad.GetButton(GamePad.Button.B, gamePadIndex [i])) && playerActive [i])
            {
                UIBeepSounds();
                if (charSelection)
                {                    
                    if (playerActive [i] && !playerRDY [i])
                    {
                        playerCount--;
                        playerActive [i] = false;
                    }
                }
            }   


            
            if (playerDpad [i].y == 0f)
            {
                pressedPlayerDpad [i] = false;
            }

            //###################### KeyBoardSupport for player 1 ######################

            if ((Input.GetKeyDown(KeyCode.A)) && !playerActive [0])
            {
                UIBeepSounds();
                if (charSelection)
                {                    
                    if (!playerActive [i])
                    {
                        playerCount++;
                        playerActive [i] = true;
                    }                    
                }
            }
            
            if ((Input.GetKeyDown(KeyCode.B)) && !playerActive [0])
            {
                UIBeepSounds();
                if (charSelection)
                {                    
                    if (playerActive [i] && !playerRDY [i])
                    {
                        playerCount--;
                        playerActive [i] = false;
                    }
                }
            }   
            
            if ((Input.GetKeyDown(KeyCode.X)) && !playerActive [0])
            {
                if (!charSelection)           
                    Startlevel();
            }

            PlayerPrefs.SetInt("playerCount", playerCount);
        }

        //Iterate through cars or levels

        //Iterate through characters
        if (charSelection)
        {
            for (int i = 0; i < MAXPLAYER; i++)
            {
                charPreviewers [i].SetActive(playerActive [i]);

                playerDpad [i] = GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex [i]);
                
                if ((GamePad.GetButton(GamePad.Button.A, gamePadIndex [i])) && playerActive [i])
                {
                    if (playerRDY [0] && playerRDY [1] && playerRDY [2] && playerRDY [3] && playerCount > 0)
                    {
                        charSelection = false;
                        UIBeepSounds();
                        toggleLevelUI();
                    }
                }

                if ((GamePad.GetButton(GamePad.Button.A, gamePadIndex [i])) && playerActive [i])
                {
                    UIBeepSounds();
                  
                    if (!playerActive [i])
                        playerRDY [i] = false;
                    if (playerActive [i] && !playerRDY [i])
                        playerRDY [i] = true;  
                }
                
                if ((GamePad.GetButton(GamePad.Button.B, gamePadIndex [i])) && playerActive [i])
                {
                    UIBeepSounds();
                       
                    if (playerActive [i] && !playerRDY [i])
                        playerRDY [i] = true;
                    if (playerActive [i])
                        playerRDY [i] = false; 
                }

                if (playerActive [i] && !pressedPlayerDpad [i])
                {
                    if ((GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex [i]).y < 0f))
                    {
                        pressedPlayerDpad [i] = true;
                        UIBeepSounds();
                        if (playerChosenCharacter [i] < 2)
                            playerChosenCharacter [i]++;
                        else
                            playerChosenCharacter [i] = 0; 
                    }
                    if ((GamePad.GetAxis(GamePad.Axis.Dpad, gamePadIndex [i]).y > 0f))
                    {
                        pressedPlayerDpad [i] = true;
                        UIBeepSounds();
                        if (playerChosenCharacter [i] > 0)
                            playerChosenCharacter [i]--;
                        else
                            playerChosenCharacter [i] = 2;
                    }

                    charPreviewers [i].GetComponent<CharPreviewer>().SelectChar(playerChosenCharacter [i]);
                }
                
                if (playerDpad [i].y == 0f)
                {
                    pressedPlayerDpad [i] = false;
                }
                
                //###################### KeyBoardSupport for player 1 ######################

                if ((Input.GetKeyDown(KeyCode.A)) && playerActive [0])
                {
                    if (playerRDY [0] && playerRDY [1] && playerRDY [2] && playerRDY [3] && playerCount > 0)
                    {
                        charSelection = false;
                        UIBeepSounds();
                        toggleLevelUI();
                    }
                }

                if ((Input.GetKeyDown(KeyCode.A)) && playerActive [0])
                {
                    UIBeepSounds();
                  
                    if (!playerActive [0])
                        playerRDY [0] = false;
                    if (playerActive [0] && !playerRDY [0])
                        playerRDY [0] = true;  
                }
                
                if ((Input.GetKeyDown(KeyCode.B)) && playerActive [0])
                {
                    UIBeepSounds();
                       
                    if (playerActive [0] && !playerRDY [0])
                        playerRDY [0] = true;
                    if (playerActive [0])
                        playerRDY [0] = false; 
                }

                if (Input.GetKeyUp(KeyCode.UpArrow) && !pressedArrow)
                {
                    pressedArrow = true;
                    UIBeepSounds();
                    if (playerChosenCharacter [0] < allCharacters.Length-1)
                        playerChosenCharacter [0]++;
                    else
                        playerChosenCharacter [0] = 0; 
                }
                if (Input.GetKeyUp(KeyCode.DownArrow) && !pressedArrow)
                {
                    pressedArrow = true;
                    UIBeepSounds();
                    if (playerChosenCharacter [0] > 0)
                        playerChosenCharacter [0]--;
                    else
                        playerChosenCharacter [0] = allCharacters.Length-1;
                }
            }
            PlayerPrefsX.SetIntArray("playerChosenCharacter", playerChosenCharacter);
        } 
        else
        {
            //Iterate through levels because charselection is false

            Vector2 playerAnyDpad = GamePad.GetAxis(GamePad.Axis.Dpad, GamePad.Index.Any);
            
            if ((playerAnyDpad.y < 0f) && !pressedDpad) //&& currentMenu==1 ?
            {                              
                IterateThroughLevels_Forward(); 
                
                Debug.Log("Searching: " + currentLevelSelection);
                
                Dpad();
            }            
            //Navigate up the MainMenu
            if ((playerAnyDpad.y > 0f) && !pressedDpad)
            {        
                IterateThroughLevels_Backward();                        
                
                Dpad();
            }
            
            if ((playerAnyDpad.y == 0f) && pressedDpad)
            {
                pressedDpad = false;
            }

            if (GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any))
            {
                charSelection = true;
                UIBeepSounds();
                toggleLevelUI();
            }
            
            //###################### KeyBoardSupport for player 1 ######################
            
            if ((Input.GetKeyDown(KeyCode.A)) && playerActive [0])
            {           
                    Startlevel();
            }

            if ((Input.GetKeyUp(KeyCode.DownArrow) && !pressedArrow))
            {                              
                IterateThroughLevels_Forward(); 
                
                Debug.Log("Searching: " + currentLevelSelection);
                
                Dpad();
            }            
            //Navigate up the MainMenu
            if ((Input.GetKeyUp(KeyCode.UpArrow)) && !pressedArrow)
            {        
                IterateThroughLevels_Backward();                        
                
                Dpad();
            }


            if (Input.GetKeyUp(KeyCode.Y))
            {
                charSelection = true;
                UIBeepSounds();
                toggleLevelUI();
            }
        }

        //###################### KeyBoardSupport for player 1 ######################

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            pressedArrow = false;
        }
    }

    void CharPreviewers()
    {        
        if (playerCount > 0)
            for (int i = 0; i < charPreviewers.Length; i++)
        {
            Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
            Vector3 pos = new Vector3(0f, 0f, 0f);
            pos.x = min.x + ((max.x * 2) / (playerCount + 1)) + ((max.x * 2) / (playerCount + 1) * i);
            charPreviewers [i].transform.position = pos;
        }
    }

    void IterateThroughLevels_Backward()
    {        
        Level [currentLevelSelection].GetComponent<LevelStats>().currentlySelected = false;
        
        if (currentLevelSelection < Level.Length - 1)
            currentLevelSelection++;
        else
            currentLevelSelection = 0;
        //Debug.Log("Searching: " + currentMissionSelection);
        
        Level [currentLevelSelection].GetComponent<LevelStats>().currentlySelected = true;
    }

    void IterateThroughLevels_Forward()
    {        
        Level [currentLevelSelection].GetComponent<LevelStats>().currentlySelected = false;
        
        if (currentLevelSelection < Level.Length - 1)
            currentLevelSelection++;
        else
            currentLevelSelection = 0;
        //Debug.Log("Searching: " + currentMissionSelection);

        Level [currentLevelSelection].GetComponent<LevelStats>().currentlySelected = true;
    }
        
    // Update is called once per frame
    public void Startlevel()
    {
        SceneManager.LoadScene(currentLevelSelection.ToString());
    }
        
    //Makes navigation through the main menu possible
    void Dpad()
    {
        pressedArrow = true;             //for keyboardsupport
        pressedDpad = true;
            
        currentLevelSelection %= Level.Length;                //Avoid numbers bigger than the menu options
        if (currentLevelSelection < 0)                               //and check if the numbers get negativ
        {
            currentLevelSelection = Level.Length - 1;         //if so set the number to the last index
        }
        UIBeepSounds();  
        CharPreviewers();
    }
}
