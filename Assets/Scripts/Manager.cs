using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private bool GameOver = false;
    public GameObject[] characters;
    public GameObject[] players;
    public int playerCount = 1;
    public GameObject currentCheckPoint;

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

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                if (players[i].transform.position.x > rightestPos && players[i].transform.position.x > 0)
                {
                    rightestPos = players[i].transform.position.x;
                }
                leftestPos = rightestPos;
                if (players[i].transform.position.x < leftestPos && players[i].transform.position.x > 0)
                {
                    leftestPos = players[i].transform.position.x;
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
            GameObject temp = (GameObject)Instantiate(players[i], currentCheckPoint.transform.root);
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
