using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private bool GameOver = false;
    public GameObject[] playableCharacters;     //Array of gameobjects which contain playable characters
    public GameObject[] playerChosenCharacters; //Chosen characters by players
    public int playerCount = 1;                 //total number of players
    public GameObject currentCheckPoint;        //current checkpoint at which players can respawn

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(CountPlayersAlive() <= 0 && !GameOver)
        {
            GameOver = true;
            StartCoroutine(RestartScene());
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

        for (int i = 0; i < playerChosenCharacters.Length; i++)
        {
            if (playerChosenCharacters[i] != null)
            {
                if (playerChosenCharacters[i].transform.position.x > rightestPos && playerChosenCharacters[i].transform.position.x > 0)
                {
                    rightestPos = playerChosenCharacters[i].transform.position.x;
                }
                leftestPos = rightestPos;
                if (playerChosenCharacters[i].transform.position.x < leftestPos && playerChosenCharacters[i].transform.position.x > 0)
                {
                    leftestPos = playerChosenCharacters[i].transform.position.x;
                }
            }

        }
        //Debug.Log("Right: " + rightestPos + "Left: " + leftestPos);
        float x = rightestPos-(rightestPos- leftestPos)/2;
        Camera.main.transform.position = new Vector3(x, 0f, -10f);

        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < playerCount; i++)
        {
            GameObject temp = (GameObject)Instantiate(playerChosenCharacters[i], currentCheckPoint.transform.root);
            temp.SendMessage("SetPlayerID", i);
        }
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    int CountPlayersAlive()
    {
        GameObject[] playerAlive = GameObject.FindGameObjectsWithTag("Player");
        return playerAlive.Length;
    }
}
