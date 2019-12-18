using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using UnityEngine.SceneManagement;

public class Server : MonoBehaviour
{
	private List<ServerClient> clientList;
	private List<ServerClient> disconnectList;
	public int port = 2269;

	private TcpListener server;
	private bool serverStarted;

	public void Init()
	{
		DontDestroyOnLoad(gameObject);
		clientList = new List<ServerClient>();
		disconnectList = new List<ServerClient>();
		try
		{
			//IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			server = new TcpListener(IPAddress.Any, port);
			server.Start();

			StartListening();
			serverStarted = true;

			//Debug.Log("Server has been started!");
		}
		catch (Exception e)
		{
			Debug.Log("Socket Error:" + e.Message);

		}
	}
	private void Update()
	{
		if ( !serverStarted)
		{
			Debug.Log("Server has not started!");
			return;
		}
		foreach ( ServerClient client in clientList)
		{
			//Debug.Log(client.clientName);
			//Is the client still connected?
			if (!isConnected(client.tcpClient))
			{
				client.tcpClient.Close();
				disconnectList.Add(client);
				continue;
			}
			// check for message from the client
			else
			{
				NetworkStream stream = client.tcpClient.GetStream();
				if (stream.DataAvailable)
				{
					StreamReader reader = new StreamReader(stream,true);
					string data = reader.ReadLine();
					if ( data != null)
					{
						//process the message with this function
						
						OnIncomingData(client, data);
					}
				}
			}	
		}
		for (int i = 0; i < disconnectList.Count; i++)
		{

			//Tell our player somebody has disconnected
			clientList.Remove(disconnectList[i]);
			disconnectList.RemoveAt(i);
		}
	}
	private void StartListening()
	{
		server.BeginAcceptTcpClient(AcceptTcpClient, server);
	}
	private void AcceptTcpClient(IAsyncResult ar)
	{
		TcpListener listener = (TcpListener)ar.AsyncState;

		string allUsers = "";
		foreach (ServerClient i in clientList)
		{
			allUsers += i.clientName + '|';
		}
		ServerClient sc = new ServerClient(listener.EndAcceptTcpClient(ar));
		clientList.Add(sc);

		StartListening();

		// send a message to everyone

		Broadcast("SWHO|", clientList[clientList.Count - 1]);
	}
	private bool isConnected(TcpClient client)
	{
		try
		{
			if ( client != null && client.Client !=null && client.Client.Connected)
			{
				// ?????
				if ( client.Client.Poll(0, SelectMode.SelectRead)){
					return !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
				}
				return true;
			}
			else
			{
				return false;
			}
		}
		catch
		{
			return false;
		}
	}

	private void Broadcast( string data, List<ServerClient> clients)
	{
		foreach ( ServerClient sc in clients)
		{
			try
			{
				StreamWriter writer = new StreamWriter(sc.tcpClient.GetStream());

				writer.WriteLine(data);
				writer.Flush();
			}
			catch (Exception e)
			{

				Debug.Log("Write error!"+ e.Message + " to client " + sc.clientName);
			}
		}
	}
	private void Broadcast(string data, ServerClient client)
	{
		List<ServerClient> sc = new List<ServerClient> { client };
		Broadcast(data, sc);
	}
	private void OnIncomingData(ServerClient client, string data)
	{
		string[] aData = data.Split('|');
		Debug.Log(" Server: " + data);

		//
		if ( SceneManager.GetActiveScene().name == "Lobby")
		{
			//Broadcast(data, clientList);
			switch (aData[0])
			{
				case "CWHO":
					client.clientName = aData[1];
					client.isHost = (aData[2] == "1")?true:false;
					client.canMove = (aData[2] == "1") ? false : true;
					client.clientColor = aData[3];
					Broadcast("SCONN|" + client.clientName + "|" + ((client.isHost) ? 1 : 0).ToString() + "|" + client.clientColor, clientList);
					break;
				case "MSG":
					client.clientName = aData[1];
					client.clientColor = aData[3];
					string message = aData[2];
					Broadcast("MSG|" + client.clientName + "|" + message + "|" + client.clientColor, clientList);
					break;
				case "SMSG":
					client.clientName = aData[1];
					client.isReady = bool.Parse(aData[2]);
					Broadcast("SMSG|" + client.clientName + "|" + ((client.isReady) ? 1 : 0).ToString() , clientList);
					break;
				case "HI":
					Broadcast("HI|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4],clientList[clientList.Count-1]);
					break;
				case "GS":
					Broadcast("GS|" + aData[1], clientList);
					break;
			}
		}
		else
		{


			switch (aData[0])
			{
				case "CWHO":
					client.clientName = aData[1];
					client.isHost = (aData[2] == "1") ? true : false;
					client.clientColor = aData[3];
					Broadcast("SCONN|" + client.clientName + "|" + ((client.isHost)?1:0).ToString() + "|" + client.clientColor, clientList);
					break;
				//system messages
				case "SMSG":
					client.clientName = aData[1];
					client.isReady = bool.Parse(aData[2]);
					Broadcast("SMSG|" + client.clientName + "|" + ((client.isReady) ? 1 : 0).ToString(), clientList);
					break;
				case "HI":
					Broadcast("HI|" + aData[1] + "|" + aData[2] + "|" + aData[3] + "|" + aData[4], clientList[clientList.Count - 1]);
					break;
			}
		}
		
	}
}

public class ServerClient
{
	public string clientName;
	public string clientColor;
	public TcpClient tcpClient;
	public bool isHost;
	public bool isReady;
	public bool canMove;

	public ServerClient(TcpClient clientSocket)
	{
		tcpClient = clientSocket;
	}
}
