using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;
	private Button currentButton;
	private bool pointerOver;


	public void MouseOverButton()
	{
		pointerOver = true;
	}
	private void Start()
	{
		currentButton = gameObject.GetComponent<Button>();
	}
	void Update()
    {
		if (menuButtonController.index == thisIndex )
		{
			//trigger animations
			animator.SetBool("selected",true);
			if ( Input.GetAxis("Submit") == 1 )
			{

				//Debug.Log(this.gameObject.name);
				animator.SetBool("pressed", true);
				currentButton.onClick.Invoke();
				Debug.Log(currentButton.name);
			}
			else if (animator.GetBool("pressed"))
			{
				animator.SetBool("pressed", false);
				animatorFunctions.disableOnce = true;
				currentButton.onClick.Invoke();
			}
		}
		else
		{
			animator.SetBool("selected", false);
		}
        
    }
}
