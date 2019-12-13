using UnityEngine.UI;
using UnityEngine;

public class BackButtonController : MonoBehaviour
{
	[Header("Back or cancel button")]
	public GameObject button;

	private RectTransform currentCanvasTransform;
	private void Start()
	{
		currentCanvasTransform = gameObject.GetComponent<RectTransform>();
	}
	void Update()
    {
		// If you press escape and the canvas is visible, you can invoke the button func.
		if ( Input.GetKeyDown("escape") )
		{
			if (currentCanvasTransform.offsetMin == Vector2.zero && currentCanvasTransform.offsetMax == Vector2.zero || gameObject.name == "Canvas")
			{

				button.GetComponent<Button>().onClick.Invoke();
			}	
		}
    }
}
