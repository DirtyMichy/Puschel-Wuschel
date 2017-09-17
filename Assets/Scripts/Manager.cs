using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    private bool GameOver = false;
    public GameObject[] players;

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

        //LoadScene will be removed and instead the player respawns at the last checkpoint
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    int CountPlayersAlive()
    {
        GameObject[] playerAlive = GameObject.FindGameObjectsWithTag("Player");
        return playerAlive.Length;
    }
}
