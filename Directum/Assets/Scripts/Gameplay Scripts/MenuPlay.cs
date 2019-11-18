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

	public static bool wasRestarted;


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
			introCanvasStatus = GameObject.Find("IntroCanvas").activeInHierarchy;
		}
		else if(sceneLoadCounter!= 0 && SceneManager.GetActiveScene().name == "MainMenu")
		{
			introCanvasStatus = GameObject.Find("IntroCanvas").activeInHierarchy;
			GameObject.Find("IntroCanvas").SetActive(false);

		}
		
	}

	public void setSceneName(string scene)
	{
		sceneName = scene;
		if ( sceneName == "GameMain")
		{
			wasRestarted = true;
		}
	}
	private void Update()
	{
		if ( buttonAnimController.fadeIn )
		{

			StartCoroutine(WaitToLoadScene(1.01f));
		}
		//Debug.Log(sceneLoadCounter);
		

	}
	void OnEnable()
	{
		//Debug.Log("OnEnable called");
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//Debug.Log("OnSceneLoaded: " + scene.name);
		//Debug.Log(mode);
	}
	void OnDisable()
	{
		//Debug.Log("OnDisable");
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	IEnumerator WaitToLoadScene(float duration)
	{
		//Play the transition, then load next scene ->
		yield return new WaitForSeconds(duration);
		if ( sceneName == SceneManager.GetActiveScene().name)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
		else
		{
			SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		}
		sceneLoadCounter++;
	}

}
