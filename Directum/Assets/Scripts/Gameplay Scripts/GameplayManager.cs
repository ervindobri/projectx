using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;

public class GameplayManager : MonoBehaviour
{


	ButtonAnimController buttonAnimController;
	private GameObject messagePanelObject;
	public MessagePanelController messagePanel;

	private GameObject gameOverPanel;
	public GameObject clientPrefab;
	public GameObject serverPrefab;
	private GameObject content;
	private AudioSource audioSource;
	public bool gameWon;
	private bool busy;

	public static GameplayManager Instance { get; set; }

	private void Awake()
	{

		Instance = this;
		DontDestroyOnLoad(gameObject);
		audioSource = gameObject.GetComponent<AudioSource>();
		messagePanelObject = GameObject.Find("MessagePanel").gameObject;
		messagePanel = messagePanelObject.GetComponent<MessagePanelController>();

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
	public static string GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				return ip.ToString();
			}
		}
		throw new Exception("No network adapters with an IPv4 address in the system!");
	}
	public void SetHostIP()
	{
		GameObject hostIp = GameObject.FindGameObjectWithTag("HostIp");
		IPAddress[] IpInHostAddress = Dns.GetHostAddresses(Dns.GetHostName());

		hostIp.GetComponent<Text>().text = GetLocalIPAddress();
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
			process.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + "/ConcurentTCP.exe";
			process.Start();
			//Process.Start(@);

			// Host is also a client -> instantiate

			messagePanel.text.text = "SERVER CREATED SUCCESSFULLY";


			Client client = Instantiate(clientPrefab).GetComponent<Client>();

			// Since this is Host -> he gets index 1 for playerprefs
			bool loadStatus = SettingsPanelController.Instance.SavePlayerPrefs(1);
			// Set client name -> load file from playerprefs, if its not saved, set default name
			if ( !loadStatus )
			{
				client.clientName = "Player1";
				client.playerColor = Color.red;
			}
			else
			{
				client.clientName = PlayerPrefs.GetString("p1name");
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
		bool connectionStatus;
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
			bool loadStatus = SettingsPanelController.Instance.SavePlayerPrefs(2);
			// Set client name -> load file
			if ( !loadStatus )
			{
				client.clientName = "Player2";
				client.playerColor = Color.red;
			}
			else
			{
				client.clientName = PlayerPrefs.GetString("p2name");
				client.playerColor = new Color(PlayerPrefs.GetFloat("p2c1"), PlayerPrefs.GetFloat("p2c2"), PlayerPrefs.GetFloat("p2c3"), 1);
			}
			//client.isReady = false;
			connectionStatus = client.ConnectToServer(host, 2269);
			// Transition fade in , then Set scene to Lobby
			if (connectionStatus)
			{
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
