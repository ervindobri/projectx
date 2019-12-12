using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
	[Header("Style Object")]
	public GameObject titleStyle;
	[Header("Style Parent")]
	public GameObject titleStylePanel;

	private Animator titleStylePanelAnimator;
	private readonly string panelFadeIn = "Panel Open";

	private GameObject menuCanvas;

	private void Start()
	{
		titleStylePanelAnimator = titleStylePanel.GetComponent<Animator>();

		//gameObject.GetComponent<Animator>().SetTrigger("playIntro");
		titleStylePanelAnimator.Play(panelFadeIn);
		menuCanvas = GameObject.Find("MenuCanvas");
		if (MenuPlay.sceneLoadCounter == 0 && SceneManager.GetActiveScene().name == "MainMenu")
		{
			menuCanvas.GetComponent<Canvas>().sortingOrder = 0;
		}
	}
	void Update()
    {
		StartCoroutine(WaitABitAndSetCanvas(4.5f));

	}
	IEnumerator WaitABitAndSetCanvas(float duration)
	{
		//Play the transition, then load next scene ->
		yield return new WaitForSeconds(duration);
		gameObject.SetActive(false);
	}
}
