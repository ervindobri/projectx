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
	private GameObject playersList;
	public GameObject textPrefab;
	public GameObject messagePrefab;
	private GameObject playerMessages;
	public InputField inputField;
	public Color playerColor;
	public string[] colors = { "0" };

	public List<GameClient> players;
	private  GameClient hostData;
	
 
	private bool wasDisplayed;
	private GameObject messagePanel;

	//Booleans for checking turns
	public bool myTurn = false;
	internal bool isReady;

	private void Start()
	{
		hostData = new GameClient();
		players = new List<GameClient>();
		DontDestroyOnLoad(gameObject);
		messagePanel = GameObject.Find("MessagePanel").gameObject;
	}
	public void DisplayClientName(GameClient client)
	{
		string[] pcolors = client.playerColor.Split('-');
		GameObject go = Instantiate(textPrefab, playersList.transform);
		go.GetComponent<Text>().text = client.playerName;
		//Put the clientName in the playersList panel
		go.GetComponent<Text>().color = new Color(float.Parse(pcolors[0]), float.Parse(pcolors[1]), float.Parse(pcolors[2]), float.Parse(pcolors[3]));
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
			//bool ihost = (isHost == "host") ? true : false;
			//UserConnected(clientName, ihost, false, null);
		}
		catch (Exception e)
		{
			Debug.Log("Socket error: "+ e.Message);

		}
		return socketReady;
	}
	private void Update()
	{
		//For testing
		Debug.Log(players.Count);
		//If we are connected
		if ( SceneManager.GetActiveScene().name == "Lobby" && !wasDisplayed )
		{
			playersList = GameObject.FindGameObjectWithTag("Content");
			playerMessages = GameObject.FindGameObjectWithTag("Message");
			wasDisplayed = true;
			DisplayClientName( players[0]);
			//if ( isHost == "client" && players.Count > 1)
			//{
			//	DisplayClientName(players[1]);
			//}
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
	private void OnDisable()
	{
		CloseSocket();
	}
	// This function represents incoming data from other client(s) through SERVER
	private void OnIncomingData(string data)
	{
		string[] aData = data.Split('|');

		Debug.Log(" Client: " + data);
		// If the scene is lobby -> host is already connected!
		if (SceneManager.GetActiveScene().name == "Lobby")
		{
			if (aData[0] == "server" && aData[1] == "client")
				switch (aData[2])
				{
					case "syst":
						switch (aData[3])
						{
							case "newc":
								for (int i = 1; i < aData.Length - 1; i++)
								{
									UserConnected(aData[i], false, false, null);
								}
								//
								Send("serverclientsystname" + clientName + "|" + "|" + playerColor.r.ToString() + "-" + playerColor.g.ToString() + "-" + playerColor.b.ToString() + "-" + playerColor.a.ToString());
								break;
							case "newclient":
								//Get a display message that someone connected!

								//messagePanel.GetComponent<Animator>().SetTrigger("dispMessage");
								MessagePanelController messagePanelController = FindObjectOfType<MessagePanelController>();
								messagePanelController.SetMessage(aData[1] + " has connected!");
								UserConnected(aData[1], false, false, aData[3]);

								// Display the connected players name in the player list view
								colors = aData[3].Split('-');
								GameObject pl = Instantiate(textPrefab, playersList.transform) as GameObject;
								pl.GetComponent<Text>().color = new Color(float.Parse(colors[0]), float.Parse(colors[1]), float.Parse(colors[2]), float.Parse(colors[3]));
								pl.GetComponent<Text>().text = aData[1];

								// Send the connected person your data so they will know you are there too!
								//Send("HI|" + clientName + "|" + ((isHost) ? 1 : 0).ToString() + "|" + ((isReady) ? 1 : 0).ToString() + "|" + playerColor.r.ToString() + "-" + playerColor.g.ToString() + "-" + playerColor.b.ToString() + "-" + playerColor.a.ToString());
								break;
								break;
						}
						break;
						

					
					case "chat":
						//client.send("serverclientsystchat");
						//write aData[3] on lobby panel;
						break;
					case "file":
						//fajl fogadasa
						break;
					case "imag":
						//kep fogadasa
						break;
					case "ready":
						string ready = " is ready!";
						//Displays a message on the chat panel
						GameObject msg = Instantiate(messagePrefab, playerMessages.transform) as GameObject;
						msg.GetComponent<Image>().color = new Color(0f, 1f, 0.4412079f, 1f);
						msg.GetComponentInChildren<Text>().text = aData[1] + ready;
						foreach (var item in players)
						{
							if (item.playerName == aData[1])
							{
								item.isReady = true;
							}
						}
						//Client
						//kiirni hogy a masik jatekos ready
						break;
					case "start":
					StartGame();
					break;
			}
		}
		//Before and after  the lobby scene is loaded:
		else
		{
			if (aData[0] == "server" && aData[1] == "client")
				switch (aData[2])
				{
					case "move":

						//Debug.Log(int.Parse(aData[3]) + "|" + int.Parse(aData[4]));
						ConnectLines.Instance.drawMove(int.Parse(aData[3]), int.Parse(aData[4]));
						Debug.Log(ConnectLines.Instance.isMyTurn);
						//Debug.Log(isHost);
						//Debug.Log(aData[3] + "|" + aData[4]);
						break;
					case "chat":
						//write aData[3] on lobby panel
						break;
					case "syst":
						switch (aData[3])
						{
							case "start":
								//Client.send("serverclientsyststart");
								//inditsd a jatekot
								break;
							case "pause":
								//Client.send("serverclientsystpause");
								//megallitja a jatekot
								break;
							case "disconnected":
								//kiirni hogy valaki disconnectalt+ a nevet
								break;
							case "rematch":
								//rematch
								break;
						}
						break;
				}
		}
		
	}
	private void UserConnected(string name,bool host, bool ready, string ccolor)
	{
		GameClient gc = new GameClient();
		gc.isHost = host;
		gc.playerName = name;
		gc.playerColor = ccolor;
		gc.isReady = ready;
		players.Add(gc);
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
}
