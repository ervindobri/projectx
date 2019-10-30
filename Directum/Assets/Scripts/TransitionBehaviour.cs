using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : MonoBehaviour
{
	public GameObject panelStyleObject;
	private Animator panelAnimator;
	ButtonAnimController buttonAnimController;
	private void Awake()
	{
		panelAnimator = panelStyleObject.GetComponent<Animator>();
		GameObject buttonAnimControllerObject = GameObject.FindWithTag("Canvas");
		if (buttonAnimControllerObject != null)
		{
			buttonAnimController = buttonAnimControllerObject.GetComponent<ButtonAnimController>();
		}
		if (buttonAnimControllerObject == null)
		{
			Debug.Log("Could not find 'ButtonAnimController' script...");
		}
		if (buttonAnimController.fadeOut)
		{
			buttonAnimController.PanelAnimationFadeOut();
		}
	}
	private void Update()
	{
		//Debug.Log("FADING OUT");
		//buttonAnimController.PanelAnimationFadeOut();
		
	}
}
