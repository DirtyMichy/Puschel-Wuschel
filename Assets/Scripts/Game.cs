using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game { 

	public static Game current;
	public string test;
    public bool firstTimeEntering = true;
	public int playerCount;
	public int campaignStatus;
	public int[] collected;
	public int[] playerChosenCharacter;

	public Game () {
		collected = new int[10];
		playerChosenCharacter = new int[4];
	}

}