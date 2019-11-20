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
	public bool isHost;

	[Header("Chat objects:")]
	private GameObject playersList;
	public GameObject textPrefab;
	private GameObject playerMessages;
	public InputField inputField;


	private List<GameClient> players = new List<GameClient>();
	private bool displayed;

	private void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	public bool DisplayClientName()
	{
		//Put the clientName in the playersList panel
		textPrefab.GetComponent<Text>().text = clientName;
		if ( Instantiate(textPrefab, playersList.transform) ) {
			return true;
		}
		return false;
	}

	public void OnSendButton()
	{
		string message = clientName + " says: " + inputField.text;
		Send(message);
		inputField.Select();
		inputField.text = "";
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
		}
		catch (Exception e)
		{
			Debug.Log("Socket error: "+ e.Message);

		}
		return socketReady;
	}
	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "Lobby" && !displayed)
		{
			playersList = GameObject.FindGameObjectWithTag("Content");
			playerMessages = GameObject.FindGameObjectWithTag("Message");
			//Display client name on the list
			DisplayClientName();
			displayed = true;
		}
		//If we are connected
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
		if ( SceneManager.GetActiveScene().name == "Lobby")
		{
			GameObject go = Instantiate(textPrefab, playerMessages.transform) as GameObject;
			go.GetComponent<Text>().text = data;
		}
		else
		{
			Debug.Log("Client:" + data);
			string[] aData = data.Split('|');
			switch (aData[0])
			{
				case "SWHO":
					for (int i = 1; i < aData.Length - 1; i++)
					{
						UserConnected(aData[i], false);
					}
					Send("CWHO|" + clientName + "|" + ((isHost) ? 1 : 0).ToString());
					break;
				case "SCONN":
					UserConnected(aData[1], false);
					break;
			}
		}
		
	}
	private void UserConnected(string name,bool host)
	{
		GameClient gc = new GameClient();
		gc.playerName = name;

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
		//Debug.Log("sent");
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
	public bool isHost;
}
