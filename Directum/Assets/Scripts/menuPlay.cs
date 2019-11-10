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
	private bool introCanvasStatus;

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
			GameObject.Find("IntroCanvas").SetActive(true);

		}
		else
		{
			GameObject.Find("IntroCanvas").SetActive(false);

		}

		//
		introCanvasStatus = GameObject.Find("IntroCanvas").activeInHierarchy;
	}
	public void setSceneName(string scene)
	{
		sceneName = scene;
	}
	private void Update()
	{
		if ( buttonAnimController.fadeIn )
		{
			StartCoroutine(WaitToLoadScene(1.01f));
		}
		//Debug.Log(sceneLoadCounter);
		

	}
	IEnumerator WaitToLoadScene(float duration)
	{
		//Play the transition, then load next scene ->
		yield return new WaitForSeconds(duration);
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		sceneLoadCounter++;
	}

}
