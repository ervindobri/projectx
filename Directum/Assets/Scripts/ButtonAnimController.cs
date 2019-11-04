using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimController : MonoBehaviour
{
	[Header("STYLE OBJECTS")]
	public GameObject styleObjectIn;
	public GameObject styleObjectOut;
	[Header("STYLE PARENT")]
	public GameObject panelIn;
	public GameObject panelOut;
	[Header("STYLE BUTTON")]
	public GameObject styleButtonIn;
	//public GameObject styleButtonOut;


	// [Header("PANEL ANIMS")]
	private string panelFadeIn = "Panel Open";
	private string panelFadeOut = "Panel Close";
	private string styleExpand = "Expand";
	private string styleLoop = "Loop";
	private string styleClose = "Loop";

	// [Header("BUTTON ANIMS")]
	private string buttonFadeIn = "Button Open";
	private string buttonFadeOut = "Button Close";

	private Animator panelAnimator;
	private Animator styleAnimator;
	private Animator buttonAnimator;
	public bool fadeIn,fadeOut;

	public void PanelAnimationFadeIn()
	{
		Debug.Log("Button was clicked!");
		panelAnimator = panelIn.GetComponent<Animator>();
		//panelAnimator.Play(panelFadeOut);
		panelAnimator.Play(panelFadeIn);

		buttonAnimator = styleButtonIn.GetComponent<Animator>();
		//buttonAnimator.Play(buttonFadeOut);
		buttonAnimator.Play(buttonFadeIn);

		styleAnimator = panelIn.GetComponent<Animator>();
		styleAnimator.Play(styleLoop);
		fadeIn = true;
	}
	public void PanelAnimationFadeOut()
	{
		//Debug.Log("Fade out!");
		styleAnimator = panelOut.GetComponent<Animator>();
		styleAnimator.Play(styleClose);
		StartCoroutine(WaitABit(1));
		fadeOut = true;
	}
	IEnumerator WaitABit(float duration)
	{
		//Play the transition, then load next scene ->
		//buttonAnimController.PanelAnimationFadeIn();
		yield return new WaitForSeconds(duration);
	}
	public void exitButton()
	{
		//The 'Quit()' function only works in the compiled game
		Application.Quit();
		Debug.Log("Exited!");
	}
	public void setTransitionToNextPanel(string trigger)
	{
		this.gameObject.GetComponent<Animator>().SetTrigger(trigger);
	}
}
