using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
	public static bool socketReady;
	private TcpClient socket;
	private NetworkStream stream;
	private StreamReader reader;
	private StreamWriter writer;

	public string clientName;
	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void ConnectToServer(string host, int port)
	{
		//already connected
		if (socketReady)
		{
			Debug.Log(" Already conected");
			return;
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
