using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : MonoBehaviour
{
	public Text text;
	private void Start()
	{
		text = GameObject.Find("DisplayText").GetComponent<Text>();
	}

	public void DisplayMessage()
	{
		gameObject.GetComponent<Animator>().SetTrigger("dispMessage");	
	}
	public void SetMessage(string message)
	{
		text.text = message;
	}
	public void PlayAudio(AudioClip audioClip)
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
	}
}
