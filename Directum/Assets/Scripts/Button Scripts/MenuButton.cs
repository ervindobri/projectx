using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;

	void Update()
    {
		if (menuButtonController.index == thisIndex)
		{
			//trigger animations
			animator.SetBool("selected",true);
			if ( Input.GetAxis("Submit") == 1 || Input.GetMouseButtonDown(0) )
			{

				//Debug.Log(this.gameObject.name);
				animator.SetBool("pressed", true);
				this.gameObject.GetComponent<Button>().onClick.Invoke();
			}
			else if (animator.GetBool("pressed"))
			{
				animator.SetBool("pressed", false);
				animatorFunctions.disableOnce = true;
				this.gameObject.GetComponent<Button>().onClick.Invoke();

			}
		}
		else
		{
			animator.SetBool("selected", false);
		}
        
    }
}
