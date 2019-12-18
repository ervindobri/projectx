using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveTimerManager : MonoBehaviour
{
	//Current player dependencies
	public static GameObject currentMovingPlayer;

	//Current timer dependencies
	public static GameObject currentMoveTimerObject;


	private Client client;

	//Panels
	private GameObject gameOverPanel;
	public string currentMovingPlayerName;

	public Dictionary<GameClient, MoveTimer> moveTimers;
	public Dictionary<GameClient, GameObject> playerPanels;
	public Dictionary<GameObject, Image> panelGlow;

	public static MoveTimerManager Instance { set; get; }
	void Start()
    {
		Instance = this;
		client = FindObjectOfType<Client>();
		gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");
		gameOverPanel.GetComponent<Canvas>().sortingLayerName = "Default";
		//For storing 
		playerPanels = new Dictionary<GameClient, GameObject>();
		moveTimers = new Dictionary<GameClient, MoveTimer>();
		playerPanels.Add(client.players[0], client.players[0].playerPanel);
		playerPanels.Add(client.players[1], client.players[1].playerPanel);
		//Get glow components:
		panelGlow= new Dictionary<GameObject, Image>();
		//Get the player names for future uses
		foreach (var item in playerPanels)
		{
			panelGlow.Add(item.Value, item.Value.transform.Find("Glow").GetComponent<Image>());

		}
		// 0 - the hosts movetimer and name - players[0] is the host
		client.players[0].moveTimer.isTicking= true;
		currentMovingPlayerName = client.players[0].playerName;
	}

    // Update is called once per frame
    void Update()
    {
		Debug.Log("gamewon: " + GameOverPanelController.Instance.gameWon + "deadend:" + ConnectLines.Instance.deadEnd);
		//If it's my turn -> start timer
		if ( GameOverPanelController.Instance.disconnectedPlayer == null)
		{
			if (ConnectLines.Instance.isMyTurn && !PauseMenuController.Instance.isPaused && !GameOverPanelController.Instance.gameWon && !ConnectLines.Instance.deadEnd)
			{
				foreach (GameClient p in client.players)
				{
					if (ConnectLines.Instance.client.clientName == p.playerName)
					{
						panelGlow[p.playerPanel].enabled = true;
						p.moveTimer.isTicking = true;
						currentMovingPlayerName = p.playerName;
					}
					else
					{
						panelGlow[p.playerPanel].enabled = false;
						p.moveTimer.isTicking = false;
						p.moveTimer.resetTrigger = true;
					}
				}
			}
			else if (!ConnectLines.Instance.isMyTurn && !PauseMenuController.Instance.isPaused && !GameOverPanelController.Instance.gameWon && !ConnectLines.Instance.deadEnd)
			{
				foreach (GameClient p in client.players)
				{
					if (ConnectLines.Instance.client.clientName != p.playerName)
					{
						panelGlow[p.playerPanel].enabled = true;
						p.moveTimer.isTicking = true;
						currentMovingPlayerName = p.playerName;
					}
					else
					{
						panelGlow[p.playerPanel].enabled = false;
						p.moveTimer.isTicking = false;
						p.moveTimer.resetTrigger = true;
					}
				}
			}
			else
			{
				//Debug.Log("Both timers stopped!");
				client.TriggerMoveTimers(false);

			}
		}
		else
		{
			//Debug.Log("Both timers stopped!");
			client.TriggerMoveTimers(false);
		}
	}
}
