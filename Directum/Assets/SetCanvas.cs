using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetCanvas : MonoBehaviour
{
	private GameObject menuCanvas;
	private void Start()
	{
		menuCanvas = GameObject.Find("MenuCanvas");
		if (MenuPlay.sceneLoadCounter == 0 && SceneManager.GetActiveScene().name == "MainMenu")
		{
			menuCanvas.GetComponent<Canvas>().sortingOrder = 0;
		}
	}
	private void Update()
	{
		StartCoroutine(WaitABitAndSetCanvas(4.5f));	

	}
	IEnumerator WaitABitAndSetCanvas(float duration)
	{
		//Play the transition, then load next scene ->
		yield return new WaitForSeconds(duration);

		menuCanvas.GetComponent<Canvas>().sortingOrder = 1;

		this.gameObject.SetActive(false);
	}
}
