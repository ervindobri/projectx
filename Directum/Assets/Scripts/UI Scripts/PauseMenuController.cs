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
	private MoveTimer moveTimer;
	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		panelTransform = GetComponent<RectTransform>();
		moveTimerObject = GameplayManager.currentMoveTimer;
		moveTimer = moveTimerObject.GetComponent<MoveTimer>();
	}
	public void showPanel(AudioClip audioClip)
	{
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
	}
	public void hidePanel()
	{
		pausedTime = Time.time - startTime;
		GameTimer.isTicking = true;
		moveTimer.isTicking = true;

		panelTransform.localScale = new Vector3(0f, 0f, 0f);
		GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
		foreach (var p in points)
		{
			p.GetComponent<CircleCollider2D>().enabled = true;
		}
	}
}
