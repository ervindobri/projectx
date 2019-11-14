using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
	public static string movingPlayerName;
	private GameObject[] playerPanels;
	private List<Text> playerTexts;
	public int moveCounter;

	private GameObject[] moveTimers;
	public static GameObject currentMoveTimer;
	private void Start()
	{
		playerTexts = new List<Text>();
		playerPanels = GameObject.FindGameObjectsWithTag("Player");
		moveTimers = GameObject.FindGameObjectsWithTag("Movetimer");
		for (int i = 0; i < playerPanels.Length; i++)
		{
			playerTexts.Add(playerPanels[i].transform.Find("Text").GetComponent<Text>());
		}
		foreach (var text in playerTexts)
		{
			Debug.Log(text);
		}
		//First move goes for Player1
		if (playerTexts[0].text == "PLAYER 1")
		{
			movingPlayerName = playerTexts[0].text;
			currentMoveTimer = moveTimers[0];
		}
	}
	private void Update()
	{
		Debug.Log(movingPlayerName + "+" + currentMoveTimer);
		//if Player1 made a move->next player has to make a move
		if ( movingPlayerName == playerTexts[0].text)
		{
			playerPanels[0].GetComponent<MoveTimer>().isTicking = true;
			//Debug.Log("Player 1 is making a move!");
			if ( playerPanels[0].GetComponent<MoveTimer>().alreadyMoved )
			{
				movingPlayerName = playerTexts[1].text;
				playerPanels[0].GetComponent<MoveTimer>().isTicking = false;
				currentMoveTimer = moveTimers[1];
				playerPanels[0].GetComponent<MoveTimer>().alreadyMoved = false;
			}
		}
		//if Player2 has to move 
		if (movingPlayerName == playerTexts[1].text)
		{
			playerPanels[1].GetComponent<MoveTimer>().isTicking = true;
			//Debug.Log("Player 2 is making a move!");
			if (playerPanels[1].GetComponent<MoveTimer>().alreadyMoved )
			{
				movingPlayerName = playerTexts[0].text;
				playerPanels[1].GetComponent<MoveTimer>().isTicking = false;
				currentMoveTimer = moveTimers[0];
				playerPanels[1].GetComponent<MoveTimer>().alreadyMoved = false;
			}
		}
	}
}
