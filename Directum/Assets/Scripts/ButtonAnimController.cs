using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimController : MonoBehaviour
{
	[Header("STYLE OBJECT")]
	public GameObject styleObject;
	[Header("STYLE PARENT")]
	public GameObject panel;
	[Header("STYLE BUTTON")]
	private GameObject styleButton;


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
	public bool fadeIn;
	private void Start()
	{
		styleButton = GameObject.Find("PlayButton");
	}
	public void PanelAnimationFadeIn()
	{
		Debug.Log("Button was clicked!");
		panelAnimator = panel.GetComponent<Animator>();
		//panelAnimator.Play(panelFadeOut);
		panelAnimator.Play(panelFadeIn);

		buttonAnimator = styleButton.GetComponent<Animator>();
		//buttonAnimator.Play(buttonFadeOut);
		buttonAnimator.Play(buttonFadeIn);

		styleAnimator = panel.GetComponent<Animator>();
		styleAnimator.Play(styleLoop);

		fadeIn = true;
	}
	public void PanelAnimationFadeOut()
	{
		Debug.Log("Fade out!");

		//panelAnimator = panel.GetComponent<Animator>();
		//panelAnimator.Play(panelFadeIn);


		//buttonAnimator = styleButton.GetComponent<Animator>();
		//buttonAnimator.Play(buttonFadeOut);

		styleAnimator = panel.GetComponent<Animator>();
		styleAnimator.Play(styleClose);
		//styleAnimator.Play(styleExpand);
		//panelAnimator.Play(panelFadeOut);

	}
}
