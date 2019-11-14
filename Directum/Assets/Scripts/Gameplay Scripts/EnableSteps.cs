using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableSteps : MonoBehaviour
{
	private int clickNumber = 0;
	private Animator canvasAnimator;
	float alpha1, alpha2;
	private void Start()
	{
		canvasAnimator = this.gameObject.GetComponent<Animator>();
		alpha1 = GameObject.Find("VibratingPanel/StepTwoPanel/Canvas").GetComponent<CanvasGroup>().alpha;
		alpha2 = GameObject.Find("VibratingPanel/StepThreePanel/Canvas").GetComponent<CanvasGroup>().alpha;
		//Debug.Log("step2" + alpha1 +" step3" +  alpha2);

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
			GameObject.Find("VibratingPanel/StepThreePanel").GetComponent<Animator>().SetTrigger("jk");
		}
	}
}
