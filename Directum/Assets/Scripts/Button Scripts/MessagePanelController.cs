using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : MonoBehaviour
{
	public Text text;
	private void Start()
	{
		text = GameObject.Find("DisplayText").GetComponent<Text>();
	}

	public void SetMessageAndNotify(string message)
	{
		gameObject.GetComponent<Animator>().SetTrigger("dispMessage");
		text.text = message;
	}
	public void PlayAudio(AudioClip audioClip)
	{
		gameObject.GetComponent<AudioSource>().PlayOneShot(audioClip);
	}
}
