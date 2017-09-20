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
