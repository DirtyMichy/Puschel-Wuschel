using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private bool GameOver = false;
    public GameObject[] playableCharacters;     //Array of gameobjects which contain playable characters
    public GameObject[] playerCharactersAlive; //Chosen characters by players
    public int[] playerChosenCharacter;         //index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
    public int playerCount = 1;                 //total number of players
    public GameObject currentCheckPoint;        //current checkpoint at which players can respawn

	// Use this for initialization
    void Start () {
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

        }
        //Debug.Log("Right: " + rightestPos + "Left: " + leftestPos);
        float x = rightestPos-(rightestPos- leftestPos)/2;

        if(!GameOver)
        Camera.main.transform.position = new Vector3(x, 0f, -10f);

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
    }

    int CountPlayersAlive()
    {
        playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
        return playerCharactersAlive.Length;
    }
}
