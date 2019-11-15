using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
	private bool socketReady;
	private TcpClient socket;
	private NetworkStream stream;
	private StreamReader reader;
	private StreamWriter writer;

	private GameObject messagePanelObject;
	private MessagePanelController messagePanel;

	private void Start()
	{
		messagePanelObject = GameObject.Find("MessagePanel").gameObject;
		messagePanel = messagePanelObject.GetComponent<MessagePanelController>();

		DontDestroyOnLoad(this.gameObject);
	}

	public void ConnectToServer()
	{
		//already connected
		if (socketReady)
		{
			Debug.Log(" Already conected");
			return;
		}
		string host = "127.0.0.1";
		int port = 2269;

		string h;
		int p;
		//Check if anything is typed in the inputfields.
		h = GameObject.Find("HostInput").GetComponent<InputField>().text;
		if ( h!= "")
		{
			//Debug.Log(h);
			host = h;
		}
		int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
		if ( p != 0)
		{
			//Debug.Log(p);
			port = p;
		}
		// Create the socket
		try
		{
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);
			socketReady = true;
			messagePanel.text.text = "CONNECTED TO HOST SUCCESSFULLY";
		}
		catch (Exception e)
		{
			Debug.Log("Socket error: "+ e.Message);
			messagePanel.text.text = "SOCKET ERROR!" ;
		}
	}
	private void Update()
	{
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
			Send("Hello GANYE");
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
		Debug.Log("Server sent : " + data);
	}
	public void Send(string data)
	{
		if ( !socketReady)
		{
			return;
		}
		writer.WriteLine(data);
		writer.Flush();
	}

	private void CloseSocket()
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
	public float[] playerColor;
}
