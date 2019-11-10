using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
	[Header("Style Object")]
	public GameObject titleStyle;
	[Header("Style Parent")]
	public GameObject titleStylePanel;

	private Animator titleStyleAnimator;
	private Animator titleStylePanelAnimator;


	// [Header("PANEL ANIMS")]
	private string panelFadeIn = "Panel Open";
	private string panelFadeOut = "Panel Close";
	private string styleExpand = "Expand";


	private Image image;

	private void Start()
	{
		titleStyleAnimator = titleStyle.GetComponent<Animator>();
		titleStylePanelAnimator = titleStylePanel.GetComponent<Animator>();


		titleStylePanelAnimator.Play(panelFadeIn);
		image = GameObject.Find("Image").GetComponent<Image>();
	}
	void Update()
    {
		
    }

}
