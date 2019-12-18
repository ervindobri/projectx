using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net;
using System.Diagnostics;
using System.Net.Sockets;

public class ConnectionManager : MonoBehaviour
{


	ButtonAnimController buttonAnimController;
	private MessagePanelController messagePanelController;

	public GameObject clientPrefab;
	public GameObject serverPrefab;

	public bool gameWon;

	public static ConnectionManager Instance { get; set; }

	private void Awake()
	{

		Instance = this;
		DontDestroyOnLoad(gameObject);

		messagePanelController = FindObjectOfType<MessagePanelController>();

		PlayerPrefs.SetString("localhost", "127.0.0.1");

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
		//IPAddress[] IpInHostAddress = Dns.GetHostAddresses(Dns.GetHostName());

		hostIp.GetComponent<Text>().text = GetLocalIPAddress();
	}
	public void HostButton()
	{
		try
		{
			//Start C++ Server as a new process with arguments as host and port
			Process process = new Process();
			process.EnableRaisingEvents = false;
			process.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + "/ConcurentTCP.exe";
			process.Start();
			//Process.Start(@);

			Client client = Instantiate(clientPrefab).GetComponent<Client>();

			//UnityEngine.Debug.Log(PlayerPrefs.GetString("playername"));
			SettingsPanelController.Instance.SavePlayerPrefs();
			// Set client name -> load file from playerprefs
			SetNameAndColor(client, "Player1");
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
			messagePanelController.SetMessageAndNotify("COULDN'T CREATE SERVER!");
		}
	}
	public void ConnectToServerButton()
	{
		bool connectionStatus;
		//int port;
		string host = GameObject.Find("HostInput").GetComponent<InputField>().text;

		if (host == "")
		{
			host = PlayerPrefs.GetString("localhost");
		}
		try 
		{
			Client client = Instantiate(clientPrefab).GetComponent<Client>();
			// Since this is the 2nd client -> index 2 for saving player datas
			SettingsPanelController.Instance.SavePlayerPrefs();
			// Set client name -> load file
			SetNameAndColor(client,"Player2");
			//client.isReady = false;
			connectionStatus = client.ConnectToServer(host, 2269);
			// Only go to the lobby if the name is valid and connection was successful
			if ( connectionStatus )
			{
				buttonAnimController.PanelAnimationFadeIn();
			}
		}
		catch (Exception)
		{
			messagePanelController.SetMessageAndNotify("SOCKET ERROR!");
		}
	}

	private void SetNameAndColor(Client client,string defaultName)
	{
		if (PlayerPrefs.GetString("playername") == "")
		{
			client.clientName = defaultName;
		}
		else
		{
			client.clientName = PlayerPrefs.GetString("playername");
		}
		if ( PlayerPrefs.GetFloat("playercolor1") == 1f && PlayerPrefs.GetFloat("playercolor2") == 1f && PlayerPrefs.GetFloat("playercolor3") == 1f)
		{
			client.playerColor = Color.red;
		}
		else
		{
			client.playerColor = new Color(PlayerPrefs.GetFloat("playercolor1"), PlayerPrefs.GetFloat("playercolor2"), PlayerPrefs.GetFloat("playercolor3"), 1);
		}
	}

	public void DestroyOnReload()
	{
		Destroy(this.gameObject);
	}
}
