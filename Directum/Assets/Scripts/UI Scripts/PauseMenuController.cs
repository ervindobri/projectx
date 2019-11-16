using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
	private RectTransform panelTransform;

	private AudioSource audioSource;
	public static float pausedTime;
	private float startTime;
	private GameObject moveTimerObject;
	private GameObject panelObject;
	public MoveTimer moveTimer;

	private GameObject gameplayManagerObject;
	private GameplayManager gameplayManager;

	public static bool isPaused;
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		panelTransform = GetComponent<RectTransform>();
		moveTimerObject = GameplayManager.currentMoveTimer;
		panelObject = GameplayManager.currentMovingPlayer;
		moveTimer = panelObject.GetComponent<MoveTimer>();

		gameplayManagerObject = GameObject.Find("GameplayManager");
		gameplayManager = gameplayManagerObject.GetComponent<GameplayManager>();
	}
	private void Update()
	{
		//Debug.Log(moveTimer.isTicking);
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
		moveTimer.isTicking = false;
		panelTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
		foreach (var p in points)
		{
			p.GetComponent<CircleCollider2D>().enabled = false;
		}
		panelTransform.GetComponentInChildren<Animator>().SetTrigger("fadeIn");
		isPaused = true;
	}
	public void hidePanel()
	{
		pausedTime = Time.time - startTime;
		GameTimer.isTicking = true;
		foreach (var item in gameplayManager.moveTimers)
		{
			item.isTicking = true;
		}

		panelTransform.localScale = new Vector3(0f, 0f, 0f);
		GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
		foreach (var p in points)
		{
			p.GetComponent<CircleCollider2D>().enabled = true;
		}
		isPaused = false;
	}
}
