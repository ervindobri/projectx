using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorFunctions : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	public bool disableOnce;

	public  void PlaySound(AudioClip whichSound)
	{
		//if (!disableOnce)
		//{
			menuButtonController.audioSource.volume = 0.13f;
			menuButtonController.audioSource.PlayOneShot(whichSound);

		//}
		//else
		//{
		//	disableOnce = false;
		//}
	}
}
