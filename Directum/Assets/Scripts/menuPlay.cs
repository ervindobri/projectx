using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuPlay : MonoBehaviour
{

	public GameObject styleObject;
	private Animator styleAnimator;
	public string sceneName;
	ButtonAnimController buttonAnimController;
	public static int sceneLoadCounter;

	private void Start()
	{
		styleAnimator = styleObject.GetComponent<Animator>();
		GameObject buttonAnimControllerObject = GameObject.FindWithTag("Canvas");
		if (buttonAnimControllerObject != null)
		{
			buttonAnimController = buttonAnimControllerObject.GetComponent<ButtonAnimController>();
		}
		if (buttonAnimControllerObject == null)
		{
			Debug.Log("Could not find 'ButtonAnimController' script...");
		}
		// If this was the first scene loaded, don't play transition fade out
		if ( sceneLoadCounter == 0 && SceneManager.GetActiveScene().name == "MainMenu")
		{
			GameObject.Find("Styles - Fade Out").SetActive(false);
		}

	}
	private void Update()
	{
		if (buttonAnimController.fadeIn)
		{
			StartCoroutine(WaitToLoadScene(1.01f));
		}

	}
	IEnumerator WaitToLoadScene(float duration)
	{
		//Play the transition, then load next scene ->
		yield return new WaitForSeconds(duration);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		sceneLoadCounter++;
	}

}
