using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuPlay : MonoBehaviour
{

	public GameObject styleObject;
	public string sceneName;
	ButtonAnimController buttonAnimController;
	public static int sceneLoadCounter;

	public static bool wasRestarted;

	private void Awake()
	{
		if (SceneManager.GetActiveScene().name == "MainMenu")
		{
			GameObject gm = GameObject.Find("GameplayManager");
			if (gm != null)
			{
				Destroy(gm);
			}
		}
		
	}
	private void Start()
	{
		GameObject buttonAnimControllerObject = GameObject.FindWithTag("Canvas");
		GameObject introCanvas = GameObject.Find("IntroCanvas");
		if (buttonAnimControllerObject != null)
		{
			buttonAnimController = buttonAnimControllerObject.GetComponent<ButtonAnimController>();
		}
		else
		{
			Debug.Log("Could not find 'ButtonAnimController' script...");
		}
		// If this was the first scene loaded, don't play transition fade out
		if (sceneLoadCounter == 0 && SceneManager.GetActiveScene().name == "MainMenu")
		{
			GameObject.Find("Styles - Fade Out").SetActive(false);
			introCanvas.SetActive(true);
		}
		else if (sceneLoadCounter != 0 && SceneManager.GetActiveScene().name == "MainMenu")
		{
			//introCanvasStatus = introCanvasObject.activeInHierarchy;
			introCanvas.SetActive(false);

		}
		
	}

	public void SetSceneName(string scene)
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
