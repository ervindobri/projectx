using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TextConverter : MonoBehaviour
{
	private InputField inputField;
    void Start()
    {
		inputField = GetComponent<InputField>();
		inputField.onValueChanged.AddListener(delegate { OnValueChange(); });
	}
    public void OnValueChange()
	{
		inputField.text = inputField.text.ToUpper();
		//Debug.Log(inputField.text);
	}
}
