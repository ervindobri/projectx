using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
	private void Update()
	{
		if( gameObject.GetComponent<Toggle>().isOn)
		{
			AudioListener.volume = 1f;
		}
		else
		{
			AudioListener.volume = 0;

		}
	}
}
