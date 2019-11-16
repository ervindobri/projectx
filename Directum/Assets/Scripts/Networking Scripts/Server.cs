using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;

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
		Debug.Log(IPAddress.Any);
		
		try
		{
			//IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
			server = new TcpListener(IPAddress.Any, port);
			server.Start();

			StartListening();
			serverStarted = true;

			Debug.Log("Server has been started!");
		}
		catch (Exception e)
		{
			Debug.Log("Socket Error:" + e.Message);
			throw;
		}
	}
	private void Update()
	{
		if ( !serverStarted)
		{
			Debug.Log("Server has not started!");
			return;
		}
		foreach ( ServerClient c in clientList)
		{
			//Is the client still connected?
			if (!isConnected(c.tcpClient))
			{
				c.tcpClient.Close();
				disconnectList.Add(c);
				continue;
			}
			// check for message from the client
			else
			{
				NetworkStream stream = c.tcpClient.GetStream();
				if (stream.DataAvailable)
				{
					StreamReader reader = new StreamReader(stream,true);
					string data = reader.ReadLine();
					if ( data != null)
					{
						//process the message with this function
						OnIncomingData(c, data);
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
	}
	private void StartListening()
	{
		server.BeginAcceptTcpClient(AcceptTcpClient, server);
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
			return false;

		}
		catch
		{

			return false;
		}
	}
	private void OnIncomingData(ServerClient c, string data)
	{
		Debug.Log(c.clientName + " has sent the following message:" + data);
	}
	private void AcceptTcpClient(IAsyncResult ar)
	{
		TcpListener listener = (TcpListener)ar.AsyncState;

		clientList.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
		StartListening();

		// send a message to everyone
		Broadcast(clientList[clientList.Count - 1].clientName + " has connected", clientList);
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
}

public class ServerClient
{
	public TcpClient tcpClient;
	public string clientName;

	public ServerClient(TcpClient clientSocket)
	{
		clientName = "Guest";
		tcpClient = clientSocket;
	}
}
