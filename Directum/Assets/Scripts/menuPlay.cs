using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class menuPlay : MonoBehaviour
{

	public GameObject styleObject;
	private Animator styleAnimator;
	public string sceneName;
	ButtonAnimController buttonAnimController;
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
	}
	private void Update()
	{
		if ( buttonAnimController.fadeIn)
		{
			StartCoroutine(WaitToLoadScene(2f));
		}
	}
	IEnumerator WaitToLoadScene(float duration)
	{
		//Play the transition, then load next scene ->
		//buttonAnimController.PanelAnimationFadeIn();
		yield return new WaitForSeconds(duration);

		styleAnimator.SetTrigger("loadPlayMenu");
		SceneManager.LoadScene(sceneName);
	}

}
