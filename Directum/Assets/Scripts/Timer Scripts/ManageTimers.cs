using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageTimers : MonoBehaviour
{
	//Current player dependencies
	public static GameObject currentMovingPlayer;
	private List<Text> playerNames;

	//Current timer dependencies
	private GameObject[] moveTimerObjects;
	public static GameObject currentMoveTimerObject;

	public List<MoveTimer> moveTimers;
	private Client client;

	//Panels
	private GameObject gameOverPanel;
	private GameObject[] playerPanels;
	public string currentMovingPlayerName;

	public static ManageTimers Instance { set; get; }
	void Start()
    {
		Instance = this;
		client = FindObjectOfType<Client>();
		gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");
		gameOverPanel.GetComponent<Canvas>().sortingLayerName = "Default";
		playerPanels = GameObject.FindGameObjectsWithTag("Player");
		playerNames = new List<Text>();
		//Initially player1 is starting, but in our case non-Host client is starting
		for (int i = 0; i < playerPanels.Length; i++)
		{
			playerNames.Add(playerPanels[i].transform.Find("Name").GetComponent<Text>());
			//UnityEngine.Debug.Log(playerTexts[i]);
		}
		moveTimerObjects = GameObject.FindGameObjectsWithTag("Movetimer");
		moveTimers = new List<MoveTimer>();
		for (int i = 0; i < playerPanels.Length; i++)
		{
			moveTimers.Add(playerPanels[i].GetComponent<MoveTimer>());
		}

		// ....
		moveTimers[0].isTicking = true;
		currentMovingPlayerName = playerNames[0].text;
	}

    // Update is called once per frame
    void Update()
    {
		Debug.Log("gamewon" + GameOverPanelController.Instance.gameWon + "deadend:" + ConnectLines.Instance.deadEnd);
		//Debug.Log(ConnectLines.Instance.client.clientName + "," + ConnectLines.Instance.isMyTurn);
		//If it's my turn -> start timer
        if ( ConnectLines.Instance.isMyTurn && !PauseMenuController.Instance.isPaused && !GameOverPanelController.Instance.gameWon && !ConnectLines.Instance.deadEnd)
		{
			//Debug.Log(ConnectLines.Instance.client.clientName + "'s turn!");
			for (int i = 0; i < client.players.Count; i++)
			{
				if (ConnectLines.Instance.client.clientName == client.players[i].playerName)
				{
					//copy paste code (getcomponent)
					playerPanels[i].transform.Find("Glow").GetComponent<Image>().enabled = true;
					moveTimers[i].isTicking = true;
					currentMovingPlayerName = playerNames[i].text;
				}
				else
				{
					playerPanels[i].transform.Find("Glow").GetComponent<Image>().enabled = false;
					moveTimers[i].isTicking = false;
					//moveTimers[i].alreadyMoved = false;
					moveTimers[i].resetTrigger = true;
				}
			}
		}
		else if ( !ConnectLines.Instance.isMyTurn && !PauseMenuController.Instance.isPaused && !GameOverPanelController.Instance.gameWon && !ConnectLines.Instance.deadEnd)
		{
			//foreach (var player in client.players)
			//{
			//	//if (ConnectLines.Instance.client.clientName != player.playerName)
			//	//{
			//	//	map<player, GameObject>panels 

			//	//	//panels[player]...is a panel
			//	//}
			//	//player.panels 
			//    //player.Timer 
			//}
			for (int i = 0; i < client.players.Count; i++)
			{

				if (ConnectLines.Instance.client.clientName != client.players[i].playerName)
				{
					playerPanels[i].transform.Find("Glow").GetComponent<Image>().enabled = true;
					moveTimers[i].isTicking = true;
					currentMovingPlayerName = playerNames[i].text;
				}
				else
				{
					playerPanels[i].transform.Find("Glow").GetComponent<Image>().enabled = false;
					moveTimers[i].isTicking = false;
					//moveTimers[i].alreadyMoved = false;
					moveTimers[i].resetTrigger = true;
				}
			}
		}
		else if (GameOverPanelController.Instance.gameWon || ConnectLines.Instance.deadEnd )
		{
			Debug.Log("Both timers stopped!");
			for (int i = 0; i < client.players.Count; i++)
			{
				moveTimers[i].isTicking = false;
			}

		}
		else
		{
			Debug.Log("Both timers stopped!");
			for (int i = 0; i < client.players.Count; i++)
			{
				moveTimers[i].isTicking = false;
			}
		}
    }
}
