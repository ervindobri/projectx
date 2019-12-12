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
			if (currentCanvasTransform.offsetMin == new Vector2(0, 0) && currentCanvasTransform.offsetMax == new Vector2(0, 0) || gameObject.name == "Canvas")
			{

				button.GetComponent<Button>().onClick.Invoke();
			}	
		}
    }
}
