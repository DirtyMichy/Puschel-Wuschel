using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

public class Manager : MonoBehaviour
{

    private bool GameOver = false;
    public GameObject[] playableCharacters;     //Array of gameobjects which contain playable characters
    public GameObject[] playerCharactersAlive; //Chosen characters by players
    public int[] playerChosenCharacter;         //index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
    public int playerCount = 1;                 //total number of players
    public int collectedMuffins = 0;
    public GameObject currentCheckPoint;        //current checkpoint at which players can respawn
    public float zoomStart = 15f;

    // Use this for initialization
    void Awake()
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        playerChosenCharacter = PlayerPrefsX.GetIntArray("playerChosenCharacter");

        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            //playerID = 0,playableCharacters[playerChosenCharacter[2]] means player 1 gets character 3
            GameObject temp = (GameObject)Instantiate(playableCharacters [playerChosenCharacter [playerID]], playableCharacters [playerChosenCharacter [playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters [playerChosenCharacter [playerID]].transform.rotation);
            temp.SendMessage("SetPlayerID", playerID);
        }
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (CountPlayersAlive() <= 0 && !GameOver)
        {
            GameOver = true;
            StartCoroutine(RespawnPlayers());
        }

        if (Input.GetKeyDown(KeyCode.Escape) || GamePad.GetButton(GamePad.Button.Y, GamePad.Index.Any))
        {
            SceneManager.LoadScene("Menu");
        }

        /*
        float playerPos = 0;
        int leadingPlayer = 0;

        for (int i = 0; i < players.Length; i++)
        {
            if(players[i].transform.position.x > playerPos && players[i].transform.position.x > 0)
            {
                players[i].GetComponent<PlayerController>().isLeader = false;
                playerPos = players[i].transform.position.x;
                leadingPlayer = i;
            }
        }

        Debug.Log(leadingPlayer);
        players[leadingPlayer].GetComponent<PlayerController>().isLeader = true;
        */

        //if the distance between the leftest and the rightest player gets greater than 15, the camera starts to zoom out
        if (CountPlayersAlive() > 1)
        {
            float leftestPos = 0;
            float rightestPos = 0;

            for (int i = 0; i < playerCharactersAlive.Length; i++)
            {
                if (playerCharactersAlive [i] != null)
                {
                    if (playerCharactersAlive [i].transform.position.x > rightestPos)
                    {
                        rightestPos = playerCharactersAlive [i].transform.position.x;
                    }

                    leftestPos = rightestPos;

                    if (playerCharactersAlive [i].transform.position.x < leftestPos)
                    {
                        leftestPos = playerCharactersAlive [i].transform.position.x;
                    }
                }
            }
        
        
            float distance = Mathf.Abs(leftestPos - rightestPos);
            float x = 0;

            Debug.Log("Distance: " + distance + "Left: " + Mathf.Abs(leftestPos) + "Right: " + Mathf.Abs(rightestPos));



            if (distance > zoomStart)
            {
                Debug.Log("Scaling");

                Camera.main.orthographicSize = distance / (zoomStart / 5f);
                
                x = rightestPos - distance / (zoomStart / 7.5f);

                if (x > 0)
                    Camera.main.transform.position = new Vector3(x, (Camera.main.orthographicSize - 5f), -10f);

                Vector3 scale = new Vector3(distance / zoomStart, distance / zoomStart, 1f);
            
                Camera.main.transform.localScale = scale;
            } else
            {                
                Camera.main.transform.localScale = new Vector3(1f, 1f, 1f);
            
                Camera.main.orthographicSize = 5f;

                x = rightestPos - distance / (zoomStart / 7.5f);

                if (x > 0)
                    Camera.main.transform.position = new Vector3(x, 0f, -10f);
                else
                    Camera.main.transform.position = new Vector3(0f, 0f, -10f);
            }
            //Debug.Log("Right: " + rightestPos + "Left: " + leftestPos);
            //float x = rightestPos-(rightestPos- leftestPos)/2;
        
            //if(!GameOver)
            //Camera.main.transform.position = new Vector3(x, 0f, -10f);
        } else
        {
            if (CountPlayersAlive() == 1)
            if (playerCharactersAlive [0].transform.position.x > 0f)
                Camera.main.transform.position = new Vector3(playerCharactersAlive [0].transform.position.x, 0f, -10f);
            else
                Camera.main.transform.position = new Vector3(0f, 0f, -10f);
            else
            if (currentCheckPoint.transform.position.x > 0f)
                Camera.main.transform.position = new Vector3(currentCheckPoint.transform.position.x, 0f, -10f);
            else
                Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        }
        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    IEnumerator RespawnPlayers()
    {
        yield return new WaitForSeconds(3f);

        GameOver = false;

        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            //playerID = 0,playableCharacters[playerChosenCharacter[2]] means player 1 gets character 3
            GameObject temp = (GameObject)Instantiate(playableCharacters [playerChosenCharacter [playerID]], playableCharacters [playerChosenCharacter [playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters [playerChosenCharacter [playerID]].transform.rotation);
            temp.SendMessage("SetPlayerID", playerID);
        }
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void setCheckPoint(GameObject cp)
    {
        currentCheckPoint = cp;
        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            if (!playerCharactersAlive [playerID])
            {
                GameObject temp = (GameObject)Instantiate(playableCharacters [playerChosenCharacter [playerID]], playableCharacters [playerChosenCharacter [playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters [playerChosenCharacter [playerID]].transform.rotation);
                temp.SendMessage("SetPlayerID", playerID);
            }
        }
    }

    int CountPlayersAlive()
    {
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
        return playerCharactersAlive.Length;
    }
}
