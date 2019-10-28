using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionBehaviour : MonoBehaviour
{
	public GameObject styleObject;
	private Animator styleAnimator;
	ButtonAnimController buttonAnimController;
	private void Awake()
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
		//styleAnimator.SetBool("fadeOut", true);
		buttonAnimController.PanelAnimationFadeOut();
	}
	private void Update()
	{
		//Debug.Log("FADING OUT");
		//buttonAnimController.PanelAnimationFadeOut();
	}
}
