using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;

public class Manager : MonoBehaviour {

    private bool GameOver = false;
    public GameObject[] playableCharacters;     //Array of gameobjects which contain playable characters
    public GameObject[] playerCharactersAlive; //Chosen characters by players
    public int[] playerChosenCharacter;         //index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
    public int playerCount = 1;                 //total number of players
    public int collectedMuffins = 0;
    public GameObject currentCheckPoint;        //current checkpoint at which players can respawn


	// Use this for initialization
    void Awake () 
    {
        playerCount = PlayerPrefs.GetInt("playerCount");
        playerChosenCharacter = PlayerPrefsX.GetIntArray("playerChosenCharacter");

        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            //playerID = 0,playableCharacters[playerChosenCharacter[2]] means player 1 gets character 3
            GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
            temp.SendMessage("SetPlayerID", playerID);
        }
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(CountPlayersAlive() <= 0 && !GameOver)
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

        float leftestPos = 0;
        float rightestPos = 0;

        for (int i = 0; i < playerCharactersAlive.Length; i++)
        {
            if (playerCharactersAlive[i] != null)
            {
                if (playerCharactersAlive[i].transform.position.x > rightestPos && playerCharactersAlive[i].transform.position.x > 0)
                {
                    rightestPos = playerCharactersAlive[i].transform.position.x;
                }

                leftestPos = rightestPos;

                if (playerCharactersAlive[i].transform.position.x < leftestPos && playerCharactersAlive[i].transform.position.x > 0)
                {
                    leftestPos = playerCharactersAlive[i].transform.position.x;
                }
            }
            
            float distance = (Mathf.Abs(leftestPos) + Mathf.Abs(rightestPos));
            float x = Mathf.Abs(rightestPos)-(Mathf.Abs(rightestPos)- Mathf.Abs(leftestPos))/2;

            if(distance > 15f )
            {
                Debug.Log("Distance: " + distance);
                
                Camera.main.orthographicSize=distance/3f;

                Vector3 pos = new Vector3(x, (Camera.main.orthographicSize-5f), -10f);

                Camera.main.transform.position = pos;
                
                Vector3 scale = new Vector3(distance/15f , distance/15f, 1f);

                Camera.main.transform.localScale = scale;
            }
            else
            {                
                Camera.main.transform.localScale = new Vector3(1f,1f,1f);
                
                Camera.main.orthographicSize=5f;

                Camera.main.transform.position = new Vector3(x, 0f, -10f);
            }

        }
        //Debug.Log("Right: " + rightestPos + "Left: " + leftestPos);
        //float x = rightestPos-(rightestPos- leftestPos)/2;

        //if(!GameOver)
        //Camera.main.transform.position = new Vector3(x, 0f, -10f);

        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    IEnumerator RespawnPlayers()
    {
        yield return new WaitForSeconds(3f);

        GameOver = false;

        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            //playerID = 0,playableCharacters[playerChosenCharacter[2]] means player 1 gets character 3
            GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
            temp.SendMessage("SetPlayerID", playerID);
        }
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void setCheckPoint(GameObject cp)
    {
        currentCheckPoint=cp;
        for (int playerID = 0; playerID < playerCount; playerID++)
        {
            if(!playerCharactersAlive[playerID])
            {
                GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
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
