using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Diagnostics;

public class GameplayManager : MonoBehaviour
{
	public static GameObject currentMovingPlayer;
	public static string currentMovingPlayerName;
	private GameObject[] playerPanels;
	private List<Text> playerTexts;
	public int moveCounter;

	private GameObject[] moveTimerObjects;
	public static GameObject currentMoveTimer;
	public List<MoveTimer> moveTimers;

	ButtonAnimController buttonAnimController;
	private GameObject messagePanelObject;
	private MessagePanelController messagePanel;

	private GameObject gameOverPanel;
	public GameObject clientPrefab;
	public GameObject serverPrefab;
	private GameObject gameplayManagerObject;
	private GameplayManager gameplayManager;

	public static GameplayManager Instance { get; set; }

	private void Awake()
	{
		Instance = this;
		messagePanelObject = GameObject.Find("MessagePanel").gameObject;
		messagePanel = messagePanelObject.GetComponent<MessagePanelController>();
		DontDestroyOnLoad(gameObject);
		if ( SceneManager.GetActiveScene().name == "GameMain")
		{
			gameplayManagerObject = GameObject.Find("GameplayManager");
			gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
			gameOverPanel = GameObject.Find("GameOverPanel");
			gameOverPanel.GetComponent<Canvas>().sortingLayerName = "Default";
			playerTexts = new List<Text>();
			playerPanels = GameObject.FindGameObjectsWithTag("Player");
			for (int i = 0; i < playerPanels.Length; i++)
			{
				playerTexts.Add(playerPanels[i].transform.Find("Title").GetComponent<Text>());
				//UnityEngine.Debug.Log(playerTexts[i]);
			}
			moveTimerObjects = GameObject.FindGameObjectsWithTag("Movetimer");
			moveTimers = new List<MoveTimer>();
			for (int i = 0; i < playerPanels.Length; i++)
			{
				moveTimers.Add(playerPanels[i].GetComponent<MoveTimer>());
				//UnityEngine.Debug.Log(moveTimers[i]);
			}
			//First move goes for Player1
			if (playerTexts[0].text == "PLAYER 1")
			{
				currentMovingPlayer = playerPanels[0];
				currentMovingPlayerName = playerTexts[0].text;
				currentMoveTimer = moveTimerObjects[0];
				//UnityEngine.Debug.Log(currentMoveTimer);
			}
		}
		if ( SceneManager.GetActiveScene().name == "PlayMenu")
		{
			GameObject buttonAnimControllerObject = GameObject.FindWithTag("Canvas");
			if (buttonAnimControllerObject != null)
			{
				buttonAnimController = buttonAnimControllerObject.GetComponent<ButtonAnimController>();
			}
			if (buttonAnimControllerObject == null)
			{
				UnityEngine.Debug.Log("Could not find 'ButtonAnimController' script...");
			}
		}

		
		
	}
	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "GameMain")
		{
			if (gameOverPanel.GetComponent<Canvas>().sortingLayerName == "GameOver")
			{
				// If GameOverPanel is active stop all timers
				GameTimer.isTicking = false;
				GameTimer.audioSource.volume = 0f;
				foreach (var item in gameplayManager.moveTimers)
				{
					item.isTicking = false;
				}
				// Set winner name and time
				gameOverPanel.transform.Find("Winner").GetComponent<Text>().text = currentMovingPlayerName + "WON!";
				gameOverPanel.transform.Find("FinalTimer").GetComponent<Text>().text = "Time: " + GameObject.Find("Timer").GetComponent<Text>().text;

			}
			else
			{
				//Debug.Log(movingPlayerName + "+" + currentMoveTimer);
				//if Player1 made a move->next player has to make a move
				if (currentMovingPlayerName == playerTexts[0].text && !PauseMenuController.isPaused)
				{
					moveTimers[0].isTicking = true;
					//Debug.Log("Player 1 is making a move!");
					if (moveTimers[0].alreadyMoved)
					{
						currentMovingPlayer = playerPanels[1];
						currentMovingPlayerName = playerTexts[1].text;
						currentMoveTimer = moveTimerObjects[1];
						//
						moveTimers[0].isTicking = false;
						moveTimers[0].alreadyMoved = false;
					}
				}
				//if Player2 has to move 
				if (currentMovingPlayerName == playerTexts[1].text && !PauseMenuController.isPaused)
				{
					moveTimers[1].isTicking = true;
					//Debug.Log("Player 2 is making a move!");
					if (moveTimers[1].alreadyMoved)
					{
						// The current player is set to be player 1
						currentMovingPlayer = playerPanels[0];
						currentMovingPlayerName = playerTexts[0].text;
						currentMoveTimer = moveTimerObjects[0];
						//
						moveTimers[1].isTicking = false;
						moveTimers[1].alreadyMoved = false;
					}
				}

			}
			
		}
		if (SceneManager.GetActiveScene().name == "PlayMenu")
		{
		}
	}

	public void HostButton()
	{
		try
		{
			int port;
			int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out port);
			//Start C++ Server as a new process with arguments as host and port
			/*Process process = new Process();
			  process.StartInfo.FileName = "TCPServer.exe";
			  process.StartInfo.Arguments = "-n";
			*/
			// Host is also a client -> instantiate
			
			Server server = Instantiate(serverPrefab).GetComponent<Server>();
			server.Init();
			messagePanel.text.text = "SERVER CREATED SUCCESSFULLY";


			Client client = Instantiate(clientPrefab).GetComponent<Client>();
			// Set client name -> load file
			PlayerData data = SaveSystem.LoadPlayer();
			client.clientName = data.playerName;
			client.ConnectToServer("127.0.0.1", port);
			UnityEngine.Debug.Log(client.clientName + " has connected to server");

			// Transition fade in , then Set scene to Lobby
			buttonAnimController.PanelAnimationFadeIn();
		}
		catch (Exception e)
		{
			messagePanel.text.text = "COULDN'T CREATE SERVER!";
			//messagePanel.text.text = e.Message;
			throw;
		}
	}
	public void ConnectToServerButton()
	{
		int port;
		string host = GameObject.Find("HostInput").GetComponent<InputField>().text;
		int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out port);

		if (host == "")
		{
			host = "127.0.0.1";
		}
		if (port == 0) {
		
			port = 2269;
		}
		try 
		{
			Client client = Instantiate(clientPrefab).GetComponent<Client>();
			// Set client name -> load file
			PlayerData data = SaveSystem.LoadPlayer();
			client.clientName = data.playerName;
			client.ConnectToServer(host, port);
			UnityEngine.Debug.Log(client.clientName + " has connected to server");

			messagePanel.text.text = "CONNECTED TO HOST SUCCESSFULLY";
			// Transition fade in , then Set scene to Lobby
			buttonAnimController.PanelAnimationFadeIn();
		}
		catch (Exception)
		{
			messagePanel.text.text = "SOCKET ERROR!";
			throw;
		}
	}
	public void DestroyOnReload()
	{
		Destroy(this);
	}
}
