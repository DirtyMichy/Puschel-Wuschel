using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GamepadInput;
using System.IO;

public class Manager : MonoBehaviour
{
	private bool GameOver = false;
	public GameObject[] playableCharacters;
	//Array of gameobjects which contain playable characters
	public GameObject[] playerCharactersAlive;
	//Chosen characters by players
	public int[] playerChosenCharacter;
	//index of playableCharacter, playerChosenCharacter[0]=2 means player 1 has chosen character 3
	public int playerCount = 1;
	//total number of players
	public int collectedMuffins = 0;
	public GameObject currentCheckPoint;
	//current checkpoint at which players can respawn
	public float zoomStart = 15f;
	private float cameraOffsetY = -0.76f;
	public GameObject spawnParticles;

	public static Manager currentGameManager;

	// Use this for initialization
	void Awake()
	{
		//First checkpoint will be the start Sprite
		currentCheckPoint = GameObject.Find("Misc/Start");

		currentGameManager = this;

		if (!File.Exists(Application.dataPath + "/fluffy.plush"))
			Game.current = new Game();
		else
		{
			SaveLoad.Load();
			Game.current = SaveLoad.savedGames[0];
		}

		playerCount = Game.current.playerCount;
		playerChosenCharacter = Game.current.playerChosenCharacter;

		for (int playerID = 0; playerID < playerCount; playerID++)
		{
			//playerID = 0,playableCharacters[playerChosenCharacter[2]] means player 1 gets character 3
			GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
			temp.SendMessage("SetPlayerID", playerID);
			Instantiate(spawnParticles, temp.transform.position, temp.transform.rotation);
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

		if (Input.GetKeyDown(KeyCode.Escape) || GamePad.GetButton(GamePad.Button.Start, GamePad.Index.Any))
		{
			SceneManager.LoadScene("Menu");
		}

		//if the distance between the leftest and the rightest player gets greater than 15, the camera starts to zoom out
		if (CountPlayersAlive() > 1)
		{
			float leftestPos = 0;
			float rightestPos = 0;

			//get rightest player
			for (int i = 0; i < playerCharactersAlive.Length; i++)
			{
				if (playerCharactersAlive[i] != null)
				{
					if (playerCharactersAlive[i].transform.position.x > rightestPos)
					{
						rightestPos = playerCharactersAlive[i].transform.position.x;
					}
				}
			}

			leftestPos = rightestPos;

			for (int i = 0; i < playerCharactersAlive.Length; i++)
			{
				if (playerCharactersAlive[i] != null)
				{
					if (playerCharactersAlive[i].transform.position.x < leftestPos)
					{
						leftestPos = playerCharactersAlive[i].transform.position.x;
					}
				}
			}		

			float distance = Mathf.Abs(leftestPos - rightestPos);
			float x = 0;

			Debug.Log("Distance: " + distance + "Left: " + Mathf.Abs(leftestPos) + "Right: " + Mathf.Abs(rightestPos));

			if (distance > zoomStart)
			{
				Debug.Log("Optimier mich!");

				Camera.main.orthographicSize = distance / (zoomStart / 5f);
                
				x = rightestPos - distance / (zoomStart / 7.5f);

				if (x > 0)
					Camera.main.transform.position = new Vector3(x, (Camera.main.orthographicSize - 5f) + cameraOffsetY, -10f);
				else
					Camera.main.transform.position = new Vector3(0f, (Camera.main.orthographicSize - 5f) + cameraOffsetY, -10f);

				Vector3 scale = new Vector3(distance / zoomStart, distance / zoomStart, 1f);
            
				Camera.main.transform.localScale = scale;
			}
			else
			{                
				Camera.main.transform.localScale = new Vector3(1f, 1f, 1f);
            
				Camera.main.orthographicSize = 5f;

				x = rightestPos - distance / (zoomStart / 7.5f);

				if (x > 0)
					Camera.main.transform.position = new Vector3(x, cameraOffsetY, -10f);
				else
					Camera.main.transform.position = new Vector3(0f, cameraOffsetY, -10f);
			}

		}
		else
		{
			if (CountPlayersAlive() == 1)
			{
				Camera.main.orthographicSize = 5f;
				if (playerCharactersAlive[0].transform.position.x > 0f)
					Camera.main.transform.position = new Vector3(playerCharactersAlive[0].transform.position.x, cameraOffsetY, -10f);
				else
					Camera.main.transform.position = new Vector3(0f, cameraOffsetY, -10f);
			}
			else//zoom towards the last checkpoint if all players are dead
            if (currentCheckPoint.transform.position.x > 0f)
				Camera.main.transform.position = new Vector3(currentCheckPoint.transform.position.x, cameraOffsetY, -10f);
			else
				Camera.main.transform.position = new Vector3(0f, cameraOffsetY, -10f);
		}
	}

	IEnumerator RespawnPlayers()
	{
		yield return new WaitForSeconds(2f);

		GameOver = false;

		for (int playerID = 0; playerID < playerCount; playerID++)
		{
			//playerID = 0,playableCharacters[playerChosenCharacter[2]] means player 1 gets character 3
			GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
			temp.SendMessage("SetPlayerID", playerID);
			Instantiate(spawnParticles, temp.transform.position, temp.transform.rotation);
		}
		playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
	}

	public void setCheckPoint(GameObject cp)
	{
		currentCheckPoint = cp;
		for (int playerID = 0; playerID < playerCount; playerID++)
		{
			if (playerCharactersAlive[playerID] == null)
			{
				GameObject temp = (GameObject)Instantiate(playableCharacters[playerChosenCharacter[playerID]], playableCharacters[playerChosenCharacter[playerID]].transform.position = currentCheckPoint.transform.position, playableCharacters[playerChosenCharacter[playerID]].transform.rotation);
				temp.SendMessage("SetPlayerID", playerID);
				Instantiate(spawnParticles, temp.transform.position, temp.transform.rotation);
			}
		}
	}

	int CountPlayersAlive()
	{
		playerCharactersAlive = GameObject.FindGameObjectsWithTag("Player");
		return playerCharactersAlive.Length;
	}
}