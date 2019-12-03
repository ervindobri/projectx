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
	private MoveTimer currentMoveTimer;
	public static GameObject currentMoveTimerObject;

	public List<MoveTimer> moveTimers;

	ButtonAnimController buttonAnimController;
	private GameObject messagePanelObject;
	public MessagePanelController messagePanel;

	private GameObject gameOverPanel;
	public GameObject clientPrefab;
	public GameObject serverPrefab;
	public GameObject playerPrefab;
	private GameObject content;
	private AudioSource audioSource;
	private bool gameWon = true;

	private Client client;
	public static GameplayManager Instance { get; set; }

	private void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(gameObject);
		audioSource = gameObject.GetComponent<AudioSource>();
		messagePanelObject = GameObject.Find("MessagePanel").gameObject;
		messagePanel = messagePanelObject.GetComponent<MessagePanelController>();
		if ( SceneManager.GetActiveScene().name == "GameMain")
		{
			//Get the current client
			client = FindObjectOfType<Client>();
			content = GameObject.FindGameObjectWithTag("Content");
			if ( content == null)
			{
				UnityEngine.Debug.Log("Content not found!");
			}
			//GameObject p1 = Instantiate(playerPrefab, content.transform) as GameObject;
			//p1.transform.Find("TitleImage/Title").GetComponent<Text>().text = "PLAYER 1";
			//p1.transform.Find("Name").GetComponent<Text>().text = client.players[0].playerName;
			//GameObject p2 = Instantiate(playerPrefab, content.transform) as GameObject;
			//p2.transform.Find("TitleImage/Title").GetComponent<Text>().text = "PLAYER 2";
			//p2.transform.Find("Name").GetComponent<Text>().text = client.players[1].playerName;
			//if ( p1== null || p2== null)
			//{
			//	UnityEngine.Debug.Log("Couldn't instantiate players!");
			//}
			//---------------------------------------------------------
			gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");
			gameOverPanel.GetComponent<Canvas>().sortingLayerName = "Default";

			//Initially player1 is starting, but in our case non-Host client is starting
			//for (int i = 0; i < playerPanels.Length; i++)
			//{
			//	playerTexts.Add(playerPanels[i].transform.Find("TitleImage/Title").GetComponent<Text>());
			//	//UnityEngine.Debug.Log(playerTexts[i]);
			//}
			//moveTimerObjects = GameObject.FindGameObjectsWithTag("Movetimer");
			//moveTimers = new List<MoveTimer>();
			//for (int i = 0; i < playerPanels.Length; i++)
			//{
			//	moveTimers.Add(playerPanels[i].GetComponent<MoveTimer>());
			//}
			//First move goes for Player1

			//if ( isPlayer1Turn )
			//{
			//	currentMovingPlayer = playerPanels[0];
			//	currentMovingPlayerName = playerTexts[0].text;
			//	currentMoveTimerObject = moveTimerObjects[0];
			//	currentMoveTimer = moveTimers[0];
			//}
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
		if (SceneManager.GetActiveScene().name == "MainMenu")
		{
			DestroyOnReload();
		}
	}
	private void Update()
	{
		//UnityEngine.Debug.Log(currentMoveTimer);

		if (SceneManager.GetActiveScene().name == "GameMain")
		{
			//if (gameOverPanel.GetComponent<Canvas>().sortingLayerName == "GameOver")
			//{
			//	// If GameOverPanel is active play clip, stop all timers
			//	if ( gameWon )
			//	{
			//		AudioClip ac = Resources.Load("Audio/WINNER", typeof(AudioClip)) as AudioClip;
			//		audioSource.PlayOneShot(ac);
			//		gameWon = false;
			//	}
			//	GameTimer.isTicking = false;
			//	GameTimer.audioSource.volume = 0f;
			//	foreach (var item in moveTimers)
			//	{
			//		item.isTicking = false;
			//	}
			//	// Set winner name and time
			//	gameOverPanel.transform.Find("Winner").GetComponent<Text>().text = currentMovingPlayerName + " won";
			//	gameOverPanel.transform.Find("FinalTimer").GetComponent<Text>().text = GameObject.Find("Timer").GetComponent<Text>().text;

			//}
			//else
			//{
			//	//if Player1 made a move->next player has to make a move
			//	//if ( isPlayer1Turn && !PauseMenuController.isPaused)
			//	//{
			//	//	moveTimers[0].isTicking = true;
			//	//	if ( !moveTimers[0].alreadyMoved )
			//	//	{
			//	//		UnityEngine.Debug.Log("Player1 has to move!");
			//	//		return;
			//	//	}
			//	//	else
			//	//	{
			//	//		currentMoveTimer.isTicking = false;
			//	//		currentMoveTimer.alreadyMoved = false;
			//	//		currentMoveTimer = moveTimers[1];
			//	//		currentMovingPlayer = playerPanels[1];
			//	//		currentMovingPlayerName = playerTexts[1].text;
			//	//		currentMoveTimerObject = moveTimerObjects[1];
			//	//		isPlayer1Turn = false;
			//	//	}
			//	//}
			//	////if Player2 has to move 
			//	//if ( !isPlayer1Turn && !PauseMenuController.isPaused)
			//	//{
			//	//	moveTimers[1].isTicking = true;
			//	//	if (!moveTimers[1].alreadyMoved)
			//	//	{
			//	//		UnityEngine.Debug.Log("Player2 has to move!");
			//	//		return;
			//	//	}
			//	//	else
			//	//	{
			//	//		isPlayer1Turn = true;
			//	//		moveTimers[1].isTicking = false;
			//	//		moveTimers[1].alreadyMoved = false;
			//	//		currentMoveTimer = moveTimers[0];
			//	//		currentMoveTimerObject = moveTimerObjects[0];
			//	//		currentMovingPlayer = playerPanels[0];
			//	//		currentMovingPlayerName = playerTexts[0].text;
			//	//	}
			//	//}
			//}
			
		}
		if (SceneManager.GetActiveScene().name == "PlayMenu")
		{
		}
		if (SceneManager.GetActiveScene().name == "Lobby")
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
			Process process = new Process();
			process.EnableRaisingEvents = false;
			process.StartInfo.FileName = Application.dataPath + "/path/to/The.app/Contents/MacOS/The";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.RedirectStandardInput = true;
			process.StartInfo.RedirectStandardError = true;
			Process.Start(@"X:\Sapientia EMTE\III.év\Szoftvertervezés\MainBackup\ConcurentTCP\x64\Debug\ConcurentTCP.exe");

			// Host is also a client -> instantiate

			//Server server = Instantiate(serverPrefab).GetComponent<Server>();
			//server.Init();
			messagePanel.text.text = "SERVER CREATED SUCCESSFULLY";


			Client client = Instantiate(clientPrefab).GetComponent<Client>();

			// Since this is Host -> he gets index 1 for playerprefs
			SettingsPanelController.Instance.SavePlayerPrefs(1);
			// Set client name -> load file from playerprefs
			string name = PlayerPrefs.GetString("p1name");
			if (client.clientName == "")
			{
				client.clientName = name;
				client.playerColor = new Color(PlayerPrefs.GetFloat("p1c1"), PlayerPrefs.GetFloat("p1c2"), PlayerPrefs.GetFloat("p1c3"), 1);
			}
			client.isHost = "host";
			//client.isReady = false;
			bool connectionStatus = client.ConnectToServer("127.0.0.1", 2269);

			// Transition fade in , then Set scene to Lobby
			if ( connectionStatus)
			{
				buttonAnimController.PanelAnimationFadeIn();
			}
		}
		catch (Exception)
		{
			messagePanel.text.text = "COULDN'T CREATE SERVER!";
			//messagePanel.text.text = e.Message;
		}
	}
	public void ConnectToServerButton()
	{
		//int port;
		string host = GameObject.Find("HostInput").GetComponent<InputField>().text;
		//int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out port);

		if (host == "")
		{
			host = "127.0.0.1";
		}
		try 
		{
			Client client = Instantiate(clientPrefab).GetComponent<Client>();
			// Since this is the 2nd client -> index 2 for saving player datas
			SettingsPanelController.Instance.SavePlayerPrefs(2);
			// Set client name -> load file
			string name = PlayerPrefs.GetString("p2name");
			if (client.clientName == "")
			{
				client.clientName = name;
				client.playerColor = new Color(PlayerPrefs.GetFloat("p2c1"), PlayerPrefs.GetFloat("p2c2"), PlayerPrefs.GetFloat("p2c3"), 1);
			}
			//client.isReady = false;
			bool connectionStatus = client.ConnectToServer(host, 2269);
			// Transition fade in , then Set scene to Lobby
			if (connectionStatus)
			{
				messagePanel.text.text = "CONNECTED TO HOST SUCCESSFULLY";
				buttonAnimController.PanelAnimationFadeIn();
			}
		}
		catch (Exception)
		{
			messagePanel.text.text = "SOCKET ERROR!";
		}
	}

	public void DestroyOnReload()
	{
		Destroy(this.gameObject);
	}
}
