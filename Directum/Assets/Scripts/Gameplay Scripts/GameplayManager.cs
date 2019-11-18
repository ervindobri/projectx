using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class GameplayManager : MonoBehaviour
{
	public static string movingPlayerName;
	private GameObject[] playerPanels;
	private List<Text> playerTexts;
	public int moveCounter;

    public GameObject clientPrefab;

	private GameObject[] moveTimers;
	public static GameObject currentMoveTimer;
    public static GameplayManager Instance { get; set; }
	private void Start()
	{
        Instance = this;
        DontDestroyOnLoad(gameObject);
		playerTexts = new List<Text>();
		playerPanels = GameObject.FindGameObjectsWithTag("Player");
		moveTimers = GameObject.FindGameObjectsWithTag("Movetimer");
		for (int i = 0; i < playerPanels.Length; i++)
		{
			playerTexts.Add(playerPanels[i].transform.Find("Text").GetComponent<Text>());
		}
		foreach (var text in playerTexts)
		{
			Debug.Log(text);
		}
		//First move goes for Player1
		if (playerTexts[0].text == "PLAYER 1")
		{
			movingPlayerName = playerTexts[0].text;
			currentMoveTimer = moveTimers[0];
		}
	}
    public void ConnectButton()
    {
        Debug.Log("Connect");
    }
    public void HostButton()
    {
        Debug.Log("Host");
        Debug.Log("server started");
        int port=2269;
        int p;
        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0)
        {
            //Debug.Log(p);
            port = p;
        }
        try
        {
            Process.Start(@"D:\Sapientia\V.felev\szoftver\projectX\projectx\GameServer\Debug\GameServer.exe");
            Client client = Instantiate(clientPrefab).GetComponent<Client>();
            client.ConnectToServer("127.0.0.1", port);
            Debug.Log("server started");
        }
        catch(Exception e)
        {
            Debug.Log(e.Message+"nem sikerult elinditani a servert");
        }
    }
	private void Update()
	{
		Debug.Log(movingPlayerName + "+" + currentMoveTimer);
		//if Player1 made a move->next player has to make a move
		if ( movingPlayerName == playerTexts[0].text)
		{
			playerPanels[0].GetComponent<MoveTimer>().isTicking = true;
			//Debug.Log("Player 1 is making a move!");
			if ( playerPanels[0].GetComponent<MoveTimer>().alreadyMoved )
			{
				movingPlayerName = playerTexts[1].text;
				playerPanels[0].GetComponent<MoveTimer>().isTicking = false;
				currentMoveTimer = moveTimers[1];
				playerPanels[0].GetComponent<MoveTimer>().alreadyMoved = false;
			}
		}
		//if Player2 has to move 
		if (movingPlayerName == playerTexts[1].text)
		{
			playerPanels[1].GetComponent<MoveTimer>().isTicking = true;
			//Debug.Log("Player 2 is making a move!");
			if (playerPanels[1].GetComponent<MoveTimer>().alreadyMoved )
			{
				movingPlayerName = playerTexts[0].text;
				playerPanels[1].GetComponent<MoveTimer>().isTicking = false;
				currentMoveTimer = moveTimers[0];
				playerPanels[1].GetComponent<MoveTimer>().alreadyMoved = false;
			}
		}
	}
    public void ConnectedToServer()
    {
        string host = "127.0.0.1";
        int port = 2269;
        Debug.Log("server started");
        string h;
        int p;
        //Check if anything is typed in the inputfields.
        h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (h != "")
        {
            //Debug.Log(h);
            host = h;
        }
        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p);
        if (p != 0)
        {
            //Debug.Log(p);
            port = p;
        }
        try
        { 
            Client client = Instantiate(clientPrefab).GetComponent<Client>();
            client.ConnectToServer(host, port);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
}
