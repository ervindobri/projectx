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
	private GameObject panelObject;
	private MoveTimer moveTimer;

	private GameObject gameplayManagerObject;
	private GameplayManager gameplayManager;
	private GameObject pausePanel;

	public static bool isPaused;
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		panelTransform = GetComponent<RectTransform>();
		panelObject = GameplayManager.currentMovingPlayer;
		moveTimer = panelObject.GetComponent<MoveTimer>();
		gameplayManagerObject = GameObject.Find("GameplayManager");
		gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
		pausePanel = GameObject.Find("PausePanel");
	}
	private void Update()
	{
		if (Input.GetKey("p") && !isPaused)
		{
			showPanel(audioClip);
		}
		else if ( Input.GetKey("escape") && isPaused)
		{
			hidePanel();
		}
	}
	public void showPanel(AudioClip audioClip)
	{
		foreach (var item in gameplayManager.moveTimers)
		{
			item.isTicking = false;
		}
		audioSource.PlayOneShot(audioClip);
		startTime = Time.time - pausedTime;
		GameTimer.isTicking = false;
		GameTimer.audioSource.volume = 0f;
		moveTimer.isTicking = false;
		pausePanel.GetComponent<Canvas>().sortingLayerName = "Pause";
		pausePanel.GetComponentInChildren<Animator>().SetTrigger("fadeIn");
		isPaused = true;
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
