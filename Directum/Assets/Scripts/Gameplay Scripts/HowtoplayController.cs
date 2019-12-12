using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowtoplayController : MonoBehaviour
{
	private int clickNumber = 0;
	private Animator canvasAnimator;
	private Animator lastPanel;

	private void Start()
	{
		canvasAnimator = this.gameObject.GetComponent<Animator>();
		lastPanel = GameObject.Find("VibratingPanel/StepThreePanel").GetComponent<Animator>();

	}
	private void Update()
	{
		//Debug.Log(alpha1 + "  " + alpha2);
		if (Input.GetMouseButtonDown(0))
		{
			canvasAnimator.SetTrigger("mouseClicked");
			clickNumber++;
		}
		if ( clickNumber == 3)
		{
			lastPanel.SetTrigger("jk");
		}
	}
}
