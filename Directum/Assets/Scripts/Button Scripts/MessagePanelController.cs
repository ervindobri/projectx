using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanelController : MonoBehaviour
{
	public Text text;
	private void Start()
	{
		text = GameObject.Find("DisplayText").GetComponent<Text>();
	}

	public void SetMessage(string message)
	{
		text.text = message;
	}
}
