using System.Collections;
using UnityEngine;

public class ButtonAnimController : MonoBehaviour
{
	[Header("STYLE OBJECTS")]
	public GameObject styleObjectIn;
	public GameObject styleObjectOut;
	[Header("STYLE PARENT")]
	public GameObject panelIn;
	public GameObject panelOut;

	// [Header("PANEL ANIMS")]
	private readonly string panelFadeIn = "Panel Open";
	private readonly string styleLoop = "Loop";
	private readonly string styleClose = "Loop";


	private Animator panelAnimator;
	private Animator styleAnimator;
	//private Animator buttonAnimator;
	public bool fadeIn,fadeOut;
	private Client client;

	public void PanelAnimationFadeIn()
	{
		//Debug.Log("Button was clicked!");
		panelAnimator = panelIn.GetComponent<Animator>();
		//panelAnimator.Play(panelFadeOut);
		panelAnimator.Play(panelFadeIn);
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
		yield return new WaitForSeconds(duration);
	}
	public void ExitButton()
	{
		//The 'Quit()' function only works in the compiled game
		Application.Quit();
		client = FindObjectOfType<Client>();
		client.CloseSocket();
		Debug.Log("Exited!");

	}
	public void SetTransitionToNextPanel(string setTrigger)
	{
		
		this.gameObject.GetComponent<Animator>().SetTrigger(setTrigger);
	}
	public void ResetTriggerBeforeTransition(string resetTrigger)
	{
		this.gameObject.GetComponent<Animator>().ResetTrigger(resetTrigger);
	}
}
