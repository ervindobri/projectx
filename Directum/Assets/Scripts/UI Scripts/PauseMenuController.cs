using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
	private RectTransform panelTransform;

	private AudioSource audioSource;
	[Header("Pause Sound")]
	public AudioClip audioClip;
	public static float pausedTime;
	private float startTime;
	public GameObject panelObject;
	private GameObject gameOverPanel;
	public MoveTimer moveTimer;

	private GameObject gameplayManagerObject;
	private GameplayManager gameplayManager;
	private GameObject pausePanel;
	private Client client;
	public static bool isPaused;
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		panelTransform = GetComponent<RectTransform>();
		panelObject = GameplayManager.currentMovingPlayer;

		gameOverPanel = GameObject.FindGameObjectWithTag("GameOver");
		gameOverPanel.GetComponent<Canvas>().sortingLayerName = "Default";
		//UnityEngine.Debug.Log(panelObject);

		if ( panelObject != null)
		{
			moveTimer = panelObject.GetComponent<MoveTimer>();
		}
		else
		{
			Debug.Log("No move timer");
		}

		//UnityEngine.Debug.Log(moveTimer);
		gameplayManagerObject = GameObject.Find("GameplayManager");
		gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
		pausePanel = GameObject.Find("PausePanel");

		//Reach the client for further message sending solutions
		client = FindObjectOfType<Client>();
	}
	private void Update()
	{
		if (Input.GetKey("p") && !isPaused)
		{
			//Send a message to server that pause is on
			// Message format: Pause|GameTimer|MoveTimer1|MoveTimer2
			showPanel(audioClip);
			isPaused = true;
		}
		else if ( Input.GetKey("escape") && isPaused)
		{
			//Send a message to server that pause is off
			hidePanel();
			isPaused = false;
		}
	}
	public void showPanel(AudioClip audioClip)
	{
		//Send a message to server that game was stopped:

		//client.Send("P|" + "0|0|0");
		audioSource.PlayOneShot(audioClip);
		startTime = Time.time - pausedTime;
		GameTimer.isTicking = false;
		GameTimer.audioSource.volume = 0f;
		//moveTimer.isTicking = false;
		pausePanel.GetComponent<Canvas>().sortingLayerName = "Pause";
		pausePanel.GetComponentInChildren<Animator>().SetTrigger("fadeIn");
	}
	public void hidePanel()
	{
		pausedTime = Time.time - startTime;
		GameTimer.isTicking = true;
		GameTimer.audioSource.volume = 0.7f;

		foreach (var item in gameplayManager.moveTimers)
		{
			item.isTicking = true;
		}
		pausePanel.GetComponent<Canvas>().sortingLayerName = "Default";
		isPaused = false;
	}
}
