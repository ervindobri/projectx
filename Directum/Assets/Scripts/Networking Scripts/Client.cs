using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Net.Sockets;
using System;
using UnityEngine.SceneManagement;

public class Client : MonoBehaviour
{
	private bool socketReady;
	private TcpClient socket;
	private NetworkStream stream;
	private StreamReader reader;
	private StreamWriter writer;

	public string clientName;
	public string isHost = "client";


	[Header("Chat objects:")]
	public GameObject textPrefab;
	public GameObject messagePrefab;

	private GameObject playersList;
	private GameObject playerMessages;
	public InputField inputField;
	public Color playerColor;
	public string[] colors = { "0" };

	public List<GameClient> players;

	

	public bool wasDisplayed = false;

	//Booleans for checking turns
	public bool myTurn = false;
	private GameObject readyPanel;
	public GameObject readyPrefab;
	public GameObject lilmsgPrefab;

	public bool invalidName;

	// TODO: turn player timers on and off

	public void TriggerMoveTimers(bool turnOn)
	{
		if ( turnOn )
		{
			foreach (var player in players)
			{
				player.moveTimer.isTicking = true;
			}
		}
		else
		{
			foreach (var player in players)
			{
				player.moveTimer.isTicking = false;
			}
		}
	}
	private void Start()
	{
		//hostData = new GameClient();
		players = new List<GameClient>();
		DontDestroyOnLoad(gameObject);

		System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
		wasDisplayed = false;
	}
	public void DisplayClientName(GameClient client)
	{
		string[] pColors = client.playerColor.Split('-');
		GameObject playerName = Instantiate(textPrefab, playersList.transform);
		playerName.GetComponent<Text>().text = client.playerName;
		//Put the clientName in the playersList panel
		playerName.GetComponent<Text>().color = new Color(float.Parse(pColors[0]), float.Parse(pColors[1]), float.Parse(pColors[2]), float.Parse(pColors[3]));
	}
	public void DisplayNotification(string message)
	{
		MessagePanelController messagePanelController = FindObjectOfType<MessagePanelController>();
		messagePanelController.SetMessageAndNotify(message);
	}
	public bool ConnectToServer(string host, int port)
	{
		//already connected
		if (socketReady)
		{
			Debug.Log("Already conected");
			return false;
		}
		// Create the socket
		try
		{
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);
			socketReady = true;
			Send("serverclientsystnewcl" + playerColor.r.ToString() + "-" +
				playerColor.g.ToString() + "-" + playerColor.b.ToString() + "-" +
				playerColor.a.ToString() + "|" + clientName);
		}
		catch (Exception e)
		{
			Debug.Log("Socket error: "+ e.Message);
			DisplayNotification("Couldn't connect to server!");

		}
		return socketReady;
	}
	private void Update()
	{
		//If we are connected
		if ( SceneManager.GetActiveScene().name == "Lobby" && !wasDisplayed )
		{
			playersList = GameObject.FindGameObjectWithTag("Content");
			playerMessages = GameObject.FindGameObjectWithTag("Message");
			foreach (GameClient player in players)
			{
				//Debug.Log(player.playerName);
				DisplayClientName(player);
			}
			wasDisplayed = true;
		}
		if (socketReady)
		{
			if (stream.DataAvailable)
			{
				string data = reader.ReadLine();
				if ( data != null)
				{
					OnIncomingData(data);
				}
			}
		}
	}
	private void OnApplicationQuit()
	{
		CloseSocket();
	}

	// This function represents incoming data from other client(s) through SERVER
	private void OnIncomingData(string data)
	{
		string[] aData = data.Split('|');

		//deserialize<struct name>(kapja a stringet);

		Debug.Log(" Client: " + data);
		// If the scene is lobby -> host is already connected!
		if (SceneManager.GetActiveScene().name == "Lobby")
		{
			if (aData[0] == "client" && aData[1] == "server")
				switch (aData[2])
				{
					case "syst":
						switch (aData[3])
						{
							case "newclient":
								//Get a display message that someone connected!
								DisplayNotification(aData[4] + " has connected!");
								UserConnected(aData[4], false, false, aData[5]);
								// Display the connected players name in the player list view
								break;
							case "ready":
								//If we get a ready signal , we check our players list with the players data who is ready
								foreach (GameClient p in players)
								{
									if (p.playerName == aData[4] && !p.isReady)
									{
										p.isReady = true;
										DisplayChatMessage(messagePrefab, aData[4] + " is ready!", "0-1-0.4412-1");
									}
								}
								break;
							case "start":
								StartGame();
								break;
							case "disconnect":
								//If someone disconnects send a notification with his name and remove him from the players list
								ClientDisconnected(playersList , aData[4], aData[5]);
								break;
							default:
								DisplayNotification("CANNOT PROCESS RECEIVED DATA!");
								break;
						}
						break;
					case "chat":
						DisplayChatMessage(messagePrefab, aData[3] + " says:" + aData[4], aData[5]);
						break;
					default:
						DisplayNotification("CANNOT PROCESS RECEIVED DATA!");
						break;

				}
		}
		//Before and after  the lobby scene is loaded:
		else
		{
			if (aData[0] == "client" && aData[1] == "server")
			{
				switch (aData[2])
				{
					case "move":
						ConnectLines.Instance.DrawMove(int.Parse(aData[3]), int.Parse(aData[4]));
						break;
					case "chat":
						DisplayChatMessage(lilmsgPrefab, aData[3] + ":" + aData[4], aData[5]);
						break;
					case "syst":
						switch (aData[3])
						{
							case "newclient":
								// index starts with 4 due to protocoll
								// data received in format: name|color|name|color etc...
								for (int i = 4; i < aData.Length - 1; i += 2)
								{
									UserConnected(aData[i], false, false, aData[i + 1]);
								}
								//canDisplay = true;
								break;
							//Resume game with this 'ready' message
							case "ready":
								foreach (GameClient p in players)
								{
									if (p.playerName == aData[4] && !p.isReady)
									{
										p.isReady = true;
										readyPanel = GameObject.FindGameObjectWithTag("ReadyPanel");
										GameObject ready = Instantiate(readyPrefab, readyPanel.transform) as GameObject;
										ready.GetComponentInChildren<Text>().text = aData[4];
									}
								}
								break;
							case "pause":
								//In-game pause
								PauseMenuController pmc = FindObjectOfType<PauseMenuController>();
								pmc.ShowPanel();
								pmc.isPaused = true;
								break;
							case "timeout":
								ConnectLines.Instance.isMyTurn = true;
								break;
							case "disconnect":
								//Somebody has disconnected
								ClientDisconnected(null, aData[4], aData[5]);
								break;
							default:
								DisplayNotification("CANNOT PROCESS RECEIVED DATA!");
								break;
						}
						break;
					default:
						DisplayNotification("CANNOT PROCESS RECEIVED DATA!");
						break;
				}

			}
		}
		
	}

	private void ClientDisconnected(GameObject content, string name, string color)
	{
		if ( content != null)
		{
			foreach(GameClient p in players)
			{
				if (p.playerName == name && p.playerColor == color)
				{
					content = GameObject.FindGameObjectWithTag("Content");
					foreach (Transform child in content.transform)
					{

						if (child.gameObject.GetComponent<Text>().text == p.playerName)
						{
							Destroy(child.gameObject);
							players.Remove(p);
							DisplayNotification(name + " HAS DISCONNECTED!");
						}
					}
				}
			}
		}
		else
		{
			try
			{
				foreach (GameClient p in players)
				{
					if (p.playerName == name)
					{
						Debug.Log(p.playerName + " has disconnected!");
						players.Remove(p);
						DisplayNotification(name + " HAS DISCONNECTED!");
						GameOverPanelController.Instance.disconnectedPlayer = p.playerName;
					}

				}
			}
			catch (Exception e)
			{

				Debug.Log(e.Message);
			}
			

		}
		
	}

	private void DisplayChatMessage(GameObject msgPrefab, string message, string msgcolor)
	{
		playerMessages = GameObject.FindGameObjectWithTag("Message");
		string[] colors = msgcolor.Split('-');
		Color color = new Color(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]), float.Parse(colors[3]));
		
		GameObject msg = Instantiate(msgPrefab, playerMessages.transform) as GameObject;
		msg.GetComponent<Image>().color = color;	//set the color of the message
		msg.GetComponentInChildren<Text>().text = message;	// set the message
	}
	private void UserConnected(string name,bool host, bool ready, string ccolor)
	{
		GameClient gameClient = new GameClient();
		gameClient.isHost = host;
		gameClient.playerName = name;
		gameClient.playerColor = ccolor;
		gameClient.isReady = ready;
		if (players.Count > 0)
		{
			if ( CheckForValidPlayer(gameClient) )
			{
				players.Add(gameClient);
				if (SceneManager.GetActiveScene().name == "Lobby")
				{
					DisplayClientName(players[players.Count - 1]);
				}
			}
		}
		else
		{
			players.Add(gameClient);
			if (SceneManager.GetActiveScene().name == "Lobby")
			{
				DisplayClientName(players[players.Count - 1]);
			}
		}
	}

	public bool CheckForValidPlayer(GameClient newClient)
	{
		foreach (GameClient player in players)
		{
			//If there is a gameclient in the list with the same name and color , its probably the same client
			if (player.playerName == newClient.playerName && player.playerColor == newClient.playerColor)
			{
				//Client already present in list
				Debug.Log("Client already in players list!");
				DisplayNotification("CHOOSE ANOTHER NAME/COLOR");
				return false;
			}
		}
		return true;
	}

	public void Send(string data)
	{
		if ( !socketReady )
		{
			//Debug.Log("not sent");
			return;
		}
		writer.WriteLine(data);
		writer.Flush();
	}
	public void StartGame()
	{
		SceneManager.LoadScene("GameMain");
		foreach (var item in players)
		{
			item.isReady = false;
		}
	}
	public void CloseSocket()
	{
		if (!socketReady)
		{
			return;
		}
		writer.Close();
		reader.Close();
		socket.Close();
		socketReady = false;
	}
}

public class GameClient
{
	public string playerName;
	public string playerColor;
	public bool isHost;
	public bool isReady;
	public bool canMove;

	public GameObject playerPanel;
	public MoveTimer moveTimer;
}
