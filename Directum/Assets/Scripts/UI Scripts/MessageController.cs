using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
	private Text message;
	private RectTransform msgBackground;

	private readonly float maxChar = 30f;
	private float numOfCharacters, lines;
	private void Start()
	{
		msgBackground = GetComponent<RectTransform>();
		message = gameObject.GetComponentInChildren<Text>();
		CheckIfMultiLine();
	}
	private void CheckIfMultiLine()
	{
		numOfCharacters = message.text.Length;
		if ( numOfCharacters >= 30)
		{
			lines = numOfCharacters / maxChar;
		}
		else
		{
			lines = 1;
		}
		//Debug.Log(numOfCharacters + "lines:"+ lines);
		//Subtract 6 from anchoredposY
		msgBackground.anchoredPosition = new Vector2(msgBackground.anchoredPosition.x,msgBackground.anchoredPosition.y - 6*lines);
		msgBackground.sizeDelta = new Vector2(msgBackground.sizeDelta.x, msgBackground.sizeDelta.y * lines);

	}
}
